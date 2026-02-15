using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002B2 RID: 690
	public class WormShader : ChromaShader
	{
		// Token: 0x06002584 RID: 9604 RVA: 0x00555369 File Offset: 0x00553569
		public WormShader()
		{
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x005572CB File Offset: 0x005554CB
		public WormShader(Color skinColor, Color eyeColor, Color innerEyeColor)
		{
			this._skinColor = skinColor.ToVector4();
			this._eyeColor = eyeColor.ToVector4();
			this._innerEyeColor = innerEyeColor.ToVector4();
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x005572FC File Offset: 0x005554FC
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float amount = Math.Max(0f, (float)Math.Sin((double)(time * -3f + canvasPositionOfIndex.X)));
				Vector4 vector = Vector4.Lerp(this._skinColor, this._eyeColor, amount);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x00557360 File Offset: 0x00555560
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			time *= 0.25f;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				canvasPositionOfIndex.X -= time * 1.5f;
				canvasPositionOfIndex.X %= 2f;
				if (canvasPositionOfIndex.X < 0f)
				{
					canvasPositionOfIndex.X += 2f;
				}
				float num = (canvasPositionOfIndex - new Vector2(0.5f)).Length();
				Vector4 vector = this._skinColor;
				if (num < 0.5f)
				{
					float num2 = MathHelper.Clamp((num - 0.5f + 0.2f) / 0.2f, 0f, 1f);
					vector = Vector4.Lerp(vector, this._eyeColor, 1f - num2);
					if (num < 0.4f)
					{
						num2 = MathHelper.Clamp((num - 0.4f + 0.2f) / 0.2f, 0f, 1f);
						vector = Vector4.Lerp(vector, this._innerEyeColor, 1f - num2);
					}
				}
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FD7 RID: 20439
		private readonly Vector4 _skinColor;

		// Token: 0x04004FD8 RID: 20440
		private readonly Vector4 _eyeColor;

		// Token: 0x04004FD9 RID: 20441
		private readonly Vector4 _innerEyeColor;
	}
}
