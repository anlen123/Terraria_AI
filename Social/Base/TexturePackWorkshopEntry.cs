using System;
using Terraria.IO;

namespace Terraria.Social.Base
{
	// Token: 0x02000158 RID: 344
	public class TexturePackWorkshopEntry : AWorkshopEntry
	{
		// Token: 0x06001D36 RID: 7478 RVA: 0x005009C6 File Offset: 0x004FEBC6
		public static string GetHeaderTextFor(ResourcePack resourcePack, ulong workshopEntryId, string[] tags, WorkshopItemPublicSettingId publicity, string previewImagePath)
		{
			return AWorkshopEntry.CreateHeaderJson("ResourcePack", workshopEntryId, tags, publicity, previewImagePath);
		}
	}
}
