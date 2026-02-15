using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000335 RID: 821
	public class NPCWasNearPlayerTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		// Token: 0x0600280C RID: 10252 RVA: 0x00009E06 File Offset: 0x00008006
		public void PrepareSamplesBasedOptimizations()
		{
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x005711BC File Offset: 0x0056F3BC
		public NPCWasNearPlayerTracker()
		{
			this._wasNearPlayer = new HashSet<string>();
			this._playerHitboxesForBestiary = new List<Rectangle>();
			this._wasSeenNearPlayerByNetId = new List<int>();
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x005711F0 File Offset: 0x0056F3F0
		public void RegisterWasNearby(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			bool flag = !this._wasNearPlayer.Contains(bestiaryCreditId);
			this.SetWasSeenDirectly(bestiaryCreditId);
			if (Main.netMode == 2 && flag)
			{
				NetManager.Instance.Broadcast(NetBestiaryModule.SerializeSight(npc.netID), -1);
			}
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x00571240 File Offset: 0x0056F440
		public void SetWasSeenDirectly(string persistentId)
		{
			object entryCreationLock = this._entryCreationLock;
			lock (entryCreationLock)
			{
				if (this._wasNearPlayer.Add(persistentId))
				{
					AchievementsHelper.TryGrantingBestiary100PercentAchievement();
				}
			}
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x00571290 File Offset: 0x0056F490
		public bool GetWasNearbyBefore(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			return this.GetWasNearbyBefore(bestiaryCreditId);
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x005712AB File Offset: 0x0056F4AB
		public bool GetWasNearbyBefore(string persistentIdentifier)
		{
			return this._wasNearPlayer.Contains(persistentIdentifier);
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x005712BC File Offset: 0x0056F4BC
		public void Save(BinaryWriter writer)
		{
			object entryCreationLock = this._entryCreationLock;
			lock (entryCreationLock)
			{
				writer.Write(this._wasNearPlayer.Count);
				foreach (string value in this._wasNearPlayer)
				{
					writer.Write(value);
				}
			}
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x00571348 File Offset: 0x0056F548
		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string item = reader.ReadString();
				this._wasNearPlayer.Add(item);
			}
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x0057137C File Offset: 0x0056F57C
		public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				reader.ReadString();
			}
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x005713A3 File Offset: 0x0056F5A3
		public void Reset()
		{
			this._wasNearPlayer.Clear();
			this._playerHitboxesForBestiary.Clear();
			this._wasSeenNearPlayerByNetId.Clear();
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x005713C8 File Offset: 0x0056F5C8
		public void ScanWorldForFinds()
		{
			this._playerHitboxesForBestiary.Clear();
			for (int i = 0; i < 255; i++)
			{
				Player player = Main.player[i];
				if (player.active)
				{
					this._playerHitboxesForBestiary.Add(player.HitboxForBestiaryNearbyCheck);
				}
			}
			for (int j = 0; j < Main.maxNPCs; j++)
			{
				NPC npc = Main.npc[j];
				if (npc.active && npc.CountsAsACritter && !this._wasSeenNearPlayerByNetId.Contains(npc.netID))
				{
					Rectangle hitbox = npc.Hitbox;
					for (int k = 0; k < this._playerHitboxesForBestiary.Count; k++)
					{
						Rectangle value = this._playerHitboxesForBestiary[k];
						if (hitbox.Intersects(value))
						{
							this._wasSeenNearPlayerByNetId.Add(npc.netID);
							this.RegisterWasNearby(npc);
						}
					}
				}
			}
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x005714A8 File Offset: 0x0056F6A8
		public void OnPlayerJoining(int playerIndex)
		{
			foreach (string key in this._wasNearPlayer)
			{
				int npcNetId;
				if (ContentSamples.NpcNetIdsByPersistentIds.TryGetValue(key, out npcNetId))
				{
					NetManager.Instance.SendToClient(NetBestiaryModule.SerializeSight(npcNetId), playerIndex);
				}
			}
		}

		// Token: 0x040050F9 RID: 20729
		private object _entryCreationLock = new object();

		// Token: 0x040050FA RID: 20730
		private HashSet<string> _wasNearPlayer;

		// Token: 0x040050FB RID: 20731
		private List<Rectangle> _playerHitboxesForBestiary;

		// Token: 0x040050FC RID: 20732
		private List<int> _wasSeenNearPlayerByNetId;
	}
}
