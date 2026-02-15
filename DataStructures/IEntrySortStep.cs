using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
	// Token: 0x02000541 RID: 1345
	public interface IEntrySortStep<T> : IComparer<T>
	{
		// Token: 0x06003754 RID: 14164
		string GetDisplayNameKey();
	}
}
