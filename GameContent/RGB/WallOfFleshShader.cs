using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002B1 RID: 689
	public class WallOfFleshShader : ChromaShader
	{
		// Token: 0x06002582 RID: 9602 RVA: 0x00557227 File Offset: 0x00555427
		public WallOfFleshShader(Color primaryColor, Color secondaryColor)
		{
			this._primaryColor = primaryColor.ToVector4();
			this._secondaryColor = secondaryColor.ToVector4();
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x0055724C File Offset: 0x0055544C
		[RgbProcessor(new EffectDetailLevel[]
		{
			1,
			0
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = this._secondaryColor;
				float num = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.3f, time / 5f);
				num = Math.Max(0f, 1f - num * 2f);
				vector = Vector4.Lerp(vector, this._primaryColor, (float)Math.Sqrt((double)num) * 0.75f);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FD5 RID: 20437
		private readonly Vector4 _primaryColor;

		// Token: 0x04004FD6 RID: 20438
		private readonly Vector4 _secondaryColor;
	}
}
