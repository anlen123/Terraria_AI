using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002C5 RID: 709
	public static class CommonConditions
	{
		// Token: 0x04005010 RID: 20496
		public static readonly ChromaCondition InMenu = new CommonConditions.SimpleCondition(() => Main.gameMenu && !Main.drunkWorld);

		// Token: 0x04005011 RID: 20497
		public static readonly ChromaCondition DrunkMenu = new CommonConditions.SimpleCondition(() => Main.gameMenu && Main.drunkWorld);

		// Token: 0x02000812 RID: 2066
		private class SimpleCondition : ChromaCondition
		{
			// Token: 0x060042E5 RID: 17125 RVA: 0x006BE8D5 File Offset: 0x006BCAD5
			public SimpleCondition(Func<bool> condition)
			{
				this._condition = condition;
			}

			// Token: 0x060042E6 RID: 17126 RVA: 0x006BE8E4 File Offset: 0x006BCAE4
			public override bool IsActive()
			{
				return this._condition();
			}

			// Token: 0x040071BA RID: 29114
			private Func<bool> _condition;
		}

		// Token: 0x02000813 RID: 2067
		private class SceneCondition : CommonConditions.SimpleCondition
		{
			// Token: 0x060042E7 RID: 17127 RVA: 0x006BE8F4 File Offset: 0x006BCAF4
			public SceneCondition(Func<SceneMetrics, bool> condition) : base(() => condition(Main.SceneMetrics))
			{
			}
		}

		// Token: 0x02000814 RID: 2068
		private class PlayerCondition : CommonConditions.SimpleCondition
		{
			// Token: 0x060042E8 RID: 17128 RVA: 0x006BE920 File Offset: 0x006BCB20
			public PlayerCondition(Func<Player, bool> condition) : base(() => condition(Main.LocalPlayer))
			{
			}
		}

		// Token: 0x02000815 RID: 2069
		public static class SurfaceBiome
		{
			// Token: 0x040071BB RID: 29115
			public static readonly ChromaCondition Ocean = new CommonConditions.SurfaceBiome.SurfaceCondition((SceneMetrics scene) => scene.ZoneBeach);

			// Token: 0x040071BC RID: 29116
			public static readonly ChromaCondition Desert = new CommonConditions.SurfaceBiome.SurfaceCondition((SceneMetrics scene) => scene.ZoneDesert);

			// Token: 0x040071BD RID: 29117
			public static readonly ChromaCondition Jungle = new CommonConditions.SurfaceBiome.SurfaceCondition((SceneMetrics scene) => scene.ZoneJungle);

			// Token: 0x040071BE RID: 29118
			public static readonly ChromaCondition Snow = new CommonConditions.SurfaceBiome.SurfaceCondition((SceneMetrics scene) => scene.ZoneSnow);

			// Token: 0x040071BF RID: 29119
			public static readonly ChromaCondition Mushroom = new CommonConditions.SurfaceBiome.SurfaceCondition((SceneMetrics scene) => scene.ZoneGlowshroom);

			// Token: 0x040071C0 RID: 29120
			public static readonly ChromaCondition Corruption = new CommonConditions.SurfaceBiome.SurfaceCondition((SceneMetrics scene) => scene.ZoneCorrupt);

			// Token: 0x040071C1 RID: 29121
			public static readonly ChromaCondition Hallow = new CommonConditions.SurfaceBiome.SurfaceCondition((SceneMetrics scene) => scene.ZoneHallow);

			// Token: 0x040071C2 RID: 29122
			public static readonly ChromaCondition Crimson = new CommonConditions.SurfaceBiome.SurfaceCondition((SceneMetrics scene) => scene.ZoneCrimson);

			// Token: 0x02000ACB RID: 2763
			private class SurfaceCondition : CommonConditions.SceneCondition
			{
				// Token: 0x06004C6B RID: 19563 RVA: 0x006D96B8 File Offset: 0x006D78B8
				public SurfaceCondition(Func<SceneMetrics, bool> condition) : base((SceneMetrics scene) => scene.ZoneOverworldHeight && condition(scene))
				{
				}
			}
		}

		// Token: 0x02000816 RID: 2070
		public static class MiscBiome
		{
			// Token: 0x040071C3 RID: 29123
			public static readonly ChromaCondition Meteorite = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneMeteor);
		}

		// Token: 0x02000817 RID: 2071
		public static class UndergroundBiome
		{
			// Token: 0x040071C4 RID: 29124
			public static readonly ChromaCondition Hive = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneHive);

			// Token: 0x040071C5 RID: 29125
			public static readonly ChromaCondition Jungle = new CommonConditions.UndergroundBiome.UndergroundCondition((SceneMetrics scene) => scene.ZoneJungle);

			// Token: 0x040071C6 RID: 29126
			public static readonly ChromaCondition Mushroom = new CommonConditions.UndergroundBiome.UndergroundCondition((SceneMetrics scene) => scene.ZoneGlowshroom);

			// Token: 0x040071C7 RID: 29127
			public static readonly ChromaCondition Ice = new CommonConditions.UndergroundBiome.UndergroundCondition((SceneMetrics scene) => scene.ZoneSnow);

			// Token: 0x040071C8 RID: 29128
			public static readonly ChromaCondition HallowIce = new CommonConditions.UndergroundBiome.UndergroundCondition((SceneMetrics scene) => scene.ZoneSnow && scene.ZoneHallow);

			// Token: 0x040071C9 RID: 29129
			public static readonly ChromaCondition CrimsonIce = new CommonConditions.UndergroundBiome.UndergroundCondition((SceneMetrics scene) => scene.ZoneSnow && scene.ZoneCrimson);

			// Token: 0x040071CA RID: 29130
			public static readonly ChromaCondition CorruptIce = new CommonConditions.UndergroundBiome.UndergroundCondition((SceneMetrics scene) => scene.ZoneSnow && scene.ZoneCorrupt);

			// Token: 0x040071CB RID: 29131
			public static readonly ChromaCondition Hallow = new CommonConditions.UndergroundBiome.UndergroundCondition((SceneMetrics scene) => scene.ZoneHallow);

			// Token: 0x040071CC RID: 29132
			public static readonly ChromaCondition Crimson = new CommonConditions.UndergroundBiome.UndergroundCondition((SceneMetrics scene) => scene.ZoneCrimson);

			// Token: 0x040071CD RID: 29133
			public static readonly ChromaCondition Corrupt = new CommonConditions.UndergroundBiome.UndergroundCondition((SceneMetrics scene) => scene.ZoneCorrupt);

			// Token: 0x040071CE RID: 29134
			public static readonly ChromaCondition Desert = new CommonConditions.UndergroundBiome.UndergroundCondition((SceneMetrics scene) => scene.ZoneDesert);

			// Token: 0x040071CF RID: 29135
			public static readonly ChromaCondition HallowDesert = new CommonConditions.UndergroundBiome.UndergroundCondition((SceneMetrics scene) => scene.ZoneDesert && scene.ZoneHallow);

			// Token: 0x040071D0 RID: 29136
			public static readonly ChromaCondition CrimsonDesert = new CommonConditions.UndergroundBiome.UndergroundCondition((SceneMetrics scene) => scene.ZoneDesert && scene.ZoneCrimson);

			// Token: 0x040071D1 RID: 29137
			public static readonly ChromaCondition CorruptDesert = new CommonConditions.UndergroundBiome.UndergroundCondition((SceneMetrics scene) => scene.ZoneDesert && scene.ZoneCorrupt);

			// Token: 0x040071D2 RID: 29138
			public static readonly ChromaCondition Temple = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneLihzhardTemple);

			// Token: 0x040071D3 RID: 29139
			public static readonly ChromaCondition Dungeon = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneDungeon);

			// Token: 0x040071D4 RID: 29140
			public static readonly ChromaCondition Marble = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneMarble);

			// Token: 0x040071D5 RID: 29141
			public static readonly ChromaCondition Granite = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneGranite);

			// Token: 0x040071D6 RID: 29142
			public static readonly ChromaCondition GemCave = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneGemCave);

			// Token: 0x040071D7 RID: 29143
			public static readonly ChromaCondition Shimmer = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneShimmer);

			// Token: 0x02000ACE RID: 2766
			private class UndergroundCondition : CommonConditions.SceneCondition
			{
				// Token: 0x06004C79 RID: 19577 RVA: 0x006D9744 File Offset: 0x006D7944
				public UndergroundCondition(Func<SceneMetrics, bool> condition) : base((SceneMetrics scene) => !scene.ZoneOverworldHeight && condition(scene))
				{
				}
			}
		}

		// Token: 0x02000818 RID: 2072
		public static class Boss
		{
			// Token: 0x040071D8 RID: 29144
			public static int HighestTierBossOrEvent;

			// Token: 0x040071D9 RID: 29145
			public static readonly ChromaCondition EaterOfWorlds = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 13);

			// Token: 0x040071DA RID: 29146
			public static readonly ChromaCondition Destroyer = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 134);

			// Token: 0x040071DB RID: 29147
			public static readonly ChromaCondition KingSlime = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 50);

			// Token: 0x040071DC RID: 29148
			public static readonly ChromaCondition QueenSlime = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 657);

			// Token: 0x040071DD RID: 29149
			public static readonly ChromaCondition BrainOfCthulhu = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 266);

			// Token: 0x040071DE RID: 29150
			public static readonly ChromaCondition DukeFishron = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 370);

			// Token: 0x040071DF RID: 29151
			public static readonly ChromaCondition QueenBee = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 222);

			// Token: 0x040071E0 RID: 29152
			public static readonly ChromaCondition Plantera = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 262);

			// Token: 0x040071E1 RID: 29153
			public static readonly ChromaCondition Empress = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 636);

			// Token: 0x040071E2 RID: 29154
			public static readonly ChromaCondition EyeOfCthulhu = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 4);

			// Token: 0x040071E3 RID: 29155
			public static readonly ChromaCondition TheTwins = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 126);

			// Token: 0x040071E4 RID: 29156
			public static readonly ChromaCondition MoonLord = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 398);

			// Token: 0x040071E5 RID: 29157
			public static readonly ChromaCondition WallOfFlesh = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 113);

			// Token: 0x040071E6 RID: 29158
			public static readonly ChromaCondition Golem = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 245);

			// Token: 0x040071E7 RID: 29159
			public static readonly ChromaCondition Cultist = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 439);

			// Token: 0x040071E8 RID: 29160
			public static readonly ChromaCondition Skeletron = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 35);

			// Token: 0x040071E9 RID: 29161
			public static readonly ChromaCondition SkeletronPrime = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 127);

			// Token: 0x040071EA RID: 29162
			public static readonly ChromaCondition Deerclops = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == 668);
		}

		// Token: 0x02000819 RID: 2073
		public static class Weather
		{
			// Token: 0x040071EB RID: 29163
			public static readonly ChromaCondition Rain = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneRain && !scene.ZoneSnow);

			// Token: 0x040071EC RID: 29164
			public static readonly ChromaCondition Sandstorm = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneSandstorm);

			// Token: 0x040071ED RID: 29165
			public static readonly ChromaCondition Blizzard = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneSnow && scene.ZoneRain);

			// Token: 0x040071EE RID: 29166
			public static readonly ChromaCondition SlimeRain = new CommonConditions.SceneCondition((SceneMetrics scene) => Main.slimeRain && scene.ZoneOverworldHeight);
		}

		// Token: 0x0200081A RID: 2074
		public static class Depth
		{
			// Token: 0x060042EE RID: 17134 RVA: 0x006BEEBC File Offset: 0x006BD0BC
			private static bool IsInFrontOfDirtWall(Point tilePosition)
			{
				if (!WorldGen.InWorld(tilePosition.X, tilePosition.Y, 0))
				{
					return false;
				}
				if (Main.tile[tilePosition.X, tilePosition.Y] == null)
				{
					return false;
				}
				ushort wall = Main.tile[tilePosition.X, tilePosition.Y].wall;
				if (wall <= 61)
				{
					if (wall <= 16)
					{
						if (wall != 2 && wall != 16)
						{
							return false;
						}
					}
					else if (wall - 54 > 5 && wall != 61)
					{
						return false;
					}
				}
				else if (wall <= 185)
				{
					if (wall - 170 > 1 && wall != 185)
					{
						return false;
					}
				}
				else if (wall - 196 > 3 && wall - 212 > 3)
				{
					return false;
				}
				return true;
			}

			// Token: 0x040071EF RID: 29167
			public static readonly ChromaCondition Sky = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneSkyHeight);

			// Token: 0x040071F0 RID: 29168
			public static readonly ChromaCondition Surface = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneOverworldHeight && !CommonConditions.Depth.IsInFrontOfDirtWall(scene.TileCenter));

			// Token: 0x040071F1 RID: 29169
			public static readonly ChromaCondition Vines = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneOverworldHeight && CommonConditions.Depth.IsInFrontOfDirtWall(scene.TileCenter));

			// Token: 0x040071F2 RID: 29170
			public static readonly ChromaCondition Underground = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneDirtLayerHeight);

			// Token: 0x040071F3 RID: 29171
			public static readonly ChromaCondition Caverns = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneRockLayerHeight && scene.TileCenter.Y <= Main.maxTilesY - 400);

			// Token: 0x040071F4 RID: 29172
			public static readonly ChromaCondition Magma = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneRockLayerHeight && scene.TileCenter.Y > Main.maxTilesY - 400);

			// Token: 0x040071F5 RID: 29173
			public static readonly ChromaCondition Underworld = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.ZoneUnderworldHeight);
		}

		// Token: 0x0200081B RID: 2075
		public static class Events
		{
			// Token: 0x040071F6 RID: 29174
			public static readonly ChromaCondition BloodMoon = new CommonConditions.SceneCondition((SceneMetrics _) => Main.bloodMoon && !Main.snowMoon && !Main.pumpkinMoon);

			// Token: 0x040071F7 RID: 29175
			public static readonly ChromaCondition FrostMoon = new CommonConditions.SceneCondition((SceneMetrics _) => Main.snowMoon);

			// Token: 0x040071F8 RID: 29176
			public static readonly ChromaCondition PumpkinMoon = new CommonConditions.SceneCondition((SceneMetrics _) => Main.pumpkinMoon);

			// Token: 0x040071F9 RID: 29177
			public static readonly ChromaCondition SolarEclipse = new CommonConditions.SceneCondition((SceneMetrics _) => Main.eclipse);

			// Token: 0x040071FA RID: 29178
			public static readonly ChromaCondition SolarPillar = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.CloseEnoughToSolarTower);

			// Token: 0x040071FB RID: 29179
			public static readonly ChromaCondition NebulaPillar = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.CloseEnoughToNebulaTower);

			// Token: 0x040071FC RID: 29180
			public static readonly ChromaCondition VortexPillar = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.CloseEnoughToVortexTower);

			// Token: 0x040071FD RID: 29181
			public static readonly ChromaCondition StardustPillar = new CommonConditions.SceneCondition((SceneMetrics scene) => scene.CloseEnoughToStardustTower);

			// Token: 0x040071FE RID: 29182
			public static readonly ChromaCondition PirateInvasion = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == -3);

			// Token: 0x040071FF RID: 29183
			public static readonly ChromaCondition DD2Event = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == -6);

			// Token: 0x04007200 RID: 29184
			public static readonly ChromaCondition FrostLegion = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == -2);

			// Token: 0x04007201 RID: 29185
			public static readonly ChromaCondition MartianMadness = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == -4);

			// Token: 0x04007202 RID: 29186
			public static readonly ChromaCondition GoblinArmy = new CommonConditions.SceneCondition((SceneMetrics _) => CommonConditions.Boss.HighestTierBossOrEvent == -1);
		}

		// Token: 0x0200081C RID: 2076
		public static class Alert
		{
			// Token: 0x04007203 RID: 29187
			public static readonly ChromaCondition MoonlordComing = new CommonConditions.SceneCondition((SceneMetrics _) => NPC.MoonLordCountdown > 0);

			// Token: 0x04007204 RID: 29188
			public static readonly ChromaCondition Keybinds = new CommonConditions.SimpleCondition(() => Main.InGameUI.CurrentState == Main.ManageControlsMenu || Main.MenuUI.CurrentState == Main.ManageControlsMenu);

			// Token: 0x04007205 RID: 29189
			public static readonly ChromaCondition Drowning = new CommonConditions.PlayerCondition((Player player) => player.breath != player.breathMax);

			// Token: 0x04007206 RID: 29190
			public static readonly ChromaCondition LavaIndicator = new CommonConditions.PlayerCondition((Player player) => player.lavaWet);
		}

		// Token: 0x0200081D RID: 2077
		public static class CriticalAlert
		{
			// Token: 0x04007207 RID: 29191
			public static readonly ChromaCondition LowLife = new CommonConditions.PlayerCondition((Player player) => Main.ChromaPainter.PotionAlert);

			// Token: 0x04007208 RID: 29192
			public static readonly ChromaCondition Death = new CommonConditions.PlayerCondition((Player player) => player.dead);
		}
	}
}
