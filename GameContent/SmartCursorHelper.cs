using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ID;

namespace Terraria.GameContent
{
	// Token: 0x02000265 RID: 613
	public class SmartCursorHelper
	{
		// Token: 0x060023A5 RID: 9125 RVA: 0x0053F9E4 File Offset: 0x0053DBE4
		public static void SmartCursorLookup(Player player)
		{
			Main.SmartCursorShowing = false;
			if (!player.controlUseItem || !Main.SmartCursorIsUsed)
			{
				SmartCursorHelper._lockedDesiredDirection = null;
				SmartCursorHelper._lockedContinuityCoords = null;
			}
			if (!Main.SmartCursorIsUsed)
			{
				return;
			}
			SmartCursorHelper.SmartCursorUsageInfo smartCursorUsageInfo = new SmartCursorHelper.SmartCursorUsageInfo
			{
				player = player,
				item = player.inventory[player.selectedItem],
				mouse = Main.MouseWorld,
				position = player.position,
				Center = player.Center
			};
			float gravDir = player.gravDir;
			int tileTargetX = Player.tileTargetX;
			int tileTargetY = Player.tileTargetY;
			int tileRangeX = Player.tileRangeX;
			int tileRangeY = Player.tileRangeY;
			smartCursorUsageInfo.screenTargetX = Utils.Clamp<int>(tileTargetX, 10, Main.maxTilesX - 10);
			smartCursorUsageInfo.screenTargetY = Utils.Clamp<int>(tileTargetY, 10, Main.maxTilesY - 10);
			if (Main.tile[smartCursorUsageInfo.screenTargetX, smartCursorUsageInfo.screenTargetY] == null)
			{
				return;
			}
			bool flag = SmartCursorHelper.IsHoveringOverAnInteractableTileThatBlocksSmartCursor(smartCursorUsageInfo);
			SmartCursorHelper.TryFindingPaintInplayerInventory(smartCursorUsageInfo, out smartCursorUsageInfo.paintLookup, out smartCursorUsageInfo.paintCoatingLookup);
			int num = smartCursorUsageInfo.item.tileBoost;
			if (smartCursorUsageInfo.item.createWall > 0 || smartCursorUsageInfo.item.createTile > 0 || smartCursorUsageInfo.item.tileWand > 0)
			{
				num += player.blockRange;
			}
			TileReachCheckSettings.Simple.GetTileRegion(player, out smartCursorUsageInfo.reachableStartX, out smartCursorUsageInfo.reachableStartY, out smartCursorUsageInfo.reachableEndX, out smartCursorUsageInfo.reachableEndY, num);
			smartCursorUsageInfo.reachableStartX = Utils.Clamp<int>(smartCursorUsageInfo.reachableStartX, 10, Main.maxTilesX - 10);
			smartCursorUsageInfo.reachableEndX = Utils.Clamp<int>(smartCursorUsageInfo.reachableEndX, 10, Main.maxTilesX - 10);
			smartCursorUsageInfo.reachableStartY = Utils.Clamp<int>(smartCursorUsageInfo.reachableStartY, 10, Main.maxTilesY - 10);
			smartCursorUsageInfo.reachableEndY = Utils.Clamp<int>(smartCursorUsageInfo.reachableEndY, 10, Main.maxTilesY - 10);
			if (flag && smartCursorUsageInfo.screenTargetX >= smartCursorUsageInfo.reachableStartX && smartCursorUsageInfo.screenTargetX <= smartCursorUsageInfo.reachableEndX && smartCursorUsageInfo.screenTargetY >= smartCursorUsageInfo.reachableStartY && smartCursorUsageInfo.screenTargetY <= smartCursorUsageInfo.reachableEndY)
			{
				return;
			}
			SmartCursorHelper._grappleTargets.Clear();
			int[] grappling = player.grappling;
			int grapCount = player.grapCount;
			for (int i = 0; i < grapCount; i++)
			{
				Projectile projectile = Main.projectile[grappling[i]];
				int x = (int)projectile.Center.X / 16;
				int y = (int)projectile.Center.Y / 16;
				SmartCursorHelper._grappleTargets.Add(new Point(x, y));
			}
			int num2 = -1;
			int num3 = -1;
			if (!Player.SmartCursorSettings.SmartAxeAfterPickaxe)
			{
				SmartCursorHelper.Step_Axe(smartCursorUsageInfo, ref num2, ref num3);
			}
			SmartCursorHelper.Step_ForceCursorToAnyMinableThing(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_Pickaxe_MineShinies(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_Pickaxe_MineSolids(player, player.position, player.Center, player.width, player.direction, smartCursorUsageInfo, SmartCursorHelper._grappleTargets, ref num2, ref num3);
			if (Player.SmartCursorSettings.SmartAxeAfterPickaxe)
			{
				SmartCursorHelper.Step_Axe(smartCursorUsageInfo, ref num2, ref num3);
			}
			SmartCursorHelper.Step_ColoredWrenches(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_MulticolorWrench(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_Hammers(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_ActuationRod(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_WireCutter(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_Platforms(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_MinecartTracks(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_Walls(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_PumpkinSeeds(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_GrassSeeds(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_Moss(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_Pigronata(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_Boulders(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_Torch(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_LawnMower(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_BlocksFilling(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_BlocksLines(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_PaintRoller(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_PaintBrush(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_PaintScrapper(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_Acorns(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_GemCorns(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_EmptyBuckets(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_Actuators(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_AlchemySeeds(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_PlanterBox(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_ClayPots(smartCursorUsageInfo, ref num2, ref num3);
			SmartCursorHelper.Step_StaffOfRegrowth(smartCursorUsageInfo, ref num2, ref num3);
			if (num2 != -1 && num3 != -1)
			{
				Main.SmartCursorX = (Player.tileTargetX = num2);
				Main.SmartCursorY = (Player.tileTargetY = num3);
				Main.SmartCursorShowing = true;
			}
			SmartCursorHelper._grappleTargets.Clear();
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x0053FE0C File Offset: 0x0053E00C
		private static void TryFindingPaintInplayerInventory(SmartCursorHelper.SmartCursorUsageInfo providedInfo, out int paintLookup, out int coatingLookup)
		{
			Item[] inventory = providedInfo.player.inventory;
			paintLookup = 0;
			coatingLookup = 0;
			if (providedInfo.item.type != 1071 && providedInfo.item.type != 1543 && providedInfo.item.type != 1072 && providedInfo.item.type != 1544)
			{
				return;
			}
			Item item = providedInfo.player.FindPaintOrCoating();
			if (item == null)
			{
				return;
			}
			coatingLookup = (int)item.paintCoating;
			paintLookup = (int)item.paint;
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x0053FE9C File Offset: 0x0053E09C
		private static bool IsHoveringOverAnInteractableTileThatBlocksSmartCursor(SmartCursorHelper.SmartCursorUsageInfo providedInfo)
		{
			bool result = false;
			Tile tile = Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY];
			if (tile.active())
			{
				ushort type = tile.type;
				if (type <= 212)
				{
					if (type <= 88)
					{
						if (type <= 33)
						{
							if (type <= 13)
							{
								if (type - 10 > 1 && type != 13)
								{
									goto IL_293;
								}
							}
							else if (type != 21 && type != 29 && type != 33)
							{
								goto IL_293;
							}
						}
						else if (type <= 55)
						{
							if (type - 49 > 1 && type != 55)
							{
								goto IL_293;
							}
						}
						else if (type != 79 && type != 85 && type != 88)
						{
							goto IL_293;
						}
					}
					else if (type <= 136)
					{
						if (type <= 104)
						{
							if (type != 97 && type != 104)
							{
								goto IL_293;
							}
						}
						else if (type != 125 && type != 132 && type != 136)
						{
							goto IL_293;
						}
					}
					else if (type <= 174)
					{
						if (type != 139 && type != 144 && type != 174)
						{
							goto IL_293;
						}
					}
					else if (type != 207 && type != 209 && type != 212)
					{
						goto IL_293;
					}
				}
				else if (type <= 425)
				{
					if (type <= 314)
					{
						if (type <= 219)
						{
							if (type != 216 && type != 219)
							{
								goto IL_293;
							}
						}
						else if (type != 237 && type != 287)
						{
							if (type != 314)
							{
								goto IL_293;
							}
							if (providedInfo.player.gravDir == 1f)
							{
								result = true;
								goto IL_293;
							}
							goto IL_293;
						}
					}
					else if (type <= 354)
					{
						if (type - 334 > 1 && type != 338 && type != 354)
						{
							goto IL_293;
						}
					}
					else if (type - 386 > 3 && type != 411 && type != 425)
					{
						goto IL_293;
					}
				}
				else if (type <= 494)
				{
					if (type <= 464)
					{
						if (type != 441 && type - 463 > 1)
						{
							goto IL_293;
						}
					}
					else if (type - 467 > 1 && type != 491 && type != 494)
					{
						goto IL_293;
					}
				}
				else if (type <= 621)
				{
					if (type - 510 > 1 && type != 573 && type != 621)
					{
						goto IL_293;
					}
				}
				else if (type != 642 && type != 699 && type != 707)
				{
					goto IL_293;
				}
				result = true;
				IL_293:
				if (TileID.Sets.Torches[(int)tile.type])
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x0054014D File Offset: 0x0053E34D
		private static bool AllowNormalBlockPlacementBehaviourForItemType(int itemType)
		{
			return itemType >= 0 && itemType < (int)ItemID.Count && itemType != 213 && itemType != 5295 && !ItemID.Sets.GrassSeeds[itemType] && !ItemID.Sets.Moss[itemType];
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x00540184 File Offset: 0x0053E384
		private static void Step_StaffOfRegrowth(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if ((providedInfo.item.type == 213 || providedInfo.item.type == 5295) && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						bool flag = !Main.tile[i - 1, j].active() || !Main.tile[i, j + 1].active() || !Main.tile[i + 1, j].active() || !Main.tile[i, j - 1].active();
						bool flag2 = !Main.tile[i - 1, j - 1].active() || !Main.tile[i - 1, j + 1].active() || !Main.tile[i + 1, j + 1].active() || !Main.tile[i + 1, j - 1].active();
						if (tile.active() && !tile.inActive() && tile.type == 0 && (flag || (tile.type == 0 && flag2)))
						{
							SmartCursorHelper._targets.Add(new Point(i, j));
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[k];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x00540410 File Offset: 0x0053E610
		private static void Step_GrassSeeds(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (focusedX > -1 || focusedY > -1)
			{
				return;
			}
			int type = providedInfo.item.type;
			if (type < 0 || type >= (int)ItemID.Count || !ItemID.Sets.GrassSeeds[type])
			{
				return;
			}
			SmartCursorHelper._targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					bool flag = !Main.tile[i - 1, j].active() || !Main.tile[i, j + 1].active() || !Main.tile[i + 1, j].active() || !Main.tile[i, j - 1].active();
					bool flag2 = !Main.tile[i - 1, j - 1].active() || !Main.tile[i - 1, j + 1].active() || !Main.tile[i + 1, j + 1].active() || !Main.tile[i + 1, j - 1].active();
					if (tile.active() && !tile.inActive() && (flag || flag2))
					{
						bool flag3;
						if (type <= 195)
						{
							if (type == 59)
							{
								goto IL_172;
							}
							if (type - 194 > 1)
							{
								goto IL_165;
							}
							flag3 = (tile.type == 59);
						}
						else
						{
							if (type == 2171)
							{
								goto IL_172;
							}
							if (type != 5214)
							{
								goto IL_165;
							}
							flag3 = (tile.type == 57);
						}
						IL_1A5:
						if (flag3)
						{
							SmartCursorHelper._targets.Add(new Point(i, j));
							goto IL_1BA;
						}
						goto IL_1BA;
						IL_172:
						flag3 = (tile.type == 0 || tile.type == 59);
						goto IL_1A5;
						IL_165:
						flag3 = (tile.type == 0);
						goto IL_1A5;
					}
					IL_1BA:;
				}
			}
			if (SmartCursorHelper._targets.Count > 0)
			{
				float num = -1f;
				Point point = SmartCursorHelper._targets[0];
				for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
				{
					float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						point = SmartCursorHelper._targets[k];
					}
				}
				if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = point.X;
					focusedY = point.Y;
				}
			}
			SmartCursorHelper._targets.Clear();
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x005406F8 File Offset: 0x0053E8F8
		private static void Step_Moss(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (focusedX > -1 || focusedY > -1)
			{
				return;
			}
			int type = providedInfo.item.type;
			if (type < 0 || type >= (int)ItemID.Count || !ItemID.Sets.Moss[type])
			{
				return;
			}
			SmartCursorHelper._targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					bool flag = !Main.tile[i - 1, j].active() || !Main.tile[i, j + 1].active() || !Main.tile[i + 1, j].active() || !Main.tile[i, j - 1].active();
					bool flag2 = !Main.tile[i - 1, j - 1].active() || !Main.tile[i - 1, j + 1].active() || !Main.tile[i + 1, j + 1].active() || !Main.tile[i + 1, j - 1].active();
					if (tile.active() && !tile.inActive() && (flag || flag2) && (tile.type == 1 || tile.type == 38))
					{
						SmartCursorHelper._targets.Add(new Point(i, j));
					}
				}
			}
			if (SmartCursorHelper._targets.Count > 0)
			{
				float num = -1f;
				Point point = SmartCursorHelper._targets[0];
				for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
				{
					float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						point = SmartCursorHelper._targets[k];
					}
				}
				if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = point.X;
					focusedY = point.Y;
				}
			}
			SmartCursorHelper._targets.Clear();
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x0054097C File Offset: 0x0053EB7C
		private static void Step_ClayPots(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile == 78 && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				bool flag = false;
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active())
				{
					flag = true;
				}
				if (!Collision.InTileBounds(providedInfo.screenTargetX, providedInfo.screenTargetY, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					flag = true;
				}
				if (!flag)
				{
					for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
					{
						for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
						{
							Tile tile = Main.tile[i, j];
							Tile tile2 = Main.tile[i, j + 1];
							if ((!tile.active() || Main.tileCut[(int)tile.type] || TileID.Sets.BreakableWhenPlacing[(int)tile.type]) && tile2.nactive() && !tile2.halfBrick() && tile2.slope() == 0 && Main.tileSolid[(int)tile2.type])
							{
								SmartCursorHelper._targets.Add(new Point(i, j));
							}
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						if (Collision.EmptyTile(SmartCursorHelper._targets[k].X, SmartCursorHelper._targets[k].Y, true))
						{
							float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
							if (num == -1f || num2 < num)
							{
								num = num2;
								point = SmartCursorHelper._targets[k];
							}
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num != -1f)
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x00540BF4 File Offset: 0x0053EDF4
		private static void Step_PlanterBox(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile == 380 && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				bool flag = false;
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type == 380)
				{
					flag = true;
				}
				if (!flag)
				{
					for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
					{
						for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
						{
							Tile tile = Main.tile[i, j];
							if (tile.active() && tile.type == 380)
							{
								if (!Main.tile[i - 1, j].active() || Main.tileCut[(int)Main.tile[i - 1, j].type] || TileID.Sets.BreakableWhenPlacing[(int)Main.tile[i - 1, j].type])
								{
									SmartCursorHelper._targets.Add(new Point(i - 1, j));
								}
								if (!Main.tile[i + 1, j].active() || Main.tileCut[(int)Main.tile[i + 1, j].type] || TileID.Sets.BreakableWhenPlacing[(int)Main.tile[i + 1, j].type])
								{
									SmartCursorHelper._targets.Add(new Point(i + 1, j));
								}
							}
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[k];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num != -1f)
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x00540E9C File Offset: 0x0053F09C
		private static void Step_AlchemySeeds(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile == 82 && focusedX == -1 && focusedY == -1)
			{
				int placeStyle = providedInfo.item.placeStyle;
				SmartCursorHelper._targets.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						Tile tile2 = Main.tile[i, j + 1];
						bool flag = !tile.active() || TileID.Sets.BreakableWhenPlacing[(int)tile.type] || (Main.tileCut[(int)tile.type] && tile.type != 82 && tile.type != 83) || WorldGen.IsHarvestableHerbWithSeed((int)tile.type, (int)(tile.frameX / 18), j);
						bool flag2 = tile2.nactive() && !tile2.halfBrick() && tile2.slope() == 0;
						if (flag && flag2)
						{
							if (placeStyle == 0)
							{
								if (tile2.type != 78 && tile2.type != 380 && tile2.type != 2 && tile2.type != 477 && tile2.type != 109 && tile2.type != 492)
								{
									goto IL_358;
								}
								if (tile.liquid > 0)
								{
									goto IL_358;
								}
							}
							else if (placeStyle == 1)
							{
								if (tile2.type != 78 && tile2.type != 380 && tile2.type != 60)
								{
									goto IL_358;
								}
								if (tile.liquid > 0)
								{
									goto IL_358;
								}
							}
							else if (placeStyle == 2)
							{
								if (tile2.type != 78 && tile2.type != 380 && tile2.type != 0 && tile2.type != 59)
								{
									goto IL_358;
								}
								if (tile.liquid > 0)
								{
									goto IL_358;
								}
							}
							else if (placeStyle == 3)
							{
								if (tile2.type != 78 && tile2.type != 380 && tile2.type != 203 && tile2.type != 199 && tile2.type != 23 && tile2.type != 25)
								{
									goto IL_358;
								}
								if (tile.liquid > 0)
								{
									goto IL_358;
								}
							}
							else if (placeStyle == 4)
							{
								if (tile2.type != 78 && tile2.type != 380 && tile2.type != 53 && tile2.type != 116)
								{
									goto IL_358;
								}
								if (tile.liquid > 0 && tile.lava())
								{
									goto IL_358;
								}
							}
							else if (placeStyle == 5)
							{
								if (tile2.type != 78 && tile2.type != 380 && tile2.type != 57 && tile2.type != 633)
								{
									goto IL_358;
								}
								if (tile.liquid > 0 && !tile.lava())
								{
									goto IL_358;
								}
							}
							else if (placeStyle == 6 && ((tile2.type != 78 && tile2.type != 380 && tile2.type != 147 && tile2.type != 161 && tile2.type != 163 && tile2.type != 164 && tile2.type != 200) || (tile.liquid > 0 && tile.lava())))
							{
								goto IL_358;
							}
							SmartCursorHelper._targets.Add(new Point(i, j));
						}
						IL_358:;
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[k];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x00541320 File Offset: 0x0053F520
		private static void Step_Actuators(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.type == 849 && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if ((tile.wire() || tile.wire2() || tile.wire3() || tile.wire4()) && !tile.actuator() && tile.active())
						{
							SmartCursorHelper._targets.Add(new Point(i, j));
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[k];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x005414D0 File Offset: 0x0053F6D0
		private static void Step_EmptyBuckets(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.type == 205 && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (tile.liquid > 0)
						{
							int num = (int)tile.liquidType();
							int num2 = 0;
							for (int k = i - 1; k <= i + 1; k++)
							{
								for (int l = j - 1; l <= j + 1; l++)
								{
									if ((int)Main.tile[k, l].liquidType() == num)
									{
										num2 += (int)Main.tile[k, l].liquid;
									}
								}
							}
							if (num2 > 100)
							{
								SmartCursorHelper._targets.Add(new Point(i, j));
							}
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num3 = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int m = 0; m < SmartCursorHelper._targets.Count; m++)
					{
						float num4 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[m].X, (float)SmartCursorHelper._targets[m].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num3 == -1f || num4 < num3)
						{
							num3 = num4;
							point = SmartCursorHelper._targets[m];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x005416D0 File Offset: 0x0053F8D0
		private static void Step_PaintScrapper(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (ItemID.Sets.IsPaintScraper[providedInfo.item.type] && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						bool flag = false;
						if (tile.active())
						{
							flag |= (tile.color() > 0);
							flag |= (tile.type == 184);
							flag |= tile.fullbrightBlock();
							flag |= tile.invisibleBlock();
						}
						if (tile.wall > 0)
						{
							flag |= (tile.wallColor() > 0);
							flag |= tile.fullbrightWall();
							flag |= tile.invisibleWall();
						}
						if (flag)
						{
							SmartCursorHelper._targets.Add(new Point(i, j));
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[k];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x005418C4 File Offset: 0x0053FAC4
		private static void Step_PaintBrush(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if ((providedInfo.item.type == 1071 || providedInfo.item.type == 1543) && (providedInfo.paintLookup != 0 || providedInfo.paintCoatingLookup != 0) && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				int paintLookup = providedInfo.paintLookup;
				int paintCoatingLookup = providedInfo.paintCoatingLookup;
				if (paintLookup != 0 || paintCoatingLookup != 0)
				{
					for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
					{
						for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
						{
							Tile tile = Main.tile[i, j];
							if (tile.active() && (false | (paintLookup != 0 && (int)tile.color() != paintLookup) | (paintCoatingLookup == 1 && !tile.fullbrightBlock()) | (paintCoatingLookup == 2 && !tile.invisibleBlock())))
							{
								SmartCursorHelper._targets.Add(new Point(i, j));
							}
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[k];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x00541ACC File Offset: 0x0053FCCC
		private static void Step_PaintRoller(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if ((providedInfo.item.type == 1072 || providedInfo.item.type == 1544) && (providedInfo.paintLookup != 0 || providedInfo.paintCoatingLookup != 0) && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				int paintLookup = providedInfo.paintLookup;
				int paintCoatingLookup = providedInfo.paintCoatingLookup;
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (tile.wall > 0 && (!tile.active() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type]) && (false | (paintLookup != 0 && (int)tile.wallColor() != paintLookup) | (paintCoatingLookup == 1 && !tile.fullbrightWall()) | (paintCoatingLookup == 2 && !tile.invisibleWall())))
						{
							SmartCursorHelper._targets.Add(new Point(i, j));
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[k];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x00541CFC File Offset: 0x0053FEFC
		private static void Step_BlocksLines(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			int type = providedInfo.item.type;
			if (type < 0 || type >= (int)ItemID.Count)
			{
				return;
			}
			if (Player.SmartCursorSettings.SmartBlocksEnabled && providedInfo.item.createTile > -1 && SmartCursorHelper.AllowNormalBlockPlacementBehaviourForItemType(type) && Main.tileSolid[providedInfo.item.createTile] && !Main.tileSolidTop[providedInfo.item.createTile] && !Main.tileFrameImportant[providedInfo.item.createTile] && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				bool flag = false;
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active())
				{
					flag = true;
				}
				if (!Collision.InTileBounds(providedInfo.screenTargetX, providedInfo.screenTargetY, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					flag = true;
				}
				if (!flag)
				{
					for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
					{
						for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
						{
							Tile tile = Main.tile[i, j];
							if (!tile.active() || Main.tileCut[(int)tile.type] || TileID.Sets.BreakableWhenPlacing[(int)tile.type])
							{
								bool flag2 = false;
								if (Main.tile[i - 1, j].active() && Main.tileSolid[(int)Main.tile[i - 1, j].type] && !Main.tileSolidTop[(int)Main.tile[i - 1, j].type])
								{
									flag2 = true;
								}
								if (Main.tile[i + 1, j].active() && Main.tileSolid[(int)Main.tile[i + 1, j].type] && !Main.tileSolidTop[(int)Main.tile[i + 1, j].type])
								{
									flag2 = true;
								}
								if (Main.tile[i, j - 1].active() && Main.tileSolid[(int)Main.tile[i, j - 1].type] && !Main.tileSolidTop[(int)Main.tile[i, j - 1].type])
								{
									flag2 = true;
								}
								if (Main.tile[i, j + 1].active() && Main.tileSolid[(int)Main.tile[i, j + 1].type] && !Main.tileSolidTop[(int)Main.tile[i, j + 1].type])
								{
									flag2 = true;
								}
								if (flag2)
								{
									SmartCursorHelper._targets.Add(new Point(i, j));
								}
							}
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						if (Collision.EmptyTile(SmartCursorHelper._targets[k].X, SmartCursorHelper._targets[k].Y, false))
						{
							float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
							if (num == -1f || num2 < num)
							{
								num = num2;
								point = SmartCursorHelper._targets[k];
							}
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num != -1f)
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x005420EC File Offset: 0x005402EC
		private static void Step_Boulders(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile > -1 && providedInfo.item.createTile < (int)TileID.Count && TileID.Sets.Boulders[providedInfo.item.createTile] && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j + 1];
						Tile tile2 = Main.tile[i + 1, j + 1];
						bool flag = true;
						if (!tile2.nactive() || !tile.nactive())
						{
							flag = false;
						}
						if (tile2.slope() > 0 || tile.slope() > 0 || tile2.halfBrick() || tile.halfBrick())
						{
							flag = false;
						}
						if ((!Main.tileSolid[(int)tile2.type] && !Main.tileTable[(int)tile2.type]) || (!Main.tileSolid[(int)tile.type] && !Main.tileTable[(int)tile.type]))
						{
							flag = false;
						}
						if (Main.tileNoAttach[(int)tile2.type] || Main.tileNoAttach[(int)tile.type])
						{
							flag = false;
						}
						for (int k = i; k <= i + 1; k++)
						{
							for (int l = j - 1; l <= j; l++)
							{
								Tile tile3 = Main.tile[k, l];
								if (tile3.active() && !Main.tileCut[(int)tile3.type])
								{
									flag = false;
								}
							}
						}
						int x = i * 16;
						int y = j * 16 - 16;
						int width = 32;
						int height = 32;
						Rectangle value = new Rectangle(x, y, width, height);
						for (int m = 0; m < 255; m++)
						{
							Player player = Main.player[m];
							if (player.active && !player.dead && player.Hitbox.Intersects(value))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							SmartCursorHelper._targets.Add(new Point(i, j));
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int n = 0; n < SmartCursorHelper._targets.Count; n++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[n].X, (float)SmartCursorHelper._targets[n].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[n];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x00542410 File Offset: 0x00540610
		private static void Step_Pigronata(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile == 454 && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					int num = providedInfo.reachableStartY;
					while (num <= providedInfo.reachableEndY && (double)num <= Main.worldSurface - 2.0)
					{
						bool flag = true;
						for (int j = i - 2; j <= i + 1; j++)
						{
							for (int k = num - 1; k <= num + 2; k++)
							{
								Tile tile = Main.tile[j, k];
								if (k == num - 1)
								{
									if (!WorldGen.SolidTile(tile))
									{
										flag = false;
									}
								}
								else if (tile.active() && (!Main.tileCut[(int)tile.type] || tile.type == 454))
								{
									flag = false;
								}
							}
						}
						if (flag)
						{
							SmartCursorHelper._targets.Add(new Point(i, num));
						}
						num++;
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num2 = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int l = 0; l < SmartCursorHelper._targets.Count; l++)
					{
						float num3 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[l].X, (float)SmartCursorHelper._targets[l].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num2 == -1f || num3 < num2)
						{
							num2 = num3;
							point = SmartCursorHelper._targets[l];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x00542620 File Offset: 0x00540820
		private static void Step_PumpkinSeeds(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile == 254 && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j + 1];
						Tile tile2 = Main.tile[i + 1, j + 1];
						if ((double)j > Main.worldSurface - 2.0)
						{
							break;
						}
						bool flag = true;
						if (!tile2.active() || !tile.active())
						{
							flag = false;
						}
						if (tile2.slope() > 0 || tile.slope() > 0 || tile2.halfBrick() || tile.halfBrick())
						{
							flag = false;
						}
						if (tile2.type != 2 && tile2.type != 477 && tile2.type != 109 && tile2.type != 492)
						{
							flag = false;
						}
						if (tile.type != 2 && tile.type != 477 && tile.type != 109 && tile.type != 492)
						{
							flag = false;
						}
						for (int k = i; k <= i + 1; k++)
						{
							for (int l = j - 1; l <= j; l++)
							{
								Tile tile3 = Main.tile[k, l];
								if (tile3.active() && (tile3.type < 0 || tile3.type >= TileID.Count || Main.tileSolid[(int)tile3.type] || !WorldGen.CanCutTile(k, l, TileCuttingContext.TilePlacement)))
								{
									flag = false;
								}
							}
						}
						if (flag)
						{
							SmartCursorHelper._targets.Add(new Point(i, j));
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int m = 0; m < SmartCursorHelper._targets.Count; m++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[m].X, (float)SmartCursorHelper._targets[m].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[m];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x005428EC File Offset: 0x00540AEC
		private static void Step_Walls(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			int width = providedInfo.player.width;
			int height = providedInfo.player.height;
			if (providedInfo.item.createWall > 0 && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (tile.wall == 0 && (!tile.active() || !Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type]) && Collision.CanHitWithCheck(providedInfo.position, width, height, new Vector2((float)i, (float)j) * 16f, 16, 16, new Utils.TileActionAttempt(DelegateMethods.NotDoorStand)))
						{
							bool flag = false;
							if (Main.tile[i - 1, j].active() || Main.tile[i - 1, j].wall > 0)
							{
								flag = true;
							}
							if (Main.tile[i + 1, j].active() || Main.tile[i + 1, j].wall > 0)
							{
								flag = true;
							}
							if (Main.tile[i, j - 1].active() || Main.tile[i, j - 1].wall > 0)
							{
								flag = true;
							}
							if (Main.tile[i, j + 1].active() || Main.tile[i, j + 1].wall > 0)
							{
								flag = true;
							}
							if (WorldGen.IsOpenDoorAnchorFrame(i, j))
							{
								flag = false;
							}
							if (flag)
							{
								SmartCursorHelper._targets.Add(new Point(i, j));
							}
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[k];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x00542BC8 File Offset: 0x00540DC8
		private static void Step_MinecartTracks(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if ((providedInfo.item.type == 2340 || providedInfo.item.type == 2739) && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				Vector2 value = (Main.MouseWorld - providedInfo.Center).SafeNormalize(Vector2.UnitY);
				float num = Vector2.Dot(value, -Vector2.UnitY);
				bool flag = num >= 0.5f;
				bool flag2 = num <= -0.5f;
				float num2 = Vector2.Dot(value, Vector2.UnitX);
				bool flag3 = num2 >= 0.5f;
				bool flag4 = num2 <= -0.5f;
				bool flag5 = flag && flag4;
				bool flag6 = flag && flag3;
				bool flag7 = flag2 && flag4;
				bool flag8 = flag2 && flag3;
				if (flag5)
				{
					flag4 = false;
				}
				if (flag6)
				{
					flag3 = false;
				}
				if (flag7)
				{
					flag4 = false;
				}
				if (flag8)
				{
					flag3 = false;
				}
				bool flag9 = false;
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type == 314)
				{
					flag9 = true;
				}
				if (!flag9)
				{
					for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
					{
						for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
						{
							Tile tile = Main.tile[i, j];
							if (tile.active() && tile.type == 314)
							{
								bool flag10 = Main.tile[i + 1, j + 1].active() && Main.tile[i + 1, j + 1].type == 314;
								bool flag11 = Main.tile[i + 1, j - 1].active() && Main.tile[i + 1, j - 1].type == 314;
								bool flag12 = Main.tile[i - 1, j + 1].active() && Main.tile[i - 1, j + 1].type == 314;
								bool flag13 = Main.tile[i - 1, j - 1].active() && Main.tile[i - 1, j - 1].type == 314;
								if (flag5 && (!Main.tile[i - 1, j - 1].active() || Main.tileCut[(int)Main.tile[i - 1, j - 1].type] || TileID.Sets.BreakableWhenPlacing[(int)Main.tile[i - 1, j - 1].type]) && (flag10 || !flag11) && !flag12)
								{
									SmartCursorHelper._targets.Add(new Point(i - 1, j - 1));
								}
								if (flag4 && (!Main.tile[i - 1, j].active() || Main.tileCut[(int)Main.tile[i - 1, j].type] || TileID.Sets.BreakableWhenPlacing[(int)Main.tile[i - 1, j].type]))
								{
									SmartCursorHelper._targets.Add(new Point(i - 1, j));
								}
								if (flag7 && (!Main.tile[i - 1, j + 1].active() || Main.tileCut[(int)Main.tile[i - 1, j + 1].type] || TileID.Sets.BreakableWhenPlacing[(int)Main.tile[i - 1, j + 1].type]) && (flag11 || !flag10) && !flag13)
								{
									SmartCursorHelper._targets.Add(new Point(i - 1, j + 1));
								}
								if (flag6 && (!Main.tile[i + 1, j - 1].active() || Main.tileCut[(int)Main.tile[i + 1, j - 1].type] || TileID.Sets.BreakableWhenPlacing[(int)Main.tile[i + 1, j - 1].type]) && (flag12 || !flag13) && !flag10)
								{
									SmartCursorHelper._targets.Add(new Point(i + 1, j - 1));
								}
								if (flag3 && (!Main.tile[i + 1, j].active() || Main.tileCut[(int)Main.tile[i + 1, j].type] || TileID.Sets.BreakableWhenPlacing[(int)Main.tile[i + 1, j].type]))
								{
									SmartCursorHelper._targets.Add(new Point(i + 1, j));
								}
								if (flag8 && (!Main.tile[i + 1, j + 1].active() || Main.tileCut[(int)Main.tile[i + 1, j + 1].type] || TileID.Sets.BreakableWhenPlacing[(int)Main.tile[i + 1, j + 1].type]) && (flag13 || !flag12) && !flag11)
								{
									SmartCursorHelper._targets.Add(new Point(i + 1, j + 1));
								}
							}
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num3 = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						if ((!Main.tile[SmartCursorHelper._targets[k].X, SmartCursorHelper._targets[k].Y - 1].active() || Main.tile[SmartCursorHelper._targets[k].X, SmartCursorHelper._targets[k].Y - 1].type != 314) && (!Main.tile[SmartCursorHelper._targets[k].X, SmartCursorHelper._targets[k].Y + 1].active() || Main.tile[SmartCursorHelper._targets[k].X, SmartCursorHelper._targets[k].Y + 1].type != 314))
						{
							float num4 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
							if (num3 == -1f || num4 < num3)
							{
								num3 = num4;
								point = SmartCursorHelper._targets[k];
							}
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num3 != -1f)
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
			if (providedInfo.item.type == 2492 && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				bool flag14 = false;
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type == 314)
				{
					flag14 = true;
				}
				if (!flag14)
				{
					for (int l = providedInfo.reachableStartX; l <= providedInfo.reachableEndX; l++)
					{
						for (int m = providedInfo.reachableStartY; m <= providedInfo.reachableEndY; m++)
						{
							Tile tile2 = Main.tile[l, m];
							if (tile2.active() && tile2.type == 314)
							{
								if (!Main.tile[l - 1, m].active() || Main.tileCut[(int)Main.tile[l - 1, m].type] || TileID.Sets.BreakableWhenPlacing[(int)Main.tile[l - 1, m].type])
								{
									SmartCursorHelper._targets.Add(new Point(l - 1, m));
								}
								if (!Main.tile[l + 1, m].active() || Main.tileCut[(int)Main.tile[l + 1, m].type] || TileID.Sets.BreakableWhenPlacing[(int)Main.tile[l + 1, m].type])
								{
									SmartCursorHelper._targets.Add(new Point(l + 1, m));
								}
							}
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num5 = -1f;
					Point point2 = SmartCursorHelper._targets[0];
					for (int n = 0; n < SmartCursorHelper._targets.Count; n++)
					{
						if ((!Main.tile[SmartCursorHelper._targets[n].X, SmartCursorHelper._targets[n].Y - 1].active() || Main.tile[SmartCursorHelper._targets[n].X, SmartCursorHelper._targets[n].Y - 1].type != 314) && (!Main.tile[SmartCursorHelper._targets[n].X, SmartCursorHelper._targets[n].Y + 1].active() || Main.tile[SmartCursorHelper._targets[n].X, SmartCursorHelper._targets[n].Y + 1].type != 314))
						{
							float num6 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[n].X, (float)SmartCursorHelper._targets[n].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
							if (num5 == -1f || num6 < num5)
							{
								num5 = num6;
								point2 = SmartCursorHelper._targets[n];
							}
						}
					}
					if (Collision.InTileBounds(point2.X, point2.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num5 != -1f)
					{
						focusedX = point2.X;
						focusedY = point2.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x005436B4 File Offset: 0x005418B4
		private static void Step_Platforms(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile >= 0 && TileID.Sets.Platforms[providedInfo.item.createTile] && focusedX == -1 && focusedY == -1 && !SmartCursorHelper.IsPlatform(providedInfo.screenTargetX, providedInfo.screenTargetY))
			{
				SmartCursorHelper._targets.Clear();
				SmartCursorHelper._points.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Point desiredDirectionFrom = SmartCursorHelper.GetDesiredDirectionFrom(providedInfo.mouse - new Point(i, j).ToWorldCoordinates(8f, 8f));
						bool flag = !SmartCursorHelper.IsPlatform(i, j);
						if (flag && desiredDirectionFrom.Y == 0 && Main.tile[i, j].active() && !WorldGen.SolidTile(i, j, false) && (SmartCursorHelper.IsPlatform(i - 1, j) || SmartCursorHelper.IsPlatform(i + 1, j)))
						{
							flag = false;
						}
						if (!flag)
						{
							int num = (desiredDirectionFrom.X == desiredDirectionFrom.Y) ? 2 : ((desiredDirectionFrom.X == -desiredDirectionFrom.Y) ? 1 : 0);
							if ((num == 0 || (int)Main.tile[i, j].slope() != num) && (desiredDirectionFrom.X != 0 || (!SmartCursorHelper.IsPlatform(i - 1, j + desiredDirectionFrom.Y) && !SmartCursorHelper.IsPlatform(i + 1, j + desiredDirectionFrom.Y))))
							{
								Tile tile = Main.tile[i + desiredDirectionFrom.X, j + desiredDirectionFrom.Y];
								if ((!tile.active() || Main.tileCut[(int)tile.type]) && SmartCursorHelper.AllowedForContinuity(i + desiredDirectionFrom.X, j + desiredDirectionFrom.Y, 2))
								{
									SmartCursorHelper._targets.Add(new Point(i + desiredDirectionFrom.X, j + desiredDirectionFrom.Y));
									SmartCursorHelper._points.Add(new Point(desiredDirectionFrom.X, desiredDirectionFrom.Y));
								}
							}
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num2 = -1f;
					float num3 = -1f;
					Point point = SmartCursorHelper._targets[0];
					Point point2 = SmartCursorHelper._points[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						Point point3 = SmartCursorHelper._targets[k];
						Point point4 = SmartCursorHelper._points[k];
						Vector2 value = providedInfo.mouse - SmartCursorHelper._targets[k].ToWorldCoordinates(8f, 8f);
						float num4 = value.Length();
						float num5 = Vector2.Dot(value, point4.ToVector2());
						if (num2 == -1f || num4 < num2 || (num4 == num2 && num5 > num3))
						{
							num2 = num4;
							num3 = num5;
							point = point3;
							point2 = point4;
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
						SmartCursorHelper._lockedDesiredDirection = new Point?(new Point(point2.X, point2.Y));
						SmartCursorHelper._lockedContinuityCoords = new Point?(new Point(focusedX, focusedY));
					}
				}
				SmartCursorHelper._targets.Clear();
				SmartCursorHelper._points.Clear();
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060023BB RID: 9147 RVA: 0x00543A2E File Offset: 0x00541C2E
		public static Point? LockedDesiredDirection
		{
			get
			{
				return SmartCursorHelper._lockedDesiredDirection;
			}
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x00543A35 File Offset: 0x00541C35
		public static bool TileTargetDesired()
		{
			return SmartCursorHelper._lockedContinuityCoords == null || (Main.SmartCursorShowing && Player.tileTargetX == Main.SmartCursorX && Player.tileTargetY == Main.SmartCursorY);
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x00543A68 File Offset: 0x00541C68
		private static bool AllowedForContinuity(int x, int y, int skipsAllowed)
		{
			if (SmartCursorHelper._lockedContinuityCoords == null)
			{
				return true;
			}
			Point value = SmartCursorHelper._lockedContinuityCoords.Value;
			if (x == value.X && y == value.Y)
			{
				return true;
			}
			if (SmartCursorHelper._lockedDesiredDirection == null)
			{
				return false;
			}
			for (int i = 0; i < skipsAllowed; i++)
			{
				value.X += SmartCursorHelper._lockedDesiredDirection.Value.X;
				value.Y += SmartCursorHelper._lockedDesiredDirection.Value.Y;
				if (x == value.X && y == value.Y)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x00543B08 File Offset: 0x00541D08
		private static Point GetDesiredDirectionFrom(Vector2 offset)
		{
			if (SmartCursorHelper._lockedDesiredDirection != null)
			{
				return SmartCursorHelper._lockedDesiredDirection.Value;
			}
			float num = offset.ToRotation();
			if (num < 0f)
			{
				num += 6.2831855f;
			}
			float num2 = 0.7853982f;
			return (((float)((int)((num + num2 / 2f) % 6.2831855f / num2)) * num2).ToRotationVector2() * 1.5f).ToPoint();
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x00543B72 File Offset: 0x00541D72
		private static bool IsPlatform(int x, int y)
		{
			return Main.tile[x, y].active() && TileID.Sets.Platforms[(int)Main.tile[x, y].type];
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x00543BA0 File Offset: 0x00541DA0
		private static void Step_WireCutter(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.type == 510 && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (tile.wire() || tile.wire2() || tile.wire3() || tile.wire4() || tile.actuator())
						{
							SmartCursorHelper._targets.Add(new Point(i, j));
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[k];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x00543D48 File Offset: 0x00541F48
		private static void Step_ActuationRod(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			bool actuationRodLock = providedInfo.player.ActuationRodLock;
			bool actuationRodLockSetting = providedInfo.player.ActuationRodLockSetting;
			if (providedInfo.item.type == 3620 && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (tile.active() && tile.actuator() && (!actuationRodLock || actuationRodLockSetting == tile.inActive()))
						{
							SmartCursorHelper._targets.Add(new Point(i, j));
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[k];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x00543F04 File Offset: 0x00542104
		private static void Step_Hammers(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			int width = providedInfo.player.width;
			int height = providedInfo.player.height;
			if (providedInfo.item.hammer > 0 && focusedX == -1 && focusedY == -1)
			{
				Vector2 vector = providedInfo.mouse - providedInfo.Center;
				int num = Math.Sign(vector.X);
				int num2 = Math.Sign(vector.Y);
				if (Math.Abs(vector.X) > Math.Abs(vector.Y) * 3f)
				{
					num2 = 0;
					providedInfo.mouse.Y = providedInfo.Center.Y;
				}
				if (Math.Abs(vector.Y) > Math.Abs(vector.X) * 3f)
				{
					num = 0;
					providedInfo.mouse.X = providedInfo.Center.X;
				}
				int num3 = (int)providedInfo.Center.X / 16;
				int num4 = (int)providedInfo.Center.Y / 16;
				SmartCursorHelper._points.Clear();
				SmartCursorHelper._endpoints.Clear();
				int num5 = 1;
				if (num2 == -1 && num != 0)
				{
					num5 = -1;
				}
				int num6 = (int)((providedInfo.position.X + (float)(width / 2) + (float)((width / 2 - 1) * num)) / 16f);
				int num7 = (int)(((double)providedInfo.position.Y + 0.1) / 16.0);
				if (num5 == -1)
				{
					num7 = (int)((providedInfo.position.Y + (float)height - 1f) / 16f);
				}
				int num8 = width / 16 + ((width % 16 == 0) ? 0 : 1);
				int num9 = height / 16 + ((height % 16 == 0) ? 0 : 1);
				if (num != 0)
				{
					for (int i = 0; i < num9; i++)
					{
						if (Main.tile[num6, num7 + i * num5] != null)
						{
							SmartCursorHelper._points.Add(new Point(num6, num7 + i * num5));
						}
					}
				}
				if (num2 != 0)
				{
					for (int j = 0; j < num8; j++)
					{
						if (Main.tile[(int)(providedInfo.position.X / 16f) + j, num7] != null)
						{
							SmartCursorHelper._points.Add(new Point((int)(providedInfo.position.X / 16f) + j, num7));
						}
					}
				}
				int num10 = (int)((providedInfo.mouse.X + (float)((width / 2 - 1) * num)) / 16f);
				int num11 = (int)(((double)providedInfo.mouse.Y + 0.1 - (double)(height / 2 + 1)) / 16.0);
				if (num5 == -1)
				{
					num11 = (int)((providedInfo.mouse.Y + (float)(height / 2) - 1f) / 16f);
				}
				if (providedInfo.player.gravDir == -1f && num2 == 0)
				{
					num11++;
				}
				if (num11 < 10)
				{
					num11 = 10;
				}
				if (num11 > Main.maxTilesY - 10)
				{
					num11 = Main.maxTilesY - 10;
				}
				int num12 = width / 16 + ((width % 16 == 0) ? 0 : 1);
				int num13 = height / 16 + ((height % 16 == 0) ? 0 : 1);
				if (num != 0)
				{
					for (int k = 0; k < num13; k++)
					{
						if (Main.tile[num10, num11 + k * num5] != null)
						{
							SmartCursorHelper._endpoints.Add(new Point(num10, num11 + k * num5));
						}
					}
				}
				if (num2 != 0)
				{
					for (int l = 0; l < num12; l++)
					{
						if (Main.tile[(int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num11] != null)
						{
							SmartCursorHelper._endpoints.Add(new Point((int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num11));
						}
					}
				}
				SmartCursorHelper._targets.Clear();
				while (SmartCursorHelper._points.Count > 0)
				{
					Point point = SmartCursorHelper._points[0];
					Point point2 = SmartCursorHelper._endpoints[0];
					Point point3 = Collision.HitLineWall(point.X, point.Y, point2.X, point2.Y);
					if (point3.X == -1 || point3.Y == -1)
					{
						SmartCursorHelper._points.Remove(point);
						SmartCursorHelper._endpoints.Remove(point2);
					}
					else
					{
						if (point3.X != point2.X || point3.Y != point2.Y)
						{
							SmartCursorHelper._targets.Add(point3);
						}
						Main.tile[point3.X, point3.Y];
						if (Collision.HitWallSubstep(point3.X, point3.Y))
						{
							SmartCursorHelper._targets.Add(point3);
						}
						SmartCursorHelper._points.Remove(point);
						SmartCursorHelper._endpoints.Remove(point2);
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num14 = -1f;
					Point point4 = new Point(-1, -1);
					for (int m = 0; m < SmartCursorHelper._targets.Count; m++)
					{
						if (!Main.tile[SmartCursorHelper._targets[m].X, SmartCursorHelper._targets[m].Y].active() || Main.tile[SmartCursorHelper._targets[m].X, SmartCursorHelper._targets[m].Y].type != 26)
						{
							float num15 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[m].X, (float)SmartCursorHelper._targets[m].Y) * 16f + Vector2.One * 8f, providedInfo.Center);
							if (num14 == -1f || num15 < num14)
							{
								num14 = num15;
								point4 = SmartCursorHelper._targets[m];
							}
						}
					}
					if (point4.X != -1 && Collision.InTileBounds(point4.X, point4.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						providedInfo.player.poundRelease = false;
						focusedX = point4.X;
						focusedY = point4.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
				SmartCursorHelper._points.Clear();
				SmartCursorHelper._endpoints.Clear();
			}
			if (providedInfo.item.hammer > 0 && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				for (int n = providedInfo.reachableStartX; n <= providedInfo.reachableEndX; n++)
				{
					for (int num16 = providedInfo.reachableStartY; num16 <= providedInfo.reachableEndY; num16++)
					{
						if (Main.tile[n, num16].wall > 0 && Collision.HitWallSubstep(n, num16))
						{
							SmartCursorHelper._targets.Add(new Point(n, num16));
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num17 = -1f;
					Point point5 = new Point(-1, -1);
					for (int num18 = 0; num18 < SmartCursorHelper._targets.Count; num18++)
					{
						if (!Main.tile[SmartCursorHelper._targets[num18].X, SmartCursorHelper._targets[num18].Y].active() || Main.tile[SmartCursorHelper._targets[num18].X, SmartCursorHelper._targets[num18].Y].type != 26)
						{
							float num19 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[num18].X, (float)SmartCursorHelper._targets[num18].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
							if (num17 == -1f || num19 < num17)
							{
								num17 = num19;
								point5 = SmartCursorHelper._targets[num18];
							}
						}
					}
					if (point5.X != -1 && Collision.InTileBounds(point5.X, point5.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						providedInfo.player.poundRelease = false;
						focusedX = point5.X;
						focusedY = point5.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x00544770 File Offset: 0x00542970
		private static void Step_MulticolorWrench(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.type == 3625 && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				WiresUI.Settings.MultiToolMode multiToolMode = WiresUI.Settings.ToolMode;
				WiresUI.Settings.MultiToolMode multiToolMode2 = (WiresUI.Settings.MultiToolMode)0;
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire())
				{
					multiToolMode2 |= WiresUI.Settings.MultiToolMode.Red;
				}
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire2())
				{
					multiToolMode2 |= WiresUI.Settings.MultiToolMode.Blue;
				}
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire3())
				{
					multiToolMode2 |= WiresUI.Settings.MultiToolMode.Green;
				}
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire4())
				{
					multiToolMode2 |= WiresUI.Settings.MultiToolMode.Yellow;
				}
				multiToolMode &= ~WiresUI.Settings.MultiToolMode.Cutter;
				bool flag = multiToolMode == multiToolMode2;
				multiToolMode = WiresUI.Settings.ToolMode;
				if (!flag)
				{
					bool flag2 = (multiToolMode & WiresUI.Settings.MultiToolMode.Red) > (WiresUI.Settings.MultiToolMode)0;
					bool flag3 = (multiToolMode & WiresUI.Settings.MultiToolMode.Blue) > (WiresUI.Settings.MultiToolMode)0;
					bool flag4 = (multiToolMode & WiresUI.Settings.MultiToolMode.Green) > (WiresUI.Settings.MultiToolMode)0;
					bool flag5 = (multiToolMode & WiresUI.Settings.MultiToolMode.Yellow) > (WiresUI.Settings.MultiToolMode)0;
					bool flag6 = (multiToolMode & WiresUI.Settings.MultiToolMode.Cutter) > (WiresUI.Settings.MultiToolMode)0;
					for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
					{
						for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
						{
							Tile tile = Main.tile[i, j];
							if (flag6)
							{
								if ((tile.wire() && flag2) || (tile.wire2() && flag3) || (tile.wire3() && flag4) || (tile.wire4() && flag5))
								{
									SmartCursorHelper._targets.Add(new Point(i, j));
								}
							}
							else if ((tile.wire() && flag2) || (tile.wire2() && flag3) || (tile.wire3() && flag4) || (tile.wire4() && flag5))
							{
								if (flag2)
								{
									if (!Main.tile[i - 1, j].wire())
									{
										SmartCursorHelper._targets.Add(new Point(i - 1, j));
									}
									if (!Main.tile[i + 1, j].wire())
									{
										SmartCursorHelper._targets.Add(new Point(i + 1, j));
									}
									if (!Main.tile[i, j - 1].wire())
									{
										SmartCursorHelper._targets.Add(new Point(i, j - 1));
									}
									if (!Main.tile[i, j + 1].wire())
									{
										SmartCursorHelper._targets.Add(new Point(i, j + 1));
									}
								}
								if (flag3)
								{
									if (!Main.tile[i - 1, j].wire2())
									{
										SmartCursorHelper._targets.Add(new Point(i - 1, j));
									}
									if (!Main.tile[i + 1, j].wire2())
									{
										SmartCursorHelper._targets.Add(new Point(i + 1, j));
									}
									if (!Main.tile[i, j - 1].wire2())
									{
										SmartCursorHelper._targets.Add(new Point(i, j - 1));
									}
									if (!Main.tile[i, j + 1].wire2())
									{
										SmartCursorHelper._targets.Add(new Point(i, j + 1));
									}
								}
								if (flag4)
								{
									if (!Main.tile[i - 1, j].wire3())
									{
										SmartCursorHelper._targets.Add(new Point(i - 1, j));
									}
									if (!Main.tile[i + 1, j].wire3())
									{
										SmartCursorHelper._targets.Add(new Point(i + 1, j));
									}
									if (!Main.tile[i, j - 1].wire3())
									{
										SmartCursorHelper._targets.Add(new Point(i, j - 1));
									}
									if (!Main.tile[i, j + 1].wire3())
									{
										SmartCursorHelper._targets.Add(new Point(i, j + 1));
									}
								}
								if (flag5)
								{
									if (!Main.tile[i - 1, j].wire4())
									{
										SmartCursorHelper._targets.Add(new Point(i - 1, j));
									}
									if (!Main.tile[i + 1, j].wire4())
									{
										SmartCursorHelper._targets.Add(new Point(i + 1, j));
									}
									if (!Main.tile[i, j - 1].wire4())
									{
										SmartCursorHelper._targets.Add(new Point(i, j - 1));
									}
									if (!Main.tile[i, j + 1].wire4())
									{
										SmartCursorHelper._targets.Add(new Point(i, j + 1));
									}
								}
							}
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[k];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x00544D1C File Offset: 0x00542F1C
		private static void Step_ColoredWrenches(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if ((providedInfo.item.type == 509 || providedInfo.item.type == 850 || providedInfo.item.type == 851 || providedInfo.item.type == 3612) && focusedX == -1 && focusedY == -1)
			{
				SmartCursorHelper._targets.Clear();
				int num = 0;
				if (providedInfo.item.type == 509)
				{
					num = 1;
				}
				if (providedInfo.item.type == 850)
				{
					num = 2;
				}
				if (providedInfo.item.type == 851)
				{
					num = 3;
				}
				if (providedInfo.item.type == 3612)
				{
					num = 4;
				}
				bool flag = false;
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire() && num == 1)
				{
					flag = true;
				}
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire2() && num == 2)
				{
					flag = true;
				}
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire3() && num == 3)
				{
					flag = true;
				}
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire4() && num == 4)
				{
					flag = true;
				}
				if (!flag)
				{
					for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
					{
						for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
						{
							Tile tile = Main.tile[i, j];
							if ((tile.wire() && num == 1) || (tile.wire2() && num == 2) || (tile.wire3() && num == 3) || (tile.wire4() && num == 4))
							{
								if (num == 1)
								{
									if (!Main.tile[i - 1, j].wire())
									{
										SmartCursorHelper._targets.Add(new Point(i - 1, j));
									}
									if (!Main.tile[i + 1, j].wire())
									{
										SmartCursorHelper._targets.Add(new Point(i + 1, j));
									}
									if (!Main.tile[i, j - 1].wire())
									{
										SmartCursorHelper._targets.Add(new Point(i, j - 1));
									}
									if (!Main.tile[i, j + 1].wire())
									{
										SmartCursorHelper._targets.Add(new Point(i, j + 1));
									}
								}
								if (num == 2)
								{
									if (!Main.tile[i - 1, j].wire2())
									{
										SmartCursorHelper._targets.Add(new Point(i - 1, j));
									}
									if (!Main.tile[i + 1, j].wire2())
									{
										SmartCursorHelper._targets.Add(new Point(i + 1, j));
									}
									if (!Main.tile[i, j - 1].wire2())
									{
										SmartCursorHelper._targets.Add(new Point(i, j - 1));
									}
									if (!Main.tile[i, j + 1].wire2())
									{
										SmartCursorHelper._targets.Add(new Point(i, j + 1));
									}
								}
								if (num == 3)
								{
									if (!Main.tile[i - 1, j].wire3())
									{
										SmartCursorHelper._targets.Add(new Point(i - 1, j));
									}
									if (!Main.tile[i + 1, j].wire3())
									{
										SmartCursorHelper._targets.Add(new Point(i + 1, j));
									}
									if (!Main.tile[i, j - 1].wire3())
									{
										SmartCursorHelper._targets.Add(new Point(i, j - 1));
									}
									if (!Main.tile[i, j + 1].wire3())
									{
										SmartCursorHelper._targets.Add(new Point(i, j + 1));
									}
								}
								if (num == 4)
								{
									if (!Main.tile[i - 1, j].wire4())
									{
										SmartCursorHelper._targets.Add(new Point(i - 1, j));
									}
									if (!Main.tile[i + 1, j].wire4())
									{
										SmartCursorHelper._targets.Add(new Point(i + 1, j));
									}
									if (!Main.tile[i, j - 1].wire4())
									{
										SmartCursorHelper._targets.Add(new Point(i, j - 1));
									}
									if (!Main.tile[i, j + 1].wire4())
									{
										SmartCursorHelper._targets.Add(new Point(i, j + 1));
									}
								}
							}
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num2 = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						float num3 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num2 == -1f || num3 < num2)
						{
							num2 = num3;
							point = SmartCursorHelper._targets[k];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x00545290 File Offset: 0x00543490
		private static void Step_Acorns(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			int num = 9;
			int num2 = 14;
			int num3 = 20;
			if (providedInfo.item.type == 27 && focusedX == -1 && focusedY == -1 && providedInfo.reachableStartY > 20)
			{
				SmartCursorHelper._targets.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						Tile tile2 = Main.tile[i, j - 1];
						Tile tile3 = Main.tile[i, j + 1];
						Tile tile4 = Main.tile[i - 1, j];
						Tile tile5 = Main.tile[i + 1, j];
						Tile tile6 = Main.tile[i - 2, j];
						Tile tile7 = Main.tile[i + 2, j];
						Tile tile8 = Main.tile[i - 3, j];
						Tile tile9 = Main.tile[i + 3, j];
						if ((!tile.active() || Main.tileCut[(int)tile.type] || TileID.Sets.BreakableWhenPlacing[(int)tile.type]) && (!tile2.active() || Main.tileCut[(int)tile2.type] || TileID.Sets.BreakableWhenPlacing[(int)tile2.type]) && tile3.active() && WorldGen.SolidTile2(tile3))
						{
							bool flag = (tile4.active() && TileID.Sets.CommonSapling[(int)tile4.type]) || (tile5.active() && TileID.Sets.CommonSapling[(int)tile5.type]);
							bool flag2 = flag || (tile6.active() && TileID.Sets.CommonSapling[(int)tile6.type]) || (tile7.active() && TileID.Sets.CommonSapling[(int)tile7.type]) || (tile8.active() && TileID.Sets.CommonSapling[(int)tile8.type]) || (tile9.active() && TileID.Sets.CommonSapling[(int)tile9.type]);
							ushort type = tile3.type;
							if (type <= 116)
							{
								if (type <= 53)
								{
									if (type != 2 && type != 23)
									{
										if (type != 53)
										{
											goto IL_37D;
										}
										goto IL_34D;
									}
								}
								else if (type <= 109)
								{
									if (type != 60)
									{
										if (type != 109)
										{
											goto IL_37D;
										}
									}
									else
									{
										if (!flag2 && WorldGen.EmptyTileCheck(i - 2, i + 2, j - num2 + 1, j, 20))
										{
											SmartCursorHelper._targets.Add(new Point(i, j));
											goto IL_37D;
										}
										goto IL_37D;
									}
								}
								else
								{
									if (type != 112 && type != 116)
									{
										goto IL_37D;
									}
									goto IL_34D;
								}
							}
							else if (type <= 234)
							{
								if (type != 147 && type != 199)
								{
									if (type != 234)
									{
										goto IL_37D;
									}
									goto IL_34D;
								}
							}
							else if (type <= 492)
							{
								if (type != 477 && type != 492)
								{
									goto IL_37D;
								}
							}
							else if (type != 633 && type - 661 > 1)
							{
								goto IL_37D;
							}
							if (!flag2 && tile4.liquid == 0 && tile.liquid == 0 && tile5.liquid == 0 && WorldGen.EmptyTileCheck(i - 2, i + 2, j - num + 1, j, 20))
							{
								SmartCursorHelper._targets.Add(new Point(i, j));
								goto IL_37D;
							}
							goto IL_37D;
							IL_34D:
							if (!flag && tile.liquid == 0 && WorldGen.EmptyTileCheck(i, i, j - num3, j, 20))
							{
								SmartCursorHelper._targets.Add(new Point(i, j));
							}
						}
						IL_37D:;
					}
				}
				SmartCursorHelper._toRemove.Clear();
				for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
				{
					bool flag3 = false;
					for (int l = -1; l < 2; l += 2)
					{
						Tile tile10 = Main.tile[SmartCursorHelper._targets[k].X + l, SmartCursorHelper._targets[k].Y + 1];
						if (tile10.active())
						{
							ushort type = tile10.type;
							if (type <= 116)
							{
								if (type <= 53)
								{
									if (type != 2 && type != 23 && type != 53)
									{
										goto IL_4A6;
									}
								}
								else if (type <= 109)
								{
									if (type != 60 && type != 109)
									{
										goto IL_4A6;
									}
								}
								else if (type != 112 && type != 116)
								{
									goto IL_4A6;
								}
							}
							else if (type <= 234)
							{
								if (type != 147 && type != 199 && type != 234)
								{
									goto IL_4A6;
								}
							}
							else if (type <= 492)
							{
								if (type != 477 && type != 492)
								{
									goto IL_4A6;
								}
							}
							else if (type != 633 && type - 661 > 1)
							{
								goto IL_4A6;
							}
							flag3 = true;
						}
						IL_4A6:;
					}
					if (!flag3)
					{
						SmartCursorHelper._toRemove.Add(SmartCursorHelper._targets[k]);
					}
				}
				for (int m = 0; m < SmartCursorHelper._toRemove.Count; m++)
				{
					SmartCursorHelper._targets.Remove(SmartCursorHelper._toRemove[m]);
				}
				SmartCursorHelper._toRemove.Clear();
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num4 = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int n = 0; n < SmartCursorHelper._targets.Count; n++)
					{
						float num5 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[n].X, (float)SmartCursorHelper._targets[n].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num4 == -1f || num5 < num4)
						{
							num4 = num5;
							point = SmartCursorHelper._targets[n];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023C6 RID: 9158 RVA: 0x005458BC File Offset: 0x00543ABC
		private static void Step_GemCorns(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			WorldGen.GrowTreeSettings growTreeSettings;
			if (!WorldGen.GrowTreeSettings.Profiles.TryGetFromItemId(providedInfo.item.type, out growTreeSettings))
			{
				return;
			}
			if (focusedX == -1 && focusedY == -1 && providedInfo.reachableStartY > 20)
			{
				SmartCursorHelper._targets.Clear();
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						Tile tile2 = Main.tile[i, j - 1];
						Tile tile3 = Main.tile[i, j + 1];
						Tile tile4 = Main.tile[i - 1, j];
						Tile tile5 = Main.tile[i + 1, j];
						Tile tile6 = Main.tile[i - 2, j];
						Tile tile7 = Main.tile[i + 2, j];
						Tile tile8 = Main.tile[i - 3, j];
						Tile tile9 = Main.tile[i + 3, j];
						if (growTreeSettings.GroundTest((int)tile3.type) && (!tile.active() || Main.tileCut[(int)tile.type] || TileID.Sets.BreakableWhenPlacing[(int)tile.type]) && (!tile2.active() || Main.tileCut[(int)tile2.type] || TileID.Sets.BreakableWhenPlacing[(int)tile2.type]) && (!tile4.active() || !TileID.Sets.CommonSapling[(int)tile4.type]) && (!tile5.active() || !TileID.Sets.CommonSapling[(int)tile5.type]) && (!tile6.active() || !TileID.Sets.CommonSapling[(int)tile6.type]) && (!tile7.active() || !TileID.Sets.CommonSapling[(int)tile7.type]) && (!tile8.active() || !TileID.Sets.CommonSapling[(int)tile8.type]) && (!tile9.active() || !TileID.Sets.CommonSapling[(int)tile9.type]) && tile3.active() && WorldGen.SolidTile2(tile3) && tile4.liquid == 0 && tile.liquid == 0 && tile5.liquid == 0 && WorldGen.EmptyTileCheck(i - 2, i + 2, j - growTreeSettings.TreeHeightMax, j, (int)growTreeSettings.SaplingTileType))
						{
							SmartCursorHelper._targets.Add(new Point(i, j));
						}
					}
				}
				SmartCursorHelper._toRemove.Clear();
				for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
				{
					bool flag = false;
					for (int l = -1; l < 2; l += 2)
					{
						Tile tile10 = Main.tile[SmartCursorHelper._targets[k].X + l, SmartCursorHelper._targets[k].Y + 1];
						if (tile10.active() && growTreeSettings.GroundTest((int)tile10.type))
						{
							flag = true;
						}
					}
					if (!flag)
					{
						SmartCursorHelper._toRemove.Add(SmartCursorHelper._targets[k]);
					}
				}
				for (int m = 0; m < SmartCursorHelper._toRemove.Count; m++)
				{
					SmartCursorHelper._targets.Remove(SmartCursorHelper._toRemove[m]);
				}
				SmartCursorHelper._toRemove.Clear();
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int n = 0; n < SmartCursorHelper._targets.Count; n++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[n].X, (float)SmartCursorHelper._targets[n].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[n];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point.X;
						focusedY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x00545D08 File Offset: 0x00543F08
		private static void Step_ForceCursorToAnyMinableThing(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
		{
			int reachableStartX = providedInfo.reachableStartX;
			int reachableStartY = providedInfo.reachableStartY;
			int reachableEndX = providedInfo.reachableEndX;
			int reachableEndY = providedInfo.reachableEndY;
			int screenTargetX = providedInfo.screenTargetX;
			int screenTargetY = providedInfo.screenTargetY;
			Vector2 mouse = providedInfo.mouse;
			Item item = providedInfo.item;
			if (fX != -1 || fY != -1)
			{
				return;
			}
			if (PlayerInput.UsingGamepad)
			{
				return;
			}
			Point point = mouse.ToTileCoordinates();
			int x = point.X;
			int y = point.Y;
			if (!Collision.InTileBounds(x, y, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
			{
				return;
			}
			Tile tile = Main.tile[x, y];
			bool flag = tile.active() && WorldGen.CanKillTile(x, y) && (!Main.tileSolid[(int)tile.type] || Main.tileSolidTop[(int)tile.type]);
			if (flag && Main.tileAxe[(int)tile.type] && item.axe < 1)
			{
				flag = false;
			}
			if (flag && Main.tileHammer[(int)tile.type] && item.hammer < 1)
			{
				flag = false;
			}
			if (flag && !Main.tileHammer[(int)tile.type] && !Main.tileAxe[(int)tile.type] && item.pick < 1)
			{
				flag = false;
			}
			if (!flag)
			{
				return;
			}
			fX = x;
			fY = y;
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x00545E4C File Offset: 0x0054404C
		public static void Step_Pickaxe_MineShinies(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
		{
			int reachableStartX = providedInfo.reachableStartX;
			int reachableStartY = providedInfo.reachableStartY;
			int reachableEndX = providedInfo.reachableEndX;
			int reachableEndY = providedInfo.reachableEndY;
			int screenTargetX = providedInfo.screenTargetX;
			int screenTargetY = providedInfo.screenTargetY;
			Item item = providedInfo.item;
			Vector2 mouse = providedInfo.mouse;
			if (item.pick <= 0 || fX != -1 || fY != -1)
			{
				return;
			}
			SmartCursorHelper._targets.Clear();
			if (item.type != 1333 && item.type != 523)
			{
				bool flag = item.type != 4384;
			}
			int num = 0;
			for (int i = reachableStartX; i <= reachableEndX; i++)
			{
				for (int j = reachableStartY; j <= reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					Main.tile[i - 1, j];
					Main.tile[i + 1, j];
					Main.tile[i, j + 1];
					if (tile.active())
					{
						int num2 = TileID.Sets.SmartCursorPickaxePriorityOverride[(int)tile.type];
						if (num2 > 0)
						{
							if (num < num2)
							{
								num = num2;
							}
							SmartCursorHelper._targets.Add(new Point(i, j));
						}
					}
				}
			}
			SmartCursorHelper._targets2.Clear();
			foreach (Point point in SmartCursorHelper._targets2)
			{
				Tile tile2 = Main.tile[point.X, point.Y];
				if (TileID.Sets.SmartCursorPickaxePriorityOverride[(int)tile2.type] < num)
				{
					SmartCursorHelper._targets2.Add(point);
				}
			}
			foreach (Point item2 in SmartCursorHelper._targets2)
			{
				SmartCursorHelper._targets.Remove(item2);
			}
			if (SmartCursorHelper._targets.Count > 0)
			{
				float num3 = -1f;
				Point point2 = SmartCursorHelper._targets[0];
				for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
				{
					float num4 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, mouse);
					if (num3 == -1f || num4 < num3)
					{
						num3 = num4;
						point2 = SmartCursorHelper._targets[k];
					}
				}
				if (Collision.InTileBounds(point2.X, point2.Y, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
				{
					fX = point2.X;
					fY = point2.Y;
				}
			}
			SmartCursorHelper._targets.Clear();
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x00546144 File Offset: 0x00544344
		public static void Step_Pickaxe_MineSolids(Player player, Vector2 position, Vector2 Center, int width, int direction, SmartCursorHelper.SmartCursorUsageInfo providedInfo, List<Point> grappleTargets, ref int focusedX, ref int focusedY)
		{
			int height = player.height;
			float gravDir = player.gravDir;
			int whoAmI = player.whoAmI;
			if (providedInfo.item.pick > 0 && focusedX == -1 && focusedY == -1)
			{
				if (PlayerInput.UsingGamepad)
				{
					Vector2 navigatorDirections = PlayerInput.Triggers.Current.GetNavigatorDirections();
					Vector2 gamepadThumbstickLeft = PlayerInput.GamepadThumbstickLeft;
					Vector2 gamepadThumbstickRight = PlayerInput.GamepadThumbstickRight;
					if (navigatorDirections == Vector2.Zero && gamepadThumbstickLeft.Length() < 0.05f && gamepadThumbstickRight.Length() < 0.05f)
					{
						providedInfo.mouse = Center + new Vector2((float)(direction * 1000), 0f);
					}
				}
				Vector2 vector = providedInfo.mouse - Center;
				int num = Math.Sign(vector.X);
				int num2 = Math.Sign(vector.Y);
				if (Math.Abs(vector.X) > Math.Abs(vector.Y) * 3f)
				{
					num2 = 0;
					providedInfo.mouse.Y = Center.Y;
				}
				if (Math.Abs(vector.Y) > Math.Abs(vector.X) * 3f)
				{
					num = 0;
					providedInfo.mouse.X = Center.X;
				}
				int num3 = (int)Center.X / 16;
				int num4 = (int)Center.Y / 16;
				SmartCursorHelper._points.Clear();
				SmartCursorHelper._endpoints.Clear();
				int num5 = 1;
				if (num2 == -1 && num != 0)
				{
					num5 = -1;
				}
				int num6 = (int)((position.X + (float)(width / 2) + (float)((width / 2 - 1) * num)) / 16f);
				int num7 = (int)(((double)position.Y + 0.1) / 16.0);
				if (num5 == -1)
				{
					num7 = (int)((position.Y + (float)height - 1f) / 16f);
				}
				int num8 = width / 16 + ((width % 16 == 0) ? 0 : 1);
				int num9 = height / 16 + ((height % 16 == 0) ? 0 : 1);
				if (num != 0)
				{
					for (int i = 0; i < num9; i++)
					{
						if (Main.tile[num6, num7 + i * num5] != null)
						{
							SmartCursorHelper._points.Add(new Point(num6, num7 + i * num5));
						}
					}
				}
				if (num2 != 0)
				{
					for (int j = 0; j < num8; j++)
					{
						if (Main.tile[(int)(position.X / 16f) + j, num7] != null)
						{
							SmartCursorHelper._points.Add(new Point((int)(position.X / 16f) + j, num7));
						}
					}
				}
				int num10 = (int)((providedInfo.mouse.X + (float)((width / 2 - 1) * num)) / 16f);
				int num11 = (int)(((double)providedInfo.mouse.Y + 0.1 - (double)(height / 2 + 1)) / 16.0);
				if (num5 == -1)
				{
					num11 = (int)((providedInfo.mouse.Y + (float)(height / 2) - 1f) / 16f);
				}
				if (gravDir == -1f && num2 == 0)
				{
					num11++;
				}
				if (gravDir == 1f && num == 0)
				{
					num11++;
				}
				if (num11 < 10)
				{
					num11 = 10;
				}
				if (num11 > Main.maxTilesY - 10)
				{
					num11 = Main.maxTilesY - 10;
				}
				int num12 = width / 16 + ((width % 16 == 0) ? 0 : 1);
				int num13 = height / 16 + ((height % 16 == 0) ? 0 : 1);
				if (WorldGen.InWorld(num10, num11, 40))
				{
					if (num != 0)
					{
						for (int k = 0; k < num13; k++)
						{
							if (Main.tile[num10, num11 + k * num5] != null)
							{
								SmartCursorHelper._endpoints.Add(new Point(num10, num11 + k * num5));
							}
						}
					}
					if (num2 != 0)
					{
						for (int l = 0; l < num12; l++)
						{
							if (Main.tile[(int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num11] != null)
							{
								SmartCursorHelper._endpoints.Add(new Point((int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num11));
							}
						}
					}
				}
				SmartCursorHelper._targets.Clear();
				while (SmartCursorHelper._points.Count > 0 && SmartCursorHelper._endpoints.Count > 0)
				{
					Point point = SmartCursorHelper._points[0];
					Point point2 = SmartCursorHelper._endpoints[0];
					Point point3;
					if (!Collision.HitLine(point.X, point.Y, point2.X, point2.Y, num * (int)gravDir, -num2 * (int)gravDir, grappleTargets, out point3))
					{
						SmartCursorHelper._points.Remove(point);
						SmartCursorHelper._endpoints.Remove(point2);
					}
					else
					{
						if (point3.X != point2.X || point3.Y != point2.Y)
						{
							SmartCursorHelper._targets.Add(point3);
						}
						Tile tile = Main.tile[point3.X, point3.Y];
						if (!tile.inActive() && tile.active() && Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type] && !grappleTargets.Contains(point3))
						{
							SmartCursorHelper._targets.Add(point3);
						}
						SmartCursorHelper._points.Remove(point);
						SmartCursorHelper._endpoints.Remove(point2);
					}
				}
				SmartCursorHelper._toRemove.Clear();
				for (int m = 0; m < SmartCursorHelper._targets.Count; m++)
				{
					if (!WorldGen.CanKillTile(SmartCursorHelper._targets[m].X, SmartCursorHelper._targets[m].Y))
					{
						SmartCursorHelper._toRemove.Add(SmartCursorHelper._targets[m]);
					}
				}
				for (int n = 0; n < SmartCursorHelper._toRemove.Count; n++)
				{
					SmartCursorHelper._targets.Remove(SmartCursorHelper._toRemove[n]);
				}
				SmartCursorHelper._toRemove.Clear();
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num14 = -1f;
					Point point4 = SmartCursorHelper._targets[0];
					Vector2 value = Center;
					if (Main.netMode == 1)
					{
						int num15 = 0;
						int num16 = 0;
						int num17 = 0;
						for (int num18 = 0; num18 < whoAmI; num18++)
						{
							Player player2 = Main.player[num18];
							if (player2.active && !player2.dead && player2.HeldItem.pick > 0 && player2.itemAnimation > 0)
							{
								if (player.Distance(player2.Center) <= 8f)
								{
									num15++;
								}
								if (player.Distance(player2.Center) <= 80f && Math.Abs(player2.Center.Y - Center.Y) <= 12f)
								{
									num16++;
								}
							}
						}
						for (int num19 = whoAmI + 1; num19 < 255; num19++)
						{
							Player player3 = Main.player[num19];
							if (player3.active && !player3.dead && player3.HeldItem.pick > 0 && player3.itemAnimation > 0 && player.Distance(player3.Center) <= 8f)
							{
								num17++;
							}
						}
						if (num15 > 0)
						{
							if (num15 % 2 == 1)
							{
								value.X += 12f;
							}
							else
							{
								value.X -= 12f;
							}
							if (num16 % 2 == 1)
							{
								value.Y -= 12f;
							}
						}
						if (num17 > 0 && num15 == 0)
						{
							if (num17 % 2 == 1)
							{
								value.X -= 12f;
							}
							else
							{
								value.X += 12f;
							}
						}
					}
					for (int num20 = 0; num20 < SmartCursorHelper._targets.Count; num20++)
					{
						float num21 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[num20].X, (float)SmartCursorHelper._targets[num20].Y) * 16f + Vector2.One * 8f, value);
						if (num14 == -1f || num21 < num14)
						{
							num14 = num21;
							point4 = SmartCursorHelper._targets[num20];
						}
					}
					if (Collision.InTileBounds(point4.X, point4.Y, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						focusedX = point4.X;
						focusedY = point4.Y;
					}
				}
				SmartCursorHelper._points.Clear();
				SmartCursorHelper._endpoints.Clear();
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x005469E4 File Offset: 0x00544BE4
		public static void Step_Axe(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
		{
			int reachableStartX = providedInfo.reachableStartX;
			int reachableStartY = providedInfo.reachableStartY;
			int reachableEndX = providedInfo.reachableEndX;
			int reachableEndY = providedInfo.reachableEndY;
			int screenTargetX = providedInfo.screenTargetX;
			int screenTargetY = providedInfo.screenTargetY;
			if (providedInfo.item.axe > 0 && fX == -1 && fY == -1)
			{
				float num = -1f;
				for (int i = reachableStartX; i <= reachableEndX; i++)
				{
					for (int j = reachableStartY; j <= reachableEndY; j++)
					{
						if (Main.tile[i, j].active())
						{
							Tile tile = Main.tile[i, j];
							if (Main.tileAxe[(int)tile.type] && !TileID.Sets.IgnoreSmartCursorPriorityAxe[(int)tile.type])
							{
								int num2 = i;
								int num3 = j;
								int type = (int)tile.type;
								if (TileID.Sets.IsATreeTrunk[type])
								{
									if (Collision.InTileBounds(num2 + 1, num3, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
									{
										if (Main.tile[num2, num3].frameY >= 198 && Main.tile[num2, num3].frameX == 44)
										{
											num2++;
										}
										if (Main.tile[num2, num3].frameX == 66 && Main.tile[num2, num3].frameY <= 44)
										{
											num2++;
										}
										if (Main.tile[num2, num3].frameX == 44 && Main.tile[num2, num3].frameY >= 132 && Main.tile[num2, num3].frameY <= 176)
										{
											num2++;
										}
									}
									if (Collision.InTileBounds(num2 - 1, num3, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
									{
										if (Main.tile[num2, num3].frameY >= 198 && Main.tile[num2, num3].frameX == 66)
										{
											num2--;
										}
										if (Main.tile[num2, num3].frameX == 88 && Main.tile[num2, num3].frameY >= 66 && Main.tile[num2, num3].frameY <= 110)
										{
											num2--;
										}
										if (Main.tile[num2, num3].frameX == 22 && Main.tile[num2, num3].frameY >= 132 && Main.tile[num2, num3].frameY <= 176)
										{
											num2--;
										}
									}
									while (Main.tile[num2, num3].active() && (int)Main.tile[num2, num3].type == type && (int)Main.tile[num2, num3 + 1].type == type && Collision.InTileBounds(num2, num3 + 1, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
									{
										num3++;
									}
								}
								if (tile.type == 80)
								{
									if (Collision.InTileBounds(num2 + 1, num3, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
									{
										if (Main.tile[num2, num3].frameX == 54)
										{
											num2++;
										}
										if (Main.tile[num2, num3].frameX == 108 && Main.tile[num2, num3].frameY == 36)
										{
											num2++;
										}
									}
									if (Collision.InTileBounds(num2 - 1, num3, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
									{
										if (Main.tile[num2, num3].frameX == 36)
										{
											num2--;
										}
										if (Main.tile[num2, num3].frameX == 108 && Main.tile[num2, num3].frameY == 18)
										{
											num2--;
										}
									}
									while (Main.tile[num2, num3].active() && Main.tile[num2, num3].type == 80 && Main.tile[num2, num3 + 1].type == 80 && Collision.InTileBounds(num2, num3 + 1, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
									{
										num3++;
									}
								}
								if (tile.type != 323)
								{
									if (tile.type != 72)
									{
										goto IL_4C4;
									}
								}
								while (Main.tile[num2, num3].active() && ((Main.tile[num2, num3].type == 323 && Main.tile[num2, num3 + 1].type == 323) || (Main.tile[num2, num3].type == 72 && Main.tile[num2, num3 + 1].type == 72)) && Collision.InTileBounds(num2, num3 + 1, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
								{
									num3++;
								}
								IL_4C4:
								float num4 = Vector2.Distance(new Vector2((float)num2, (float)num3) * 16f + Vector2.One * 8f, providedInfo.mouse);
								if (num == -1f || num4 < num)
								{
									num = num4;
									fX = num2;
									fY = num3;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x00546F24 File Offset: 0x00545124
		private static void Step_BlocksFilling(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
		{
			if (!Player.SmartCursorSettings.SmartBlocksEnabled)
			{
				return;
			}
			int type = providedInfo.item.type;
			if (type < 0 || type >= (int)ItemID.Count)
			{
				return;
			}
			int reachableStartX = providedInfo.reachableStartX;
			int reachableStartY = providedInfo.reachableStartY;
			int reachableEndX = providedInfo.reachableEndX;
			int reachableEndY = providedInfo.reachableEndY;
			int screenTargetX = providedInfo.screenTargetX;
			int screenTargetY = providedInfo.screenTargetY;
			if (!Player.SmartCursorSettings.SmartBlocksEnabled && providedInfo.item.createTile > -1 && SmartCursorHelper.AllowNormalBlockPlacementBehaviourForItemType(type) && Main.tileSolid[providedInfo.item.createTile] && !Main.tileSolidTop[providedInfo.item.createTile] && !Main.tileFrameImportant[providedInfo.item.createTile] && fX == -1 && fY == -1)
			{
				SmartCursorHelper._targets.Clear();
				bool flag = false;
				if (Main.tile[screenTargetX, screenTargetY].active())
				{
					flag = true;
				}
				if (!Collision.InTileBounds(screenTargetX, screenTargetY, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
				{
					flag = true;
				}
				if (!flag)
				{
					for (int i = reachableStartX; i <= reachableEndX; i++)
					{
						for (int j = reachableStartY; j <= reachableEndY; j++)
						{
							Tile tile = Main.tile[i, j];
							if (!tile.active() || Main.tileCut[(int)tile.type] || TileID.Sets.BreakableWhenPlacing[(int)tile.type])
							{
								int num = 0;
								if (Main.tile[i - 1, j].active() && Main.tileSolid[(int)Main.tile[i - 1, j].type] && !Main.tileSolidTop[(int)Main.tile[i - 1, j].type])
								{
									num++;
								}
								if (Main.tile[i + 1, j].active() && Main.tileSolid[(int)Main.tile[i + 1, j].type] && !Main.tileSolidTop[(int)Main.tile[i + 1, j].type])
								{
									num++;
								}
								if (Main.tile[i, j - 1].active() && Main.tileSolid[(int)Main.tile[i, j - 1].type] && !Main.tileSolidTop[(int)Main.tile[i, j - 1].type])
								{
									num++;
								}
								if (Main.tile[i, j + 1].active() && Main.tileSolid[(int)Main.tile[i, j + 1].type] && !Main.tileSolidTop[(int)Main.tile[i, j + 1].type])
								{
									num++;
								}
								if (num >= 2)
								{
									SmartCursorHelper._targets.Add(new Point(i, j));
								}
							}
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num2 = -1f;
					float num3 = float.PositiveInfinity;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						if (Collision.EmptyTile(SmartCursorHelper._targets[k].X, SmartCursorHelper._targets[k].Y, true))
						{
							Vector2 vector = new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f - providedInfo.mouse;
							bool flag2 = false;
							float num4 = Math.Abs(vector.X);
							float num5 = vector.Length();
							if (num4 < num3)
							{
								flag2 = true;
							}
							if (num4 == num3 && (num2 == -1f || num5 < num2))
							{
								flag2 = true;
							}
							if (flag2)
							{
								num2 = num5;
								num3 = num4;
								point = SmartCursorHelper._targets[k];
							}
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, reachableStartX, reachableStartY, reachableEndX, reachableEndY) && num2 != -1f)
					{
						fX = point.X;
						fY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x00547374 File Offset: 0x00545574
		private static void Step_Torch(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
		{
			int reachableStartX = providedInfo.reachableStartX;
			int reachableStartY = providedInfo.reachableStartY;
			int reachableEndX = providedInfo.reachableEndX;
			int reachableEndY = providedInfo.reachableEndY;
			int screenTargetX = providedInfo.screenTargetX;
			int screenTargetY = providedInfo.screenTargetY;
			int type = providedInfo.item.type;
			if (type >= 0 && type < (int)ItemID.Count && ItemID.Sets.Torches[type] && fX == -1 && fY == -1)
			{
				SmartCursorHelper._targets.Clear();
				bool flag = !ItemID.Sets.WaterTorches[type];
				for (int i = reachableStartX; i <= reachableEndX; i++)
				{
					for (int j = reachableStartY; j <= reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if ((!flag || tile.liquid <= 0) && (!tile.active() || TileID.Sets.BreakableWhenPlacing[(int)tile.type] || (Main.tileCut[(int)tile.type] && tile.type != 82 && tile.type != 83)))
						{
							bool flag2 = false;
							for (int k = i - 8; k <= i + 8; k++)
							{
								for (int l = j - 8; l <= j + 8; l++)
								{
									if (Main.tile[k, l] != null)
									{
										Tile tile2 = Main.tile[k, l];
										if (TileID.Sets.Torches[(int)tile2.type])
										{
											flag2 = true;
											break;
										}
									}
								}
								if (flag2)
								{
									break;
								}
							}
							if (!flag2 && SmartCursorHelper.IsValidSpotForTorch(i, j, tile))
							{
								SmartCursorHelper._targets.Add(new Point(i, j));
							}
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int m = 0; m < SmartCursorHelper._targets.Count; m++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[m].X, (float)SmartCursorHelper._targets[m].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[m];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
					{
						fX = point.X;
						fY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x00547608 File Offset: 0x00545808
		private static bool IsValidSpotForTorch(int x, int y, Tile tileCache)
		{
			if (tileCache.wall > 0)
			{
				return true;
			}
			if (TileID.Sets.Torches[(int)tileCache.type])
			{
				return false;
			}
			Tile tile = Main.tile[x - 1, y];
			if (tile.active() && (tile.slope() == 0 || tile.slope() % 2 != 1) && ((Main.tileSolid[(int)tile.type] && !Main.tileNoAttach[(int)tile.type] && !Main.tileSolidTop[(int)tile.type] && !TileID.Sets.NotReallySolid[(int)tile.type]) || TileID.Sets.IsBeam[(int)tile.type] || (WorldGen.IsTreeType((int)tile.type) && WorldGen.IsTreeType((int)Main.tile[x - 1, y - 1].type) && WorldGen.IsTreeType((int)Main.tile[x - 1, y + 1].type))))
			{
				return true;
			}
			Tile tile2 = Main.tile[x + 1, y];
			if (tile2.active() && (tile2.slope() == 0 || tile2.slope() % 2 != 0) && ((Main.tileSolid[(int)tile2.type] && !Main.tileNoAttach[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type] && !TileID.Sets.NotReallySolid[(int)tile2.type]) || TileID.Sets.IsBeam[(int)tile2.type] || (WorldGen.IsTreeType((int)tile2.type) && WorldGen.IsTreeType((int)Main.tile[x + 1, y - 1].type) && WorldGen.IsTreeType((int)Main.tile[x + 1, y + 1].type))))
			{
				return true;
			}
			Tile tile3 = Main.tile[x, y + 1];
			return tile3.active() && tile3.slope() == 0 && !tile3.halfBrick() && (((Main.tileSolid[(int)tile3.type] && !Main.tileSolidTop[(int)tile3.type]) || TileID.Sets.Platforms[(int)tile3.type]) && !TileID.Sets.NotReallySolid[(int)tile3.type]);
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x00547824 File Offset: 0x00545A24
		private static void Step_LawnMower(SmartCursorHelper.SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
		{
			int reachableStartX = providedInfo.reachableStartX;
			int reachableStartY = providedInfo.reachableStartY;
			int reachableEndX = providedInfo.reachableEndX;
			int reachableEndY = providedInfo.reachableEndY;
			int screenTargetX = providedInfo.screenTargetX;
			int screenTargetY = providedInfo.screenTargetY;
			if (providedInfo.item.type == 4049 && fX == -1 && fY == -1)
			{
				SmartCursorHelper._targets.Clear();
				for (int i = reachableStartX; i <= reachableEndX; i++)
				{
					for (int j = reachableStartY; j <= reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (tile.active() && (tile.type == 2 || tile.type == 109))
						{
							SmartCursorHelper._targets.Add(new Point(i, j));
						}
					}
				}
				if (SmartCursorHelper._targets.Count > 0)
				{
					float num = -1f;
					Point point = SmartCursorHelper._targets[0];
					for (int k = 0; k < SmartCursorHelper._targets.Count; k++)
					{
						float num2 = Vector2.Distance(new Vector2((float)SmartCursorHelper._targets[k].X, (float)SmartCursorHelper._targets[k].Y) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							point = SmartCursorHelper._targets[k];
						}
					}
					if (Collision.InTileBounds(point.X, point.Y, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
					{
						fX = point.X;
						fY = point.Y;
					}
				}
				SmartCursorHelper._targets.Clear();
			}
		}

		// Token: 0x04004D82 RID: 19842
		private static List<Point> _targets = new List<Point>();

		// Token: 0x04004D83 RID: 19843
		private static List<Point> _grappleTargets = new List<Point>();

		// Token: 0x04004D84 RID: 19844
		private static List<Point> _points = new List<Point>();

		// Token: 0x04004D85 RID: 19845
		private static List<Point> _endpoints = new List<Point>();

		// Token: 0x04004D86 RID: 19846
		private static List<Point> _toRemove = new List<Point>();

		// Token: 0x04004D87 RID: 19847
		private static List<Point> _targets2 = new List<Point>();

		// Token: 0x04004D88 RID: 19848
		private static Point? _lockedDesiredDirection;

		// Token: 0x04004D89 RID: 19849
		private static Point? _lockedContinuityCoords;

		// Token: 0x020007E9 RID: 2025
		public class SmartCursorUsageInfo
		{
			// Token: 0x0400710E RID: 28942
			public Player player;

			// Token: 0x0400710F RID: 28943
			public Item item;

			// Token: 0x04007110 RID: 28944
			public Vector2 mouse;

			// Token: 0x04007111 RID: 28945
			public Vector2 position;

			// Token: 0x04007112 RID: 28946
			public Vector2 Center;

			// Token: 0x04007113 RID: 28947
			public int screenTargetX;

			// Token: 0x04007114 RID: 28948
			public int screenTargetY;

			// Token: 0x04007115 RID: 28949
			public int reachableStartX;

			// Token: 0x04007116 RID: 28950
			public int reachableEndX;

			// Token: 0x04007117 RID: 28951
			public int reachableStartY;

			// Token: 0x04007118 RID: 28952
			public int reachableEndY;

			// Token: 0x04007119 RID: 28953
			public int paintLookup;

			// Token: 0x0400711A RID: 28954
			public int paintCoatingLookup;
		}
	}
}
