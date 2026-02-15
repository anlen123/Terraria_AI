using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation.Dungeon.Features;

namespace Terraria.GameContent.Generation.Dungeon.Entrances
{
	// Token: 0x020004F0 RID: 1264
	public abstract class DungeonEntrance
	{
		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06003528 RID: 13608 RVA: 0x00614680 File Offset: 0x00612880
		public bool Processed
		{
			get
			{
				return this.calculated || this.generated;
			}
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x00614692 File Offset: 0x00612892
		public DungeonEntrance(DungeonEntranceSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x0600352A RID: 13610
		public abstract void CalculateEntrance(DungeonData data, int x, int y);

		// Token: 0x0600352B RID: 13611
		public abstract bool GenerateEntrance(DungeonData data, int x, int y);

		// Token: 0x0600352C RID: 13612 RVA: 0x006146AC File Offset: 0x006128AC
		public virtual bool CanGenerateFeatureAt(DungeonData data, IDungeonFeature feature, int x, int y)
		{
			return !(feature is DungeonGlobalBiomeChests);
		}

		// Token: 0x04005A82 RID: 23170
		public DungeonEntranceSettings settings;

		// Token: 0x04005A83 RID: 23171
		public bool calculated;

		// Token: 0x04005A84 RID: 23172
		public bool generated;

		// Token: 0x04005A85 RID: 23173
		public DungeonBounds Bounds = new DungeonBounds();

		// Token: 0x04005A86 RID: 23174
		public Point OldManSpawn;
	}
}
