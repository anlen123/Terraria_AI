using System;
using Terraria.IO;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000AD RID: 173
	public static class Passes
	{
		// Token: 0x020006D7 RID: 1751
		public class Clear : GenPass
		{
			// Token: 0x06003F1E RID: 16158 RVA: 0x0069843D File Offset: 0x0069663D
			public Clear() : base("clear", 1.0)
			{
			}

			// Token: 0x06003F1F RID: 16159 RVA: 0x00698454 File Offset: 0x00696654
			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				for (int i = 0; i < GenBase._worldWidth; i++)
				{
					for (int j = 0; j < GenBase._worldHeight; j++)
					{
						if (GenBase._tiles[i, j] == null)
						{
							GenBase._tiles[i, j] = new Tile();
						}
						else
						{
							GenBase._tiles[i, j].ClearEverything();
						}
					}
				}
			}
		}

		// Token: 0x020006D8 RID: 1752
		public class ScatterCustom : GenPass
		{
			// Token: 0x06003F20 RID: 16160 RVA: 0x006984B3 File Offset: 0x006966B3
			public ScatterCustom(string name, double loadWeight, int count, GenBase.CustomPerUnitAction perUnit = null) : base(name, loadWeight)
			{
				this._perUnit = perUnit;
				this._count = count;
			}

			// Token: 0x06003F21 RID: 16161 RVA: 0x006984CC File Offset: 0x006966CC
			public void SetCustomAction(GenBase.CustomPerUnitAction perUnit)
			{
				this._perUnit = perUnit;
			}

			// Token: 0x06003F22 RID: 16162 RVA: 0x006984D8 File Offset: 0x006966D8
			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				int i = this._count;
				while (i > 0)
				{
					if (this._perUnit(GenBase._random.Next(1, GenBase._worldWidth), GenBase._random.Next(1, GenBase._worldHeight), new object[0]))
					{
						i--;
					}
				}
			}

			// Token: 0x04006791 RID: 26513
			private GenBase.CustomPerUnitAction _perUnit;

			// Token: 0x04006792 RID: 26514
			private int _count;
		}
	}
}
