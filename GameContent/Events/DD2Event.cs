using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Events
{
	// Token: 0x020004FA RID: 1274
	public class DD2Event
	{
		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06003566 RID: 13670 RVA: 0x00617E37 File Offset: 0x00616037
		public static bool ReadyToFindBartender
		{
			get
			{
				return NPC.downedBoss2;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06003567 RID: 13671 RVA: 0x00617E3E File Offset: 0x0061603E
		public static bool DownedInvasionAnyDifficulty
		{
			get
			{
				return DD2Event.DownedInvasionT1 || DD2Event.DownedInvasionT2 || DD2Event.DownedInvasionT3;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06003568 RID: 13672 RVA: 0x00617E55 File Offset: 0x00616055
		// (set) Token: 0x06003569 RID: 13673 RVA: 0x00617E5C File Offset: 0x0061605C
		public static int TimeLeftBetweenWaves
		{
			get
			{
				return DD2Event._timeLeftUntilSpawningBegins;
			}
			set
			{
				DD2Event._timeLeftUntilSpawningBegins = value;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x0600356A RID: 13674 RVA: 0x00617E64 File Offset: 0x00616064
		public static bool EnemySpawningIsOnHold
		{
			get
			{
				return DD2Event._timeLeftUntilSpawningBegins != 0;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x0600356B RID: 13675 RVA: 0x00617E6E File Offset: 0x0061606E
		public static bool EnemiesShouldChasePlayers
		{
			get
			{
				return DD2Event.Ongoing || true;
			}
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x00617E7A File Offset: 0x0061607A
		public static void Save(BinaryWriter writer)
		{
			writer.Write(DD2Event.DownedInvasionT1);
			writer.Write(DD2Event.DownedInvasionT2);
			writer.Write(DD2Event.DownedInvasionT3);
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x00617EA0 File Offset: 0x006160A0
		public static void Load(BinaryReader reader, int gameVersionNumber)
		{
			if (gameVersionNumber < 178)
			{
				NPC.savedBartender = false;
				DD2Event.ResetProgressEntirely();
				return;
			}
			NPC.savedBartender = reader.ReadBoolean();
			DD2Event.DownedInvasionT1 = reader.ReadBoolean();
			DD2Event.DownedInvasionT2 = reader.ReadBoolean();
			DD2Event.DownedInvasionT3 = reader.ReadBoolean();
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x00617EED File Offset: 0x006160ED
		public static void ResetProgressEntirely()
		{
			DD2Event.DownedInvasionT1 = (DD2Event.DownedInvasionT2 = (DD2Event.DownedInvasionT3 = false));
			DD2Event.Ongoing = false;
			DD2Event.ArenaHitbox = default(Rectangle);
			DD2Event._arenaHitboxingCooldown = 0;
			DD2Event._timeLeftUntilSpawningBegins = 0;
			DD2Event._damageTracker = null;
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x00617F24 File Offset: 0x00616124
		public static void ReportEventProgress()
		{
			int progressWave;
			int progressMax;
			int progress;
			DD2Event.GetInvasionStatus(out progressWave, out progressMax, out progress, false);
			Main.ReportInvasionProgress(progress, progressMax, 3, progressWave);
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x00617F48 File Offset: 0x00616148
		public static void SyncInvasionProgress(int toWho)
		{
			int num;
			int num2;
			int number;
			DD2Event.GetInvasionStatus(out num, out num2, out number, false);
			NetMessage.SendData(78, toWho, -1, null, number, (float)num2, 3f, (float)num, 0, 0, 0);
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x00617F78 File Offset: 0x00616178
		public static void UpdateTime()
		{
			if (!DD2Event.Ongoing && !Main.dedServ)
			{
				Filters.Scene.Deactivate("CrystalDestructionVortex", new object[0]);
				Filters.Scene.Deactivate("CrystalDestructionColor", new object[0]);
				Filters.Scene.Deactivate("CrystalWin", new object[0]);
				return;
			}
			if (Main.netMode != 1 && !NPC.AnyNPCs(548))
			{
				DD2Event.StopInvasion(false);
			}
			if (Main.netMode == 1)
			{
				if (DD2Event._timeLeftUntilSpawningBegins > 0)
				{
					DD2Event._timeLeftUntilSpawningBegins--;
				}
				if (DD2Event._timeLeftUntilSpawningBegins < 0)
				{
					DD2Event._timeLeftUntilSpawningBegins = 0;
				}
				return;
			}
			if (DD2Event._timeLeftUntilSpawningBegins > 0)
			{
				DD2Event._timeLeftUntilSpawningBegins--;
				if (DD2Event._timeLeftUntilSpawningBegins == 0)
				{
					int num;
					int progressMax;
					int progress;
					DD2Event.GetInvasionStatus(out num, out progressMax, out progress, false);
					if (!DD2Event.LostThisRun)
					{
						WorldGen.BroadcastText(Lang.GetInvasionWaveText(num, DD2Event.GetEnemiesForWave(num)), DD2Event.INFO_NEW_WAVE_COLOR);
						if (num == 7 && DD2Event.OngoingDifficulty == 3)
						{
							DD2Event.SummonBetsy();
						}
					}
					else
					{
						DD2Event.LoseInvasionMessage();
					}
					if (Main.netMode != 1)
					{
						Main.ReportInvasionProgress(progress, progressMax, 3, num);
					}
					if (Main.netMode == 2)
					{
						NetMessage.SendData(78, -1, -1, null, Main.invasionProgress, (float)Main.invasionProgressMax, 3f, (float)num, 0, 0, 0);
					}
				}
			}
			if (DD2Event._timeLeftUntilSpawningBegins < 0)
			{
				DD2Event._timeLeftUntilSpawningBegins = 0;
			}
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x006180C0 File Offset: 0x006162C0
		public static void StartInvasion(int difficultyOverride = -1)
		{
			if (Main.netMode != 1)
			{
				DD2Event._crystalsDropping_toDrop = 0;
				DD2Event._crystalsDropping_alreadyDropped = 0;
				DD2Event._crystalsDropping_lastWave = 0;
				DD2Event._timeLeftUntilSpawningBegins = 0;
				DD2Event.Ongoing = true;
				DD2Event.FindProperDifficulty();
				if (difficultyOverride != -1)
				{
					DD2Event.OngoingDifficulty = difficultyOverride;
				}
				DD2Event._deadGoblinSpots.Clear();
				DD2Event._downedDarkMageT1 = false;
				DD2Event._downedOgreT2 = false;
				DD2Event._spawnedBetsyT3 = false;
				DD2Event.LostThisRun = false;
				DD2Event.WonThisRun = false;
				NPC.totalInvasionPoints = 0f;
				NPC.waveKills = 0f;
				NPC.waveNumber = 1;
				DD2Event.ClearAllTowersInGame();
				NPCDamageTracker.Start(DD2Event._damageTracker = new DD2Event.DamageTracker());
				WorldGen.BroadcastText(NetworkText.FromKey("DungeonDefenders2.InvasionStart", new object[0]), DD2Event.INFO_START_INVASION_COLOR);
				NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				if (Main.netMode != 1)
				{
					Main.ReportInvasionProgress(0, 1, 3, 1);
				}
				if (Main.netMode == 2)
				{
					NetMessage.SendData(78, -1, -1, null, 0, 1f, 3f, 1f, 0, 0, 0);
				}
				DD2Event.SetEnemySpawningOnHold(300);
				DD2Event.WipeEntities();
			}
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x006181D8 File Offset: 0x006163D8
		public static void StopInvasion(bool win = false)
		{
			if (DD2Event.Ongoing)
			{
				if (win)
				{
					DD2Event.WinInvasionInternal();
				}
				DD2Event.Ongoing = false;
				DD2Event._deadGoblinSpots.Clear();
				if (Main.netMode != 1)
				{
					NPC.totalInvasionPoints = 0f;
					NPC.waveKills = 0f;
					NPC.waveNumber = 0;
					DD2Event.WipeEntities();
					NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				}
			}
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x00618248 File Offset: 0x00616448
		private static void WinInvasionInternal()
		{
			if (DD2Event.OngoingDifficulty >= 1)
			{
				DD2Event.DownedInvasionT1 = true;
			}
			if (DD2Event.OngoingDifficulty >= 2)
			{
				DD2Event.DownedInvasionT2 = true;
			}
			if (DD2Event.OngoingDifficulty >= 3)
			{
				DD2Event.DownedInvasionT3 = true;
			}
			if (DD2Event.OngoingDifficulty == 1)
			{
				DD2Event.DropMedals(3);
			}
			if (DD2Event.OngoingDifficulty == 2)
			{
				DD2Event.DropMedals(15);
			}
			if (DD2Event.OngoingDifficulty == 3)
			{
				AchievementsHelper.NotifyProgressionEvent(23);
				DD2Event.DropMedals(60);
			}
			WorldGen.BroadcastText(NetworkText.FromKey("DungeonDefenders2.InvasionWin", new object[0]), DD2Event.INFO_START_INVASION_COLOR);
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x006182CC File Offset: 0x006164CC
		public static void LoseInvasionMessage()
		{
			WorldGen.BroadcastText(NetworkText.FromKey("DungeonDefenders2.InvasionLose", new object[0]), DD2Event.INFO_FAILURE_INVASION_COLOR);
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06003576 RID: 13686 RVA: 0x006182E8 File Offset: 0x006164E8
		public static bool ReadyForTier2
		{
			get
			{
				return Main.hardMode && NPC.downedMechBossAny;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06003577 RID: 13687 RVA: 0x006182F8 File Offset: 0x006164F8
		public static bool ReadyForTier3
		{
			get
			{
				return Main.hardMode && NPC.downedGolemBoss;
			}
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x00618308 File Offset: 0x00616508
		private static void FindProperDifficulty()
		{
			DD2Event.OngoingDifficulty = 1;
			if (DD2Event.ReadyForTier2)
			{
				DD2Event.OngoingDifficulty = 2;
			}
			if (DD2Event.ReadyForTier3)
			{
				DD2Event.OngoingDifficulty = 3;
			}
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x0061832C File Offset: 0x0061652C
		public static void CheckProgress(int slainMonsterID)
		{
			if (Main.netMode == 1)
			{
				return;
			}
			if (!DD2Event.Ongoing)
			{
				return;
			}
			if (DD2Event.LostThisRun || DD2Event.WonThisRun)
			{
				return;
			}
			if (DD2Event.EnemySpawningIsOnHold)
			{
				return;
			}
			int num;
			int num2;
			int num3;
			DD2Event.GetInvasionStatus(out num, out num2, out num3, false);
			float num4 = (float)DD2Event.GetMonsterPointsWorth(slainMonsterID);
			float waveKills = NPC.waveKills;
			NPC.waveKills += num4;
			NPC.totalInvasionPoints += num4;
			num3 += (int)num4;
			bool flag = false;
			int num5 = num;
			if (NPC.waveKills >= (float)num2 && num2 != 0)
			{
				NPC.waveKills = 0f;
				NPC.waveNumber++;
				flag = true;
				DD2Event.GetInvasionStatus(out num, out num2, out num3, true);
				if (DD2Event.WonThisRun)
				{
					if ((float)num3 != waveKills && num4 != 0f)
					{
						if (Main.netMode != 1)
						{
							Main.ReportInvasionProgress(num3, num2, 3, num);
						}
						if (Main.netMode == 2)
						{
							NetMessage.SendData(78, -1, -1, null, Main.invasionProgress, (float)Main.invasionProgressMax, 3f, (float)num, 0, 0, 0);
						}
					}
					return;
				}
				int num6 = num;
				string key = "DungeonDefenders2.WaveComplete";
				if (num6 == 2)
				{
					key = "DungeonDefenders2.WaveCompleteFirst";
				}
				WorldGen.BroadcastText(NetworkText.FromKey(key, new object[0]), DD2Event.INFO_NEW_WAVE_COLOR);
				DD2Event.SetEnemySpawningOnHold(1800);
				if (DD2Event.OngoingDifficulty == 1)
				{
					if (num6 == 5)
					{
						DD2Event.DropMedals(1);
					}
					if (num6 == 4)
					{
						DD2Event.DropMedals(1);
					}
				}
				if (DD2Event.OngoingDifficulty == 2)
				{
					if (num6 == 7)
					{
						DD2Event.DropMedals(6);
					}
					if (num6 == 6)
					{
						DD2Event.DropMedals(3);
					}
					if (num6 == 5)
					{
						DD2Event.DropMedals(1);
					}
				}
				if (DD2Event.OngoingDifficulty == 3)
				{
					if (num6 == 7)
					{
						DD2Event.DropMedals(25);
					}
					if (num6 == 6)
					{
						DD2Event.DropMedals(11);
					}
					if (num6 == 5)
					{
						DD2Event.DropMedals(3);
					}
					if (num6 == 4)
					{
						DD2Event.DropMedals(1);
					}
				}
			}
			if ((float)num3 != waveKills)
			{
				if (flag)
				{
					int num7 = 1;
					int num8 = 1;
					if (Main.netMode != 1)
					{
						Main.ReportInvasionProgress(num7, num8, 3, num5);
					}
					if (Main.netMode == 2)
					{
						NetMessage.SendData(78, -1, -1, null, num7, (float)num8, 3f, (float)num5, 0, 0, 0);
						return;
					}
				}
				else
				{
					if (Main.netMode != 1)
					{
						Main.ReportInvasionProgress(num3, num2, 3, num);
					}
					if (Main.netMode == 2)
					{
						NetMessage.SendData(78, -1, -1, null, Main.invasionProgress, (float)Main.invasionProgressMax, 3f, (float)num, 0, 0, 0);
					}
				}
			}
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x00618558 File Offset: 0x00616758
		public static void StartVictoryScene()
		{
			DD2Event.WonThisRun = true;
			if (DD2Event._damageTracker != null)
			{
				DD2Event._damageTracker.Stop(true);
			}
			int num = NPC.FindFirstNPC(548);
			if (num == -1)
			{
				return;
			}
			Main.npc[num].ai[1] = 2f;
			Main.npc[num].ai[0] = 2f;
			Main.npc[num].netUpdate = true;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i] != null && Main.npc[i].active && Main.npc[i].type == 549)
				{
					Main.npc[i].ai[0] = 0f;
					Main.npc[i].ai[1] = 1f;
					Main.npc[i].netUpdate = true;
				}
			}
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x0061862D File Offset: 0x0061682D
		public static void ReportLoss()
		{
			if (DD2Event._damageTracker != null)
			{
				DD2Event._damageTracker.Stop(false);
			}
			DD2Event.LostThisRun = true;
			DD2Event.SetEnemySpawningOnHold(30);
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x00618650 File Offset: 0x00616850
		private static void GetInvasionStatus(out int currentWave, out int requiredKillCount, out int currentKillCount, bool currentlyInCheckProgress = false)
		{
			currentWave = NPC.waveNumber;
			requiredKillCount = 10;
			currentKillCount = (int)NPC.waveKills;
			int ongoingDifficulty = DD2Event.OngoingDifficulty;
			if (ongoingDifficulty == 2)
			{
				requiredKillCount = DD2Event.Difficulty_2_GetRequiredWaveKills(ref currentWave, ref currentKillCount, currentlyInCheckProgress);
				return;
			}
			if (ongoingDifficulty == 3)
			{
				requiredKillCount = DD2Event.Difficulty_3_GetRequiredWaveKills(ref currentWave, ref currentKillCount, currentlyInCheckProgress);
				return;
			}
			requiredKillCount = DD2Event.Difficulty_1_GetRequiredWaveKills(ref currentWave, ref currentKillCount, currentlyInCheckProgress);
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x006186A0 File Offset: 0x006168A0
		private static short[] GetEnemiesForWave(int wave)
		{
			int ongoingDifficulty = DD2Event.OngoingDifficulty;
			if (ongoingDifficulty == 2)
			{
				return DD2Event.Difficulty_2_GetEnemiesForWave(wave);
			}
			if (ongoingDifficulty == 3)
			{
				return DD2Event.Difficulty_3_GetEnemiesForWave(wave);
			}
			return DD2Event.Difficulty_1_GetEnemiesForWave(wave);
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x006186D0 File Offset: 0x006168D0
		private static int GetMonsterPointsWorth(int slainMonsterID)
		{
			int ongoingDifficulty = DD2Event.OngoingDifficulty;
			if (ongoingDifficulty == 2)
			{
				return DD2Event.Difficulty_2_GetMonsterPointsWorth(slainMonsterID);
			}
			if (ongoingDifficulty == 3)
			{
				return DD2Event.Difficulty_3_GetMonsterPointsWorth(slainMonsterID);
			}
			return DD2Event.Difficulty_1_GetMonsterPointsWorth(slainMonsterID);
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x00618700 File Offset: 0x00616900
		public static void SpawnMonsterFromGate(Vector2 gateBottom, bool leftGate)
		{
			int ongoingDifficulty = DD2Event.OngoingDifficulty;
			if (ongoingDifficulty == 2)
			{
				DD2Event.Difficulty_2_SpawnMonsterFromGate(gateBottom, leftGate);
				return;
			}
			if (ongoingDifficulty == 3)
			{
				DD2Event.Difficulty_3_SpawnMonsterFromGate(gateBottom, leftGate);
				return;
			}
			DD2Event.Difficulty_1_SpawnMonsterFromGate(gateBottom, leftGate);
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x00618734 File Offset: 0x00616934
		public static void SummonCrystal(int x, int y, int whoAsks)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendData(113, -1, -1, null, x, (float)y, 0f, 0f, 0, 0, 0);
				return;
			}
			DD2Event.SummonCrystalDirect(x, y, whoAsks);
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x0061876C File Offset: 0x0061696C
		public static void SummonCrystalDirect(int x, int y, int whoAsks)
		{
			if (NPC.AnyNPCs(548))
			{
				return;
			}
			Tile tileSafely = Framing.GetTileSafely(x, y);
			if (!tileSafely.active() || tileSafely.type != 466)
			{
				return;
			}
			Point point = new Point(x * 16, y * 16);
			point.X -= (int)(tileSafely.frameX / 18 * 16);
			point.Y -= (int)(tileSafely.frameY / 18 * 16);
			point.X += 40;
			point.Y += 64;
			DD2Event.StartInvasion(-1);
			NPC.NewNPC(Main.player[whoAsks].GetNPCSource_TileInteraction(x, y), point.X, point.Y, 548, 0, 0f, 0f, 0f, 0f, 255);
			DD2Event.DropStarterCrystals();
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x00618848 File Offset: 0x00616A48
		public static bool WouldFailSpawningHere(int x, int y)
		{
			Point point;
			Point point2;
			StrayMethods.CheckArenaScore(new Point(x, y).ToWorldCoordinates(8f, 8f), out point, out point2, 5, 10);
			int num = point2.X - x;
			int num2 = x - point.X;
			return num < 60 || num2 < 60;
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x00618894 File Offset: 0x00616A94
		public static void FailureMessage(int client)
		{
			LocalizedText text = Language.GetText("DungeonDefenders2.BartenderWarning");
			Color color = new Color(255, 255, 0);
			if (Main.netMode == 2)
			{
				ChatHelper.SendChatMessageToClient(NetworkText.FromKey(text.Key, new object[0]), color, client);
				return;
			}
			Main.NewText(text.Value, color.R, color.G, color.B);
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x00618900 File Offset: 0x00616B00
		public static void WipeEntities()
		{
			DD2Event.ClearAllTowersInGame();
			DD2Event.ClearAllDD2HostilesInGame();
			DD2Event.ClearAllDD2EnergyCrystalsInChests();
			if (Main.netMode == 2)
			{
				NetMessage.SendData(114, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x00618944 File Offset: 0x00616B44
		public static void ClearAllTowersInGame()
		{
			for (int i = 0; i < 1000; i++)
			{
				if (Main.projectile[i].active && ProjectileID.Sets.IsADD2Turret[Main.projectile[i].type])
				{
					Main.projectile[i].Kill();
				}
			}
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x00618990 File Offset: 0x00616B90
		public static void ClearAllDD2HostilesInGame()
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active && NPCID.Sets.BelongsToInvasionOldOnesArmy[Main.npc[i].type])
				{
					Main.npc[i].active = false;
					if (Main.netMode == 2)
					{
						NetMessage.SendData(23, -1, -1, null, i, 0f, 0f, 0f, 0, 0, 0);
					}
				}
			}
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x00618A04 File Offset: 0x00616C04
		public static void ClearAllDD2EnergyCrystalsInGame()
		{
			for (int i = 0; i < 400; i++)
			{
				WorldItem worldItem = Main.item[i];
				if (worldItem.active && worldItem.type == 3822)
				{
					worldItem.TurnToAir(false);
					if (Main.netMode == 2)
					{
						NetMessage.SendData(21, -1, -1, null, i, 0f, 0f, 0f, 0, 0, 0);
					}
				}
			}
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x00618A6C File Offset: 0x00616C6C
		public static void ClearAllDD2EnergyCrystalsInChests()
		{
			if (Main.netMode == 1)
			{
				return;
			}
			List<int> currentlyOpenChests = Chest.GetCurrentlyOpenChests();
			for (int i = 0; i < 8000; i++)
			{
				Chest chest = Main.chest[i];
				if (chest != null && currentlyOpenChests.Contains(i))
				{
					for (int j = 0; j < chest.maxItems; j++)
					{
						if (chest.item[j].type == 3822 && chest.item[j].stack > 0)
						{
							chest.item[j].TurnToAir(false);
							if (Main.netMode != 0)
							{
								NetMessage.SendData(32, -1, -1, null, i, (float)j, 0f, 0f, 0, 0, 0);
							}
						}
					}
				}
			}
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x00618B13 File Offset: 0x00616D13
		public static void AnnounceGoblinDeath(NPC n)
		{
			DD2Event._deadGoblinSpots.Add(n.Bottom);
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x00618B28 File Offset: 0x00616D28
		public static bool CanRaiseGoblinsHere(Vector2 spot)
		{
			int num = 0;
			using (List<Vector2>.Enumerator enumerator = DD2Event._deadGoblinSpots.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (Vector2.DistanceSquared(enumerator.Current, spot) <= 640000f)
					{
						num++;
						if (num >= 3)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x00618B90 File Offset: 0x00616D90
		public static void RaiseGoblins(NPC caller, Vector2 spot)
		{
			List<Vector2> list = new List<Vector2>();
			foreach (Vector2 vector in DD2Event._deadGoblinSpots)
			{
				if (Vector2.DistanceSquared(vector, spot) <= 722500f)
				{
					list.Add(vector);
				}
			}
			foreach (Vector2 item in list)
			{
				DD2Event._deadGoblinSpots.Remove(item);
			}
			int num = 0;
			foreach (Vector2 vec in list)
			{
				Point origin = vec.ToTileCoordinates();
				origin.X += Main.rand.Next(-15, 16);
				Point point;
				if (WorldUtils.Find(origin, Searches.Chain(new Searches.Down(50), new GenCondition[]
				{
					new Conditions.IsSolid()
				}), out point))
				{
					if (DD2Event.OngoingDifficulty == 3)
					{
						NPC.NewNPC(caller.GetSpawnSourceForNPCFromNPCAI(), point.X * 16 + 8, point.Y * 16, 567, 0, 0f, 0f, 0f, 0f, 255);
					}
					else
					{
						NPC.NewNPC(caller.GetSpawnSourceForNPCFromNPCAI(), point.X * 16 + 8, point.Y * 16, 566, 0, 0f, 0f, 0f, 0f, 255);
					}
					if (++num >= 8)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x00618D58 File Offset: 0x00616F58
		public static void FindArenaHitbox()
		{
			if (DD2Event._arenaHitboxingCooldown > 0)
			{
				DD2Event._arenaHitboxingCooldown--;
				return;
			}
			DD2Event._arenaHitboxingCooldown = 60;
			Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 vector2 = new Vector2(0f, 0f);
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active && (npc.type == 549 || npc.type == 548))
				{
					Vector2 vector3 = npc.TopLeft;
					if (vector.X > vector3.X)
					{
						vector.X = vector3.X;
					}
					if (vector.Y > vector3.Y)
					{
						vector.Y = vector3.Y;
					}
					vector3 = npc.BottomRight;
					if (vector2.X < vector3.X)
					{
						vector2.X = vector3.X;
					}
					if (vector2.Y < vector3.Y)
					{
						vector2.Y = vector3.Y;
					}
				}
			}
			Vector2 value = new Vector2(16f, 16f) * 50f;
			vector -= value;
			vector2 += value;
			Vector2 vector4 = vector2 - vector;
			DD2Event.ArenaHitbox.X = (int)vector.X;
			DD2Event.ArenaHitbox.Y = (int)vector.Y;
			DD2Event.ArenaHitbox.Width = (int)vector4.X;
			DD2Event.ArenaHitbox.Height = (int)vector4.Y;
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x00618EEE File Offset: 0x006170EE
		public static bool ShouldBlockBuilding(Vector2 worldPosition)
		{
			return DD2Event.ArenaHitbox.Contains(worldPosition.ToPoint());
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x00618F00 File Offset: 0x00617100
		public static void DropMedals(int numberOfMedals)
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == 548)
				{
					Main.npc[i].DropItemInstanced(Main.npc[i].position, Main.npc[i].Size, 3817, numberOfMedals, false);
				}
			}
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x00618F6C File Offset: 0x0061716C
		public static bool ShouldDropCrystals()
		{
			int num;
			int num2;
			int num3;
			DD2Event.GetInvasionStatus(out num, out num2, out num3, false);
			if (DD2Event._crystalsDropping_lastWave < num)
			{
				DD2Event._crystalsDropping_lastWave++;
				if (DD2Event._crystalsDropping_alreadyDropped > 0)
				{
					DD2Event._crystalsDropping_alreadyDropped -= DD2Event._crystalsDropping_toDrop;
				}
				if (DD2Event.OngoingDifficulty == 1)
				{
					switch (num)
					{
					case 1:
						DD2Event._crystalsDropping_toDrop = 20;
						break;
					case 2:
						DD2Event._crystalsDropping_toDrop = 20;
						break;
					case 3:
						DD2Event._crystalsDropping_toDrop = 30;
						break;
					case 4:
						DD2Event._crystalsDropping_toDrop = 30;
						break;
					case 5:
						DD2Event._crystalsDropping_toDrop = 40;
						break;
					}
				}
				else if (DD2Event.OngoingDifficulty == 2)
				{
					switch (num)
					{
					case 1:
						DD2Event._crystalsDropping_toDrop = 20;
						break;
					case 2:
						DD2Event._crystalsDropping_toDrop = 20;
						break;
					case 3:
						DD2Event._crystalsDropping_toDrop = 20;
						break;
					case 4:
						DD2Event._crystalsDropping_toDrop = 20;
						break;
					case 5:
						DD2Event._crystalsDropping_toDrop = 20;
						break;
					case 6:
						DD2Event._crystalsDropping_toDrop = 30;
						break;
					case 7:
						DD2Event._crystalsDropping_toDrop = 30;
						break;
					}
				}
				else if (DD2Event.OngoingDifficulty == 3)
				{
					switch (num)
					{
					case 1:
						DD2Event._crystalsDropping_toDrop = 20;
						break;
					case 2:
						DD2Event._crystalsDropping_toDrop = 20;
						break;
					case 3:
						DD2Event._crystalsDropping_toDrop = 20;
						break;
					case 4:
						DD2Event._crystalsDropping_toDrop = 20;
						break;
					case 5:
						DD2Event._crystalsDropping_toDrop = 30;
						break;
					case 6:
						DD2Event._crystalsDropping_toDrop = 30;
						break;
					case 7:
						DD2Event._crystalsDropping_toDrop = 30;
						break;
					}
				}
			}
			if (Main.netMode != 0 && Main.expertMode)
			{
				DD2Event._crystalsDropping_toDrop = (int)((float)DD2Event._crystalsDropping_toDrop * NPC.GetBalance());
			}
			float num4 = (float)num3 / (float)num2;
			if ((float)DD2Event._crystalsDropping_alreadyDropped < (float)DD2Event._crystalsDropping_toDrop * num4)
			{
				DD2Event._crystalsDropping_alreadyDropped++;
				return true;
			}
			return false;
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x00619148 File Offset: 0x00617348
		private static void SummonBetsy()
		{
			if (DD2Event._spawnedBetsyT3)
			{
				return;
			}
			if (NPC.AnyNPCs(551))
			{
				return;
			}
			Vector2 center = new Vector2(1f, 1f);
			int num = NPC.FindFirstNPC(548);
			if (num != -1)
			{
				center = Main.npc[num].Center;
			}
			NPC.SpawnOnPlayer((int)Player.FindClosest(center, 1, 1), 551, 0f, 0f, 0f, 0f);
			DD2Event._spawnedBetsyT3 = true;
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x006191C4 File Offset: 0x006173C4
		private static void DropStarterCrystals()
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == 548)
				{
					for (int j = 0; j < 5; j++)
					{
						Item.NewItem(new EntitySource_WorldEvent(), Main.npc[i].position, Main.npc[i].width, Main.npc[i].height, 3822, 2, false, 0, false);
					}
					return;
				}
			}
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x00619248 File Offset: 0x00617448
		private static void SetEnemySpawningOnHold(int forHowLong)
		{
			DD2Event._timeLeftUntilSpawningBegins = forHowLong;
			if (Main.netMode == 2)
			{
				NetMessage.SendData(116, -1, -1, null, DD2Event._timeLeftUntilSpawningBegins, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x00619284 File Offset: 0x00617484
		private static short[] Difficulty_1_GetEnemiesForWave(int wave)
		{
			DD2Event.LaneSpawnRate = 60;
			switch (wave)
			{
			case 1:
				DD2Event.LaneSpawnRate = 90;
				return new short[]
				{
					552
				};
			case 2:
				return new short[]
				{
					552,
					555
				};
			case 3:
				DD2Event.LaneSpawnRate = 55;
				return new short[]
				{
					552,
					555,
					561
				};
			case 4:
				DD2Event.LaneSpawnRate = 50;
				return new short[]
				{
					552,
					555,
					561,
					558
				};
			case 5:
				DD2Event.LaneSpawnRate = 40;
				return new short[]
				{
					552,
					555,
					561,
					558,
					564
				};
			default:
				return new short[]
				{
					552
				};
			}
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x0061933C File Offset: 0x0061753C
		private static int Difficulty_1_GetRequiredWaveKills(ref int waveNumber, ref int currentKillCount, bool currentlyInCheckProgress)
		{
			switch (waveNumber)
			{
			case -1:
				return 0;
			case 1:
				return 60;
			case 2:
				return 80;
			case 3:
				return 100;
			case 4:
				DD2Event._deadGoblinSpots.Clear();
				return 120;
			case 5:
				if (!DD2Event._downedDarkMageT1 && currentKillCount > 139)
				{
					currentKillCount = 139;
				}
				return 140;
			case 6:
				waveNumber = 5;
				currentKillCount = 1;
				if (currentlyInCheckProgress)
				{
					DD2Event.StartVictoryScene();
				}
				return 1;
			}
			return 10;
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x006193C0 File Offset: 0x006175C0
		private static void Difficulty_1_SpawnMonsterFromGate(Vector2 gateBottom, bool leftGate)
		{
			int x = (int)gateBottom.X;
			int y = (int)gateBottom.Y;
			int num = 50;
			int num2 = 6;
			if (NPC.waveNumber > 4)
			{
				num2 = 12;
			}
			else if (NPC.waveNumber > 3)
			{
				num2 = 8;
			}
			int num3 = 6;
			if (NPC.waveNumber > 4)
			{
				num3 = 8;
			}
			for (int i = 1; i < Main.CurrentFrameFlags.ActivePlayersCount; i++)
			{
				num = (int)((double)num * 1.3);
				num2 = (int)((double)num2 * 1.3);
				num3 = (int)((double)num3 * 1.3);
			}
			int num4 = Main.maxNPCs;
			switch (NPC.waveNumber)
			{
			case 1:
				if (NPC.CountNPCS(552) + NPC.CountNPCS(555) < num)
				{
					num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 552, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			case 2:
				if (NPC.CountNPCS(552) + NPC.CountNPCS(555) < num)
				{
					if (Main.rand.Next(7) == 0)
					{
						num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 555, 0, 0f, 0f, 0f, 0f, 255);
					}
					else
					{
						num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 552, 0, 0f, 0f, 0f, 0f, 255);
					}
				}
				break;
			case 3:
				if (Main.rand.Next(6) == 0 && NPC.CountNPCS(561) < num2)
				{
					num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 561, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(552) + NPC.CountNPCS(555) < num)
				{
					if (Main.rand.Next(5) == 0)
					{
						num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 555, 0, 0f, 0f, 0f, 0f, 255);
					}
					else
					{
						num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 552, 0, 0f, 0f, 0f, 0f, 255);
					}
				}
				break;
			case 4:
				if (Main.rand.Next(12) == 0 && NPC.CountNPCS(558) < num3)
				{
					num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 558, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(5) == 0 && NPC.CountNPCS(561) < num2)
				{
					num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 561, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(552) + NPC.CountNPCS(555) < num)
				{
					if (Main.rand.Next(5) == 0)
					{
						num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 555, 0, 0f, 0f, 0f, 0f, 255);
					}
					else
					{
						num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 552, 0, 0f, 0f, 0f, 0f, 255);
					}
				}
				break;
			case 5:
			{
				int num5;
				int num6;
				int num7;
				DD2Event.GetInvasionStatus(out num5, out num6, out num7, false);
				if ((!leftGate || Main.rand.Next(2) == 0) && (float)num7 > (float)num6 * 0.5f && !NPC.AnyNPCs(564))
				{
					num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 564, 0, 0f, 0f, 0f, 0f, 255);
				}
				if (Main.rand.Next(10) == 0 && NPC.CountNPCS(558) < num3)
				{
					num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 558, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(4) == 0 && NPC.CountNPCS(561) < num2)
				{
					num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 561, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(552) + NPC.CountNPCS(555) < num)
				{
					if (Main.rand.Next(4) == 0)
					{
						num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 555, 0, 0f, 0f, 0f, 0f, 255);
					}
					else
					{
						num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 552, 0, 0f, 0f, 0f, 0f, 255);
					}
				}
				break;
			}
			default:
				num4 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 552, 0, 0f, 0f, 0f, 0f, 255);
				break;
			}
			if (Main.netMode == 2 && num4 < Main.maxNPCs)
			{
				NetMessage.SendData(23, -1, -1, null, num4, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x00619930 File Offset: 0x00617B30
		private static int Difficulty_1_GetMonsterPointsWorth(int slainMonsterID)
		{
			if (NPC.waveNumber == 5 && NPC.waveKills >= 139f)
			{
				if (slainMonsterID == 564 || slainMonsterID == 565)
				{
					DD2Event._downedDarkMageT1 = true;
					return 1;
				}
				return 0;
			}
			else
			{
				if (slainMonsterID - 551 > 14 && slainMonsterID - 568 > 10)
				{
					return 0;
				}
				if (NPC.waveNumber == 5 && NPC.waveKills == 138f)
				{
					return 1;
				}
				if (!Main.expertMode)
				{
					return 1;
				}
				return 2;
			}
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x006199A4 File Offset: 0x00617BA4
		private static short[] Difficulty_2_GetEnemiesForWave(int wave)
		{
			DD2Event.LaneSpawnRate = 60;
			switch (wave)
			{
			case 1:
				DD2Event.LaneSpawnRate = 90;
				return new short[]
				{
					553,
					562
				};
			case 2:
				DD2Event.LaneSpawnRate = 70;
				return new short[]
				{
					553,
					562,
					572
				};
			case 3:
				return new short[]
				{
					553,
					556,
					562,
					559,
					572
				};
			case 4:
				DD2Event.LaneSpawnRate = 55;
				return new short[]
				{
					553,
					559,
					570,
					572,
					562
				};
			case 5:
				DD2Event.LaneSpawnRate = 50;
				return new short[]
				{
					553,
					556,
					559,
					572,
					574,
					570
				};
			case 6:
				DD2Event.LaneSpawnRate = 45;
				return new short[]
				{
					553,
					556,
					562,
					559,
					568,
					570,
					572,
					574
				};
			case 7:
				DD2Event.LaneSpawnRate = 42;
				return new short[]
				{
					553,
					556,
					572,
					559,
					568,
					574,
					570,
					576
				};
			default:
				return new short[]
				{
					553
				};
			}
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x00619A9C File Offset: 0x00617C9C
		private static int Difficulty_2_GetRequiredWaveKills(ref int waveNumber, ref int currentKillCount, bool currentlyInCheckProgress)
		{
			switch (waveNumber)
			{
			case -1:
				return 0;
			case 1:
				return 60;
			case 2:
				return 80;
			case 3:
				return 100;
			case 4:
				return 120;
			case 5:
				return 140;
			case 6:
				return 180;
			case 7:
				if (!DD2Event._downedOgreT2 && currentKillCount > 219)
				{
					currentKillCount = 219;
				}
				return 220;
			case 8:
				waveNumber = 7;
				currentKillCount = 1;
				if (currentlyInCheckProgress)
				{
					DD2Event.StartVictoryScene();
				}
				return 1;
			}
			return 10;
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x00619B28 File Offset: 0x00617D28
		private static int Difficulty_2_GetMonsterPointsWorth(int slainMonsterID)
		{
			if (NPC.waveNumber == 7 && NPC.waveKills >= 219f)
			{
				if (slainMonsterID == 576 || slainMonsterID == 577)
				{
					DD2Event._downedOgreT2 = true;
					return 1;
				}
				return 0;
			}
			else
			{
				if (slainMonsterID - 551 > 14 && slainMonsterID - 568 > 10)
				{
					return 0;
				}
				if (NPC.waveNumber == 7 && NPC.waveKills == 218f)
				{
					return 1;
				}
				if (!Main.expertMode)
				{
					return 1;
				}
				return 2;
			}
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x00619B9C File Offset: 0x00617D9C
		private static void Difficulty_2_SpawnMonsterFromGate(Vector2 gateBottom, bool leftGate)
		{
			int x = (int)gateBottom.X;
			int y = (int)gateBottom.Y;
			int num = 50;
			int num2 = 5;
			if (NPC.waveNumber > 1)
			{
				num2 = 8;
			}
			if (NPC.waveNumber > 3)
			{
				num2 = 10;
			}
			if (NPC.waveNumber > 5)
			{
				num2 = 12;
			}
			int num3 = 5;
			if (NPC.waveNumber > 4)
			{
				num3 = 7;
			}
			int num4 = 2;
			int num5 = 8;
			if (NPC.waveNumber > 3)
			{
				num5 = 12;
			}
			int num6 = 3;
			if (NPC.waveNumber > 5)
			{
				num6 = 5;
			}
			for (int i = 1; i < Main.CurrentFrameFlags.ActivePlayersCount; i++)
			{
				num = (int)((double)num * 1.3);
				num2 = (int)((double)num2 * 1.3);
				num5 = (int)((double)num * 1.3);
				num6 = (int)((double)num * 1.35);
			}
			int num7 = Main.maxNPCs;
			int num8 = Main.maxNPCs;
			switch (NPC.waveNumber)
			{
			case 1:
				if (Main.rand.Next(20) == 0 && NPC.CountNPCS(562) < num2)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 562, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(553) < num)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 553, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			case 2:
				if (Main.rand.Next(3) == 0 && NPC.CountNPCS(572) < num5)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 572, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(8) == 0 && NPC.CountNPCS(562) < num2)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 562, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(553) < num)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 553, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			case 3:
				if (Main.rand.Next(7) == 0 && NPC.CountNPCS(572) < num5)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 572, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(10) == 0 && NPC.CountNPCS(559) < num3)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 559, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(8) == 0 && NPC.CountNPCS(562) < num2)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 562, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(553) + NPC.CountNPCS(556) < num)
				{
					if (Main.rand.Next(4) == 0)
					{
						num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 556, 0, 0f, 0f, 0f, 0f, 255);
					}
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 553, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			case 4:
				if (Main.rand.Next(10) == 0 && NPC.CountNPCS(570) < num6)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 570, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(12) == 0 && NPC.CountNPCS(559) < num3)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 559, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(6) == 0 && NPC.CountNPCS(562) < num2)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 562, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(3) == 0 && NPC.CountNPCS(572) < num5)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 572, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(553) < num)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 553, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			case 5:
				if (Main.rand.Next(7) == 0 && NPC.CountNPCS(570) < num6)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 570, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(10) == 0 && NPC.CountNPCS(559) < num3)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 559, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(4) == 0 && NPC.CountNPCS(572) + NPC.CountNPCS(574) < num5)
				{
					if (Main.rand.Next(2) == 0)
					{
						num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 572, 0, 0f, 0f, 0f, 0f, 255);
					}
					else
					{
						num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 574, 0, 0f, 0f, 0f, 0f, 255);
					}
				}
				else if (NPC.CountNPCS(553) + NPC.CountNPCS(556) < num)
				{
					if (Main.rand.Next(3) == 0)
					{
						num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 556, 0, 0f, 0f, 0f, 0f, 255);
					}
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 553, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			case 6:
				if (Main.rand.Next(7) == 0 && NPC.CountNPCS(570) < num6)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 570, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(17) == 0 && NPC.CountNPCS(568) < num4)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 568, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(5) == 0 && NPC.CountNPCS(572) + NPC.CountNPCS(574) < num5)
				{
					if (Main.rand.Next(2) != 0)
					{
						num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 572, 0, 0f, 0f, 0f, 0f, 255);
					}
					else
					{
						num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 574, 0, 0f, 0f, 0f, 0f, 255);
					}
				}
				else if (Main.rand.Next(9) == 0 && NPC.CountNPCS(559) < num3)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 559, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(3) == 0 && NPC.CountNPCS(562) < num2)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 562, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(553) + NPC.CountNPCS(556) < num)
				{
					if (Main.rand.Next(3) != 0)
					{
						num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 556, 0, 0f, 0f, 0f, 0f, 255);
					}
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 553, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			case 7:
			{
				int num9;
				int num10;
				int num11;
				DD2Event.GetInvasionStatus(out num9, out num10, out num11, false);
				if ((!leftGate || Main.rand.Next(2) == 0) && (float)num11 > (float)num10 * 0.1f && !NPC.AnyNPCs(576))
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 576, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(570) < num6)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 570, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(17) == 0 && NPC.CountNPCS(568) < num4)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 568, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(572) + NPC.CountNPCS(574) < num5)
				{
					if (Main.rand.Next(3) != 0)
					{
						num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 572, 0, 0f, 0f, 0f, 0f, 255);
					}
					else
					{
						num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 574, 0, 0f, 0f, 0f, 0f, 255);
					}
				}
				else if (Main.rand.Next(11) == 0 && NPC.CountNPCS(559) < num3)
				{
					num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 559, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(553) + NPC.CountNPCS(556) < num)
				{
					if (Main.rand.Next(2) == 0)
					{
						num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 556, 0, 0f, 0f, 0f, 0f, 255);
					}
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 553, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			}
			default:
				num7 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 553, 0, 0f, 0f, 0f, 0f, 255);
				break;
			}
			if (Main.netMode == 2 && num7 < Main.maxNPCs)
			{
				NetMessage.SendData(23, -1, -1, null, num7, 0f, 0f, 0f, 0, 0, 0);
			}
			if (Main.netMode == 2 && num8 < Main.maxNPCs)
			{
				NetMessage.SendData(23, -1, -1, null, num8, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x0061A7F4 File Offset: 0x006189F4
		private static short[] Difficulty_3_GetEnemiesForWave(int wave)
		{
			DD2Event.LaneSpawnRate = 60;
			switch (wave)
			{
			case 1:
				DD2Event.LaneSpawnRate = 85;
				return new short[]
				{
					554,
					557,
					563
				};
			case 2:
				DD2Event.LaneSpawnRate = 75;
				return new short[]
				{
					554,
					557,
					563,
					573,
					578
				};
			case 3:
				DD2Event.LaneSpawnRate = 60;
				return new short[]
				{
					554,
					563,
					560,
					573,
					571
				};
			case 4:
				DD2Event.LaneSpawnRate = 60;
				return new short[]
				{
					554,
					560,
					571,
					573,
					563,
					575,
					565
				};
			case 5:
				DD2Event.LaneSpawnRate = 55;
				return new short[]
				{
					554,
					557,
					573,
					575,
					571,
					569,
					577
				};
			case 6:
				DD2Event.LaneSpawnRate = 60;
				return new short[]
				{
					554,
					557,
					563,
					578,
					569,
					571,
					577,
					565
				};
			case 7:
				DD2Event.LaneSpawnRate = 90;
				return new short[]
				{
					554,
					557,
					563,
					569,
					571,
					551
				};
			default:
				return new short[]
				{
					554
				};
			}
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x0061A8F0 File Offset: 0x00618AF0
		private static int Difficulty_3_GetRequiredWaveKills(ref int waveNumber, ref int currentKillCount, bool currentlyInCheckProgress)
		{
			switch (waveNumber)
			{
			case -1:
				return 0;
			case 1:
				return 60;
			case 2:
				return 80;
			case 3:
				return 100;
			case 4:
				return 120;
			case 5:
				return 140;
			case 6:
				return 180;
			case 7:
			{
				int num = NPC.FindFirstNPC(551);
				if (num == -1)
				{
					return 1;
				}
				currentKillCount = 100 - (int)((float)Main.npc[num].life / (float)Main.npc[num].lifeMax * 100f);
				return 100;
			}
			case 8:
				waveNumber = 7;
				currentKillCount = 1;
				if (currentlyInCheckProgress)
				{
					DD2Event.StartVictoryScene();
				}
				return 1;
			}
			return 10;
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x0061A999 File Offset: 0x00618B99
		private static int Difficulty_3_GetMonsterPointsWorth(int slainMonsterID)
		{
			if (NPC.waveNumber == 7)
			{
				if (slainMonsterID == 551)
				{
					return 1;
				}
				return 0;
			}
			else
			{
				if (slainMonsterID - 551 > 14 && slainMonsterID - 568 > 10)
				{
					return 0;
				}
				if (!Main.expertMode)
				{
					return 1;
				}
				return 2;
			}
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x0061A9D4 File Offset: 0x00618BD4
		private static void Difficulty_3_SpawnMonsterFromGate(Vector2 gateBottom, bool leftGate)
		{
			int x = (int)gateBottom.X;
			int y = (int)gateBottom.Y;
			int num = 60;
			int num2 = 7;
			if (NPC.waveNumber > 1)
			{
				num2 = 9;
			}
			if (NPC.waveNumber > 3)
			{
				num2 = 12;
			}
			if (NPC.waveNumber > 5)
			{
				num2 = 15;
			}
			int num3 = 7;
			if (NPC.waveNumber > 4)
			{
				num3 = 10;
			}
			int num4 = 2;
			if (NPC.waveNumber > 5)
			{
				num4 = 3;
			}
			int num5 = 12;
			if (NPC.waveNumber > 3)
			{
				num5 = 18;
			}
			int num6 = 4;
			if (NPC.waveNumber > 5)
			{
				num6 = 6;
			}
			int num7 = 4;
			for (int i = 1; i < Main.CurrentFrameFlags.ActivePlayersCount; i++)
			{
				num = (int)((double)num * 1.3);
				num2 = (int)((double)num2 * 1.3);
				num5 = (int)((double)num * 1.3);
				num6 = (int)((double)num * 1.35);
				num7 = (int)((double)num7 * 1.3);
			}
			int num8 = Main.maxNPCs;
			int num9 = Main.maxNPCs;
			switch (NPC.waveNumber)
			{
			case 1:
				if (Main.rand.Next(18) == 0 && NPC.CountNPCS(563) < num2)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 563, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(554) < num)
				{
					if (Main.rand.Next(7) == 0)
					{
						num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 557, 0, 0f, 0f, 0f, 0f, 255);
					}
					num9 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 554, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			case 2:
				if (Main.rand.Next(3) == 0 && NPC.CountNPCS(578) < num7)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 578, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(563) < num2)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 563, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(3) == 0 && NPC.CountNPCS(573) < num5)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 573, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(554) < num)
				{
					if (Main.rand.Next(4) == 0)
					{
						num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 557, 0, 0f, 0f, 0f, 0f, 255);
					}
					num9 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 554, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			case 3:
				if (Main.rand.Next(13) == 0 && NPC.CountNPCS(571) < num6)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 571, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(573) < num5)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 573, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(10) == 0 && NPC.CountNPCS(560) < num3)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 560, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(8) == 0 && NPC.CountNPCS(563) < num2)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 563, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(554) + NPC.CountNPCS(557) < num)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 554, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			case 4:
				if (Main.rand.Next(24) == 0 && !NPC.AnyNPCs(565))
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 565, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(12) == 0 && NPC.CountNPCS(571) < num6)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 571, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(15) == 0 && NPC.CountNPCS(560) < num3)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 560, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(563) < num2)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 563, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(5) == 0 && NPC.CountNPCS(573) + NPC.CountNPCS(575) < num5)
				{
					if (Main.rand.Next(3) != 0)
					{
						num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 573, 0, 0f, 0f, 0f, 0f, 255);
					}
					else
					{
						num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 575, 0, 0f, 0f, 0f, 0f, 255);
					}
				}
				else if (NPC.CountNPCS(554) < num)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 554, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			case 5:
				if (Main.rand.Next(20) == 0 && !NPC.AnyNPCs(577))
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 577, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(17) == 0 && NPC.CountNPCS(569) < num4)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 569, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(8) == 0 && NPC.CountNPCS(571) < num6)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 571, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(573) + NPC.CountNPCS(575) < num5)
				{
					if (Main.rand.Next(4) != 0)
					{
						num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 573, 0, 0f, 0f, 0f, 0f, 255);
					}
					else
					{
						num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 575, 0, 0f, 0f, 0f, 0f, 255);
					}
				}
				else if (NPC.CountNPCS(554) + NPC.CountNPCS(557) < num)
				{
					if (Main.rand.Next(3) == 0)
					{
						num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 557, 0, 0f, 0f, 0f, 0f, 255);
					}
					num9 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 554, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			case 6:
				if (Main.rand.Next(20) == 0 && !NPC.AnyNPCs(577))
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 577, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(20) == 0 && !NPC.AnyNPCs(565))
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 565, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(12) == 0 && NPC.CountNPCS(571) < num6)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 571, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(25) == 0 && NPC.CountNPCS(569) < num4)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 569, 0, 0f, 0f, 0f, 0f, 255);
				}
				if (Main.rand.Next(7) == 0 && NPC.CountNPCS(578) < num7)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 578, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(7) == 0 && NPC.CountNPCS(573) + NPC.CountNPCS(575) < num5)
				{
					if (Main.rand.Next(3) != 0)
					{
						num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 573, 0, 0f, 0f, 0f, 0f, 255);
					}
					else
					{
						num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 575, 0, 0f, 0f, 0f, 0f, 255);
					}
				}
				else if (Main.rand.Next(5) == 0 && NPC.CountNPCS(563) < num2)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 563, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(554) + NPC.CountNPCS(557) < num)
				{
					if (Main.rand.Next(3) == 0)
					{
						num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 557, 0, 0f, 0f, 0f, 0f, 255);
					}
					num9 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 554, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			case 7:
				if (Main.rand.Next(20) == 0 && NPC.CountNPCS(571) < num6)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 571, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(17) == 0 && NPC.CountNPCS(569) < num4)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 569, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (Main.rand.Next(10) == 0 && NPC.CountNPCS(563) < num2)
				{
					num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 563, 0, 0f, 0f, 0f, 0f, 255);
				}
				else if (NPC.CountNPCS(554) + NPC.CountNPCS(557) < num)
				{
					if (Main.rand.Next(5) == 0)
					{
						num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 557, 0, 0f, 0f, 0f, 0f, 255);
					}
					num9 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 554, 0, 0f, 0f, 0f, 0f, 255);
				}
				break;
			default:
				num8 = NPC.NewNPC(DD2Event.GetSpawnSource_OldOnesArmy(), x, y, 554, 0, 0f, 0f, 0f, 0f, 255);
				break;
			}
			if (Main.netMode == 2 && num8 < Main.maxNPCs)
			{
				NetMessage.SendData(23, -1, -1, null, num8, 0f, 0f, 0f, 0, 0, 0);
			}
			if (Main.netMode == 2 && num9 < Main.maxNPCs)
			{
				NetMessage.SendData(23, -1, -1, null, num9, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x0061B798 File Offset: 0x00619998
		public static bool IsStandActive(int x, int y)
		{
			Vector2 target = new Vector2((float)(x * 16 + 8), (float)(y * 16 + 8));
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc != null && npc.active && npc.type == 548)
				{
					return npc.Bottom.Distance(target) < 36f;
				}
			}
			return false;
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x0061B800 File Offset: 0x00619A00
		public static void RequestToSkipWaitTime(int x, int y)
		{
			if (DD2Event.TimeLeftBetweenWaves <= 60)
			{
				return;
			}
			if (!DD2Event.IsStandActive(x, y))
			{
				return;
			}
			SoundEngine.PlaySound(SoundID.NPCDeath7, x * 16 + 8, y * 16 + 8, 0f, 1f);
			if (Main.netMode == 0)
			{
				DD2Event.AttemptToSkipWaitTime();
				return;
			}
			if (Main.netMode != 2)
			{
				NetMessage.SendData(143, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x0061B877 File Offset: 0x00619A77
		public static void AttemptToSkipWaitTime()
		{
			if (Main.netMode == 1 || DD2Event.TimeLeftBetweenWaves <= 60)
			{
				return;
			}
			DD2Event.SetEnemySpawningOnHold(60);
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x0061B892 File Offset: 0x00619A92
		private static IEntitySource GetSpawnSource_OldOnesArmy()
		{
			return new EntitySource_OldOnesArmy();
		}

		// Token: 0x04005A97 RID: 23191
		private static readonly Color INFO_NEW_WAVE_COLOR = new Color(175, 55, 255);

		// Token: 0x04005A98 RID: 23192
		private static readonly Color INFO_START_INVASION_COLOR = new Color(50, 255, 130);

		// Token: 0x04005A99 RID: 23193
		private static readonly Color INFO_FAILURE_INVASION_COLOR = new Color(255, 0, 0);

		// Token: 0x04005A9A RID: 23194
		private const int INVASION_ID = 3;

		// Token: 0x04005A9B RID: 23195
		public static bool DownedInvasionT1;

		// Token: 0x04005A9C RID: 23196
		public static bool DownedInvasionT2;

		// Token: 0x04005A9D RID: 23197
		public static bool DownedInvasionT3;

		// Token: 0x04005A9E RID: 23198
		public static bool LostThisRun;

		// Token: 0x04005A9F RID: 23199
		public static bool WonThisRun;

		// Token: 0x04005AA0 RID: 23200
		public static int LaneSpawnRate = 60;

		// Token: 0x04005AA1 RID: 23201
		private static bool _downedDarkMageT1;

		// Token: 0x04005AA2 RID: 23202
		private static bool _downedOgreT2;

		// Token: 0x04005AA3 RID: 23203
		private static bool _spawnedBetsyT3;

		// Token: 0x04005AA4 RID: 23204
		public static bool Ongoing;

		// Token: 0x04005AA5 RID: 23205
		private static DD2Event.DamageTracker _damageTracker;

		// Token: 0x04005AA6 RID: 23206
		public static Rectangle ArenaHitbox;

		// Token: 0x04005AA7 RID: 23207
		private static int _arenaHitboxingCooldown;

		// Token: 0x04005AA8 RID: 23208
		public static int OngoingDifficulty;

		// Token: 0x04005AA9 RID: 23209
		private static List<Vector2> _deadGoblinSpots = new List<Vector2>();

		// Token: 0x04005AAA RID: 23210
		private static int _crystalsDropping_lastWave;

		// Token: 0x04005AAB RID: 23211
		private static int _crystalsDropping_toDrop;

		// Token: 0x04005AAC RID: 23212
		private static int _crystalsDropping_alreadyDropped;

		// Token: 0x04005AAD RID: 23213
		private static int _timeLeftUntilSpawningBegins;

		// Token: 0x02000989 RID: 2441
		public class DamageTracker : NPCDamageTracker
		{
			// Token: 0x1700058F RID: 1423
			// (get) Token: 0x0600495C RID: 18780 RVA: 0x006D06BF File Offset: 0x006CE8BF
			public override LocalizedText Name
			{
				get
				{
					return Language.GetText("Bestiary_Invasions.OldOnesArmy");
				}
			}

			// Token: 0x17000590 RID: 1424
			// (get) Token: 0x0600495D RID: 18781 RVA: 0x006D06CB File Offset: 0x006CE8CB
			public override LocalizedText KillTimeMessage
			{
				get
				{
					return Language.GetText(this._won ? "BossDamageCommand.KillTimeDefeated" : "BossDamageCommand.KillTimeLost");
				}
			}

			// Token: 0x0600495E RID: 18782 RVA: 0x006D06E6 File Offset: 0x006CE8E6
			protected override bool IncludeDamageFor(NPC npc)
			{
				return NPCID.Sets.BelongsToInvasionOldOnesArmy[npc.type] && npc.type != 548;
			}

			// Token: 0x0600495F RID: 18783 RVA: 0x006D0708 File Offset: 0x006CE908
			public void Stop(bool won)
			{
				this._won = won;
				base.Stop();
			}

			// Token: 0x04007609 RID: 30217
			private bool _won;
		}
	}
}
