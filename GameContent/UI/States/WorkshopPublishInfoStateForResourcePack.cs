using System;
using System.Collections.Generic;
using Terraria.IO;
using Terraria.Social;
using Terraria.Social.Base;
using Terraria.UI;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003A2 RID: 930
	public class WorkshopPublishInfoStateForResourcePack : AWorkshopPublishInfoState<ResourcePack>
	{
		// Token: 0x06002AA0 RID: 10912 RVA: 0x005861A0 File Offset: 0x005843A0
		public WorkshopPublishInfoStateForResourcePack(UIState stateToGoBackTo, ResourcePack resourcePack) : base(stateToGoBackTo, resourcePack)
		{
			this._instructionsTextKey = "Workshop.ResourcePackPublishDescription";
			this._publishedObjectNameDescriptorTexKey = "Workshop.ResourcePackName";
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x005861C0 File Offset: 0x005843C0
		protected override string GetPublishedObjectDisplayName()
		{
			if (this._dataObject == null)
			{
				return "null";
			}
			return this._dataObject.Name;
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x005861DC File Offset: 0x005843DC
		protected override void GoToPublishConfirmation()
		{
			if (SocialAPI.Workshop != null && this._dataObject != null)
			{
				SocialAPI.Workshop.PublishResourcePack(this._dataObject, base.GetPublishSettings());
			}
			Main.menuMode = 888;
			Main.MenuUI.SetState(this._previousUIState);
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x00586228 File Offset: 0x00584428
		protected override List<WorkshopTagOption> GetTagsToShow()
		{
			return SocialAPI.Workshop.SupportedTags.ResourcePackTags;
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x00586239 File Offset: 0x00584439
		protected override bool TryFindingTags(out FoundWorkshopEntryInfo info)
		{
			return SocialAPI.Workshop.TryGetInfoForResourcePack(this._dataObject, out info);
		}
	}
}
