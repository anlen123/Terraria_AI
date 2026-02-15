using System;
using Microsoft.Xna.Framework;

namespace Terraria.UI
{
	// Token: 0x020000E5 RID: 229
	public struct Alignment
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x004E4E5A File Offset: 0x004E305A
		public Vector2 OffsetMultiplier
		{
			get
			{
				return new Vector2(this.HorizontalOffsetMultiplier, this.VerticalOffsetMultiplier);
			}
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x004E4E6D File Offset: 0x004E306D
		private Alignment(float horizontal, float vertical)
		{
			this.HorizontalOffsetMultiplier = horizontal;
			this.VerticalOffsetMultiplier = vertical;
		}

		// Token: 0x040012EB RID: 4843
		public static readonly Alignment TopLeft = new Alignment(0f, 0f);

		// Token: 0x040012EC RID: 4844
		public static readonly Alignment Top = new Alignment(0.5f, 0f);

		// Token: 0x040012ED RID: 4845
		public static readonly Alignment TopRight = new Alignment(1f, 0f);

		// Token: 0x040012EE RID: 4846
		public static readonly Alignment Left = new Alignment(0f, 0.5f);

		// Token: 0x040012EF RID: 4847
		public static readonly Alignment Center = new Alignment(0.5f, 0.5f);

		// Token: 0x040012F0 RID: 4848
		public static readonly Alignment Right = new Alignment(1f, 0.5f);

		// Token: 0x040012F1 RID: 4849
		public static readonly Alignment BottomLeft = new Alignment(0f, 1f);

		// Token: 0x040012F2 RID: 4850
		public static readonly Alignment Bottom = new Alignment(0.5f, 1f);

		// Token: 0x040012F3 RID: 4851
		public static readonly Alignment BottomRight = new Alignment(1f, 1f);

		// Token: 0x040012F4 RID: 4852
		public readonly float VerticalOffsetMultiplier;

		// Token: 0x040012F5 RID: 4853
		public readonly float HorizontalOffsetMultiplier;
	}
}
