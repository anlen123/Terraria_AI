using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002BE RID: 702
	public class LowLifeShader : ChromaShader
	{
		// Token: 0x060025AB RID: 9643 RVA: 0x0055869C File Offset: 0x0055689C
		[RgbProcessor(new EffectDetailLevel[]
		{
			0,
			1
		}, IsTransparent = true)]
		private void ProcessAnyDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			float scaleFactor = (float)Math.Cos((double)(time * 3.1415927f)) * 0.3f + 0.7f;
			Vector4 vector = LowLifeShader._baseColor * scaleFactor;
			vector.W = LowLifeShader._baseColor.W;
			for (int i = 0; i < fragment.Count; i++)
			{
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FFF RID: 20479
		private static Vector4 _baseColor = new Color(40, 0, 8, 255).ToVector4();
	}
}
