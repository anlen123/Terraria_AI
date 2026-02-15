using System;
using System.IO;

namespace Terraria.DataStructures
{
	// Token: 0x0200058D RID: 1421
	public class CachedBuffer
	{
		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06003825 RID: 14373 RVA: 0x00630564 File Offset: 0x0062E764
		public int Length
		{
			get
			{
				return this.Data.Length;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06003826 RID: 14374 RVA: 0x0063056E File Offset: 0x0062E76E
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x00630578 File Offset: 0x0062E778
		public CachedBuffer(byte[] data)
		{
			this.Data = data;
			this._memoryStream = new MemoryStream(data);
			this.Writer = new BinaryWriter(this._memoryStream);
			this.Reader = new BinaryReader(this._memoryStream);
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x006305C7 File Offset: 0x0062E7C7
		internal CachedBuffer Activate()
		{
			this._isActive = true;
			this._memoryStream.Position = 0L;
			return this;
		}

		// Token: 0x06003829 RID: 14377 RVA: 0x006305DE File Offset: 0x0062E7DE
		public void Recycle()
		{
			if (this._isActive)
			{
				this._isActive = false;
				BufferPool.Recycle(this);
			}
		}

		// Token: 0x04005C34 RID: 23604
		public readonly byte[] Data;

		// Token: 0x04005C35 RID: 23605
		public readonly BinaryWriter Writer;

		// Token: 0x04005C36 RID: 23606
		public readonly BinaryReader Reader;

		// Token: 0x04005C37 RID: 23607
		private readonly MemoryStream _memoryStream;

		// Token: 0x04005C38 RID: 23608
		private bool _isActive = true;
	}
}
