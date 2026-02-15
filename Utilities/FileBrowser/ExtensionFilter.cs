using System;

namespace Terraria.Utilities.FileBrowser
{
	// Token: 0x020000DB RID: 219
	public struct ExtensionFilter
	{
		// Token: 0x0600187A RID: 6266 RVA: 0x004E1EDD File Offset: 0x004E00DD
		public ExtensionFilter(string filterName, params string[] filterExtensions)
		{
			this.Name = filterName;
			this.Extensions = filterExtensions;
		}

		// Token: 0x040012C3 RID: 4803
		public string Name;

		// Token: 0x040012C4 RID: 4804
		public string[] Extensions;
	}
}
