using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace Terraria.Testing
{
	// Token: 0x02000117 RID: 279
	public class PacketHistory
	{
		// Token: 0x06001AF8 RID: 6904 RVA: 0x0000357B File Offset: 0x0000177B
		public PacketHistory(int historySize = 100, int bufferSize = 65535)
		{
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x004F8850 File Offset: 0x004F6A50
		[Conditional("DEBUG")]
		public void Record(byte[] buffer, int offset, int length)
		{
			length = Math.Max(0, length);
			PacketHistory.PacketView packetView = this.AppendPacket(length);
			Buffer.BlockCopy(buffer, offset, this._buffer, packetView.Offset, length);
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x004F8884 File Offset: 0x004F6A84
		private PacketHistory.PacketView AppendPacket(int size)
		{
			int num = this._bufferPosition;
			if (num + size > this._buffer.Length)
			{
				num = 0;
			}
			PacketHistory.PacketView packetView = new PacketHistory.PacketView(num, size, DateTime.Now);
			this._packets[this._historyPosition] = packetView;
			this._historyPosition = (this._historyPosition + 1) % this._packets.Length;
			this._bufferPosition = num + size;
			return packetView;
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x004F88E8 File Offset: 0x004F6AE8
		[Conditional("DEBUG")]
		public void Dump(string reason)
		{
			byte[] dst = new byte[this._buffer.Length];
			Buffer.BlockCopy(this._buffer, this._bufferPosition, dst, 0, this._buffer.Length - this._bufferPosition);
			Buffer.BlockCopy(this._buffer, 0, dst, this._buffer.Length - this._bufferPosition, this._bufferPosition);
			StringBuilder stringBuilder = new StringBuilder();
			int num = 1;
			for (int i = 0; i < this._packets.Length; i++)
			{
				PacketHistory.PacketView packetView = this._packets[(i + this._historyPosition) % this._packets.Length];
				if (packetView.Offset != 0 || packetView.Length != 0)
				{
					stringBuilder.Append(string.Format("Packet {0} [Assumed MessageID: {4}, Size: {2}, Buffer Position: {1}, Timestamp: {3:G}]\r\n", new object[]
					{
						num++,
						packetView.Offset,
						packetView.Length,
						packetView.Time,
						this._buffer[packetView.Offset]
					}));
					for (int j = 0; j < packetView.Length; j++)
					{
						stringBuilder.Append(this._buffer[packetView.Offset + j].ToString("X2") + " ");
						if (j % 16 == 15 && j != this._packets.Length - 1)
						{
							stringBuilder.Append("\r\n");
						}
					}
					stringBuilder.Append("\r\n\r\n");
				}
			}
			stringBuilder.Append(reason);
			Directory.CreateDirectory(Path.Combine(Main.SavePath, "NetDump"));
			File.WriteAllText(Path.Combine(Main.SavePath, "NetDump", this.CreateDumpFileName()), stringBuilder.ToString());
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x004F8AB4 File Offset: 0x004F6CB4
		private string CreateDumpFileName()
		{
			DateTime dateTime = DateTime.Now.ToLocalTime();
			return string.Format("Net_{0}_{1}_{2}_{3}.txt", new object[]
			{
				Main.dedServ ? "TerrariaServer" : "Terraria",
				Main.versionNumber,
				dateTime.ToString("MM-dd-yy_HH-mm-ss-ffff", CultureInfo.InvariantCulture),
				Thread.CurrentThread.ManagedThreadId
			});
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x004F8B23 File Offset: 0x004F6D23
		[Conditional("DEBUG")]
		private void InitializeBuffer(int historySize, int bufferSize)
		{
			this._packets = new PacketHistory.PacketView[historySize];
			this._buffer = new byte[bufferSize];
		}

		// Token: 0x04001538 RID: 5432
		private byte[] _buffer;

		// Token: 0x04001539 RID: 5433
		private PacketHistory.PacketView[] _packets;

		// Token: 0x0400153A RID: 5434
		private int _bufferPosition;

		// Token: 0x0400153B RID: 5435
		private int _historyPosition;

		// Token: 0x02000727 RID: 1831
		private struct PacketView
		{
			// Token: 0x06004071 RID: 16497 RVA: 0x0069CFF2 File Offset: 0x0069B1F2
			public PacketView(int offset, int length, DateTime time)
			{
				this.Offset = offset;
				this.Length = length;
				this.Time = time;
			}

			// Token: 0x0400693E RID: 26942
			public static readonly PacketHistory.PacketView Empty = new PacketHistory.PacketView(0, 0, DateTime.Now);

			// Token: 0x0400693F RID: 26943
			public readonly int Offset;

			// Token: 0x04006940 RID: 26944
			public readonly int Length;

			// Token: 0x04006941 RID: 26945
			public readonly DateTime Time;
		}
	}
}
