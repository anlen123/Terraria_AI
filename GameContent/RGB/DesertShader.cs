using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002B7 RID: 695
	public class DesertShader : ChromaShader
	{
		// Token: 0x06002596 RID: 9622 RVA: 0x00557A34 File Offset: 0x00555C34
		public DesertShader(Color baseColor, Color sandColor)
		{
			this._baseColor = baseColor.ToVector4();
			this._sandColor = sandColor.ToVector4();
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x00557A58 File Offset: 0x00555C58
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
				fragment.GetGridPositionOfIndex(i);
				canvasPositionOfIndex.Y += (float)Math.Sin((double)(canvasPositionOfIndex.X * 2f + time * 2f)) * 0.2f;
				float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * new Vector2(0.1f, 0.5f));
				Vector4 vector = Vector4.Lerp(this._baseColor, this._sandColor, staticNoise * staticNoise);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FE4 RID: 20452
		private readonly Vector4 _baseColor;

		// Token: 0x04004FE5 RID: 20453
		private readonly Vector4 _sandColor;
	}
}
