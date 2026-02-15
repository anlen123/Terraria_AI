using System;
using System.Collections.Generic;
using System.IO;
using Terraria.Net.Sockets;

namespace Terraria.Net
{
	// Token: 0x0200016F RID: 367
	public class NetManager
	{
		// Token: 0x06001DC2 RID: 7618 RVA: 0x00501643 File Offset: 0x004FF843
		private NetManager()
		{
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x00501658 File Offset: 0x004FF858
		public void Register<T>() where T : NetModule, new()
		{
			T t = Activator.CreateInstance<T>();
			NetManager.PacketTypeStorage<T>.Id = this._moduleCount;
			NetManager.PacketTypeStorage<T>.Module = t;
			this._modules[this._moduleCount] = t;
			this._moduleCount += 1;
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x005016A2 File Offset: 0x004FF8A2
		public NetModule GetModule<T>() where T : NetModule
		{
			return NetManager.PacketTypeStorage<T>.Module;
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x005016AE File Offset: 0x004FF8AE
		public ushort GetId<T>() where T : NetModule
		{
			return NetManager.PacketTypeStorage<T>.Id;
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x005016B5 File Offset: 0x004FF8B5
		public void Read(BinaryReader reader, int userId, int readLength)
		{
			this.Read(reader, userId, readLength, true);
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x005016C4 File Offset: 0x004FF8C4
		private void Read(BinaryReader reader, int userId, int readLength, bool addToDiagnostics)
		{
			ushort num = reader.ReadUInt16();
			if (this._modules.ContainsKey(num))
			{
				this._modules[num].Deserialize(reader, userId);
			}
			if (addToDiagnostics)
			{
				Main.ActiveNetDiagnosticsUI.CountReadModuleMessage((int)num, readLength);
			}
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x0050170C File Offset: 0x004FF90C
		public void Broadcast(NetPacket packet, int ignoreClient = -1)
		{
			for (int i = 0; i < 256; i++)
			{
				if (i != ignoreClient && Netplay.Clients[i].IsConnected())
				{
					this.SendData(Netplay.Clients[i].Socket, packet);
				}
			}
			packet.Recycle();
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x00501758 File Offset: 0x004FF958
		public void Broadcast(NetPacket packet, NetManager.BroadcastCondition conditionToBroadcast, int ignoreClient = -1)
		{
			for (int i = 0; i < 256; i++)
			{
				if (i != ignoreClient && Netplay.Clients[i].IsConnected() && conditionToBroadcast(i))
				{
					this.SendData(Netplay.Clients[i].Socket, packet);
				}
			}
			packet.Recycle();
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x005017AA File Offset: 0x004FF9AA
		private void SendToSelf(NetPacket packet)
		{
			packet.Reader.BaseStream.Position = 3L;
			this.Read(packet.Reader, Main.myPlayer, packet.Length, false);
			packet.Recycle();
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x005017E0 File Offset: 0x004FF9E0
		public void BroadcastOrLoopback(NetPacket packet)
		{
			if (Main.netMode == 2)
			{
				this.Broadcast(packet, -1);
				return;
			}
			if (Main.netMode == 0)
			{
				this.SendToSelf(packet);
				return;
			}
			packet.Recycle();
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x00501809 File Offset: 0x004FFA09
		public void SendToServerOrLoopback(NetPacket packet)
		{
			if (Main.netMode == 1)
			{
				this.SendToServer(packet);
				return;
			}
			if (Main.netMode == 0)
			{
				this.SendToSelf(packet);
				return;
			}
			packet.Recycle();
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x00501831 File Offset: 0x004FFA31
		public void SendToServerOrBroadcast(NetPacket packet)
		{
			if (Main.netMode == 1)
			{
				this.SendToServer(packet);
				return;
			}
			if (Main.netMode == 2)
			{
				this.Broadcast(packet, -1);
				return;
			}
			packet.Recycle();
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x0050185B File Offset: 0x004FFA5B
		public void SendToServer(NetPacket packet)
		{
			this.SendData(Netplay.Connection.Socket, packet);
			packet.Recycle();
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x00501875 File Offset: 0x004FFA75
		public void SendToClient(NetPacket packet, int playerId)
		{
			this.SendData(Netplay.Clients[playerId].Socket, packet);
			packet.Recycle();
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x00501891 File Offset: 0x004FFA91
		public void SendToClientOrLoopback(NetPacket packet, int playerId)
		{
			if (Main.netMode == 0 && playerId == Main.myPlayer)
			{
				this.SendToSelf(packet);
				return;
			}
			this.SendToClient(packet, playerId);
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x005018B4 File Offset: 0x004FFAB4
		private void SendData(ISocket socket, NetPacket packet)
		{
			if (Main.netMode == 0)
			{
				return;
			}
			packet.ShrinkToFit();
			try
			{
				Main.ActiveNetDiagnosticsUI.CountSentModuleMessage((int)packet.Id, packet.Length);
				socket.AsyncSend(packet.Buffer.Data, 0, packet.Length, new SocketSendCallback(NetManager.EmptyCallback), null);
			}
			catch
			{
			}
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x00009E06 File Offset: 0x00008006
		private static void EmptyCallback(object state)
		{
		}

		// Token: 0x0400165E RID: 5726
		public static readonly NetManager Instance = new NetManager();

		// Token: 0x0400165F RID: 5727
		private Dictionary<ushort, NetModule> _modules = new Dictionary<ushort, NetModule>();

		// Token: 0x04001660 RID: 5728
		private ushort _moduleCount;

		// Token: 0x02000749 RID: 1865
		private class PacketTypeStorage<T> where T : NetModule
		{
			// Token: 0x04006997 RID: 27031
			public static ushort Id;

			// Token: 0x04006998 RID: 27032
			public static T Module;
		}

		// Token: 0x0200074A RID: 1866
		// (Invoke) Token: 0x060040C8 RID: 16584
		public delegate bool BroadcastCondition(int clientIndex);
	}
}
