using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using ReLogic.Graphics;
using ReLogic.OS;
using ReLogic.Utilities;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.Utilities;
using Terraria.Utilities.Terraria.Utilities;

namespace Terraria
{
	// Token: 0x02000052 RID: 82
	public static class Utils
	{
		// Token: 0x06000F30 RID: 3888 RVA: 0x000454AA File Offset: 0x000436AA
		public static Color ColorLerp_BlackToWhite(float percent)
		{
			return Color.Lerp(Color.Black, Color.White, percent);
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0040B10A File Offset: 0x0040930A
		public static double Lerp(double value1, double value2, double amount)
		{
			return value1 + (value2 - value1) * amount;
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0040B113 File Offset: 0x00409313
		public static Vector2 Round(Vector2 input)
		{
			return new Vector2((float)Math.Round((double)input.X), (float)Math.Round((double)input.Y));
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x0040B134 File Offset: 0x00409334
		public static bool IsPowerOfTwo(int x)
		{
			return x != 0 && (x & x - 1) == 0;
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x0040B143 File Offset: 0x00409343
		public static float SmoothStep(float min, float max, float x)
		{
			return MathHelper.Clamp((x - min) / (max - min), 0f, 1f);
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0040B15B File Offset: 0x0040935B
		public static double SmoothStep(double min, double max, double x)
		{
			return Utils.Clamp<double>((x - min) / (max - min), 0.0, 1.0);
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x0040B17B File Offset: 0x0040937B
		public static float UnclampedSmoothStep(float min, float max, float x)
		{
			return (x - min) / (max - min);
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x0040B17B File Offset: 0x0040937B
		public static double UnclampedSmoothStep(double min, double max, double x)
		{
			return (x - min) / (max - min);
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x0040B184 File Offset: 0x00409384
		public static Dictionary<string, string> ParseArguements(string[] args)
		{
			string text = null;
			string text2 = "";
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].Length != 0)
				{
					if (args[i][0] == '-' || args[i][0] == '+')
					{
						if (text != null)
						{
							dictionary.Add(text.ToLower(), text2);
						}
						text = args[i];
						text2 = "";
					}
					else
					{
						if (text2 != "")
						{
							text2 += " ";
						}
						text2 += args[i];
					}
				}
			}
			if (text != null)
			{
				dictionary.Add(text.ToLower(), text2);
			}
			return dictionary;
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0040B230 File Offset: 0x00409430
		public static void Swap<T>(ref T t1, ref T t2)
		{
			T t3 = t1;
			t1 = t2;
			t2 = t3;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0040B257 File Offset: 0x00409457
		public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
		{
			if (value.CompareTo(max) > 0)
			{
				return max;
			}
			if (value.CompareTo(min) < 0)
			{
				return min;
			}
			return value;
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x0040B280 File Offset: 0x00409480
		public static Rectangle Clamp(Rectangle r, Rectangle bounds)
		{
			return new Rectangle(Utils.Clamp<int>(r.X, bounds.Left, bounds.Right - r.Width), Utils.Clamp<int>(r.Y, bounds.Top, bounds.Bottom - r.Height), r.Width, r.Height);
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0040B2DE File Offset: 0x004094DE
		public static float Turn01ToCyclic010(float value)
		{
			return 1f - ((float)Math.Cos((double)(value * 6.2831855f)) * 0.5f + 0.5f);
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0040B300 File Offset: 0x00409500
		public static float PingPongFrom01To010(float value)
		{
			value %= 1f;
			if (value < 0f)
			{
				value += 1f;
			}
			if (value >= 0.5f)
			{
				return 2f - value * 2f;
			}
			return value * 2f;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0040B33C File Offset: 0x0040953C
		public static void Shift<T>(T[] array, int n)
		{
			if (n == 0 || n >= array.Length || n <= -array.Length)
			{
				return;
			}
			if (n > 0)
			{
				if (n < array.Length)
				{
					Array.Copy(array, 0, array, n, array.Length - n);
					return;
				}
			}
			else if (n > -array.Length)
			{
				Array.Copy(array, -n, array, 0, array.Length + n);
			}
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x0040B38C File Offset: 0x0040958C
		public static float MultiLerp(float percent, params float[] floats)
		{
			float num = 1f / ((float)floats.Length - 1f);
			float num2 = num;
			int num3 = 0;
			while (percent / num2 > 1f && num3 < floats.Length - 2)
			{
				num2 += num;
				num3++;
			}
			return MathHelper.Lerp(floats[num3], floats[num3 + 1], (percent - num * (float)num3) / num);
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0040B3E0 File Offset: 0x004095E0
		public static Color MultiLerp(float percent, params Color[] colors)
		{
			float num = 1f / ((float)colors.Length - 1f);
			float num2 = num;
			int num3 = 0;
			while (percent / num2 > 1f && num3 < colors.Length - 2)
			{
				num2 += num;
				num3++;
			}
			return Color.Lerp(colors[num3], colors[num3 + 1], (percent - num * (float)num3) / num);
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0040B43C File Offset: 0x0040963C
		public static float WrappedLerp(float value1, float value2, float percent)
		{
			float num = percent * 2f;
			if (num > 1f)
			{
				num = 2f - num;
			}
			return MathHelper.Lerp(value1, value2, num);
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0040B469 File Offset: 0x00409669
		public static float GetLerpValue(float from, float to, float t, bool clamped = false)
		{
			if (clamped)
			{
				if (from < to)
				{
					if (t < from)
					{
						return 0f;
					}
					if (t > to)
					{
						return 1f;
					}
				}
				else
				{
					if (t < to)
					{
						return 1f;
					}
					if (t > from)
					{
						return 0f;
					}
				}
			}
			return (t - from) / (to - from);
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x0040B4A1 File Offset: 0x004096A1
		public static float Remap(float fromValue, float fromMin, float fromMax, float toMin, float toMax, bool clamped = true)
		{
			return MathHelper.Lerp(toMin, toMax, Utils.GetLerpValue(fromMin, fromMax, fromValue, clamped));
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0040B4B5 File Offset: 0x004096B5
		public static double Remap(double fromValue, double fromMin, double fromMax, double toMin, double toMax, bool clamped = true)
		{
			return Utils.Lerp(toMin, toMax, Utils.GetLerpValue(fromMin, fromMax, fromValue, clamped));
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0040B4C9 File Offset: 0x004096C9
		public static double EaseOutBounce(double x)
		{
			return Utils.BounceEaseOut(x, 4, 2.0);
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0040B4DC File Offset: 0x004096DC
		private static double BounceEaseOut(double t, int bounces, double elasticity)
		{
			double num = (double)bounces * 3.141592653589793;
			double num2 = Math.Pow(1.0 - t, elasticity);
			double num3 = Math.Abs(Math.Sin(t * num));
			return 1.0 - num2 * num3;
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0040B523 File Offset: 0x00409723
		public static double EaseInCirc(double x)
		{
			return 1.0 - Math.Sqrt(1.0 - x * x);
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0040B541 File Offset: 0x00409741
		public static double EaseOutCirc(double x)
		{
			return Math.Sqrt(1.0 - Math.Pow(x - 1.0, 2.0));
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x0040B56C File Offset: 0x0040976C
		public static void GetPortraitMovement(double t, out double offsetX, out double scaleX)
		{
			t %= 1.0;
			double num = 0.16666666666666666;
			int num2 = (int)(t / num);
			double num3 = t % num / num;
			offsetX = 0.0;
			scaleX = 1.0;
			switch (num2)
			{
			case 0:
				offsetX = 0.0;
				scaleX = 1.0 - 2.0 * num3;
				return;
			case 1:
				offsetX = 0.0 - 0.5 * Utils.EaseOutCirc(num3);
				scaleX = -1.0;
				return;
			case 2:
				offsetX = -0.5 - 0.5 * Utils.EaseOutCirc(num3);
				scaleX = -1.0;
				return;
			case 3:
				offsetX = -1.0;
				scaleX = -1.0 + 2.0 * num3;
				return;
			case 4:
				offsetX = -1.0 + 0.5 * Utils.EaseOutCirc(num3);
				scaleX = 1.0;
				return;
			case 5:
				offsetX = -0.5 + 0.5 * Utils.EaseOutCirc(num3);
				scaleX = 1.0;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x0040B6B8 File Offset: 0x004098B8
		public static Color ShiftHue(Color color, float hueShift, float luminanceShift, float saturationBoost)
		{
			Vector3 vector = Main.rgbToHsl(color);
			float num = (vector.X + hueShift) % 1f;
			if (num < 0f)
			{
				num += 1f;
			}
			return Main.hslToRgb(num, vector.Y + saturationBoost, Utils.Clamp<float>(vector.Z + luminanceShift, 0f, 1f), color.A);
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0040B718 File Offset: 0x00409918
		public static Color ShiftBlueToCyanTheme(Color color)
		{
			return Utils.ShiftHue(color, -0.04f, 0.04f, 0.2f);
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0040B73C File Offset: 0x0040993C
		public static void ClampWithinWorld(ref int minX, ref int minY, ref int maxX, ref int maxY, bool lastValuesInclusiveToIteration = false, int fluffX = 0, int fluffY = 0)
		{
			int num = lastValuesInclusiveToIteration ? 1 : 0;
			minX = Utils.Clamp<int>(minX, fluffX, Main.maxTilesX - num - fluffX);
			maxX = Utils.Clamp<int>(maxX, fluffX, Main.maxTilesX - num - fluffX);
			minY = Utils.Clamp<int>(minY, fluffY, Main.maxTilesY - num - fluffY);
			maxY = Utils.Clamp<int>(maxY, fluffY, Main.maxTilesY - num - fluffY);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0040B7A6 File Offset: 0x004099A6
		public static void DrawNotificationIcon(SpriteBatch spritebatch, Rectangle hitbox, float rotationMultiplier = 1f, bool worldSpace = false)
		{
			Utils.DrawNotificationIcon(spritebatch, hitbox.BottomRight() + new Vector2(-7f, -6f), rotationMultiplier, worldSpace);
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0040B7CC File Offset: 0x004099CC
		public static void DrawNotificationIcon(SpriteBatch spritebatch, Vector2 position, float rotationMultiplier = 1f, bool worldSpace = false)
		{
			Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/UI_quickicon1", 1).Value;
			float amount = (float)Math.Sin((double)(6.2831855f * (Main.GlobalTimeWrappedHourly % 1f / 1f))) * 0.5f + 0.5f;
			Color color = Color.White;
			float num = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly % 2f / 2f * 6.2831855f)) * 6.2831855f * 0.035f * rotationMultiplier;
			if (worldSpace)
			{
				color = Lighting.GetColor(position.ToTileCoordinates());
				position -= Main.screenPosition;
				if (Main.LocalPlayer.gravDir == -1f)
				{
					num += 3.1415927f;
					position = Main.ReverseGravitySupport(position, 0f);
				}
			}
			Color value2 = color;
			value2.A /= 2;
			Color color2 = Color.Lerp(color, value2, amount);
			spritebatch.Draw(value, position, null, color2, num, new Vector2((float)(value.Width / 2), (float)(value.Height - 4)), 1f, SpriteEffects.None, 0f);
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0040B8E4 File Offset: 0x00409AE4
		public static Vector2 ConstrainedToPointInRectangle(Rectangle bounds, Vector2 centerTestPosition)
		{
			if (bounds.Contains(centerTestPosition.ToPoint()))
			{
				return centerTestPosition;
			}
			Vector2 vector = new Vector2((float)bounds.Center.X, (float)bounds.Center.Y);
			Vector2 vector2 = vector - centerTestPosition;
			float val = (vector2.X == 0f) ? float.MaxValue : Math.Abs((vector.X - (float)(bounds.Width / 2) - centerTestPosition.X) / vector2.X);
			float val2 = (vector2.Y == 0f) ? float.MaxValue : Math.Abs((vector.Y - (float)(bounds.Height / 2) - centerTestPosition.Y) / vector2.Y);
			float scaleFactor = Math.Min(val, val2);
			Vector2 vector3 = centerTestPosition + vector2 * scaleFactor;
			vector3.X = MathHelper.Clamp(vector3.X, (float)bounds.Left, (float)bounds.Right);
			vector3.Y = MathHelper.Clamp(vector3.Y, (float)bounds.Top, (float)bounds.Bottom);
			return vector3;
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0040B9F8 File Offset: 0x00409BF8
		private static bool CheckForGoodTeleportationSpot_CheckNoInvalidTiles(int tpx, int tpy, Utils.RandomTeleportationAttemptSettings settings)
		{
			if (settings.tilesToAvoidRange > 0 && settings.tilesToAvoid != null)
			{
				int tilesToAvoidRange = settings.tilesToAvoidRange;
				for (int i = -tilesToAvoidRange; i <= tilesToAvoidRange; i++)
				{
					for (int j = -tilesToAvoidRange; j <= tilesToAvoidRange; j++)
					{
						int num = tpx + i;
						int num2 = tpy + j;
						if (WorldGen.InWorld(num, num2, 2))
						{
							Tile tile = Main.tile[num, num2];
							if (tile != null && tile.active())
							{
								ushort type = tile.type;
								for (int k = 0; k < settings.tilesToAvoid.Length; k++)
								{
									if ((int)type == settings.tilesToAvoid[k])
									{
										return false;
									}
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0040BAA0 File Offset: 0x00409CA0
		public static Vector2 CheckForGoodTeleportationSpot(ref bool canSpawn, int teleportStartX, int teleportRangeX, int teleportStartY, int teleportRangeY, Utils.RandomTeleportationAttemptSettings settings)
		{
			int num = (int)settings.teleporteeSize.X;
			int num2 = (int)settings.teleporteeSize.Y;
			Vector2 teleporteeVelocity = settings.teleporteeVelocity;
			float teleporteeGravityDirection = settings.teleporteeGravityDirection;
			Rectangle rectangle = new Rectangle(teleportStartX, teleportStartY, teleportRangeX, teleportRangeY);
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = num;
			Vector2 vector = new Vector2((float)num4, (float)num5) * 16f + new Vector2((float)(-(float)num6 / 2 + 8), (float)(-(float)num2));
			while (!canSpawn && num3 < settings.attemptsBeforeGivingUp)
			{
				num3++;
				num4 = teleportStartX + Main.rand.Next(teleportRangeX);
				num5 = teleportStartY + Main.rand.Next(teleportRangeY);
				int num7 = 5;
				num4 = (int)MathHelper.Clamp((float)num4, (float)num7, (float)(Main.maxTilesX - num7));
				num5 = (int)MathHelper.Clamp((float)num5, (float)num7, (float)(Main.maxTilesY - num7));
				if (!settings.strictRange || rectangle.Contains(new Point(num4, num5)))
				{
					vector = new Vector2((float)num4, (float)num5) * 16f + new Vector2((float)(-(float)num6 / 2 + 8), (float)(-(float)num2));
					if (!Collision.SolidCollision(vector, num6, num2))
					{
						if (Main.tile[num4, num5] == null)
						{
							Main.tile[num4, num5] = new Tile();
						}
						Tile tile = Main.tile[num4, num5];
						if ((!settings.avoidWalls || tile.wall <= 0) && (tile.wall != 87 || (double)num5 <= Main.worldSurface || NPC.downedPlantBoss) && (!Main.wallDungeon[(int)tile.wall] || (double)num5 <= Main.worldSurface || NPC.downedBoss3) && Utils.CheckForGoodTeleportationSpot_CheckNoInvalidTiles(num4, num5, settings))
						{
							bool flag = false;
							int i = 0;
							while (i < settings.maximumFallDistanceFromOrignalPoint)
							{
								if (settings.strictRange && !rectangle.Contains(new Point(num4, num5 + i)))
								{
									flag = true;
									break;
								}
								if (Main.tile[num4, num5 + i] == null)
								{
									Main.tile[num4, num5 + i] = new Tile();
								}
								Tile tile2 = Main.tile[num4, num5 + i];
								vector = new Vector2((float)num4, (float)(num5 + i)) * 16f + new Vector2((float)(-(float)num6 / 2 + 8), (float)(-(float)num2));
								Collision.SlopeCollision(vector, teleporteeVelocity, num6, num2, teleporteeGravityDirection, false, false);
								if (!Collision.SolidCollision(vector, num6, num2 + 1, settings.allowSolidTopFloor))
								{
									i++;
								}
								else
								{
									if (tile2.active() && !tile2.inActive() && Main.tileSolid[(int)tile2.type])
									{
										break;
									}
									i++;
								}
							}
							if (!flag)
							{
								int num8 = (int)vector.X / 16;
								int num9 = (int)vector.Y / 16;
								if (Utils.CheckForGoodTeleportationSpot_CheckNoInvalidTiles(num8, num9, settings))
								{
									int num10 = (int)(vector.X + (float)num6 * 0.5f) / 16;
									int num11 = (int)(vector.Y + (float)num2) / 16;
									Tile tileSafely = Framing.GetTileSafely(num8, num9);
									Tile tileSafely2 = Framing.GetTileSafely(num10, num11);
									if ((settings.specializedConditions == null || settings.specializedConditions(tileSafely2, num10, num11)) && (!settings.avoidAnyLiquid || tileSafely2.liquid <= 0))
									{
										if (settings.mostlySolidFloor)
										{
											Tile tileSafely3 = Framing.GetTileSafely(num10 - 1, num11);
											Tile tileSafely4 = Framing.GetTileSafely(num10 + 1, num11);
											bool flag2;
											bool flag3;
											if (settings.allowSolidTopFloor)
											{
												flag2 = (!tileSafely3.inActive() && WorldGen.SolidTileAllowBottomSlope(num10 - 1, num11));
												flag3 = (!tileSafely4.inActive() && WorldGen.SolidTileAllowBottomSlope(num10 + 1, num11));
											}
											else
											{
												flag2 = (tileSafely3.active() && !tileSafely3.inActive() && Main.tileSolid[(int)tileSafely3.type] && !Main.tileSolidTop[(int)tileSafely3.type]);
												flag3 = (tileSafely4.active() && !tileSafely4.inActive() && Main.tileSolid[(int)tileSafely4.type] && !Main.tileSolidTop[(int)tileSafely4.type]);
											}
											if (!flag2 && !flag3)
											{
												continue;
											}
										}
										if ((!settings.avoidWalls || tileSafely.wall <= 0) && (!settings.avoidAnyLiquid || !Collision.WetCollision(vector, num6, num2)) && (!settings.avoidLava || !Collision.LavaCollision(vector, num6, num2)) && (!settings.avoidHurtTiles || !Collision.AnyHurtingTiles(vector, num6, num2)) && !Collision.SolidCollision(vector, num6, num2, settings.allowSolidTopFloor) && i < settings.maximumFallDistanceFromOrignalPoint - 1)
										{
											Vector2 vector2 = Vector2.UnitX * 16f;
											if (!(Collision.TileCollision(vector - vector2, vector2, num, num2, false, false, (int)teleporteeGravityDirection, false, false, true) != vector2))
											{
												vector2 = -Vector2.UnitX * 16f;
												if (!(Collision.TileCollision(vector - vector2, vector2, num, num2, false, false, (int)teleporteeGravityDirection, false, false, true) != vector2))
												{
													vector2 = Vector2.UnitY * 16f;
													if (!(Collision.TileCollision(vector - vector2, vector2, num, num2, false, false, (int)teleporteeGravityDirection, false, false, true) != vector2))
													{
														vector2 = -Vector2.UnitY * 16f;
														if (!(Collision.TileCollision(vector - vector2, vector2, num, num2, false, false, (int)teleporteeGravityDirection, false, false, true) != vector2) && (!Main.dualDungeonsSeed || !UnbreakableWallScan.InsideUnbreakableWalls(new Point(num8, num9))))
														{
															canSpawn = true;
															num5 += i;
															break;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return vector;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0040C08C File Offset: 0x0040A28C
		public static Utils.ChaseResults GetChaseResults(Vector2 chaserPosition, float chaserSpeed, Vector2 runnerPosition, Vector2 runnerVelocity)
		{
			Utils.ChaseResults chaseResults = default(Utils.ChaseResults);
			if (chaserPosition == runnerPosition)
			{
				return new Utils.ChaseResults
				{
					InterceptionHappens = true,
					InterceptionPosition = chaserPosition,
					InterceptionTime = 0f,
					ChaserVelocity = Vector2.Zero
				};
			}
			if (chaserSpeed <= 0f)
			{
				return default(Utils.ChaseResults);
			}
			Vector2 value = chaserPosition - runnerPosition;
			float num = value.Length();
			float num2 = runnerVelocity.Length();
			if (num2 == 0f)
			{
				chaseResults.InterceptionTime = num / chaserSpeed;
				chaseResults.InterceptionPosition = runnerPosition;
			}
			else
			{
				float a = chaserSpeed * chaserSpeed - num2 * num2;
				float b = 2f * Vector2.Dot(value, runnerVelocity);
				float c = -num * num;
				float num3;
				float num4;
				if (!Utils.SolveQuadratic(a, b, c, out num3, out num4))
				{
					return default(Utils.ChaseResults);
				}
				if (num3 < 0f && num4 < 0f)
				{
					return default(Utils.ChaseResults);
				}
				if (num3 > 0f && num4 > 0f)
				{
					chaseResults.InterceptionTime = Math.Min(num3, num4);
				}
				else
				{
					chaseResults.InterceptionTime = Math.Max(num3, num4);
				}
				chaseResults.InterceptionPosition = runnerPosition + runnerVelocity * chaseResults.InterceptionTime;
			}
			chaseResults.ChaserVelocity = (chaseResults.InterceptionPosition - chaserPosition) / chaseResults.InterceptionTime;
			chaseResults.InterceptionHappens = true;
			return chaseResults;
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0040C1F0 File Offset: 0x0040A3F0
		public static float GetJumpForce(float jumpHeight, float atGravity)
		{
			return (float)Math.Sqrt((double)(jumpHeight / atGravity * 2f)) * atGravity;
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0040C214 File Offset: 0x0040A414
		public static float GetJumpTimeToApex(float jumpHeight, float atGravity)
		{
			return (float)Math.Sqrt((double)(jumpHeight / atGravity * 2f));
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0040C234 File Offset: 0x0040A434
		public static Vector2 FactorAcceleration(Vector2 currentVelocity, float timeToInterception, Vector2 descendOfProjectile, int framesOfLenience)
		{
			float num = Math.Max(0f, timeToInterception - (float)framesOfLenience);
			Vector2 value = descendOfProjectile * (num * num) / 2f / timeToInterception;
			return currentVelocity - value;
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0040C274 File Offset: 0x0040A474
		public static bool SolveQuadratic(float a, float b, float c, out float result1, out float result2)
		{
			float num = b * b - 4f * a * c;
			result1 = 0f;
			result2 = 0f;
			if (num > 0f)
			{
				result1 = (-b + (float)Math.Sqrt((double)num)) / (2f * a);
				result2 = (-b - (float)Math.Sqrt((double)num)) / (2f * a);
				return true;
			}
			if (num < 0f)
			{
				return false;
			}
			result1 = (result2 = (-b + (float)Math.Sqrt((double)num)) / (2f * a));
			return true;
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0040C2FC File Offset: 0x0040A4FC
		public static double GetLerpValue(double from, double to, double t, bool clamped = false)
		{
			if (clamped)
			{
				if (from < to)
				{
					if (t < from)
					{
						return 0.0;
					}
					if (t > to)
					{
						return 1.0;
					}
				}
				else
				{
					if (t < to)
					{
						return 1.0;
					}
					if (t > from)
					{
						return 0.0;
					}
				}
			}
			return (t - from) / (to - from);
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0040C350 File Offset: 0x0040A550
		public static float GetDayTimeAs24FloatStartingFromMidnight()
		{
			if (Main.dayTime)
			{
				return 4.5f + (float)(Main.time / 54000.0) * 15f;
			}
			return 19.5f + (float)(Main.time / 32400.0) * 9f;
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0040C39D File Offset: 0x0040A59D
		public static Vector2 GetDayTimeAsDirectionIn24HClock()
		{
			return Utils.GetDayTimeAsDirectionIn24HClock(Utils.GetDayTimeAs24FloatStartingFromMidnight());
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0040C3AC File Offset: 0x0040A5AC
		public static Vector2 GetDayTimeAsDirectionIn24HClock(float timeFrom0To24)
		{
			return new Vector2(0f, -1f).RotatedBy((double)(timeFrom0To24 / 24f * 6.2831855f), default(Vector2));
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0040C3E4 File Offset: 0x0040A5E4
		public static string[] ConvertMonoArgsToDotNet(string[] brokenArgs)
		{
			ArrayList arrayList = new ArrayList();
			string text = "";
			for (int i = 0; i < brokenArgs.Length; i++)
			{
				if (brokenArgs[i].StartsWith("-"))
				{
					if (text != "")
					{
						arrayList.Add(text);
						text = "";
					}
					else
					{
						arrayList.Add("");
					}
					arrayList.Add(brokenArgs[i]);
				}
				else
				{
					if (text != "")
					{
						text += " ";
					}
					text += brokenArgs[i];
				}
			}
			arrayList.Add(text);
			string[] array = new string[arrayList.Count];
			arrayList.CopyTo(array);
			return array;
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0040C490 File Offset: 0x0040A690
		public static T Max<T>(params T[] args) where T : IComparable
		{
			T result = args[0];
			for (int i = 1; i < args.Length; i++)
			{
				if (result.CompareTo(args[i]) < 0)
				{
					result = args[i];
				}
			}
			return result;
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0040C4D8 File Offset: 0x0040A6D8
		public static float LineRectangleDistance(Rectangle rect, Vector2 lineStart, Vector2 lineEnd)
		{
			Vector2 vector = rect.TopLeft();
			Vector2 vector2 = rect.TopRight();
			Vector2 vector3 = rect.BottomLeft();
			Vector2 vector4 = rect.BottomRight();
			if (lineStart.Between(vector, vector4) || lineEnd.Between(vector, vector4))
			{
				return 0f;
			}
			float value = vector.Distance(vector.ClosestPointOnLine(lineStart, lineEnd));
			float value2 = vector2.Distance(vector2.ClosestPointOnLine(lineStart, lineEnd));
			float value3 = vector3.Distance(vector3.ClosestPointOnLine(lineStart, lineEnd));
			float value4 = vector4.Distance(vector4.ClosestPointOnLine(lineStart, lineEnd));
			return MathHelper.Min(value, MathHelper.Min(value2, MathHelper.Min(value3, value4)));
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0040C570 File Offset: 0x0040A770
		public static List<List<TextSnippet>> WordwrapStringSmart(string text, Color c, DynamicSpriteFont font, float maxWidth = -1f, int maxLines = -1)
		{
			List<List<TextSnippet>> list = new List<List<TextSnippet>>();
			List<TextSnippet> list2 = new List<TextSnippet>();
			list.Add(list2);
			foreach (PositionedSnippet positionedSnippet in ChatManager.LayoutSnippets(font, ChatManager.ParseMessage(text, c), Vector2.One, maxWidth))
			{
				while (positionedSnippet.Line >= list.Count)
				{
					if (list.Count == maxLines)
					{
						return list;
					}
					list.Add(list2 = new List<TextSnippet>());
				}
				list2.Add(positionedSnippet.Snippet);
			}
			return list;
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0040C614 File Offset: 0x0040A814
		public static string[] WordwrapString(string text, DynamicSpriteFont font, int maxWidth, int maxLines, out int lineAmount)
		{
			string[] array = font.CreateWrappedText(text, (float)maxWidth, Language.ActiveCulture.CultureInfo).Split(new char[]
			{
				'\n'
			});
			lineAmount = Math.Min(array.Length, maxLines);
			string[] array2 = new string[maxLines];
			Array.Copy(array, array2, lineAmount);
			return array2;
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0040C664 File Offset: 0x0040A864
		public static string[] WordwrapStringLegacy(string text, DynamicSpriteFont font, int maxWidth, int maxLines, out int lineAmount)
		{
			string[] array = new string[maxLines];
			int num = 0;
			List<string> list = new List<string>(text.Split(new char[]
			{
				'\n'
			}));
			List<string> list2 = new List<string>(list[0].Split(new char[]
			{
				' '
			}));
			int num2 = 1;
			while (num2 < list.Count && num2 < maxLines)
			{
				list2.Add("\n");
				list2.AddRange(list[num2].Split(new char[]
				{
					' '
				}));
				num2++;
			}
			bool flag = true;
			while (list2.Count > 0)
			{
				string text2 = list2[0];
				string str = " ";
				if (list2.Count == 1)
				{
					str = "";
				}
				if (text2 == "\n")
				{
					string[] array2 = array;
					int num3 = num++;
					array2[num3] += text2;
					flag = true;
					if (num >= maxLines)
					{
						break;
					}
					list2.RemoveAt(0);
				}
				else if (flag)
				{
					if (font.MeasureString(text2).X > (float)maxWidth)
					{
						string str2 = text2[0].ToString() ?? "";
						int num4 = 1;
						while (font.MeasureString(str2 + text2[num4].ToString() + "-").X <= (float)maxWidth)
						{
							str2 += text2[num4++].ToString();
						}
						str2 += "-";
						array[num++] = str2 + " ";
						if (num >= maxLines)
						{
							break;
						}
						list2.RemoveAt(0);
						list2.Insert(0, text2.Substring(num4));
					}
					else
					{
						string[] array3 = array;
						int num5 = num;
						array3[num5] = array3[num5] + text2 + str;
						flag = false;
						list2.RemoveAt(0);
					}
				}
				else if (font.MeasureString(array[num] + text2).X > (float)maxWidth)
				{
					num++;
					if (num >= maxLines)
					{
						break;
					}
					flag = true;
				}
				else
				{
					string[] array4 = array;
					int num6 = num;
					array4[num6] = array4[num6] + text2 + str;
					flag = false;
					list2.RemoveAt(0);
				}
			}
			lineAmount = Math.Min(num + 1, maxLines);
			return array;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0040C897 File Offset: 0x0040AA97
		public static Rectangle CenteredRectangle(Vector2 center, Vector2 size)
		{
			return new Rectangle((int)(center.X - size.X / 2f), (int)(center.Y - size.Y / 2f), (int)size.X, (int)size.Y);
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0040C8D4 File Offset: 0x0040AAD4
		public static Rectangle CenteredRectangle(Point center, Point size)
		{
			return new Rectangle(center.X - size.X / 2, center.Y - size.Y / 2, size.X, size.Y);
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0040C908 File Offset: 0x0040AB08
		public static Rectangle Including(this Rectangle rect, Point point)
		{
			int num = Math.Min(rect.Left, point.X);
			int num2 = Math.Max(rect.Right, point.X);
			int num3 = Math.Min(rect.Top, point.Y);
			int num4 = Math.Max(rect.Bottom, point.Y);
			return new Rectangle(num, num3, num2 - num, num4 - num3);
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0040C970 File Offset: 0x0040AB70
		public static Vector2 Vector2FromElipse(Vector2 angleVector, Vector2 elipseSizes)
		{
			if (elipseSizes == Vector2.Zero)
			{
				return Vector2.Zero;
			}
			if (angleVector == Vector2.Zero)
			{
				return Vector2.Zero;
			}
			angleVector.Normalize();
			Vector2 value = Vector2.Normalize(elipseSizes);
			value = Vector2.One / value;
			angleVector *= value;
			angleVector.Normalize();
			return angleVector * elipseSizes / 2f;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0040C9DE File Offset: 0x0040ABDE
		public static bool FloatIntersect(float r1StartX, float r1StartY, float r1Width, float r1Height, float r2StartX, float r2StartY, float r2Width, float r2Height)
		{
			return r1StartX <= r2StartX + r2Width && r1StartY <= r2StartY + r2Height && r1StartX + r1Width >= r2StartX && r1StartY + r1Height >= r2StartY;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0040C9DE File Offset: 0x0040ABDE
		public static bool DoubleIntersect(double r1StartX, double r1StartY, double r1Width, double r1Height, double r2StartX, double r2StartY, double r2Width, double r2Height)
		{
			return r1StartX <= r2StartX + r2Width && r1StartY <= r2StartY + r2Height && r1StartX + r1Width >= r2StartX && r1StartY + r1Height >= r2StartY;
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0040CA04 File Offset: 0x0040AC04
		public static bool LineSegmentsIntersect(Vector2D start1, Vector2D end1, Vector2D start2, Vector2D end2)
		{
			Vector2D vector2D = end1 - start1;
			Vector2D vector2D2 = end2 - start2;
			double num = Vector2D.Cross(vector2D, vector2D2);
			if (num == 0.0)
			{
				return false;
			}
			Vector2D vector2D3 = start2 - start1;
			double num2 = Vector2D.Cross(vector2D3, vector2D) / num;
			double num3 = Vector2D.Cross(vector2D3, vector2D) / num;
			double num4 = Vector2D.Cross(vector2D3, vector2D) / num;
			return 0.0 <= num3 && num3 <= 1.0 && 0.0 <= num4 && num4 <= 1.0;
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0040CA94 File Offset: 0x0040AC94
		public static long CoinsCount(out bool overFlowing, Item[] inv, params int[] ignoreSlots)
		{
			List<int> list = new List<int>(ignoreSlots);
			long num = 0L;
			for (int i = 0; i < inv.Length; i++)
			{
				if (!list.Contains(i))
				{
					switch (inv[i].type)
					{
					case 71:
						num += (long)inv[i].stack;
						break;
					case 72:
						num += (long)inv[i].stack * 100L;
						break;
					case 73:
						num += (long)inv[i].stack * 10000L;
						break;
					case 74:
						num += (long)inv[i].stack * 1000000L;
						break;
					}
				}
			}
			overFlowing = false;
			return num;
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0040CB34 File Offset: 0x0040AD34
		public static int[] CoinsSplit(long count)
		{
			int[] array = new int[4];
			long num = 0L;
			long num2 = 1000000L;
			for (int i = 3; i >= 0; i--)
			{
				array[i] = (int)((count - num) / num2);
				num += (long)array[i] * num2;
				num2 /= 100L;
			}
			return array;
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0040CB78 File Offset: 0x0040AD78
		public static long CoinsCombineStacks(out bool overFlowing, params long[] coinCounts)
		{
			long num = 0L;
			foreach (long num2 in coinCounts)
			{
				num += num2;
				if (num >= 9999999999L)
				{
					overFlowing = true;
					return 9999999999L;
				}
			}
			overFlowing = false;
			return num;
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0040CBC0 File Offset: 0x0040ADC0
		public static void PoofOfSmoke(Vector2 position)
		{
			int num = Main.rand.Next(3, 7);
			for (int i = 0; i < num; i++)
			{
				int num2 = Gore.NewGore(position, (Main.rand.NextFloat() * 6.2831855f).ToRotationVector2() * new Vector2(2f, 0.7f) * 0.7f, Main.rand.Next(11, 14), 1f);
				Main.gore[num2].scale = 0.7f;
				Main.gore[num2].velocity *= 0.5f;
			}
			for (int j = 0; j < 10; j++)
			{
				Dust dust = Main.dust[Dust.NewDust(position, 14, 14, 16, 0f, 0f, 100, default(Color), 1.5f)];
				dust.position += new Vector2(5f);
				dust.velocity = (Main.rand.NextFloat() * 6.2831855f).ToRotationVector2() * new Vector2(2f, 0.7f) * 0.7f * (0.5f + 0.5f * Main.rand.NextFloat());
			}
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0040CD11 File Offset: 0x0040AF11
		public static Vector2 ToScreenPosition(this Vector2 worldPosition)
		{
			return Vector2.Transform(worldPosition - Main.screenPosition, Main.GameViewMatrix.TransformationMatrix) / Main.UIScale;
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0040CD37 File Offset: 0x0040AF37
		public static Vector2 ScreenToWorldPosition(this Vector2 screenPosition)
		{
			return Vector2.Transform(screenPosition * Main.UIScale, Matrix.Invert(Main.GameViewMatrix.TransformationMatrix)) + Main.screenPosition;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0040CD64 File Offset: 0x0040AF64
		public static string PrettifyPercentDisplay(float percent, string originalFormat)
		{
			return percent.ToString(originalFormat, CultureInfo.InvariantCulture).TrimEnd(new char[]
			{
				'0',
				'%',
				' '
			}).TrimEnd(new char[]
			{
				'.',
				' '
			}).TrimStart(new char[]
			{
				'0',
				' '
			}) + "%";
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x0040CDC8 File Offset: 0x0040AFC8
		public static void TrimTextIfNeeded(ref string text, DynamicSpriteFont font, float scale, float maxWidth)
		{
			bool flag = false;
			Vector2 vector = font.MeasureString(text) * scale;
			while (vector.X > maxWidth)
			{
				text = Utils.TrimLastCharacter(text);
				flag = true;
				vector = font.MeasureString(text) * scale;
			}
			if (flag)
			{
				text = Utils.TrimLastCharacter(text) + "…";
			}
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x0040CE24 File Offset: 0x0040B024
		public static string FormatWith(string original, object obj)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
			return Utils._substitutionRegex.Replace(original, delegate(Match match)
			{
				if (match.Groups[1].Length != 0)
				{
					return "";
				}
				string name = match.Groups[2].ToString();
				PropertyDescriptor propertyDescriptor = properties.Find(name, false);
				if (propertyDescriptor == null)
				{
					return "";
				}
				return (propertyDescriptor.GetValue(obj) ?? "").ToString();
			});
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x0040CE6C File Offset: 0x0040B06C
		public static bool TryCreatingDirectory(string folderPath)
		{
			if (Directory.Exists(folderPath))
			{
				return true;
			}
			bool result;
			try
			{
				Directory.CreateDirectory(folderPath);
				result = true;
			}
			catch (Exception exception)
			{
				FancyErrorPrinter.ShowDirectoryCreationFailError(exception, folderPath);
				result = false;
			}
			return result;
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0040CEAC File Offset: 0x0040B0AC
		public static void OpenFolder(string folderPath)
		{
			if (!Utils.TryCreatingDirectory(folderPath))
			{
				return;
			}
			if (Platform.IsLinux)
			{
				Process.Start(new ProcessStartInfo(folderPath)
				{
					FileName = "open-folder",
					Arguments = folderPath,
					UseShellExecute = true,
					CreateNoWindow = true
				});
				return;
			}
			Process.Start(folderPath);
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0040CEFD File Offset: 0x0040B0FD
		public static TimeSpan SWTicksToTimeSpan(long swTicks)
		{
			return new TimeSpan((long)((double)swTicks * 10000000.0 / (double)Stopwatch.Frequency));
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x0040CF18 File Offset: 0x0040B118
		public static long TimeSpanToSWTicks(TimeSpan timeSpan)
		{
			return timeSpan.Ticks * Stopwatch.Frequency / 10000000L;
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0040CF30 File Offset: 0x0040B130
		public static byte[] ToByteArray(this string str)
		{
			byte[] array = new byte[str.Length * 2];
			Buffer.BlockCopy(str.ToCharArray(), 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0040CF5D File Offset: 0x0040B15D
		public static float NextFloat(this UnifiedRandom r)
		{
			return (float)r.NextDouble();
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0040CF66 File Offset: 0x0040B166
		public static float NextFloatDirection(this UnifiedRandom r)
		{
			return (float)r.NextDouble() * 2f - 1f;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0040CF7B File Offset: 0x0040B17B
		public static float NextFloat(this UnifiedRandom random, FloatRange range)
		{
			return random.NextFloat() * (range.Maximum - range.Minimum) + range.Minimum;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0040CF98 File Offset: 0x0040B198
		public static T NextFromList<T>(this UnifiedRandom random, params T[] objs)
		{
			return objs[random.Next(objs.Length)];
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0040CFAC File Offset: 0x0040B1AC
		public static bool JustBecameTrue(bool state, ref bool releasedStateHolder)
		{
			bool result = false;
			if (state)
			{
				if (releasedStateHolder)
				{
					result = true;
				}
				releasedStateHolder = false;
			}
			else
			{
				releasedStateHolder = true;
			}
			return result;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0040CFCD File Offset: 0x0040B1CD
		public static T NextFromCollection<T>(this UnifiedRandom random, List<T> objs)
		{
			return objs[random.Next(objs.Count)];
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0040CFE1 File Offset: 0x0040B1E1
		public static int Next(this UnifiedRandom random, IntRange range)
		{
			return random.Next(range.Minimum, range.Maximum + 1);
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0040CFF7 File Offset: 0x0040B1F7
		public static Point NextFromRectangle(this UnifiedRandom r, Rectangle rect)
		{
			return new Point(r.Next(rect.Left, rect.Right), r.Next(rect.Top, rect.Bottom));
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0040D026 File Offset: 0x0040B226
		public static Vector2 NextVector2Square(this UnifiedRandom r, float min, float max)
		{
			return new Vector2((max - min) * (float)r.NextDouble() + min, (max - min) * (float)r.NextDouble() + min);
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0040D047 File Offset: 0x0040B247
		public static Vector2 NextVector2FromRectangle(this UnifiedRandom r, Rectangle rect)
		{
			return new Vector2((float)rect.X + r.NextFloat() * (float)rect.Width, (float)rect.Y + r.NextFloat() * (float)rect.Height);
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0040D07A File Offset: 0x0040B27A
		public static Vector2 NextVector2Unit(this UnifiedRandom r, float startRotation = 0f, float rotationRange = 6.2831855f)
		{
			return (startRotation + rotationRange * r.NextFloat()).ToRotationVector2();
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0040D08B File Offset: 0x0040B28B
		public static Vector2 NextVector2Circular(this UnifiedRandom r, float circleHalfWidth, float circleHalfHeight)
		{
			return r.NextVector2Unit(0f, 6.2831855f) * new Vector2(circleHalfWidth, circleHalfHeight) * r.NextFloat();
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x0040D0B4 File Offset: 0x0040B2B4
		public static Vector2 NextVector2CircularEdge(this UnifiedRandom r, float circleHalfWidth, float circleHalfHeight)
		{
			return r.NextVector2Unit(0f, 6.2831855f) * new Vector2(circleHalfWidth, circleHalfHeight);
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0040D0D2 File Offset: 0x0040B2D2
		public static Vector2D NextVector2DSquare(this UnifiedRandom r, double min, double max)
		{
			return new Vector2D((max - min) * r.NextDouble() + min, (max - min) * r.NextDouble() + min);
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0040D0F1 File Offset: 0x0040B2F1
		public static Vector2D NextVector2DFromRectangle(this UnifiedRandom r, Rectangle rect)
		{
			return new Vector2D((double)rect.X + r.NextDouble() * (double)rect.Width, (double)rect.Y + r.NextDouble() * (double)rect.Height);
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0040D124 File Offset: 0x0040B324
		public static Vector2D NextVector2DUnit(this UnifiedRandom r, double startRotation = 0.0, double rotationRange = 6.2831854820251465)
		{
			return (startRotation + rotationRange * r.NextDouble()).ToRotationVector2D();
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0040D135 File Offset: 0x0040B335
		public static Vector2D NextVector2DCircular(this UnifiedRandom r, double circleHalfWidth, double circleHalfHeight)
		{
			return r.NextVector2DUnit(0.0, 6.2831854820251465) * new Vector2D(circleHalfWidth, circleHalfHeight) * r.NextDouble();
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0040D166 File Offset: 0x0040B366
		public static Vector2D NextVector2DCircularEdge(this UnifiedRandom r, double circleHalfWidth, double circleHalfHeight)
		{
			return r.NextVector2DUnit(0.0, 6.2831854820251465) * new Vector2D(circleHalfWidth, circleHalfHeight);
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0040D18C File Offset: 0x0040B38C
		public static int Width(this Asset<Texture2D> asset)
		{
			if (!asset.IsLoaded)
			{
				return 0;
			}
			return asset.Value.Width;
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0040D1A3 File Offset: 0x0040B3A3
		public static int Height(this Asset<Texture2D> asset)
		{
			if (!asset.IsLoaded)
			{
				return 0;
			}
			return asset.Value.Height;
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0040D1BA File Offset: 0x0040B3BA
		public static Rectangle Frame(this Asset<Texture2D> tex, int horizontalFrames = 1, int verticalFrames = 1, int frameX = 0, int frameY = 0, int sizeOffsetX = 0, int sizeOffsetY = 0)
		{
			if (!tex.IsLoaded)
			{
				return Rectangle.Empty;
			}
			return tex.Value.Frame(horizontalFrames, verticalFrames, frameX, frameY, sizeOffsetX, sizeOffsetY);
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0040D1DE File Offset: 0x0040B3DE
		public static Rectangle OffsetSize(this Rectangle rect, int xSize, int ySize)
		{
			rect.Width += xSize;
			rect.Height += ySize;
			return rect;
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0040D1F9 File Offset: 0x0040B3F9
		public static Vector2 Size(this Asset<Texture2D> tex)
		{
			if (!tex.IsLoaded)
			{
				return Vector2.Zero;
			}
			return tex.Value.Size();
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0040D214 File Offset: 0x0040B414
		public static Rectangle Frame(this Texture2D tex, int horizontalFrames = 1, int verticalFrames = 1, int frameX = 0, int frameY = 0, int sizeOffsetX = 0, int sizeOffsetY = 0)
		{
			int num = tex.Width / horizontalFrames;
			int num2 = tex.Height / verticalFrames;
			return new Rectangle(num * frameX, num2 * frameY, num + sizeOffsetX, num2 + sizeOffsetY);
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0040D247 File Offset: 0x0040B447
		public static Vector2 OriginFlip(this Rectangle rect, Vector2 origin, SpriteEffects effects)
		{
			if ((effects & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
			{
				origin.X = (float)rect.Width - origin.X;
			}
			if ((effects & SpriteEffects.FlipVertically) != SpriteEffects.None)
			{
				origin.Y = (float)rect.Height - origin.Y;
			}
			return origin;
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0040D27E File Offset: 0x0040B47E
		public static Vector2 Size(this Texture2D tex)
		{
			return new Vector2((float)tex.Width, (float)tex.Height);
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0040D293 File Offset: 0x0040B493
		public static void WriteRGB(this BinaryWriter bb, Color c)
		{
			bb.Write(c.R);
			bb.Write(c.G);
			bb.Write(c.B);
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0040D2BC File Offset: 0x0040B4BC
		public static void WriteVector2(this BinaryWriter bb, Vector2 v)
		{
			bb.Write(v.X);
			bb.Write(v.Y);
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0040D2D8 File Offset: 0x0040B4D8
		public static void WritePackedVector2(this BinaryWriter bb, Vector2 v)
		{
			HalfVector2 halfVector = new HalfVector2(v.X, v.Y);
			bb.Write(halfVector.PackedValue);
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0040D305 File Offset: 0x0040B505
		public static Color ReadRGB(this BinaryReader bb)
		{
			return new Color((int)bb.ReadByte(), (int)bb.ReadByte(), (int)bb.ReadByte());
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0040D31E File Offset: 0x0040B51E
		public static Vector2 ReadVector2(this BinaryReader bb)
		{
			return new Vector2(bb.ReadSingle(), bb.ReadSingle());
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0040D334 File Offset: 0x0040B534
		public static Vector2 ReadPackedVector2(this BinaryReader bb)
		{
			HalfVector2 halfVector = default(HalfVector2);
			halfVector.PackedValue = bb.ReadUInt32();
			return halfVector.ToVector2();
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0040D360 File Offset: 0x0040B560
		public static void Write7BitEncodedInt(this BinaryWriter writer, int value)
		{
			uint num;
			for (num = (uint)value; num > 127U; num >>= 7)
			{
				writer.Write((byte)(num | 4294967168U));
			}
			writer.Write((byte)num);
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0040D390 File Offset: 0x0040B590
		public static int Read7BitEncodedInt(this BinaryReader reader)
		{
			uint num = 0U;
			byte b;
			for (int i = 0; i < 28; i += 7)
			{
				b = reader.ReadByte();
				num |= (uint)((uint)(b & 127) << i);
				if (b <= 127)
				{
					return (int)num;
				}
			}
			b = reader.ReadByte();
			if (b > 15)
			{
				throw new FormatException("Bad 7bit encoded int");
			}
			return (int)(num | (uint)((uint)b << 28));
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0040D3E5 File Offset: 0x0040B5E5
		public static Vector2 Left(this Rectangle r)
		{
			return new Vector2((float)r.X, (float)(r.Y + r.Height / 2));
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0040D403 File Offset: 0x0040B603
		public static Vector2 Right(this Rectangle r)
		{
			return new Vector2((float)(r.X + r.Width), (float)(r.Y + r.Height / 2));
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0040D428 File Offset: 0x0040B628
		public static Vector2 Top(this Rectangle r)
		{
			return new Vector2((float)(r.X + r.Width / 2), (float)r.Y);
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0040D446 File Offset: 0x0040B646
		public static Vector2 Bottom(this Rectangle r)
		{
			return new Vector2((float)(r.X + r.Width / 2), (float)(r.Y + r.Height));
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x0040D46B File Offset: 0x0040B66B
		public static Vector2 TopLeft(this Rectangle r)
		{
			return new Vector2((float)r.X, (float)r.Y);
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0040D480 File Offset: 0x0040B680
		public static Vector2 TopRight(this Rectangle r)
		{
			return new Vector2((float)(r.X + r.Width), (float)r.Y);
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0040D49C File Offset: 0x0040B69C
		public static Vector2 BottomLeft(this Rectangle r)
		{
			return new Vector2((float)r.X, (float)(r.Y + r.Height));
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0040D4B8 File Offset: 0x0040B6B8
		public static Vector2 BottomRight(this Rectangle r)
		{
			return new Vector2((float)(r.X + r.Width), (float)(r.Y + r.Height));
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0040D4DB File Offset: 0x0040B6DB
		public static Vector2D TopLeftDouble(this Rectangle r)
		{
			return new Vector2D((double)r.X, (double)r.Y);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0040D4F0 File Offset: 0x0040B6F0
		public static Vector2D TopRightDouble(this Rectangle r)
		{
			return new Vector2D((double)(r.X + r.Width), (double)r.Y);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0040D50C File Offset: 0x0040B70C
		public static Vector2D BottomLeftDouble(this Rectangle r)
		{
			return new Vector2D((double)r.X, (double)(r.Y + r.Height));
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0040D528 File Offset: 0x0040B728
		public static Vector2D BottomRightDouble(this Rectangle r)
		{
			return new Vector2D((double)(r.X + r.Width), (double)(r.Y + r.Height));
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0040D54B File Offset: 0x0040B74B
		public static Vector2 Center(this Rectangle r)
		{
			return new Vector2((float)(r.X + r.Width / 2), (float)(r.Y + r.Height / 2));
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0040D572 File Offset: 0x0040B772
		public static Vector2 Size(this Rectangle r)
		{
			return new Vector2((float)r.Width, (float)r.Height);
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0040D588 File Offset: 0x0040B788
		public static float Distance(this Rectangle r, Vector2 point)
		{
			if (Utils.FloatIntersect((float)r.Left, (float)r.Top, (float)r.Width, (float)r.Height, point.X, point.Y, 0f, 0f))
			{
				return 0f;
			}
			if (point.X >= (float)r.Left && point.X <= (float)r.Right)
			{
				if (point.Y < (float)r.Top)
				{
					return (float)r.Top - point.Y;
				}
				return point.Y - (float)r.Bottom;
			}
			else if (point.Y >= (float)r.Top && point.Y <= (float)r.Bottom)
			{
				if (point.X < (float)r.Left)
				{
					return (float)r.Left - point.X;
				}
				return point.X - (float)r.Right;
			}
			else if (point.X < (float)r.Left)
			{
				if (point.Y < (float)r.Top)
				{
					return Vector2.Distance(point, r.TopLeft());
				}
				return Vector2.Distance(point, r.BottomLeft());
			}
			else
			{
				if (point.Y < (float)r.Top)
				{
					return Vector2.Distance(point, r.TopRight());
				}
				return Vector2.Distance(point, r.BottomRight());
			}
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0040D6DC File Offset: 0x0040B8DC
		public static double Distance(this Rectangle r, Vector2D point)
		{
			if (Utils.DoubleIntersect((double)r.Left, (double)r.Top, (double)r.Width, (double)r.Height, point.X, point.Y, 0.0, 0.0))
			{
				return 0.0;
			}
			if (point.X >= (double)r.Left && point.X <= (double)r.Right)
			{
				if (point.Y < (double)r.Top)
				{
					return (double)r.Top - point.Y;
				}
				return point.Y - (double)r.Bottom;
			}
			else if (point.Y >= (double)r.Top && point.Y <= (double)r.Bottom)
			{
				if (point.X < (double)r.Left)
				{
					return (double)r.Left - point.X;
				}
				return point.X - (double)r.Right;
			}
			else if (point.X < (double)r.Left)
			{
				if (point.Y < (double)r.Top)
				{
					return Vector2D.Distance(point, r.TopLeftDouble());
				}
				return Vector2D.Distance(point, r.BottomLeftDouble());
			}
			else
			{
				if (point.Y < (double)r.Top)
				{
					return Vector2D.Distance(point, r.TopRightDouble());
				}
				return Vector2D.Distance(point, r.BottomRightDouble());
			}
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0040D83C File Offset: 0x0040BA3C
		public static Vector2 ClosestPointInRect(this Rectangle r, Vector2 point)
		{
			Vector2 vector = point;
			if (vector.X < (float)r.Left)
			{
				vector.X = (float)r.Left;
			}
			if (vector.X > (float)r.Right)
			{
				vector.X = (float)r.Right;
			}
			if (vector.Y < (float)r.Top)
			{
				vector.Y = (float)r.Top;
			}
			if (vector.Y > (float)r.Bottom)
			{
				vector.Y = (float)r.Bottom;
			}
			return vector;
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0040D8C8 File Offset: 0x0040BAC8
		public static Rectangle Modified(this Rectangle r, int x, int y, int w, int h)
		{
			return new Rectangle(r.X + x, r.Y + y, r.Width + w, r.Height + h);
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0040D8F0 File Offset: 0x0040BAF0
		public static bool IntersectsConeFastInaccurate(this Rectangle targetRect, Vector2 coneCenter, float coneLength, float coneRotation, float maximumAngle)
		{
			Vector2 point = coneCenter + coneRotation.ToRotationVector2() * coneLength;
			Vector2 spinningpoint = targetRect.ClosestPointInRect(point) - coneCenter;
			float num = spinningpoint.RotatedBy((double)(-(double)coneRotation), default(Vector2)).ToRotation();
			return num >= -maximumAngle && num <= maximumAngle && spinningpoint.Length() < coneLength;
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0040D950 File Offset: 0x0040BB50
		public static bool IntersectsConeSlowMoreAccurate(this Rectangle targetRect, Vector2 coneCenter, float coneLength, float coneRotation, float maximumAngle)
		{
			Vector2 point = coneCenter + coneRotation.ToRotationVector2() * coneLength;
			return Utils.DoesFitInCone(targetRect.ClosestPointInRect(point), coneCenter, coneLength, coneRotation, maximumAngle) || Utils.DoesFitInCone(targetRect.TopLeft(), coneCenter, coneLength, coneRotation, maximumAngle) || Utils.DoesFitInCone(targetRect.TopRight(), coneCenter, coneLength, coneRotation, maximumAngle) || Utils.DoesFitInCone(targetRect.BottomLeft(), coneCenter, coneLength, coneRotation, maximumAngle) || Utils.DoesFitInCone(targetRect.BottomRight(), coneCenter, coneLength, coneRotation, maximumAngle);
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0040D9D8 File Offset: 0x0040BBD8
		public static bool DoesFitInCone(Vector2 point, Vector2 coneCenter, float coneLength, float coneRotation, float maximumAngle)
		{
			Vector2 spinningpoint = point - coneCenter;
			float num = spinningpoint.RotatedBy((double)(-(double)coneRotation), default(Vector2)).ToRotation();
			return num >= -maximumAngle && num <= maximumAngle && spinningpoint.Length() < coneLength;
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0040DA1C File Offset: 0x0040BC1C
		public static float ToRotation(this Vector2 v)
		{
			return (float)Math.Atan2((double)v.Y, (double)v.X);
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0040DA32 File Offset: 0x0040BC32
		public static double ToRotation(this Vector2D v)
		{
			return Math.Atan2(v.Y, v.X);
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x0040DA45 File Offset: 0x0040BC45
		public static Vector2 ToRotationVector2(this float f)
		{
			return new Vector2((float)Math.Cos((double)f), (float)Math.Sin((double)f));
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x0040DA5C File Offset: 0x0040BC5C
		public static Vector2D ToRotationVector2D(this double f)
		{
			return new Vector2D(Math.Cos(f), Math.Sin(f));
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0040DA70 File Offset: 0x0040BC70
		public static Vector2 RotatedBy(this Vector2 spinningpoint, double radians, Vector2 center = default(Vector2))
		{
			float num = (float)Math.Cos(radians);
			float num2 = (float)Math.Sin(radians);
			Vector2 vector = spinningpoint - center;
			Vector2 result = center;
			result.X += vector.X * num - vector.Y * num2;
			result.Y += vector.X * num2 + vector.Y * num;
			return result;
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0040DAD0 File Offset: 0x0040BCD0
		public static Vector2D RotatedBy(this Vector2D spinningpoint, double radians, Vector2D center = default(Vector2D))
		{
			double num = Math.Cos(radians);
			double num2 = Math.Sin(radians);
			Vector2D vector2D = spinningpoint - center;
			Vector2D result = center;
			result.X += vector2D.X * num - vector2D.Y * num2;
			result.Y += vector2D.X * num2 + vector2D.Y * num;
			return result;
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0040DB30 File Offset: 0x0040BD30
		public static Vector2 RotatedByRandom(this Vector2 spinninpoint, double maxRadians)
		{
			return spinninpoint.RotatedBy(Main.rand.NextDouble() * maxRadians - Main.rand.NextDouble() * maxRadians, default(Vector2));
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0040DB65 File Offset: 0x0040BD65
		public static Vector2 Floor(this Vector2 vec)
		{
			vec.X = (float)((int)vec.X);
			vec.Y = (float)((int)vec.Y);
			return vec;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0040DB86 File Offset: 0x0040BD86
		public static bool HasNaNs(this Vector2 vec)
		{
			return float.IsNaN(vec.X) || float.IsNaN(vec.Y);
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0040DBA2 File Offset: 0x0040BDA2
		public static bool Between(this Vector2 vec, Vector2 minimum, Vector2 maximum)
		{
			return vec.X >= minimum.X && vec.X <= maximum.X && vec.Y >= minimum.Y && vec.Y <= maximum.Y;
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0040DBE1 File Offset: 0x0040BDE1
		public static Vector2 ScaledBy(this Vector2 vec, Vector2 other)
		{
			return Vector2.Multiply(vec, other);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0040DBEA File Offset: 0x0040BDEA
		public static Vector2 ScaledBy(this Vector2 vec, float scaleX, float scaleY)
		{
			return Vector2.Multiply(vec, new Vector2(scaleX, scaleY));
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0040DBF9 File Offset: 0x0040BDF9
		public static Vector2 ToVector2(this Point p)
		{
			return new Vector2((float)p.X, (float)p.Y);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0040DC0E File Offset: 0x0040BE0E
		public static Vector2 ToVector2(this Point16 p)
		{
			return new Vector2((float)p.X, (float)p.Y);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0040DC23 File Offset: 0x0040BE23
		public static Vector3 ToVector3(this Vector2 v)
		{
			return new Vector3(v.X, v.Y, 0f);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0040DC3B File Offset: 0x0040BE3B
		public static Vector2D ToVector2D(this Point p)
		{
			return new Vector2D((double)p.X, (double)p.Y);
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0040DC50 File Offset: 0x0040BE50
		public static Vector2D ToVector2D(this Point16 p)
		{
			return new Vector2D((double)p.X, (double)p.Y);
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0040DC65 File Offset: 0x0040BE65
		public static Vector2 ToWorldCoordinates(this Point p, float autoAddX = 8f, float autoAddY = 8f)
		{
			return p.ToVector2() * 16f + new Vector2(autoAddX, autoAddY);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0040DC83 File Offset: 0x0040BE83
		public static Vector2 ToWorldCoordinates(this Point16 p, float autoAddX = 8f, float autoAddY = 8f)
		{
			return p.ToVector2() * 16f + new Vector2(autoAddX, autoAddY);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0040DCA4 File Offset: 0x0040BEA4
		public static Vector2 MoveTowards(this Vector2 currentPosition, Vector2 targetPosition, float maxAmountAllowedToMove)
		{
			Vector2 v = targetPosition - currentPosition;
			if (v.Length() < maxAmountAllowedToMove)
			{
				return targetPosition;
			}
			return currentPosition + v.SafeNormalize(Vector2.Zero) * maxAmountAllowedToMove;
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0040DCDC File Offset: 0x0040BEDC
		public static float MoveTowards(float original, float target, float amount)
		{
			if (original == target)
			{
				return target;
			}
			int num = Math.Sign(target - original);
			float num2 = original + amount * (float)num;
			if (Math.Sign(target - num2) != num)
			{
				return target;
			}
			return num2;
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0040DD0D File Offset: 0x0040BF0D
		public static Point16 ToTileCoordinates16(this Vector2 vec)
		{
			return new Point16((int)vec.X >> 4, (int)vec.Y >> 4);
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0040DD26 File Offset: 0x0040BF26
		public static Point16 ToTileCoordinates16(this Vector2D vec)
		{
			return new Point16((int)vec.X >> 4, (int)vec.Y >> 4);
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0040DD3F File Offset: 0x0040BF3F
		public static Point ToTileCoordinates(this Vector2 vec)
		{
			return new Point((int)vec.X >> 4, (int)vec.Y >> 4);
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0040DD58 File Offset: 0x0040BF58
		public static Point ToTileCoordinates(this Vector2D vec)
		{
			return new Point((int)vec.X >> 4, (int)vec.Y >> 4);
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0040DD71 File Offset: 0x0040BF71
		public static Point ToPoint(this Vector2 v)
		{
			return new Point((int)v.X, (int)v.Y);
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0040DD86 File Offset: 0x0040BF86
		public static Point ToPoint(this Vector2D v)
		{
			return new Point((int)v.X, (int)v.Y);
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0040DD9B File Offset: 0x0040BF9B
		public static Vector2 ToVector2(this Vector2D v)
		{
			return new Vector2((float)v.X, (float)v.Y);
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0040DDB0 File Offset: 0x0040BFB0
		public static Vector2D ToVector2D(this Vector2 v)
		{
			return new Vector2D((double)v.X, (double)v.Y);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0040DDC5 File Offset: 0x0040BFC5
		public static Vector2 SafeNormalize(this Vector2 v, Vector2 defaultValue)
		{
			if (v == Vector2.Zero || v.HasNaNs())
			{
				return defaultValue;
			}
			return Vector2.Normalize(v);
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0040DDE4 File Offset: 0x0040BFE4
		public static Vector2D SafeNormalize(this Vector2D v, Vector2D defaultValue)
		{
			if (v == Vector2D.Zero)
			{
				return defaultValue;
			}
			return Vector2D.Normalize(v);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0040DDFB File Offset: 0x0040BFFB
		public static Point ClampedInWorld(this Point p, int fluff = 0)
		{
			return new Point(Utils.Clamp<int>(p.X, fluff, Main.maxTilesX - fluff - 1), Utils.Clamp<int>(p.Y, fluff, Main.maxTilesX - fluff - 1));
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0040DE2C File Offset: 0x0040C02C
		public static Vector2 ClosestPointOnLine(this Vector2 P, Vector2 A, Vector2 B)
		{
			Vector2 value = P - A;
			Vector2 vector = B - A;
			float num = vector.LengthSquared();
			float num2 = Vector2.Dot(value, vector) / num;
			if (num2 < 0f)
			{
				return A;
			}
			if (num2 > 1f)
			{
				return B;
			}
			return A + vector * num2;
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0040DE7C File Offset: 0x0040C07C
		public static Vector2D ClosestPointOnLine(this Vector2D P, Vector2D A, Vector2D B)
		{
			Vector2D vector2D = P - A;
			Vector2D vector2D2 = B - A;
			double num = vector2D2.LengthSquared();
			double num2 = Vector2D.Dot(vector2D, vector2D2) / num;
			if (num2 < 0.0)
			{
				return A;
			}
			if (num2 > 1.0)
			{
				return B;
			}
			return A + vector2D2 * num2;
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0040DED4 File Offset: 0x0040C0D4
		public static bool RectangleLineCollision(Vector2 rectTopLeft, Vector2 rectBottomRight, Vector2 lineStart, Vector2 lineEnd)
		{
			if (lineStart.Between(rectTopLeft, rectBottomRight) || lineEnd.Between(rectTopLeft, rectBottomRight))
			{
				return true;
			}
			Vector2 p = new Vector2(rectBottomRight.X, rectTopLeft.Y);
			Vector2 vector = new Vector2(rectTopLeft.X, rectBottomRight.Y);
			Vector2[] array = new Vector2[]
			{
				rectTopLeft.ClosestPointOnLine(lineStart, lineEnd),
				p.ClosestPointOnLine(lineStart, lineEnd),
				vector.ClosestPointOnLine(lineStart, lineEnd),
				rectBottomRight.ClosestPointOnLine(lineStart, lineEnd)
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (array[0].Between(rectTopLeft, vector))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0040DF84 File Offset: 0x0040C184
		public static Vector2 RotateRandom(this Vector2 spinninpoint, double maxRadians)
		{
			return spinninpoint.RotatedBy(Main.rand.NextDouble() * maxRadians - Main.rand.NextDouble() * maxRadians, default(Vector2));
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0040DFB9 File Offset: 0x0040C1B9
		public static float AngleTo(this Vector2 Origin, Vector2 Target)
		{
			return (float)Math.Atan2((double)(Target.Y - Origin.Y), (double)(Target.X - Origin.X));
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0040DFDD File Offset: 0x0040C1DD
		public static float AngleFrom(this Vector2 Origin, Vector2 Target)
		{
			return (float)Math.Atan2((double)(Origin.Y - Target.Y), (double)(Origin.X - Target.X));
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0040E004 File Offset: 0x0040C204
		public static Vector2 rotateTowards(Vector2 currentPosition, Vector2 currentVelocity, Vector2 targetPosition, float maxChange)
		{
			float scaleFactor = currentVelocity.Length();
			float targetAngle = currentPosition.AngleTo(targetPosition);
			return currentVelocity.ToRotation().AngleTowards(targetAngle, maxChange).ToRotationVector2() * scaleFactor;
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0040E039 File Offset: 0x0040C239
		public static float Distance(this Vector2 Origin, Vector2 Target)
		{
			return Vector2.Distance(Origin, Target);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0040E042 File Offset: 0x0040C242
		public static double Distance(this Vector2D Origin, Vector2D Target)
		{
			return Vector2D.Distance(Origin, Target);
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0040E04B File Offset: 0x0040C24B
		public static float DistanceSQ(this Vector2 Origin, Vector2 Target)
		{
			return Vector2.DistanceSquared(Origin, Target);
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0040E054 File Offset: 0x0040C254
		public static Vector2 DirectionTo(this Vector2 Origin, Vector2 Target)
		{
			return Vector2.Normalize(Target - Origin);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0040E062 File Offset: 0x0040C262
		public static Vector2 DirectionFrom(this Vector2 Origin, Vector2 Target)
		{
			return Vector2.Normalize(Origin - Target);
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0040E070 File Offset: 0x0040C270
		public static bool WithinRange(this Vector2 Origin, Vector2 Target, float MaxRange)
		{
			return Vector2.DistanceSquared(Origin, Target) <= MaxRange * MaxRange;
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0040E081 File Offset: 0x0040C281
		public static Vector2 XY(this Vector4 vec)
		{
			return new Vector2(vec.X, vec.Y);
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0040E094 File Offset: 0x0040C294
		public static Vector2 ZW(this Vector4 vec)
		{
			return new Vector2(vec.Z, vec.W);
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0040E0A7 File Offset: 0x0040C2A7
		public static Vector3 XZW(this Vector4 vec)
		{
			return new Vector3(vec.X, vec.Z, vec.W);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0040E0C0 File Offset: 0x0040C2C0
		public static Vector3 YZW(this Vector4 vec)
		{
			return new Vector3(vec.Y, vec.Z, vec.W);
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0040E0DC File Offset: 0x0040C2DC
		public static Color MultiplyRGB(this Color firstColor, Color secondColor)
		{
			return new Color((int)((byte)((float)(firstColor.R * secondColor.R) / 255f)), (int)((byte)((float)(firstColor.G * secondColor.G) / 255f)), (int)((byte)((float)(firstColor.B * secondColor.B) / 255f)));
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0040E134 File Offset: 0x0040C334
		public static Color MultiplyRGBA(this Color firstColor, Color secondColor)
		{
			return new Color((int)((byte)((float)(firstColor.R * secondColor.R) / 255f)), (int)((byte)((float)(firstColor.G * secondColor.G) / 255f)), (int)((byte)((float)(firstColor.B * secondColor.B) / 255f)), (int)((byte)((float)(firstColor.A * secondColor.A) / 255f)));
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0040E1A4 File Offset: 0x0040C3A4
		public static string Hex3(this Color color)
		{
			return (color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2")).ToLower();
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0040E1F8 File Offset: 0x0040C3F8
		public static string Hex4(this Color color)
		{
			return (color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2") + color.A.ToString("X2")).ToLower();
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0040E25F File Offset: 0x0040C45F
		public static int ToDirectionInt(this bool value)
		{
			if (!value)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0040E267 File Offset: 0x0040C467
		public static int ToInt(this bool value)
		{
			if (!value)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0040E26F File Offset: 0x0040C46F
		public static int ModulusPositive(this int myInteger, int modulusNumber)
		{
			return (myInteger % modulusNumber + modulusNumber) % modulusNumber;
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0040E278 File Offset: 0x0040C478
		public static float AngleLerp(this float curAngle, float targetAngle, float amount)
		{
			float angle;
			if (targetAngle < curAngle)
			{
				float num = targetAngle + 6.2831855f;
				angle = ((num - curAngle > curAngle - targetAngle) ? MathHelper.Lerp(curAngle, targetAngle, amount) : MathHelper.Lerp(curAngle, num, amount));
			}
			else
			{
				if (targetAngle <= curAngle)
				{
					return curAngle;
				}
				float num = targetAngle - 6.2831855f;
				angle = ((targetAngle - curAngle > curAngle - num) ? MathHelper.Lerp(curAngle, num, amount) : MathHelper.Lerp(curAngle, targetAngle, amount));
			}
			return MathHelper.WrapAngle(angle);
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0040E2E0 File Offset: 0x0040C4E0
		public static float AngleTowards(this float curAngle, float targetAngle, float maxChange)
		{
			curAngle = MathHelper.WrapAngle(curAngle);
			targetAngle = MathHelper.WrapAngle(targetAngle);
			if (curAngle < targetAngle)
			{
				if (targetAngle - curAngle > 3.1415927f)
				{
					curAngle += 6.2831855f;
				}
			}
			else if (curAngle - targetAngle > 3.1415927f)
			{
				curAngle -= 6.2831855f;
			}
			curAngle += MathHelper.Clamp(targetAngle - curAngle, -maxChange, maxChange);
			return MathHelper.WrapAngle(curAngle);
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0040E340 File Offset: 0x0040C540
		public static float RotateUntil(this float curAngle, float targetAngle, float changePerTick)
		{
			curAngle = MathHelper.WrapAngle(curAngle);
			targetAngle = MathHelper.WrapAngle(targetAngle);
			if (curAngle < targetAngle)
			{
				if (targetAngle - curAngle > 3.1415927f)
				{
					curAngle += 6.2831855f;
				}
			}
			else if (curAngle - targetAngle > 3.1415927f)
			{
				curAngle -= 6.2831855f;
			}
			curAngle += changePerTick;
			curAngle = MathHelper.WrapAngle(curAngle);
			if (curAngle > targetAngle)
			{
				curAngle = targetAngle;
			}
			return curAngle;
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0040E3A0 File Offset: 0x0040C5A0
		public static bool deepCompare(this int[] firstArray, int[] secondArray)
		{
			if (firstArray == null && secondArray == null)
			{
				return true;
			}
			if (firstArray == null || secondArray == null)
			{
				return false;
			}
			if (firstArray.Length != secondArray.Length)
			{
				return false;
			}
			for (int i = 0; i < firstArray.Length; i++)
			{
				if (firstArray[i] != secondArray[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0040E3E0 File Offset: 0x0040C5E0
		public static bool deepCompare(this Rectangle[,] firstArray, Rectangle[,] secondArray)
		{
			if (firstArray == null && secondArray == null)
			{
				return true;
			}
			if (firstArray == null || secondArray == null)
			{
				return false;
			}
			if (firstArray.Length != secondArray.Length)
			{
				return false;
			}
			if (firstArray.GetLength(0) != secondArray.GetLength(0))
			{
				return false;
			}
			if (firstArray.GetLength(1) != secondArray.GetLength(1))
			{
				return false;
			}
			for (int i = 0; i < firstArray.GetLength(0); i++)
			{
				for (int j = 0; j < firstArray.GetLength(1); j++)
				{
					if (firstArray[i, j] != secondArray[i, j])
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0040E470 File Offset: 0x0040C670
		public static List<int> GetTrueIndexes(this bool[] array)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i])
				{
					list.Add(i);
				}
			}
			return list;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0040E4A0 File Offset: 0x0040C6A0
		public static List<int> GetTrueIndexes(params bool[][] arrays)
		{
			List<int> list = new List<int>();
			foreach (bool[] array in arrays)
			{
				list.AddRange(array.GetTrueIndexes());
			}
			return list.Distinct<int>().ToList<int>();
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0040E4E0 File Offset: 0x0040C6E0
		public static int Count<T>(this T[] arr, T value)
		{
			int num = 0;
			foreach (T x in arr)
			{
				if (EqualityComparer<T>.Default.Equals(x, value))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0040E51A File Offset: 0x0040C71A
		public static bool PressingShift(this KeyboardState kb)
		{
			return kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0040E538 File Offset: 0x0040C738
		public static bool PressingControl(this KeyboardState kb)
		{
			return kb.IsKeyDown(Keys.LeftControl) || kb.IsKeyDown(Keys.RightControl);
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0040E556 File Offset: 0x0040C756
		public static bool PressingAlt(this KeyboardState kb)
		{
			return kb.IsKeyDown(Keys.LeftAlt) || kb.IsKeyDown(Keys.RightAlt);
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0040E574 File Offset: 0x0040C774
		public static R[] MapArray<T, R>(T[] array, Func<T, R> mapper)
		{
			R[] array2 = new R[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = mapper(array[i]);
			}
			return array2;
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0040E5AD File Offset: 0x0040C7AD
		public static bool PlotLine(Point16 p0, Point16 p1, Utils.TileActionAttempt plot, bool jump = true)
		{
			return Utils.PlotLine((int)p0.X, (int)p0.Y, (int)p1.X, (int)p1.Y, plot, jump);
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0040E5CE File Offset: 0x0040C7CE
		public static bool PlotLine(Point p0, Point p1, Utils.TileActionAttempt plot, bool jump = true)
		{
			return Utils.PlotLine(p0.X, p0.Y, p1.X, p1.Y, plot, jump);
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0040E5F0 File Offset: 0x0040C7F0
		private static bool PlotLine(int x0, int y0, int x1, int y1, Utils.TileActionAttempt plot, bool jump = true)
		{
			if (x0 == x1 && y0 == y1)
			{
				return plot(x0, y0);
			}
			bool flag = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
			if (flag)
			{
				Utils.Swap<int>(ref x0, ref y0);
				Utils.Swap<int>(ref x1, ref y1);
			}
			int num = Math.Abs(x1 - x0);
			int num2 = Math.Abs(y1 - y0);
			int num3 = num / 2;
			int num4 = y0;
			int num5 = (x0 < x1) ? 1 : -1;
			int num6 = (y0 < y1) ? 1 : -1;
			for (int num7 = x0; num7 != x1; num7 += num5)
			{
				if (flag)
				{
					if (!plot(num4, num7))
					{
						return false;
					}
				}
				else if (!plot(num7, num4))
				{
					return false;
				}
				num3 -= num2;
				if (num3 < 0)
				{
					num4 += num6;
					if (!jump)
					{
						if (flag)
						{
							if (!plot(num4, num7))
							{
								return false;
							}
						}
						else if (!plot(num7, num4))
						{
							return false;
						}
					}
					num3 += num;
				}
			}
			return true;
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0040E6CF File Offset: 0x0040C8CF
		public static int RandomNext(ref ulong seed, int bits)
		{
			seed = Utils.RandomNextSeed(seed);
			return (int)(seed >> 48 - bits);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0040E6E5 File Offset: 0x0040C8E5
		public static ulong RandomNextSeed(ulong seed)
		{
			return seed * 25214903917UL + 11UL & 281474976710655UL;
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0040E700 File Offset: 0x0040C900
		public static float RandomFloat(ref ulong seed)
		{
			return (float)Utils.RandomNext(ref seed, 24) / 16777216f;
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0040E714 File Offset: 0x0040C914
		public static int RandomInt(ref ulong seed, int max)
		{
			if ((max & -max) == max)
			{
				return (int)((long)max * (long)Utils.RandomNext(ref seed, 31) >> 31);
			}
			int num;
			int num2;
			do
			{
				num = Utils.RandomNext(ref seed, 31);
				num2 = num % max;
			}
			while (num - num2 + (max - 1) < 0);
			return num2;
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0040E751 File Offset: 0x0040C951
		public static int RandomInt(ref ulong seed, int min, int max)
		{
			return Utils.RandomInt(ref seed, max - min) + min;
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0040E75E File Offset: 0x0040C95E
		public static bool PlotTileLine(Vector2 start, Vector2 end, float width, Utils.TileActionAttempt plot)
		{
			return Utils.PlotTileLine(start.ToVector2D(), end.ToVector2D(), (double)width, plot);
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0040E774 File Offset: 0x0040C974
		public static bool PlotTileLine(Vector2D start, Vector2D end, double width, Utils.TileActionAttempt plot)
		{
			double num = width / 2.0;
			Vector2D vector2D = end - start;
			Vector2D vector2D2 = vector2D / vector2D.Length();
			Vector2D vector2D3 = new Vector2D(-vector2D2.Y, vector2D2.X) * num;
			Point point = (start - vector2D3).ToTileCoordinates();
			Point point2 = (start + vector2D3).ToTileCoordinates();
			Point point3 = start.ToTileCoordinates();
			Point point4 = end.ToTileCoordinates();
			Point lineMinOffset = new Point(point.X - point3.X, point.Y - point3.Y);
			Point lineMaxOffset = new Point(point2.X - point3.X, point2.Y - point3.Y);
			return Utils.PlotLine(point3.X, point3.Y, point4.X, point4.Y, (int x, int y) => Utils.PlotLine(x + lineMinOffset.X, y + lineMinOffset.Y, x + lineMaxOffset.X, y + lineMaxOffset.Y, plot, false), true);
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0040E878 File Offset: 0x0040CA78
		public static bool PlotTileTale(Vector2D start, Vector2D end, double width, Utils.TileActionAttempt plot)
		{
			double halfWidth = width / 2.0;
			Vector2D vector2D = end - start;
			Vector2D vector2D2 = vector2D / vector2D.Length();
			Vector2D perpOffset = new Vector2D(-vector2D2.Y, vector2D2.X);
			Point pointStart = start.ToTileCoordinates();
			Point point = end.ToTileCoordinates();
			int length = 0;
			Utils.PlotLine(pointStart.X, pointStart.Y, point.X, point.Y, delegate(int <p0>, int <p1>)
			{
				int length2 = length;
				length = length2 + 1;
				return true;
			}, true);
			int length3 = length;
			length = length3 - 1;
			int curLength = 0;
			return Utils.PlotLine(pointStart.X, pointStart.Y, point.X, point.Y, delegate(int x, int y)
			{
				int curLength;
				double num = 1.0 - (double)curLength / (double)length;
				curLength = curLength;
				curLength++;
				Point point2 = (start - perpOffset * halfWidth * num).ToTileCoordinates();
				Point point3 = (start + perpOffset * halfWidth * num).ToTileCoordinates();
				Point point4 = new Point(point2.X - pointStart.X, point2.Y - pointStart.Y);
				Point point5 = new Point(point3.X - pointStart.X, point3.Y - pointStart.Y);
				return Utils.PlotLine(x + point4.X, y + point4.Y, x + point5.X, y + point5.Y, plot, false);
			}, true);
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0040E980 File Offset: 0x0040CB80
		public static void FloodFillTile(Point point, float maxDist, Utils.TileActionAttempt plot)
		{
			if (!WorldGen.InWorld(point, 0))
			{
				return;
			}
			List<Point> floodFillQueue = Utils._floodFillQueue1;
			List<Point> floodFillQueue2 = Utils._floodFillQueue2;
			BitSet2D floodFillBitset = Utils._floodFillBitset;
			floodFillBitset.Reset(point, (int)Math.Ceiling((double)maxDist) + 1);
			floodFillQueue2.Add(point);
			floodFillBitset.Add(point);
			while (floodFillQueue2.Count > 0)
			{
				Utils.Swap<List<Point>>(ref floodFillQueue, ref floodFillQueue2);
				floodFillQueue2.Clear();
				foreach (Point point2 in floodFillQueue)
				{
					if (plot(point2.X, point2.Y))
					{
						Point point3 = new Point(point2.X - 1, point2.Y);
						if (WorldGen.InWorld(point3, 0) && floodFillBitset.Add(point3))
						{
							floodFillQueue2.Add(point3);
						}
						point3 = new Point(point2.X + 1, point2.Y);
						if (WorldGen.InWorld(point3, 0) && floodFillBitset.Add(point3))
						{
							floodFillQueue2.Add(point3);
						}
						point3 = new Point(point2.X, point2.Y - 1);
						if (WorldGen.InWorld(point3, 0) && floodFillBitset.Add(point3))
						{
							floodFillQueue2.Add(point3);
						}
						point3 = new Point(point2.X, point2.Y + 1);
						if (WorldGen.InWorld(point3, 0) && floodFillBitset.Add(point3))
						{
							floodFillQueue2.Add(point3);
						}
					}
				}
			}
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0040EB1C File Offset: 0x0040CD1C
		public static int RandomConsecutive(double random, int odds)
		{
			return (int)Math.Log(1.0 - random, 1.0 / (double)odds);
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0040D026 File Offset: 0x0040B226
		public static Vector2 RandomVector2(UnifiedRandom random, float min, float max)
		{
			return new Vector2((max - min) * (float)random.NextDouble() + min, (max - min) * (float)random.NextDouble() + min);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0040D0D2 File Offset: 0x0040B2D2
		public static Vector2D RandomVector2D(UnifiedRandom random, double min, double max)
		{
			return new Vector2D((max - min) * random.NextDouble() + min, (max - min) * random.NextDouble() + min);
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0040EB3B File Offset: 0x0040CD3B
		public static bool IndexInRange<T>(this T[] t, int index)
		{
			return index >= 0 && index < t.Length;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0040EB49 File Offset: 0x0040CD49
		public static bool IndexInRange<T>(this List<T> t, int index)
		{
			return index >= 0 && index < t.Count;
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0040EB5A File Offset: 0x0040CD5A
		public static T SelectRandom<T>(UnifiedRandom random, params T[] choices)
		{
			return choices[random.Next(choices.Length)];
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0040EB6C File Offset: 0x0040CD6C
		public static void DrawBorderStringFourWay(SpriteBatch sb, DynamicSpriteFont font, string text, float x, float y, Color textColor, Color borderColor, Vector2 origin, float scale = 1f)
		{
			Color color = borderColor;
			Vector2 zero = Vector2.Zero;
			int i = 0;
			while (i < 5)
			{
				switch (i)
				{
				case 0:
					zero.X = x - 2f;
					zero.Y = y;
					break;
				case 1:
					zero.X = x + 2f;
					zero.Y = y;
					break;
				case 2:
					zero.X = x;
					zero.Y = y - 2f;
					break;
				case 3:
					zero.X = x;
					zero.Y = y + 2f;
					break;
				case 4:
					goto IL_90;
				default:
					goto IL_90;
				}
				IL_A4:
				DynamicSpriteFontExtensionMethods.DrawString(sb, font, text, zero, color, 0f, origin, scale, SpriteEffects.None, 0f, null, null);
				i++;
				continue;
				IL_90:
				zero.X = x;
				zero.Y = y;
				color = textColor;
				goto IL_A4;
			}
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0040EC44 File Offset: 0x0040CE44
		public static Vector2 DrawBorderString(SpriteBatch sb, string text, Vector2 pos, Color color, float scale = 1f, float anchorx = 0f, float anchory = 0f, int maxCharactersDisplayed = -1)
		{
			if (maxCharactersDisplayed != -1)
			{
				text = Utils.TrimUserString(text, maxCharactersDisplayed);
			}
			DynamicSpriteFont value = FontAssets.MouseText.Value;
			Vector2 vector = value.MeasureString(text);
			ChatManager.DrawColorCodedStringWithShadow(sb, value, text, pos, color, 0f, new Vector2(anchorx, anchory) * vector, new Vector2(scale), -1f, 1.5f);
			return vector * scale;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0040ECAC File Offset: 0x0040CEAC
		public static Vector2 DrawBorderStringBig(SpriteBatch spriteBatch, string text, Vector2 pos, Color color, float scale = 1f, float anchorx = 0f, float anchory = 0f, int maxCharactersDisplayed = -1)
		{
			if (maxCharactersDisplayed != -1 && text.Length > maxCharactersDisplayed)
			{
				text.Substring(0, maxCharactersDisplayed);
			}
			DynamicSpriteFont value = FontAssets.DeathText.Value;
			for (int i = -1; i < 2; i++)
			{
				for (int j = -1; j < 2; j++)
				{
					DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, value, text, pos + new Vector2((float)i, (float)j), Color.Black, 0f, new Vector2(anchorx, anchory) * value.MeasureString(text), scale, SpriteEffects.None, 0f, null, null);
				}
			}
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, value, text, pos, color, 0f, new Vector2(anchorx, anchory) * value.MeasureString(text), scale, SpriteEffects.None, 0f, null, null);
			return value.MeasureString(text) * scale;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0040ED71 File Offset: 0x0040CF71
		public static void DrawInvBG(SpriteBatch sb, Rectangle R, Color c = default(Color))
		{
			Utils.DrawInvBG(sb, R.X, R.Y, R.Width, R.Height, c);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0040ED92 File Offset: 0x0040CF92
		public static void DrawInvBG(SpriteBatch sb, float x, float y, float w, float h, Color c = default(Color))
		{
			Utils.DrawInvBG(sb, (int)x, (int)y, (int)w, (int)h, c);
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0040EDA8 File Offset: 0x0040CFA8
		public static void DrawInvBG(SpriteBatch sb, int x, int y, int w, int h, Color c = default(Color))
		{
			if (c == default(Color))
			{
				c = new Color(63, 65, 151, 255) * 0.785f;
			}
			Texture2D value = TextureAssets.InventoryBack13.Value;
			if (w < 20)
			{
				w = 20;
			}
			if (h < 20)
			{
				h = 20;
			}
			sb.Draw(value, new Rectangle(x, y, 10, 10), new Rectangle?(new Rectangle(0, 0, 10, 10)), c);
			sb.Draw(value, new Rectangle(x + 10, y, w - 20, 10), new Rectangle?(new Rectangle(10, 0, 10, 10)), c);
			sb.Draw(value, new Rectangle(x + w - 10, y, 10, 10), new Rectangle?(new Rectangle(value.Width - 10, 0, 10, 10)), c);
			sb.Draw(value, new Rectangle(x, y + 10, 10, h - 20), new Rectangle?(new Rectangle(0, 10, 10, 10)), c);
			sb.Draw(value, new Rectangle(x + 10, y + 10, w - 20, h - 20), new Rectangle?(new Rectangle(10, 10, 10, 10)), c);
			sb.Draw(value, new Rectangle(x + w - 10, y + 10, 10, h - 20), new Rectangle?(new Rectangle(value.Width - 10, 10, 10, 10)), c);
			sb.Draw(value, new Rectangle(x, y + h - 10, 10, 10), new Rectangle?(new Rectangle(0, value.Height - 10, 10, 10)), c);
			sb.Draw(value, new Rectangle(x + 10, y + h - 10, w - 20, 10), new Rectangle?(new Rectangle(10, value.Height - 10, 10, 10)), c);
			sb.Draw(value, new Rectangle(x + w - 10, y + h - 10, 10, 10), new Rectangle?(new Rectangle(value.Width - 10, value.Height - 10, 10, 10)), c);
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0040EFC0 File Offset: 0x0040D1C0
		public static string ReadEmbeddedResource(string path)
		{
			string result;
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
			{
				using (StreamReader streamReader = new StreamReader(manifestResourceStream))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0040F01C File Offset: 0x0040D21C
		public static void DrawSplicedPanel(SpriteBatch sb, Texture2D texture, int x, int y, int w, int h, int leftEnd, int rightEnd, int topEnd, int bottomEnd, Color c)
		{
			if (w < leftEnd + rightEnd)
			{
				w = leftEnd + rightEnd;
			}
			if (h < topEnd + bottomEnd)
			{
				h = topEnd + bottomEnd;
			}
			sb.Draw(texture, new Rectangle(x, y, leftEnd, topEnd), new Rectangle?(new Rectangle(0, 0, leftEnd, topEnd)), c);
			sb.Draw(texture, new Rectangle(x + leftEnd, y, w - leftEnd - rightEnd, topEnd), new Rectangle?(new Rectangle(leftEnd, 0, texture.Width - leftEnd - rightEnd, topEnd)), c);
			sb.Draw(texture, new Rectangle(x + w - rightEnd, y, topEnd, rightEnd), new Rectangle?(new Rectangle(texture.Width - rightEnd, 0, rightEnd, topEnd)), c);
			sb.Draw(texture, new Rectangle(x, y + topEnd, leftEnd, h - topEnd - bottomEnd), new Rectangle?(new Rectangle(0, topEnd, leftEnd, texture.Height - topEnd - bottomEnd)), c);
			sb.Draw(texture, new Rectangle(x + leftEnd, y + topEnd, w - leftEnd - rightEnd, h - topEnd - bottomEnd), new Rectangle?(new Rectangle(leftEnd, topEnd, texture.Width - leftEnd - rightEnd, texture.Height - topEnd - bottomEnd)), c);
			sb.Draw(texture, new Rectangle(x + w - rightEnd, y + topEnd, rightEnd, h - topEnd - bottomEnd), new Rectangle?(new Rectangle(texture.Width - rightEnd, topEnd, rightEnd, texture.Height - topEnd - bottomEnd)), c);
			sb.Draw(texture, new Rectangle(x, y + h - bottomEnd, leftEnd, bottomEnd), new Rectangle?(new Rectangle(0, texture.Height - bottomEnd, leftEnd, bottomEnd)), c);
			sb.Draw(texture, new Rectangle(x + leftEnd, y + h - bottomEnd, w - leftEnd - rightEnd, bottomEnd), new Rectangle?(new Rectangle(leftEnd, texture.Height - bottomEnd, texture.Width - leftEnd - rightEnd, bottomEnd)), c);
			sb.Draw(texture, new Rectangle(x + w - rightEnd, y + h - bottomEnd, rightEnd, bottomEnd), new Rectangle?(new Rectangle(texture.Width - rightEnd, texture.Height - bottomEnd, rightEnd, bottomEnd)), c);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0040F259 File Offset: 0x0040D459
		public static void DrawSettingsPanel(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
		{
			Utils.DrawPanel(TextureAssets.SettingsPanel.Value, 2, 0, spriteBatch, position, width, color);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0040F259 File Offset: 0x0040D459
		public static void DrawSettings2Panel(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
		{
			Utils.DrawPanel(TextureAssets.SettingsPanel.Value, 2, 0, spriteBatch, position, width, color);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0040F270 File Offset: 0x0040D470
		public static void DrawPanel(Texture2D texture, int edgeWidth, int edgeShove, SpriteBatch spriteBatch, Vector2 position, float width, Color color)
		{
			spriteBatch.Draw(texture, position, new Rectangle?(new Rectangle(0, 0, edgeWidth, texture.Height)), color);
			spriteBatch.Draw(texture, new Vector2(position.X + (float)edgeWidth, position.Y), new Rectangle?(new Rectangle(edgeWidth + edgeShove, 0, texture.Width - (edgeWidth + edgeShove) * 2, texture.Height)), color, 0f, Vector2.Zero, new Vector2((width - (float)(edgeWidth * 2)) / (float)(texture.Width - (edgeWidth + edgeShove) * 2), 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, new Vector2(position.X + width - (float)edgeWidth, position.Y), new Rectangle?(new Rectangle(texture.Width - edgeWidth, 0, edgeWidth, texture.Height)), color);
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0040F348 File Offset: 0x0040D548
		public static void DrawRectangle(SpriteBatch sb, Vector2 start, Vector2 end, Color colorStart, Color colorEnd, float width)
		{
			Utils.DrawLine(sb, start, new Vector2(start.X, end.Y), colorStart, colorEnd, width);
			Utils.DrawLine(sb, start, new Vector2(end.X, start.Y), colorStart, colorEnd, width);
			Utils.DrawLine(sb, end, new Vector2(start.X, end.Y), colorStart, colorEnd, width);
			Utils.DrawLine(sb, end, new Vector2(end.X, start.Y), colorStart, colorEnd, width);
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0040F3CC File Offset: 0x0040D5CC
		public static void DrawLaser(SpriteBatch sb, Texture2D tex, Vector2 start, Vector2 end, Vector2 scale, Utils.LaserLineFraming framing)
		{
			Vector2 vector = Vector2.Normalize(end - start);
			float num = (end - start).Length();
			float rotation = vector.ToRotation() - 1.5707964f;
			if (vector.HasNaNs())
			{
				return;
			}
			float num2;
			Rectangle rectangle;
			Vector2 vector2;
			Color color;
			framing(0, start, num, default(Rectangle), out num2, out rectangle, out vector2, out color);
			sb.Draw(tex, start, new Rectangle?(rectangle), color, rotation, rectangle.Size() / 2f, scale, SpriteEffects.None, 0f);
			num -= num2 * scale.Y;
			Vector2 vector3 = start + vector * ((float)rectangle.Height - vector2.Y) * scale.Y;
			if (num > 0f)
			{
				float num3 = 0f;
				while (num3 + 1f < num)
				{
					framing(1, vector3, num - num3, rectangle, out num2, out rectangle, out vector2, out color);
					if (num - num3 < (float)rectangle.Height)
					{
						num2 *= (num - num3) / (float)rectangle.Height;
						rectangle.Height = (int)(num - num3);
					}
					sb.Draw(tex, vector3, new Rectangle?(rectangle), color, rotation, vector2, scale, SpriteEffects.None, 0f);
					num3 += num2 * scale.Y;
					vector3 += vector * num2 * scale.Y;
				}
			}
			framing(2, vector3, num, default(Rectangle), out num2, out rectangle, out vector2, out color);
			sb.Draw(tex, vector3, new Rectangle?(rectangle), color, rotation, vector2, scale, SpriteEffects.None, 0f);
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0040F572 File Offset: 0x0040D772
		public static void DrawLine(SpriteBatch spriteBatch, Point start, Point end, Color color)
		{
			Utils.DrawLine(spriteBatch, new Vector2((float)(start.X << 4), (float)(start.Y << 4)), new Vector2((float)(end.X << 4), (float)(end.Y << 4)), color);
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0040F5AC File Offset: 0x0040D7AC
		public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
		{
			float num = Vector2.Distance(start, end);
			Vector2 vector = (end - start) / num;
			Vector2 value = start;
			Vector2 screenPosition = Main.screenPosition;
			float rotation = vector.ToRotation();
			for (float num2 = 0f; num2 <= num; num2 += 4f)
			{
				float num3 = num2 / num;
				spriteBatch.Draw(TextureAssets.BlackTile.Value, value - screenPosition, null, new Color(new Vector4(num3, num3, num3, 1f) * color.ToVector4()), rotation, Vector2.Zero, 0.25f, SpriteEffects.None, 0f);
				value = start + num2 * vector;
			}
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0040F660 File Offset: 0x0040D860
		public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color colorStart, Color colorEnd, float width)
		{
			float num = Vector2.Distance(start, end);
			float rotation = (end - start).ToRotation();
			int num2 = Math.Min(5, (int)num);
			for (int i = 0; i < num2; i++)
			{
				spriteBatch.Draw(TextureAssets.BlackTile.Value, Vector2.Lerp(start, end, (float)i / (float)num2) - Main.screenPosition, null, Color.Lerp(colorStart, colorEnd, ((float)i + 0.5f) / (float)num2), rotation, Vector2.Zero, new Vector2(num / (float)num2 / 16f, width / 16f), SpriteEffects.None, 0f);
			}
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0040F6FD File Offset: 0x0040D8FD
		public static void DrawRectForTilesInWorld(SpriteBatch spriteBatch, Rectangle rect, Color color)
		{
			Utils.DrawRectForTilesInWorld(spriteBatch, new Point(rect.X, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height), color);
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0040F736 File Offset: 0x0040D936
		public static void DrawRectForTilesInWorld(SpriteBatch spriteBatch, Point start, Point end, Color color)
		{
			Utils.DrawRect(spriteBatch, new Vector2((float)(start.X << 4), (float)(start.Y << 4)), new Vector2((float)((end.X << 4) - 4), (float)((end.Y << 4) - 4)), color);
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0040F771 File Offset: 0x0040D971
		public static void DrawRect(SpriteBatch spriteBatch, Rectangle rect, Color color)
		{
			Utils.DrawRect(spriteBatch, new Vector2((float)rect.X, (float)rect.Y), new Vector2((float)(rect.X + rect.Width), (float)(rect.Y + rect.Height)), color);
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0040F7B0 File Offset: 0x0040D9B0
		public static void DrawRect(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
		{
			Utils.DrawLine(spriteBatch, start, new Vector2(start.X, end.Y), color);
			Utils.DrawLine(spriteBatch, start, new Vector2(end.X, start.Y), color);
			Utils.DrawLine(spriteBatch, end, new Vector2(start.X, end.Y), color);
			Utils.DrawLine(spriteBatch, end, new Vector2(end.X, start.Y), color);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0040F821 File Offset: 0x0040DA21
		public static void DrawRect(SpriteBatch spriteBatch, Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft, Color color)
		{
			Utils.DrawLine(spriteBatch, topLeft, topRight, color);
			Utils.DrawLine(spriteBatch, topRight, bottomRight, color);
			Utils.DrawLine(spriteBatch, bottomRight, bottomLeft, color);
			Utils.DrawLine(spriteBatch, bottomLeft, topLeft, color);
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0040F850 File Offset: 0x0040DA50
		public static void DrawSelectedCraftingBarIndicator(SpriteBatch spriteBatch, int craftX, int craftY)
		{
			int num = 16;
			Color ourFavoriteColor = Main.OurFavoriteColor;
			float num2 = 16f;
			for (float num3 = num2; num3 > 0f; num3 -= 1f)
			{
				float num4 = 1f - num3 / num2;
				spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(craftX - 16, craftY + num + (int)num3 * -1, 32, 2), ourFavoriteColor * (num4 * 0.6f));
			}
			spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(craftX - 16, craftY + num, 32, 4), ourFavoriteColor);
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0040F8E0 File Offset: 0x0040DAE0
		public static void DrawCursorSingle(SpriteBatch sb, Color color, float rot = float.NaN, float scale = 1f, Vector2 manualPosition = default(Vector2), int cursorSlot = 0, int specialMode = 0)
		{
			bool flag = false;
			bool flag2 = true;
			bool flag3 = true;
			Vector2 zero = Vector2.Zero;
			Vector2 value = new Vector2((float)Main.mouseX, (float)Main.mouseY);
			if (manualPosition != Vector2.Zero)
			{
				value = manualPosition;
			}
			if (float.IsNaN(rot))
			{
				rot = 0f;
			}
			else
			{
				flag = true;
				rot -= 2.3561945f;
			}
			if (cursorSlot == 4 || cursorSlot == 5)
			{
				flag2 = false;
				zero = new Vector2(8f);
				if (flag && specialMode == 0)
				{
					float num = rot;
					if (num < 0f)
					{
						num += 6.2831855f;
					}
					for (float num2 = 0f; num2 < 4f; num2 += 1f)
					{
						if (Math.Abs(num - 1.5707964f * num2) <= 0.7853982f)
						{
							rot = 1.5707964f * num2;
							break;
						}
					}
				}
			}
			Vector2 value2 = Vector2.One;
			if ((Main.ThickMouse && cursorSlot == 0) || cursorSlot == 1)
			{
				value2 = Main.DrawThickCursor(cursorSlot == 1);
			}
			if (flag2)
			{
				sb.Draw(TextureAssets.Cursors[cursorSlot].Value, value + value2 + Vector2.One, null, color.MultiplyRGB(new Color(0.2f, 0.2f, 0.2f, 0.5f)), rot, zero, scale * 1.1f, SpriteEffects.None, 0f);
			}
			if (flag3)
			{
				sb.Draw(TextureAssets.Cursors[cursorSlot].Value, value + value2, null, color, rot, zero, scale, SpriteEffects.None, 0f);
			}
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0040FA68 File Offset: 0x0040DC68
		public static bool TryOperateInLock(object _lock, Action action)
		{
			if (!Monitor.TryEnter(_lock))
			{
				return false;
			}
			bool result;
			try
			{
				action();
				result = true;
			}
			finally
			{
				Monitor.Exit(_lock);
			}
			return result;
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0040FAA4 File Offset: 0x0040DCA4
		public static bool ParseCommandPrefix(string text, string prefix, out string remainder)
		{
			remainder = "";
			if (!text.StartsWith(prefix, true, CultureInfo.InvariantCulture))
			{
				return false;
			}
			if (text.Length == prefix.Length)
			{
				return true;
			}
			if (text[prefix.Length] != ' ')
			{
				return false;
			}
			remainder = text.Substring(prefix.Length + 1);
			return true;
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0040FAFC File Offset: 0x0040DCFC
		public static string TrimUserString(string s, int length)
		{
			if (s.Length <= length)
			{
				return s;
			}
			if (length > 0 && char.IsHighSurrogate(s[length - 1]))
			{
				length--;
			}
			return s.Substring(0, length);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0040FB2A File Offset: 0x0040DD2A
		public static string TrimLastCharacter(string s)
		{
			return Utils.TrimUserString(s, s.Length - 1);
		}

		// Token: 0x04000ED6 RID: 3798
		public static readonly int MaxFloatInt = 16777216;

		// Token: 0x04000ED7 RID: 3799
		public const long MaxCoins = 9999999999L;

		// Token: 0x04000ED8 RID: 3800
		public static Dictionary<DynamicSpriteFont, float[]> charLengths = new Dictionary<DynamicSpriteFont, float[]>();

		// Token: 0x04000ED9 RID: 3801
		private static Regex _substitutionRegex = new Regex("{(\\?(?:!)?)?([a-zA-Z][\\w\\.]*)}", RegexOptions.Compiled);

		// Token: 0x04000EDA RID: 3802
		private const ulong RANDOM_MULTIPLIER = 25214903917UL;

		// Token: 0x04000EDB RID: 3803
		private const ulong RANDOM_ADD = 11UL;

		// Token: 0x04000EDC RID: 3804
		private const ulong RANDOM_MASK = 281474976710655UL;

		// Token: 0x04000EDD RID: 3805
		private static readonly List<Point> _floodFillQueue1 = new List<Point>(2500);

		// Token: 0x04000EDE RID: 3806
		private static readonly List<Point> _floodFillQueue2 = new List<Point>(2500);

		// Token: 0x04000EDF RID: 3807
		private static readonly BitSet2D _floodFillBitset = new BitSet2D();

		// Token: 0x02000641 RID: 1601
		// (Invoke) Token: 0x06003C99 RID: 15513
		public delegate bool TileActionAttempt(int x, int y);

		// Token: 0x02000642 RID: 1602
		// (Invoke) Token: 0x06003C9D RID: 15517
		public delegate void LaserLineFraming(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distanceCovered, out Rectangle frame, out Vector2 origin, out Color color);

		// Token: 0x02000643 RID: 1603
		// (Invoke) Token: 0x06003CA1 RID: 15521
		public delegate Color ColorLerpMethod(float percent);

		// Token: 0x02000644 RID: 1604
		public class RandomTeleportationAttemptSettings
		{
			// Token: 0x0400652D RID: 25901
			public Vector2 teleporteeSize;

			// Token: 0x0400652E RID: 25902
			public Vector2 teleporteeVelocity;

			// Token: 0x0400652F RID: 25903
			public float teleporteeGravityDirection;

			// Token: 0x04006530 RID: 25904
			public bool mostlySolidFloor;

			// Token: 0x04006531 RID: 25905
			public bool avoidLava;

			// Token: 0x04006532 RID: 25906
			public bool avoidAnyLiquid;

			// Token: 0x04006533 RID: 25907
			public bool avoidHurtTiles;

			// Token: 0x04006534 RID: 25908
			public bool avoidWalls;

			// Token: 0x04006535 RID: 25909
			public int attemptsBeforeGivingUp;

			// Token: 0x04006536 RID: 25910
			public int maximumFallDistanceFromOrignalPoint;

			// Token: 0x04006537 RID: 25911
			public bool strictRange;

			// Token: 0x04006538 RID: 25912
			public int[] tilesToAvoid;

			// Token: 0x04006539 RID: 25913
			public int tilesToAvoidRange;

			// Token: 0x0400653A RID: 25914
			public bool allowSolidTopFloor;

			// Token: 0x0400653B RID: 25915
			public Func<Tile, int, int, bool> specializedConditions;
		}

		// Token: 0x02000645 RID: 1605
		public struct ChaseResults
		{
			// Token: 0x0400653C RID: 25916
			public bool InterceptionHappens;

			// Token: 0x0400653D RID: 25917
			public Vector2 InterceptionPosition;

			// Token: 0x0400653E RID: 25918
			public float InterceptionTime;

			// Token: 0x0400653F RID: 25919
			public Vector2 ChaserVelocity;
		}
	}
}
