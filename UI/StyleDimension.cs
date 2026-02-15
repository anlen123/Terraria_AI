using System;

namespace Terraria.UI
{
	// Token: 0x020000F3 RID: 243
	public struct StyleDimension
	{
		// Token: 0x06001928 RID: 6440 RVA: 0x004E6E34 File Offset: 0x004E5034
		public StyleDimension(float pixels, float precent)
		{
			this.Pixels = pixels;
			this.Precent = precent;
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x004E6E34 File Offset: 0x004E5034
		public void Set(float pixels, float precent)
		{
			this.Pixels = pixels;
			this.Precent = precent;
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x004E6E44 File Offset: 0x004E5044
		public float GetValue(float containerSize)
		{
			return this.Pixels + this.Precent * containerSize;
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x004E6E55 File Offset: 0x004E5055
		public static StyleDimension FromPixels(float pixels)
		{
			return new StyleDimension(pixels, 0f);
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x004E6E62 File Offset: 0x004E5062
		public static StyleDimension FromPercent(float percent)
		{
			return new StyleDimension(0f, percent);
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x004E6E6F File Offset: 0x004E506F
		public static StyleDimension FromPixelsAndPercent(float pixels, float percent)
		{
			return new StyleDimension(pixels, percent);
		}

		// Token: 0x04001323 RID: 4899
		public static StyleDimension Fill = new StyleDimension(0f, 1f);

		// Token: 0x04001324 RID: 4900
		public static StyleDimension Empty = new StyleDimension(0f, 0f);

		// Token: 0x04001325 RID: 4901
		public float Pixels;

		// Token: 0x04001326 RID: 4902
		public float Precent;
	}
}
