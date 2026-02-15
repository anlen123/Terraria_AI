using System;

namespace Terraria.DataStructures
{
	// Token: 0x020005A8 RID: 1448
	public class TileObjectPreviewData
	{
		// Token: 0x0600392F RID: 14639 RVA: 0x0065049C File Offset: 0x0064E69C
		public void Reset()
		{
			this._active = false;
			this._size = Point16.Zero;
			this._coordinates = Point16.Zero;
			this._objectStart = Point16.Zero;
			this._percentValid = 0f;
			this._type = 0;
			this._style = 0;
			this._alternate = -1;
			this._random = -1;
			if (this._data != null)
			{
				Array.Clear(this._data, 0, (int)(this._dataSize.X * this._dataSize.Y));
			}
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x00650524 File Offset: 0x0064E724
		public void CopyFrom(TileObjectPreviewData copy)
		{
			this._type = copy._type;
			this._style = copy._style;
			this._alternate = copy._alternate;
			this._random = copy._random;
			this._active = copy._active;
			this._size = copy._size;
			this._coordinates = copy._coordinates;
			this._objectStart = copy._objectStart;
			this._percentValid = copy._percentValid;
			if (this._data == null)
			{
				this._data = new int[(int)copy._dataSize.X, (int)copy._dataSize.Y];
				this._dataSize = copy._dataSize;
			}
			else
			{
				Array.Clear(this._data, 0, this._data.Length);
			}
			if (this._dataSize.X < copy._dataSize.X || this._dataSize.Y < copy._dataSize.Y)
			{
				int num = (int)((copy._dataSize.X > this._dataSize.X) ? copy._dataSize.X : this._dataSize.X);
				int num2 = (int)((copy._dataSize.Y > this._dataSize.Y) ? copy._dataSize.Y : this._dataSize.Y);
				this._data = new int[num, num2];
				this._dataSize = new Point16(num, num2);
			}
			for (int i = 0; i < (int)copy._dataSize.X; i++)
			{
				for (int j = 0; j < (int)copy._dataSize.Y; j++)
				{
					this._data[i, j] = copy._data[i, j];
				}
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06003931 RID: 14641 RVA: 0x006506DD File Offset: 0x0064E8DD
		// (set) Token: 0x06003932 RID: 14642 RVA: 0x006506E5 File Offset: 0x0064E8E5
		public bool Active
		{
			get
			{
				return this._active;
			}
			set
			{
				this._active = value;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06003933 RID: 14643 RVA: 0x006506EE File Offset: 0x0064E8EE
		// (set) Token: 0x06003934 RID: 14644 RVA: 0x006506F6 File Offset: 0x0064E8F6
		public ushort Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06003935 RID: 14645 RVA: 0x006506FF File Offset: 0x0064E8FF
		// (set) Token: 0x06003936 RID: 14646 RVA: 0x00650707 File Offset: 0x0064E907
		public short Style
		{
			get
			{
				return this._style;
			}
			set
			{
				this._style = value;
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06003937 RID: 14647 RVA: 0x00650710 File Offset: 0x0064E910
		// (set) Token: 0x06003938 RID: 14648 RVA: 0x00650718 File Offset: 0x0064E918
		public int Alternate
		{
			get
			{
				return this._alternate;
			}
			set
			{
				this._alternate = value;
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06003939 RID: 14649 RVA: 0x00650721 File Offset: 0x0064E921
		// (set) Token: 0x0600393A RID: 14650 RVA: 0x00650729 File Offset: 0x0064E929
		public int Random
		{
			get
			{
				return this._random;
			}
			set
			{
				this._random = value;
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x0600393B RID: 14651 RVA: 0x00650732 File Offset: 0x0064E932
		// (set) Token: 0x0600393C RID: 14652 RVA: 0x0065073C File Offset: 0x0064E93C
		public Point16 Size
		{
			get
			{
				return this._size;
			}
			set
			{
				if (value.X <= 0 || value.Y <= 0)
				{
					throw new FormatException("PlacementData.Size was set to a negative value.");
				}
				if (value.X > this._dataSize.X || value.Y > this._dataSize.Y)
				{
					int num = (int)((value.X > this._dataSize.X) ? value.X : this._dataSize.X);
					int num2 = (int)((value.Y > this._dataSize.Y) ? value.Y : this._dataSize.Y);
					int[,] array = new int[num, num2];
					if (this._data != null)
					{
						for (int i = 0; i < (int)this._dataSize.X; i++)
						{
							for (int j = 0; j < (int)this._dataSize.Y; j++)
							{
								array[i, j] = this._data[i, j];
							}
						}
					}
					this._data = array;
					this._dataSize = new Point16(num, num2);
				}
				this._size = value;
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x0600393D RID: 14653 RVA: 0x0065084F File Offset: 0x0064EA4F
		// (set) Token: 0x0600393E RID: 14654 RVA: 0x00650857 File Offset: 0x0064EA57
		public Point16 Coordinates
		{
			get
			{
				return this._coordinates;
			}
			set
			{
				this._coordinates = value;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x0600393F RID: 14655 RVA: 0x00650860 File Offset: 0x0064EA60
		// (set) Token: 0x06003940 RID: 14656 RVA: 0x00650868 File Offset: 0x0064EA68
		public Point16 ObjectStart
		{
			get
			{
				return this._objectStart;
			}
			set
			{
				this._objectStart = value;
			}
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x00650874 File Offset: 0x0064EA74
		public void AllInvalid()
		{
			for (int i = 0; i < (int)this._size.X; i++)
			{
				for (int j = 0; j < (int)this._size.Y; j++)
				{
					if (this._data[i, j] != 0)
					{
						this._data[i, j] = 2;
					}
				}
			}
		}

		// Token: 0x17000493 RID: 1171
		public int this[int x, int y]
		{
			get
			{
				if (x < 0 || y < 0 || x >= (int)this._size.X || y >= (int)this._size.Y)
				{
					throw new IndexOutOfRangeException();
				}
				return this._data[x, y];
			}
			set
			{
				if (x < 0 || y < 0 || x >= (int)this._size.X || y >= (int)this._size.Y)
				{
					throw new IndexOutOfRangeException();
				}
				this._data[x, y] = value;
			}
		}

		// Token: 0x04005D54 RID: 23892
		private ushort _type;

		// Token: 0x04005D55 RID: 23893
		private short _style;

		// Token: 0x04005D56 RID: 23894
		private int _alternate;

		// Token: 0x04005D57 RID: 23895
		private int _random;

		// Token: 0x04005D58 RID: 23896
		private bool _active;

		// Token: 0x04005D59 RID: 23897
		private Point16 _size;

		// Token: 0x04005D5A RID: 23898
		private Point16 _coordinates;

		// Token: 0x04005D5B RID: 23899
		private Point16 _objectStart;

		// Token: 0x04005D5C RID: 23900
		private int[,] _data;

		// Token: 0x04005D5D RID: 23901
		private Point16 _dataSize;

		// Token: 0x04005D5E RID: 23902
		private float _percentValid;

		// Token: 0x04005D5F RID: 23903
		public static TileObjectPreviewData placementCache;

		// Token: 0x04005D60 RID: 23904
		public static TileObjectPreviewData randomCache;

		// Token: 0x04005D61 RID: 23905
		public const int None = 0;

		// Token: 0x04005D62 RID: 23906
		public const int ValidSpot = 1;

		// Token: 0x04005D63 RID: 23907
		public const int InvalidSpot = 2;
	}
}
