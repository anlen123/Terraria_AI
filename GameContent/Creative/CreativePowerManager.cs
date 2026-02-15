using System;
using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.NetModules;
using Terraria.Net;

namespace Terraria.GameContent.Creative
{
	// Token: 0x0200031C RID: 796
	public class CreativePowerManager
	{
		// Token: 0x06002775 RID: 10101 RVA: 0x005674E3 File Offset: 0x005656E3
		private CreativePowerManager()
		{
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x00567504 File Offset: 0x00565704
		public void Register<T>(string nameInServerConfig) where T : ICreativePower, new()
		{
			T t = Activator.CreateInstance<T>();
			CreativePowerManager.PowerTypeStorage<T>.Power = t;
			CreativePowerManager.PowerTypeStorage<T>.Id = this._powersCount;
			CreativePowerManager.PowerTypeStorage<T>.Name = nameInServerConfig;
			t.DefaultPermissionLevel = PowerPermissionLevel.CanBeChangedByEveryone;
			t.CurrentPermissionLevel = PowerPermissionLevel.CanBeChangedByEveryone;
			this._powersById[this._powersCount] = t;
			this._powersByName[nameInServerConfig] = t;
			t.PowerId = this._powersCount;
			t.ServerConfigName = nameInServerConfig;
			this._powersCount += 1;
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x005675A3 File Offset: 0x005657A3
		public T GetPower<T>() where T : ICreativePower
		{
			return CreativePowerManager.PowerTypeStorage<T>.Power;
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x005675AA File Offset: 0x005657AA
		public ushort GetPowerId<T>() where T : ICreativePower
		{
			return CreativePowerManager.PowerTypeStorage<T>.Id;
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x005675B1 File Offset: 0x005657B1
		public bool TryGetPower(ushort id, out ICreativePower power)
		{
			return this._powersById.TryGetValue(id, out power);
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x005675C0 File Offset: 0x005657C0
		public static void TryListingPermissionsFrom(string line)
		{
			int length = "journeypermission_".Length;
			if (line.Length < length)
			{
				return;
			}
			if (!line.ToLower().StartsWith("journeypermission_"))
			{
				return;
			}
			string[] array = line.Substring(length).Split(new char[]
			{
				'='
			});
			if (array.Length != 2)
			{
				return;
			}
			int value;
			if (!int.TryParse(array[1].Trim(), out value))
			{
				return;
			}
			PowerPermissionLevel powerPermissionLevel = (PowerPermissionLevel)Utils.Clamp<int>(value, 0, 2);
			string key = array[0].Trim().ToLower();
			CreativePowerManager.Initialize();
			ICreativePower creativePower;
			if (!CreativePowerManager.Instance._powersByName.TryGetValue(key, out creativePower))
			{
				return;
			}
			creativePower.DefaultPermissionLevel = powerPermissionLevel;
			creativePower.CurrentPermissionLevel = powerPermissionLevel;
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x0056766C File Offset: 0x0056586C
		public static void Initialize()
		{
			if (CreativePowerManager._initialized)
			{
				return;
			}
			CreativePowerManager.Instance.Register<CreativePowers.FreezeTime>("time_setfrozen");
			CreativePowerManager.Instance.Register<CreativePowers.StartDayImmediately>("time_setdawn");
			CreativePowerManager.Instance.Register<CreativePowers.StartNoonImmediately>("time_setnoon");
			CreativePowerManager.Instance.Register<CreativePowers.StartNightImmediately>("time_setdusk");
			CreativePowerManager.Instance.Register<CreativePowers.StartMidnightImmediately>("time_setmidnight");
			CreativePowerManager.Instance.Register<CreativePowers.GodmodePower>("godmode");
			CreativePowerManager.Instance.Register<CreativePowers.ModifyWindDirectionAndStrength>("wind_setstrength");
			CreativePowerManager.Instance.Register<CreativePowers.ModifyRainPower>("rain_setstrength");
			CreativePowerManager.Instance.Register<CreativePowers.ModifyTimeRate>("time_setspeed");
			CreativePowerManager.Instance.Register<CreativePowers.FreezeRainPower>("rain_setfrozen");
			CreativePowerManager.Instance.Register<CreativePowers.FreezeWindDirectionAndStrength>("wind_setfrozen");
			CreativePowerManager.Instance.Register<CreativePowers.FarPlacementRangePower>("increaseplacementrange");
			CreativePowerManager.Instance.Register<CreativePowers.DifficultySliderPower>("setdifficulty");
			CreativePowerManager.Instance.Register<CreativePowers.StopBiomeSpreadPower>("biomespread_setfrozen");
			CreativePowerManager.Instance.Register<CreativePowers.SpawnRateSliderPerPlayerPower>("setspawnrate");
			CreativePowerManager._initialized = true;
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x00567768 File Offset: 0x00565968
		public void Reset()
		{
			foreach (KeyValuePair<ushort, ICreativePower> keyValuePair in this._powersById)
			{
				keyValuePair.Value.CurrentPermissionLevel = keyValuePair.Value.DefaultPermissionLevel;
				IPersistentPerWorldContent persistentPerWorldContent = keyValuePair.Value as IPersistentPerWorldContent;
				if (persistentPerWorldContent != null)
				{
					persistentPerWorldContent.Reset();
				}
				IPersistentPerPlayerContent persistentPerPlayerContent = keyValuePair.Value as IPersistentPerPlayerContent;
				if (persistentPerPlayerContent != null)
				{
					persistentPerPlayerContent.Reset();
				}
			}
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x005677F8 File Offset: 0x005659F8
		public void SaveToWorld(BinaryWriter writer)
		{
			foreach (KeyValuePair<ushort, ICreativePower> keyValuePair in this._powersById)
			{
				IPersistentPerWorldContent persistentPerWorldContent = keyValuePair.Value as IPersistentPerWorldContent;
				if (persistentPerWorldContent != null)
				{
					writer.Write(true);
					writer.Write(keyValuePair.Key);
					persistentPerWorldContent.Save(writer);
				}
			}
			writer.Write(false);
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x00567878 File Offset: 0x00565A78
		public void LoadFromWorld(BinaryReader reader, int versionGameWasLastSavedOn)
		{
			while (reader.ReadBoolean())
			{
				ushort key = reader.ReadUInt16();
				ICreativePower creativePower;
				if (!this._powersById.TryGetValue(key, out creativePower))
				{
					break;
				}
				IPersistentPerWorldContent persistentPerWorldContent = creativePower as IPersistentPerWorldContent;
				if (persistentPerWorldContent == null)
				{
					break;
				}
				persistentPerWorldContent.Load(reader, versionGameWasLastSavedOn);
			}
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x005678B8 File Offset: 0x00565AB8
		public void ValidateWorld(BinaryReader reader, int versionGameWasLastSavedOn)
		{
			while (reader.ReadBoolean())
			{
				ushort key = reader.ReadUInt16();
				ICreativePower creativePower;
				if (!this._powersById.TryGetValue(key, out creativePower))
				{
					break;
				}
				IPersistentPerWorldContent persistentPerWorldContent = creativePower as IPersistentPerWorldContent;
				if (persistentPerWorldContent == null)
				{
					break;
				}
				persistentPerWorldContent.ValidateWorld(reader, versionGameWasLastSavedOn);
			}
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x005678F8 File Offset: 0x00565AF8
		public void SyncThingsToJoiningPlayer(int playerIndex)
		{
			foreach (KeyValuePair<ushort, ICreativePower> keyValuePair in this._powersById)
			{
				NetPacket packet = NetCreativePowerPermissionsModule.SerializeCurrentPowerPermissionLevel(keyValuePair.Key, (int)keyValuePair.Value.CurrentPermissionLevel);
				NetManager.Instance.SendToClient(packet, playerIndex);
			}
			foreach (KeyValuePair<ushort, ICreativePower> keyValuePair2 in this._powersById)
			{
				IOnPlayerJoining onPlayerJoining = keyValuePair2.Value as IOnPlayerJoining;
				if (onPlayerJoining != null)
				{
					onPlayerJoining.OnPlayerJoining(playerIndex);
				}
			}
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x005679C0 File Offset: 0x00565BC0
		public void SaveToPlayer(Player player, BinaryWriter writer)
		{
			foreach (KeyValuePair<ushort, ICreativePower> keyValuePair in this._powersById)
			{
				IPersistentPerPlayerContent persistentPerPlayerContent = keyValuePair.Value as IPersistentPerPlayerContent;
				if (persistentPerPlayerContent != null)
				{
					writer.Write(true);
					writer.Write(keyValuePair.Key);
					persistentPerPlayerContent.Save(player, writer);
				}
			}
			writer.Write(false);
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x00567A40 File Offset: 0x00565C40
		public void LoadToPlayer(Player player, BinaryReader reader, int versionGameWasLastSavedOn)
		{
			while (reader.ReadBoolean())
			{
				ushort key = reader.ReadUInt16();
				ICreativePower creativePower;
				if (!this._powersById.TryGetValue(key, out creativePower))
				{
					break;
				}
				IPersistentPerPlayerContent persistentPerPlayerContent = creativePower as IPersistentPerPlayerContent;
				if (persistentPerPlayerContent != null)
				{
					persistentPerPlayerContent.Load(player, reader, versionGameWasLastSavedOn);
				}
			}
			if (player.difficulty != 3)
			{
				this.ResetPowersForPlayer(player);
			}
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x00567A94 File Offset: 0x00565C94
		public void ApplyLoadedDataToPlayer(Player player)
		{
			foreach (KeyValuePair<ushort, ICreativePower> keyValuePair in this._powersById)
			{
				IPersistentPerPlayerContent persistentPerPlayerContent = keyValuePair.Value as IPersistentPerPlayerContent;
				if (persistentPerPlayerContent != null)
				{
					persistentPerPlayerContent.ApplyLoadedDataToOutOfPlayerFields(player);
				}
			}
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x00567AF8 File Offset: 0x00565CF8
		public void ResetPowersForPlayer(Player player)
		{
			foreach (KeyValuePair<ushort, ICreativePower> keyValuePair in this._powersById)
			{
				IPersistentPerPlayerContent persistentPerPlayerContent = keyValuePair.Value as IPersistentPerPlayerContent;
				if (persistentPerPlayerContent != null)
				{
					persistentPerPlayerContent.ResetDataForNewPlayer(player);
				}
			}
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x00567B5C File Offset: 0x00565D5C
		public void ResetDataForNewPlayer(Player player)
		{
			foreach (KeyValuePair<ushort, ICreativePower> keyValuePair in this._powersById)
			{
				IPersistentPerPlayerContent persistentPerPlayerContent = keyValuePair.Value as IPersistentPerPlayerContent;
				if (persistentPerPlayerContent != null)
				{
					persistentPerPlayerContent.Reset();
					persistentPerPlayerContent.ResetDataForNewPlayer(player);
				}
			}
		}

		// Token: 0x040050C5 RID: 20677
		public static readonly CreativePowerManager Instance = new CreativePowerManager();

		// Token: 0x040050C6 RID: 20678
		private Dictionary<ushort, ICreativePower> _powersById = new Dictionary<ushort, ICreativePower>();

		// Token: 0x040050C7 RID: 20679
		private Dictionary<string, ICreativePower> _powersByName = new Dictionary<string, ICreativePower>();

		// Token: 0x040050C8 RID: 20680
		private ushort _powersCount;

		// Token: 0x040050C9 RID: 20681
		private static bool _initialized = false;

		// Token: 0x040050CA RID: 20682
		private const string _powerPermissionsLineHeader = "journeypermission_";

		// Token: 0x0200087B RID: 2171
		private class PowerTypeStorage<T> where T : ICreativePower
		{
			// Token: 0x04007252 RID: 29266
			public static ushort Id;

			// Token: 0x04007253 RID: 29267
			public static string Name;

			// Token: 0x04007254 RID: 29268
			public static T Power;
		}
	}
}
