using System;
using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000334 RID: 820
	public class NPCKillsTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		// Token: 0x06002802 RID: 10242 RVA: 0x00570F14 File Offset: 0x0056F114
		public NPCKillsTracker()
		{
			this._killCountsByNpcId = new Dictionary<string, int>();
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x00570F34 File Offset: 0x0056F134
		public void RegisterKill(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			int num;
			this._killCountsByNpcId.TryGetValue(bestiaryCreditId, out num);
			num++;
			this.SetKillCountDirectly(bestiaryCreditId, num);
			if (Main.netMode == 2)
			{
				NetManager.Instance.Broadcast(NetBestiaryModule.SerializeKillCount(npc.netID, num), -1);
			}
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x00570F84 File Offset: 0x0056F184
		public int GetKillCount(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			return this.GetKillCount(bestiaryCreditId);
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x00570FA0 File Offset: 0x0056F1A0
		public void SetKillCountDirectly(string persistentId, int killCount)
		{
			object entryCreationLock = this._entryCreationLock;
			lock (entryCreationLock)
			{
				bool flag2 = this._killCountsByNpcId.ContainsKey(persistentId);
				this._killCountsByNpcId[persistentId] = Utils.Clamp<int>(killCount, 0, 999999999);
				if (!flag2)
				{
					AchievementsHelper.TryGrantingBestiary100PercentAchievement();
				}
			}
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x00571008 File Offset: 0x0056F208
		public int GetKillCount(string persistentId)
		{
			int result;
			this._killCountsByNpcId.TryGetValue(persistentId, out result);
			return result;
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x00571028 File Offset: 0x0056F228
		public void Save(BinaryWriter writer)
		{
			Dictionary<string, int> killCountsByNpcId = this._killCountsByNpcId;
			lock (killCountsByNpcId)
			{
				writer.Write(this._killCountsByNpcId.Count);
				foreach (KeyValuePair<string, int> keyValuePair in this._killCountsByNpcId)
				{
					writer.Write(keyValuePair.Key);
					writer.Write(keyValuePair.Value);
				}
			}
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x005710C8 File Offset: 0x0056F2C8
		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string key = reader.ReadString();
				int value = reader.ReadInt32();
				this._killCountsByNpcId[key] = value;
			}
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x00571104 File Offset: 0x0056F304
		public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				reader.ReadString();
				reader.ReadInt32();
			}
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x00571132 File Offset: 0x0056F332
		public void Reset()
		{
			this._killCountsByNpcId.Clear();
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x00571140 File Offset: 0x0056F340
		public void OnPlayerJoining(int playerIndex)
		{
			foreach (KeyValuePair<string, int> keyValuePair in this._killCountsByNpcId)
			{
				int npcNetId;
				if (ContentSamples.NpcNetIdsByPersistentIds.TryGetValue(keyValuePair.Key, out npcNetId))
				{
					NetManager.Instance.SendToClient(NetBestiaryModule.SerializeKillCount(npcNetId, keyValuePair.Value), playerIndex);
				}
			}
		}

		// Token: 0x040050F6 RID: 20726
		private object _entryCreationLock = new object();

		// Token: 0x040050F7 RID: 20727
		public const int POSITIVE_KILL_COUNT_CAP = 999999999;

		// Token: 0x040050F8 RID: 20728
		private Dictionary<string, int> _killCountsByNpcId;
	}
}
