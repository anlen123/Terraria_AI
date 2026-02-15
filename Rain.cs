using System;
using Microsoft.Xna.Framework;

namespace Terraria
{
	// Token: 0x0200003C RID: 60
	public class Rain
	{
		// Token: 0x060004B1 RID: 1201 RVA: 0x0012B860 File Offset: 0x00129A60
		public static void ClearRain()
		{
			for (int i = 0; i < Main.maxRain; i++)
			{
				Main.rain[i].active = false;
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0012B88C File Offset: 0x00129A8C
		public static void MakeRain()
		{
			if (Main.netMode == 2)
			{
				return;
			}
			if (Main.gamePaused)
			{
				return;
			}
			if (Main.gameMenu)
			{
				return;
			}
			if (!Main.SceneMetrics.ZoneRain)
			{
				return;
			}
			if (Main.shimmerAlpha > 0f)
			{
				return;
			}
			float num = (float)Main.screenWidth / (float)Main.MaxWorldViewSize.X;
			num *= 25f;
			num *= 0.25f + 1f * Main.cloudAlpha;
			if (NPC.AnyDanger(true, false))
			{
				float num2 = num;
				num *= 0.05f;
				if (num2 > 1f && num < 1f)
				{
					num = 1f;
				}
			}
			int num3 = 0;
			while ((float)num3 < num)
			{
				int num4 = 600;
				if (Main.player[Main.myPlayer].velocity.Y < 0f)
				{
					num4 += (int)(Math.Abs(Main.player[Main.myPlayer].velocity.Y) * 30f);
				}
				Vector2 vector;
				vector.X = (float)Main.rand.Next((int)Main.screenPosition.X - num4, (int)Main.screenPosition.X + Main.screenWidth + num4);
				vector.Y = Main.screenPosition.Y - (float)Main.rand.Next(20, 100);
				vector.X -= Main.windSpeedCurrent * 15f * 40f;
				vector.X += Main.player[Main.myPlayer].velocity.X * 40f;
				if (vector.X < 0f)
				{
					vector.X = 0f;
				}
				if (vector.X > (float)((Main.maxTilesX - 1) * 16))
				{
					vector.X = (float)((Main.maxTilesX - 1) * 16);
				}
				int num5 = (int)vector.X / 16;
				int num6 = (int)vector.Y / 16;
				if (num5 < 0)
				{
					num5 = 0;
				}
				if (num5 > Main.maxTilesX - 1)
				{
					num5 = Main.maxTilesX - 1;
				}
				if (num6 < 0)
				{
					num6 = 0;
				}
				if (num6 > Main.maxTilesY - 1)
				{
					num6 = Main.maxTilesY - 1;
				}
				if (Main.remixWorld || Main.gameMenu || (!WorldGen.SolidTile(num5, num6, false) && Main.tile[num5, num6].wall <= 0))
				{
					Vector2 rainFallVelocity = Rain.GetRainFallVelocity();
					Rain.NewRain(vector, rainFallVelocity);
				}
				num3++;
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0012BADE File Offset: 0x00129CDE
		public static Vector2 GetRainFallVelocity()
		{
			return new Vector2(Main.windSpeedCurrent * 18f, 14f);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0012BAF8 File Offset: 0x00129CF8
		public void Update()
		{
			if (Main.gamePaused)
			{
				return;
			}
			this.position += this.velocity;
			if (Main.gameMenu)
			{
				if (this.position.Y > Main.screenPosition.Y + (float)Main.screenHeight + 2000f)
				{
					this.active = false;
					return;
				}
			}
			else if (Main.remixWorld)
			{
				if (this.position.Y > Main.screenPosition.Y + (float)Main.screenHeight + 100f)
				{
					this.active = false;
					return;
				}
			}
			else if (Collision.SolidCollision(this.position, 2, 2) || this.position.Y > Main.screenPosition.Y + (float)Main.screenHeight + 100f || Collision.WetCollision(this.position, 2, 2))
			{
				this.active = false;
				if ((float)Main.rand.Next(100) < Main.gfxQuality * 100f)
				{
					int num = Dust.NewDust(this.position - this.velocity, 2, 2, Dust.dustWater(), 0f, 0f, 0, default(Color), 1f);
					Dust dust = Main.dust[num];
					dust.position.X = dust.position.X - 2f;
					Dust dust2 = Main.dust[num];
					dust2.position.Y = dust2.position.Y + 2f;
					Main.dust[num].alpha = 38;
					Main.dust[num].velocity *= 0.1f;
					Main.dust[num].velocity += -this.velocity * 0.025f;
					Dust dust3 = Main.dust[num];
					dust3.velocity.Y = dust3.velocity.Y - 2f;
					Main.dust[num].scale = 0.6f;
					Main.dust[num].noGravity = true;
				}
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0012BCF4 File Offset: 0x00129EF4
		public static int NewRainForced(Vector2 Position, Vector2 Velocity)
		{
			int num = -1;
			int num2 = Main.maxRain;
			float num3 = (1f + Main.gfxQuality) / 2f;
			if (num3 < 0.9f)
			{
				num2 = (int)((float)num2 * num3);
			}
			for (int i = 0; i < num2; i++)
			{
				if (!Main.rain[i].active)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return Main.maxRain;
			}
			Rain rain = Main.rain[num];
			rain.active = true;
			rain.position = Position;
			rain.scale = 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
			rain.velocity = Velocity * rain.scale;
			rain.rotation = (float)Math.Atan2((double)rain.velocity.X, (double)(-(double)rain.velocity.Y));
			rain.type = (byte)(Main.waterStyle * 3 + Main.rand.Next(3));
			return num;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0012BDE0 File Offset: 0x00129FE0
		private static int NewRain(Vector2 Position, Vector2 Velocity)
		{
			int num = -1;
			int num2 = (int)((float)Main.maxRain * Main.cloudAlpha);
			if (num2 > Main.maxRain)
			{
				num2 = Main.maxRain;
			}
			float num3 = (float)Main.maxTilesX / 6400f;
			Math.Max(0f, Math.Min(1f, (Main.player[Main.myPlayer].position.Y / 16f - 85f * num3) / (60f * num3)));
			float num4 = (1f + Main.gfxQuality) / 2f;
			if ((double)num4 < 0.9)
			{
				num2 = (int)((float)num2 * num4);
			}
			float num5 = Utils.Clamp<float>((float)Main.SceneMetrics.SnowTileCount / (float)SceneMetrics.SnowTileThreshold, 0f, 1f);
			num5 *= num5;
			num2 = (int)((float)num2 * (1f - num5));
			num2 = (int)((double)num2 * Math.Pow((double)Main.atmo, 9.0));
			if ((double)Main.atmo < 0.4)
			{
				num2 = 0;
			}
			for (int i = 0; i < num2; i++)
			{
				if (!Main.rain[i].active)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return Main.maxRain;
			}
			Rain rain = Main.rain[num];
			rain.active = true;
			rain.position = Position;
			rain.scale = 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
			rain.velocity = Velocity * rain.scale;
			rain.rotation = (float)Math.Atan2((double)rain.velocity.X, (double)(-(double)rain.velocity.Y));
			rain.type = (byte)(Main.waterStyle * 3 + Main.rand.Next(3));
			return num;
		}

		// Token: 0x040002E3 RID: 739
		public Vector2 position;

		// Token: 0x040002E4 RID: 740
		public Vector2 velocity;

		// Token: 0x040002E5 RID: 741
		public float scale;

		// Token: 0x040002E6 RID: 742
		public float rotation;

		// Token: 0x040002E7 RID: 743
		public int alpha;

		// Token: 0x040002E8 RID: 744
		public bool active;

		// Token: 0x040002E9 RID: 745
		public byte type;
	}
}
