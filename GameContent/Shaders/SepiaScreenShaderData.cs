using System;
using Microsoft.Xna.Framework;
using Terraria.Enums;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders
{
	// Token: 0x02000294 RID: 660
	public class SepiaScreenShaderData : ScreenShaderData
	{
		// Token: 0x06002527 RID: 9511 RVA: 0x00552F74 File Offset: 0x00551174
		public SepiaScreenShaderData(string passName) : base(passName)
		{
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x00552F80 File Offset: 0x00551180
		public override void Update(GameTime gameTime)
		{
			float x = (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f;
			float num = 1f - Utils.SmoothStep((float)Main.worldSurface, (float)Main.worldSurface + 30f, x);
			Vector3 value;
			Vector3 vector = value = new Vector3(0.191f, -0.054f, -0.221f);
			Vector3 value2 = vector * 0.5f;
			Vector3 value3 = new Vector3(0f, -0.03f, 0.15f);
			Vector3 value4 = new Vector3(-0.11f, 0.01f, 0.16f);
			float cloudAlpha = Main.cloudAlpha;
			float num2;
			float num3;
			float num4;
			float num5;
			SepiaScreenShaderData.GetDaylightPowers(out num2, out num3, out num4, out num5);
			float num6 = num2 * 0.13f;
			if (Main.starGame)
			{
				float num7 = (float)Main.starGameMath(1.0) - 1f;
				num2 = num7;
				num3 = 1f - num7;
				num4 = num7;
				num5 = 1f - num7;
				num6 = num2 * 0.13f;
			}
			else if (!Main.dayTime)
			{
				if (Main.GetMoonPhase() == MoonPhase.Full)
				{
					value = new Vector3(-0.19f, 0.01f, 0.22f);
					num6 += 0.07f * num4;
				}
				if (Main.bloodMoon)
				{
					value = new Vector3(0.2f, -0.1f, -0.221f);
					num6 = 0.2f;
				}
			}
			num2 *= num;
			num3 *= num;
			num4 *= num;
			num5 *= num;
			base.UseOpacity(1f);
			base.UseIntensity(1.4f - num3 * 0.2f);
			float num8 = 0.3f - num6 * num2;
			num8 = MathHelper.Lerp(num8, 0.1f, cloudAlpha);
			float value5 = 0.2f;
			num8 = MathHelper.Lerp(num8, value5, 1f - num);
			base.UseProgress(num8);
			Vector3 vector2 = Vector3.Lerp(vector, value, num4);
			vector2 = Vector3.Lerp(vector2, value3, num5);
			vector2 = Vector3.Lerp(vector2, value4, cloudAlpha);
			vector2 = Vector3.Lerp(vector2, value2, 1f - num);
			base.UseColor(vector2);
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x00553184 File Offset: 0x00551384
		private static void GetDaylightPowers(out float nightlightPower, out float daylightPower, out float moonPower, out float dawnPower)
		{
			nightlightPower = 0f;
			daylightPower = 0f;
			moonPower = 0f;
			Vector2 dayTimeAsDirectionIn24HClock = Utils.GetDayTimeAsDirectionIn24HClock();
			Vector2 dayTimeAsDirectionIn24HClock2 = Utils.GetDayTimeAsDirectionIn24HClock(4.5f);
			Vector2 dayTimeAsDirectionIn24HClock3 = Utils.GetDayTimeAsDirectionIn24HClock(0f);
			float fromValue = Vector2.Dot(dayTimeAsDirectionIn24HClock, dayTimeAsDirectionIn24HClock3);
			float fromValue2 = Vector2.Dot(dayTimeAsDirectionIn24HClock, dayTimeAsDirectionIn24HClock2);
			nightlightPower = Utils.Remap(fromValue, -0.2f, 0.1f, 0f, 1f, true);
			daylightPower = Utils.Remap(fromValue, 0.1f, -1f, 0f, 1f, true);
			dawnPower = Utils.Remap(fromValue2, 0.66f, 1f, 0f, 1f, true);
			if (!Main.dayTime)
			{
				float num = (float)(Main.time / 32400.0) * 2f;
				if (num > 1f)
				{
					num = 2f - num;
				}
				moonPower = Utils.Remap(num, 0f, 0.25f, 0f, 1f, true);
			}
		}
	}
}
