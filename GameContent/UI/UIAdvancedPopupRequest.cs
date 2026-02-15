using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.UI
{
	// Token: 0x0200036B RID: 875
	public struct UIAdvancedPopupRequest
	{
		// Token: 0x0400519B RID: 20891
		public UIPopupTextContext Context;

		// Token: 0x0400519C RID: 20892
		public UIPopupTextAlignment Alignment;

		// Token: 0x0400519D RID: 20893
		public string Text;

		// Token: 0x0400519E RID: 20894
		public Color Color;

		// Token: 0x0400519F RID: 20895
		public int DurationInFrames;

		// Token: 0x040051A0 RID: 20896
		public Vector2 Position;

		// Token: 0x040051A1 RID: 20897
		public Vector2 Velocity;
	}
}
