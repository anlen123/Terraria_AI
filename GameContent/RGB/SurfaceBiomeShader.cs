using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002C4 RID: 708
	public class SurfaceBiomeShader : ChromaShader
	{
		// Token: 0x060025BF RID: 9663 RVA: 0x00558E90 File Offset: 0x00557090
		public SurfaceBiomeShader(Color primaryColor, Color secondaryColor)
		{
			this._primaryColor = primaryColor.ToVector4();
			this._secondaryColor = secondaryColor.ToVector4();
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x00558EB4 File Offset: 0x005570B4
		public override void Update(float elapsedTime)
		{
			this._surfaceColor = Main.ColorOfTheSkies.ToVector4() * 0.75f + Vector4.One * 0.25f;
			if (Main.dayTime)
			{
				float num = (float)(Main.time / 54000.0);
				if (num < 0.25f)
				{
					this._starVisibility = 1f - num / 0.25f;
					return;
				}
				if (num > 0.75f)
				{
					this._starVisibility = (num - 0.75f) / 0.25f;
					return;
				}
			}
			else
			{
				this._starVisibility = 1f;
			}
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x00558F4C File Offset: 0x0055714C
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 value = this._primaryColor * this._surfaceColor;
			Vector4 value2 = this._secondaryColor * this._surfaceColor;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(value, value2, (float)Math.Sin((double)(time * 0.5f + canvasPositionOfIndex.X)) * 0.5f + 0.5f);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x00558FCC File Offset: 0x005571CC
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 value = this._primaryColor * this._surfaceColor;
			Vector4 value2 = this._secondaryColor * this._surfaceColor;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				float amount = (float)Math.Sin((double)(canvasPositionOfIndex.X * 1.5f + canvasPositionOfIndex.Y + time)) * 0.5f + 0.5f;
				Vector4 vector = Vector4.Lerp(value, value2, amount);
				float num = NoiseHelper.GetDynamicNoise(gridPositionOfIndex.X, gridPositionOfIndex.Y, time / 60f);
				num = Math.Max(0f, 1f - num * 20f);
				num *= 1f - this._surfaceColor.X;
				vector = Vector4.Max(vector, new Vector4(num * this._starVisibility));
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x0400500C RID: 20492
		private readonly Vector4 _primaryColor;

		// Token: 0x0400500D RID: 20493
		private readonly Vector4 _secondaryColor;

		// Token: 0x0400500E RID: 20494
		private Vector4 _surfaceColor;

		// Token: 0x0400500F RID: 20495
		private float _starVisibility;
	}
}
