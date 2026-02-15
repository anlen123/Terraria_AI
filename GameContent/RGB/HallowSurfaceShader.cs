using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002BA RID: 698
	public class HallowSurfaceShader : ChromaShader
	{
		// Token: 0x0600259F RID: 9631 RVA: 0x005580CB File Offset: 0x005562CB
		public override void Update(float elapsedTime)
		{
			this._lightColor = Main.ColorOfTheSkies.ToVector4() * 0.75f + Vector4.One * 0.25f;
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x005580FC File Offset: 0x005562FC
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(this._skyColor, this._groundColor, (float)Math.Sin((double)(time + canvasPositionOfIndex.X)) * 0.5f + 0.5f);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x00558158 File Offset: 0x00556358
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 vector = this._skyColor * this._lightColor;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				float num = NoiseHelper.GetDynamicNoise(gridPositionOfIndex.X, gridPositionOfIndex.Y, time / 20f);
				num = Math.Max(0f, 1f - num * 5f);
				Vector4 vector2 = vector;
				if ((gridPositionOfIndex.X * 100 + gridPositionOfIndex.Y) % 2 == 0)
				{
					vector2 = Vector4.Lerp(vector2, this._yellowFlowerColor, num);
				}
				else
				{
					vector2 = Vector4.Lerp(vector2, this._pinkFlowerColor, num);
				}
				float num2 = (float)Math.Sin((double)canvasPositionOfIndex.X) * 0.3f + 0.7f;
				if (canvasPositionOfIndex.Y > num2)
				{
					vector2 = this._groundColor;
				}
				fragment.SetColor(i, vector2);
			}
		}

		// Token: 0x04004FF3 RID: 20467
		private readonly Vector4 _skyColor = new Color(150, 220, 220).ToVector4();

		// Token: 0x04004FF4 RID: 20468
		private readonly Vector4 _groundColor = new Vector4(1f, 0.2f, 0.25f, 1f);

		// Token: 0x04004FF5 RID: 20469
		private readonly Vector4 _pinkFlowerColor = new Vector4(1f, 0.2f, 0.25f, 1f);

		// Token: 0x04004FF6 RID: 20470
		private readonly Vector4 _yellowFlowerColor = new Vector4(1f, 1f, 0f, 1f);

		// Token: 0x04004FF7 RID: 20471
		private Vector4 _lightColor;
	}
}
