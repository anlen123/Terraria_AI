using System;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004E7 RID: 1255
	public abstract class DungeonFeature : IDungeonFeature
	{
		// Token: 0x06003510 RID: 13584 RVA: 0x00612918 File Offset: 0x00610B18
		public DungeonFeature(DungeonFeatureSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003511 RID: 13585
		public abstract bool GenerateFeature(DungeonData data, int x, int y);

		// Token: 0x06003512 RID: 13586 RVA: 0x000379F1 File Offset: 0x00035BF1
		public virtual bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
		{
			return true;
		}

		// Token: 0x04005A68 RID: 23144
		public DungeonFeatureSettings settings;

		// Token: 0x04005A69 RID: 23145
		public DungeonBounds Bounds = new DungeonBounds();

		// Token: 0x04005A6A RID: 23146
		public bool generated;
	}
}
