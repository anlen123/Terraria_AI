using System;
using Terraria.DataStructures;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200032F RID: 815
	public interface IBestiaryEntryFilter : IEntryFilter<BestiaryEntry>
	{
		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060027F9 RID: 10233
		bool? ForcedDisplay { get; }
	}
}
