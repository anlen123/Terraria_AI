using System;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000331 RID: 817
	public interface IBestiarySortStep : IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
	{
		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060027FA RID: 10234
		bool HiddenFromSortOptions { get; }
	}
}
