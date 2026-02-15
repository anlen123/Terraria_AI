using System;
using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000336 RID: 822
	public class NPCWasChatWithTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		// Token: 0x06002818 RID: 10264 RVA: 0x00571514 File Offset: 0x0056F714
		public NPCWasChatWithTracker()
		{
			this._chattedWithPlayer = new HashSet<string>();
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x00571534 File Offset: 0x0056F734
		public void RegisterChatStartWith(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			bool flag = !this._chattedWithPlayer.Contains(bestiaryCreditId);
			this.SetWasChatWithDirectly(bestiaryCreditId);
			if (Main.netMode == 2 && flag)
			{
				NetManager.Instance.Broadcast(NetBestiaryModule.SerializeChat(npc.netID), -1);
			}
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x00571584 File Offset: 0x0056F784
		public void SetWasChatWithDirectly(string persistentId)
		{
			object entryCreationLock = this._entryCreationLock;
			lock (entryCreationLock)
			{
				if (this._chattedWithPlayer.Add(persistentId))
				{
					AchievementsHelper.TryGrantingBestiary100PercentAchievement();
				}
			}
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x005715D4 File Offset: 0x0056F7D4
		public bool GetWasChatWith(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			return this._chattedWithPlayer.Contains(bestiaryCreditId);
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x005715F4 File Offset: 0x0056F7F4
		public bool GetWasChatWith(string persistentId)
		{
			return this._chattedWithPlayer.Contains(persistentId);
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x00571604 File Offset: 0x0056F804
		public void Save(BinaryWriter writer)
		{
			object entryCreationLock = this._entryCreationLock;
			lock (entryCreationLock)
			{
				writer.Write(this._chattedWithPlayer.Count);
				foreach (string value in this._chattedWithPlayer)
				{
					writer.Write(value);
				}
			}
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x00571690 File Offset: 0x0056F890
		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string item = reader.ReadString();
				this._chattedWithPlayer.Add(item);
			}
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x005716C4 File Offset: 0x0056F8C4
		public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				reader.ReadString();
			}
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x005716EB File Offset: 0x0056F8EB
		public void Reset()
		{
			this._chattedWithPlayer.Clear();
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x005716F8 File Offset: 0x0056F8F8
		public void OnPlayerJoining(int playerIndex)
		{
			foreach (string key in this._chattedWithPlayer)
			{
				int npcNetId;
				if (ContentSamples.NpcNetIdsByPersistentIds.TryGetValue(key, out npcNetId))
				{
					NetManager.Instance.SendToClient(NetBestiaryModule.SerializeChat(npcNetId), playerIndex);
				}
			}
		}

		// Token: 0x040050FD RID: 20733
		private object _entryCreationLock = new object();

		// Token: 0x040050FE RID: 20734
		private HashSet<string> _chattedWithPlayer;
	}
}
