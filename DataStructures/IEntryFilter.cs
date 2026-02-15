using System;
using Terraria.UI;

namespace Terraria.DataStructures
{
	// Token: 0x02000540 RID: 1344
	public interface IEntryFilter<T>
	{
		// Token: 0x06003751 RID: 14161
		bool FitsFilter(T entry);

		// Token: 0x06003752 RID: 14162
		string GetDisplayNameKey();

		// Token: 0x06003753 RID: 14163
		UIElement GetImage();
	}
}
