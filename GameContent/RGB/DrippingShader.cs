using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002CB RID: 715
	public class DrippingShader : ChromaShader
	{
		// Token: 0x060025D9 RID: 9689 RVA: 0x005599F0 File Offset: 0x00557BF0
		public DrippingShader(Color baseColor, Color liquidColor, float viscosity = 1f)
		{
			this._baseColor = baseColor.ToVector4();
			this._liquidColor = liquidColor.ToVector4();
			this._viscosity = viscosity;
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x00559A1C File Offset: 0x00557C1C
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(this._baseColor, this._liquidColor, (float)Math.Sin((double)(time * 0.5f + canvasPositionOfIndex.X)) * 0.5f + 0.5f);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x00559A80 File Offset: 0x00557C80
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				fragment.GetGridPositionOfIndex(i);
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float num = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * new Vector2(0.7f * this._viscosity, 0.075f) + new Vector2(0f, time * -0.1f * this._viscosity));
				num = Math.Max(0f, 1f - (canvasPositionOfIndex.Y * 4.5f + 0.5f) * num);
				Vector4 vector = this._baseColor;
				vector = Vector4.Lerp(vector, this._liquidColor, num);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04005023 RID: 20515
		private readonly Vector4 _baseColor;

		// Token: 0x04005024 RID: 20516
		private readonly Vector4 _liquidColor;

		// Token: 0x04005025 RID: 20517
		private readonly float _viscosity;
	}
}
