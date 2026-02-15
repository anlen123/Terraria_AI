using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ID;

namespace Terraria.GameContent.Creative
{
	// Token: 0x0200032B RID: 811
	public class ItemsSacrificedUnlocksTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060027C3 RID: 10179 RVA: 0x00568769 File Offset: 0x00566969
		// (set) Token: 0x060027C4 RID: 10180 RVA: 0x00568771 File Offset: 0x00566971
		public int LastEditId { get; private set; }

		// Token: 0x060027C5 RID: 10181 RVA: 0x0056877A File Offset: 0x0056697A
		public void DismissNewlyUnlockedFromTeamMatesIcon()
		{
			this.AnyNewUnlocksFromTeammates = false;
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x00568783 File Offset: 0x00566983
		public ItemsSacrificedUnlocksTracker()
		{
			this._sacrificeCountByItemPersistentId = new Dictionary<string, int>();
			this._sacrificesCountByItemIdCache = new Dictionary<int, int>();
			this._unlockedByTeammate = new Dictionary<int, string>();
			this._newlyUnlocked = new HashSet<int>();
			this.LastEditId = 0;
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x005687C0 File Offset: 0x005669C0
		public int GetSacrificeCount(int itemId)
		{
			int num;
			if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(itemId, out num))
			{
				itemId = num;
			}
			int result;
			this._sacrificesCountByItemIdCache.TryGetValue(itemId, out result);
			return result;
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x005687F0 File Offset: 0x005669F0
		public void ForEachItemWithResearchProgress(Action<int> action)
		{
			foreach (KeyValuePair<int, int> keyValuePair in this._sacrificesCountByItemIdCache)
			{
				if (keyValuePair.Value > 0)
				{
					action(keyValuePair.Key);
				}
			}
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x00568854 File Offset: 0x00566A54
		public void CountFullyResearchedItems(out int fullyResearchedItems, out int allItems)
		{
			fullyResearchedItems = 0;
			allItems = 0;
			for (int i = 0; i < (int)ItemID.Count; i++)
			{
				int num;
				int num2;
				if (this.TryGetSacrificeNumbers(i, out num, out num2))
				{
					allItems++;
					if (num >= num2)
					{
						fullyResearchedItems++;
					}
				}
			}
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x00568894 File Offset: 0x00566A94
		public bool TryGetSacrificeNumbers(int itemId, out int amountWeHave, out int amountNeededTotal)
		{
			int num;
			if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(itemId, out num))
			{
				itemId = num;
			}
			amountWeHave = (amountNeededTotal = 0);
			if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(itemId, out amountNeededTotal))
			{
				return false;
			}
			this._sacrificesCountByItemIdCache.TryGetValue(itemId, out amountWeHave);
			return true;
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x005688DC File Offset: 0x00566ADC
		public bool IsFullyResearched(int itemId)
		{
			int num;
			int num2;
			return this.TryGetSacrificeNumbers(itemId, out num, out num2) && num >= num2;
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x005688FF File Offset: 0x00566AFF
		public bool IsNewlyResearched(int itemId)
		{
			return this._newlyUnlocked.Contains(itemId);
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x0056890D File Offset: 0x00566B0D
		public void ClearNewlyResearchedStatus(int itemId)
		{
			this._newlyUnlocked.Remove(itemId);
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x0056891C File Offset: 0x00566B1C
		public bool TryGetTeammateUnlockCredit(int itemId, out string teammateName)
		{
			return this._unlockedByTeammate.TryGetValue(itemId, out teammateName);
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x0056892C File Offset: 0x00566B2C
		public void RegisterItemSacrifice(int itemId, int amount, string teammateName = null)
		{
			int num;
			if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(itemId, out num))
			{
				itemId = num;
			}
			string key;
			if (!ContentSamples.ItemPersistentIdsByNetIds.TryGetValue(itemId, out key))
			{
				return;
			}
			int num2;
			if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(itemId, out num2))
			{
				return;
			}
			int num3;
			this._sacrificeCountByItemPersistentId.TryGetValue(key, out num3);
			if (num3 >= num2)
			{
				return;
			}
			num3 = Math.Min(num3 + amount, num2);
			this._sacrificeCountByItemPersistentId[key] = num3;
			this._sacrificesCountByItemIdCache[itemId] = num3;
			this.MarkContentsDirty();
			if (num3 >= num2)
			{
				this._newlyUnlocked.Add(itemId);
				if (teammateName != null)
				{
					this.AnyNewUnlocksFromTeammates = true;
					this._unlockedByTeammate[itemId] = teammateName;
				}
			}
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x005689D4 File Offset: 0x00566BD4
		public void SetSacrificeCountDirectly(string persistentId, int sacrificeCount)
		{
			int value = Utils.Clamp<int>(sacrificeCount, 0, 9999);
			this._sacrificeCountByItemPersistentId[persistentId] = value;
			int key;
			if (!ContentSamples.ItemNetIdsByPersistentIds.TryGetValue(persistentId, out key))
			{
				return;
			}
			this._sacrificesCountByItemIdCache[key] = value;
			this.MarkContentsDirty();
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x00568A20 File Offset: 0x00566C20
		public void Save(BinaryWriter writer)
		{
			writer.Write(false);
			Dictionary<string, int> dictionary = new Dictionary<string, int>(this._sacrificeCountByItemPersistentId);
			writer.Write(dictionary.Count);
			foreach (KeyValuePair<string, int> keyValuePair in dictionary)
			{
				writer.Write(keyValuePair.Key);
				writer.Write(keyValuePair.Value);
			}
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x00568AA0 File Offset: 0x00566CA0
		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			if (gameVersionSaveWasMadeOn >= 282)
			{
				reader.ReadBoolean();
			}
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string key = reader.ReadString();
				int value = reader.ReadInt32();
				int key2;
				if (ContentSamples.ItemNetIdsByPersistentIds.TryGetValue(key, out key2))
				{
					int num2;
					if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(key2, out num2))
					{
						key2 = num2;
					}
					this._sacrificesCountByItemIdCache[key2] = value;
					string text;
					if (ContentSamples.ItemPersistentIdsByNetIds.TryGetValue(key2, out text))
					{
						key = text;
					}
				}
				this._sacrificeCountByItemPersistentId[key] = value;
			}
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x00568B30 File Offset: 0x00566D30
		public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				reader.ReadString();
				reader.ReadInt32();
			}
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x00568B5E File Offset: 0x00566D5E
		public void Reset()
		{
			this._sacrificeCountByItemPersistentId.Clear();
			this._sacrificesCountByItemIdCache.Clear();
			this.AnyNewUnlocksFromTeammates = false;
			this._unlockedByTeammate.Clear();
			this._newlyUnlocked.Clear();
			this.MarkContentsDirty();
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x00009E06 File Offset: 0x00008006
		public void OnPlayerJoining(int playerIndex)
		{
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x00568B9C File Offset: 0x00566D9C
		public void MarkContentsDirty()
		{
			int lastEditId = this.LastEditId;
			this.LastEditId = lastEditId + 1;
		}

		// Token: 0x040050E4 RID: 20708
		public const int POSITIVE_SACRIFICE_COUNT_CAP = 9999;

		// Token: 0x040050E5 RID: 20709
		private Dictionary<string, int> _sacrificeCountByItemPersistentId;

		// Token: 0x040050E6 RID: 20710
		private Dictionary<int, int> _sacrificesCountByItemIdCache;

		// Token: 0x040050E7 RID: 20711
		private Dictionary<int, string> _unlockedByTeammate;

		// Token: 0x040050E8 RID: 20712
		private HashSet<int> _newlyUnlocked;

		// Token: 0x040050E9 RID: 20713
		public bool AnyNewUnlocksFromTeammates;
	}
}
