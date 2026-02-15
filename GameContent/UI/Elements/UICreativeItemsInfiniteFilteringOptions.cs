using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003D6 RID: 982
	public class UICreativeItemsInfiniteFilteringOptions : UIElement
	{
		// Token: 0x14000052 RID: 82
		// (add) Token: 0x06002DC1 RID: 11713 RVA: 0x005A5B0C File Offset: 0x005A3D0C
		// (remove) Token: 0x06002DC2 RID: 11714 RVA: 0x005A5B44 File Offset: 0x005A3D44
		public event Action OnClickingOption;

		// Token: 0x06002DC3 RID: 11715 RVA: 0x005A5B7C File Offset: 0x005A3D7C
		public UICreativeItemsInfiniteFilteringOptions(EntryFilterer<Item, IItemEntryFilter> filterer, string snapPointsName, UICreativeItemsInfiniteFilteringOptions.ColorTheme theme = UICreativeItemsInfiniteFilteringOptions.ColorTheme.Blue)
		{
			this._theme = theme;
			this._filterer = filterer;
			int num = 40;
			int count = this._filterer.AvailableFilters.Count;
			int num2 = num * count;
			this.Height = new StyleDimension((float)num, 0f);
			this.Width = new StyleDimension((float)num2, 0f);
			this.Top = new StyleDimension(4f, 0f);
			base.SetPadding(0f);
			string text = "Images/UI/Creative/Infinite_Tabs_B";
			if (this._theme == UICreativeItemsInfiniteFilteringOptions.ColorTheme.Cyan)
			{
				text = "Images/UI/Creative/Infinite_Tabs_B_2";
			}
			Asset<Texture2D> asset = Main.Assets.Request<Texture2D>(text, 1);
			for (int i = 0; i < this._filterer.AvailableFilters.Count; i++)
			{
				IItemEntryFilter itemEntryFilter = this._filterer.AvailableFilters[i];
				asset.Frame(2, 4, 0, 0, 0, 0).OffsetSize(-2, -2);
				UIImageFramed uiimageFramed = new UIImageFramed(asset, asset.Frame(2, 4, 0, 0, 0, 0).OffsetSize(-2, -2));
				uiimageFramed.Left.Set((float)(num * i), 0f);
				uiimageFramed.OnLeftClick += this.singleFilterButtonClick;
				uiimageFramed.OnMouseOver += this.button_OnMouseOver;
				uiimageFramed.SetPadding(0f);
				uiimageFramed.SetSnapPoint(snapPointsName, i, null, null);
				this.AddOnHover(itemEntryFilter, uiimageFramed, i);
				UIElement image = itemEntryFilter.GetImage();
				image.IgnoresMouseInteraction = true;
				image.Left = new StyleDimension(6f, 0f);
				image.HAlign = 0f;
				uiimageFramed.Append(image);
				this._filtersByButtons[uiimageFramed] = itemEntryFilter;
				this._iconsByButtons[uiimageFramed] = image;
				base.Append(uiimageFramed);
				this.UpdateVisuals(uiimageFramed, i);
			}
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x00592D7A File Offset: 0x00590F7A
		private void button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x005A5D80 File Offset: 0x005A3F80
		private void singleFilterButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			UIImageFramed uiimageFramed = evt.Target as UIImageFramed;
			if (uiimageFramed == null)
			{
				return;
			}
			IItemEntryFilter item;
			if (!this._filtersByButtons.TryGetValue(uiimageFramed, out item))
			{
				return;
			}
			int num = this._filterer.AvailableFilters.IndexOf(item);
			if (num == -1)
			{
				return;
			}
			if (!this._filterer.ActiveFilters.Contains(item))
			{
				this._filterer.ActiveFilters.Clear();
			}
			this._filterer.ToggleFilter(num);
			this.UpdateVisuals(uiimageFramed, num);
			if (this.OnClickingOption != null)
			{
				this.OnClickingOption();
			}
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x005A5E10 File Offset: 0x005A4010
		private void UpdateVisuals(UIImageFramed button, int indexOfFilter)
		{
			bool flag = this._filterer.IsFilterActive(indexOfFilter);
			bool isMouseHovering = button.IsMouseHovering;
			int frameX = flag.ToInt();
			int frameY = flag.ToInt() * 2 + isMouseHovering.ToInt();
			button.SetFrame(2, 4, frameX, frameY, -2, -2);
			IColorable colorable = this._iconsByButtons[button] as IColorable;
			if (colorable != null)
			{
				colorable.Color = (flag ? Color.White : (Color.White * 0.5f));
			}
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x005A5E90 File Offset: 0x005A4090
		private void AddOnHover(IItemEntryFilter filter, UIElement button, int indexOfFilter)
		{
			button.OnUpdate += delegate(UIElement element)
			{
				this.ShowButtonName(element, filter, indexOfFilter);
			};
			button.OnUpdate += delegate(UIElement element)
			{
				this.UpdateVisuals(button as UIImageFramed, indexOfFilter);
			};
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x005A5EF0 File Offset: 0x005A40F0
		private void ShowButtonName(UIElement element, IItemEntryFilter number, int indexOfFilter)
		{
			if (!element.IsMouseHovering)
			{
				return;
			}
			string textValue = Language.GetTextValue(number.GetDisplayNameKey());
			Main.instance.MouseTextNoOverride(textValue, 0, 0, -1, -1, -1, -1, 0);
		}

		// Token: 0x040054DE RID: 21726
		private EntryFilterer<Item, IItemEntryFilter> _filterer;

		// Token: 0x040054DF RID: 21727
		private Dictionary<UIImageFramed, IItemEntryFilter> _filtersByButtons = new Dictionary<UIImageFramed, IItemEntryFilter>();

		// Token: 0x040054E0 RID: 21728
		private Dictionary<UIImageFramed, UIElement> _iconsByButtons = new Dictionary<UIImageFramed, UIElement>();

		// Token: 0x040054E2 RID: 21730
		private const int barFramesX = 2;

		// Token: 0x040054E3 RID: 21731
		private const int barFramesY = 4;

		// Token: 0x040054E4 RID: 21732
		private UICreativeItemsInfiniteFilteringOptions.ColorTheme _theme;

		// Token: 0x0200092A RID: 2346
		public enum ColorTheme
		{
			// Token: 0x040074C6 RID: 29894
			Blue,
			// Token: 0x040074C7 RID: 29895
			Cyan
		}
	}
}
