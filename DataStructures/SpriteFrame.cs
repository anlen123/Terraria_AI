using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures
{
	// Token: 0x020005A2 RID: 1442
	public struct SpriteFrame
	{
		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x060038EC RID: 14572 RVA: 0x0064FAAA File Offset: 0x0064DCAA
		// (set) Token: 0x060038ED RID: 14573 RVA: 0x0064FAB2 File Offset: 0x0064DCB2
		public byte CurrentColumn
		{
			get
			{
				return this._currentColumn;
			}
			set
			{
				this._currentColumn = value;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x060038EE RID: 14574 RVA: 0x0064FABB File Offset: 0x0064DCBB
		// (set) Token: 0x060038EF RID: 14575 RVA: 0x0064FAC3 File Offset: 0x0064DCC3
		public byte CurrentRow
		{
			get
			{
				return this._currentRow;
			}
			set
			{
				this._currentRow = value;
			}
		}

		// Token: 0x060038F0 RID: 14576 RVA: 0x0064FACC File Offset: 0x0064DCCC
		public SpriteFrame(byte columns, byte rows)
		{
			this.PaddingX = 2;
			this.PaddingY = 2;
			this._currentColumn = 0;
			this._currentRow = 0;
			this.ColumnCount = columns;
			this.RowCount = rows;
		}

		// Token: 0x060038F1 RID: 14577 RVA: 0x0064FAF8 File Offset: 0x0064DCF8
		public SpriteFrame(byte columns, byte rows, byte currentColumn, byte currentRow)
		{
			this.PaddingX = 2;
			this.PaddingY = 2;
			this._currentColumn = currentColumn;
			this._currentRow = currentRow;
			this.ColumnCount = columns;
			this.RowCount = rows;
		}

		// Token: 0x060038F2 RID: 14578 RVA: 0x0064FB28 File Offset: 0x0064DD28
		public SpriteFrame With(byte columnToUse, byte rowToUse)
		{
			SpriteFrame result = this;
			result.CurrentColumn = columnToUse;
			result.CurrentRow = rowToUse;
			return result;
		}

		// Token: 0x060038F3 RID: 14579 RVA: 0x0064FB50 File Offset: 0x0064DD50
		public Rectangle GetSourceRectangle(Texture2D texture)
		{
			int num = texture.Width / (int)this.ColumnCount;
			int num2 = texture.Height / (int)this.RowCount;
			return new Rectangle((int)this.CurrentColumn * num, (int)this.CurrentRow * num2, num - ((this.ColumnCount == 1) ? 0 : this.PaddingX), num2 - ((this.RowCount == 1) ? 0 : this.PaddingY));
		}

		// Token: 0x04005D24 RID: 23844
		public int PaddingX;

		// Token: 0x04005D25 RID: 23845
		public int PaddingY;

		// Token: 0x04005D26 RID: 23846
		private byte _currentColumn;

		// Token: 0x04005D27 RID: 23847
		private byte _currentRow;

		// Token: 0x04005D28 RID: 23848
		public readonly byte ColumnCount;

		// Token: 0x04005D29 RID: 23849
		public readonly byte RowCount;
	}
}
