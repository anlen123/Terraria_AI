using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003D3 RID: 979
	public class UIBestiaryEntryGrid : UIElement
	{
		// Token: 0x1400004F RID: 79
		// (add) Token: 0x06002DA2 RID: 11682 RVA: 0x005A4A38 File Offset: 0x005A2C38
		// (remove) Token: 0x06002DA3 RID: 11683 RVA: 0x005A4A70 File Offset: 0x005A2C70
		public event Action OnGridContentsChanged;

		// Token: 0x06002DA4 RID: 11684 RVA: 0x005A4AA8 File Offset: 0x005A2CA8
		public UIBestiaryEntryGrid(List<BestiaryEntry> workingSet, UIElement.MouseEvent clickOnEntryEvent)
		{
			this.Width = new StyleDimension(0f, 1f);
			this.Height = new StyleDimension(0f, 1f);
			this._workingSetEntries = workingSet;
			this._clickOnEntryEvent = clickOnEntryEvent;
			base.SetPadding(0f);
			this.UpdateEntries();
			this.FillBestiarySpaceWithEntries();
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x005A4B0A File Offset: 0x005A2D0A
		public void UpdateEntries()
		{
			this._lastEntry = this._workingSetEntries.Count;
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x005A4B20 File Offset: 0x005A2D20
		public void FillBestiarySpaceWithEntries()
		{
			base.RemoveAllChildren();
			this.UpdateEntries();
			int num;
			int num2;
			int num3;
			this.GetEntriesToShow(out num, out num2, out num3);
			this.FixBestiaryRange(0, num3);
			int atEntryIndex = this._atEntryIndex;
			int num4 = Math.Min(this._lastEntry, atEntryIndex + num3);
			List<BestiaryEntry> list = new List<BestiaryEntry>();
			for (int i = atEntryIndex; i < num4; i++)
			{
				list.Add(this._workingSetEntries[i]);
			}
			int num5 = 0;
			float num6 = 0.5f / (float)num;
			float num7 = 0.5f / (float)num2;
			for (int j = 0; j < num2; j++)
			{
				int num8 = 0;
				while (num8 < num && num5 < list.Count)
				{
					UIElement uielement = new UIBestiaryEntryButton(list[num5], false);
					num5++;
					uielement.OnLeftClick += this._clickOnEntryEvent;
					uielement.VAlign = (uielement.HAlign = 0.5f);
					uielement.Left.Set(0f, (float)num8 / (float)num - 0.5f + num6);
					uielement.Top.Set(0f, (float)j / (float)num2 - 0.5f + num7);
					uielement.SetSnapPoint("Entries", num5, new Vector2?(new Vector2(0.2f, 0.7f)), null);
					base.Append(uielement);
					num8++;
				}
			}
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x005A4C91 File Offset: 0x005A2E91
		public override void Recalculate()
		{
			base.Recalculate();
			this.FillBestiarySpaceWithEntries();
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x005A4CA0 File Offset: 0x005A2EA0
		public void GetEntriesToShow(out int maxEntriesWidth, out int maxEntriesHeight, out int maxEntriesToHave)
		{
			Rectangle rectangle = base.GetDimensions().ToRectangle();
			maxEntriesWidth = rectangle.Width / 72;
			maxEntriesHeight = rectangle.Height / 72;
			int num = 0;
			maxEntriesToHave = maxEntriesWidth * maxEntriesHeight - num;
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x005A4CE0 File Offset: 0x005A2EE0
		public string GetRangeText()
		{
			int num;
			int num2;
			int num3;
			this.GetEntriesToShow(out num, out num2, out num3);
			int atEntryIndex = this._atEntryIndex;
			int num4 = Math.Min(this._lastEntry, atEntryIndex + num3);
			int num5 = Math.Min(atEntryIndex + 1, num4);
			return string.Format("{0}-{1} ({2})", num5, num4, this._lastEntry);
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x005A4D40 File Offset: 0x005A2F40
		public void MakeButtonGoByOffset(UIElement element, int howManyPages)
		{
			element.OnLeftClick += delegate(UIMouseEvent e, UIElement v)
			{
				this.OffsetLibraryByPages(howManyPages);
			};
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x005A4D74 File Offset: 0x005A2F74
		public void OffsetLibraryByPages(int howManyPages)
		{
			int num;
			int num2;
			int num3;
			this.GetEntriesToShow(out num, out num2, out num3);
			this.OffsetLibrary(howManyPages * num3);
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x005A4D98 File Offset: 0x005A2F98
		public void OffsetLibrary(int offset)
		{
			int num;
			int num2;
			int maxEntriesToHave;
			this.GetEntriesToShow(out num, out num2, out maxEntriesToHave);
			this.FixBestiaryRange(offset, maxEntriesToHave);
			this.FillBestiarySpaceWithEntries();
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x005A4DBF File Offset: 0x005A2FBF
		private void FixBestiaryRange(int offset, int maxEntriesToHave)
		{
			this._atEntryIndex = Utils.Clamp<int>(this._atEntryIndex + offset, 0, Math.Max(0, this._lastEntry - maxEntriesToHave));
			if (this.OnGridContentsChanged != null)
			{
				this.OnGridContentsChanged();
			}
		}

		// Token: 0x040054CE RID: 21710
		private List<BestiaryEntry> _workingSetEntries;

		// Token: 0x040054CF RID: 21711
		private UIElement.MouseEvent _clickOnEntryEvent;

		// Token: 0x040054D0 RID: 21712
		private int _atEntryIndex;

		// Token: 0x040054D1 RID: 21713
		private int _lastEntry;
	}
}
