using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003C7 RID: 967
	public class UIItemSlot : UIElement
	{
		// Token: 0x06002D3D RID: 11581 RVA: 0x005A2340 File Offset: 0x005A0540
		public UIItemSlot(Item[] itemArray, int itemIndex, int itemSlotContext)
		{
			this._itemArray = itemArray;
			this._itemIndex = itemIndex;
			this._itemSlotContext = itemSlotContext;
			this.Width = new StyleDimension(48f, 0f);
			this.Height = new StyleDimension(48f, 0f);
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x005A2392 File Offset: 0x005A0592
		private void HandleItemSlotLogic()
		{
			if (!base.IsMouseHovering)
			{
				return;
			}
			Main.LocalPlayer.mouseInterface = true;
			ItemSlot.Handle(this._itemArray, this._itemSlotContext, this._itemIndex, true);
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x005A23C0 File Offset: 0x005A05C0
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			this.HandleItemSlotLogic();
			Item item = this._itemArray[this._itemIndex];
			Vector2 position = base.GetDimensions().Center() + new Vector2(52f, 52f) * -0.5f * Main.inventoryScale;
			ItemSlot.Draw(spriteBatch, ref item, this._itemSlotContext, position, default(Color));
		}

		// Token: 0x0400548B RID: 21643
		private Item[] _itemArray;

		// Token: 0x0400548C RID: 21644
		private int _itemIndex;

		// Token: 0x0400548D RID: 21645
		private int _itemSlotContext;
	}
}
