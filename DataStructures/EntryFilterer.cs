using System;
using System.Collections.Generic;
using Terraria.Localization;

namespace Terraria.DataStructures
{
	// Token: 0x02000543 RID: 1347
	public class EntryFilterer<T, U> where T : new() where U : IEntryFilter<T>
	{
		// Token: 0x06003756 RID: 14166 RVA: 0x0062DE45 File Offset: 0x0062C045
		public EntryFilterer()
		{
			this.AvailableFilters = new List<U>();
			this.ActiveFilters = new List<U>();
			this.AlwaysActiveFilters = new List<U>();
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x0062DE6E File Offset: 0x0062C06E
		public void AddFilters(List<U> filters)
		{
			this.AvailableFilters.AddRange(filters);
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x0062DE7C File Offset: 0x0062C07C
		public bool FitsFilter(T entry)
		{
			if (this._searchFilter != null && !this._searchFilter.FitsFilter(entry))
			{
				return false;
			}
			for (int i = 0; i < this.AlwaysActiveFilters.Count; i++)
			{
				U u = this.AlwaysActiveFilters[i];
				if (!u.FitsFilter(entry))
				{
					return false;
				}
			}
			if (this.ActiveFilters.Count == 0)
			{
				return true;
			}
			for (int j = 0; j < this.ActiveFilters.Count; j++)
			{
				U u = this.ActiveFilters[j];
				if (u.FitsFilter(entry))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x0062DF1C File Offset: 0x0062C11C
		public void ToggleFilter(int filterIndex)
		{
			U item = this.AvailableFilters[filterIndex];
			if (this.ActiveFilters.Contains(item))
			{
				this.ActiveFilters.Remove(item);
				return;
			}
			this.ActiveFilters.Add(item);
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x0062DF60 File Offset: 0x0062C160
		public bool IsFilterActive(int filterIndex)
		{
			if (!this.AvailableFilters.IndexInRange(filterIndex))
			{
				return false;
			}
			U item = this.AvailableFilters[filterIndex];
			return this.ActiveFilters.Contains(item);
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x0062DF96 File Offset: 0x0062C196
		public void SetSearchFilterObject<Z>(Z searchFilter) where Z : ISearchFilter<T>, U
		{
			this._searchFilterFromConstructor = searchFilter;
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x0062DFA4 File Offset: 0x0062C1A4
		public void SetSearchFilter(string searchFilter)
		{
			if (string.IsNullOrWhiteSpace(searchFilter))
			{
				this._searchFilter = null;
				return;
			}
			this._searchFilter = this._searchFilterFromConstructor;
			this._searchFilter.SetSearch(searchFilter);
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x0062DFD0 File Offset: 0x0062C1D0
		public string GetDisplayName()
		{
			object obj = new
			{
				this.ActiveFilters.Count
			};
			return Language.GetTextValueWith("BestiaryInfo.Filters", obj);
		}

		// Token: 0x04005B6D RID: 23405
		public List<U> AvailableFilters;

		// Token: 0x04005B6E RID: 23406
		public List<U> ActiveFilters;

		// Token: 0x04005B6F RID: 23407
		public List<U> AlwaysActiveFilters;

		// Token: 0x04005B70 RID: 23408
		private ISearchFilter<T> _searchFilter;

		// Token: 0x04005B71 RID: 23409
		private ISearchFilter<T> _searchFilterFromConstructor;
	}
}
