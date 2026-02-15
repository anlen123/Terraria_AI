using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000539 RID: 1337
	public class BackgroundVariantSet
	{
		// Token: 0x0600373F RID: 14143 RVA: 0x0062DB0D File Offset: 0x0062BD0D
		public void Clear()
		{
			this.Pure.Clear();
			this.Corrupt.Clear();
			this.Crimson.Clear();
			this.Hallow.Clear();
		}

		// Token: 0x04005B5C RID: 23388
		public BackgroundVariant Pure = new BackgroundVariant();

		// Token: 0x04005B5D RID: 23389
		public BackgroundVariant Corrupt = new BackgroundVariant();

		// Token: 0x04005B5E RID: 23390
		public BackgroundVariant Crimson = new BackgroundVariant();

		// Token: 0x04005B5F RID: 23391
		public BackgroundVariant Hallow = new BackgroundVariant();
	}
}
