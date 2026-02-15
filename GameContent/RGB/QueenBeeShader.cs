using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002AC RID: 684
	public class QueenBeeShader : ChromaShader
	{
		// Token: 0x06002572 RID: 9586 RVA: 0x005568F5 File Offset: 0x00554AF5
		public QueenBeeShader(Color primaryColor, Color secondaryColor)
		{
			this._primaryColor = primaryColor.ToVector4();
			this._secondaryColor = secondaryColor.ToVector4();
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x00556918 File Offset: 0x00554B18
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(this._primaryColor, this._secondaryColor, (float)Math.Sin((double)(time * 2f + canvasPositionOfIndex.X * 10f)) * 0.5f + 0.5f);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x00556980 File Offset: 0x00554B80
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			time *= 0.5f;
			for (int i = 0; i < fragment.Count; i++)
			{
				float amount = MathHelper.Clamp((float)Math.Sin((double)fragment.GetCanvasPositionOfIndex(i).X * 5.0 - (double)(4f * time)) * 1.5f, 0f, 1f);
				Vector4 vector = Vector4.Lerp(this._primaryColor, this._secondaryColor, amount);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FC3 RID: 20419
		private readonly Vector4 _primaryColor;

		// Token: 0x04004FC4 RID: 20420
		private readonly Vector4 _secondaryColor;
	}
}
