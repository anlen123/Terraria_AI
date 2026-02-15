using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002A5 RID: 677
	public class GolemShader : ChromaShader
	{
		// Token: 0x0600255D RID: 9565 RVA: 0x00555B47 File Offset: 0x00553D47
		public GolemShader(Color glowColor, Color coreColor, Color backgroundColor)
		{
			this._glowColor = glowColor.ToVector4();
			this._coreColor = coreColor.ToVector4();
			this._backgroundColor = backgroundColor.ToVector4();
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x00555B78 File Offset: 0x00553D78
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 value = Vector4.Lerp(this._backgroundColor, this._coreColor, Math.Max(0f, (float)Math.Sin((double)(time * 0.5f))));
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(value, this._glowColor, Math.Max(0f, (float)Math.Sin((double)(canvasPositionOfIndex.X * 2f + time + 101f))));
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x00555C04 File Offset: 0x00553E04
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			float num = 0.5f + (float)Math.Sin((double)(time * 3f)) * 0.1f;
			Vector2 vector = new Vector2(1.6f, 0.5f);
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				ref Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector4 vector2 = this._backgroundColor;
				float num2 = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.Y) * 10f + time * 2f) % 10f - Math.Abs(canvasPositionOfIndex.X - vector.X);
				if (num2 > 0f)
				{
					float amount = Math.Max(0f, 1.2f - num2);
					if (num2 < 0.2f)
					{
						amount = num2 * 5f;
					}
					vector2 = Vector4.Lerp(vector2, this._glowColor, amount);
				}
				float num3 = (canvasPositionOfIndex - vector).Length();
				if (num3 < num)
				{
					float amount2 = 1f - MathHelper.Clamp((num3 - num + 0.1f) / 0.1f, 0f, 1f);
					vector2 = Vector4.Lerp(vector2, this._coreColor, amount2);
				}
				fragment.SetColor(i, vector2);
			}
		}

		// Token: 0x04004FAE RID: 20398
		private readonly Vector4 _glowColor;

		// Token: 0x04004FAF RID: 20399
		private readonly Vector4 _coreColor;

		// Token: 0x04004FB0 RID: 20400
		private readonly Vector4 _backgroundColor;
	}
}
