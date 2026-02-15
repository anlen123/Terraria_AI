using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x0200029F RID: 671
	public class DukeFishronShader : ChromaShader
	{
		// Token: 0x0600254C RID: 9548 RVA: 0x0055500B File Offset: 0x0055320B
		public DukeFishronShader(Color primaryColor, Color secondaryColor)
		{
			this._primaryColor = primaryColor.ToVector4();
			this._secondaryColor = secondaryColor.ToVector4();
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x00555030 File Offset: 0x00553230
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(this._primaryColor, this._secondaryColor, Math.Max(0f, (float)Math.Sin((double)(time * 2f + canvasPositionOfIndex.X))));
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x00555090 File Offset: 0x00553290
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				ref Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float dynamicNoise = NoiseHelper.GetDynamicNoise(fragment.GetGridPositionOfIndex(i).Y, time);
				float num = (float)Math.Sin((double)(canvasPositionOfIndex.X + 2f * time + dynamicNoise)) - 0.2f;
				num = Math.Max(0f, num);
				Vector4 vector = Vector4.Lerp(this._primaryColor, this._secondaryColor, num);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004F9E RID: 20382
		private readonly Vector4 _primaryColor;

		// Token: 0x04004F9F RID: 20383
		private readonly Vector4 _secondaryColor;
	}
}
