using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003D5 RID: 981
	public class UIBestiaryFilteringOptionsGrid : UIPanel
	{
		// Token: 0x14000051 RID: 81
		// (add) Token: 0x06002DB4 RID: 11700 RVA: 0x005A527C File Offset: 0x005A347C
		// (remove) Token: 0x06002DB5 RID: 11701 RVA: 0x005A52B4 File Offset: 0x005A34B4
		public event Action OnClickingOption;

		// Token: 0x06002DB6 RID: 11702 RVA: 0x005A52EC File Offset: 0x005A34EC
		public UIBestiaryFilteringOptionsGrid(EntryFilterer<BestiaryEntry, IBestiaryEntryFilter> filterer)
		{
			this._filterer = filterer;
			this._filterButtons = new List<GroupOptionButton<int>>();
			this._areFiltersAvailable = new List<bool>();
			this._filterAvailabilityTests = new List<List<BestiaryEntry>>();
			this.Width = new StyleDimension(0f, 1f);
			this.Height = new StyleDimension(0f, 1f);
			this.BackgroundColor = new Color(35, 40, 83) * 0.5f;
			this.BorderColor = new Color(35, 40, 83) * 0.5f;
			this.IgnoresMouseInteraction = false;
			base.SetPadding(0f);
			this.BuildContainer();
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x005A53A0 File Offset: 0x005A35A0
		private void BuildContainer()
		{
			int num;
			int num2;
			int num3;
			int num4;
			int num5;
			float num6;
			float num7;
			int num8;
			this.GetDisplaySettings(out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8);
			UIPanel uipanel = new UIPanel
			{
				Width = new StyleDimension((float)(num5 * num3 + 10), 0f),
				Height = new StyleDimension((float)(num8 * num4 + 10), 0f),
				HAlign = 1f,
				VAlign = 0f,
				Left = new StyleDimension(0f, 0f),
				Top = new StyleDimension(0f, 0f)
			};
			uipanel.BorderColor = new Color(89, 116, 213, 255) * 0.9f;
			uipanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
			uipanel.SetPadding(0f);
			base.Append(uipanel);
			this._container = uipanel;
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x005A549C File Offset: 0x005A369C
		public void SetupAvailabilityTest(List<BestiaryEntry> allAvailableEntries)
		{
			this._filterAvailabilityTests.Clear();
			for (int i = 0; i < this._filterer.AvailableFilters.Count; i++)
			{
				List<BestiaryEntry> list = new List<BestiaryEntry>();
				this._filterAvailabilityTests.Add(list);
				IBestiaryEntryFilter bestiaryEntryFilter = this._filterer.AvailableFilters[i];
				for (int j = 0; j < allAvailableEntries.Count; j++)
				{
					if (bestiaryEntryFilter.FitsFilter(allAvailableEntries[j]))
					{
						list.Add(allAvailableEntries[j]);
					}
				}
			}
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x005A5520 File Offset: 0x005A3720
		public void UpdateAvailability()
		{
			int num;
			int num2;
			int num3;
			int num4;
			int num5;
			float num6;
			float num7;
			int num8;
			this.GetDisplaySettings(out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8);
			this._container.RemoveAllChildren();
			this._filterButtons.Clear();
			this._areFiltersAvailable.Clear();
			int num9 = -1;
			int num10 = -1;
			for (int i = 0; i < this._filterer.AvailableFilters.Count; i++)
			{
				int num11 = i / num5;
				int num12 = i % num5;
				IBestiaryEntryFilter bestiaryEntryFilter = this._filterer.AvailableFilters[i];
				List<BestiaryEntry> entries = this._filterAvailabilityTests[i];
				if (this.GetIsFilterAvailableForEntries(bestiaryEntryFilter, entries))
				{
					GroupOptionButton<int> groupOptionButton = new GroupOptionButton<int>(i, null, null, Color.White, null, 1f, 0.5f, 10f)
					{
						Width = new StyleDimension((float)num, 0f),
						Height = new StyleDimension((float)num2, 0f),
						HAlign = 0f,
						VAlign = 0f,
						Top = new StyleDimension(num7 + (float)(num11 * num4), 0f),
						Left = new StyleDimension(num6 + (float)(num12 * num3), 0f)
					};
					groupOptionButton.OnLeftClick += this.ClickOption;
					groupOptionButton.SetSnapPoint("Filters", i, null, null);
					groupOptionButton.ShowHighlightWhenSelected = false;
					this.AddOnHover(bestiaryEntryFilter, groupOptionButton);
					this._container.Append(groupOptionButton);
					UIElement image = bestiaryEntryFilter.GetImage();
					if (image != null)
					{
						image.Left = new StyleDimension((float)num9, 0f);
						image.Top = new StyleDimension((float)num10, 0f);
						groupOptionButton.Append(image);
					}
					this._filterButtons.Add(groupOptionButton);
				}
				else
				{
					this._filterer.ActiveFilters.Remove(bestiaryEntryFilter);
					GroupOptionButton<int> groupOptionButton2 = new GroupOptionButton<int>(-2, null, null, Color.White, null, 1f, 0.5f, 10f)
					{
						Width = new StyleDimension((float)num, 0f),
						Height = new StyleDimension((float)num2, 0f),
						HAlign = 0f,
						VAlign = 0f,
						Top = new StyleDimension(num7 + (float)(num11 * num4), 0f),
						Left = new StyleDimension(num6 + (float)(num12 * num3), 0f),
						FadeFromBlack = 0.5f
					};
					groupOptionButton2.ShowHighlightWhenSelected = false;
					groupOptionButton2.SetPadding(0f);
					groupOptionButton2.SetSnapPoint("Filters", i, null, null);
					Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow", 1);
					UIImageFramed uiimageFramed = new UIImageFramed(asset, asset.Frame(16, 5, 0, 4, 0, 0))
					{
						HAlign = 0.5f,
						VAlign = 0.5f,
						Color = Color.White * 0.2f
					};
					uiimageFramed.Left = new StyleDimension((float)num9, 0f);
					uiimageFramed.Top = new StyleDimension((float)num10, 0f);
					groupOptionButton2.Append(uiimageFramed);
					this._filterButtons.Add(groupOptionButton2);
					this._container.Append(groupOptionButton2);
				}
			}
			this.UpdateButtonSelections();
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x005A587C File Offset: 0x005A3A7C
		public void GetEntriesToShow(out int maxEntriesWidth, out int maxEntriesHeight, out int maxEntriesToHave)
		{
			int num;
			int num2;
			int num3;
			int num4;
			int num5;
			float num6;
			float num7;
			int num8;
			this.GetDisplaySettings(out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8);
			maxEntriesWidth = num5;
			maxEntriesHeight = num8;
			maxEntriesToHave = this._filterer.AvailableFilters.Count;
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x005A58BC File Offset: 0x005A3ABC
		private void GetDisplaySettings(out int widthPerButton, out int heightPerButton, out int widthWithSpacing, out int heightWithSpacing, out int perRow, out float offsetLeft, out float offsetTop, out int howManyRows)
		{
			widthPerButton = 32;
			heightPerButton = 32;
			int num = 2;
			widthWithSpacing = widthPerButton + num;
			heightWithSpacing = heightPerButton + num;
			perRow = (int)Math.Ceiling(Math.Sqrt((double)this._filterer.AvailableFilters.Count));
			perRow = 12;
			howManyRows = (int)Math.Ceiling((double)((float)this._filterer.AvailableFilters.Count / (float)perRow));
			offsetLeft = (float)(perRow * widthWithSpacing - num) * 0.5f;
			offsetTop = (float)(howManyRows * heightWithSpacing - num) * 0.5f;
			offsetLeft = 6f;
			offsetTop = 6f;
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x005A595C File Offset: 0x005A3B5C
		private void UpdateButtonSelections()
		{
			foreach (GroupOptionButton<int> groupOptionButton in this._filterButtons)
			{
				bool flag = this._filterer.IsFilterActive(groupOptionButton.OptionValue);
				groupOptionButton.SetCurrentOption(flag ? groupOptionButton.OptionValue : -1);
				if (flag)
				{
					groupOptionButton.SetColor(new Color(152, 175, 235), 1f);
				}
				else
				{
					groupOptionButton.SetColor(Colors.InventoryDefaultColor, 0.7f);
				}
			}
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x005A5A00 File Offset: 0x005A3C00
		private bool GetIsFilterAvailableForEntries(IBestiaryEntryFilter filter, List<BestiaryEntry> entries)
		{
			bool? forcedDisplay = filter.ForcedDisplay;
			if (forcedDisplay != null)
			{
				return forcedDisplay.Value;
			}
			for (int i = 0; i < entries.Count; i++)
			{
				if (filter.FitsFilter(entries[i]) && entries[i].UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x005A5A64 File Offset: 0x005A3C64
		private void AddOnHover(IBestiaryEntryFilter filter, UIElement button)
		{
			button.OnUpdate += delegate(UIElement element)
			{
				this.ShowButtonName(element, filter);
			};
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x005A5A98 File Offset: 0x005A3C98
		private void ShowButtonName(UIElement element, IBestiaryEntryFilter number)
		{
			if (!element.IsMouseHovering)
			{
				return;
			}
			string textValue = Language.GetTextValue(number.GetDisplayNameKey());
			Main.instance.MouseText(textValue, 0, 0, -1, -1, -1, -1, 0);
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x005A5ACC File Offset: 0x005A3CCC
		private void ClickOption(UIMouseEvent evt, UIElement listeningElement)
		{
			int optionValue = ((GroupOptionButton<int>)listeningElement).OptionValue;
			this._filterer.ToggleFilter(optionValue);
			this.UpdateButtonSelections();
			if (this.OnClickingOption != null)
			{
				this.OnClickingOption();
			}
		}

		// Token: 0x040054D8 RID: 21720
		private EntryFilterer<BestiaryEntry, IBestiaryEntryFilter> _filterer;

		// Token: 0x040054D9 RID: 21721
		private List<GroupOptionButton<int>> _filterButtons;

		// Token: 0x040054DA RID: 21722
		private List<bool> _areFiltersAvailable;

		// Token: 0x040054DB RID: 21723
		private List<List<BestiaryEntry>> _filterAvailabilityTests;

		// Token: 0x040054DD RID: 21725
		private UIElement _container;
	}
}
