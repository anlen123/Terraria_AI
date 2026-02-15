using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Utilities;

namespace Terraria
{
	// Token: 0x02000027 RID: 39
	public class HitTile
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x00015A64 File Offset: 0x00013C64
		public static void ClearAllTilesAtThisLocation(int x, int y)
		{
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active)
				{
					Main.player[i].hitTile.ClearThisTile(x, y);
				}
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00015AA4 File Offset: 0x00013CA4
		public void ClearThisTile(int x, int y)
		{
			for (int i = 0; i <= 500; i++)
			{
				int num = this.order[i];
				HitTile.HitTileObject hitTileObject = this.data[num];
				if (hitTileObject.X == x && hitTileObject.Y == y)
				{
					this.Clear(i);
					this.Prune();
				}
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00015AF4 File Offset: 0x00013CF4
		public HitTile()
		{
			HitTile.rand = new UnifiedRandom();
			this.data = new HitTile.HitTileObject[501];
			this.order = new int[501];
			for (int i = 0; i <= 500; i++)
			{
				this.data[i] = new HitTile.HitTileObject();
				this.order[i] = i;
			}
			this.bufferLocation = 0;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00015B60 File Offset: 0x00013D60
		public int TryFinding(int x, int y, int hitType)
		{
			for (int i = 0; i <= 500; i++)
			{
				int num = this.order[i];
				HitTile.HitTileObject hitTileObject = this.data[num];
				if (hitTileObject.type == hitType)
				{
					if (hitTileObject.X == x && hitTileObject.Y == y)
					{
						return num;
					}
				}
				else if (i != 0 && hitTileObject.type == 0)
				{
					break;
				}
			}
			return -1;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00015BB8 File Offset: 0x00013DB8
		public void TryClearingAndPruning(int x, int y, int hitType)
		{
			int num = this.TryFinding(x, y, hitType);
			if (num != -1)
			{
				this.Clear(num);
				this.Prune();
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00015BE0 File Offset: 0x00013DE0
		public int HitObject(int x, int y, int hitType)
		{
			HitTile.HitTileObject hitTileObject;
			for (int i = 0; i <= 500; i++)
			{
				int num = this.order[i];
				hitTileObject = this.data[num];
				if (hitTileObject.type == hitType)
				{
					if (hitTileObject.X == x && hitTileObject.Y == y)
					{
						return num;
					}
				}
				else if (i != 0 && hitTileObject.type == 0)
				{
					break;
				}
			}
			hitTileObject = this.data[this.bufferLocation];
			hitTileObject.X = x;
			hitTileObject.Y = y;
			hitTileObject.type = hitType;
			return this.bufferLocation;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00015C60 File Offset: 0x00013E60
		public void UpdatePosition(int tileId, int x, int y)
		{
			if (tileId < 0 || tileId > 500)
			{
				return;
			}
			HitTile.HitTileObject hitTileObject = this.data[tileId];
			hitTileObject.X = x;
			hitTileObject.Y = y;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00015C84 File Offset: 0x00013E84
		public int AddDamage(int tileId, int damageAmount, bool updateAmount = true)
		{
			if (tileId < 0 || tileId > 500)
			{
				return 0;
			}
			if (tileId == this.bufferLocation && damageAmount == 0)
			{
				return 0;
			}
			HitTile.HitTileObject hitTileObject = this.data[tileId];
			if (!updateAmount)
			{
				return hitTileObject.damage + damageAmount;
			}
			hitTileObject.damage += damageAmount;
			hitTileObject.timeToLive = 60;
			hitTileObject.animationTimeElapsed = 0;
			hitTileObject.animationDirection = (Main.rand.NextFloat() * 6.2831855f).ToRotationVector2() * 2f;
			this.SortSlots(tileId);
			return hitTileObject.damage;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00015D14 File Offset: 0x00013F14
		private void SortSlots(int tileId)
		{
			if (tileId == this.bufferLocation)
			{
				this.bufferLocation = this.order[500];
				if (tileId != this.bufferLocation)
				{
					this.data[this.bufferLocation].Clear();
				}
				for (int i = 500; i > 0; i--)
				{
					this.order[i] = this.order[i - 1];
				}
				this.order[0] = this.bufferLocation;
				return;
			}
			for (int i = 0; i <= 500; i++)
			{
				if (this.order[i] == tileId)
				{
					IL_AE:
					while (i > 1)
					{
						int num = this.order[i - 1];
						this.order[i - 1] = this.order[i];
						this.order[i] = num;
						i--;
					}
					this.order[1] = tileId;
					return;
				}
			}
			goto IL_AE;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00015DDC File Offset: 0x00013FDC
		public void Clear(int tileId)
		{
			if (tileId < 0 || tileId > 500)
			{
				return;
			}
			this.data[tileId].Clear();
			for (int i = 0; i < 500; i++)
			{
				if (this.order[i] == tileId)
				{
					IL_4D:
					while (i < 500)
					{
						this.order[i] = this.order[i + 1];
						i++;
					}
					this.order[500] = tileId;
					return;
				}
			}
			goto IL_4D;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00015E4C File Offset: 0x0001404C
		public void Prune()
		{
			bool flag = false;
			for (int i = 0; i <= 500; i++)
			{
				HitTile.HitTileObject hitTileObject = this.data[i];
				if (hitTileObject.type != 0)
				{
					Tile tile = Main.tile[hitTileObject.X, hitTileObject.Y];
					if (hitTileObject.timeToLive <= 1)
					{
						hitTileObject.Clear();
						flag = true;
					}
					else
					{
						hitTileObject.timeToLive--;
						if ((double)hitTileObject.timeToLive < 12.0)
						{
							hitTileObject.damage -= 10;
						}
						else if ((double)hitTileObject.timeToLive < 24.0)
						{
							hitTileObject.damage -= 7;
						}
						else if ((double)hitTileObject.timeToLive < 36.0)
						{
							hitTileObject.damage -= 5;
						}
						else if ((double)hitTileObject.timeToLive < 48.0)
						{
							hitTileObject.damage -= 2;
						}
						if (hitTileObject.damage < 0)
						{
							hitTileObject.Clear();
							flag = true;
						}
						else if (hitTileObject.type == 1)
						{
							if (!tile.active())
							{
								hitTileObject.Clear();
								flag = true;
							}
						}
						else if (tile.wall == 0)
						{
							hitTileObject.Clear();
							flag = true;
						}
					}
				}
			}
			if (!flag)
			{
				return;
			}
			int num = 1;
			while (flag)
			{
				flag = false;
				for (int j = num; j < 500; j++)
				{
					if (this.data[this.order[j]].type == 0 && this.data[this.order[j + 1]].type != 0)
					{
						int num2 = this.order[j];
						this.order[j] = this.order[j + 1];
						this.order[j + 1] = num2;
						flag = true;
					}
				}
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00016008 File Offset: 0x00014208
		public void DrawFreshAnimations(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < this.data.Length; i++)
			{
				this.data[i].animationTimeElapsed++;
			}
			if (!Main.SettingsEnabled_MinersWobble)
			{
				return;
			}
			int num = 1;
			Vector2 zero = new Vector2((float)Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			zero = Vector2.Zero;
			bool flag = Main.ShouldShowInvisibleBlocksAndWalls();
			for (int j = 0; j < this.data.Length; j++)
			{
				if (this.data[j].type == num)
				{
					int damage = this.data[j].damage;
					if (damage >= 20)
					{
						int x = this.data[j].X;
						int y = this.data[j].Y;
						if (WorldGen.InWorld(x, y, 0))
						{
							Tile tile = Main.tile[x, y];
							bool flag2 = tile != null;
							if (flag2 && num == 1)
							{
								flag2 = (flag2 && tile.active() && Main.tileSolid[(int)Main.tile[x, y].type] && (!tile.invisibleBlock() || flag));
							}
							if (flag2 && num == 2)
							{
								flag2 = (flag2 && tile.wall != 0 && (!tile.invisibleWall() || flag));
							}
							if (flag2)
							{
								bool flag3 = false;
								bool flag4 = false;
								if (tile.type == 10)
								{
									flag3 = false;
								}
								else if (Main.tileSolid[(int)tile.type] && !Main.tileSolidTop[(int)tile.type])
								{
									flag3 = true;
								}
								else if (WorldGen.IsTreeType((int)tile.type))
								{
									flag4 = true;
									int num2 = (int)(tile.frameX / 22);
									int num3 = (int)(tile.frameY / 22);
									if (num3 < 9)
									{
										flag3 = (((num2 != 1 && num2 != 2) || num3 < 6 || num3 > 8) && (num2 != 3 || num3 > 2) && (num2 != 4 || num3 < 3 || num3 > 5) && (num2 != 5 || num3 < 6 || num3 > 8));
									}
								}
								else if (tile.type == 72)
								{
									flag4 = true;
									if (tile.frameX <= 34)
									{
										flag3 = true;
									}
								}
								if (flag3 && tile.slope() == 0 && !tile.halfBrick())
								{
									int num4 = 0;
									if (damage >= 80)
									{
										num4 = 3;
									}
									else if (damage >= 60)
									{
										num4 = 2;
									}
									else if (damage >= 40)
									{
										num4 = 1;
									}
									else if (damage >= 20)
									{
										num4 = 0;
									}
									Rectangle value = new Rectangle(this.data[j].crackStyle * 18, num4 * 18, 16, 16);
									value.Inflate(-2, -2);
									if (flag4)
									{
										value.X = (4 + this.data[j].crackStyle / 2) * 18;
									}
									int animationTimeElapsed = this.data[j].animationTimeElapsed;
									if ((float)animationTimeElapsed < 10f)
									{
										float num5 = (float)animationTimeElapsed / 10f;
										Color color = Lighting.GetColor(x, y);
										float rotation = 0f;
										Vector2 zero2 = Vector2.Zero;
										float num6 = 0.5f;
										float num7 = num5 % num6;
										num7 *= 1f / num6;
										if ((int)(num5 / num6) % 2 == 1)
										{
											num7 = 1f - num7;
										}
										Tile tileSafely = Framing.GetTileSafely(x, y);
										Tile tile2 = tileSafely;
										Texture2D texture2D = Main.instance.TilePaintSystem.TryGetTileAndRequestIfNotReady((int)tileSafely.type, 0, (int)tileSafely.color());
										if (texture2D != null)
										{
											Vector2 vector = new Vector2(8f);
											Vector2 value2 = new Vector2(1f);
											float scaleFactor = num7 * 0.2f + 1f;
											float num8 = 1f - num7;
											num8 = 1f;
											color *= num8 * num8 * 0.8f;
											Vector2 scale = scaleFactor * value2;
											Vector2 position = (new Vector2((float)(x * 16 - (int)Main.screenPosition.X), (float)(y * 16 - (int)Main.screenPosition.Y)) + zero + vector + zero2).Floor();
											spriteBatch.Draw(texture2D, position, new Rectangle?(new Rectangle((int)tile2.frameX, (int)tile2.frameY, 16, 16)), color, rotation, vector, scale, SpriteEffects.None, 0f);
											color.A = 180;
											spriteBatch.Draw(TextureAssets.TileCrack.Value, position, new Rectangle?(value), color, rotation, vector, scale, SpriteEffects.None, 0f);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0400013D RID: 317
		internal const int UNUSED = 0;

		// Token: 0x0400013E RID: 318
		internal const int TILE = 1;

		// Token: 0x0400013F RID: 319
		internal const int WALL = 2;

		// Token: 0x04000140 RID: 320
		internal const int MAX_HITTILES = 500;

		// Token: 0x04000141 RID: 321
		internal const int TIMETOLIVE = 60;

		// Token: 0x04000142 RID: 322
		private static UnifiedRandom rand;

		// Token: 0x04000143 RID: 323
		private static int lastCrack = -1;

		// Token: 0x04000144 RID: 324
		public HitTile.HitTileObject[] data;

		// Token: 0x04000145 RID: 325
		private int[] order;

		// Token: 0x04000146 RID: 326
		private int bufferLocation;

		// Token: 0x020005F1 RID: 1521
		public class HitTileObject
		{
			// Token: 0x06003B46 RID: 15174 RVA: 0x006596B8 File Offset: 0x006578B8
			public HitTileObject()
			{
				this.Clear();
			}

			// Token: 0x06003B47 RID: 15175 RVA: 0x006596C8 File Offset: 0x006578C8
			public void Clear()
			{
				this.X = 0;
				this.Y = 0;
				this.damage = 0;
				this.type = 0;
				this.timeToLive = 0;
				if (HitTile.rand == null)
				{
					HitTile.rand = new UnifiedRandom((int)DateTime.Now.Ticks);
				}
				this.crackStyle = HitTile.rand.Next(4);
				while (this.crackStyle == HitTile.lastCrack)
				{
					this.crackStyle = HitTile.rand.Next(4);
				}
				HitTile.lastCrack = this.crackStyle;
			}

			// Token: 0x04006340 RID: 25408
			public int X;

			// Token: 0x04006341 RID: 25409
			public int Y;

			// Token: 0x04006342 RID: 25410
			public int damage;

			// Token: 0x04006343 RID: 25411
			public int type;

			// Token: 0x04006344 RID: 25412
			public int timeToLive;

			// Token: 0x04006345 RID: 25413
			public int crackStyle;

			// Token: 0x04006346 RID: 25414
			public int animationTimeElapsed;

			// Token: 0x04006347 RID: 25415
			public Vector2 animationDirection;
		}
	}
}
