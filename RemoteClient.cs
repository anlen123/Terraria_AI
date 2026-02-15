using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.Net.Sockets;

namespace Terraria
{
	// Token: 0x02000031 RID: 49
	public class RemoteClient
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060002CE RID: 718 RVA: 0x00041BF4 File Offset: 0x0003FDF4
		// (remove) Token: 0x060002CF RID: 719 RVA: 0x00041C28 File Offset: 0x0003FE28
		public static event Action<int, Point> NetSectionActivated;

		// Token: 0x060002D0 RID: 720 RVA: 0x00041C5B File Offset: 0x0003FE5B
		public bool IsConnected()
		{
			return this.Socket != null && this.Socket.IsConnected();
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x00041C72 File Offset: 0x0003FE72
		public bool ReadBufferFull
		{
			get
			{
				return NetMessage.buffer[this.Id].RemainingReadBufferLength < this.ReadBuffer.Length;
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00041C90 File Offset: 0x0003FE90
		public void SpamUpdate()
		{
			if (!Netplay.SpamCheck)
			{
				this.SpamProjectile = 0f;
				this.SpamDeleteBlock = 0f;
				this.SpamAddBlock = 0f;
				this.SpamWater = 0f;
				return;
			}
			if (this.SpamProjectile > this.SpamProjectileMax)
			{
				NetMessage.BootPlayer(this.Id, NetworkText.FromKey("Net.CheatingProjectileSpam", new object[0]));
			}
			if (this.SpamAddBlock > this.SpamAddBlockMax)
			{
				NetMessage.BootPlayer(this.Id, NetworkText.FromKey("Net.CheatingTileSpam", new object[0]));
			}
			if (this.SpamDeleteBlock > this.SpamDeleteBlockMax)
			{
				NetMessage.BootPlayer(this.Id, NetworkText.FromKey("Net.CheatingTileRemovalSpam", new object[0]));
			}
			if (this.SpamWater > this.SpamWaterMax)
			{
				NetMessage.BootPlayer(this.Id, NetworkText.FromKey("Net.CheatingLiquidSpam", new object[0]));
			}
			this.SpamProjectile -= 0.4f;
			if (this.SpamProjectile < 0f)
			{
				this.SpamProjectile = 0f;
			}
			this.SpamAddBlock -= 0.3f;
			if (this.SpamAddBlock < 0f)
			{
				this.SpamAddBlock = 0f;
			}
			this.SpamDeleteBlock -= 5f;
			if (this.SpamDeleteBlock < 0f)
			{
				this.SpamDeleteBlock = 0f;
			}
			this.SpamWater -= 0.2f;
			if (this.SpamWater < 0f)
			{
				this.SpamWater = 0f;
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00041E1D File Offset: 0x0004001D
		public void SpamClear()
		{
			this.SpamProjectile = 0f;
			this.SpamAddBlock = 0f;
			this.SpamDeleteBlock = 0f;
			this.SpamWater = 0f;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00041E4C File Offset: 0x0004004C
		public static void CheckSection(int playerIndex, Vector2 position, int fluff = 1)
		{
			RemoteClient remoteClient = Netplay.Clients[playerIndex];
			if (remoteClient.CheckingSections)
			{
				return;
			}
			RemoteClient.CheckSection_ForClient(remoteClient, position, fluff);
			try
			{
				remoteClient.CheckingSections = true;
				for (int i = 0; i < 255; i++)
				{
					Player player = Main.player[i];
					if (player.active && player.spectating == playerIndex)
					{
						RemoteClient.CheckSection(i, position, fluff);
					}
				}
			}
			finally
			{
				remoteClient.CheckingSections = false;
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00041EC4 File Offset: 0x000400C4
		private static void CheckSection_ForClient(RemoteClient client, Vector2 position, int fluff)
		{
			ActiveSections.CheckSection(position, fluff);
			int sectionX = Netplay.GetSectionX((int)(position.X / 16f));
			int sectionY = Netplay.GetSectionY((int)(position.Y / 16f));
			int num = 0;
			for (int i = sectionX - fluff; i < sectionX + fluff + 1; i++)
			{
				for (int j = sectionY - fluff; j < sectionY + fluff + 1; j++)
				{
					if (i >= 0 && i < Main.maxSectionsX && j >= 0 && j < Main.maxSectionsY)
					{
						bool flag = client.IsSectionActive(new Point(i, j));
						client.TileSectionsCheckTime[i, j] = Main.GameUpdateCount;
						if (!flag)
						{
							RemoteClient.NetSectionActivated(client.Id, new Point(i, j));
						}
						if (!client.TileSections[i, j])
						{
							num++;
						}
					}
				}
			}
			if (num > 0)
			{
				int num2 = num;
				NetMessage.SendData(9, client.Id, -1, Lang.inter[44].ToNetworkText(), num2, 0f, 0f, 0f, 0, 0, 0);
				client.StatusText2 = Language.GetTextValue("Net.IsReceivingTileData");
				client.StatusMax += num2;
				for (int k = sectionX - fluff; k < sectionX + fluff + 1; k++)
				{
					for (int l = sectionY - fluff; l < sectionY + fluff + 1; l++)
					{
						NetMessage.SendSection(client.Id, k, l);
					}
				}
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0004202C File Offset: 0x0004022C
		public bool SectionRange(int size, int firstX, int firstY)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = firstX;
				int num2 = firstY;
				if (i == 1)
				{
					num += size;
				}
				if (i == 2)
				{
					num2 += size;
				}
				if (i == 3)
				{
					num += size;
					num2 += size;
				}
				int sectionX = Netplay.GetSectionX(num);
				int sectionY = Netplay.GetSectionY(num2);
				if (this.TileSections[sectionX, sectionY])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00042087 File Offset: 0x00040287
		public bool IsSectionActive(Point sectionCoords)
		{
			sectionCoords = sectionCoords.ClampSectionCoords();
			return this.TileSectionsCheckTime[sectionCoords.X, sectionCoords.Y] + ActiveSections.SectionInactiveTime >= Main.GameUpdateCount;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x000420B8 File Offset: 0x000402B8
		public void Reset()
		{
			Array.Clear(this.TileSections, 0, this.TileSections.Length);
			Array.Clear(this.TileSectionsCheckTime, 0, this.TileSectionsCheckTime.Length);
			if (this.Id < 255)
			{
				Main.player[this.Id] = new Player();
			}
			this.TimeOutTimer = 0;
			this.StatusCount = 0;
			this.StatusMax = 0;
			this.StatusText2 = "";
			this.StatusText = "";
			this.State = 0;
			this._isReading = false;
			this.PendingTermination = false;
			this.PendingTerminationApproved = false;
			this.SpamClear();
			this.IsActive = false;
			NetMessage.buffer[this.Id].Reset();
			if (this.Socket != null)
			{
				this.Socket.Close();
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0004218B File Offset: 0x0004038B
		public void ServerWriteCallBack(object state)
		{
			NetMessage.buffer[this.Id].spamCount--;
			if (this.StatusMax > 0)
			{
				this.StatusCount++;
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x000421BD File Offset: 0x000403BD
		public void Update()
		{
			if (!this.IsActive)
			{
				this.State = 0;
				this.IsActive = true;
			}
			this.TryRead();
			this.UpdateStatusText();
		}

		// Token: 0x060002DB RID: 731 RVA: 0x000421E4 File Offset: 0x000403E4
		private void TryRead()
		{
			if (this._isReading)
			{
				return;
			}
			try
			{
				if (this.Socket.IsDataAvailable() && !this.ReadBufferFull)
				{
					this._isReading = true;
					this.Socket.AsyncReceive(this.ReadBuffer, 0, this.ReadBuffer.Length, new SocketReceiveCallback(this.ServerReadCallBack), null);
				}
			}
			catch
			{
				this.PendingTermination = true;
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00042260 File Offset: 0x00040460
		private void ServerReadCallBack(object state, int length)
		{
			if (!Netplay.Disconnect)
			{
				if (length == 0)
				{
					this.PendingTermination = true;
				}
				else
				{
					try
					{
						NetMessage.ReceiveBytes(this.ReadBuffer, length, this.Id);
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
			this._isReading = false;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x000422BC File Offset: 0x000404BC
		private void UpdateStatusText()
		{
			if (this.StatusMax > 0 && this.StatusText2 != "")
			{
				if (this.StatusCount >= this.StatusMax)
				{
					this.StatusText = Language.GetTextValue("Net.ClientStatusComplete", this.Socket.GetRemoteAddress(), this.Name, this.StatusText2);
					this.StatusText2 = "";
					this.StatusMax = 0;
					this.StatusCount = 0;
					return;
				}
				this.StatusText = string.Concat(new object[]
				{
					"(",
					this.Socket.GetRemoteAddress(),
					") ",
					this.Name,
					" ",
					this.StatusText2,
					": ",
					(int)((float)this.StatusCount / (float)this.StatusMax * 100f),
					"%"
				});
				return;
			}
			else
			{
				if (this.State == 0)
				{
					this.StatusText = Language.GetTextValue("Net.ClientConnecting", string.Format("({0}) {1}", this.Socket.GetRemoteAddress(), this.Name));
					return;
				}
				if (this.State == 1)
				{
					this.StatusText = Language.GetTextValue("Net.ClientSendingData", this.Socket.GetRemoteAddress(), this.Name);
					return;
				}
				if (this.State == 2)
				{
					this.StatusText = Language.GetTextValue("Net.ClientRequestedWorldInfo", this.Socket.GetRemoteAddress(), this.Name);
					return;
				}
				if (this.State != 3 && this.State == 10)
				{
					try
					{
						this.StatusText = Language.GetTextValue("Net.ClientPlaying", this.Socket.GetRemoteAddress(), this.Name);
					}
					catch (Exception)
					{
						this.PendingTermination = true;
					}
				}
				return;
			}
		}

		// Token: 0x040001FC RID: 508
		public ISocket Socket;

		// Token: 0x040001FD RID: 509
		public int Id;

		// Token: 0x040001FE RID: 510
		public string Name = "Anonymous";

		// Token: 0x040001FF RID: 511
		public bool IsActive;

		// Token: 0x04000200 RID: 512
		public bool PendingTermination;

		// Token: 0x04000201 RID: 513
		public bool PendingTerminationApproved;

		// Token: 0x04000202 RID: 514
		public bool IsAnnouncementCompleted;

		// Token: 0x04000203 RID: 515
		public int State;

		// Token: 0x04000204 RID: 516
		public int TimeOutTimer;

		// Token: 0x04000205 RID: 517
		public string StatusText = "";

		// Token: 0x04000206 RID: 518
		public string StatusText2;

		// Token: 0x04000207 RID: 519
		public int StatusCount;

		// Token: 0x04000208 RID: 520
		public int StatusMax;

		// Token: 0x04000209 RID: 521
		public bool[,] TileSections = new bool[Main.maxTilesX / 200 + 1, Main.maxTilesY / 150 + 1];

		// Token: 0x0400020A RID: 522
		public uint[,] TileSectionsCheckTime = new uint[Main.maxTilesX / 200 + 1, Main.maxTilesY / 150 + 1];

		// Token: 0x0400020B RID: 523
		public bool CheckingSections;

		// Token: 0x0400020C RID: 524
		public byte[] ReadBuffer;

		// Token: 0x0400020D RID: 525
		public float SpamProjectile;

		// Token: 0x0400020E RID: 526
		public float SpamAddBlock;

		// Token: 0x0400020F RID: 527
		public float SpamDeleteBlock;

		// Token: 0x04000210 RID: 528
		public float SpamWater;

		// Token: 0x04000211 RID: 529
		public float SpamProjectileMax = 100f;

		// Token: 0x04000212 RID: 530
		public float SpamAddBlockMax = 100f;

		// Token: 0x04000213 RID: 531
		public float SpamDeleteBlockMax = 500f;

		// Token: 0x04000214 RID: 532
		public float SpamWaterMax = 50f;

		// Token: 0x04000215 RID: 533
		private volatile bool _isReading;
	}
}
