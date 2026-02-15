using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.Map;
using Terraria.Net;
using Terraria.Net.Sockets;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria
{
	// Token: 0x02000033 RID: 51
	public class Netplay
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060002E8 RID: 744 RVA: 0x00042698 File Offset: 0x00040898
		// (remove) Token: 0x060002E9 RID: 745 RVA: 0x000426CC File Offset: 0x000408CC
		public static event Action OnDisconnect;

		// Token: 0x060002EA RID: 746 RVA: 0x00042700 File Offset: 0x00040900
		private static void UpdateServerInMainThread()
		{
			for (int i = 0; i < 256; i++)
			{
				NetMessage.CheckBytes(i);
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00042724 File Offset: 0x00040924
		private static string GetLocalIPAddress()
		{
			string result = "";
			foreach (IPAddress ipaddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
			{
				if (Netplay.AcceptedFamilyType(ipaddress.AddressFamily))
				{
					result = ipaddress.ToString();
					break;
				}
			}
			return result;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00042770 File Offset: 0x00040970
		private static void ResetNetDiag()
		{
			Main.ActiveNetDiagnosticsUI.Reset();
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0004277C File Offset: 0x0004097C
		public static void ResetSections()
		{
			foreach (RemoteClient remoteClient in Netplay.Clients)
			{
				Array.Clear(remoteClient.TileSections, 0, remoteClient.TileSections.Length);
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x000427B8 File Offset: 0x000409B8
		public static void AddBan(int plr)
		{
			RemoteAddress remoteAddress = Netplay.Clients[plr].Socket.GetRemoteAddress();
			using (StreamWriter streamWriter = new StreamWriter(Netplay.BanFilePath, true))
			{
				streamWriter.WriteLine("//" + Main.player[plr].name);
				streamWriter.WriteLine(remoteAddress.GetIdentifier());
			}
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00042828 File Offset: 0x00040A28
		public static bool IsBanned(RemoteAddress address)
		{
			try
			{
				string identifier = address.GetIdentifier();
				if (File.Exists(Netplay.BanFilePath))
				{
					using (StreamReader streamReader = new StreamReader(Netplay.BanFilePath))
					{
						string a;
						while ((a = streamReader.ReadLine()) != null)
						{
							if (a == identifier)
							{
								return true;
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00009E06 File Offset: 0x00008006
		private static void OpenPort(int port)
		{
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00009E06 File Offset: 0x00008006
		private static void ClosePort(int port)
		{
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00009E06 File Offset: 0x00008006
		private static void ServerFullWriteCallBack(object state)
		{
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0004289C File Offset: 0x00040A9C
		private static void OnConnectionAccepted(ISocket client)
		{
			int num = Netplay.FindNextOpenClientSlot();
			if (num != -1)
			{
				Netplay.Clients[num].Reset();
				Netplay.Clients[num].Socket = client;
			}
			else
			{
				MessageBuffer obj = Netplay.fullBuffer;
				lock (obj)
				{
					Netplay.KickClient(client, NetworkText.FromKey("CLI.ServerIsFull", new object[0]));
				}
			}
			if (Netplay.FindNextOpenClientSlot() == -1)
			{
				Netplay.StopListening();
				Netplay.IsListening = false;
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00042924 File Offset: 0x00040B24
		private static void KickClient(ISocket client, NetworkText kickMessage)
		{
			BinaryWriter writer = Netplay.fullBuffer.writer;
			if (writer == null)
			{
				Netplay.fullBuffer.ResetWriter();
				writer = Netplay.fullBuffer.writer;
			}
			writer.BaseStream.Position = 0L;
			long position = writer.BaseStream.Position;
			writer.BaseStream.Position += 2L;
			writer.Write(2);
			kickMessage.Serialize(writer);
			if (Main.dedServ)
			{
				Console.WriteLine(Language.GetTextValue("CLI.ClientWasBooted", client.GetRemoteAddress().ToString(), kickMessage));
			}
			int num = (int)writer.BaseStream.Position;
			writer.BaseStream.Position = position;
			writer.Write((short)num);
			writer.BaseStream.Position = (long)num;
			client.AsyncSend(Netplay.fullBuffer.writeBuffer, 0, num, new SocketSendCallback(Netplay.ServerFullWriteCallBack), client);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x000429FE File Offset: 0x00040BFE
		public static void OnConnectedToSocialServer(ISocket client)
		{
			Netplay.StartSocialClient(client);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00042A06 File Offset: 0x00040C06
		private static bool StartListening()
		{
			if (SocialAPI.Network != null)
			{
				SocialAPI.Network.StartListening(new SocketConnectionAccepted(Netplay.OnConnectionAccepted));
			}
			return Netplay.TcpListener.StartListening(new SocketConnectionAccepted(Netplay.OnConnectionAccepted));
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00042A3C File Offset: 0x00040C3C
		private static void StopListening()
		{
			if (SocialAPI.Network != null)
			{
				SocialAPI.Network.StopListening();
			}
			Netplay.TcpListener.StopListening();
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00042A59 File Offset: 0x00040C59
		public static void StartServer()
		{
			Netplay.InitializeServer();
			Netplay._serverThread = new Thread(new ThreadStart(Netplay.ServerLoop))
			{
				IsBackground = true,
				Name = "Server Loop Thread"
			};
			Netplay._serverThread.Start();
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00042A94 File Offset: 0x00040C94
		private static void InitializeServer()
		{
			Netplay.Connection.ResetSpecialFlags();
			Netplay.ResetNetDiag();
			if (Main.rand == null)
			{
				Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);
			}
			Main.myPlayer = 255;
			Netplay.ServerIP = IPAddress.Any;
			Main.menuMode = 14;
			Main.statusText = Lang.menu[8].Value;
			Main.netMode = 2;
			Netplay.Disconnect = false;
			for (int i = 0; i < 256; i++)
			{
				Netplay.Clients[i] = new RemoteClient();
				Netplay.Clients[i].Reset();
				Netplay.Clients[i].Id = i;
				Netplay.Clients[i].ReadBuffer = new byte[1024];
			}
			Netplay.TcpListener = new TcpSocket();
			if (!Netplay.Disconnect)
			{
				if (!Netplay.StartListening())
				{
					Main.statusText = Language.GetTextValue("Error.TriedToRunServerTwice");
					Netplay.SaveOnServerExit = false;
					Netplay.Disconnect = true;
				}
				Main.statusText = Language.GetTextValue("CLI.ServerStarted");
			}
			if (!Netplay.UseUPNP)
			{
				return;
			}
			try
			{
				Netplay.OpenPort(Netplay.ListenPort);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00042BBC File Offset: 0x00040DBC
		private static void ServerLoop()
		{
			int num = 0;
			Netplay.StartBroadCasting();
			while (!Netplay.Disconnect)
			{
				Netplay.StartListeningIfNeeded();
				Netplay.UpdateConnectedClients();
				num = (num + 1) % 10;
				Thread.Sleep((num == 0) ? 1 : 0);
			}
			Netplay.StopBroadCasting();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00042BFC File Offset: 0x00040DFC
		private static void UpdateConnectedClients()
		{
			int num = 0;
			for (int i = 0; i < 256; i++)
			{
				if (Netplay.Clients[i].PendingTermination)
				{
					num++;
					if (Netplay.Clients[i].PendingTerminationApproved)
					{
						Netplay.Clients[i].Reset();
						NetMessage.SyncDisconnectedPlayer(i);
					}
				}
				else if (Netplay.Clients[i].IsConnected())
				{
					Netplay.Clients[i].Update();
					num++;
				}
				else if (Netplay.Clients[i].IsActive)
				{
					Netplay.Clients[i].PendingTermination = true;
					Netplay.Clients[i].PendingTerminationApproved = true;
				}
				else
				{
					Netplay.Clients[i].StatusText2 = "";
					if (i < 255)
					{
						bool active = Main.player[i].active;
						Main.player[i].active = false;
						if (active)
						{
							Player.Hooks.PlayerDisconnect(i);
						}
					}
				}
			}
			Netplay.HasClients = (num != 0);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00042CE8 File Offset: 0x00040EE8
		private static void StartListeningIfNeeded()
		{
			if (!Netplay.IsListening)
			{
				if (Netplay.Clients.Any((RemoteClient client) => !client.IsConnected()))
				{
					try
					{
						Netplay.StartListening();
						Netplay.IsListening = true;
					}
					catch
					{
						if (!Main.ignoreErrors)
						{
							throw;
						}
					}
				}
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00042D54 File Offset: 0x00040F54
		private static void UpdateClientInMainThread()
		{
			if (Main.netMode != 1)
			{
				return;
			}
			if (Netplay.Connection.IsActive && !Netplay.Connection.ServerWantsToRunCheckBytesInClientLoopThread)
			{
				NetMessage.CheckBytes(256);
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00042D84 File Offset: 0x00040F84
		public static void AddCurrentServerToRecentList()
		{
			if (Netplay.Connection.Socket.GetRemoteAddress().Type != AddressType.Tcp)
			{
				return;
			}
			for (int i = 0; i < Main.maxMP; i++)
			{
				if (Main.recentIP[i].ToLower() == Netplay.ServerIPText.ToLower() && Main.recentPort[i] == Netplay.ListenPort)
				{
					for (int j = i; j < Main.maxMP - 1; j++)
					{
						Main.recentIP[j] = Main.recentIP[j + 1];
						Main.recentPort[j] = Main.recentPort[j + 1];
						Main.recentWorld[j] = Main.recentWorld[j + 1];
					}
				}
			}
			for (int k = Main.maxMP - 1; k > 0; k--)
			{
				Main.recentIP[k] = Main.recentIP[k - 1];
				Main.recentPort[k] = Main.recentPort[k - 1];
				Main.recentWorld[k] = Main.recentWorld[k - 1];
			}
			Main.recentIP[0] = Netplay.ServerIPText;
			Main.recentPort[0] = Netplay.ListenPort;
			Main.recentWorld[0] = Main.worldName;
			Main.SaveRecent();
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00042E90 File Offset: 0x00041090
		public static void SocialClientLoop(object threadContext)
		{
			ISocket socket = (ISocket)threadContext;
			Netplay.ClientLoopSetup(socket.GetRemoteAddress());
			Netplay.Connection.Socket = socket;
			Netplay.InnerClientLoop();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00042EC0 File Offset: 0x000410C0
		public static void TcpClientLoop()
		{
			Netplay.ClientLoopSetup(new TcpAddress(Netplay.ServerIP, Netplay.ListenPort));
			Main.menuMode = 14;
			bool flag = true;
			while (flag)
			{
				flag = false;
				try
				{
					Netplay.Connection.Socket.Connect(new TcpAddress(Netplay.ServerIP, Netplay.ListenPort));
					flag = false;
				}
				catch
				{
					if (Platform.IsOSX)
					{
						Thread.Sleep(200);
						Netplay.Connection.Socket.Close();
						Netplay.Connection.Socket = new TcpSocket();
					}
					if (!Netplay.Disconnect && Main.gameMenu)
					{
						flag = true;
					}
				}
			}
			Netplay.InnerClientLoop();
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00042F6C File Offset: 0x0004116C
		private static void ClientLoopSetup(RemoteAddress address)
		{
			Netplay.Connection.ResetSpecialFlags();
			Netplay.ResetNetDiag();
			Main.ServerSideCharacter = false;
			if (Main.rand == null)
			{
				Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);
			}
			Main.player[Main.myPlayer].hostile = false;
			Main.player[Main.myPlayer].clientClone(Main.clientPlayer);
			for (int i = 0; i < 255; i++)
			{
				if (i != Main.myPlayer)
				{
					Main.player[i] = new Player();
				}
			}
			Main.netMode = 1;
			Main.menuMode = 14;
			if (!Main.autoPass)
			{
				Main.statusText = Language.GetTextValue("Net.ConnectingTo", address.GetFriendlyName());
			}
			Netplay.Disconnect = false;
			Netplay.Connection = new RemoteServer();
			Netplay.Connection.ReadBuffer = new byte[1024];
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00043048 File Offset: 0x00041248
		private static void InnerClientLoop()
		{
			try
			{
				NetMessage.buffer[256].Reset();
				int num = -1;
				while (!Netplay.Disconnect)
				{
					if (Netplay.Connection.IsActive && Netplay.Connection.ServerWantsToRunCheckBytesInClientLoopThread)
					{
						NetMessage.CheckBytes(256);
					}
					if (Netplay.Connection.IsConnected())
					{
						Netplay.Connection.IsActive = true;
						if (Netplay.Connection.State == 0)
						{
							Main.statusText = Language.GetTextValue("Net.FoundServer");
							Netplay.Connection.State = 1;
							NetMessage.SendData(1, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
							Ping.Reset();
						}
						if (Netplay.Connection.State == 2 && num != Netplay.Connection.State)
						{
							Main.statusText = Language.GetTextValue("Net.SendingPlayerData");
						}
						if (Netplay.Connection.State == 3 && num != Netplay.Connection.State)
						{
							Main.statusText = Language.GetTextValue("Net.RequestingWorldInformation");
						}
						if (Netplay.Connection.State == 4)
						{
							WorldGen.worldCleared = false;
							Netplay.Connection.State = 5;
							if (Main.cloudBGActive >= 1f)
							{
								Main.cloudBGAlpha = 1f;
							}
							else
							{
								Main.cloudBGAlpha = 0f;
							}
							Main.windSpeedCurrent = Main.windSpeedTarget;
							Cloud.resetClouds();
							Main.cloudAlpha = Main.maxRaining;
							Main.ToggleGameplayUpdates(false);
							WorldGen.clearWorld();
							if (Main.mapEnabled)
							{
								Main.Map.Load();
							}
						}
						if (Netplay.Connection.State == 5 && Main.loadMapLock)
						{
							float num2 = (float)Main.loadMapLastX / (float)Main.maxTilesX;
							Main.statusText = string.Concat(new object[]
							{
								Lang.gen[68].Value,
								" ",
								(int)(num2 * 100f + 1f),
								"%"
							});
						}
						else if (Netplay.Connection.State == 5 && WorldGen.worldCleared)
						{
							Netplay.Connection.State = 6;
							Main.player[Main.myPlayer].FindSpawn();
							NetMessage.SendData(8, -1, -1, null, Main.player[Main.myPlayer].SpawnX, (float)Main.player[Main.myPlayer].SpawnY, (float)Main.player[Main.myPlayer].team, 0f, 0, 0, 0);
						}
						if (Netplay.Connection.State == 6 && num != Netplay.Connection.State)
						{
							Main.statusText = Language.GetTextValue("Net.RequestingTileData");
						}
						if (!Netplay.Connection.IsReading && !Netplay.Disconnect && Netplay.Connection.Socket.IsDataAvailable() && !Netplay.Connection.ReadBufferFull)
						{
							Netplay.Connection.IsReading = true;
							Netplay.Connection.Socket.AsyncReceive(Netplay.Connection.ReadBuffer, 0, Netplay.Connection.ReadBuffer.Length, new SocketReceiveCallback(Netplay.Connection.ClientReadCallBack), null);
						}
						if (Netplay.Connection.StatusMax > 0 && Netplay.Connection.StatusText != "")
						{
							if (Netplay.Connection.StatusCount >= Netplay.Connection.StatusMax)
							{
								Main.statusText = Language.GetTextValue("Net.StatusComplete", Netplay.Connection.StatusText);
								Netplay.Connection.StatusText = "";
								Netplay.Connection.StatusMax = 0;
								Netplay.Connection.StatusCount = 0;
							}
							else
							{
								int num3;
								int num4;
								Main.ActiveNetDiagnosticsUI.GetLastSentRecvBytes(out num3, out num4);
								Main.statusText = string.Format("{0}: {1}% ({2:0.0} kB/s)", Netplay.Connection.StatusText, Netplay.Connection.StatusCount * 100 / Netplay.Connection.StatusMax, (double)num4 / 1024.0);
							}
						}
						Thread.Sleep(1);
					}
					num = Netplay.Connection.State;
				}
				try
				{
					Netplay.Connection.IsActive = false;
					Netplay.Connection.Socket.Close();
				}
				catch
				{
				}
				if (!Main.gameMenu)
				{
					Main.gameMenu = true;
					Main.SwitchNetMode(0);
					MapHelper.noStatusText = true;
					Player.SavePlayer(Main.ActivePlayerFileData, false);
					Player.ClearPlayerTempInfo();
					Main.ActivePlayerFileData.StopPlayTimer();
					SoundEngine.StopTrackedSounds();
					MapHelper.noStatusText = false;
					Main.menuMode = 14;
				}
				NetMessage.buffer[256].Reset();
				if (Main.menuMode == 15 && Main.statusText == Language.GetTextValue("Net.LostConnection"))
				{
					Main.menuMode = 14;
				}
				if (Netplay.Connection.StatusText != "" && Netplay.Connection.StatusText != null)
				{
					Main.statusText = Language.GetTextValue("Net.LostConnection");
				}
				Netplay.Connection.StatusCount = 0;
				Netplay.Connection.StatusMax = 0;
				Netplay.Connection.StatusText = "";
				Main.SwitchNetMode(0);
			}
			catch (Exception value)
			{
				try
				{
					using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", true))
					{
						streamWriter.WriteLine(DateTime.Now);
						streamWriter.WriteLine(value);
						streamWriter.WriteLine("");
					}
				}
				catch
				{
				}
				Netplay.Disconnect = true;
			}
			if (Netplay.OnDisconnect != null)
			{
				Netplay.OnDisconnect();
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000435DC File Offset: 0x000417DC
		private static int FindNextOpenClientSlot()
		{
			for (int i = 0; i < Main.maxNetPlayers; i++)
			{
				if (!Netplay.Clients[i].IsConnected())
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0004360A File Offset: 0x0004180A
		public static void StartSocialClient(ISocket socket)
		{
			new Thread(new ParameterizedThreadStart(Netplay.SocialClientLoop))
			{
				Name = "Social Client Thread",
				IsBackground = true
			}.Start(socket);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00043635 File Offset: 0x00041835
		public static void StartTcpClient()
		{
			new Thread(new ThreadStart(Netplay.TcpClientLoop))
			{
				Name = "TCP Client Thread",
				IsBackground = true
			}.Start();
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0004365F File Offset: 0x0004185F
		public static bool SetRemoteIP(string remoteAddress)
		{
			return Netplay.SetRemoteIPOld(remoteAddress);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00043668 File Offset: 0x00041868
		public static bool SetRemoteIPOld(string remoteAddress)
		{
			Netplay.IsHostAndPlay = false;
			try
			{
				IPAddress ipaddress;
				if (IPAddress.TryParse(remoteAddress, out ipaddress))
				{
					Netplay.ServerIP = ipaddress;
					Netplay.ServerIPText = ipaddress.ToString();
					return true;
				}
				IPAddress[] addressList = Dns.GetHostEntry(remoteAddress).AddressList;
				for (int i = 0; i < addressList.Length; i++)
				{
					if (Netplay.AcceptedFamilyType(addressList[i].AddressFamily))
					{
						Netplay.ServerIP = addressList[i];
						Netplay.ServerIPText = remoteAddress;
						return true;
					}
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x000436F0 File Offset: 0x000418F0
		public static void SetRemoteIPAsync(string remoteAddress, Action successCallBack)
		{
			try
			{
				IPAddress ipaddress;
				if (IPAddress.TryParse(remoteAddress, out ipaddress))
				{
					Netplay.ServerIP = ipaddress;
					Netplay.ServerIPText = ipaddress.ToString();
					successCallBack();
				}
				else
				{
					Netplay.InvalidateAllOngoingIPSetAttempts();
					Dns.BeginGetHostAddresses(remoteAddress, new AsyncCallback(Netplay.SetRemoteIPAsyncCallback), new Netplay.SetRemoteIPRequestInfo
					{
						RequestId = Netplay._currentRequestId,
						SuccessCallback = successCallBack,
						RemoteAddress = remoteAddress
					});
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0004376C File Offset: 0x0004196C
		public static void InvalidateAllOngoingIPSetAttempts()
		{
			Netplay._currentRequestId++;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0004377A File Offset: 0x0004197A
		private static bool AcceptedFamilyType(AddressFamily family)
		{
			return family == AddressFamily.InterNetwork;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00043784 File Offset: 0x00041984
		private static void SetRemoteIPAsyncCallback(IAsyncResult ar)
		{
			Netplay.SetRemoteIPRequestInfo setRemoteIPRequestInfo = (Netplay.SetRemoteIPRequestInfo)ar.AsyncState;
			if (setRemoteIPRequestInfo.RequestId != Netplay._currentRequestId)
			{
				return;
			}
			try
			{
				bool flag = false;
				IPAddress[] array = Dns.EndGetHostAddresses(ar);
				for (int i = 0; i < array.Length; i++)
				{
					if (Netplay.AcceptedFamilyType(array[i].AddressFamily))
					{
						Netplay.ServerIP = array[i];
						Netplay.ServerIPText = setRemoteIPRequestInfo.RemoteAddress;
						flag = true;
						break;
					}
				}
				if (flag)
				{
					setRemoteIPRequestInfo.SuccessCallback();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0004380C File Offset: 0x00041A0C
		public static void Initialize()
		{
			if (Main.dedServ)
			{
				for (int i = 0; i < 257; i++)
				{
					if (i < 256)
					{
						Netplay.Clients[i] = new RemoteClient();
					}
					NetMessage.buffer[i] = new MessageBuffer();
					NetMessage.buffer[i].whoAmI = i;
				}
			}
			NetMessage.buffer[256] = new MessageBuffer();
			NetMessage.buffer[256].whoAmI = 256;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00043882 File Offset: 0x00041A82
		public static void UpdateInMainThread()
		{
			if (Main.dedServ)
			{
				Netplay.UpdateServerInMainThread();
			}
			else
			{
				Netplay.UpdateClientInMainThread();
			}
			Netplay.UpdateDataRates();
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0004389C File Offset: 0x00041A9C
		public static void UpdateDataRates()
		{
			long timestamp = Stopwatch.GetTimestamp();
			if (Utils.SWTicksToTimeSpan(timestamp - Netplay.swTicksLast).TotalSeconds < 1.0)
			{
				return;
			}
			Netplay.swTicksLast = timestamp;
			Main.ActiveNetDiagnosticsUI.RotateSendRecvCounters();
		}

		// Token: 0x0600030F RID: 783 RVA: 0x000438DF File Offset: 0x00041ADF
		public static int GetSectionX(int x)
		{
			return x / 200;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x000438E8 File Offset: 0x00041AE8
		public static int GetSectionY(int y)
		{
			return y / 150;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000438F4 File Offset: 0x00041AF4
		private static void BroadcastThread()
		{
			Netplay.BroadcastClient = new UdpClient();
			new IPEndPoint(IPAddress.Any, 0);
			Netplay.BroadcastClient.EnableBroadcast = true;
			new DateTime(0L);
			int num = 0;
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					int value = 1010;
					binaryWriter.Write(value);
					binaryWriter.Write(Netplay.ListenPort);
					binaryWriter.Write(Main.worldName);
					string text = Dns.GetHostName();
					if (text == "localhost")
					{
						text = Environment.MachineName;
					}
					binaryWriter.Write(text);
					binaryWriter.Write((ushort)Main.maxTilesX);
					binaryWriter.Write(Main.ActiveWorldFileData.HasCrimson);
					binaryWriter.Write(Main.ActiveWorldFileData.GameMode);
					binaryWriter.Write((byte)Main.maxNetPlayers);
					num = (int)memoryStream.Position;
					binaryWriter.Write(0);
					binaryWriter.Write(Main.ActiveWorldFileData.IsHardMode);
					binaryWriter.Flush();
					array = memoryStream.ToArray();
				}
			}
			for (;;)
			{
				int num2 = 0;
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active)
					{
						num2++;
					}
				}
				array[num] = (byte)num2;
				try
				{
					Netplay.BroadcastClient.Send(array, array.Length, new IPEndPoint(IPAddress.Broadcast, 8888));
				}
				catch
				{
				}
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00043A84 File Offset: 0x00041C84
		public static void StartBroadCasting()
		{
			if (Netplay.broadcastThread != null)
			{
				Netplay.StopBroadCasting();
			}
			Netplay.broadcastThread = new Thread(new ThreadStart(Netplay.BroadcastThread));
			Netplay.broadcastThread.Start();
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00043AB2 File Offset: 0x00041CB2
		public static void StopBroadCasting()
		{
			if (Netplay.broadcastThread != null)
			{
				Netplay.broadcastThread.Abort();
				Netplay.broadcastThread = null;
			}
			if (Netplay.BroadcastClient != null)
			{
				Netplay.BroadcastClient.Close();
				Netplay.BroadcastClient = null;
			}
		}

		// Token: 0x04000221 RID: 545
		public const int MaxConnections = 256;

		// Token: 0x04000222 RID: 546
		public const int NetBufferSize = 1024;

		// Token: 0x04000223 RID: 547
		public const int DefaultPort = 7777;

		// Token: 0x04000225 RID: 549
		public static string BanFilePath = "banlist.txt";

		// Token: 0x04000226 RID: 550
		public static string ServerPassword = "";

		// Token: 0x04000227 RID: 551
		public static RemoteClient[] Clients = new RemoteClient[256];

		// Token: 0x04000228 RID: 552
		public static RemoteServer Connection = new RemoteServer();

		// Token: 0x04000229 RID: 553
		public static IPAddress ServerIP;

		// Token: 0x0400022A RID: 554
		public static string ServerIPText = "";

		// Token: 0x0400022B RID: 555
		public static bool IsHostAndPlay;

		// Token: 0x0400022C RID: 556
		public static string HostToken;

		// Token: 0x0400022D RID: 557
		public static ISocket TcpListener;

		// Token: 0x0400022E RID: 558
		public static int ListenPort = 7777;

		// Token: 0x0400022F RID: 559
		public static bool IsListening = true;

		// Token: 0x04000230 RID: 560
		public static bool UseUPNP = true;

		// Token: 0x04000231 RID: 561
		public static bool SaveOnServerExit = true;

		// Token: 0x04000232 RID: 562
		public static bool Disconnect;

		// Token: 0x04000233 RID: 563
		public static bool SpamCheck = false;

		// Token: 0x04000234 RID: 564
		public static bool HasClients;

		// Token: 0x04000235 RID: 565
		private static Thread _serverThread;

		// Token: 0x04000236 RID: 566
		public static MessageBuffer fullBuffer = new MessageBuffer();

		// Token: 0x04000237 RID: 567
		private static int _currentRequestId;

		// Token: 0x04000238 RID: 568
		private static long swTicksLast;

		// Token: 0x04000239 RID: 569
		private static UdpClient BroadcastClient = null;

		// Token: 0x0400023A RID: 570
		private static Thread broadcastThread = null;

		// Token: 0x02000605 RID: 1541
		private class SetRemoteIPRequestInfo
		{
			// Token: 0x04006404 RID: 25604
			public int RequestId;

			// Token: 0x04006405 RID: 25605
			public Action SuccessCallback;

			// Token: 0x04006406 RID: 25606
			public string RemoteAddress;
		}
	}
}
