using System;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x0200058E RID: 1422
	public class ColorSlidersSet
	{
		// Token: 0x0600382A RID: 14378 RVA: 0x006305F8 File Offset: 0x0062E7F8
		public void SetHSL(Color color)
		{
			Vector3 vector = Main.rgbToHsl(color);
			this.Hue = vector.X;
			this.Saturation = vector.Y;
			this.Luminance = vector.Z;
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x00630630 File Offset: 0x0062E830
		public void SetHSL(Vector3 vector)
		{
			this.Hue = vector.X;
			this.Saturation = vector.Y;
			this.Luminance = vector.Z;
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x00630658 File Offset: 0x0062E858
		public Color GetColor()
		{
			Color result = Main.hslToRgb(this.Hue, this.Saturation, this.Luminance, byte.MaxValue);
			result.A = (byte)(this.Alpha * 255f);
			return result;
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x00630697 File Offset: 0x0062E897
		public Vector3 GetHSLVector()
		{
			return new Vector3(this.Hue, this.Saturation, this.Luminance);
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x006306B0 File Offset: 0x0062E8B0
		public void ApplyToMainLegacyBars()
		{
			Main.hBar = this.Hue;
			Main.sBar = this.Saturation;
			Main.lBar = this.Luminance;
			Main.aBar = this.Alpha;
		}

		// Token: 0x04005C39 RID: 23609
		public float Hue;

		// Token: 0x04005C3A RID: 23610
		public float Saturation;

		// Token: 0x04005C3B RID: 23611
		public float Luminance;

		// Token: 0x04005C3C RID: 23612
		public float Alpha = 1f;
	}
}
