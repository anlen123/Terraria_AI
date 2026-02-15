using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003DE RID: 990
	public class UIItemIcon : UIElement
	{
		// Token: 0x06002DF5 RID: 11765 RVA: 0x005A6E7F File Offset: 0x005A507F
		public UIItemIcon(Item item, bool blackedOut)
		{
			this._item = item;
			this.Width.Set(32f, 0f);
			this.Height.Set(32f, 0f);
			this._blackedOut = blackedOut;
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x005A6EC0 File Offset: 0x005A50C0
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			ItemSlot.DrawItemIcon(this._item, 31, spriteBatch, dimensions.Center(), this._item.scale, 32f, this._blackedOut ? Color.Black : Color.White, 1f, false);
		}

		// Token: 0x040054F7 RID: 21751
		private Item _item;

		// Token: 0x040054F8 RID: 21752
		private bool _blackedOut;
	}
}
