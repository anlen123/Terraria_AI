using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002C9 RID: 713
	public class TempleShader : ChromaShader
	{
		// Token: 0x060025D4 RID: 9684 RVA: 0x00559760 File Offset: 0x00557960
		[RgbProcessor(new EffectDetailLevel[]
		{
			0,
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				ref Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector4 vector = this._backgroundColor;
				float num = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.Y * 7) * 10f + time) % 10f - (canvasPositionOfIndex.X + 2f);
				if (num > 0f)
				{
					float amount = Math.Max(0f, 1.2f - num);
					if (num < 0.2f)
					{
						amount = num * 5f;
					}
					vector = Vector4.Lerp(vector, this._glowColor, amount);
				}
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x0400501E RID: 20510
		private readonly Vector4 _backgroundColor = new Vector4(0.05f, 0.025f, 0f, 1f);

		// Token: 0x0400501F RID: 20511
		private readonly Vector4 _glowColor = Color.Orange.ToVector4();
	}
}
