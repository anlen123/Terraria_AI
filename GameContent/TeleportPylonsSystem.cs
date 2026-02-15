using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.Tile_Entities;
using Terraria.Localization;
using Terraria.Net;

namespace Terraria.GameContent
{
	// Token: 0x02000268 RID: 616
	public class TeleportPylonsSystem : IOnPlayerJoining
	{
		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060023DA RID: 9178 RVA: 0x005482BC File Offset: 0x005464BC
		public List<TeleportPylonInfo> Pylons
		{
			get
			{
				return this._pylons;
			}
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x005482C4 File Offset: 0x005464C4
		public void Update()
		{
			if (Main.netMode == 1)
			{
				return;
			}
			if (this._cooldownForUpdatingPylonsList > 0)
			{
				this._cooldownForUpdatingPylonsList--;
				return;
			}
			this._cooldownForUpdatingPylonsList = int.MaxValue;
			this.UpdatePylonsListAndBroadcastChanges();
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x005482F8 File Offset: 0x005464F8
		public bool HasPylonOfType(TeleportPylonType pylonType)
		{
			return this._pylons.Any((TeleportPylonInfo x) => x.TypeOfPylon == pylonType);
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x00548329 File Offset: 0x00546529
		public bool HasAnyPylon()
		{
			return this._pylons.Count > 0;
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x00548339 File Offset: 0x00546539
		public void RequestImmediateUpdate()
		{
			if (Main.netMode == 1)
			{
				return;
			}
			this._cooldownForUpdatingPylonsList = int.MaxValue;
			this.UpdatePylonsListAndBroadcastChanges();
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x00548358 File Offset: 0x00546558
		private void UpdatePylonsListAndBroadcastChanges()
		{
			Utils.Swap<List<TeleportPylonInfo>>(ref this._pylons, ref this._pylonsOld);
			this._pylons.Clear();
			foreach (TileEntity tileEntity in TileEntity.ByPosition.Values)
			{
				TETeleportationPylon teteleportationPylon = tileEntity as TETeleportationPylon;
				TeleportPylonType typeOfPylon;
				if (teteleportationPylon != null && teteleportationPylon.TryGetPylonType(out typeOfPylon))
				{
					TeleportPylonInfo item = new TeleportPylonInfo
					{
						PositionInTiles = teteleportationPylon.Position,
						TypeOfPylon = typeOfPylon
					};
					this._pylons.Add(item);
				}
			}
			IEnumerable<TeleportPylonInfo> enumerable = this._pylonsOld.Except(this._pylons);
			foreach (TeleportPylonInfo info in this._pylons.Except(this._pylonsOld))
			{
				NetManager.Instance.BroadcastOrLoopback(NetTeleportPylonModule.SerializePylonWasAddedOrRemoved(info, NetTeleportPylonModule.SubPacketType.PylonWasAdded));
			}
			foreach (TeleportPylonInfo info2 in enumerable)
			{
				NetManager.Instance.BroadcastOrLoopback(NetTeleportPylonModule.SerializePylonWasAddedOrRemoved(info2, NetTeleportPylonModule.SubPacketType.PylonWasRemoved));
			}
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x005484B8 File Offset: 0x005466B8
		public void AddForClient(TeleportPylonInfo info)
		{
			if (this._pylons.Contains(info))
			{
				return;
			}
			this._pylons.Add(info);
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x005484D8 File Offset: 0x005466D8
		public void RemoveForClient(TeleportPylonInfo info)
		{
			this._pylons.RemoveAll((TeleportPylonInfo x) => x.Equals(info));
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x0054850C File Offset: 0x0054670C
		public void HandleTeleportRequest(TeleportPylonInfo info, int playerIndex)
		{
			Player player = Main.player[playerIndex];
			string key = null;
			bool flag = true;
			if (flag)
			{
				flag &= TeleportPylonsSystem.IsPlayerNearAPylon(player);
				if (!flag)
				{
					key = "Net.CannotTeleportToPylonBecausePlayerIsNotNearAPylon";
				}
			}
			if (flag)
			{
				int necessaryNPCCount = this.HowManyNPCsDoesPylonNeed(info, player);
				flag &= this.DoesPylonHaveEnoughNPCsAroundIt(info, necessaryNPCCount);
				if (!flag)
				{
					key = "Net.CannotTeleportToPylonBecauseNotEnoughNPCs";
				}
			}
			if (flag)
			{
				if (!NPC.downedPlantBoss && (double)info.PositionInTiles.Y > Main.worldSurface && Framing.GetTileSafely((int)info.PositionInTiles.X, (int)info.PositionInTiles.Y).wall == 87)
				{
					flag = false;
				}
				if (!flag)
				{
					key = "Net.CannotTeleportToPylonBecauseAccessingLihzahrdTempleEarly";
				}
			}
			if (flag)
			{
				this._sceneMetrics.Scan(new SceneMetricsScanSettings
				{
					BiomeScanCenterPositionInWorld = info.PositionInTiles.ToWorldCoordinates(8f, 8f)
				});
				flag = this.DoesPylonAcceptTeleportation(info, player);
				if (!flag)
				{
					key = "Net.CannotTeleportToPylonBecauseNotMeetingBiomeRequirements";
				}
			}
			if (flag)
			{
				bool flag2 = false;
				int num = 0;
				for (int i = 0; i < this._pylons.Count; i++)
				{
					TeleportPylonInfo teleportPylonInfo = this._pylons[i];
					if (player.InTileEntityInteractionRange((int)teleportPylonInfo.PositionInTiles.X, (int)teleportPylonInfo.PositionInTiles.Y, 3, 4, TileReachCheckSettings.Pylons))
					{
						if (num < 1)
						{
							num = 1;
						}
						int necessaryNPCCount2 = this.HowManyNPCsDoesPylonNeed(teleportPylonInfo, player);
						if (this.DoesPylonHaveEnoughNPCsAroundIt(teleportPylonInfo, necessaryNPCCount2))
						{
							if (num < 2)
							{
								num = 2;
							}
							this._sceneMetrics.Scan(new SceneMetricsScanSettings
							{
								BiomeScanCenterPositionInWorld = teleportPylonInfo.PositionInTiles.ToWorldCoordinates(8f, 8f)
							});
							if (this.DoesPylonAcceptTeleportation(teleportPylonInfo, player))
							{
								flag2 = true;
								break;
							}
						}
					}
				}
				if (!flag2)
				{
					flag = false;
					switch (num)
					{
					default:
						key = "Net.CannotTeleportToPylonBecausePlayerIsNotNearAPylon";
						break;
					case 1:
						key = "Net.CannotTeleportToPylonBecauseNotEnoughNPCsAtCurrentPylon";
						break;
					case 2:
						key = "Net.CannotTeleportToPylonBecauseNotMeetingBiomeRequirements";
						break;
					}
				}
			}
			if (flag)
			{
				Vector2 vector = info.PositionInTiles.ToWorldCoordinates(8f, 8f) - new Vector2(0f, (float)player.HeightOffsetBoost);
				int num2 = 9;
				int typeOfPylon = (int)info.TypeOfPylon;
				int number = 0;
				player.Teleport(vector, num2, typeOfPylon);
				player.velocity = Vector2.Zero;
				if (Main.netMode == 2)
				{
					RemoteClient.CheckSection(player.whoAmI, player.position, 1);
					NetMessage.SendData(65, -1, -1, null, 0, (float)player.whoAmI, vector.X, vector.Y, num2, number, typeOfPylon);
					return;
				}
			}
			else
			{
				ChatHelper.SendChatMessageToClient(NetworkText.FromKey(key, new object[0]), new Color(255, 240, 20), playerIndex);
			}
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x005487A3 File Offset: 0x005469A3
		public static bool IsPlayerNearAPylon(Player player)
		{
			return player.IsTileTypeInInteractionRange(597, TileReachCheckSettings.Pylons);
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x005487B8 File Offset: 0x005469B8
		private bool DoesPylonHaveEnoughNPCsAroundIt(TeleportPylonInfo info, int necessaryNPCCount)
		{
			if (necessaryNPCCount <= 0)
			{
				return true;
			}
			Point16 positionInTiles = info.PositionInTiles;
			return TeleportPylonsSystem.DoesPositionHaveEnoughNPCs(necessaryNPCCount, positionInTiles);
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x005487DC File Offset: 0x005469DC
		public static bool DoesPositionHaveEnoughNPCs(int necessaryNPCCount, Point16 centerPoint)
		{
			Rectangle rectangle = Utils.CenteredRectangle(centerPoint, SceneMetrics.ZoneScanSize);
			int num = necessaryNPCCount;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active && npc.isLikeATownNPC && !npc.homeless && rectangle.Contains(npc.homeTileX, npc.homeTileY))
				{
					Vector2 value = new Vector2((float)npc.homeTileX, (float)npc.homeTileY);
					Vector2 value2 = new Vector2(npc.Center.X / 16f, npc.Center.Y / 16f);
					if (Vector2.Distance(value, value2) < 100f)
					{
						num--;
						if (num == 0)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x0054889B File Offset: 0x00546A9B
		public void RequestTeleportation(TeleportPylonInfo info, Player player)
		{
			NetManager.Instance.SendToServerOrLoopback(NetTeleportPylonModule.SerializeUseRequest(info));
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x005488B0 File Offset: 0x00546AB0
		private bool DoesPylonAcceptTeleportation(TeleportPylonInfo info, Player player)
		{
			switch (info.TypeOfPylon)
			{
			case TeleportPylonType.SurfacePurity:
			{
				bool flag = (double)info.PositionInTiles.Y <= Main.worldSurface;
				if (Main.remixWorld)
				{
					flag = ((double)info.PositionInTiles.Y > Main.rockLayer && (int)info.PositionInTiles.Y < Main.maxTilesY - 350);
				}
				bool flag2 = (int)info.PositionInTiles.X >= Main.maxTilesX - 380 || info.PositionInTiles.X <= 380;
				return flag && !flag2 && (!this._sceneMetrics.EnoughTilesForJungle && !this._sceneMetrics.EnoughTilesForSnow && !this._sceneMetrics.EnoughTilesForDesert && !this._sceneMetrics.EnoughTilesForGlowingMushroom && !this._sceneMetrics.EnoughTilesForHallow && !this._sceneMetrics.EnoughTilesForCrimson && !this._sceneMetrics.EnoughTilesForCorruption);
			}
			case TeleportPylonType.Jungle:
				return this._sceneMetrics.EnoughTilesForJungle;
			case TeleportPylonType.Hallow:
				return this._sceneMetrics.EnoughTilesForHallow;
			case TeleportPylonType.Underground:
				return (double)info.PositionInTiles.Y >= Main.worldSurface;
			case TeleportPylonType.Beach:
			{
				bool flag3 = (double)info.PositionInTiles.Y <= Main.worldSurface && (double)info.PositionInTiles.Y > Main.worldSurface * 0.3499999940395355;
				bool flag4 = (int)info.PositionInTiles.X >= Main.maxTilesX - 380 || info.PositionInTiles.X <= 380;
				if (Main.remixWorld)
				{
					flag3 |= ((double)info.PositionInTiles.Y > Main.rockLayer && (int)info.PositionInTiles.Y < Main.maxTilesY - 350);
					flag4 |= ((double)info.PositionInTiles.X < (double)Main.maxTilesX * 0.43 || (double)info.PositionInTiles.X > (double)Main.maxTilesX * 0.57);
				}
				return flag4 && flag3;
			}
			case TeleportPylonType.Desert:
				return this._sceneMetrics.EnoughTilesForDesert;
			case TeleportPylonType.Snow:
				return this._sceneMetrics.EnoughTilesForSnow;
			case TeleportPylonType.GlowingMushroom:
				return (!Main.remixWorld || (int)info.PositionInTiles.Y < Main.maxTilesY - 200) && this._sceneMetrics.EnoughTilesForGlowingMushroom;
			case TeleportPylonType.Victory:
				return true;
			case TeleportPylonType.Underworld:
				return (int)info.PositionInTiles.Y >= Main.UnderworldLayer;
			case TeleportPylonType.Shimmer:
				return this._sceneMetrics.EnoughTilesForShimmer;
			default:
				return true;
			}
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x00548B68 File Offset: 0x00546D68
		private int HowManyNPCsDoesPylonNeed(TeleportPylonInfo info, Player player)
		{
			TeleportPylonType typeOfPylon = info.TypeOfPylon;
			if (typeOfPylon != TeleportPylonType.Victory)
			{
				return 2;
			}
			return 0;
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x00548B83 File Offset: 0x00546D83
		public void Reset()
		{
			this._pylons.Clear();
			this._cooldownForUpdatingPylonsList = 0;
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x00548B98 File Offset: 0x00546D98
		public void OnPlayerJoining(int playerIndex)
		{
			foreach (TeleportPylonInfo info in this._pylons)
			{
				NetManager.Instance.SendToClient(NetTeleportPylonModule.SerializePylonWasAddedOrRemoved(info, NetTeleportPylonModule.SubPacketType.PylonWasAdded), playerIndex);
			}
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x00548BF8 File Offset: 0x00546DF8
		public static void SpawnInWorldDust(int tileStyle, Rectangle dustBox)
		{
			float r = 1f;
			float g = 1f;
			float b = 1f;
			switch ((byte)tileStyle)
			{
			case 0:
				r = 0.05f;
				g = 0.8f;
				b = 0.3f;
				break;
			case 1:
				r = 0.7f;
				g = 0.8f;
				b = 0.05f;
				break;
			case 2:
				r = 0.5f;
				g = 0.3f;
				b = 0.7f;
				break;
			case 3:
				r = 0.4f;
				g = 0.4f;
				b = 0.6f;
				break;
			case 4:
				r = 0.2f;
				g = 0.2f;
				b = 0.95f;
				break;
			case 5:
				r = 0.85f;
				g = 0.45f;
				b = 0.1f;
				break;
			case 6:
				r = 1f;
				g = 1f;
				b = 1.2f;
				break;
			case 7:
				r = 0.4f;
				g = 0.7f;
				b = 1.2f;
				break;
			case 8:
				r = 0.7f;
				g = 0.7f;
				b = 0.7f;
				break;
			case 9:
				r = 0.05f;
				g = 0.8f;
				b = 0.3f;
				break;
			case 10:
				r = 0.05f;
				g = 0.8f;
				b = 0.3f;
				break;
			}
			int num = Dust.NewDust(dustBox.TopLeft(), dustBox.Width, dustBox.Height, 43, 0f, 0f, 254, new Color(r, g, b, 1f), 0.5f);
			Main.dust[num].velocity *= 0.1f;
			Dust dust = Main.dust[num];
			dust.velocity.Y = dust.velocity.Y - 0.2f;
		}

		// Token: 0x04004D8E RID: 19854
		private List<TeleportPylonInfo> _pylons = new List<TeleportPylonInfo>();

		// Token: 0x04004D8F RID: 19855
		private List<TeleportPylonInfo> _pylonsOld = new List<TeleportPylonInfo>();

		// Token: 0x04004D90 RID: 19856
		private int _cooldownForUpdatingPylonsList;

		// Token: 0x04004D91 RID: 19857
		private const int CooldownTimePerPylonsListUpdate = 2147483647;

		// Token: 0x04004D92 RID: 19858
		private SceneMetrics _sceneMetrics = new SceneMetrics();
	}
}
