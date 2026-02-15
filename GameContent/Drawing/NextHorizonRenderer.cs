using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Skies;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x0200043A RID: 1082
	public class NextHorizonRenderer : IHorizonRenderer
	{
		// Token: 0x060030AB RID: 12459 RVA: 0x005BB9FC File Offset: 0x005B9BFC
		private void LoadTextures()
		{
			if (NextHorizonRenderer._sunriseTextures != null)
			{
				return;
			}
			NextHorizonRenderer._sunriseTextures = new Asset<Texture2D>[]
			{
				Main.Assets.Request<Texture2D>("Images/Misc/Sunrise/Sunrise_Blue", 1),
				Main.Assets.Request<Texture2D>("Images/Misc/Sunrise/Sunrise_Violet", 1),
				Main.Assets.Request<Texture2D>("Images/Misc/Sunrise/Sunrise_Yellow", 1),
				Main.Assets.Request<Texture2D>("Images/Misc/Sunrise/Sunrise_Aluminum", 1)
			};
			NextHorizonRenderer._sunsetTextures = new Asset<Texture2D>[]
			{
				Main.Assets.Request<Texture2D>("Images/Misc/Sunset/Sunset_Blue", 1),
				Main.Assets.Request<Texture2D>("Images/Misc/Sunset/Sunset_Dark", 1),
				Main.Assets.Request<Texture2D>("Images/Misc/Sunset/Sunset_Pink", 1),
				Main.Assets.Request<Texture2D>("Images/Misc/Sunset/Sunset_Red", 1)
			};
			NextHorizonRenderer._sunflareGradientTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/colorgradient", 1);
			NextHorizonRenderer._sunflareGradientDitherTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/colorgradientdither", 1);
			NextHorizonRenderer._sunflarePointBlurryTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/Lens/PointBlurry", 1);
			NextHorizonRenderer._sunflarePointSharpTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/Lens/PointSharp", 1);
			NextHorizonRenderer._sunflare1Texture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/flare1", 1);
			NextHorizonRenderer._sunflare2Texture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/flare2", 1);
			NextHorizonRenderer._bokehTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/Lens/Flare1", 1);
			NextHorizonRenderer._spectraTexture = Main.Assets.Request<Texture2D>("Images/Misc/Sunflare/Lens/Flare2", 1);
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x005BBB68 File Offset: 0x005B9D68
		private static Rectangle GetGradientRect()
		{
			int num = 400;
			int val = (int)((1.0 - Utils.GetLerpValue(40.0, Main.worldSurface, (double)(Main.screenPosition.Y / 16f), false)) * (double)num);
			int y = Math.Max(0, val) - num;
			return new Rectangle(0, y, Main.screenWidth, Main.screenHeight + num);
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x005BBBCC File Offset: 0x005B9DCC
		public void DrawHorizon()
		{
			if (!Main.ShouldDrawSurfaceBackground())
			{
				return;
			}
			this.LoadTextures();
			int sunriseSunsetTextureIndex = this.GetSunriseSunsetTextureIndex();
			Asset<Texture2D> asset = NextHorizonRenderer._sunriseTextures[sunriseSunsetTextureIndex % NextHorizonRenderer._sunriseTextures.Length];
			Asset<Texture2D> asset2 = NextHorizonRenderer._sunsetTextures[sunriseSunsetTextureIndex % NextHorizonRenderer._sunsetTextures.Length];
			float num;
			float num2;
			float num3;
			NextHorizonRenderer.GetVisibilities(out num, out num2, out num3);
			SpriteBatch spriteBatch = Main.spriteBatch;
			Rectangle gradientRect = NextHorizonRenderer.GetGradientRect();
			foreach (BackgroundGradientDrawer backgroundGradientDrawer in SunGradients.BackgroundDrawers)
			{
				backgroundGradientDrawer.Draw();
			}
			if (num2 != 0f)
			{
				spriteBatch.Draw(asset.Value, gradientRect, Color.White * num2);
			}
			if (num != 0f)
			{
				spriteBatch.Draw(asset2.Value, gradientRect, Color.White * num);
			}
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x005BA5EB File Offset: 0x005B87EB
		public float GetMoonStrength()
		{
			return Utils.Remap((float)Math.Abs(4 - Main.moonPhase), 0f, 4f, 0f, 1f, true);
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x005BBCB0 File Offset: 0x005B9EB0
		public void DrawSurfaceLayer(int layerIndex)
		{
			if (!Main.ShouldDrawSurfaceBackground())
			{
				return;
			}
			this.LoadTextures();
			SpriteBatch spriteBatch = Main.spriteBatch;
			Rectangle gradientRect = NextHorizonRenderer.GetGradientRect();
			float num;
			float num2;
			float num3;
			NextHorizonRenderer.GetVisibilities(out num, out num2, out num3);
			int sunriseSunsetTextureIndex = this.GetSunriseSunsetTextureIndex();
			List<Color[]> sunrises = SunGradients.Sunrises;
			Color[] array = sunrises[sunriseSunsetTextureIndex % sunrises.Count];
			List<Color[]> sunsets = SunGradients.Sunsets;
			Color[] array2 = sunsets[sunriseSunsetTextureIndex % sunsets.Count];
			Color transparent = Color.Transparent;
			this.BlendColor(ref transparent, array2[0], num);
			this.BlendColor(ref transparent, array[0], num2);
			switch (layerIndex)
			{
			}
			Asset<Texture2D> asset = NextHorizonRenderer._sunriseTextures[sunriseSunsetTextureIndex % NextHorizonRenderer._sunriseTextures.Length];
			Asset<Texture2D> asset2 = NextHorizonRenderer._sunsetTextures[sunriseSunsetTextureIndex % NextHorizonRenderer._sunsetTextures.Length];
			TileBatch tileBatch = Main.tileBatch;
			if (layerIndex == 3)
			{
				float scale = 0.6f;
				float scale2 = 1f;
				spriteBatch.Draw(NextHorizonRenderer._sunflareGradientTexture.Value, gradientRect, null, array[0] * scale2 * num2 * scale, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
				spriteBatch.Draw(NextHorizonRenderer._sunflareGradientTexture.Value, gradientRect, null, array2[0] * scale2 * num * scale, 0f, Vector2.Zero, SpriteEffects.None, 0f);
			}
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x005BBE53 File Offset: 0x005BA053
		private int GetSunriseSunsetTextureIndex()
		{
			return Main.HorizonPhase;
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x005BBE5C File Offset: 0x005BA05C
		public void ModifyHorizonLight(ref Color color)
		{
			if (!Main.ShouldDrawSurfaceBackground())
			{
				return;
			}
			float opacity;
			float opacity2;
			float num;
			NextHorizonRenderer.GetVisibilities(out opacity, out opacity2, out num);
			int sunriseSunsetTextureIndex = this.GetSunriseSunsetTextureIndex();
			List<Color[]> sunrises = SunGradients.Sunrises;
			Color[] gradient = sunrises[sunriseSunsetTextureIndex % sunrises.Count];
			List<Color[]> sunsets = SunGradients.Sunsets;
			Color[] gradient2 = sunsets[sunriseSunsetTextureIndex % sunsets.Count];
			this.BlendColor(ref color, gradient2, opacity);
			this.BlendColor(ref color, gradient, opacity2);
		}

		// Token: 0x060030B2 RID: 12466 RVA: 0x005BBECC File Offset: 0x005BA0CC
		public void DrawSun(Vector2 sunPosition)
		{
			float num;
			float num2;
			float num3;
			NextHorizonRenderer.GetVisibilities(out num, out num2, out num3);
			num *= num3;
			num2 *= num3;
			this.LoadTextures();
			Color value = new Color(255, 255, 255, 0);
			SpriteBatch spriteBatch = Main.spriteBatch;
			spriteBatch.Draw(NextHorizonRenderer._sunflare1Texture.Value, sunPosition, null, value * num * 0.75f, 0f, NextHorizonRenderer._sunflare1Texture.Size() / 2f, 3f, SpriteEffects.None, 0f);
			spriteBatch.Draw(NextHorizonRenderer._sunflare1Texture.Value, sunPosition, null, value * num * 0.35f, 0f, NextHorizonRenderer._sunflare1Texture.Size() / 2f, 2f, SpriteEffects.None, 0f);
			spriteBatch.Draw(NextHorizonRenderer._sunflare2Texture.Value, sunPosition, null, value * num2 * 0.7f * 0.5f, 0f, NextHorizonRenderer._sunflare2Texture.Size() / 2f, 2f, SpriteEffects.None, 0f);
			spriteBatch.Draw(NextHorizonRenderer._sunflare2Texture.Value, sunPosition, null, value * num2 * 0.3f * 0.5f, 0f, NextHorizonRenderer._sunflare2Texture.Size() / 2f, 1.5f, SpriteEffects.None, 0f);
			spriteBatch.Draw(NextHorizonRenderer._sunflare2Texture.Value, sunPosition, null, value * num2 * 0.2f * 0.5f, 0f, NextHorizonRenderer._sunflare2Texture.Size() / 2f, 1f, SpriteEffects.None, 0f);
		}

		// Token: 0x060030B3 RID: 12467 RVA: 0x005BC0BB File Offset: 0x005BA2BB
		private void BlendColor(ref Color color, Color[] gradient, float opacity)
		{
			this.BlendColor(ref color, gradient[gradient.Length / 2], opacity);
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x005BC0D0 File Offset: 0x005BA2D0
		private void BlendColor(ref Color color, Color colorToChoose, float opacity)
		{
			if (opacity <= 0f)
			{
				return;
			}
			Color value = new Color((int)Math.Max(color.R, colorToChoose.R), (int)Math.Max(color.G, colorToChoose.G), (int)Math.Max(color.B, colorToChoose.B), (int)Math.Max(color.A, colorToChoose.A));
			color = Color.Lerp(color, value, opacity);
		}

		// Token: 0x060030B5 RID: 12469 RVA: 0x005BC148 File Offset: 0x005BA348
		private static void GetVisibilities(out float sunsetVisibility, out float sunriseVisibility, out float celestialVisibility)
		{
			sunsetVisibility = 1f;
			sunriseVisibility = 1f;
			celestialVisibility = NextHorizonRenderer.GetCelestialEffectPower();
			float num = 1f;
			num *= Main.atmo;
			float num2 = 1f - Main.cloudAlpha;
			num *= num2 * num2;
			num *= 1f - Main.SmoothedMushroomLightInfluence;
			sunriseVisibility *= num;
			sunsetVisibility *= num;
			double time = Main.time;
			double num3 = 54000.0;
			if (Main.dayTime)
			{
				float fromMin = 3600f;
				int num4 = 2700;
				float fromMax = 10800f;
				float num5 = -10800f;
				float num6 = -3600f;
				sunriseVisibility *= Utils.Remap((float)time, 0f, (float)num4, 0f, 1f, true) * Utils.Remap((float)time, fromMin, fromMax, 1f, 0f, true);
				float num7 = Utils.Remap((float)time, (float)num3 + num5, (float)num3 + num6, 0f, 1f, true);
				float num8 = Utils.Remap((float)time, (float)num3 + num6, (float)num3, 1f, 0f, true);
				sunsetVisibility *= num7 * num8 * num8;
				if (Main.eclipse)
				{
					sunsetVisibility = 0f;
					sunriseVisibility = 0f;
				}
			}
			else
			{
				sunriseVisibility = 0f;
				sunsetVisibility = 0f;
			}
			if (Main.gameMenu && WorldGen.drunkWorldGen)
			{
				sunsetVisibility = (sunriseVisibility = 0f);
			}
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x005BC29E File Offset: 0x005BA49E
		public void CloudsStart()
		{
			this._drawData.Clear();
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x005BC2AC File Offset: 0x005BA4AC
		public void DrawCloud(float globalCloudAlpha, Cloud theCloud, int cloudPass, float cY)
		{
			Asset<Texture2D> asset = TextureAssets.Cloud[theCloud.type];
			Vector2 position = new Vector2(theCloud.position.X, cY) + asset.Size() / 2f;
			Color value = theCloud.cloudColor(Main.ColorOfTheSkies);
			this.OriginalColorsForCloud(theCloud, cloudPass, ref value);
			if (Main.atmo < 1f)
			{
				value *= Main.atmo;
			}
			this._drawData.Add(new DrawData(asset.Value, position, null, value * globalCloudAlpha, theCloud.rotation, asset.Size() / 2f, theCloud.scale, theCloud.spriteDir, 0f));
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x005BC36C File Offset: 0x005BA56C
		private void OriginalColorsForCloud(Cloud theCloud, int cloudPass, ref Color cloudColor)
		{
			if (cloudPass == 1)
			{
				float num = theCloud.scale * 0.8f;
				float num2 = (theCloud.scale + 1f) / 2f * 0.9f;
				cloudColor.R = (byte)((float)cloudColor.R * num);
				cloudColor.G = (byte)((float)cloudColor.G * num2);
			}
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x005BC3C4 File Offset: 0x005BA5C4
		private void BetterColorsForClouds(Cloud theCloud, int cloudPass, ref Vector2 cloudDrawPosition, ref Color cloudColor)
		{
			float num = 0f;
			if (cloudPass != 1)
			{
				if (cloudPass == 2)
				{
					num = 0.35f;
				}
			}
			else
			{
				num = 0.7f;
			}
			if (Main.keyState.IsKeyDown(Keys.LeftShift))
			{
				num = 0f;
			}
			if (num > 0f)
			{
				float visibility;
				float visibility2;
				float num2;
				NextHorizonRenderer.GetVisibilities(out visibility, out visibility2, out num2);
				int sunriseSunsetTextureIndex = this.GetSunriseSunsetTextureIndex();
				List<Color[]> sunrises = SunGradients.Sunrises;
				Color[] gradient = sunrises[sunriseSunsetTextureIndex % sunrises.Count];
				List<Color[]> sunsets = SunGradients.Sunsets;
				Color[] gradient2 = sunsets[sunriseSunsetTextureIndex % sunsets.Count];
				float normalizedScreenHeight = cloudDrawPosition.Y / (float)Main.screenHeight;
				float alpha = theCloud.Alpha;
				this.BlendColorAlongGradientBasedOnHeight(ref cloudColor, visibility, normalizedScreenHeight, gradient2, alpha);
				this.BlendColorAlongGradientBasedOnHeight(ref cloudColor, visibility2, normalizedScreenHeight, gradient, alpha);
			}
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x005BC48C File Offset: 0x005BA68C
		private void BlendColorAlongGradientBasedOnHeight(ref Color color, float visibility, float normalizedScreenHeight, Color[] gradient, float opacity)
		{
			float num = MathHelper.Clamp(normalizedScreenHeight * (float)gradient.Length, 0f, (float)(gradient.Length - 1));
			float num2 = num % 1f;
			int num3 = (int)Math.Floor((double)num);
			if (num2 == 0f || num3 == gradient.Length - 1)
			{
				this.BlendColor(ref color, gradient[num3] * opacity, visibility);
				return;
			}
			Color colorToChoose = Color.Lerp(gradient[num3], gradient[num3 + 1], num2) * opacity;
			this.BlendColor(ref color, colorToChoose, visibility);
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x005BC514 File Offset: 0x005BA714
		private static float GetCelestialEffectPower()
		{
			float num = 1800f;
			float num2 = 1800f;
			float toMax = 0f;
			if (Main.dayTime)
			{
				return Utils.Remap((float)Main.time, 0f, num * 2f, 0f, 1f, true) * Utils.Remap((float)Main.time, 54000f - num, 54000f, 1f, toMax, true);
			}
			return Utils.Remap((float)Main.time, 0f, num2 * 2f, 0f, 1f, true) * Utils.Remap((float)Main.time, 32400f - num2, 32400f, 1f, 0f, true);
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x005BC5C4 File Offset: 0x005BA7C4
		public void CloudsEnd()
		{
			if (this._drawData.Count == 0)
			{
				return;
			}
			Main.spriteBatch.End();
			SpriteDrawBuffer spriteBuffer = Main.spriteBuffer;
			foreach (DrawData drawData in this._drawData)
			{
				drawData.Draw(spriteBuffer);
			}
			MiscShaderData miscShaderData = GameShaders.Misc["HorizonClouds"];
			miscShaderData.UseSpriteTransformMatrix(new Matrix?(Main.LatestSurfaceBackgroundBeginner.transformMatrix));
			Color color;
			Color color2;
			HorizonHelper.GetCelestialBodyColors(out color, out color2);
			Color color3 = Main.dayTime ? color : color2;
			AuroraSky.ModifyTileColor(ref color3, 1f);
			miscShaderData.UseColor(color3);
			Vector2 celestialBodyPosition = NextHorizonRenderer.GetCelestialBodyPosition();
			float val;
			float val2;
			float num;
			NextHorizonRenderer.GetVisibilities(out val, out val2, out num);
			float num2 = Math.Max(val, val2) * num;
			if (!Main.dayTime)
			{
				num2 = Math.Max(num2, num * 0.15f);
			}
			num2 *= Utils.Clamp<float>(1f - Main.cloudBGAlpha, 0f, 1f);
			miscShaderData.UseShaderSpecificData(new Vector4(celestialBodyPosition.X, celestialBodyPosition.Y, num2, 0f));
			for (int i = 0; i < this._drawData.Count; i++)
			{
				miscShaderData.Apply(new DrawData?(this._drawData[i]));
				spriteBuffer.DrawSingle(i);
			}
			spriteBuffer.Unbind();
			Main.LatestSurfaceBackgroundBeginner.Begin(Main.spriteBatch);
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x005BC750 File Offset: 0x005BA950
		private static Vector2 GetCelestialBodyPosition()
		{
			return Main.LastCelestialBodyPosition * Main.ScreenSize.ToVector2();
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x005BC768 File Offset: 0x005BA968
		public void DrawLensFlare()
		{
			if (!Main.ShouldDrawSurfaceBackground() || !Main.ForegroundSunlightEffects)
			{
				return;
			}
			SpriteBatch spriteBatch = Main.spriteBatch;
			Vector2 celestialBodyPosition = NextHorizonRenderer.GetCelestialBodyPosition();
			Vector2 screenCenter = Main.ScreenSize.ToVector2() / 2f;
			float temporalIntensity;
			float temporalIntensity2;
			float celestialVisibility;
			NextHorizonRenderer.GetVisibilities(out temporalIntensity, out temporalIntensity2, out celestialVisibility);
			float num = this.AdjustIntensity(temporalIntensity2, celestialVisibility);
			float num2 = this.AdjustIntensity(temporalIntensity, celestialVisibility);
			if ((double)num <= 0.01 && (double)num2 <= 0.01)
			{
				return;
			}
			Main.LatestSurfaceBackgroundBeginner.Begin(spriteBatch, SpriteSortMode.Immediate);
			EffectPass effectPass = Main.pixelShader.CurrentTechnique.Passes[0];
			MiscShaderData miscShaderData = GameShaders.Misc["LensFlare"];
			miscShaderData.UseImage1(Main.HorizonHelper.SunVisibilityPixelTexture);
			miscShaderData.Apply(null);
			this.DrawSunriseFlare(spriteBatch, celestialBodyPosition, screenCenter, num);
			this.DrawSunsetFlare(spriteBatch, celestialBodyPosition, screenCenter, num2);
			spriteBatch.End();
			effectPass.Apply();
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x005BC858 File Offset: 0x005BAA58
		private float AdjustIntensity(float temporalIntensity, float celestialVisibility)
		{
			float num = temporalIntensity * celestialVisibility;
			num *= num * num;
			int sunScorchCounter = Main.SceneMetrics.PerspectivePlayer.sunScorchCounter;
			if (sunScorchCounter > 0)
			{
				float num2 = Utils.GetLerpValue(0f, 300f, (float)sunScorchCounter, true);
				num2 = 1f - num2;
				num = 1f - num2 * num2;
				num *= celestialVisibility;
				num *= 5f;
			}
			return num;
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x005BC8B8 File Offset: 0x005BAAB8
		private void DrawSunsetFlare(SpriteBatch spriteBatch, Vector2 sunPosition, Vector2 screenCenter, float intensity)
		{
			if (intensity <= 0.01f)
			{
				return;
			}
			this.LoadTextures();
			LensFlareElement lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._sunflarePointBlurryTexture;
			lensFlareElement.RepeatTimes = 3;
			lensFlareElement.DistanceStart = 0.33f;
			lensFlareElement.DistanceAlongIndex = 0.05f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.ScaleOverIndex = -0.04f;
			lensFlareElement.Color = new Color(43, 32, 0, 0) * 0.47058824f;
			lensFlareElement.IntensityOverIndex = -0.125f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._sunflarePointSharpTexture;
			lensFlareElement.RepeatTimes = 3;
			lensFlareElement.DistanceStart = 0.03f;
			lensFlareElement.DistanceAlongIndex = 0.05f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.ScaleOverIndex = 0.04f;
			lensFlareElement.Color = new Color(43, 32, 0, 0) * 0.47058824f;
			lensFlareElement.IntensityOverIndex = -0.125f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._sunflarePointBlurryTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.41f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.Color = new Color(255, 0, 65, 0) * 0.11764706f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._bokehTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.475f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.Color = new Color(255, 255, 255, 0) * 0.15686275f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._bokehTexture;
			lensFlareElement.RepeatTimes = 6;
			lensFlareElement.DistanceStart = 0.225f;
			lensFlareElement.DistanceAlongIndex = 0.04f;
			lensFlareElement.ScaleStart = 0.24f;
			lensFlareElement.ScaleOverIndex = -0.04f;
			lensFlareElement.Color = new Color(255, 255, 255, 0) * 0.078431375f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._sunflarePointBlurryTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.6f;
			lensFlareElement.ScaleStart = 1f;
			lensFlareElement.Color = new Color(255, 157, 0, 0) * 0.15686275f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._spectraTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.65f;
			lensFlareElement.ScaleStart = 0.4f;
			lensFlareElement.Rotation = 3.1415927f;
			lensFlareElement.Color = new Color(255, 255, 255, 0) * 0.039215688f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x005BCBEC File Offset: 0x005BADEC
		private void DrawSunriseFlare(SpriteBatch spriteBatch, Vector2 sunPosition, Vector2 screenCenter, float intensity)
		{
			if (intensity <= 0.01f)
			{
				return;
			}
			this.LoadTextures();
			LensFlareElement lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._sunflarePointSharpTexture;
			lensFlareElement.RepeatTimes = 3;
			lensFlareElement.DistanceStart = 0.33f;
			lensFlareElement.DistanceAlongIndex = 0.05f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.ScaleOverIndex = -0.04f;
			lensFlareElement.Color = new Color(0, 32, 43, 0) * 0.47058824f;
			lensFlareElement.IntensityOverIndex = -0.125f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._sunflarePointSharpTexture;
			lensFlareElement.RepeatTimes = 3;
			lensFlareElement.DistanceStart = 0.03f;
			lensFlareElement.DistanceAlongIndex = 0.05f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.ScaleOverIndex = 0.04f;
			lensFlareElement.Color = new Color(0, 32, 43, 0) * 0.47058824f;
			lensFlareElement.IntensityOverIndex = -0.125f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._sunflarePointBlurryTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.41f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.Color = new Color(65, 0, 255, 0) * 0.11764706f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._bokehTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.525f;
			lensFlareElement.Rotation = 0.01f;
			lensFlareElement.ScaleStart = 0.3f;
			lensFlareElement.Color = new Color(255, 255, 255, 0) * 0.15686275f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._bokehTexture;
			lensFlareElement.RepeatTimes = 6;
			lensFlareElement.DistanceStart = 0.225f;
			lensFlareElement.DistanceAlongIndex = 0.04f;
			lensFlareElement.ScaleStart = 0.24f;
			lensFlareElement.ScaleOverIndex = -0.04f;
			lensFlareElement.Color = new Color(255, 255, 255, 0) * 0.078431375f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._sunflarePointBlurryTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.6f;
			lensFlareElement.ScaleStart = 1f;
			lensFlareElement.Color = new Color(0, 157, 255, 0) * 0.15686275f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
			lensFlareElement = default(LensFlareElement);
			lensFlareElement.Texture = NextHorizonRenderer._spectraTexture;
			lensFlareElement.RepeatTimes = 1;
			lensFlareElement.DistanceStart = 0.65f;
			lensFlareElement.ScaleStart = 0.38f;
			lensFlareElement.Rotation = 3.1415927f;
			lensFlareElement.Color = new Color(255, 255, 255, 0) * 0.039215688f;
			lensFlareElement.Draw(spriteBatch, sunPosition, screenCenter, intensity);
		}

		// Token: 0x040056D9 RID: 22233
		private static Asset<Texture2D>[] _sunriseTextures;

		// Token: 0x040056DA RID: 22234
		private static Asset<Texture2D>[] _sunsetTextures;

		// Token: 0x040056DB RID: 22235
		private static Asset<Texture2D> _sunflareGradientTexture;

		// Token: 0x040056DC RID: 22236
		private static Asset<Texture2D> _sunflareGradientDitherTexture;

		// Token: 0x040056DD RID: 22237
		private static Asset<Texture2D> _sunflarePointBlurryTexture;

		// Token: 0x040056DE RID: 22238
		private static Asset<Texture2D> _sunflarePointSharpTexture;

		// Token: 0x040056DF RID: 22239
		private static Asset<Texture2D> _bokehTexture;

		// Token: 0x040056E0 RID: 22240
		private static Asset<Texture2D> _spectraTexture;

		// Token: 0x040056E1 RID: 22241
		private static Asset<Texture2D> _sunflare1Texture;

		// Token: 0x040056E2 RID: 22242
		private static Asset<Texture2D> _sunflare2Texture;

		// Token: 0x040056E3 RID: 22243
		private List<DrawData> _drawData = new List<DrawData>(200);
	}
}
