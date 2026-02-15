using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	// Token: 0x02000447 RID: 1095
	public class AuroraSky : CustomSky
	{
		// Token: 0x060031BA RID: 12730 RVA: 0x00009E06 File Offset: 0x00008006
		public override void OnLoad()
		{
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x005E2180 File Offset: 0x005E0380
		public override void Update(GameTime gameTime)
		{
			if (FocusHelper.PauseSkies)
			{
				return;
			}
			if (this._isLeaving)
			{
				this._opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
				if (this._opacity < 0f)
				{
					this._isActive = false;
					this._opacity = 0f;
					return;
				}
			}
			else
			{
				this._opacity += (float)gameTime.ElapsedGameTime.TotalSeconds * 0.3f;
				if (this._opacity > 1f)
				{
					this._opacity = 1f;
				}
			}
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x005E2219 File Offset: 0x005E0419
		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth != 3.4028235E+38f)
			{
				return;
			}
			AuroraSky.DrawAuroraSky(this.vertexStrip, this._opacity, ref this._lastSkyColor);
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x005E223C File Offset: 0x005E043C
		private static void DrawAuroraSky(VertexStrip vertexStrip, float skyOpacity, ref Color lastSkyColor)
		{
			MiscShaderData miscShaderData = GameShaders.Misc["Aurora"];
			float num = Main.dayTime ? 54000f : 32400f;
			float fromValue = (float)Main.time;
			skyOpacity *= Utils.Remap(fromValue, 0f, 180f, 0f, 1f, true) * Utils.Remap(fromValue, num - 180f, num, 1f, 0f, true);
			if (skyOpacity <= 0.01f || Main.dayTime)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			int num2 = 1;
			float num3 = 1f;
			float num4 = 1f;
			bool flag5 = false;
			float saturation = 1f;
			switch (Main.GetMoonPhase())
			{
			case MoonPhase.Full:
				flag = true;
				num2 = 3;
				break;
			case MoonPhase.ThreeQuartersAtLeft:
				num2 = 2;
				flag5 = true;
				break;
			case MoonPhase.HalfAtLeft:
				flag2 = true;
				flag3 = true;
				num2 = 3;
				flag4 = true;
				num4 *= 0.5f;
				break;
			case MoonPhase.QuarterAtLeft:
				return;
			case MoonPhase.Empty:
				flag2 = true;
				num2 = 3;
				break;
			case MoonPhase.QuarterAtRight:
				num2 = 2;
				flag5 = true;
				saturation = 0.5f;
				break;
			case MoonPhase.HalfAtRight:
				return;
			case MoonPhase.ThreeQuartersAtRight:
				flag2 = true;
				flag3 = true;
				num2 = 3;
				flag4 = true;
				num4 *= 0.5f;
				saturation = 0.5f;
				break;
			}
			PlayerInput.SetZoom_Background();
			Main.spriteBatch.End();
			Vector2 vector = new Vector2(1920f, 1080f);
			float scale = (float)Main.ScreenSize.X / vector.X;
			miscShaderData.UseSpriteTransformMatrix(new Matrix?(Main.LatestSurfaceBackgroundBeginner.transformMatrix * Matrix.CreateScale(scale)));
			Vector2 lastCelestialBodyPosition = Main.LastCelestialBodyPosition;
			lastCelestialBodyPosition.Y *= vector.X / vector.Y / ((float)Main.ScreenSize.X / (float)Main.ScreenSize.Y);
			float num5 = Main.GlobalTimeWrappedHourly / 60f;
			for (int i = 0; i < num2; i++)
			{
				vertexStrip.Reset(0);
				int num6 = 140;
				float num7 = 2.5f;
				float num8 = 0f;
				float luminosity = 1f;
				Vector4 specificData = new Vector4(0f, 0f, 0f, 0f);
				if (i == 0)
				{
					specificData.Y = 0f;
				}
				if (i == 1)
				{
					specificData.Y = 0.7f;
				}
				if (i == 2)
				{
					specificData.Y = 0.8f;
				}
				if (flag4)
				{
					luminosity = 1f;
					specificData.X = 0.3f;
				}
				if (flag2)
				{
					num7 = 1f;
					num8 += 0.33f;
					if (i != 0)
					{
						specificData.Y = 0.4f + (float)i * 0.2f;
					}
					if (!flag3)
					{
						specificData.Z = 0.2f;
					}
				}
				if (flag && i != 0)
				{
					specificData.Y = 0.4f;
				}
				if (flag5 && i == 0)
				{
					specificData.Y = 0.3f;
				}
				if (flag5 && i == 1)
				{
					specificData.Y = 0.5f;
				}
				if (flag && i == 0)
				{
					specificData.Y = 0.5f;
				}
				if (flag2 && i == 0)
				{
					specificData.Y = 0.7f;
				}
				miscShaderData.UseShaderSpecificData(specificData);
				for (int j = num6; j >= 0; j--)
				{
					float num9 = (float)j / (float)num6;
					float num10 = num9;
					if (flag5 && i == 1)
					{
						num9 = Utils.Remap(num9, 0f, 1f, 50f / (float)num6, 90f / (float)num6, true);
					}
					float amount = num9;
					if (!flag)
					{
						amount = 1f - num9;
					}
					float num11 = MathHelper.Lerp(0.4f, 0.1f, num9);
					float num12 = 0.4f + num5;
					float num13 = 3f;
					float num14 = 0.5f + (float)Math.Cos((double)num9 * 3.141592653589793 * (double)num13 + (double)num12) * 0.4f * MathHelper.Lerp(1f, 0.3f, amount);
					float num15 = Utils.Remap(Math.Abs((float)Math.Sin((double)num9 * 3.141592653589793 * (double)num13 + (double)num12)), 0f, 0.98f, 0f, 1f, true);
					float num16 = MathHelper.Lerp(0.2f, 0.05f, amount) * num3;
					float num17 = 0.5f - 0.5f * (float)Math.Cos((double)(num9 * 6.2831855f));
					float num18 = num5;
					if (flag5)
					{
						float num19 = num5 * 0.16f;
						if (i == 1)
						{
							Utils.Remap(num9, 0f, 1f, 50f / (float)num6, 90f / (float)num6, true);
						}
						num11 += (1f - num9) * 0.05f;
						num16 += 0.05f;
						if (i == 1)
						{
							num11 = 0.5f + (float)Math.Cos((double)(num19 * 6.2831855f * 0.15f + num9 * 60f)) * 0.03f;
							float num20 = num9 + num19;
							num14 = 0.5f + (float)Math.Cos((double)num20 * 3.141592653589793 * 2.0) * 1.4f * MathHelper.Lerp(1f, 0.3f, num9);
							num14 += (float)Math.Sin((double)(num19 * 6.2831855f)) * MathHelper.Lerp(0.4f, 0.13f, num9);
							num11 -= (float)Math.Cos((double)(num19 * 6.2831855f * 3f + num9 * 5f)) * 0.06f;
							num16 += 0.15f;
							num14 = num10 * 1.1f;
							num15 = 1f - (float)Math.Sin((double)(num10 * 6.2831855f * 2f + 1.5707964f)) * 0.35f - 0.35f;
							num11 = (float)Math.Sin((double)(num10 * 6.2831855f * 2f + 1.5707964f)) * 0.0125f + 0.55f;
							num16 = 0.16f * num3 + 0.05f + (float)Math.Sin((double)(num10 * 6.2831855f * 2f)) * 0.025f;
							num16 += 0.2f;
						}
						if (i == 0)
						{
							float num21 = Utils.Remap(num9, 0f, 0.3f, 0f, 1f, true);
							num17 *= num21 * num21 * num21;
							num16 -= 0.1f;
							num16 += 0.8f * num9 * num9;
						}
					}
					if (flag && i == 0)
					{
						float num22 = num5 * 0.16f;
						num11 = 0.5f + (float)Math.Cos((double)(num22 * 6.2831855f * 0.15f + num9 * 60f)) * 0.03f;
						float num23 = num9 + num22;
						num14 = 0.5f + (float)Math.Cos((double)num23 * 3.141592653589793 * 2.0) * 1.4f * MathHelper.Lerp(1f, 0.3f, num9);
						num14 += (float)Math.Sin((double)(num22 * 6.2831855f)) * MathHelper.Lerp(0.4f, 0.13f, num9);
						num16 += (float)(Math.Sin((double)(num22 * 6.2831855f)) + 1.0) * MathHelper.Lerp(0.24f, 0.15f, num9) * num3;
						num11 -= (float)Math.Cos((double)(num22 * 6.2831855f * 3f + num9 * 5f)) * 0.06f;
						num14 = num10 * 1.1f;
						num11 = (float)Math.Sin((double)(num10 * 6.2831855f * 2f + 1.5707964f + num5 * 2f + 3.1415927f)) * 0.025f + 0.55f;
						num16 = 0.16f * num3 + 0.05f + (float)Math.Sin((double)(num10 * 6.2831855f * 2f + num5 * 2f)) * 0.02f;
						num15 = 1f - (float)Math.Sin((double)(num10 * 6.2831855f * 2f + 1.5707964f)) * 0.35f - 0.35f;
					}
					if (flag2)
					{
						float num24 = num5 * 0.16f;
						if (i == 0)
						{
							num11 = 0.5f + (float)Math.Cos((double)(num24 * 6.2831855f * 0.15f + num9 * 60f)) * 0.03f;
							float num25 = num9 + num24;
							num14 = 0.5f + (float)Math.Cos((double)num25 * 3.141592653589793 * 2.0) * 1.4f * MathHelper.Lerp(1f, 0.3f, num9);
							num14 += (float)Math.Sin((double)(num24 * 6.2831855f)) * MathHelper.Lerp(0.4f, 0.13f, num9);
							num11 -= (float)Math.Cos((double)(num24 * 6.2831855f * 3f + num9 * 5f)) * 0.06f;
							num16 += 0.15f;
							num14 = num10 * 1.1f;
							num15 = 1f - (float)Math.Sin((double)(num10 * 6.2831855f * 2f + 1.5707964f)) * 0.35f - 0.35f;
							num11 = (float)Math.Sin((double)(num10 * 6.2831855f * 2f + 1.5707964f)) * 0.025f + 0.55f;
							num16 = 0.16f * num3 + 0.05f + (float)Math.Sin((double)(num10 * 6.2831855f * 2f)) * 0.05f;
						}
						else if (i == 1 || i == 2)
						{
							num11 = MathHelper.Lerp(0.3f, 0.3f, num9);
							Math.Sin((double)(num5 * 6.2831855f));
							float value = (float)Math.Cos((double)(num5 * 6.2831855f));
							if (i == 1)
							{
								num16 += 0.5f * num9;
							}
							num11 -= (float)Math.Cos((double)(num9 * 6.2831855f + num5 * 2f)) * 0.07f;
							num15 = Utils.Remap(Math.Abs(value), 0f, 0.98f, 0f, 1f, true);
							num15 = 1f;
							num14 = num9;
							num18 += 0.35f;
							if (!flag3)
							{
								num18 -= 0.35f;
							}
							num17 *= 0.55f;
							if (i == 2)
							{
								Math.Sin((double)(num5 * 6.2831855f));
								Math.Cos((double)(num5 * 6.2831855f));
								num11 -= (float)Math.Cos((double)(num5 * 6.2831855f * 0.35f + num9 * 13.73f)) * 0.04f * (1f - num9) + 0.04f;
								num11 -= 0.03f;
							}
						}
						else if (i == 1)
						{
							num11 = MathHelper.Lerp(0.4f, 0.1f, num9);
							Math.Sin((double)(num5 * 6.2831855f));
							float value2 = (float)Math.Cos((double)(num5 * 6.2831855f));
							num11 -= (float)Math.Cos((double)(num9 * 6.2831855f + num5 * 2f)) * 0.07f;
							num15 = Utils.Remap(Math.Abs(value2), 0f, 0.98f, 0f, 1f, true);
							num15 = 1f;
							num14 = num9;
							num18 += 0.35f;
							num17 *= 0.55f;
						}
						else if (i == 2)
						{
							num11 = MathHelper.Lerp(0.1f, 0.4f, num9);
							Math.Sin((double)(num5 * 6.2831855f));
							float value3 = (float)Math.Cos((double)(num5 * 6.2831855f));
							num11 -= (float)Math.Cos((double)(num5 * 6.2831855f * 0.35f)) * 0.15f * (1f - num9);
							num18 += 0.35f;
							num15 = Utils.Remap(Math.Abs(value3), 0f, 0.98f, 0f, 1f, true);
							num15 = 1f;
							num14 = num9;
						}
					}
					if (flag3)
					{
						num18 = num5 + (float)i * 0.05f;
						num7 = 0.5f;
						num8 = 0.02f;
					}
					if (flag2 && !flag3)
					{
						luminosity = 1f;
						num8 = 0.45f;
					}
					if (flag && i != 0)
					{
						num17 = Math.Max(num17 * 2f, num9);
						if (num17 > 1f)
						{
							num17 = 1f;
						}
						num14 = MathHelper.Lerp(num14, lastCelestialBodyPosition.X, num9);
						num11 += 0.05f;
						num11 = MathHelper.Lerp(num11, lastCelestialBodyPosition.Y + 0.025f, num9);
						num17 *= 0.5f;
					}
					Vector2 vector2 = vector * new Vector2(num14, num11);
					Vector2 vector3 = vector * new Vector2(num14, num11 - num16);
					if (!flag)
					{
						float num26 = Main.GlobalTimeWrappedHourly * 0.1f;
						vector2 += ((num26 + 0.3f) * 6.2831855f).ToRotationVector2() * 2f;
						vector3 += ((num26 * 0.8f + 0.67f) * 6.2831855f).ToRotationVector2() * 2f;
						vector3.Y += (float)Math.Sin((double)((num26 + num9) * 6.2831855f * 3f)) * 15f - 15f;
						vector2.Y += (float)Math.Sin((double)((num26 + num9) * 6.2831855f * 0.5f)) * 1f;
						vector3.Y += (float)Math.Sin((double)((num26 + num9) * 6.2831855f * 0.5f)) * 1f;
						vector2.X += (float)Math.Sin((double)((num26 + num9) * 6.2831855f * 1f)) * 3f;
						vector3.X += (float)Math.Sin((double)((num26 + num9) * 6.2831855f * 0.75f)) * 3f;
					}
					Color color = Main.hslToRgb((float)((double)num18 + Math.Cos((double)(num9 * 6.2831855f * num7)) * 0.1) % 1f, saturation, 0.5f, byte.MaxValue);
					Color value4 = Main.hslToRgb((float)((double)num18 + Math.Cos((double)(num9 * 6.2831855f * num7)) * 0.1 + (double)num8) % 1f, saturation, luminosity, byte.MaxValue);
					if (i == 0 && j == 19)
					{
						lastSkyColor = color;
					}
					float num27 = num15 * skyOpacity * num17 * num4;
					if (flag)
					{
						float fromValue2 = (vector * new Vector2(num14, num11 - num16 * 0.25f)).Distance(vector * lastCelestialBodyPosition);
						num27 *= Utils.Remap(fromValue2, 29f, 60f, 0f, 1f, true);
						float num28 = 505f;
						float num29 = 1f - num9;
						num29 *= num29 * num29;
						if (i == 1)
						{
							vector2.X -= num28 * num29;
							vector3.X -= num28 * num29;
							num27 -= num9 * num9 * 0.36f;
						}
						if (i == 2)
						{
							vector2.X += num28 * num29;
							vector3.X += num28 * num29;
							num27 -= num9 * num9 * 0.36f;
						}
					}
					vertexStrip.AddVertexPair(vector2, vector3, num9, color * num27, value4 * num27);
				}
				miscShaderData.Apply(null);
				vertexStrip.PrepareIndices(true);
				vertexStrip.DrawTrail();
			}
			Main.LatestSurfaceBackgroundBeginner.Begin(Main.spriteBatch);
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x005E31DC File Offset: 0x005E13DC
		public static void ModifyTileColor(ref Color tileColor, float intensity)
		{
			AuroraSky auroraSky = SkyManager.Instance["Aurora"] as AuroraSky;
			if (auroraSky == null)
			{
				return;
			}
			float opacity = auroraSky._opacity;
			if (opacity <= 0f)
			{
				return;
			}
			MoonPhase moonPhase = Main.GetMoonPhase();
			if (moonPhase == MoonPhase.QuarterAtLeft)
			{
				return;
			}
			Color lastSkyColor = auroraSky._lastSkyColor;
			lastSkyColor.A = byte.MaxValue;
			tileColor = Color.Lerp(tileColor, lastSkyColor, opacity * intensity);
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x005E3245 File Offset: 0x005E1445
		public override void Activate(Vector2 position, params object[] args)
		{
			this._isActive = true;
			this._isLeaving = false;
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x005E3255 File Offset: 0x005E1455
		public override void Deactivate(params object[] args)
		{
			this._isLeaving = true;
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x005E325E File Offset: 0x005E145E
		public override void Reset()
		{
			this._opacity = 0f;
			this._isActive = false;
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x005E3272 File Offset: 0x005E1472
		public override bool IsActive()
		{
			return this._isActive;
		}

		// Token: 0x0400578F RID: 22415
		private UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x04005790 RID: 22416
		private bool _isActive;

		// Token: 0x04005791 RID: 22417
		private bool _isLeaving;

		// Token: 0x04005792 RID: 22418
		private float _opacity;

		// Token: 0x04005793 RID: 22419
		private VertexStrip vertexStrip = new VertexStrip();

		// Token: 0x04005794 RID: 22420
		private Color _lastSkyColor;

		// Token: 0x02000947 RID: 2375
		// (Invoke) Token: 0x06004851 RID: 18513
		private delegate void ScriptMethodSignature(VertexStrip vertexStrip, float skyOpacity, ref Color lastSkyColor);
	}
}
