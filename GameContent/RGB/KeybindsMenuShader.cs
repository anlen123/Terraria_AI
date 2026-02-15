using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002BD RID: 701
	internal class KeybindsMenuShader : ChromaShader
	{
		// Token: 0x060025A8 RID: 9640 RVA: 0x00558610 File Offset: 0x00556810
		[RgbProcessor(new EffectDetailLevel[]
		{
			0,
			1
		}, IsTransparent = true)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			float scaleFactor = (float)Math.Cos((double)(time * 1.5707964f)) * 0.2f + 0.8f;
			Vector4 vector = KeybindsMenuShader._baseColor * scaleFactor;
			vector.W = KeybindsMenuShader._baseColor.W;
			for (int i = 0; i < fragment.Count; i++)
			{
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FFE RID: 20478
		private static Vector4 _baseColor = new Color(20, 20, 20, 245).ToVector4();
	}
}
