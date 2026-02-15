using System;
using System.IO;
using Terraria.DataStructures;

namespace Terraria.Net
{
	// Token: 0x02000170 RID: 368
	public struct NetPacket
	{
		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06001DD4 RID: 7636 RVA: 0x00501930 File Offset: 0x004FFB30
		// (set) Token: 0x06001DD5 RID: 7637 RVA: 0x00501938 File Offset: 0x004FFB38
		public int Length { get; private set; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x00501941 File Offset: 0x004FFB41
		public BinaryWriter Writer
		{
			get
			{
				return this.Buffer.Writer;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001DD7 RID: 7639 RVA: 0x0050194E File Offset: 0x004FFB4E
		public BinaryReader Reader
		{
			get
			{
				return this.Buffer.Reader;
			}
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x0050195C File Offset: 0x004FFB5C
		public NetPacket(ushort id, int size)
		{
			this = default(NetPacket);
			this.Id = id;
			this.Length = size + 5;
			if (this.Length > 65535)
			{
				throw new ArgumentOutOfRangeException("Tried to create a packet with length > " + ushort.MaxValue);
			}
			this.Buffer = BufferPool.Request(this.Length);
			this.Writer.Write((ushort)this.Length);
			this.Writer.Write(82);
			this.Writer.Write(id);
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x005019E3 File Offset: 0x004FFBE3
		public void Recycle()
		{
			this.Buffer.Recycle();
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x005019F0 File Offset: 0x004FFBF0
		public void ShrinkToFit()
		{
			if (this.Length == (int)this.Writer.BaseStream.Position)
			{
				return;
			}
			if (this.Writer.BaseStream.Position > (long)this.Length)
			{
				throw new IndexOutOfRangeException("Overwrite on supplied Length. Consider letting Length default to max packet size if you don't know how long it will be");
			}
			this.Length = (int)this.Writer.BaseStream.Position;
			this.Writer.Seek(0, SeekOrigin.Begin);
			this.Writer.Write((ushort)this.Length);
			this.Writer.Seek(this.Length, SeekOrigin.Begin);
		}

		// Token: 0x04001661 RID: 5729
		public const int HEADER_SIZE = 5;

		// Token: 0x04001662 RID: 5730
		public readonly ushort Id;

		// Token: 0x04001664 RID: 5732
		public readonly CachedBuffer Buffer;
	}
}
