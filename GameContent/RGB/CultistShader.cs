using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x0200029C RID: 668
	public class CultistShader : ChromaShader
	{
		// Token: 0x06002546 RID: 9542 RVA: 0x00554B3C File Offset: 0x00552D3C
		[RgbProcessor(new EffectDetailLevel[]
		{
			1,
			0
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			time *= 2f;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = this._backgroundColor;
				float num = time * 0.5f + canvasPositionOfIndex.X + canvasPositionOfIndex.Y;
				float num2 = (float)Math.Cos((double)num) * 2f + 2f;
				num2 = MathHelper.Clamp(num2, 0f, 1f);
				num = (num + 3.1415927f) % 18.849556f;
				Vector4 value;
				if (num < 6.2831855f)
				{
					float num3 = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.3f + new Vector2(12.5f, time * 0.2f));
					num3 = Math.Max(0f, 1f - num3 * num3 * 4f * num3);
					num3 = MathHelper.Clamp(num3, 0f, 1f);
					value = Vector4.Lerp(this._fireDarkColor, this._fireBrightColor, num3);
				}
				else if (num < 12.566371f)
				{
					float num4 = NoiseHelper.GetDynamicNoise(new Vector2((canvasPositionOfIndex.X + canvasPositionOfIndex.Y) * 0.2f, 0f), time / 5f);
					num4 = Math.Max(0f, 1f - num4 * 1.5f);
					value = Vector4.Lerp(this._iceDarkColor, this._iceBrightColor, num4);
				}
				else
				{
					float num5 = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.15f, time * 0.05f);
					num5 = (float)Math.Sin((double)(num5 * 15f)) * 0.5f + 0.5f;
					num5 = Math.Max(0f, 1f - 5f * num5);
					value = Vector4.Lerp(this._lightningDarkColor, this._lightningBrightColor, num5);
				}
				vector = Vector4.Lerp(vector, value, num2);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004F93 RID: 20371
		private readonly Vector4 _lightningDarkColor = new Color(23, 11, 23).ToVector4();

		// Token: 0x04004F94 RID: 20372
		private readonly Vector4 _lightningBrightColor = new Color(249, 140, 255).ToVector4();

		// Token: 0x04004F95 RID: 20373
		private readonly Vector4 _fireDarkColor = Color.Red.ToVector4();

		// Token: 0x04004F96 RID: 20374
		private readonly Vector4 _fireBrightColor = new Color(255, 196, 0).ToVector4();

		// Token: 0x04004F97 RID: 20375
		private readonly Vector4 _iceDarkColor = new Color(4, 4, 148).ToVector4();

		// Token: 0x04004F98 RID: 20376
		private readonly Vector4 _iceBrightColor = new Color(208, 233, 255).ToVector4();

		// Token: 0x04004F99 RID: 20377
		private readonly Vector4 _backgroundColor = Color.Black.ToVector4();
	}
}
