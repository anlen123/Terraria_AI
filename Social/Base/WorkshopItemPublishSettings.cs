using System;
using System.Linq;

namespace Terraria.Social.Base
{
	// Token: 0x0200015C RID: 348
	public class WorkshopItemPublishSettings
	{
		// Token: 0x06001D4A RID: 7498 RVA: 0x00500C28 File Offset: 0x004FEE28
		public string[] GetUsedTagsInternalNames()
		{
			return (from x in this.UsedTags
			select x.InternalNameForAPIs).ToArray<string>();
		}

		// Token: 0x04001631 RID: 5681
		public WorkshopTagOption[] UsedTags = new WorkshopTagOption[0];

		// Token: 0x04001632 RID: 5682
		public WorkshopItemPublicSettingId Publicity;

		// Token: 0x04001633 RID: 5683
		public string PreviewImagePath;
	}
}
