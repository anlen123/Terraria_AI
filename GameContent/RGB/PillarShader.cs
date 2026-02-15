using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002A9 RID: 681
	public class PillarShader : ChromaShader
	{
		// Token: 0x06002569 RID: 9577 RVA: 0x0055638C File Offset: 0x0055458C
		public PillarShader(Color primaryColor, Color secondaryColor)
		{
			this._primaryColor = primaryColor.ToVector4();
			this._secondaryColor = secondaryColor.ToVector4();
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x005563B0 File Offset: 0x005545B0
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(this._primaryColor, this._secondaryColor, (float)Math.Sin((double)(time * 2.5f + canvasPositionOfIndex.X)) * 0.5f + 0.5f);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x00556414 File Offset: 0x00554614
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector2 value = new Vector2(1.5f, 0.5f);
			time *= 4f;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 vector = fragment.GetCanvasPositionOfIndex(i) - value;
				float num = vector.Length() * 2f;
				float num2 = (float)Math.Atan2((double)vector.Y, (double)vector.X);
				float amount = (float)Math.Sin((double)(num * 4f - time - num2)) * 0.5f + 0.5f;
				Vector4 vector2 = Vector4.Lerp(this._primaryColor, this._secondaryColor, amount);
				if (num < 1f)
				{
					float num3 = num / 1f;
					num3 *= num3 * num3;
					float amount2 = (float)Math.Sin((double)(4f - time - num2)) * 0.5f + 0.5f;
					vector2 = Vector4.Lerp(this._primaryColor, this._secondaryColor, amount2) * num3;
				}
				vector2.W = 1f;
				fragment.SetColor(i, vector2);
			}
		}

		// Token: 0x04004FBA RID: 20410
		private readonly Vector4 _primaryColor;

		// Token: 0x04004FBB RID: 20411
		private readonly Vector4 _secondaryColor;
	}
}
