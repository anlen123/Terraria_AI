using System;
using System.Collections.Generic;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon
{
	// Token: 0x0200049C RID: 1180
	public class DungeonGenerationStyleData
	{
		// Token: 0x060033B5 RID: 13237 RVA: 0x000379F1 File Offset: 0x00035BF1
		public virtual bool CanGenerateFeatureAt(DungeonData data, DungeonRoom room, IDungeonFeature feature, int x, int y)
		{
			return true;
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x005F9323 File Offset: 0x005F7523
		public virtual void GetBookshelfMinMaxSizes(int defaultMin, int defaultMax, out int min, out int max)
		{
			min = defaultMin;
			max = defaultMax;
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x005F932C File Offset: 0x005F752C
		public bool TileIsInStyle(int tileType, bool includeCracked = true)
		{
			return (this.BrickGrassTileType != null && tileType == (int)this.BrickGrassTileType.Value) || (includeCracked && tileType == (int)this.BrickCrackedTileType) || tileType == (int)this.BrickTileType;
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x005F9362 File Offset: 0x005F7562
		public bool WallIsInStyle(int wallType, bool includeWindows = false)
		{
			return (includeWindows && (wallType == (int)this.WindowGlassWallType || wallType == (int)this.WindowEdgeWallType || wallType == (int)this.WindowClosedGlassWallType)) || wallType == (int)this.BrickWallType;
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x005F9390 File Offset: 0x005F7590
		public int GetPlatformStyle(UnifiedRandom genRand)
		{
			int num = (this.PlatformItemTypes == null || this.PlatformItemTypes.Length == 0) ? -1 : this.PlatformItemTypes[genRand.Next(this.PlatformItemTypes.Length)];
			if (num >= 0)
			{
				return (int)ItemID.Sets.DerivedPlacementDetails[num].tileStyle;
			}
			return -1;
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x005F93E0 File Offset: 0x005F75E0
		public int GetWindowPlatformStyle(UnifiedRandom genRand)
		{
			int num = (this.WindowPlatformItemTypes == null || this.WindowPlatformItemTypes.Length == 0) ? -1 : this.WindowPlatformItemTypes[genRand.Next(this.WindowPlatformItemTypes.Length)];
			if (num >= 0)
			{
				return (int)ItemID.Sets.DerivedPlacementDetails[num].tileStyle;
			}
			return -1;
		}

		// Token: 0x04005948 RID: 22856
		public byte Style;

		// Token: 0x04005949 RID: 22857
		public int UnbreakableWallProgressionTier = -1;

		// Token: 0x0400594A RID: 22858
		public ushort BrickTileType;

		// Token: 0x0400594B RID: 22859
		public ushort? BrickGrassTileType;

		// Token: 0x0400594C RID: 22860
		public ushort BrickCrackedTileType;

		// Token: 0x0400594D RID: 22861
		public ushort BrickWallType;

		// Token: 0x0400594E RID: 22862
		public ushort WindowGlassWallType;

		// Token: 0x0400594F RID: 22863
		public ushort WindowClosedGlassWallType;

		// Token: 0x04005950 RID: 22864
		public ushort WindowEdgeWallType;

		// Token: 0x04005951 RID: 22865
		public int[] WindowPlatformItemTypes;

		// Token: 0x04005952 RID: 22866
		public ushort PitTrapTileType;

		// Token: 0x04005953 RID: 22867
		public int LiquidType = -1;

		// Token: 0x04005954 RID: 22868
		public int LockedBiomeChestType;

		// Token: 0x04005955 RID: 22869
		public int LockedBiomeChestStyle;

		// Token: 0x04005956 RID: 22870
		public int BiomeChestItemType;

		// Token: 0x04005957 RID: 22871
		public int BiomeChestLootItemType;

		// Token: 0x04005958 RID: 22872
		public int[] ChestItemTypes;

		// Token: 0x04005959 RID: 22873
		public int[] DoorItemTypes;

		// Token: 0x0400595A RID: 22874
		public int[] PlatformItemTypes;

		// Token: 0x0400595B RID: 22875
		public int[] ChandelierItemTypes;

		// Token: 0x0400595C RID: 22876
		public int[] LanternItemTypes;

		// Token: 0x0400595D RID: 22877
		public int[] TableItemTypes;

		// Token: 0x0400595E RID: 22878
		public int[] WorkbenchItemTypes;

		// Token: 0x0400595F RID: 22879
		public int[] CandleItemTypes;

		// Token: 0x04005960 RID: 22880
		public int[] VaseOrStatueItemTypes;

		// Token: 0x04005961 RID: 22881
		public int[] BookcaseItemTypes;

		// Token: 0x04005962 RID: 22882
		public int[] ChairItemTypes;

		// Token: 0x04005963 RID: 22883
		public int[] BedItemTypes;

		// Token: 0x04005964 RID: 22884
		public int[] PianoItemTypes;

		// Token: 0x04005965 RID: 22885
		public int[] DresserItemTypes;

		// Token: 0x04005966 RID: 22886
		public int[] SofaItemTypes;

		// Token: 0x04005967 RID: 22887
		public int[] BathtubItemTypes;

		// Token: 0x04005968 RID: 22888
		public int[] LampItemTypes;

		// Token: 0x04005969 RID: 22889
		public int[] CandelabraItemTypes;

		// Token: 0x0400596A RID: 22890
		public int[] ClockItemTypes;

		// Token: 0x0400596B RID: 22891
		public int[] BannerItemTypes;

		// Token: 0x0400596C RID: 22892
		public bool EdgeDither;

		// Token: 0x0400596D RID: 22893
		public DungeonRoomType BiomeRoomType;

		// Token: 0x0400596E RID: 22894
		public List<DungeonGenerationStyleData> SubStyles;
	}
}
