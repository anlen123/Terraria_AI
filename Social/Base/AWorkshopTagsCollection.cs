using System;
using System.Collections.Generic;

namespace Terraria.Social.Base
{
	// Token: 0x0200015F RID: 351
	public abstract class AWorkshopTagsCollection
	{
		// Token: 0x06001D61 RID: 7521 RVA: 0x00500CC7 File Offset: 0x004FEEC7
		protected void AddWorldTag(string tagNameKey, string tagInternalName)
		{
			this.WorldTags.Add(new WorkshopTagOption(tagNameKey, tagInternalName));
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x00500CDB File Offset: 0x004FEEDB
		protected void AddResourcePackTag(string tagNameKey, string tagInternalName)
		{
			this.ResourcePackTags.Add(new WorkshopTagOption(tagNameKey, tagInternalName));
		}

		// Token: 0x0400163A RID: 5690
		public readonly List<WorkshopTagOption> WorldTags = new List<WorkshopTagOption>();

		// Token: 0x0400163B RID: 5691
		public readonly List<WorkshopTagOption> ResourcePackTags = new List<WorkshopTagOption>();
	}
}
