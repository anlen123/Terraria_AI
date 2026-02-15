using System;
using Microsoft.Xna.Framework;
using ReLogic.Threading;
using Terraria.GameContent;
using Terraria.GameContent.Liquid;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.Graphics.Light
{
	// Token: 0x020001F9 RID: 505
	public class TileLightScanner
	{
		// Token: 0x060020C0 RID: 8384 RVA: 0x00522DBC File Offset: 0x00520FBC
		public void ExportTo(Rectangle area, LightMap outputMap, TileLightScannerOptions options)
		{
			this._drawInvisibleWalls = options.DrawInvisibleWalls;
			FastParallel.For(area.Left, area.Right, delegate(int start, int end, object context)
			{
				for (int i = start; i < end; i++)
				{
					for (int j = area.Top; j < area.Bottom; j++)
					{
						if (this.IsTileNullOrTouchingNull(i, j))
						{
							outputMap.SetMaskAt(i - area.X, j - area.Y, LightMaskMode.None);
							outputMap[i - area.X, j - area.Y] = Vector3.Zero;
						}
						else
						{
							LightMaskMode tileMask = this.GetTileMask(Main.tile[i, j]);
							outputMap.SetMaskAt(i - area.X, j - area.Y, tileMask);
							Vector3 value;
							this.GetTileLight(i, j, out value);
							outputMap[i - area.X, j - area.Y] = value;
						}
					}
				}
			}, null);
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x00522E18 File Offset: 0x00521018
		private bool IsTileNullOrTouchingNull(int x, int y)
		{
			return !WorldGen.InWorld(x, y, 1) || Main.tile[x, y] == null || Main.tile[x + 1, y] == null || Main.tile[x - 1, y] == null || Main.tile[x, y - 1] == null || Main.tile[x, y + 1] == null;
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x00522E82 File Offset: 0x00521082
		public void Update()
		{
			this._random.NextSeed();
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x00522E8F File Offset: 0x0052108F
		public LightMaskMode GetMaskMode(int x, int y)
		{
			return this.GetTileMask(Main.tile[x, y]);
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x00522EA4 File Offset: 0x005210A4
		private LightMaskMode GetTileMask(Tile tile)
		{
			if (this.LightIsBlocked(tile) && tile.type != 131 && !tile.inActive() && tile.slope() == 0)
			{
				if (TileID.Sets.CrackedBricks[(int)tile.type])
				{
					return LightMaskMode.CrackedBricks;
				}
				return LightMaskMode.Solid;
			}
			else
			{
				if (tile.lava() || tile.liquid <= 128)
				{
					return LightMaskMode.None;
				}
				if (!tile.honey())
				{
					return LightMaskMode.Water;
				}
				return LightMaskMode.Honey;
			}
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x00522F14 File Offset: 0x00521114
		public void GetTileLight(int x, int y, out Vector3 outputColor)
		{
			outputColor = Vector3.Zero;
			Tile tile = Main.tile[x, y];
			FastRandom fastRandom = this._random.WithModifier(x, y);
			if (y <= (int)Main.worldSurface)
			{
				this.ApplySurfaceLight(tile, x, y, ref outputColor);
			}
			else if (y > Main.UnderworldLayer)
			{
				this.ApplyHellLight(tile, x, y, ref outputColor);
			}
			this.ApplyWallLight(tile, x, y, ref fastRandom, ref outputColor);
			if (tile.active())
			{
				this.ApplyTileLight(tile, x, y, ref fastRandom, ref outputColor);
			}
			this.ApplyLiquidLight(tile, ref outputColor);
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x00522F98 File Offset: 0x00521198
		private void ApplyLiquidLight(Tile tile, ref Vector3 lightColor)
		{
			if (tile.liquid <= 0)
			{
				return;
			}
			if (tile.lava())
			{
				float num = 0.55f;
				num += (float)(270 - (int)Main.mouseTextColor) / 900f;
				if (lightColor.X < num)
				{
					lightColor.X = num;
				}
				if (lightColor.Y < num)
				{
					lightColor.Y = num * 0.6f;
				}
				if (lightColor.Z < num)
				{
					lightColor.Z = num * 0.2f;
					return;
				}
			}
			else if (tile.shimmer())
			{
				float num2 = 0.7f;
				float num3 = 0.7f;
				num2 += (float)(270 - (int)Main.mouseTextColor) / 900f;
				num3 += (float)(270 - (int)Main.mouseTextColor) / 125f;
				if (lightColor.X < num2)
				{
					lightColor.X = num2 * 0.6f;
				}
				if (lightColor.Y < num3)
				{
					lightColor.Y = num3 * 0.25f;
				}
				if (lightColor.Z < num2)
				{
					lightColor.Z = num2 * 0.9f;
				}
			}
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x00523092 File Offset: 0x00521292
		private bool LightIsBlocked(Tile tile)
		{
			return tile.active() && Main.tileBlockLight[(int)tile.type] && (!tile.invisibleBlock() || this._drawInvisibleWalls);
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x005230BC File Offset: 0x005212BC
		private void ApplyWallLight(Tile tile, int x, int y, ref FastRandom localRandom, ref Vector3 lightColor)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			bool flag = false;
			ushort wall = tile.wall;
			if (wall <= 156)
			{
				if (wall <= 44)
				{
					if (wall != 33)
					{
						if (wall == 44)
						{
							if (!this.LightIsBlocked(tile))
							{
								num = (float)Main.DiscoR / 255f * 0.15f;
								num2 = (float)Main.DiscoG / 255f * 0.15f;
								num3 = (float)Main.DiscoB / 255f * 0.15f;
							}
						}
					}
					else if (!this.LightIsBlocked(tile))
					{
						num = 0.089999996f;
						num2 = 0.052500002f;
						num3 = 0.24f;
					}
				}
				else if (wall != 137)
				{
					switch (wall)
					{
					case 153:
						num = 0.6f;
						num2 = 0.3f;
						break;
					case 154:
						num = 0.6f;
						num3 = 0.6f;
						break;
					case 155:
						num = 0.6f;
						num2 = 0.6f;
						num3 = 0.6f;
						break;
					case 156:
						num2 = 0.6f;
						break;
					}
				}
				else if (!this.LightIsBlocked(tile))
				{
					float num4 = 0.4f;
					num4 += (float)(270 - (int)Main.mouseTextColor) / 1500f;
					num4 += (float)localRandom.Next(0, 50) * 0.0005f;
					num = 1f * num4;
					num2 = 0.5f * num4;
					num3 = 0.1f * num4;
				}
			}
			else if (wall <= 176)
			{
				switch (wall)
				{
				case 164:
					num = 0.6f;
					break;
				case 165:
					num3 = 0.6f;
					break;
				case 166:
					num = 0.6f;
					num2 = 0.6f;
					break;
				default:
					switch (wall)
					{
					case 174:
						if (!this.LightIsBlocked(tile))
						{
							num = 0.2975f;
						}
						break;
					case 175:
						if (!this.LightIsBlocked(tile))
						{
							if (tile.wallColor() == 0)
							{
								num = 0.075f;
								num2 = 0.15f;
								num3 = 0.4f;
							}
							else
							{
								flag = true;
							}
						}
						break;
					case 176:
						if (!this.LightIsBlocked(tile))
						{
							num = 0.1f;
							num2 = 0.1f;
							num3 = 0.1f;
						}
						break;
					}
					break;
				}
			}
			else if (wall != 182)
			{
				switch (wall)
				{
				case 341:
					if (!this.LightIsBlocked(tile))
					{
						num = 0.25f;
						num2 = 0.1f;
						num3 = 0f;
					}
					break;
				case 342:
					if (!this.LightIsBlocked(tile))
					{
						num = 0.3f;
						num2 = 0f;
						num3 = 0.17f;
					}
					break;
				case 343:
					if (!this.LightIsBlocked(tile))
					{
						num = 0f;
						num2 = 0.25f;
						num3 = 0f;
					}
					break;
				case 344:
					if (!this.LightIsBlocked(tile))
					{
						num = 0f;
						num2 = 0.16f;
						num3 = 0.34f;
					}
					break;
				case 345:
					if (!this.LightIsBlocked(tile))
					{
						num = 0.3f;
						num2 = 0f;
						num3 = 0.35f;
					}
					break;
				case 346:
					if (!this.LightIsBlocked(tile))
					{
						num = (float)Main.DiscoR / 255f * 0.25f;
						num2 = (float)Main.DiscoG / 255f * 0.25f;
						num3 = (float)Main.DiscoB / 255f * 0.25f;
					}
					break;
				default:
					if (wall == 357 && !this.LightIsBlocked(tile))
					{
						num = 0.15f;
						num2 = 0.27f;
						num3 = 0.3f;
						flag = true;
					}
					break;
				}
			}
			else if (!this.LightIsBlocked(tile))
			{
				num = 0.24f;
				num2 = 0.12f;
				num3 = 0.089999996f;
			}
			if (flag && tile.wallColor() != 0)
			{
				Color color = WorldGen.paintColor((int)tile.wallColor());
				num = (float)color.R / 765f;
				num2 = (float)color.G / 765f;
				num3 = (float)color.B / 765f;
			}
			if (lightColor.X < num)
			{
				lightColor.X = num;
			}
			if (lightColor.Y < num2)
			{
				lightColor.Y = num2;
			}
			if (lightColor.Z < num3)
			{
				lightColor.Z = num3;
			}
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x005234FC File Offset: 0x005216FC
		private void ApplyTileLight(Tile tile, int x, int y, ref FastRandom localRandom, ref Vector3 lightColor)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			bool flag = false;
			if (Main.tileLighted[(int)tile.type])
			{
				ushort type = tile.type;
				if (type <= 327)
				{
					if (type <= 133)
					{
						if (type <= 49)
						{
							if (type <= 20)
							{
								if (type != 4)
								{
									if (type == 17)
									{
										goto IL_3670;
									}
									if (type != 20)
									{
										goto IL_45A8;
									}
									int num4 = (int)(tile.frameX / 18);
									if (num4 >= 30 && num4 <= 32)
									{
										num = 0.325f;
										num2 = 0.15f;
										num3 = 0.05f;
										goto IL_45A8;
									}
									goto IL_45A8;
								}
								else
								{
									if (tile.frameX < 66)
									{
										TorchID.TorchColor((int)(tile.frameY / 22), out num, out num2, out num3);
										goto IL_45A8;
									}
									goto IL_45A8;
								}
							}
							else if (type <= 37)
							{
								if (type == 22)
								{
									goto IL_36D2;
								}
								switch (type)
								{
								case 26:
								case 31:
									goto IL_41C8;
								case 27:
									if (tile.frameY < 36)
									{
										num = 0.3f;
										num2 = 0.27f;
										goto IL_45A8;
									}
									goto IL_45A8;
								case 28:
								case 29:
								case 30:
								case 32:
								case 36:
									goto IL_45A8;
								case 33:
									if (tile.frameX == 0)
									{
										switch (tile.frameY / 22)
										{
										case 0:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 1:
											num = 0.55f;
											num2 = 0.85f;
											num3 = 0.35f;
											goto IL_45A8;
										case 2:
											num = 0.65f;
											num2 = 0.95f;
											num3 = 0.5f;
											goto IL_45A8;
										case 3:
											num = 0.2f;
											num2 = 0.75f;
											num3 = 1f;
											goto IL_45A8;
										case 5:
											num = 0.85f;
											num2 = 0.6f;
											num3 = 1f;
											goto IL_45A8;
										case 7:
										case 8:
											num = 0.75f;
											num2 = 0.85f;
											num3 = 1f;
											goto IL_45A8;
										case 9:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 10:
											num = 1f;
											num2 = 0.97f;
											num3 = 0.85f;
											goto IL_45A8;
										case 14:
											num = 1f;
											num2 = 1f;
											num3 = 0.6f;
											goto IL_45A8;
										case 15:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 18:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 19:
											if (tile.color() == 0)
											{
												num = 0.37f;
												num2 = 0.8f;
												num3 = 1f;
												goto IL_45A8;
											}
											flag = true;
											goto IL_45A8;
										case 20:
											num = 0f;
											num2 = 0.9f;
											num3 = 1f;
											goto IL_45A8;
										case 21:
											num = 0.25f;
											num2 = 0.7f;
											num3 = 1f;
											goto IL_45A8;
										case 23:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 24:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 25:
											num = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
											num2 = 0.3f;
											num3 = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
											goto IL_45A8;
										case 28:
											num = 0.9f;
											num2 = 0.75f;
											num3 = 1f;
											goto IL_45A8;
										case 29:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 30:
										{
											Vector3 vector = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f, byte.MaxValue).ToVector3() * 1.2f;
											num = vector.X;
											num2 = vector.Y;
											num3 = vector.Z;
											goto IL_45A8;
										}
										case 31:
											num = 1f;
											num2 = 0.97f;
											num3 = 0.85f;
											goto IL_45A8;
										case 32:
											num = 0.55f;
											num2 = 0.45f;
											num3 = 0.95f;
											goto IL_45A8;
										case 33:
											num = 1f;
											num2 = 0.6f;
											num3 = 0.1f;
											goto IL_45A8;
										case 34:
											num = 0.3f;
											num2 = 0.75f;
											num3 = 0.55f;
											goto IL_45A8;
										case 35:
											num = 0.9f;
											num2 = 0.55f;
											num3 = 0.7f;
											goto IL_45A8;
										case 36:
											num = 0.55f;
											num2 = 0.85f;
											num3 = 1f;
											goto IL_45A8;
										case 37:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 38:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 39:
											num = 0.4f;
											num2 = 0.8f;
											num3 = 0.9f;
											goto IL_45A8;
										case 40:
											num = 1f;
											num2 = 1f;
											num3 = 1f;
											goto IL_45A8;
										case 41:
											num = 0.95f;
											num2 = 0.5f;
											num3 = 0.4f;
											goto IL_45A8;
										case 42:
										{
											Vector4 vector2 = LiquidRenderer.GetShimmerBaseColor((float)x, (float)y) * 1.5f;
											num = MathHelper.Clamp(vector2.X, 0f, 1f);
											num2 = MathHelper.Clamp(vector2.Y, 0f, 1f);
											num3 = MathHelper.Clamp(vector2.Z, 0f, 1f);
											goto IL_45A8;
										}
										case 43:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 44:
											num = 1f;
											num2 = 0.6666667f;
											num3 = 0.7764706f;
											goto IL_45A8;
										case 45:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 46:
											num = 0.9529412f;
											num2 = 0.90588236f;
											num3 = 0.36078432f;
											goto IL_45A8;
										case 47:
											num = 0.63529414f;
											num2 = 0.5019608f;
											num3 = 1f;
											goto IL_45A8;
										case 48:
											num = 1f;
											num2 = 0.39215687f;
											num3 = 0.39215687f;
											goto IL_45A8;
										case 49:
											num = 0.74509805f;
											num2 = 0.74509805f;
											num3 = 1f;
											goto IL_45A8;
										case 50:
											num = 0.6666667f;
											num2 = 0.7058824f;
											num3 = 1f;
											goto IL_45A8;
										case 51:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 52:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.75f;
											goto IL_45A8;
										case 53:
											num = 1f;
											num2 = 0.85499996f;
											num3 = 0.585f;
											goto IL_45A8;
										case 54:
											num = 0.5f;
											num2 = 0.9f;
											num3 = 1f;
											flag = true;
											goto IL_45A8;
										case 55:
											num = 1f;
											num2 = 0.9f;
											num3 = 0.9f;
											goto IL_45A8;
										case 56:
											num = 0.7058824f;
											num2 = 0.9019608f;
											num3 = 1f;
											goto IL_45A8;
										case 57:
											num = 0.5882353f;
											num2 = 0.92156863f;
											num3 = 0.9607843f;
											goto IL_45A8;
										case 58:
											num = 0.6666667f;
											num2 = 0.9607843f;
											num3 = 1f;
											goto IL_45A8;
										case 59:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 60:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 61:
											num = 0.92156863f;
											num2 = 0.4117647f;
											num3 = 1f;
											goto IL_45A8;
										case 62:
											num = 0.74509805f;
											num2 = 0.74509805f;
											num3 = 1f;
											goto IL_45A8;
										case 63:
											num = 0.84313726f;
											num2 = 0.6862745f;
											num3 = 0.9607843f;
											goto IL_45A8;
										}
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									}
									goto IL_45A8;
								case 34:
									if (tile.frameX % 108 < 54)
									{
										int num5 = (int)(tile.frameY / 54);
										switch (num5 + (int)(37 * (tile.frameX / 108)))
										{
										case 7:
											num = 0.95f;
											num2 = 0.95f;
											num3 = 0.5f;
											goto IL_45A8;
										case 8:
											num = 0.85f;
											num2 = 0.6f;
											num3 = 1f;
											goto IL_45A8;
										case 9:
											num = 1f;
											num2 = 0.6f;
											num3 = 0.6f;
											goto IL_45A8;
										case 11:
										case 17:
											num = 0.75f;
											num2 = 0.85f;
											num3 = 1f;
											goto IL_45A8;
										case 12:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 13:
											num = 1f;
											num2 = 0.97f;
											num3 = 0.85f;
											goto IL_45A8;
										case 15:
											num = 1f;
											num2 = 1f;
											num3 = 0.7f;
											goto IL_45A8;
										case 16:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 18:
											num = 1f;
											num2 = 1f;
											num3 = 0.6f;
											goto IL_45A8;
										case 19:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 23:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 24:
											if (tile.color() == 0)
											{
												num = 0.37f;
												num2 = 0.8f;
												num3 = 1f;
												goto IL_45A8;
											}
											flag = true;
											goto IL_45A8;
										case 25:
											num = 0f;
											num2 = 0.9f;
											num3 = 1f;
											goto IL_45A8;
										case 26:
											num = 0.25f;
											num2 = 0.7f;
											num3 = 1f;
											goto IL_45A8;
										case 27:
											num = 0.55f;
											num2 = 0.85f;
											num3 = 0.35f;
											goto IL_45A8;
										case 28:
											num = 0.65f;
											num2 = 0.95f;
											num3 = 0.5f;
											goto IL_45A8;
										case 29:
											num = 0.2f;
											num2 = 0.75f;
											num3 = 1f;
											goto IL_45A8;
										case 30:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 32:
											num = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
											num2 = 0.3f;
											num3 = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
											goto IL_45A8;
										case 35:
											num = 0.9f;
											num2 = 0.75f;
											num3 = 1f;
											goto IL_45A8;
										case 36:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 37:
										{
											Vector3 vector3 = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f, byte.MaxValue).ToVector3() * 1.2f;
											num = vector3.X;
											num2 = vector3.Y;
											num3 = vector3.Z;
											goto IL_45A8;
										}
										case 38:
											num = 1f;
											num2 = 0.97f;
											num3 = 0.85f;
											goto IL_45A8;
										case 39:
											num = 0.55f;
											num2 = 0.45f;
											num3 = 0.95f;
											goto IL_45A8;
										case 40:
											num = 1f;
											num2 = 0.6f;
											num3 = 0.1f;
											goto IL_45A8;
										case 41:
											num = 0.3f;
											num2 = 0.75f;
											num3 = 0.55f;
											goto IL_45A8;
										case 42:
											num = 0.9f;
											num2 = 0.55f;
											num3 = 0.7f;
											goto IL_45A8;
										case 43:
											num = 0.55f;
											num2 = 0.85f;
											num3 = 1f;
											goto IL_45A8;
										case 44:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 45:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 46:
											num = 0.4f;
											num2 = 0.8f;
											num3 = 0.9f;
											goto IL_45A8;
										case 47:
											num = 1f;
											num2 = 1f;
											num3 = 1f;
											goto IL_45A8;
										case 48:
											num = 0.95f;
											num2 = 0.5f;
											num3 = 0.4f;
											goto IL_45A8;
										case 49:
										{
											Vector4 vector4 = LiquidRenderer.GetShimmerBaseColor((float)x, (float)y) * 1.5f;
											num = MathHelper.Clamp(vector4.X, 0f, 1f);
											num2 = MathHelper.Clamp(vector4.Y, 0f, 1f);
											num3 = MathHelper.Clamp(vector4.Z, 0f, 1f);
											goto IL_45A8;
										}
										case 50:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 51:
											num = 1f;
											num2 = 0.6666667f;
											num3 = 0.7764706f;
											goto IL_45A8;
										case 52:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 53:
											num = 0.9529412f;
											num2 = 0.90588236f;
											num3 = 0.36078432f;
											goto IL_45A8;
										case 54:
											num = 0.63529414f;
											num2 = 0.5019608f;
											num3 = 1f;
											goto IL_45A8;
										case 55:
											num = 1f;
											num2 = 0.39215687f;
											num3 = 0.39215687f;
											goto IL_45A8;
										case 56:
											num = 0.74509805f;
											num2 = 0.74509805f;
											num3 = 1f;
											goto IL_45A8;
										case 57:
											num = 0.6666667f;
											num2 = 0.7058824f;
											num3 = 1f;
											goto IL_45A8;
										case 58:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 59:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.75f;
											goto IL_45A8;
										case 60:
											num = 1f;
											num2 = 0.85499996f;
											num3 = 0.585f;
											goto IL_45A8;
										case 61:
											num = 0.5f;
											num2 = 0.9f;
											num3 = 1f;
											flag = true;
											goto IL_45A8;
										case 62:
											num = 1f;
											num2 = 0.9f;
											num3 = 0.9f;
											goto IL_45A8;
										case 63:
											num = 0.7058824f;
											num2 = 0.9019608f;
											num3 = 1f;
											goto IL_45A8;
										case 64:
											num = 0.5882353f;
											num2 = 0.92156863f;
											num3 = 0.9607843f;
											goto IL_45A8;
										case 65:
											num = 0.6666667f;
											num2 = 0.9607843f;
											num3 = 1f;
											goto IL_45A8;
										case 66:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 67:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 68:
											num = 0.92156863f;
											num2 = 0.4117647f;
											num3 = 1f;
											goto IL_45A8;
										case 69:
											num = 0.74509805f;
											num2 = 0.74509805f;
											num3 = 1f;
											goto IL_45A8;
										case 70:
											num = 0.84313726f;
											num2 = 0.6862745f;
											num3 = 0.9607843f;
											goto IL_45A8;
										}
										num = 1f;
										num2 = 0.95f;
										num3 = 0.8f;
										goto IL_45A8;
									}
									goto IL_45A8;
								case 35:
									if (tile.frameX < 36)
									{
										num = 0.75f;
										num2 = 0.6f;
										num3 = 0.3f;
										goto IL_45A8;
									}
									goto IL_45A8;
								case 37:
								{
									float num6 = (float)localRandom.Next(95, 106) * 0.01f;
									num = 0.56f * num6;
									num2 = 0.43f * num6;
									num3 = 0.15f * num6;
									goto IL_45A8;
								}
								default:
									goto IL_45A8;
								}
							}
							else if (type != 42)
							{
								if (type != 49)
								{
									goto IL_45A8;
								}
								if (tile.frameX == 0)
								{
									num = 0f;
									num2 = 0.35f;
									num3 = 0.8f;
									goto IL_45A8;
								}
								goto IL_45A8;
							}
							else
							{
								if (tile.frameX == 0)
								{
									switch (tile.frameY / 36)
									{
									case 0:
										num = 0.7f;
										num2 = 0.65f;
										num3 = 0.55f;
										goto IL_45A8;
									case 1:
										num = 0.9f;
										num2 = 0.75f;
										num3 = 0.6f;
										goto IL_45A8;
									case 2:
										num = 0.8f;
										num2 = 0.6f;
										num3 = 0.6f;
										goto IL_45A8;
									case 3:
										num = 0.65f;
										num2 = 0.5f;
										num3 = 0.2f;
										goto IL_45A8;
									case 4:
										num = 0.5f;
										num2 = 0.7f;
										num3 = 0.4f;
										goto IL_45A8;
									case 5:
										num = 0.9f;
										num2 = 0.4f;
										num3 = 0.2f;
										goto IL_45A8;
									case 6:
										num = 0.7f;
										num2 = 0.75f;
										num3 = 0.3f;
										goto IL_45A8;
									case 7:
									{
										float num7 = Main.demonTorch * 0.2f;
										num = 0.9f - num7;
										num2 = 0.9f - num7;
										num3 = 0.7f + num7;
										goto IL_45A8;
									}
									case 8:
										num = 0.75f;
										num2 = 0.6f;
										num3 = 0.3f;
										goto IL_45A8;
									case 9:
										num = 1f;
										num2 = 0.3f;
										num3 = 0.5f;
										num3 += Main.demonTorch * 0.2f;
										num -= Main.demonTorch * 0.1f;
										num2 -= Main.demonTorch * 0.2f;
										goto IL_45A8;
									case 11:
										num = 0.85f;
										num2 = 0.6f;
										num3 = 1f;
										goto IL_45A8;
									case 14:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									case 15:
									case 16:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									case 17:
										num = 1f;
										num2 = 0.97f;
										num3 = 0.85f;
										goto IL_45A8;
									case 18:
										num = 0.75f;
										num2 = 0.85f;
										num3 = 1f;
										goto IL_45A8;
									case 21:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									case 22:
										num = 1f;
										num2 = 1f;
										num3 = 0.6f;
										goto IL_45A8;
									case 23:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									case 27:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									case 28:
										if (tile.color() == 0)
										{
											num = 0.37f;
											num2 = 0.8f;
											num3 = 1f;
											goto IL_45A8;
										}
										flag = true;
										goto IL_45A8;
									case 29:
										num = 0f;
										num2 = 0.9f;
										num3 = 1f;
										goto IL_45A8;
									case 30:
										num = 0.25f;
										num2 = 0.7f;
										num3 = 1f;
										goto IL_45A8;
									case 32:
										num = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
										num2 = 0.3f;
										num3 = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
										goto IL_45A8;
									case 35:
										num = 0.7f;
										num2 = 0.6f;
										num3 = 0.9f;
										goto IL_45A8;
									case 36:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									case 37:
									{
										Vector3 vector5 = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f, byte.MaxValue).ToVector3() * 1.2f;
										num = vector5.X;
										num2 = vector5.Y;
										num3 = vector5.Z;
										goto IL_45A8;
									}
									case 38:
										num = 1f;
										num2 = 0.97f;
										num3 = 0.85f;
										goto IL_45A8;
									case 39:
										num = 0.55f;
										num2 = 0.45f;
										num3 = 0.95f;
										goto IL_45A8;
									case 40:
										num = 1f;
										num2 = 0.6f;
										num3 = 0.1f;
										goto IL_45A8;
									case 41:
										num = 0.3f;
										num2 = 0.75f;
										num3 = 0.55f;
										goto IL_45A8;
									case 42:
										num = 0.9f;
										num2 = 0.55f;
										num3 = 0.7f;
										goto IL_45A8;
									case 43:
										num = 0.55f;
										num2 = 0.85f;
										num3 = 1f;
										goto IL_45A8;
									case 44:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									case 45:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									case 46:
										num = 0.4f;
										num2 = 0.8f;
										num3 = 0.9f;
										goto IL_45A8;
									case 47:
										num = 1f;
										num2 = 1f;
										num3 = 1f;
										goto IL_45A8;
									case 48:
										num = 0.95f;
										num2 = 0.5f;
										num3 = 0.4f;
										goto IL_45A8;
									case 49:
									{
										Vector4 vector6 = LiquidRenderer.GetShimmerBaseColor((float)x, (float)y) * 1.5f;
										num = MathHelper.Clamp(vector6.X, 0f, 1f);
										num2 = MathHelper.Clamp(vector6.Y, 0f, 1f);
										num3 = MathHelper.Clamp(vector6.Z, 0f, 1f);
										goto IL_45A8;
									}
									case 50:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									case 51:
										num = 1f;
										num2 = 0.6666667f;
										num3 = 0.7764706f;
										goto IL_45A8;
									case 52:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									case 53:
										num = 0.9529412f;
										num2 = 0.90588236f;
										num3 = 0.36078432f;
										goto IL_45A8;
									case 54:
										num = 0.63529414f;
										num2 = 0.5019608f;
										num3 = 1f;
										goto IL_45A8;
									case 55:
										num = 1f;
										num2 = 0.39215687f;
										num3 = 0.39215687f;
										goto IL_45A8;
									case 56:
										num = 0.74509805f;
										num2 = 0.74509805f;
										num3 = 1f;
										goto IL_45A8;
									case 57:
										num = 0.6666667f;
										num2 = 0.7058824f;
										num3 = 1f;
										goto IL_45A8;
									case 58:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									case 59:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.75f;
										goto IL_45A8;
									case 60:
										num = 1f;
										num2 = 0.85499996f;
										num3 = 0.585f;
										goto IL_45A8;
									case 61:
										num = 0.5f;
										num2 = 0.9f;
										num3 = 1f;
										flag = true;
										goto IL_45A8;
									case 62:
										num = 1f;
										num2 = 0.9f;
										num3 = 0.9f;
										goto IL_45A8;
									case 63:
										num = 0.7058824f;
										num2 = 0.9019608f;
										num3 = 1f;
										goto IL_45A8;
									case 64:
										num = 0.5882353f;
										num2 = 0.92156863f;
										num3 = 0.9607843f;
										goto IL_45A8;
									case 65:
										num = 0.6666667f;
										num2 = 0.9607843f;
										num3 = 1f;
										goto IL_45A8;
									case 66:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									case 67:
										num = 1f;
										num2 = 0.95f;
										num3 = 0.65f;
										goto IL_45A8;
									case 68:
										num = 0.92156863f;
										num2 = 0.4117647f;
										num3 = 1f;
										goto IL_45A8;
									case 69:
										num = 0.74509805f;
										num2 = 0.74509805f;
										num3 = 1f;
										goto IL_45A8;
									case 70:
										num = 0.84313726f;
										num2 = 0.6862745f;
										num3 = 0.9607843f;
										goto IL_45A8;
									}
									num = 1f;
									num2 = 1f;
									num3 = 1f;
									goto IL_45A8;
								}
								goto IL_45A8;
							}
						}
						else if (type <= 83)
						{
							if (type <= 72)
							{
								if (type == 61)
								{
									goto IL_4164;
								}
								if (type - 70 > 2)
								{
									goto IL_45A8;
								}
								goto IL_4082;
							}
							else
							{
								if (type == 77)
								{
									num = 0.75f;
									num2 = 0.45f;
									num3 = 0.25f;
									goto IL_45A8;
								}
								if (type != 83)
								{
									goto IL_45A8;
								}
								if (tile.frameX == 18 && !Main.dayTime)
								{
									num = 0.1f;
									num2 = 0.4f;
									num3 = 0.6f;
								}
								if (tile.frameX == 90 && !Main.raining && Main.time > 40500.0)
								{
									num = 0.9f;
									num2 = 0.72f;
									num3 = 0.18f;
									goto IL_45A8;
								}
								goto IL_45A8;
							}
						}
						else if (type <= 100)
						{
							if (type != 84)
							{
								switch (type)
								{
								case 92:
									if (tile.frameY <= 18 && tile.frameX == 0)
									{
										num = 1f;
										num2 = 1f;
										num3 = 1f;
										goto IL_45A8;
									}
									goto IL_45A8;
								case 93:
									if (tile.frameX == 0)
									{
										switch (tile.frameY / 54)
										{
										case 1:
											num = 0.95f;
											num2 = 0.95f;
											num3 = 0.5f;
											goto IL_45A8;
										case 2:
											num = 0.85f;
											num2 = 0.6f;
											num3 = 1f;
											goto IL_45A8;
										case 3:
											num = 0.75f;
											num2 = 1f;
											num3 = 0.6f;
											goto IL_45A8;
										case 4:
										case 5:
											num = 0.75f;
											num2 = 0.85f;
											num3 = 1f;
											goto IL_45A8;
										case 6:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 7:
											num = 1f;
											num2 = 0.97f;
											num3 = 0.85f;
											goto IL_45A8;
										case 9:
											num = 1f;
											num2 = 1f;
											num3 = 0.7f;
											goto IL_45A8;
										case 10:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 12:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 13:
											num = 1f;
											num2 = 1f;
											num3 = 0.6f;
											goto IL_45A8;
										case 14:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 18:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 19:
											if (tile.color() == 0)
											{
												num = 0.37f;
												num2 = 0.8f;
												num3 = 1f;
												goto IL_45A8;
											}
											flag = true;
											goto IL_45A8;
										case 20:
											num = 0f;
											num2 = 0.9f;
											num3 = 1f;
											goto IL_45A8;
										case 21:
											num = 0.25f;
											num2 = 0.7f;
											num3 = 1f;
											goto IL_45A8;
										case 23:
											num = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
											num2 = 0.3f;
											num3 = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
											goto IL_45A8;
										case 24:
											num = 0.35f;
											num2 = 0.5f;
											num3 = 0.3f;
											goto IL_45A8;
										case 25:
											num = 0.34f;
											num2 = 0.4f;
											num3 = 0.31f;
											goto IL_45A8;
										case 26:
											num = 0.25f;
											num2 = 0.32f;
											num3 = 0.5f;
											goto IL_45A8;
										case 29:
											num = 0.9f;
											num2 = 0.75f;
											num3 = 1f;
											goto IL_45A8;
										case 30:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 31:
										{
											Vector3 vector7 = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f, byte.MaxValue).ToVector3() * 1.2f;
											num = vector7.X;
											num2 = vector7.Y;
											num3 = vector7.Z;
											goto IL_45A8;
										}
										case 32:
											num = 1f;
											num2 = 0.97f;
											num3 = 0.85f;
											goto IL_45A8;
										case 33:
											num = 0.55f;
											num2 = 0.45f;
											num3 = 0.95f;
											goto IL_45A8;
										case 34:
											num = 1f;
											num2 = 0.6f;
											num3 = 0.1f;
											goto IL_45A8;
										case 35:
											num = 0.3f;
											num2 = 0.75f;
											num3 = 0.55f;
											goto IL_45A8;
										case 36:
											num = 0.9f;
											num2 = 0.55f;
											num3 = 0.7f;
											goto IL_45A8;
										case 37:
											num = 0.55f;
											num2 = 0.85f;
											num3 = 1f;
											goto IL_45A8;
										case 38:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 39:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 40:
											num = 0.4f;
											num2 = 0.8f;
											num3 = 0.9f;
											goto IL_45A8;
										case 41:
											num = 1f;
											num2 = 1f;
											num3 = 1f;
											goto IL_45A8;
										case 42:
											num = 0.95f;
											num2 = 0.5f;
											num3 = 0.4f;
											goto IL_45A8;
										case 43:
										{
											Vector4 vector8 = LiquidRenderer.GetShimmerBaseColor((float)x, (float)y) * 1.5f;
											num = MathHelper.Clamp(vector8.X, 0f, 1f);
											num2 = MathHelper.Clamp(vector8.Y, 0f, 1f);
											num3 = MathHelper.Clamp(vector8.Z, 0f, 1f);
											goto IL_45A8;
										}
										case 44:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 45:
											num = 1f;
											num2 = 0.6666667f;
											num3 = 0.7764706f;
											goto IL_45A8;
										case 46:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 47:
											num = 0.9529412f;
											num2 = 0.90588236f;
											num3 = 0.36078432f;
											goto IL_45A8;
										case 48:
											num = 0.63529414f;
											num2 = 0.5019608f;
											num3 = 1f;
											goto IL_45A8;
										case 49:
											num = 1f;
											num2 = 0.39215687f;
											num3 = 0.39215687f;
											goto IL_45A8;
										case 50:
											num = 0.74509805f;
											num2 = 0.74509805f;
											num3 = 1f;
											goto IL_45A8;
										case 51:
											num = 0.6666667f;
											num2 = 0.7058824f;
											num3 = 1f;
											goto IL_45A8;
										case 52:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 53:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.75f;
											goto IL_45A8;
										case 54:
											num = 1f;
											num2 = 0.85499996f;
											num3 = 0.585f;
											goto IL_45A8;
										case 55:
											num = 0.5f;
											num2 = 0.9f;
											num3 = 1f;
											flag = true;
											goto IL_45A8;
										case 56:
											num = 1f;
											num2 = 0.9f;
											num3 = 0.9f;
											goto IL_45A8;
										case 57:
											num = 0.7058824f;
											num2 = 0.9019608f;
											num3 = 1f;
											goto IL_45A8;
										case 58:
											num = 0.5882353f;
											num2 = 0.92156863f;
											num3 = 0.9607843f;
											goto IL_45A8;
										case 59:
											num = 0.6666667f;
											num2 = 0.9607843f;
											num3 = 1f;
											goto IL_45A8;
										case 60:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 61:
											num = 1f;
											num2 = 0.95f;
											num3 = 0.65f;
											goto IL_45A8;
										case 62:
											num = 0.92156863f;
											num2 = 0.4117647f;
											num3 = 1f;
											goto IL_45A8;
										case 63:
											num = 0.74509805f;
											num2 = 0.74509805f;
											num3 = 1f;
											goto IL_45A8;
										case 64:
											num = 0.84313726f;
											num2 = 0.6862745f;
											num3 = 0.9607843f;
											goto IL_45A8;
										}
										num = 1f;
										num2 = 0.97f;
										num3 = 0.85f;
										goto IL_45A8;
									}
									goto IL_45A8;
								case 94:
								case 97:
								case 99:
									goto IL_45A8;
								case 95:
									if (tile.frameX < 36)
									{
										num = 1f;
										num2 = 0.95f;
										num3 = 0.8f;
										goto IL_45A8;
									}
									goto IL_45A8;
								case 96:
									if (tile.frameX >= 36)
									{
										num = 0.5f;
										num2 = 0.35f;
										num3 = 0.1f;
										goto IL_45A8;
									}
									goto IL_45A8;
								case 98:
									if (tile.frameY == 0)
									{
										num = 1f;
										num2 = 0.97f;
										num3 = 0.85f;
										goto IL_45A8;
									}
									goto IL_45A8;
								case 100:
									break;
								default:
									goto IL_45A8;
								}
							}
							else
							{
								int num8 = (int)(tile.frameX / 18);
								if (num8 == 2)
								{
									float num9 = (float)(270 - (int)Main.mouseTextColor) / 400f;
									if (num9 > 1f)
									{
										num9 = 1f;
									}
									else if (num9 < 0f)
									{
										num9 = 0f;
									}
									num = num9 * 1.4f;
									num2 = num9 * 1.2f;
									num3 = num9 / 2f;
									goto IL_45A8;
								}
								if (num8 == 5)
								{
									float num9 = 0.9f;
									num = num9;
									num2 = num9 * 0.8f;
									num3 = num9 * 0.2f;
									goto IL_45A8;
								}
								if (num8 == 6)
								{
									float num9 = 0.08f;
									num2 = num9 * 0.8f;
									num3 = num9;
									goto IL_45A8;
								}
								goto IL_45A8;
							}
						}
						else
						{
							switch (type)
							{
							case 125:
							{
								float num10 = (float)localRandom.Next(28, 42) * 0.01f;
								num10 += (float)(270 - (int)Main.mouseTextColor) / 800f;
								num2 = (lightColor.Y = 0.3f * num10);
								num3 = (lightColor.Z = 0.6f * num10);
								goto IL_45A8;
							}
							case 126:
								if (tile.frameX < 36)
								{
									num = (float)Main.DiscoR / 255f;
									num2 = (float)Main.DiscoG / 255f;
									num3 = (float)Main.DiscoB / 255f;
									goto IL_45A8;
								}
								goto IL_45A8;
							case 127:
							case 128:
								goto IL_45A8;
							case 129:
								switch (tile.frameX / 18 % 3)
								{
								case 0:
									num = 0f;
									num2 = 0.05f;
									num3 = 0.25f;
									goto IL_45A8;
								case 1:
									num = 0.2f;
									num2 = 0f;
									num3 = 0.15f;
									goto IL_45A8;
								case 2:
									num = 0.1f;
									num2 = 0f;
									num3 = 0.2f;
									goto IL_45A8;
								default:
									goto IL_45A8;
								}
								break;
							default:
								if (type != 133)
								{
									goto IL_45A8;
								}
								goto IL_3670;
							}
						}
					}
					else if (type <= 204)
					{
						if (type <= 160)
						{
							if (type == 140)
							{
								goto IL_36D2;
							}
							if (type != 149)
							{
								if (type != 160)
								{
									goto IL_45A8;
								}
								num = (float)Main.DiscoR / 255f * 0.25f;
								num2 = (float)Main.DiscoG / 255f * 0.25f;
								num3 = (float)Main.DiscoB / 255f * 0.25f;
								goto IL_45A8;
							}
							else
							{
								if (tile.frameX <= 36)
								{
									switch (tile.frameX / 18)
									{
									case 0:
										num = 0.1f;
										num2 = 0.2f;
										num3 = 0.5f;
										break;
									case 1:
										num = 0.5f;
										num2 = 0.1f;
										num3 = 0.1f;
										break;
									case 2:
										num = 0.2f;
										num2 = 0.5f;
										num3 = 0.1f;
										break;
									}
									num *= (float)localRandom.Next(970, 1031) * 0.001f;
									num2 *= (float)localRandom.Next(970, 1031) * 0.001f;
									num3 *= (float)localRandom.Next(970, 1031) * 0.001f;
									goto IL_45A8;
								}
								goto IL_45A8;
							}
						}
						else if (type <= 184)
						{
							switch (type)
							{
							case 171:
								if (tile.frameX < 10)
								{
									x -= (int)tile.frameX;
									y -= (int)tile.frameY;
								}
								switch ((Main.tile[x, y].frameY & 15360) >> 10)
								{
								case 1:
									num = 0.1f;
									num2 = 0.1f;
									num3 = 0.1f;
									break;
								case 2:
									num = 0.2f;
									break;
								case 3:
									num2 = 0.2f;
									break;
								case 4:
									num3 = 0.2f;
									break;
								case 5:
									num = 0.125f;
									num2 = 0.125f;
									break;
								case 6:
									num = 0.2f;
									num2 = 0.1f;
									break;
								case 7:
									num = 0.125f;
									num2 = 0.125f;
									break;
								case 8:
									num = 0.08f;
									num2 = 0.175f;
									break;
								case 9:
									num2 = 0.125f;
									num3 = 0.125f;
									break;
								case 10:
									num = 0.125f;
									num3 = 0.125f;
									break;
								case 11:
									num = 0.1f;
									num2 = 0.1f;
									num3 = 0.2f;
									break;
								default:
									num2 = (num = (num3 = 0f));
									break;
								}
								num *= 0.5f;
								num2 *= 0.5f;
								num3 *= 0.5f;
								goto IL_45A8;
							case 172:
								goto IL_45A8;
							case 173:
								break;
							case 174:
								if (tile.frameX == 0)
								{
									num = 1f;
									num2 = 0.95f;
									num3 = 0.65f;
									goto IL_45A8;
								}
								goto IL_45A8;
							default:
								if (type != 184)
								{
									goto IL_45A8;
								}
								if (tile.frameX == 110)
								{
									num = 0.25f;
									num2 = 0.1f;
									num3 = 0f;
								}
								if (tile.frameX == 132)
								{
									num = 0f;
									num2 = 0.25f;
									num3 = 0f;
								}
								if (tile.frameX == 154)
								{
									num = 0f;
									num2 = 0.16f;
									num3 = 0.34f;
								}
								if (tile.frameX == 176)
								{
									num = 0.3f;
									num2 = 0f;
									num3 = 0.17f;
								}
								if (tile.frameX == 198)
								{
									num = 0.3f;
									num2 = 0f;
									num3 = 0.35f;
								}
								if (tile.frameX == 220)
								{
									num = (float)Main.DiscoR / 255f * 0.25f;
									num2 = (float)Main.DiscoG / 255f * 0.25f;
									num3 = (float)Main.DiscoB / 255f * 0.25f;
									goto IL_45A8;
								}
								goto IL_45A8;
							}
						}
						else
						{
							if (type == 190)
							{
								goto IL_4082;
							}
							if (type != 204)
							{
								goto IL_45A8;
							}
							goto IL_3832;
						}
					}
					else if (type <= 271)
					{
						if (type <= 215)
						{
							if (type != 209)
							{
								if (type != 215)
								{
									goto IL_45A8;
								}
								if (tile.frameY < 36)
								{
									float num11 = (float)localRandom.Next(28, 42) * 0.005f;
									num11 += (float)(270 - (int)Main.mouseTextColor) / 700f;
									switch (tile.frameX / 54)
									{
									case 1:
										num = 0.7f;
										num2 = 1f;
										num3 = 0.5f;
										break;
									case 2:
										num = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
										num2 = 0.3f;
										num3 = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
										break;
									case 3:
										num = 0.45f;
										num2 = 0.75f;
										num3 = 1f;
										break;
									case 4:
										num = 1.15f;
										num2 = 1.15f;
										num3 = 0.5f;
										break;
									case 5:
										num = (float)Main.DiscoR / 255f;
										num2 = (float)Main.DiscoG / 255f;
										num3 = (float)Main.DiscoB / 255f;
										break;
									case 6:
										num = 0.75f;
										num2 = 1.2824999f;
										num3 = 1.2f;
										break;
									case 7:
										num = 0.95f;
										num2 = 0.65f;
										num3 = 1.3f;
										break;
									case 8:
										num = 1.4f;
										num2 = 0.85f;
										num3 = 0.55f;
										break;
									case 9:
										num = 0.25f;
										num2 = 1.3f;
										num3 = 0.8f;
										break;
									case 10:
										num = 0.95f;
										num2 = 0.4f;
										num3 = 1.4f;
										break;
									case 11:
										num = 1.4f;
										num2 = 0.7f;
										num3 = 0.5f;
										break;
									case 12:
										num = 1.25f;
										num2 = 0.6f;
										num3 = 1.2f;
										break;
									case 13:
										num = 0.75f;
										num2 = 1.45f;
										num3 = 0.9f;
										break;
									case 14:
										num = 0.25f;
										num2 = 0.65f;
										num3 = 1f;
										break;
									case 15:
										TorchID.TorchColor(23, out num, out num2, out num3);
										break;
									default:
										num = 0.9f;
										num2 = 0.3f;
										num3 = 0.1f;
										break;
									}
									num += num11;
									num2 += num11;
									num3 += num11;
									goto IL_45A8;
								}
								goto IL_45A8;
							}
							else
							{
								if (tile.frameX == 234 || tile.frameX == 252)
								{
									Vector3 vector9 = PortalHelper.GetPortalColor(Main.myPlayer, 0).ToVector3() * 0.65f;
									num = vector9.X;
									num2 = vector9.Y;
									num3 = vector9.Z;
									goto IL_45A8;
								}
								if (tile.frameX == 306 || tile.frameX == 324)
								{
									Vector3 vector10 = PortalHelper.GetPortalColor(Main.myPlayer, 1).ToVector3() * 0.65f;
									num = vector10.X;
									num2 = vector10.Y;
									num3 = vector10.Z;
									goto IL_45A8;
								}
								goto IL_45A8;
							}
						}
						else
						{
							switch (type)
							{
							case 235:
								if ((double)lightColor.X < 0.6)
								{
									lightColor.X = 0.6f;
								}
								if ((double)lightColor.Y < 0.6)
								{
									lightColor.Y = 0.6f;
									goto IL_45A8;
								}
								goto IL_45A8;
							case 236:
								goto IL_45A8;
							case 237:
								num = 0.1f;
								num2 = 0.1f;
								goto IL_45A8;
							case 238:
								if ((double)lightColor.X < 0.5)
								{
									lightColor.X = 0.5f;
								}
								if ((double)lightColor.Z < 0.5)
								{
									lightColor.Z = 0.5f;
									goto IL_45A8;
								}
								goto IL_45A8;
							default:
								switch (type)
								{
								case 262:
									num = 0.75f;
									num3 = 0.75f;
									goto IL_45A8;
								case 263:
									num = 0.75f;
									num2 = 0.75f;
									goto IL_45A8;
								case 264:
									num3 = 0.75f;
									goto IL_45A8;
								case 265:
									num2 = 0.75f;
									goto IL_45A8;
								case 266:
									num = 0.75f;
									goto IL_45A8;
								case 267:
									num = 0.75f;
									num2 = 0.75f;
									num3 = 0.75f;
									goto IL_45A8;
								case 268:
									num = 0.75f;
									num2 = 0.375f;
									goto IL_45A8;
								case 269:
									goto IL_45A8;
								case 270:
									num = 0.73f;
									num2 = 1f;
									num3 = 0.41f;
									goto IL_45A8;
								case 271:
									num = 0.45f;
									num2 = 0.95f;
									num3 = 1f;
									goto IL_45A8;
								default:
									goto IL_45A8;
								}
								break;
							}
						}
					}
					else if (type <= 302)
					{
						if (type == 286)
						{
							goto IL_1281;
						}
						if (type != 302)
						{
							goto IL_45A8;
						}
						goto IL_3670;
					}
					else if (type - 316 > 2)
					{
						if (type != 327)
						{
							goto IL_45A8;
						}
						float num12 = 0.5f;
						num12 += (float)(270 - (int)Main.mouseTextColor) / 1500f;
						num12 += (float)localRandom.Next(0, 50) * 0.0005f;
						num = 1f * num12;
						num2 = 0.5f * num12;
						num3 = 0.1f * num12;
						goto IL_45A8;
					}
					else
					{
						int num13 = x - (int)(tile.frameX / 18);
						int num14 = y - (int)(tile.frameY / 18);
						int num15 = num13 / 3 * (num14 / 3);
						num15 %= Main.cageFrames;
						int num16 = (int)(tile.type - 316);
						bool flag2 = Main.jellyfishCageMode[num16, num15] == 2;
						if (tile.type == 316)
						{
							if (flag2)
							{
								num = 0.2f;
								num2 = 0.3f;
								num3 = 0.8f;
							}
							else
							{
								num = 0.1f;
								num2 = 0.2f;
								num3 = 0.5f;
							}
						}
						if (tile.type == 317)
						{
							if (flag2)
							{
								num = 0.2f;
								num2 = 0.7f;
								num3 = 0.3f;
							}
							else
							{
								num = 0.05f;
								num2 = 0.45f;
								num3 = 0.1f;
							}
						}
						if (tile.type != 318)
						{
							goto IL_45A8;
						}
						if (flag2)
						{
							num = 0.7f;
							num2 = 0.2f;
							num3 = 0.5f;
							goto IL_45A8;
						}
						num = 0.4f;
						num2 = 0.1f;
						num3 = 0.25f;
						goto IL_45A8;
					}
					if (tile.frameX < 36)
					{
						switch (tile.frameY / 36)
						{
						case 1:
							num = 0.95f;
							num2 = 0.95f;
							num3 = 0.5f;
							goto IL_45A8;
						case 2:
							num = 0.85f;
							num2 = 0.6f;
							num3 = 1f;
							goto IL_45A8;
						case 3:
							num = 1f;
							num2 = 0.6f;
							num3 = 0.6f;
							goto IL_45A8;
						case 5:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.65f;
							goto IL_45A8;
						case 6:
						case 7:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.65f;
							goto IL_45A8;
						case 8:
							num = 1f;
							num2 = 0.97f;
							num3 = 0.85f;
							goto IL_45A8;
						case 9:
							num = 0.75f;
							num2 = 0.85f;
							num3 = 1f;
							goto IL_45A8;
						case 11:
							num = 1f;
							num2 = 1f;
							num3 = 0.7f;
							goto IL_45A8;
						case 12:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.65f;
							goto IL_45A8;
						case 13:
							num = 1f;
							num2 = 1f;
							num3 = 0.6f;
							goto IL_45A8;
						case 14:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.65f;
							goto IL_45A8;
						case 18:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.65f;
							goto IL_45A8;
						case 19:
							if (tile.color() == 0)
							{
								num = 0.37f;
								num2 = 0.8f;
								num3 = 1f;
								goto IL_45A8;
							}
							flag = true;
							goto IL_45A8;
						case 20:
							num = 0f;
							num2 = 0.9f;
							num3 = 1f;
							goto IL_45A8;
						case 21:
							num = 0.25f;
							num2 = 0.7f;
							num3 = 1f;
							goto IL_45A8;
						case 22:
							num = 0.35f;
							num2 = 0.5f;
							num3 = 0.3f;
							goto IL_45A8;
						case 23:
							num = 0.34f;
							num2 = 0.4f;
							num3 = 0.31f;
							goto IL_45A8;
						case 24:
							num = 0.25f;
							num2 = 0.32f;
							num3 = 0.5f;
							goto IL_45A8;
						case 25:
							num = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
							num2 = 0.3f;
							num3 = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
							goto IL_45A8;
						case 29:
							num = 0.9f;
							num2 = 0.75f;
							num3 = 1f;
							goto IL_45A8;
						case 30:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.65f;
							goto IL_45A8;
						case 31:
						{
							Vector3 vector11 = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f, byte.MaxValue).ToVector3() * 1.2f;
							num = vector11.X;
							num2 = vector11.Y;
							num3 = vector11.Z;
							goto IL_45A8;
						}
						case 32:
							num = 1f;
							num2 = 0.97f;
							num3 = 0.85f;
							goto IL_45A8;
						case 33:
							num = 0.55f;
							num2 = 0.45f;
							num3 = 0.95f;
							goto IL_45A8;
						case 34:
							num = 1f;
							num2 = 0.6f;
							num3 = 0.1f;
							goto IL_45A8;
						case 35:
							num = 0.3f;
							num2 = 0.75f;
							num3 = 0.55f;
							goto IL_45A8;
						case 36:
							num = 0.9f;
							num2 = 0.55f;
							num3 = 0.7f;
							goto IL_45A8;
						case 37:
							num = 0.55f;
							num2 = 0.85f;
							num3 = 1f;
							goto IL_45A8;
						case 38:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.65f;
							goto IL_45A8;
						case 39:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.65f;
							goto IL_45A8;
						case 40:
							num = 0.4f;
							num2 = 0.8f;
							num3 = 0.9f;
							goto IL_45A8;
						case 41:
							num = 1f;
							num2 = 1f;
							num3 = 1f;
							goto IL_45A8;
						case 42:
							num = 0.95f;
							num2 = 0.5f;
							num3 = 0.4f;
							goto IL_45A8;
						case 43:
						{
							Vector4 vector12 = LiquidRenderer.GetShimmerBaseColor((float)x, (float)y) * 1.5f;
							num = MathHelper.Clamp(vector12.X, 0f, 1f);
							num2 = MathHelper.Clamp(vector12.Y, 0f, 1f);
							num3 = MathHelper.Clamp(vector12.Z, 0f, 1f);
							goto IL_45A8;
						}
						case 44:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.65f;
							goto IL_45A8;
						case 45:
							num = 1f;
							num2 = 0.6666667f;
							num3 = 0.7764706f;
							goto IL_45A8;
						case 46:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.65f;
							goto IL_45A8;
						case 47:
							num = 0.9529412f;
							num2 = 0.90588236f;
							num3 = 0.36078432f;
							goto IL_45A8;
						case 48:
							num = 0.63529414f;
							num2 = 0.5019608f;
							num3 = 1f;
							goto IL_45A8;
						case 49:
							num = 1f;
							num2 = 0.39215687f;
							num3 = 0.39215687f;
							goto IL_45A8;
						case 50:
							num = 0.74509805f;
							num2 = 0.74509805f;
							num3 = 1f;
							goto IL_45A8;
						case 51:
							num = 0.6666667f;
							num2 = 0.7058824f;
							num3 = 1f;
							goto IL_45A8;
						case 52:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.65f;
							goto IL_45A8;
						case 53:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.75f;
							goto IL_45A8;
						case 54:
							num = 1f;
							num2 = 0.85499996f;
							num3 = 0.585f;
							goto IL_45A8;
						case 55:
							num = 0.5f;
							num2 = 0.9f;
							num3 = 1f;
							flag = true;
							goto IL_45A8;
						case 56:
							num = 1f;
							num2 = 0.9f;
							num3 = 0.9f;
							goto IL_45A8;
						case 57:
							num = 0.7058824f;
							num2 = 0.9019608f;
							num3 = 1f;
							goto IL_45A8;
						case 58:
							num = 0.5882353f;
							num2 = 0.92156863f;
							num3 = 0.9607843f;
							goto IL_45A8;
						case 59:
							num = 0.6666667f;
							num2 = 0.9607843f;
							num3 = 1f;
							goto IL_45A8;
						case 60:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.65f;
							goto IL_45A8;
						case 61:
							num = 1f;
							num2 = 0.95f;
							num3 = 0.65f;
							goto IL_45A8;
						case 62:
							num = 0.92156863f;
							num2 = 0.4117647f;
							num3 = 1f;
							goto IL_45A8;
						case 63:
							num = 0.74509805f;
							num2 = 0.74509805f;
							num3 = 1f;
							goto IL_45A8;
						case 64:
							num = 0.84313726f;
							num2 = 0.6862745f;
							num3 = 0.9607843f;
							goto IL_45A8;
						}
						num = 1f;
						num2 = 0.95f;
						num3 = 0.65f;
						goto IL_45A8;
					}
					goto IL_45A8;
					IL_3670:
					num = 0.83f;
					num2 = 0.6f;
					num3 = 0.5f;
					goto IL_45A8;
					IL_36D2:
					if (tile.color() != 27 && tile.color() != 26)
					{
						num = 0.12f;
					}
					num2 = 0.07f;
					num3 = 0.32f;
					goto IL_45A8;
				}
				if (type > 540)
				{
					if (type <= 638)
					{
						if (type <= 582)
						{
							if (type != 548)
							{
								switch (type)
								{
								case 564:
									if (tile.frameX < 36)
									{
										num = 0.05f;
										num2 = 0.3f;
										num3 = 0.55f;
										goto IL_45A8;
									}
									goto IL_45A8;
								case 565:
								case 566:
								case 567:
								case 571:
									goto IL_45A8;
								case 568:
									num = 1f;
									num2 = 0.61f;
									num3 = 0.65f;
									goto IL_45A8;
								case 569:
									num = 0.12f;
									num2 = 1f;
									num3 = 0.66f;
									goto IL_45A8;
								case 570:
									num = 0.57f;
									num2 = 0.57f;
									num3 = 1f;
									goto IL_45A8;
								case 572:
									switch (tile.frameY / 36)
									{
									case 0:
										num = 0.9f;
										num2 = 0.5f;
										num3 = 0.7f;
										goto IL_45A8;
									case 1:
										num = 0.7f;
										num2 = 0.55f;
										num3 = 0.96f;
										goto IL_45A8;
									case 2:
										num = 0.45f;
										num2 = 0.96f;
										num3 = 0.95f;
										goto IL_45A8;
									case 3:
										num = 0.5f;
										num2 = 0.96f;
										num3 = 0.62f;
										goto IL_45A8;
									case 4:
										num = 0.47f;
										num2 = 0.69f;
										num3 = 0.95f;
										goto IL_45A8;
									case 5:
										num = 0.92f;
										num2 = 0.57f;
										num3 = 0.51f;
										goto IL_45A8;
									default:
										goto IL_45A8;
									}
									break;
								default:
									switch (type)
									{
									case 578:
										goto IL_4082;
									case 579:
										goto IL_45A8;
									case 580:
										num = 0.7f;
										num2 = 0.3f;
										num3 = 0.2f;
										goto IL_45A8;
									case 581:
										num = 1f;
										num2 = 0.75f;
										num3 = 0.5f;
										goto IL_45A8;
									case 582:
										break;
									default:
										goto IL_45A8;
									}
									break;
								}
							}
							else
							{
								if (tile.frameX / 54 >= 7)
								{
									num = 0.7f;
									num2 = 0.3f;
									num3 = 0.2f;
									goto IL_45A8;
								}
								goto IL_45A8;
							}
						}
						else if (type <= 614)
						{
							switch (type)
							{
							case 592:
								if (tile.frameY > 0)
								{
									float num17 = (float)localRandom.Next(28, 42) * 0.005f;
									num17 += (float)(270 - (int)Main.mouseTextColor) / 700f;
									num = 1.35f;
									num2 = 0.45f;
									num3 = 0.15f;
									num += num17;
									num2 += num17;
									num3 += num17;
									goto IL_45A8;
								}
								goto IL_45A8;
							case 593:
								if (tile.frameX < 18)
								{
									num = 0.8f;
									num2 = 0.3f;
									num3 = 0.1f;
									goto IL_45A8;
								}
								goto IL_45A8;
							case 594:
								if (tile.frameX < 36)
								{
									num = 0.8f;
									num2 = 0.3f;
									num3 = 0.1f;
									goto IL_45A8;
								}
								goto IL_45A8;
							case 595:
							case 596:
								goto IL_45A8;
							case 597:
								switch (tile.frameX / 54)
								{
								case 0:
									num = 0.05f;
									num2 = 0.8f;
									num3 = 0.3f;
									break;
								case 1:
									num = 0.7f;
									num2 = 0.8f;
									num3 = 0.05f;
									break;
								case 2:
									num = 0.7f;
									num2 = 0.5f;
									num3 = 0.9f;
									break;
								case 3:
									num = 0.6f;
									num2 = 0.6f;
									num3 = 0.8f;
									break;
								case 4:
									num = 0.4f;
									num2 = 0.4f;
									num3 = 1.15f;
									break;
								case 5:
									num = 0.85f;
									num2 = 0.45f;
									num3 = 0.1f;
									break;
								case 6:
									num = 0.8f;
									num2 = 0.8f;
									num3 = 1f;
									break;
								case 7:
									num = 0.5f;
									num2 = 0.8f;
									num3 = 1.2f;
									break;
								}
								num *= 0.75f;
								num2 *= 0.75f;
								num3 *= 0.75f;
								goto IL_45A8;
							case 598:
								break;
							default:
								if (type - 613 > 1)
								{
									goto IL_45A8;
								}
								num = 0.7f;
								num2 = 0.3f;
								num3 = 0.2f;
								goto IL_45A8;
							}
						}
						else
						{
							switch (type)
							{
							case 619:
								goto IL_1281;
							case 620:
							{
								Color value = new Color(230, 230, 230, 0).MultiplyRGBA(Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.5f % 1f, 1f, 0.5f, byte.MaxValue));
								value *= 0.4f;
								num = (float)value.R / 255f;
								num2 = (float)value.G / 255f;
								num3 = (float)value.B / 255f;
								goto IL_45A8;
							}
							case 621:
							case 622:
							case 623:
							case 624:
								goto IL_45A8;
							case 625:
							case 626:
								goto IL_B9D;
							case 627:
							case 628:
								goto IL_BB4;
							default:
								switch (type)
								{
								case 633:
								case 637:
								case 638:
									num = 0.325f;
									num2 = 0.15f;
									num3 = 0.05f;
									goto IL_45A8;
								case 634:
									num = 0.65f;
									num2 = 0.3f;
									num3 = 0.1f;
									goto IL_45A8;
								case 635:
								case 636:
									goto IL_45A8;
								default:
									goto IL_45A8;
								}
								break;
							}
						}
						num = 0.7f;
						num2 = 0.2f;
						num3 = 0.1f;
						goto IL_45A8;
					}
					if (type <= 703)
					{
						if (type <= 663)
						{
							if (type != 646)
							{
								switch (type)
								{
								case 656:
									num = 0.2f;
									num2 = 0.55f;
									num3 = 0.5f;
									goto IL_45A8;
								case 657:
								case 661:
								case 662:
									goto IL_45A8;
								case 658:
									if (tile.invisibleBlock())
									{
										goto IL_45A8;
									}
									TorchID.TorchColor(23, out num, out num2, out num3);
									switch (tile.frameY / 54)
									{
									default:
										num *= 0.2f;
										num2 *= 0.2f;
										num3 *= 0.2f;
										goto IL_45A8;
									case 1:
										num *= 0.3f;
										num2 *= 0.3f;
										num3 *= 0.3f;
										goto IL_45A8;
									case 2:
										num *= 0.1f;
										num2 *= 0.1f;
										num3 *= 0.1f;
										goto IL_45A8;
									}
									break;
								case 659:
									break;
								case 660:
									TorchID.TorchColor(23, out num, out num2, out num3);
									goto IL_45A8;
								case 663:
									if (Main.moondialCooldown == 0)
									{
										num = 0f;
										num2 = 0.25f;
										num3 = 0.45f;
										goto IL_45A8;
									}
									goto IL_45A8;
								default:
									goto IL_45A8;
								}
							}
							else
							{
								if (tile.frameX == 0)
								{
									num = 0.2f;
									num2 = 0.3f;
									num3 = 0.32f;
									goto IL_45A8;
								}
								goto IL_45A8;
							}
						}
						else if (type != 667)
						{
							switch (type)
							{
							case 687:
								goto IL_B41;
							case 688:
								goto IL_B86;
							case 689:
								goto IL_B58;
							case 690:
								goto IL_B6F;
							case 691:
								goto IL_B9D;
							case 692:
								goto IL_BB4;
							case 693:
							case 694:
							case 697:
							case 698:
							case 700:
							case 701:
							case 702:
								goto IL_45A8;
							case 695:
							case 696:
								goto IL_41C8;
							case 699:
								num = 0.4f;
								num2 = 0.2f;
								num3 = 0.15f;
								goto IL_45A8;
							case 703:
								goto IL_4164;
							default:
								goto IL_45A8;
							}
						}
					}
					else if (type <= 711)
					{
						if (type != 708)
						{
							if (type != 711)
							{
								goto IL_45A8;
							}
							num = 0.01f;
							num2 = 0.01f;
							num3 = 0.01f;
							goto IL_45A8;
						}
					}
					else
					{
						switch (type)
						{
						case 717:
						{
							float num12 = 0.55f;
							num12 += (float)(270 - (int)Main.mouseTextColor) / 800f;
							num12 += localRandom.NextFloat() * 0.03f;
							num12 *= 0.5f;
							num = num12 * 1.1f;
							num2 = num12 * 0.4f;
							num3 = num12 * 0.1f;
							goto IL_45A8;
						}
						case 718:
							if (!Main.dayTime && !WorldGen.SolidTile3(x, y - 1))
							{
								num = localRandom.NextFloat() * 0.04f + 0.1f + (float)Main.DiscoR / 800f;
								num2 = localRandom.NextFloat() * 0.04f + 0.1f + (float)Main.DiscoG / 800f;
								num3 = localRandom.NextFloat() * 0.04f + 0.1f + (float)Main.DiscoB / 800f;
								goto IL_45A8;
							}
							goto IL_45A8;
						case 719:
						{
							int num18 = (x + y + (int)(Main.GlobalTimeWrappedHourly * 15f)) % 14;
							float num19 = 0f;
							float num20 = 0f;
							float num21 = 0f;
							switch (num18)
							{
							case 0:
								num19 = 255f;
								num20 = 171f;
								num21 = 183f;
								break;
							case 1:
								num19 = 255f;
								num20 = 170f;
								num21 = 220f;
								break;
							case 2:
								num19 = 252f;
								num20 = 171f;
								num21 = 255f;
								break;
							case 3:
								num19 = 224f;
								num20 = 171f;
								num21 = 255f;
								break;
							case 4:
								num19 = 192f;
								num20 = 171f;
								num21 = 255f;
								break;
							case 5:
								num19 = 174f;
								num20 = 178f;
								num21 = 255f;
								break;
							case 6:
								num19 = 168f;
								num20 = 195f;
								num21 = 255f;
								break;
							case 7:
								num19 = 167f;
								num20 = 224f;
								num21 = 255f;
								break;
							case 8:
								num19 = 168f;
								num20 = 255f;
								num21 = 252f;
								break;
							case 9:
								num19 = 162f;
								num20 = 255f;
								num21 = 233f;
								break;
							case 10:
								num19 = 158f;
								num20 = 255f;
								num21 = 198f;
								break;
							case 11:
								num19 = 207f;
								num20 = 255f;
								num21 = 173f;
								break;
							case 12:
								num19 = 255f;
								num20 = 213f;
								num21 = 186f;
								break;
							case 13:
								num19 = 255f;
								num20 = 192f;
								num21 = 182f;
								break;
							}
							num = num19 / 255f;
							num2 = num20 / 255f;
							num3 = num21 / 255f;
							goto IL_45A8;
						}
						default:
							if (type != 739)
							{
								goto IL_45A8;
							}
							num = 0.35f;
							num2 = 0.63f;
							num3 = 0.7f;
							flag = true;
							goto IL_45A8;
						}
					}
					Vector4 shimmerBaseColor = LiquidRenderer.GetShimmerBaseColor((float)x, (float)y);
					num = shimmerBaseColor.X;
					num2 = shimmerBaseColor.Y;
					num3 = shimmerBaseColor.Z;
					goto IL_45A8;
					IL_B9D:
					num = 0.3f;
					num2 = 0f;
					num3 = 0.35f;
					goto IL_45A8;
					IL_BB4:
					num = (float)Main.DiscoR / 255f * 0.25f;
					num2 = (float)Main.DiscoG / 255f * 0.25f;
					num3 = (float)Main.DiscoB / 255f * 0.25f;
					goto IL_45A8;
				}
				if (type <= 405)
				{
					if (type <= 372)
					{
						switch (type)
						{
						case 336:
							num = 0.85f;
							num2 = 0.5f;
							num3 = 0.3f;
							goto IL_45A8;
						case 337:
						case 338:
						case 339:
						case 345:
						case 346:
						case 351:
						case 352:
						case 353:
						case 355:
							goto IL_45A8;
						case 340:
							num = 0.45f;
							num2 = 1f;
							num3 = 0.45f;
							goto IL_45A8;
						case 341:
							num = 0.4f * Main.demonTorch + 0.6f * (1f - Main.demonTorch);
							num2 = 0.35f;
							num3 = 1f * Main.demonTorch + 0.6f * (1f - Main.demonTorch);
							goto IL_45A8;
						case 342:
							num = 0.5f;
							num2 = 0.5f;
							num3 = 1.1f;
							goto IL_45A8;
						case 343:
							num = 0.85f;
							num2 = 0.85f;
							num3 = 0.3f;
							goto IL_45A8;
						case 344:
							num = 0.6f;
							num2 = 1.026f;
							num3 = 0.96000004f;
							goto IL_45A8;
						case 347:
							goto IL_3832;
						case 348:
						case 349:
							goto IL_4082;
						case 350:
						{
							double num22 = Main.timeForVisualEffects * 0.08;
							num2 = (num3 = (num = (float)(-(float)Math.Cos(((int)(num22 / 6.283) % 3 == 1) ? num22 : 0.0) * 0.1 + 0.1)));
							goto IL_45A8;
						}
						case 354:
							num = 0.65f;
							num2 = 0.35f;
							num3 = 0.15f;
							goto IL_45A8;
						case 356:
							if (Main.sundialCooldown == 0)
							{
								num = 0.45f;
								num2 = 0.25f;
								num3 = 0f;
								goto IL_45A8;
							}
							goto IL_45A8;
						default:
							if (type == 370)
							{
								num = 0.32f;
								num2 = 0.16f;
								num3 = 0.12f;
								goto IL_45A8;
							}
							if (type != 372)
							{
								goto IL_45A8;
							}
							if (tile.frameX == 0)
							{
								num = 0.9f;
								num2 = 0.1f;
								num3 = 0.75f;
								goto IL_45A8;
							}
							goto IL_45A8;
						}
					}
					else if (type <= 390)
					{
						if (type != 381)
						{
							if (type != 390)
							{
								goto IL_45A8;
							}
							num = 0.4f;
							num2 = 0.2f;
							num3 = 0.1f;
							goto IL_45A8;
						}
					}
					else
					{
						if (type == 391)
						{
							num = 0.3f;
							num2 = 0.1f;
							num3 = 0.25f;
							goto IL_45A8;
						}
						if (type != 405)
						{
							goto IL_45A8;
						}
						if (tile.frameX < 54)
						{
							float num23 = (float)localRandom.Next(28, 42) * 0.005f;
							num23 += (float)(270 - (int)Main.mouseTextColor) / 700f;
							switch (tile.frameX / 54)
							{
							case 1:
								num = 0.7f;
								num2 = 1f;
								num3 = 0.5f;
								break;
							case 2:
								num = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
								num2 = 0.3f;
								num3 = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
								break;
							case 3:
								num = 0.45f;
								num2 = 0.75f;
								num3 = 1f;
								break;
							case 4:
								num = 1.15f;
								num2 = 1.15f;
								num3 = 0.5f;
								break;
							case 5:
								num = (float)Main.DiscoR / 255f;
								num2 = (float)Main.DiscoG / 255f;
								num3 = (float)Main.DiscoB / 255f;
								break;
							default:
								num = 0.9f;
								num2 = 0.3f;
								num3 = 0.1f;
								break;
							}
							num += num23;
							num2 += num23;
							num3 += num23;
							goto IL_45A8;
						}
						goto IL_45A8;
					}
				}
				else if (type <= 491)
				{
					if (type <= 429)
					{
						switch (type)
						{
						case 415:
							num = 0.7f;
							num2 = 0.5f;
							num3 = 0.1f;
							goto IL_45A8;
						case 416:
							num = 0f;
							num2 = 0.6f;
							num3 = 0.7f;
							goto IL_45A8;
						case 417:
							num = 0.6f;
							num2 = 0.2f;
							num3 = 0.6f;
							goto IL_45A8;
						case 418:
							num = 0.6f;
							num2 = 0.6f;
							num3 = 0.9f;
							goto IL_45A8;
						default:
						{
							if (type != 429)
							{
								goto IL_45A8;
							}
							short num24 = tile.frameX / 18;
							bool flag3 = num24 % 2 >= 1;
							bool flag4 = num24 % 4 >= 2;
							bool flag5 = num24 % 8 >= 4;
							bool flag6 = num24 % 16 >= 8;
							if (flag3)
							{
								num += 0.5f;
							}
							if (flag4)
							{
								num2 += 0.5f;
							}
							if (flag5)
							{
								num3 += 0.5f;
							}
							if (flag6)
							{
								num += 0.2f;
								num2 += 0.2f;
								goto IL_45A8;
							}
							goto IL_45A8;
						}
						}
					}
					else
					{
						if (type == 463)
						{
							num = 0.2f;
							num2 = 0.4f;
							num3 = 0.8f;
							goto IL_45A8;
						}
						if (type != 491)
						{
							goto IL_45A8;
						}
						num = 0.5f;
						num2 = 0.4f;
						num3 = 0.7f;
						goto IL_45A8;
					}
				}
				else if (type <= 517)
				{
					switch (type)
					{
					case 500:
						num = 0.525f;
						num2 = 0.375f;
						num3 = 0.075f;
						goto IL_45A8;
					case 501:
						num = 0f;
						num2 = 0.45f;
						num3 = 0.525f;
						goto IL_45A8;
					case 502:
						num = 0.45f;
						num2 = 0.15f;
						num3 = 0.45f;
						goto IL_45A8;
					case 503:
						num = 0.45f;
						num2 = 0.45f;
						num3 = 0.675f;
						goto IL_45A8;
					default:
						if (type != 517)
						{
							goto IL_45A8;
						}
						break;
					}
				}
				else if (type != 519)
				{
					switch (type)
					{
					case 528:
						goto IL_4082;
					case 529:
					case 530:
					case 531:
					case 532:
					case 533:
					case 538:
						goto IL_45A8;
					case 534:
					case 535:
						goto IL_B58;
					case 536:
					case 537:
						goto IL_B6F;
					case 539:
					case 540:
						goto IL_B86;
					default:
						goto IL_45A8;
					}
				}
				else
				{
					if (tile.frameY != 90)
					{
						goto IL_45A8;
					}
					if (tile.color() == 0)
					{
						float num25 = (float)localRandom.Next(28, 42) * 0.005f;
						num25 += (float)(270 - (int)Main.mouseTextColor) / 1000f;
						num = 0.1f;
						num2 = 0.2f + num25 / 2f;
						num3 = 0.7f + num25;
						goto IL_45A8;
					}
					flag = true;
					goto IL_45A8;
				}
				IL_B41:
				num = 0.25f;
				num2 = 0.1f;
				num3 = 0f;
				goto IL_45A8;
				IL_B58:
				num = 0f;
				num2 = 0.25f;
				num3 = 0f;
				goto IL_45A8;
				IL_B6F:
				num = 0f;
				num2 = 0.16f;
				num3 = 0.34f;
				goto IL_45A8;
				IL_B86:
				num = 0.3f;
				num2 = 0f;
				num3 = 0.17f;
				goto IL_45A8;
				IL_1281:
				num = 0.1f;
				num2 = 0.2f;
				num3 = 0.7f;
				goto IL_45A8;
				IL_3832:
				if (tile.color() != 27 && tile.color() != 26)
				{
					num = 0.35f;
					goto IL_45A8;
				}
				goto IL_45A8;
				IL_4082:
				if (tile.type == 349 && tile.frameX < 36)
				{
					goto IL_45A8;
				}
				float num26 = (float)localRandom.Next(28, 42) * 0.005f;
				num26 += (float)(270 - (int)Main.mouseTextColor) / 1000f;
				if (tile.color() == 0)
				{
					num = 0f;
					num2 = 0.2f + num26 / 2f;
					num3 = 1f;
					goto IL_45A8;
				}
				flag = true;
				goto IL_45A8;
				IL_4164:
				if (tile.frameX == 144)
				{
					float num27 = 1f + (float)(270 - (int)Main.mouseTextColor) / 400f;
					float num28 = 0.8f - (float)(270 - (int)Main.mouseTextColor) / 400f;
					num = 0.42f * num28;
					num2 = 0.81f * num27;
					num3 = 0.52f * num28;
					goto IL_45A8;
				}
				goto IL_45A8;
				IL_41C8:
				if (((tile.type == 31 || tile.type == 696) && tile.frameX >= 36) || ((tile.type == 26 || tile.type == 695) && tile.frameX >= 54))
				{
					float num29 = (float)localRandom.Next(-5, 6) * 0.0025f;
					num = 0.5f + num29 * 2f;
					num2 = 0.2f + num29;
					num3 = 0.1f;
				}
				else
				{
					float num30 = (float)localRandom.Next(-5, 6) * 0.0025f;
					num = 0.31f + num30;
					num2 = 0.1f;
					num3 = 0.44f + num30 * 2f;
				}
			}
			IL_45A8:
			if (flag && tile.color() != 0)
			{
				Color color = WorldGen.paintColor((int)tile.color());
				num = (float)color.R / 255f;
				num2 = (float)color.G / 255f;
				num3 = (float)color.B / 255f;
			}
			if (lightColor.X < num)
			{
				lightColor.X = num;
			}
			if (lightColor.Y < num2)
			{
				lightColor.Y = num2;
			}
			if (lightColor.Z < num3)
			{
				lightColor.Z = num3;
			}
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x00527B2C File Offset: 0x00525D2C
		private void ApplySurfaceLight(Tile tile, int x, int y, ref Vector3 lightColor)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = (float)Main.tileColor.R / 255f;
			float num5 = (float)Main.tileColor.G / 255f;
			float num6 = (float)Main.tileColor.B / 255f;
			float num7 = (num4 + num5 + num6) / 3f;
			if (tile.active() && TileID.Sets.AllowLightInWater[(int)tile.type])
			{
				if (lightColor.X < num7 && (Main.wallLight[(int)tile.wall] || tile.wall == 73 || tile.wall == 227 || (tile.invisibleWall() && !this._drawInvisibleWalls)))
				{
					num = num4;
					num2 = num5;
					num3 = num6;
				}
			}
			else if ((!tile.active() || !Main.tileNoSunLight[(int)tile.type] || ((tile.slope() != 0 || tile.halfBrick() || (tile.invisibleBlock() && !this._drawInvisibleWalls)) && Main.tile[x, y - 1].liquid == 0 && Main.tile[x, y + 1].liquid == 0 && Main.tile[x - 1, y].liquid == 0 && Main.tile[x + 1, y].liquid == 0)) && lightColor.X < num7 && (Main.wallLight[(int)tile.wall] || tile.wall == 73 || tile.wall == 227 || (tile.invisibleWall() && !this._drawInvisibleWalls)))
			{
				if (tile.liquid < 200)
				{
					if (!tile.halfBrick() || Main.tile[x, y - 1].liquid < 200)
					{
						num = num4;
						num2 = num5;
						num3 = num6;
					}
				}
				else if (Main.liquidAlpha[13] > 0f)
				{
					if (Main.rand == null)
					{
						Main.rand = new UnifiedRandom();
					}
					num3 = num6 * 0.175f * (1f + Main.rand.NextFloat() * 0.13f) * Main.liquidAlpha[13];
				}
			}
			if ((!tile.active() || tile.halfBrick() || !Main.tileNoSunLight[(int)tile.type]) && ((tile.wall >= 88 && tile.wall <= 93) || tile.wall == 241) && tile.liquid < 255)
			{
				num = num4;
				num2 = num5;
				num3 = num6;
				int num8 = (int)(tile.wall - 88);
				if (tile.wall == 241)
				{
					num8 = 6;
				}
				switch (num8)
				{
				case 0:
					num *= 0.9f;
					num2 *= 0.15f;
					num3 *= 0.9f;
					break;
				case 1:
					num *= 0.9f;
					num2 *= 0.9f;
					num3 *= 0.15f;
					break;
				case 2:
					num *= 0.15f;
					num2 *= 0.15f;
					num3 *= 0.9f;
					break;
				case 3:
					num *= 0.15f;
					num2 *= 0.9f;
					num3 *= 0.15f;
					break;
				case 4:
					num *= 0.9f;
					num2 *= 0.15f;
					num3 *= 0.15f;
					break;
				case 5:
				{
					float num9 = 0.2f;
					float num10 = 0.7f - num9;
					num *= num10 + (float)Main.DiscoR / 255f * num9;
					num2 *= num10 + (float)Main.DiscoG / 255f * num9;
					num3 *= num10 + (float)Main.DiscoB / 255f * num9;
					break;
				}
				case 6:
					num *= 0.9f;
					num2 *= 0.5f;
					num3 *= 0f;
					break;
				}
			}
			float num11 = 1f - Main.shimmerDarken;
			num *= num11;
			num2 *= num11;
			num3 *= num11;
			if (lightColor.X < num)
			{
				lightColor.X = num;
			}
			if (lightColor.Y < num2)
			{
				lightColor.Y = num2;
			}
			if (lightColor.Z < num3)
			{
				lightColor.Z = num3;
			}
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x00527F58 File Offset: 0x00526158
		private void ApplyHellLight(Tile tile, int x, int y, ref Vector3 lightColor)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0.55f + (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 2f)) * 0.08f;
			if ((!tile.active() || !Main.tileNoSunLight[(int)tile.type] || ((tile.slope() != 0 || tile.halfBrick()) && Main.tile[x, y - 1].liquid == 0 && Main.tile[x, y + 1].liquid == 0 && Main.tile[x - 1, y].liquid == 0 && Main.tile[x + 1, y].liquid == 0)) && lightColor.X < num4 && (Main.wallLight[(int)tile.wall] || tile.wall == 73 || tile.wall == 227 || (tile.invisibleWall() && !this._drawInvisibleWalls)) && tile.liquid < 200 && (!tile.halfBrick() || Main.tile[x, y - 1].liquid < 200))
			{
				num = num4;
				num2 = num4 * 0.6f;
				num3 = num4 * 0.2f;
			}
			if ((!tile.active() || tile.halfBrick() || !Main.tileNoSunLight[(int)tile.type]) && ((tile.wall >= 88 && tile.wall <= 93) || tile.wall == 241) && tile.liquid < 255)
			{
				num = num4;
				num2 = num4 * 0.6f;
				num3 = num4 * 0.2f;
				int num5 = (int)(tile.wall - 88);
				if (tile.wall == 241)
				{
					num5 = 6;
				}
				switch (num5)
				{
				case 0:
					num *= 0.9f;
					num2 *= 0.15f;
					num3 *= 0.9f;
					break;
				case 1:
					num *= 0.9f;
					num2 *= 0.9f;
					num3 *= 0.15f;
					break;
				case 2:
					num *= 0.15f;
					num2 *= 0.15f;
					num3 *= 0.9f;
					break;
				case 3:
					num *= 0.15f;
					num2 *= 0.9f;
					num3 *= 0.15f;
					break;
				case 4:
					num *= 0.9f;
					num2 *= 0.15f;
					num3 *= 0.15f;
					break;
				case 5:
				{
					float num6 = 0.2f;
					float num7 = 0.7f - num6;
					num *= num7 + (float)Main.DiscoR / 255f * num6;
					num2 *= num7 + (float)Main.DiscoG / 255f * num6;
					num3 *= num7 + (float)Main.DiscoB / 255f * num6;
					break;
				}
				case 6:
					num *= 0.9f;
					num2 *= 0.5f;
					num3 *= 0f;
					break;
				}
			}
			if (lightColor.X < num)
			{
				lightColor.X = num;
			}
			if (lightColor.Y < num2)
			{
				lightColor.Y = num2;
			}
			if (lightColor.Z < num3)
			{
				lightColor.Z = num3;
			}
		}

		// Token: 0x04004B24 RID: 19236
		private FastRandom _random = FastRandom.CreateWithRandomSeed();

		// Token: 0x04004B25 RID: 19237
		private bool _drawInvisibleWalls;
	}
}
