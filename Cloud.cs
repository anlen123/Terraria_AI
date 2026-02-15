using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Utilities;

namespace Terraria
{
	// Token: 0x02000035 RID: 53
	public class Cloud
	{
		// Token: 0x0600031E RID: 798 RVA: 0x00044250 File Offset: 0x00042450
		public static void resetClouds()
		{
			if (Main.dedServ)
			{
				return;
			}
			Main.windSpeedCurrent = Main.windSpeedTarget;
			for (int i = 0; i < 200; i++)
			{
				Main.cloud[i].active = false;
			}
			for (int j = 0; j < Main.numClouds; j++)
			{
				Cloud.addCloud();
				Main.cloud[j].Alpha = 1f;
			}
			for (int k = 0; k < 200; k++)
			{
				Main.cloud[k].Alpha = 1f;
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000442D4 File Offset: 0x000424D4
		public static void addCloud()
		{
			if (Main.netMode == 2)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < 200; i++)
			{
				if (!Main.cloud[i].active)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				Main.cloud[num].kill = false;
				Main.cloud[num].rSpeed = 0f;
				Main.cloud[num].sSpeed = 0f;
				Main.cloud[num].scale = (float)Cloud.rand.Next(70, 131) * 0.01f;
				Main.cloud[num].rotation = (float)Cloud.rand.Next(-10, 11) * 0.01f;
				Main.cloud[num].width = (int)((float)TextureAssets.Cloud[Main.cloud[num].type].Width() * Main.cloud[num].scale);
				Main.cloud[num].height = (int)((float)TextureAssets.Cloud[Main.cloud[num].type].Height() * Main.cloud[num].scale);
				Main.cloud[num].Alpha = 0f;
				Main.cloud[num].spriteDir = SpriteEffects.None;
				if (Cloud.rand.Next(2) == 0)
				{
					Main.cloud[num].spriteDir = SpriteEffects.FlipHorizontally;
				}
				float num2 = Main.windSpeedCurrent;
				if (!Main.gameMenu)
				{
					num2 = Main.windSpeedCurrent - Main.player[Main.myPlayer].velocity.X * 0.1f;
				}
				int num3 = 0;
				int num4 = 0;
				if (num2 > 0f)
				{
					num3 -= 200;
				}
				if (num2 < 0f)
				{
					num4 += 200;
				}
				int num5 = 300;
				float x = (float)Cloud.rand.Next(num3 - num5, Main.screenWidth + num4 + num5);
				Main.cloud[num].Alpha = 0f;
				Main.cloud[num].position.Y = (float)Cloud.rand.Next((int)((float)(-(float)Main.screenHeight) * 0.25f), (int)((float)Main.screenHeight * 0.15f));
				if (Cloud.rand.Next(3) == 0)
				{
					Cloud cloud = Main.cloud[num];
					cloud.position.Y = cloud.position.Y - (float)Cloud.rand.Next((int)((float)Main.screenHeight * 0.1f));
				}
				Main.cloud[num].type = Cloud.rand.Next(4);
				if ((Main.cloudAlpha > 0f && Cloud.rand.Next(4) != 0) || (Main.cloudBGActive >= 1f && Cloud.rand.Next(2) == 0))
				{
					Main.cloud[num].type = Cloud.rand.Next(18, 22);
					if ((double)Main.cloud[num].scale >= 1.15)
					{
						Cloud cloud2 = Main.cloud[num];
						cloud2.position.Y = cloud2.position.Y - 150f;
					}
					if (Main.cloud[num].scale >= 1f)
					{
						Cloud cloud3 = Main.cloud[num];
						cloud3.position.Y = cloud3.position.Y - 150f;
					}
				}
				else if (Main.cloudBGActive <= 0f && Main.cloudAlpha == 0f && Main.cloud[num].scale < 1f && Main.cloud[num].position.Y < (float)(-(float)Main.screenHeight) * 0.15f && (double)Main.numClouds <= 80.0)
				{
					Main.cloud[num].type = Cloud.rand.Next(9, 14);
				}
				else if ((((double)Main.cloud[num].scale < 1.15 && Main.cloud[num].position.Y < (float)(-(float)Main.screenHeight) * 0.3f) || ((double)Main.cloud[num].scale < 0.85 && Main.cloud[num].position.Y < (float)Main.screenHeight * 0.15f)) && ((double)Main.numClouds > 70.0 || Main.cloudBGActive >= 1f))
				{
					Main.cloud[num].type = Cloud.rand.Next(4, 9);
				}
				else if (Main.cloud[num].position.Y > (float)(-(float)Main.screenHeight) * 0.15f && Cloud.rand.Next(2) == 0 && (double)Main.numClouds > 20.0)
				{
					Main.cloud[num].type = Cloud.rand.Next(14, 18);
				}
				if (Cloud.rand.Next((Main.dontStarveWorld || Main.tenthAnniversaryWorld) ? 25 : 150) == 0)
				{
					Main.cloud[num].type = Cloud.RollRareCloud();
				}
				else if (Main.tenthAnniversaryWorld && Cloud.rand.Next(3) == 0)
				{
					Main.cloud[num].type = Cloud.RollRareCloud();
				}
				if ((double)Main.cloud[num].scale > 1.2)
				{
					Cloud cloud4 = Main.cloud[num];
					cloud4.position.Y = cloud4.position.Y + 100f;
				}
				if ((double)Main.cloud[num].scale > 1.3)
				{
					Main.cloud[num].scale = 1.3f;
				}
				if ((double)Main.cloud[num].scale < 0.7)
				{
					Main.cloud[num].scale = 0.7f;
				}
				Main.cloud[num].active = true;
				Main.cloud[num].position.X = x;
				if (Main.cloud[num].position.X > (float)(Main.screenWidth + 400))
				{
					Main.cloud[num].Alpha = 1f;
				}
				if (Main.cloud[num].position.X + (float)TextureAssets.Cloud[Main.cloud[num].type].Width() * Main.cloud[num].scale < -400f)
				{
					Main.cloud[num].Alpha = 1f;
				}
				Rectangle rectangle = new Rectangle((int)Main.cloud[num].position.X, (int)Main.cloud[num].position.Y, Main.cloud[num].width, Main.cloud[num].height);
				for (int j = 0; j < 200; j++)
				{
					if (num != j && Main.cloud[j].active)
					{
						Rectangle value = new Rectangle((int)Main.cloud[j].position.X, (int)Main.cloud[j].position.Y, Main.cloud[j].width, Main.cloud[j].height);
						if (rectangle.Intersects(value))
						{
							Main.cloud[num].active = false;
						}
					}
				}
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x000449B8 File Offset: 0x00042BB8
		private static int RollRareCloud()
		{
			int result = -1;
			bool flag = false;
			while (!flag)
			{
				if (Main.tenthAnniversaryWorld)
				{
					result = Cloud.rand.Next(22, 37);
				}
				else
				{
					result = Cloud.rand.Next(22, 41);
				}
				switch (result)
				{
				case 25:
				case 26:
					flag = NPC.downedBoss1;
					break;
				default:
					flag = true;
					break;
				case 28:
					if (Cloud.rand.Next(10) == 0)
					{
						flag = true;
					}
					break;
				case 30:
				case 35:
					flag = Main.hardMode;
					break;
				case 31:
					flag = NPC.downedBoss3;
					break;
				case 36:
					flag = (NPC.downedBoss2 && WorldGen.crimson);
					break;
				case 37:
				case 38:
				case 39:
				case 40:
					if (Main.dontStarveWorld || Cloud.rand.Next(10) == 0)
					{
						flag = true;
					}
					break;
				}
			}
			return result;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00044A9C File Offset: 0x00042C9C
		public Color cloudColor(Color bgColor)
		{
			float num = this.scale * this.Alpha;
			if (num > 1f)
			{
				num = 1f;
			}
			byte b = (byte)((float)((int)((float)bgColor.R * num)));
			float num2 = (float)((int)((float)bgColor.G * num));
			float num3 = (float)((int)((float)bgColor.B * num));
			float num4 = (float)((int)((float)bgColor.A * num));
			return new Color((int)b, (int)((byte)num2), (int)((byte)num3), (int)((byte)num4));
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00003BA0 File Offset: 0x00001DA0
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00044B04 File Offset: 0x00042D04
		public static void UpdateClouds()
		{
			if (Main.netMode == 2)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < 200; i++)
			{
				if (Main.cloud[i].active)
				{
					Main.cloud[i].Update();
					if (!Main.cloud[i].kill)
					{
						num++;
					}
				}
			}
			for (int j = 0; j < 200; j++)
			{
				if (Main.cloud[j].active)
				{
					if (j > 1 && (!Main.cloud[j - 1].active || (double)Main.cloud[j - 1].scale > (double)Main.cloud[j].scale + 0.02))
					{
						Cloud cloud = (Cloud)Main.cloud[j - 1].Clone();
						Main.cloud[j - 1] = (Cloud)Main.cloud[j].Clone();
						Main.cloud[j] = cloud;
					}
					if (j < 199 && (!Main.cloud[j].active || (double)Main.cloud[j + 1].scale < (double)Main.cloud[j].scale - 0.02))
					{
						Cloud cloud2 = (Cloud)Main.cloud[j + 1].Clone();
						Main.cloud[j + 1] = (Cloud)Main.cloud[j].Clone();
						Main.cloud[j] = cloud2;
					}
				}
			}
			if (num < Main.numClouds)
			{
				Cloud.addCloud();
				return;
			}
			if (num > Main.numClouds)
			{
				int num2 = Cloud.rand.Next(num);
				int num3 = 0;
				while (Main.cloud[num2].kill && num3 < 100)
				{
					num3++;
					num2 = Cloud.rand.Next(num);
				}
				Main.cloud[num2].kill = true;
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00044CC8 File Offset: 0x00042EC8
		public static void UpdateCloudParallax()
		{
			float num = 0f;
			Vector2 center = Main.Camera.Center;
			if (Cloud.lastCameraCenter != null)
			{
				num = center.X - Cloud.lastCameraCenter.Value.X;
			}
			Cloud.lastCameraCenter = new Vector2?(center);
			if (num == 0f)
			{
				return;
			}
			for (int i = 0; i < 200; i++)
			{
				Cloud cloud = Main.cloud[i];
				if (cloud.active)
				{
					Cloud cloud2 = cloud;
					cloud2.position.X = cloud2.position.X - num * cloud.GetParallax();
				}
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00044D54 File Offset: 0x00042F54
		private float GetParallax()
		{
			float num;
			float num2;
			if (this.scale < 1f)
			{
				num = 0.07f;
				num2 = this.scale + 0.15f;
				num2 = (num2 + 1f) / 2f;
			}
			else if ((double)this.scale <= 1.15)
			{
				num = 0.19f;
				num2 = this.scale - 0.075f;
			}
			else
			{
				num = 0.23f;
				num2 = this.scale - 0.15f - 0.075f;
			}
			num2 *= num2;
			return num * num2;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00044DDC File Offset: 0x00042FDC
		public void Update()
		{
			if (WorldGen.drunkWorldGenText && Main.gameMenu)
			{
				if (Main.onlyShimmerOceanWorlds)
				{
					this.type = 29;
				}
				else if (WorldGen.dontStarveWorldGen)
				{
					if ((double)this.scale < 0.7)
					{
						this.type = 40;
					}
					else if ((double)this.scale < 0.8)
					{
						this.type = 39;
					}
					else if ((double)this.scale < 0.9)
					{
						this.type = 38;
					}
					else
					{
						this.type = 37;
					}
				}
				else if (WorldGen.getGoodWorldGen)
				{
					if ((double)this.scale < 0.8)
					{
						this.type = 25;
					}
					else
					{
						this.type = 26;
					}
				}
				else
				{
					this.type = 28;
				}
			}
			if (this.scale == 1f)
			{
				this.scale -= 0.0001f;
			}
			if ((double)this.scale == 1.15)
			{
				this.scale -= 0.0001f;
			}
			this.position.X = this.position.X + Main.windSpeedCurrent * 9f * this.GetParallax() * (float)Main.dayRate;
			float num = 600f;
			if (Main.bgAlphaFrontLayer[4] == 1f && this.position.Y > 200f)
			{
				this.kill = true;
				this.Alpha -= 0.005f * (float)Main.dayRate;
			}
			if (!this.kill)
			{
				if (this.Alpha < 1f)
				{
					this.Alpha += 0.001f * (float)Main.dayRate;
					if (this.Alpha > 1f)
					{
						this.Alpha = 1f;
					}
				}
			}
			else
			{
				this.Alpha -= 0.001f * (float)Main.dayRate;
				if (this.Alpha <= 0f)
				{
					this.active = false;
				}
			}
			if (this.position.X + (float)TextureAssets.Cloud[this.type].Width() * this.scale < -num || this.position.X > (float)Main.screenWidth + num)
			{
				this.active = false;
			}
			this.rSpeed += (float)Cloud.rand.Next(-10, 11) * 2E-05f;
			if ((double)this.rSpeed > 0.0002)
			{
				this.rSpeed = 0.0002f;
			}
			if ((double)this.rSpeed < -0.0002)
			{
				this.rSpeed = -0.0002f;
			}
			if ((double)this.rotation > 0.02)
			{
				this.rotation = 0.02f;
			}
			if ((double)this.rotation < -0.02)
			{
				this.rotation = -0.02f;
			}
			this.rotation += this.rSpeed;
			this.width = (int)((float)TextureAssets.Cloud[this.type].Width() * this.scale);
			this.height = (int)((float)TextureAssets.Cloud[this.type].Height() * this.scale);
			if (this.type >= 9 && this.type <= 13 && (Main.cloudAlpha > 0f || Main.cloudBGActive >= 1f))
			{
				this.kill = true;
			}
		}

		// Token: 0x04000251 RID: 593
		public Vector2 position;

		// Token: 0x04000252 RID: 594
		public float scale;

		// Token: 0x04000253 RID: 595
		public float rotation;

		// Token: 0x04000254 RID: 596
		public float rSpeed;

		// Token: 0x04000255 RID: 597
		public float sSpeed;

		// Token: 0x04000256 RID: 598
		public bool active;

		// Token: 0x04000257 RID: 599
		public SpriteEffects spriteDir;

		// Token: 0x04000258 RID: 600
		public int type;

		// Token: 0x04000259 RID: 601
		public int width;

		// Token: 0x0400025A RID: 602
		public int height;

		// Token: 0x0400025B RID: 603
		public float Alpha;

		// Token: 0x0400025C RID: 604
		public bool kill;

		// Token: 0x0400025D RID: 605
		private static UnifiedRandom rand = new UnifiedRandom();

		// Token: 0x0400025E RID: 606
		public static Vector2? lastCameraCenter;
	}
}
