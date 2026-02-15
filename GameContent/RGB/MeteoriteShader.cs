using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002BF RID: 703
	public class MeteoriteShader : ChromaShader
	{
		// Token: 0x060025AE RID: 9646 RVA: 0x00558724 File Offset: 0x00556924
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(this._baseColor, this._secondaryColor, (float)Math.Sin((double)(time + canvasPositionOfIndex.X)) * 0.5f + 0.5f);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x00558780 File Offset: 0x00556980
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector4 vector = this._baseColor;
				float dynamicNoise = NoiseHelper.GetDynamicNoise(gridPositionOfIndex.X, gridPositionOfIndex.Y, time / 10f);
				vector = Vector4.Lerp(vector, this._secondaryColor, dynamicNoise * dynamicNoise);
				float num = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.5f + new Vector2(0f, time * 0.05f), time / 20f);
				num = Math.Max(0f, 1f - num * 2f);
				vector = Vector4.Lerp(vector, this._glowColor, (float)Math.Sqrt((double)num) * 0.75f);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04005000 RID: 20480
		private readonly Vector4 _baseColor = new Color(39, 15, 26).ToVector4();

		// Token: 0x04005001 RID: 20481
		private readonly Vector4 _secondaryColor = new Color(69, 50, 43).ToVector4();

		// Token: 0x04005002 RID: 20482
		private readonly Vector4 _glowColor = Color.DarkOrange.ToVector4();
	}
}
