using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Liquid
{
	// Token: 0x02000282 RID: 642
	public class LiquidRenderer
	{
		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06002495 RID: 9365 RVA: 0x0054EAC8 File Offset: 0x0054CCC8
		// (remove) Token: 0x06002496 RID: 9366 RVA: 0x0054EB00 File Offset: 0x0054CD00
		public event Action<Color[], Rectangle> WaveFilters;

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06002497 RID: 9367 RVA: 0x004DD19C File Offset: 0x004DB39C
		private static Tile[,] Tiles
		{
			get
			{
				return Main.tile;
			}
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x0054EB35 File Offset: 0x0054CD35
		public static void LoadContent()
		{
			LiquidRenderer.Instance = new LiquidRenderer();
			LiquidRenderer.Instance.PrepareAssets();
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x0054EBB8 File Offset: 0x0054CDB8
		private void PrepareAssets()
		{
			if (Main.dedServ)
			{
				return;
			}
			for (int i = 0; i < this._liquidTextures.Length; i++)
			{
				this._liquidTextures[i] = Main.Assets.Request<Texture2D>("Images/Misc/water_" + i, 1);
			}
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x0054EC04 File Offset: 0x0054CE04
		private unsafe void InternalPrepareDraw(Rectangle drawArea)
		{
			Rectangle rectangle = new Rectangle(drawArea.X - 2, drawArea.Y - 2, drawArea.Width + 4, drawArea.Height + 4);
			this._drawArea = drawArea;
			if (this._cache.Length < rectangle.Width * rectangle.Height + 1)
			{
				this._cache = new LiquidRenderer.LiquidCache[rectangle.Width * rectangle.Height + 1];
			}
			if (this._drawCache.Length < drawArea.Width * drawArea.Height + 1)
			{
				this._drawCache = new LiquidRenderer.LiquidDrawCache[drawArea.Width * drawArea.Height + 1];
			}
			if (this._drawCacheForShimmer.Length < drawArea.Width * drawArea.Height + 1)
			{
				this._drawCacheForShimmer = new LiquidRenderer.SpecialLiquidDrawCache[drawArea.Width * drawArea.Height + 1];
			}
			if (this._waveMask.Length < drawArea.Width * drawArea.Height)
			{
				this._waveMask = new Color[drawArea.Width * drawArea.Height];
			}
			fixed (LiquidRenderer.LiquidCache* ptr = &this._cache[1])
			{
				LiquidRenderer.LiquidCache* ptr2 = ptr;
				int num = rectangle.Height * 2 + 2;
				LiquidRenderer.LiquidCache* ptr3 = ptr2;
				for (int i = rectangle.X; i < rectangle.X + rectangle.Width; i++)
				{
					for (int j = rectangle.Y; j < rectangle.Y + rectangle.Height; j++)
					{
						Tile tile = LiquidRenderer.Tiles[i, j];
						if (tile == null)
						{
							tile = new Tile();
						}
						ptr3->LiquidLevel = (float)tile.liquid / 255f;
						ptr3->IsHalfBrick = (tile.halfBrick() && ptr3[-1].HasLiquid && !TileID.Sets.Platforms[(int)tile.type]);
						ptr3->IsSolid = WorldGen.SolidOrSlopedTile(tile);
						ptr3->HasLiquid = (tile.liquid > 0);
						ptr3->VisibleLiquidLevel = 0f;
						ptr3->HasWall = (tile.wall > 0);
						ptr3->Type = tile.liquidType();
						if (ptr3->IsHalfBrick && !ptr3->HasLiquid)
						{
							ptr3->Type = ptr3[-1].Type;
						}
						ptr3++;
					}
				}
				ptr3 = ptr2;
				ptr3 += num;
				for (int k = 2; k < rectangle.Width - 2; k++)
				{
					for (int l = 2; l < rectangle.Height - 2; l++)
					{
						float num2 = 0f;
						if (ptr3->IsHalfBrick && ptr3[-1].HasLiquid)
						{
							num2 = 1f;
						}
						else if (!ptr3->HasLiquid)
						{
							LiquidRenderer.LiquidCache liquidCache = ptr3[-1];
							LiquidRenderer.LiquidCache liquidCache2 = ptr3[1];
							LiquidRenderer.LiquidCache liquidCache3 = ptr3[-rectangle.Height];
							LiquidRenderer.LiquidCache liquidCache4 = ptr3[rectangle.Height];
							if (liquidCache.HasLiquid && liquidCache2.HasLiquid && liquidCache.Type == liquidCache2.Type && !liquidCache.IsSolid && !liquidCache2.IsSolid)
							{
								num2 = liquidCache.LiquidLevel + liquidCache2.LiquidLevel;
								ptr3->Type = liquidCache.Type;
							}
							if (liquidCache3.HasLiquid && liquidCache4.HasLiquid && liquidCache3.Type == liquidCache4.Type && !liquidCache3.IsSolid && !liquidCache4.IsSolid)
							{
								num2 = Math.Max(num2, liquidCache3.LiquidLevel + liquidCache4.LiquidLevel);
								ptr3->Type = liquidCache3.Type;
							}
							num2 *= 0.5f;
						}
						else
						{
							num2 = ptr3->LiquidLevel;
						}
						ptr3->VisibleLiquidLevel = num2;
						ptr3->HasVisibleLiquid = (num2 != 0f);
						ptr3++;
					}
					ptr3 += 4;
				}
				ptr3 = ptr2;
				for (int m = 0; m < rectangle.Width; m++)
				{
					for (int n = 0; n < rectangle.Height - 10; n++)
					{
						if (ptr3->HasVisibleLiquid && (!ptr3->IsSolid || ptr3->IsHalfBrick))
						{
							ptr3->Opacity = 1f;
							ptr3->VisibleType = ptr3->Type;
							float num3 = 1f / (float)(LiquidRenderer.WATERFALL_LENGTH[(int)ptr3->Type] + 1);
							float num4 = 1f;
							for (int num5 = 1; num5 <= LiquidRenderer.WATERFALL_LENGTH[(int)ptr3->Type]; num5++)
							{
								num4 -= num3;
								if (ptr3[num5].IsSolid)
								{
									break;
								}
								ptr3[num5].VisibleLiquidLevel = Math.Max(ptr3[num5].VisibleLiquidLevel, ptr3->VisibleLiquidLevel * num4);
								ptr3[num5].Opacity = num4;
								ptr3[num5].VisibleType = ptr3->Type;
							}
						}
						if (ptr3->IsSolid && !ptr3->IsHalfBrick)
						{
							ptr3->VisibleLiquidLevel = 1f;
							ptr3->HasVisibleLiquid = false;
						}
						else
						{
							ptr3->HasVisibleLiquid = (ptr3->VisibleLiquidLevel != 0f);
						}
						ptr3++;
					}
					ptr3 += 10;
				}
				ptr3 = ptr2;
				ptr3 += num;
				for (int num6 = 2; num6 < rectangle.Width - 2; num6++)
				{
					for (int num7 = 2; num7 < rectangle.Height - 2; num7++)
					{
						if (!ptr3->HasVisibleLiquid)
						{
							ptr3->HasLeftEdge = false;
							ptr3->HasTopEdge = false;
							ptr3->HasRightEdge = false;
							ptr3->HasBottomEdge = false;
						}
						else
						{
							LiquidRenderer.LiquidCache liquidCache = ptr3[-1];
							LiquidRenderer.LiquidCache liquidCache2 = ptr3[1];
							LiquidRenderer.LiquidCache liquidCache3 = ptr3[-rectangle.Height];
							LiquidRenderer.LiquidCache liquidCache4 = ptr3[rectangle.Height];
							float num8 = 0f;
							float num9 = 1f;
							float num10 = 0f;
							float num11 = 1f;
							float visibleLiquidLevel = ptr3->VisibleLiquidLevel;
							if (!liquidCache.HasVisibleLiquid)
							{
								num10 += liquidCache2.VisibleLiquidLevel * (1f - visibleLiquidLevel);
							}
							if (!liquidCache2.HasVisibleLiquid && !liquidCache2.IsSolid && !liquidCache2.IsHalfBrick)
							{
								num11 -= liquidCache.VisibleLiquidLevel * (1f - visibleLiquidLevel);
							}
							if (!liquidCache3.HasVisibleLiquid && !liquidCache3.IsSolid && !liquidCache3.IsHalfBrick)
							{
								num8 += liquidCache4.VisibleLiquidLevel * (1f - visibleLiquidLevel);
							}
							if (!liquidCache4.HasVisibleLiquid && !liquidCache4.IsSolid && !liquidCache4.IsHalfBrick)
							{
								num9 -= liquidCache3.VisibleLiquidLevel * (1f - visibleLiquidLevel);
							}
							ptr3->LeftWall = num8;
							ptr3->RightWall = num9;
							ptr3->BottomWall = num11;
							ptr3->TopWall = num10;
							Point zero = Point.Zero;
							ptr3->HasTopEdge = ((!liquidCache.HasVisibleLiquid && !liquidCache.IsSolid) || num10 != 0f);
							ptr3->HasBottomEdge = ((!liquidCache2.HasVisibleLiquid && !liquidCache2.IsSolid) || num11 != 1f);
							ptr3->HasLeftEdge = ((!liquidCache3.HasVisibleLiquid && !liquidCache3.IsSolid) || num8 != 0f);
							ptr3->HasRightEdge = ((!liquidCache4.HasVisibleLiquid && !liquidCache4.IsSolid) || num9 != 1f);
							if (!ptr3->HasLeftEdge)
							{
								if (ptr3->HasRightEdge)
								{
									zero.X += 32;
								}
								else
								{
									zero.X += 16;
								}
							}
							if (ptr3->HasLeftEdge && ptr3->HasRightEdge)
							{
								zero.X = 16;
								zero.Y += 32;
								if (ptr3->HasTopEdge)
								{
									zero.Y = 16;
								}
							}
							else if (!ptr3->HasTopEdge)
							{
								if (!ptr3->HasLeftEdge && !ptr3->HasRightEdge)
								{
									zero.Y += 48;
								}
								else
								{
									zero.Y += 16;
								}
							}
							if (zero.Y == 16 && (ptr3->HasLeftEdge ^ ptr3->HasRightEdge) && (num7 + rectangle.Y) % 2 == 0)
							{
								zero.Y += 16;
							}
							ptr3->FrameOffset = zero;
						}
						ptr3++;
					}
					ptr3 += 4;
				}
				ptr3 = ptr2;
				ptr3 += num;
				for (int num12 = 2; num12 < rectangle.Width - 2; num12++)
				{
					for (int num13 = 2; num13 < rectangle.Height - 2; num13++)
					{
						if (ptr3->HasVisibleLiquid)
						{
							LiquidRenderer.LiquidCache liquidCache = ptr3[-1];
							LiquidRenderer.LiquidCache liquidCache2 = ptr3[1];
							LiquidRenderer.LiquidCache liquidCache3 = ptr3[-rectangle.Height];
							LiquidRenderer.LiquidCache liquidCache4 = ptr3[rectangle.Height];
							ptr3->VisibleLeftWall = ptr3->LeftWall;
							ptr3->VisibleRightWall = ptr3->RightWall;
							ptr3->VisibleTopWall = ptr3->TopWall;
							ptr3->VisibleBottomWall = ptr3->BottomWall;
							if (liquidCache.HasVisibleLiquid && liquidCache2.HasVisibleLiquid)
							{
								if (ptr3->HasLeftEdge)
								{
									ptr3->VisibleLeftWall = (ptr3->LeftWall * 2f + liquidCache.LeftWall + liquidCache2.LeftWall) * 0.25f;
								}
								if (ptr3->HasRightEdge)
								{
									ptr3->VisibleRightWall = (ptr3->RightWall * 2f + liquidCache.RightWall + liquidCache2.RightWall) * 0.25f;
								}
							}
							if (liquidCache3.HasVisibleLiquid && liquidCache4.HasVisibleLiquid)
							{
								if (ptr3->HasTopEdge)
								{
									ptr3->VisibleTopWall = (ptr3->TopWall * 2f + liquidCache3.TopWall + liquidCache4.TopWall) * 0.25f;
								}
								if (ptr3->HasBottomEdge)
								{
									ptr3->VisibleBottomWall = (ptr3->BottomWall * 2f + liquidCache3.BottomWall + liquidCache4.BottomWall) * 0.25f;
								}
							}
						}
						ptr3++;
					}
					ptr3 += 4;
				}
				ptr3 = ptr2;
				ptr3 += num;
				for (int num14 = 2; num14 < rectangle.Width - 2; num14++)
				{
					for (int num15 = 2; num15 < rectangle.Height - 2; num15++)
					{
						if (ptr3->HasLiquid)
						{
							LiquidRenderer.LiquidCache liquidCache = ptr3[-1];
							LiquidRenderer.LiquidCache liquidCache2 = ptr3[1];
							LiquidRenderer.LiquidCache liquidCache3 = ptr3[-rectangle.Height];
							LiquidRenderer.LiquidCache liquidCache4 = ptr3[rectangle.Height];
							if (ptr3->HasTopEdge && !ptr3->HasBottomEdge && (ptr3->HasLeftEdge ^ ptr3->HasRightEdge))
							{
								if (ptr3->HasRightEdge)
								{
									ptr3->VisibleRightWall = liquidCache2.VisibleRightWall;
									ptr3->VisibleTopWall = liquidCache3.VisibleTopWall;
								}
								else
								{
									ptr3->VisibleLeftWall = liquidCache2.VisibleLeftWall;
									ptr3->VisibleTopWall = liquidCache4.VisibleTopWall;
								}
							}
							else if (liquidCache2.FrameOffset.X == 16 && liquidCache2.FrameOffset.Y == 32)
							{
								if (ptr3->VisibleLeftWall > 0.5f)
								{
									ptr3->VisibleLeftWall = 0f;
									ptr3->FrameOffset = new Point(0, 0);
								}
								else if (ptr3->VisibleRightWall < 0.5f)
								{
									ptr3->VisibleRightWall = 1f;
									ptr3->FrameOffset = new Point(32, 0);
								}
							}
						}
						ptr3++;
					}
					ptr3 += 4;
				}
				ptr3 = ptr2;
				ptr3 += num;
				for (int num16 = 2; num16 < rectangle.Width - 2; num16++)
				{
					for (int num17 = 2; num17 < rectangle.Height - 2; num17++)
					{
						if (ptr3->HasLiquid)
						{
							LiquidRenderer.LiquidCache liquidCache = ptr3[-1];
							LiquidRenderer.LiquidCache liquidCache2 = ptr3[1];
							LiquidRenderer.LiquidCache liquidCache3 = ptr3[-rectangle.Height];
							LiquidRenderer.LiquidCache liquidCache4 = ptr3[rectangle.Height];
							if (!ptr3->HasBottomEdge && !ptr3->HasLeftEdge && !ptr3->HasTopEdge && !ptr3->HasRightEdge)
							{
								if (liquidCache3.HasTopEdge && liquidCache.HasLeftEdge)
								{
									ptr3->FrameOffset.X = Math.Max(4, (int)(16f - liquidCache.VisibleLeftWall * 16f)) - 4;
									ptr3->FrameOffset.Y = 48 + Math.Max(4, (int)(16f - liquidCache3.VisibleTopWall * 16f)) - 4;
									ptr3->VisibleLeftWall = 0f;
									ptr3->VisibleTopWall = 0f;
									ptr3->VisibleRightWall = 1f;
									ptr3->VisibleBottomWall = 1f;
								}
								else if (liquidCache4.HasTopEdge && liquidCache.HasRightEdge)
								{
									ptr3->FrameOffset.X = 32 - Math.Min(16, (int)(liquidCache.VisibleRightWall * 16f) - 4);
									ptr3->FrameOffset.Y = 48 + Math.Max(4, (int)(16f - liquidCache4.VisibleTopWall * 16f)) - 4;
									ptr3->VisibleLeftWall = 0f;
									ptr3->VisibleTopWall = 0f;
									ptr3->VisibleRightWall = 1f;
									ptr3->VisibleBottomWall = 1f;
								}
							}
						}
						ptr3++;
					}
					ptr3 += 4;
				}
				ptr3 = ptr2;
				ptr3 += num;
				fixed (LiquidRenderer.LiquidDrawCache* ptr4 = &this._drawCache[0])
				{
					LiquidRenderer.LiquidDrawCache* ptr5 = ptr4;
					fixed (Color* ptr6 = &this._waveMask[0])
					{
						Color* ptr7 = ptr6;
						LiquidRenderer.LiquidDrawCache* ptr8 = ptr5;
						Color* ptr9 = ptr7;
						for (int num18 = 2; num18 < rectangle.Width - 2; num18++)
						{
							for (int num19 = 2; num19 < rectangle.Height - 2; num19++)
							{
								if (ptr3->HasVisibleLiquid)
								{
									float num20 = Math.Min(0.75f, ptr3->VisibleLeftWall);
									float num21 = Math.Max(0.25f, ptr3->VisibleRightWall);
									float num22 = Math.Min(0.75f, ptr3->VisibleTopWall);
									float num23 = Math.Max(0.25f, ptr3->VisibleBottomWall);
									if (ptr3->IsHalfBrick && ptr3->IsSolid && num23 > 0.5f)
									{
										num23 = 0.5f;
									}
									ptr8->IsVisible = (ptr3->HasWall || !ptr3->IsHalfBrick || !ptr3->HasLiquid || ptr3->LiquidLevel >= 1f);
									ptr8->SourceRectangle = new Rectangle((int)(16f - num21 * 16f) + ptr3->FrameOffset.X, (int)(16f - num23 * 16f) + ptr3->FrameOffset.Y, (int)Math.Ceiling((double)((num21 - num20) * 16f)), (int)Math.Ceiling((double)((num23 - num22) * 16f)));
									ptr8->IsSurfaceLiquid = (ptr3->FrameOffset.X == 16 && ptr3->FrameOffset.Y == 0 && (double)(num19 + rectangle.Y) > Main.worldSurface - 40.0);
									ptr8->Opacity = ptr3->Opacity;
									ptr8->LiquidOffset = new Vector2((float)Math.Floor((double)(num20 * 16f)), (float)Math.Floor((double)(num22 * 16f)));
									ptr8->Type = ptr3->VisibleType;
									ptr8->HasWall = ptr3->HasWall;
									byte b = LiquidRenderer.WAVE_MASK_STRENGTH[(int)ptr3->VisibleType];
									byte b2 = (byte)(b >> 1);
									ptr9->R = b2;
									ptr9->G = b2;
									ptr9->B = LiquidRenderer.VISCOSITY_MASK[(int)ptr3->VisibleType];
									ptr9->A = b;
									LiquidRenderer.LiquidCache* ptr10 = ptr3 - 1;
									if (num19 != 2 && !ptr10->HasVisibleLiquid && !ptr10->IsSolid && !ptr10->IsHalfBrick)
									{
										*(ptr9 - 1) = *ptr9;
									}
								}
								else
								{
									ptr8->IsVisible = false;
									int num24 = (!ptr3->IsSolid && !ptr3->IsHalfBrick) ? 4 : 3;
									byte b3 = LiquidRenderer.WAVE_MASK_STRENGTH[num24];
									byte b4 = (byte)(b3 >> 1);
									ptr9->R = b4;
									ptr9->G = b4;
									ptr9->B = LiquidRenderer.VISCOSITY_MASK[num24];
									ptr9->A = b3;
								}
								ptr3++;
								ptr8++;
								ptr9++;
							}
							ptr3 += 4;
						}
					}
				}
				ptr3 = ptr2;
				for (int num25 = rectangle.X; num25 < rectangle.X + rectangle.Width; num25++)
				{
					for (int num26 = rectangle.Y; num26 < rectangle.Y + rectangle.Height; num26++)
					{
						if (ptr3->VisibleType == 1 && ptr3->HasVisibleLiquid && Dust.lavaBubbles < 200)
						{
							if (this._random.Next(700) == 0)
							{
								Dust.NewDust(new Vector2((float)(num25 * 16), (float)(num26 * 16)), 16, 16, 35, 0f, 0f, 0, Color.White, 1f);
							}
							if (this._random.Next(350) == 0)
							{
								int num27 = Dust.NewDust(new Vector2((float)(num25 * 16), (float)(num26 * 16)), 16, 8, 35, 0f, 0f, 50, Color.White, 1.5f);
								Main.dust[num27].velocity *= 0.8f;
								Dust dust = Main.dust[num27];
								dust.velocity.X = dust.velocity.X * 2f;
								Dust dust2 = Main.dust[num27];
								dust2.velocity.Y = dust2.velocity.Y - (float)this._random.Next(1, 7) * 0.1f;
								if (this._random.Next(10) == 0)
								{
									Dust dust3 = Main.dust[num27];
									dust3.velocity.Y = dust3.velocity.Y * (float)this._random.Next(2, 5);
								}
								Main.dust[num27].noGravity = true;
							}
						}
						ptr3++;
					}
				}
				fixed (LiquidRenderer.LiquidDrawCache* ptr4 = &this._drawCache[0])
				{
					LiquidRenderer.LiquidDrawCache* ptr11 = ptr4;
					fixed (LiquidRenderer.SpecialLiquidDrawCache* ptr12 = &this._drawCacheForShimmer[0])
					{
						LiquidRenderer.SpecialLiquidDrawCache* ptr13 = ptr12;
						LiquidRenderer.LiquidDrawCache* ptr14 = ptr11;
						LiquidRenderer.SpecialLiquidDrawCache* ptr15 = ptr13;
						for (int num28 = 2; num28 < rectangle.Width - 2; num28++)
						{
							for (int num29 = 2; num29 < rectangle.Height - 2; num29++)
							{
								if (ptr14->IsVisible && ptr14->Type == 3)
								{
									ptr15->X = num28;
									ptr15->Y = num29;
									ptr15->IsVisible = ptr14->IsVisible;
									ptr15->HasWall = ptr14->HasWall;
									ptr15->IsSurfaceLiquid = ptr14->IsSurfaceLiquid;
									ptr15->LiquidOffset = ptr14->LiquidOffset;
									ptr15->Opacity = ptr14->Opacity;
									ptr15->SourceRectangle = ptr14->SourceRectangle;
									ptr15->Type = ptr14->Type;
									ptr14->IsVisible = false;
									ptr15++;
								}
								ptr14++;
							}
						}
						ptr15->IsVisible = false;
					}
				}
			}
			if (this.WaveFilters != null)
			{
				this.WaveFilters(this._waveMask, this.GetCachedDrawArea());
			}
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x00550118 File Offset: 0x0054E318
		public unsafe void DrawNormalLiquids(SpriteBatch spriteBatch, Vector2 drawOffset, int waterStyle, float globalAlpha, bool isBackgroundDraw, bool waterOnly)
		{
			Rectangle drawArea = this._drawArea;
			Main.tileBatch.Restart();
			fixed (LiquidRenderer.LiquidDrawCache* ptr = &this._drawCache[0])
			{
				LiquidRenderer.LiquidDrawCache* ptr2 = ptr;
				for (int i = drawArea.X; i < drawArea.X + drawArea.Width; i++)
				{
					for (int j = drawArea.Y; j < drawArea.Y + drawArea.Height; j++)
					{
						if (ptr2->IsVisible && (!waterOnly || ptr2->Type == 0))
						{
							Main.tileBatch.SetLayer(0U, 0);
							Rectangle sourceRectangle = ptr2->SourceRectangle;
							if (ptr2->IsSurfaceLiquid)
							{
								sourceRectangle.Y = 1280;
							}
							else if (sourceRectangle.X == 16)
							{
								sourceRectangle.Y += this._waterfallAnimationFrame * 80;
							}
							else
							{
								sourceRectangle.Y += this._animationFrame * 80;
							}
							Vector2 liquidOffset = ptr2->LiquidOffset;
							float num = ptr2->Opacity * (isBackgroundDraw ? 1f : LiquidRenderer.DEFAULT_OPACITY[(int)ptr2->Type]);
							int num2 = (int)ptr2->Type;
							if (num2 == 0)
							{
								num2 = waterStyle;
								num *= globalAlpha;
							}
							else if (num2 == 1)
							{
								num *= Main.player[Main.myPlayer].lavaOpacity;
							}
							else if (num2 == 2)
							{
								num2 = 11;
							}
							num = Math.Min(1f, num);
							VertexColors colors;
							Lighting.GetCornerColors(i, j, out colors, 1f);
							colors.BottomLeftColor *= num;
							colors.BottomRightColor *= num;
							colors.TopLeftColor *= num;
							colors.TopRightColor *= num;
							Main.DrawTileInWater(drawOffset, i, j);
							Main.tileBatch.Draw(this._liquidTextures[num2].Value, new Vector2((float)(i << 4), (float)(j << 4)) + drawOffset + liquidOffset, new Rectangle?(sourceRectangle), colors, Vector2.Zero, 1f, SpriteEffects.None);
						}
						ptr2++;
					}
				}
			}
			int value = Main.tileBatch.End();
			(isBackgroundDraw ? TimeLogger.LiquidBackgroundDrawCalls : TimeLogger.LiquidDrawCalls).Add(value);
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x00550380 File Offset: 0x0054E580
		public unsafe void DrawShimmer(SpriteBatch spriteBatch, Vector2 drawOffset, bool isBackgroundDraw)
		{
			Rectangle drawArea = this._drawArea;
			Main.tileBatch.Restart();
			fixed (LiquidRenderer.SpecialLiquidDrawCache* ptr = &this._drawCacheForShimmer[0])
			{
				LiquidRenderer.SpecialLiquidDrawCache* ptr2 = ptr;
				int num = this._drawCacheForShimmer.Length;
				int num2 = 0;
				while (num2 < num && ptr2->IsVisible)
				{
					Main.tileBatch.SetLayer(0U, 0);
					Rectangle sourceRectangle = ptr2->SourceRectangle;
					if (ptr2->IsSurfaceLiquid)
					{
						sourceRectangle.Y = 1280;
					}
					else
					{
						sourceRectangle.Y += this._animationFrame * 80;
					}
					Vector2 liquidOffset = ptr2->LiquidOffset;
					float num3 = ptr2->Opacity * (isBackgroundDraw ? 1f : 0.75f);
					int num4 = 14;
					num3 = Math.Min(1f, num3);
					int num5 = ptr2->X + drawArea.X - 2;
					int num6 = ptr2->Y + drawArea.Y - 2;
					VertexColors colors;
					Lighting.GetCornerColors(num5, num6, out colors, 1f);
					LiquidRenderer.SetShimmerVertexColors(ref colors, num3, num5, num6);
					Main.DrawTileInWater(drawOffset, num5, num6);
					Main.tileBatch.Draw(this._liquidTextures[num4].Value, new Vector2((float)(num5 << 4), (float)(num6 << 4)) + drawOffset + liquidOffset, new Rectangle?(sourceRectangle), colors, Vector2.Zero, 1f, SpriteEffects.None);
					sourceRectangle = ptr2->SourceRectangle;
					bool flag = sourceRectangle.X != 16 || sourceRectangle.Y % 80 != 48;
					if (flag || (num5 + num6) % 2 == 0)
					{
						sourceRectangle.X += 48;
						sourceRectangle.Y += 80 * this.GetShimmerFrame(flag, (float)num5, (float)num6);
						LiquidRenderer.SetShimmerVertexColors_Sparkle(ref colors, ptr2->Opacity, num5, num6, flag);
						Main.tileBatch.Draw(this._liquidTextures[num4].Value, new Vector2((float)(num5 << 4), (float)(num6 << 4)) + drawOffset + liquidOffset, new Rectangle?(sourceRectangle), colors, Vector2.Zero, 1f, SpriteEffects.None);
					}
					ptr2++;
					num2++;
				}
			}
			int value = Main.tileBatch.End();
			(isBackgroundDraw ? TimeLogger.LiquidBackgroundDrawCalls : TimeLogger.LiquidDrawCalls).Add(value);
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x005505D8 File Offset: 0x0054E7D8
		public static VertexColors SetShimmerVertexColors_Sparkle(ref VertexColors colors, float opacity, int x, int y, bool top)
		{
			colors.BottomLeftColor = LiquidRenderer.GetShimmerGlitterColor(top, (float)x, (float)(y + 1));
			colors.BottomRightColor = LiquidRenderer.GetShimmerGlitterColor(top, (float)(x + 1), (float)(y + 1));
			colors.TopLeftColor = LiquidRenderer.GetShimmerGlitterColor(top, (float)x, (float)y);
			colors.TopRightColor = LiquidRenderer.GetShimmerGlitterColor(top, (float)(x + 1), (float)y);
			colors.BottomLeftColor *= opacity;
			colors.BottomRightColor *= opacity;
			colors.TopLeftColor *= opacity;
			colors.TopRightColor *= opacity;
			return colors;
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x00550694 File Offset: 0x0054E894
		public static void SetShimmerVertexColors(ref VertexColors colors, float opacity, int x, int y)
		{
			colors.BottomLeftColor = Color.White;
			colors.BottomRightColor = Color.White;
			colors.TopLeftColor = Color.White;
			colors.TopRightColor = Color.White;
			colors.BottomLeftColor *= opacity;
			colors.BottomRightColor *= opacity;
			colors.TopLeftColor *= opacity;
			colors.TopRightColor *= opacity;
			colors.BottomLeftColor = new Color(colors.BottomLeftColor.ToVector4() * LiquidRenderer.GetShimmerBaseColor((float)x, (float)(y + 1)));
			colors.BottomRightColor = new Color(colors.BottomRightColor.ToVector4() * LiquidRenderer.GetShimmerBaseColor((float)(x + 1), (float)(y + 1)));
			colors.TopLeftColor = new Color(colors.TopLeftColor.ToVector4() * LiquidRenderer.GetShimmerBaseColor((float)x, (float)y));
			colors.TopRightColor = new Color(colors.TopRightColor.ToVector4() * LiquidRenderer.GetShimmerBaseColor((float)(x + 1), (float)y));
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x005507C1 File Offset: 0x0054E9C1
		public static float GetShimmerWave(ref float worldPositionX, ref float worldPositionY)
		{
			return (float)Math.Sin(((double)((worldPositionX + worldPositionY / 6f) / 10f) - Main.timeForVisualEffects / 360.0) * 6.2831854820251465);
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x005507F8 File Offset: 0x0054E9F8
		public static Color GetShimmerGlitterColor(bool top, float worldPositionX, float worldPositionY)
		{
			Color color = Main.hslToRgb((float)(((double)(worldPositionX + worldPositionY / 6f) + Main.timeForVisualEffects / 30.0) / 6.0) % 1f, 1f, 0.5f, byte.MaxValue);
			color.A = 0;
			return new Color(color.ToVector4() * LiquidRenderer.GetShimmerGlitterOpacity(top, worldPositionX, worldPositionY));
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x00550868 File Offset: 0x0054EA68
		public static float GetShimmerGlitterOpacity(bool top, float worldPositionX, float worldPositionY)
		{
			if (top)
			{
				return 0.5f;
			}
			float num = Utils.Remap((float)Math.Sin(((double)((worldPositionX + worldPositionY / 6f) / 10f) - Main.timeForVisualEffects / 360.0) * 6.2831854820251465), -0.5f, 1f, 0f, 0.35f, true);
			float num2 = (float)Math.Sin((double)(LiquidRenderer.SimpleWhiteNoise((uint)worldPositionX, (uint)worldPositionY) / 10f) + Main.timeForVisualEffects / 180.0);
			return Utils.Remap(num * num2, 0f, 0.5f, 0f, 1f, true);
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x0055090D File Offset: 0x0054EB0D
		private static uint SimpleWhiteNoise(uint x, uint y)
		{
			x = 36469U * (x & 65535U) + (x >> 16);
			y = 18012U * (y & 65535U) + (y >> 16);
			return (x << 16) + y;
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x00550940 File Offset: 0x0054EB40
		public int GetShimmerFrame(bool top, float worldPositionX, float worldPositionY)
		{
			worldPositionX += 0.5f;
			worldPositionY += 0.5f;
			double num = (double)((worldPositionX + worldPositionY / 6f) / 10f) - Main.timeForVisualEffects / 360.0;
			if (!top)
			{
				num += (double)(worldPositionX + worldPositionY);
			}
			return ((int)num % 16 + 16) % 16;
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x00550998 File Offset: 0x0054EB98
		public static Vector4 GetShimmerBaseColor(float worldPositionX, float worldPositionY)
		{
			float shimmerWave = LiquidRenderer.GetShimmerWave(ref worldPositionX, ref worldPositionY);
			return Vector4.Lerp(new Vector4(0.64705884f, 0.50980395f, 0.93333334f, 1f), new Vector4(0.8039216f, 0.8039216f, 1f, 1f), 0.1f + shimmerWave * 0.4f);
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x005509F4 File Offset: 0x0054EBF4
		public bool HasFullWater(int x, int y)
		{
			x -= this._drawArea.X;
			y -= this._drawArea.Y;
			int num = x * this._drawArea.Height + y;
			return num < 0 || num >= this._drawCache.Length || (this._drawCache[num].IsVisible && !this._drawCache[num].IsSurfaceLiquid);
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x00550A6C File Offset: 0x0054EC6C
		public float GetVisibleLiquid(int x, int y)
		{
			x -= this._drawArea.X;
			y -= this._drawArea.Y;
			if (x < 0 || x >= this._drawArea.Width || y < 0 || y >= this._drawArea.Height)
			{
				return 0f;
			}
			int num = (x + 2) * (this._drawArea.Height + 4) + y + 2;
			if (!this._cache[num].HasVisibleLiquid)
			{
				return 0f;
			}
			return this._cache[num].VisibleLiquidLevel;
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x00550B04 File Offset: 0x0054ED04
		public void Update(GameTime gameTime)
		{
			if (FocusHelper.PauseLiquidRenderer)
			{
				return;
			}
			float num = Main.windSpeedCurrent * 25f;
			if (num < 0f)
			{
				num -= 6f;
			}
			else
			{
				num += 6f;
			}
			this._frameState += num * (float)gameTime.ElapsedGameTime.TotalSeconds;
			this._waterfallFrameState += 0.5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (this._frameState < 0f)
			{
				this._frameState += 16f;
			}
			this._frameState %= 16f;
			this._waterfallFrameState %= 16f;
			this._animationFrame = (int)this._frameState;
			this._waterfallAnimationFrame = (int)this._waterfallFrameState;
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x00550BDC File Offset: 0x0054EDDC
		public void PrepareDraw(Rectangle drawArea)
		{
			this.InternalPrepareDraw(drawArea);
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x00550BE8 File Offset: 0x0054EDE8
		public void SetWaveMaskData(ref Texture2D texture)
		{
			try
			{
				if (texture == null || texture.Width < this._drawArea.Height || texture.Height < this._drawArea.Width)
				{
					Console.WriteLine("WaveMaskData texture recreated. {0}x{1}", this._drawArea.Height, this._drawArea.Width);
					if (texture != null)
					{
						try
						{
							texture.Dispose();
						}
						catch
						{
						}
					}
					texture = new Texture2D(Main.instance.GraphicsDevice, this._drawArea.Height, this._drawArea.Width, false, SurfaceFormat.Color);
				}
				texture.SetData<Color>(0, new Rectangle?(new Rectangle(0, 0, this._drawArea.Height, this._drawArea.Width)), this._waveMask, 0, this._drawArea.Width * this._drawArea.Height);
			}
			catch
			{
				texture = new Texture2D(Main.instance.GraphicsDevice, this._drawArea.Height, this._drawArea.Width, false, SurfaceFormat.Color);
				texture.SetData<Color>(0, new Rectangle?(new Rectangle(0, 0, this._drawArea.Height, this._drawArea.Width)), this._waveMask, 0, this._drawArea.Width * this._drawArea.Height);
			}
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x00550D60 File Offset: 0x0054EF60
		public Rectangle GetCachedDrawArea()
		{
			return this._drawArea;
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x00550D68 File Offset: 0x0054EF68
		// Note: this type is marked as 'beforefieldinit'.
		static LiquidRenderer()
		{
			byte[] array = new byte[5];
			array[1] = 200;
			array[2] = 240;
			LiquidRenderer.VISCOSITY_MASK = array;
		}

		// Token: 0x04004F27 RID: 20263
		private const int ANIMATION_FRAME_COUNT = 16;

		// Token: 0x04004F28 RID: 20264
		private const int CACHE_PADDING = 2;

		// Token: 0x04004F29 RID: 20265
		private const int CACHE_PADDING_2 = 4;

		// Token: 0x04004F2A RID: 20266
		private static readonly int[] WATERFALL_LENGTH = new int[]
		{
			10,
			3,
			2,
			10
		};

		// Token: 0x04004F2B RID: 20267
		private static readonly float[] DEFAULT_OPACITY = new float[]
		{
			0.6f,
			0.95f,
			0.95f,
			0.75f
		};

		// Token: 0x04004F2C RID: 20268
		private static readonly byte[] WAVE_MASK_STRENGTH = new byte[5];

		// Token: 0x04004F2D RID: 20269
		private static readonly byte[] VISCOSITY_MASK;

		// Token: 0x04004F2F RID: 20271
		public const float MIN_LIQUID_SIZE = 0.25f;

		// Token: 0x04004F30 RID: 20272
		public static LiquidRenderer Instance;

		// Token: 0x04004F31 RID: 20273
		private readonly Asset<Texture2D>[] _liquidTextures = new Asset<Texture2D>[15];

		// Token: 0x04004F32 RID: 20274
		private LiquidRenderer.LiquidCache[] _cache = new LiquidRenderer.LiquidCache[1];

		// Token: 0x04004F33 RID: 20275
		private LiquidRenderer.LiquidDrawCache[] _drawCache = new LiquidRenderer.LiquidDrawCache[1];

		// Token: 0x04004F34 RID: 20276
		private LiquidRenderer.SpecialLiquidDrawCache[] _drawCacheForShimmer = new LiquidRenderer.SpecialLiquidDrawCache[1];

		// Token: 0x04004F35 RID: 20277
		private int _animationFrame;

		// Token: 0x04004F36 RID: 20278
		private int _waterfallAnimationFrame;

		// Token: 0x04004F37 RID: 20279
		private Rectangle _drawArea = new Rectangle(0, 0, 1, 1);

		// Token: 0x04004F38 RID: 20280
		private readonly UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x04004F39 RID: 20281
		private Color[] _waveMask = new Color[1];

		// Token: 0x04004F3A RID: 20282
		private float _frameState;

		// Token: 0x04004F3B RID: 20283
		private float _waterfallFrameState;

		// Token: 0x02000806 RID: 2054
		private struct LiquidCache
		{
			// Token: 0x04007186 RID: 29062
			public float LiquidLevel;

			// Token: 0x04007187 RID: 29063
			public float VisibleLiquidLevel;

			// Token: 0x04007188 RID: 29064
			public float Opacity;

			// Token: 0x04007189 RID: 29065
			public bool IsSolid;

			// Token: 0x0400718A RID: 29066
			public bool IsHalfBrick;

			// Token: 0x0400718B RID: 29067
			public bool HasLiquid;

			// Token: 0x0400718C RID: 29068
			public bool HasVisibleLiquid;

			// Token: 0x0400718D RID: 29069
			public bool HasWall;

			// Token: 0x0400718E RID: 29070
			public Point FrameOffset;

			// Token: 0x0400718F RID: 29071
			public bool HasLeftEdge;

			// Token: 0x04007190 RID: 29072
			public bool HasRightEdge;

			// Token: 0x04007191 RID: 29073
			public bool HasTopEdge;

			// Token: 0x04007192 RID: 29074
			public bool HasBottomEdge;

			// Token: 0x04007193 RID: 29075
			public float LeftWall;

			// Token: 0x04007194 RID: 29076
			public float RightWall;

			// Token: 0x04007195 RID: 29077
			public float BottomWall;

			// Token: 0x04007196 RID: 29078
			public float TopWall;

			// Token: 0x04007197 RID: 29079
			public float VisibleLeftWall;

			// Token: 0x04007198 RID: 29080
			public float VisibleRightWall;

			// Token: 0x04007199 RID: 29081
			public float VisibleBottomWall;

			// Token: 0x0400719A RID: 29082
			public float VisibleTopWall;

			// Token: 0x0400719B RID: 29083
			public byte Type;

			// Token: 0x0400719C RID: 29084
			public byte VisibleType;
		}

		// Token: 0x02000807 RID: 2055
		private struct LiquidDrawCache
		{
			// Token: 0x0400719D RID: 29085
			public Rectangle SourceRectangle;

			// Token: 0x0400719E RID: 29086
			public Vector2 LiquidOffset;

			// Token: 0x0400719F RID: 29087
			public bool IsVisible;

			// Token: 0x040071A0 RID: 29088
			public float Opacity;

			// Token: 0x040071A1 RID: 29089
			public byte Type;

			// Token: 0x040071A2 RID: 29090
			public bool IsSurfaceLiquid;

			// Token: 0x040071A3 RID: 29091
			public bool HasWall;
		}

		// Token: 0x02000808 RID: 2056
		private struct SpecialLiquidDrawCache
		{
			// Token: 0x040071A4 RID: 29092
			public int X;

			// Token: 0x040071A5 RID: 29093
			public int Y;

			// Token: 0x040071A6 RID: 29094
			public Rectangle SourceRectangle;

			// Token: 0x040071A7 RID: 29095
			public Vector2 LiquidOffset;

			// Token: 0x040071A8 RID: 29096
			public bool IsVisible;

			// Token: 0x040071A9 RID: 29097
			public float Opacity;

			// Token: 0x040071AA RID: 29098
			public byte Type;

			// Token: 0x040071AB RID: 29099
			public bool IsSurfaceLiquid;

			// Token: 0x040071AC RID: 29100
			public bool HasWall;
		}
	}
}
