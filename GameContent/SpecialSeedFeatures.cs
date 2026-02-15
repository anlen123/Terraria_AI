using System;

namespace Terraria.GameContent
{
	// Token: 0x02000244 RID: 580
	public class SpecialSeedFeatures
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060022BC RID: 8892 RVA: 0x00539769 File Offset: 0x00537969
		public static bool ShouldDropExtraGel
		{
			get
			{
				return Main.tenthAnniversaryWorld && Main.drunkWorld && !Main.remixWorld && !Main.notTheBeesWorld;
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x060022BD RID: 8893 RVA: 0x00539769 File Offset: 0x00537969
		public static bool ShouldDropExtraWood
		{
			get
			{
				return Main.tenthAnniversaryWorld && Main.drunkWorld && !Main.remixWorld && !Main.notTheBeesWorld;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x060022BE RID: 8894 RVA: 0x0053978A File Offset: 0x0053798A
		public static bool DungeonEntranceHasATree
		{
			get
			{
				return Main.drunkWorld && !SpecialSeedFeatures.NoDungeonGuardian;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x060022BF RID: 8895 RVA: 0x0053979D File Offset: 0x0053799D
		public static bool DungeonEntranceHasStairs
		{
			get
			{
				return !SpecialSeedFeatures.DungeonEntranceIsUnderground && !WorldGen.SecretSeed.roundLandmasses.Enabled;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060022C0 RID: 8896 RVA: 0x005397B5 File Offset: 0x005379B5
		public static bool DungeonEntranceIsBuried
		{
			get
			{
				return WorldGen.SecretSeed.surfaceIsDesert.Enabled && !SpecialSeedFeatures.DungeonEntranceIsUnderground;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060022C1 RID: 8897 RVA: 0x005397CD File Offset: 0x005379CD
		public static bool DungeonEntranceIsUnderground
		{
			get
			{
				return Main.drunkWorld || WorldGen.SecretSeed.noSurface.Enabled;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060022C2 RID: 8898 RVA: 0x005397E2 File Offset: 0x005379E2
		public static bool NoDungeonGuardian
		{
			get
			{
				return Main.onlyShimmerOceanWorlds;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060022C3 RID: 8899 RVA: 0x005397E9 File Offset: 0x005379E9
		public static bool BossesKeepSpawning
		{
			get
			{
				return Main.getGoodWorld && Main.dontStarveWorld && !Main.tenthAnniversaryWorld;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060022C4 RID: 8900 RVA: 0x005397E2 File Offset: 0x005379E2
		public static bool ShimmerSpawnHalfOfWorld
		{
			get
			{
				return Main.onlyShimmerOceanWorlds;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x060022C5 RID: 8901 RVA: 0x005397E2 File Offset: 0x005379E2
		public static bool RainbowSandAndBlackSandWalls
		{
			get
			{
				return Main.onlyShimmerOceanWorlds;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x060022C6 RID: 8902 RVA: 0x00539803 File Offset: 0x00537A03
		public static bool SpawnOnBeach
		{
			get
			{
				return Main.tenthAnniversaryWorld && !Main.remixWorld && !Main.dontStarveWorld;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x060022C7 RID: 8903 RVA: 0x0053981D File Offset: 0x00537A1D
		public static bool SpawnOnBeachOnDungeonSide
		{
			get
			{
				return SpecialSeedFeatures.SpawnOnBeach && Main.onlyShimmerOceanWorlds;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060022C8 RID: 8904 RVA: 0x0053982D File Offset: 0x00537A2D
		public static bool Mechdusa
		{
			get
			{
				return Main.remixWorld && Main.getGoodWorld;
			}
		}
	}
}
