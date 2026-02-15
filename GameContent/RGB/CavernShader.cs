using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002B4 RID: 692
	public class CavernShader : ChromaShader
	{
		// Token: 0x0600258A RID: 9610 RVA: 0x0055757D File Offset: 0x0055577D
		public CavernShader(Color backColor, Color frontColor, float speed)
		{
			this._backColor = backColor.ToVector4();
			this._frontColor = frontColor.ToVector4();
			this._speed = speed;
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x005575A8 File Offset: 0x005557A8
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(this._backColor, this._frontColor, (float)Math.Sin((double)(time * this._speed + canvasPositionOfIndex.X)) * 0.5f + 0.5f);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x0055760C File Offset: 0x0055580C
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			time *= this._speed * 0.5f;
			float num = time % 1f;
			bool flag = time % 2f > 1f;
			Vector4 vector = flag ? this._frontColor : this._backColor;
			Vector4 value = flag ? this._backColor : this._frontColor;
			num *= 1.2f;
			for (int i = 0; i < fragment.Count; i++)
			{
				float num2 = NoiseHelper.GetStaticNoise(fragment.GetCanvasPositionOfIndex(i) * 0.5f + new Vector2(0f, time * 0.5f));
				Vector4 vector2 = vector;
				num2 += num;
				if (num2 > 0.999f)
				{
					float amount = MathHelper.Clamp((num2 - 0.999f) / 0.2f, 0f, 1f);
					vector2 = Vector4.Lerp(vector2, value, amount);
				}
				fragment.SetColor(i, vector2);
			}
		}

		// Token: 0x04004FDE RID: 20446
		private readonly Vector4 _backColor;

		// Token: 0x04004FDF RID: 20447
		private readonly Vector4 _frontColor;

		// Token: 0x04004FE0 RID: 20448
		private readonly float _speed;
	}
}
