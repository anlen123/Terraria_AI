using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.ID;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	// Token: 0x02000505 RID: 1285
	public class JunglePass : GenPass
	{
		// Token: 0x060035FD RID: 13821 RVA: 0x0061F55A File Offset: 0x0061D75A
		public JunglePass() : base(GenPassNameID.Jungle, 10154.65234375)
		{
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x0061F570 File Offset: 0x0061D770
		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = Lang.gen[11].Value;
			this._worldScale = (double)Main.maxTilesX / 4200.0 * 1.5;
			double worldScale = this._worldScale;
			Point point = this.CreateStartPoint();
			int num = point.X;
			int num2 = point.Y;
			Point zero = Point.Zero;
			this.ApplyRandomMovement(ref num, ref num2, 100, 100);
			zero.X += num;
			zero.Y += num2;
			this.PlaceFirstPassMud(num, num2, 3);
			this.PlaceGemsAt(num, num2, 63, 2);
			progress.Set(0.15);
			this.ApplyRandomMovement(ref num, ref num2, 250, 150);
			zero.X += num;
			zero.Y += num2;
			this.PlaceFirstPassMud(num, num2, 0);
			this.PlaceGemsAt(num, num2, 65, 2);
			progress.Set(0.3);
			int oldX = num;
			int oldY = num2;
			this.ApplyRandomMovement(ref num, ref num2, 400, 150);
			zero.X += num;
			zero.Y += num2;
			this.PlaceFirstPassMud(num, num2, -3);
			this.PlaceGemsAt(num, num2, 67, 2);
			progress.Set(0.45);
			num = zero.X / 3;
			num2 = zero.Y / 3;
			int num3 = GenBase._random.Next((int)(400.0 * worldScale), (int)(600.0 * worldScale));
			int num4 = (int)(25.0 * worldScale);
			num = Utils.Clamp<int>(num, GenVars.leftBeachEnd + num3 / 2 + num4, GenVars.rightBeachStart - num3 / 2 - num4);
			GenVars.mudWall = true;
			WorldGen.TileRunner(num, num2, (double)num3, 10000, 59, false, 0.0, -20.0, true, true, -1);
			if (!WorldGen.SecretSeed.extraLiquid.Enabled)
			{
				this.GenerateTunnelToSurface(num, num2);
			}
			GenVars.mudWall = false;
			progress.Set(0.6);
			this.GenerateHolesInMudWalls();
			this.GenerateFinishingTouches(progress, oldX, oldY);
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x0061F78C File Offset: 0x0061D98C
		private void PlaceGemsAt(int x, int y, ushort baseGem, int gemVariants)
		{
			int num = 0;
			while ((double)num < 6.0 * this._worldScale)
			{
				WorldGen.TileRunner(x + GenBase._random.Next(-(int)(125.0 * this._worldScale), (int)(125.0 * this._worldScale)), y + GenBase._random.Next(-(int)(125.0 * this._worldScale), (int)(125.0 * this._worldScale)), (double)GenBase._random.Next(3, 7), GenBase._random.Next(3, 8), GenBase._random.Next((int)baseGem, (int)baseGem + gemVariants), false, 0.0, 0.0, false, true, -1);
				num++;
			}
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x0061F85C File Offset: 0x0061DA5C
		private void PlaceFirstPassMud(int x, int y, int xSpeedScale)
		{
			GenVars.mudWall = true;
			WorldGen.TileRunner(x, y, (double)GenBase._random.Next((int)(250.0 * this._worldScale), (int)(500.0 * this._worldScale)), GenBase._random.Next(50, 150), 59, false, (double)(GenVars.CurrentDungeonGenVars.dungeonSide * xSpeedScale), 0.0, false, true, -1);
			GenVars.mudWall = false;
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x0061F8D6 File Offset: 0x0061DAD6
		private Point CreateStartPoint()
		{
			return new Point(GenVars.jungleOriginX, (int)((double)Main.maxTilesY + Main.rockLayer) / 2);
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x0061F8F4 File Offset: 0x0061DAF4
		private void ApplyRandomMovement(ref int x, ref int y, int xRange, int yRange)
		{
			x += GenBase._random.Next((int)((double)(-(double)xRange) * this._worldScale), 1 + (int)((double)xRange * this._worldScale));
			y += GenBase._random.Next((int)((double)(-(double)yRange) * this._worldScale), 1 + (int)((double)yRange * this._worldScale));
			y = Utils.Clamp<int>(y, (int)Main.rockLayer, Main.maxTilesY);
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x0061F964 File Offset: 0x0061DB64
		private void GenerateTunnelToSurface(int i, int j)
		{
			double num = (double)GenBase._random.Next(5, 11);
			Vector2D vector2D;
			vector2D.X = (double)i;
			vector2D.Y = (double)j;
			Vector2D vector2D2;
			vector2D2.X = (double)GenBase._random.Next(-10, 11) * 0.1;
			vector2D2.Y = (double)GenBase._random.Next(10, 20) * 0.1;
			int num2 = 0;
			bool flag = true;
			while (flag)
			{
				if (vector2D.Y < Main.worldSurface)
				{
					if (WorldGen.drunkWorldGen)
					{
						flag = false;
					}
					int num3 = (int)vector2D.X;
					int num4 = (int)vector2D.Y;
					num3 = Utils.Clamp<int>(num3, 10, Main.maxTilesX - 10);
					num4 = Utils.Clamp<int>(num4, 10, Main.maxTilesY - 10);
					if (num4 < 5)
					{
						num4 = 5;
					}
					if (Main.tile[num3, num4].wall == 0 && !Main.tile[num3, num4].active() && Main.tile[num3, num4 - 3].wall == 0 && !Main.tile[num3, num4 - 3].active() && Main.tile[num3, num4 - 1].wall == 0 && !Main.tile[num3, num4 - 1].active() && Main.tile[num3, num4 - 4].wall == 0 && !Main.tile[num3, num4 - 4].active() && Main.tile[num3, num4 - 2].wall == 0 && !Main.tile[num3, num4 - 2].active() && Main.tile[num3, num4 - 5].wall == 0 && !Main.tile[num3, num4 - 5].active())
					{
						flag = false;
					}
				}
				GenVars.JungleX = (int)vector2D.X;
				num += (double)GenBase._random.Next(-20, 21) * 0.1;
				if (num < 5.0)
				{
					num = 5.0;
				}
				if (num > 10.0)
				{
					num = 10.0;
				}
				int value = (int)(vector2D.X - num * 0.5);
				int num5 = (int)(vector2D.X + num * 0.5);
				int num6 = (int)(vector2D.Y - num * 0.5);
				int num7 = (int)(vector2D.Y + num * 0.5);
				int num8 = Utils.Clamp<int>(value, 10, Main.maxTilesX - 10);
				num5 = Utils.Clamp<int>(num5, 10, Main.maxTilesX - 10);
				num6 = Utils.Clamp<int>(num6, 10, Main.maxTilesY - 10);
				num7 = Utils.Clamp<int>(num7, 10, Main.maxTilesY - 10);
				for (int k = num8; k < num5; k++)
				{
					for (int l = num6; l < num7; l++)
					{
						if (Math.Abs((double)k - vector2D.X) + Math.Abs((double)l - vector2D.Y) < num * 0.5 * (1.0 + (double)GenBase._random.Next(-10, 11) * 0.015))
						{
							WorldGen.KillTile(k, l, false, false, false);
						}
					}
				}
				num2++;
				if (num2 > 10 && GenBase._random.Next(50) < num2)
				{
					num2 = 0;
					int num9 = -2;
					if (GenBase._random.Next(2) == 0)
					{
						num9 = 2;
					}
					WorldGen.TileRunner((int)vector2D.X, (int)vector2D.Y, (double)GenBase._random.Next(3, 20), GenBase._random.Next(10, 100), -1, false, (double)num9, 0.0, false, true, -1);
				}
				vector2D += vector2D2;
				vector2D2.Y += (double)GenBase._random.Next(-10, 11) * 0.01;
				if (vector2D2.Y > 0.0)
				{
					vector2D2.Y = 0.0;
				}
				if (vector2D2.Y < -2.0)
				{
					vector2D2.Y = -2.0;
				}
				vector2D2.X += (double)GenBase._random.Next(-10, 11) * 0.1;
				if (vector2D.X < (double)(i - 200))
				{
					vector2D2.X += (double)GenBase._random.Next(5, 21) * 0.1;
				}
				if (vector2D.X > (double)(i + 200))
				{
					vector2D2.X -= (double)GenBase._random.Next(5, 21) * 0.1;
				}
				if (vector2D2.X > 1.5)
				{
					vector2D2.X = 1.5;
				}
				if (vector2D2.X < -1.5)
				{
					vector2D2.X = -1.5;
				}
			}
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x0061FE80 File Offset: 0x0061E080
		private void GenerateHolesInMudWalls()
		{
			for (int i = 0; i < Main.maxTilesX / 4; i++)
			{
				int num = GenBase._random.Next(20, Main.maxTilesX - 20);
				int num2 = GenBase._random.Next((int)GenVars.worldSurface + 10, Main.UnderworldLayer);
				while (Main.tile[num, num2].wall != 64 && Main.tile[num, num2].wall != 15)
				{
					num = GenBase._random.Next(20, Main.maxTilesX - 20);
					num2 = GenBase._random.Next((int)GenVars.worldSurface + 10, Main.UnderworldLayer);
				}
				WorldGen.MudWallRunner(num, num2);
			}
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x0061FF34 File Offset: 0x0061E134
		private void GenerateFinishingTouches(GenerationProgress progress, int oldX, int oldY)
		{
			int num = oldX;
			int num2 = oldY;
			double worldScale = this._worldScale;
			int num3 = 0;
			while ((double)num3 <= 20.0 * worldScale)
			{
				progress.Set((60.0 + (double)num3 / worldScale) * 0.01);
				num += GenBase._random.Next((int)(-5.0 * worldScale), (int)(6.0 * worldScale));
				num2 += GenBase._random.Next((int)(-5.0 * worldScale), (int)(6.0 * worldScale));
				WorldGen.TileRunner(num, num2, (double)GenBase._random.Next(40, 100), GenBase._random.Next(300, 500), 59, false, 0.0, 0.0, false, true, -1);
				num3++;
			}
			int num4 = 0;
			while ((double)num4 <= 10.0 * worldScale)
			{
				progress.Set((80.0 + (double)num4 / worldScale * 2.0) * 0.01);
				num = oldX + GenBase._random.Next((int)(-600.0 * worldScale), (int)(600.0 * worldScale));
				num2 = oldY + GenBase._random.Next((int)(-200.0 * worldScale), (int)(200.0 * worldScale));
				while (num < 1 || num >= Main.maxTilesX - 1 || num2 < 1 || num2 >= Main.maxTilesY - 1 || Main.tile[num, num2].type != 59)
				{
					num = oldX + GenBase._random.Next((int)(-600.0 * worldScale), (int)(600.0 * worldScale));
					num2 = oldY + GenBase._random.Next((int)(-200.0 * worldScale), (int)(200.0 * worldScale));
				}
				int num5 = 0;
				while ((double)num5 < 8.0 * worldScale)
				{
					num += GenBase._random.Next(-30, 31);
					num2 += GenBase._random.Next(-30, 31);
					int type = -1;
					if (GenBase._random.Next(7) == 0)
					{
						type = -2;
					}
					WorldGen.TileRunner(num, num2, (double)GenBase._random.Next(10, 20), GenBase._random.Next(30, 70), type, false, 0.0, 0.0, false, true, -1);
					num5++;
				}
				num4++;
			}
			int num6 = 0;
			while ((double)num6 <= 300.0 * worldScale)
			{
				num = oldX + GenBase._random.Next((int)(-600.0 * worldScale), (int)(600.0 * worldScale));
				num2 = oldY + GenBase._random.Next((int)(-200.0 * worldScale), (int)(200.0 * worldScale));
				while (num < 1 || num >= Main.maxTilesX - 1 || num2 < 1 || num2 >= Main.maxTilesY - 1 || Main.tile[num, num2].type != 59)
				{
					num = oldX + GenBase._random.Next((int)(-600.0 * worldScale), (int)(600.0 * worldScale));
					num2 = oldY + GenBase._random.Next((int)(-200.0 * worldScale), (int)(200.0 * worldScale));
				}
				WorldGen.TileRunner(num, num2, (double)GenBase._random.Next(4, 10), GenBase._random.Next(5, 30), 1, false, 0.0, 0.0, false, true, -1);
				if (GenBase._random.Next(4) == 0)
				{
					int type2 = GenBase._random.Next(63, 69);
					WorldGen.TileRunner(num + GenBase._random.Next(-1, 2), num2 + GenBase._random.Next(-1, 2), (double)GenBase._random.Next(3, 7), GenBase._random.Next(4, 8), type2, false, 0.0, 0.0, false, true, -1);
				}
				num6++;
			}
		}

		// Token: 0x04005AE1 RID: 23265
		private double _worldScale;
	}
}
