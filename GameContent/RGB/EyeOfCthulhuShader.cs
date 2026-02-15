using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002A1 RID: 673
	public class EyeOfCthulhuShader : ChromaShader
	{
		// Token: 0x06002553 RID: 9555 RVA: 0x00555510 File Offset: 0x00553710
		public EyeOfCthulhuShader(Color eyeColor, Color veinColor, Color backgroundColor)
		{
			this._eyeColor = eyeColor.ToVector4();
			this._veinColor = veinColor.ToVector4();
			this._backgroundColor = backgroundColor.ToVector4();
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x00555540 File Offset: 0x00553740
		[RgbProcessor(new EffectDetailLevel[]
		{
			0
		})]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(this._veinColor, this._eyeColor, (float)Math.Sin((double)(time + canvasPositionOfIndex.X * 4f)) * 0.5f + 0.5f);
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x005555A4 File Offset: 0x005537A4
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			if (device.Type != null && device.Type != 6)
			{
				this.ProcessLowDetail(device, fragment, quality, time);
				return;
			}
			float num = time * 0.2f % 2f;
			int num2 = 1;
			if (num > 1f)
			{
				num = 2f - num;
				num2 = -1;
			}
			Vector2 value = new Vector2(num * 7f - 3.5f, 0f) + fragment.CanvasCenter;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = this._backgroundColor;
				Vector2 vector2 = canvasPositionOfIndex - value;
				float num3 = vector2.Length();
				if (num3 < 0.5f)
				{
					float amount = 1f - MathHelper.Clamp((num3 - 0.5f + 0.2f) / 0.2f, 0f, 1f);
					float num4 = MathHelper.Clamp((vector2.X + 0.5f - 0.2f) / 0.6f, 0f, 1f);
					if (num2 == 1)
					{
						num4 = 1f - num4;
					}
					Vector4 value2 = Vector4.Lerp(this._eyeColor, this._veinColor, num4);
					vector = Vector4.Lerp(vector, value2, amount);
				}
				fragment.SetColor(i, vector);
			}
		}

		// Token: 0x04004FA1 RID: 20385
		private readonly Vector4 _eyeColor;

		// Token: 0x04004FA2 RID: 20386
		private readonly Vector4 _veinColor;

		// Token: 0x04004FA3 RID: 20387
		private readonly Vector4 _backgroundColor;
	}
}
