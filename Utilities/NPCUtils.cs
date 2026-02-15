using System;
using Microsoft.Xna.Framework;

namespace Terraria.Utilities
{
	// Token: 0x020000CF RID: 207
	public static class NPCUtils
	{
		// Token: 0x06001817 RID: 6167 RVA: 0x004E0602 File Offset: 0x004DE802
		public static NPCUtils.TargetSearchResults SearchForTarget(Vector2 position, NPCUtils.TargetSearchFlag flags = NPCUtils.TargetSearchFlag.All, NPCUtils.SearchFilter<Player> playerFilter = null, NPCUtils.SearchFilter<NPC> npcFilter = null)
		{
			return NPCUtils.SearchForTarget(null, position, flags, playerFilter, npcFilter);
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x004E060E File Offset: 0x004DE80E
		public static NPCUtils.TargetSearchResults SearchForTarget(NPC searcher, NPCUtils.TargetSearchFlag flags = NPCUtils.TargetSearchFlag.All, NPCUtils.SearchFilter<Player> playerFilter = null, NPCUtils.SearchFilter<NPC> npcFilter = null)
		{
			return NPCUtils.SearchForTarget(searcher, searcher.Center, flags, playerFilter, npcFilter);
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x004E0620 File Offset: 0x004DE820
		public static NPCUtils.TargetSearchResults SearchForTarget(NPC searcher, Vector2 position, NPCUtils.TargetSearchFlag flags = NPCUtils.TargetSearchFlag.All, NPCUtils.SearchFilter<Player> playerFilter = null, NPCUtils.SearchFilter<NPC> npcFilter = null)
		{
			float num = float.MaxValue;
			int nearestNPCIndex = -1;
			float num2 = float.MaxValue;
			float nearestTankDistance = float.MaxValue;
			int nearestTankIndex = -1;
			NPCUtils.TargetType tankType = NPCUtils.TargetType.Player;
			if ((flags & NPCUtils.TargetSearchFlag.NPCs) != NPCUtils.TargetSearchFlag.None)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.active && npc.whoAmI != searcher.whoAmI && (npcFilter == null || npcFilter(npc)))
					{
						float num3 = Vector2.DistanceSquared(position, npc.Center);
						if (num3 < num)
						{
							nearestNPCIndex = i;
							num = num3;
						}
					}
				}
			}
			if ((flags & NPCUtils.TargetSearchFlag.Players) != NPCUtils.TargetSearchFlag.None)
			{
				for (int j = 0; j < 255; j++)
				{
					Player player = Main.player[j];
					if (player.active && !player.dead && !player.ghost && (playerFilter == null || playerFilter(player)))
					{
						float num4 = Vector2.Distance(position, player.Center);
						float num5 = num4 - (float)player.aggro;
						bool flag = searcher != null && player.npcTypeNoAggro[searcher.type];
						if (searcher != null && flag && searcher.direction == 0)
						{
							num5 += 1000f;
						}
						if (num5 < num2)
						{
							nearestTankIndex = j;
							num2 = num5;
							nearestTankDistance = num4;
							tankType = NPCUtils.TargetType.Player;
						}
						if (player.tankPet >= 0 && !flag)
						{
							Vector2 center = Main.projectile[player.tankPet].Center;
							num4 = Vector2.Distance(position, center);
							num5 = num4 - 200f;
							if (num5 < num2 && num5 < 200f && Collision.CanHit(position, 0, 0, center, 0, 0))
							{
								nearestTankIndex = j;
								num2 = num5;
								nearestTankDistance = num4;
								tankType = NPCUtils.TargetType.TankPet;
							}
						}
					}
				}
			}
			return new NPCUtils.TargetSearchResults(searcher, nearestNPCIndex, (float)Math.Sqrt((double)num), nearestTankIndex, nearestTankDistance, num2, tankType);
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x004E07E0 File Offset: 0x004DE9E0
		public static void TargetClosestOldOnesInvasion(NPC searcher, bool faceTarget = true, Vector2? checkPosition = null)
		{
			NPCUtils.TargetSearchResults targetSearchResults = NPCUtils.SearchForTarget(searcher, NPCUtils.TargetSearchFlag.All, NPCUtils.SearchFilters.OnlyPlayersInCertainDistance(searcher.Center, 200f), new NPCUtils.SearchFilter<NPC>(NPCUtils.SearchFilters.OnlyCrystal));
			if (!targetSearchResults.FoundTarget)
			{
				return;
			}
			searcher.target = targetSearchResults.NearestTargetIndex;
			searcher.targetRect = targetSearchResults.NearestTargetHitbox;
			if (searcher.ShouldFaceTarget(ref targetSearchResults, null) && faceTarget)
			{
				searcher.FaceTarget();
			}
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x004E0850 File Offset: 0x004DEA50
		public static void TargetClosestNonBees(NPC searcher, bool faceTarget = true, Vector2? checkPosition = null)
		{
			NPCUtils.TargetSearchResults targetSearchResults = NPCUtils.SearchForTarget(searcher, NPCUtils.TargetSearchFlag.All, null, new NPCUtils.SearchFilter<NPC>(NPCUtils.SearchFilters.NonBeeNPCs));
			if (!targetSearchResults.FoundTarget)
			{
				return;
			}
			searcher.target = targetSearchResults.NearestTargetIndex;
			searcher.targetRect = targetSearchResults.NearestTargetHitbox;
			if (searcher.ShouldFaceTarget(ref targetSearchResults, null) && faceTarget)
			{
				searcher.FaceTarget();
			}
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x004E08B4 File Offset: 0x004DEAB4
		public static void TargetClosestDownwindFromNPC(NPC searcher, float distanceMaxX, bool faceTarget = true, Vector2? checkPosition = null)
		{
			NPCUtils.TargetSearchResults targetSearchResults = NPCUtils.SearchForTarget(searcher, NPCUtils.TargetSearchFlag.Players, NPCUtils.SearchFilters.DownwindFromNPC(searcher, distanceMaxX), null);
			if (!targetSearchResults.FoundTarget)
			{
				return;
			}
			searcher.target = targetSearchResults.NearestTargetIndex;
			searcher.targetRect = targetSearchResults.NearestTargetHitbox;
			if (searcher.ShouldFaceTarget(ref targetSearchResults, null) && faceTarget)
			{
				searcher.FaceTarget();
			}
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x004E0910 File Offset: 0x004DEB10
		public static void TargetClosestCommon(NPC searcher, bool faceTarget = true, Vector2? checkPosition = null)
		{
			searcher.TargetClosest(faceTarget);
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x004E091C File Offset: 0x004DEB1C
		public static void TargetClosestBetsy(NPC searcher, bool faceTarget = true, Vector2? checkPosition = null)
		{
			NPCUtils.TargetSearchResults targetSearchResults = NPCUtils.SearchForTarget(searcher, NPCUtils.TargetSearchFlag.All, null, new NPCUtils.SearchFilter<NPC>(NPCUtils.SearchFilters.OnlyCrystal));
			if (!targetSearchResults.FoundTarget)
			{
				return;
			}
			NPCUtils.TargetType value = targetSearchResults.NearestTargetType;
			if (targetSearchResults.FoundTank && !targetSearchResults.NearestTankOwner.dead)
			{
				value = NPCUtils.TargetType.Player;
			}
			searcher.target = targetSearchResults.NearestTargetIndex;
			searcher.targetRect = targetSearchResults.NearestTargetHitbox;
			if (searcher.ShouldFaceTarget(ref targetSearchResults, new NPCUtils.TargetType?(value)) && faceTarget)
			{
				searcher.FaceTarget();
			}
		}

		// Token: 0x020006EF RID: 1775
		// (Invoke) Token: 0x06003F8B RID: 16267
		public delegate bool SearchFilter<T>(T entity) where T : Entity;

		// Token: 0x020006F0 RID: 1776
		// (Invoke) Token: 0x06003F8F RID: 16271
		public delegate void NPCTargetingMethod(NPC searcher, bool faceTarget, Vector2? checkPosition);

		// Token: 0x020006F1 RID: 1777
		public static class SearchFilters
		{
			// Token: 0x06003F92 RID: 16274 RVA: 0x00699B58 File Offset: 0x00697D58
			public static bool OnlyCrystal(NPC npc)
			{
				return npc.type == 548 && !npc.dontTakeDamageFromHostiles;
			}

			// Token: 0x06003F93 RID: 16275 RVA: 0x00699B72 File Offset: 0x00697D72
			public static NPCUtils.SearchFilter<Player> OnlyPlayersInCertainDistance(Vector2 position, float maxDistance)
			{
				return (Player player) => player.Distance(position) <= maxDistance;
			}

			// Token: 0x06003F94 RID: 16276 RVA: 0x00699B94 File Offset: 0x00697D94
			public static bool NonBeeNPCs(NPC npc)
			{
				return (npc.type != 1 || (npc.ai[1] != 1124f && npc.ai[1] != 1125f)) && npc.type != 211 && npc.type != 210 && npc.type != 222 && npc.CanBeChasedBy(null, false);
			}

			// Token: 0x06003F95 RID: 16277 RVA: 0x00699BF9 File Offset: 0x00697DF9
			public static NPCUtils.SearchFilter<Player> DownwindFromNPC(NPC npc, float maxDistanceX)
			{
				return delegate(Player player)
				{
					float windSpeedCurrent = Main.windSpeedCurrent;
					float num = player.Center.X - npc.Center.X;
					float num2 = Math.Abs(num);
					float num3 = Math.Abs(player.Center.Y - npc.Center.Y);
					return player.active && !player.dead && num3 < 100f && num2 < maxDistanceX && ((num > 0f && windSpeedCurrent > 0f) || (num < 0f && windSpeedCurrent < 0f));
				};
			}
		}

		// Token: 0x020006F2 RID: 1778
		public enum TargetType
		{
			// Token: 0x040067D4 RID: 26580
			None,
			// Token: 0x040067D5 RID: 26581
			NPC,
			// Token: 0x040067D6 RID: 26582
			Player,
			// Token: 0x040067D7 RID: 26583
			TankPet
		}

		// Token: 0x020006F3 RID: 1779
		public struct TargetSearchResults
		{
			// Token: 0x170004F3 RID: 1267
			// (get) Token: 0x06003F96 RID: 16278 RVA: 0x00699C1C File Offset: 0x00697E1C
			public int NearestTargetIndex
			{
				get
				{
					NPCUtils.TargetType nearestTargetType = this._nearestTargetType;
					if (nearestTargetType == NPCUtils.TargetType.NPC)
					{
						return this.NearestNPC.WhoAmIToTargettingIndex;
					}
					if (nearestTargetType - NPCUtils.TargetType.Player <= 1)
					{
						return this._nearestTankIndex;
					}
					return -1;
				}
			}

			// Token: 0x170004F4 RID: 1268
			// (get) Token: 0x06003F97 RID: 16279 RVA: 0x00699C50 File Offset: 0x00697E50
			public Rectangle NearestTargetHitbox
			{
				get
				{
					switch (this._nearestTargetType)
					{
					case NPCUtils.TargetType.NPC:
						return this.NearestNPC.Hitbox;
					case NPCUtils.TargetType.Player:
						return this.NearestTankOwner.Hitbox;
					case NPCUtils.TargetType.TankPet:
						return Main.projectile[this.NearestTankOwner.tankPet].Hitbox;
					default:
						return Rectangle.Empty;
					}
				}
			}

			// Token: 0x170004F5 RID: 1269
			// (get) Token: 0x06003F98 RID: 16280 RVA: 0x00699CAE File Offset: 0x00697EAE
			public NPCUtils.TargetType NearestTargetType
			{
				get
				{
					return this._nearestTargetType;
				}
			}

			// Token: 0x170004F6 RID: 1270
			// (get) Token: 0x06003F99 RID: 16281 RVA: 0x00699CB6 File Offset: 0x00697EB6
			public bool FoundTarget
			{
				get
				{
					return this._nearestTargetType > NPCUtils.TargetType.None;
				}
			}

			// Token: 0x170004F7 RID: 1271
			// (get) Token: 0x06003F9A RID: 16282 RVA: 0x00699CC1 File Offset: 0x00697EC1
			public NPC NearestNPC
			{
				get
				{
					if (this._nearestNPCIndex != -1)
					{
						return Main.npc[this._nearestNPCIndex];
					}
					return null;
				}
			}

			// Token: 0x170004F8 RID: 1272
			// (get) Token: 0x06003F9B RID: 16283 RVA: 0x00699CDA File Offset: 0x00697EDA
			public bool FoundNPC
			{
				get
				{
					return this._nearestNPCIndex != -1;
				}
			}

			// Token: 0x170004F9 RID: 1273
			// (get) Token: 0x06003F9C RID: 16284 RVA: 0x00699CE8 File Offset: 0x00697EE8
			public int NearestNPCIndex
			{
				get
				{
					return this._nearestNPCIndex;
				}
			}

			// Token: 0x170004FA RID: 1274
			// (get) Token: 0x06003F9D RID: 16285 RVA: 0x00699CF0 File Offset: 0x00697EF0
			public float NearestNPCDistance
			{
				get
				{
					return this._nearestNPCDistance;
				}
			}

			// Token: 0x170004FB RID: 1275
			// (get) Token: 0x06003F9E RID: 16286 RVA: 0x00699CF8 File Offset: 0x00697EF8
			public Player NearestTankOwner
			{
				get
				{
					if (this._nearestTankIndex != -1)
					{
						return Main.player[this._nearestTankIndex];
					}
					return null;
				}
			}

			// Token: 0x170004FC RID: 1276
			// (get) Token: 0x06003F9F RID: 16287 RVA: 0x00699D11 File Offset: 0x00697F11
			public bool FoundTank
			{
				get
				{
					return this._nearestTankIndex != -1;
				}
			}

			// Token: 0x170004FD RID: 1277
			// (get) Token: 0x06003FA0 RID: 16288 RVA: 0x00699D1F File Offset: 0x00697F1F
			public int NearestTankOwnerIndex
			{
				get
				{
					return this._nearestTankIndex;
				}
			}

			// Token: 0x170004FE RID: 1278
			// (get) Token: 0x06003FA1 RID: 16289 RVA: 0x00699D27 File Offset: 0x00697F27
			public float NearestTankDistance
			{
				get
				{
					return this._nearestTankDistance;
				}
			}

			// Token: 0x170004FF RID: 1279
			// (get) Token: 0x06003FA2 RID: 16290 RVA: 0x00699D2F File Offset: 0x00697F2F
			public float AdjustedTankDistance
			{
				get
				{
					return this._adjustedTankDistance;
				}
			}

			// Token: 0x17000500 RID: 1280
			// (get) Token: 0x06003FA3 RID: 16291 RVA: 0x00699D37 File Offset: 0x00697F37
			public NPCUtils.TargetType NearestTankType
			{
				get
				{
					return this._nearestTankType;
				}
			}

			// Token: 0x06003FA4 RID: 16292 RVA: 0x00699D40 File Offset: 0x00697F40
			public TargetSearchResults(NPC searcher, int nearestNPCIndex, float nearestNPCDistance, int nearestTankIndex, float nearestTankDistance, float adjustedTankDistance, NPCUtils.TargetType tankType)
			{
				this._nearestNPCIndex = nearestNPCIndex;
				this._nearestNPCDistance = nearestNPCDistance;
				this._nearestTankIndex = nearestTankIndex;
				this._adjustedTankDistance = adjustedTankDistance;
				this._nearestTankDistance = nearestTankDistance;
				this._nearestTankType = tankType;
				if (this._nearestNPCIndex != -1 && this._nearestTankIndex != -1)
				{
					if (this._nearestNPCDistance < this._adjustedTankDistance)
					{
						this._nearestTargetType = NPCUtils.TargetType.NPC;
						return;
					}
					this._nearestTargetType = tankType;
					return;
				}
				else
				{
					if (this._nearestNPCIndex != -1)
					{
						this._nearestTargetType = NPCUtils.TargetType.NPC;
						return;
					}
					if (this._nearestTankIndex != -1)
					{
						this._nearestTargetType = tankType;
						return;
					}
					this._nearestTargetType = NPCUtils.TargetType.None;
					return;
				}
			}

			// Token: 0x040067D8 RID: 26584
			private NPCUtils.TargetType _nearestTargetType;

			// Token: 0x040067D9 RID: 26585
			private int _nearestNPCIndex;

			// Token: 0x040067DA RID: 26586
			private float _nearestNPCDistance;

			// Token: 0x040067DB RID: 26587
			private int _nearestTankIndex;

			// Token: 0x040067DC RID: 26588
			private float _nearestTankDistance;

			// Token: 0x040067DD RID: 26589
			private float _adjustedTankDistance;

			// Token: 0x040067DE RID: 26590
			private NPCUtils.TargetType _nearestTankType;
		}

		// Token: 0x020006F4 RID: 1780
		[Flags]
		public enum TargetSearchFlag
		{
			// Token: 0x040067E0 RID: 26592
			None = 0,
			// Token: 0x040067E1 RID: 26593
			NPCs = 1,
			// Token: 0x040067E2 RID: 26594
			Players = 2,
			// Token: 0x040067E3 RID: 26595
			All = 3
		}
	}
}
