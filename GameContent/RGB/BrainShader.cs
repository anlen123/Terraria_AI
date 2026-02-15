using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x0200029B RID: 667
	public class BrainShader : ChromaShader
	{
		// Token: 0x06002543 RID: 9539 RVA: 0x005549DD File Offset: 0x00552BDD
		public BrainShader(Color brainColor, Color veinColor)
		{
			this._brainColor = brainColor.ToVector4();
			this._veinColor = veinColor.ToVector4();
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x00554A00 File Offset: 0x00552C00
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 vector = Vector4.Lerp(this._brainColor, this._veinColor, Math.Max(0f, (float)Math.Sin((double)(time * 3f))));
			for (int i = 0; i < fragment.Count; i++)
			{
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x00554A54 File Offset: 0x00552C54
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			new Vector2(1.6f, 0.5f);
			Vector4 value = Vector4.Lerp(this._brainColor, this._veinColor, Math.Max(0f, (float)Math.Sin((double)(time * 3f))) * 0.5f + 0.5f);
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = this._brainColor;
				float num = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.15f + new Vector2(time * 0.002f), time * 0.03f);
				num = (float)Math.Sin((double)(num * 10f)) * 0.5f + 0.5f;
				num = Math.Max(0f, 1f - 5f * num);
				vector = Vector4.Lerp(vector, value, num);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004F91 RID: 20369
		private readonly Vector4 _brainColor;

		// Token: 0x04004F92 RID: 20370
		private readonly Vector4 _veinColor;
	}
}
