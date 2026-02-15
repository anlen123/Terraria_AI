using System;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000A7 RID: 167
	public static class Conditions
	{
		// Token: 0x020006B1 RID: 1713
		public class IsTile : GenCondition
		{
			// Token: 0x06003ECA RID: 16074 RVA: 0x006974BF File Offset: 0x006956BF
			public IsTile(params ushort[] types)
			{
				this._types = types;
			}

			// Token: 0x06003ECB RID: 16075 RVA: 0x006974D0 File Offset: 0x006956D0
			protected override bool CheckValidity(int x, int y)
			{
				if (!WorldGen.InWorld(x, y, 0))
				{
					return false;
				}
				if (GenBase._tiles[x, y].active())
				{
					for (int i = 0; i < this._types.Length; i++)
					{
						if (GenBase._tiles[x, y].type == this._types[i])
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x0400675F RID: 26463
			private ushort[] _types;
		}

		// Token: 0x020006B2 RID: 1714
		public class Continue : GenCondition
		{
			// Token: 0x06003ECC RID: 16076 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			protected override bool CheckValidity(int x, int y)
			{
				return false;
			}
		}

		// Token: 0x020006B3 RID: 1715
		public class BoolCheck : GenCondition
		{
			// Token: 0x06003ECE RID: 16078 RVA: 0x00697535 File Offset: 0x00695735
			public BoolCheck(bool theBool)
			{
				this._theBool = theBool;
			}

			// Token: 0x06003ECF RID: 16079 RVA: 0x00697544 File Offset: 0x00695744
			protected override bool CheckValidity(int x, int y)
			{
				return this._theBool;
			}

			// Token: 0x04006760 RID: 26464
			private bool _theBool;
		}

		// Token: 0x020006B4 RID: 1716
		public class MysticSnake : GenCondition
		{
			// Token: 0x06003ED0 RID: 16080 RVA: 0x0069754C File Offset: 0x0069574C
			protected override bool CheckValidity(int x, int y)
			{
				return GenBase._tiles[x, y].active() && !Main.tileCut[(int)GenBase._tiles[x, y].type] && GenBase._tiles[x, y].type != 504;
			}
		}

		// Token: 0x020006B5 RID: 1717
		public class InWorld : GenCondition
		{
			// Token: 0x06003ED2 RID: 16082 RVA: 0x006975A2 File Offset: 0x006957A2
			public InWorld(int fluff)
			{
				this._fluff = fluff;
			}

			// Token: 0x06003ED3 RID: 16083 RVA: 0x006975B1 File Offset: 0x006957B1
			protected override bool CheckValidity(int x, int y)
			{
				return WorldGen.InWorld(x, y, this._fluff);
			}

			// Token: 0x04006761 RID: 26465
			private int _fluff;
		}

		// Token: 0x020006B6 RID: 1718
		public class IsSolid : GenCondition
		{
			// Token: 0x06003ED4 RID: 16084 RVA: 0x006975C0 File Offset: 0x006957C0
			protected override bool CheckValidity(int x, int y)
			{
				return WorldGen.InWorld(x, y, 10) && GenBase._tiles[x, y].active() && Main.tileSolid[(int)GenBase._tiles[x, y].type];
			}
		}

		// Token: 0x020006B7 RID: 1719
		public class HasLava : GenCondition
		{
			// Token: 0x06003ED6 RID: 16086 RVA: 0x006975FB File Offset: 0x006957FB
			protected override bool CheckValidity(int x, int y)
			{
				return GenBase._tiles[x, y].liquid > 0 && GenBase._tiles[x, y].liquidType() == 1;
			}
		}

		// Token: 0x020006B8 RID: 1720
		public class NotNull : GenCondition
		{
			// Token: 0x06003ED8 RID: 16088 RVA: 0x00697627 File Offset: 0x00695827
			protected override bool CheckValidity(int x, int y)
			{
				return GenBase._tiles[x, y] != null;
			}
		}
	}
}
