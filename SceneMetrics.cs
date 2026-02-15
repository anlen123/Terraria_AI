using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria
{
	// Token: 0x0200001E RID: 30
	public class SceneMetrics
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x0000EC6A File Offset: 0x0000CE6A
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x0000EC72 File Offset: 0x0000CE72
		public uint LastScanTime { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000EC7B File Offset: 0x0000CE7B
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x0000EC83 File Offset: 0x0000CE83
		public Vector2 Center { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x0000EC8C File Offset: 0x0000CE8C
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x0000EC94 File Offset: 0x0000CE94
		public Point TileCenter { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x0000EC9D File Offset: 0x0000CE9D
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x0000ECA5 File Offset: 0x0000CEA5
		public Point BestOrePosition { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000ECAE File Offset: 0x0000CEAE
		public static int SnowTileThreshold
		{
			get
			{
				if (WorldGen.Skyblock.lowTiles)
				{
					return SceneMetrics.SnowTileSkyblockThreshold;
				}
				return SceneMetrics.SnowTileNormalThreshold;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000FB RID: 251 RVA: 0x0000ECC2 File Offset: 0x0000CEC2
		public static int DesertTileThreshold
		{
			get
			{
				if (WorldGen.Skyblock.lowTiles)
				{
					return SceneMetrics.DesertTileSkyblockThreshold;
				}
				return SceneMetrics.DesertTileNormalThreshold;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000ECD6 File Offset: 0x0000CED6
		// (set) Token: 0x060000FD RID: 253 RVA: 0x0000ECDE File Offset: 0x0000CEDE
		public int ShimmerTileCount { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000ECE7 File Offset: 0x0000CEE7
		// (set) Token: 0x060000FF RID: 255 RVA: 0x0000ECEF File Offset: 0x0000CEEF
		public int EvilTileCount { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000100 RID: 256 RVA: 0x0000ECF8 File Offset: 0x0000CEF8
		// (set) Token: 0x06000101 RID: 257 RVA: 0x0000ED00 File Offset: 0x0000CF00
		public int HolyTileCount { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000ED09 File Offset: 0x0000CF09
		// (set) Token: 0x06000103 RID: 259 RVA: 0x0000ED11 File Offset: 0x0000CF11
		public int HoneyBlockCount { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000104 RID: 260 RVA: 0x0000ED1A File Offset: 0x0000CF1A
		// (set) Token: 0x06000105 RID: 261 RVA: 0x0000ED22 File Offset: 0x0000CF22
		public int ActiveMusicBox { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000ED2B File Offset: 0x0000CF2B
		// (set) Token: 0x06000107 RID: 263 RVA: 0x0000ED33 File Offset: 0x0000CF33
		public int SandTileCount { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000108 RID: 264 RVA: 0x0000ED3C File Offset: 0x0000CF3C
		// (set) Token: 0x06000109 RID: 265 RVA: 0x0000ED44 File Offset: 0x0000CF44
		public int MushroomTileCount { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600010A RID: 266 RVA: 0x0000ED4D File Offset: 0x0000CF4D
		// (set) Token: 0x0600010B RID: 267 RVA: 0x0000ED55 File Offset: 0x0000CF55
		public int SnowTileCount { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600010C RID: 268 RVA: 0x0000ED5E File Offset: 0x0000CF5E
		// (set) Token: 0x0600010D RID: 269 RVA: 0x0000ED66 File Offset: 0x0000CF66
		public int WaterCandleCount { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600010E RID: 270 RVA: 0x0000ED6F File Offset: 0x0000CF6F
		// (set) Token: 0x0600010F RID: 271 RVA: 0x0000ED77 File Offset: 0x0000CF77
		public int PeaceCandleCount { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000110 RID: 272 RVA: 0x0000ED80 File Offset: 0x0000CF80
		// (set) Token: 0x06000111 RID: 273 RVA: 0x0000ED88 File Offset: 0x0000CF88
		public int ShadowCandleCount { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000112 RID: 274 RVA: 0x0000ED91 File Offset: 0x0000CF91
		// (set) Token: 0x06000113 RID: 275 RVA: 0x0000ED99 File Offset: 0x0000CF99
		public int PartyMonolithCount { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000114 RID: 276 RVA: 0x0000EDA2 File Offset: 0x0000CFA2
		// (set) Token: 0x06000115 RID: 277 RVA: 0x0000EDAA File Offset: 0x0000CFAA
		public int MeteorTileCount { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000116 RID: 278 RVA: 0x0000EDB3 File Offset: 0x0000CFB3
		// (set) Token: 0x06000117 RID: 279 RVA: 0x0000EDBB File Offset: 0x0000CFBB
		public int BloodTileCount { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000118 RID: 280 RVA: 0x0000EDC4 File Offset: 0x0000CFC4
		// (set) Token: 0x06000119 RID: 281 RVA: 0x0000EDCC File Offset: 0x0000CFCC
		public int JungleTileCount { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600011A RID: 282 RVA: 0x0000EDD5 File Offset: 0x0000CFD5
		// (set) Token: 0x0600011B RID: 283 RVA: 0x0000EDDD File Offset: 0x0000CFDD
		public int DungeonTileCount { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600011C RID: 284 RVA: 0x0000EDE6 File Offset: 0x0000CFE6
		// (set) Token: 0x0600011D RID: 285 RVA: 0x0000EDEE File Offset: 0x0000CFEE
		public bool HasSunflower { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600011E RID: 286 RVA: 0x0000EDF7 File Offset: 0x0000CFF7
		// (set) Token: 0x0600011F RID: 287 RVA: 0x0000EDFF File Offset: 0x0000CFFF
		public bool HasGardenGnome { get; private set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0000EE08 File Offset: 0x0000D008
		// (set) Token: 0x06000121 RID: 289 RVA: 0x0000EE10 File Offset: 0x0000D010
		public bool HasClock { get; private set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000EE19 File Offset: 0x0000D019
		// (set) Token: 0x06000123 RID: 291 RVA: 0x0000EE21 File Offset: 0x0000D021
		public bool HasCampfire { get; private set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000124 RID: 292 RVA: 0x0000EE2A File Offset: 0x0000D02A
		// (set) Token: 0x06000125 RID: 293 RVA: 0x0000EE32 File Offset: 0x0000D032
		public bool HasStarInBottle { get; private set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000126 RID: 294 RVA: 0x0000EE3B File Offset: 0x0000D03B
		// (set) Token: 0x06000127 RID: 295 RVA: 0x0000EE43 File Offset: 0x0000D043
		public bool HasHeartLantern { get; private set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000128 RID: 296 RVA: 0x0000EE4C File Offset: 0x0000D04C
		// (set) Token: 0x06000129 RID: 297 RVA: 0x0000EE54 File Offset: 0x0000D054
		public int ActiveFountainColor { get; private set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600012A RID: 298 RVA: 0x0000EE5D File Offset: 0x0000D05D
		// (set) Token: 0x0600012B RID: 299 RVA: 0x0000EE65 File Offset: 0x0000D065
		public int ActiveMonolithType { get; private set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600012C RID: 300 RVA: 0x0000EE6E File Offset: 0x0000D06E
		// (set) Token: 0x0600012D RID: 301 RVA: 0x0000EE76 File Offset: 0x0000D076
		public bool BloodMoonMonolith { get; private set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600012E RID: 302 RVA: 0x0000EE7F File Offset: 0x0000D07F
		// (set) Token: 0x0600012F RID: 303 RVA: 0x0000EE87 File Offset: 0x0000D087
		public bool MoonLordMonolith { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000130 RID: 304 RVA: 0x0000EE90 File Offset: 0x0000D090
		// (set) Token: 0x06000131 RID: 305 RVA: 0x0000EE98 File Offset: 0x0000D098
		public bool EchoMonolith { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000132 RID: 306 RVA: 0x0000EEA1 File Offset: 0x0000D0A1
		// (set) Token: 0x06000133 RID: 307 RVA: 0x0000EEA9 File Offset: 0x0000D0A9
		public int ShimmerMonolithState { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000134 RID: 308 RVA: 0x0000EEB2 File Offset: 0x0000D0B2
		// (set) Token: 0x06000135 RID: 309 RVA: 0x0000EEBA File Offset: 0x0000D0BA
		public bool CRTMonolith { get; private set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000136 RID: 310 RVA: 0x0000EEC3 File Offset: 0x0000D0C3
		// (set) Token: 0x06000137 RID: 311 RVA: 0x0000EECB File Offset: 0x0000D0CB
		public bool RetroMonolith { get; private set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000EED4 File Offset: 0x0000D0D4
		// (set) Token: 0x06000139 RID: 313 RVA: 0x0000EEDC File Offset: 0x0000D0DC
		public bool NoirMonolith { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000EEE5 File Offset: 0x0000D0E5
		// (set) Token: 0x0600013B RID: 315 RVA: 0x0000EEED File Offset: 0x0000D0ED
		public bool RadioThingMonolith { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000EEF6 File Offset: 0x0000D0F6
		// (set) Token: 0x0600013D RID: 317 RVA: 0x0000EEFE File Offset: 0x0000D0FE
		public bool HasCatBast { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000EF07 File Offset: 0x0000D107
		// (set) Token: 0x0600013F RID: 319 RVA: 0x0000EF0F File Offset: 0x0000D10F
		public int GraveyardTileCount { get; private set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000140 RID: 320 RVA: 0x0000EF18 File Offset: 0x0000D118
		// (set) Token: 0x06000141 RID: 321 RVA: 0x0000EF20 File Offset: 0x0000D120
		public int DesertSandTileCount { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000142 RID: 322 RVA: 0x0000EF29 File Offset: 0x0000D129
		// (set) Token: 0x06000143 RID: 323 RVA: 0x0000EF31 File Offset: 0x0000D131
		public int OceanSandTileCount { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000EF3A File Offset: 0x0000D13A
		public bool EnoughTilesForShimmer
		{
			get
			{
				return this.ShimmerTileCount >= SceneMetrics.ShimmerTileThreshold;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000145 RID: 325 RVA: 0x0000EF4C File Offset: 0x0000D14C
		public bool EnoughTilesForJungle
		{
			get
			{
				return this.JungleTileCount >= SceneMetrics.JungleTileThreshold;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000EF5E File Offset: 0x0000D15E
		public bool EnoughTilesForHallow
		{
			get
			{
				return this.HolyTileCount >= SceneMetrics.HallowTileThreshold;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000147 RID: 327 RVA: 0x0000EF70 File Offset: 0x0000D170
		public bool EnoughTilesForSnow
		{
			get
			{
				return this.SnowTileCount >= SceneMetrics.SnowTileThreshold;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000EF82 File Offset: 0x0000D182
		public bool EnoughTilesForGlowingMushroom
		{
			get
			{
				return this.MushroomTileCount >= SceneMetrics.MushroomTileThreshold;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000EF94 File Offset: 0x0000D194
		public bool EnoughTilesForDesert
		{
			get
			{
				return this.DesertSandTileCount >= SceneMetrics.DesertTileThreshold;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600014A RID: 330 RVA: 0x0000EFA6 File Offset: 0x0000D1A6
		public bool EnoughTilesForCorruption
		{
			get
			{
				return this.EvilTileCount >= SceneMetrics.CorruptionTileThreshold;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
		public bool EnoughTilesForCrimson
		{
			get
			{
				return this.BloodTileCount >= SceneMetrics.CrimsonTileThreshold;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000EFCA File Offset: 0x0000D1CA
		public bool EnoughTilesForMeteor
		{
			get
			{
				return this.MeteorTileCount >= SceneMetrics.MeteorTileThreshold;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600014D RID: 333 RVA: 0x0000EFDC File Offset: 0x0000D1DC
		public bool EnoughTilesForDungeon
		{
			get
			{
				return this.DungeonTileCount >= SceneMetrics.DungeonTileThreshold;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600014E RID: 334 RVA: 0x0000EFEE File Offset: 0x0000D1EE
		public bool EnoughTilesForGraveyard
		{
			get
			{
				return this.GraveyardTileCount >= SceneMetrics.GraveyardTileThreshold;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600014F RID: 335 RVA: 0x0000F000 File Offset: 0x0000D200
		// (set) Token: 0x06000150 RID: 336 RVA: 0x0000F008 File Offset: 0x0000D208
		public bool BehindBackwall { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000151 RID: 337 RVA: 0x0000F011 File Offset: 0x0000D211
		public bool CloseEnoughToSolarTower
		{
			get
			{
				return this.WithinRangeOfNPC(517, (double)SceneMetrics.NPCEventZoneRadius);
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000F024 File Offset: 0x0000D224
		public bool CloseEnoughToVortexTower
		{
			get
			{
				return this.WithinRangeOfNPC(422, (double)SceneMetrics.NPCEventZoneRadius);
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000153 RID: 339 RVA: 0x0000F037 File Offset: 0x0000D237
		public bool CloseEnoughToNebulaTower
		{
			get
			{
				return this.WithinRangeOfNPC(507, (double)SceneMetrics.NPCEventZoneRadius);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000154 RID: 340 RVA: 0x0000F04A File Offset: 0x0000D24A
		public bool CloseEnoughToStardustTower
		{
			get
			{
				return this.WithinRangeOfNPC(493, (double)SceneMetrics.NPCEventZoneRadius);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000F05D File Offset: 0x0000D25D
		public bool CloseEnoughToDD2LanePortal
		{
			get
			{
				return this.WithinRangeOfNPC(549, (double)SceneMetrics.NPCEventZoneRadius);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000156 RID: 342 RVA: 0x0000F070 File Offset: 0x0000D270
		public float? DistanceToMoonLord
		{
			get
			{
				Vector2 vector = this.ClosestNPCPosition[398];
				if (vector == Vector2.Zero)
				{
					return null;
				}
				return new float?(Vector2.Distance(this.Center, vector));
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000F0B8 File Offset: 0x0000D2B8
		public float? MoonLordSkyIntensity
		{
			get
			{
				float? distanceToMoonLord = Main.SceneMetrics.DistanceToMoonLord;
				if (distanceToMoonLord != null)
				{
					float value = distanceToMoonLord.Value;
					return new float?(1f - Utils.SmoothStep(3000f, 6000f, value));
				}
				return null;
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000F106 File Offset: 0x0000D306
		public bool AnyNPCs(int type)
		{
			return this.ClosestNPCPosition[type] != Vector2.Zero;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000F11E File Offset: 0x0000D31E
		// (set) Token: 0x0600015A RID: 346 RVA: 0x0000F126 File Offset: 0x0000D326
		public int TownNPCCount { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000F12F File Offset: 0x0000D32F
		// (set) Token: 0x0600015C RID: 348 RVA: 0x0000F137 File Offset: 0x0000D337
		public Player PerspectivePlayer { get; private set; }

		// Token: 0x0600015D RID: 349 RVA: 0x0000F140 File Offset: 0x0000D340
		public SceneMetrics()
		{
			this.Reset();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000F19C File Offset: 0x0000D39C
		public void Scan(SceneMetricsScanSettings settings)
		{
			if (this.LastScanTime == Main.GameUpdateCount && this.Center == settings.BiomeScanCenterPositionInWorld)
			{
				return;
			}
			this.Reset();
			this.LastScanTime = Main.GameUpdateCount;
			this.Center = settings.BiomeScanCenterPositionInWorld;
			this.TileCenter = this.Center.ToTileCoordinates().ClampedInWorld(0);
			this.ScanTiles();
			if (settings.VisualScanArea != null)
			{
				this.ScanOnScreenTiles(settings.VisualScanArea.Value);
			}
			if (settings.ScanNPCPositions)
			{
				this.ScanNPCPositions();
			}
			this.AggregateTileCounts();
			this.CalculateZones();
			if (settings.PerspectivePlayer != null)
			{
				this.AddPlayerEffects(settings.PerspectivePlayer);
			}
			this.CanPlayCreditsRoll = (this.ActiveMusicBox == 85);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000F264 File Offset: 0x0000D464
		private void ScanTiles()
		{
			Rectangle tileRectangle = Utils.CenteredRectangle(this.TileCenter, SceneMetrics.ZoneScanSize);
			tileRectangle = WorldUtils.ClampToWorld(tileRectangle, 0);
			for (int i = tileRectangle.Left; i < tileRectangle.Right; i++)
			{
				for (int j = tileRectangle.Top; j < tileRectangle.Bottom; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null)
					{
						if (!tile.active())
						{
							if (tile.liquid > 0)
							{
								this._liquidCounts[(int)tile.liquidType()]++;
							}
						}
						else
						{
							this._tileCounts[(int)tile.type]++;
							if (TileID.Sets.isDesertBiomeSand[(int)tile.type] && WorldGen.oceanDepths(i, j))
							{
								int num = this.OceanSandTileCount;
								this.OceanSandTileCount = num + 1;
							}
							if (TileID.Sets.Campfires[(int)tile.type] && tile.frameY < 36)
							{
								this.HasCampfire = true;
							}
							if (tile.type == 49 && tile.frameX < 18)
							{
								int num = this.WaterCandleCount;
								this.WaterCandleCount = num + 1;
							}
							if (tile.type == 372 && tile.frameX < 18)
							{
								int num = this.PeaceCandleCount;
								this.PeaceCandleCount = num + 1;
							}
							if (tile.type == 646 && tile.frameX < 18)
							{
								int num = this.ShadowCandleCount;
								this.ShadowCandleCount = num + 1;
							}
							if (tile.type == 405 && tile.frameX < 54)
							{
								this.HasCampfire = true;
							}
							if (tile.type == 506 && tile.frameX < 72)
							{
								this.HasCatBast = true;
							}
							if (tile.type == 42 && tile.frameY >= 324 && tile.frameY <= 358)
							{
								this.HasHeartLantern = true;
							}
							if (tile.type == 42 && tile.frameY >= 252 && tile.frameY <= 286)
							{
								this.HasStarInBottle = true;
							}
							if (tile.type == 91)
							{
								int num2 = (int)(tile.frameX / 18);
								for (short num3 = tile.frameY; num3 >= 54; num3 -= 54)
								{
									num2 += 111;
								}
								bool flag = false;
								if ((tile.frameX < 396 && tile.frameY < 54) || num2 == 311 || num2 == 312)
								{
									flag = true;
								}
								if (!flag)
								{
									int num4 = (int)(tile.frameX / 18 - 21);
									for (int k = (int)tile.frameY; k >= 54; k -= 54)
									{
										num4 += 90;
										num4 += 21;
									}
									if (num2 >= 311)
									{
										num4--;
									}
									if (num2 >= 312)
									{
										num4--;
									}
									int num5 = BannerSystem.BannerToItem(num4);
									if (ItemID.Sets.BannerStrength.IndexInRange(num5) && ItemID.Sets.BannerStrength[num5].Enabled)
									{
										this.NPCBannerBuff[num4] = true;
										this.hasBanner = true;
									}
								}
							}
							this.UpdateOreFinder(new Point(i, j), tile);
						}
					}
				}
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000F578 File Offset: 0x0000D778
		private void ScanOnScreenTiles(Rectangle visualScanArea)
		{
			visualScanArea = WorldUtils.ClampToWorld(visualScanArea, 0);
			for (int i = visualScanArea.Left; i < visualScanArea.Right; i++)
			{
				for (int j = visualScanArea.Top; j < visualScanArea.Bottom; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.active())
					{
						if (tile.type == 104)
						{
							this.HasClock = true;
						}
						ushort type = tile.type;
						if (type <= 509)
						{
							if (type <= 207)
							{
								if (type != 139)
								{
									if (type == 207)
									{
										if (tile.frameY >= 72)
										{
											switch (tile.frameX / 36)
											{
											case 0:
												this.ActiveFountainColor = 0;
												break;
											case 1:
												this.ActiveFountainColor = 12;
												break;
											case 2:
												this.ActiveFountainColor = 3;
												break;
											case 3:
												this.ActiveFountainColor = 5;
												break;
											case 4:
												this.ActiveFountainColor = 2;
												break;
											case 5:
												this.ActiveFountainColor = 10;
												break;
											case 6:
												this.ActiveFountainColor = 4;
												break;
											case 7:
												this.ActiveFountainColor = 9;
												break;
											case 8:
												this.ActiveFountainColor = 8;
												break;
											case 9:
												this.ActiveFountainColor = 6;
												break;
											default:
												this.ActiveFountainColor = -1;
												break;
											}
										}
									}
								}
								else if (tile.frameX >= 36)
								{
									this.ActiveMusicBox = (int)(tile.frameY / 36);
								}
							}
							else if (type != 410)
							{
								if (type != 480)
								{
									if (type == 509)
									{
										if (tile.frameY >= 56)
										{
											this.ActiveMonolithType = 4;
										}
									}
								}
								else if (tile.frameY >= 54)
								{
									this.BloodMoonMonolith = true;
								}
							}
							else if (tile.frameY >= 56)
							{
								int activeMonolithType = (int)(tile.frameX / 36);
								this.ActiveMonolithType = activeMonolithType;
							}
						}
						else if (type <= 720)
						{
							if (type != 657)
							{
								if (type != 658)
								{
									if (type == 720)
									{
										if (tile.frameY >= 54)
										{
											this.CRTMonolith = true;
										}
									}
								}
								else
								{
									int shimmerMonolithState = (int)(tile.frameY / 54);
									this.ShimmerMonolithState = shimmerMonolithState;
								}
							}
							else if (tile.frameY >= 54)
							{
								this.EchoMonolith = true;
							}
						}
						else if (type != 721)
						{
							if (type != 725)
							{
								if (type == 733)
								{
									if (tile.frameY >= 54)
									{
										this.RadioThingMonolith = true;
									}
								}
							}
							else if (tile.frameY >= 54)
							{
								this.NoirMonolith = true;
							}
						}
						else if (tile.frameY >= 54)
						{
							this.RetroMonolith = true;
						}
					}
				}
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000F85C File Offset: 0x0000DA5C
		private void AggregateTileCounts()
		{
			int num = -10;
			if (Main.infectedSeed)
			{
				num *= 3;
			}
			if (this._tileCounts[27] > 0)
			{
				this.HasSunflower = true;
			}
			if (this._tileCounts[567] > 0)
			{
				this.HasGardenGnome = true;
			}
			this.ShimmerTileCount = this._liquidCounts[3];
			this.HoneyBlockCount = this._tileCounts[229];
			this.HolyTileCount = this._tileCounts[109] + this._tileCounts[492] + this._tileCounts[110] + this._tileCounts[113] + this._tileCounts[117] + this._tileCounts[116] + this._tileCounts[164] + this._tileCounts[403] + this._tileCounts[402];
			this.SnowTileCount = this._tileCounts[147] + this._tileCounts[148] + this._tileCounts[161] + this._tileCounts[162] + this._tileCounts[164] + this._tileCounts[163] + this._tileCounts[200];
			if (Main.remixWorld)
			{
				this.JungleTileCount = this._tileCounts[60] + this._tileCounts[61] + this._tileCounts[62] + this._tileCounts[74] + this._tileCounts[225];
				this.EvilTileCount = this._tileCounts[23] + this._tileCounts[661] + this._tileCounts[24] + this._tileCounts[25] + this._tileCounts[32] + this._tileCounts[112] + this._tileCounts[163] + this._tileCounts[400] + this._tileCounts[398] + this._tileCounts[27] * num + this._tileCounts[474];
				this.BloodTileCount = this._tileCounts[199] + this._tileCounts[662] + this._tileCounts[201] + this._tileCounts[203] + this._tileCounts[200] + this._tileCounts[401] + this._tileCounts[399] + this._tileCounts[234] + this._tileCounts[352] + this._tileCounts[27] * num + this._tileCounts[195];
			}
			else
			{
				this.JungleTileCount = this._tileCounts[60] + this._tileCounts[61] + this._tileCounts[62] + this._tileCounts[74] + this._tileCounts[226] + this._tileCounts[225];
				this.EvilTileCount = this._tileCounts[23] + this._tileCounts[661] + this._tileCounts[24] + this._tileCounts[25] + this._tileCounts[32] + this._tileCounts[112] + this._tileCounts[163] + this._tileCounts[400] + this._tileCounts[398] + this._tileCounts[27] * num;
				this.BloodTileCount = this._tileCounts[199] + this._tileCounts[662] + this._tileCounts[201] + this._tileCounts[203] + this._tileCounts[200] + this._tileCounts[401] + this._tileCounts[399] + this._tileCounts[234] + this._tileCounts[352] + this._tileCounts[27] * num;
			}
			this.MushroomTileCount = this._tileCounts[70] + this._tileCounts[71] + this._tileCounts[72] + this._tileCounts[528];
			this.MeteorTileCount = this._tileCounts[37];
			this.DungeonTileCount = this._tileCounts[41] + this._tileCounts[43] + this._tileCounts[44] + this._tileCounts[481] + this._tileCounts[482] + this._tileCounts[483];
			this.SandTileCount = this._tileCounts[53] + this._tileCounts[112] + this._tileCounts[116] + this._tileCounts[234] + this._tileCounts[397] + this._tileCounts[398] + this._tileCounts[402] + this._tileCounts[399] + this._tileCounts[396] + this._tileCounts[400] + this._tileCounts[403] + this._tileCounts[401];
			this.PartyMonolithCount = this._tileCounts[455];
			this.GraveyardTileCount = this._tileCounts[85];
			this.GraveyardTileCount -= this._tileCounts[27] / 2;
			if (this._tileCounts[27] > 0)
			{
				this.HasSunflower = true;
			}
			if (this.GraveyardTileCount > SceneMetrics.GraveyardTileMin)
			{
				this.HasSunflower = false;
			}
			if (this.GraveyardTileCount < 0)
			{
				this.GraveyardTileCount = 0;
			}
			if (this.HolyTileCount < 0)
			{
				this.HolyTileCount = 0;
			}
			if (this.EvilTileCount < 0)
			{
				this.EvilTileCount = 0;
			}
			if (this.BloodTileCount < 0)
			{
				this.BloodTileCount = 0;
			}
			int holyTileCount = this.HolyTileCount;
			this.HolyTileCount -= this.EvilTileCount;
			this.HolyTileCount -= this.BloodTileCount;
			this.EvilTileCount -= holyTileCount;
			this.BloodTileCount -= holyTileCount;
			if (this.HolyTileCount < 0)
			{
				this.HolyTileCount = 0;
			}
			if (this.EvilTileCount < 0)
			{
				this.EvilTileCount = 0;
			}
			if (this.BloodTileCount < 0)
			{
				this.BloodTileCount = 0;
			}
			this.DesertSandTileCount = Math.Max(0, this.SandTileCount - this.OceanSandTileCount);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000FE84 File Offset: 0x0000E084
		private void CalculateZones()
		{
			Tile tileSafely = Framing.GetTileSafely(this.TileCenter);
			this.BehindBackwall = (tileSafely.wall > 0);
			this.ZoneSkyHeight = ((double)this.TileCenter.Y <= Main.worldSurface * 0.3499999940395355);
			this.ZoneOverworldHeight = ((double)this.TileCenter.Y <= Main.worldSurface && (double)this.TileCenter.Y > Main.worldSurface * 0.3499999940395355);
			this.BelowSurface = ((double)this.TileCenter.Y > Main.worldSurface);
			this.ZoneDirtLayerHeight = ((double)this.TileCenter.Y <= Main.rockLayer && (double)this.TileCenter.Y > Main.worldSurface);
			this.ZoneRockLayerHeight = (this.TileCenter.Y <= Main.UnderworldLayer && (double)this.TileCenter.Y > Main.rockLayer);
			this.ZoneUnderworldHeight = (this.TileCenter.Y > Main.UnderworldLayer);
			this.ZoneCorrupt = this.EnoughTilesForCorruption;
			this.ZoneCrimson = this.EnoughTilesForCrimson;
			this.ZoneHallow = this.EnoughTilesForHallow;
			this.ZoneJungle = (this.EnoughTilesForJungle && !this.ZoneUnderworldHeight);
			this.ZoneSnow = this.EnoughTilesForSnow;
			this.ZoneDesert = this.EnoughTilesForDesert;
			this.ZoneGlowshroom = this.EnoughTilesForGlowingMushroom;
			this.ZoneMeteor = this.EnoughTilesForMeteor;
			this.ZoneGraveyard = this.EnoughTilesForGraveyard;
			this.ZoneDungeon = (this.EnoughTilesForDungeon && this.BelowSurface && Main.wallDungeon[(int)tileSafely.wall]);
			this.ZoneLihzhardTemple = (tileSafely.wall == 87);
			this.ZoneGranite = (tileSafely.wall == 184 || tileSafely.wall == 180);
			this.ZoneMarble = (tileSafely.wall == 183 || tileSafely.wall == 178);
			this.ZoneHive = (tileSafely.wall == 108 || tileSafely.wall == 86);
			this.ZoneGemCave = (tileSafely.wall >= 48 && tileSafely.wall <= 53);
			this.ZoneBeach = WorldGen.oceanDepths(this.TileCenter.X, this.TileCenter.Y);
			this.ZoneUndergroundDesert = (this.ZoneDesert && this.BelowSurface && (WallID.Sets.Conversion.Sandstone[(int)tileSafely.wall] || WallID.Sets.Conversion.HardenedSand[(int)tileSafely.wall] || tileSafely.wall == 223) && !Main.wallHouse[(int)tileSafely.wall]);
			this.SurfaceAtmospherics = WorldGen.IsSurfaceForAtmospherics(this.TileCenter);
			if (Main.remixWorld && this.ZoneDungeon)
			{
				this.SurfaceAtmospherics = false;
			}
			this.ZoneRain = (Main.raining && this.SurfaceAtmospherics);
			this.ZoneSandstorm = (this.ZoneDesert && this.SurfaceAtmospherics && Sandstorm.Happening);
			if (this.ZoneSandstorm)
			{
				this.ZoneRain = false;
			}
			this.UndergroundForShimmering = ((double)this.TileCenter.Y > Main.worldSurface + 84.0 && this.TileCenter.Y < Main.maxTilesY - 396);
			this.ZoneShimmer = (this.EnoughTilesForShimmer && this.UndergroundForShimmering && !this.ZoneDungeon);
			this.ZoneWaterCandle = (this.WaterCandleCount > 0);
			this.ZonePeaceCandle = (this.PeaceCandleCount > 0);
			this.ZoneShadowCandle = (this.ShadowCandleCount > 0);
			if (Main.dualDungeonsSeed && this.BelowSurface && !this.ZoneUnderworldHeight)
			{
				NPCSpawningFlagsForDualDungeons npcspawningFlagsForDualDungeons = default(NPCSpawningFlagsForDualDungeons);
				Point point = new Point(this.TileCenter.X, this.TileCenter.Y);
				int spawnTileType = 0;
				int spawnWallType = 0;
				for (int i = 0; i < 300; i++)
				{
					Tile tileSafely2 = Framing.GetTileSafely(point);
					if (npcspawningFlagsForDualDungeons.CanScan(tileSafely2) && npcspawningFlagsForDualDungeons.ScanZonesFor(true, point.X, point.Y, (int)tileSafely2.type, (int)tileSafely2.wall, true))
					{
						Tile tileSafely3 = Framing.GetTileSafely(new Point(point.X, point.Y - 1));
						spawnTileType = (int)tileSafely2.type;
						spawnWallType = (int)tileSafely3.wall;
						break;
					}
					point.Y++;
				}
				npcspawningFlagsForDualDungeons.ScanZonesFor(false, point.X, point.Y, spawnTileType, spawnWallType, true);
				this.ZoneDungeon = npcspawningFlagsForDualDungeons.ZoneDungeon;
				this.ZoneSnow = npcspawningFlagsForDualDungeons.ZoneSnow;
				this.ZoneGlowshroom = npcspawningFlagsForDualDungeons.ZoneGlowshroom;
				this.ZoneCorrupt = npcspawningFlagsForDualDungeons.ZoneCorrupt;
				this.ZoneCrimson = npcspawningFlagsForDualDungeons.ZoneCrimson;
				this.ZoneJungle = npcspawningFlagsForDualDungeons.ZoneJungle;
				this.ZoneHallow = npcspawningFlagsForDualDungeons.ZoneHallow;
				this.ZoneLihzhardTemple = npcspawningFlagsForDualDungeons.ZoneLihzhardTemple;
				this.ZoneUndergroundDesert = npcspawningFlagsForDualDungeons.ZoneUndergroundDesert;
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00010388 File Offset: 0x0000E588
		private void ScanNPCPositions()
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active)
				{
					Vector2 vector = this.ClosestNPCPosition[npc.type];
					if (vector == Vector2.Zero || Vector2.DistanceSquared(this.Center, npc.Center) < Vector2.DistanceSquared(this.Center, vector))
					{
						this.ClosestNPCPosition[npc.type] = npc.Center;
					}
					if (npc.townNPC && Utils.CenteredRectangle(this.Center, SceneMetrics.TownNPCRectSize).Contains(npc.Center.ToPoint()))
					{
						int townNPCCount = this.TownNPCCount;
						this.TownNPCCount = townNPCCount + 1;
					}
				}
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00010454 File Offset: 0x0000E654
		private void AddPlayerEffects(Player player)
		{
			this.PerspectivePlayer = player;
			if (player.inventory[player.selectedItem].type == 148)
			{
				this.ZoneWaterCandle = true;
			}
			if (player.inventory[player.selectedItem].type == 3117)
			{
				this.ZonePeaceCandle = true;
			}
			if (player.inventory[player.selectedItem].type == 5322)
			{
				this.ZoneShadowCandle = true;
			}
			if (player.musicBox >= 0)
			{
				this.ActiveMusicBox = player.musicBox;
			}
			if (player.happyFunTorchTime)
			{
				this.InTorchGodMinigame = true;
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000104EC File Offset: 0x0000E6EC
		public int GetTileCount(ushort tileId)
		{
			return this._tileCounts[(int)tileId];
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000104F8 File Offset: 0x0000E6F8
		public void Reset()
		{
			this.LastScanTime = uint.MaxValue;
			Array.Clear(this._tileCounts, 0, this._tileCounts.Length);
			Array.Clear(this._liquidCounts, 0, this._liquidCounts.Length);
			Array.Clear(this.ClosestNPCPosition, 0, this.ClosestNPCPosition.Length);
			this.SandTileCount = 0;
			this.EvilTileCount = 0;
			this.BloodTileCount = 0;
			this.GraveyardTileCount = 0;
			this.DesertSandTileCount = 0;
			this.MushroomTileCount = 0;
			this.SnowTileCount = 0;
			this.HolyTileCount = 0;
			this.HoneyBlockCount = 0;
			this.ShimmerTileCount = 0;
			this.MeteorTileCount = 0;
			this.JungleTileCount = 0;
			this.DungeonTileCount = 0;
			this.OceanSandTileCount = 0;
			this.HasCampfire = false;
			this.HasSunflower = false;
			this.HasGardenGnome = false;
			this.HasStarInBottle = false;
			this.HasHeartLantern = false;
			this.HasClock = false;
			this.HasCatBast = false;
			this.ActiveMusicBox = -1;
			this.WaterCandleCount = 0;
			this.PeaceCandleCount = 0;
			this.ShadowCandleCount = 0;
			this.ActiveFountainColor = -1;
			this.ActiveMonolithType = -1;
			this.PartyMonolithCount = 0;
			this.BloodMoonMonolith = false;
			this.MoonLordMonolith = false;
			this.EchoMonolith = false;
			this.ShimmerMonolithState = 0;
			this.CRTMonolith = false;
			this.RetroMonolith = false;
			this.NoirMonolith = false;
			this.RadioThingMonolith = false;
			this.BehindBackwall = false;
			this.BelowSurface = false;
			this.ZoneSkyHeight = false;
			this.ZoneOverworldHeight = false;
			this.ZoneDirtLayerHeight = false;
			this.ZoneRockLayerHeight = false;
			this.ZoneUnderworldHeight = false;
			this.ZoneCorrupt = false;
			this.ZoneCrimson = false;
			this.ZoneHallow = false;
			this.ZoneJungle = false;
			this.ZoneSnow = false;
			this.ZoneDesert = false;
			this.ZoneGlowshroom = false;
			this.ZoneMeteor = false;
			this.ZoneGraveyard = false;
			this.ZoneDungeon = false;
			this.ZoneLihzhardTemple = false;
			this.ZoneGranite = false;
			this.ZoneMarble = false;
			this.ZoneHive = false;
			this.ZoneGemCave = false;
			this.ZoneBeach = false;
			this.ZoneUndergroundDesert = false;
			this.SurfaceAtmospherics = false;
			this.ZoneRain = false;
			this.ZoneSandstorm = false;
			this.UndergroundForShimmering = false;
			this.ZoneShimmer = false;
			this.ZoneWaterCandle = false;
			this.ZonePeaceCandle = false;
			this.ZoneShadowCandle = false;
			this.InTorchGodMinigame = false;
			Array.Clear(this.NPCBannerBuff, 0, this.NPCBannerBuff.Length);
			this.hasBanner = false;
			this.CanPlayCreditsRoll = false;
			this.BestOreType = -1;
			this.BestOrePosition = default(Point);
			this._bestOreDistSq = int.MaxValue;
			this.TownNPCCount = 0;
			this.PerspectivePlayer = SceneMetrics._dummyPlayer;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00010780 File Offset: 0x0000E980
		private void UpdateOreFinder(Point pos, Tile tile)
		{
			int num = (int)Main.tileOreFinderPriority[(int)tile.type];
			if (num <= 0)
			{
				return;
			}
			int num2 = (int)((this.BestOreType < 0) ? -1 : Main.tileOreFinderPriority[this.BestOreType]);
			if (num < num2)
			{
				return;
			}
			if (!SceneMetrics.IsValidForOreFinder(tile))
			{
				return;
			}
			Point point = new Point(pos.X - this.TileCenter.X, pos.Y - this.TileCenter.Y);
			int num3 = point.X * point.X + point.Y * point.Y;
			if (num == num2 && num3 >= this._bestOreDistSq)
			{
				return;
			}
			this.BestOreType = (int)tile.type;
			this.BestOrePosition = pos;
			this._bestOreDistSq = num3;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00010838 File Offset: 0x0000EA38
		public static bool IsValidForOreFinder(Tile t)
		{
			if (t.type == 227)
			{
				return t.frameX >= 272 && t.frameX <= 374;
			}
			return t.type != 129 || t.frameX >= 324;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00010894 File Offset: 0x0000EA94
		public bool WithinRangeOfNPC(int type, double range)
		{
			Vector2 vector = this.ClosestNPCPosition[type];
			return vector != Vector2.Zero && (double)Vector2.DistanceSquared(this.Center, vector) <= range * range;
		}

		// Token: 0x0400009D RID: 157
		private static readonly Point AssumedConstantScreenSize = new Point(1920, 1200);

		// Token: 0x0400009E RID: 158
		private static readonly int ZoneScanPadding = 25;

		// Token: 0x0400009F RID: 159
		public static readonly Point ZoneScanSize = new Point(SceneMetrics.AssumedConstantScreenSize.X / 16 + SceneMetrics.ZoneScanPadding * 2 - 1, SceneMetrics.AssumedConstantScreenSize.Y / 16 + SceneMetrics.ZoneScanPadding * 2 - 1);

		// Token: 0x040000A0 RID: 160
		public static readonly Vector2 TownNPCRectSize = SceneMetrics.AssumedConstantScreenSize.ToVector2() * 2f;

		// Token: 0x040000A4 RID: 164
		private int _bestOreDistSq;

		// Token: 0x040000A6 RID: 166
		public int BestOreType;

		// Token: 0x040000A7 RID: 167
		public static int ShimmerTileThreshold = 300;

		// Token: 0x040000A8 RID: 168
		public static int CorruptionTileThreshold = 300;

		// Token: 0x040000A9 RID: 169
		public static int CorruptionTileMax = 1000;

		// Token: 0x040000AA RID: 170
		public static int CrimsonTileThreshold = 300;

		// Token: 0x040000AB RID: 171
		public static int CrimsonTileMax = 1000;

		// Token: 0x040000AC RID: 172
		public static int HallowTileThreshold = 125;

		// Token: 0x040000AD RID: 173
		public static int HallowTileMax = 600;

		// Token: 0x040000AE RID: 174
		public static int JungleTileThreshold = 140;

		// Token: 0x040000AF RID: 175
		public static int JungleTileMax = 700;

		// Token: 0x040000B0 RID: 176
		public static int SnowTileNormalThreshold = 1500;

		// Token: 0x040000B1 RID: 177
		public static int SnowTileSkyblockThreshold = 300;

		// Token: 0x040000B2 RID: 178
		public static int SnowTileMax = 6000;

		// Token: 0x040000B3 RID: 179
		public static int DesertTileNormalThreshold = 1500;

		// Token: 0x040000B4 RID: 180
		public static int DesertTileSkyblockThreshold = 300;

		// Token: 0x040000B5 RID: 181
		public static int MushroomTileThreshold = 100;

		// Token: 0x040000B6 RID: 182
		public static int MushroomTileMax = 160;

		// Token: 0x040000B7 RID: 183
		public static int MeteorTileThreshold = 75;

		// Token: 0x040000B8 RID: 184
		public static int DungeonTileThreshold = 250;

		// Token: 0x040000B9 RID: 185
		public static int GraveyardTileMax = 36;

		// Token: 0x040000BA RID: 186
		public static int GraveyardTileMin = 16;

		// Token: 0x040000BB RID: 187
		public static int GraveyardTileThreshold = 28;

		// Token: 0x040000E1 RID: 225
		public bool BelowSurface;

		// Token: 0x040000E2 RID: 226
		public bool ZoneSkyHeight;

		// Token: 0x040000E3 RID: 227
		public bool ZoneOverworldHeight;

		// Token: 0x040000E4 RID: 228
		public bool ZoneDirtLayerHeight;

		// Token: 0x040000E5 RID: 229
		public bool ZoneRockLayerHeight;

		// Token: 0x040000E6 RID: 230
		public bool ZoneUnderworldHeight;

		// Token: 0x040000E7 RID: 231
		public bool ZoneCorrupt;

		// Token: 0x040000E8 RID: 232
		public bool ZoneCrimson;

		// Token: 0x040000E9 RID: 233
		public bool ZoneHallow;

		// Token: 0x040000EA RID: 234
		public bool ZoneJungle;

		// Token: 0x040000EB RID: 235
		public bool ZoneSnow;

		// Token: 0x040000EC RID: 236
		public bool ZoneDesert;

		// Token: 0x040000ED RID: 237
		public bool ZoneGlowshroom;

		// Token: 0x040000EE RID: 238
		public bool ZoneMeteor;

		// Token: 0x040000EF RID: 239
		public bool ZoneGraveyard;

		// Token: 0x040000F0 RID: 240
		public bool ZoneDungeon;

		// Token: 0x040000F1 RID: 241
		public bool ZoneLihzhardTemple;

		// Token: 0x040000F2 RID: 242
		public bool ZoneGranite;

		// Token: 0x040000F3 RID: 243
		public bool ZoneMarble;

		// Token: 0x040000F4 RID: 244
		public bool ZoneHive;

		// Token: 0x040000F5 RID: 245
		public bool ZoneGemCave;

		// Token: 0x040000F6 RID: 246
		public bool ZoneBeach;

		// Token: 0x040000F7 RID: 247
		public bool ZoneUndergroundDesert;

		// Token: 0x040000F8 RID: 248
		public bool ZoneRain;

		// Token: 0x040000F9 RID: 249
		public bool ZoneSandstorm;

		// Token: 0x040000FA RID: 250
		public bool SurfaceAtmospherics;

		// Token: 0x040000FB RID: 251
		public bool UndergroundForShimmering;

		// Token: 0x040000FC RID: 252
		public bool ZoneShimmer;

		// Token: 0x040000FD RID: 253
		public bool ZoneWaterCandle;

		// Token: 0x040000FE RID: 254
		public bool ZonePeaceCandle;

		// Token: 0x040000FF RID: 255
		public bool ZoneShadowCandle;

		// Token: 0x04000100 RID: 256
		public bool InTorchGodMinigame;

		// Token: 0x04000101 RID: 257
		public static int NPCEventZoneRadius = 4000;

		// Token: 0x04000102 RID: 258
		public bool CanPlayCreditsRoll;

		// Token: 0x04000103 RID: 259
		public bool[] NPCBannerBuff = new bool[BannerSystem.MaxBannerTypes];

		// Token: 0x04000104 RID: 260
		public bool hasBanner;

		// Token: 0x04000105 RID: 261
		public Vector2[] ClosestNPCPosition = new Vector2[(int)NPCID.Count];

		// Token: 0x04000108 RID: 264
		private static Player _dummyPlayer = new Player();

		// Token: 0x04000109 RID: 265
		private readonly int[] _tileCounts = new int[(int)TileID.Count];

		// Token: 0x0400010A RID: 266
		private readonly int[] _liquidCounts = new int[(int)LiquidID.Count];
	}
}
