using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameContent.Liquid;
using Terraria.GameContent.Tile_Entities;
using Terraria.Graphics;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Testing;
using Terraria.UI;
using Terraria.Utilities;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x02000444 RID: 1092
	public class TileDrawing : TileDrawingBase
	{
		// Token: 0x06003139 RID: 12601 RVA: 0x005C8A4C File Offset: 0x005C6C4C
		private void AddSpecialPoint(int x, int y, TileDrawing.TileCounterType type)
		{
			Point[] array = this._specialPositions[(int)type];
			int[] specialsCount = this._specialsCount;
			int num = specialsCount[(int)type];
			specialsCount[(int)type] = num + 1;
			array[num] = new Point(x, y);
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600313A RID: 12602 RVA: 0x004DD5A7 File Offset: 0x004DB7A7
		private bool[] _tileSolid
		{
			get
			{
				return Main.tileSolid;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x005C8A81 File Offset: 0x005C6C81
		private bool[] _tileSolidTop
		{
			get
			{
				return Main.tileSolidTop;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x0600313C RID: 12604 RVA: 0x005C8A88 File Offset: 0x005C6C88
		private Dust[] _dust
		{
			get
			{
				return Main.dust;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600313D RID: 12605 RVA: 0x005C8A8F File Offset: 0x005C6C8F
		private Gore[] _gore
		{
			get
			{
				return Main.gore;
			}
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x005C8A98 File Offset: 0x005C6C98
		public TileDrawing(TilePaintSystemV2 paintSystem)
		{
			this._paintSystem = paintSystem;
			this._rand = new UnifiedRandom();
			for (int i = 0; i < this._specialPositions.Length; i++)
			{
				this._specialPositions[i] = new Point[9000];
			}
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x005C8CC8 File Offset: 0x005C6EC8
		public void PreparePaintForTilesOnScreen()
		{
			if (Main.GameUpdateCount % 6U > 0U)
			{
				return;
			}
			Vector2 vector;
			int firstTileX;
			int lastTileX;
			int firstTileY;
			int lastTileY;
			TileDrawing.GetScreenDrawArea(!Main.drawToScreen, out vector, out firstTileX, out lastTileX, out firstTileY, out lastTileY);
			this.PrepareForAreaDrawing(firstTileX, lastTileX, firstTileY, lastTileY, true);
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x005C8D08 File Offset: 0x005C6F08
		public void PrepareForAreaDrawing(int firstTileX, int lastTileX, int firstTileY, int lastTileY, bool prepareLazily)
		{
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			TilePaintSystemV2.TileVariationkey tileVariationkey = default(TilePaintSystemV2.TileVariationkey);
			TilePaintSystemV2.WallVariationKey wallVariationKey = default(TilePaintSystemV2.WallVariationKey);
			for (int i = firstTileY; i < lastTileY + 4; i++)
			{
				for (int j = firstTileX - 2; j < lastTileX + 2; j++)
				{
					Tile tile = Main.tile[j, i];
					if (tile != null)
					{
						if (tile.active())
						{
							Main.instance.LoadTiles((int)tile.type);
							tileVariationkey.TileType = (int)tile.type;
							tileVariationkey.PaintColor = (int)tile.color();
							int tileStyle = 0;
							ushort type = tile.type;
							if (type != 5)
							{
								if (type == 323)
								{
									tileStyle = this.GetPalmTreeBiome(j, i);
								}
							}
							else
							{
								tileStyle = TileDrawing.GetTreeBiome(j, i, (int)tile.frameX, (int)tile.frameY);
							}
							tileVariationkey.TileStyle = tileStyle;
							if (tileVariationkey.PaintColor != 0)
							{
								this._paintSystem.RequestTile(ref tileVariationkey);
							}
						}
						if (tile.wall != 0)
						{
							Main.instance.LoadWall((int)tile.wall);
							wallVariationKey.WallType = (int)tile.wall;
							wallVariationKey.PaintColor = (int)tile.wallColor();
							if (wallVariationKey.PaintColor != 0)
							{
								this._paintSystem.RequestWall(ref wallVariationKey);
							}
						}
						if (!prepareLazily)
						{
							this.MakeExtraPreparations(tile, j, i);
						}
					}
				}
			}
			TimeLogger.FindPaintedTiles.AddTime(fromTimestamp);
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x005C8E70 File Offset: 0x005C7070
		private void MakeExtraPreparations(Tile tile, int x, int y)
		{
			ushort type = tile.type;
			if (type <= 589)
			{
				if (type != 5)
				{
					if (type != 323)
					{
						if (type - 583 > 6)
						{
							return;
						}
						int num = 0;
						int num2 = 0;
						int num3 = 0;
						int num4 = 0;
						int textureIndex = 0;
						int xoffset = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
						if (WorldGen.GetGemTreeFoliageData(x, y, xoffset, ref num, ref textureIndex, out num2, out num3, out num4))
						{
							TilePaintSystemV2.TreeFoliageVariantKey treeFoliageVariantKey = new TilePaintSystemV2.TreeFoliageVariantKey
							{
								TextureIndex = textureIndex,
								PaintColor = (int)tile.color()
							};
							this._paintSystem.RequestTreeTop(ref treeFoliageVariantKey);
							this._paintSystem.RequestTreeBranch(ref treeFoliageVariantKey);
							return;
						}
					}
					else
					{
						int textureIndex2 = 15;
						if (x >= WorldGen.beachDistance && x <= Main.maxTilesX - WorldGen.beachDistance)
						{
							textureIndex2 = 21;
						}
						TilePaintSystemV2.TreeFoliageVariantKey treeFoliageVariantKey2 = new TilePaintSystemV2.TreeFoliageVariantKey
						{
							TextureIndex = textureIndex2,
							PaintColor = (int)tile.color()
						};
						this._paintSystem.RequestTreeTop(ref treeFoliageVariantKey2);
						this._paintSystem.RequestTreeBranch(ref treeFoliageVariantKey2);
					}
				}
				else
				{
					int num5 = 0;
					int num6 = 0;
					int num7 = 0;
					int num8 = 0;
					int textureIndex3 = 0;
					int xoffset2 = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
					if (WorldGen.GetCommonTreeFoliageData(x, y, xoffset2, ref num5, ref textureIndex3, out num6, out num7, out num8))
					{
						TilePaintSystemV2.TreeFoliageVariantKey treeFoliageVariantKey3 = new TilePaintSystemV2.TreeFoliageVariantKey
						{
							TextureIndex = textureIndex3,
							PaintColor = (int)tile.color()
						};
						this._paintSystem.RequestTreeTop(ref treeFoliageVariantKey3);
						this._paintSystem.RequestTreeBranch(ref treeFoliageVariantKey3);
						return;
					}
				}
			}
			else if (type != 596 && type != 616)
			{
				if (type != 634)
				{
					return;
				}
				int num9 = 0;
				int num10 = 0;
				int num11 = 0;
				int num12 = 0;
				int textureIndex4 = 0;
				int xoffset3 = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
				if (WorldGen.GetAshTreeFoliageData(x, y, xoffset3, ref num9, ref textureIndex4, out num10, out num11, out num12))
				{
					TilePaintSystemV2.TreeFoliageVariantKey treeFoliageVariantKey4 = new TilePaintSystemV2.TreeFoliageVariantKey
					{
						TextureIndex = textureIndex4,
						PaintColor = (int)tile.color()
					};
					this._paintSystem.RequestTreeTop(ref treeFoliageVariantKey4);
					this._paintSystem.RequestTreeBranch(ref treeFoliageVariantKey4);
					return;
				}
			}
			else
			{
				int num13 = 0;
				int num14 = 0;
				int num15 = 0;
				int num16 = 0;
				int textureIndex5 = 0;
				int xoffset4 = (tile.frameX == 44).ToInt() - (tile.frameX == 66).ToInt();
				if (WorldGen.GetVanityTreeFoliageData(x, y, xoffset4, ref num13, ref textureIndex5, out num14, out num15, out num16))
				{
					TilePaintSystemV2.TreeFoliageVariantKey treeFoliageVariantKey5 = new TilePaintSystemV2.TreeFoliageVariantKey
					{
						TextureIndex = textureIndex5,
						PaintColor = (int)tile.color()
					};
					this._paintSystem.RequestTreeTop(ref treeFoliageVariantKey5);
					this._paintSystem.RequestTreeBranch(ref treeFoliageVariantKey5);
					return;
				}
			}
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x005C9138 File Offset: 0x005C7338
		public void Update()
		{
			if (Main.dedServ)
			{
				return;
			}
			double num = (double)Math.Abs(Main.WindForVisuals);
			num = (double)Utils.GetLerpValue(0.08f, 1.2f, (float)num, true);
			this._treeWindCounter += 0.004166666666666667 + 0.004166666666666667 * num * 2.0;
			this._grassWindCounter += 0.005555555555555556 + 0.005555555555555556 * num * 4.0;
			this._sunflowerWindCounter += 0.002380952380952381 + 0.002380952380952381 * num * 5.0;
			this._vineWindCounter += 0.008333333333333333 + 0.008333333333333333 * num * 0.4000000059604645;
			this.UpdateLeafFrequency();
			this.EnsureWindGridSize();
			this._windGrid.Update();
			this._shouldShowInvisibleBlocks = Main.ShouldShowInvisibleBlocksAndWalls();
			if (this._shouldShowInvisibleBlocks_LastFrame != this._shouldShowInvisibleBlocks)
			{
				this._shouldShowInvisibleBlocks_LastFrame = this._shouldShowInvisibleBlocks;
				Main.sectionManager.SetAllFramedSectionsAsNeedingRefresh();
			}
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x005C9263 File Offset: 0x005C7463
		public void SpecificHacksForCapture()
		{
			Main.sectionManager.SetAllFramedSectionsAsNeedingRefresh();
			this.ClearSpecialBlockCounts();
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x005C9278 File Offset: 0x005C7478
		public void PreDrawTiles(bool solidLayer, bool intoRenderTargets = false)
		{
			bool flag = intoRenderTargets || Lighting.UpdateEveryFrame;
			if (!solidLayer && flag)
			{
				this.ClearSpecialBlockCounts();
			}
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x005C92A0 File Offset: 0x005C74A0
		public void ClearSpecialBlockCounts()
		{
			this._specialsCount[3] = 0;
			this._specialsCount[2] = 0;
			this._specialsCount[6] = 0;
			this._specialsCount[4] = 0;
			this._specialsCount[1] = 0;
			this._specialsCount[10] = 0;
			this._specialsCount[0] = 0;
			this._specialsCount[7] = 0;
			this._specialsCount[8] = 0;
			this._specialsCount[9] = 0;
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x005C930C File Offset: 0x005C750C
		private void DrawNature(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth, SideFlags seams = SideFlags.None)
		{
			this._natureRenderer.DrawNature(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth, seams);
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x005C9338 File Offset: 0x005C7538
		private void DrawNatureGlowmask(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
		{
			this._natureRenderer.DrawGlowmask(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x005C9360 File Offset: 0x005C7560
		public void PostDrawTiles(bool solidLayer)
		{
			if (!solidLayer)
			{
				TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
				SpriteBatchBeginner beginner = new SpriteBatchBeginner(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
				beginner.Begin(Main.spriteBatch);
				this.DrawMultiTileVines();
				this.DrawMultiTileGrass();
				this.DrawVoidLenses();
				this.DrawTeleportationPylons();
				this.DrawMasterTrophies();
				this.DrawGrass();
				this.DrawAnyDirectionalGrass();
				this.DrawTrees();
				this.DrawVines();
				this.DrawReverseVines();
				Main.spriteBatch.End();
				TimeLogger.TileExtras.AddTime(fromTimestamp);
				this._natureRenderer.DrawAfterAllObjects(beginner);
			}
			if (solidLayer)
			{
				TimeLogger.StartTimestamp fromTimestamp2 = TimeLogger.Start();
				this.DrawEntities_HatRacks();
				this.DrawEntities_DisplayDolls();
				TimeLogger.ClothingRacks.AddTime(fromTimestamp2);
			}
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x005C9424 File Offset: 0x005C7624
		public void DrawLiquidBehindTiles(int waterStyleOverride = -1)
		{
			Main.tileBatch.Restart();
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 screenOffset;
			int num;
			int num2;
			int num3;
			int num4;
			TileDrawing.GetScreenDrawArea(!Main.drawToScreen, out screenOffset, out num, out num2, out num3, out num4);
			for (int i = num3; i < num4 + 4; i++)
			{
				for (int j = num - 2; j < num2 + 2; j++)
				{
					Tile tile = Main.tile[j, i];
					if (tile != null)
					{
						Main.tileBatch.SetLayer(0U, 0);
						this.DrawTile_LiquidBehindTile(false, waterStyleOverride, unscaledPosition, screenOffset, j, i, tile);
					}
				}
			}
			int value = Main.tileBatch.End();
			TimeLogger.LiquidBackgroundDrawCalls.Add(value);
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x005C94D0 File Offset: 0x005C76D0
		public void Draw(bool solidLayer, bool intoRenderTargets, int waterStyleOverride = -1)
		{
			this.FlushLogData = (solidLayer ? TimeLogger.FlushSolidTiles : TimeLogger.FlushNonSolidTiles);
			this.DrawCallLogData = (solidLayer ? TimeLogger.SolidDrawCalls : TimeLogger.NonSolidDrawCalls);
			this._isActiveAndNotPaused = FocusHelper.AllowTileDrawingToEmitEffects;
			this._perspectivePlayer = Main.SceneMetrics.PerspectivePlayer;
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			if (!solidLayer)
			{
				Main.critterCage = false;
			}
			this.EnsureWindGridSize();
			this.ClearLegacyCachedDraws();
			this.ClearCachedTileDraws(solidLayer);
			float num = 255f * (1f - Main.gfxQuality) + 30f * Main.gfxQuality;
			this._highQualityLightingRequirement.R = (byte)num;
			this._highQualityLightingRequirement.G = (byte)((double)num * 1.1);
			this._highQualityLightingRequirement.B = (byte)((double)num * 1.2);
			float num2 = 50f * (1f - Main.gfxQuality) + 2f * Main.gfxQuality;
			this._mediumQualityLightingRequirement.R = (byte)num2;
			this._mediumQualityLightingRequirement.G = (byte)((double)num2 * 1.1);
			this._mediumQualityLightingRequirement.B = (byte)((double)num2 * 1.2);
			if (DebugOptions.devLightTilesCheat)
			{
				this._highQualityLightingRequirement.R = byte.MaxValue;
				this._highQualityLightingRequirement.G = byte.MaxValue;
				this._highQualityLightingRequirement.B = byte.MaxValue;
				this._mediumQualityLightingRequirement.R = byte.MaxValue;
				this._mediumQualityLightingRequirement.G = byte.MaxValue;
				this._mediumQualityLightingRequirement.B = byte.MaxValue;
			}
			Vector2 vector;
			int num3;
			int num4;
			int num5;
			int num6;
			TileDrawing.GetScreenDrawArea(!Main.drawToScreen, out vector, out num3, out num4, out num5, out num6);
			byte b = (byte)(100f + 150f * Main.martianLight);
			this._martianGlow = new Color((int)b, (int)b, (int)b, 0);
			this._lastPaintLookupKey = new TilePaintSystemV2.TileVariationkey
			{
				TileType = -1
			};
			for (int i = num5; i < num6 + 4; i++)
			{
				for (int j = num3 - 2; j < num4 + 2; j++)
				{
					Tile tile = Main.tile[j, i];
					if (tile == null)
					{
						tile = new Tile();
						Main.tile[j, i] = tile;
						Main.mapTime += 60;
					}
					else if (tile.active() && this.IsTileDrawLayerSolid(tile.type) == solidLayer && (!DebugOptions.ShowUnbreakableWall || tile.wall != 350))
					{
						if (solidLayer)
						{
							Main.tileBatch.SetLayer(TileDrawing.Layer_LiquidBehindTiles, 0);
							this.DrawTile_LiquidBehindTile(solidLayer, waterStyleOverride, unscaledPosition, vector, j, i, tile);
						}
						Main.tileBatch.SetLayer(TileDrawing.Layer_Tiles, 0);
						ushort type = tile.type;
						short frameX = tile.frameX;
						short frameY = tile.frameY;
						if (!TextureAssets.Tile[(int)type].IsLoaded)
						{
							Main.instance.LoadTiles((int)type);
						}
						if (type <= 454)
						{
							if (type <= 126)
							{
								if (type <= 52)
								{
									if (type <= 34)
									{
										if (type != 27)
										{
											if (type != 34)
											{
												goto IL_8B8;
											}
											if (frameX % 54 == 0 && frameY % 54 == 0)
											{
												this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine);
												goto IL_8DF;
											}
											goto IL_8DF;
										}
										else
										{
											if (frameX % 36 == 0 && frameY == 0)
											{
												this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass);
												goto IL_8DF;
											}
											goto IL_8DF;
										}
									}
									else
									{
										if (type == 42)
										{
											goto IL_62A;
										}
										if (type != 52)
										{
											goto IL_8B8;
										}
										goto IL_5A3;
									}
								}
								else if (type <= 91)
								{
									if (type == 62)
									{
										goto IL_5A3;
									}
									if (type != 91)
									{
										goto IL_8B8;
									}
									if (frameX % 18 == 0 && frameY % 54 == 0)
									{
										this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine);
										goto IL_8DF;
									}
									goto IL_8DF;
								}
								else if (type != 95)
								{
									if (type == 115)
									{
										goto IL_5A3;
									}
									if (type != 126)
									{
										goto IL_8B8;
									}
								}
							}
							else if (type <= 238)
							{
								if (type <= 205)
								{
									if (type == 184)
									{
										this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.AnyDirectionalGrass);
										goto IL_8DF;
									}
									if (type != 205)
									{
										goto IL_8B8;
									}
									goto IL_5A3;
								}
								else if (type != 233)
								{
									if (type != 236 && type != 238)
									{
										goto IL_8B8;
									}
									goto IL_6DB;
								}
								else
								{
									if (frameY == 0 && frameX % 54 == 0)
									{
										this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass);
									}
									if (frameY == 36 && frameX % 36 == 0)
									{
										this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass);
										goto IL_8DF;
									}
									goto IL_8DF;
								}
							}
							else if (type <= 375)
							{
								if (type - 270 <= 1)
								{
									goto IL_62A;
								}
								if (type - 373 > 2)
								{
									goto IL_8B8;
								}
								goto IL_84A;
							}
							else
							{
								if (type == 382)
								{
									goto IL_5A3;
								}
								if (type != 444)
								{
									if (type != 454)
									{
										goto IL_8B8;
									}
									if (frameX % 72 == 0 && frameY % 54 == 0)
									{
										this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine);
										goto IL_8DF;
									}
									goto IL_8DF;
								}
							}
							if (frameX % 36 == 0 && frameY % 36 == 0)
							{
								this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine);
								goto IL_8DF;
							}
							goto IL_8DF;
						}
						else if (type <= 597)
						{
							if (type <= 530)
							{
								if (type <= 465)
								{
									if (type == 461)
									{
										goto IL_84A;
									}
									if (type != 465)
									{
										goto IL_8B8;
									}
								}
								else
								{
									switch (type)
									{
									case 485:
									case 489:
									case 490:
										if (frameY == 0 && frameX % 36 == 0)
										{
											this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass);
											goto IL_8DF;
										}
										goto IL_8DF;
									case 486:
									case 487:
									case 488:
									case 492:
										goto IL_8B8;
									case 491:
										if (frameX == 18 && frameY == 18)
										{
											this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.VoidLens);
											goto IL_8D3;
										}
										goto IL_8D3;
									case 493:
										if (frameY == 0 && frameX % 18 == 0)
										{
											this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass);
											goto IL_8DF;
										}
										goto IL_8DF;
									default:
										switch (type)
										{
										case 519:
											if (frameX / 18 <= 4)
											{
												this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass);
												goto IL_8DF;
											}
											goto IL_8DF;
										case 520:
										case 529:
											goto IL_8B8;
										case 521:
										case 522:
										case 523:
										case 524:
										case 525:
										case 526:
										case 527:
											if (frameY == 0 && frameX % 36 == 0)
											{
												this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass);
												goto IL_8DF;
											}
											goto IL_8DF;
										case 528:
											goto IL_5A3;
										case 530:
											if (frameX >= 270)
											{
												goto IL_8D3;
											}
											if (frameX % 54 == 0 && frameY == 0)
											{
												this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass);
												goto IL_8DF;
											}
											goto IL_8DF;
										default:
											goto IL_8B8;
										}
										break;
									}
								}
							}
							else if (type <= 572)
							{
								if (type == 549)
								{
									this.CrawlToBottomOfReverseVineAndAddSpecialPoint(i, j);
									goto IL_8DF;
								}
								if (type != 572)
								{
									goto IL_8B8;
								}
								goto IL_62A;
							}
							else
							{
								if (type == 581)
								{
									goto IL_62A;
								}
								if (type - 591 > 1)
								{
									if (type != 597)
									{
										goto IL_8B8;
									}
									if (frameX % 54 == 0 && frameY == 0)
									{
										this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.TeleportationPylon);
										goto IL_8D3;
									}
									goto IL_8D3;
								}
							}
							if (frameX % 36 == 0 && frameY % 54 == 0)
							{
								this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine);
								goto IL_8DF;
							}
							goto IL_8DF;
						}
						else if (type <= 652)
						{
							if (type <= 636)
							{
								if (type != 617)
								{
									if (type != 636)
									{
										goto IL_8B8;
									}
								}
								else
								{
									if (frameX % 54 == 0 && frameY % 72 == 0)
									{
										this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MasterTrophy);
										goto IL_8D3;
									}
									goto IL_8D3;
								}
							}
							else if (type != 638)
							{
								if (type != 651)
								{
									if (type != 652)
									{
										goto IL_8B8;
									}
									if (frameX % 36 == 0)
									{
										this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass);
										goto IL_8DF;
									}
									goto IL_8DF;
								}
								else
								{
									if (frameX % 54 == 0)
									{
										this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass);
										goto IL_8DF;
									}
									goto IL_8DF;
								}
							}
						}
						else if (type <= 698)
						{
							if (type == 660)
							{
								goto IL_62A;
							}
							if (type != 698)
							{
								goto IL_8B8;
							}
							if (frameX % 18 == 0 && frameY == 0)
							{
								this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine);
								goto IL_8DF;
							}
							goto IL_8DF;
						}
						else
						{
							if (type == 702)
							{
								goto IL_6DB;
							}
							if (type != 705)
							{
								if (type != 709)
								{
									goto IL_8B8;
								}
								goto IL_84A;
							}
							else
							{
								if (frameX % 486 >= 270)
								{
									goto IL_8D3;
								}
								if (frameX % 54 == 0 && frameY % 36 == 0)
								{
									this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass);
									goto IL_8DF;
								}
								goto IL_8DF;
							}
						}
						IL_5A3:
						this.CrawlToTopOfVineAndAddSpecialPoint(i, j);
						goto IL_8DF;
						IL_62A:
						if (frameX % 18 == 0 && frameY % 36 == 0)
						{
							this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileVine);
							goto IL_8DF;
						}
						goto IL_8DF;
						IL_6DB:
						if (frameX % 36 == 0 && frameY == 0)
						{
							this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.MultiTileGrass);
							goto IL_8DF;
						}
						goto IL_8DF;
						IL_84A:
						this.EmitLiquidDrops(i, j, tile, type);
						goto IL_8DF;
						IL_8B8:
						if (this.ShouldSwayInWind(j, i, tile))
						{
							this.AddSpecialPoint(j, i, TileDrawing.TileCounterType.WindyGrass);
							goto IL_8DF;
						}
						IL_8D3:
						this.DrawSingleTile(unscaledPosition, vector, j, i);
					}
					IL_8DF:;
				}
			}
			base.RestartLayeredBatch();
			if (solidLayer)
			{
				Main.instance.DrawTileCracks(1, Main.player[Main.myPlayer].hitReplace);
				Main.instance.DrawTileCracks(1, Main.player[Main.myPlayer].hitTile);
				base.RestartSpriteBatch();
			}
			this.DrawSpecialTilesLegacy(unscaledPosition, vector);
			if (TileObject.objectPreview.Active && Main.LocalPlayer.cursorItemIconEnabled && Main.placementPreview && !CaptureManager.Instance.Active)
			{
				Main.instance.LoadTiles((int)TileObject.objectPreview.Type);
				float placementPreviewOpacity = Main.LocalPlayer.GetPlacementPreviewOpacity();
				TileObject.DrawPreview(Main.spriteBatch, TileObject.objectPreview, unscaledPosition - vector, placementPreviewOpacity);
			}
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x005C9E90 File Offset: 0x005C8090
		private void CrawlToTopOfVineAndAddSpecialPoint(int j, int i)
		{
			int y = j;
			for (int k = j - 1; k > 0; k--)
			{
				Tile tile = Main.tile[i, k];
				if (WorldGen.BottomEdgeCanBeAttachedTo(i, k) || !tile.active())
				{
					y = k + 1;
					break;
				}
			}
			Point item = new Point(i, y);
			if (this._vineRootsPositions.Contains(item))
			{
				return;
			}
			this._vineRootsPositions.Add(item);
			this.AddSpecialPoint(i, y, TileDrawing.TileCounterType.Vine);
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x005C9F00 File Offset: 0x005C8100
		private void CrawlToBottomOfReverseVineAndAddSpecialPoint(int j, int i)
		{
			int y = j;
			for (int k = j; k < Main.maxTilesY; k++)
			{
				Tile tile = Main.tile[i, k];
				if (WorldGen.TopEdgeCanBeAttachedTo(i, k) || !tile.active())
				{
					y = k - 1;
					break;
				}
			}
			Point item = new Point(i, y);
			if (this._reverseVineRootsPositions.Contains(item))
			{
				return;
			}
			this._reverseVineRootsPositions.Add(item);
			this.AddSpecialPoint(i, y, TileDrawing.TileCounterType.ReverseVine);
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x005C9F70 File Offset: 0x005C8170
		private static float SmoothStep(float x)
		{
			return x * x * (3f - 2f * x);
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x005C9F84 File Offset: 0x005C8184
		static TileDrawing()
		{
			Random random = new Random(0);
			for (int i = 0; i < TileDrawing.noise.Length; i++)
			{
				TileDrawing.noise[i] = (float)(random.NextDouble() * 2.0 - 1.0);
			}
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x005CA000 File Offset: 0x005C8200
		private static float LinearNoise(float x)
		{
			int num = (int)x;
			if (x < 0f)
			{
				num--;
			}
			int num2 = num + 1;
			float num3 = x - (float)num;
			float num4 = TileDrawing.noise[num & 255];
			float num5 = TileDrawing.noise[num2 & 255];
			float num6 = num3;
			return num4 * (1f - num6) + num5 * num6;
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x005CA051 File Offset: 0x005C8251
		private static uint Hash(uint x)
		{
			x ^= x >> 16;
			x *= 2146121005U;
			x ^= x >> 15;
			x *= 2221713035U;
			x ^= x >> 16;
			return x;
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x005CA07E File Offset: 0x005C827E
		private static uint Hash2(uint x)
		{
			x ^= x >> 15;
			x *= 3513297581U;
			x ^= x >> 15;
			x *= 2943497623U;
			x ^= x >> 15;
			return x;
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x005CA0AC File Offset: 0x005C82AC
		private static float DistToWanderingCircle(Point pos, int gridSize, float wanderDist, float cyclesPerTick, uint seed)
		{
			Point point = new Point(pos.X / gridSize, pos.Y / gridSize);
			Vector2 value = new Vector2(((float)point.X + 0.5f) * (float)gridSize, ((float)point.Y + 0.5f) * (float)gridSize);
			float num = (float)Main.timeForVisualEffects;
			uint num2 = (TileDrawing.Hash((uint)point.X) ^ TileDrawing.Hash2((uint)point.Y) ^ seed) & 16777215U;
			uint num3 = (TileDrawing.Hash2((uint)point.X) ^ TileDrawing.Hash((uint)point.Y) ^ seed) & 16777215U;
			value.X += TileDrawing.LinearNoise((num2 + num) * cyclesPerTick) * wanderDist;
			value.Y += TileDrawing.LinearNoise((num3 + num) * cyclesPerTick) * wanderDist;
			return Vector2.Distance(pos.ToVector2(), value);
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x005CA180 File Offset: 0x005C8380
		private static float LavaLightA(int tileX, int tileY)
		{
			float val = TileDrawing.DistToWanderingCircle(new Point(tileX, tileY), 7, 2f, 0.025f, 2221713035U);
			float val2 = TileDrawing.DistToWanderingCircle(new Point(tileX + 3, tileY), 7, 2f, 0.025f, 657044585U);
			float val3 = TileDrawing.DistToWanderingCircle(new Point(tileX, tileY + 3), 7, 2f, 0.025f, 741521833U);
			float val4 = TileDrawing.DistToWanderingCircle(new Point(tileX + 3, tileY + 3), 7, 2f, 0.025f, 56936621U);
			float num = Math.Min(Math.Min(val, val2), Math.Min(val3, val4));
			num = TileDrawing.SmoothStep(1f - Utils.GetLerpValue(0f, 3.5f, num, true));
			float toMin = 0f;
			if (!WorldGen.SolidTile(tileX, tileY - 1, false))
			{
				toMin = 0.8f;
			}
			return Utils.Remap(num, 0.7f, 1f, toMin, 1f, true);
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x005CA26C File Offset: 0x005C846C
		private void DrawSingleTile(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY)
		{
			TileDrawInfo tileDrawInfo = new TileDrawInfo();
			tileDrawInfo.tileCache = Main.tile[tileX, tileY];
			tileDrawInfo.typeCache = tileDrawInfo.tileCache.type;
			tileDrawInfo.tileFrameX = tileDrawInfo.tileCache.frameX;
			tileDrawInfo.tileFrameY = tileDrawInfo.tileCache.frameY;
			tileDrawInfo.tileLight = Lighting.GetColor(tileX, tileY);
			if (tileDrawInfo.tileCache.liquid > 0 && tileDrawInfo.tileCache.type == 518)
			{
				return;
			}
			this.GetTileDrawData(tileX, tileY, tileDrawInfo.tileCache, tileDrawInfo.typeCache, ref tileDrawInfo.tileFrameX, ref tileDrawInfo.tileFrameY, out tileDrawInfo.tileWidth, out tileDrawInfo.tileHeight, out tileDrawInfo.tileTop, out tileDrawInfo.halfBrickHeight, out tileDrawInfo.addFrX, out tileDrawInfo.addFrY, out tileDrawInfo.tileSpriteEffect, out tileDrawInfo.glowTexture, out tileDrawInfo.glowSourceRect, out tileDrawInfo.glowColor);
			if (tileDrawInfo.tileTop < 0)
			{
				Main.tileBatch.SetLayer(TileDrawing.Layer_OverTiles, 0);
			}
			else if (tileDrawInfo.tileTop + tileDrawInfo.tileHeight <= 16)
			{
				Main.tileBatch.SetLayer(TileDrawing.Layer_Tiles, 0);
			}
			else
			{
				Main.tileBatch.SetLayer(TileDrawing.Layer_BehindTiles, 0);
			}
			tileDrawInfo.drawTexture = this.GetTileDrawTexture(tileDrawInfo.tileCache, tileX, tileY);
			Texture2D texture2D = null;
			Rectangle empty = Rectangle.Empty;
			Color transparent = Color.Transparent;
			if (TileID.Sets.HasOutlines[(int)tileDrawInfo.typeCache])
			{
				this.GetTileOutlineInfo(tileX, tileY, tileDrawInfo.typeCache, ref tileDrawInfo.tileLight, ref texture2D, ref transparent);
			}
			if (this._perspectivePlayer.dangerSense && TileDrawing.IsTileDangerous(this._perspectivePlayer, tileDrawInfo.tileCache, tileDrawInfo.typeCache))
			{
				if (tileDrawInfo.tileLight.R < 255)
				{
					tileDrawInfo.tileLight.R = byte.MaxValue;
				}
				if (tileDrawInfo.tileLight.G < 50)
				{
					tileDrawInfo.tileLight.G = 50;
				}
				if (tileDrawInfo.tileLight.B < 50)
				{
					tileDrawInfo.tileLight.B = 50;
				}
				if (this._isActiveAndNotPaused && this._rand.Next(30) == 0)
				{
					int num = Dust.NewDust(new Vector2((float)(tileX * 16), (float)(tileY * 16)), 16, 16, 60, 0f, 0f, 100, default(Color), 0.3f);
					this._dust[num].fadeIn = 1f;
					this._dust[num].velocity *= 0.1f;
					this._dust[num].noLight = true;
					this._dust[num].noGravity = true;
				}
			}
			if (this._perspectivePlayer.findTreasure && Main.IsTileSpelunkable(tileDrawInfo.typeCache, tileDrawInfo.tileFrameX, tileDrawInfo.tileFrameY))
			{
				if (tileDrawInfo.tileLight.R < 200)
				{
					tileDrawInfo.tileLight.R = 200;
				}
				if (tileDrawInfo.tileLight.G < 170)
				{
					tileDrawInfo.tileLight.G = 170;
				}
				if (this._isActiveAndNotPaused && this._rand.Next(60) == 0)
				{
					int num2 = Dust.NewDust(new Vector2((float)(tileX * 16), (float)(tileY * 16)), 16, 16, 204, 0f, 0f, 150, default(Color), 0.3f);
					this._dust[num2].fadeIn = 1f;
					this._dust[num2].velocity *= 0.1f;
					this._dust[num2].noLight = true;
				}
			}
			if (this._perspectivePlayer.biomeSight)
			{
				Color white = Color.White;
				if (Main.IsTileBiomeSightable(tileDrawInfo.typeCache, tileDrawInfo.tileFrameX, tileDrawInfo.tileFrameY, ref white))
				{
					if (tileDrawInfo.tileLight.R < white.R)
					{
						tileDrawInfo.tileLight.R = white.R;
					}
					if (tileDrawInfo.tileLight.G < white.G)
					{
						tileDrawInfo.tileLight.G = white.G;
					}
					if (tileDrawInfo.tileLight.B < white.B)
					{
						tileDrawInfo.tileLight.B = white.B;
					}
					if (this._isActiveAndNotPaused && this._rand.Next(480) == 0)
					{
						Color newColor = white;
						int num3 = Dust.NewDust(new Vector2((float)(tileX * 16), (float)(tileY * 16)), 16, 16, 267, 0f, 0f, 150, newColor, 0.3f);
						this._dust[num3].noGravity = true;
						this._dust[num3].fadeIn = 1f;
						this._dust[num3].velocity *= 0.1f;
						this._dust[num3].noLightEmittence = true;
					}
				}
			}
			if (this._isActiveAndNotPaused)
			{
				if (!Lighting.UpdateEveryFrame || new FastRandom(Main.TileFrameSeed).WithModifier(tileX, tileY).Next(4) == 0)
				{
					this.DrawTiles_EmitParticles(tileY, tileX, tileDrawInfo.tileCache, tileDrawInfo.typeCache, tileDrawInfo.tileFrameX, tileDrawInfo.tileFrameY, tileDrawInfo.tileLight);
				}
				tileDrawInfo.tileLight = this.DrawTiles_GetLightOverride(tileY, tileX, tileDrawInfo.tileCache, tileDrawInfo.typeCache, tileDrawInfo.tileFrameX, tileDrawInfo.tileFrameY, tileDrawInfo.tileLight);
			}
			bool flag = false;
			if (tileDrawInfo.glowTexture != null || Main.tileGlowMask[(int)tileDrawInfo.typeCache] != -1 || Main.tileFlame[(int)tileDrawInfo.typeCache])
			{
				flag = true;
			}
			if (tileDrawInfo.tileLight.R >= 1 || tileDrawInfo.tileLight.G >= 1 || tileDrawInfo.tileLight.B >= 1 || TileID.Sets.IgnoreDrawLightConditions[(int)tileDrawInfo.typeCache])
			{
				flag = true;
			}
			if (tileDrawInfo.tileCache.wall > 0 && (tileDrawInfo.tileCache.wall == 318 || tileDrawInfo.tileCache.fullbrightWall()))
			{
				flag = true;
			}
			flag &= this.IsVisible(tileDrawInfo.tileCache);
			this.CacheSpecialDraws_Part1(tileX, tileY, (int)tileDrawInfo.typeCache, (int)tileDrawInfo.tileFrameX, (int)tileDrawInfo.tileFrameY, !flag);
			this.CacheSpecialDraws_Part2(tileX, tileY, tileDrawInfo, !flag);
			if (tileDrawInfo.typeCache == 72 && tileDrawInfo.tileFrameX >= 36)
			{
				int num4 = 0;
				if (tileDrawInfo.tileFrameY == 18)
				{
					num4 = 1;
				}
				else if (tileDrawInfo.tileFrameY == 36)
				{
					num4 = 2;
				}
				Main.tileBatch.Draw(TextureAssets.ShroomCap.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X - 22), (float)(tileY * 16 - (int)screenPosition.Y - 26)) + screenOffset, new Rectangle?(new Rectangle(num4 * 62, 0, 60, 42)), Lighting.GetColor(tileX, tileY), TileDrawing._zero, 1f, tileDrawInfo.tileSpriteEffect);
			}
			Rectangle rectangle = new Rectangle((int)tileDrawInfo.tileFrameX + tileDrawInfo.addFrX, (int)tileDrawInfo.tileFrameY + tileDrawInfo.addFrY, tileDrawInfo.tileWidth, tileDrawInfo.tileHeight - tileDrawInfo.halfBrickHeight);
			float num5 = ((float)tileDrawInfo.tileWidth - 16f) / 2f;
			if (tileDrawInfo.typeCache >= 0 && TileID.Sets.DoNotAdjustDrawPositionBasedOnTileWidth[(int)tileDrawInfo.typeCache])
			{
				num5 = 0f;
			}
			Vector2 vector = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - num5, (float)(tileY * 16 - (int)screenPosition.Y + tileDrawInfo.tileTop + tileDrawInfo.halfBrickHeight)) + screenOffset;
			if (flag)
			{
				tileDrawInfo.colorTint = Color.White;
				tileDrawInfo.finalColor = TileDrawing.GetFinalLight(tileDrawInfo.tileCache, tileDrawInfo.typeCache, tileDrawInfo.tileLight, tileDrawInfo.colorTint);
				ushort num6 = tileDrawInfo.typeCache;
				if (num6 <= 272)
				{
					if (num6 <= 114)
					{
						if (num6 <= 80)
						{
							if (num6 != 51)
							{
								if (num6 != 80)
								{
									goto IL_CF8;
								}
								bool flag2;
								bool flag3;
								bool flag4;
								WorldGen.GetCactusType(tileX, tileY, (int)tileDrawInfo.tileFrameX, (int)tileDrawInfo.tileFrameY, out flag2, out flag3, out flag4);
								if (flag2)
								{
									rectangle.Y += 54;
								}
								if (flag3)
								{
									rectangle.Y += 108;
								}
								if (flag4)
								{
									rectangle.Y += 162;
									goto IL_CF8;
								}
								goto IL_CF8;
							}
						}
						else
						{
							if (num6 == 83)
							{
								tileDrawInfo.drawTexture = this.GetTileDrawTexture(tileDrawInfo.tileCache, tileX, tileY);
								goto IL_CF8;
							}
							if (num6 != 114)
							{
								goto IL_CF8;
							}
							if (tileDrawInfo.tileFrameY > 0)
							{
								rectangle.Height += 2;
								goto IL_CF8;
							}
							goto IL_CF8;
						}
					}
					else if (num6 <= 136)
					{
						if (num6 != 129)
						{
							if (num6 != 136)
							{
								goto IL_CF8;
							}
							int num7 = (int)(tileDrawInfo.tileFrameX / 18);
							if (num7 == 1)
							{
								vector.X += -2f;
								goto IL_CF8;
							}
							if (num7 != 2)
							{
								goto IL_CF8;
							}
							vector.X += 2f;
							goto IL_CF8;
						}
						else
						{
							tileDrawInfo.finalColor = new Color(255, 255, 255, 100);
							int num8 = 2;
							if (tileDrawInfo.tileFrameX >= 324)
							{
								tileDrawInfo.finalColor = Color.Transparent;
							}
							if (tileDrawInfo.tileFrameY < 36)
							{
								vector.Y += (float)(num8 * (tileDrawInfo.tileFrameY == 0).ToDirectionInt());
								goto IL_CF8;
							}
							vector.X += (float)(num8 * (tileDrawInfo.tileFrameY == 36).ToDirectionInt());
							goto IL_CF8;
						}
					}
					else
					{
						if (num6 == 160)
						{
							goto IL_A7C;
						}
						if (num6 != 272)
						{
							goto IL_CF8;
						}
						int num9 = Main.tileFrame[(int)tileDrawInfo.typeCache];
						num9 += tileX % 2;
						num9 += tileY % 2;
						num9 += tileX % 3;
						num9 += tileY % 3;
						num9 %= 2;
						num9 *= 90;
						tileDrawInfo.addFrY += num9;
						rectangle.Y += num9;
						goto IL_CF8;
					}
				}
				else if (num6 <= 697)
				{
					if (num6 <= 442)
					{
						if (num6 != 323)
						{
							if (num6 != 442)
							{
								goto IL_CF8;
							}
							int num7 = (int)(tileDrawInfo.tileFrameX / 22);
							if (num7 == 3)
							{
								vector.X += 2f;
								goto IL_CF8;
							}
							goto IL_CF8;
						}
						else
						{
							if (tileDrawInfo.tileCache.frameX <= 132 && tileDrawInfo.tileCache.frameX >= 88)
							{
								return;
							}
							vector.X += (float)tileDrawInfo.tileCache.frameY;
							goto IL_CF8;
						}
					}
					else
					{
						if (num6 == 692)
						{
							goto IL_A7C;
						}
						if (num6 != 697)
						{
							goto IL_CF8;
						}
					}
				}
				else if (num6 <= 726)
				{
					if (num6 - 723 > 1)
					{
						if (num6 != 726)
						{
							goto IL_CF8;
						}
						vector.X -= 2f;
						switch (tileDrawInfo.tileCache.blockType())
						{
						case 2:
							vector.X += 6f;
							vector.Y += 2f;
							goto IL_CF8;
						case 3:
							vector.X -= 6f;
							vector.Y += 2f;
							goto IL_CF8;
						case 4:
							vector.X += 6f;
							goto IL_CF8;
						case 5:
							vector.X -= 6f;
							goto IL_CF8;
						default:
							goto IL_CF8;
						}
					}
					else
					{
						switch (tileDrawInfo.tileFrameX / 18)
						{
						case 0:
							vector += new Vector2(0f, 2f);
							goto IL_CF8;
						case 1:
							vector += new Vector2(0f, -2f);
							goto IL_CF8;
						case 2:
							vector += new Vector2(-2f, 0f);
							goto IL_CF8;
						case 3:
							vector += new Vector2(2f, 0f);
							goto IL_CF8;
						default:
							goto IL_CF8;
						}
					}
				}
				else if (num6 != 751)
				{
					if (num6 != 752)
					{
						goto IL_CF8;
					}
					if (tileDrawInfo.tileFrameX != 0 || tileDrawInfo.tileFrameY != 0)
					{
						return;
					}
					vector.X += 8f;
					goto IL_CF8;
				}
				else
				{
					if (tileDrawInfo.tileFrameX != 0 || tileDrawInfo.tileCache.frameY != 0)
					{
						return;
					}
					vector.X += 11f;
					vector.Y -= 8f;
					goto IL_CF8;
				}
				tileDrawInfo.finalColor = tileDrawInfo.tileLight * 0.5f;
				goto IL_CF8;
				IL_A7C:
				Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 255);
				if (tileDrawInfo.tileCache.inActive())
				{
					color = tileDrawInfo.tileCache.actColor(color);
				}
				tileDrawInfo.finalColor = color;
				IL_CF8:
				if (tileDrawInfo.typeCache == 314)
				{
					this.DrawTile_MinecartTrack(screenPosition, screenOffset, tileX, tileY, tileDrawInfo);
				}
				else if (tileDrawInfo.typeCache == 171)
				{
					this.DrawXmasTree(screenPosition, screenOffset, tileX, tileY, tileDrawInfo);
				}
				else
				{
					this.DrawBasicTile(screenPosition, screenOffset, tileX, tileY, tileDrawInfo, rectangle, vector);
				}
				if (Main.tileGlowMask[(int)tileDrawInfo.tileCache.type] != -1)
				{
					short num10 = Main.tileGlowMask[(int)tileDrawInfo.tileCache.type];
					if (TextureAssets.GlowMask.IndexInRange((int)num10))
					{
						tileDrawInfo.drawTexture = TextureAssets.GlowMask[(int)num10].Value;
					}
					double num11 = Main.timeForVisualEffects * 0.08;
					Color color2 = Color.White;
					bool flag5 = false;
					num6 = tileDrawInfo.tileCache.type;
					if (num6 <= 445)
					{
						if (num6 <= 370)
						{
							if (num6 <= 209)
							{
								if (num6 != 129)
								{
									if (num6 != 209)
									{
										goto IL_1316;
									}
									color2 = PortalHelper.GetPortalColor(Main.myPlayer, (tileDrawInfo.tileCache.frameX >= 288) ? 1 : 0);
									goto IL_1316;
								}
								else
								{
									if (tileDrawInfo.tileFrameX < 324)
									{
										flag5 = true;
										goto IL_1316;
									}
									tileDrawInfo.drawTexture = this.GetTileDrawTexture(tileDrawInfo.tileCache, tileX, tileY);
									color2 = Main.hslToRgb(0.7f + (float)Math.Sin((double)(6.2831855f * Main.GlobalTimeWrappedHourly * 0.16f + (float)tileX * 0.3f + (float)tileY * 0.7f)) * 0.16f, 1f, 0.5f, byte.MaxValue);
									color2.A /= 2;
									color2 *= 0.3f;
									int num12 = 72;
									for (float num13 = 0f; num13 < 6.2831855f; num13 += 1.5707964f)
									{
										Main.tileBatch.Draw(tileDrawInfo.drawTexture, vector + num13.ToRotationVector2() * 2f, new Rectangle?(new Rectangle((int)tileDrawInfo.tileFrameX + tileDrawInfo.addFrX, (int)tileDrawInfo.tileFrameY + tileDrawInfo.addFrY + num12, tileDrawInfo.tileWidth, tileDrawInfo.tileHeight)), color2, Vector2.Zero, 1f, SpriteEffects.None);
									}
									color2 = new Color(255, 255, 255, 100);
									goto IL_1316;
								}
							}
							else
							{
								if (num6 == 350)
								{
									color2 = new Color(new Vector4((float)(-(float)Math.Cos(((int)(num11 / 6.283) % 3 == 1) ? num11 : 0.0) * 0.2 + 0.2)));
									goto IL_1316;
								}
								if (num6 != 370)
								{
									goto IL_1316;
								}
							}
						}
						else if (num6 <= 390)
						{
							if (num6 == 381)
							{
								goto IL_101D;
							}
							if (num6 != 390)
							{
								goto IL_1316;
							}
						}
						else
						{
							if (num6 == 391)
							{
								color2 = new Color(250, 250, 250, 200);
								goto IL_1316;
							}
							if (num6 != 429 && num6 != 445)
							{
								goto IL_1316;
							}
							tileDrawInfo.drawTexture = this.GetTileDrawTexture(tileDrawInfo.tileCache, tileX, tileY);
							tileDrawInfo.addFrY = 18;
							goto IL_1316;
						}
						color2 = this._meteorGlow;
						goto IL_1316;
					}
					if (num6 <= 667)
					{
						if (num6 <= 540)
						{
							if (num6 == 517)
							{
								goto IL_101D;
							}
							switch (num6)
							{
							case 534:
							case 535:
								goto IL_102A;
							case 536:
							case 537:
								goto IL_1037;
							case 538:
								goto IL_1316;
							case 539:
							case 540:
								goto IL_1044;
							default:
								goto IL_1316;
							}
						}
						else
						{
							switch (num6)
							{
							case 625:
							case 626:
								goto IL_1051;
							case 627:
							case 628:
								goto IL_105E;
							case 629:
							case 630:
							case 631:
							case 632:
								goto IL_1316;
							case 633:
								color2 = Color.Lerp(Color.White, tileDrawInfo.finalColor, 0.75f);
								goto IL_1316;
							default:
								if (num6 != 659 && num6 != 667)
								{
									goto IL_1316;
								}
								break;
							}
						}
					}
					else if (num6 <= 708)
					{
						switch (num6)
						{
						case 687:
							goto IL_101D;
						case 688:
							goto IL_1044;
						case 689:
							goto IL_102A;
						case 690:
							goto IL_1037;
						case 691:
							goto IL_1051;
						case 692:
							goto IL_105E;
						case 693:
						case 694:
						case 695:
						case 696:
						case 697:
						case 698:
							goto IL_1316;
						case 699:
							color2 = Color.White;
							goto IL_1316;
						default:
							if (num6 != 708)
							{
								goto IL_1316;
							}
							break;
						}
					}
					else
					{
						if (num6 == 717)
						{
							float num14 = TileDrawing.LavaLightA(tileX, tileY);
							color2 = new Color(num14, num14, num14, num14 / 2f);
							goto IL_1316;
						}
						if (num6 == 718)
						{
							color2 = new Color(0, 0, 0, 0);
							goto IL_1316;
						}
						if (num6 != 725)
						{
							goto IL_1316;
						}
						float opacity = Filters.Scene["Noir"].Opacity;
						if (opacity > 0f && tileDrawInfo.tileFrameX % 36 == 0 && tileDrawInfo.tileFrameY == 54)
						{
							Vector2 position = vector + new Vector2(16f, 24f);
							SpriteEffects effects = (tileDrawInfo.tileFrameX >= 36) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
							color2 = new Color(255, 255, 255, 100) * opacity;
							Main.tileBatch.Draw(tileDrawInfo.drawTexture, position, new Rectangle?(tileDrawInfo.drawTexture.Frame(1, 1, 0, 0, 0, 0)), color2, tileDrawInfo.drawTexture.Frame(1, 1, 0, 0, 0, 0).Center.ToVector2(), 1f, effects);
						}
						flag5 = true;
						goto IL_1316;
					}
					color2 = LiquidRenderer.GetShimmerGlitterColor(true, (float)tileX, (float)tileY);
					goto IL_1316;
					IL_102A:
					color2 = this._kryptonMossGlow;
					goto IL_1316;
					IL_1037:
					color2 = this._xenonMossGlow;
					goto IL_1316;
					IL_1044:
					color2 = this._argonMossGlow;
					goto IL_1316;
					IL_1051:
					color2 = this._violetMossGlow;
					goto IL_1316;
					IL_105E:
					color2 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
					goto IL_1316;
					IL_101D:
					color2 = this._lavaMossGlow;
					IL_1316:
					if (!flag5)
					{
						if (tileDrawInfo.tileCache.slope() == 0 && !tileDrawInfo.tileCache.halfBrick())
						{
							Main.tileBatch.Draw(tileDrawInfo.drawTexture, vector, new Rectangle?(new Rectangle((int)tileDrawInfo.tileFrameX + tileDrawInfo.addFrX, (int)tileDrawInfo.tileFrameY + tileDrawInfo.addFrY, tileDrawInfo.tileWidth, tileDrawInfo.tileHeight)), color2, Vector2.Zero, 1f, SpriteEffects.None);
						}
						else if (tileDrawInfo.tileCache.halfBrick())
						{
							Main.tileBatch.Draw(tileDrawInfo.drawTexture, vector, new Rectangle?(rectangle), color2, TileDrawing._zero, 1f, SpriteEffects.None);
						}
						else if (TileID.Sets.HasSlopeFrames[(int)tileDrawInfo.tileCache.type])
						{
							Main.tileBatch.Draw(tileDrawInfo.drawTexture, vector, new Rectangle?(new Rectangle((int)tileDrawInfo.tileFrameX + tileDrawInfo.addFrX, (int)tileDrawInfo.tileFrameY + tileDrawInfo.addFrY, 16, 16)), color2, TileDrawing._zero, 1f, tileDrawInfo.tileSpriteEffect);
						}
						else
						{
							int num15 = (int)tileDrawInfo.tileCache.slope();
							int num16 = 2;
							for (int i = 0; i < 8; i++)
							{
								int num17 = i * -2;
								int num18 = 16 - i * 2;
								int num19 = 16 - num18;
								int num20;
								switch (num15)
								{
								case 1:
									num17 = 0;
									num20 = i * 2;
									num18 = 14 - i * 2;
									num19 = 0;
									break;
								case 2:
									num17 = 0;
									num20 = 16 - i * 2 - 2;
									num18 = 14 - i * 2;
									num19 = 0;
									break;
								case 3:
									num20 = i * 2;
									break;
								default:
									num20 = 16 - i * 2 - 2;
									break;
								}
								Main.tileBatch.Draw(tileDrawInfo.drawTexture, vector + new Vector2((float)num20, (float)(i * num16 + num17)), new Rectangle?(new Rectangle((int)tileDrawInfo.tileFrameX + tileDrawInfo.addFrX + num20, (int)tileDrawInfo.tileFrameY + tileDrawInfo.addFrY + num19, num16, num18)), color2, TileDrawing._zero, 1f, tileDrawInfo.tileSpriteEffect);
							}
							int num21 = (num15 > 2) ? 0 : 14;
							Main.tileBatch.Draw(tileDrawInfo.drawTexture, vector + new Vector2(0f, (float)num21), new Rectangle?(new Rectangle((int)tileDrawInfo.tileFrameX + tileDrawInfo.addFrX, (int)tileDrawInfo.tileFrameY + tileDrawInfo.addFrY + num21, 16, 2)), color2, TileDrawing._zero, 1f, tileDrawInfo.tileSpriteEffect);
						}
					}
				}
				if (tileDrawInfo.glowTexture != null)
				{
					if (tileDrawInfo.typeCache == 412)
					{
						int num22 = Main.tileFrame[(int)tileDrawInfo.typeCache] / 60;
						int num23 = (num22 + 1) % 4;
						float num24 = (float)(Main.tileFrame[(int)tileDrawInfo.typeCache] % 60) / 60f;
						Rectangle glowSourceRect = tileDrawInfo.glowSourceRect;
						glowSourceRect.Y += num22 * 18 * 3;
						Rectangle glowSourceRect2 = tileDrawInfo.glowSourceRect;
						glowSourceRect2.Y += num23 * 18 * 3;
						Vector2 position2 = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)tileDrawInfo.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + tileDrawInfo.tileTop)) + screenOffset;
						Main.tileBatch.Draw(tileDrawInfo.glowTexture, position2, new Rectangle?(glowSourceRect), tileDrawInfo.glowColor * (1f - num24), TileDrawing._zero, 1f, tileDrawInfo.tileSpriteEffect);
						Main.tileBatch.Draw(tileDrawInfo.glowTexture, position2, new Rectangle?(glowSourceRect2), tileDrawInfo.glowColor * num24, TileDrawing._zero, 1f, tileDrawInfo.tileSpriteEffect);
					}
					else
					{
						Vector2 vector2 = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)tileDrawInfo.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + tileDrawInfo.tileTop)) + screenOffset;
						if (TileID.Sets.Platforms[(int)tileDrawInfo.typeCache])
						{
							vector2 = vector;
						}
						Main.tileBatch.SetLayer(TileDrawing.Layer_Tiles, 1);
						Main.tileBatch.Draw(tileDrawInfo.glowTexture, vector2, new Rectangle?(tileDrawInfo.glowSourceRect), tileDrawInfo.glowColor, TileDrawing._zero, 1f, tileDrawInfo.tileSpriteEffect);
						if (TileID.Sets.Platforms[(int)tileDrawInfo.typeCache] && tileDrawInfo.tileCache.slope() != 0)
						{
							Tile tile = Main.tile[tileX, tileY + 1];
							Tile tile2 = Main.tile[tileX - 1, tileY + 1];
							Tile tile3 = Main.tile[tileX + 1, tileY + 1];
							bool shouldShowInvisibleBlocks = this._shouldShowInvisibleBlocks;
							if (tileDrawInfo.tileCache.slope() == 1 && tile3.active() && (shouldShowInvisibleBlocks || !tile3.invisibleBlock()) && Main.tileSolid[(int)tile3.type] && tile3.slope() != 2 && !tile3.halfBrick() && (!tile.active() || (!shouldShowInvisibleBlocks && tile.invisibleBlock()) || (tile.blockType() != 0 && tile.blockType() != 5) || !TileID.Sets.BlocksStairs[(int)tile.type]))
							{
								Rectangle glowSourceRect3 = tileDrawInfo.glowSourceRect;
								if (TileID.Sets.Platforms[(int)tile3.type] && tile3.slope() == 0)
								{
									glowSourceRect3.X = 324;
								}
								else
								{
									glowSourceRect3.X = 198;
								}
								Main.tileBatch.SetLayer(TileDrawing.Layer_BehindTiles, 1);
								Main.tileBatch.Draw(tileDrawInfo.glowTexture, vector2 + new Vector2(0f, 16f), new Rectangle?(glowSourceRect3), tileDrawInfo.glowColor, TileDrawing._zero, 1f, tileDrawInfo.tileSpriteEffect);
							}
							else if (tileDrawInfo.tileCache.slope() == 2 && tile2.active() && (shouldShowInvisibleBlocks || !tile2.invisibleBlock()) && Main.tileSolid[(int)tile2.type] && tile2.slope() != 1 && !tile2.halfBrick() && (!tile.active() || (!shouldShowInvisibleBlocks && tile.invisibleBlock()) || (tile.blockType() != 0 && tile.blockType() != 4) || !TileID.Sets.BlocksStairs[(int)tile.type]))
							{
								Rectangle glowSourceRect4 = tileDrawInfo.glowSourceRect;
								if (TileID.Sets.Platforms[(int)tile2.type] && tile2.slope() == 0)
								{
									glowSourceRect4.X = 306;
								}
								else
								{
									glowSourceRect4.X = 162;
								}
								Main.tileBatch.SetLayer(TileDrawing.Layer_BehindTiles, 1);
								Main.tileBatch.Draw(tileDrawInfo.glowTexture, vector2 + new Vector2(0f, 16f), new Rectangle?(glowSourceRect4), tileDrawInfo.glowColor, TileDrawing._zero, 1f, tileDrawInfo.tileSpriteEffect);
							}
						}
					}
				}
				if (texture2D != null)
				{
					empty = new Rectangle((int)tileDrawInfo.tileFrameX + tileDrawInfo.addFrX, (int)tileDrawInfo.tileFrameY + tileDrawInfo.addFrY, tileDrawInfo.tileWidth, tileDrawInfo.tileHeight);
					int num25 = 0;
					int num26 = 0;
					Main.tileBatch.Draw(texture2D, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)tileDrawInfo.tileWidth - 16f) / 2f + (float)num25, (float)(tileY * 16 - (int)screenPosition.Y + tileDrawInfo.tileTop + num26)) + screenOffset, new Rectangle?(empty), transparent, TileDrawing._zero, 1f, tileDrawInfo.tileSpriteEffect);
				}
			}
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x005CBD60 File Offset: 0x005C9F60
		private bool IsVisible(Tile tile)
		{
			bool flag = tile.invisibleBlock();
			ushort type = tile.type;
			if (type != 19)
			{
				if (type == 541 || type == 631)
				{
					flag = true;
				}
			}
			else if (tile.frameY / 18 == 48)
			{
				flag = true;
			}
			return !flag || this._shouldShowInvisibleBlocks;
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x005CBDB0 File Offset: 0x005C9FB0
		public Texture2D GetTileDrawTexture(Tile tile, int tileX, int tileY)
		{
			TilePaintSystemV2.TileVariationkey key = new TilePaintSystemV2.TileVariationkey
			{
				TileType = (int)tile.type,
				TileStyle = 0,
				PaintColor = (int)tile.color()
			};
			ushort type = tile.type;
			if (type != 5)
			{
				if (type != 83)
				{
					if (type == 323)
					{
						key.TileStyle = this.GetPalmTreeBiome(tileX, tileY);
					}
				}
				else if (WorldGen.IsAlchemyPlantHarvestable((int)(tile.frameX / 18), tileY))
				{
					key.TileType = 84;
				}
			}
			else
			{
				key.TileStyle = TileDrawing.GetTreeBiome(tileX, tileY, (int)tile.frameX, (int)tile.frameY);
			}
			return this.GetTileDrawTexture(key);
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x005CBE54 File Offset: 0x005CA054
		public Texture2D GetTileDrawTexture(int tileType, int paintColor)
		{
			return this.GetTileDrawTexture(new TilePaintSystemV2.TileVariationkey
			{
				TileType = tileType,
				PaintColor = paintColor
			});
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x005CBE80 File Offset: 0x005CA080
		public Texture2D GetTileDrawTexture(TilePaintSystemV2.TileVariationkey key)
		{
			if (this._lastPaintLookupKey == key)
			{
				return this._lastPaintLookupTexture;
			}
			this._lastPaintLookupKey = key;
			this._lastPaintLookupTexture = this.LookupTileDrawTexture(key);
			return this._lastPaintLookupTexture;
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x005CBEB4 File Offset: 0x005CA0B4
		private Texture2D LookupTileDrawTexture(TilePaintSystemV2.TileVariationkey key)
		{
			Main.instance.LoadTiles(key.TileType);
			if (key.PaintColor != 0 || key.TileStyle != 0)
			{
				Texture2D texture2D = this._paintSystem.TryGetTileAndRequestIfNotReady(key.TileType, key.TileStyle, key.PaintColor);
				if (texture2D != null)
				{
					return texture2D;
				}
			}
			return TextureAssets.Tile[key.TileType].Value;
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x005CBF18 File Offset: 0x005CA118
		private Texture2D LookupCageTopDrawTexture(TilePaintSystemV2.CageTopVariationkey key)
		{
			if (key.PaintColor != 0)
			{
				Texture2D texture2D = this._paintSystem.TryGetCageTopAndRequestIfNotReady(key.CageStyle, key.PaintColor);
				if (texture2D != null)
				{
					return texture2D;
				}
			}
			return TextureAssets.CageTop[key.CageStyle].Value;
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x005CBF5C File Offset: 0x005CA15C
		private void DrawBasicTile(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData, Rectangle normalTileRect, Vector2 normalTilePosition)
		{
			bool flag = !TileID.Sets.DontDrawTileSliced[(int)drawData.tileCache.type];
			bool flag2 = !TileID.Sets.DontDrawTileSlopes[(int)drawData.tileCache.type];
			if (drawData.typeCache == 380 || TileID.Sets.Platforms[(int)drawData.typeCache])
			{
				this.DrawTile_BackRope(screenPosition, screenOffset, tileX, tileY, drawData);
			}
			if (flag2 && drawData.tileCache.slope() > 0)
			{
				if (TileID.Sets.Platforms[(int)drawData.tileCache.type])
				{
					Tile tile = Main.tile[tileX, tileY + 1];
					Tile tile2 = Main.tile[tileX - 1, tileY + 1];
					Tile tile3 = Main.tile[tileX + 1, tileY + 1];
					bool shouldShowInvisibleBlocks = this._shouldShowInvisibleBlocks;
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle?(normalTileRect), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					if (drawData.tileCache.slope() == 1 && tile3.active() && (shouldShowInvisibleBlocks || !tile3.invisibleBlock()) && Main.tileSolid[(int)tile3.type] && tile3.slope() != 2 && !tile3.halfBrick() && (!tile.active() || (!shouldShowInvisibleBlocks && tile.invisibleBlock()) || (tile.blockType() != 0 && tile.blockType() != 5) || !TileID.Sets.BlocksStairs[(int)tile.type]))
					{
						Main.tileBatch.SetLayer(TileDrawing.Layer_BehindTiles, 0);
						Rectangle value = new Rectangle(198, (int)drawData.tileFrameY, 16, 16);
						if (TileID.Sets.Platforms[(int)tile3.type] && tile3.slope() == 0)
						{
							value.X = 324;
						}
						Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 16f), new Rectangle?(value), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						return;
					}
					if (drawData.tileCache.slope() == 2 && tile2.active() && (shouldShowInvisibleBlocks || !tile2.invisibleBlock()) && Main.tileSolid[(int)tile2.type] && tile2.slope() != 1 && !tile2.halfBrick() && (!tile.active() || (!shouldShowInvisibleBlocks && tile.invisibleBlock()) || (tile.blockType() != 0 && tile.blockType() != 4) || !TileID.Sets.BlocksStairs[(int)tile.type]))
					{
						Main.tileBatch.SetLayer(TileDrawing.Layer_BehindTiles, 0);
						Rectangle value2 = new Rectangle(162, (int)drawData.tileFrameY, 16, 16);
						if (TileID.Sets.Platforms[(int)tile2.type] && tile2.slope() == 0)
						{
							value2.X = 306;
						}
						Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 16f), new Rectangle?(value2), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						return;
					}
				}
				else
				{
					if (TileID.Sets.HasSlopeFrames[(int)drawData.tileCache.type])
					{
						Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, 16, 16)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						return;
					}
					int num = (int)drawData.tileCache.slope();
					int num2 = 2;
					for (int i = 0; i < 8; i++)
					{
						int num3 = i * -2;
						int num4 = 16 - i * 2;
						int num5 = 16 - num4;
						int num6;
						switch (num)
						{
						case 1:
							num3 = 0;
							num6 = i * 2;
							num4 = 14 - i * 2;
							num5 = 0;
							break;
						case 2:
							num3 = 0;
							num6 = 16 - i * 2 - 2;
							num4 = 14 - i * 2;
							num5 = 0;
							break;
						case 3:
							num6 = i * 2;
							break;
						default:
							num6 = 16 - i * 2 - 2;
							break;
						}
						Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2((float)num6, (float)(i * num2 + num3)), new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX + num6, (int)drawData.tileFrameY + drawData.addFrY + num5, num2, num4)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
					int num7 = (num > 2) ? 0 : 14;
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, (float)num7), new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY + num7, 16, 2)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					return;
				}
			}
			else if (flag2 && !TileID.Sets.Platforms[(int)drawData.typeCache] && !TileID.Sets.IgnoresNearbyHalfbricksWhenDrawn[(int)drawData.typeCache] && this._tileSolid[(int)drawData.typeCache] && !TileID.Sets.NotReallySolid[(int)drawData.typeCache] && !drawData.tileCache.halfBrick() && (Main.tile[tileX - 1, tileY].halfBrick() || Main.tile[tileX + 1, tileY].halfBrick()))
			{
				if (Main.tile[tileX - 1, tileY].halfBrick() && Main.tile[tileX + 1, tileY].halfBrick())
				{
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 8f), new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, drawData.addFrY + (int)drawData.tileFrameY + 8, drawData.tileWidth, 8)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					Rectangle value3 = new Rectangle(126 + drawData.addFrX, drawData.addFrY, 16, 8);
					if (Main.tile[tileX, tileY - 1].active() && !Main.tile[tileX, tileY - 1].bottomSlope() && Main.tile[tileX, tileY - 1].type == drawData.typeCache)
					{
						value3 = new Rectangle(90 + drawData.addFrX, drawData.addFrY, 16, 8);
					}
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle?(value3), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					return;
				}
				if (Main.tile[tileX - 1, tileY].halfBrick())
				{
					int num8 = 4;
					if (TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[(int)drawData.typeCache])
					{
						num8 = 2;
					}
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 8f), new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, drawData.addFrY + (int)drawData.tileFrameY + 8, drawData.tileWidth, 8)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2((float)num8, 0f), new Rectangle?(new Rectangle((int)drawData.tileFrameX + num8 + drawData.addFrX, drawData.addFrY + (int)drawData.tileFrameY, drawData.tileWidth - num8, drawData.tileHeight)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle?(new Rectangle(144 + drawData.addFrX, drawData.addFrY, num8, 8)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					if (num8 == 2)
					{
						Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle?(new Rectangle(148 + drawData.addFrX, drawData.addFrY, 2, 2)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						return;
					}
				}
				else if (Main.tile[tileX + 1, tileY].halfBrick())
				{
					int num9 = 4;
					if (TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[(int)drawData.typeCache])
					{
						num9 = 2;
					}
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 8f), new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, drawData.addFrY + (int)drawData.tileFrameY + 8, drawData.tileWidth, 8)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, drawData.addFrY + (int)drawData.tileFrameY, drawData.tileWidth - num9, drawData.tileHeight)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2((float)(16 - num9), 0f), new Rectangle?(new Rectangle(144 + (16 - num9), 0, num9, 8)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					if (num9 == 2)
					{
						Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(14f, 0f), new Rectangle?(new Rectangle(156, 0, 2, 2)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						return;
					}
				}
			}
			else
			{
				if (flag && Lighting.NotRetro && this._tileSolid[(int)drawData.typeCache] && !drawData.tileCache.halfBrick())
				{
					this.DrawSingleTile_SlicedBlock(normalTilePosition, tileX, tileY, drawData);
					return;
				}
				if (drawData.halfBrickHeight == 8 && (!Main.tile[tileX, tileY + 1].active() || !this._tileSolid[(int)Main.tile[tileX, tileY + 1].type] || Main.tile[tileX, tileY + 1].halfBrick()))
				{
					if (TileID.Sets.Platforms[(int)drawData.typeCache])
					{
						Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle?(normalTileRect), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
					else
					{
						Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle?(normalTileRect.Modified(0, 0, 0, -4)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition + new Vector2(0f, 4f), new Rectangle?(new Rectangle(144 + drawData.addFrX, 66 + drawData.addFrY, drawData.tileWidth, 4)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
				}
				else if (TileID.Sets.CritterCageLidStyle[(int)drawData.typeCache] >= 0)
				{
					int num10 = TileID.Sets.CritterCageLidStyle[(int)drawData.typeCache];
					if ((num10 < 3 && normalTileRect.Y % 54 == 0) || (num10 >= 3 && normalTileRect.Y % 36 == 0))
					{
						Vector2 position = normalTilePosition;
						position.Y += 8f;
						Rectangle value4 = normalTileRect;
						value4.Y += 8;
						value4.Height -= 8;
						Main.tileBatch.Draw(drawData.drawTexture, position, new Rectangle?(value4), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						position = normalTilePosition;
						position.Y -= 2f;
						value4 = normalTileRect;
						if (num10 == 0)
						{
							value4.X = normalTileRect.X % 108;
						}
						value4.Y = 0;
						value4.Height = 10;
						Texture2D texture = this.LookupCageTopDrawTexture(new TilePaintSystemV2.CageTopVariationkey
						{
							CageStyle = num10,
							PaintColor = (int)drawData.tileCache.color()
						});
						Main.tileBatch.Draw(texture, position, new Rectangle?(value4), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
					else
					{
						Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle?(normalTileRect), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
				}
				else if (drawData.typeCache == 711)
				{
					Rectangle rectangle = new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight);
					if (normalTileRect.X == 0 && normalTileRect.Y == 0)
					{
						Rectangle rectangle2 = rectangle;
						rectangle2.X += 38;
						for (float num11 = 0f; num11 < 1f; num11 += 0.33333334f)
						{
							float num12 = Main.GlobalTimeWrappedHourly % 2f / 2f;
							Color color = Main.hslToRgb((num12 + num11) % 1f, 1f, 0.5f, byte.MaxValue);
							color.A = 0;
							color *= 0.3f;
							for (int j = 0; j < 2; j++)
							{
								if (j == 1)
								{
									rectangle2.Width = rectangle.Width + 2;
								}
								else
								{
									rectangle2.Width = rectangle.Width;
								}
								for (int k = 0; k < 2; k++)
								{
									if (k == 1)
									{
										rectangle2.Height = rectangle.Height + 2;
									}
									else
									{
										rectangle2.Height = rectangle.Height;
									}
									Main.tileBatch.Draw(drawData.drawTexture, (normalTilePosition + new Vector2((float)(j * 16), (float)(k * 16)) + ((num12 + num11) * 6.2831855f).ToRotationVector2() * 4f).Floor(), new Rectangle?(new Rectangle(rectangle2.X + j * 18, rectangle2.Y + k * 18, rectangle2.Width, rectangle2.Height)), color, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
								}
							}
						}
					}
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle?(rectangle), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
				}
				else
				{
					Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle?(normalTileRect), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
				}
				this.DrawSingleTile_Flames(screenPosition, screenOffset, tileX, tileY, drawData);
			}
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x005CCF9C File Offset: 0x005CB19C
		private int GetPalmTreeBiome(int tileX, int tileY)
		{
			int num = tileY;
			while (Main.tile[tileX, num].active() && Main.tile[tileX, num].type == 323)
			{
				num++;
			}
			return this.GetPalmTreeVariant(tileX, num);
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x005CCFE8 File Offset: 0x005CB1E8
		private static int GetTreeBiome(int tileX, int tileY, int tileFrameX, int tileFrameY)
		{
			int num = tileX;
			int num2 = tileY;
			int type = (int)Main.tile[num, num2].type;
			if (tileFrameX == 66 && tileFrameY <= 45)
			{
				num++;
			}
			if (tileFrameX == 88 && tileFrameY >= 66 && tileFrameY <= 110)
			{
				num--;
			}
			if (tileFrameY >= 198)
			{
				if (tileFrameX == 66)
				{
					num--;
				}
				else if (tileFrameX == 44)
				{
					num++;
				}
			}
			else if (tileFrameY >= 132)
			{
				if (tileFrameX == 22)
				{
					num--;
				}
				else if (tileFrameX == 44)
				{
					num++;
				}
			}
			while (Main.tile[num, num2].active() && (int)Main.tile[num, num2].type == type)
			{
				num2++;
			}
			return TileDrawing.GetTreeVariant(num, num2);
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x005CD09C File Offset: 0x005CB29C
		public static int GetTreeVariant(int x, int y)
		{
			if (Main.tile[x, y] == null || !Main.tile[x, y].active())
			{
				return -1;
			}
			int type = (int)Main.tile[x, y].type;
			if (type > 109)
			{
				if (type <= 199)
				{
					if (type == 147)
					{
						return 3;
					}
					if (type != 199)
					{
						return -1;
					}
				}
				else
				{
					if (type == 492)
					{
						return 2;
					}
					if (type == 661)
					{
						return 0;
					}
					if (type != 662)
					{
						return -1;
					}
				}
				return 4;
			}
			if (type <= 60)
			{
				if (type != 23)
				{
					if (type != 60)
					{
						return -1;
					}
					if ((double)y <= Main.worldSurface)
					{
						return 1;
					}
					return 5;
				}
			}
			else
			{
				if (type == 70)
				{
					return 6;
				}
				if (type != 109)
				{
					return -1;
				}
				return 2;
			}
			return 0;
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x005CD14C File Offset: 0x005CB34C
		private Color GetFallenStarFurnitureFlameColor()
		{
			float num = Utils.WrappedLerp(0.5f, 1f, Main.GlobalTimeWrappedHourly % 2f / 2f);
			int num2 = (int)(150f * num);
			return new Color(150, num2, num2, 50);
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x005CD194 File Offset: 0x005CB394
		private Color GetHallowedFurnitureFlameColor()
		{
			float num = Utils.WrappedLerp(0.5f, 1f, Main.GlobalTimeWrappedHourly % 2f / 2f);
			int num2 = (int)(170f * num);
			return new Color(170, num2, num2, 75);
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x005CD1DA File Offset: 0x005CB3DA
		private Color GetCloudFurnitureFlameColor()
		{
			return this.GetWrappedFurnitureFlameColor(new Color(255, 255, 255, 0), 0.75f, 1f);
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x005CD201 File Offset: 0x005CB401
		private Color GetLibrarianFurnitureFlameColor()
		{
			return this.GetWrappedFurnitureFlameColor(new Color(255, 255, 255, 0), 0.25f, 1f);
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x005CD201 File Offset: 0x005CB401
		private Color GetForbiddenFurnitureFlameColor()
		{
			return this.GetWrappedFurnitureFlameColor(new Color(255, 255, 255, 0), 0.25f, 1f);
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x005CD201 File Offset: 0x005CB401
		private Color GetBoulderFurnitureFlameColor()
		{
			return this.GetWrappedFurnitureFlameColor(new Color(255, 255, 255, 0), 0.25f, 1f);
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x005CD228 File Offset: 0x005CB428
		private Color GetWrappedFurnitureFlameColor(Color baseColor, float min = 0.75f, float max = 1f)
		{
			float scale = Utils.WrappedLerp(min, max, Main.GlobalTimeWrappedHourly % 2f / 2f);
			return baseColor * scale;
		}

		// Token: 0x06003166 RID: 12646 RVA: 0x005CD258 File Offset: 0x005CB458
		private TileDrawing.TileFlameData GetTileFlameData(int tileX, int tileY, int type, int tileFrameY)
		{
			if (type == 270)
			{
				return new TileDrawing.TileFlameData
				{
					flameTexture = TextureAssets.FireflyJar.Value,
					flameColor = new Color(200, 200, 200, 0),
					flameCount = 1
				};
			}
			if (type == 271)
			{
				return new TileDrawing.TileFlameData
				{
					flameTexture = TextureAssets.LightningbugJar.Value,
					flameColor = new Color(200, 200, 200, 0),
					flameCount = 1
				};
			}
			if (type == 581)
			{
				return new TileDrawing.TileFlameData
				{
					flameTexture = TextureAssets.GlowMask[291].Value,
					flameColor = new Color(200, 100, 100, 0),
					flameCount = 1
				};
			}
			if (!Main.tileFlame[type])
			{
				return default(TileDrawing.TileFlameData);
			}
			ulong flameSeed = Main.TileFrameSeed ^ (ulong)((long)tileX << 32 | (long)((ulong)tileY));
			int num = 0;
			if (type == 4)
			{
				num = 0;
			}
			else if (type == 33 || type == 174)
			{
				num = 1;
			}
			else if (type == 100 || type == 173)
			{
				num = 2;
			}
			else if (type == 34)
			{
				num = 3;
			}
			else if (type == 93)
			{
				num = 4;
			}
			else if (type == 49)
			{
				num = 5;
			}
			else if (type == 372)
			{
				num = 16;
			}
			else if (type == 646)
			{
				num = 17;
			}
			else if (type == 98)
			{
				num = 6;
			}
			else if (type == 35)
			{
				num = 7;
			}
			else if (type == 42)
			{
				num = 13;
			}
			TileDrawing.TileFlameData result = new TileDrawing.TileFlameData
			{
				flameTexture = TextureAssets.Flames[num].Value,
				flameSeed = flameSeed
			};
			switch (num)
			{
			case 1:
			{
				int num2 = (int)(Main.tile[tileX, tileY].frameY / 22);
				switch (num2)
				{
				case 5:
				case 6:
				case 7:
				case 10:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					return result;
				case 8:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.3f;
					result.flameRangeMultY = 0.3f;
					return result;
				case 9:
				case 11:
				case 13:
				case 15:
					break;
				case 12:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.15f;
					return result;
				case 14:
					result.flameCount = 8;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.1f;
					return result;
				case 16:
					result.flameCount = 4;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.15f;
					return result;
				default:
					switch (num2)
					{
					case 27:
					case 28:
						result.flameCount = 1;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 43:
						result.flameCount = 1;
						result.flameColor = this.GetFallenStarFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 44:
						result.flameCount = 3;
						result.flameColor = new Color(200, 200, 200, 150);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.35f;
						return result;
					case 45:
						result.flameCount = 1;
						result.flameColor = this.GetHallowedFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 47:
					case 48:
					case 49:
					case 51:
					case 52:
					case 54:
						result.flameCount = 0;
						return result;
					case 56:
						result.flameCount = 1;
						result.flameColor = this.GetCloudFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 57:
					case 60:
						result.flameCount = 1;
						result.flameColor = new Color(200, 200, 200, 150);
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 58:
						result.flameCount = 1;
						result.flameColor = this.GetLibrarianFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 61:
						result.flameCount = 1;
						result.flameColor = this.GetForbiddenFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 63:
						result.flameCount = 1;
						result.flameColor = this.GetBoulderFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					}
					break;
				}
				result.flameCount = 7;
				result.flameColor = new Color(100, 100, 100, 0);
				result.flameRangeXMin = -10;
				result.flameRangeXMax = 11;
				result.flameRangeYMin = -10;
				result.flameRangeYMax = 1;
				result.flameRangeMultX = 0.15f;
				result.flameRangeMultY = 0.35f;
				return result;
			}
			case 2:
			{
				int num3 = (int)(Main.tile[tileX, tileY].frameY / 36);
				if (num3 <= 6)
				{
					if (num3 == 3)
					{
						result.flameCount = 3;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.05f;
						result.flameRangeMultY = 0.15f;
						return result;
					}
					if (num3 == 6)
					{
						result.flameCount = 5;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.15f;
						return result;
					}
				}
				else
				{
					switch (num3)
					{
					case 9:
						result.flameCount = 7;
						result.flameColor = new Color(100, 100, 100, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.3f;
						result.flameRangeMultY = 0.3f;
						return result;
					case 10:
					case 12:
						break;
					case 11:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.1f;
						result.flameRangeMultY = 0.15f;
						return result;
					case 13:
						result.flameCount = 8;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.1f;
						result.flameRangeMultY = 0.1f;
						return result;
					default:
						switch (num3)
						{
						case 28:
						case 29:
							result.flameCount = 1;
							result.flameColor = new Color(75, 75, 75, 0);
							result.flameRangeXMin = -10;
							result.flameRangeXMax = 11;
							result.flameRangeYMin = -10;
							result.flameRangeYMax = 1;
							result.flameRangeMultX = 0f;
							result.flameRangeMultY = 0f;
							return result;
						case 44:
							result.flameCount = 1;
							result.flameColor = this.GetFallenStarFurnitureFlameColor();
							result.flameRangeMultX = 0f;
							result.flameRangeMultY = 0f;
							return result;
						case 45:
							result.flameCount = 3;
							result.flameColor = new Color(200, 200, 200, 150);
							result.flameRangeXMin = -10;
							result.flameRangeXMax = 11;
							result.flameRangeYMin = -10;
							result.flameRangeYMax = 1;
							result.flameRangeMultX = 0.15f;
							result.flameRangeMultY = 0.35f;
							return result;
						case 46:
							result.flameCount = 1;
							result.flameColor = this.GetHallowedFurnitureFlameColor();
							result.flameRangeMultX = 0f;
							result.flameRangeMultY = 0f;
							return result;
						case 48:
						case 49:
						case 50:
						case 52:
						case 53:
						case 55:
							result.flameCount = 0;
							return result;
						case 57:
							result.flameCount = 1;
							result.flameColor = this.GetCloudFurnitureFlameColor();
							result.flameRangeMultX = 0f;
							result.flameRangeMultY = 0f;
							return result;
						case 58:
						case 61:
							result.flameCount = 1;
							result.flameColor = new Color(200, 200, 200, 150);
							result.flameRangeMultX = 0f;
							result.flameRangeMultY = 0f;
							return result;
						case 59:
							result.flameCount = 1;
							result.flameColor = this.GetLibrarianFurnitureFlameColor();
							result.flameRangeMultX = 0f;
							result.flameRangeMultY = 0f;
							return result;
						case 62:
							result.flameCount = 1;
							result.flameColor = this.GetForbiddenFurnitureFlameColor();
							result.flameRangeMultX = 0f;
							result.flameRangeMultY = 0f;
							return result;
						case 64:
							result.flameCount = 1;
							result.flameColor = this.GetBoulderFurnitureFlameColor();
							result.flameRangeMultX = 0f;
							result.flameRangeMultY = 0f;
							return result;
						}
						break;
					}
				}
				result.flameCount = 7;
				result.flameColor = new Color(100, 100, 100, 0);
				result.flameRangeXMin = -10;
				result.flameRangeXMax = 11;
				result.flameRangeYMin = -10;
				result.flameRangeYMax = 1;
				result.flameRangeMultX = 0.15f;
				result.flameRangeMultY = 0.35f;
				return result;
			}
			case 3:
			{
				int num4 = (int)(Main.tile[tileX, tileY].frameY / 54);
				if (Main.tile[tileX, tileY].frameX >= 108)
				{
					num4 += (int)(37 * (Main.tile[tileX, tileY].frameX / 108));
				}
				switch (num4)
				{
				case 8:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					return result;
				case 9:
					result.flameCount = 3;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -1;
					result.flameRangeXMax = 1;
					result.flameRangeYMin = -1;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 2f;
					result.flameRangeMultY = 2f;
					return result;
				case 10:
				case 12:
				case 13:
				case 14:
				case 16:
				case 19:
					break;
				case 11:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.3f;
					result.flameRangeMultY = 0.3f;
					return result;
				case 15:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.15f;
					return result;
				case 17:
				case 20:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					return result;
				case 18:
					result.flameCount = 8;
					result.flameColor = new Color(75, 75, 75, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.1f;
					return result;
				default:
					switch (num4)
					{
					case 34:
					case 35:
						result.flameCount = 1;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 50:
						result.flameCount = 1;
						result.flameColor = this.GetFallenStarFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 51:
						result.flameCount = 3;
						result.flameColor = new Color(200, 200, 200, 150);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.35f;
						return result;
					case 52:
						result.flameCount = 1;
						result.flameColor = this.GetHallowedFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 54:
					case 55:
					case 56:
					case 58:
					case 59:
					case 61:
						result.flameCount = 0;
						return result;
					case 63:
						result.flameCount = 1;
						result.flameColor = this.GetCloudFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 64:
					case 67:
						result.flameCount = 1;
						result.flameColor = new Color(200, 200, 200, 150);
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 65:
						result.flameCount = 1;
						result.flameColor = this.GetLibrarianFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 68:
						result.flameCount = 1;
						result.flameColor = this.GetForbiddenFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 70:
						result.flameCount = 1;
						result.flameColor = this.GetBoulderFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					}
					break;
				}
				result.flameCount = 7;
				result.flameColor = new Color(100, 100, 100, 0);
				result.flameRangeXMin = -10;
				result.flameRangeXMax = 11;
				result.flameRangeYMin = -10;
				result.flameRangeYMax = 1;
				result.flameRangeMultX = 0.15f;
				result.flameRangeMultY = 0.35f;
				return result;
			}
			case 4:
			{
				int num5 = (int)(Main.tile[tileX, tileY].frameY / 54);
				switch (num5)
				{
				case 1:
					result.flameCount = 3;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.15f;
					result.flameRangeMultY = 0.15f;
					return result;
				case 2:
				case 4:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.075f;
					result.flameRangeMultY = 0.075f;
					return result;
				case 3:
					result.flameCount = 7;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -20;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.2f;
					result.flameRangeMultY = 0.35f;
					return result;
				case 5:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.3f;
					result.flameRangeMultY = 0.3f;
					return result;
				case 6:
				case 7:
				case 8:
				case 10:
				case 11:
					break;
				case 9:
					result.flameCount = 7;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 1;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.15f;
					return result;
				case 12:
					result.flameCount = 1;
					result.flameColor = new Color(100, 100, 100, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.01f;
					result.flameRangeMultY = 0.01f;
					return result;
				case 13:
					result.flameCount = 8;
					result.flameColor = new Color(50, 50, 50, 0);
					result.flameRangeXMin = -10;
					result.flameRangeXMax = 11;
					result.flameRangeYMin = -10;
					result.flameRangeYMax = 11;
					result.flameRangeMultX = 0.1f;
					result.flameRangeMultY = 0.1f;
					return result;
				default:
					switch (num5)
					{
					case 28:
					case 29:
						result.flameCount = 1;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 44:
						result.flameCount = 1;
						result.flameColor = this.GetFallenStarFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 45:
						result.flameCount = 3;
						result.flameColor = new Color(200, 200, 200, 150);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.35f;
						return result;
					case 46:
						result.flameCount = 1;
						result.flameColor = this.GetHallowedFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 48:
					case 49:
					case 50:
					case 52:
					case 53:
					case 55:
						result.flameCount = 0;
						return result;
					case 57:
						result.flameCount = 1;
						result.flameColor = this.GetCloudFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 58:
					case 61:
						result.flameCount = 1;
						result.flameColor = new Color(200, 200, 200, 150);
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 59:
						result.flameCount = 1;
						result.flameColor = this.GetLibrarianFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 62:
						result.flameCount = 1;
						result.flameColor = this.GetForbiddenFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 64:
						result.flameCount = 1;
						result.flameColor = this.GetBoulderFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					}
					break;
				}
				result.flameCount = 7;
				result.flameColor = new Color(100, 100, 100, 0);
				result.flameRangeXMin = -10;
				result.flameRangeXMax = 11;
				result.flameRangeYMin = -10;
				result.flameRangeYMax = 1;
				result.flameRangeMultX = 0.15f;
				result.flameRangeMultY = 0.35f;
				return result;
			}
			case 5:
			case 6:
				break;
			case 7:
				result.flameCount = 4;
				result.flameColor = new Color(50, 50, 50, 0);
				result.flameRangeXMin = -10;
				result.flameRangeXMax = 11;
				result.flameRangeYMin = -10;
				result.flameRangeYMax = 10;
				result.flameRangeMultX = 0f;
				result.flameRangeMultY = 0f;
				return result;
			default:
				if (num == 13)
				{
					switch (tileFrameY / 36)
					{
					case 1:
					case 3:
					case 6:
					case 8:
					case 19:
					case 27:
					case 29:
					case 30:
					case 31:
					case 32:
					case 36:
					case 39:
					case 53:
					case 57:
					case 60:
					case 62:
					case 66:
					case 69:
						result.flameCount = 7;
						result.flameColor = new Color(100, 100, 100, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.35f;
						return result;
					case 2:
					case 16:
					case 25:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.1f;
						return result;
					case 11:
						result.flameCount = 7;
						result.flameColor = new Color(50, 50, 50, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 11;
						result.flameRangeMultX = 0.075f;
						result.flameRangeMultY = 0.075f;
						return result;
					case 34:
					case 35:
						result.flameCount = 1;
						result.flameColor = new Color(75, 75, 75, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 44:
						result.flameCount = 7;
						result.flameColor = new Color(100, 100, 100, 0);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.35f;
						return result;
					case 50:
						result.flameCount = 1;
						result.flameColor = this.GetFallenStarFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 51:
						result.flameCount = 3;
						result.flameColor = new Color(200, 200, 200, 150);
						result.flameRangeXMin = -10;
						result.flameRangeXMax = 11;
						result.flameRangeYMin = -10;
						result.flameRangeYMax = 1;
						result.flameRangeMultX = 0.15f;
						result.flameRangeMultY = 0.35f;
						return result;
					case 52:
						result.flameCount = 1;
						result.flameColor = this.GetHallowedFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 54:
					case 55:
					case 56:
					case 58:
					case 59:
					case 61:
						result.flameCount = 0;
						return result;
					case 63:
						result.flameCount = 1;
						result.flameColor = this.GetCloudFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 64:
					case 67:
						result.flameCount = 1;
						result.flameColor = new Color(200, 200, 200, 150);
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 65:
						result.flameCount = 1;
						result.flameColor = this.GetLibrarianFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 68:
						result.flameCount = 1;
						result.flameColor = this.GetForbiddenFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					case 70:
						result.flameCount = 1;
						result.flameColor = this.GetBoulderFurnitureFlameColor();
						result.flameRangeMultX = 0f;
						result.flameRangeMultY = 0f;
						return result;
					}
					result.flameCount = 0;
					return result;
				}
				break;
			}
			result.flameCount = 7;
			result.flameColor = new Color(100, 100, 100, 0);
			if (tileFrameY / 22 == 14)
			{
				result.flameColor = new Color((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
			}
			result.flameRangeXMin = -10;
			result.flameRangeXMax = 11;
			result.flameRangeYMin = -10;
			result.flameRangeYMax = 1;
			result.flameRangeMultX = 0.15f;
			result.flameRangeMultY = 0.35f;
			return result;
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x005CF0BC File Offset: 0x005CD2BC
		private void DrawSingleTile_Flames(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
		{
			if (drawData.typeCache == 548 && drawData.tileFrameX / 54 > 6)
			{
				Main.tileBatch.Draw(TextureAssets.GlowMask[297].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), Color.White, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 613)
			{
				Main.tileBatch.Draw(TextureAssets.GlowMask[298].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), Color.White, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 614)
			{
				Main.tileBatch.Draw(TextureAssets.GlowMask[299].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), Color.White, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 593)
			{
				Main.tileBatch.Draw(TextureAssets.GlowMask[295].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), Color.White, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 594)
			{
				Main.tileBatch.Draw(TextureAssets.GlowMask[296].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), Color.White, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 215 && drawData.tileFrameY < 36)
			{
				int num = 15;
				Color color = new Color(255, 255, 255, 0);
				int num2 = (int)(drawData.tileFrameX / 54);
				if (num2 != 5)
				{
					if (num2 != 14)
					{
						if (num2 == 15)
						{
							color = new Color(255, 255, 255, 200);
						}
					}
					else
					{
						color = new Color(50, 50, 100, 20);
					}
				}
				else
				{
					color = new Color((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
				}
				Main.tileBatch.Draw(TextureAssets.Flames[num].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), color, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 85)
			{
				float graveyardVisualIntensity = Main.GraveyardVisualIntensity;
				if (graveyardVisualIntensity > 0f)
				{
					ulong num3 = Main.TileFrameSeed ^ (ulong)((long)tileX << 32 | (long)((ulong)tileY));
					TileDrawing.TileFlameData tileFlameData = this.GetTileFlameData(tileX, tileY, (int)drawData.typeCache, (int)drawData.tileFrameY);
					if (num3 == 0UL)
					{
						num3 = tileFlameData.flameSeed;
					}
					tileFlameData.flameSeed = num3;
					Vector2 vector = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset;
					Rectangle value = new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight);
					for (int i = 0; i < tileFlameData.flameCount; i++)
					{
						Color color2 = tileFlameData.flameColor * graveyardVisualIntensity;
						float x = (float)Utils.RandomInt(ref tileFlameData.flameSeed, tileFlameData.flameRangeXMin, tileFlameData.flameRangeXMax) * tileFlameData.flameRangeMultX;
						float y = (float)Utils.RandomInt(ref tileFlameData.flameSeed, tileFlameData.flameRangeYMin, tileFlameData.flameRangeYMax) * tileFlameData.flameRangeMultY;
						for (float num4 = 0f; num4 < 1f; num4 += 0.25f)
						{
							Main.tileBatch.Draw(tileFlameData.flameTexture, vector + new Vector2(x, y) + Vector2.UnitX.RotatedBy((double)(num4 * 6.2831855f), default(Vector2)) * 2f, new Rectangle?(value), color2, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
						Main.tileBatch.Draw(tileFlameData.flameTexture, vector, new Rectangle?(value), Color.White * graveyardVisualIntensity, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
				}
			}
			if (drawData.typeCache == 356 && Main.sundialCooldown == 0)
			{
				Texture2D value2 = TextureAssets.GlowMask[325].Value;
				Rectangle value3 = new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight);
				Color color3 = new Color(100, 100, 100, 0);
				int num5 = tileX - (int)(drawData.tileFrameX / 18);
				int num6 = tileY - (int)(drawData.tileFrameY / 18);
				ulong num7 = Main.TileFrameSeed ^ (ulong)((long)num5 << 32 | (long)((ulong)num6));
				for (int j = 0; j < 7; j++)
				{
					float num8 = (float)Utils.RandomInt(ref num7, -10, 11) * 0.15f;
					float num9 = (float)Utils.RandomInt(ref num7, -10, 1) * 0.35f;
					Main.tileBatch.Draw(value2, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num8, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num9) + screenOffset, new Rectangle?(value3), color3, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
				}
			}
			if (drawData.typeCache == 663 && Main.moondialCooldown == 0)
			{
				Texture2D value4 = TextureAssets.GlowMask[335].Value;
				Rectangle value5 = new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight);
				value5.Y += 54 * Main.moonPhase;
				Main.tileBatch.Draw(value4, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(value5), Color.White * ((float)Main.mouseTextColor / 255f), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 286)
			{
				Main.tileBatch.Draw(TextureAssets.GlowSnail.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), new Color(75, 100, 255, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 582)
			{
				Main.tileBatch.Draw(TextureAssets.GlowMask[293].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), new Color(200, 100, 100, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 391)
			{
				Main.tileBatch.Draw(TextureAssets.GlowMask[131].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), new Color(250, 250, 250, 200), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 619)
			{
				Main.tileBatch.Draw(TextureAssets.GlowMask[300].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), new Color(75, 100, 255, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 270)
			{
				Main.tileBatch.Draw(TextureAssets.FireflyJar.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(200, 200, 200, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 271)
			{
				Main.tileBatch.Draw(TextureAssets.LightningbugJar.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(200, 200, 200, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 581)
			{
				Main.tileBatch.Draw(TextureAssets.GlowMask[291].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(200, 200, 200, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 316 || drawData.typeCache == 317 || drawData.typeCache == 318)
			{
				Main.tileBatch.Draw(TextureAssets.JellyfishBowl[(int)(drawData.typeCache - 316)].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), new Color(200, 200, 200, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 149 && drawData.tileFrameX < 54)
			{
				Main.tileBatch.Draw(TextureAssets.XmasLight.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(200, 200, 200, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 300 || drawData.typeCache == 302 || drawData.typeCache == 303 || drawData.typeCache == 306)
			{
				int num10 = 9;
				if (drawData.typeCache == 302)
				{
					num10 = 10;
				}
				if (drawData.typeCache == 303)
				{
					num10 = 11;
				}
				if (drawData.typeCache == 306)
				{
					num10 = 12;
				}
				Main.tileBatch.Draw(TextureAssets.Flames[num10].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), new Color(200, 200, 200, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			else if (Main.tileFlame[(int)drawData.typeCache])
			{
				ulong num11 = Main.TileFrameSeed ^ (ulong)((long)tileX << 32 | (long)((ulong)tileY));
				int typeCache = (int)drawData.typeCache;
				int num12 = 0;
				if (typeCache == 4)
				{
					num12 = 0;
				}
				else if (typeCache == 33 || typeCache == 174)
				{
					num12 = 1;
				}
				else if (typeCache == 100 || typeCache == 173)
				{
					num12 = 2;
				}
				else if (typeCache == 34)
				{
					num12 = 3;
				}
				else if (typeCache == 93)
				{
					num12 = 4;
				}
				else if (typeCache == 49)
				{
					num12 = 5;
				}
				else if (typeCache == 372)
				{
					num12 = 16;
				}
				else if (typeCache == 646)
				{
					num12 = 17;
				}
				else if (typeCache == 98)
				{
					num12 = 6;
				}
				else if (typeCache == 35)
				{
					num12 = 7;
				}
				else if (typeCache == 42)
				{
					num12 = 13;
				}
				if (num12 == 7)
				{
					for (int k = 0; k < 4; k++)
					{
						float num13 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
						float num14 = (float)Utils.RandomInt(ref num11, -10, 10) * 0.15f;
						num13 = 0f;
						num14 = 0f;
						Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num13, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num14) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
				}
				else if (num12 == 1)
				{
					int num15 = (int)(Main.tile[tileX, tileY].frameY / 22);
					bool flag = num15 >= 44;
					if (num15 == 5 || num15 == 6 || num15 == 7 || num15 == 10)
					{
						for (int l = 0; l < 7; l++)
						{
							float num16 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.075f;
							float num17 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.075f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num16, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num17) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num15 == 8)
					{
						for (int m = 0; m < 7; m++)
						{
							float num18 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.3f;
							float num19 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.3f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num18, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num19) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num15 == 12)
					{
						for (int n = 0; n < 7; n++)
						{
							float num20 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.1f;
							float num21 = (float)Utils.RandomInt(ref num11, -10, 1) * 0.15f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num20, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num21) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num15 == 14)
					{
						for (int num22 = 0; num22 < 8; num22++)
						{
							float num23 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.1f;
							float num24 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.1f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num23, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num24) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(75, 75, 75, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num15 == 16)
					{
						for (int num25 = 0; num25 < 4; num25++)
						{
							float num26 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							float num27 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num26, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num27) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(75, 75, 75, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num15 == 27 || num15 == 28)
					{
						Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(75, 75, 75, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
					else if (num15 == 43)
					{
						TileDrawing.TileFlameData tileFlameData2 = this.GetTileFlameData(tileX, tileY, (int)drawData.typeCache, (int)drawData.tileFrameY);
						if (num11 == 0UL)
						{
							num11 = tileFlameData2.flameSeed;
						}
						tileFlameData2.flameSeed = num11;
						for (int num28 = 0; num28 < tileFlameData2.flameCount; num28++)
						{
							float num29 = (float)Utils.RandomInt(ref tileFlameData2.flameSeed, tileFlameData2.flameRangeXMin, tileFlameData2.flameRangeXMax) * tileFlameData2.flameRangeMultX;
							float num30 = (float)Utils.RandomInt(ref tileFlameData2.flameSeed, tileFlameData2.flameRangeYMin, tileFlameData2.flameRangeYMax) * tileFlameData2.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData2.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num29, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num30) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), tileFlameData2.flameColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (flag)
					{
						TileDrawing.TileFlameData tileFlameData3 = this.GetTileFlameData(tileX, tileY, (int)drawData.typeCache, (int)drawData.tileFrameY);
						if (num11 == 0UL)
						{
							num11 = tileFlameData3.flameSeed;
						}
						tileFlameData3.flameSeed = num11;
						for (int num31 = 0; num31 < tileFlameData3.flameCount; num31++)
						{
							float num32 = (float)Utils.RandomInt(ref tileFlameData3.flameSeed, tileFlameData3.flameRangeXMin, tileFlameData3.flameRangeXMax) * tileFlameData3.flameRangeMultX;
							float num33 = (float)Utils.RandomInt(ref tileFlameData3.flameSeed, tileFlameData3.flameRangeYMin, tileFlameData3.flameRangeYMax) * tileFlameData3.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData3.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num32, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num33) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), tileFlameData3.flameColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else
					{
						for (int num34 = 0; num34 < 7; num34++)
						{
							float num35 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							float num36 = (float)Utils.RandomInt(ref num11, -10, 1) * 0.35f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num35, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num36) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(100, 100, 100, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
				}
				else if (num12 == 2)
				{
					int num37 = (int)(Main.tile[tileX, tileY].frameY / 36);
					bool flag2 = num37 >= 45;
					if (num37 == 3)
					{
						for (int num38 = 0; num38 < 3; num38++)
						{
							float num39 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.05f;
							float num40 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num39, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num40) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num37 == 6)
					{
						for (int num41 = 0; num41 < 5; num41++)
						{
							float num42 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							float num43 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num42, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num43) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(75, 75, 75, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num37 == 9)
					{
						for (int num44 = 0; num44 < 7; num44++)
						{
							float num45 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.3f;
							float num46 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.3f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num45, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num46) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(100, 100, 100, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num37 == 11)
					{
						for (int num47 = 0; num47 < 7; num47++)
						{
							float num48 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.1f;
							float num49 = (float)Utils.RandomInt(ref num11, -10, 1) * 0.15f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num48, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num49) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num37 == 13)
					{
						for (int num50 = 0; num50 < 8; num50++)
						{
							float num51 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.1f;
							float num52 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.1f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num51, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num52) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(75, 75, 75, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num37 == 28 || num37 == 29)
					{
						Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(75, 75, 75, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
					else if (num37 == 44)
					{
						TileDrawing.TileFlameData tileFlameData4 = this.GetTileFlameData(tileX, tileY, (int)drawData.typeCache, (int)drawData.tileFrameY);
						if (num11 == 0UL)
						{
							num11 = tileFlameData4.flameSeed;
						}
						tileFlameData4.flameSeed = num11;
						for (int num53 = 0; num53 < tileFlameData4.flameCount; num53++)
						{
							float num54 = (float)Utils.RandomInt(ref tileFlameData4.flameSeed, tileFlameData4.flameRangeXMin, tileFlameData4.flameRangeXMax) * tileFlameData4.flameRangeMultX;
							float num55 = (float)Utils.RandomInt(ref tileFlameData4.flameSeed, tileFlameData4.flameRangeYMin, tileFlameData4.flameRangeYMax) * tileFlameData4.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData4.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num54, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num55) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), tileFlameData4.flameColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (flag2)
					{
						TileDrawing.TileFlameData tileFlameData5 = this.GetTileFlameData(tileX, tileY, (int)drawData.typeCache, (int)drawData.tileFrameY);
						if (num11 == 0UL)
						{
							num11 = tileFlameData5.flameSeed;
						}
						tileFlameData5.flameSeed = num11;
						for (int num56 = 0; num56 < tileFlameData5.flameCount; num56++)
						{
							float num57 = (float)Utils.RandomInt(ref tileFlameData5.flameSeed, tileFlameData5.flameRangeXMin, tileFlameData5.flameRangeXMax) * tileFlameData5.flameRangeMultX;
							float num58 = (float)Utils.RandomInt(ref tileFlameData5.flameSeed, tileFlameData5.flameRangeYMin, tileFlameData5.flameRangeYMax) * tileFlameData5.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData5.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num57, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num58) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), tileFlameData5.flameColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else
					{
						for (int num59 = 0; num59 < 7; num59++)
						{
							float num60 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							float num61 = (float)Utils.RandomInt(ref num11, -10, 1) * 0.35f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num60, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num61) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), new Color(100, 100, 100, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
				}
				else if (num12 == 3)
				{
					int num62 = (int)(Main.tile[tileX, tileY].frameY / 54);
					if (Main.tile[tileX, tileY].frameX >= 108)
					{
						num62 += (int)(37 * (Main.tile[tileX, tileY].frameX / 108));
					}
					bool flag3 = num62 >= 51;
					if (num62 == 8)
					{
						for (int num63 = 0; num63 < 7; num63++)
						{
							float num64 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.075f;
							float num65 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.075f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num64, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num65) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num62 == 9)
					{
						for (int num66 = 0; num66 < 3; num66++)
						{
							float num67 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.05f;
							float num68 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num67, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num68) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num62 == 11)
					{
						for (int num69 = 0; num69 < 7; num69++)
						{
							float num70 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.3f;
							float num71 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.3f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num70, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num71) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num62 == 15)
					{
						for (int num72 = 0; num72 < 7; num72++)
						{
							float num73 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.1f;
							float num74 = (float)Utils.RandomInt(ref num11, -10, 1) * 0.15f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num73, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num74) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num62 == 17 || num62 == 20)
					{
						for (int num75 = 0; num75 < 7; num75++)
						{
							float num76 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.075f;
							float num77 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.075f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num76, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num77) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num62 == 18)
					{
						for (int num78 = 0; num78 < 8; num78++)
						{
							float num79 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.1f;
							float num80 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.1f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num79, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num80) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(75, 75, 75, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num62 == 34 || num62 == 35)
					{
						Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(75, 75, 75, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
					else if (num62 == 50)
					{
						TileDrawing.TileFlameData tileFlameData6 = this.GetTileFlameData(tileX, tileY, (int)drawData.typeCache, (int)drawData.tileFrameY);
						if (num11 == 0UL)
						{
							num11 = tileFlameData6.flameSeed;
						}
						tileFlameData6.flameSeed = num11;
						for (int num81 = 0; num81 < tileFlameData6.flameCount; num81++)
						{
							float num82 = (float)Utils.RandomInt(ref tileFlameData6.flameSeed, tileFlameData6.flameRangeXMin, tileFlameData6.flameRangeXMax) * tileFlameData6.flameRangeMultX;
							float num83 = (float)Utils.RandomInt(ref tileFlameData6.flameSeed, tileFlameData6.flameRangeYMin, tileFlameData6.flameRangeYMax) * tileFlameData6.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData6.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num82, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num83) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), tileFlameData6.flameColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (flag3)
					{
						TileDrawing.TileFlameData tileFlameData7 = this.GetTileFlameData(tileX, tileY, (int)drawData.typeCache, (int)drawData.tileFrameY);
						if (num11 == 0UL)
						{
							num11 = tileFlameData7.flameSeed;
						}
						tileFlameData7.flameSeed = num11;
						for (int num84 = 0; num84 < tileFlameData7.flameCount; num84++)
						{
							float num85 = (float)Utils.RandomInt(ref tileFlameData7.flameSeed, tileFlameData7.flameRangeXMin, tileFlameData7.flameRangeXMax) * tileFlameData7.flameRangeMultX;
							float num86 = (float)Utils.RandomInt(ref tileFlameData7.flameSeed, tileFlameData7.flameRangeYMin, tileFlameData7.flameRangeYMax) * tileFlameData7.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData7.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num85, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num86) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), tileFlameData7.flameColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else
					{
						for (int num87 = 0; num87 < 7; num87++)
						{
							float num88 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							float num89 = (float)Utils.RandomInt(ref num11, -10, 1) * 0.35f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num88, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num89) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), new Color(100, 100, 100, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
				}
				else if (num12 == 4)
				{
					int num90 = (int)(Main.tile[tileX, tileY].frameY / 54);
					bool flag4 = num90 >= 45;
					if (num90 == 1)
					{
						for (int num91 = 0; num91 < 3; num91++)
						{
							float num92 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							float num93 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num92, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num93) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num90 == 2 || num90 == 4)
					{
						for (int num94 = 0; num94 < 7; num94++)
						{
							float num95 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.075f;
							float num96 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.075f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num95, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num96) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num90 == 3)
					{
						for (int num97 = 0; num97 < 7; num97++)
						{
							float num98 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.2f;
							float num99 = (float)Utils.RandomInt(ref num11, -20, 1) * 0.35f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num98, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num99) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(100, 100, 100, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num90 == 5)
					{
						for (int num100 = 0; num100 < 7; num100++)
						{
							float num101 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.3f;
							float num102 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.3f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num101, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num102) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num90 == 9)
					{
						for (int num103 = 0; num103 < 7; num103++)
						{
							float num104 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.1f;
							float num105 = (float)Utils.RandomInt(ref num11, -10, 1) * 0.15f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num104, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num105) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num90 == 13)
					{
						for (int num106 = 0; num106 < 8; num106++)
						{
							float num107 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.1f;
							float num108 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.1f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num107, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num108) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(75, 75, 75, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num90 == 12)
					{
						float num109 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.01f;
						float num110 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.01f;
						Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num109, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num110) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(Utils.RandomInt(ref num11, 90, 111), Utils.RandomInt(ref num11, 90, 111), Utils.RandomInt(ref num11, 90, 111), 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
					else if (num90 == 28 || num90 == 29)
					{
						Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(75, 75, 75, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
					else if (num90 == 44)
					{
						TileDrawing.TileFlameData tileFlameData8 = this.GetTileFlameData(tileX, tileY, (int)drawData.typeCache, (int)drawData.tileFrameY);
						if (num11 == 0UL)
						{
							num11 = tileFlameData8.flameSeed;
						}
						tileFlameData8.flameSeed = num11;
						for (int num111 = 0; num111 < tileFlameData8.flameCount; num111++)
						{
							float num112 = (float)Utils.RandomInt(ref tileFlameData8.flameSeed, tileFlameData8.flameRangeXMin, tileFlameData8.flameRangeXMax) * tileFlameData8.flameRangeMultX;
							float num113 = (float)Utils.RandomInt(ref tileFlameData8.flameSeed, tileFlameData8.flameRangeYMin, tileFlameData8.flameRangeYMax) * tileFlameData8.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData8.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num112, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num113) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), tileFlameData8.flameColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (flag4)
					{
						TileDrawing.TileFlameData tileFlameData9 = this.GetTileFlameData(tileX, tileY, (int)drawData.typeCache, (int)drawData.tileFrameY);
						if (num11 == 0UL)
						{
							num11 = tileFlameData9.flameSeed;
						}
						tileFlameData9.flameSeed = num11;
						for (int num114 = 0; num114 < tileFlameData9.flameCount; num114++)
						{
							float num115 = (float)Utils.RandomInt(ref tileFlameData9.flameSeed, tileFlameData9.flameRangeXMin, tileFlameData9.flameRangeXMax) * tileFlameData9.flameRangeMultX;
							float num116 = (float)Utils.RandomInt(ref tileFlameData9.flameSeed, tileFlameData9.flameRangeYMin, tileFlameData9.flameRangeYMax) * tileFlameData9.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData9.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num115, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num116) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), tileFlameData9.flameColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else
					{
						for (int num117 = 0; num117 < 7; num117++)
						{
							float num118 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							float num119 = (float)Utils.RandomInt(ref num11, -10, 1) * 0.35f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num118, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num119) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), new Color(100, 100, 100, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
				}
				else if (num12 == 13)
				{
					int num120 = (int)(drawData.tileFrameY / 36);
					bool flag5 = num120 >= 51;
					if (num120 == 1 || num120 == 3 || num120 == 6 || num120 == 8 || num120 == 19 || num120 == 27 || num120 == 29 || num120 == 30 || num120 == 31 || num120 == 32 || num120 == 36 || num120 == 39)
					{
						for (int num121 = 0; num121 < 7; num121++)
						{
							float num122 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							float num123 = (float)Utils.RandomInt(ref num11, -10, 1) * 0.35f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num122, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num123) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(100, 100, 100, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num120 == 25 || num120 == 16 || num120 == 2)
					{
						for (int num124 = 0; num124 < 7; num124++)
						{
							float num125 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							float num126 = (float)Utils.RandomInt(ref num11, -10, 1) * 0.1f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num125, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num126) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(50, 50, 50, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num120 == 29)
					{
						for (int num127 = 0; num127 < 7; num127++)
						{
							float num128 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
							float num129 = (float)Utils.RandomInt(ref num11, -10, 1) * 0.15f;
							Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num128, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num129) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(25, 25, 25, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (num120 == 34 || num120 == 35)
					{
						Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(75, 75, 75, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
					else if (num120 == 50)
					{
						TileDrawing.TileFlameData tileFlameData10 = this.GetTileFlameData(tileX, tileY, (int)drawData.typeCache, (int)drawData.tileFrameY);
						if (num11 == 0UL)
						{
							num11 = tileFlameData10.flameSeed;
						}
						tileFlameData10.flameSeed = num11;
						for (int num130 = 0; num130 < tileFlameData10.flameCount; num130++)
						{
							float num131 = (float)Utils.RandomInt(ref tileFlameData10.flameSeed, tileFlameData10.flameRangeXMin, tileFlameData10.flameRangeXMax) * tileFlameData10.flameRangeMultX;
							float num132 = (float)Utils.RandomInt(ref tileFlameData10.flameSeed, tileFlameData10.flameRangeYMin, tileFlameData10.flameRangeYMax) * tileFlameData10.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData10.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num131, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num132) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), tileFlameData10.flameColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
					else if (flag5)
					{
						TileDrawing.TileFlameData tileFlameData11 = this.GetTileFlameData(tileX, tileY, (int)drawData.typeCache, (int)drawData.tileFrameY);
						if (num11 == 0UL)
						{
							num11 = tileFlameData11.flameSeed;
						}
						tileFlameData11.flameSeed = num11;
						for (int num133 = 0; num133 < tileFlameData11.flameCount; num133++)
						{
							float num134 = (float)Utils.RandomInt(ref tileFlameData11.flameSeed, tileFlameData11.flameRangeXMin, tileFlameData11.flameRangeXMax) * tileFlameData11.flameRangeMultX;
							float num135 = (float)Utils.RandomInt(ref tileFlameData11.flameSeed, tileFlameData11.flameRangeYMin, tileFlameData11.flameRangeYMax) * tileFlameData11.flameRangeMultY;
							Main.tileBatch.Draw(tileFlameData11.flameTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num134, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num135) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), tileFlameData11.flameColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
						}
					}
				}
				else
				{
					Color color4 = new Color(100, 100, 100, 0);
					if (drawData.tileCache.type == 4)
					{
						int num136 = (int)(drawData.tileCache.frameY / 22);
						if (num136 != 14)
						{
							if (num136 != 22)
							{
								if (num136 == 23)
								{
									color4 = new Color(255, 255, 255, 200);
								}
							}
							else
							{
								color4 = new Color(50, 50, 100, 20);
							}
						}
						else
						{
							color4 = new Color((float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f, 0f);
						}
					}
					if (drawData.tileCache.type == 646)
					{
						color4 = new Color(100, 100, 100, 150);
					}
					for (int num137 = 0; num137 < 7; num137++)
					{
						float num138 = (float)Utils.RandomInt(ref num11, -10, 11) * 0.15f;
						float num139 = (float)Utils.RandomInt(ref num11, -10, 1) * 0.35f;
						Main.tileBatch.Draw(TextureAssets.Flames[num12].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f + num138, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop) + num139) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), color4, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
					}
				}
			}
			if (drawData.typeCache == 144)
			{
				Main.tileBatch.Draw(TextureAssets.Timer.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color(200, 200, 200, 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 237)
			{
				Main.tileBatch.Draw(TextureAssets.SunAltar.Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset, new Rectangle?(new Rectangle((int)drawData.tileFrameX, (int)drawData.tileFrameY, drawData.tileWidth, drawData.tileHeight)), new Color((int)(Main.mouseTextColor / 2), (int)(Main.mouseTextColor / 2), (int)(Main.mouseTextColor / 2), 0), TileDrawing._zero, 1f, drawData.tileSpriteEffect);
			}
			if (drawData.typeCache == 658 && drawData.tileFrameX % 36 == 0 && drawData.tileFrameY % 54 == 0)
			{
				int num140 = (int)(drawData.tileFrameY / 54);
				if (num140 != 2)
				{
					Texture2D value6 = TextureAssets.GlowMask[334].Value;
					Vector2 value7 = new Vector2(0f, -10f);
					Vector2 position = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - (float)drawData.tileWidth / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop)) + screenOffset + value7;
					Rectangle value8 = value6.Frame(1, 1, 0, 0, 0, 0);
					Color color5 = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 0);
					if (num140 == 0)
					{
						color5 *= 0.75f;
					}
					Main.tileBatch.Draw(value6, position, new Rectangle?(value8), color5, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
				}
			}
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x005D36C0 File Offset: 0x005D18C0
		private int GetPalmTreeVariant(int x, int y)
		{
			int num = -1;
			if (Main.tile[x, y].active() && Main.tile[x, y].type == 53)
			{
				num = 0;
			}
			if (Main.tile[x, y].active() && Main.tile[x, y].type == 234)
			{
				num = 1;
			}
			if (Main.tile[x, y].active() && Main.tile[x, y].type == 116)
			{
				num = 2;
			}
			if (Main.tile[x, y].active() && Main.tile[x, y].type == 112)
			{
				num = 3;
			}
			if (WorldGen.IsPalmOasisTree(x))
			{
				num += 4;
			}
			return num;
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x005D3788 File Offset: 0x005D1988
		private void DrawSingleTile_SlicedBlock(Vector2 normalTilePosition, int tileX, int tileY, TileDrawInfo drawData)
		{
			Color color = default(Color);
			Vector2 origin = default(Vector2);
			Rectangle rectangle;
			Vector3 vector3;
			Vector2 position;
			if (drawData.tileLight.R > this._highQualityLightingRequirement.R || drawData.tileLight.G > this._highQualityLightingRequirement.G || drawData.tileLight.B > this._highQualityLightingRequirement.B)
			{
				Vector3[] array = drawData.colorSlices;
				Lighting.GetColor9Slice(tileX, tileY, ref array);
				Vector3 vector = drawData.tileLight.ToVector3();
				Vector3 vector2 = drawData.colorTint.ToVector3();
				if (drawData.tileCache.fullbrightBlock())
				{
					array = this._glowPaintColorSlices;
				}
				for (int i = 0; i < 9; i++)
				{
					rectangle.X = 0;
					rectangle.Y = 0;
					rectangle.Width = 4;
					rectangle.Height = 4;
					switch (i)
					{
					case 1:
						rectangle.Width = 8;
						rectangle.X = 4;
						break;
					case 2:
						rectangle.X = 12;
						break;
					case 3:
						rectangle.Height = 8;
						rectangle.Y = 4;
						break;
					case 4:
						rectangle.Width = 8;
						rectangle.Height = 8;
						rectangle.X = 4;
						rectangle.Y = 4;
						break;
					case 5:
						rectangle.X = 12;
						rectangle.Y = 4;
						rectangle.Height = 8;
						break;
					case 6:
						rectangle.Y = 12;
						break;
					case 7:
						rectangle.Width = 8;
						rectangle.Height = 4;
						rectangle.X = 4;
						rectangle.Y = 12;
						break;
					case 8:
						rectangle.X = 12;
						rectangle.Y = 12;
						break;
					}
					vector3.X = (array[i].X + vector.X) * 0.5f;
					vector3.Y = (array[i].Y + vector.Y) * 0.5f;
					vector3.Z = (array[i].Z + vector.Z) * 0.5f;
					TileDrawing.GetFinalLight(drawData.tileCache, drawData.typeCache, ref vector3, ref vector2);
					position.X = normalTilePosition.X + (float)rectangle.X;
					position.Y = normalTilePosition.Y + (float)rectangle.Y;
					rectangle.X += (int)drawData.tileFrameX + drawData.addFrX;
					rectangle.Y += (int)drawData.tileFrameY + drawData.addFrY;
					int num = (int)(vector3.X * 255f);
					int num2 = (int)(vector3.Y * 255f);
					int num3 = (int)(vector3.Z * 255f);
					if (num > 255)
					{
						num = 255;
					}
					if (num2 > 255)
					{
						num2 = 255;
					}
					if (num3 > 255)
					{
						num3 = 255;
					}
					num3 <<= 16;
					num2 <<= 8;
					color.PackedValue = (uint)(num | num2 | num3 | -16777216);
					Main.tileBatch.Draw(drawData.drawTexture, position, new Rectangle?(rectangle), color, origin, 1f, drawData.tileSpriteEffect);
				}
				return;
			}
			if (drawData.tileLight.R > this._mediumQualityLightingRequirement.R || drawData.tileLight.G > this._mediumQualityLightingRequirement.G || drawData.tileLight.B > this._mediumQualityLightingRequirement.B)
			{
				Vector3[] array2 = drawData.colorSlices;
				Lighting.GetColor4Slice(tileX, tileY, ref array2);
				Vector3 vector4 = drawData.tileLight.ToVector3();
				Vector3 vector5 = drawData.colorTint.ToVector3();
				if (drawData.tileCache.fullbrightBlock())
				{
					array2 = this._glowPaintColorSlices;
				}
				rectangle.Width = 8;
				rectangle.Height = 8;
				for (int j = 0; j < 4; j++)
				{
					rectangle.X = 0;
					rectangle.Y = 0;
					switch (j)
					{
					case 1:
						rectangle.X = 8;
						break;
					case 2:
						rectangle.Y = 8;
						break;
					case 3:
						rectangle.X = 8;
						rectangle.Y = 8;
						break;
					}
					vector3.X = (array2[j].X + vector4.X) * 0.5f;
					vector3.Y = (array2[j].Y + vector4.Y) * 0.5f;
					vector3.Z = (array2[j].Z + vector4.Z) * 0.5f;
					TileDrawing.GetFinalLight(drawData.tileCache, drawData.typeCache, ref vector3, ref vector5);
					position.X = normalTilePosition.X + (float)rectangle.X;
					position.Y = normalTilePosition.Y + (float)rectangle.Y;
					rectangle.X += (int)drawData.tileFrameX + drawData.addFrX;
					rectangle.Y += (int)drawData.tileFrameY + drawData.addFrY;
					int num4 = (int)(vector3.X * 255f);
					int num5 = (int)(vector3.Y * 255f);
					int num6 = (int)(vector3.Z * 255f);
					if (num4 > 255)
					{
						num4 = 255;
					}
					if (num5 > 255)
					{
						num5 = 255;
					}
					if (num6 > 255)
					{
						num6 = 255;
					}
					num6 <<= 16;
					num5 <<= 8;
					color.PackedValue = (uint)(num4 | num5 | num6 | -16777216);
					Main.tileBatch.Draw(drawData.drawTexture, position, new Rectangle?(rectangle), color, origin, 1f, drawData.tileSpriteEffect);
				}
				return;
			}
			Main.tileBatch.Draw(drawData.drawTexture, normalTilePosition, new Rectangle?(new Rectangle((int)drawData.tileFrameX + drawData.addFrX, (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight)), drawData.finalColor, TileDrawing._zero, 1f, drawData.tileSpriteEffect);
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x005D3DD4 File Offset: 0x005D1FD4
		private void DrawXmasTree(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
		{
			int num = 2;
			if (tileY - (int)drawData.tileFrameY > 0 && drawData.tileFrameY == 7 && Main.tile[tileX, tileY - (int)drawData.tileFrameY] != null)
			{
				drawData.tileTop -= (int)(16 * drawData.tileFrameY);
				drawData.tileFrameX = Main.tile[tileX, tileY - (int)drawData.tileFrameY].frameX;
				drawData.tileFrameY = Main.tile[tileX, tileY - (int)drawData.tileFrameY].frameY;
			}
			if (drawData.tileFrameX >= 10)
			{
				int num2 = 0;
				if ((drawData.tileFrameY & 1) == 1)
				{
					num2++;
				}
				if ((drawData.tileFrameY & 2) == 2)
				{
					num2 += 2;
				}
				if ((drawData.tileFrameY & 4) == 4)
				{
					num2 += 4;
				}
				int num3 = 0;
				if ((drawData.tileFrameY & 8) == 8)
				{
					num3++;
				}
				if ((drawData.tileFrameY & 16) == 16)
				{
					num3 += 2;
				}
				if ((drawData.tileFrameY & 32) == 32)
				{
					num3 += 4;
				}
				int num4 = 0;
				if ((drawData.tileFrameY & 64) == 64)
				{
					num4++;
				}
				if ((drawData.tileFrameY & 128) == 128)
				{
					num4 += 2;
				}
				if ((drawData.tileFrameY & 256) == 256)
				{
					num4 += 4;
				}
				if ((drawData.tileFrameY & 512) == 512)
				{
					num4 += 8;
				}
				int num5 = 0;
				if ((drawData.tileFrameY & 1024) == 1024)
				{
					num5++;
				}
				if ((drawData.tileFrameY & 2048) == 2048)
				{
					num5 += 2;
				}
				if ((drawData.tileFrameY & 4096) == 4096)
				{
					num5 += 4;
				}
				if ((drawData.tileFrameY & 8192) == 8192)
				{
					num5 += 8;
				}
				Color color = Lighting.GetColor(tileX + 1, tileY - 3);
				Main.tileBatch.Draw(TextureAssets.XmasTree[0].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + num)) + screenOffset, new Rectangle?(new Rectangle(0, 0, 64, 128)), color, TileDrawing._zero, 1f, SpriteEffects.None);
				if (num2 > 0)
				{
					num2--;
					Color color2 = color;
					if (num2 != 3)
					{
						color2 = new Color(255, 255, 255, 255);
					}
					Main.tileBatch.Draw(TextureAssets.XmasTree[3].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + num)) + screenOffset, new Rectangle?(new Rectangle(66 * num2, 0, 64, 128)), color2, TileDrawing._zero, 1f, SpriteEffects.None);
				}
				if (num3 > 0)
				{
					num3--;
					Main.tileBatch.Draw(TextureAssets.XmasTree[1].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + num)) + screenOffset, new Rectangle?(new Rectangle(66 * num3, 0, 64, 128)), color, TileDrawing._zero, 1f, SpriteEffects.None);
				}
				if (num4 > 0)
				{
					num4--;
					Main.tileBatch.Draw(TextureAssets.XmasTree[2].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + num)) + screenOffset, new Rectangle?(new Rectangle(66 * num4, 0, 64, 128)), color, TileDrawing._zero, 1f, SpriteEffects.None);
				}
				if (num5 > 0)
				{
					num5--;
					Main.tileBatch.Draw(TextureAssets.XmasTree[4].Value, new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + num)) + screenOffset, new Rectangle?(new Rectangle(66 * num5, 130 * Main.tileFrame[171], 64, 128)), new Color(255, 255, 255, 255), TileDrawing._zero, 1f, SpriteEffects.None);
				}
			}
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x005D42BC File Offset: 0x005D24BC
		private void DrawTile_BackRope(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
		{
			if (!WorldGen.InWorld(tileX, tileY, 1))
			{
				return;
			}
			int num = tileX;
			int num2 = tileY;
			if (!WorldGen.IsRope(tileX, tileY, out num, out num2, 5))
			{
				return;
			}
			Tile tile = Main.tile[tileX, num];
			if (tile == null)
			{
				return;
			}
			int y = (tileY + tileX) % 3 * 18;
			Texture2D tileDrawTexture = this.GetTileDrawTexture(tile, tileX, tileY);
			Main.tileBatch.Draw(tileDrawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)(tileY * 16 - (int)screenPosition.Y)) + screenOffset, new Rectangle?(new Rectangle(90, y, 16, 16)), drawData.tileLight, default(Vector2), 1f, drawData.tileSpriteEffect);
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x005D4378 File Offset: 0x005D2578
		private void DrawTile_MinecartTrack(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData)
		{
			drawData.tileLight = TileDrawing.GetFinalLight(drawData.tileCache, drawData.typeCache, drawData.tileLight, drawData.colorTint);
			int paintColor;
			int paintColor2;
			Minecart.TrackColors(tileX, tileY, drawData.tileCache, out paintColor, out paintColor2);
			drawData.drawTexture = this.GetTileDrawTexture((int)drawData.tileCache.type, paintColor);
			Texture2D tileDrawTexture = this.GetTileDrawTexture((int)drawData.tileCache.type, paintColor2);
			this.DrawTile_BackRope(screenPosition, screenOffset, tileX, tileY, drawData);
			if (drawData.tileFrameY != -1)
			{
				Main.tileBatch.Draw(tileDrawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)(tileY * 16 - (int)screenPosition.Y)) + screenOffset, new Rectangle?(Minecart.GetSourceRect((int)drawData.tileFrameY, Main.tileFrame[314])), drawData.tileLight, default(Vector2), 1f, drawData.tileSpriteEffect);
			}
			Main.tileBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)(tileY * 16 - (int)screenPosition.Y)) + screenOffset, new Rectangle?(Minecart.GetSourceRect((int)drawData.tileFrameX, Main.tileFrame[314])), drawData.tileLight, default(Vector2), 1f, drawData.tileSpriteEffect);
			if (Minecart.DrawLeftDecoration((int)drawData.tileFrameY))
			{
				Main.tileBatch.Draw(tileDrawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY + 1) * 16 - (int)screenPosition.Y)) + screenOffset, new Rectangle?(Minecart.GetSourceRect(36, 0)), drawData.tileLight, default(Vector2), 1f, drawData.tileSpriteEffect);
			}
			if (Minecart.DrawLeftDecoration((int)drawData.tileFrameX))
			{
				Main.tileBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY + 1) * 16 - (int)screenPosition.Y)) + screenOffset, new Rectangle?(Minecart.GetSourceRect(36, 0)), drawData.tileLight, default(Vector2), 1f, drawData.tileSpriteEffect);
			}
			if (Minecart.DrawRightDecoration((int)drawData.tileFrameY))
			{
				Main.tileBatch.Draw(tileDrawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY + 1) * 16 - (int)screenPosition.Y)) + screenOffset, new Rectangle?(Minecart.GetSourceRect(37, Main.tileFrame[314])), drawData.tileLight, default(Vector2), 1f, drawData.tileSpriteEffect);
			}
			if (Minecart.DrawRightDecoration((int)drawData.tileFrameX))
			{
				Main.tileBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY + 1) * 16 - (int)screenPosition.Y)) + screenOffset, new Rectangle?(Minecart.GetSourceRect(37, 0)), drawData.tileLight, default(Vector2), 1f, drawData.tileSpriteEffect);
			}
			if (Minecart.DrawBumper((int)drawData.tileFrameX))
			{
				Main.tileBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY - 1) * 16 - (int)screenPosition.Y)) + screenOffset, new Rectangle?(Minecart.GetSourceRect(39, 0)), drawData.tileLight, default(Vector2), 1f, drawData.tileSpriteEffect);
				return;
			}
			if (Minecart.DrawBouncyBumper((int)drawData.tileFrameX))
			{
				Main.tileBatch.Draw(drawData.drawTexture, new Vector2((float)(tileX * 16 - (int)screenPosition.X), (float)((tileY - 1) * 16 - (int)screenPosition.Y)) + screenOffset, new Rectangle?(Minecart.GetSourceRect(38, 0)), drawData.tileLight, default(Vector2), 1f, drawData.tileSpriteEffect);
			}
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x005D47A4 File Offset: 0x005D29A4
		private void DrawTile_LiquidBehindTile(bool solidLayer, int waterStyleOverride, Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, Tile tileCache)
		{
			Tile tile = Main.tile[tileX + 1, tileY];
			Tile tile2 = Main.tile[tileX - 1, tileY];
			Tile tile3 = Main.tile[tileX, tileY - 1];
			Tile tile4 = Main.tile[tileX, tileY + 1];
			if (tile == null)
			{
				tile = new Tile();
				Main.tile[tileX + 1, tileY] = tile;
			}
			if (tile2 == null)
			{
				tile2 = new Tile();
				Main.tile[tileX - 1, tileY] = tile2;
			}
			if (tile3 == null)
			{
				tile3 = new Tile();
				Main.tile[tileX, tileY - 1] = tile3;
			}
			if (tile4 == null)
			{
				tile4 = new Tile();
				Main.tile[tileX, tileY + 1] = tile4;
			}
			if (tile.type == 379)
			{
				tile = new Tile();
			}
			if (tile2.type == 379)
			{
				tile2 = new Tile();
			}
			if (tile3.type == 379)
			{
				tile3 = new Tile();
			}
			if (tile4.type == 379)
			{
				tile4 = new Tile();
			}
			if (DebugOptions.hideWater)
			{
				return;
			}
			if (!tileCache.active())
			{
				return;
			}
			if (tileCache.inActive() || this._tileSolidTop[(int)tileCache.type])
			{
				return;
			}
			if (tileCache.halfBrick() && (tile2.liquid > 160 || tile.liquid > 160) && Main.instance.waterfallManager.CheckForWaterfall(tileX, tileY))
			{
				return;
			}
			if (TileID.Sets.BlocksWaterDrawingBehindSelf[(int)tileCache.type] && tileCache.slope() == 0)
			{
				return;
			}
			int num = 0;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			int num2 = 0;
			bool flag6 = false;
			int num3 = (int)tileCache.slope();
			int num4 = tileCache.blockType();
			if (tileCache.type == 379 && tileCache.liquid > 0)
			{
				return;
			}
			if (tileCache.type == 546 && tileCache.liquid > 0)
			{
				flag5 = true;
				flag4 = true;
				flag = true;
				flag2 = true;
				switch (tileCache.liquidType())
				{
				case 0:
					flag6 = true;
					break;
				case 1:
					num2 = 1;
					break;
				case 2:
					num2 = 11;
					break;
				case 3:
					num2 = 14;
					break;
				}
				num = (int)tileCache.liquid;
			}
			else
			{
				if (tileCache.liquid > 0 && num4 != 0 && (num4 != 1 || tileCache.liquid > 160))
				{
					flag5 = true;
					switch (tileCache.liquidType())
					{
					case 0:
						flag6 = true;
						break;
					case 1:
						num2 = 1;
						break;
					case 2:
						num2 = 11;
						break;
					case 3:
						num2 = 14;
						break;
					}
					if ((int)tileCache.liquid > num)
					{
						num = (int)tileCache.liquid;
					}
				}
				if (tile2.liquid > 0 && num3 != 1 && num3 != 3)
				{
					flag = true;
					switch (tile2.liquidType())
					{
					case 0:
						flag6 = true;
						break;
					case 1:
						num2 = 1;
						break;
					case 2:
						num2 = 11;
						break;
					case 3:
						num2 = 14;
						break;
					}
					if ((int)tile2.liquid > num)
					{
						num = (int)tile2.liquid;
					}
				}
				if (tile.liquid > 0 && num3 != 2 && num3 != 4)
				{
					flag2 = true;
					switch (tile.liquidType())
					{
					case 0:
						flag6 = true;
						break;
					case 1:
						num2 = 1;
						break;
					case 2:
						num2 = 11;
						break;
					case 3:
						num2 = 14;
						break;
					}
					if ((int)tile.liquid > num)
					{
						num = (int)tile.liquid;
					}
				}
				if (tile3.liquid > 0 && num3 != 3 && num3 != 4)
				{
					flag3 = true;
					switch (tile3.liquidType())
					{
					case 0:
						flag6 = true;
						break;
					case 1:
						num2 = 1;
						break;
					case 2:
						num2 = 11;
						break;
					case 3:
						num2 = 14;
						break;
					}
				}
				if (tile4.liquid > 0 && num3 != 1 && num3 != 2)
				{
					if (tile4.liquid > 240)
					{
						flag4 = true;
					}
					switch (tile4.liquidType())
					{
					case 0:
						flag6 = true;
						break;
					case 1:
						num2 = 1;
						break;
					case 2:
						num2 = 11;
						break;
					case 3:
						num2 = 14;
						break;
					}
				}
			}
			if (!flag3 && !flag4 && !flag && !flag2 && !flag5)
			{
				return;
			}
			if (waterStyleOverride != -1)
			{
				Main.waterStyle = waterStyleOverride;
			}
			if (num2 == 0)
			{
				num2 = Main.waterStyle;
			}
			VertexColors vertexColors;
			Lighting.GetCornerColors(tileX, tileY, out vertexColors, 1f);
			Vector2 value = new Vector2((float)(tileX * 16), (float)(tileY * 16));
			Rectangle rectangle = new Rectangle(0, 4, 16, 16);
			if (flag4 && (flag || flag2))
			{
				flag = true;
				flag2 = true;
			}
			if (tileCache.active() && (Main.tileSolidTop[(int)tileCache.type] || !Main.tileSolid[(int)tileCache.type]))
			{
				return;
			}
			if ((!flag3 || (!flag && !flag2)) && (!flag4 || !flag3))
			{
				if (flag3)
				{
					rectangle = new Rectangle(0, 4, 16, 4);
					if (tileCache.halfBrick() || tileCache.slope() != 0)
					{
						rectangle = new Rectangle(0, 4, 16, 12);
					}
				}
				else if (flag4 && !flag && !flag2)
				{
					value = new Vector2((float)(tileX * 16), (float)(tileY * 16 + 12));
					rectangle = new Rectangle(0, 4, 16, 4);
				}
				else
				{
					int num5 = (int)((float)(256 - num) / 32f);
					int y = 4;
					if (tile3.liquid == 0 && (num4 != 0 || !WorldGen.SolidTile(tileX, tileY - 1, false)))
					{
						y = 0;
					}
					int num6 = num5 * 2;
					if (tileCache.slope() != 0)
					{
						value = new Vector2((float)(tileX * 16), (float)(tileY * 16 + num6));
						rectangle = new Rectangle(0, num6, 16, 16 - num6);
					}
					else if ((flag && flag2) || tileCache.halfBrick())
					{
						value = new Vector2((float)(tileX * 16), (float)(tileY * 16 + num6));
						rectangle = new Rectangle(0, y, 16, 16 - num6);
					}
					else if (flag)
					{
						value = new Vector2((float)(tileX * 16), (float)(tileY * 16 + num6));
						rectangle = new Rectangle(0, y, 4, 16 - num6);
					}
					else
					{
						value = new Vector2((float)(tileX * 16 + 12), (float)(tileY * 16 + num6));
						rectangle = new Rectangle(0, y, 4, 16 - num6);
					}
				}
			}
			Vector2 vector = value - screenPosition + screenOffset;
			float num7 = 0.5f;
			if (num2 != 1)
			{
				if (num2 == 11)
				{
					num7 = Math.Max(num7 * 1.7f, 1f);
				}
			}
			else
			{
				num7 = Main.player[Main.myPlayer].lavaOpacity;
			}
			if ((num2 != 1 || Main.player[Main.myPlayer].lavaOpacity >= 1f) && ((double)tileY <= Main.worldSurface || num7 > 1f))
			{
				num7 = 1f;
				if (tileCache.wall == 21)
				{
					num7 = 0.9f;
				}
				else if (tileCache.wall > 0)
				{
					num7 = 0.6f;
				}
			}
			if (tileCache.halfBrick() && tile3.liquid > 0 && tileCache.wall > 0)
			{
				num7 = 0f;
			}
			if (num3 == 4 && tile2.liquid == 0 && !WorldGen.SolidTile(tileX - 1, tileY, false))
			{
				num7 = 0f;
			}
			if (num3 == 3 && tile.liquid == 0 && !WorldGen.SolidTile(tileX + 1, tileY, false))
			{
				num7 = 0f;
			}
			vertexColors.BottomLeftColor *= num7;
			vertexColors.BottomRightColor *= num7;
			vertexColors.TopLeftColor *= num7;
			vertexColors.TopRightColor *= num7;
			if (tileCache.halfBrick() && tile3.liquid > 0 && (double)tileY > Main.worldSurface)
			{
				vertexColors.TopLeftColor *= 0f;
				vertexColors.TopRightColor *= 0f;
			}
			bool flag7 = false;
			if (flag6)
			{
				for (int i = 0; i < 15; i++)
				{
					if (Main.IsLiquidStyleWater(i) && Main.liquidAlpha[i] > 0f && i != num2)
					{
						this.DrawPartialLiquid(!solidLayer, tileCache, ref vector, ref rectangle, i, ref vertexColors);
						flag7 = true;
						break;
					}
				}
			}
			VertexColors vertexColors2 = vertexColors;
			float scale = flag7 ? Main.liquidAlpha[num2] : 1f;
			vertexColors2.BottomLeftColor *= scale;
			vertexColors2.BottomRightColor *= scale;
			vertexColors2.TopLeftColor *= scale;
			vertexColors2.TopRightColor *= scale;
			if (num2 == 14)
			{
				LiquidRenderer.SetShimmerVertexColors(ref vertexColors2, solidLayer ? 0.75f : 1f, tileX, tileY);
			}
			this.DrawPartialLiquid(!solidLayer, tileCache, ref vector, ref rectangle, num2, ref vertexColors2);
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x005D507C File Offset: 0x005D327C
		private void CacheSpecialDraws_Part1(int tileX, int tileY, int tileType, int drawDataTileFrameX, int drawDataTileFrameY, bool skipDraw)
		{
			if (tileType == 395)
			{
				Point point = new Point(tileX, tileY);
				if (drawDataTileFrameX % 36 != 0)
				{
					point.X--;
				}
				if (drawDataTileFrameY % 36 != 0)
				{
					point.Y--;
				}
				if (!this._itemFrameTileEntityPositions.ContainsKey(point))
				{
					this._itemFrameTileEntityPositions[point] = TileEntityType<TEItemFrame>.Find(point.X, point.Y);
					if (this._itemFrameTileEntityPositions[point] != -1)
					{
						this.AddSpecialLegacyPoint(point);
					}
				}
			}
			if (tileType == 698)
			{
				Point point2 = new Point(tileX, tileY);
				if (drawDataTileFrameX % 18 != 0)
				{
					point2.X--;
				}
				if (drawDataTileFrameY % 36 != 0)
				{
					point2.Y--;
				}
				if (!this._deadCellsDisplayJarTileEntityPositions.ContainsKey(point2))
				{
					this._deadCellsDisplayJarTileEntityPositions[point2] = TileEntityType<TEDeadCellsDisplayJar>.Find(point2.X, point2.Y);
					if (this._deadCellsDisplayJarTileEntityPositions[point2] != -1)
					{
						this.AddSpecialLegacyPoint(point2);
					}
				}
			}
			if (tileType == 520)
			{
				Point point3 = new Point(tileX, tileY);
				if (!this._foodPlatterTileEntityPositions.ContainsKey(point3))
				{
					this._foodPlatterTileEntityPositions[point3] = TileEntityType<TEFoodPlatter>.Find(point3.X, point3.Y);
					if (this._foodPlatterTileEntityPositions[point3] != -1)
					{
						this.AddSpecialLegacyPoint(point3);
					}
				}
			}
			if (tileType == 471)
			{
				Point point4 = new Point(tileX, tileY);
				point4.X -= drawDataTileFrameX % 54 / 18;
				point4.Y -= drawDataTileFrameY % 54 / 18;
				if (!this._weaponRackTileEntityPositions.ContainsKey(point4))
				{
					this._weaponRackTileEntityPositions[point4] = TileEntityType<TEWeaponsRack>.Find(point4.X, point4.Y);
					if (this._weaponRackTileEntityPositions[point4] != -1)
					{
						this.AddSpecialLegacyPoint(point4);
					}
				}
			}
			if (tileType == 470)
			{
				Point point5 = new Point(tileX, tileY);
				point5.X -= drawDataTileFrameX % 36 / 18;
				point5.Y -= drawDataTileFrameY % 54 / 18;
				if (!this._displayDollTileEntityPositions.ContainsKey(point5))
				{
					this._displayDollTileEntityPositions[point5] = TileEntityType<TEDisplayDoll>.Find(point5.X, point5.Y);
					if (this._displayDollTileEntityPositions[point5] != -1)
					{
						this.AddSpecialLegacyPoint(point5);
					}
				}
			}
			if (tileType == 475)
			{
				Point point6 = new Point(tileX, tileY);
				point6.X -= drawDataTileFrameX % 54 / 18;
				point6.Y -= drawDataTileFrameY % 72 / 18;
				if (!this._hatRackTileEntityPositions.ContainsKey(point6))
				{
					this._hatRackTileEntityPositions[point6] = TileEntityType<TEHatRack>.Find(point6.X, point6.Y);
					if (this._hatRackTileEntityPositions[point6] != -1)
					{
						this.AddSpecialLegacyPoint(point6);
					}
				}
			}
			if (tileType == 620 && drawDataTileFrameX == 0 && drawDataTileFrameY == 0)
			{
				this.AddSpecialLegacyPoint(tileX, tileY);
			}
			if (tileType == 237 && drawDataTileFrameX == 18 && drawDataTileFrameY == 0)
			{
				this.AddSpecialLegacyPoint(tileX, tileY);
			}
			if (skipDraw)
			{
				return;
			}
			if (tileType <= 589)
			{
				if (tileType != 5)
				{
					if (tileType != 323)
					{
						if (tileType - 583 > 6)
						{
							return;
						}
					}
					else
					{
						if (drawDataTileFrameX <= 132 && drawDataTileFrameX >= 88)
						{
							this.AddSpecialPoint(tileX, tileY, TileDrawing.TileCounterType.Tree);
							return;
						}
						return;
					}
				}
			}
			else if (tileType != 596 && tileType != 616 && tileType != 634)
			{
				return;
			}
			if (drawDataTileFrameY >= 198 && drawDataTileFrameX >= 22)
			{
				this.AddSpecialPoint(tileX, tileY, TileDrawing.TileCounterType.Tree);
			}
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x005D53E0 File Offset: 0x005D35E0
		private void CacheSpecialDraws_Part2(int tileX, int tileY, TileDrawInfo drawData, bool skipDraw)
		{
			if (TileID.Sets.BasicChest[(int)drawData.typeCache])
			{
				Point point = new Point(tileX, tileY);
				if (drawData.tileFrameX % 36 != 0)
				{
					point.X--;
				}
				if (drawData.tileFrameY % 36 != 0)
				{
					point.Y--;
				}
				if (!this._chestPositions.ContainsKey(point))
				{
					this._chestPositions[point] = Chest.FindChest(point.X, point.Y);
				}
				int num = (int)(drawData.tileFrameX / 18);
				short num2 = drawData.tileFrameY / 18;
				int num3 = (int)(drawData.tileFrameX / 36);
				int num4 = num * 18;
				drawData.addFrX = num4 - (int)drawData.tileFrameX;
				int num5 = (int)(num2 * 18);
				if (this._chestPositions[point] != -1)
				{
					int frame = Main.chest[this._chestPositions[point]].frame;
					if (frame == 1)
					{
						num5 += 38;
					}
					if (frame == 2)
					{
						num5 += 76;
					}
				}
				drawData.addFrY = num5 - (int)drawData.tileFrameY;
				if (num2 != 0)
				{
					drawData.tileHeight = 18;
				}
				if (drawData.typeCache == 21 && (num3 == 48 || num3 == 49))
				{
					drawData.glowSourceRect = new Rectangle(16 * (num % 2), (int)drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight);
				}
			}
			if (drawData.typeCache == 378)
			{
				Point point2 = new Point(tileX, tileY);
				if (drawData.tileFrameX % 36 != 0)
				{
					point2.X--;
				}
				if (drawData.tileFrameY % 54 != 0)
				{
					point2.Y -= (int)(drawData.tileFrameY / 18);
				}
				if (!this._trainingDummyTileEntityPositions.ContainsKey(point2))
				{
					this._trainingDummyTileEntityPositions[point2] = TileEntityType<TETrainingDummy>.Find(point2.X, point2.Y);
				}
				TETrainingDummy tetrainingDummy;
				if (this._trainingDummyTileEntityPositions[point2] != -1 && TileEntity.TryGet<TETrainingDummy>(this._trainingDummyTileEntityPositions[point2], out tetrainingDummy))
				{
					int npc = tetrainingDummy.npc;
					if (npc != -1)
					{
						int num6 = Main.npc[npc].frame.Y / 55;
						num6 *= 54;
						num6 += (int)drawData.tileFrameY;
						drawData.addFrY = num6 - (int)drawData.tileFrameY;
					}
				}
			}
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x005D5614 File Offset: 0x005D3814
		private static Color GetFinalLight(Tile tileCache, ushort typeCache, Color tileLight, Color tint)
		{
			int num = (int)((float)(tileLight.R * tint.R) / 255f);
			int num2 = (int)((float)(tileLight.G * tint.G) / 255f);
			int num3 = (int)((float)(tileLight.B * tint.B) / 255f);
			if (num > 255)
			{
				num = 255;
			}
			if (num2 > 255)
			{
				num2 = 255;
			}
			if (num3 > 255)
			{
				num3 = 255;
			}
			num3 <<= 16;
			num2 <<= 8;
			tileLight.PackedValue = (uint)(num | num2 | num3 | -16777216);
			if (tileCache.fullbrightBlock())
			{
				tileLight = Color.White;
			}
			if (tileCache.inActive())
			{
				tileLight = tileCache.actColor(tileLight);
			}
			else if (TileDrawing.ShouldTileShine(typeCache, tileCache.frameX))
			{
				tileLight = Main.shine(tileLight, (int)typeCache);
			}
			return tileLight;
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x005D56E8 File Offset: 0x005D38E8
		private static void GetFinalLight(Tile tileCache, ushort typeCache, ref Vector3 tileLight, ref Vector3 tint)
		{
			tileLight *= tint;
			if (tileCache.inActive())
			{
				tileCache.actColor(ref tileLight);
				return;
			}
			if (TileDrawing.ShouldTileShine(typeCache, tileCache.frameX))
			{
				Main.shine(ref tileLight, (int)typeCache);
			}
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x005D5728 File Offset: 0x005D3928
		private static bool ShouldTileShine(ushort type, short frameX)
		{
			if ((Main.shimmerAlpha > 0f && Main.tileSolid[(int)type]) || type == 165)
			{
				return true;
			}
			if (!Main.tileShine2[(int)type])
			{
				return false;
			}
			if (type != 21 && type != 441)
			{
				return type - 467 > 1 || (frameX >= 144 && frameX < 178);
			}
			return frameX >= 36 && frameX < 178;
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x005D579C File Offset: 0x005D399C
		private static bool IsTileDangerous(Player localPlayer, Tile tileCache, ushort typeCache)
		{
			bool flag = false || typeCache == 135 || typeCache == 137 || TileID.Sets.Boulders[(int)typeCache] || typeCache == 141 || typeCache == 210 || typeCache == 442 || typeCache == 443 || typeCache == 444 || typeCache == 411 || typeCache == 485 || typeCache == 85 || typeCache == 654 || (typeCache == 314 && Minecart.IsPressurePlate(tileCache));
			flag |= (Main.getGoodWorld && typeCache == 230);
			flag |= (Main.dontStarveWorld && typeCache == 80);
			if (tileCache.slope() == 0 && !tileCache.inActive())
			{
				flag = (flag || typeCache == 32 || typeCache == 69 || typeCache == 48 || typeCache == 232 || typeCache == 352 || typeCache == 483 || typeCache == 482 || typeCache == 481 || typeCache == 51 || typeCache == 229);
				if (!localPlayer.fireWalk)
				{
					flag = (flag || typeCache == 37 || typeCache == 58 || typeCache == 76);
				}
				if (!localPlayer.iceSkate)
				{
					flag = (flag || typeCache == 162);
				}
			}
			return flag;
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x005D58E6 File Offset: 0x005D3AE6
		private bool IsTileDrawLayerSolid(ushort typeCache)
		{
			if (TileID.Sets.DrawTileInSolidLayer[(int)typeCache] != null)
			{
				return TileID.Sets.DrawTileInSolidLayer[(int)typeCache].Value;
			}
			return this._tileSolid[(int)typeCache];
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x005D5914 File Offset: 0x005D3B14
		private void GetTileOutlineInfo(int x, int y, ushort typeCache, ref Color tileLight, ref Texture2D highlightTexture, ref Color highlightColor)
		{
			bool isTileSelected;
			if (Main.InSmartCursorHighlightArea(x, y, out isTileSelected))
			{
				int num = (int)((tileLight.R + tileLight.G + tileLight.B) / 3);
				if (num > 10)
				{
					highlightTexture = TextureAssets.HighlightMask[(int)typeCache].Value;
					highlightColor = Colors.GetSelectionGlowColor(isTileSelected, num);
				}
			}
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x005D5968 File Offset: 0x005D3B68
		private void DrawPartialLiquid(bool behindBlocks, Tile tileCache, ref Vector2 position, ref Rectangle liquidSize, int liquidType, ref VertexColors colors)
		{
			int num = (int)tileCache.slope();
			bool flag = !TileID.Sets.BlocksWaterDrawingBehindSelf[(int)tileCache.type];
			if (!behindBlocks)
			{
				flag = false;
			}
			if (flag || num == 0)
			{
				Main.tileBatch.Draw(TextureAssets.Liquid[liquidType].Value, position, new Rectangle?(liquidSize), colors, default(Vector2), 1f, SpriteEffects.None);
				return;
			}
			liquidSize.X += 18 * (num - 1);
			if (num == 1)
			{
				Main.tileBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, new Rectangle?(liquidSize), colors, Vector2.Zero, 1f, SpriteEffects.None);
				return;
			}
			if (num == 2)
			{
				Main.tileBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, new Rectangle?(liquidSize), colors, Vector2.Zero, 1f, SpriteEffects.None);
				return;
			}
			if (num == 3)
			{
				Main.tileBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, new Rectangle?(liquidSize), colors, Vector2.Zero, 1f, SpriteEffects.None);
				return;
			}
			if (num == 4)
			{
				Main.tileBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, position, new Rectangle?(liquidSize), colors, Vector2.Zero, 1f, SpriteEffects.None);
			}
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x005D5AE7 File Offset: 0x005D3CE7
		private bool InAPlaceWithWind(int x, int y, int width, int height)
		{
			return WorldGen.InAPlaceWithWind(x, y, width, height);
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x005D5AF4 File Offset: 0x005D3CF4
		private void GetTileDrawData(int x, int y, Tile tileCache, ushort typeCache, ref short tileFrameX, ref short tileFrameY, out int tileWidth, out int tileHeight, out int tileTop, out int halfBrickHeight, out int addFrX, out int addFrY, out SpriteEffects tileSpriteEffect, out Texture2D glowTexture, out Rectangle glowSourceRect, out Color glowColor)
		{
			tileTop = 0;
			tileWidth = 16;
			tileHeight = 16;
			halfBrickHeight = 0;
			addFrY = Main.tileFrame[(int)typeCache] * 38;
			addFrX = 0;
			tileSpriteEffect = SpriteEffects.None;
			glowTexture = null;
			glowSourceRect = Rectangle.Empty;
			glowColor = Color.Transparent;
			Color color = Lighting.GetColor(x, y);
			switch (typeCache)
			{
			case 3:
			case 24:
			case 61:
			case 71:
			case 110:
			case 201:
			case 637:
				break;
			case 4:
				tileWidth = 20;
				tileHeight = 20;
				if (WorldGen.SolidTile(x, y - 1, false))
				{
					tileTop = 4;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 5:
			{
				tileWidth = 20;
				tileHeight = 20;
				int treeBiome = TileDrawing.GetTreeBiome(x, y, (int)tileFrameX, (int)tileFrameY);
				tileFrameX += (short)(176 * (treeBiome + 1));
				goto IL_25BA;
			}
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 13:
			case 19:
			case 22:
			case 23:
			case 25:
			case 29:
			case 30:
			case 34:
			case 35:
			case 37:
			case 38:
			case 39:
			case 40:
			case 41:
			case 42:
			case 43:
			case 44:
			case 45:
			case 46:
			case 47:
			case 48:
			case 50:
			case 51:
			case 53:
			case 54:
			case 55:
			case 56:
			case 57:
			case 58:
			case 59:
			case 60:
			case 63:
			case 64:
			case 65:
			case 66:
			case 67:
			case 68:
			case 70:
			case 75:
			case 76:
			case 86:
			case 87:
			case 88:
			case 91:
			case 92:
			case 93:
			case 94:
			case 95:
			case 97:
			case 98:
			case 99:
			case 101:
			case 103:
			case 104:
			case 107:
			case 108:
			case 109:
			case 111:
			case 112:
			case 114:
			case 116:
			case 117:
			case 118:
			case 119:
			case 120:
			case 121:
			case 122:
			case 123:
			case 125:
			case 126:
			case 127:
			case 128:
			case 130:
			case 131:
			case 140:
			case 141:
			case 144:
			case 145:
			case 146:
			case 147:
			case 148:
			case 149:
			case 150:
			case 151:
			case 152:
			case 153:
			case 154:
			case 155:
			case 156:
			case 157:
			case 158:
			case 159:
			case 160:
			case 161:
			case 162:
			case 163:
			case 164:
			case 165:
			case 166:
			case 167:
			case 168:
			case 169:
			case 170:
			case 171:
			case 175:
			case 176:
			case 177:
			case 179:
			case 180:
			case 181:
			case 182:
			case 183:
			case 188:
			case 189:
			case 190:
			case 191:
			case 192:
			case 193:
			case 194:
			case 195:
			case 196:
			case 197:
			case 198:
			case 199:
			case 200:
			case 202:
			case 203:
			case 204:
			case 206:
			case 208:
			case 209:
			case 211:
			case 212:
			case 213:
			case 214:
			case 215:
			case 216:
			case 221:
			case 222:
			case 223:
			case 224:
			case 225:
			case 226:
			case 229:
			case 230:
			case 232:
			case 234:
			case 236:
			case 237:
			case 239:
			case 240:
			case 241:
			case 242:
			case 245:
			case 246:
			case 248:
			case 249:
			case 250:
			case 251:
			case 252:
			case 253:
			case 255:
			case 256:
			case 257:
			case 258:
			case 259:
			case 260:
			case 261:
			case 262:
			case 263:
			case 264:
			case 265:
			case 266:
			case 267:
			case 268:
			case 269:
			case 273:
			case 274:
			case 284:
			case 287:
			case 311:
			case 312:
			case 313:
			case 314:
			case 315:
			case 319:
			case 320:
			case 321:
			case 322:
			case 325:
			case 334:
			case 335:
			case 337:
			case 338:
			case 346:
			case 347:
			case 348:
			case 350:
			case 353:
			case 356:
			case 357:
			case 365:
			case 366:
			case 367:
			case 368:
			case 369:
			case 370:
			case 371:
			case 373:
			case 374:
			case 375:
			case 380:
			case 381:
			case 383:
			case 384:
			case 385:
			case 386:
			case 387:
			case 395:
			case 396:
			case 397:
			case 398:
			case 399:
			case 400:
			case 401:
			case 402:
			case 403:
			case 404:
			case 407:
			case 408:
			case 409:
			case 415:
			case 416:
			case 417:
			case 418:
			case 419:
			case 420:
			case 423:
			case 424:
			case 425:
			case 426:
			case 427:
			case 429:
			case 430:
			case 431:
			case 432:
			case 433:
			case 434:
			case 435:
			case 436:
			case 437:
			case 438:
			case 439:
			case 440:
			case 444:
			case 445:
			case 446:
			case 447:
			case 448:
			case 449:
			case 450:
			case 451:
			case 460:
			case 461:
			case 465:
			case 471:
			case 472:
			case 473:
			case 474:
			case 477:
			case 478:
			case 479:
			case 481:
			case 482:
			case 483:
			case 484:
			case 486:
			case 492:
			case 495:
			case 496:
			case 498:
			case 500:
			case 501:
			case 502:
			case 503:
			case 504:
			case 510:
			case 511:
			case 512:
			case 513:
			case 514:
			case 515:
			case 516:
			case 517:
			case 531:
			case 534:
			case 535:
			case 536:
			case 537:
			case 539:
			case 540:
			case 545:
			case 546:
			case 549:
			case 557:
			case 562:
			case 563:
			case 566:
			case 573:
			case 591:
			case 618:
			case 625:
			case 626:
			case 627:
			case 628:
			case 630:
			case 631:
			case 633:
			case 635:
			case 641:
			case 655:
			case 659:
			case 661:
			case 662:
			case 663:
				goto IL_25BA;
			case 12:
			case 31:
			case 96:
			case 639:
			case 665:
				goto IL_1AAA;
			case 14:
			case 21:
			case 411:
			case 467:
			case 469:
				if (tileFrameY == 18)
				{
					tileHeight = 18;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 15:
			case 497:
				if (tileFrameY % 40 == 18)
				{
					tileHeight = 18;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 16:
			case 17:
			case 26:
			case 32:
			case 69:
			case 72:
			case 77:
			case 124:
			case 137:
			case 138:
			case 352:
			case 462:
			case 487:
			case 488:
			case 574:
			case 575:
			case 576:
			case 577:
			case 578:
			case 664:
				goto IL_1187;
			case 18:
			{
				int num = (int)(tileFrameX / 2016);
				addFrX -= 2016 * num;
				addFrY += 20 * num;
				goto IL_25BA;
			}
			case 20:
			case 590:
			case 595:
				tileHeight = 18;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 27:
				if (tileFrameY % 74 == 54)
				{
					tileHeight = 18;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 28:
			case 105:
			case 470:
			case 475:
			case 506:
			case 547:
			case 548:
			case 552:
			case 560:
			case 597:
			case 613:
			case 621:
			case 622:
			case 623:
			case 653:
				goto IL_17A9;
			case 33:
			case 49:
			case 174:
			case 372:
			case 646:
				tileHeight = 20;
				tileTop = -4;
				goto IL_25BA;
			case 36:
				tileTop = 2;
				goto IL_25BA;
			case 52:
			case 62:
			case 115:
			case 205:
			case 382:
			case 528:
			case 636:
			case 638:
				tileTop = -2;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 73:
			case 74:
			case 113:
				tileTop = -12;
				tileHeight = 32;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 78:
			case 85:
			case 133:
			case 134:
			case 173:
			case 210:
			case 233:
			case 254:
			case 283:
			case 378:
			case 457:
			case 466:
			case 520:
			case 651:
			case 652:
				tileTop = 2;
				goto IL_25BA;
			case 79:
			{
				tileHeight = 18;
				int num2 = (int)(tileFrameY / 2016);
				addFrY -= 2016 * num2;
				addFrX += 144 * num2;
				goto IL_25BA;
			}
			case 80:
			case 142:
			case 143:
				tileTop = 2;
				goto IL_25BA;
			case 81:
				tileTop -= 8;
				tileHeight = 26;
				tileWidth = 24;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 82:
			case 83:
			case 84:
				tileHeight = 20;
				tileTop = -2;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 89:
				tileTop = 2;
				goto IL_25BA;
			case 90:
			{
				int num3 = (int)(tileFrameY / 2016);
				addFrY -= 2016 * num3;
				addFrX += 144 * num3;
				goto IL_25BA;
			}
			case 100:
			{
				tileTop = 2;
				int num4 = (int)(tileFrameY / 2016);
				addFrY -= 2016 * num4;
				addFrX += 72 * num4;
				goto IL_25BA;
			}
			case 102:
				tileTop = 2;
				goto IL_25BA;
			case 106:
				addFrY = Main.tileFrame[(int)typeCache] * 54;
				goto IL_25BA;
			case 129:
				addFrY = 0;
				if (tileFrameX >= 324)
				{
					int num5 = (int)((tileFrameX - 324) / 18);
					int num6 = (num5 + Main.tileFrame[(int)typeCache]) % 6 - num5;
					addFrX = num6 * 18;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 132:
			case 135:
				tileTop = 2;
				tileHeight = 18;
				goto IL_25BA;
			case 136:
				if (tileFrameX == 0)
				{
					tileTop = 2;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 139:
			{
				tileTop = 2;
				int num7 = (int)(tileFrameY / 2016);
				addFrY -= 2016 * num7;
				addFrX += 72 * num7;
				goto IL_25BA;
			}
			case 172:
			case 376:
				if (tileFrameY % 38 == 18)
				{
					tileHeight = 18;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 178:
				if (tileFrameY <= 36)
				{
					tileTop = 2;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 184:
				tileWidth = 20;
				if (tileFrameY <= 36)
				{
					tileTop = 2;
					goto IL_25BA;
				}
				if (tileFrameY <= 108)
				{
					tileTop = -2;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 185:
			case 186:
			case 187:
				tileTop = 2;
				if (typeCache == 185)
				{
					if (tileFrameY == 18 && tileFrameX >= 576 && tileFrameX <= 882)
					{
						Main.tileShine2[185] = true;
					}
					else
					{
						Main.tileShine2[185] = false;
					}
					if (tileFrameY == 18)
					{
						int num8 = (int)(tileFrameX / 1908);
						addFrX -= 1908 * num8;
						addFrY += 18 * num8;
						goto IL_25BA;
					}
					goto IL_25BA;
				}
				else if (typeCache == 186)
				{
					if (tileFrameX >= 864 && tileFrameX <= 1170)
					{
						Main.tileShine2[186] = true;
						goto IL_25BA;
					}
					Main.tileShine2[186] = false;
					goto IL_25BA;
				}
				else
				{
					if (typeCache == 187)
					{
						int num9 = (int)(tileFrameX / 1890);
						addFrX -= 1890 * num9;
						addFrY += 36 * num9;
						goto IL_25BA;
					}
					goto IL_25BA;
				}
				break;
			case 207:
				tileTop = 2;
				if (tileFrameY >= 72)
				{
					addFrY = Main.tileFrame[(int)typeCache];
					int num10 = x;
					if (tileFrameX % 36 != 0)
					{
						num10--;
					}
					addFrY += num10 % 6;
					if (addFrY >= 6)
					{
						addFrY -= 6;
					}
					addFrY *= 72;
					goto IL_25BA;
				}
				addFrY = 0;
				goto IL_25BA;
			case 217:
			case 218:
			case 564:
				addFrY = Main.tileFrame[(int)typeCache] * 36;
				tileTop = 2;
				goto IL_25BA;
			case 219:
			case 220:
			case 642:
				addFrY = Main.tileFrame[(int)typeCache] * 54;
				tileTop = 2;
				goto IL_25BA;
			case 227:
				tileWidth = 32;
				tileHeight = 38;
				if (tileFrameX == 238)
				{
					tileTop -= 6;
				}
				else
				{
					tileTop -= 20;
				}
				if (tileFrameX == 204)
				{
					bool flag;
					bool flag2;
					bool flag3;
					WorldGen.GetCactusType(x, y, (int)tileFrameX, (int)tileFrameY, out flag, out flag2, out flag3);
					if (flag2)
					{
						tileFrameX += 238;
					}
					if (flag)
					{
						tileFrameX += 204;
					}
					if (flag3)
					{
						tileFrameX += 272;
					}
				}
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 228:
			case 231:
			case 243:
			case 247:
				tileTop = 2;
				addFrY = Main.tileFrame[(int)typeCache] * 54;
				goto IL_25BA;
			case 235:
				addFrY = Main.tileFrame[(int)typeCache] * 18;
				goto IL_25BA;
			case 238:
				tileTop = 2;
				addFrY = Main.tileFrame[(int)typeCache] * 36;
				goto IL_25BA;
			case 244:
				tileTop = 2;
				if (tileFrameX < 54)
				{
					addFrY = Main.tileFrame[(int)typeCache] * 36;
					goto IL_25BA;
				}
				addFrY = 0;
				goto IL_25BA;
			case 270:
			case 271:
			case 581:
			{
				int i = Main.tileFrame[(int)typeCache] + x % 6;
				if (x % 2 == 0)
				{
					i += 3;
				}
				if (x % 3 == 0)
				{
					i += 3;
				}
				if (x % 4 == 0)
				{
					i += 3;
				}
				while (i > 5)
				{
					i -= 6;
				}
				addFrX = i * 18;
				addFrY = 0;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			}
			case 272:
				addFrY = 0;
				goto IL_25BA;
			case 275:
			case 276:
			case 277:
			case 278:
			case 279:
			case 280:
			case 281:
			case 296:
			case 297:
			case 309:
			case 358:
			case 359:
			case 413:
			case 414:
			case 542:
			case 550:
			case 551:
			case 553:
			case 554:
			case 558:
			case 559:
			case 599:
			case 600:
			case 601:
			case 602:
			case 603:
			case 604:
			case 605:
			case 606:
			case 607:
			case 608:
			case 609:
			case 610:
			case 611:
			case 612:
			case 632:
			case 640:
			case 643:
			case 644:
			case 645:
				goto IL_1DA6;
			case 282:
			case 505:
			case 543:
			{
				tileTop = 2;
				Main.critterCage = true;
				int waterAnimalCageFrame = this.GetWaterAnimalCageFrame(x, y, (int)tileFrameX, (int)tileFrameY);
				addFrY = Main.fishBowlFrame[waterAnimalCageFrame] * 36;
				goto IL_25BA;
			}
			case 285:
			case 286:
			case 298:
			case 299:
			case 310:
			case 339:
			case 361:
			case 362:
			case 363:
			case 364:
			case 391:
			case 392:
			case 393:
			case 394:
			case 532:
			case 533:
			case 538:
			case 544:
			case 555:
			case 556:
			case 582:
			case 619:
			case 629:
			{
				tileTop = 2;
				Main.critterCage = true;
				int smallAnimalCageFrame = this.GetSmallAnimalCageFrame(x, y, (int)tileFrameX, (int)tileFrameY);
				if (typeCache <= 391)
				{
					if (typeCache > 299)
					{
						if (typeCache <= 339)
						{
							if (typeCache == 310)
							{
								goto IL_2211;
							}
							if (typeCache != 339)
							{
								goto IL_25BA;
							}
						}
						else
						{
							switch (typeCache)
							{
							case 361:
								goto IL_21D8;
							case 362:
								break;
							case 363:
								goto IL_21FE;
							case 364:
								goto IL_2211;
							default:
								if (typeCache != 391)
								{
									goto IL_25BA;
								}
								goto IL_2211;
							}
						}
						addFrY = Main.grasshopperCageFrame[smallAnimalCageFrame] * 36;
						goto IL_25BA;
					}
					if (typeCache <= 286)
					{
						if (typeCache == 285)
						{
							addFrY = Main.snailCageFrame[smallAnimalCageFrame] * 36;
							goto IL_25BA;
						}
						if (typeCache != 286)
						{
							goto IL_25BA;
						}
						goto IL_21C5;
					}
					else if (typeCache != 298)
					{
						if (typeCache != 299)
						{
							goto IL_25BA;
						}
						goto IL_21FE;
					}
					IL_21D8:
					addFrY = Main.frogCageFrame[smallAnimalCageFrame] * 36;
					goto IL_25BA;
					IL_21FE:
					addFrY = Main.mouseCageFrame[smallAnimalCageFrame] * 36;
					goto IL_25BA;
				}
				if (typeCache <= 538)
				{
					if (typeCache <= 532)
					{
						if (typeCache - 392 <= 2)
						{
							addFrY = Main.slugCageFrame[(int)(typeCache - 392), smallAnimalCageFrame] * 36;
							goto IL_25BA;
						}
						if (typeCache != 532)
						{
							goto IL_25BA;
						}
						addFrY = Main.maggotCageFrame[smallAnimalCageFrame] * 36;
						goto IL_25BA;
					}
					else
					{
						if (typeCache == 533)
						{
							addFrY = Main.ratCageFrame[smallAnimalCageFrame] * 36;
							goto IL_25BA;
						}
						if (typeCache != 538)
						{
							goto IL_25BA;
						}
					}
				}
				else if (typeCache <= 556)
				{
					if (typeCache != 544)
					{
						if (typeCache - 555 > 1)
						{
							goto IL_25BA;
						}
						addFrY = Main.waterStriderCageFrame[smallAnimalCageFrame] * 36;
						goto IL_25BA;
					}
				}
				else
				{
					if (typeCache == 582)
					{
						goto IL_21C5;
					}
					if (typeCache == 619)
					{
						goto IL_2211;
					}
					if (typeCache != 629)
					{
						goto IL_25BA;
					}
				}
				addFrY = Main.ladybugCageFrame[smallAnimalCageFrame] * 36;
				goto IL_25BA;
				IL_21C5:
				addFrY = Main.snail2CageFrame[smallAnimalCageFrame] * 36;
				goto IL_25BA;
				IL_2211:
				addFrY = Main.wormCageFrame[smallAnimalCageFrame] * 36;
				goto IL_25BA;
			}
			case 288:
			case 289:
			case 290:
			case 291:
			case 292:
			case 293:
			case 294:
			case 295:
			case 360:
			case 580:
			case 620:
			{
				tileTop = 2;
				Main.critterCage = true;
				int waterAnimalCageFrame2 = this.GetWaterAnimalCageFrame(x, y, (int)tileFrameX, (int)tileFrameY);
				int num11 = (int)(typeCache - 288);
				if (typeCache == 360 || typeCache == 580 || typeCache == 620)
				{
					num11 = 8;
				}
				addFrY = Main.butterflyCageFrame[num11, waterAnimalCageFrame2] * 36;
				goto IL_25BA;
			}
			case 300:
			case 301:
			case 302:
			case 303:
			case 304:
			case 305:
			case 306:
			case 307:
			case 308:
			case 354:
			case 355:
			case 499:
				addFrY = Main.tileFrame[(int)typeCache] * 54;
				tileTop = 2;
				goto IL_25BA;
			case 316:
			case 317:
			case 318:
			{
				tileTop = 2;
				Main.critterCage = true;
				int smallAnimalCageFrame2 = this.GetSmallAnimalCageFrame(x, y, (int)tileFrameX, (int)tileFrameY);
				int num12 = (int)(typeCache - 316);
				addFrY = Main.jellyfishCageFrame[num12, smallAnimalCageFrame2] * 36;
				goto IL_25BA;
			}
			case 323:
			{
				tileWidth = 20;
				tileHeight = 20;
				int palmTreeBiome = this.GetPalmTreeBiome(x, y);
				tileFrameY = (short)(22 * palmTreeBiome);
				goto IL_25BA;
			}
			case 324:
				tileWidth = 20;
				tileHeight = 20;
				tileTop = -2;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 326:
			case 327:
			case 328:
			case 329:
			case 345:
			case 351:
			case 421:
			case 422:
			case 458:
			case 459:
				goto IL_2550;
			case 330:
			case 331:
			case 332:
			case 333:
				tileTop += 2;
				goto IL_25BA;
			case 336:
			case 340:
			case 341:
			case 342:
			case 343:
			case 344:
				addFrY = Main.tileFrame[(int)typeCache] * 90;
				tileTop = 2;
				goto IL_25BA;
			case 349:
			{
				tileTop = 2;
				int num13 = (int)(tileFrameX % 36);
				int num14 = (int)(tileFrameY % 54);
				int num15;
				if (Animation.GetTemporaryFrame(x - num13 / 18, y - num14 / 18, out num15))
				{
					tileFrameX = (short)(36 * num15 + num13);
					goto IL_25BA;
				}
				goto IL_25BA;
			}
			case 377:
				addFrY = Main.tileFrame[(int)typeCache] * 38;
				tileTop = 2;
				goto IL_25BA;
			case 379:
				addFrY = Main.tileFrame[(int)typeCache] * 90;
				goto IL_25BA;
			case 388:
			case 389:
			{
				int num16 = 94;
				tileTop = -2;
				if ((int)tileFrameY == num16 - 20 || (int)tileFrameY == num16 * 2 - 20 || tileFrameY == 0 || (int)tileFrameY == num16)
				{
					tileHeight = 18;
				}
				if (tileFrameY != 0 && (int)tileFrameY != num16)
				{
					tileTop = 0;
					goto IL_25BA;
				}
				goto IL_25BA;
			}
			case 390:
				addFrY = Main.tileFrame[(int)typeCache] * 36;
				goto IL_25BA;
			case 405:
			{
				tileHeight = 16;
				if (tileFrameY > 0)
				{
					tileHeight = 18;
				}
				int num17 = Main.tileFrame[(int)typeCache];
				if (tileFrameX >= 54)
				{
					num17 = 0;
				}
				addFrY = num17 * 38;
				goto IL_25BA;
			}
			case 406:
			{
				tileHeight = 16;
				if (tileFrameY % 54 >= 36)
				{
					tileHeight = 18;
				}
				int num18 = Main.tileFrame[(int)typeCache];
				if (tileFrameY >= 108)
				{
					num18 = (int)(6 - tileFrameY / 54);
				}
				else if (tileFrameY >= 54)
				{
					num18 = Main.tileFrame[(int)typeCache] - 1;
				}
				addFrY = num18 * 56;
				addFrY += (int)(tileFrameY / 54 * 2);
				goto IL_25BA;
			}
			case 410:
				if (tileFrameY == 36)
				{
					tileHeight = 18;
				}
				if (tileFrameY >= 56)
				{
					addFrY = Main.tileFrame[(int)typeCache];
					addFrY *= 56;
					goto IL_25BA;
				}
				addFrY = 0;
				goto IL_25BA;
			case 412:
				addFrY = 0;
				tileTop = 2;
				goto IL_25BA;
			case 428:
				tileTop += 4;
				if (PressurePlateHelper.PressurePlatesPressed.ContainsKey(new Point(x, y)))
				{
					addFrX += 18;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 441:
			case 468:
			{
				if (tileFrameY == 18)
				{
					tileHeight = 18;
				}
				int num19 = (int)(tileFrameX % 36);
				int num20 = (int)(tileFrameY % 38);
				int num21;
				if (Animation.GetTemporaryFrame(x - num19 / 18, y - num20 / 18, out num21))
				{
					tileFrameY = (short)(38 * num21 + num20);
					goto IL_25BA;
				}
				goto IL_25BA;
			}
			case 442:
				tileWidth = 20;
				tileHeight = 20;
				switch (tileFrameX / 22)
				{
				case 1:
					tileTop = -4;
					goto IL_25BA;
				case 2:
					tileTop = -2;
					tileWidth = 24;
					goto IL_25BA;
				case 3:
					tileTop = -2;
					goto IL_25BA;
				default:
					goto IL_25BA;
				}
				break;
			case 443:
				if (tileFrameX / 36 >= 2)
				{
					tileTop = -2;
					goto IL_25BA;
				}
				tileTop = 2;
				goto IL_25BA;
			case 452:
			{
				int num22 = Main.tileFrame[(int)typeCache];
				if (tileFrameX >= 54)
				{
					num22 = 0;
				}
				addFrY = num22 * 54;
				goto IL_25BA;
			}
			case 453:
			{
				int num23 = Main.tileFrameCounter[(int)typeCache];
				num23 /= 20;
				int num24 = y - (int)(tileFrameY / 18);
				num23 += num24 + x;
				num23 %= 3;
				addFrY = num23 * 54;
				goto IL_25BA;
			}
			case 454:
				addFrY = Main.tileFrame[(int)typeCache] * 54;
				goto IL_25BA;
			case 455:
			{
				addFrY = 0;
				tileTop = 2;
				int num25 = 1 + Main.tileFrame[(int)typeCache];
				if (!BirthdayParty.PartyIsUp)
				{
					num25 = 0;
				}
				addFrY = num25 * 54;
				goto IL_25BA;
			}
			case 456:
			{
				int num26 = Main.tileFrameCounter[(int)typeCache];
				num26 /= 20;
				int num27 = y - (int)(tileFrameY / 18);
				int num28 = x - (int)(tileFrameX / 18);
				num26 += num27 + num28;
				num26 %= 4;
				addFrY = num26 * 54;
				goto IL_25BA;
			}
			case 463:
			case 464:
				addFrY = Main.tileFrame[(int)typeCache] * 72;
				tileTop = 2;
				goto IL_25BA;
			case 476:
				tileWidth = 20;
				tileHeight = 18;
				goto IL_25BA;
			case 480:
			case 509:
			case 657:
				goto IL_2477;
			case 485:
			{
				tileTop = 2;
				int num29 = Main.tileFrameCounter[(int)typeCache];
				num29 /= 5;
				int num30 = y - (int)(tileFrameY / 18);
				int num31 = x - (int)(tileFrameX / 18);
				num29 += num30 + num31;
				num29 %= 4;
				addFrY = num29 * 36;
				goto IL_25BA;
			}
			case 489:
			{
				tileTop = 2;
				int num32 = y - (int)(tileFrameY / 18);
				int num33 = x - (int)(tileFrameX / 18);
				if (this.InAPlaceWithWind(num33, num32, 2, 3))
				{
					int num34 = Main.tileFrameCounter[(int)typeCache];
					num34 /= 5;
					num34 += num32 + num33;
					num34 %= 16;
					addFrY = num34 * 54;
					goto IL_25BA;
				}
				goto IL_25BA;
			}
			case 490:
			{
				tileTop = 2;
				int y2 = y - (int)(tileFrameY / 18);
				int x2 = x - (int)(tileFrameX / 18);
				bool flag4 = this.InAPlaceWithWind(x2, y2, 2, 2);
				int num35 = flag4 ? Main.tileFrame[(int)typeCache] : 0;
				int num36 = 0;
				if (flag4)
				{
					if (Math.Abs(Main.WindForVisuals) > 0.5f)
					{
						switch (Main.weatherVaneBobframe)
						{
						case 0:
							num36 = 0;
							break;
						case 1:
							num36 = 1;
							break;
						case 2:
							num36 = 2;
							break;
						case 3:
							num36 = 1;
							break;
						case 4:
							num36 = 0;
							break;
						case 5:
							num36 = -1;
							break;
						case 6:
							num36 = -2;
							break;
						case 7:
							num36 = -1;
							break;
						}
					}
					else
					{
						switch (Main.weatherVaneBobframe)
						{
						case 0:
							num36 = 0;
							break;
						case 1:
							num36 = 1;
							break;
						case 2:
							num36 = 0;
							break;
						case 3:
							num36 = -1;
							break;
						case 4:
							num36 = 0;
							break;
						case 5:
							num36 = 1;
							break;
						case 6:
							num36 = 0;
							break;
						case 7:
							num36 = -1;
							break;
						}
					}
				}
				num35 += num36;
				if (num35 < 0)
				{
					num35 += 12;
				}
				num35 %= 12;
				addFrY = num35 * 36;
				goto IL_25BA;
			}
			case 491:
				tileTop = 2;
				addFrX = 54;
				goto IL_25BA;
			case 493:
				if (tileFrameY == 0)
				{
					int num37 = Main.tileFrameCounter[(int)typeCache];
					float num38 = Math.Abs(Main.WindForVisuals);
					int num39 = y - (int)(tileFrameY / 18);
					int num40 = x - (int)(tileFrameX / 18);
					if (!this.InAPlaceWithWind(x, num39, 1, 1))
					{
						num38 = 0f;
					}
					if (num38 >= 0.1f)
					{
						if (num38 < 0.5f)
						{
							num37 /= 20;
							num37 += num39 + num40;
							num37 %= 6;
							if (Main.WindForVisuals < 0f)
							{
								num37 = 6 - num37;
							}
							else
							{
								num37++;
							}
							addFrY = num37 * 36;
						}
						else
						{
							num37 /= 10;
							num37 += num39 + num40;
							num37 %= 6;
							if (Main.WindForVisuals < 0f)
							{
								num37 = 12 - num37;
							}
							else
							{
								num37 += 7;
							}
							addFrY = num37 * 36;
						}
					}
				}
				tileTop = 2;
				goto IL_25BA;
			case 494:
				tileTop = 2;
				goto IL_25BA;
			case 507:
			case 508:
			{
				int num41 = 20;
				int num42 = (Main.tileFrameCounter[(int)typeCache] + x * 11 + y * 27) % (num41 * 8);
				addFrY = 90 * (num42 / num41);
				goto IL_25BA;
			}
			case 518:
			{
				int num43 = (int)(tileCache.liquid / 16);
				num43 -= 3;
				if (WorldGen.SolidTile(x, y - 1, false) && num43 > 8)
				{
					num43 = 8;
				}
				if (tileCache.liquid == 0)
				{
					Tile tileSafely = Framing.GetTileSafely(x, y + 1);
					if (tileSafely.nactive())
					{
						int num44 = tileSafely.blockType();
						if (num44 == 1)
						{
							num43 = -16 + Math.Max(8, (int)(tileSafely.liquid / 16));
						}
						else if (num44 == 3 || num44 == 2)
						{
							num43 -= 4;
						}
					}
				}
				tileTop -= num43;
				goto IL_25BA;
			}
			case 519:
				tileTop = 2;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 521:
			case 522:
			case 523:
			case 524:
			case 525:
			case 526:
			case 527:
			{
				tileTop = 2;
				Main.critterCage = true;
				int waterAnimalCageFrame3 = this.GetWaterAnimalCageFrame(x, y, (int)tileFrameX, (int)tileFrameY);
				int num45 = (int)(typeCache - 521);
				addFrY = Main.dragonflyJarFrame[num45, waterAnimalCageFrame3] * 36;
				goto IL_25BA;
			}
			case 529:
			{
				int num46 = y + 1;
				int num47;
				int num48;
				int num49;
				WorldGen.GetBiomeInfluence(x, x, num46, num46, out num47, out num48, out num49);
				int num50 = num47;
				if (num50 < num48)
				{
					num50 = num48;
				}
				if (num50 < num49)
				{
					num50 = num49;
				}
				int num51;
				if (num47 == 0 && num48 == 0 && num49 == 0)
				{
					if (x < WorldGen.beachDistance || x > Main.maxTilesX - WorldGen.beachDistance)
					{
						num51 = 1;
					}
					else
					{
						num51 = 0;
					}
				}
				else if (num49 == num50)
				{
					num51 = 2;
				}
				else if (num48 == num50)
				{
					num51 = 3;
				}
				else
				{
					num51 = 4;
				}
				addFrY += 34 * num51 - (int)tileFrameY;
				tileHeight = 32;
				tileTop = -14;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			}
			case 530:
			{
				int num52 = y - (int)(tileFrameY % 36 / 18) + 2;
				int num53 = x - (int)(tileFrameX % 54 / 18);
				int num54;
				int num55;
				int num56;
				WorldGen.GetBiomeInfluence(num53, num53 + 3, num52, num52, out num54, out num55, out num56);
				int num57 = num54;
				if (num57 < num55)
				{
					num57 = num55;
				}
				if (num57 < num56)
				{
					num57 = num56;
				}
				int num58;
				if (num54 == 0 && num55 == 0 && num56 == 0)
				{
					num58 = 0;
				}
				else if (num56 == num57)
				{
					num58 = 1;
				}
				else if (num55 == num57)
				{
					num58 = 2;
				}
				else
				{
					num58 = 3;
				}
				addFrY += 36 * num58;
				tileTop = 2;
				goto IL_25BA;
			}
			case 541:
				addFrY = (this._shouldShowInvisibleBlocks ? 0 : 90);
				goto IL_25BA;
			case 561:
				tileTop -= 2;
				tileHeight = 20;
				addFrY = (int)(tileFrameY / 18 * 4);
				goto IL_25BA;
			case 565:
				tileTop = 2;
				if (tileFrameX < 36)
				{
					addFrY = Main.tileFrame[(int)typeCache] * 36;
					goto IL_25BA;
				}
				addFrY = 0;
				goto IL_25BA;
			case 567:
				tileWidth = 26;
				tileHeight = 18;
				if (tileFrameY == 0)
				{
					tileTop = -2;
				}
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 568:
			case 569:
			case 570:
			{
				tileTop = 2;
				Main.critterCage = true;
				int waterAnimalCageFrame4 = this.GetWaterAnimalCageFrame(x, y, (int)tileFrameX, (int)tileFrameY);
				addFrY = Main.fairyJarFrame[waterAnimalCageFrame4] * 36;
				goto IL_25BA;
			}
			case 571:
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
				}
				tileTop = 2;
				goto IL_25BA;
			case 572:
			{
				int j;
				for (j = Main.tileFrame[(int)typeCache] + x % 4; j > 3; j -= 4)
				{
				}
				addFrX = j * 18;
				addFrY = 0;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			}
			case 579:
			{
				tileWidth = 20;
				tileHeight = 20;
				tileTop -= 2;
				bool flag5 = (float)(x * 16 + 8) > Main.LocalPlayer.Center.X;
				if (tileFrameX > 0)
				{
					if (flag5)
					{
						addFrY = 22;
						goto IL_25BA;
					}
					addFrY = 0;
					goto IL_25BA;
				}
				else
				{
					if (flag5)
					{
						addFrY = 0;
						goto IL_25BA;
					}
					addFrY = 22;
					goto IL_25BA;
				}
				break;
			}
			case 583:
			case 584:
			case 585:
			case 586:
			case 587:
			case 588:
			case 589:
			case 596:
			case 616:
			case 634:
				tileWidth = 20;
				tileHeight = 20;
				goto IL_25BA;
			case 592:
				addFrY = Main.tileFrame[(int)typeCache] * 54;
				goto IL_25BA;
			case 593:
			{
				if (tileFrameX >= 18)
				{
					addFrX = -18;
				}
				tileTop = 2;
				int num59;
				if (Animation.GetTemporaryFrame(x, y, out num59))
				{
					addFrY = (int)((short)(18 * num59));
					goto IL_25BA;
				}
				if (tileFrameX < 18)
				{
					addFrY = Main.tileFrame[(int)typeCache] * 18;
					goto IL_25BA;
				}
				addFrY = 0;
				goto IL_25BA;
			}
			case 594:
			{
				if (tileFrameX >= 36)
				{
					addFrX = -36;
				}
				tileTop = 2;
				int num60 = (int)(tileFrameX % 36);
				int num61 = (int)(tileFrameY % 36);
				int num62;
				if (Animation.GetTemporaryFrame(x - num60 / 18, y - num61 / 18, out num62))
				{
					addFrY = (int)((short)(36 * num62));
					goto IL_25BA;
				}
				if (tileFrameX < 36)
				{
					addFrY = Main.tileFrame[(int)typeCache] * 36;
					goto IL_25BA;
				}
				addFrY = 0;
				goto IL_25BA;
			}
			case 598:
			{
				tileTop = 2;
				Main.critterCage = true;
				int waterAnimalCageFrame5 = this.GetWaterAnimalCageFrame(x, y, (int)tileFrameX, (int)tileFrameY);
				addFrY = Main.lavaFishBowlFrame[waterAnimalCageFrame5] * 36;
				goto IL_25BA;
			}
			case 614:
				addFrX = Main.tileFrame[(int)typeCache] * 54;
				addFrY = 0;
				tileTop = 2;
				goto IL_25BA;
			case 615:
				tileHeight = 18;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			case 617:
				tileTop = 2;
				tileFrameY %= 144;
				tileFrameX %= 54;
				goto IL_25BA;
			case 624:
				goto IL_1459;
			case 647:
				goto IL_164A;
			case 648:
			{
				tileTop = 2;
				int num63 = (int)(tileFrameX / 1890);
				addFrX -= 1890 * num63;
				addFrY += 36 * num63;
				goto IL_25BA;
			}
			case 649:
			{
				tileTop = 2;
				int num64 = (int)(tileFrameX / 1908);
				addFrX -= 1908 * num64;
				addFrY += 18 * num64;
				goto IL_25BA;
			}
			case 650:
				tileTop = 2;
				goto IL_25BA;
			case 654:
				tileTop += 2;
				goto IL_25BA;
			case 656:
				goto IL_147C;
			case 658:
				tileTop = 2;
				switch (tileFrameY / 54)
				{
				default:
					addFrY = Main.tileFrame[(int)typeCache];
					addFrY *= 54;
					goto IL_25BA;
				case 1:
					addFrY = Main.tileFrame[(int)typeCache];
					addFrY *= 54;
					addFrY += 486;
					goto IL_25BA;
				case 2:
					addFrY = Main.tileFrame[(int)typeCache];
					addFrY *= 54;
					addFrY += 972;
					goto IL_25BA;
				}
				break;
			case 660:
			{
				int k = Main.tileFrame[(int)typeCache] + x % 5;
				if (x % 2 == 0)
				{
					k += 3;
				}
				if (x % 3 == 0)
				{
					k += 3;
				}
				if (x % 4 == 0)
				{
					k += 3;
				}
				while (k > 4)
				{
					k -= 5;
				}
				addFrX = k * 18;
				addFrY = 0;
				if (x % 2 == 0)
				{
					tileSpriteEffect = SpriteEffects.FlipHorizontally;
					goto IL_25BA;
				}
				goto IL_25BA;
			}
			default:
			{
				switch (typeCache)
				{
				case 695:
				case 704:
				case 712:
				case 713:
				case 714:
				case 715:
				case 716:
					goto IL_1187;
				case 696:
					goto IL_1AAA;
				case 697:
				case 702:
				case 707:
				case 709:
				case 717:
				case 718:
				case 722:
				case 723:
				case 724:
				case 727:
				case 728:
				case 729:
				case 730:
				case 731:
				case 732:
				case 734:
				case 735:
				case 736:
				case 737:
				case 738:
					goto IL_25BA;
				case 698:
				{
					tileWidth = 36;
					int num65 = (int)(tileFrameX / 18);
					tileFrameX = (short)(num65 * 38);
					tileHeight = 44;
					goto IL_25BA;
				}
				case 699:
					goto IL_17A9;
				case 700:
					goto IL_1459;
				case 701:
					goto IL_147C;
				case 703:
					goto IL_1145;
				case 705:
					tileTop = 2;
					goto IL_25BA;
				case 706:
					goto IL_164A;
				case 708:
					goto IL_2550;
				case 710:
					goto IL_1DA6;
				case 711:
					if (tileFrameX > 0)
					{
						tileWidth = 18;
					}
					tileHeight = 20;
					glowTexture = TextureAssets.Tile[711].Value;
					glowSourceRect = new Rectangle((int)tileFrameX + addFrX, (int)tileFrameY + addFrY, tileWidth, tileHeight);
					goto IL_25BA;
				case 719:
				{
					int num66 = (x + y + (int)(Main.GlobalTimeWrappedHourly * 15f)) % 14;
					int num67 = num66 / 4;
					int num68 = num66 % 4;
					addFrX += 288 * num67;
					addFrY += 270 * num68;
					goto IL_25BA;
				}
				case 720:
				case 721:
				case 725:
					goto IL_2477;
				case 726:
					tileFrameX = 0;
					tileFrameY = 0;
					tileWidth = 20;
					tileHeight = 20;
					goto IL_25BA;
				case 733:
					tileTop = 2;
					if (tileFrameY < 54)
					{
						addFrX += 54;
					}
					tileFrameX %= 54;
					tileFrameY %= 54;
					goto IL_25BA;
				case 739:
					break;
				default:
					switch (typeCache)
					{
					case 748:
						break;
					case 749:
					case 750:
						goto IL_25BA;
					case 751:
					{
						tileHeight = 46;
						tileWidth = 56;
						int num69 = (x + y * 2) % 7;
						tileFrameY += (short)(num69 * 46);
						goto IL_25BA;
					}
					case 752:
						tileHeight = 38;
						tileWidth = 36;
						tileTop = 2;
						goto IL_25BA;
					default:
						goto IL_25BA;
					}
					break;
				}
				int num70 = Main.tileFrame[(int)typeCache];
				addFrY = num70 * 90;
				goto IL_25BA;
			}
			}
			IL_1145:
			tileHeight = 20;
			if (x % 2 == 0)
			{
				tileSpriteEffect = SpriteEffects.FlipHorizontally;
				goto IL_25BA;
			}
			goto IL_25BA;
			IL_1187:
			tileHeight = 18;
			goto IL_25BA;
			IL_1459:
			tileWidth = 20;
			tileHeight = 16;
			tileTop += 2;
			if (x % 2 == 0)
			{
				tileSpriteEffect = SpriteEffects.FlipHorizontally;
				goto IL_25BA;
			}
			goto IL_25BA;
			IL_147C:
			tileWidth = 24;
			tileHeight = 34;
			tileTop -= 16;
			if (x % 2 == 0)
			{
				tileSpriteEffect = SpriteEffects.FlipHorizontally;
				goto IL_25BA;
			}
			goto IL_25BA;
			IL_164A:
			tileTop = 2;
			goto IL_25BA;
			IL_17A9:
			tileTop = 2;
			goto IL_25BA;
			IL_1AAA:
			addFrY = Main.tileFrame[(int)typeCache] * 36;
			goto IL_25BA;
			IL_1DA6:
			tileTop = 2;
			Main.critterCage = true;
			int bigAnimalCageFrame = this.GetBigAnimalCageFrame(x, y, (int)tileFrameX, (int)tileFrameY);
			if (typeCache <= 542)
			{
				if (typeCache <= 309)
				{
					switch (typeCache)
					{
					case 275:
						goto IL_1F03;
					case 276:
						goto IL_1F3C;
					case 277:
						addFrY = Main.mallardCageFrame[bigAnimalCageFrame] * 54;
						goto IL_25BA;
					case 278:
						addFrY = Main.duckCageFrame[bigAnimalCageFrame] * 54;
						goto IL_25BA;
					case 279:
						break;
					case 280:
						addFrY = Main.blueBirdCageFrame[bigAnimalCageFrame] * 54;
						goto IL_25BA;
					case 281:
						addFrY = Main.redBirdCageFrame[bigAnimalCageFrame] * 54;
						goto IL_25BA;
					default:
						if (typeCache - 296 <= 1)
						{
							addFrY = Main.scorpionCageFrame[0, bigAnimalCageFrame] * 54;
							goto IL_25BA;
						}
						if (typeCache != 309)
						{
							goto IL_25BA;
						}
						addFrY = Main.penguinCageFrame[bigAnimalCageFrame] * 54;
						goto IL_25BA;
					}
				}
				else if (typeCache <= 359)
				{
					if (typeCache != 358)
					{
						if (typeCache != 359)
						{
							goto IL_25BA;
						}
						goto IL_1F03;
					}
				}
				else
				{
					if (typeCache - 413 <= 1)
					{
						goto IL_1F3C;
					}
					if (typeCache != 542)
					{
						goto IL_25BA;
					}
					addFrY = Main.owlCageFrame[bigAnimalCageFrame] * 54;
					goto IL_25BA;
				}
				addFrY = Main.birdCageFrame[bigAnimalCageFrame] * 54;
				goto IL_25BA;
			}
			if (typeCache > 612)
			{
				if (typeCache <= 640)
				{
					if (typeCache != 632 && typeCache != 640)
					{
						goto IL_25BA;
					}
				}
				else if (typeCache - 643 > 2)
				{
					if (typeCache != 710)
					{
						goto IL_25BA;
					}
					int num71 = Main.pufferfishCageFrame[bigAnimalCageFrame] / 33;
					addFrX = 108 * num71;
					addFrY = (Main.pufferfishCageFrame[bigAnimalCageFrame] - num71 * 33) * 54;
					goto IL_25BA;
				}
				addFrY = Main.macawCageFrame[bigAnimalCageFrame] * 54;
				goto IL_25BA;
			}
			switch (typeCache)
			{
			case 550:
			case 551:
				addFrY = Main.turtleCageFrame[bigAnimalCageFrame] * 54;
				goto IL_25BA;
			case 552:
			case 555:
			case 556:
			case 557:
				goto IL_25BA;
			case 553:
				addFrY = Main.grebeCageFrame[bigAnimalCageFrame] * 54;
				goto IL_25BA;
			case 554:
				addFrY = Main.seagullCageFrame[bigAnimalCageFrame] * 54;
				goto IL_25BA;
			case 558:
			case 559:
				addFrY = Main.seahorseCageFrame[bigAnimalCageFrame] * 54;
				goto IL_25BA;
			default:
				if (typeCache - 599 > 6)
				{
					if (typeCache - 606 > 6)
					{
						goto IL_25BA;
					}
					goto IL_1F3C;
				}
				break;
			}
			IL_1F03:
			addFrY = Main.bunnyCageFrame[bigAnimalCageFrame] * 54;
			goto IL_25BA;
			IL_1F3C:
			addFrY = Main.squirrelCageFrame[bigAnimalCageFrame] * 54;
			goto IL_25BA;
			IL_2477:
			tileTop = 2;
			if (tileFrameY >= 54)
			{
				addFrY = Main.tileFrame[(int)typeCache];
				addFrY *= 54;
				goto IL_25BA;
			}
			addFrY = 0;
			goto IL_25BA;
			IL_2550:
			addFrY = Main.tileFrame[(int)typeCache] * 90;
			IL_25BA:
			if (TileID.Sets.Campfires[(int)tileCache.type])
			{
				if (tileFrameY < 36)
				{
					addFrY = Main.tileFrame[(int)typeCache] * 36;
				}
				else
				{
					addFrY = 252;
				}
				tileTop = 2;
			}
			if (tileCache.halfBrick())
			{
				halfBrickHeight = 8;
			}
			int num72;
			if (typeCache <= 412)
			{
				if (typeCache <= 79)
				{
					if (typeCache <= 33)
					{
						switch (typeCache)
						{
						case 10:
							num72 = (int)(tileFrameY / 54);
							if (tileFrameX < 54 && num72 == 32)
							{
								glowTexture = TextureAssets.GlowMask[57].Value;
								glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 54), tileWidth, tileHeight);
								glowColor = this._martianGlow;
								return;
							}
							return;
						case 11:
							num72 = (int)(tileFrameY / 54);
							if (tileFrameX >= 54)
							{
								return;
							}
							if (num72 == 32)
							{
								glowTexture = TextureAssets.GlowMask[58].Value;
								glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 54), tileWidth, tileHeight);
								glowColor = this._martianGlow;
							}
							if (num72 == 33)
							{
								glowTexture = TextureAssets.GlowMask[119].Value;
								glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 54), tileWidth, tileHeight);
								glowColor = this._meteorGlow;
								return;
							}
							return;
						case 12:
						case 13:
						case 16:
						case 17:
						case 20:
							return;
						case 14:
							num72 = (int)(tileFrameX / 54);
							if (num72 == 31)
							{
								glowTexture = TextureAssets.GlowMask[67].Value;
								glowSourceRect = new Rectangle((int)(tileFrameX % 54), (int)tileFrameY, tileWidth, tileHeight);
								glowColor = this._martianGlow;
							}
							if (num72 == 32)
							{
								glowTexture = TextureAssets.GlowMask[124].Value;
								glowSourceRect = new Rectangle((int)(tileFrameX % 54), (int)tileFrameY, tileWidth, tileHeight);
								glowColor = this._meteorGlow;
								return;
							}
							return;
						case 15:
							break;
						case 18:
							num72 = (int)(tileFrameX / 36);
							if (num72 == 27)
							{
								glowTexture = TextureAssets.GlowMask[69].Value;
								glowSourceRect = new Rectangle((int)(tileFrameX % 36), (int)tileFrameY, tileWidth, tileHeight);
								glowColor = this._martianGlow;
							}
							if (num72 == 28)
							{
								glowTexture = TextureAssets.GlowMask[125].Value;
								glowSourceRect = new Rectangle((int)(tileFrameX % 36), (int)tileFrameY, tileWidth, tileHeight);
								glowColor = this._meteorGlow;
								return;
							}
							return;
						case 19:
							num72 = (int)(tileFrameY / 18);
							if (num72 == 26)
							{
								glowTexture = TextureAssets.GlowMask[65].Value;
								glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 18), tileWidth, tileHeight);
								glowColor = this._martianGlow;
							}
							if (num72 == 27)
							{
								glowTexture = TextureAssets.GlowMask[112].Value;
								glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 18), tileWidth, tileHeight);
								glowColor = this._meteorGlow;
								return;
							}
							return;
						case 21:
							goto IL_30AE;
						default:
							if (typeCache != 33)
							{
								return;
							}
							if (tileFrameX / 18 != 0)
							{
								return;
							}
							num72 = (int)(tileFrameY / 22);
							if (num72 == 26)
							{
								glowTexture = TextureAssets.GlowMask[61].Value;
								glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 22), tileWidth, tileHeight);
								glowColor = this._martianGlow;
								return;
							}
							return;
						}
					}
					else if (typeCache != 34)
					{
						if (typeCache != 42)
						{
							if (typeCache != 79)
							{
								return;
							}
							num72 = (int)(tileFrameY / 36);
							if (num72 == 27)
							{
								glowTexture = TextureAssets.GlowMask[53].Value;
								glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 36), tileWidth, tileHeight);
								glowColor = this._martianGlow;
							}
							if (num72 == 28)
							{
								glowTexture = TextureAssets.GlowMask[114].Value;
								glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 36), tileWidth, tileHeight);
								glowColor = this._meteorGlow;
								return;
							}
							return;
						}
						else
						{
							num72 = (int)(tileFrameY / 36);
							int num73 = (int)(tileFrameY / 2016);
							addFrY -= 2016 * num73;
							addFrX += 36 * num73;
							if (num72 == 33)
							{
								glowTexture = TextureAssets.GlowMask[63].Value;
								glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 36), tileWidth, tileHeight);
								glowColor = this._martianGlow;
								return;
							}
							return;
						}
					}
					else
					{
						if (tileFrameX / 54 != 0)
						{
							return;
						}
						num72 = (int)(tileFrameY / 54);
						if (num72 == 33)
						{
							glowTexture = TextureAssets.GlowMask[55].Value;
							glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 54), tileWidth, tileHeight);
							glowColor = this._martianGlow;
							return;
						}
						return;
					}
				}
				else if (typeCache <= 104)
				{
					switch (typeCache)
					{
					case 87:
					{
						num72 = (int)(tileFrameX / 54);
						int num74 = (int)(tileFrameX / 1998);
						addFrX -= 1998 * num74;
						addFrY += 36 * num74;
						if (num72 == 26)
						{
							glowTexture = TextureAssets.GlowMask[64].Value;
							glowSourceRect = new Rectangle((int)(tileFrameX % 54), (int)tileFrameY, tileWidth, tileHeight);
							glowColor = this._martianGlow;
						}
						if (num72 == 27)
						{
							glowTexture = TextureAssets.GlowMask[121].Value;
							glowSourceRect = new Rectangle((int)(tileFrameX % 54), (int)tileFrameY, tileWidth, tileHeight);
							glowColor = this._meteorGlow;
							return;
						}
						return;
					}
					case 88:
					{
						num72 = (int)(tileFrameX / 54);
						int num75 = (int)(tileFrameX / 1998);
						addFrX -= 1998 * num75;
						addFrY += 36 * num75;
						if (num72 == 24)
						{
							glowTexture = TextureAssets.GlowMask[59].Value;
							glowSourceRect = new Rectangle((int)(tileFrameX % 54), (int)tileFrameY, tileWidth, tileHeight);
							glowColor = this._martianGlow;
						}
						if (num72 == 25)
						{
							glowTexture = TextureAssets.GlowMask[120].Value;
							glowSourceRect = new Rectangle((int)(tileFrameX % 54), (int)tileFrameY, tileWidth, tileHeight);
							glowColor = this._meteorGlow;
							return;
						}
						return;
					}
					case 89:
					{
						num72 = (int)(tileFrameX / 54);
						int num76 = (int)(tileFrameX / 1998);
						addFrX -= 1998 * num76;
						addFrY += 36 * num76;
						if (num72 == 29)
						{
							glowTexture = TextureAssets.GlowMask[66].Value;
							glowSourceRect = new Rectangle((int)(tileFrameX % 54), (int)tileFrameY, tileWidth, tileHeight);
							glowColor = this._martianGlow;
						}
						if (num72 == 30)
						{
							glowTexture = TextureAssets.GlowMask[123].Value;
							glowSourceRect = new Rectangle((int)(tileFrameX % 54), (int)tileFrameY, tileWidth, tileHeight);
							glowColor = this._meteorGlow;
							return;
						}
						return;
					}
					case 90:
						num72 = (int)(tileFrameY / 36);
						if (num72 == 27)
						{
							glowTexture = TextureAssets.GlowMask[52].Value;
							glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 36), tileWidth, tileHeight);
							glowColor = this._martianGlow;
						}
						if (num72 == 28)
						{
							glowTexture = TextureAssets.GlowMask[113].Value;
							glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 36), tileWidth, tileHeight);
							glowColor = this._meteorGlow;
							return;
						}
						return;
					case 91:
					case 92:
						return;
					case 93:
					{
						num72 = (int)(tileFrameY / 54);
						int num77 = (int)(tileFrameY / 1998);
						addFrY -= 1998 * num77;
						addFrX += 36 * num77;
						tileTop += 2;
						if (num72 == 27)
						{
							glowTexture = TextureAssets.GlowMask[62].Value;
							glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 54), tileWidth, tileHeight);
							glowColor = this._martianGlow;
							return;
						}
						return;
					}
					default:
						switch (typeCache)
						{
						case 100:
							if (tileFrameX / 36 != 0)
							{
								return;
							}
							num72 = (int)(tileFrameY / 36);
							if (num72 == 27)
							{
								glowTexture = TextureAssets.GlowMask[68].Value;
								glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 36), tileWidth, tileHeight);
								glowColor = this._martianGlow;
								return;
							}
							return;
						case 101:
						{
							num72 = (int)(tileFrameX / 54);
							int num78 = (int)(tileFrameX / 1998);
							addFrX -= 1998 * num78;
							addFrY += 72 * num78;
							if (num72 == 28)
							{
								glowTexture = TextureAssets.GlowMask[60].Value;
								glowSourceRect = new Rectangle((int)(tileFrameX % 54), (int)tileFrameY, tileWidth, tileHeight);
								glowColor = this._martianGlow;
							}
							if (num72 == 29)
							{
								glowTexture = TextureAssets.GlowMask[115].Value;
								glowSourceRect = new Rectangle((int)(tileFrameX % 54), (int)tileFrameY, tileWidth, tileHeight);
								glowColor = this._meteorGlow;
								return;
							}
							return;
						}
						case 102:
						case 103:
							return;
						case 104:
						{
							num72 = (int)(tileFrameX / 36);
							int num79 = (int)(tileFrameX / 2016);
							addFrX -= 2016 * num79;
							addFrY += 90 * num79;
							tileTop = 2;
							if (num72 == 24)
							{
								glowTexture = TextureAssets.GlowMask[51].Value;
								glowSourceRect = new Rectangle((int)(tileFrameX % 36), (int)tileFrameY, tileWidth, tileHeight);
								glowColor = this._martianGlow;
							}
							if (num72 == 25)
							{
								glowTexture = TextureAssets.GlowMask[118].Value;
								glowSourceRect = new Rectangle((int)(tileFrameX % 36), (int)tileFrameY, tileWidth, tileHeight);
								glowColor = this._meteorGlow;
								return;
							}
							return;
						}
						default:
							return;
						}
						break;
					}
				}
				else if (typeCache != 172)
				{
					if (typeCache != 184)
					{
						if (typeCache != 412)
						{
							return;
						}
						glowTexture = TextureAssets.GlowMask[202].Value;
						glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY + addFrY, tileWidth, tileHeight);
						glowColor = new Color(255, 255, 255, 255);
						return;
					}
					else
					{
						if (tileCache.frameX == 110)
						{
							glowTexture = TextureAssets.GlowMask[127].Value;
							glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
							glowColor = this._lavaMossGlow;
						}
						if (tileCache.frameX == 132)
						{
							glowTexture = TextureAssets.GlowMask[127].Value;
							glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
							glowColor = this._kryptonMossGlow;
						}
						if (tileCache.frameX == 154)
						{
							glowTexture = TextureAssets.GlowMask[127].Value;
							glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
							glowColor = this._xenonMossGlow;
						}
						if (tileCache.frameX == 176)
						{
							glowTexture = TextureAssets.GlowMask[127].Value;
							glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
							glowColor = this._argonMossGlow;
						}
						if (tileCache.frameX == 198)
						{
							glowTexture = TextureAssets.GlowMask[127].Value;
							glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
							glowColor = this._violetMossGlow;
						}
						if (tileCache.frameX == 220)
						{
							glowTexture = TextureAssets.GlowMask[127].Value;
							glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY, tileWidth, tileHeight);
							glowColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
							return;
						}
						return;
					}
				}
				else
				{
					num72 = (int)(tileFrameY / 38);
					int num80 = (int)(tileFrameY / 2014);
					addFrY -= 2014 * num80;
					addFrX += 36 * num80;
					if (num72 == 28)
					{
						glowTexture = TextureAssets.GlowMask[88].Value;
						glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 38), tileWidth, tileHeight);
						glowColor = this._martianGlow;
					}
					if (num72 == 29)
					{
						glowTexture = TextureAssets.GlowMask[122].Value;
						glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 38), tileWidth, tileHeight);
						glowColor = this._meteorGlow;
						return;
					}
					return;
				}
			}
			else if (typeCache <= 497)
			{
				if (typeCache <= 463)
				{
					if (typeCache != 441)
					{
						if (typeCache != 463)
						{
							return;
						}
						glowTexture = TextureAssets.GlowMask[243].Value;
						glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY + addFrY, tileWidth, tileHeight);
						glowColor = new Color(127, 127, 127, 0);
						return;
					}
				}
				else
				{
					if (typeCache == 467)
					{
						goto IL_30AE;
					}
					if (typeCache != 468)
					{
						if (typeCache != 497)
						{
							return;
						}
						goto IL_2FA6;
					}
				}
				num72 = (int)(tileFrameX / 36);
				if (num72 == 48)
				{
					glowTexture = TextureAssets.GlowMask[56].Value;
					glowSourceRect = new Rectangle((int)(tileFrameX % 36), (int)tileFrameY, tileWidth, tileHeight);
					glowColor = this._martianGlow;
				}
				if (num72 == 49)
				{
					glowTexture = TextureAssets.GlowMask[117].Value;
					glowSourceRect = new Rectangle((int)(tileFrameX % 36), (int)tileFrameY, tileWidth, tileHeight);
					glowColor = this._meteorGlow;
					return;
				}
				return;
			}
			else
			{
				if (typeCache > 638)
				{
					if (typeCache != 656)
					{
						if (typeCache != 657)
						{
							if (typeCache != 701)
							{
								return;
							}
						}
						else
						{
							if (tileFrameY >= 54)
							{
								glowTexture = TextureAssets.GlowMask[330].Value;
								glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY + addFrY, tileWidth, tileHeight);
								glowColor = Color.White;
								return;
							}
							return;
						}
					}
					glowTexture = TextureAssets.GlowMask[329].Value;
					glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY + addFrY, tileWidth, tileHeight);
					glowColor = new Color(255, 255, 255, 0) * ((float)Main.mouseTextColor / 255f);
					return;
				}
				switch (typeCache)
				{
				case 564:
					if (tileCache.frameX < 36)
					{
						glowTexture = TextureAssets.GlowMask[267].Value;
						glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY + addFrY, tileWidth, tileHeight);
						glowColor = new Color(200, 200, 200, 0) * ((float)Main.mouseTextColor / 255f);
					}
					addFrY = 0;
					return;
				case 565:
				case 566:
				case 567:
					return;
				case 568:
					glowTexture = TextureAssets.GlowMask[268].Value;
					glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY + addFrY, tileWidth, tileHeight);
					glowColor = Color.White;
					return;
				case 569:
					glowTexture = TextureAssets.GlowMask[269].Value;
					glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY + addFrY, tileWidth, tileHeight);
					glowColor = Color.White;
					return;
				case 570:
					glowTexture = TextureAssets.GlowMask[270].Value;
					glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY + addFrY, tileWidth, tileHeight);
					glowColor = Color.White;
					return;
				default:
					if (typeCache == 580)
					{
						glowTexture = TextureAssets.GlowMask[289].Value;
						glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY + addFrY, tileWidth, tileHeight);
						glowColor = new Color(225, 110, 110, 0);
						return;
					}
					switch (typeCache)
					{
					case 634:
						glowTexture = TextureAssets.GlowMask[315].Value;
						glowSourceRect = new Rectangle((int)tileFrameX, (int)tileFrameY + addFrY, tileWidth, tileHeight);
						glowColor = Color.White;
						return;
					case 635:
					case 636:
						return;
					case 637:
						glowTexture = this.GetTileDrawTexture(tileCache, x, y);
						glowSourceRect = new Rectangle((int)tileFrameX + addFrX, (int)tileFrameY + addFrY, tileWidth, tileHeight);
						glowColor = Color.Lerp(Color.White, color, 0.75f);
						return;
					case 638:
						glowTexture = TextureAssets.GlowMask[327].Value;
						glowSourceRect = new Rectangle((int)tileFrameX + addFrX, (int)tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight);
						glowColor = Color.Lerp(Color.White, color, 0.75f);
						return;
					default:
						return;
					}
					break;
				}
			}
			IL_2FA6:
			num72 = (int)(tileFrameY / 40);
			int num81 = num72 / 51;
			addFrY -= 2040 * num81;
			addFrX += 36 * num81;
			if (typeCache != 15)
			{
				return;
			}
			if (num72 == 32)
			{
				glowTexture = TextureAssets.GlowMask[54].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 40), tileWidth, tileHeight);
				glowColor = this._martianGlow;
			}
			if (num72 == 33)
			{
				glowTexture = TextureAssets.GlowMask[116].Value;
				glowSourceRect = new Rectangle((int)tileFrameX, (int)(tileFrameY % 40), tileWidth, tileHeight);
				glowColor = this._meteorGlow;
				return;
			}
			return;
			IL_30AE:
			num72 = (int)(tileFrameX / 36);
			if (num72 == 48)
			{
				glowTexture = TextureAssets.GlowMask[56].Value;
				glowSourceRect = new Rectangle((int)(tileFrameX % 36), (int)tileFrameY, tileWidth, tileHeight);
				glowColor = this._martianGlow;
			}
			if (num72 == 49)
			{
				glowTexture = TextureAssets.GlowMask[117].Value;
				glowSourceRect = new Rectangle((int)(tileFrameX % 36), (int)tileFrameY, tileWidth, tileHeight);
				glowColor = this._meteorGlow;
				return;
			}
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x005D9304 File Offset: 0x005D7504
		private bool IsWindBlocked(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return tile == null || (tile.wall > 0 && !WallID.Sets.AllowsWind[(int)tile.wall]) || (double)y > Main.worldSurface;
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x005D9348 File Offset: 0x005D7548
		private int GetWaterAnimalCageFrame(int x, int y, int tileFrameX, int tileFrameY)
		{
			int num = x - tileFrameX / 18;
			int num2 = y - tileFrameY / 18;
			return num / 2 * (num2 / 3) % Main.cageFrames;
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x005D9370 File Offset: 0x005D7570
		private int GetSmallAnimalCageFrame(int x, int y, int tileFrameX, int tileFrameY)
		{
			int num = x - tileFrameX / 18;
			int num2 = y - tileFrameY / 18;
			return num / 3 * (num2 / 3) % Main.cageFrames;
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x005D9398 File Offset: 0x005D7598
		private int GetBigAnimalCageFrame(int x, int y, int tileFrameX, int tileFrameY)
		{
			int num = x - tileFrameX / 18;
			int num2 = y - tileFrameY / 18;
			return num / 6 * (num2 / 4) % Main.cageFrames;
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x005D93C0 File Offset: 0x005D75C0
		public static void GetScreenDrawArea(bool useOffscreenRange, out Vector2 drawOffSet, out int firstTileX, out int lastTileX, out int firstTileY, out int lastTileY)
		{
			Vector2 scaledPosition = Main.Camera.ScaledPosition;
			Vector2 scaledSize = Main.Camera.ScaledSize;
			drawOffSet = (useOffscreenRange ? new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange) : Vector2.Zero);
			firstTileX = (int)((scaledPosition.X - drawOffSet.X) / 16f - 1f);
			lastTileX = (int)((scaledPosition.X + scaledSize.X + drawOffSet.X) / 16f) + 2;
			firstTileY = (int)((scaledPosition.Y - drawOffSet.Y) / 16f - 1f);
			lastTileY = (int)((scaledPosition.Y + scaledSize.Y + drawOffSet.Y) / 16f) + 5;
			if (firstTileX < 4)
			{
				firstTileX = 4;
			}
			if (lastTileX > Main.maxTilesX - 4)
			{
				lastTileX = Main.maxTilesX - 4;
			}
			if (firstTileY < 4)
			{
				firstTileY = 4;
			}
			if (lastTileY > Main.maxTilesY - 4)
			{
				lastTileY = Main.maxTilesY - 4;
			}
			if (Main.sectionManager.AnyUnfinishedSections)
			{
				TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
				WorldGen.SectionTileFrameWithCheck(firstTileX, firstTileY, lastTileX, lastTileY);
				TimeLogger.SectionFraming.AddTime(fromTimestamp);
			}
			if (Main.sectionManager.AnyNeedRefresh)
			{
				TimeLogger.StartTimestamp fromTimestamp2 = TimeLogger.Start();
				WorldGen.RefreshSections(firstTileX, firstTileY, lastTileX, lastTileY);
				TimeLogger.SectionRefresh.AddTime(fromTimestamp2);
			}
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x005D9510 File Offset: 0x005D7710
		public void ClearCachedTileDraws(bool solidLayer)
		{
			if (solidLayer)
			{
				this._displayDollTileEntityPositions.Clear();
				this._hatRackTileEntityPositions.Clear();
				this._vineRootsPositions.Clear();
				this._reverseVineRootsPositions.Clear();
			}
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x005D9541 File Offset: 0x005D7741
		private void AddSpecialLegacyPoint(Point p)
		{
			this.AddSpecialLegacyPoint(p.X, p.Y);
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x005D9555 File Offset: 0x005D7755
		private void AddSpecialLegacyPoint(int x, int y)
		{
			this._specialTileX[this._specialTilesCount] = x;
			this._specialTileY[this._specialTilesCount] = y;
			this._specialTilesCount++;
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x005D9584 File Offset: 0x005D7784
		private void ClearLegacyCachedDraws()
		{
			this._chestPositions.Clear();
			this._trainingDummyTileEntityPositions.Clear();
			this._foodPlatterTileEntityPositions.Clear();
			this._itemFrameTileEntityPositions.Clear();
			this._deadCellsDisplayJarTileEntityPositions.Clear();
			this._weaponRackTileEntityPositions.Clear();
			this._specialTilesCount = 0;
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x005D95DC File Offset: 0x005D77DC
		private Color DrawTiles_GetLightOverride(int j, int i, Tile tileCache, ushort typeCache, short tileFrameX, short tileFrameY, Color tileLight)
		{
			if (tileCache.fullbrightBlock())
			{
				return Color.White;
			}
			if (typeCache > 84)
			{
				if (typeCache <= 541)
				{
					if (typeCache - 481 <= 2)
					{
						float scale = 1f + (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly / 1.5f * 6.2831855f)) * 0.15f;
						byte a = tileLight.A;
						tileLight *= scale;
						tileLight.A = a;
						return tileLight;
					}
					if (typeCache != 541)
					{
						return tileLight;
					}
				}
				else if (typeCache != 631)
				{
					if (typeCache != 703)
					{
						return tileLight;
					}
					goto IL_140;
				}
				return Color.White;
			}
			if (typeCache <= 61)
			{
				if (typeCache != 19)
				{
					if (typeCache != 61)
					{
						return tileLight;
					}
				}
				else
				{
					if (tileFrameY / 18 == 48)
					{
						return Color.White;
					}
					return tileLight;
				}
			}
			else if (typeCache != 83)
			{
				if (typeCache != 84)
				{
					return tileLight;
				}
				if (tileFrameX / 18 != 6)
				{
					return tileLight;
				}
				byte b = (Main.mouseTextColor + tileLight.G * 2) / 3;
				byte b2 = (Main.mouseTextColor + tileLight.B * 2) / 3;
				if (b > tileLight.G)
				{
					tileLight.G = b;
				}
				if (b2 > tileLight.B)
				{
					tileLight.B = b2;
					return tileLight;
				}
				return tileLight;
			}
			else
			{
				int num = (int)(tileFrameX / 18);
				if (WorldGen.IsAlchemyPlantHarvestable(num, j) && num == 5)
				{
					tileLight.A = Main.mouseTextColor / 2;
					tileLight.G = Main.mouseTextColor;
					tileLight.B = Main.mouseTextColor;
					return tileLight;
				}
				return tileLight;
			}
			IL_140:
			if (tileFrameX == 144)
			{
				tileLight.A = (tileLight.R = (tileLight.G = (tileLight.B = (byte)(245f - (float)Main.mouseTextColor * 1.5f))));
			}
			return tileLight;
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x005D97B4 File Offset: 0x005D79B4
		private void DrawTiles_EmitParticles(int j, int i, Tile tileCache, ushort typeCache, short tileFrameX, short tileFrameY, Color tileLight)
		{
			bool flag = this.IsVisible(tileCache);
			int num = this._leafFrequency;
			num /= 4;
			if (typeCache == 718 && !Main.dayTime && this._rand.Next(3) == 0 && !WorldGen.SolidTile3(i, j - 1))
			{
				if (Main.player[Main.myPlayer].RollLuck(100) == 0)
				{
					int num2 = Gore.NewGore(new Vector2((float)(i * 16 + this._rand.Next(16)), (float)(j * 16 - 12)), default(Vector2), 16, 1f);
					Main.gore[num2].scale *= this._rand.NextFloat() * 0.5f + 0.75f;
					Main.gore[num2].velocity *= 0.2f;
					Gore gore = Main.gore[num2];
					gore.velocity.Y = gore.velocity.Y - (float)this._rand.Next(5, 31) * 0.1f;
					if (this._rand.Next(5) == 0)
					{
						Gore gore2 = Main.gore[num2];
						gore2.velocity.Y = gore2.velocity.Y - (float)this._rand.Next(5, 41) * 0.1f;
					}
					if (this._rand.Next(3) == 0)
					{
						Main.gore[num2].velocity *= 0.5f;
					}
					Main.gore[num2].velocity /= Main.gore[num2].scale;
					int num3 = Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default(Vector2), 16, 1f);
					Main.gore[num3].scale = Main.gore[num2].scale;
					Main.gore[num3].position = Main.gore[num2].position;
					Main.gore[num3].velocity = Main.gore[num2].velocity;
				}
				if (Main.player[Main.myPlayer].RollLuck(60) == 0)
				{
					int num4 = Gore.NewGore(new Vector2((float)(i * 16 + this._rand.Next(16)), (float)(j * 16 - 12)), default(Vector2), 17, 1f);
					Main.gore[num4].scale *= this._rand.NextFloat() * 0.5f + 0.75f;
					Main.gore[num4].velocity *= 0.2f;
					Gore gore3 = Main.gore[num4];
					gore3.velocity.Y = gore3.velocity.Y - (float)this._rand.Next(5, 41) * 0.1f;
					if (this._rand.Next(5) == 0)
					{
						Gore gore4 = Main.gore[num4];
						gore4.velocity.Y = gore4.velocity.Y - (float)this._rand.Next(5, 51) * 0.1f;
					}
					if (this._rand.Next(3) == 0)
					{
						Main.gore[num4].velocity *= 0.5f;
					}
					Main.gore[num4].velocity /= Main.gore[num4].scale;
					int num5 = Gore.NewGore(new Vector2((float)(i * 16), (float)(j * 16)), default(Vector2), 17, 1f);
					Main.gore[num5].scale = Main.gore[num4].scale;
					Main.gore[num5].position = Main.gore[num4].position;
					Main.gore[num5].velocity = Main.gore[num4].velocity;
				}
				if (Main.player[Main.myPlayer].RollLuck(30) == 0)
				{
					int num6 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16 - 2)), 1, 1, 58, 0f, 0f, 150, default(Color), 1f);
					Main.dust[num6].scale *= this._rand.NextFloat() * 0.5f + 0.75f;
					Main.dust[num6].color = new Color(255, 255, 255, 0);
					Main.dust[num6].velocity *= 0.2f;
					Dust dust = Main.dust[num6];
					dust.velocity.Y = dust.velocity.Y - (float)this._rand.Next(5, 51) * 0.1f;
					if (this._rand.Next(5) == 0)
					{
						Dust dust2 = Main.dust[num6];
						dust2.velocity.Y = dust2.velocity.Y - (float)this._rand.Next(5, 61) * 0.1f;
					}
					if (this._rand.Next(3) == 0)
					{
						Main.dust[num6].velocity *= 0.5f;
					}
					Main.dust[num6].velocity /= Main.dust[num6].scale;
				}
			}
			if (typeCache == 244 && tileFrameX == 18 && tileFrameY == 18 && this._rand.Next(2) == 0)
			{
				if (this._rand.Next(500) == 0)
				{
					Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default(Vector2), 415, (float)this._rand.Next(51, 101) * 0.01f);
				}
				else if (this._rand.Next(250) == 0)
				{
					Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default(Vector2), 414, (float)this._rand.Next(51, 101) * 0.01f);
				}
				else if (this._rand.Next(80) == 0)
				{
					Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default(Vector2), 413, (float)this._rand.Next(51, 101) * 0.01f);
				}
				else if (this._rand.Next(10) == 0)
				{
					Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default(Vector2), 412, (float)this._rand.Next(51, 101) * 0.01f);
				}
				else if (this._rand.Next(3) == 0)
				{
					Gore.NewGore(new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8)), default(Vector2), 411, (float)this._rand.Next(51, 101) * 0.01f);
				}
			}
			if (typeCache == 565 && tileFrameX == 0 && tileFrameY == 18 && this._rand.Next(3) == 0)
			{
				Vector2 value = new Point(i, j).ToWorldCoordinates(8f, 8f);
				int type = 1202;
				float scale = 8f + Main.rand.NextFloat() * 1.6f;
				Vector2 position = value + new Vector2(0f, -18f);
				Vector2 vector = Main.rand.NextVector2Circular(0.7f, 0.25f) * 0.4f + Main.rand.NextVector2CircularEdge(1f, 0.4f) * 0.1f;
				vector *= 4f;
				Gore.NewGorePerfect(position, vector, type, scale);
			}
			if (typeCache == 215 && tileFrameY < 36 && this._rand.Next(3) == 0 && tileFrameY == 0)
			{
				int num7 = Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16 - 4)), 4, 8, 31, 0f, 0f, 100, default(Color), 1f);
				if (tileFrameX == 0)
				{
					Dust dust3 = this._dust[num7];
					dust3.position.X = dust3.position.X + (float)this._rand.Next(8);
				}
				if (tileFrameX == 36)
				{
					Dust dust4 = this._dust[num7];
					dust4.position.X = dust4.position.X - (float)this._rand.Next(8);
				}
				this._dust[num7].alpha += this._rand.Next(100);
				this._dust[num7].velocity *= 0.2f;
				Dust dust5 = this._dust[num7];
				dust5.velocity.Y = dust5.velocity.Y - (0.5f + (float)this._rand.Next(10) * 0.1f);
				this._dust[num7].fadeIn = 0.5f + (float)this._rand.Next(10) * 0.1f;
			}
			if (typeCache == 592 && tileFrameY == 18 && this._rand.Next(3) == 0)
			{
				int num8 = Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16 + 4)), 4, 8, 31, 0f, 0f, 100, default(Color), 1f);
				if (tileFrameX == 0)
				{
					Dust dust6 = this._dust[num8];
					dust6.position.X = dust6.position.X + (float)this._rand.Next(8);
				}
				if (tileFrameX == 36)
				{
					Dust dust7 = this._dust[num8];
					dust7.position.X = dust7.position.X - (float)this._rand.Next(8);
				}
				this._dust[num8].alpha += this._rand.Next(100);
				this._dust[num8].velocity *= 0.2f;
				Dust dust8 = this._dust[num8];
				dust8.velocity.Y = dust8.velocity.Y - (0.5f + (float)this._rand.Next(10) * 0.1f);
				this._dust[num8].fadeIn = 0.5f + (float)this._rand.Next(10) * 0.1f;
			}
			else if (typeCache == 406 && tileFrameY == 54 && tileFrameX == 0 && this._rand.Next(3) == 0)
			{
				Vector2 position2 = new Vector2((float)(i * 16 + 16), (float)(j * 16 + 8));
				Vector2 velocity = new Vector2(0f, 0f);
				if (Main.WindForVisuals < 0f)
				{
					velocity.X = -Main.WindForVisuals;
				}
				int type2 = this._rand.Next(825, 828);
				if (this._rand.Next(4) == 0)
				{
					Gore.NewGore(position2, velocity, type2, this._rand.NextFloat() * 0.2f + 0.2f);
				}
				else if (this._rand.Next(2) == 0)
				{
					Gore.NewGore(position2, velocity, type2, this._rand.NextFloat() * 0.3f + 0.3f);
				}
				else
				{
					Gore.NewGore(position2, velocity, type2, this._rand.NextFloat() * 0.4f + 0.4f);
				}
			}
			else if (typeCache == 452 && tileFrameY == 0 && tileFrameX == 0 && this._rand.Next(3) == 0)
			{
				Vector2 position3 = new Vector2((float)(i * 16 + 16), (float)(j * 16 + 8));
				Vector2 velocity2 = new Vector2(0f, 0f);
				if (Main.WindForVisuals < 0f)
				{
					velocity2.X = -Main.WindForVisuals;
				}
				int num9 = Main.tileFrame[(int)typeCache];
				int type3 = 907 + num9 / 5;
				if (this._rand.Next(2) == 0)
				{
					Gore.NewGore(position3, velocity2, type3, this._rand.NextFloat() * 0.4f + 0.4f);
				}
			}
			if (typeCache == 192 && this._rand.Next(num) == 0)
			{
				this.EmitLivingTreeLeaf(i, j, 910);
			}
			if (typeCache == 384 && this._rand.Next(num) == 0)
			{
				this.EmitLivingTreeLeaf(i, j, 914);
			}
			if ((typeCache == 666 || typeCache == 712) && tileCache.liquid <= 0 && j - 1 > 0 && this._rand.Next(100) == 0 && !WorldGen.ActiveAndWalkableTile(i, j - 1) && !WorldGen.AnyLiquidAt(i, j - 1, -1))
			{
				ParticleOrchestrator.RequestParticleSpawn(true, ParticleOrchestraType.PooFly, new ParticleOrchestraSettings
				{
					PositionInWorld = new Vector2((float)(i * 16 + 8), (float)(j * 16 - 8))
				}, null);
			}
			if (typeCache == 711 && tileFrameX == 0 && tileFrameY == 0)
			{
				if (this._rand.Next(45) == 0)
				{
					ParticleOrchestrator.RequestParticleSpawn(true, ParticleOrchestraType.RainbowBoulder3, new ParticleOrchestraSettings
					{
						PositionInWorld = new Vector2((float)(i * 16 + 16), (float)(j * 16 + 16))
					}, null);
				}
				if (this._rand.Next(3) != 0)
				{
					ParticleOrchestrator.RequestParticleSpawn(true, ParticleOrchestraType.RainbowBoulder2, new ParticleOrchestraSettings
					{
						PositionInWorld = new Vector2((float)(i * 16 + 16), (float)(j * 16 + 16)) + this._rand.NextVector2Circular(16f, 16f),
						MovementVector = this._rand.NextVector2Circular(1f, 0.5f) * 0.5f
					}, null);
				}
			}
			if (TileID.Sets.SpawnsNatureFlies[(int)typeCache] && tileCache.liquid <= 0)
			{
				float num10 = Utils.GetLerpValue(0.08f, 0.18f, Math.Abs(Main.WindForVisuals), true);
				num10 += 0.3f;
				if (this._rand.NextFloat() < num10)
				{
					bool flag2 = this._rand.Next(600) == 0;
					if (!flag2)
					{
						int num11;
						int num12;
						this._windGrid.GetWindTime(i, j, 8, out num11, out num12, out num12);
						flag2 = (num11 > 0 && this._rand.Next(48) == 0);
					}
					if (flag2)
					{
						ParticleOrchestrator.RequestParticleSpawn(true, ParticleOrchestraType.NatureFly, new ParticleOrchestraSettings
						{
							PositionInWorld = new Vector2((float)(i * 16 + 8), (float)(j * 16))
						}, null);
					}
				}
			}
			if (this._rand.Next(1200) == 0)
			{
				bool flag3 = j + 1 < 0;
				bool flag4 = false;
				int num13 = 3;
				if ((double)j < Main.worldSurface)
				{
					if (this._rand.Next(10) != 0)
					{
						flag3 = true;
					}
					else
					{
						num13--;
						flag4 = true;
					}
				}
				if (!TileID.Sets.MakesRubbleDust[(int)typeCache])
				{
					flag3 = true;
				}
				if (!flag3 && WorldGen.ActiveAndWalkableTile(i, j + 1))
				{
					flag3 = true;
				}
				if (!flag3 && !WallID.Sets.AllowsWind[(int)Main.tile[i, j].wall])
				{
					if (this._rand.Next(2) == 0)
					{
						flag3 = true;
					}
					else
					{
						num13--;
					}
				}
				if (!flag3)
				{
					for (int k = 0; k < num13; k++)
					{
						int num14 = WorldGen.KillTile_MakeTileDust(i, j, tileCache);
						Dust dust9 = Main.dust[num14];
						Dust dust10 = dust9;
						dust10.position.Y = dust10.position.Y + 8f;
						dust9.velocity *= 0.1f;
						if (flag4)
						{
							dust9.scale -= 0.3f;
						}
					}
				}
			}
			if (!flag)
			{
				return;
			}
			if (typeCache == 238 && this._rand.Next(10) == 0)
			{
				int num15 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 168, 0f, 0f, 0, default(Color), 1f);
				this._dust[num15].noGravity = true;
				this._dust[num15].alpha = 200;
			}
			if (typeCache == 139)
			{
				if (tileCache.frameX == 36 && tileCache.frameY % 36 == 0 && (int)Main.timeForVisualEffects % 7 == 0 && this._rand.Next(3) == 0)
				{
					int num16 = this._rand.Next(570, 573);
					Vector2 position4 = new Vector2((float)(i * 16 + 8), (float)(j * 16 - 8));
					Vector2 velocity3 = new Vector2(Main.WindForVisuals * 2f, -0.5f);
					velocity3.X *= 1f + (float)this._rand.Next(-50, 51) * 0.01f;
					velocity3.Y *= 1f + (float)this._rand.Next(-50, 51) * 0.01f;
					if (num16 == 572)
					{
						position4.X -= 8f;
					}
					if (num16 == 571)
					{
						position4.X -= 4f;
					}
					Gore.NewGore(position4, velocity3, num16, 0.8f);
				}
			}
			else if (typeCache == 463)
			{
				if (tileFrameY == 54 && tileFrameX == 0)
				{
					for (int l = 0; l < 4; l++)
					{
						if (this._rand.Next(2) != 0)
						{
							Dust dust11 = Dust.NewDustDirect(new Vector2((float)(i * 16 + 4), (float)(j * 16)), 36, 8, 16, 0f, 0f, 0, default(Color), 1f);
							dust11.noGravity = true;
							dust11.alpha = 140;
							dust11.fadeIn = 1.2f;
							dust11.velocity = Vector2.Zero;
						}
					}
				}
				if (tileFrameY == 18 && (tileFrameX == 0 || tileFrameX == 36))
				{
					for (int m = 0; m < 1; m++)
					{
						if (this._rand.Next(13) == 0)
						{
							Dust dust12 = Dust.NewDustDirect(new Vector2((float)(i * 16), (float)(j * 16)), 8, 8, 274, 0f, 0f, 0, default(Color), 1f);
							dust12.position = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 8));
							dust12.position.X = dust12.position.X + (float)((tileFrameX == 36) ? 4 : -4);
							dust12.noGravity = true;
							dust12.alpha = 128;
							dust12.fadeIn = 1.2f;
							dust12.noLight = true;
							dust12.velocity = new Vector2(0f, this._rand.NextFloatDirection() * 1.2f);
						}
					}
				}
			}
			else if (typeCache == 497)
			{
				if (tileCache.frameY / 40 == 31 && tileCache.frameY % 40 == 0)
				{
					for (int n = 0; n < 1; n++)
					{
						if (this._rand.Next(10) == 0)
						{
							Dust dust13 = Dust.NewDustDirect(new Vector2((float)(i * 16), (float)(j * 16 + 8)), 16, 12, 43, 0f, 0f, 0, default(Color), 1f);
							dust13.noGravity = true;
							dust13.alpha = 254;
							dust13.color = Color.White;
							dust13.scale = 0.7f;
							dust13.velocity = Vector2.Zero;
							dust13.noLight = true;
						}
					}
				}
			}
			else if (typeCache == 165 && tileFrameX >= 162 && tileFrameX <= 214 && tileFrameY == 72)
			{
				if (this._rand.Next(60) == 0)
				{
					int num17 = Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16 + 6)), 8, 4, 153, 0f, 0f, 0, default(Color), 1f);
					this._dust[num17].scale -= (float)this._rand.Next(3) * 0.1f;
					this._dust[num17].velocity.Y = 0f;
					Dust dust14 = this._dust[num17];
					dust14.velocity.X = dust14.velocity.X * 0.05f;
					this._dust[num17].alpha = 100;
				}
			}
			else if (typeCache == 42 && tileFrameX == 0)
			{
				int num18 = (int)(tileFrameY / 36);
				if (tileFrameY / 18 % 2 == 1)
				{
					if (num18 == 7)
					{
						if (this._rand.Next(50) == 0)
						{
							int num19 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 + 4)), 8, 8, 58, 0f, 0f, 150, default(Color), 1f);
							this._dust[num19].velocity *= 0.5f;
						}
						if (this._rand.Next(100) == 0)
						{
							int num20 = Gore.NewGore(new Vector2((float)(i * 16 - 2), (float)(j * 16 - 4)), default(Vector2), this._rand.Next(16, 18), 1f);
							this._gore[num20].scale *= 0.7f;
							this._gore[num20].velocity *= 0.25f;
						}
					}
					else if (num18 == 29)
					{
						if (this._rand.Next(40) == 0)
						{
							int num21 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16)), 8, 8, 59, 0f, 0f, 100, default(Color), 1f);
							if (this._rand.Next(3) != 0)
							{
								this._dust[num21].noGravity = true;
							}
							this._dust[num21].velocity *= 0.3f;
							Dust dust15 = this._dust[num21];
							dust15.velocity.Y = dust15.velocity.Y - 1.5f;
						}
					}
					else if (num18 == 50)
					{
						if (this._rand.Next(10) == 0)
						{
							int num22 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16)), 8, 8, 57, 0f, 0f, 100, default(Color), 1f);
							if (this._rand.Next(3) != 0)
							{
								this._dust[num22].noGravity = true;
							}
							this._dust[num22].velocity *= 0.3f;
							Dust dust16 = this._dust[num22];
							dust16.velocity.Y = dust16.velocity.Y - 1.5f;
						}
					}
					else if (num18 == 51 && this._rand.Next(40) == 0)
					{
						int num23 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 + 2)), 4, 4, 242, 0f, 0f, 100, default(Color), 1f);
						if (this._rand.Next(3) != 0)
						{
							this._dust[num23].noGravity = true;
						}
						this._dust[num23].velocity *= 0.3f;
						Dust dust17 = this._dust[num23];
						dust17.velocity.Y = dust17.velocity.Y - 1.5f;
					}
				}
			}
			if (typeCache == 4 && this._rand.Next(40) == 0 && tileFrameX < 66)
			{
				int num24 = (int)MathHelper.Clamp((float)(tileCache.frameY / 22), 0f, (float)(TorchID.Count - 1));
				int num25 = TorchID.Dust[num24];
				int num26;
				if (tileFrameX == 22)
				{
					num26 = Dust.NewDust(new Vector2((float)(i * 16 + 6), (float)(j * 16)), 4, 4, num25, 0f, 0f, 100, default(Color), 1f);
				}
				else if (tileFrameX == 44)
				{
					num26 = Dust.NewDust(new Vector2((float)(i * 16 + 2), (float)(j * 16)), 4, 4, num25, 0f, 0f, 100, default(Color), 1f);
				}
				else
				{
					num26 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16)), 4, 4, num25, 0f, 0f, 100, default(Color), 1f);
				}
				if (this._rand.Next(3) != 0)
				{
					this._dust[num26].noGravity = true;
				}
				this._dust[num26].velocity *= 0.3f;
				Dust dust18 = this._dust[num26];
				dust18.velocity.Y = dust18.velocity.Y - 1.5f;
				if (num25 == 66)
				{
					this._dust[num26].color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
					this._dust[num26].noGravity = true;
				}
			}
			if (typeCache == 93 && this._rand.Next(40) == 0 && tileFrameX == 0)
			{
				int num27 = (int)(tileFrameY / 54);
				if (tileFrameY / 18 % 3 == 0)
				{
					int num28;
					if (num27 <= 20)
					{
						if (num27 != 0)
						{
							switch (num27)
							{
							case 6:
							case 7:
							case 8:
							case 10:
							case 14:
							case 15:
							case 16:
								break;
							case 9:
							case 11:
							case 12:
							case 13:
							case 17:
							case 18:
							case 19:
								goto IL_19D7;
							case 20:
								num28 = 59;
								goto IL_19DA;
							default:
								goto IL_19D7;
							}
						}
						num28 = 6;
						goto IL_19DA;
					}
					if (num27 == 44)
					{
						num28 = 57;
						goto IL_19DA;
					}
					if (num27 == 45)
					{
						num28 = 242;
						goto IL_19DA;
					}
					IL_19D7:
					num28 = -1;
					IL_19DA:
					if (num28 != -1)
					{
						int num29 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 + 2)), 4, 4, num28, 0f, 0f, 100, default(Color), 1f);
						if (this._rand.Next(3) != 0)
						{
							this._dust[num29].noGravity = true;
						}
						this._dust[num29].velocity *= 0.3f;
						Dust dust19 = this._dust[num29];
						dust19.velocity.Y = dust19.velocity.Y - 1.5f;
					}
				}
			}
			if (typeCache == 100 && this._rand.Next(40) == 0 && tileFrameX < 36)
			{
				int num30 = (int)(tileFrameY / 36);
				if (tileFrameY / 18 % 2 == 0)
				{
					int num31;
					if (num30 <= 20)
					{
						switch (num30)
						{
						case 0:
						case 5:
						case 7:
						case 8:
						case 10:
						case 12:
						case 14:
						case 15:
						case 16:
							num31 = 6;
							goto IL_1B2D;
						case 1:
						case 2:
						case 3:
						case 4:
						case 6:
						case 9:
						case 11:
						case 13:
							break;
						default:
							if (num30 == 20)
							{
								num31 = 59;
								goto IL_1B2D;
							}
							break;
						}
					}
					else
					{
						if (num30 == 44)
						{
							num31 = 57;
							goto IL_1B2D;
						}
						if (num30 == 45)
						{
							num31 = 242;
							goto IL_1B2D;
						}
					}
					num31 = -1;
					IL_1B2D:
					if (num31 != -1)
					{
						Vector2 position5;
						if (tileFrameX == 0)
						{
							if (this._rand.Next(3) == 0)
							{
								position5 = new Vector2((float)(i * 16 + 4), (float)(j * 16 + 2));
							}
							else
							{
								position5 = new Vector2((float)(i * 16 + 14), (float)(j * 16 + 2));
							}
						}
						else if (this._rand.Next(3) == 0)
						{
							position5 = new Vector2((float)(i * 16 + 6), (float)(j * 16 + 2));
						}
						else
						{
							position5 = new Vector2((float)(i * 16), (float)(j * 16 + 2));
						}
						int num32 = Dust.NewDust(position5, 4, 4, num31, 0f, 0f, 100, default(Color), 1f);
						if (this._rand.Next(3) != 0)
						{
							this._dust[num32].noGravity = true;
						}
						this._dust[num32].velocity *= 0.3f;
						Dust dust20 = this._dust[num32];
						dust20.velocity.Y = dust20.velocity.Y - 1.5f;
					}
				}
			}
			if (typeCache == 98 && this._rand.Next(40) == 0 && tileFrameY == 0 && tileFrameX == 0)
			{
				int num33 = Dust.NewDust(new Vector2((float)(i * 16 + 12), (float)(j * 16 + 2)), 4, 4, 6, 0f, 0f, 100, default(Color), 1f);
				if (this._rand.Next(3) != 0)
				{
					this._dust[num33].noGravity = true;
				}
				this._dust[num33].velocity *= 0.3f;
				Dust dust21 = this._dust[num33];
				dust21.velocity.Y = dust21.velocity.Y - 1.5f;
			}
			if (typeCache == 49 && tileFrameX == 0 && this._rand.Next(2) == 0)
			{
				int num34 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 - 4)), 4, 4, 172, 0f, 0f, 100, default(Color), 1f);
				if (this._rand.Next(3) == 0)
				{
					this._dust[num34].scale = 0.5f;
				}
				else
				{
					this._dust[num34].scale = 0.9f;
					this._dust[num34].noGravity = true;
				}
				this._dust[num34].velocity *= 0.3f;
				Dust dust22 = this._dust[num34];
				dust22.velocity.Y = dust22.velocity.Y - 1.5f;
			}
			if (typeCache == 372 && tileFrameX == 0 && this._rand.Next(2) == 0)
			{
				int num35 = Dust.NewDust(new Vector2((float)(i * 16 + 4), (float)(j * 16 - 4)), 4, 4, 242, 0f, 0f, 100, default(Color), 1f);
				if (this._rand.Next(3) == 0)
				{
					this._dust[num35].scale = 0.5f;
				}
				else
				{
					this._dust[num35].scale = 0.9f;
					this._dust[num35].noGravity = true;
				}
				this._dust[num35].velocity *= 0.3f;
				Dust dust23 = this._dust[num35];
				dust23.velocity.Y = dust23.velocity.Y - 1.5f;
			}
			if (typeCache == 646 && tileFrameX == 0)
			{
				this._rand.Next(2);
			}
			if (typeCache == 34 && this._rand.Next(40) == 0 && tileFrameX % 108 < 54)
			{
				int num36 = (int)(tileFrameY / 54);
				if (tileFrameX >= 108)
				{
					num36 += (int)(37 * (tileFrameX / 108));
				}
				int num37 = (int)(tileFrameX / 18 % 3);
				if (tileFrameY / 18 % 3 == 1 && num37 != 1)
				{
					int num38;
					if (num36 <= 21)
					{
						switch (num36)
						{
						case 0:
						case 1:
						case 2:
						case 3:
						case 4:
						case 5:
						case 12:
						case 13:
						case 16:
							break;
						case 6:
						case 7:
						case 8:
						case 9:
						case 10:
						case 11:
						case 14:
						case 15:
							goto IL_1FAA;
						default:
							if (num36 != 19 && num36 != 21)
							{
								goto IL_1FAA;
							}
							break;
						}
						num38 = 6;
						goto IL_1FAD;
					}
					if (num36 == 25)
					{
						num38 = 59;
						goto IL_1FAD;
					}
					if (num36 == 50)
					{
						num38 = 57;
						goto IL_1FAD;
					}
					if (num36 == 51)
					{
						num38 = 242;
						goto IL_1FAD;
					}
					IL_1FAA:
					num38 = -1;
					IL_1FAD:
					if (num38 != -1)
					{
						int num39 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16 + 2)), 14, 6, num38, 0f, 0f, 100, default(Color), 1f);
						if (this._rand.Next(3) != 0)
						{
							this._dust[num39].noGravity = true;
						}
						this._dust[num39].velocity *= 0.3f;
						Dust dust24 = this._dust[num39];
						dust24.velocity.Y = dust24.velocity.Y - 1.5f;
					}
				}
			}
			if (typeCache == 83)
			{
				int style = (int)(tileFrameX / 18);
				if (WorldGen.IsAlchemyPlantHarvestable(style, j))
				{
					this.EmitAlchemyHerbParticles(j, i, style);
				}
			}
			if (typeCache == 22 && this._rand.Next(400) == 0)
			{
				Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14, 0f, 0f, 0, default(Color), 1f);
				return;
			}
			if ((typeCache == 23 || typeCache == 24 || typeCache == 32) && this._rand.Next(500) == 0)
			{
				Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14, 0f, 0f, 0, default(Color), 1f);
				return;
			}
			if (typeCache == 25 && this._rand.Next(700) == 0)
			{
				Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14, 0f, 0f, 0, default(Color), 1f);
				return;
			}
			if (typeCache == 112 && this._rand.Next(700) == 0)
			{
				Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14, 0f, 0f, 0, default(Color), 1f);
				return;
			}
			if ((typeCache == 31 || typeCache == 696) && this._rand.Next(20) == 0)
			{
				if (tileFrameX >= 36)
				{
					int num40 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 5, 0f, 0f, 100, default(Color), 1f);
					this._dust[num40].velocity.Y = 0f;
					Dust dust25 = this._dust[num40];
					dust25.velocity.X = dust25.velocity.X * 0.3f;
					return;
				}
				Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14, 0f, 0f, 100, default(Color), 1f);
				return;
			}
			else if ((typeCache == 26 || typeCache == 695) && this._rand.Next(20) == 0)
			{
				if (tileFrameX >= 54)
				{
					int num41 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 5, 0f, 0f, 100, default(Color), 1f);
					this._dust[num41].scale = 1.5f;
					this._dust[num41].noGravity = true;
					this._dust[num41].velocity *= 0.75f;
					return;
				}
				Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14, 0f, 0f, 100, default(Color), 1f);
				return;
			}
			else
			{
				if ((typeCache == 71 || typeCache == 72) && tileCache.color() == 0 && this._rand.Next(500) == 0)
				{
					Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 41, 0f, 0f, 250, default(Color), 0.8f);
					return;
				}
				if ((typeCache == 17 || typeCache == 77 || typeCache == 133) && this._rand.Next(40) == 0)
				{
					if (tileFrameX == 18 & tileFrameY == 18)
					{
						int num42 = Dust.NewDust(new Vector2((float)(i * 16 - 4), (float)(j * 16 - 6)), 8, 6, 6, 0f, 0f, 100, default(Color), 1f);
						if (this._rand.Next(3) != 0)
						{
							this._dust[num42].noGravity = true;
							return;
						}
					}
				}
				else if (typeCache == 405 && this._rand.Next(20) == 0)
				{
					if (tileFrameX == 18 & tileFrameY == 18)
					{
						int num43 = Dust.NewDust(new Vector2((float)(i * 16 - 4), (float)(j * 16 - 6)), 24, 10, 6, 0f, 0f, 100, default(Color), 1f);
						if (this._rand.Next(5) != 0)
						{
							this._dust[num43].noGravity = true;
							return;
						}
					}
				}
				else if (typeCache == 37 && this._rand.Next(250) == 0)
				{
					int num44 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 6, 0f, 0f, 0, default(Color), (float)this._rand.Next(3));
					if (this._dust[num44].scale > 1f)
					{
						this._dust[num44].noGravity = true;
						return;
					}
				}
				else
				{
					if ((typeCache == 58 || typeCache == 76 || typeCache == 684) && this._rand.Next(250) == 0)
					{
						int num45 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 6, 0f, 0f, 0, default(Color), (float)this._rand.Next(3));
						if (this._dust[num45].scale > 1f)
						{
							this._dust[num45].noGravity = true;
						}
						this._dust[num45].noLight = true;
						return;
					}
					if (typeCache == 61 || typeCache == 703)
					{
						if (tileFrameX == 144 && this._rand.Next(60) == 0)
						{
							int num46 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 44, 0f, 0f, 250, default(Color), 0.4f);
							this._dust[num46].fadeIn = 0.7f;
							return;
						}
					}
					else if (Main.tileShine[(int)typeCache] > 0)
					{
						if (tileLight.R > 20 || tileLight.B > 20 || tileLight.G > 20)
						{
							int num47 = (int)tileLight.R;
							if ((int)tileLight.G > num47)
							{
								num47 = (int)tileLight.G;
							}
							if ((int)tileLight.B > num47)
							{
								num47 = (int)tileLight.B;
							}
							num47 /= 30;
							if (this._rand.Next(Main.tileShine[(int)typeCache]) < num47 && ((typeCache != 21 && typeCache != 441) || (tileFrameX >= 36 && tileFrameX < 180) || (tileFrameX >= 396 && tileFrameX <= 409)) && ((typeCache != 467 && typeCache != 468) || (tileFrameX >= 144 && tileFrameX < 180)))
							{
								Color white = Color.White;
								if (typeCache == 617)
								{
									int num48 = i;
									int num49 = j;
									WorldGen.GetTopLeftAndStyles(ref num48, ref num49, 3, 4, 18, 18);
									int num50 = num49;
									Tile tile = Main.tile[num48 + 1, num49 + 1];
									if (!this.IsVisible(tile))
									{
										num50 = num49 + 3;
									}
									if (j >= num50)
									{
										int num51 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 43, 0f, 0f, 254, white, 0.5f);
										this._dust[num51].velocity *= 0f;
										return;
									}
								}
								else
								{
									if (typeCache == 178)
									{
										int num52 = (int)(tileFrameX / 18);
										if (num52 == 0)
										{
											white = new Color(255, 0, 255, 255);
										}
										else if (num52 == 1)
										{
											white = new Color(255, 255, 0, 255);
										}
										else if (num52 == 2)
										{
											white = new Color(0, 0, 255, 255);
										}
										else if (num52 == 3)
										{
											white = new Color(0, 255, 0, 255);
										}
										else if (num52 == 4)
										{
											white = new Color(255, 0, 0, 255);
										}
										else if (num52 == 5)
										{
											white = new Color(255, 255, 255, 255);
										}
										else if (num52 == 6)
										{
											white = new Color(255, 255, 0, 255);
										}
										int num53 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 43, 0f, 0f, 254, white, 0.5f);
										this._dust[num53].velocity *= 0f;
										return;
									}
									if (typeCache == 63)
									{
										white = new Color(0, 0, 255, 255);
									}
									if (typeCache == 64)
									{
										white = new Color(255, 0, 0, 255);
									}
									if (typeCache == 65)
									{
										white = new Color(0, 255, 0, 255);
									}
									if (typeCache == 66)
									{
										white = new Color(255, 255, 0, 255);
									}
									if (typeCache == 67)
									{
										white = new Color(255, 0, 255, 255);
									}
									if (typeCache == 68)
									{
										white = new Color(255, 255, 255, 255);
									}
									if (typeCache == 566)
									{
										white = new Color(255, 255, 0, 255);
									}
									if (typeCache == 12 || typeCache == 665)
									{
										white = new Color(255, 0, 0, 255);
									}
									if (typeCache == 639)
									{
										white = new Color(0, 0, 255, 255);
									}
									if (typeCache == 204)
									{
										white = new Color(255, 0, 0, 255);
									}
									if (typeCache == 211)
									{
										white = new Color(50, 255, 100, 255);
									}
									int num54 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 43, 0f, 0f, 254, white, 0.5f);
									this._dust[num54].velocity *= 0f;
									return;
								}
							}
						}
					}
					else if (Main.tileSolid[(int)tileCache.type] && Main.shimmerAlpha > 0f && (tileLight.R > 20 || tileLight.B > 20 || tileLight.G > 20))
					{
						int num55 = (int)tileLight.R;
						if ((int)tileLight.G > num55)
						{
							num55 = (int)tileLight.G;
						}
						if ((int)tileLight.B > num55)
						{
							num55 = (int)tileLight.B;
						}
						int maxValue = 500;
						if ((float)this._rand.Next(maxValue) < 2f * Main.shimmerAlpha)
						{
							Color white2 = Color.White;
							float scale2 = ((float)num55 / 255f + 1f) / 2f;
							int num56 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 43, 0f, 0f, 254, white2, scale2);
							this._dust[num56].velocity *= 0f;
						}
					}
				}
				return;
			}
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x005DC365 File Offset: 0x005DA565
		private void EmitLivingTreeLeaf(int i, int j, int leafGoreType)
		{
			this.EmitLivingTreeLeaf_Below(i, j, leafGoreType);
			if (this._rand.Next(2) == 0)
			{
				this.EmitLivingTreeLeaf_Sideways(i, j, leafGoreType);
			}
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x005DC388 File Offset: 0x005DA588
		private void EmitLivingTreeLeaf_Below(int x, int y, int leafGoreType)
		{
			Tile tile = Main.tile[x, y + 1];
			if (WorldGen.SolidTile(tile) || tile.liquid > 0)
			{
				return;
			}
			float windForVisuals = Main.WindForVisuals;
			if (windForVisuals < -0.2f && (WorldGen.SolidTile(Main.tile[x - 1, y + 1]) || WorldGen.SolidTile(Main.tile[x - 2, y + 1])))
			{
				return;
			}
			if (windForVisuals > 0.2f && (WorldGen.SolidTile(Main.tile[x + 1, y + 1]) || WorldGen.SolidTile(Main.tile[x + 2, y + 1])))
			{
				return;
			}
			Gore.NewGorePerfect(new Vector2((float)(x * 16), (float)(y * 16 + 16)), Vector2.Zero, leafGoreType, 1f).Frame.CurrentColumn = Main.tile[x, y].color();
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x005DC468 File Offset: 0x005DA668
		private void EmitLivingTreeLeaf_Sideways(int x, int y, int leafGoreType)
		{
			int num = 0;
			if (Main.WindForVisuals > 0.2f)
			{
				num = 1;
			}
			else if (Main.WindForVisuals < -0.2f)
			{
				num = -1;
			}
			Tile tile = Main.tile[x + num, y];
			if (WorldGen.SolidTile(tile) || tile.liquid > 0)
			{
				return;
			}
			int num2 = 0;
			if (num == -1)
			{
				num2 = -10;
			}
			Gore.NewGorePerfect(new Vector2((float)(x * 16 + 8 + 4 * num + num2), (float)(y * 16 + 8)), Vector2.Zero, leafGoreType, 1f).Frame.CurrentColumn = Main.tile[x, y].color();
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x005DC504 File Offset: 0x005DA704
		private void EmitLiquidDrops(int j, int i, Tile tileCache, ushort typeCache)
		{
			int num = 60;
			if (typeCache == 374)
			{
				num = 120;
			}
			else if (typeCache == 375)
			{
				num = 180;
			}
			else if (typeCache == 461)
			{
				num = 180;
			}
			if (tileCache.liquid != 0 || this._rand.Next(num * 2) != 0)
			{
				return;
			}
			Rectangle rectangle = new Rectangle(i * 16, j * 16, 16, 16);
			rectangle.X -= 34;
			rectangle.Width += 68;
			rectangle.Y -= 100;
			rectangle.Height = 400;
			for (int k = 0; k < 600; k++)
			{
				Gore gore = this._gore[k];
				if (gore.active && gore.type >= 0 && gore.type < GoreID.Count && GoreID.Sets.IsDrip[gore.type])
				{
					Rectangle value = new Rectangle((int)gore.position.X, (int)gore.position.Y, 16, 16);
					if (rectangle.Intersects(value))
					{
						return;
					}
				}
			}
			Vector2 position = new Vector2((float)(i * 16), (float)(j * 16));
			int type = 706;
			if (Main.waterStyle == 14)
			{
				type = 706;
			}
			else if (Main.waterStyle == 13)
			{
				type = 706;
			}
			else if (Main.waterStyle == 12)
			{
				type = 1147;
			}
			else if (Main.waterStyle > 1)
			{
				type = 706 + Main.waterStyle - 1;
			}
			if (typeCache == 374)
			{
				type = 716;
			}
			if (typeCache == 375)
			{
				type = 717;
			}
			if (typeCache == 461)
			{
				type = 943;
				if (Main.SceneMetrics.ZoneCorrupt)
				{
					type = 1160;
				}
				if (Main.SceneMetrics.ZoneCrimson)
				{
					type = 1161;
				}
				if (Main.SceneMetrics.ZoneHallow)
				{
					type = 1162;
				}
			}
			if (typeCache == 709)
			{
				type = 1383;
			}
			int num2 = Gore.NewGore(position, default(Vector2), type, 1f);
			this._gore[num2].velocity *= 0f;
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x005DC724 File Offset: 0x005DA924
		private float GetWindCycle(int x, int y, double windCounter)
		{
			if (!Main.SettingsEnabled_TilesSwayInWind)
			{
				return 0f;
			}
			float num = (float)x * 0.5f + (float)(y / 100) * 0.5f;
			float num2 = (float)Math.Cos(windCounter * 6.2831854820251465 + (double)num) * 0.5f;
			if (Main.remixWorld)
			{
				if ((double)y <= Main.worldSurface)
				{
					return 0f;
				}
				num2 += Main.WindForVisuals;
			}
			else
			{
				if ((double)y >= Main.worldSurface)
				{
					return 0f;
				}
				num2 += Main.WindForVisuals;
			}
			float lerpValue = Utils.GetLerpValue(0.08f, 0.18f, Math.Abs(Main.WindForVisuals), true);
			return num2 * lerpValue;
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x005DC7C8 File Offset: 0x005DA9C8
		private bool ShouldSwayInWind(int x, int y, Tile tileCache)
		{
			return Main.SettingsEnabled_TilesSwayInWind && TileID.Sets.SwaysInWindBasic[(int)tileCache.type] && (tileCache.type != 227 || (tileCache.frameX != 204 && tileCache.frameX != 238 && tileCache.frameX != 408 && tileCache.frameX != 442 && tileCache.frameX != 476));
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x005DC840 File Offset: 0x005DAA40
		private void UpdateLeafFrequency()
		{
			float num = Math.Abs(Main.WindForVisuals);
			if (num <= 0.1f)
			{
				this._leafFrequency = 2000;
			}
			else if (num <= 0.2f)
			{
				this._leafFrequency = 1000;
			}
			else if (num <= 0.3f)
			{
				this._leafFrequency = 450;
			}
			else if (num <= 0.4f)
			{
				this._leafFrequency = 300;
			}
			else if (num <= 0.5f)
			{
				this._leafFrequency = 200;
			}
			else if (num <= 0.6f)
			{
				this._leafFrequency = 130;
			}
			else if (num <= 0.7f)
			{
				this._leafFrequency = 75;
			}
			else if (num <= 0.8f)
			{
				this._leafFrequency = 50;
			}
			else if (num <= 0.9f)
			{
				this._leafFrequency = 40;
			}
			else if (num <= 1f)
			{
				this._leafFrequency = 30;
			}
			else if (num <= 1.1f)
			{
				this._leafFrequency = 20;
			}
			else
			{
				this._leafFrequency = 10;
			}
			this._leafFrequency *= 7;
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x005DC954 File Offset: 0x005DAB54
		private void EnsureWindGridSize()
		{
			Vector2 vector;
			int num;
			int num2;
			int num3;
			int num4;
			TileDrawing.GetScreenDrawArea(!Main.drawToScreen, out vector, out num, out num2, out num3, out num4);
			this._windGrid.SetSize(num2 - num, num4 - num3);
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x005DC98C File Offset: 0x005DAB8C
		private void EmitTreeLeaves(int tilePosX, int tilePosY, int grassPosX, int grassPosY)
		{
			if (this._isActiveAndNotPaused)
			{
				int num = grassPosY - tilePosY;
				Tile tile = Main.tile[tilePosX, tilePosY];
				if (tile.liquid > 0)
				{
					return;
				}
				int num2;
				int num3;
				WorldGen.GetTreeLeaf(tilePosX, tile, Main.tile[grassPosX, grassPosY], ref num, out num2, out num3);
				if (num3 == -1 || num3 == 912 || num3 == 913 || num3 == 1278)
				{
					return;
				}
				bool flag = (num3 >= 917 && num3 <= 925) || (num3 >= 1113 && num3 <= 1121);
				int num4 = this._leafFrequency;
				bool flag2 = tilePosX - grassPosX != 0;
				if (flag)
				{
					num4 /= 2;
				}
				if (!WorldGen.DoesWindBlowAtThisHeight(tilePosY))
				{
					num4 = 10000;
				}
				if (flag2)
				{
					num4 *= 3;
				}
				if (this._rand.Next(num4) == 0)
				{
					int num5 = 2;
					Vector2 vector = new Vector2((float)(tilePosX * 16 + 8), (float)(tilePosY * 16 + 8));
					if (flag2)
					{
						int num6 = tilePosX - grassPosX;
						vector.X += (float)(num6 * 12);
						int num7 = 0;
						if (tile.frameY == 220)
						{
							num7 = 1;
						}
						else if (tile.frameY == 242)
						{
							num7 = 2;
						}
						if (tile.frameX == 66)
						{
							switch (num7)
							{
							case 0:
								vector += new Vector2(0f, -6f);
								break;
							case 1:
								vector += new Vector2(0f, -6f);
								break;
							case 2:
								vector += new Vector2(0f, 8f);
								break;
							}
						}
						else
						{
							switch (num7)
							{
							case 0:
								vector += new Vector2(0f, 4f);
								break;
							case 1:
								vector += new Vector2(2f, -6f);
								break;
							case 2:
								vector += new Vector2(6f, -6f);
								break;
							}
						}
					}
					else
					{
						vector += new Vector2(-16f, -16f);
						if (flag)
						{
							vector.Y -= (float)(Main.rand.Next(0, 28) * 4);
						}
					}
					if (!WorldGen.SolidTile(vector.ToTileCoordinates()))
					{
						Gore.NewGoreDirect(vector, Utils.RandomVector2(Main.rand, (float)(-(float)num5), (float)num5), num3, 0.7f + Main.rand.NextFloat() * 0.6f).Frame.CurrentColumn = Main.tile[tilePosX, tilePosY].color();
					}
				}
			}
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x005DCC34 File Offset: 0x005DAE34
		private void DrawSpecialTilesLegacy(Vector2 screenPosition, Vector2 offSet)
		{
			if (this._specialTilesCount == 0)
			{
				return;
			}
			base.RestartLayeredBatch();
			for (int i = 0; i < this._specialTilesCount; i++)
			{
				int num = this._specialTileX[i];
				int num2 = this._specialTileY[i];
				Tile tile = Main.tile[num, num2];
				ushort type = tile.type;
				short frameX = tile.frameX;
				short frameY = tile.frameY;
				Main.tileBatch.SetLayer(0U, 0);
				if (type == 237)
				{
					Main.tileBatch.Draw(TextureAssets.SunOrb.Value, new Vector2((float)(num * 16 - (int)screenPosition.X) + 8f, (float)(num2 * 16 - (int)screenPosition.Y - 36)) + offSet, new Rectangle?(new Rectangle(0, 0, TextureAssets.SunOrb.Width(), TextureAssets.SunOrb.Height())), new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, 0), new Vector2((float)(TextureAssets.SunOrb.Width() / 2), (float)(TextureAssets.SunOrb.Height() / 2)), 1f, SpriteEffects.None);
				}
				if (type == 334 && frameX >= 5000)
				{
					int j = (int)frameX;
					int num3 = 0;
					int num4 = j % 5000;
					num4 -= 100;
					while (j >= 5000)
					{
						num3++;
						j -= 5000;
					}
					int num5 = (int)Main.tile[num + 1, num2].frameX;
					if (num5 >= 25000)
					{
						num5 -= 25000;
					}
					else
					{
						num5 -= 10000;
					}
					Item item = new Item();
					item.netDefaults(num4);
					item.Prefix(num5);
					Main.instance.LoadItem(item.type);
					Texture2D value = TextureAssets.Item[item.type].Value;
					Rectangle rectangle;
					if (Main.itemAnimations[item.type] != null)
					{
						rectangle = Main.itemAnimations[item.type].GetFrame(value, -1);
					}
					else
					{
						rectangle = value.Frame(1, 1, 0, 0, 0, 0);
					}
					int width = rectangle.Width;
					int height = rectangle.Height;
					float num6 = 1f;
					if (width > 40 || height > 40)
					{
						if (width > height)
						{
							num6 = 40f / (float)width;
						}
						else
						{
							num6 = 40f / (float)height;
						}
					}
					num6 *= item.scale;
					SpriteEffects effects = SpriteEffects.None;
					if (num3 >= 3)
					{
						effects = SpriteEffects.FlipHorizontally;
					}
					Color color = Lighting.GetColor(num, num2);
					Main.tileBatch.Draw(value, new Vector2((float)(num * 16 - (int)screenPosition.X + 24), (float)(num2 * 16 - (int)screenPosition.Y + 8)) + offSet, new Rectangle?(rectangle), Lighting.GetColor(num, num2), new Vector2((float)(width / 2), (float)(height / 2)), num6, effects);
					if (item.color != default(Color))
					{
						Main.tileBatch.Draw(value, new Vector2((float)(num * 16 - (int)screenPosition.X + 24), (float)(num2 * 16 - (int)screenPosition.Y + 8)) + offSet, new Rectangle?(rectangle), item.GetColor(color), new Vector2((float)(width / 2), (float)(height / 2)), num6, effects);
					}
				}
				TEItemFrame teitemFrame;
				if (type == 395 && TileEntity.TryGetAt<TEItemFrame>(num, num2, out teitemFrame))
				{
					Item item2 = teitemFrame.item;
					if (!item2.IsAir)
					{
						Vector2 screenPositionForItemCenter = new Vector2((float)(num * 16 - (int)screenPosition.X + 16), (float)(num2 * 16 - (int)screenPosition.Y + 16)) + offSet;
						Color color2 = Lighting.GetColor(num, num2);
						ItemSlot.DrawItemIcon(item2, 40, Main.spriteBatch, screenPositionForItemCenter, item2.scale, 20f, color2, 1f, false);
					}
				}
				TEFoodPlatter tefoodPlatter;
				if (type == 520 && TileEntity.TryGetAt<TEFoodPlatter>(num, num2, out tefoodPlatter))
				{
					Item item3 = tefoodPlatter.item;
					if (!item3.IsAir)
					{
						Main.instance.LoadItem(item3.type);
						Texture2D value2 = TextureAssets.Item[item3.type].Value;
						Rectangle rectangle2;
						if (ItemID.Sets.IsFood[item3.type])
						{
							rectangle2 = value2.Frame(1, 3, 0, 2, 0, 0);
						}
						else
						{
							rectangle2 = value2.Frame(1, 1, 0, 0, 0, 0);
						}
						int width2 = rectangle2.Width;
						int height2 = rectangle2.Height;
						float num7 = 1f;
						SpriteEffects effects2 = (tile.frameX == 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
						Color color3 = Lighting.GetColor(num, num2);
						Color color4 = color3;
						float num8 = 1f;
						ItemSlot.GetItemLight(ref color4, ref num8, item3, false, 1f);
						num7 *= num8;
						Vector2 position = new Vector2((float)(num * 16 - (int)screenPosition.X + 8), (float)(num2 * 16 - (int)screenPosition.Y + 16)) + offSet;
						position.Y += 2f;
						Vector2 origin = new Vector2((float)(width2 / 2), (float)height2);
						Main.tileBatch.Draw(value2, position, new Rectangle?(rectangle2), color4, origin, num7, effects2);
						if (item3.color != default(Color))
						{
							Main.tileBatch.Draw(value2, position, new Rectangle?(rectangle2), item3.GetColor(color3), origin, num7, effects2);
						}
					}
				}
				TEWeaponsRack teweaponsRack;
				if (type == 471 && TileEntity.TryGetAt<TEWeaponsRack>(num, num2, out teweaponsRack))
				{
					Item item4 = teweaponsRack.item;
					if (!item4.IsAir)
					{
						Vector2 screenPositionForItemCenter2 = new Vector2((float)(num * 16 - (int)screenPosition.X + 24), (float)(num2 * 16 - (int)screenPosition.Y + 24)) + offSet;
						Color color5 = Lighting.GetColor(num, num2);
						bool flip = true;
						if (tile.frameX < 54)
						{
							flip = false;
						}
						ItemSlot.DrawItemIcon(item4, 40, Main.spriteBatch, screenPositionForItemCenter2, item4.scale, 40f, color5, 1f, flip);
					}
				}
				if (type == 620)
				{
					Texture2D value3 = TextureAssets.Extra[202].Value;
					int num9 = 2;
					Main.critterCage = true;
					int waterAnimalCageFrame = this.GetWaterAnimalCageFrame(num, num2, (int)frameX, (int)frameY);
					int num10 = 8;
					int num11 = Main.butterflyCageFrame[num10, waterAnimalCageFrame];
					int num12 = 6;
					float num13 = 1f;
					Rectangle value4 = new Rectangle(0, 34 * num11, 32, 32);
					Vector2 vector = new Vector2((float)(num * 16 - (int)screenPosition.X), (float)(num2 * 16 - (int)screenPosition.Y + num9)) + offSet;
					Main.tileBatch.Draw(value3, vector, new Rectangle?(value4), new Color(255, 255, 255, 255), Vector2.Zero, 1f, SpriteEffects.None);
					for (int k = 0; k < num12; k++)
					{
						Color color6 = new Color(127, 127, 127, 0).MultiplyRGBA(Main.hslToRgb((Main.GlobalTimeWrappedHourly + (float)k / (float)num12) % 1f, 1f, 0.5f, byte.MaxValue));
						color6 *= 1f - num13 * 0.5f;
						color6.A = 0;
						int num14 = 2;
						Vector2 position2 = vector + ((float)k / (float)num12 * 6.2831855f).ToRotationVector2() * ((float)num14 * num13 + 2f);
						Main.tileBatch.Draw(value3, position2, new Rectangle?(value4), color6, Vector2.Zero, 1f, SpriteEffects.None);
					}
					Main.tileBatch.Draw(value3, vector, new Rectangle?(value4), new Color(255, 255, 255, 0) * 0.1f, Vector2.Zero, 1f, SpriteEffects.None);
				}
			}
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x005DD3F0 File Offset: 0x005DB5F0
		private void DrawEntities_DisplayDolls()
		{
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
			foreach (KeyValuePair<Point, int> keyValuePair in this._displayDollTileEntityPositions)
			{
				TEDisplayDoll tedisplayDoll;
				if (keyValuePair.Value != -1 && TileEntity.TryGetAt<TEDisplayDoll>(keyValuePair.Key.X, keyValuePair.Key.Y, out tedisplayDoll))
				{
					tedisplayDoll.Draw(keyValuePair.Key.X, keyValuePair.Key.Y);
				}
			}
			Main.spriteBatch.End();
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x005DD4B4 File Offset: 0x005DB6B4
		private void DrawEntities_HatRacks()
		{
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
			foreach (KeyValuePair<Point, int> keyValuePair in this._hatRackTileEntityPositions)
			{
				TEHatRack tehatRack;
				if (keyValuePair.Value != -1 && TileEntity.TryGetAt<TEHatRack>(keyValuePair.Key.X, keyValuePair.Key.Y, out tehatRack))
				{
					tehatRack.Draw(keyValuePair.Key.X, keyValuePair.Key.Y);
				}
			}
			Main.spriteBatch.End();
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x005DD578 File Offset: 0x005DB778
		private void DrawTrees()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 0;
			int num2 = this._specialsCount[num];
			float num3 = 0.08f;
			float num4 = 0.06f;
			for (int i = 0; i < num2; i++)
			{
				Point point = this._specialPositions[num][i];
				int x = point.X;
				int y = point.Y;
				Tile tile = Main.tile[x, y];
				if (tile != null && tile.active())
				{
					ushort type = tile.type;
					short frameX = tile.frameX;
					short frameY = tile.frameY;
					bool flag = tile.wall > 0;
					WorldGen.GetTreeFoliageDataMethod getTreeFoliageDataMethod = null;
					try
					{
						bool flag2 = false;
						if (type <= 589)
						{
							if (type != 5)
							{
								if (type - 583 <= 6)
								{
									flag2 = true;
									getTreeFoliageDataMethod = new WorldGen.GetTreeFoliageDataMethod(WorldGen.GetGemTreeFoliageData);
								}
							}
							else
							{
								flag2 = true;
								getTreeFoliageDataMethod = new WorldGen.GetTreeFoliageDataMethod(WorldGen.GetCommonTreeFoliageData);
							}
						}
						else if (type != 596 && type != 616)
						{
							if (type == 634)
							{
								flag2 = true;
								getTreeFoliageDataMethod = new WorldGen.GetTreeFoliageDataMethod(WorldGen.GetAshTreeFoliageData);
							}
						}
						else
						{
							flag2 = true;
							getTreeFoliageDataMethod = new WorldGen.GetTreeFoliageDataMethod(WorldGen.GetVanityTreeFoliageData);
						}
						if (flag2 && frameY >= 198 && frameX >= 22)
						{
							int treeFrame = WorldGen.GetTreeFrame(tile);
							if (frameX == 22)
							{
								int num5 = 0;
								int num6 = 80;
								int num7 = 80;
								int num8 = 0;
								int grassPosX = x + num8;
								int grassPosY = y;
								if (!getTreeFoliageDataMethod(x, y, num8, ref treeFrame, ref num5, out grassPosY, out num6, out num7))
								{
									goto IL_9C6;
								}
								this.EmitTreeLeaves(x, y, grassPosX, grassPosY);
								if (num5 == 14)
								{
									float num9 = (float)this._rand.Next(28, 42) * 0.005f;
									num9 += (float)(270 - (int)Main.mouseTextColor) / 1000f;
									if (tile.color() == 0)
									{
										Lighting.AddLight(x, y, 0.1f, 0.2f + num9 / 2f, 0.7f + num9);
									}
									else
									{
										Color color = WorldGen.paintColor((int)tile.color());
										float r = (float)color.R / 255f;
										float g = (float)color.G / 255f;
										float b = (float)color.B / 255f;
										Lighting.AddLight(x, y, r, g, b);
									}
								}
								byte tileColor = tile.color();
								Texture2D treeTopTexture = this.GetTreeTopTexture(num5, 0, tileColor);
								Vector2 position = position = new Vector2((float)(x * 16 - (int)unscaledPosition.X + 8), (float)(y * 16 - (int)unscaledPosition.Y + 16)) + zero;
								float num10 = 0f;
								if (!flag)
								{
									num10 = this.GetWindCycle(x, y, this._treeWindCounter);
								}
								position.X += num10 * 2f;
								position.Y += Math.Abs(num10) * 2f;
								Color color2 = Lighting.GetColor(x, y);
								if (tile.fullbrightBlock())
								{
									color2 = Color.White;
								}
								this.DrawNature(treeTopTexture, position, new Rectangle(treeFrame * (num6 + 2), 0, num6, num7), color2, num10 * num3, new Vector2((float)(num6 / 2), (float)num7), 1f, SpriteEffects.None, 0f, SideFlags.None);
								if (type == 634)
								{
									Texture2D value = TextureAssets.GlowMask[316].Value;
									Color white = Color.White;
									this.DrawNatureGlowmask(value, position, new Rectangle?(new Rectangle(treeFrame * (num6 + 2), 0, num6, num7)), white, num10 * num3, new Vector2((float)(num6 / 2), (float)num7), 1f, SpriteEffects.None, 0f);
								}
							}
							else if (frameX == 44)
							{
								int num11 = 0;
								int num12 = x;
								int grassPosY2 = y;
								int num13 = 1;
								int num14;
								int num15;
								if (!getTreeFoliageDataMethod(x, y, num13, ref treeFrame, ref num11, out grassPosY2, out num14, out num15))
								{
									goto IL_9C6;
								}
								this.EmitTreeLeaves(x, y, num12 + num13, grassPosY2);
								if (num11 == 14)
								{
									float num16 = (float)this._rand.Next(28, 42) * 0.005f;
									num16 += (float)(270 - (int)Main.mouseTextColor) / 1000f;
									if (tile.color() == 0)
									{
										Lighting.AddLight(x, y, 0.1f, 0.2f + num16 / 2f, 0.7f + num16);
									}
									else
									{
										Color color3 = WorldGen.paintColor((int)tile.color());
										float r2 = (float)color3.R / 255f;
										float g2 = (float)color3.G / 255f;
										float b2 = (float)color3.B / 255f;
										Lighting.AddLight(x, y, r2, g2, b2);
									}
								}
								byte tileColor2 = tile.color();
								Texture2D treeBranchTexture = this.GetTreeBranchTexture(num11, 0, tileColor2);
								Vector2 position2 = new Vector2((float)(x * 16), (float)(y * 16)) - unscaledPosition.Floor() + zero + new Vector2(16f, 12f);
								float num17 = 0f;
								if (!flag)
								{
									num17 = this.GetWindCycle(x, y, this._treeWindCounter);
								}
								if (num17 > 0f)
								{
									position2.X += num17;
								}
								position2.X += Math.Abs(num17) * 2f;
								Color color4 = Lighting.GetColor(x, y);
								if (tile.fullbrightBlock())
								{
									color4 = Color.White;
								}
								this.DrawNature(treeBranchTexture, position2, new Rectangle(0, treeFrame * 42, 40, 40), color4, num17 * num4, new Vector2(40f, 24f), 1f, SpriteEffects.None, 0f, SideFlags.None);
								if (type == 634)
								{
									Texture2D value2 = TextureAssets.GlowMask[317].Value;
									Color white2 = Color.White;
									this.DrawNatureGlowmask(value2, position2, new Rectangle?(new Rectangle(0, treeFrame * 42, 40, 40)), white2, num17 * num4, new Vector2(40f, 24f), 1f, SpriteEffects.None, 0f);
								}
							}
							else if (frameX == 66)
							{
								int num18 = 0;
								int num19 = x;
								int grassPosY3 = y;
								int num20 = -1;
								int num21;
								int num22;
								if (!getTreeFoliageDataMethod(x, y, num20, ref treeFrame, ref num18, out grassPosY3, out num21, out num22))
								{
									goto IL_9C6;
								}
								this.EmitTreeLeaves(x, y, num19 + num20, grassPosY3);
								if (num18 == 14)
								{
									float num23 = (float)this._rand.Next(28, 42) * 0.005f;
									num23 += (float)(270 - (int)Main.mouseTextColor) / 1000f;
									if (tile.color() == 0)
									{
										Lighting.AddLight(x, y, 0.1f, 0.2f + num23 / 2f, 0.7f + num23);
									}
									else
									{
										Color color5 = WorldGen.paintColor((int)tile.color());
										float r3 = (float)color5.R / 255f;
										float g3 = (float)color5.G / 255f;
										float b3 = (float)color5.B / 255f;
										Lighting.AddLight(x, y, r3, g3, b3);
									}
								}
								byte tileColor3 = tile.color();
								Texture2D treeBranchTexture2 = this.GetTreeBranchTexture(num18, 0, tileColor3);
								Vector2 position3 = new Vector2((float)(x * 16), (float)(y * 16)) - unscaledPosition.Floor() + zero + new Vector2(0f, 18f);
								float num24 = 0f;
								if (!flag)
								{
									num24 = this.GetWindCycle(x, y, this._treeWindCounter);
								}
								if (num24 < 0f)
								{
									position3.X += num24;
								}
								position3.X -= Math.Abs(num24) * 2f;
								Color color6 = Lighting.GetColor(x, y);
								if (tile.fullbrightBlock())
								{
									color6 = Color.White;
								}
								this.DrawNature(treeBranchTexture2, position3, new Rectangle(42, treeFrame * 42, 40, 40), color6, num24 * num4, new Vector2(0f, 30f), 1f, SpriteEffects.None, 0f, SideFlags.None);
								if (type == 634)
								{
									Texture2D value3 = TextureAssets.GlowMask[317].Value;
									Color white3 = Color.White;
									this.DrawNatureGlowmask(value3, position3, new Rectangle?(new Rectangle(42, treeFrame * 42, 40, 40)), white3, num24 * num4, new Vector2(0f, 30f), 1f, SpriteEffects.None, 0f);
								}
							}
						}
						if (type == 323 && frameX >= 88 && frameX <= 132)
						{
							int num25 = 0;
							if (frameX == 110)
							{
								num25 = 1;
							}
							else if (frameX == 132)
							{
								num25 = 2;
							}
							int treeTextureIndex = 15;
							int num26 = 80;
							int num27 = 80;
							int num28 = 32;
							int num29 = 0;
							int palmTreeBiome = this.GetPalmTreeBiome(x, y);
							int y2 = palmTreeBiome * 82;
							if (palmTreeBiome >= 4 && palmTreeBiome <= 7)
							{
								treeTextureIndex = 21;
								num26 = 114;
								num27 = 98;
								y2 = (palmTreeBiome - 4) * 98;
								num28 = 48;
								num29 = 2;
							}
							int frameY2 = (int)Main.tile[x, y].frameY;
							byte tileColor4 = tile.color();
							Texture2D treeTopTexture2 = this.GetTreeTopTexture(treeTextureIndex, palmTreeBiome, tileColor4);
							Vector2 position4 = new Vector2((float)(x * 16 - (int)unscaledPosition.X - num28 + frameY2 + num26 / 2), (float)(y * 16 - (int)unscaledPosition.Y + 16 + num29)) + zero;
							float num30 = 0f;
							if (!flag)
							{
								num30 = this.GetWindCycle(x, y, this._treeWindCounter);
							}
							position4.X += num30 * 2f;
							position4.Y += Math.Abs(num30) * 2f;
							Color color7 = Lighting.GetColor(x, y);
							if (tile.fullbrightBlock())
							{
								color7 = Color.White;
							}
							this.DrawNature(treeTopTexture2, position4, new Rectangle(num25 * (num26 + 2), y2, num26, num27), color7, num30 * num3, new Vector2((float)(num26 / 2), (float)num27), 1f, SpriteEffects.None, 0f, SideFlags.None);
						}
					}
					catch
					{
					}
				}
				IL_9C6:;
			}
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x005DDF78 File Offset: 0x005DC178
		private Texture2D GetTreeTopTexture(int treeTextureIndex, int treeTextureStyle, byte tileColor)
		{
			Texture2D texture2D = this._paintSystem.TryGetTreeTopAndRequestIfNotReady(treeTextureIndex, treeTextureStyle, (int)tileColor);
			if (texture2D == null)
			{
				texture2D = TextureAssets.TreeTop[treeTextureIndex].Value;
			}
			return texture2D;
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x005DDFA8 File Offset: 0x005DC1A8
		private Texture2D GetTreeBranchTexture(int treeTextureIndex, int treeTextureStyle, byte tileColor)
		{
			Texture2D texture2D = this._paintSystem.TryGetTreeBranchAndRequestIfNotReady(treeTextureIndex, treeTextureStyle, (int)tileColor);
			if (texture2D == null)
			{
				texture2D = TextureAssets.TreeBranch[treeTextureIndex].Value;
			}
			return texture2D;
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x005DDFD8 File Offset: 0x005DC1D8
		private void DrawGrass()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 1;
			int num2 = this._specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point point = this._specialPositions[num][i];
				int x = point.X;
				int y = point.Y;
				Tile tile = Main.tile[x, y];
				if (tile != null && tile.active() && this.IsVisible(tile))
				{
					ushort num3 = tile.type;
					short frameX = tile.frameX;
					short frameY = tile.frameY;
					int num4;
					int num5;
					int num6;
					int num7;
					int num8;
					int num9;
					SpriteEffects effects;
					Texture2D texture2D;
					Rectangle value;
					Color color;
					this.GetTileDrawData(x, y, tile, num3, ref frameX, ref frameY, out num4, out num5, out num6, out num7, out num8, out num9, out effects, out texture2D, out value, out color);
					bool flag = this._rand.Next(4) == 0;
					Color color2 = Lighting.GetColor(x, y);
					this.DrawAnimatedTile_AdjustForVisionChangers(x, y, tile, num3, frameX, frameY, ref color2, flag);
					color2 = this.DrawTiles_GetLightOverride(y, x, tile, num3, frameX, frameY, color2);
					if (this._isActiveAndNotPaused && flag)
					{
						this.DrawTiles_EmitParticles(y, x, tile, num3, frameX, frameY, color2);
					}
					if (num3 == 83 && WorldGen.IsAlchemyPlantHarvestable((int)(frameX / 18), y))
					{
						num3 = 84;
						Main.instance.LoadTiles((int)num3);
					}
					Vector2 position = new Vector2((float)(x * 16 - (int)unscaledPosition.X + 8), (float)(y * 16 - (int)unscaledPosition.Y + 16)) + zero;
					float num10 = this.GetWindCycle(x, y, this._grassWindCounter);
					if (!WallID.Sets.AllowsWind[(int)tile.wall])
					{
						num10 = 0f;
					}
					if (!this.InAPlaceWithWind(x, y, 1, 1))
					{
						num10 = 0f;
					}
					num10 += this.GetWindGridPush(x, y, 20, 0.35f);
					position.X += num10 * 1f;
					position.Y += Math.Abs(num10) * 1f;
					Texture2D tileDrawTexture = this.GetTileDrawTexture(tile, x, y);
					if (tileDrawTexture != null)
					{
						this.DrawNature(tileDrawTexture, position, new Rectangle((int)frameX + num8, (int)frameY + num9, num4, num5 - num7), color2, num10 * 0.1f, new Vector2((float)(num4 / 2), (float)(16 - num7 - num6)), 1f, effects, 0f, SideFlags.None);
						if (texture2D != null)
						{
							this.DrawNatureGlowmask(texture2D, position, new Rectangle?(value), color, num10 * 0.1f, new Vector2((float)(num4 / 2), (float)(16 - num7 - num6)), 1f, effects, 0f);
						}
					}
				}
			}
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x005DE27C File Offset: 0x005DC47C
		private void DrawAnyDirectionalGrass()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 10;
			int num2 = this._specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point point = this._specialPositions[num][i];
				int x = point.X;
				int y = point.Y;
				Tile tile = Main.tile[x, y];
				if (tile != null && tile.active() && this.IsVisible(tile))
				{
					ushort num3 = tile.type;
					short frameX = tile.frameX;
					short frameY = tile.frameY;
					int num4;
					int num5;
					int num6;
					int num7;
					int num8;
					int num9;
					SpriteEffects effects;
					Texture2D texture2D;
					Rectangle rectangle;
					Color color;
					this.GetTileDrawData(x, y, tile, num3, ref frameX, ref frameY, out num4, out num5, out num6, out num7, out num8, out num9, out effects, out texture2D, out rectangle, out color);
					bool flag = this._rand.Next(4) == 0;
					Color color2 = Lighting.GetColor(x, y);
					this.DrawAnimatedTile_AdjustForVisionChangers(x, y, tile, num3, frameX, frameY, ref color2, flag);
					color2 = this.DrawTiles_GetLightOverride(y, x, tile, num3, frameX, frameY, color2);
					if (this._isActiveAndNotPaused && flag)
					{
						this.DrawTiles_EmitParticles(y, x, tile, num3, frameX, frameY, color2);
					}
					if (num3 == 83 && WorldGen.IsAlchemyPlantHarvestable((int)(frameX / 18), y))
					{
						num3 = 84;
						Main.instance.LoadTiles((int)num3);
					}
					Vector2 position = new Vector2((float)(x * 16 - (int)unscaledPosition.X), (float)(y * 16 - (int)unscaledPosition.Y)) + zero;
					float num10 = this.GetWindCycle(x, y, this._grassWindCounter);
					if (!WallID.Sets.AllowsWind[(int)tile.wall])
					{
						num10 = 0f;
					}
					if (!this.InAPlaceWithWind(x, y, 1, 1))
					{
						num10 = 0f;
					}
					float num11;
					float num12;
					this.GetWindGridPush2Axis(x, y, 20, 0.35f, out num11, out num12);
					int num13 = 1;
					int num14 = 0;
					Vector2 origin = new Vector2((float)(num4 / 2), (float)(16 - num7 - num6));
					switch (frameY / 54)
					{
					case 0:
						num13 = 1;
						num14 = 0;
						origin = new Vector2((float)(num4 / 2), (float)(16 - num7 - num6));
						position.X += 8f;
						position.Y += 16f;
						position.X += num10;
						position.Y += Math.Abs(num10);
						break;
					case 1:
						num10 *= -1f;
						num13 = -1;
						num14 = 0;
						origin = new Vector2((float)(num4 / 2), (float)(-(float)num6));
						position.X += 8f;
						position.X += -num10;
						position.Y += -Math.Abs(num10);
						break;
					case 2:
						num13 = 0;
						num14 = 1;
						origin = new Vector2(2f, (float)((16 - num7 - num6) / 2));
						position.Y += 8f;
						position.Y += num10;
						position.X += -Math.Abs(num10);
						break;
					case 3:
						num10 *= -1f;
						num13 = 0;
						num14 = -1;
						origin = new Vector2(14f, (float)((16 - num7 - num6) / 2));
						position.X += 16f;
						position.Y += 8f;
						position.Y += -num10;
						position.X += Math.Abs(num10);
						break;
					}
					num10 += num11 * (float)num13 + num12 * (float)num14;
					Texture2D tileDrawTexture = this.GetTileDrawTexture(tile, x, y);
					if (tileDrawTexture != null)
					{
						this.DrawNature(tileDrawTexture, position, new Rectangle((int)frameX + num8, (int)frameY + num9, num4, num5 - num7), color2, num10 * 0.1f, origin, 1f, effects, 0f, SideFlags.None);
						if (texture2D != null)
						{
							this.DrawNatureGlowmask(texture2D, position, new Rectangle?(new Rectangle((int)frameX + num8, (int)frameY + num9, num4, num5 - num7)), color, num10 * 0.1f, origin, 1f, effects, 0f);
						}
					}
				}
			}
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x005DE698 File Offset: 0x005DC898
		private void DrawAnimatedTile_AdjustForVisionChangers(int i, int j, Tile tileCache, ushort typeCache, short tileFrameX, short tileFrameY, ref Color tileLight, bool canDoDust)
		{
			if (this._perspectivePlayer.dangerSense && TileDrawing.IsTileDangerous(this._perspectivePlayer, tileCache, typeCache))
			{
				if (tileLight.R < 255)
				{
					tileLight.R = byte.MaxValue;
				}
				if (tileLight.G < 50)
				{
					tileLight.G = 50;
				}
				if (tileLight.B < 50)
				{
					tileLight.B = 50;
				}
				if (this._isActiveAndNotPaused && canDoDust && this._rand.Next(30) == 0)
				{
					int num = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 60, 0f, 0f, 100, default(Color), 0.3f);
					this._dust[num].fadeIn = 1f;
					this._dust[num].velocity *= 0.1f;
					this._dust[num].noLight = true;
					this._dust[num].noGravity = true;
				}
			}
			if (this._perspectivePlayer.findTreasure && Main.IsTileSpelunkable(typeCache, tileFrameX, tileFrameY))
			{
				if (tileLight.R < 200)
				{
					tileLight.R = 200;
				}
				if (tileLight.G < 170)
				{
					tileLight.G = 170;
				}
				if (this._isActiveAndNotPaused && (this._rand.Next(60) == 0 && canDoDust))
				{
					int num2 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 204, 0f, 0f, 150, default(Color), 0.3f);
					this._dust[num2].fadeIn = 1f;
					this._dust[num2].velocity *= 0.1f;
					this._dust[num2].noLight = true;
				}
			}
			if (this._perspectivePlayer.biomeSight)
			{
				Color white = Color.White;
				if (Main.IsTileBiomeSightable(typeCache, tileFrameX, tileFrameY, ref white))
				{
					if (tileLight.R < white.R)
					{
						tileLight.R = white.R;
					}
					if (tileLight.G < white.G)
					{
						tileLight.G = white.G;
					}
					if (tileLight.B < white.B)
					{
						tileLight.B = white.B;
					}
					if (this._isActiveAndNotPaused && canDoDust && this._rand.Next(480) == 0)
					{
						Color newColor = white;
						int num3 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 267, 0f, 0f, 150, newColor, 0.3f);
						this._dust[num3].noGravity = true;
						this._dust[num3].fadeIn = 1f;
						this._dust[num3].velocity *= 0.1f;
						this._dust[num3].noLightEmittence = true;
					}
				}
			}
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x005DE9C4 File Offset: 0x005DCBC4
		private float GetWindGridPush(int i, int j, int pushAnimationTimeTotal, float pushForcePerFrame)
		{
			int num;
			int num2;
			int num3;
			this._windGrid.GetWindTime(i, j, pushAnimationTimeTotal, out num, out num2, out num3);
			if (num >= pushAnimationTimeTotal / 2)
			{
				return (float)(pushAnimationTimeTotal - num) * pushForcePerFrame * (float)num2;
			}
			return (float)num * pushForcePerFrame * (float)num2;
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x005DEA00 File Offset: 0x005DCC00
		private void GetWindGridPush2Axis(int i, int j, int pushAnimationTimeTotal, float pushForcePerFrame, out float pushX, out float pushY)
		{
			int num;
			int num2;
			int num3;
			this._windGrid.GetWindTime(i, j, pushAnimationTimeTotal, out num, out num2, out num3);
			if (num >= pushAnimationTimeTotal / 2)
			{
				pushX = (float)(pushAnimationTimeTotal - num) * pushForcePerFrame * (float)num2;
				pushY = (float)(pushAnimationTimeTotal - num) * pushForcePerFrame * (float)num3;
				return;
			}
			pushX = (float)num * pushForcePerFrame * (float)num2;
			pushY = (float)num * pushForcePerFrame * (float)num3;
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x005DEA58 File Offset: 0x005DCC58
		private float GetWindGridPushComplex(int i, int j, int pushAnimationTimeTotal, float totalPushForce, int loops, bool flipDirectionPerLoop)
		{
			int num;
			int num2;
			int num3;
			this._windGrid.GetWindTime(i, j, pushAnimationTimeTotal, out num, out num2, out num3);
			float num4 = (float)num / (float)pushAnimationTimeTotal;
			int num5 = (int)(num4 * (float)loops);
			float num6 = num4 * (float)loops % 1f;
			float num7 = 1f / (float)loops;
			if (flipDirectionPerLoop && num5 % 2 == 1)
			{
				num2 *= -1;
			}
			if (num4 * (float)loops % 1f > 0.5f)
			{
				return (1f - num6) * totalPushForce * (float)num2 * (float)(loops - num5);
			}
			return num6 * totalPushForce * (float)num2 * (float)(loops - num5);
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x005DEAE0 File Offset: 0x005DCCE0
		private void DrawMasterTrophies()
		{
			int num = 9;
			int num2 = this._specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point point = this._specialPositions[num][i];
				Tile tile = Main.tile[point.X, point.Y];
				if (tile != null && tile.active())
				{
					Tile tile2 = Main.tile[point.X + 1, point.Y + 1];
					if (tile2 != null && tile2.active() && this.IsVisible(tile2))
					{
						Texture2D value = TextureAssets.Extra[198].Value;
						int frameY = (int)(tile.frameX / 54);
						bool flag = tile.frameY / 72 != 0;
						int horizontalFrames = 1;
						int verticalFrames = 28;
						Rectangle rectangle = value.Frame(horizontalFrames, verticalFrames, 0, frameY, 0, 0);
						Vector2 origin = rectangle.Size() / 2f;
						Vector2 value2 = point.ToWorldCoordinates(24f, 64f);
						float num3 = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.2831855f / 5f));
						Vector2 value3 = value2 + new Vector2(0f, -40f) + new Vector2(0f, num3 * 4f);
						Color color = Lighting.GetColor(point.X, point.Y);
						if (tile2.fullbrightBlock())
						{
							color = Color.White;
						}
						SpriteEffects effects = flag ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
						Main.spriteBatch.Draw(value, value3 - Main.screenPosition, new Rectangle?(rectangle), color, 0f, origin, 1f, effects, 0f);
						float scale = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.2831855f / 2f)) * 0.3f + 0.7f;
						Color color2 = color;
						color2.A = 0;
						color2 = color2 * 0.1f * scale;
						for (float num4 = 0f; num4 < 1f; num4 += 0.16666667f)
						{
							Main.spriteBatch.Draw(value, value3 - Main.screenPosition + (6.2831855f * num4).ToRotationVector2() * (6f + num3 * 2f), new Rectangle?(rectangle), color2, 0f, origin, 1f, effects, 0f);
						}
					}
				}
			}
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x005DED4C File Offset: 0x005DCF4C
		private void DrawTeleportationPylons()
		{
			int num = 8;
			int num2 = this._specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point point = this._specialPositions[num][i];
				Tile tile = Main.tile[point.X, point.Y];
				if (tile != null && tile.active())
				{
					Tile tile2 = Main.tile[point.X + 1, point.Y + 1];
					if (tile2 != null && tile2.active() && this.IsVisible(tile2))
					{
						Texture2D value = TextureAssets.Extra[181].Value;
						int num3 = (int)(tile.frameX / 54);
						int num4 = 3;
						int horizontalFrames = num4 + 11;
						int verticalFrames = 8;
						int frameY = (Main.tileFrameCounter[597] + point.X + point.Y) % 64 / 8;
						Rectangle rectangle = value.Frame(horizontalFrames, verticalFrames, num4 + num3, frameY, 0, 0);
						Rectangle value2 = value.Frame(horizontalFrames, verticalFrames, 2, frameY, 0, 0);
						value.Frame(horizontalFrames, verticalFrames, 0, frameY, 0, 0);
						Vector2 origin = rectangle.Size() / 2f;
						Vector2 value3 = point.ToWorldCoordinates(24f, 64f);
						float num5 = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.2831855f / 5f));
						Vector2 vector = value3 + new Vector2(0f, -40f) + new Vector2(0f, num5 * 4f);
						bool flag = this._rand.Next(4) == 0;
						if (this._isActiveAndNotPaused && flag && this._rand.Next(10) == 0)
						{
							Rectangle dustBox = Utils.CenteredRectangle(vector, rectangle.Size());
							TeleportPylonsSystem.SpawnInWorldDust(num3, dustBox);
						}
						Color color = Lighting.GetColor(point.X, point.Y);
						if (tile2.fullbrightBlock())
						{
							color = Color.White;
						}
						color = Color.Lerp(color, Color.White, 0.8f);
						Main.spriteBatch.Draw(value, vector - Main.screenPosition, new Rectangle?(rectangle), color * 0.7f, 0f, origin, 1f, SpriteEffects.None, 0f);
						float scale = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.2831855f / 1f)) * 0.2f + 0.8f;
						Color color2 = new Color(255, 255, 255, 0) * 0.1f * scale;
						for (float num6 = 0f; num6 < 1f; num6 += 0.16666667f)
						{
							Main.spriteBatch.Draw(value, vector - Main.screenPosition + (6.2831855f * num6).ToRotationVector2() * (6f + num5 * 2f), new Rectangle?(rectangle), color2, 0f, origin, 1f, SpriteEffects.None, 0f);
						}
						int num7 = 0;
						bool flag2;
						if (Main.InSmartCursorHighlightArea(point.X, point.Y, out flag2))
						{
							num7 = 1;
							if (flag2)
							{
								num7 = 2;
							}
						}
						if (num7 != 0)
						{
							int num8 = (int)((color.R + color.G + color.B) / 3);
							if (num8 > 10)
							{
								Color selectionGlowColor = Colors.GetSelectionGlowColor(num7 == 2, num8);
								Main.spriteBatch.Draw(value, vector - Main.screenPosition, new Rectangle?(value2), selectionGlowColor, 0f, origin, 1f, SpriteEffects.None, 0f);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x005DF0DC File Offset: 0x005DD2DC
		private void DrawVoidLenses()
		{
			int num = 6;
			int num2 = this._specialsCount[num];
			this._voidLensData.Clear();
			for (int i = 0; i < num2; i++)
			{
				Point point = this._specialPositions[num][i];
				VoidLensHelper voidLensHelper = new VoidLensHelper(point.ToWorldCoordinates(8f, 8f), 1f);
				if (!Main.gamePaused)
				{
					voidLensHelper.Update();
				}
				int selectionMode = 0;
				bool flag;
				if (Main.InSmartCursorHighlightArea(point.X, point.Y, out flag))
				{
					selectionMode = 1;
					if (flag)
					{
						selectionMode = 2;
					}
				}
				voidLensHelper.DrawToDrawData(this._voidLensData, selectionMode);
			}
			foreach (DrawData drawData in this._voidLensData)
			{
				drawData.Draw(Main.spriteBatch);
			}
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x005DF1C4 File Offset: 0x005DD3C4
		private void DrawMultiTileGrass()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 2;
			int num2 = this._specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point point = this._specialPositions[num][i];
				int x = point.X;
				int num3 = point.Y;
				int sizeX = 1;
				int num4 = 1;
				Tile tile = Main.tile[x, num3];
				if (tile != null && tile.active())
				{
					ushort type = Main.tile[x, num3].type;
					if (type <= 485)
					{
						if (type <= 233)
						{
							if (type != 27)
							{
								if (type == 233)
								{
									if (Main.tile[x, num3].frameY == 0)
									{
										sizeX = 3;
									}
									else
									{
										sizeX = 2;
									}
									num4 = 2;
								}
							}
							else
							{
								sizeX = 2;
								num4 = 5;
							}
						}
						else
						{
							if (type == 236 || type == 238)
							{
								goto IL_16F;
							}
							if (type == 485)
							{
								goto IL_1A1;
							}
						}
					}
					else
					{
						if (type <= 651)
						{
							switch (type)
							{
							case 489:
								sizeX = 2;
								num4 = 3;
								goto IL_1D1;
							case 490:
								goto IL_1A1;
							case 491:
							case 492:
								goto IL_1D1;
							case 493:
								sizeX = 1;
								num4 = 2;
								goto IL_1D1;
							default:
								switch (type)
								{
								case 519:
									sizeX = 1;
									num4 = this.ClimbCatTail(x, num3);
									num3 -= num4 - 1;
									goto IL_1D1;
								case 520:
								case 528:
								case 529:
									goto IL_1D1;
								case 521:
								case 522:
								case 523:
								case 524:
								case 525:
								case 526:
								case 527:
									goto IL_1A1;
								case 530:
									break;
								default:
									if (type != 651)
									{
										goto IL_1D1;
									}
									break;
								}
								break;
							}
						}
						else
						{
							if (type == 652)
							{
								goto IL_1A1;
							}
							if (type == 702)
							{
								goto IL_16F;
							}
							if (type != 705)
							{
								goto IL_1D1;
							}
						}
						sizeX = 3;
						num4 = 2;
					}
					IL_1D1:
					this.DrawMultiTileGrassInWind(unscaledPosition, zero, x, num3, sizeX, num4);
					goto IL_1E1;
					IL_16F:
					num4 = (sizeX = 2);
					goto IL_1D1;
					IL_1A1:
					sizeX = 2;
					num4 = 2;
					goto IL_1D1;
				}
				IL_1E1:;
			}
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x005DF3C0 File Offset: 0x005DD5C0
		private int ClimbCatTail(int originx, int originy)
		{
			int num = 0;
			int i = originy;
			while (i > 10)
			{
				Tile tile = Main.tile[originx, i];
				if (!tile.active() || tile.type != 519)
				{
					break;
				}
				if (tile.frameX >= 180)
				{
					num++;
					break;
				}
				i--;
				num++;
			}
			return num;
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x005DF418 File Offset: 0x005DD618
		private void DrawMultiTileVines()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 3;
			int num2 = this._specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point point = this._specialPositions[num][i];
				int x = point.X;
				int y = point.Y;
				int sizeX = 1;
				int sizeY = 1;
				Tile tile = Main.tile[x, y];
				if (tile != null && tile.active())
				{
					ushort type = Main.tile[x, y].type;
					if (type <= 444)
					{
						if (type <= 91)
						{
							if (type != 34)
							{
								if (type == 42)
								{
									goto IL_14E;
								}
								if (type == 91)
								{
									sizeX = 1;
									sizeY = 3;
								}
							}
							else
							{
								sizeX = 3;
								sizeY = 3;
							}
						}
						else
						{
							if (type <= 126)
							{
								if (type != 95 && type != 126)
								{
									goto IL_174;
								}
							}
							else
							{
								if (type - 270 <= 1)
								{
									goto IL_14E;
								}
								if (type != 444)
								{
									goto IL_174;
								}
							}
							sizeX = 2;
							sizeY = 2;
						}
					}
					else
					{
						if (type <= 572)
						{
							if (type == 454)
							{
								sizeX = 4;
								sizeY = 3;
								goto IL_174;
							}
							if (type != 465)
							{
								if (type != 572)
								{
									goto IL_174;
								}
								goto IL_14E;
							}
						}
						else if (type <= 592)
						{
							if (type == 581)
							{
								goto IL_14E;
							}
							if (type - 591 > 1)
							{
								goto IL_174;
							}
						}
						else
						{
							if (type == 660)
							{
								goto IL_14E;
							}
							if (type != 698)
							{
								goto IL_174;
							}
							sizeX = 1;
							sizeY = 1;
							goto IL_174;
						}
						sizeX = 2;
						sizeY = 3;
					}
					IL_174:
					this.DrawMultiTileVinesInWind(unscaledPosition, zero, x, y, sizeX, sizeY);
					goto IL_184;
					IL_14E:
					sizeX = 1;
					sizeY = 2;
					goto IL_174;
				}
				IL_184:;
			}
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x005DF5B8 File Offset: 0x005DD7B8
		private void DrawVines()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 4;
			int num2 = this._specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point point = this._specialPositions[num][i];
				int x = point.X;
				int y = point.Y;
				this.DrawVineStrip(unscaledPosition, zero, x, y);
			}
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x005DF61C File Offset: 0x005DD81C
		private void DrawReverseVines()
		{
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 7;
			int num2 = this._specialsCount[num];
			for (int i = 0; i < num2; i++)
			{
				Point point = this._specialPositions[num][i];
				int x = point.X;
				int y = point.Y;
				this.DrawRisingVineStrip(unscaledPosition, zero, x, y);
			}
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x005DF680 File Offset: 0x005DD880
		private void DrawMultiTileGrassInWind(Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY)
		{
			float windCycle = this.GetWindCycle(topLeftX, topLeftY, this._sunflowerWindCounter);
			Vector2 value = new Vector2((float)(topLeftX * 16 - (int)screenPosition.X) + (float)sizeX * 16f * 0.5f, (float)(topLeftY * 16 - (int)screenPosition.Y + 16 * sizeY)) + offSet;
			float num = 0.07f;
			int type = (int)Main.tile[topLeftX, topLeftY].type;
			Texture2D texture2D = null;
			Color color = Color.Transparent;
			bool flag = this.InAPlaceWithWind(topLeftX, topLeftY, sizeX, sizeY);
			bool flag2 = false;
			int num2 = 0;
			if (type != 27)
			{
				if (type != 519)
				{
					if (type - 521 > 6)
					{
						num = 0.15f;
					}
					else
					{
						num = 0f;
						flag = false;
					}
				}
				else
				{
					flag = this.InAPlaceWithWind(topLeftX, topLeftY, sizeX, 1);
				}
			}
			else
			{
				color = Color.White;
				flag2 = true;
				num2 = 74;
			}
			for (int i = topLeftX; i < topLeftX + sizeX; i++)
			{
				for (int j = topLeftY; j < topLeftY + sizeY; j++)
				{
					Tile tile = Main.tile[i, j];
					ushort type2 = tile.type;
					if ((int)type2 == type && this.IsVisible(tile))
					{
						Math.Abs(((float)(i - topLeftX) + 0.5f) / (float)sizeX - 0.5f);
						short frameX = tile.frameX;
						short frameY = tile.frameY;
						float num3 = 1f - (float)(j - topLeftY + 1) / (float)sizeY;
						if (num3 == 0f)
						{
							num3 = 0.1f;
						}
						if (!flag)
						{
							num3 = 0f;
						}
						int width;
						int num4;
						int num5;
						int num6;
						int num7;
						int num8;
						SpriteEffects spriteEffects;
						Texture2D texture2D2;
						Rectangle rectangle;
						Color color2;
						this.GetTileDrawData(i, j, tile, type2, ref frameX, ref frameY, out width, out num4, out num5, out num6, out num7, out num8, out spriteEffects, out texture2D2, out rectangle, out color2);
						bool flag3 = this._rand.Next(4) == 0;
						Color color3 = Lighting.GetColor(i, j);
						this.DrawAnimatedTile_AdjustForVisionChangers(i, j, tile, type2, frameX, frameY, ref color3, flag3);
						color3 = this.DrawTiles_GetLightOverride(j, i, tile, type2, frameX, frameY, color3);
						if (this._isActiveAndNotPaused && flag3)
						{
							this.DrawTiles_EmitParticles(j, i, tile, type2, frameX, frameY, color3);
						}
						Vector2 value2 = new Vector2((float)(i * 16 - (int)screenPosition.X), (float)(j * 16 - (int)screenPosition.Y + num5)) + offSet;
						if (tile.type == 493 && tile.frameY == 0)
						{
							if (Main.WindForVisuals >= 0f)
							{
								spriteEffects ^= SpriteEffects.FlipHorizontally;
							}
							if ((spriteEffects & SpriteEffects.FlipHorizontally) == SpriteEffects.None)
							{
								value2.X -= 6f;
							}
							else
							{
								value2.X += 6f;
							}
						}
						Vector2 vector = new Vector2(windCycle * 1f, Math.Abs(windCycle) * 2f * num3);
						Vector2 origin = value - value2;
						Texture2D tileDrawTexture = this.GetTileDrawTexture(tile, i, j);
						if (tileDrawTexture != null)
						{
							if (flag2)
							{
								texture2D = tileDrawTexture;
							}
							SideFlags sideFlags = SideFlags.None;
							if (i > topLeftX)
							{
								sideFlags |= SideFlags.Left;
							}
							if (i < topLeftX + sizeX - 1)
							{
								sideFlags |= SideFlags.Right;
							}
							if (j > topLeftY)
							{
								sideFlags |= SideFlags.Top;
							}
							if (j < topLeftY + sizeY - 1)
							{
								sideFlags |= SideFlags.Bottom;
							}
							this.DrawNature(tileDrawTexture, value + new Vector2(0f, vector.Y), new Rectangle((int)frameX + num7, (int)frameY + num8, width, num4 - num6), color3, windCycle * num * num3, origin, 1f, spriteEffects, 0f, sideFlags);
							if (texture2D != null)
							{
								this.DrawNatureGlowmask(texture2D, value + new Vector2(0f, vector.Y), new Rectangle?(new Rectangle((int)frameX + num7, (int)frameY + num8 + num2, width, num4 - num6)), color, windCycle * num * num3, origin, 1f, spriteEffects, 0f);
							}
						}
					}
				}
			}
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x005DFA48 File Offset: 0x005DDC48
		private void DrawVineStrip(Vector2 screenPosition, Vector2 offSet, int x, int startY)
		{
			int num = 0;
			int num2 = 0;
			Vector2 vector = new Vector2((float)(x * 16 + 8), (float)(startY * 16 - 2));
			float num3 = Math.Abs(Main.WindForVisuals) / 1.2f;
			num3 = MathHelper.Lerp(0.2f, 1f, num3);
			float num4 = -0.08f * num3;
			float windCycle = this.GetWindCycle(x, startY, this._vineWindCounter);
			float num5 = 0f;
			float num6 = 0f;
			for (int i = startY; i < Main.maxTilesY - 10; i++)
			{
				Tile tile = Main.tile[x, i];
				if (tile == null)
				{
					break;
				}
				ushort type = tile.type;
				if (!tile.active() || !TileID.Sets.VineThreads[(int)type])
				{
					break;
				}
				num++;
				if (num2 >= 5)
				{
					num4 += 0.0075f * num3;
				}
				if (num2 >= 2)
				{
					num4 += 0.0025f;
				}
				if (Main.remixWorld)
				{
					if (WallID.Sets.AllowsWind[(int)tile.wall] && (double)i > Main.worldSurface)
					{
						num2++;
					}
				}
				else if (WallID.Sets.AllowsWind[(int)tile.wall] && (double)i < Main.worldSurface)
				{
					num2++;
				}
				float windGridPush = this.GetWindGridPush(x, i, 20, 0.01f);
				if (windGridPush == 0f && num6 != 0f)
				{
					num5 *= -0.78f;
				}
				else
				{
					num5 -= windGridPush;
				}
				num6 = windGridPush;
				short frameX = tile.frameX;
				short frameY = tile.frameY;
				Color color = Lighting.GetColor(x, i);
				int num7;
				int num8;
				int num9;
				int num10;
				int num11;
				int num12;
				SpriteEffects effects;
				Texture2D texture2D;
				Rectangle value;
				Color color2;
				this.GetTileDrawData(x, i, tile, type, ref frameX, ref frameY, out num7, out num8, out num9, out num10, out num11, out num12, out effects, out texture2D, out value, out color2);
				Vector2 position = new Vector2((float)(-(float)((int)screenPosition.X)), (float)(-(float)((int)screenPosition.Y))) + offSet + vector;
				if (tile.fullbrightBlock())
				{
					color = Color.White;
				}
				float num13 = (float)num2 * num4 * windCycle + num5;
				if (this._perspectivePlayer.biomeSight)
				{
					Color white = Color.White;
					if (Main.IsTileBiomeSightable(type, frameX, frameY, ref white))
					{
						if (color.R < white.R)
						{
							color.R = white.R;
						}
						if (color.G < white.G)
						{
							color.G = white.G;
						}
						if (color.B < white.B)
						{
							color.B = white.B;
						}
						if (this._isActiveAndNotPaused && this._rand.Next(480) == 0)
						{
							Color newColor = white;
							int num14 = Dust.NewDust(new Vector2((float)(x * 16), (float)(i * 16)), 16, 16, 267, 0f, 0f, 150, newColor, 0.3f);
							this._dust[num14].noGravity = true;
							this._dust[num14].fadeIn = 1f;
							this._dust[num14].velocity *= 0.1f;
							this._dust[num14].noLightEmittence = true;
						}
					}
				}
				Texture2D tileDrawTexture = this.GetTileDrawTexture(tile, x, i);
				if (tileDrawTexture == null)
				{
					return;
				}
				if (this.IsVisible(tile))
				{
					Tile tile2 = Main.tile[x, i + 1];
					bool flag = tile2.active() && TileID.Sets.VineThreads[(int)tile2.type];
					this.DrawNature(tileDrawTexture, position, new Rectangle((int)frameX + num11, (int)frameY + num12, num7, num8 - num10), color, num13, new Vector2((float)(num7 / 2), (float)(num10 - num9)), 1f, effects, 0f, flag ? SideFlags.Bottom : SideFlags.None);
					if (texture2D != null)
					{
						this.DrawNatureGlowmask(texture2D, position, new Rectangle?(value), color2, num13, new Vector2((float)(num7 / 2), (float)(num10 - num9)), 1f, effects, 0f);
					}
				}
				vector += (num13 + 1.5707964f).ToRotationVector2() * 16f;
			}
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x005DFE44 File Offset: 0x005DE044
		private void DrawRisingVineStrip(Vector2 screenPosition, Vector2 offSet, int x, int startY)
		{
			int num = 0;
			int num2 = 0;
			Vector2 vector = new Vector2((float)(x * 16 + 8), (float)(startY * 16 + 16 + 2));
			float num3 = Math.Abs(Main.WindForVisuals) / 1.2f;
			num3 = MathHelper.Lerp(0.2f, 1f, num3);
			float num4 = -0.08f * num3;
			float windCycle = this.GetWindCycle(x, startY, this._vineWindCounter);
			float num5 = 0f;
			float num6 = 0f;
			for (int i = startY; i > 10; i--)
			{
				Tile tile = Main.tile[x, i];
				if (tile != null)
				{
					ushort type = tile.type;
					if (!tile.active() || !TileID.Sets.ReverseVineThreads[(int)type])
					{
						break;
					}
					num++;
					if (num2 >= 5)
					{
						num4 += 0.0075f * num3;
					}
					if (num2 >= 2)
					{
						num4 += 0.0025f;
					}
					if (WallID.Sets.AllowsWind[(int)tile.wall] && (double)i < Main.worldSurface)
					{
						num2++;
					}
					float windGridPush = this.GetWindGridPush(x, i, 40, -0.004f);
					if (windGridPush == 0f && num6 != 0f)
					{
						num5 *= -0.78f;
					}
					else
					{
						num5 -= windGridPush;
					}
					num6 = windGridPush;
					short frameX = tile.frameX;
					short frameY = tile.frameY;
					Color color = Lighting.GetColor(x, i);
					int num7;
					int num8;
					int num9;
					int num10;
					int num11;
					int num12;
					SpriteEffects effects;
					Texture2D texture2D;
					Rectangle rectangle;
					Color color2;
					this.GetTileDrawData(x, i, tile, type, ref frameX, ref frameY, out num7, out num8, out num9, out num10, out num11, out num12, out effects, out texture2D, out rectangle, out color2);
					Vector2 position = new Vector2((float)(-(float)((int)screenPosition.X)), (float)(-(float)((int)screenPosition.Y))) + offSet + vector;
					if (tile.fullbrightBlock())
					{
						color = Color.White;
					}
					float num13 = (float)num2 * -num4 * windCycle + num5;
					Texture2D tileDrawTexture = this.GetTileDrawTexture(tile, x, i);
					if (tileDrawTexture == null)
					{
						return;
					}
					if (this.IsVisible(tile))
					{
						Tile tile2 = Main.tile[x, i - 1];
						bool flag = tile2.active() && TileID.Sets.ReverseVineThreads[(int)tile2.type];
						this.DrawNature(tileDrawTexture, position, new Rectangle((int)frameX + num11, (int)frameY + num12, num7, num8 - num10), color, num13, new Vector2((float)(num7 / 2), (float)(num10 - num9 + num8)), 1f, effects, 0f, flag ? SideFlags.Top : SideFlags.None);
					}
					vector += (num13 - 1.5707964f).ToRotationVector2() * 16f;
				}
			}
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x005E00B8 File Offset: 0x005DE2B8
		private float GetAverageWindGridPush(int topLeftX, int topLeftY, int sizeX, int sizeY, int totalPushTime, float pushForcePerFrame)
		{
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					float windGridPush = this.GetWindGridPush(topLeftX + i, topLeftY + j, totalPushTime, pushForcePerFrame);
					if (windGridPush != 0f)
					{
						num += windGridPush;
						num2++;
					}
				}
			}
			if (num2 == 0)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x005E0118 File Offset: 0x005DE318
		private float GetHighestWindGridPushComplex(int topLeftX, int topLeftY, int sizeX, int sizeY, int totalPushTime, float pushForcePerFrame, int loops, bool swapLoopDir)
		{
			float result = 0f;
			int num = int.MaxValue;
			for (int i = 0; i < 1; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					int num2;
					int num3;
					int num4;
					this._windGrid.GetWindTime(topLeftX + i + sizeX / 2, topLeftY + j, totalPushTime, out num2, out num3, out num4);
					float windGridPushComplex = this.GetWindGridPushComplex(topLeftX + i, topLeftY + j, totalPushTime, pushForcePerFrame, loops, swapLoopDir);
					if (num2 < num && num2 != 0)
					{
						result = windGridPushComplex;
						num = num2;
					}
				}
			}
			return result;
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x005E0190 File Offset: 0x005DE390
		private void DrawMultiTileVinesInWind(Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY)
		{
			float num = this.GetWindCycle(topLeftX, topLeftY, this._sunflowerWindCounter);
			float num2 = num;
			int totalPushTime = 60;
			float pushForcePerFrame = 1.26f;
			float highestWindGridPushComplex = this.GetHighestWindGridPushComplex(topLeftX, topLeftY, sizeX, sizeY, totalPushTime, pushForcePerFrame, 3, true);
			num += highestWindGridPushComplex;
			Vector2 value = new Vector2((float)(topLeftX * 16 - (int)screenPosition.X) + (float)sizeX * 16f * 0.5f, (float)(topLeftY * 16 - (int)screenPosition.Y)) + offSet;
			Tile tile = Main.tile[topLeftX, topLeftY];
			int type = (int)tile.type;
			Vector2 value2 = new Vector2(0f, -2f);
			value += value2;
			bool flag;
			if (type == 465 || type - 591 <= 1)
			{
				flag = (WorldGen.IsBelowANonHammeredPlatform(topLeftX, topLeftY) && WorldGen.IsBelowANonHammeredPlatform(topLeftX + 1, topLeftY));
			}
			else
			{
				flag = (sizeX == 1 && WorldGen.IsBelowANonHammeredPlatform(topLeftX, topLeftY));
			}
			if (flag)
			{
				value.Y -= 8f;
				value2.Y -= 8f;
			}
			Texture2D texture2D = null;
			Color transparent = Color.Transparent;
			float? num3 = null;
			float num4 = 1f;
			float num5 = -4f;
			bool flag2 = false;
			bool flag3 = false;
			float num6 = 0.15f;
			if (type <= 444)
			{
				int num7;
				if (type <= 95)
				{
					if (type != 34)
					{
						if (type == 42)
						{
							num3 = new float?((float)1);
							num5 = 0f;
							num7 = (int)(tile.frameY / 36);
							if (num7 <= 55)
							{
								if (num7 == 0)
								{
									num3 = null;
									num5 = -1f;
									goto IL_6F9;
								}
								switch (num7)
								{
								case 9:
									num3 = new float?(0f);
									goto IL_6F9;
								case 10:
								case 11:
								case 13:
								case 15:
								case 16:
								case 17:
								case 18:
								case 19:
								case 20:
								case 21:
								case 22:
								case 23:
								case 24:
								case 25:
								case 26:
								case 27:
								case 29:
								case 31:
								case 36:
								case 37:
									goto IL_6F9;
								case 12:
									num3 = null;
									num5 = -1f;
									goto IL_6F9;
								case 14:
									num3 = null;
									num5 = -1f;
									goto IL_6F9;
								case 28:
									num3 = null;
									num5 = -1f;
									goto IL_6F9;
								case 30:
									num3 = new float?(0f);
									goto IL_6F9;
								case 32:
									num3 = new float?(0f);
									goto IL_6F9;
								case 33:
									num3 = new float?(0f);
									goto IL_6F9;
								case 34:
									num3 = null;
									num5 = -1f;
									goto IL_6F9;
								case 35:
									num3 = new float?(0f);
									goto IL_6F9;
								case 38:
									num3 = null;
									num5 = -1f;
									goto IL_6F9;
								case 39:
									num3 = null;
									num5 = -1f;
									flag2 = true;
									goto IL_6F9;
								case 40:
								case 41:
								case 42:
								case 43:
									num3 = new float?(0f);
									num3 = null;
									num5 = -1f;
									flag2 = true;
									goto IL_6F9;
								default:
									if (num7 - 54 > 1)
									{
										goto IL_6F9;
									}
									break;
								}
							}
							else if (num7 <= 65)
							{
								if (num7 != 60 && num7 != 65)
								{
									goto IL_6F9;
								}
							}
							else if (num7 != 67 && num7 != 70)
							{
								goto IL_6F9;
							}
							num3 = new float?(0f);
							goto IL_6F9;
						}
						if (type != 95)
						{
							goto IL_6F9;
						}
						goto IL_67D;
					}
				}
				else if (type != 126)
				{
					if (type - 270 > 1 && type != 444)
					{
						goto IL_6F9;
					}
					goto IL_67D;
				}
				num3 = new float?((float)1);
				num5 = 0f;
				num7 = (int)(tile.frameY / 54 + tile.frameX / 108 * 37);
				if (num7 > 44)
				{
					if (num7 - 54 > 1 && num7 != 60)
					{
						switch (num7)
						{
						case 65:
						case 67:
						case 68:
						case 70:
							break;
						case 66:
						case 69:
							goto IL_6F9;
						default:
							goto IL_6F9;
						}
					}
					num3 = new float?(0f);
					goto IL_6F9;
				}
				switch (num7)
				{
				case 9:
					num3 = null;
					num5 = -1f;
					flag2 = true;
					num6 *= 0.3f;
					goto IL_6F9;
				case 10:
					goto IL_6F9;
				case 11:
					num6 *= 0.5f;
					goto IL_6F9;
				case 12:
					num3 = null;
					num5 = -1f;
					goto IL_6F9;
				default:
					switch (num7)
					{
					case 18:
						num3 = null;
						num5 = -1f;
						goto IL_6F9;
					case 19:
					case 20:
					case 22:
					case 24:
					case 26:
					case 27:
					case 28:
					case 29:
					case 30:
					case 31:
					case 34:
					case 38:
						goto IL_6F9;
					case 21:
						num3 = null;
						num5 = -1f;
						goto IL_6F9;
					case 23:
						num3 = new float?(0f);
						goto IL_6F9;
					case 25:
						num3 = null;
						num5 = -1f;
						flag2 = true;
						goto IL_6F9;
					case 32:
						num6 *= 0.5f;
						goto IL_6F9;
					case 33:
						num6 *= 0.5f;
						goto IL_6F9;
					case 35:
						num3 = new float?(0f);
						goto IL_6F9;
					case 36:
						num3 = null;
						num5 = -1f;
						flag2 = true;
						goto IL_6F9;
					case 37:
						num3 = null;
						num5 = -1f;
						flag2 = true;
						num6 *= 0.5f;
						goto IL_6F9;
					case 39:
						num3 = null;
						num5 = -1f;
						flag2 = true;
						goto IL_6F9;
					case 40:
					case 41:
					case 42:
					case 43:
						num3 = null;
						num5 = -2f;
						flag2 = true;
						num6 *= 0.5f;
						goto IL_6F9;
					case 44:
						num3 = null;
						num5 = -3f;
						goto IL_6F9;
					default:
						goto IL_6F9;
					}
					break;
				}
			}
			else if (type <= 581)
			{
				if (type != 454 && type != 572 && type != 581)
				{
					goto IL_6F9;
				}
			}
			else if (type <= 592)
			{
				if (type == 591)
				{
					num4 = 0.5f;
					num5 = -2f;
					goto IL_6F9;
				}
				if (type != 592)
				{
					goto IL_6F9;
				}
				num4 = 0.5f;
				num5 = -2f;
				texture2D = TextureAssets.GlowMask[294].Value;
				transparent = new Color(255, 255, 255, 0);
				goto IL_6F9;
			}
			else if (type != 660)
			{
				if (type != 698)
				{
					goto IL_6F9;
				}
				num4 = 0.5f;
				num5 = -1f;
				offSet.X -= 10f;
				flag3 = true;
				goto IL_6F9;
			}
			IL_67D:
			num3 = new float?((float)1);
			num5 = 0f;
			IL_6F9:
			if (flag2)
			{
				value += new Vector2(0f, 16f);
			}
			num6 *= -1f;
			bool flag4 = this.InAPlaceWithWind(topLeftX, topLeftY, sizeX, sizeY);
			if (flag3 || !flag4)
			{
				num -= num2;
			}
			ulong num8 = 0UL;
			for (int i = topLeftX; i < topLeftX + sizeX; i++)
			{
				for (int j = topLeftY; j < topLeftY + sizeY; j++)
				{
					Tile tile2 = Main.tile[i, j];
					ushort type2 = tile2.type;
					if ((int)type2 == type && this.IsVisible(tile2))
					{
						Math.Abs(((float)(i - topLeftX) + 0.5f) / (float)sizeX - 0.5f);
						short frameX = tile2.frameX;
						short frameY = tile2.frameY;
						float num9 = (float)(j - topLeftY + 1) / (float)sizeY;
						if (num9 == 0f)
						{
							num9 = 0.1f;
						}
						if (num3 != null)
						{
							num9 = num3.Value;
						}
						if (flag2 && j == topLeftY)
						{
							num9 = 0f;
						}
						int width;
						int num10;
						int num11;
						int num12;
						int num13;
						int num14;
						SpriteEffects effects;
						Texture2D texture2D2;
						Rectangle rectangle;
						Color color;
						this.GetTileDrawData(i, j, tile2, type2, ref frameX, ref frameY, out width, out num10, out num11, out num12, out num13, out num14, out effects, out texture2D2, out rectangle, out color);
						bool flag5 = this._rand.Next(4) == 0;
						Color color2 = Lighting.GetColor(i, j);
						this.DrawAnimatedTile_AdjustForVisionChangers(i, j, tile2, type2, frameX, frameY, ref color2, flag5);
						color2 = this.DrawTiles_GetLightOverride(j, i, tile2, type2, frameX, frameY, color2);
						if (this._isActiveAndNotPaused && flag5)
						{
							this.DrawTiles_EmitParticles(j, i, tile2, type2, frameX, frameY, color2);
						}
						Vector2 vector = new Vector2((float)(i * 16 - (int)screenPosition.X), (float)(j * 16 - (int)screenPosition.Y + num11)) + offSet;
						vector += value2;
						Vector2 vector2 = new Vector2(num * num4, Math.Abs(num) * num5 * num9);
						Vector2 vector3 = value - vector;
						Texture2D tileDrawTexture = this.GetTileDrawTexture(tile2, i, j);
						if (tileDrawTexture != null)
						{
							Vector2 vector4 = value + new Vector2(0f, vector2.Y);
							Rectangle rectangle2 = new Rectangle((int)frameX + num13, (int)frameY + num14, width, num10 - num12);
							float num15 = num * num6 * num9;
							if (type2 == 660 && j == topLeftY + sizeY - 1)
							{
								Texture2D value3 = TextureAssets.Extra[260].Value;
								float num16 = ((float)((i + j) % 200) * 0.11f + (float)Main.timeForVisualEffects / 360f) % 1f;
								Color white = Color.White;
								Main.spriteBatch.Draw(value3, vector4, new Rectangle?(rectangle2), white, num15, vector3, 1f, effects, 0f);
							}
							Main.spriteBatch.Draw(tileDrawTexture, vector4, new Rectangle?(rectangle2), color2, num15, vector3, 1f, effects, 0f);
							if (type2 == 660 && j == topLeftY + sizeY - 1)
							{
								Texture2D value4 = TextureAssets.Extra[260].Value;
								Color color3 = Main.hslToRgb(((float)((i + j) % 200) * 0.11f + (float)Main.timeForVisualEffects / 360f) % 1f, 1f, 0.8f, byte.MaxValue);
								color3.A = 127;
								Rectangle value5 = rectangle2;
								Vector2 position = vector4;
								Vector2 origin = vector3;
								Main.spriteBatch.Draw(value4, position, new Rectangle?(value5), color3, num15, origin, 1f, effects, 0f);
							}
							TEDeadCellsDisplayJar tedeadCellsDisplayJar;
							if (type2 == 698 && TileEntity.TryGetAt<TEDeadCellsDisplayJar>(topLeftX, topLeftY, out tedeadCellsDisplayJar))
							{
								Item item = tedeadCellsDisplayJar.item;
								short num17 = frameX / 38;
								int num18 = 0;
								int num19;
								switch (num17)
								{
								default:
									num19 = 22;
									num18 = -1;
									break;
								case 1:
									num19 = 18;
									break;
								case 2:
									num19 = 20;
									break;
								}
								Rectangle rectangle3 = rectangle2;
								rectangle3.Y += 46;
								Rectangle value6 = rectangle3;
								value6.Y += 46;
								int num20 = 1;
								Color color4 = new Color(150, 150, 255);
								if (!item.IsAir)
								{
									num20 = item.rare;
									if (item.expert)
									{
										num20 = -12;
									}
									if (num20 == -12)
									{
										color4 = new Color((int)((byte)Main.DiscoR), (int)((byte)Main.DiscoG), (int)((byte)Main.DiscoB));
									}
									else if (num20 == -13)
									{
										color4 = new Color(255, (int)((byte)(Main.masterColor * 200f)), 0);
									}
									else
									{
										color4 = item.GetPopupRarityColor();
									}
								}
								Vector3 vector5 = Main.rgbToHsl(color4);
								float x = vector5.X;
								Color color5 = color4;
								color4 *= 0.25f;
								if (num20 != -1)
								{
									color5 = Main.hslToRgb(x, MathHelper.Clamp(vector5.Y + 0.5f, 0f, 1f), MathHelper.Clamp(vector5.Z, 0f, 1f), 127);
								}
								Main.spriteBatch.Draw(tileDrawTexture, vector4, new Rectangle?(value6), color4, num15, vector3, 1f, effects, 0f);
								if (!item.IsAir)
								{
									Vector2 spinningpoint = new Vector2(-2f, (float)(24 + num18));
									int num7 = (int)(frameX / 38);
									if (num7 != 1)
									{
										if (num7 == 2)
										{
											spinningpoint.Y += 6f;
										}
									}
									else
									{
										spinningpoint.Y += 4f;
									}
									Vector2 screenPositionForItemCenter = vector4 + spinningpoint.RotatedBy((double)num15, default(Vector2)) + new Vector2(2f, 0f);
									color2 = Color.Lerp(color2, Color.White, 0.5f);
									ItemSlot.DrawItemIcon(item, 40, Main.spriteBatch, screenPositionForItemCenter, item.scale, (float)num19, color2, 1f, false);
								}
								color4.A = byte.MaxValue;
								Main.spriteBatch.Draw(tileDrawTexture, vector4, new Rectangle?(rectangle3), color5, num15, vector3, 1f, effects, 0f);
							}
							if (texture2D != null)
							{
								Main.spriteBatch.Draw(texture2D, vector4, new Rectangle?(rectangle2), transparent, num15, vector3, 1f, effects, 0f);
							}
							TileDrawing.TileFlameData tileFlameData = this.GetTileFlameData(i, j, (int)type2, (int)frameY);
							if (num8 == 0UL)
							{
								num8 = tileFlameData.flameSeed;
							}
							tileFlameData.flameSeed = num8;
							for (int k = 0; k < tileFlameData.flameCount; k++)
							{
								float x2 = (float)Utils.RandomInt(ref tileFlameData.flameSeed, tileFlameData.flameRangeXMin, tileFlameData.flameRangeXMax) * tileFlameData.flameRangeMultX;
								float y = (float)Utils.RandomInt(ref tileFlameData.flameSeed, tileFlameData.flameRangeYMin, tileFlameData.flameRangeYMax) * tileFlameData.flameRangeMultY;
								Main.spriteBatch.Draw(tileFlameData.flameTexture, vector4 + new Vector2(x2, y), new Rectangle?(rectangle2), tileFlameData.flameColor, num15, vector3, 1f, effects, 0f);
							}
						}
					}
				}
			}
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x005E0F94 File Offset: 0x005DF194
		private void EmitAlchemyHerbParticles(int j, int i, int style)
		{
			if (style == 0 && this._rand.Next(100) == 0)
			{
				int num = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16 - 4)), 16, 16, 19, 0f, 0f, 160, default(Color), 0.1f);
				Dust dust = this._dust[num];
				dust.velocity.X = dust.velocity.X / 2f;
				Dust dust2 = this._dust[num];
				dust2.velocity.Y = dust2.velocity.Y / 2f;
				this._dust[num].noGravity = true;
				this._dust[num].fadeIn = 1f;
			}
			if (style == 1 && this._rand.Next(100) == 0)
			{
				Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 41, 0f, 0f, 250, default(Color), 0.8f);
			}
			if (style == 3)
			{
				if (this._rand.Next(200) == 0)
				{
					int num2 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14, 0f, 0f, 100, default(Color), 0.2f);
					this._dust[num2].fadeIn = 1.2f;
				}
				if (this._rand.Next(75) == 0)
				{
					int num3 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 27, 0f, 0f, 100, default(Color), 1f);
					Dust dust3 = this._dust[num3];
					dust3.velocity.X = dust3.velocity.X / 2f;
					Dust dust4 = this._dust[num3];
					dust4.velocity.Y = dust4.velocity.Y / 2f;
				}
			}
			if (style == 4 && this._rand.Next(150) == 0)
			{
				int num4 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 8, 16, 0f, 0f, 0, default(Color), 1f);
				Dust dust5 = this._dust[num4];
				dust5.velocity.X = dust5.velocity.X / 3f;
				Dust dust6 = this._dust[num4];
				dust6.velocity.Y = dust6.velocity.Y / 3f;
				Dust dust7 = this._dust[num4];
				dust7.velocity.Y = dust7.velocity.Y - 0.7f;
				this._dust[num4].alpha = 50;
				this._dust[num4].scale *= 0.1f;
				this._dust[num4].fadeIn = 0.9f;
				this._dust[num4].noGravity = true;
			}
			if (style == 5 && this._rand.Next(40) == 0)
			{
				int num5 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16 - 6)), 16, 16, 6, 0f, 0f, 0, default(Color), 1.5f);
				Dust dust8 = this._dust[num5];
				dust8.velocity.Y = dust8.velocity.Y - 2f;
				this._dust[num5].noGravity = true;
			}
			if (style == 6 && this._rand.Next(30) == 0)
			{
				Color newColor = new Color(50, 255, 255, 255);
				int num6 = Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 43, 0f, 0f, 254, newColor, 0.5f);
				this._dust[num6].velocity *= 0f;
			}
		}

		// Token: 0x04005751 RID: 22353
		public static readonly uint Layer_LiquidBehindTiles = 0U;

		// Token: 0x04005752 RID: 22354
		public static readonly uint Layer_BehindTiles = 1U;

		// Token: 0x04005753 RID: 22355
		public static readonly uint Layer_Tiles = 2U;

		// Token: 0x04005754 RID: 22356
		public static readonly uint Layer_OverTiles = 3U;

		// Token: 0x04005755 RID: 22357
		private const int MAX_SPECIALS = 9000;

		// Token: 0x04005756 RID: 22358
		private const int MAX_SPECIALS_LEGACY = 1000;

		// Token: 0x04005757 RID: 22359
		private const float FORCE_FOR_MIN_WIND = 0.08f;

		// Token: 0x04005758 RID: 22360
		private const float FORCE_FOR_MAX_WIND = 1.2f;

		// Token: 0x04005759 RID: 22361
		private int _leafFrequency = 100000;

		// Token: 0x0400575A RID: 22362
		private int[] _specialsCount = new int[11];

		// Token: 0x0400575B RID: 22363
		private Point[][] _specialPositions = new Point[11][];

		// Token: 0x0400575C RID: 22364
		private Dictionary<Point, int> _displayDollTileEntityPositions = new Dictionary<Point, int>();

		// Token: 0x0400575D RID: 22365
		private Dictionary<Point, int> _hatRackTileEntityPositions = new Dictionary<Point, int>();

		// Token: 0x0400575E RID: 22366
		private Dictionary<Point, int> _trainingDummyTileEntityPositions = new Dictionary<Point, int>();

		// Token: 0x0400575F RID: 22367
		private Dictionary<Point, int> _itemFrameTileEntityPositions = new Dictionary<Point, int>();

		// Token: 0x04005760 RID: 22368
		private Dictionary<Point, int> _deadCellsDisplayJarTileEntityPositions = new Dictionary<Point, int>();

		// Token: 0x04005761 RID: 22369
		private Dictionary<Point, int> _foodPlatterTileEntityPositions = new Dictionary<Point, int>();

		// Token: 0x04005762 RID: 22370
		private Dictionary<Point, int> _weaponRackTileEntityPositions = new Dictionary<Point, int>();

		// Token: 0x04005763 RID: 22371
		private Dictionary<Point, int> _chestPositions = new Dictionary<Point, int>();

		// Token: 0x04005764 RID: 22372
		private int _specialTilesCount;

		// Token: 0x04005765 RID: 22373
		private int[] _specialTileX = new int[1000];

		// Token: 0x04005766 RID: 22374
		private int[] _specialTileY = new int[1000];

		// Token: 0x04005767 RID: 22375
		private UnifiedRandom _rand;

		// Token: 0x04005768 RID: 22376
		private double _treeWindCounter;

		// Token: 0x04005769 RID: 22377
		private double _grassWindCounter;

		// Token: 0x0400576A RID: 22378
		private double _sunflowerWindCounter;

		// Token: 0x0400576B RID: 22379
		private double _vineWindCounter;

		// Token: 0x0400576C RID: 22380
		private WindGrid _windGrid = new WindGrid();

		// Token: 0x0400576D RID: 22381
		private bool _shouldShowInvisibleBlocks;

		// Token: 0x0400576E RID: 22382
		private bool _shouldShowInvisibleBlocks_LastFrame;

		// Token: 0x0400576F RID: 22383
		private List<Point> _vineRootsPositions = new List<Point>();

		// Token: 0x04005770 RID: 22384
		private List<Point> _reverseVineRootsPositions = new List<Point>();

		// Token: 0x04005771 RID: 22385
		private TilePaintSystemV2 _paintSystem;

		// Token: 0x04005772 RID: 22386
		private INatureRenderer _natureRenderer = new NextNatureRenderer();

		// Token: 0x04005773 RID: 22387
		private Color _martianGlow = new Color(0, 0, 0, 0);

		// Token: 0x04005774 RID: 22388
		private Color _meteorGlow = new Color(100, 100, 100, 0);

		// Token: 0x04005775 RID: 22389
		private Color _lavaMossGlow = new Color(150, 100, 50, 0);

		// Token: 0x04005776 RID: 22390
		private Color _kryptonMossGlow = new Color(0, 200, 0, 0);

		// Token: 0x04005777 RID: 22391
		private Color _xenonMossGlow = new Color(0, 180, 250, 0);

		// Token: 0x04005778 RID: 22392
		private Color _argonMossGlow = new Color(225, 0, 125, 0);

		// Token: 0x04005779 RID: 22393
		private Color _violetMossGlow = new Color(150, 0, 250, 0);

		// Token: 0x0400577A RID: 22394
		private bool _isActiveAndNotPaused;

		// Token: 0x0400577B RID: 22395
		private Player _perspectivePlayer = new Player();

		// Token: 0x0400577C RID: 22396
		private Color _highQualityLightingRequirement;

		// Token: 0x0400577D RID: 22397
		private Color _mediumQualityLightingRequirement;

		// Token: 0x0400577E RID: 22398
		private static readonly Vector2 _zero = default(Vector2);

		// Token: 0x0400577F RID: 22399
		private static float[] noise = new float[256];

		// Token: 0x04005780 RID: 22400
		private TilePaintSystemV2.TileVariationkey _lastPaintLookupKey;

		// Token: 0x04005781 RID: 22401
		private Texture2D _lastPaintLookupTexture;

		// Token: 0x04005782 RID: 22402
		private Vector3[] _glowPaintColorSlices = new Vector3[]
		{
			Vector3.One,
			Vector3.One,
			Vector3.One,
			Vector3.One,
			Vector3.One,
			Vector3.One,
			Vector3.One,
			Vector3.One,
			Vector3.One
		};

		// Token: 0x04005783 RID: 22403
		private List<DrawData> _voidLensData = new List<DrawData>();

		// Token: 0x02000944 RID: 2372
		private enum TileCounterType
		{
			// Token: 0x04007518 RID: 29976
			Tree,
			// Token: 0x04007519 RID: 29977
			WindyGrass,
			// Token: 0x0400751A RID: 29978
			MultiTileGrass,
			// Token: 0x0400751B RID: 29979
			MultiTileVine,
			// Token: 0x0400751C RID: 29980
			Vine,
			// Token: 0x0400751D RID: 29981
			BiomeGrass,
			// Token: 0x0400751E RID: 29982
			VoidLens,
			// Token: 0x0400751F RID: 29983
			ReverseVine,
			// Token: 0x04007520 RID: 29984
			TeleportationPylon,
			// Token: 0x04007521 RID: 29985
			MasterTrophy,
			// Token: 0x04007522 RID: 29986
			AnyDirectionalGrass,
			// Token: 0x04007523 RID: 29987
			Count
		}

		// Token: 0x02000945 RID: 2373
		private struct TileFlameData
		{
			// Token: 0x04007524 RID: 29988
			public Texture2D flameTexture;

			// Token: 0x04007525 RID: 29989
			public ulong flameSeed;

			// Token: 0x04007526 RID: 29990
			public int flameCount;

			// Token: 0x04007527 RID: 29991
			public Color flameColor;

			// Token: 0x04007528 RID: 29992
			public int flameRangeXMin;

			// Token: 0x04007529 RID: 29993
			public int flameRangeXMax;

			// Token: 0x0400752A RID: 29994
			public int flameRangeYMin;

			// Token: 0x0400752B RID: 29995
			public int flameRangeYMax;

			// Token: 0x0400752C RID: 29996
			public float flameRangeMultX;

			// Token: 0x0400752D RID: 29997
			public float flameRangeMultY;
		}
	}
}
