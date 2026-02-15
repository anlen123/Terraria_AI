using System;
using System.IO;
using Terraria.Net.Sockets;

namespace Terraria
{
	// Token: 0x02000032 RID: 50
	public class RemoteServer
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0004252B File Offset: 0x0004072B
		public bool HideStatusTextPercent
		{
			get
			{
				return this.ServerSpecialFlags[0];
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00042539 File Offset: 0x00040739
		public bool StatusTextHasShadows
		{
			get
			{
				return this.ServerSpecialFlags[1];
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00042547 File Offset: 0x00040747
		public bool ServerWantsToRunCheckBytesInClientLoopThread
		{
			get
			{
				return this.ServerSpecialFlags[2];
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00042555 File Offset: 0x00040755
		public void ResetSpecialFlags()
		{
			this.ServerSpecialFlags = 0;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00042563 File Offset: 0x00040763
		public bool ReadBufferFull
		{
			get
			{
				return NetMessage.buffer[256].RemainingReadBufferLength < this.ReadBuffer.Length;
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0004257F File Offset: 0x0004077F
		public bool IsConnected()
		{
			return !this.PendingTermination && this.Socket.IsConnected();
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00042596 File Offset: 0x00040796
		public void ClientWriteCallBack(object state)
		{
			NetMessage.buffer[256].spamCount--;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x000425B0 File Offset: 0x000407B0
		public void ClientReadCallBack(object state, int streamLength)
		{
			try
			{
				if (!Netplay.Disconnect)
				{
					if (streamLength == 0)
					{
						this.PendingTermination = true;
					}
					else
					{
						if (Main.ignoreErrors)
						{
							try
							{
								NetMessage.ReceiveBytes(this.ReadBuffer, streamLength, 256);
								goto IL_41;
							}
							catch
							{
								goto IL_41;
							}
						}
						NetMessage.ReceiveBytes(this.ReadBuffer, streamLength, 256);
					}
				}
				IL_41:
				this.IsReading = false;
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
		}

		// Token: 0x04000216 RID: 534
		public ISocket Socket = new TcpSocket();

		// Token: 0x04000217 RID: 535
		public bool IsActive;

		// Token: 0x04000218 RID: 536
		public int State;

		// Token: 0x04000219 RID: 537
		public int TimeOutTimer;

		// Token: 0x0400021A RID: 538
		public bool PendingTermination;

		// Token: 0x0400021B RID: 539
		public bool IsReading;

		// Token: 0x0400021C RID: 540
		public byte[] ReadBuffer;

		// Token: 0x0400021D RID: 541
		public string StatusText;

		// Token: 0x0400021E RID: 542
		public int StatusCount;

		// Token: 0x0400021F RID: 543
		public int StatusMax;

		// Token: 0x04000220 RID: 544
		public BitsByte ServerSpecialFlags;
	}
}
