using System;
using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000BA RID: 186
	public class SimpleStructure : GenStructure
	{
		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06001786 RID: 6022 RVA: 0x004DDB48 File Offset: 0x004DBD48
		public int Width
		{
			get
			{
				return this._width;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06001787 RID: 6023 RVA: 0x004DDB50 File Offset: 0x004DBD50
		public int Height
		{
			get
			{
				return this._height;
			}
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x004DDB58 File Offset: 0x004DBD58
		public SimpleStructure(params string[] data)
		{
			this.ReadData(data);
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x004DDB67 File Offset: 0x004DBD67
		public SimpleStructure(string data)
		{
			this.ReadData(data.Split(new char[]
			{
				'\n'
			}));
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x004DDB88 File Offset: 0x004DBD88
		private void ReadData(string[] lines)
		{
			this._height = lines.Length;
			this._width = lines[0].Length;
			this._data = new int[this._width, this._height];
			for (int i = 0; i < this._height; i++)
			{
				for (int j = 0; j < this._width; j++)
				{
					int num = (int)lines[i][j];
					if (num >= 48 && num <= 57)
					{
						this._data[j, i] = num - 48;
					}
					else
					{
						this._data[j, i] = -1;
					}
				}
			}
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x004DDC1A File Offset: 0x004DBE1A
		public SimpleStructure SetActions(params GenAction[] actions)
		{
			this._actions = actions;
			return this;
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x004DDC24 File Offset: 0x004DBE24
		public SimpleStructure Mirror(bool horizontalMirror, bool verticalMirror)
		{
			this._xMirror = horizontalMirror;
			this._yMirror = verticalMirror;
			return this;
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x004DDC38 File Offset: 0x004DBE38
		public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
		{
			if (!structures.CanPlace(new Rectangle(origin.X, origin.Y, this._width, this._height), 0))
			{
				return false;
			}
			for (int i = 0; i < this._width; i++)
			{
				for (int j = 0; j < this._height; j++)
				{
					int num = this._xMirror ? (-i) : i;
					int num2 = this._yMirror ? (-j) : j;
					if (this._data[i, j] != -1 && !this._actions[this._data[i, j]].Apply(origin, num + origin.X, num2 + origin.Y, new object[0]))
					{
						return false;
					}
				}
			}
			structures.AddProtectedStructure(new Rectangle(origin.X, origin.Y, this._width, this._height), 0);
			return true;
		}

		// Token: 0x04001266 RID: 4710
		private int[,] _data;

		// Token: 0x04001267 RID: 4711
		private int _width;

		// Token: 0x04001268 RID: 4712
		private int _height;

		// Token: 0x04001269 RID: 4713
		private GenAction[] _actions;

		// Token: 0x0400126A RID: 4714
		private bool _xMirror;

		// Token: 0x0400126B RID: 4715
		private bool _yMirror;
	}
}
