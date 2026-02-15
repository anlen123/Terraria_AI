using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002BB RID: 699
	public class IceShader : ChromaShader
	{
		// Token: 0x060025A3 RID: 9635 RVA: 0x005582D4 File Offset: 0x005564D4
		public IceShader(Color baseColor, Color iceColor)
		{
			this._baseColor = baseColor.ToVector4();
			this._iceColor = iceColor.ToVector4();
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x00558320 File Offset: 0x00556520
		[RgbProcessor(new EffectDetailLevel[]
		{
			0,
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float num = NoiseHelper.GetDynamicNoise(new Vector2((canvasPositionOfIndex.X - canvasPositionOfIndex.Y) * 0.2f, 0f), time / 5f);
				num = Math.Max(0f, 1f - num * 1.5f);
				float num2 = NoiseHelper.GetDynamicNoise(new Vector2((canvasPositionOfIndex.X - canvasPositionOfIndex.Y) * 0.3f, 0.3f), time / 20f);
				num2 = Math.Max(0f, 1f - num2 * 5f);
				Vector4 vector = Vector4.Lerp(this._baseColor, this._iceColor, num);
				vector = Vector4.Lerp(vector, this._shineColor, num2);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FF8 RID: 20472
		private readonly Vector4 _baseColor;

		// Token: 0x04004FF9 RID: 20473
		private readonly Vector4 _iceColor;

		// Token: 0x04004FFA RID: 20474
		private readonly Vector4 _shineColor = new Vector4(1f, 1f, 0.7f, 1f);
	}
}
