using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000542 RID: 1346
	public interface ISearchFilter<T> : IEntryFilter<T>
	{
		// Token: 0x06003755 RID: 14165
		void SetSearch(string searchText);
	}
}
