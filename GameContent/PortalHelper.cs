using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent
{
	// Token: 0x02000280 RID: 640
	public class PortalHelper
	{
		// Token: 0x06002478 RID: 9336 RVA: 0x0054D57C File Offset: 0x0054B77C
		static PortalHelper()
		{
			for (int i = 0; i < PortalHelper.SLOPE_EDGES.Length; i++)
			{
				PortalHelper.SLOPE_EDGES[i].Normalize();
			}
			for (int j = 0; j < PortalHelper.FoundPortals.GetLength(0); j++)
			{
				PortalHelper.FoundPortals[j, 0] = -1;
				PortalHelper.FoundPortals[j, 1] = -1;
			}
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x0054D718 File Offset: 0x0054B918
		public static void UpdatePortalPoints()
		{
			PortalHelper.anyPortalAtAll = false;
			for (int i = 0; i < PortalHelper.FoundPortals.GetLength(0); i++)
			{
				PortalHelper.FoundPortals[i, 0] = -1;
				PortalHelper.FoundPortals[i, 1] = -1;
			}
			for (int j = 0; j < PortalHelper.PortalCooldownForPlayers.Length; j++)
			{
				if (PortalHelper.PortalCooldownForPlayers[j] > 0)
				{
					PortalHelper.PortalCooldownForPlayers[j]--;
				}
			}
			for (int k = 0; k < PortalHelper.PortalCooldownForNPCs.Length; k++)
			{
				if (PortalHelper.PortalCooldownForNPCs[k] > 0)
				{
					PortalHelper.PortalCooldownForNPCs[k]--;
				}
			}
			for (int l = 0; l < 1000; l++)
			{
				Projectile projectile = Main.projectile[l];
				if (projectile.active && projectile.type == 602 && projectile.ai[1] >= 0f && projectile.ai[1] <= 1f && projectile.owner >= 0 && projectile.owner <= 255)
				{
					PortalHelper.FoundPortals[projectile.owner, (int)projectile.ai[1]] = l;
					if (PortalHelper.FoundPortals[projectile.owner, 0] != -1 && PortalHelper.FoundPortals[projectile.owner, 1] != -1)
					{
						PortalHelper.anyPortalAtAll = true;
					}
				}
			}
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x0054D873 File Offset: 0x0054BA73
		public static void ResetNPCSlotData(int npcIndex)
		{
			PortalHelper.PortalCooldownForNPCs[npcIndex] = 0;
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x0054D880 File Offset: 0x0054BA80
		public static void TryGoingThroughPortals(Entity ent)
		{
			if (!PortalHelper.anyPortalAtAll)
			{
				return;
			}
			float num = 0f;
			Vector2 velocity = ent.velocity;
			int width = ent.width;
			int height = ent.height;
			int num2 = 1;
			if (ent is Player)
			{
				num2 = (int)((Player)ent).gravDir;
			}
			for (int i = 0; i < PortalHelper.FoundPortals.GetLength(0); i++)
			{
				if (PortalHelper.FoundPortals[i, 0] != -1 && PortalHelper.FoundPortals[i, 1] != -1 && (!(ent is Player) || (i < PortalHelper.PortalCooldownForPlayers.Length && PortalHelper.PortalCooldownForPlayers[i] <= 0)) && (!(ent is NPC) || (i < PortalHelper.PortalCooldownForNPCs.Length && PortalHelper.PortalCooldownForNPCs[i] <= 0)))
				{
					for (int j = 0; j < 2; j++)
					{
						Projectile projectile = Main.projectile[PortalHelper.FoundPortals[i, j]];
						Vector2 lineStart;
						Vector2 lineEnd;
						PortalHelper.GetPortalEdges(projectile.Center, projectile.ai[0], out lineStart, out lineEnd);
						if (Collision.CheckAABBvLineCollision(ent.position + ent.velocity, ent.Size, lineStart, lineEnd, 2f, ref num))
						{
							Projectile projectile2 = Main.projectile[PortalHelper.FoundPortals[i, 1 - j]];
							float scaleFactor = ent.Hitbox.Distance(projectile.Center);
							int num3;
							int num4;
							Vector2 vector = PortalHelper.GetPortalOutingPoint(ent.Size, projectile2.Center, projectile2.ai[0], out num3, out num4) + Vector2.Normalize(new Vector2((float)num3, (float)num4)) * scaleFactor;
							Vector2 vector2 = Vector2.UnitX * 16f;
							if (!(Collision.TileCollision(vector - vector2, vector2, width, height, true, true, num2, false, false, true) != vector2))
							{
								vector2 = -Vector2.UnitX * 16f;
								if (!(Collision.TileCollision(vector - vector2, vector2, width, height, true, true, num2, false, false, true) != vector2))
								{
									vector2 = Vector2.UnitY * 16f;
									if (!(Collision.TileCollision(vector - vector2, vector2, width, height, true, true, num2, false, false, true) != vector2))
									{
										vector2 = -Vector2.UnitY * 16f;
										if (!(Collision.TileCollision(vector - vector2, vector2, width, height, true, true, num2, false, false, true) != vector2))
										{
											float num5 = 0.1f;
											if (num4 == -num2)
											{
												num5 = 0.1f;
											}
											if (ent.velocity == Vector2.Zero)
											{
												ent.velocity = (projectile.ai[0] - 1.5707964f).ToRotationVector2() * num5;
											}
											if (ent.velocity.Length() < num5)
											{
												ent.velocity.Normalize();
												ent.velocity *= num5;
											}
											Vector2 vector3 = Vector2.Normalize(new Vector2((float)num3, (float)num4));
											if (vector3.HasNaNs() || vector3 == Vector2.Zero)
											{
												vector3 = Vector2.UnitX * (float)ent.direction;
											}
											ent.velocity = vector3 * ent.velocity.Length();
											if ((num4 == -num2 && Math.Sign(ent.velocity.Y) != -num2) || Math.Abs(ent.velocity.Y) < 0.1f)
											{
												ent.velocity.Y = (float)(-(float)num2) * 0.1f;
											}
											int num6 = (int)((float)(projectile2.owner * 2) + projectile2.ai[1]);
											int lastPortalColorIndex = num6 + ((num6 % 2 == 0) ? 1 : -1);
											if (ent is Player)
											{
												Player player = (Player)ent;
												player.lastPortalColorIndex = lastPortalColorIndex;
												player.Teleport(vector, 4, num6);
												if (Main.netMode == 1)
												{
													NetMessage.SendData(96, -1, -1, null, player.whoAmI, vector.X, vector.Y, (float)num6, 0, 0, 0);
													NetMessage.SendData(13, -1, -1, null, player.whoAmI, 0f, 0f, 0f, 0, 0, 0);
												}
												PortalHelper.PortalCooldownForPlayers[i] = 10;
												return;
											}
											if (ent is NPC)
											{
												NPC npc = (NPC)ent;
												npc.lastPortalColorIndex = lastPortalColorIndex;
												npc.Teleport(vector, 4, num6);
												if (Main.netMode == 2)
												{
													NetMessage.SendData(100, -1, -1, null, npc.whoAmI, vector.X, vector.Y, (float)num6, 0, 0, 0);
													NetMessage.SendData(23, -1, -1, null, npc.whoAmI, 0f, 0f, 0f, 0, 0, 0);
												}
												PortalHelper.PortalCooldownForPlayers[i] = 10;
												if (num4 == -1 && ent.velocity.Y > -3f)
												{
													ent.velocity.Y = -3f;
												}
											}
											return;
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x0054DD78 File Offset: 0x0054BF78
		public static int TryPlacingPortal(Projectile theBolt, Vector2 velocity, Vector2 theCrashVelocity)
		{
			Vector2 vector = velocity / velocity.Length();
			Point point = PortalHelper.FindCollision(theBolt.position, theBolt.position + velocity + vector * 32f).ToTileCoordinates();
			Tile tile = Main.tile[point.X, point.Y];
			Vector2 vector2 = new Vector2((float)(point.X * 16 + 8), (float)(point.Y * 16 + 8));
			if (!WorldGen.SolidOrSlopedTile(tile))
			{
				return -1;
			}
			int num = (int)tile.slope();
			bool flag = tile.halfBrick();
			for (int i = 0; i < (flag ? 2 : PortalHelper.EDGES.Length); i++)
			{
				Point point2;
				if (Vector2.Dot(PortalHelper.EDGES[i], vector) > 0f && PortalHelper.FindValidLine(point, (int)PortalHelper.EDGES[i].Y, (int)(-(int)PortalHelper.EDGES[i].X), out point2))
				{
					vector2 = new Vector2((float)(point2.X * 16 + 8), (float)(point2.Y * 16 + 8));
					return PortalHelper.AddPortal(theBolt, vector2 - PortalHelper.EDGES[i] * (flag ? 0f : 8f), (float)Math.Atan2((double)PortalHelper.EDGES[i].Y, (double)PortalHelper.EDGES[i].X) + 1.5707964f, (int)theBolt.ai[0], theBolt.direction);
				}
			}
			if (num != 0)
			{
				Vector2 vector3 = PortalHelper.SLOPE_EDGES[num - 1];
				Point point3;
				if (Vector2.Dot(vector3, -vector) > 0f && PortalHelper.FindValidLine(point, -PortalHelper.SLOPE_OFFSETS[num - 1].Y, PortalHelper.SLOPE_OFFSETS[num - 1].X, out point3))
				{
					vector2 = new Vector2((float)(point3.X * 16 + 8), (float)(point3.Y * 16 + 8));
					return PortalHelper.AddPortal(theBolt, vector2, (float)Math.Atan2((double)vector3.Y, (double)vector3.X) - 1.5707964f, (int)theBolt.ai[0], theBolt.direction);
				}
			}
			return -1;
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x0054DFC4 File Offset: 0x0054C1C4
		private static bool FindValidLine(Point position, int xOffset, int yOffset, out Point bestPosition)
		{
			bestPosition = position;
			if (PortalHelper.IsValidLine(position, xOffset, yOffset))
			{
				return true;
			}
			Point point = new Point(position.X - xOffset, position.Y - yOffset);
			if (PortalHelper.IsValidLine(point, xOffset, yOffset))
			{
				bestPosition = point;
				return true;
			}
			Point point2 = new Point(position.X + xOffset, position.Y + yOffset);
			if (PortalHelper.IsValidLine(point2, xOffset, yOffset))
			{
				bestPosition = point2;
				return true;
			}
			return false;
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x0054E03C File Offset: 0x0054C23C
		private static bool IsValidLine(Point position, int xOffset, int yOffset)
		{
			Tile tile = Main.tile[position.X, position.Y];
			Tile tile2 = Main.tile[position.X - xOffset, position.Y - yOffset];
			Tile tile3 = Main.tile[position.X + xOffset, position.Y + yOffset];
			return !PortalHelper.BlockPortals(Main.tile[position.X + yOffset, position.Y - xOffset]) && !PortalHelper.BlockPortals(Main.tile[position.X + yOffset - xOffset, position.Y - xOffset - yOffset]) && !PortalHelper.BlockPortals(Main.tile[position.X + yOffset + xOffset, position.Y - xOffset + yOffset]) && (PortalHelper.CanPlacePortalOn(tile) && PortalHelper.CanPlacePortalOn(tile2) && PortalHelper.CanPlacePortalOn(tile3) && tile2.HasSameSlope(tile) && tile3.HasSameSlope(tile));
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x0054E130 File Offset: 0x0054C330
		private static bool CanPlacePortalOn(Tile t)
		{
			return PortalHelper.DoesTileTypeSupportPortals(t.type) && WorldGen.SolidOrSlopedTile(t);
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x0054E147 File Offset: 0x0054C347
		private static bool DoesTileTypeSupportPortals(ushort tileType)
		{
			return tileType != 496;
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x0054E154 File Offset: 0x0054C354
		private static bool BlockPortals(Tile t)
		{
			return t.active() && !Main.tileCut[(int)t.type] && !TileID.Sets.BreakableWhenPlacing[(int)t.type] && Main.tileSolid[(int)t.type];
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x0054E18C File Offset: 0x0054C38C
		private static Vector2 FindCollision(Vector2 startPosition, Vector2 stopPosition)
		{
			int lastX = 0;
			int lastY = 0;
			Utils.PlotLine(startPosition.ToTileCoordinates(), stopPosition.ToTileCoordinates(), delegate(int x, int y)
			{
				lastX = x;
				lastY = y;
				return !WorldGen.SolidOrSlopedTile(x, y);
			}, false);
			return new Vector2((float)lastX * 16f, (float)lastY * 16f);
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x0054E1EC File Offset: 0x0054C3EC
		private static int AddPortal(Projectile sourceProjectile, Vector2 position, float angle, int form, int direction)
		{
			if (!PortalHelper.SupportedTilesAreFine(position, angle))
			{
				return -1;
			}
			PortalHelper.RemoveMyOldPortal(form);
			PortalHelper.RemoveIntersectingPortals(position, angle);
			int num = Projectile.NewProjectile(Projectile.InheritSource(sourceProjectile), position.X, position.Y, 0f, 0f, 602, 0, 0f, Main.myPlayer, angle, (float)form, 0f, null);
			Main.projectile[num].direction = direction;
			Main.projectile[num].netUpdate = true;
			return num;
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x0054E268 File Offset: 0x0054C468
		private static void RemoveMyOldPortal(int form)
		{
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.type == 602 && projectile.owner == Main.myPlayer && projectile.ai[1] == (float)form)
				{
					projectile.Kill();
					return;
				}
			}
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x0054E2C4 File Offset: 0x0054C4C4
		private static void RemoveIntersectingPortals(Vector2 position, float angle)
		{
			Vector2 a;
			Vector2 a2;
			PortalHelper.GetPortalEdges(position, angle, out a, out a2);
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.type == 602)
				{
					Vector2 b;
					Vector2 b2;
					PortalHelper.GetPortalEdges(projectile.Center, projectile.ai[0], out b, out b2);
					if (Collision.CheckLinevLine(a, a2, b, b2).Length != 0)
					{
						if (projectile.owner != Main.myPlayer && Main.netMode != 2)
						{
							NetMessage.SendData(95, -1, -1, null, projectile.owner, (float)((int)projectile.ai[1]), 0f, 0f, 0, 0, 0);
						}
						projectile.Kill();
					}
				}
			}
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x0054E382 File Offset: 0x0054C582
		public static Color GetPortalColor(int colorIndex)
		{
			return PortalHelper.GetPortalColor(colorIndex / 2, colorIndex % 2);
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x0054E390 File Offset: 0x0054C590
		public static Color GetPortalColor(int player, int portal)
		{
			Color result = Color.White;
			if (Main.netMode == 0)
			{
				if (portal == 0)
				{
					result = Main.hslToRgb(0.12f, 1f, 0.5f, byte.MaxValue);
				}
				else
				{
					result = Main.hslToRgb(0.52f, 1f, 0.6f, byte.MaxValue);
				}
			}
			else
			{
				float num = 0.08f;
				result = Main.hslToRgb((0.5f + (float)player * (num * 2f) + (float)portal * num) % 1f, 1f, 0.5f, byte.MaxValue);
			}
			result.A = 66;
			return result;
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x0054E428 File Offset: 0x0054C628
		private static void GetPortalEdges(Vector2 position, float angle, out Vector2 start, out Vector2 end)
		{
			Vector2 value = angle.ToRotationVector2();
			start = position + value * -22f;
			end = position + value * 22f;
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x0054E46C File Offset: 0x0054C66C
		private static Vector2 GetPortalOutingPoint(Vector2 objectSize, Vector2 portalPosition, float portalAngle, out int bonusX, out int bonusY)
		{
			int num = (int)Math.Round((double)(MathHelper.WrapAngle(portalAngle) / 0.7853982f));
			if (num == 2 || num == -2)
			{
				bonusX = ((num == 2) ? -1 : 1);
				bonusY = 0;
				return portalPosition + new Vector2((num == 2) ? (-objectSize.X) : 0f, -objectSize.Y / 2f);
			}
			if (num == 0 || num == 4)
			{
				bonusX = 0;
				bonusY = ((num == 0) ? 1 : -1);
				return portalPosition + new Vector2(-objectSize.X / 2f, (num == 0) ? 0f : (-objectSize.Y));
			}
			if (num == -3 || num == 3)
			{
				bonusX = ((num == -3) ? 1 : -1);
				bonusY = -1;
				return portalPosition + new Vector2((num == -3) ? 0f : (-objectSize.X), -objectSize.Y);
			}
			if (num == 1 || num == -1)
			{
				bonusX = ((num == -1) ? 1 : -1);
				bonusY = 1;
				return portalPosition + new Vector2((num == -1) ? 0f : (-objectSize.X), 0f);
			}
			bonusX = 0;
			bonusY = 0;
			return portalPosition;
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x0054E58C File Offset: 0x0054C78C
		public static void SyncPortalsOnPlayerJoin(int plr, int fluff, List<Point> dontInclude, out List<Point> portalSections)
		{
			portalSections = new List<Point>();
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && (projectile.type == 602 || projectile.type == 601))
				{
					Vector2 center = projectile.Center;
					int sectionX = Netplay.GetSectionX((int)(center.X / 16f));
					int sectionY = Netplay.GetSectionY((int)(center.Y / 16f));
					for (int j = sectionX - fluff; j < sectionX + fluff + 1; j++)
					{
						for (int k = sectionY - fluff; k < sectionY + fluff + 1; k++)
						{
							if (j >= 0 && j < Main.maxSectionsX && k >= 0 && k < Main.maxSectionsY && !Netplay.Clients[plr].TileSections[j, k] && !dontInclude.Contains(new Point(j, k)))
							{
								portalSections.Add(new Point(j, k));
							}
						}
					}
				}
			}
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x0054E694 File Offset: 0x0054C894
		public static void SyncPortalSections(Vector2 portalPosition, int fluff)
		{
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active)
				{
					RemoteClient.CheckSection(i, portalPosition, fluff);
				}
			}
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x0054E6C8 File Offset: 0x0054C8C8
		public static bool SupportedTilesAreFine(Vector2 portalCenter, float portalAngle)
		{
			Point point = portalCenter.ToTileCoordinates();
			int num = (int)Math.Round((double)(MathHelper.WrapAngle(portalAngle) / 0.7853982f));
			int num2;
			int num3;
			if (num == 2 || num == -2)
			{
				num2 = ((num == 2) ? -1 : 1);
				num3 = 0;
			}
			else if (num == 0 || num == 4)
			{
				num2 = 0;
				num3 = ((num == 0) ? 1 : -1);
			}
			else if (num == -3 || num == 3)
			{
				num2 = ((num == -3) ? 1 : -1);
				num3 = -1;
			}
			else
			{
				if (num != 1 && num != -1)
				{
					Main.NewText(string.Concat(new object[]
					{
						"Broken portal! (over4s = ",
						num,
						" , ",
						portalAngle,
						")"
					}), byte.MaxValue, byte.MaxValue, byte.MaxValue);
					return false;
				}
				num2 = ((num == -1) ? 1 : -1);
				num3 = 1;
			}
			if (num2 != 0 && num3 != 0)
			{
				int num4 = 3;
				if (num2 == -1 && num3 == 1)
				{
					num4 = 5;
				}
				if (num2 == 1 && num3 == -1)
				{
					num4 = 2;
				}
				if (num2 == 1 && num3 == 1)
				{
					num4 = 4;
				}
				num4--;
				return PortalHelper.SupportedSlope(point.X, point.Y, num4) && PortalHelper.SupportedSlope(point.X + num2, point.Y - num3, num4) && PortalHelper.SupportedSlope(point.X - num2, point.Y + num3, num4);
			}
			if (num2 != 0)
			{
				if (num2 == 1)
				{
					point.X--;
				}
				return PortalHelper.SupportedNormal(point.X, point.Y) && PortalHelper.SupportedNormal(point.X, point.Y - 1) && PortalHelper.SupportedNormal(point.X, point.Y + 1);
			}
			if (num3 != 0)
			{
				if (num3 == 1)
				{
					point.Y--;
				}
				return (PortalHelper.SupportedNormal(point.X, point.Y) && PortalHelper.SupportedNormal(point.X + 1, point.Y) && PortalHelper.SupportedNormal(point.X - 1, point.Y)) || (PortalHelper.SupportedHalfbrick(point.X, point.Y) && PortalHelper.SupportedHalfbrick(point.X + 1, point.Y) && PortalHelper.SupportedHalfbrick(point.X - 1, point.Y));
			}
			return true;
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x0054E8F0 File Offset: 0x0054CAF0
		private static bool SupportedSlope(int x, int y, int slope)
		{
			Tile tile = Main.tile[x, y];
			return tile != null && tile.nactive() && !Main.tileCut[(int)tile.type] && !TileID.Sets.BreakableWhenPlacing[(int)tile.type] && Main.tileSolid[(int)tile.type] && (int)tile.slope() == slope && PortalHelper.DoesTileTypeSupportPortals(tile.type);
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x0054E958 File Offset: 0x0054CB58
		private static bool SupportedHalfbrick(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return tile != null && tile.nactive() && !Main.tileCut[(int)tile.type] && !TileID.Sets.BreakableWhenPlacing[(int)tile.type] && Main.tileSolid[(int)tile.type] && tile.halfBrick() && PortalHelper.DoesTileTypeSupportPortals(tile.type);
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x0054E9BC File Offset: 0x0054CBBC
		private static bool SupportedNormal(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return tile != null && tile.nactive() && !Main.tileCut[(int)tile.type] && !TileID.Sets.BreakableWhenPlacing[(int)tile.type] && Main.tileSolid[(int)tile.type] && !TileID.Sets.NotReallySolid[(int)tile.type] && !tile.halfBrick() && tile.slope() == 0 && PortalHelper.DoesTileTypeSupportPortals(tile.type);
		}

		// Token: 0x04004F1A RID: 20250
		public const int PORTALS_PER_PERSON = 2;

		// Token: 0x04004F1B RID: 20251
		private static int[,] FoundPortals = new int[256, 2];

		// Token: 0x04004F1C RID: 20252
		private static int[] PortalCooldownForPlayers = new int[256];

		// Token: 0x04004F1D RID: 20253
		private static int[] PortalCooldownForNPCs = new int[Main.maxNPCs];

		// Token: 0x04004F1E RID: 20254
		private static readonly Vector2[] EDGES = new Vector2[]
		{
			new Vector2(0f, 1f),
			new Vector2(0f, -1f),
			new Vector2(1f, 0f),
			new Vector2(-1f, 0f)
		};

		// Token: 0x04004F1F RID: 20255
		private static readonly Vector2[] SLOPE_EDGES = new Vector2[]
		{
			new Vector2(1f, -1f),
			new Vector2(-1f, -1f),
			new Vector2(1f, 1f),
			new Vector2(-1f, 1f)
		};

		// Token: 0x04004F20 RID: 20256
		private static readonly Point[] SLOPE_OFFSETS = new Point[]
		{
			new Point(1, -1),
			new Point(-1, -1),
			new Point(1, 1),
			new Point(-1, 1)
		};

		// Token: 0x04004F21 RID: 20257
		private static bool anyPortalAtAll = false;
	}
}
