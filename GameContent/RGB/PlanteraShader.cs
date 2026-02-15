using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002AB RID: 683
	public class PlanteraShader : ChromaShader
	{
		// Token: 0x0600256F RID: 9583 RVA: 0x00556705 File Offset: 0x00554905
		public PlanteraShader(Color bulbColor, Color vineColor, Color backgroundColor)
		{
			this._bulbColor = bulbColor.ToVector4();
			this._vineColor = vineColor.ToVector4();
			this._backgroundColor = backgroundColor.ToVector4();
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x00556734 File Offset: 0x00554934
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(this._bulbColor, this._vineColor, (float)Math.Sin((double)(time * 2f + canvasPositionOfIndex.X * 10f)) * 0.5f + 0.5f);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x0055679C File Offset: 0x0055499C
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				canvasPositionOfIndex.X -= 1.8f;
				if (canvasPositionOfIndex.X < 0f)
				{
					canvasPositionOfIndex.X *= -1f;
					gridPositionOfIndex.Y += 101;
				}
				float num = NoiseHelper.GetStaticNoise(gridPositionOfIndex.Y);
				num = (num * 5f + time * 0.4f) % 5f;
				float num2 = 1f;
				if (num > 1f)
				{
					num2 = 1f - MathHelper.Clamp((num - 0.4f - 1f) / 0.4f, 0f, 1f);
					num = 1f;
				}
				float num3 = num - canvasPositionOfIndex.X / 5f;
				Vector4 vector = this._backgroundColor;
				if (num3 > 0f)
				{
					float num4 = 1f;
					if (num3 < 0.2f)
					{
						num4 = num3 / 0.2f;
					}
					if ((gridPositionOfIndex.X + 7 * gridPositionOfIndex.Y) % 5 == 0)
					{
						vector = Vector4.Lerp(this._backgroundColor, this._bulbColor, num4 * num2);
					}
					else
					{
						vector = Vector4.Lerp(this._backgroundColor, this._vineColor, num4 * num2);
					}
				}
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FC0 RID: 20416
		private readonly Vector4 _bulbColor;

		// Token: 0x04004FC1 RID: 20417
		private readonly Vector4 _vineColor;

		// Token: 0x04004FC2 RID: 20418
		private readonly Vector4 _backgroundColor;
	}
}
