using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x02000433 RID: 1075
	public class HorizonHelper
	{
		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06003083 RID: 12419 RVA: 0x005BA0F1 File Offset: 0x005B82F1
		public Texture2D SunVisibilityPixelTexture
		{
			get
			{
				return this._pixelTarget;
			}
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x005BA14C File Offset: 0x005B834C
		public void UpdateSunVisibility(RenderTarget2D bigTarget)
		{
			if (!Main.ForegroundSunlightEffects)
			{
				return;
			}
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
			if (this._tinyTarget == null || this._tinyTarget.IsContentLost)
			{
				this._tinyTarget = new RenderTarget2D(graphicsDevice, this.SmallTextureSize, this.SmallTextureSize, true, SurfaceFormat.Alpha8, DepthFormat.None);
			}
			if (this._pixelTarget == null || this._pixelTarget.IsContentLost)
			{
				this._pixelTarget = new RenderTarget2D(graphicsDevice, 1, 1, false, SurfaceFormat.Alpha8, DepthFormat.None);
			}
			Rectangle rectangle = Utils.CenteredRectangle(Main.ReverseGravitySupport(Main.LastCelestialBodyPosition * Main.ScreenSize.ToVector2(), 0f), new Vector2((float)this.SampleAreaSize) * Main.BackgroundViewMatrix.RenderZoom);
			if (HorizonHelper.DebugSunVisibility)
			{
				this.Test_DrawSmallTarget(bigTarget, rectangle);
			}
			graphicsDevice.SetRenderTarget(this._tinyTarget);
			graphicsDevice.Clear(Color.Transparent);
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
			Main.spriteBatch.Draw(bigTarget, this._tinyTarget.Bounds, new Rectangle?(rectangle), Color.White);
			Main.spriteBatch.End();
			graphicsDevice.SetRenderTarget(this._pixelTarget);
			graphicsDevice.Clear(Color.White);
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, this._horizonBlendState, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
			Main.spriteBatch.Draw(this._tinyTarget, this._pixelTarget.Bounds, Color.White);
			Main.spriteBatch.End();
			graphicsDevice.SetRenderTarget(null);
			TimeLogger.SunVisibility.AddTime(fromTimestamp);
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x005BA2EC File Offset: 0x005B84EC
		private void Test_DrawSmallTarget(RenderTarget2D bigTarget, Rectangle sunSampleRect)
		{
			GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
			graphicsDevice.SetRenderTarget(bigTarget);
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, new BlendState
			{
				ColorDestinationBlend = Blend.Zero,
				ColorSourceBlend = Blend.SourceAlpha,
				AlphaDestinationBlend = Blend.Zero,
				AlphaSourceBlend = Blend.SourceAlpha
			}, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
			Main.spriteBatch.Draw(this._tinyTarget, new Rectangle(0, 0, sunSampleRect.Width, sunSampleRect.Height), Color.White);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin();
			Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(sunSampleRect.Left, sunSampleRect.Top, 1, sunSampleRect.Height), Color.Red);
			Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(sunSampleRect.Right, sunSampleRect.Top, 1, sunSampleRect.Height), Color.Red);
			Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(sunSampleRect.Left, sunSampleRect.Top, sunSampleRect.Width, 1), Color.Red);
			Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(sunSampleRect.Left, sunSampleRect.Bottom, sunSampleRect.Width, 1), Color.Red);
			Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(sunSampleRect.Width, 0, 1, sunSampleRect.Height), Color.Red);
			Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, sunSampleRect.Height, sunSampleRect.Width, 1), Color.Red);
			byte[] array = new byte[1];
			this._pixelTarget.GetData<byte>(array);
			float num = (float)array[0] / 255f;
			Utils.DrawBorderString(Main.spriteBatch, string.Format("{0:F3}", num), new Vector2(10f, (float)(sunSampleRect.Height + 20)), Color.White, 1f, 0f, 0f, -1);
			Main.spriteBatch.End();
			graphicsDevice.SetRenderTarget(null);
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x005BA517 File Offset: 0x005B8717
		public static void GetCelestialBodyColors(out Color sunColor, out Color moonColor)
		{
			sunColor = new Color(255, 246, 204);
			moonColor = HorizonHelper.GetMoonColor() * HorizonHelper.GetMoonStrength();
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x005BA548 File Offset: 0x005B8748
		private static Color GetMoonColor()
		{
			Color result = new Color(230, 235, 255);
			int num = Main.moonType;
			if (!TextureAssets.Moon.IndexInRange(num))
			{
				num = Utils.Clamp<int>(num, 0, 8);
			}
			result = HorizonHelper.MoonColors[num];
			if (Main.pumpkinMoon)
			{
				result = new Color(255, 225, 180);
			}
			if (Main.snowMoon)
			{
				result = new Color(220, 220, 255);
			}
			if (WorldGen.drunkWorldGen)
			{
				result = new Color(255, 255, 255);
			}
			return result;
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x005BA5EB File Offset: 0x005B87EB
		public static float GetMoonStrength()
		{
			return Utils.Remap((float)Math.Abs(4 - Main.moonPhase), 0f, 4f, 0f, 1f, true);
		}

		// Token: 0x040056C1 RID: 22209
		public static bool DebugSunVisibility = false;

		// Token: 0x040056C2 RID: 22210
		private readonly int SampleAreaSize = 128;

		// Token: 0x040056C3 RID: 22211
		private readonly int SmallTextureSize = 64;

		// Token: 0x040056C4 RID: 22212
		private RenderTarget2D _tinyTarget;

		// Token: 0x040056C5 RID: 22213
		private RenderTarget2D _pixelTarget;

		// Token: 0x040056C6 RID: 22214
		private BlendState _horizonBlendState = new BlendState
		{
			AlphaSourceBlend = Blend.Zero,
			AlphaDestinationBlend = Blend.InverseSourceAlpha,
			ColorSourceBlend = Blend.Zero,
			ColorDestinationBlend = Blend.InverseSourceAlpha
		};

		// Token: 0x040056C7 RID: 22215
		private static Color[] MoonColors = new Color[]
		{
			new Color(230, 235, 255),
			new Color(250, 235, 160),
			new Color(230, 255, 230),
			new Color(160, 240, 255),
			new Color(180, 255, 255),
			new Color(230, 255, 230),
			new Color(255, 180, 255),
			new Color(255, 200, 180),
			new Color(225, 180, 255)
		};
	}
}
