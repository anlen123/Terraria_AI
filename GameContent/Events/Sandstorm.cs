using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Events
{
	// Token: 0x020004FB RID: 1275
	public class Sandstorm
	{
		// Token: 0x060035A5 RID: 13733 RVA: 0x0061B8F7 File Offset: 0x00619AF7
		private static bool HasSufficientWind()
		{
			return Math.Abs(Main.windSpeedCurrent) >= 0.6f;
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x0061B90D File Offset: 0x00619B0D
		public static void WorldClear()
		{
			Sandstorm.Happening = false;
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x0061B918 File Offset: 0x00619B18
		public static void UpdateTime()
		{
			if (Main.netMode != 1)
			{
				if (Sandstorm.Happening)
				{
					if (Sandstorm.TimeLeft > 86400)
					{
						Sandstorm.TimeLeft = 0;
					}
					Sandstorm.TimeLeft -= Main.dayRate;
					if (!Sandstorm.HasSufficientWind())
					{
						Sandstorm.TimeLeft -= 15 * Main.dayRate;
					}
					if (Main.windSpeedCurrent == 0f)
					{
						Sandstorm.TimeLeft = 0;
					}
					if (Sandstorm.TimeLeft <= 0)
					{
						Sandstorm.StopSandstorm();
					}
				}
				else
				{
					int num = 21600;
					if (Main.hardMode)
					{
						num *= 2;
					}
					else
					{
						num *= 3;
					}
					if (Sandstorm.HasSufficientWind())
					{
						for (int i = 0; i < Main.dayRate; i++)
						{
							if (Main.rand.Next(num) == 0)
							{
								Sandstorm.StartSandstorm();
							}
						}
					}
				}
				if (Main.rand.Next(18000) == 0)
				{
					Sandstorm.ChangeSeverityIntentions();
				}
			}
			Sandstorm.UpdateSeverity();
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x0061B9F0 File Offset: 0x00619BF0
		private static void ChangeSeverityIntentions()
		{
			if (Sandstorm.Happening)
			{
				Sandstorm.IntendedSeverity = 0.4f + Main.rand.NextFloat();
			}
			else if (Main.rand.Next(3) == 0)
			{
				Sandstorm.IntendedSeverity = 0f;
			}
			else
			{
				Sandstorm.IntendedSeverity = Main.rand.NextFloat() * 0.3f;
			}
			if (Main.netMode != 1)
			{
				NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x0061BA70 File Offset: 0x00619C70
		private static void UpdateSeverity()
		{
			if (float.IsNaN(Sandstorm.Severity))
			{
				Sandstorm.Severity = 0f;
			}
			if (float.IsNaN(Sandstorm.IntendedSeverity))
			{
				Sandstorm.IntendedSeverity = 0f;
			}
			int num = Math.Sign(Sandstorm.IntendedSeverity - Sandstorm.Severity);
			Sandstorm.Severity = MathHelper.Clamp(Sandstorm.Severity + 0.003f * (float)num, 0f, 1f);
			int num2 = Math.Sign(Sandstorm.IntendedSeverity - Sandstorm.Severity);
			if (num != num2)
			{
				Sandstorm.Severity = Sandstorm.IntendedSeverity;
			}
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x0061BAFB File Offset: 0x00619CFB
		private static void StartSandstorm()
		{
			Sandstorm.Happening = true;
			Sandstorm.TimeLeft = Main.rand.Next(28800, 86401);
			Sandstorm.ChangeSeverityIntentions();
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x0061BB21 File Offset: 0x00619D21
		private static void StopSandstorm()
		{
			Sandstorm.Happening = false;
			Sandstorm.TimeLeft = 0;
			Sandstorm.ChangeSeverityIntentions();
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x0061BB34 File Offset: 0x00619D34
		public static bool ShowSandstormVisuals()
		{
			return Sandstorm.Happening && Main.SceneMetrics.ZoneSandstorm && SurfaceBackgroundID.Sets.IsDesertVariant[Main.bgStyle] && Main.bgDelay < 50;
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x0061BB64 File Offset: 0x00619D64
		public static void EmitDust()
		{
			if (Main.gamePaused)
			{
				return;
			}
			int desertSandTileCount = Main.SceneMetrics.DesertSandTileCount;
			if (!Sandstorm.ShowSandstormVisuals())
			{
				return;
			}
			if (desertSandTileCount < 100)
			{
				return;
			}
			int maxValue = 1;
			if (Main.rand.Next(maxValue) != 0)
			{
				return;
			}
			int num = Math.Sign(Main.windSpeedCurrent);
			float num2 = Math.Abs(Main.windSpeedCurrent);
			if (num2 < 0.01f)
			{
				return;
			}
			float num3 = (float)num * MathHelper.Lerp(0.9f, 1f, num2);
			float num4 = 2000f / (float)desertSandTileCount;
			float num5 = 3f / num4;
			num5 = MathHelper.Clamp(num5, 0.77f, 1f);
			int num6 = (int)num4;
			float num7 = (float)Main.screenWidth / (float)Main.maxScreenW;
			int num8 = (int)(1000f * num7);
			float num9 = 20f * Sandstorm.Severity;
			float num10 = (float)num8 * (Main.gfxQuality * 0.5f + 0.5f) + (float)num8 * 0.1f - (float)Dust.SandStormCount;
			if (num10 <= 0f)
			{
				return;
			}
			float num11 = (float)Main.screenWidth + 1000f;
			float num12 = (float)Main.screenHeight;
			WeightedRandom<Color> weightedRandom = new WeightedRandom<Color>();
			weightedRandom.Add(new Color(200, 160, 20, 180), (double)(Main.SceneMetrics.GetTileCount(53) + Main.SceneMetrics.GetTileCount(396) + Main.SceneMetrics.GetTileCount(397)));
			weightedRandom.Add(new Color(103, 98, 122, 180), (double)(Main.SceneMetrics.GetTileCount(112) + Main.SceneMetrics.GetTileCount(400) + Main.SceneMetrics.GetTileCount(398)));
			weightedRandom.Add(new Color(135, 43, 34, 180), (double)(Main.SceneMetrics.GetTileCount(234) + Main.SceneMetrics.GetTileCount(401) + Main.SceneMetrics.GetTileCount(399)));
			weightedRandom.Add(new Color(213, 196, 197, 180), (double)(Main.SceneMetrics.GetTileCount(116) + Main.SceneMetrics.GetTileCount(403) + Main.SceneMetrics.GetTileCount(402)));
			float num13 = MathHelper.Lerp(0.2f, 0.35f, Sandstorm.Severity);
			float num14 = MathHelper.Lerp(0.5f, 0.7f, Sandstorm.Severity);
			float amount = (num5 - 0.77f) / 0.23000002f;
			int maxValue2 = (int)MathHelper.Lerp(1f, 10f, amount);
			int num15 = 0;
			while ((float)num15 < num9)
			{
				if (Main.rand.Next(num6 / 4) == 0)
				{
					Vector2 vector = new Vector2(Main.rand.NextFloat() * num11 - 500f, Main.rand.NextFloat() * -50f);
					if (Main.rand.Next(3) == 0 && num == 1)
					{
						vector.X = (float)(Main.rand.Next(500) - 500);
					}
					else if (Main.rand.Next(3) == 0 && num == -1)
					{
						vector.X = (float)(Main.rand.Next(500) + Main.screenWidth);
					}
					if (vector.X < 0f || vector.X > (float)Main.screenWidth)
					{
						vector.Y += Main.rand.NextFloat() * num12 * 0.9f;
					}
					vector += Main.screenPosition;
					int num16 = (int)vector.X / 16;
					int num17 = (int)vector.Y / 16;
					if (WorldGen.InWorld(num16, num17, 10) && Main.tile[num16, num17] != null && Main.tile[num16, num17].wall == 0)
					{
						for (int i = 0; i < 1; i++)
						{
							Dust dust = Main.dust[Dust.NewDust(vector, 10, 10, 268, 0f, 0f, 0, default(Color), 1f)];
							dust.velocity.Y = 2f + Main.rand.NextFloat() * 0.2f;
							Dust dust2 = dust;
							dust2.velocity.Y = dust2.velocity.Y * dust.scale;
							Dust dust3 = dust;
							dust3.velocity.Y = dust3.velocity.Y * 0.35f;
							dust.velocity.X = num3 * 5f + Main.rand.NextFloat() * 1f;
							Dust dust4 = dust;
							dust4.velocity.X = dust4.velocity.X + num3 * num14 * 20f;
							dust.fadeIn += num14 * 0.2f;
							dust.velocity *= 1f + num13 * 0.5f;
							dust.color = weightedRandom;
							dust.velocity *= 1f + num13;
							dust.velocity *= num5;
							dust.scale = 0.9f;
							num10 -= 1f;
							if (num10 <= 0f)
							{
								break;
							}
							if (Main.rand.Next(maxValue2) != 0)
							{
								i--;
								vector += Utils.RandomVector2(Main.rand, -10f, 10f) + dust.velocity * -1.1f;
								num16 = (int)vector.X / 16;
								num17 = (int)vector.Y / 16;
								if (WorldGen.InWorld(num16, num17, 10) && Main.tile[num16, num17] != null)
								{
									ushort wall = Main.tile[num16, num17].wall;
								}
							}
						}
						if (num10 <= 0f)
						{
							break;
						}
					}
				}
				num15++;
			}
		}

		// Token: 0x04005AAE RID: 23214
		private const int SANDSTORM_DURATION_MINIMUM = 28800;

		// Token: 0x04005AAF RID: 23215
		private const int SANDSTORM_DURATION_MAXIMUM = 86400;

		// Token: 0x04005AB0 RID: 23216
		public static bool Happening;

		// Token: 0x04005AB1 RID: 23217
		public static int TimeLeft;

		// Token: 0x04005AB2 RID: 23218
		public static float Severity;

		// Token: 0x04005AB3 RID: 23219
		public static float IntendedSeverity;
	}
}
