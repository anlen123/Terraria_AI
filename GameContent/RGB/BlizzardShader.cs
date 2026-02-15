using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002B3 RID: 691
	public class BlizzardShader : ChromaShader
	{
		// Token: 0x06002588 RID: 9608 RVA: 0x00557488 File Offset: 0x00555688
		public BlizzardShader(Vector4 frontColor, Vector4 backColor, float panSpeedX, float panSpeedY)
		{
			this._frontColor = frontColor;
			this._backColor = backColor;
			this._timeScaleX = panSpeedX;
			this._timeScaleY = panSpeedY;
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x005574F8 File Offset: 0x005556F8
		[RgbProcessor(new EffectDetailLevel[]
		{
			0,
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			if (quality == null)
			{
				time *= 0.25f;
			}
			for (int i = 0; i < fragment.Count; i++)
			{
				float staticNoise = NoiseHelper.GetStaticNoise(fragment.GetCanvasPositionOfIndex(i) * new Vector2(0.2f, 0.4f) + new Vector2(time * this._timeScaleX, time * this._timeScaleY));
				Vector4 vector = Vector4.Lerp(this._backColor, this._frontColor, staticNoise * staticNoise);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FDA RID: 20442
		private readonly Vector4 _backColor = new Vector4(0.1f, 0.1f, 0.3f, 1f);

		// Token: 0x04004FDB RID: 20443
		private readonly Vector4 _frontColor = new Vector4(1f, 1f, 1f, 1f);

		// Token: 0x04004FDC RID: 20444
		private readonly float _timeScaleX;

		// Token: 0x04004FDD RID: 20445
		private readonly float _timeScaleY;
	}
}
