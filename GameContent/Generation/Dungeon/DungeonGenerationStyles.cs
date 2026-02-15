using System;
using System.Collections.Generic;
using Terraria.GameContent.Generation.Dungeon.Features;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon
{
	// Token: 0x0200049D RID: 1181
	public static class DungeonGenerationStyles
	{
		// Token: 0x060033BC RID: 13244 RVA: 0x005F9444 File Offset: 0x005F7644
		public static DungeonGenerationStyleData GetCurrentDungeonStyle()
		{
			return new DungeonGenerationStyleData
			{
				Style = 0,
				UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.Dungeon,
				BrickTileType = GenVars.CurrentDungeonGenVars.brickTileType,
				BrickCrackedTileType = GenVars.CurrentDungeonGenVars.brickCrackedTileType,
				BrickWallType = GenVars.CurrentDungeonGenVars.brickWallType,
				WindowGlassWallType = GenVars.CurrentDungeonGenVars.windowGlassWallType,
				WindowClosedGlassWallType = GenVars.CurrentDungeonGenVars.windowClosedGlassWallType,
				WindowEdgeWallType = GenVars.CurrentDungeonGenVars.windowEdgeWallType,
				WindowPlatformItemTypes = GenVars.CurrentDungeonGenVars.windowPlatformItemTypes,
				PitTrapTileType = GenVars.CurrentDungeonGenVars.brickCrackedTileType,
				LockedBiomeChestType = -1,
				LockedBiomeChestStyle = -1,
				BiomeChestItemType = -1,
				BiomeChestLootItemType = -1,
				ChestItemTypes = new int[0],
				DoorItemTypes = new int[0],
				PlatformItemTypes = new int[0],
				ChandelierItemTypes = new int[0],
				LanternItemTypes = new int[0],
				TableItemTypes = new int[0],
				WorkbenchItemTypes = new int[0],
				CandleItemTypes = new int[0],
				VaseOrStatueItemTypes = new int[0],
				BookcaseItemTypes = new int[0],
				ChairItemTypes = new int[0],
				BedItemTypes = new int[0],
				PianoItemTypes = new int[0],
				DresserItemTypes = new int[0],
				SofaItemTypes = new int[0],
				BathtubItemTypes = new int[0],
				LampItemTypes = new int[0],
				CandelabraItemTypes = new int[0],
				ClockItemTypes = new int[0],
				BannerItemTypes = new int[0],
				EdgeDither = false,
				BiomeRoomType = DungeonRoomType.BiomeStructured
			};
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x005F9604 File Offset: 0x005F7804
		public static DungeonGenerationStyleData GetStyleForTile(List<DungeonGenerationStyleData> styles, int tileType)
		{
			foreach (DungeonGenerationStyleData dungeonGenerationStyleData in styles)
			{
				if (dungeonGenerationStyleData.TileIsInStyle(tileType, true))
				{
					return dungeonGenerationStyleData;
				}
				if (dungeonGenerationStyleData.SubStyles != null && dungeonGenerationStyleData.SubStyles.Count > 0)
				{
					foreach (DungeonGenerationStyleData dungeonGenerationStyleData2 in dungeonGenerationStyleData.SubStyles)
					{
						if (dungeonGenerationStyleData2.TileIsInStyle(tileType, true))
						{
							return dungeonGenerationStyleData2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x005F96C0 File Offset: 0x005F78C0
		public static DungeonGenerationStyleData GetStyleForWall(List<DungeonGenerationStyleData> styles, int wallType)
		{
			foreach (DungeonGenerationStyleData dungeonGenerationStyleData in styles)
			{
				if (dungeonGenerationStyleData.WallIsInStyle(wallType, false))
				{
					return dungeonGenerationStyleData;
				}
				if (dungeonGenerationStyleData.SubStyles != null && dungeonGenerationStyleData.SubStyles.Count > 0)
				{
					foreach (DungeonGenerationStyleData dungeonGenerationStyleData2 in dungeonGenerationStyleData.SubStyles)
					{
						if (dungeonGenerationStyleData2.WallIsInStyle(wallType, false))
						{
							return dungeonGenerationStyleData2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0400596F RID: 22895
		public static DungeonGenerationStyleData Shimmer = new DungeonGenerationStyles.ShimmerStyleData
		{
			Style = 11,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EarlyGame,
			BrickTileType = 667,
			BrickCrackedTileType = 123,
			BrickWallType = 322,
			WindowGlassWallType = 93,
			WindowClosedGlassWallType = 149,
			WindowEdgeWallType = 37,
			WindowPlatformItemTypes = new int[]
			{
				94
			},
			PitTrapTileType = 123,
			LiquidType = 3,
			LockedBiomeChestType = -1,
			LockedBiomeChestStyle = -1,
			BiomeChestItemType = -1,
			BiomeChestLootItemType = -1,
			ChestItemTypes = new int[]
			{
				5556
			},
			DoorItemTypes = new int[]
			{
				5558
			},
			PlatformItemTypes = new int[]
			{
				5562
			},
			ChandelierItemTypes = new int[]
			{
				5555
			},
			LanternItemTypes = new int[]
			{
				5560
			},
			TableItemTypes = new int[]
			{
				5565
			},
			WorkbenchItemTypes = new int[]
			{
				5566
			},
			CandleItemTypes = new int[]
			{
				5553
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				5550
			},
			ChairItemTypes = new int[]
			{
				5554
			},
			BedItemTypes = new int[]
			{
				5549
			},
			PianoItemTypes = new int[]
			{
				5561
			},
			DresserItemTypes = new int[]
			{
				5551
			},
			SofaItemTypes = new int[]
			{
				5564
			},
			BathtubItemTypes = new int[]
			{
				5548
			},
			LampItemTypes = new int[]
			{
				5559
			},
			CandelabraItemTypes = new int[]
			{
				5552
			},
			ClockItemTypes = new int[]
			{
				5557
			},
			BannerItemTypes = new int[]
			{
				337,
				339,
				338,
				340,
				5497,
				5498
			},
			EdgeDither = true,
			BiomeRoomType = DungeonRoomType.BiomeRugged
		};

		// Token: 0x04005970 RID: 22896
		public static DungeonGenerationStyleData Spider = new DungeonGenerationStyleData
		{
			Style = 12,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EarlyGame,
			BrickTileType = 156,
			BrickCrackedTileType = 123,
			BrickWallType = 62,
			WindowGlassWallType = 21,
			WindowClosedGlassWallType = 4,
			WindowEdgeWallType = 36,
			WindowPlatformItemTypes = new int[]
			{
				94
			},
			PitTrapTileType = 123,
			LockedBiomeChestType = -1,
			LockedBiomeChestStyle = -1,
			BiomeChestItemType = -1,
			BiomeChestLootItemType = -1,
			ChestItemTypes = new int[]
			{
				952
			},
			DoorItemTypes = new int[]
			{
				4415
			},
			PlatformItemTypes = new int[]
			{
				4416
			},
			ChandelierItemTypes = new int[]
			{
				106,
				107,
				108,
				710,
				711,
				712
			},
			LanternItemTypes = new int[]
			{
				2037
			},
			TableItemTypes = new int[]
			{
				32
			},
			WorkbenchItemTypes = new int[]
			{
				36
			},
			CandleItemTypes = new int[]
			{
				105,
				713
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				354
			},
			ChairItemTypes = new int[]
			{
				34
			},
			BedItemTypes = new int[]
			{
				224
			},
			PianoItemTypes = new int[]
			{
				333
			},
			DresserItemTypes = new int[]
			{
				334
			},
			SofaItemTypes = new int[]
			{
				2397
			},
			BathtubItemTypes = new int[]
			{
				336
			},
			LampItemTypes = new int[]
			{
				342
			},
			CandelabraItemTypes = new int[]
			{
				349,
				714
			},
			ClockItemTypes = new int[]
			{
				359
			},
			BannerItemTypes = null,
			EdgeDither = true,
			BiomeRoomType = DungeonRoomType.BiomeRugged
		};

		// Token: 0x04005971 RID: 22897
		public static DungeonGenerationStyleData LivingWood = new DungeonGenerationStyles.LivingWoodStyleData
		{
			Style = 13,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EarlyGame,
			BrickTileType = 191,
			BrickCrackedTileType = 192,
			BrickWallType = 244,
			WindowGlassWallType = 21,
			WindowClosedGlassWallType = 4,
			WindowEdgeWallType = 196,
			WindowPlatformItemTypes = new int[]
			{
				2629
			},
			PitTrapTileType = 123,
			LockedBiomeChestType = -1,
			LockedBiomeChestStyle = -1,
			BiomeChestItemType = -1,
			BiomeChestLootItemType = -1,
			ChestItemTypes = new int[]
			{
				831
			},
			DoorItemTypes = new int[]
			{
				819
			},
			PlatformItemTypes = new int[]
			{
				2629
			},
			ChandelierItemTypes = new int[]
			{
				2141
			},
			LanternItemTypes = new int[]
			{
				2145
			},
			TableItemTypes = new int[]
			{
				829
			},
			WorkbenchItemTypes = new int[]
			{
				2633
			},
			CandleItemTypes = new int[]
			{
				2153
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				2135
			},
			ChairItemTypes = new int[]
			{
				806
			},
			BedItemTypes = new int[]
			{
				2139
			},
			PianoItemTypes = new int[]
			{
				2245
			},
			DresserItemTypes = new int[]
			{
				3914
			},
			SofaItemTypes = new int[]
			{
				2636
			},
			BathtubItemTypes = new int[]
			{
				2126
			},
			LampItemTypes = new int[]
			{
				2131
			},
			CandelabraItemTypes = new int[]
			{
				2149
			},
			ClockItemTypes = new int[]
			{
				2596
			},
			BannerItemTypes = null,
			EdgeDither = true,
			BiomeRoomType = DungeonRoomType.BiomeRugged
		};

		// Token: 0x04005972 RID: 22898
		public static DungeonGenerationStyleData Cavern = new DungeonGenerationStyleData
		{
			Style = 1,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EarlyGame,
			BrickTileType = 38,
			BrickCrackedTileType = 123,
			BrickWallType = 349,
			WindowGlassWallType = 21,
			WindowClosedGlassWallType = 4,
			WindowEdgeWallType = 5,
			WindowPlatformItemTypes = new int[]
			{
				94,
				4416
			},
			PitTrapTileType = 123,
			LockedBiomeChestType = -1,
			LockedBiomeChestStyle = -1,
			BiomeChestItemType = -1,
			BiomeChestLootItemType = -1,
			ChestItemTypes = new int[]
			{
				306,
				5886
			},
			DoorItemTypes = new int[]
			{
				25,
				4415
			},
			PlatformItemTypes = new int[]
			{
				94,
				4416
			},
			ChandelierItemTypes = new int[]
			{
				106,
				107,
				108,
				710,
				711,
				712,
				5885
			},
			LanternItemTypes = new int[]
			{
				2037,
				5890
			},
			TableItemTypes = new int[]
			{
				32,
				5894
			},
			WorkbenchItemTypes = new int[]
			{
				36,
				5896
			},
			CandleItemTypes = new int[]
			{
				105,
				713,
				5883
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				354,
				5881
			},
			ChairItemTypes = new int[]
			{
				34,
				5884
			},
			BedItemTypes = new int[]
			{
				224,
				5880
			},
			PianoItemTypes = new int[]
			{
				333,
				5891
			},
			DresserItemTypes = new int[]
			{
				334,
				5888
			},
			SofaItemTypes = new int[]
			{
				2397,
				5893
			},
			BathtubItemTypes = new int[]
			{
				336,
				5879
			},
			LampItemTypes = new int[]
			{
				342,
				5889
			},
			CandelabraItemTypes = new int[]
			{
				349,
				714,
				5882
			},
			ClockItemTypes = new int[]
			{
				359,
				5887
			},
			BannerItemTypes = new int[]
			{
				337,
				339,
				338,
				340,
				5497,
				5498
			},
			EdgeDither = true,
			BiomeRoomType = DungeonRoomType.BiomeStructured,
			SubStyles = new List<DungeonGenerationStyleData>
			{
				DungeonGenerationStyles.Shimmer,
				DungeonGenerationStyles.Spider,
				DungeonGenerationStyles.LivingWood
			}
		};

		// Token: 0x04005973 RID: 22899
		public static DungeonGenerationStyleData Snow = new DungeonGenerationStyleData
		{
			Style = 2,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EarlyGame,
			BrickTileType = 161,
			BrickCrackedTileType = 224,
			BrickWallType = 71,
			WindowGlassWallType = 90,
			WindowClosedGlassWallType = 149,
			WindowEdgeWallType = 31,
			WindowPlatformItemTypes = new int[]
			{
				3908
			},
			PitTrapTileType = 224,
			LockedBiomeChestType = 21,
			LockedBiomeChestStyle = 27,
			BiomeChestItemType = 1532,
			BiomeChestLootItemType = 1572,
			ChestItemTypes = new int[]
			{
				681,
				5805
			},
			DoorItemTypes = new int[]
			{
				2044,
				5807
			},
			PlatformItemTypes = new int[]
			{
				3908,
				5812
			},
			ChandelierItemTypes = new int[]
			{
				2059,
				5804
			},
			LanternItemTypes = new int[]
			{
				2040,
				5810
			},
			TableItemTypes = new int[]
			{
				2248,
				5815
			},
			WorkbenchItemTypes = new int[]
			{
				2252,
				5817
			},
			CandleItemTypes = new int[]
			{
				2049,
				5802
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				2031,
				5800
			},
			ChairItemTypes = new int[]
			{
				2288,
				5803
			},
			BedItemTypes = new int[]
			{
				2068,
				5799
			},
			PianoItemTypes = new int[]
			{
				2247,
				5811
			},
			DresserItemTypes = new int[]
			{
				3913,
				5808
			},
			SofaItemTypes = new int[]
			{
				2635,
				5814
			},
			BathtubItemTypes = new int[]
			{
				2076,
				5798
			},
			LampItemTypes = new int[]
			{
				2086,
				5809
			},
			CandelabraItemTypes = new int[]
			{
				2100,
				5801
			},
			ClockItemTypes = new int[]
			{
				2594,
				5806
			},
			BannerItemTypes = null,
			EdgeDither = true,
			BiomeRoomType = DungeonRoomType.BiomeRugged
		};

		// Token: 0x04005974 RID: 22900
		public static DungeonGenerationStyleData Desert = new DungeonGenerationStyleData
		{
			Style = 3,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EarlyGame,
			BrickTileType = 396,
			BrickCrackedTileType = 53,
			BrickWallType = 187,
			WindowGlassWallType = 89,
			WindowClosedGlassWallType = 151,
			WindowEdgeWallType = 34,
			WindowPlatformItemTypes = new int[]
			{
				4311
			},
			PitTrapTileType = 53,
			LockedBiomeChestType = 467,
			LockedBiomeChestStyle = 13,
			BiomeChestItemType = 4712,
			BiomeChestLootItemType = 4607,
			ChestItemTypes = new int[]
			{
				4267
			},
			DoorItemTypes = new int[]
			{
				4307
			},
			PlatformItemTypes = new int[]
			{
				4311
			},
			ChandelierItemTypes = new int[]
			{
				4305
			},
			LanternItemTypes = new int[]
			{
				4309
			},
			TableItemTypes = new int[]
			{
				4314
			},
			WorkbenchItemTypes = new int[]
			{
				4315
			},
			CandleItemTypes = new int[]
			{
				4303
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				4300
			},
			ChairItemTypes = new int[]
			{
				4304
			},
			BedItemTypes = new int[]
			{
				4299
			},
			PianoItemTypes = new int[]
			{
				4310
			},
			DresserItemTypes = new int[]
			{
				4301
			},
			SofaItemTypes = new int[]
			{
				4313
			},
			BathtubItemTypes = new int[]
			{
				4298
			},
			LampItemTypes = new int[]
			{
				4308
			},
			CandelabraItemTypes = new int[]
			{
				4302
			},
			ClockItemTypes = new int[]
			{
				4306
			},
			BannerItemTypes = new int[]
			{
				790,
				791,
				789
			},
			EdgeDither = false,
			BiomeRoomType = DungeonRoomType.BiomeRugged
		};

		// Token: 0x04005975 RID: 22901
		public static DungeonGenerationStyleData Corruption = new DungeonGenerationStyleData
		{
			Style = 4,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EvilBoss,
			BrickTileType = 25,
			BrickCrackedTileType = 112,
			BrickWallType = 3,
			WindowGlassWallType = 88,
			WindowClosedGlassWallType = 41,
			WindowEdgeWallType = 33,
			WindowPlatformItemTypes = new int[]
			{
				631
			},
			PitTrapTileType = 112,
			LockedBiomeChestType = 21,
			LockedBiomeChestStyle = 24,
			BiomeChestItemType = 1529,
			BiomeChestLootItemType = 1571,
			ChestItemTypes = new int[]
			{
				625,
				3965,
				5763
			},
			DoorItemTypes = new int[]
			{
				650,
				3967,
				5765
			},
			PlatformItemTypes = new int[]
			{
				631,
				3957,
				5770
			},
			ChandelierItemTypes = new int[]
			{
				2056,
				3964,
				5762
			},
			LanternItemTypes = new int[]
			{
				2033,
				3970,
				5768
			},
			TableItemTypes = new int[]
			{
				638,
				3974,
				5773
			},
			WorkbenchItemTypes = new int[]
			{
				635,
				3975,
				5775
			},
			CandleItemTypes = new int[]
			{
				2046,
				3962,
				5760
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				2021,
				3960,
				5758
			},
			ChairItemTypes = new int[]
			{
				628,
				3963,
				5761
			},
			BedItemTypes = new int[]
			{
				644,
				3959,
				5757
			},
			PianoItemTypes = new int[]
			{
				641,
				3971,
				5769
			},
			DresserItemTypes = new int[]
			{
				647,
				3968,
				5766
			},
			SofaItemTypes = new int[]
			{
				2398,
				3973,
				5772
			},
			BathtubItemTypes = new int[]
			{
				2073,
				3958,
				5756
			},
			LampItemTypes = new int[]
			{
				2083,
				3969,
				5767
			},
			CandelabraItemTypes = new int[]
			{
				2093,
				3961,
				5759
			},
			ClockItemTypes = new int[]
			{
				2593,
				3966,
				5764
			},
			BannerItemTypes = null,
			EdgeDither = true,
			BiomeRoomType = DungeonRoomType.BiomeRugged
		};

		// Token: 0x04005976 RID: 22902
		public static DungeonGenerationStyleData Crimson = new DungeonGenerationStyleData
		{
			Style = 5,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.EvilBoss,
			BrickTileType = 203,
			BrickCrackedTileType = 234,
			BrickWallType = 83,
			WindowGlassWallType = 92,
			WindowClosedGlassWallType = 85,
			WindowEdgeWallType = 174,
			WindowPlatformItemTypes = new int[]
			{
				913
			},
			PitTrapTileType = 234,
			LockedBiomeChestType = 21,
			LockedBiomeChestStyle = 25,
			BiomeChestItemType = 1530,
			BiomeChestLootItemType = 1569,
			ChestItemTypes = new int[]
			{
				914,
				2617,
				5784
			},
			DoorItemTypes = new int[]
			{
				912,
				817,
				5786
			},
			PlatformItemTypes = new int[]
			{
				913,
				3907,
				5791
			},
			ChandelierItemTypes = new int[]
			{
				2142,
				2057,
				5783
			},
			LanternItemTypes = new int[]
			{
				2146,
				2034,
				5789
			},
			TableItemTypes = new int[]
			{
				917,
				828,
				5794
			},
			WorkbenchItemTypes = new int[]
			{
				916,
				813,
				5796
			},
			CandleItemTypes = new int[]
			{
				2154,
				2047,
				5781
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				2136,
				2022,
				5779
			},
			ChairItemTypes = new int[]
			{
				915,
				809,
				5782
			},
			BedItemTypes = new int[]
			{
				920,
				2067,
				5778
			},
			PianoItemTypes = new int[]
			{
				919,
				2246,
				5790
			},
			DresserItemTypes = new int[]
			{
				918,
				2640,
				5787
			},
			SofaItemTypes = new int[]
			{
				2401,
				2634,
				5793
			},
			BathtubItemTypes = new int[]
			{
				2127,
				2074,
				5777
			},
			LampItemTypes = new int[]
			{
				2132,
				2084,
				5788
			},
			CandelabraItemTypes = new int[]
			{
				2150,
				2094,
				5780
			},
			ClockItemTypes = new int[]
			{
				2604,
				2598,
				5785
			},
			BannerItemTypes = null,
			EdgeDither = true,
			BiomeRoomType = DungeonRoomType.BiomeRugged
		};

		// Token: 0x04005977 RID: 22903
		public static DungeonGenerationStyleData Crystal = new DungeonGenerationStyles.ShimmerStyleData
		{
			Style = 15,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.Hallow,
			BrickTileType = 385,
			BrickCrackedTileType = 116,
			BrickWallType = 186,
			WindowGlassWallType = 88,
			WindowClosedGlassWallType = 43,
			WindowEdgeWallType = 22,
			WindowPlatformItemTypes = new int[]
			{
				633
			},
			PitTrapTileType = 116,
			LockedBiomeChestType = -1,
			LockedBiomeChestStyle = -1,
			BiomeChestItemType = -1,
			BiomeChestLootItemType = -1,
			ChestItemTypes = new int[]
			{
				3884
			},
			DoorItemTypes = new int[]
			{
				3888
			},
			PlatformItemTypes = new int[]
			{
				3903
			},
			ChandelierItemTypes = new int[]
			{
				3894
			},
			LanternItemTypes = new int[]
			{
				3891
			},
			TableItemTypes = new int[]
			{
				3920
			},
			WorkbenchItemTypes = new int[]
			{
				3909
			},
			CandleItemTypes = new int[]
			{
				3890
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				3917
			},
			ChairItemTypes = new int[]
			{
				3889
			},
			BedItemTypes = new int[]
			{
				3897
			},
			PianoItemTypes = new int[]
			{
				3915
			},
			DresserItemTypes = new int[]
			{
				3911
			},
			SofaItemTypes = new int[]
			{
				3918
			},
			BathtubItemTypes = new int[]
			{
				3895
			},
			LampItemTypes = new int[]
			{
				3892
			},
			CandelabraItemTypes = new int[]
			{
				3893
			},
			ClockItemTypes = new int[]
			{
				3898
			},
			BannerItemTypes = null,
			EdgeDither = false,
			BiomeRoomType = DungeonRoomType.BiomeStructured
		};

		// Token: 0x04005978 RID: 22904
		public static DungeonGenerationStyleData Hallow = new DungeonGenerationStyleData
		{
			Style = 6,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.Hallow,
			BrickTileType = 117,
			BrickCrackedTileType = 116,
			BrickWallType = 28,
			WindowGlassWallType = 91,
			WindowClosedGlassWallType = 43,
			WindowEdgeWallType = 22,
			WindowPlatformItemTypes = new int[]
			{
				633
			},
			PitTrapTileType = 116,
			LockedBiomeChestType = 21,
			LockedBiomeChestStyle = 26,
			BiomeChestItemType = 1531,
			BiomeChestLootItemType = 1260,
			ChestItemTypes = new int[]
			{
				627,
				3884
			},
			DoorItemTypes = new int[]
			{
				652,
				3888
			},
			PlatformItemTypes = new int[]
			{
				633,
				3903
			},
			ChandelierItemTypes = new int[]
			{
				2061,
				3894
			},
			LanternItemTypes = new int[]
			{
				2039,
				3891
			},
			TableItemTypes = new int[]
			{
				640,
				3920
			},
			WorkbenchItemTypes = new int[]
			{
				637,
				3909
			},
			CandleItemTypes = new int[]
			{
				2051,
				3890
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				2027,
				3917
			},
			ChairItemTypes = new int[]
			{
				630,
				3889
			},
			BedItemTypes = new int[]
			{
				646,
				3897
			},
			PianoItemTypes = new int[]
			{
				643,
				3915
			},
			DresserItemTypes = new int[]
			{
				649,
				3911
			},
			SofaItemTypes = new int[]
			{
				2400,
				3918
			},
			BathtubItemTypes = new int[]
			{
				2078,
				3895
			},
			LampItemTypes = new int[]
			{
				2088,
				3892
			},
			CandelabraItemTypes = new int[]
			{
				2099,
				3893
			},
			ClockItemTypes = new int[]
			{
				2602,
				3898
			},
			BannerItemTypes = null,
			EdgeDither = true,
			BiomeRoomType = DungeonRoomType.BiomeRugged,
			SubStyles = new List<DungeonGenerationStyleData>
			{
				DungeonGenerationStyles.Crystal
			}
		};

		// Token: 0x04005979 RID: 22905
		public static DungeonGenerationStyleData GlowingMushroom = new DungeonGenerationStyleData
		{
			Style = 7,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.JungleBoss,
			BrickTileType = 59,
			BrickGrassTileType = new ushort?(70),
			BrickCrackedTileType = 123,
			BrickWallType = 80,
			WindowGlassWallType = 90,
			WindowClosedGlassWallType = 60,
			WindowEdgeWallType = 78,
			WindowPlatformItemTypes = new int[]
			{
				2549
			},
			PitTrapTileType = 123,
			LockedBiomeChestType = -1,
			LockedBiomeChestStyle = -1,
			BiomeChestItemType = -1,
			BiomeChestLootItemType = -1,
			ChestItemTypes = new int[]
			{
				2544
			},
			DoorItemTypes = new int[]
			{
				818
			},
			PlatformItemTypes = new int[]
			{
				2549
			},
			ChandelierItemTypes = new int[]
			{
				2543
			},
			LanternItemTypes = new int[]
			{
				2546
			},
			TableItemTypes = new int[]
			{
				2550
			},
			WorkbenchItemTypes = new int[]
			{
				814
			},
			CandleItemTypes = new int[]
			{
				2542
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				2540
			},
			ChairItemTypes = new int[]
			{
				810
			},
			BedItemTypes = new int[]
			{
				2538
			},
			PianoItemTypes = new int[]
			{
				2548
			},
			DresserItemTypes = new int[]
			{
				2545
			},
			SofaItemTypes = new int[]
			{
				2413
			},
			BathtubItemTypes = new int[]
			{
				2537
			},
			LampItemTypes = new int[]
			{
				2547
			},
			CandelabraItemTypes = new int[]
			{
				2541
			},
			ClockItemTypes = new int[]
			{
				2599
			},
			BannerItemTypes = null,
			EdgeDither = true,
			BiomeRoomType = DungeonRoomType.BiomeRugged
		};

		// Token: 0x0400597A RID: 22906
		public static DungeonGenerationStyleData Beehive = new DungeonGenerationStyles.BeehiveStyleData
		{
			Style = 9,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.JungleBoss,
			BrickTileType = 225,
			BrickCrackedTileType = 123,
			BrickWallType = 86,
			WindowGlassWallType = 89,
			WindowClosedGlassWallType = 172,
			WindowEdgeWallType = 151,
			WindowPlatformItemTypes = new int[]
			{
				2630
			},
			PitTrapTileType = 123,
			LiquidType = 2,
			LockedBiomeChestType = -1,
			LockedBiomeChestStyle = -1,
			BiomeChestItemType = -1,
			BiomeChestLootItemType = -1,
			ChestItemTypes = new int[]
			{
				2249
			},
			DoorItemTypes = new int[]
			{
				1711
			},
			PlatformItemTypes = new int[]
			{
				2630
			},
			ChandelierItemTypes = new int[]
			{
				2058
			},
			LanternItemTypes = new int[]
			{
				2035
			},
			TableItemTypes = new int[]
			{
				1717
			},
			WorkbenchItemTypes = new int[]
			{
				2251
			},
			CandleItemTypes = new int[]
			{
				2648
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				2023
			},
			ChairItemTypes = new int[]
			{
				1707
			},
			BedItemTypes = new int[]
			{
				1721
			},
			PianoItemTypes = new int[]
			{
				2255
			},
			DresserItemTypes = new int[]
			{
				2395
			},
			SofaItemTypes = new int[]
			{
				2411
			},
			BathtubItemTypes = new int[]
			{
				2124
			},
			LampItemTypes = new int[]
			{
				2129
			},
			CandelabraItemTypes = new int[]
			{
				2095
			},
			ClockItemTypes = new int[]
			{
				2240
			},
			BannerItemTypes = null,
			EdgeDither = true,
			BiomeRoomType = DungeonRoomType.BiomeRugged
		};

		// Token: 0x0400597B RID: 22907
		public static DungeonGenerationStyleData LivingMahogany = new DungeonGenerationStyles.LivingWoodStyleData
		{
			Style = 14,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.JungleBoss,
			BrickTileType = 383,
			BrickCrackedTileType = 384,
			BrickWallType = 244,
			WindowGlassWallType = 21,
			WindowClosedGlassWallType = 42,
			WindowEdgeWallType = 196,
			WindowPlatformItemTypes = new int[]
			{
				2629
			},
			PitTrapTileType = 123,
			LockedBiomeChestType = -1,
			LockedBiomeChestStyle = -1,
			BiomeChestItemType = -1,
			BiomeChestLootItemType = -1,
			ChestItemTypes = new int[]
			{
				831
			},
			DoorItemTypes = new int[]
			{
				819
			},
			PlatformItemTypes = new int[]
			{
				2629
			},
			ChandelierItemTypes = new int[]
			{
				2141
			},
			LanternItemTypes = new int[]
			{
				2145
			},
			TableItemTypes = new int[]
			{
				829
			},
			WorkbenchItemTypes = new int[]
			{
				2633
			},
			CandleItemTypes = new int[]
			{
				2153
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				2135
			},
			ChairItemTypes = new int[]
			{
				806
			},
			BedItemTypes = new int[]
			{
				2139
			},
			PianoItemTypes = new int[]
			{
				2245
			},
			DresserItemTypes = new int[]
			{
				3914
			},
			SofaItemTypes = new int[]
			{
				2636
			},
			BathtubItemTypes = new int[]
			{
				2126
			},
			LampItemTypes = new int[]
			{
				2131
			},
			CandelabraItemTypes = new int[]
			{
				2149
			},
			ClockItemTypes = new int[]
			{
				2596
			},
			BannerItemTypes = null,
			EdgeDither = true,
			BiomeRoomType = DungeonRoomType.BiomeRugged
		};

		// Token: 0x0400597C RID: 22908
		public static DungeonGenerationStyleData Jungle = new DungeonGenerationStyleData
		{
			Style = 8,
			UnbreakableWallProgressionTier = DualDungeonUnbreakableWallTiers.JungleBoss,
			BrickTileType = 59,
			BrickGrassTileType = new ushort?(60),
			BrickCrackedTileType = 123,
			BrickWallType = 64,
			WindowGlassWallType = 91,
			WindowClosedGlassWallType = 42,
			WindowEdgeWallType = 24,
			WindowPlatformItemTypes = new int[]
			{
				632
			},
			PitTrapTileType = 123,
			LockedBiomeChestType = 21,
			LockedBiomeChestStyle = 23,
			BiomeChestItemType = 1528,
			BiomeChestLootItemType = 1156,
			ChestItemTypes = new int[]
			{
				626,
				680
			},
			DoorItemTypes = new int[]
			{
				651
			},
			PlatformItemTypes = new int[]
			{
				632
			},
			ChandelierItemTypes = new int[]
			{
				2060
			},
			LanternItemTypes = new int[]
			{
				2038,
				4578
			},
			TableItemTypes = new int[]
			{
				639
			},
			WorkbenchItemTypes = new int[]
			{
				636
			},
			CandleItemTypes = new int[]
			{
				2050
			},
			VaseOrStatueItemTypes = null,
			BookcaseItemTypes = new int[]
			{
				2026
			},
			ChairItemTypes = new int[]
			{
				629
			},
			BedItemTypes = new int[]
			{
				645
			},
			PianoItemTypes = new int[]
			{
				642
			},
			DresserItemTypes = new int[]
			{
				648
			},
			SofaItemTypes = new int[]
			{
				2399
			},
			BathtubItemTypes = new int[]
			{
				2077
			},
			LampItemTypes = new int[]
			{
				2087
			},
			CandelabraItemTypes = new int[]
			{
				2098
			},
			ClockItemTypes = new int[]
			{
				2597
			},
			BannerItemTypes = null,
			EdgeDither = true,
			BiomeRoomType = DungeonRoomType.BiomeRugged,
			SubStyles = new List<DungeonGenerationStyleData>
			{
				DungeonGenerationStyles.Beehive,
				DungeonGenerationStyles.LivingMahogany
			}
		};

		// Token: 0x0400597D RID: 22909
		public static DungeonGenerationStyleData Temple = new DungeonGenerationStyles.TempleStyleData
		{
			Style = 10,
			BrickTileType = 226,
			BrickCrackedTileType = 123,
			BrickWallType = 87,
			WindowGlassWallType = 92,
			WindowClosedGlassWallType = 42,
			WindowEdgeWallType = 24,
			WindowPlatformItemTypes = new int[]
			{
				3906
			},
			PitTrapTileType = 123,
			LockedBiomeChestType = -1,
			LockedBiomeChestStyle = -1,
			BiomeChestItemType = -1,
			BiomeChestLootItemType = -1,
			ChestItemTypes = new int[]
			{
				1142
			},
			DoorItemTypes = new int[]
			{
				1137
			},
			PlatformItemTypes = new int[]
			{
				3906
			},
			ChandelierItemTypes = new int[]
			{
				2062
			},
			LanternItemTypes = new int[]
			{
				2041
			},
			TableItemTypes = new int[]
			{
				1144
			},
			WorkbenchItemTypes = new int[]
			{
				1145
			},
			CandleItemTypes = new int[]
			{
				2052
			},
			VaseOrStatueItemTypes = new int[]
			{
				1152,
				1153,
				1154
			},
			BookcaseItemTypes = new int[]
			{
				2030
			},
			ChairItemTypes = new int[]
			{
				1143
			},
			BedItemTypes = new int[]
			{
				2069
			},
			PianoItemTypes = new int[]
			{
				2385
			},
			DresserItemTypes = new int[]
			{
				2396
			},
			SofaItemTypes = new int[]
			{
				2416
			},
			BathtubItemTypes = new int[]
			{
				2079
			},
			LampItemTypes = new int[]
			{
				2089
			},
			CandelabraItemTypes = new int[]
			{
				2101
			},
			ClockItemTypes = new int[]
			{
				2595
			},
			BannerItemTypes = null,
			EdgeDither = false,
			BiomeRoomType = DungeonRoomType.BiomeStructured
		};

		// Token: 0x0200097D RID: 2429
		private class ShimmerStyleData : DungeonGenerationStyleData
		{
			// Token: 0x06004932 RID: 18738 RVA: 0x006CF645 File Offset: 0x006CD845
			public override bool CanGenerateFeatureAt(DungeonData data, DungeonRoom room, IDungeonFeature feature, int x, int y)
			{
				return !(feature is DungeonPitTrap) && !(feature is DungeonWindow);
			}
		}

		// Token: 0x0200097E RID: 2430
		private class LivingWoodStyleData : DungeonGenerationStyleData
		{
			// Token: 0x06004934 RID: 18740 RVA: 0x006CF665 File Offset: 0x006CD865
			public override bool CanGenerateFeatureAt(DungeonData data, DungeonRoom room, IDungeonFeature feature, int x, int y)
			{
				return !(feature is DungeonGlobalSpikes) && !(feature is DungeonPitTrap);
			}

			// Token: 0x06004935 RID: 18741 RVA: 0x006CF67D File Offset: 0x006CD87D
			public override void GetBookshelfMinMaxSizes(int defaultMin, int defaultMax, out int min, out int max)
			{
				min = 3;
				max = 7;
			}
		}

		// Token: 0x0200097F RID: 2431
		private class BeehiveStyleData : DungeonGenerationStyleData
		{
			// Token: 0x06004937 RID: 18743 RVA: 0x006CF686 File Offset: 0x006CD886
			public override bool CanGenerateFeatureAt(DungeonData data, DungeonRoom room, IDungeonFeature feature, int x, int y)
			{
				return !(feature is DungeonGlobalPaintings) && !(feature is DungeonGlobalSpikes) && !(feature is DungeonPitTrap) && !(feature is DungeonWindow);
			}
		}

		// Token: 0x02000980 RID: 2432
		private class TempleStyleData : DungeonGenerationStyleData
		{
			// Token: 0x06004939 RID: 18745 RVA: 0x006CF6AE File Offset: 0x006CD8AE
			public override bool CanGenerateFeatureAt(DungeonData data, DungeonRoom room, IDungeonFeature feature, int x, int y)
			{
				return !(feature is DungeonPitTrap) && !(feature is DungeonPillar);
			}
		}
	}
}
