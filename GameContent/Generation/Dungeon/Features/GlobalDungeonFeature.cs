using System;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004E8 RID: 1256
	public abstract class GlobalDungeonFeature : IDungeonFeature
	{
		// Token: 0x06003513 RID: 13587 RVA: 0x00612932 File Offset: 0x00610B32
		public GlobalDungeonFeature(DungeonFeatureSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003514 RID: 13588
		public abstract bool GenerateFeature(DungeonData data);

		// Token: 0x04005A6B RID: 23147
		public DungeonFeatureSettings settings;

		// Token: 0x04005A6C RID: 23148
		public bool generated;
	}
}
