using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003CD RID: 973
	public class UICreativeItemGrid : UIDynamicItemCollection<Item>
	{
		// Token: 0x06002D88 RID: 11656 RVA: 0x001FC399 File Offset: 0x001FA599
		protected override Item GetItem(Item entry)
		{
			return entry;
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x005A412C File Offset: 0x005A232C
		protected override void DrawSlot(SpriteBatch spriteBatch, Item item, Vector2 pos, bool hovering)
		{
			ItemsSacrificedUnlocksTracker itemSacrifices = Main.LocalPlayerCreativeTracker.ItemSacrifices;
			int context = itemSacrifices.IsFullyResearched(item.type) ? 29 : 34;
			if (hovering)
			{
				this._item.SetDefaults(item.type, null);
				item = this._item;
				Main.LocalPlayer.mouseInterface = true;
				ItemSlot.Handle(ref item, context, true);
				itemSacrifices.ClearNewlyResearchedStatus(item.type);
			}
			UILinkPointNavigator.Shortcuts.ItemSlotShouldHighlightAsSelected = hovering;
			item.newAndShiny = itemSacrifices.IsNewlyResearched(item.type);
			ItemSlot.Draw(spriteBatch, ref item, context, pos, default(Color));
			item.newAndShiny = false;
		}

		// Token: 0x040054BF RID: 21695
		private Item _item = new Item();
	}
}
