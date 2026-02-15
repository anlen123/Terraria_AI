using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002AA RID: 682
	public class PirateInvasionShader : ChromaShader
	{
		// Token: 0x0600256C RID: 9580 RVA: 0x00556528 File Offset: 0x00554728
		public PirateInvasionShader(Color cannonBallColor, Color splashColor, Color waterColor, Color backgroundColor)
		{
			this._cannonBallColor = cannonBallColor.ToVector4();
			this._splashColor = splashColor.ToVector4();
			this._waterColor = waterColor.ToVector4();
			this._backgroundColor = backgroundColor.ToVector4();
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x00556564 File Offset: 0x00554764
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(this._waterColor, this._cannonBallColor, (float)Math.Sin((double)(time * 0.5f + canvasPositionOfIndex.X)) * 0.5f + 0.5f);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x005565C8 File Offset: 0x005547C8
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
				gridPositionOfIndex.X /= 2;
				float num = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.X) * 40f + time * 1f) % 40f;
				float amount = 0f;
				float num2 = num - canvasPositionOfIndex.Y / 1.2f;
				if (num > 1f)
				{
					float num3 = 1f - canvasPositionOfIndex.Y / 1.2f;
					amount = (1f - Math.Min(1f, num2 - num3)) * (1f - Math.Min(1f, num3 / 1f));
				}
				Vector4 vector = this._backgroundColor;
				if (num2 > 0f)
				{
					float amount2 = Math.Max(0f, 1.2f - num2 * 4f);
					if (num2 < 0.1f)
					{
						amount2 = num2 / 0.1f;
					}
					vector = Vector4.Lerp(vector, this._cannonBallColor, amount2);
					vector = Vector4.Lerp(vector, this._splashColor, amount);
				}
				if (canvasPositionOfIndex.Y > 0.8f)
				{
					vector = this._waterColor;
				}
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FBC RID: 20412
		private readonly Vector4 _cannonBallColor;

		// Token: 0x04004FBD RID: 20413
		private readonly Vector4 _splashColor;

		// Token: 0x04004FBE RID: 20414
		private readonly Vector4 _waterColor;

		// Token: 0x04004FBF RID: 20415
		private readonly Vector4 _backgroundColor;
	}
}
