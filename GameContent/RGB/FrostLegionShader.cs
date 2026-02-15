using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002A2 RID: 674
	public class FrostLegionShader : ChromaShader
	{
		// Token: 0x06002556 RID: 9558 RVA: 0x005556E5 File Offset: 0x005538E5
		public FrostLegionShader(Color primaryColor, Color secondaryColor)
		{
			this._primaryColor = primaryColor.ToVector4();
			this._secondaryColor = secondaryColor.ToVector4();
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x00555708 File Offset: 0x00553908
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
				float staticNoise = NoiseHelper.GetStaticNoise(fragment.GetGridPositionOfIndex(i).X / 2);
				float num = (canvasPositionOfIndex.Y + canvasPositionOfIndex.X / 2f - staticNoise + time) % 2f;
				if (num < 0f)
				{
					num += 2f;
				}
				if (num < 0.2f)
				{
					num = 1f - num / 0.2f;
				}
				float amount = num / 2f;
				Vector4 vector = Vector4.Lerp(this._primaryColor, this._secondaryColor, amount);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FA4 RID: 20388
		private readonly Vector4 _primaryColor;

		// Token: 0x04004FA5 RID: 20389
		private readonly Vector4 _secondaryColor;
	}
}
