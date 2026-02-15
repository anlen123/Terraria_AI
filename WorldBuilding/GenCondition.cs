using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000A6 RID: 166
	public abstract class GenCondition : GenBase
	{
		// Token: 0x06001731 RID: 5937 RVA: 0x004DD1B4 File Offset: 0x004DB3B4
		public bool IsValid(int x, int y)
		{
			switch (this._areaType)
			{
			case GenCondition.AreaType.And:
				for (int i = x; i < x + this._width; i++)
				{
					for (int j = y; j < y + this._height; j++)
					{
						if (!this.CheckValidity(i, j))
						{
							return this.InvertResults;
						}
					}
				}
				return !this.InvertResults;
			case GenCondition.AreaType.Or:
				for (int k = x; k < x + this._width; k++)
				{
					for (int l = y; l < y + this._height; l++)
					{
						if (this.CheckValidity(k, l))
						{
							return !this.InvertResults;
						}
					}
				}
				return this.InvertResults;
			case GenCondition.AreaType.None:
				return this.CheckValidity(x, y) ^ this.InvertResults;
			default:
				return true;
			}
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x004DD277 File Offset: 0x004DB477
		public GenCondition Not()
		{
			this.InvertResults = !this.InvertResults;
			return this;
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x004DD289 File Offset: 0x004DB489
		public GenCondition AreaOr(int width, int height)
		{
			this._areaType = GenCondition.AreaType.Or;
			this._width = width;
			this._height = height;
			return this;
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x004DD2A1 File Offset: 0x004DB4A1
		public GenCondition AreaAnd(int width, int height)
		{
			this._areaType = GenCondition.AreaType.And;
			this._width = width;
			this._height = height;
			return this;
		}

		// Token: 0x06001735 RID: 5941
		protected abstract bool CheckValidity(int x, int y);

		// Token: 0x040011BD RID: 4541
		private bool InvertResults;

		// Token: 0x040011BE RID: 4542
		private int _width;

		// Token: 0x040011BF RID: 4543
		private int _height;

		// Token: 0x040011C0 RID: 4544
		private GenCondition.AreaType _areaType = GenCondition.AreaType.None;

		// Token: 0x020006B0 RID: 1712
		private enum AreaType
		{
			// Token: 0x0400675C RID: 26460
			And,
			// Token: 0x0400675D RID: 26461
			Or,
			// Token: 0x0400675E RID: 26462
			None
		}
	}
}
