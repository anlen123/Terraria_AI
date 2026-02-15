using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002CE RID: 718
	public class VineShader : ChromaShader
	{
		// Token: 0x060025E2 RID: 9698 RVA: 0x00559ED8 File Offset: 0x005580D8
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				fragment.GetCanvasPositionOfIndex(i);
				fragment.SetColor(i, this._backgroundColor);
			}
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x00559F0C File Offset: 0x0055810C
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				ref Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float num = NoiseHelper.GetStaticNoise(gridPositionOfIndex.X);
				num = (num * 10f + time * 0.4f) % 10f;
				float num2 = 1f;
				if (num > 1f)
				{
					num2 = 1f - MathHelper.Clamp((num - 0.4f - 1f) / 0.4f, 0f, 1f);
					num = 1f;
				}
				float num3 = num - canvasPositionOfIndex.Y / 1f;
				Vector4 vector = this._backgroundColor;
				if (num3 > 0f)
				{
					float num4 = 1f;
					if (num3 < 0.2f)
					{
						num4 = num3 / 0.2f;
					}
					vector = Vector4.Lerp(this._backgroundColor, this._vineColor, num4 * num2);
				}
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x0400502C RID: 20524
		private readonly Vector4 _backgroundColor = new Color(46, 17, 6).ToVector4();

		// Token: 0x0400502D RID: 20525
		private readonly Vector4 _vineColor = Color.Green.ToVector4();
	}
}
