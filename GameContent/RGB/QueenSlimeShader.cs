using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002AD RID: 685
	public class QueenSlimeShader : ChromaShader
	{
		// Token: 0x06002575 RID: 9589 RVA: 0x00556A00 File Offset: 0x00554C00
		public QueenSlimeShader(Color slimeColor, Color debrisColor)
		{
			this._slimeColor = slimeColor.ToVector4();
			this._debrisColor = debrisColor.ToVector4();
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x00556A24 File Offset: 0x00554C24
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				float num = NoiseHelper.GetDynamicNoise(fragment.GetCanvasPositionOfIndex(i), time * 0.25f);
				num = Math.Max(0f, 1f - num * 2f);
				Vector4 vector = Vector4.Lerp(this._slimeColor, this._debrisColor, num);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x00556A8C File Offset: 0x00554C8C
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			new Vector2(1.6f, 0.5f);
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = this._slimeColor;
				float num = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.3f + new Vector2(0f, time * 0.1f));
				num = Math.Max(0f, 1f - num * 3f);
				num = (float)Math.Sqrt((double)num);
				vector = Vector4.Lerp(vector, this._debrisColor, num);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FC5 RID: 20421
		private readonly Vector4 _slimeColor;

		// Token: 0x04004FC6 RID: 20422
		private readonly Vector4 _debrisColor;
	}
}
