using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003DA RID: 986
	public class UIBestiaryEntryInfoPage : UIPanel
	{
		// Token: 0x06002DDA RID: 11738 RVA: 0x005A67A4 File Offset: 0x005A49A4
		public UIBestiaryEntryInfoPage()
		{
			this.Width.Set(230f, 0f);
			this.Height.Set(0f, 1f);
			base.SetPadding(0f);
			this.BorderColor = new Color(89, 116, 213, 255);
			this.BackgroundColor = new Color(73, 94, 171);
			UIList uilist = new UIList
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f)
			};
			uilist.SetPadding(2f);
			uilist.PaddingBottom = 4f;
			uilist.PaddingTop = 4f;
			base.Append(uilist);
			this._list = uilist;
			uilist.ListPadding = 4f;
			uilist.ManualSortMethod = new Action<List<UIElement>>(this.ManualIfnoSortingMethod);
			UIScrollbar uiscrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue);
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(-20f, 1f);
			uiscrollbar.HAlign = 1f;
			uiscrollbar.VAlign = 0.5f;
			uiscrollbar.Left.Set(-6f, 0f);
			this._scrollbar = uiscrollbar;
			this._list.SetScrollbar(this._scrollbar);
			this.CheckScrollBar();
			this.AppendBorderOverEverything();
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x005A6910 File Offset: 0x005A4B10
		public void UpdateScrollbar(int scrollWheelValue)
		{
			if (this._scrollbar != null)
			{
				this._scrollbar.ViewPosition -= (float)scrollWheelValue;
			}
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x005A6930 File Offset: 0x005A4B30
		private void AppendBorderOverEverything()
		{
			UIPanel uipanel = new UIPanel
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(0f, 1f),
				IgnoresMouseInteraction = true
			};
			uipanel.BorderColor = new Color(89, 116, 213, 255);
			uipanel.BackgroundColor = Color.Transparent;
			base.Append(uipanel);
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x00009E06 File Offset: 0x00008006
		private void ManualIfnoSortingMethod(List<UIElement> list)
		{
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x005A699F File Offset: 0x005A4B9F
		public override void Recalculate()
		{
			base.Recalculate();
			this.CheckScrollBar();
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x005A69B0 File Offset: 0x005A4BB0
		private void CheckScrollBar()
		{
			if (this._scrollbar != null)
			{
				bool flag = this._scrollbar.CanScroll;
				flag = true;
				if (this._isScrollbarAttached && !flag)
				{
					base.RemoveChild(this._scrollbar);
					this._isScrollbarAttached = false;
					this._list.Width.Set(0f, 1f);
					return;
				}
				if (!this._isScrollbarAttached && flag)
				{
					base.Append(this._scrollbar);
					this._isScrollbarAttached = true;
					this._list.Width.Set(-20f, 1f);
				}
			}
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x005A6A49 File Offset: 0x005A4C49
		public void FillInfoForEntry(BestiaryEntry entry, ExtraBestiaryInfoPageInformation extraInfo)
		{
			this._list.Clear();
			if (entry == null)
			{
				return;
			}
			this.AddInfoToList(entry, extraInfo);
			this.Recalculate();
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x005A6A68 File Offset: 0x005A4C68
		private BestiaryUICollectionInfo GetUICollectionInfo(BestiaryEntry entry, ExtraBestiaryInfoPageInformation extraInfo)
		{
			IBestiaryUICollectionInfoProvider uiinfoProvider = entry.UIInfoProvider;
			BestiaryUICollectionInfo result;
			if (uiinfoProvider != null)
			{
				result = uiinfoProvider.GetEntryUICollectionInfo();
			}
			else
			{
				result = default(BestiaryUICollectionInfo);
			}
			result.OwnerEntry = entry;
			return result;
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x005A6A9C File Offset: 0x005A4C9C
		private void AddInfoToList(BestiaryEntry entry, ExtraBestiaryInfoPageInformation extraInfo)
		{
			BestiaryUICollectionInfo uicollectionInfo = this.GetUICollectionInfo(entry, extraInfo);
			IEnumerable<IGrouping<UIBestiaryEntryInfoPage.BestiaryInfoCategory, IBestiaryInfoElement>> enumerable = from x in new List<IBestiaryInfoElement>(entry.Info).GroupBy(new Func<IBestiaryInfoElement, UIBestiaryEntryInfoPage.BestiaryInfoCategory>(this.GetBestiaryInfoCategory))
			orderby x.Key
			select x;
			UIElement item = null;
			foreach (IGrouping<UIBestiaryEntryInfoPage.BestiaryInfoCategory, IBestiaryInfoElement> source in enumerable)
			{
				if (source.Count<IBestiaryInfoElement>() != 0)
				{
					bool flag = false;
					foreach (IBestiaryInfoElement bestiaryInfoElement in source.OrderByDescending(new Func<IBestiaryInfoElement, float>(this.GetIndividualElementPriority)))
					{
						UIElement uielement = bestiaryInfoElement.ProvideUIElement(uicollectionInfo);
						if (uielement != null)
						{
							this._list.Add(uielement);
							flag = true;
						}
					}
					if (flag)
					{
						UIHorizontalSeparator uihorizontalSeparator = new UIHorizontalSeparator(2, true)
						{
							Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
							Color = new Color(89, 116, 213, 255) * 0.9f
						};
						this._list.Add(uihorizontalSeparator);
						item = uihorizontalSeparator;
					}
				}
			}
			this._list.Remove(item);
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x005A6C00 File Offset: 0x005A4E00
		private float GetIndividualElementPriority(IBestiaryInfoElement element)
		{
			IBestiaryPrioritizedElement bestiaryPrioritizedElement = element as IBestiaryPrioritizedElement;
			if (bestiaryPrioritizedElement != null)
			{
				return bestiaryPrioritizedElement.OrderPriority;
			}
			return 0f;
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x005A6C24 File Offset: 0x005A4E24
		private UIBestiaryEntryInfoPage.BestiaryInfoCategory GetBestiaryInfoCategory(IBestiaryInfoElement element)
		{
			if (element is NPCPortraitInfoElement)
			{
				return UIBestiaryEntryInfoPage.BestiaryInfoCategory.Portrait;
			}
			if (element is FlavorTextBestiaryInfoElement)
			{
				return UIBestiaryEntryInfoPage.BestiaryInfoCategory.FlavorText;
			}
			if (element is NamePlateInfoElement)
			{
				return UIBestiaryEntryInfoPage.BestiaryInfoCategory.Nameplate;
			}
			if (element is ItemFromCatchingNPCBestiaryInfoElement)
			{
				return UIBestiaryEntryInfoPage.BestiaryInfoCategory.ItemsFromCatchingNPC;
			}
			if (element is ItemDropBestiaryInfoElement)
			{
				return UIBestiaryEntryInfoPage.BestiaryInfoCategory.ItemsFromDrops;
			}
			if (element is NPCStatsReportInfoElement || element is NPCKillCounterInfoElement)
			{
				return UIBestiaryEntryInfoPage.BestiaryInfoCategory.Stats;
			}
			return UIBestiaryEntryInfoPage.BestiaryInfoCategory.Misc;
		}

		// Token: 0x040054EE RID: 21742
		private UIList _list;

		// Token: 0x040054EF RID: 21743
		private UIScrollbar _scrollbar;

		// Token: 0x040054F0 RID: 21744
		private bool _isScrollbarAttached;

		// Token: 0x0200092C RID: 2348
		private enum BestiaryInfoCategory
		{
			// Token: 0x040074CD RID: 29901
			Nameplate,
			// Token: 0x040074CE RID: 29902
			Portrait,
			// Token: 0x040074CF RID: 29903
			FlavorText,
			// Token: 0x040074D0 RID: 29904
			Stats,
			// Token: 0x040074D1 RID: 29905
			ItemsFromCatchingNPC,
			// Token: 0x040074D2 RID: 29906
			ItemsFromDrops,
			// Token: 0x040074D3 RID: 29907
			Misc
		}
	}
}
