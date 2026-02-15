using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.NetModules;
using Terraria.Net;

namespace Terraria.GameContent.Ambience
{
	// Token: 0x02000362 RID: 866
	public class AmbienceServer
	{
		// Token: 0x060028C2 RID: 10434 RVA: 0x0057438E File Offset: 0x0057258E
		private static bool IsSunnyDay()
		{
			return !Main.IsItRaining && Main.dayTime && !Main.eclipse;
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x005743A8 File Offset: 0x005725A8
		private static bool IsSunset()
		{
			return Main.dayTime && Main.time > 40500.0;
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x005743C3 File Offset: 0x005725C3
		private static bool IsCalmNight()
		{
			return !Main.IsItRaining && !Main.dayTime && !Main.bloodMoon && !Main.pumpkinMoon && !Main.snowMoon;
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x005743EC File Offset: 0x005725EC
		public AmbienceServer()
		{
			this.ResetSpawnTime();
			this._spawnConditions[SkyEntityType.BirdsV] = new Func<bool>(AmbienceServer.IsSunnyDay);
			this._spawnConditions[SkyEntityType.Wyvern] = (() => AmbienceServer.IsSunnyDay() && Main.hardMode);
			this._spawnConditions[SkyEntityType.Airship] = (() => AmbienceServer.IsSunnyDay() && Main.IsItAHappyWindyDay);
			this._spawnConditions[SkyEntityType.AirBalloon] = (() => AmbienceServer.IsSunnyDay() && !Main.IsItAHappyWindyDay);
			this._spawnConditions[SkyEntityType.Eyeball] = (() => !Main.dayTime);
			this._spawnConditions[SkyEntityType.Butterflies] = (() => AmbienceServer.IsSunnyDay() && !Main.IsItAHappyWindyDay && !NPC.TooWindyForButterflies && NPC.butterflyChance < 6);
			this._spawnConditions[SkyEntityType.LostKite] = (() => Main.dayTime && !Main.eclipse && Main.IsItAHappyWindyDay);
			this._spawnConditions[SkyEntityType.Vulture] = (() => AmbienceServer.IsSunnyDay());
			this._spawnConditions[SkyEntityType.Bats] = (() => (AmbienceServer.IsSunset() && AmbienceServer.IsSunnyDay()) || AmbienceServer.IsCalmNight());
			this._spawnConditions[SkyEntityType.PixiePosse] = (() => AmbienceServer.IsSunnyDay() || AmbienceServer.IsCalmNight());
			this._spawnConditions[SkyEntityType.Seagulls] = (() => AmbienceServer.IsSunnyDay());
			this._spawnConditions[SkyEntityType.SlimeBalloons] = (() => AmbienceServer.IsSunnyDay() && Main.IsItAHappyWindyDay);
			this._spawnConditions[SkyEntityType.Gastropods] = (() => AmbienceServer.IsCalmNight());
			this._spawnConditions[SkyEntityType.Pegasus] = (() => AmbienceServer.IsSunnyDay());
			this._spawnConditions[SkyEntityType.EaterOfSouls] = (() => AmbienceServer.IsSunnyDay() || AmbienceServer.IsCalmNight());
			this._spawnConditions[SkyEntityType.Crimera] = (() => AmbienceServer.IsSunnyDay() || AmbienceServer.IsCalmNight());
			this._spawnConditions[SkyEntityType.Hellbats] = (() => true);
			this._secondarySpawnConditionsPerPlayer[SkyEntityType.Vulture] = ((Player player) => player.ZoneDesert);
			this._secondarySpawnConditionsPerPlayer[SkyEntityType.PixiePosse] = ((Player player) => player.ZoneHallow);
			this._secondarySpawnConditionsPerPlayer[SkyEntityType.Seagulls] = ((Player player) => player.ZoneBeach);
			this._secondarySpawnConditionsPerPlayer[SkyEntityType.Gastropods] = ((Player player) => player.ZoneHallow);
			this._secondarySpawnConditionsPerPlayer[SkyEntityType.Pegasus] = ((Player player) => player.ZoneHallow);
			this._secondarySpawnConditionsPerPlayer[SkyEntityType.EaterOfSouls] = ((Player player) => player.ZoneCorrupt);
			this._secondarySpawnConditionsPerPlayer[SkyEntityType.Crimera] = ((Player player) => player.ZoneCrimson);
			this._secondarySpawnConditionsPerPlayer[SkyEntityType.Bats] = ((Player player) => player.ZoneJungle);
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x00574857 File Offset: 0x00572A57
		private bool IsPlayerAtRightHeightForType(SkyEntityType type, Player plr)
		{
			if (type == SkyEntityType.Hellbats)
			{
				return AmbienceServer.IsPlayerInAPlaceWhereTheyCanSeeAmbienceHell(plr);
			}
			return AmbienceServer.IsPlayerInAPlaceWhereTheyCanSeeAmbienceSky(plr);
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x0057486C File Offset: 0x00572A6C
		public void Update()
		{
			this.SpawnForcedEntities();
			if (this._updatesUntilNextAttempt > 0)
			{
				this._updatesUntilNextAttempt -= Main.dayRate;
				return;
			}
			this.ResetSpawnTime();
			IEnumerable<SkyEntityType> source = from pair in this._spawnConditions
			where pair.Value()
			select pair.Key;
			if (source.Count((SkyEntityType type) => true) == 0)
			{
				return;
			}
			Player player;
			AmbienceServer.FindPlayerThatCanSeeBackgroundAmbience(out player);
			if (player == null)
			{
				return;
			}
			IEnumerable<SkyEntityType> source2 = from type in source
			where this.IsPlayerAtRightHeightForType(type, player) && this._secondarySpawnConditionsPerPlayer.ContainsKey(type) && this._secondarySpawnConditionsPerPlayer[type](player)
			select type;
			int num = source2.Count((SkyEntityType type) => true);
			if (num == 0 || Main.rand.Next(5) < 3)
			{
				source2 = from type in source
				where this.IsPlayerAtRightHeightForType(type, player) && (!this._secondarySpawnConditionsPerPlayer.ContainsKey(type) || this._secondarySpawnConditionsPerPlayer[type](player))
				select type;
				num = source2.Count((SkyEntityType type) => true);
			}
			if (num == 0)
			{
				return;
			}
			SkyEntityType type2 = source2.ElementAt(Main.rand.Next(num));
			this.SpawnForPlayer(player, type2);
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x005749E1 File Offset: 0x00572BE1
		public void ResetSpawnTime()
		{
			this._updatesUntilNextAttempt = Main.rand.Next(600, 7200);
			if (Main.tenthAnniversaryWorld)
			{
				this._updatesUntilNextAttempt /= 2;
			}
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x00574A12 File Offset: 0x00572C12
		public void ForceEntitySpawn(AmbienceServer.AmbienceSpawnInfo info)
		{
			this._forcedSpawns.Add(info);
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x00574A20 File Offset: 0x00572C20
		private void SpawnForcedEntities()
		{
			if (this._forcedSpawns.Count == 0)
			{
				return;
			}
			for (int i = this._forcedSpawns.Count - 1; i >= 0; i--)
			{
				AmbienceServer.AmbienceSpawnInfo ambienceSpawnInfo = this._forcedSpawns[i];
				Player player;
				if (ambienceSpawnInfo.targetPlayer == -1)
				{
					AmbienceServer.FindPlayerThatCanSeeBackgroundAmbience(out player);
				}
				else
				{
					player = Main.player[ambienceSpawnInfo.targetPlayer];
				}
				if (player != null && this.IsPlayerAtRightHeightForType(ambienceSpawnInfo.skyEntityType, player))
				{
					this.SpawnForPlayer(player, ambienceSpawnInfo.skyEntityType);
				}
				this._forcedSpawns.RemoveAt(i);
			}
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x00574AAC File Offset: 0x00572CAC
		private static void FindPlayerThatCanSeeBackgroundAmbience(out Player player)
		{
			player = null;
			int num = Main.player.Count((Player plr) => plr.active && AmbienceServer.IsPlayerInAPlaceWhereTheyCanSeeAmbience(plr));
			if (num == 0)
			{
				return;
			}
			player = (from plr in Main.player
			where plr.active && AmbienceServer.IsPlayerInAPlaceWhereTheyCanSeeAmbience(plr)
			select plr).ElementAt(Main.rand.Next(num));
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x00574B25 File Offset: 0x00572D25
		private static bool IsPlayerInAPlaceWhereTheyCanSeeAmbience(Player plr)
		{
			return AmbienceServer.IsPlayerInAPlaceWhereTheyCanSeeAmbienceSky(plr) || AmbienceServer.IsPlayerInAPlaceWhereTheyCanSeeAmbienceHell(plr);
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x00574B37 File Offset: 0x00572D37
		private static bool IsPlayerInAPlaceWhereTheyCanSeeAmbienceSky(Player plr)
		{
			return (double)plr.position.Y <= Main.worldSurface * 16.0 + 1600.0;
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x00574B63 File Offset: 0x00572D63
		private static bool IsPlayerInAPlaceWhereTheyCanSeeAmbienceHell(Player plr)
		{
			return plr.position.Y >= (float)((Main.UnderworldLayer - 100) * 16);
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x00574B81 File Offset: 0x00572D81
		private void SpawnForPlayer(Player player, SkyEntityType type)
		{
			NetManager.Instance.BroadcastOrLoopback(NetAmbienceModule.SerializeSkyEntitySpawn(player, type));
		}

		// Token: 0x0400514B RID: 20811
		private const int MINIMUM_SECONDS_BETWEEN_SPAWNS = 10;

		// Token: 0x0400514C RID: 20812
		private const int MAXIMUM_SECONDS_BETWEEN_SPAWNS = 120;

		// Token: 0x0400514D RID: 20813
		private readonly Dictionary<SkyEntityType, Func<bool>> _spawnConditions = new Dictionary<SkyEntityType, Func<bool>>();

		// Token: 0x0400514E RID: 20814
		private readonly Dictionary<SkyEntityType, Func<Player, bool>> _secondarySpawnConditionsPerPlayer = new Dictionary<SkyEntityType, Func<Player, bool>>();

		// Token: 0x0400514F RID: 20815
		private int _updatesUntilNextAttempt;

		// Token: 0x04005150 RID: 20816
		private List<AmbienceServer.AmbienceSpawnInfo> _forcedSpawns = new List<AmbienceServer.AmbienceSpawnInfo>();

		// Token: 0x020008C4 RID: 2244
		public struct AmbienceSpawnInfo
		{
			// Token: 0x040072FF RID: 29439
			public SkyEntityType skyEntityType;

			// Token: 0x04007300 RID: 29440
			public int targetPlayer;
		}
	}
}
