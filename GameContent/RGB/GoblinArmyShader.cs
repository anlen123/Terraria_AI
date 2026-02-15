using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002A4 RID: 676
	public class GoblinArmyShader : ChromaShader
	{
		// Token: 0x0600255A RID: 9562 RVA: 0x00555955 File Offset: 0x00553B55
		public GoblinArmyShader(Color primaryColor, Color secondaryColor)
		{
			this._primaryColor = primaryColor.ToVector4();
			this._secondaryColor = secondaryColor.ToVector4();
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x00555978 File Offset: 0x00553B78
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			time *= 0.5f;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				canvasPositionOfIndex.Y = 1f;
				float num = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.3f + new Vector2(12.5f, time * 0.2f));
				num = Math.Max(0f, 1f - num * num * 4f * num);
				num = MathHelper.Clamp(num, 0f, 1f);
				Vector4 vector = Vector4.Lerp(this._primaryColor, this._secondaryColor, num);
				vector = Vector4.Lerp(vector, Vector4.One, num * num);
				Vector4 vector2 = Vector4.Lerp(new Vector4(0f, 0f, 0f, 1f), vector, num);
				fragment.SetColor(i, vector2);
			}
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x00555A5C File Offset: 0x00553C5C
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float num = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.3f + new Vector2(12.5f, time * 0.2f));
				num = Math.Max(0f, 1f - num * num * 4f * num * (1.2f - canvasPositionOfIndex.Y)) * canvasPositionOfIndex.Y * canvasPositionOfIndex.Y;
				num = MathHelper.Clamp(num, 0f, 1f);
				Vector4 vector = Vector4.Lerp(this._primaryColor, this._secondaryColor, num);
				vector = Vector4.Lerp(vector, Vector4.One, num * num * num);
				Vector4 vector2 = Vector4.Lerp(new Vector4(0f, 0f, 0f, 1f), vector, num);
				fragment.SetColor(i, vector2);
			}
		}

		// Token: 0x04004FAC RID: 20396
		private readonly Vector4 _primaryColor;

		// Token: 0x04004FAD RID: 20397
		private readonly Vector4 _secondaryColor;
	}
}
