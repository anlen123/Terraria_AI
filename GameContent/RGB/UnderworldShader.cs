using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002B0 RID: 688
	public class UnderworldShader : ChromaShader
	{
		// Token: 0x0600257F RID: 9599 RVA: 0x00557138 File Offset: 0x00555338
		public UnderworldShader(Color backColor, Color frontColor, float speed)
		{
			this._backColor = backColor.ToVector4();
			this._frontColor = frontColor.ToVector4();
			this._speed = speed;
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00557164 File Offset: 0x00555364
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

		// Token: 0x06002581 RID: 9601 RVA: 0x005571C8 File Offset: 0x005553C8
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				float dynamicNoise = NoiseHelper.GetDynamicNoise(fragment.GetCanvasPositionOfIndex(i) * 0.5f, time * this._speed / 3f);
				Vector4 vector = Vector4.Lerp(this._backColor, this._frontColor, dynamicNoise);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FD2 RID: 20434
		private readonly Vector4 _backColor;

		// Token: 0x04004FD3 RID: 20435
		private readonly Vector4 _frontColor;

		// Token: 0x04004FD4 RID: 20436
		private readonly float _speed;
	}
}
