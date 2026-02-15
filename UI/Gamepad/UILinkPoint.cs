using System;
using Microsoft.Xna.Framework;

namespace Terraria.UI.Gamepad
{
	// Token: 0x02000106 RID: 262
	public class UILinkPoint
	{
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06001A51 RID: 6737 RVA: 0x004F4F8C File Offset: 0x004F318C
		// (set) Token: 0x06001A52 RID: 6738 RVA: 0x004F4F94 File Offset: 0x004F3194
		public int Page { get; private set; }

		// Token: 0x06001A53 RID: 6739 RVA: 0x004F4F9D File Offset: 0x004F319D
		public UILinkPoint(int id, bool enabled, int left, int right, int up, int down)
		{
			this.ID = id;
			this.Enabled = enabled;
			this.Left = left;
			this.Right = right;
			this.Up = up;
			this.Down = down;
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x004F4FD2 File Offset: 0x004F31D2
		public void SetPage(int page)
		{
			this.Page = page;
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x004F4FDB File Offset: 0x004F31DB
		public void Unlink()
		{
			this.Left = -3;
			this.Right = -4;
			this.Up = -1;
			this.Down = -2;
		}

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06001A56 RID: 6742 RVA: 0x004F4FFC File Offset: 0x004F31FC
		// (remove) Token: 0x06001A57 RID: 6743 RVA: 0x004F5034 File Offset: 0x004F3234
		public event Func<string> OnSpecialInteracts;

		// Token: 0x06001A58 RID: 6744 RVA: 0x004F5069 File Offset: 0x004F3269
		public string SpecialInteractions()
		{
			if (this.OnSpecialInteracts != null)
			{
				return this.OnSpecialInteracts();
			}
			return string.Empty;
		}

		// Token: 0x040014C1 RID: 5313
		public int ID;

		// Token: 0x040014C3 RID: 5315
		public bool Enabled;

		// Token: 0x040014C4 RID: 5316
		public Vector2 Position;

		// Token: 0x040014C5 RID: 5317
		public int Left;

		// Token: 0x040014C6 RID: 5318
		public int Right;

		// Token: 0x040014C7 RID: 5319
		public int Up;

		// Token: 0x040014C8 RID: 5320
		public int Down;
	}
}
