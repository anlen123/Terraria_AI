using System;
using Microsoft.Xna.Framework;
using Terraria.Utilities;

namespace Terraria
{
	// Token: 0x02000020 RID: 32
	public class Star
	{
		// Token: 0x0600016B RID: 363 RVA: 0x00010A24 File Offset: 0x0000EC24
		public static void NightSetup()
		{
			Star.starfallBoost = 1f;
			int maxValue = 10;
			int maxValue2 = 3;
			if (Main.tenthAnniversaryWorld)
			{
				maxValue = 5;
				maxValue2 = 2;
			}
			if (Main.rand.Next(maxValue) == 0)
			{
				Star.starfallBoost = (float)Main.rand.Next(300, 501) * 0.01f;
			}
			else if (Main.rand.Next(maxValue2) == 0)
			{
				Star.starfallBoost = (float)Main.rand.Next(100, 151) * 0.01f;
			}
			Star.starFallCount = 0;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00010AAC File Offset: 0x0000ECAC
		public static void StarFall(float positionX)
		{
			Star.starFallCount++;
			int num = -1;
			float num2 = -1f;
			float num3 = positionX / Main.rightWorld * (float)Main.MaxWorldViewSize.X;
			for (int i = 0; i < Main.numStars; i++)
			{
				if (!Main.star[i].hidden && !Main.star[i].falling)
				{
					float num4 = Math.Abs(Main.star[i].position.X - num3);
					if (num2 == -1f || num4 < num2)
					{
						num = i;
						num2 = num4;
					}
				}
			}
			if (num >= 0)
			{
				Main.star[num].Fall();
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00010B4C File Offset: 0x0000ED4C
		public static void SpawnStars(int s = -1)
		{
			FastRandom fastRandom = FastRandom.CreateWithRandomSeed();
			int num = fastRandom.Next(200, 400);
			int num2 = 0;
			int num3 = num;
			if (s >= 0)
			{
				num2 = s;
				num3 = s + 1;
			}
			for (int i = num2; i < num3; i++)
			{
				Main.star[i] = new Star();
				if (s >= 0)
				{
					Main.star[i].fadeIn = 1f;
					int num4 = 10;
					int num5 = -2000;
					for (int j = 0; j < num4; j++)
					{
						float num6 = (float)fastRandom.Next(1921);
						int num7 = 2000;
						for (int k = 0; k < Main.numStars; k++)
						{
							if (k != s && !Main.star[k].hidden && !Main.star[k].falling)
							{
								int num8 = (int)Math.Abs(num6 - Main.star[k].position.X);
								if (num8 < num7)
								{
									num7 = num8;
								}
							}
						}
						if (s == 0 || num7 > num5)
						{
							num5 = num7;
							Main.star[i].position.X = num6;
						}
					}
				}
				else
				{
					Main.star[i].position.X = (float)fastRandom.Next(1921);
				}
				Main.star[i].position.Y = (float)fastRandom.Next(1201);
				Main.star[i].rotation = (float)fastRandom.Next(628) * 0.01f;
				Main.star[i].scale = (float)fastRandom.Next(70, 130) * 0.006f;
				Main.star[i].type = fastRandom.Next(0, 4);
				Main.star[i].twinkle = (float)fastRandom.Next(60, 101) * 0.01f;
				Main.star[i].twinkleSpeed = (float)fastRandom.Next(30, 110) * 0.0001f;
				Main.star[i].velocity *= 0f;
				if (fastRandom.Next(2) == 0)
				{
					Main.star[i].twinkleSpeed *= -1f;
				}
				Main.star[i].rotationSpeed = (float)fastRandom.Next(5, 50) * 0.0001f;
				if (fastRandom.Next(2) == 0)
				{
					Main.star[i].rotationSpeed *= -1f;
				}
				if (fastRandom.Next(40) == 0)
				{
					Main.star[i].scale *= 2f;
					Main.star[i].twinkleSpeed /= 2f;
					Main.star[i].rotationSpeed /= 2f;
				}
			}
			if (s == -1)
			{
				Main.numStars = num;
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00010E2C File Offset: 0x0000F02C
		public void Fall()
		{
			if (WorldGen.SecretSeed.anySecretSeedIsActive && !Main.starGame)
			{
				return;
			}
			this.fallTime = 0;
			this.falling = true;
			this.fallSpeed.Y = (float)Main.rand.Next(700, 1001) * 0.01f;
			this.fallSpeed.X = (float)Main.rand.Next(-400, 401) * 0.01f;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00010EA4 File Offset: 0x0000F0A4
		public void Update()
		{
			if (this.falling && !this.hidden)
			{
				this.fallTime += Main.dayRate;
				this.position += this.fallSpeed * (float)(Main.dayRate + 99) / 100f;
				if (this.position.Y > 1500f)
				{
					this.hidden = true;
				}
				if (Main.starGame && this.position.Length() > 99999f)
				{
					this.hidden = true;
				}
				this.twinkle += this.twinkleSpeed * 3f;
				if (this.twinkle > 1f)
				{
					this.twinkle = 1f;
					this.twinkleSpeed *= -1f;
				}
				else if ((double)this.twinkle < 0.6)
				{
					this.twinkle = 0.6f;
					this.twinkleSpeed *= -1f;
				}
				this.rotation += 0.5f;
				if ((double)this.rotation > 6.28)
				{
					this.rotation -= 6.28f;
				}
				if (this.rotation < 0f)
				{
					this.rotation += 6.28f;
					return;
				}
			}
			else
			{
				if (this.fadeIn > 0f)
				{
					float num = 6.1728395E-05f * (float)Main.dayRate;
					num *= 10f;
					this.fadeIn -= num;
					if (this.fadeIn < 0f)
					{
						this.fadeIn = 0f;
					}
				}
				this.twinkle += this.twinkleSpeed;
				if (this.twinkle > 1f)
				{
					this.twinkle = 1f;
					this.twinkleSpeed *= -1f;
				}
				else if ((double)this.twinkle < 0.6)
				{
					this.twinkle = 0.6f;
					this.twinkleSpeed *= -1f;
				}
				this.rotation += this.rotationSpeed;
				if ((double)this.rotation > 6.28)
				{
					this.rotation -= 6.28f;
				}
				if (this.rotation < 0f)
				{
					this.rotation += 6.28f;
				}
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0001111C File Offset: 0x0000F31C
		public static void UpdateStars()
		{
			if (Main.onlyDrawFancyUI)
			{
				return;
			}
			if (!Main.dayTime)
			{
				Star.dayCheck = false;
			}
			else if (!Star.dayCheck && Main.time >= 27000.0)
			{
				for (int i = 0; i < Main.numStars; i++)
				{
					if (Main.star[i].hidden)
					{
						Star.SpawnStars(i);
					}
				}
			}
			for (int j = 0; j < Main.numStars; j++)
			{
				Main.star[j].Update();
			}
			if (Main.gameMenu && WorldGen.generatingWorld && WorldGen.SecretSeed.anySecretSeedIsActive)
			{
				for (int k = 0; k < Main.numStars; k++)
				{
					if (!Main.star[k].falling && !Main.star[k].hidden && Main.star[k].scale > 0f)
					{
						for (int l = 0; l < Main.numStars; l++)
						{
							if (k != l && !Main.star[l].falling && !Main.star[l].hidden && Main.star[l].position != Main.star[k].position)
							{
								Vector2 vector = Main.star[l].position - Main.star[k].position;
								float num = vector.X * vector.X + vector.Y * vector.Y;
								vector *= 0.005f * Main.star[l].scale;
								if (num != 0f)
								{
									if (vector.X != 0f)
									{
										Star star = Main.star[k];
										star.velocity.X = star.velocity.X + vector.X / num / Main.star[k].scale;
									}
									if (vector.Y != 0f)
									{
										Star star2 = Main.star[k];
										star2.velocity.Y = star2.velocity.Y + vector.Y / num / Main.star[k].scale;
									}
								}
								if (float.IsNaN(Main.star[k].velocity.X) || float.IsInfinity(Main.star[k].velocity.X) || float.IsNaN(Main.star[k].velocity.Y) || float.IsInfinity(Main.star[k].velocity.Y))
								{
									Main.star[k].velocity = default(Vector2);
									Main.star[k].position = default(Vector2);
									Main.star[k].hidden = true;
								}
							}
						}
						Main.star[k].position += Main.star[k].velocity;
						Main.star[k].rotation += Main.star[k].velocity.X * 0.02f;
						if (Main.star[k].position.X < 0f)
						{
							Main.star[k].velocity.X = Math.Abs(Main.star[k].velocity.X);
						}
						if (Main.star[k].position.X > (float)Main.MaxWorldViewSize.X)
						{
							Main.star[k].velocity.X = -Math.Abs(Main.star[k].velocity.X);
						}
						if (Main.star[k].position.Y < 0f)
						{
							Main.star[k].velocity.Y = Math.Abs(Main.star[k].velocity.Y);
						}
						if (Main.star[k].position.Y > (float)Main.MaxWorldViewSize.Y)
						{
							Main.star[k].velocity.Y = -Math.Abs(Main.star[k].velocity.Y);
						}
					}
				}
			}
		}

		// Token: 0x0400010F RID: 271
		public Vector2 position;

		// Token: 0x04000110 RID: 272
		public float scale;

		// Token: 0x04000111 RID: 273
		public float rotation;

		// Token: 0x04000112 RID: 274
		public int type;

		// Token: 0x04000113 RID: 275
		public float twinkle;

		// Token: 0x04000114 RID: 276
		public float twinkleSpeed;

		// Token: 0x04000115 RID: 277
		public float rotationSpeed;

		// Token: 0x04000116 RID: 278
		public bool falling;

		// Token: 0x04000117 RID: 279
		public bool hidden;

		// Token: 0x04000118 RID: 280
		public Vector2 fallSpeed;

		// Token: 0x04000119 RID: 281
		public int fallTime;

		// Token: 0x0400011A RID: 282
		public Vector2 velocity;

		// Token: 0x0400011B RID: 283
		public static bool dayCheck = false;

		// Token: 0x0400011C RID: 284
		public static float starfallBoost = 1f;

		// Token: 0x0400011D RID: 285
		public static int starFallCount = 0;

		// Token: 0x0400011E RID: 286
		public float fadeIn;
	}
}
