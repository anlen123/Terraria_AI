using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent
{
	// Token: 0x02000277 RID: 631
	public class CoinLossRevengeSystem
	{
		// Token: 0x0600242F RID: 9263 RVA: 0x0054B184 File Offset: 0x00549384
		public void AddMarkerFromReader(BinaryReader reader)
		{
			int uniqueID = reader.ReadInt32();
			Vector2 coords = reader.ReadVector2();
			int npcNetId = reader.ReadInt32();
			float npcHPPercent = reader.ReadSingle();
			int npcType = reader.ReadInt32();
			int npcAiStyle = reader.ReadInt32();
			int coinValue = reader.ReadInt32();
			float baseValue = reader.ReadSingle();
			bool spawnedFromStatue = reader.ReadBoolean();
			CoinLossRevengeSystem.RevengeMarker marker = new CoinLossRevengeSystem.RevengeMarker(coords, npcNetId, npcHPPercent, npcType, npcAiStyle, coinValue, baseValue, spawnedFromStatue, this._gameTime, uniqueID);
			this.AddMarker(marker);
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x0054B1F4 File Offset: 0x005493F4
		private void AddMarker(CoinLossRevengeSystem.RevengeMarker marker)
		{
			object markersLock = this._markersLock;
			lock (markersLock)
			{
				this._markers.Add(marker);
			}
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x0054B23C File Offset: 0x0054943C
		public void DestroyMarker(int markerUniqueID)
		{
			object markersLock = this._markersLock;
			lock (markersLock)
			{
				this._markers.RemoveAll((CoinLossRevengeSystem.RevengeMarker x) => x.UniqueID == markerUniqueID);
			}
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x0054B29C File Offset: 0x0054949C
		public CoinLossRevengeSystem()
		{
			this._markers = new List<CoinLossRevengeSystem.RevengeMarker>();
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x0054B2BC File Offset: 0x005494BC
		public void CacheEnemy(NPC npc)
		{
			if (npc.boss || (npc.realLife != -1 && npc.realLife != npc.whoAmI) || npc.rarity > 0 || npc.extraValue < CoinLossRevengeSystem.MinimumCoinsForCaching)
			{
				return;
			}
			if (npc.position.X < Main.leftWorld + 640f + 16f || npc.position.X + (float)npc.width > Main.rightWorld - 640f - 32f || npc.position.Y < Main.topWorld + 640f + 16f || npc.position.Y > Main.bottomWorld - 640f - 32f - (float)npc.height)
			{
				return;
			}
			int num = npc.netID;
			int num2;
			if (NPCID.Sets.RespawnEnemyID.TryGetValue(num, out num2))
			{
				num = num2;
			}
			if (num == 0)
			{
				return;
			}
			CoinLossRevengeSystem.RevengeMarker marker = new CoinLossRevengeSystem.RevengeMarker(npc.Center, num, npc.GetLifePercent(), npc.type, npc.aiStyle, npc.extraValue, npc.value, npc.SpawnedFromStatue, this._gameTime, -1);
			this.AddMarker(marker);
			if (Main.netMode == 2)
			{
				NetMessage.SendCoinLossRevengeMarker(marker, -1, -1);
			}
			if (CoinLossRevengeSystem.DisplayCaching)
			{
				Main.NewText("Cached " + npc.GivenOrTypeName, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x0054B420 File Offset: 0x00549620
		public void Reset()
		{
			object markersLock = this._markersLock;
			lock (markersLock)
			{
				this._markers.Clear();
			}
			this._gameTime = 0;
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x0054B46C File Offset: 0x0054966C
		public void Update()
		{
			this._gameTime++;
			if (Main.netMode == 1 && this._gameTime % 60 == 0)
			{
				this.RemoveExpiredOrInvalidMarkers();
			}
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x0054B498 File Offset: 0x00549698
		public void CheckRespawns()
		{
			object markersLock = this._markersLock;
			lock (markersLock)
			{
				if (this._markers.Count == 0)
				{
					return;
				}
			}
			List<Tuple<int, Rectangle, Rectangle>> list = new List<Tuple<int, Rectangle, Rectangle>>();
			for (int i = 0; i < 255; i++)
			{
				Player player = Main.player[i];
				if (player.active && !player.dead)
				{
					list.Add(Tuple.Create<int, Rectangle, Rectangle>(i, Utils.CenteredRectangle(player.Center, CoinLossRevengeSystem._playerBoxSizeInner), Utils.CenteredRectangle(player.Center, CoinLossRevengeSystem._playerBoxSizeOuter)));
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			this.RemoveExpiredOrInvalidMarkers();
			markersLock = this._markersLock;
			lock (markersLock)
			{
				List<CoinLossRevengeSystem.RevengeMarker> list2 = new List<CoinLossRevengeSystem.RevengeMarker>();
				for (int j = 0; j < this._markers.Count; j++)
				{
					CoinLossRevengeSystem.RevengeMarker revengeMarker = this._markers[j];
					bool flag2 = false;
					Tuple<int, Rectangle, Rectangle> tuple = null;
					foreach (Tuple<int, Rectangle, Rectangle> tuple2 in list)
					{
						if (revengeMarker.Intersects(tuple2.Item2, tuple2.Item3))
						{
							tuple = tuple2;
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						revengeMarker.SetRespawnAttemptLock(false);
					}
					else if (!revengeMarker.RespawnAttemptLocked)
					{
						revengeMarker.SetRespawnAttemptLock(true);
						if (revengeMarker.WouldNPCBeDiscouraged(Main.player[tuple.Item1]))
						{
							revengeMarker.SetToExpire();
						}
						else
						{
							revengeMarker.SpawnEnemy();
							list2.Add(revengeMarker);
							if (Main.dedServ)
							{
								NetMessage.SendData(127, -1, -1, null, revengeMarker.UniqueID, 0f, 0f, 0f, 0, 0, 0);
							}
						}
					}
				}
				this._markers = this._markers.Except(list2).ToList<CoinLossRevengeSystem.RevengeMarker>();
			}
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x0054B6C4 File Offset: 0x005498C4
		private void RemoveExpiredOrInvalidMarkers()
		{
			object markersLock = this._markersLock;
			lock (markersLock)
			{
				IEnumerable<CoinLossRevengeSystem.RevengeMarker> enumerable = from x in this._markers
				where x.IsExpired(this._gameTime)
				select x;
				IEnumerable<CoinLossRevengeSystem.RevengeMarker> enumerable2 = from x in this._markers
				where x.IsInvalid()
				select x;
				this._markers.RemoveAll((CoinLossRevengeSystem.RevengeMarker x) => x.IsInvalid());
				this._markers.RemoveAll((CoinLossRevengeSystem.RevengeMarker x) => x.IsExpired(this._gameTime));
				if (Main.dedServ)
				{
					foreach (CoinLossRevengeSystem.RevengeMarker revengeMarker in enumerable)
					{
						NetMessage.SendData(127, -1, -1, null, revengeMarker.UniqueID, 0f, 0f, 0f, 0, 0, 0);
					}
					foreach (CoinLossRevengeSystem.RevengeMarker revengeMarker2 in enumerable2)
					{
						NetMessage.SendData(127, -1, -1, null, revengeMarker2.UniqueID, 0f, 0f, 0f, 0, 0, 0);
					}
				}
			}
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x0054B864 File Offset: 0x00549A64
		public CoinLossRevengeSystem.RevengeMarker DrawMapIcons(SpriteBatch spriteBatch, Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale, float drawScale, ref string unused)
		{
			CoinLossRevengeSystem.RevengeMarker result = null;
			object markersLock = this._markersLock;
			lock (markersLock)
			{
				foreach (CoinLossRevengeSystem.RevengeMarker revengeMarker in this._markers)
				{
					if (revengeMarker.DrawMapIcon(spriteBatch, mapTopLeft, mapX2Y2AndOff, mapRect, mapScale, drawScale, this._gameTime))
					{
						result = revengeMarker;
					}
				}
			}
			return result;
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x0054B8F8 File Offset: 0x00549AF8
		public void SendAllMarkersToPlayer(int plr)
		{
			object markersLock = this._markersLock;
			lock (markersLock)
			{
				foreach (CoinLossRevengeSystem.RevengeMarker marker in this._markers)
				{
					NetMessage.SendCoinLossRevengeMarker(marker, plr, -1);
				}
			}
		}

		// Token: 0x04004DC5 RID: 19909
		public static bool DisplayCaching = false;

		// Token: 0x04004DC6 RID: 19910
		public static int MinimumCoinsForCaching = Item.buyPrice(0, 0, 10, 0);

		// Token: 0x04004DC7 RID: 19911
		private const int PLAYER_BOX_WIDTH_INNER = 1968;

		// Token: 0x04004DC8 RID: 19912
		private const int PLAYER_BOX_HEIGHT_INNER = 1200;

		// Token: 0x04004DC9 RID: 19913
		private const int PLAYER_BOX_WIDTH_OUTER = 2608;

		// Token: 0x04004DCA RID: 19914
		private const int PLAYER_BOX_HEIGHT_OUTER = 1840;

		// Token: 0x04004DCB RID: 19915
		private static readonly Vector2 _playerBoxSizeInner = new Vector2(1968f, 1200f);

		// Token: 0x04004DCC RID: 19916
		private static readonly Vector2 _playerBoxSizeOuter = new Vector2(2608f, 1840f);

		// Token: 0x04004DCD RID: 19917
		private List<CoinLossRevengeSystem.RevengeMarker> _markers;

		// Token: 0x04004DCE RID: 19918
		private readonly object _markersLock = new object();

		// Token: 0x04004DCF RID: 19919
		private int _gameTime;

		// Token: 0x020007F5 RID: 2037
		public class RevengeMarker
		{
			// Token: 0x0600428E RID: 17038 RVA: 0x006BDACF File Offset: 0x006BBCCF
			public void SetToExpire()
			{
				this._forceExpire = true;
			}

			// Token: 0x17000539 RID: 1337
			// (get) Token: 0x0600428F RID: 17039 RVA: 0x006BDAD8 File Offset: 0x006BBCD8
			public bool RespawnAttemptLocked
			{
				get
				{
					return this._attemptedRespawn;
				}
			}

			// Token: 0x06004290 RID: 17040 RVA: 0x006BDAE0 File Offset: 0x006BBCE0
			public void SetRespawnAttemptLock(bool state)
			{
				this._attemptedRespawn = state;
			}

			// Token: 0x06004291 RID: 17041 RVA: 0x006BDAEC File Offset: 0x006BBCEC
			public RevengeMarker(Vector2 coords, int npcNetId, float npcHPPercent, int npcType, int npcAiStyle, int coinValue, float baseValue, bool spawnedFromStatue, int gameTime, int uniqueID = -1)
			{
				this._location = coords;
				this._npcNetID = npcNetId;
				this._npcHPPercent = npcHPPercent;
				this._npcTypeAgainstDiscouragement = npcType;
				this._npcAIStyleAgainstDiscouragement = npcAiStyle;
				this._coinsValue = coinValue;
				this._baseValue = baseValue;
				this._spawnedFromStatue = spawnedFromStatue;
				this._hitbox = Utils.CenteredRectangle(this._location, CoinLossRevengeSystem.RevengeMarker.EnemyBoxSize);
				this._expirationTime = this.CalculateExpirationTime(gameTime, coinValue);
				if (uniqueID == -1)
				{
					this._uniqueID = CoinLossRevengeSystem.RevengeMarker._uniqueIDCounter++;
					return;
				}
				this._uniqueID = uniqueID;
			}

			// Token: 0x06004292 RID: 17042 RVA: 0x006BDB84 File Offset: 0x006BBD84
			public bool IsInvalid()
			{
				int npcinvasionGroup = NPC.GetNPCInvasionGroup(this._npcTypeAgainstDiscouragement);
				switch (npcinvasionGroup)
				{
				case -3:
					return !DD2Event.Ongoing;
				case -2:
					return !Main.pumpkinMoon || Main.dayTime;
				case -1:
					return !Main.snowMoon || Main.dayTime;
				case 1:
				case 2:
				case 3:
				case 4:
					return npcinvasionGroup != Main.invasionType;
				}
				int npcTypeAgainstDiscouragement = this._npcTypeAgainstDiscouragement;
				if (npcTypeAgainstDiscouragement <= 166)
				{
					if (npcTypeAgainstDiscouragement - 158 > 1 && npcTypeAgainstDiscouragement != 162 && npcTypeAgainstDiscouragement != 166)
					{
						return false;
					}
				}
				else if (npcTypeAgainstDiscouragement != 251 && npcTypeAgainstDiscouragement != 253)
				{
					switch (npcTypeAgainstDiscouragement)
					{
					case 460:
					case 461:
					case 462:
					case 463:
					case 466:
					case 467:
					case 468:
					case 469:
					case 477:
					case 478:
					case 479:
						break;
					case 464:
					case 465:
					case 470:
					case 471:
					case 472:
					case 473:
					case 474:
					case 475:
					case 476:
						return false;
					default:
						return false;
					}
				}
				if (!Main.eclipse || !Main.dayTime)
				{
					return true;
				}
				return false;
			}

			// Token: 0x06004293 RID: 17043 RVA: 0x006BDCA8 File Offset: 0x006BBEA8
			public bool IsExpired(int gameTime)
			{
				return this._forceExpire || this._expirationTime <= gameTime;
			}

			// Token: 0x06004294 RID: 17044 RVA: 0x006BDCC0 File Offset: 0x006BBEC0
			private int CalculateExpirationTime(int gameCacheTime, int coinValue)
			{
				int num;
				if (coinValue < CoinLossRevengeSystem.RevengeMarker._expirationCompSilver)
				{
					num = (int)MathHelper.Lerp(0f, 3600f, Utils.GetLerpValue((float)CoinLossRevengeSystem.RevengeMarker._expirationCompCopper, (float)CoinLossRevengeSystem.RevengeMarker._expirationCompSilver, (float)coinValue, false));
				}
				else if (coinValue < CoinLossRevengeSystem.RevengeMarker._expirationCompGold)
				{
					num = (int)MathHelper.Lerp(36000f, 108000f, Utils.GetLerpValue((float)CoinLossRevengeSystem.RevengeMarker._expirationCompSilver, (float)CoinLossRevengeSystem.RevengeMarker._expirationCompGold, (float)coinValue, false));
				}
				else if (coinValue < CoinLossRevengeSystem.RevengeMarker._expirationCompPlat)
				{
					num = (int)MathHelper.Lerp(108000f, 216000f, Utils.GetLerpValue((float)CoinLossRevengeSystem.RevengeMarker._expirationCompSilver, (float)CoinLossRevengeSystem.RevengeMarker._expirationCompGold, (float)coinValue, false));
				}
				else
				{
					num = 432000;
				}
				num += 18000;
				return gameCacheTime + num;
			}

			// Token: 0x06004295 RID: 17045 RVA: 0x006BDD6D File Offset: 0x006BBF6D
			public bool Intersects(Rectangle rectInner, Rectangle rectOuter)
			{
				return rectOuter.Intersects(this._hitbox);
			}

			// Token: 0x06004296 RID: 17046 RVA: 0x006BDD7C File Offset: 0x006BBF7C
			public void SpawnEnemy()
			{
				int num = NPC.NewNPC(new EntitySource_RevengeSystem(), (int)this._location.X, (int)this._location.Y, this._npcNetID, 0, 0f, 0f, 0f, 0f, 255);
				NPC npc = Main.npc[num];
				npc.Center = this._location;
				if (this._npcNetID < 0)
				{
					npc.SetDefaults(this._npcNetID, default(NPCSpawnParams));
				}
				int num2;
				if (NPCID.Sets.SpecialSpawningRules.TryGetValue(this._npcNetID, out num2) && num2 == 0)
				{
					Point point = npc.position.ToTileCoordinates();
					npc.ai[0] = (float)point.X;
					npc.ai[1] = (float)point.Y;
					npc.netUpdate = true;
				}
				npc.timeLeft += 3600;
				npc.extraValue = this._coinsValue;
				npc.value = this._baseValue;
				npc.SpawnedFromStatue = this._spawnedFromStatue;
				float num3 = Math.Max(0.5f, this._npcHPPercent);
				npc.life = (int)((float)npc.lifeMax * num3);
				if (num < Main.maxNPCs)
				{
					if (Main.netMode == 0)
					{
						npc.moneyPing(this._location);
					}
					else
					{
						NetMessage.SendData(23, -1, -1, null, num, 0f, 0f, 0f, 0, 0, 0);
						NetMessage.SendData(92, -1, -1, null, num, (float)this._coinsValue, this._location.X, this._location.Y, 0, 0, 0);
					}
				}
				if (CoinLossRevengeSystem.DisplayCaching)
				{
					Main.NewText("Spawned " + npc.GivenOrTypeName, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				}
			}

			// Token: 0x06004297 RID: 17047 RVA: 0x006BDF34 File Offset: 0x006BC134
			public bool WouldNPCBeDiscouraged(Player playerTarget)
			{
				int num;
				switch (this._npcAIStyleAgainstDiscouragement)
				{
				case 2:
					return NPC.DespawnEncouragement_AIStyle2_FloatingEye_IsDiscouraged(this._npcTypeAgainstDiscouragement, playerTarget.position, 255);
				case 3:
					return !NPC.DespawnEncouragement_AIStyle3_Fighters_NotDiscouraged(this._npcTypeAgainstDiscouragement, playerTarget.position, null);
				case 6:
				{
					bool flag = false;
					num = this._npcTypeAgainstDiscouragement;
					if (num <= 95)
					{
						if (num != 10 && num != 39 && num != 95)
						{
							goto IL_97;
						}
					}
					else if (num != 117 && num != 510)
					{
						if (num == 513)
						{
							flag = !playerTarget.ZoneUndergroundDesert;
							goto IL_97;
						}
						goto IL_97;
					}
					flag = true;
					IL_97:
					return flag && (double)playerTarget.position.Y < Main.worldSurface * 16.0;
				}
				}
				num = this._npcNetID;
				if (num != 253)
				{
					return num == 490 && Main.dayTime;
				}
				return !Main.eclipse;
			}

			// Token: 0x06004298 RID: 17048 RVA: 0x006BE024 File Offset: 0x006BC224
			public bool DrawMapIcon(SpriteBatch spriteBatch, Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale, float drawScale, int gameTime)
			{
				Vector2 vector = this._location / 16f - mapTopLeft;
				vector *= mapScale;
				vector += mapX2Y2AndOff;
				if (mapRect != null && !mapRect.Value.Contains(vector.ToPoint()))
				{
					return false;
				}
				Texture2D value = TextureAssets.MapDeath.Value;
				if (this._coinsValue < 100)
				{
					value = TextureAssets.Coin[0].Value;
				}
				else if (this._coinsValue < 10000)
				{
					value = TextureAssets.Coin[1].Value;
				}
				else if (this._coinsValue < 1000000)
				{
					value = TextureAssets.Coin[2].Value;
				}
				else
				{
					value = TextureAssets.Coin[3].Value;
				}
				Rectangle rectangle = value.Frame(1, 8, 0, 0, 0, 0);
				spriteBatch.Draw(value, vector, new Rectangle?(rectangle), Color.White, 0f, rectangle.Size() / 2f, drawScale, SpriteEffects.None, 0f);
				return Utils.CenteredRectangle(vector, rectangle.Size() * drawScale).Contains(Main.MouseScreen.ToPoint());
			}

			// Token: 0x06004299 RID: 17049 RVA: 0x006BE148 File Offset: 0x006BC348
			public void UseMouseOver(SpriteBatch spriteBatch, ref string mouseTextString, float drawScale = 1f)
			{
				mouseTextString = "";
				Vector2 vector = Main.MouseScreen / drawScale + new Vector2(-28f) + new Vector2(4f, 0f);
				ItemSlot.DrawMoney(spriteBatch, "", vector.X, vector.Y, Utils.CoinsSplit((long)this._coinsValue), true, false);
			}

			// Token: 0x1700053A RID: 1338
			// (get) Token: 0x0600429A RID: 17050 RVA: 0x006BE1B0 File Offset: 0x006BC3B0
			public int UniqueID
			{
				get
				{
					return this._uniqueID;
				}
			}

			// Token: 0x0600429B RID: 17051 RVA: 0x006BE1B8 File Offset: 0x006BC3B8
			public void WriteSelfTo(BinaryWriter writer)
			{
				writer.Write(this._uniqueID);
				writer.WriteVector2(this._location);
				writer.Write(this._npcNetID);
				writer.Write(this._npcHPPercent);
				writer.Write(this._npcTypeAgainstDiscouragement);
				writer.Write(this._npcAIStyleAgainstDiscouragement);
				writer.Write(this._coinsValue);
				writer.Write(this._baseValue);
				writer.Write(this._spawnedFromStatue);
			}

			// Token: 0x04007144 RID: 28996
			private static int _uniqueIDCounter = 0;

			// Token: 0x04007145 RID: 28997
			private static readonly int _expirationCompCopper = Item.buyPrice(0, 0, 0, 1);

			// Token: 0x04007146 RID: 28998
			private static readonly int _expirationCompSilver = Item.buyPrice(0, 0, 1, 0);

			// Token: 0x04007147 RID: 28999
			private static readonly int _expirationCompGold = Item.buyPrice(0, 1, 0, 0);

			// Token: 0x04007148 RID: 29000
			private static readonly int _expirationCompPlat = Item.buyPrice(1, 0, 0, 0);

			// Token: 0x04007149 RID: 29001
			private const int ONE_MINUTE = 3600;

			// Token: 0x0400714A RID: 29002
			private const int ENEMY_BOX_WIDTH = 2160;

			// Token: 0x0400714B RID: 29003
			private const int ENEMY_BOX_HEIGHT = 1440;

			// Token: 0x0400714C RID: 29004
			public static readonly Vector2 EnemyBoxSize = new Vector2(2160f, 1440f);

			// Token: 0x0400714D RID: 29005
			private readonly Vector2 _location;

			// Token: 0x0400714E RID: 29006
			private readonly Rectangle _hitbox;

			// Token: 0x0400714F RID: 29007
			private readonly int _npcNetID;

			// Token: 0x04007150 RID: 29008
			private readonly float _npcHPPercent;

			// Token: 0x04007151 RID: 29009
			private readonly float _baseValue;

			// Token: 0x04007152 RID: 29010
			private readonly int _coinsValue;

			// Token: 0x04007153 RID: 29011
			private readonly int _npcTypeAgainstDiscouragement;

			// Token: 0x04007154 RID: 29012
			private readonly int _npcAIStyleAgainstDiscouragement;

			// Token: 0x04007155 RID: 29013
			private readonly int _expirationTime;

			// Token: 0x04007156 RID: 29014
			private readonly bool _spawnedFromStatue;

			// Token: 0x04007157 RID: 29015
			private readonly int _uniqueID;

			// Token: 0x04007158 RID: 29016
			private bool _forceExpire;

			// Token: 0x04007159 RID: 29017
			private bool _attemptedRespawn;
		}
	}
}
