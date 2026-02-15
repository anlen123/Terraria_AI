using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x0200029D RID: 669
	public class DD2Shader : ChromaShader
	{
		// Token: 0x06002548 RID: 9544 RVA: 0x00554DF2 File Offset: 0x00552FF2
		public DD2Shader(Color darkGlowColor, Color lightGlowColor)
		{
			this._darkGlowColor = darkGlowColor.ToVector4();
			this._lightGlowColor = lightGlowColor.ToVector4();
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x00554E14 File Offset: 0x00553014
		[RgbProcessor(new EffectDetailLevel[]
		{
			0,
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector2 canvasCenter = fragment.CanvasCenter;
			if (quality == null)
			{
				canvasCenter = new Vector2(1.7f, 0.5f);
			}
			time *= 0.5f;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = new Vector4(0f, 0f, 0f, 1f);
				float num = (canvasPositionOfIndex - canvasCenter).Length();
				float num2 = num * num * 0.75f;
				float num3 = (num - time) % 1f;
				if (num3 < 0f)
				{
					num3 += 1f;
				}
				if (num3 > 0.8f)
				{
					num3 *= 1f - (num3 - 1f + 0.2f) / 0.2f;
				}
				else
				{
					num3 /= 0.8f;
				}
				Vector4 value = Vector4.Lerp(this._darkGlowColor, this._lightGlowColor, num3 * num3);
				num3 *= MathHelper.Clamp(1f - num2, 0f, 1f) * 0.75f + 0.25f;
				vector = Vector4.Lerp(vector, value, num3);
				if (num < 0.5f)
				{
					float amount = 1f - MathHelper.Clamp((num - 0.5f + 0.4f) / 0.4f, 0f, 1f);
					vector = Vector4.Lerp(vector, this._lightGlowColor, amount);
				}
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004F9A RID: 20378
		private readonly Vector4 _darkGlowColor;

		// Token: 0x04004F9B RID: 20379
		private readonly Vector4 _lightGlowColor;
	}
}
