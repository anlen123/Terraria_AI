using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x0200029E RID: 670
	public class DeathShader : ChromaShader
	{
		// Token: 0x0600254A RID: 9546 RVA: 0x00554F86 File Offset: 0x00553186
		public DeathShader(Color primaryColor, Color secondaryColor)
		{
			this._primaryColor = primaryColor.ToVector4();
			this._secondaryColor = secondaryColor.ToVector4();
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x00554FA8 File Offset: 0x005531A8
		[RgbProcessor(new EffectDetailLevel[]
		{
			1,
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			time *= 3f;
			float amount = 0f;
			float num = time % 12.566371f;
			if (num < 3.1415927f)
			{
				amount = (float)Math.Sin((double)num);
			}
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(this._primaryColor, this._secondaryColor, amount);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004F9C RID: 20380
		private readonly Vector4 _primaryColor;

		// Token: 0x04004F9D RID: 20381
		private readonly Vector4 _secondaryColor;
	}
}
