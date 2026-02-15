using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002B5 RID: 693
	public class CorruptSurfaceShader : ChromaShader
	{
		// Token: 0x0600258D RID: 9613 RVA: 0x005576F8 File Offset: 0x005558F8
		public CorruptSurfaceShader(Color color)
		{
			this._baseColor = color.ToVector4();
			this._skyColor = Vector4.Lerp(this._baseColor, Color.DeepSkyBlue.ToVector4(), 0.5f);
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x0055773B File Offset: 0x0055593B
		public CorruptSurfaceShader(Color vineColor, Color skyColor)
		{
			this._baseColor = vineColor.ToVector4();
			this._skyColor = skyColor.ToVector4();
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x0055775D File Offset: 0x0055595D
		public override void Update(float elapsedTime)
		{
			this._lightColor = Main.ColorOfTheSkies.ToVector4() * 0.75f + Vector4.One * 0.25f;
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x00557790 File Offset: 0x00555990
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 value = this._skyColor * this._lightColor;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(this._baseColor, value, (float)Math.Sin((double)(time * 0.5f + canvasPositionOfIndex.X)) * 0.5f + 0.5f);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x00557800 File Offset: 0x00555A00
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 vector = this._skyColor * this._lightColor;
			for (int i = 0; i < fragment.Count; i++)
			{
				ref Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float num = NoiseHelper.GetStaticNoise(gridPositionOfIndex.X);
				num = (num * 10f + time * 0.4f) % 10f;
				float num2 = 1f;
				if (num > 1f)
				{
					num2 = MathHelper.Clamp(1f - (num - 1.4f), 0f, 1f);
					num = 1f;
				}
				float num3 = (float)Math.Sin((double)canvasPositionOfIndex.X) * 0.3f + 0.7f;
				float num4 = num - (1f - canvasPositionOfIndex.Y);
				Vector4 vector2 = vector;
				if (num4 > 0f)
				{
					float num5 = 1f;
					if (num4 < 0.2f)
					{
						num5 = num4 * 5f;
					}
					vector2 = Vector4.Lerp(vector2, this._baseColor, num5 * num2);
				}
				if (canvasPositionOfIndex.Y > num3)
				{
					vector2 = this._baseColor;
				}
				fragment.SetColor(i, vector2);
			}
		}

		// Token: 0x04004FE1 RID: 20449
		private readonly Vector4 _baseColor;

		// Token: 0x04004FE2 RID: 20450
		private readonly Vector4 _skyColor;

		// Token: 0x04004FE3 RID: 20451
		private Vector4 _lightColor;
	}
}
