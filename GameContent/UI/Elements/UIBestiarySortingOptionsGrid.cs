using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003D4 RID: 980
	public class UIBestiarySortingOptionsGrid : UIPanel
	{
		// Token: 0x14000050 RID: 80
		// (add) Token: 0x06002DAE RID: 11694 RVA: 0x005A4DF8 File Offset: 0x005A2FF8
		// (remove) Token: 0x06002DAF RID: 11695 RVA: 0x005A4E30 File Offset: 0x005A3030
		public event Action OnClickingOption;

		// Token: 0x06002DB0 RID: 11696 RVA: 0x005A4E68 File Offset: 0x005A3068
		public UIBestiarySortingOptionsGrid(EntrySorter<BestiaryEntry, IBestiarySortStep> sorter)
		{
			this._sorter = sorter;
			this._buttonsBySorting = new List<GroupOptionButton<int>>();
			this.Width = new StyleDimension(0f, 1f);
			this.Height = new StyleDimension(0f, 1f);
			this.BackgroundColor = new Color(35, 40, 83) * 0.5f;
			this.BorderColor = new Color(35, 40, 83) * 0.5f;
			this.IgnoresMouseInteraction = false;
			base.SetPadding(0f);
			this.BuildGrid();
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x005A4F0C File Offset: 0x005A310C
		private void BuildGrid()
		{
			int num = 2;
			int num2 = 26 + num;
			int num3 = 0;
			for (int i = 0; i < this._sorter.Steps.Count; i++)
			{
				if (!this._sorter.Steps[i].HiddenFromSortOptions)
				{
					num3++;
				}
			}
			UIPanel uipanel = new UIPanel
			{
				Width = new StyleDimension(126f, 0f),
				Height = new StyleDimension((float)(num3 * num2 + 5 + 3), 0f),
				HAlign = 1f,
				VAlign = 0f,
				Left = new StyleDimension(-118f, 0f),
				Top = new StyleDimension(0f, 0f)
			};
			uipanel.BorderColor = new Color(89, 116, 213, 255) * 0.9f;
			uipanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
			uipanel.SetPadding(0f);
			base.Append(uipanel);
			int num4 = 0;
			for (int j = 0; j < this._sorter.Steps.Count; j++)
			{
				IBestiarySortStep bestiarySortStep = this._sorter.Steps[j];
				if (!bestiarySortStep.HiddenFromSortOptions)
				{
					GroupOptionButton<int> groupOptionButton = new GroupOptionButton<int>(j, Language.GetText(bestiarySortStep.GetDisplayNameKey()), null, Color.White, null, 0.8f, 0.5f, 10f)
					{
						Width = new StyleDimension(114f, 0f),
						Height = new StyleDimension((float)(num2 - num), 0f),
						HAlign = 0.5f,
						Top = new StyleDimension((float)(5 + num2 * num4), 0f)
					};
					groupOptionButton.ShowHighlightWhenSelected = false;
					groupOptionButton.OnLeftClick += this.ClickOption;
					groupOptionButton.SetSnapPoint("SortSteps", num4, null, null);
					uipanel.Append(groupOptionButton);
					this._buttonsBySorting.Add(groupOptionButton);
					num4++;
				}
			}
			foreach (GroupOptionButton<int> groupOptionButton2 in this._buttonsBySorting)
			{
				groupOptionButton2.SetCurrentOption(-1);
			}
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x005A5180 File Offset: 0x005A3380
		private void ClickOption(UIMouseEvent evt, UIElement listeningElement)
		{
			int num = ((GroupOptionButton<int>)listeningElement).OptionValue;
			if (num == this._currentSelected)
			{
				num = this._defaultStepIndex;
			}
			foreach (GroupOptionButton<int> groupOptionButton in this._buttonsBySorting)
			{
				bool flag = num == groupOptionButton.OptionValue;
				groupOptionButton.SetCurrentOption(flag ? num : -1);
				if (flag)
				{
					groupOptionButton.SetColor(new Color(152, 175, 235), 1f);
				}
				else
				{
					groupOptionButton.SetColor(Colors.InventoryDefaultColor, 0.7f);
				}
			}
			this._currentSelected = num;
			this._sorter.SetPrioritizedStepIndex(num);
			if (this.OnClickingOption != null)
			{
				this.OnClickingOption();
			}
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x005A525C File Offset: 0x005A345C
		public void GetEntriesToShow(out int maxEntriesWidth, out int maxEntriesHeight, out int maxEntriesToHave)
		{
			maxEntriesWidth = 1;
			maxEntriesHeight = this._buttonsBySorting.Count;
			maxEntriesToHave = this._buttonsBySorting.Count;
		}

		// Token: 0x040054D3 RID: 21715
		private EntrySorter<BestiaryEntry, IBestiarySortStep> _sorter;

		// Token: 0x040054D4 RID: 21716
		private List<GroupOptionButton<int>> _buttonsBySorting;

		// Token: 0x040054D5 RID: 21717
		private int _currentSelected = -1;

		// Token: 0x040054D6 RID: 21718
		private int _defaultStepIndex;
	}
}
