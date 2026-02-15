using System;
using Terraria.IO;

namespace Terraria.Social.Base
{
	// Token: 0x02000157 RID: 343
	public class WorldWorkshopEntry : AWorkshopEntry
	{
		// Token: 0x06001D34 RID: 7476 RVA: 0x005009AD File Offset: 0x004FEBAD
		public static string GetHeaderTextFor(WorldFileData world, ulong workshopEntryId, string[] tags, WorkshopItemPublicSettingId publicity, string previewImagePath)
		{
			return AWorkshopEntry.CreateHeaderJson("World", workshopEntryId, tags, publicity, previewImagePath);
		}
	}
}
