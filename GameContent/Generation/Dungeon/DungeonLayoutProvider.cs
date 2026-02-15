using System;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon
{
	// Token: 0x020004A1 RID: 1185
	public abstract class DungeonLayoutProvider
	{
		// Token: 0x060033FD RID: 13309 RVA: 0x005FF5D3 File Offset: 0x005FD7D3
		public DungeonLayoutProvider(DungeonLayoutProviderSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x060033FE RID: 13310
		public abstract void ProvideLayout(DungeonData data, GenerationProgress progress, UnifiedRandom genRand, ref int roomDelay);

		// Token: 0x040059A3 RID: 22947
		public DungeonLayoutProviderSettings settings;
	}
}
