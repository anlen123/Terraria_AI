using System;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200035D RID: 861
	public class NPCNetIdBestiaryInfoElement : IBestiaryInfoElement, IBestiaryEntryDisplayIndex
	{
		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x060028A8 RID: 10408 RVA: 0x00573203 File Offset: 0x00571403
		// (set) Token: 0x060028A9 RID: 10409 RVA: 0x0057320B File Offset: 0x0057140B
		public int NetId { get; private set; }

		// Token: 0x060028AA RID: 10410 RVA: 0x00573214 File Offset: 0x00571414
		public NPCNetIdBestiaryInfoElement(int npcNetId)
		{
			this.NetId = npcNetId;
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000762F3 File Offset: 0x000744F3
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x00573223 File Offset: 0x00571423
		public int BestiaryDisplayIndex
		{
			get
			{
				return ContentSamples.NpcBestiarySortingId[this.NetId];
			}
		}
	}
}
