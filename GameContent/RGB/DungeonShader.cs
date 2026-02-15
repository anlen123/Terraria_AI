using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002B8 RID: 696
	public class DungeonShader : ChromaShader
	{
		// Token: 0x06002598 RID: 9624 RVA: 0x00557AF0 File Offset: 0x00555CF0
		[RgbProcessor(new EffectDetailLevel[]
		{
			0,
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				ref Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float num = ((NoiseHelper.GetStaticNoise(gridPositionOfIndex.Y) * 10f + time) % 10f - (canvasPositionOfIndex.X + 2f)) * 0.5f;
				Vector4 vector = this._backgroundColor;
				if (num > 0f)
				{
					float num2 = Math.Max(0f, 1.2f - num);
					float amount = MathHelper.Clamp(num2 * num2 * num2, 0f, 1f);
					if (num < 0.2f)
					{
						num2 = num / 0.2f;
					}
					Vector4 value = Vector4.Lerp(this._spiritTrailColor, this._spiritColor, amount);
					vector = Vector4.Lerp(vector, value, num2);
				}
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FE6 RID: 20454
		private readonly Vector4 _backgroundColor = new Color(5, 5, 5).ToVector4();

		// Token: 0x04004FE7 RID: 20455
		private readonly Vector4 _spiritTrailColor = new Color(6, 51, 222).ToVector4();

		// Token: 0x04004FE8 RID: 20456
		private readonly Vector4 _spiritColor = Color.White.ToVector4();
	}
}
