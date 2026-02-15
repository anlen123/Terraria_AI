using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Testing;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x02000445 RID: 1093
	public class WallDrawing : TileDrawingBase
	{
		// Token: 0x060031A8 RID: 12712 RVA: 0x005E1350 File Offset: 0x005DF550
		public void LerpVertexColorsWithColor(ref VertexColors colors, Color lerpColor, float percent)
		{
			colors.TopLeftColor = Color.Lerp(colors.TopLeftColor, lerpColor, percent);
			colors.TopRightColor = Color.Lerp(colors.TopRightColor, lerpColor, percent);
			colors.BottomLeftColor = Color.Lerp(colors.BottomLeftColor, lerpColor, percent);
			colors.BottomRightColor = Color.Lerp(colors.BottomRightColor, lerpColor, percent);
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x005E13A9 File Offset: 0x005DF5A9
		public WallDrawing(TilePaintSystemV2 paintSystem)
		{
			this._paintSystem = paintSystem;
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x005E13B8 File Offset: 0x005DF5B8
		public void Update()
		{
			if (Main.dedServ)
			{
				return;
			}
			this._shouldShowInvisibleWalls = Main.ShouldShowInvisibleBlocksAndWalls();
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x005E13D0 File Offset: 0x005DF5D0
		public static void DrawOutline(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color color)
		{
			Main.spriteBatch.Draw(texture, position, new Rectangle?(sourceRectangle), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x005E1408 File Offset: 0x005DF608
		public void DrawWalls()
		{
			this.FlushLogData = TimeLogger.FlushWallTiles;
			this.DrawCallLogData = TimeLogger.WallDrawCalls;
			if (DebugOptions.hideWalls)
			{
				return;
			}
			float gfxQuality = Main.gfxQuality;
			Vector2 screenPosition = Main.screenPosition;
			int[] wallBlend = Main.wallBlend;
			this._tileArray = Main.tile;
			int num = (int)(120f * (1f - gfxQuality) + 40f * gfxQuality);
			if (DebugOptions.devLightTilesCheat)
			{
				num = 1000;
			}
			int num2 = (int)((float)num * 0.4f);
			int num3 = (int)((float)num * 0.35f);
			int num4 = (int)((float)num * 0.3f);
			Vector2 value;
			int num5;
			int num6;
			int num7;
			int num8;
			TileDrawing.GetScreenDrawArea(!Main.drawToScreen, out value, out num5, out num6, out num7, out num8);
			VertexColors colors = default(VertexColors);
			Rectangle value2 = new Rectangle(0, 0, 32, 32);
			int underworldLayer = Main.UnderworldLayer;
			this._lastPaintLookupKey = default(TilePaintSystemV2.WallVariationKey);
			for (int i = num7; i < num8; i++)
			{
				for (int j = num5; j < num6; j++)
				{
					Tile tile = this._tileArray[j, i];
					if (tile == null)
					{
						tile = new Tile();
						this._tileArray[j, i] = tile;
					}
					ushort wall = tile.wall;
					if (wall > 0 && !this.FullTile(j, i) && (wall != 318 || this._shouldShowInvisibleWalls) && (!tile.invisibleWall() || this._shouldShowInvisibleWalls))
					{
						Color color = Lighting.GetColor(j, i);
						if (tile.fullbrightWall())
						{
							color = Color.White;
						}
						if (wall == 318)
						{
							color = Color.White;
						}
						if (color.R != 0 || color.G != 0 || color.B != 0 || i >= underworldLayer)
						{
							Main.instance.LoadWall((int)wall);
							Texture2D wallDrawTexture = this.GetWallDrawTexture(tile);
							Main.tileBatch.SetLayer((uint)((int)wall | (int)tile.wallColor() << 11), 0);
							value2.X = tile.wallFrameX();
							value2.Y = tile.wallFrameY() + (int)(Main.wallFrame[(int)wall] * 180);
							ushort wall2 = tile.wall;
							if (wall2 - 242 <= 1)
							{
								int num9 = 20;
								int num10 = ((int)Main.wallFrameCounter[(int)wall] + j * 11 + i * 27) % (num9 * 8);
								value2.Y = tile.wallFrameY() + 180 * (num10 / num9);
							}
							if (Lighting.NotRetro && !Main.wallLight[(int)wall] && tile.wall != 241 && (tile.wall < 88 || tile.wall > 93) && !WorldGen.SolidTile(tile))
							{
								if (tile.wall == 346)
								{
									Color color2 = new Color((int)((byte)Main.DiscoR), (int)((byte)Main.DiscoG), (int)((byte)Main.DiscoB));
									colors.BottomLeftColor = color2;
									colors.BottomRightColor = color2;
									colors.TopLeftColor = color2;
									colors.TopRightColor = color2;
								}
								else if (tile.wall == 44)
								{
									Color color3 = new Color((int)((byte)Main.DiscoR), (int)((byte)Main.DiscoG), (int)((byte)Main.DiscoB));
									colors.BottomLeftColor = color3;
									colors.BottomRightColor = color3;
									colors.TopLeftColor = color3;
									colors.TopRightColor = color3;
								}
								else
								{
									Lighting.GetCornerColors(j, i, out colors, 1f);
									wall2 = tile.wall;
									if (wall2 - 341 <= 4)
									{
										this.LerpVertexColorsWithColor(ref colors, Color.White, 0.5f);
									}
									if (tile.fullbrightWall())
									{
										colors = WallDrawing._glowPaintColors;
									}
								}
								Main.tileBatch.Draw(wallDrawTexture, new Vector2((float)(j * 16 - (int)screenPosition.X - 8), (float)(i * 16 - (int)screenPosition.Y - 8)) + value, new Rectangle?(value2), colors, Vector2.Zero, 1f, SpriteEffects.None);
								if (tile.wall == 347)
								{
									Texture2D value3 = TextureAssets.GlowMask[361].Value;
									LiquidRenderer.SetShimmerVertexColors_Sparkle(ref colors, 0.7f, j, i, true);
									Main.tileBatch.Draw(value3, new Vector2((float)(j * 16 - (int)screenPosition.X - 8), (float)(i * 16 - (int)screenPosition.Y - 8)) + value, new Rectangle?(value2), colors, Vector2.Zero, 1f, SpriteEffects.None);
								}
							}
							else
							{
								Color color4 = color;
								if (wall == 44 || wall == 346)
								{
									color4 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
								}
								if (wall - 341 <= 4)
								{
									color4 = Color.Lerp(color4, Color.White, 0.5f);
								}
								Main.tileBatch.Draw(wallDrawTexture, new Vector2((float)(j * 16 - (int)screenPosition.X - 8), (float)(i * 16 - (int)screenPosition.Y - 8)) + value, new Rectangle?(value2), color4, Vector2.Zero, 1f, SpriteEffects.None);
								if (tile.wall == 347)
								{
									Texture2D value4 = TextureAssets.GlowMask[361].Value;
									Color color5 = LiquidRenderer.GetShimmerGlitterColor(true, (float)j, (float)i) * 0.7f;
									Main.tileBatch.Draw(value4, new Vector2((float)(j * 16 - (int)screenPosition.X - 8), (float)(i * 16 - (int)screenPosition.Y - 8)) + value, new Rectangle?(value2), color5, Vector2.Zero, 1f, SpriteEffects.None);
								}
							}
							if ((int)color.R > num2 || (int)color.G > num3 || (int)color.B > num4)
							{
								bool flag = this._tileArray[j - 1, i].wall > 0 && wallBlend[(int)this._tileArray[j - 1, i].wall] != wallBlend[(int)tile.wall];
								bool flag2 = this._tileArray[j + 1, i].wall > 0 && wallBlend[(int)this._tileArray[j + 1, i].wall] != wallBlend[(int)tile.wall];
								bool flag3 = this._tileArray[j, i - 1].wall > 0 && wallBlend[(int)this._tileArray[j, i - 1].wall] != wallBlend[(int)tile.wall];
								bool flag4 = this._tileArray[j, i + 1].wall > 0 && wallBlend[(int)this._tileArray[j, i + 1].wall] != wallBlend[(int)tile.wall];
								if (flag)
								{
									WallDrawing.DrawOutline(TextureAssets.WallOutline.Value, new Vector2((float)(j * 16 - (int)screenPosition.X), (float)(i * 16 - (int)screenPosition.Y)) + value, new Rectangle(0, 0, 2, 16), color);
								}
								if (flag2)
								{
									WallDrawing.DrawOutline(TextureAssets.WallOutline.Value, new Vector2((float)(j * 16 - (int)screenPosition.X + 14), (float)(i * 16 - (int)screenPosition.Y)) + value, new Rectangle(14, 0, 2, 16), color);
								}
								if (flag3)
								{
									WallDrawing.DrawOutline(TextureAssets.WallOutline.Value, new Vector2((float)(j * 16 - (int)screenPosition.X), (float)(i * 16 - (int)screenPosition.Y)) + value, new Rectangle(0, 0, 16, 2), color);
								}
								if (flag4)
								{
									WallDrawing.DrawOutline(TextureAssets.WallOutline.Value, new Vector2((float)(j * 16 - (int)screenPosition.X), (float)(i * 16 - (int)screenPosition.Y + 14)) + value, new Rectangle(0, 14, 16, 2), color);
								}
							}
						}
					}
				}
			}
			base.RestartLayeredBatch();
			Main.instance.DrawTileCracks(2, Main.LocalPlayer.hitReplace);
			Main.instance.DrawTileCracks(2, Main.LocalPlayer.hitTile);
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x005E1C12 File Offset: 0x005DFE12
		public Texture2D GetWallDrawTexture(Tile tile)
		{
			return this.GetWallDrawTexture((int)tile.wall, (int)tile.wallColor());
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x005E1C28 File Offset: 0x005DFE28
		public Texture2D GetWallDrawTexture(int wallType, int paintColor)
		{
			TilePaintSystemV2.WallVariationKey wallVariationKey = new TilePaintSystemV2.WallVariationKey
			{
				WallType = wallType,
				PaintColor = paintColor
			};
			if (this._lastPaintLookupKey == wallVariationKey)
			{
				return this._lastPaintLookupTexture;
			}
			this._lastPaintLookupKey = wallVariationKey;
			this._lastPaintLookupTexture = this.LookupWallDrawTexture(wallVariationKey);
			return this._lastPaintLookupTexture;
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x005E1C80 File Offset: 0x005DFE80
		private Texture2D LookupWallDrawTexture(TilePaintSystemV2.WallVariationKey key)
		{
			if (key.PaintColor != 0)
			{
				Texture2D texture2D = this._paintSystem.TryGetWallAndRequestIfNotReady(key.WallType, key.PaintColor);
				if (texture2D != null)
				{
					return texture2D;
				}
			}
			return TextureAssets.Wall[key.WallType].Value;
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x005E1CC4 File Offset: 0x005DFEC4
		protected bool FullTile(int x, int y)
		{
			if (this._tileArray[x - 1, y] == null || this._tileArray[x - 1, y].blockType() != 0 || this._tileArray[x + 1, y] == null || this._tileArray[x + 1, y].blockType() != 0)
			{
				return false;
			}
			Tile tile = this._tileArray[x, y];
			if (tile == null)
			{
				return false;
			}
			if (tile.active())
			{
				if (Main.tileFrameImportant[(int)tile.type] || TileID.Sets.DrawsWalls[(int)tile.type])
				{
					return false;
				}
				if (tile.invisibleBlock() && !this._shouldShowInvisibleWalls)
				{
					return false;
				}
				if (DebugOptions.ShowUnbreakableWall && tile.wall == 350)
				{
					return false;
				}
				if (Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type])
				{
					int frameX = (int)tile.frameX;
					int frameY = (int)tile.frameY;
					if (Main.tileLargeFrames[(int)tile.type] > 0)
					{
						if (frameY == 18 || frameY == 108)
						{
							if (frameX >= 18 && frameX <= 54)
							{
								return true;
							}
							if (frameX >= 108 && frameX <= 144)
							{
								return true;
							}
						}
					}
					else if (frameY == 0)
					{
						if (frameX >= 180 && frameX <= 198)
						{
							return true;
						}
					}
					else if (frameY == 18)
					{
						if (frameX >= 18 && frameX <= 54)
						{
							return true;
						}
						if (frameX >= 108 && frameX <= 144)
						{
							return true;
						}
						if (frameX >= 180 && frameX <= 198)
						{
							return true;
						}
					}
					else if (frameY == 36)
					{
						if (frameX >= 108 && frameX <= 144)
						{
							return true;
						}
						if (frameX >= 180 && frameX <= 198)
						{
							return true;
						}
					}
					else if (frameY >= 90 && frameY <= 180)
					{
						if (frameX <= 54)
						{
							return true;
						}
						if (frameX >= 144 && frameX <= 216)
						{
							return true;
						}
					}
					else if (frameY == 198 && frameX >= 108 && frameX <= 144)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04005784 RID: 22404
		public static bool QuickPaintLookup = true;

		// Token: 0x04005785 RID: 22405
		private static VertexColors _glowPaintColors = new VertexColors(Color.White);

		// Token: 0x04005786 RID: 22406
		private Tile[,] _tileArray;

		// Token: 0x04005787 RID: 22407
		private TilePaintSystemV2 _paintSystem;

		// Token: 0x04005788 RID: 22408
		private bool _shouldShowInvisibleWalls;

		// Token: 0x04005789 RID: 22409
		private TilePaintSystemV2.WallVariationKey _lastPaintLookupKey;

		// Token: 0x0400578A RID: 22410
		private Texture2D _lastPaintLookupTexture;
	}
}
