using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ObjectData;

namespace Terraria
{
	// Token: 0x02000038 RID: 56
	public struct TileObject
	{
		// Token: 0x0600035A RID: 858 RVA: 0x00046878 File Offset: 0x00044A78
		public static bool Place(TileObject toBePlaced)
		{
			TileObjectData tileData = TileObjectData.GetTileData(toBePlaced.type, toBePlaced.style, toBePlaced.alternate);
			if (tileData == null)
			{
				return false;
			}
			if (tileData.HookPlaceOverride.hook != null)
			{
				int arg;
				int arg2;
				if (tileData.HookPlaceOverride.processedCoordinates)
				{
					arg = toBePlaced.xCoord;
					arg2 = toBePlaced.yCoord;
				}
				else
				{
					arg = toBePlaced.xCoord + (int)tileData.Origin.X;
					arg2 = toBePlaced.yCoord + (int)tileData.Origin.Y;
				}
				if (tileData.HookPlaceOverride.hook(arg, arg2, toBePlaced.type, toBePlaced.style, 1, toBePlaced.alternate) == tileData.HookPlaceOverride.badReturn)
				{
					return false;
				}
			}
			else
			{
				ushort num = (ushort)toBePlaced.type;
				int num2 = tileData.CalculatePlacementStyle(toBePlaced.style, toBePlaced.alternate, toBePlaced.random);
				int num3 = 0;
				if (tileData.StyleWrapLimit > 0)
				{
					num3 = num2 / tileData.StyleWrapLimit * tileData.StyleLineSkip;
					num2 %= tileData.StyleWrapLimit;
				}
				int num4;
				int num5;
				if (tileData.StyleHorizontal)
				{
					num4 = tileData.CoordinateFullWidth * num2;
					num5 = tileData.CoordinateFullHeight * num3;
				}
				else
				{
					num4 = tileData.CoordinateFullWidth * num3;
					num5 = tileData.CoordinateFullHeight * num2;
				}
				int num6 = toBePlaced.xCoord;
				int num7 = toBePlaced.yCoord;
				for (int i = 0; i < tileData.Width; i++)
				{
					for (int j = 0; j < tileData.Height; j++)
					{
						Tile tileSafely = Framing.GetTileSafely(num6 + i, num7 + j);
						if (tileSafely.active() && tileSafely.type != 484 && (Main.tileCut[(int)tileSafely.type] || TileID.Sets.BreakableWhenPlacing[(int)tileSafely.type]))
						{
							WorldGen.KillTile(num6 + i, num7 + j, false, false, false);
							if (!Main.tile[num6 + i, num7 + j].active() && Main.netMode != 0)
							{
								NetMessage.SendData(17, -1, -1, null, 0, (float)(num6 + i), (float)(num7 + j), 0f, 0, 0, 0);
							}
						}
					}
				}
				for (int k = 0; k < tileData.Width; k++)
				{
					int num8 = num4 + k * (tileData.CoordinateWidth + tileData.CoordinatePadding);
					int num9 = num5;
					for (int l = 0; l < tileData.Height; l++)
					{
						Tile tileSafely2 = Framing.GetTileSafely(num6 + k, num7 + l);
						if (!tileSafely2.active())
						{
							tileSafely2.active(true);
							tileSafely2.frameX = (short)num8;
							tileSafely2.frameY = (short)num9;
							tileSafely2.type = num;
						}
						num9 += tileData.CoordinateHeights[l] + tileData.CoordinatePadding;
					}
				}
			}
			if (tileData.FlattenAnchors)
			{
				AnchorData anchorData = tileData.AnchorBottom;
				if (anchorData.tileCount != 0 && (anchorData.type & AnchorType.SolidTile) == AnchorType.SolidTile)
				{
					int num10 = toBePlaced.xCoord + anchorData.checkStart;
					int j2 = toBePlaced.yCoord + tileData.Height;
					for (int m = 0; m < anchorData.tileCount; m++)
					{
						Tile tileSafely3 = Framing.GetTileSafely(num10 + m, j2);
						if (Main.tileSolid[(int)tileSafely3.type] && !Main.tileSolidTop[(int)tileSafely3.type] && tileSafely3.blockType() != 0)
						{
							WorldGen.SlopeTile(num10 + m, j2, 0, false, true);
						}
					}
				}
				anchorData = tileData.AnchorTop;
				if (anchorData.tileCount != 0 && (anchorData.type & AnchorType.SolidTile) == AnchorType.SolidTile)
				{
					int num11 = toBePlaced.xCoord + anchorData.checkStart;
					int j3 = toBePlaced.yCoord - 1;
					for (int n = 0; n < anchorData.tileCount; n++)
					{
						Tile tileSafely4 = Framing.GetTileSafely(num11 + n, j3);
						if (Main.tileSolid[(int)tileSafely4.type] && !Main.tileSolidTop[(int)tileSafely4.type] && tileSafely4.blockType() != 0)
						{
							WorldGen.SlopeTile(num11 + n, j3, 0, false, true);
						}
					}
				}
				anchorData = tileData.AnchorRight;
				if (anchorData.tileCount != 0 && (anchorData.type & AnchorType.SolidTile) == AnchorType.SolidTile)
				{
					int i2 = toBePlaced.xCoord + tileData.Width;
					int num12 = toBePlaced.yCoord + anchorData.checkStart;
					for (int num13 = 0; num13 < anchorData.tileCount; num13++)
					{
						Tile tileSafely5 = Framing.GetTileSafely(i2, num12 + num13);
						if (Main.tileSolid[(int)tileSafely5.type] && !Main.tileSolidTop[(int)tileSafely5.type] && tileSafely5.blockType() != 0)
						{
							WorldGen.SlopeTile(i2, num12 + num13, 0, false, true);
						}
					}
				}
				anchorData = tileData.AnchorLeft;
				if (anchorData.tileCount != 0 && (anchorData.type & AnchorType.SolidTile) == AnchorType.SolidTile)
				{
					int i3 = toBePlaced.xCoord - 1;
					int num14 = toBePlaced.yCoord + anchorData.checkStart;
					for (int num15 = 0; num15 < anchorData.tileCount; num15++)
					{
						Tile tileSafely6 = Framing.GetTileSafely(i3, num14 + num15);
						if (Main.tileSolid[(int)tileSafely6.type] && !Main.tileSolidTop[(int)tileSafely6.type] && tileSafely6.blockType() != 0)
						{
							WorldGen.SlopeTile(i3, num14 + num15, 0, false, true);
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00046D98 File Offset: 0x00044F98
		public static bool CanPlace(int x, int y, int type, int style, int dir, out TileObject objectData, bool onlyCheck = false, int? forcedRandom = null)
		{
			TileObjectData tileData = TileObjectData.GetTileData(type, style, 0);
			objectData = TileObject.Empty;
			if (tileData == null)
			{
				return false;
			}
			int num = x - (int)tileData.Origin.X;
			int num2 = y - (int)tileData.Origin.Y;
			if (num < 0 || num + tileData.Width >= Main.maxTilesX || num2 < 0 || num2 + tileData.Height >= Main.maxTilesY)
			{
				return false;
			}
			bool flag = tileData.RandomStyleRange > 0;
			if (TileObjectPreviewData.placementCache == null)
			{
				TileObjectPreviewData.placementCache = new TileObjectPreviewData();
			}
			TileObjectPreviewData.placementCache.Reset();
			int num3 = 0;
			int num4 = 0;
			if (tileData.AlternatesCount != 0)
			{
				num4 = tileData.AlternatesCount;
			}
			float num5 = -1f;
			float num6 = -1f;
			int num7 = 0;
			TileObjectData tileObjectData = null;
			int i = num3 - 1;
			while (i < num4)
			{
				i++;
				TileObjectData tileData2 = TileObjectData.GetTileData(type, style, i);
				if (tileData2.Direction == TileObjectDirection.None || ((tileData2.Direction != TileObjectDirection.PlaceLeft || dir != 1) && (tileData2.Direction != TileObjectDirection.PlaceRight || dir != -1)))
				{
					int num8 = x - (int)tileData2.Origin.X;
					int num9 = y - (int)tileData2.Origin.Y;
					if (num8 < 5 || num8 + tileData2.Width > Main.maxTilesX - 5 || num9 < 5 || num9 + tileData2.Height > Main.maxTilesY - 5)
					{
						return false;
					}
					Rectangle rectangle = new Rectangle(0, 0, tileData2.Width, tileData2.Height);
					int num10 = 0;
					int num11 = 0;
					if (tileData2.AnchorTop.tileCount != 0)
					{
						if (rectangle.Y == 0)
						{
							rectangle.Y = -1;
							rectangle.Height++;
							num11++;
						}
						int checkStart = tileData2.AnchorTop.checkStart;
						if (checkStart < rectangle.X)
						{
							rectangle.Width += rectangle.X - checkStart;
							num10 += rectangle.X - checkStart;
							rectangle.X = checkStart;
						}
						int num12 = checkStart + tileData2.AnchorTop.tileCount - 1;
						int num13 = rectangle.X + rectangle.Width - 1;
						if (num12 > num13)
						{
							rectangle.Width += num12 - num13;
						}
					}
					if (tileData2.AnchorBottom.tileCount != 0)
					{
						if (rectangle.Y + rectangle.Height == tileData2.Height)
						{
							rectangle.Height++;
						}
						int checkStart2 = tileData2.AnchorBottom.checkStart;
						if (checkStart2 < rectangle.X)
						{
							rectangle.Width += rectangle.X - checkStart2;
							num10 += rectangle.X - checkStart2;
							rectangle.X = checkStart2;
						}
						int num14 = checkStart2 + tileData2.AnchorBottom.tileCount - 1;
						int num15 = rectangle.X + rectangle.Width - 1;
						if (num14 > num15)
						{
							rectangle.Width += num14 - num15;
						}
					}
					if (tileData2.AnchorLeft.tileCount != 0)
					{
						if (rectangle.X == 0)
						{
							rectangle.X = -1;
							rectangle.Width++;
							num10++;
						}
						int num16 = tileData2.AnchorLeft.checkStart;
						if ((tileData2.AnchorLeft.type & AnchorType.Tree) == AnchorType.Tree)
						{
							num16--;
						}
						if (num16 < rectangle.Y)
						{
							rectangle.Width += rectangle.Y - num16;
							num11 += rectangle.Y - num16;
							rectangle.Y = num16;
						}
						int num17 = num16 + tileData2.AnchorLeft.tileCount - 1;
						if ((tileData2.AnchorLeft.type & AnchorType.Tree) == AnchorType.Tree)
						{
							num17 += 2;
						}
						int num18 = rectangle.Y + rectangle.Height - 1;
						if (num17 > num18)
						{
							rectangle.Height += num17 - num18;
						}
					}
					if (tileData2.AnchorRight.tileCount != 0)
					{
						if (rectangle.X + rectangle.Width == tileData2.Width)
						{
							rectangle.Width++;
						}
						int num19 = tileData2.AnchorLeft.checkStart;
						if ((tileData2.AnchorRight.type & AnchorType.Tree) == AnchorType.Tree)
						{
							num19--;
						}
						if (num19 < rectangle.Y)
						{
							rectangle.Width += rectangle.Y - num19;
							num11 += rectangle.Y - num19;
							rectangle.Y = num19;
						}
						int num20 = num19 + tileData2.AnchorRight.tileCount - 1;
						if ((tileData2.AnchorRight.type & AnchorType.Tree) == AnchorType.Tree)
						{
							num20 += 2;
						}
						int num21 = rectangle.Y + rectangle.Height - 1;
						if (num20 > num21)
						{
							rectangle.Height += num20 - num21;
						}
					}
					if (onlyCheck)
					{
						TileObject.objectPreview.Reset();
						TileObject.objectPreview.Active = true;
						TileObject.objectPreview.Type = (ushort)type;
						TileObject.objectPreview.Style = (short)style;
						TileObject.objectPreview.Alternate = i;
						TileObject.objectPreview.Size = new Point16(rectangle.Width, rectangle.Height);
						TileObject.objectPreview.ObjectStart = new Point16(num10, num11);
						TileObject.objectPreview.Coordinates = new Point16(num8 - num10, num9 - num11);
					}
					float num22 = 0f;
					float num23 = (float)(tileData2.Width * tileData2.Height);
					float num24 = 0f;
					float num25 = 0f;
					for (int j = 0; j < tileData2.Width; j++)
					{
						for (int k = 0; k < tileData2.Height; k++)
						{
							Tile tileSafely = Framing.GetTileSafely(num8 + j, num9 + k);
							bool flag2 = !tileData2.LiquidPlace(tileSafely);
							bool flag3 = false;
							if (tileData2.AnchorWall)
							{
								num25 += 1f;
								if (!tileData2.isValidWallAnchor((int)tileSafely.wall))
								{
									flag3 = true;
								}
								else
								{
									num24 += 1f;
								}
							}
							bool flag4 = false;
							if (tileSafely.active() && (!Main.tileCut[(int)tileSafely.type] || tileSafely.type == 484 || tileSafely.type == 654) && !TileID.Sets.BreakableWhenPlacing[(int)tileSafely.type])
							{
								flag4 = true;
							}
							if (flag4 || flag2 || flag3)
							{
								if (onlyCheck)
								{
									TileObject.objectPreview[j + num10, k + num11] = 2;
								}
							}
							else
							{
								if (onlyCheck)
								{
									TileObject.objectPreview[j + num10, k + num11] = 1;
								}
								num22 += 1f;
							}
						}
					}
					AnchorData anchorData = tileData2.AnchorBottom;
					if (anchorData.tileCount != 0)
					{
						num25 += (float)anchorData.tileCount;
						int height = tileData2.Height;
						for (int l = 0; l < anchorData.tileCount; l++)
						{
							int num26 = anchorData.checkStart + l;
							Tile tileSafely = Framing.GetTileSafely(num8 + num26, num9 + height);
							bool flag5 = false;
							if (tileSafely.nactive())
							{
								if ((anchorData.type & AnchorType.SolidTile) == AnchorType.SolidTile && Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type] && !Main.tileNoAttach[(int)tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
								{
									flag5 = tileData2.isValidTileAnchor((int)tileSafely.type);
								}
								if (!flag5 && ((anchorData.type & AnchorType.SolidWithTop) == AnchorType.SolidWithTop || (anchorData.type & AnchorType.Table) == AnchorType.Table))
								{
									if (TileID.Sets.Platforms[(int)tileSafely.type])
									{
										int num27 = (int)tileSafely.frameX / TileObjectData.PlatformFrameWidth();
										if (!tileSafely.halfBrick() && WorldGen.PlatformProperTopFrame(tileSafely.frameX))
										{
											flag5 = true;
										}
									}
									else if (Main.tileSolid[(int)tileSafely.type] && Main.tileSolidTop[(int)tileSafely.type])
									{
										flag5 = true;
									}
								}
								if (!flag5 && (anchorData.type & AnchorType.Table) == AnchorType.Table && !TileID.Sets.Platforms[(int)tileSafely.type] && Main.tileTable[(int)tileSafely.type] && tileSafely.blockType() == 0)
								{
									flag5 = true;
								}
								if (!flag5 && (anchorData.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type])
								{
									int num28 = tileSafely.blockType();
									if (num28 - 4 <= 1)
									{
										flag5 = tileData2.isValidTileAnchor((int)tileSafely.type);
									}
								}
								if (!flag5 && (anchorData.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor((int)tileSafely.type))
								{
									flag5 = true;
								}
							}
							else if (!flag5 && (anchorData.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
							{
								flag5 = true;
							}
							if (!flag5)
							{
								if (onlyCheck)
								{
									TileObject.objectPreview[num26 + num10, height + num11] = 2;
								}
							}
							else
							{
								if (onlyCheck)
								{
									TileObject.objectPreview[num26 + num10, height + num11] = 1;
								}
								num24 += 1f;
							}
						}
					}
					int num29 = -1;
					anchorData = tileData2.AnchorTop;
					if (anchorData.tileCount != 0)
					{
						num25 += (float)anchorData.tileCount;
						int num30 = -1;
						for (int m = 0; m < anchorData.tileCount; m++)
						{
							int num31 = -1;
							int num32 = anchorData.checkStart + m;
							Tile tileSafely = Framing.GetTileSafely(num8 + num32, num9 + num30);
							bool flag6 = false;
							if (tileSafely.nactive())
							{
								if (Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type] && !Main.tileNoAttach[(int)tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
								{
									flag6 = tileData2.isValidTileAnchor((int)tileSafely.type);
									if (flag6)
									{
										num31 = 0;
									}
								}
								if (!flag6 && (anchorData.type & AnchorType.SolidBottom) == AnchorType.SolidBottom && ((Main.tileSolid[(int)tileSafely.type] && (!Main.tileSolidTop[(int)tileSafely.type] || (TileID.Sets.Platforms[(int)tileSafely.type] && (tileSafely.halfBrick() || tileSafely.topSlope())))) || tileSafely.halfBrick() || tileSafely.topSlope()) && !TileID.Sets.NotReallySolid[(int)tileSafely.type] && !tileSafely.bottomSlope())
								{
									flag6 = tileData2.isValidTileAnchor((int)tileSafely.type);
									if (flag6)
									{
										num31 = 0;
									}
								}
								if (!flag6 && (anchorData.type & AnchorType.Platform) == AnchorType.Platform && TileID.Sets.Platforms[(int)tileSafely.type])
								{
									flag6 = tileData2.isValidTileAnchor((int)tileSafely.type);
									if (flag6)
									{
										if (tileSafely.halfBrick() || tileSafely.topSlope())
										{
											num31 = 0;
										}
										else
										{
											num31 = 8;
										}
									}
								}
								if (!flag6 && (anchorData.type & AnchorType.PlatformNonHammered) == AnchorType.PlatformNonHammered && TileID.Sets.Platforms[(int)tileSafely.type] && tileSafely.slope() == 0 && !tileSafely.halfBrick())
								{
									flag6 = tileData2.isValidTileAnchor((int)tileSafely.type);
									if (flag6)
									{
										num31 = 8;
									}
								}
								if (!flag6 && (anchorData.type & AnchorType.PlanterBox) == AnchorType.PlanterBox && tileSafely.type == 380)
								{
									flag6 = tileData2.isValidTileAnchor((int)tileSafely.type);
									if (flag6)
									{
										num31 = 0;
									}
								}
								if (!flag6 && (anchorData.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type])
								{
									int num28 = tileSafely.blockType();
									if (num28 - 2 <= 1)
									{
										flag6 = tileData2.isValidTileAnchor((int)tileSafely.type);
									}
									if (flag6)
									{
										num31 = 0;
									}
								}
								if (!flag6 && (anchorData.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor((int)tileSafely.type))
								{
									flag6 = true;
									if (flag6)
									{
										num31 = 0;
									}
								}
							}
							else if (!flag6 && (anchorData.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
							{
								flag6 = true;
								num31 = 0;
							}
							if (flag6 && (anchorData.type & AnchorType.AllFlatHeight) == AnchorType.AllFlatHeight)
							{
								if (num29 == -1)
								{
									num29 = num31;
								}
								if (num29 != num31)
								{
									flag6 = false;
								}
							}
							if (!flag6)
							{
								if (onlyCheck)
								{
									TileObject.objectPreview[num32 + num10, num30 + num11] = 2;
								}
							}
							else
							{
								if (onlyCheck)
								{
									TileObject.objectPreview[num32 + num10, num30 + num11] = 1;
								}
								num24 += 1f;
							}
						}
					}
					anchorData = tileData2.AnchorRight;
					if (anchorData.tileCount != 0)
					{
						num25 += (float)anchorData.tileCount;
						int width = tileData2.Width;
						for (int n = 0; n < anchorData.tileCount; n++)
						{
							int num33 = anchorData.checkStart + n;
							Tile tileSafely = Framing.GetTileSafely(num8 + width, num9 + num33);
							bool flag7 = false;
							if (tileSafely.nactive())
							{
								if (Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type] && !Main.tileNoAttach[(int)tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
								{
									flag7 = tileData2.isValidTileAnchor((int)tileSafely.type);
								}
								if (!flag7 && (anchorData.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type])
								{
									int num28 = tileSafely.blockType();
									if (num28 == 2 || num28 == 4)
									{
										flag7 = tileData2.isValidTileAnchor((int)tileSafely.type);
									}
								}
								if (!flag7 && (anchorData.type & AnchorType.Tree) == AnchorType.Tree && TileID.Sets.IsATreeTrunk[(int)tileSafely.type])
								{
									flag7 = true;
									if (n == 0)
									{
										num25 += 1f;
										Tile tileSafely2 = Framing.GetTileSafely(num8 + width, num9 + num33 - 1);
										if (tileSafely2.nactive() && TileID.Sets.IsATreeTrunk[(int)tileSafely2.type])
										{
											num24 += 1f;
											if (onlyCheck)
											{
												TileObject.objectPreview[width + num10, num33 + num11 - 1] = 1;
											}
										}
										else if (onlyCheck)
										{
											TileObject.objectPreview[width + num10, num33 + num11 - 1] = 2;
										}
									}
									if (n == anchorData.tileCount - 1)
									{
										num25 += 1f;
										Tile tileSafely3 = Framing.GetTileSafely(num8 + width, num9 + num33 + 1);
										if (tileSafely3.nactive() && TileID.Sets.IsATreeTrunk[(int)tileSafely3.type])
										{
											num24 += 1f;
											if (onlyCheck)
											{
												TileObject.objectPreview[width + num10, num33 + num11 + 1] = 1;
											}
										}
										else if (onlyCheck)
										{
											TileObject.objectPreview[width + num10, num33 + num11 + 1] = 2;
										}
									}
								}
								if (!flag7 && (anchorData.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor((int)tileSafely.type))
								{
									flag7 = true;
								}
							}
							else if (!flag7 && (anchorData.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
							{
								flag7 = true;
							}
							if (!flag7)
							{
								if (onlyCheck)
								{
									TileObject.objectPreview[width + num10, num33 + num11] = 2;
								}
							}
							else
							{
								if (onlyCheck)
								{
									TileObject.objectPreview[width + num10, num33 + num11] = 1;
								}
								num24 += 1f;
							}
						}
					}
					anchorData = tileData2.AnchorLeft;
					if (anchorData.tileCount != 0)
					{
						num25 += (float)anchorData.tileCount;
						int num34 = -1;
						for (int num35 = 0; num35 < anchorData.tileCount; num35++)
						{
							int num36 = anchorData.checkStart + num35;
							Tile tileSafely = Framing.GetTileSafely(num8 + num34, num9 + num36);
							bool flag8 = false;
							if (tileSafely.nactive())
							{
								if (Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type] && !Main.tileNoAttach[(int)tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
								{
									flag8 = tileData2.isValidTileAnchor((int)tileSafely.type);
								}
								if (!flag8 && (anchorData.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type])
								{
									int num28 = tileSafely.blockType();
									if (num28 == 3 || num28 == 5)
									{
										flag8 = tileData2.isValidTileAnchor((int)tileSafely.type);
									}
								}
								if (!flag8 && (anchorData.type & AnchorType.Tree) == AnchorType.Tree && TileID.Sets.IsATreeTrunk[(int)tileSafely.type])
								{
									flag8 = true;
									if (num35 == 0)
									{
										num25 += 1f;
										Tile tileSafely4 = Framing.GetTileSafely(num8 + num34, num9 + num36 - 1);
										if (tileSafely4.nactive() && TileID.Sets.IsATreeTrunk[(int)tileSafely4.type])
										{
											num24 += 1f;
											if (onlyCheck)
											{
												TileObject.objectPreview[num34 + num10, num36 + num11 - 1] = 1;
											}
										}
										else if (onlyCheck)
										{
											TileObject.objectPreview[num34 + num10, num36 + num11 - 1] = 2;
										}
									}
									if (num35 == anchorData.tileCount - 1)
									{
										num25 += 1f;
										Tile tileSafely5 = Framing.GetTileSafely(num8 + num34, num9 + num36 + 1);
										if (tileSafely5.nactive() && TileID.Sets.IsATreeTrunk[(int)tileSafely5.type])
										{
											num24 += 1f;
											if (onlyCheck)
											{
												TileObject.objectPreview[num34 + num10, num36 + num11 + 1] = 1;
											}
										}
										else if (onlyCheck)
										{
											TileObject.objectPreview[num34 + num10, num36 + num11 + 1] = 2;
										}
									}
								}
								if (!flag8 && (anchorData.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor((int)tileSafely.type))
								{
									flag8 = true;
								}
							}
							else if (!flag8 && (anchorData.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
							{
								flag8 = true;
							}
							if (!flag8)
							{
								if (onlyCheck)
								{
									TileObject.objectPreview[num34 + num10, num36 + num11] = 2;
								}
							}
							else
							{
								if (onlyCheck)
								{
									TileObject.objectPreview[num34 + num10, num36 + num11] = 1;
								}
								num24 += 1f;
							}
						}
					}
					if (tileData2.HookCheckIfCanPlace.hook != null)
					{
						if (tileData2.HookCheckIfCanPlace.processedCoordinates)
						{
							Point16 origin = tileData2.Origin;
							Point16 origin2 = tileData2.Origin;
						}
						if (tileData2.HookCheckIfCanPlace.hook(x, y, type, style, dir, i) == tileData2.HookCheckIfCanPlace.badReturn && tileData2.HookCheckIfCanPlace.badResponse == 0)
						{
							num24 = 0f;
							num22 = 0f;
							TileObject.objectPreview.AllInvalid();
						}
					}
					float num37 = num24 / num25;
					if (num25 == 0f)
					{
						num37 = 1f;
					}
					float num38 = num22 / num23;
					if (num38 == 1f && num25 == 0f)
					{
						num37 = 1f;
						num38 = 1f;
					}
					if (num37 == 1f && num38 == 1f)
					{
						num5 = 1f;
						num6 = 1f;
						num7 = i;
						tileObjectData = tileData2;
						break;
					}
					if (num37 > num5 || (num37 == num5 && num38 > num6))
					{
						TileObjectPreviewData.placementCache.CopyFrom(TileObject.objectPreview);
						num5 = num37;
						num6 = num38;
						tileObjectData = tileData2;
						num7 = i;
					}
				}
			}
			int num39 = -1;
			if (flag)
			{
				if (TileObjectPreviewData.randomCache == null)
				{
					TileObjectPreviewData.randomCache = new TileObjectPreviewData();
				}
				bool flag9 = false;
				if ((int)TileObjectPreviewData.randomCache.Type == type)
				{
					Point16 coordinates = TileObjectPreviewData.randomCache.Coordinates;
					Point16 objectStart = TileObjectPreviewData.randomCache.ObjectStart;
					int num40 = (int)(coordinates.X + objectStart.X);
					int num41 = (int)(coordinates.Y + objectStart.Y);
					int num42 = x - (int)tileData.Origin.X;
					int num43 = y - (int)tileData.Origin.Y;
					if (num40 != num42 || num41 != num43)
					{
						flag9 = true;
					}
				}
				else
				{
					flag9 = true;
				}
				int randomStyleRange = tileData.RandomStyleRange;
				int num44 = Main.rand.Next(tileData.RandomStyleRange);
				if (forcedRandom != null)
				{
					num44 = (forcedRandom.Value % randomStyleRange + randomStyleRange) % randomStyleRange;
				}
				if (flag9 || forcedRandom != null)
				{
					num39 = num44;
				}
				else
				{
					num39 = TileObjectPreviewData.randomCache.Random;
				}
			}
			if (tileData.SpecificRandomStyles != null)
			{
				if (TileObjectPreviewData.randomCache == null)
				{
					TileObjectPreviewData.randomCache = new TileObjectPreviewData();
				}
				bool flag10 = false;
				if ((int)TileObjectPreviewData.randomCache.Type == type)
				{
					Point16 coordinates2 = TileObjectPreviewData.randomCache.Coordinates;
					Point16 objectStart2 = TileObjectPreviewData.randomCache.ObjectStart;
					int num45 = (int)(coordinates2.X + objectStart2.X);
					int num46 = (int)(coordinates2.Y + objectStart2.Y);
					int num47 = x - (int)tileData.Origin.X;
					int num48 = y - (int)tileData.Origin.Y;
					if (num45 != num47 || num46 != num48)
					{
						flag10 = true;
					}
				}
				else
				{
					flag10 = true;
				}
				int num49 = tileData.SpecificRandomStyles.Length;
				int num50 = Main.rand.Next(num49);
				if (forcedRandom != null)
				{
					num50 = (forcedRandom.Value % num49 + num49) % num49;
				}
				if (flag10 || forcedRandom != null)
				{
					num39 = tileData.SpecificRandomStyles[num50] - style;
				}
				else
				{
					num39 = TileObjectPreviewData.randomCache.Random;
				}
			}
			if (onlyCheck)
			{
				if (num5 != 1f || num6 != 1f)
				{
					TileObject.objectPreview.CopyFrom(TileObjectPreviewData.placementCache);
					i = num7;
				}
				TileObject.objectPreview.Random = num39;
				if (tileData.RandomStyleRange > 0 || tileData.SpecificRandomStyles != null)
				{
					TileObjectPreviewData.randomCache.CopyFrom(TileObject.objectPreview);
				}
			}
			if (!onlyCheck)
			{
				objectData.xCoord = x - (int)tileObjectData.Origin.X;
				objectData.yCoord = y - (int)tileObjectData.Origin.Y;
				objectData.type = type;
				objectData.style = style;
				objectData.alternate = i;
				objectData.random = num39;
			}
			// 允许凭空放置方块，不需要检查锚点连接
			// 如果 num5 是 -1f 表示没有找到任何有效的放置位置，返回 false
			if (num5 == -1f)
			{
				return false;
			}
			// 只需要检查空间可用性，不需要锚点完全满足
			return num6 > 0f;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x000482D4 File Offset: 0x000464D4
		public static void DrawPreview(SpriteBatch sb, TileObjectPreviewData op, Vector2 position, float opacity)
		{
			Point16 coordinates = op.Coordinates;
			Texture2D value = TextureAssets.Tile[(int)op.Type].Value;
			TileObjectData tileData = TileObjectData.GetTileData((int)op.Type, (int)op.Style, op.Alternate);
			int num = tileData.CalculatePlacementStyle((int)op.Style, op.Alternate, op.Random);
			int num2 = 0;
			int num3 = tileData.DrawYOffset;
			int drawXOffset = tileData.DrawXOffset;
			num += tileData.DrawStyleOffset;
			int num4 = tileData.StyleWrapLimit;
			int num5 = tileData.StyleLineSkip;
			if (tileData.StyleWrapLimitVisualOverride != null)
			{
				num4 = tileData.StyleWrapLimitVisualOverride.Value;
			}
			if (tileData.styleLineSkipVisualOverride != null)
			{
				num5 = tileData.styleLineSkipVisualOverride.Value;
			}
			if (num4 > 0)
			{
				num2 = num / num4 * num5;
				num %= num4;
			}
			int num6;
			int num7;
			if (tileData.StyleHorizontal)
			{
				num6 = tileData.CoordinateFullWidth * num;
				num7 = tileData.CoordinateFullHeight * num2;
			}
			else
			{
				num6 = tileData.CoordinateFullWidth * num2;
				num7 = tileData.CoordinateFullHeight * num;
			}
			for (int i = 0; i < (int)op.Size.X; i++)
			{
				int x = num6 + (i - (int)op.ObjectStart.X) * (tileData.CoordinateWidth + tileData.CoordinatePadding);
				int num8 = num7;
				int j = 0;
				while (j < (int)op.Size.Y)
				{
					int num9 = (int)coordinates.X + i;
					int num10 = (int)coordinates.Y + j;
					if (j == 0 && tileData.DrawStepDown != 0 && WorldGen.SolidTile(Framing.GetTileSafely(num9, num10 - 1)))
					{
						num3 += tileData.DrawStepDown;
					}
					if (op.Type == 567)
					{
						if (j == 0)
						{
							num3 = tileData.DrawYOffset - 2;
						}
						else
						{
							num3 = tileData.DrawYOffset;
						}
					}
					int num11 = op[i, j];
					Color color;
					if (num11 == 1)
					{
						color = Color.White;
						goto IL_1D7;
					}
					if (num11 == 2)
					{
						color = Color.Red * 0.7f;
						goto IL_1D7;
					}
					IL_4EC:
					j++;
					continue;
					IL_1D7:
					color *= 0.5f;
					color *= opacity;
					if (i >= (int)op.ObjectStart.X && i < (int)op.ObjectStart.X + tileData.Width && j >= (int)op.ObjectStart.Y && j < (int)op.ObjectStart.Y + tileData.Height)
					{
						SpriteEffects spriteEffects = SpriteEffects.None;
						if (tileData.DrawFlipHorizontal && num9 % 2 == 0)
						{
							spriteEffects |= SpriteEffects.FlipHorizontally;
						}
						if (tileData.DrawFlipVertical && num10 % 2 == 0)
						{
							spriteEffects |= SpriteEffects.FlipVertically;
						}
						int coordinateWidth = tileData.CoordinateWidth;
						int num12 = tileData.CoordinateHeights[j - (int)op.ObjectStart.Y];
						float num13 = (float)(coordinateWidth - 16) / 2f;
						if (op.Type >= 0 && TileID.Sets.DoNotAdjustDrawPositionBasedOnTileWidth[(int)op.Type])
						{
							num13 = 0f;
						}
						Vector2 vector = new Vector2((float)(num9 * 16 - (int)(position.X + num13 + (float)drawXOffset)), (float)(num10 * 16 - (int)position.Y + num3));
						Rectangle rectangle = new Rectangle(x, num8, coordinateWidth, num12);
						if (tileData.DrawFrameOffsets != null)
						{
							Rectangle rectangle2 = tileData.DrawFrameOffsets[i - (int)op.ObjectStart.X, j - (int)op.ObjectStart.Y];
							rectangle.X += rectangle2.X;
							rectangle.Y += rectangle2.Y;
							rectangle.Width += rectangle2.Width;
							rectangle.Height += rectangle2.Height;
						}
						if (TileID.Sets.CritterCageLidStyle[(int)op.Type] >= 0)
						{
							int num14 = TileID.Sets.CritterCageLidStyle[(int)op.Type];
							if ((num14 < 3 && rectangle.Y % 54 == 0) || (num14 >= 3 && rectangle.Y % 36 == 0))
							{
								Vector2 position2 = vector;
								position2.Y += 8f;
								Rectangle value2 = rectangle;
								value2.Y += 8;
								value2.Height -= 8;
								Main.spriteBatch.Draw(value, position2, new Rectangle?(value2), color, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
								position2 = vector;
								position2.Y -= 2f;
								value2 = rectangle;
								if (num14 == 0)
								{
									value2.X = rectangle.X % 108;
								}
								value2.Y = 0;
								value2.Height = 10;
								Main.spriteBatch.Draw(TextureAssets.CageTop[num14].Value, position2, new Rectangle?(value2), color, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
							}
							else
							{
								sb.Draw(value, vector, new Rectangle?(rectangle), color, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
							}
						}
						else
						{
							sb.Draw(value, vector, new Rectangle?(rectangle), color, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
						}
						num8 += num12 + tileData.CoordinatePadding;
						goto IL_4EC;
					}
					goto IL_4EC;
				}
			}
		}

		// Token: 0x04000269 RID: 617
		public int xCoord;

		// Token: 0x0400026A RID: 618
		public int yCoord;

		// Token: 0x0400026B RID: 619
		public int type;

		// Token: 0x0400026C RID: 620
		public int style;

		// Token: 0x0400026D RID: 621
		public int alternate;

		// Token: 0x0400026E RID: 622
		public int random;

		// Token: 0x0400026F RID: 623
		public static TileObject Empty = default(TileObject);

		// Token: 0x04000270 RID: 624
		public static TileObjectPreviewData objectPreview = new TileObjectPreviewData();
	}
}
