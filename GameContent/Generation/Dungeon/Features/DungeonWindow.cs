using System;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004D0 RID: 1232
	public abstract class DungeonWindow : DungeonFeature
	{
		// Token: 0x060034C8 RID: 13512 RVA: 0x00609E5F File Offset: 0x0060805F
		public DungeonWindow(DungeonFeatureSettings settings) : base(settings)
		{
			DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x00609E78 File Offset: 0x00608078
		public override bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
		{
			return feature is DungeonGlobalWallVariants;
		}
	}
}
