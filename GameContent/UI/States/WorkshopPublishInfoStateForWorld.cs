using System;
using System.Collections.Generic;
using Terraria.IO;
using Terraria.Social;
using Terraria.Social.Base;
using Terraria.UI;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003A1 RID: 929
	public class WorkshopPublishInfoStateForWorld : AWorkshopPublishInfoState<WorldFileData>
	{
		// Token: 0x06002A9B RID: 10907 RVA: 0x005860F4 File Offset: 0x005842F4
		public WorkshopPublishInfoStateForWorld(UIState stateToGoBackTo, WorldFileData world) : base(stateToGoBackTo, world)
		{
			this._instructionsTextKey = "Workshop.WorldPublishDescription";
			this._publishedObjectNameDescriptorTexKey = "Workshop.WorldName";
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x00586114 File Offset: 0x00584314
		protected override string GetPublishedObjectDisplayName()
		{
			if (this._dataObject == null)
			{
				return "null";
			}
			return this._dataObject.Name;
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x00586130 File Offset: 0x00584330
		protected override void GoToPublishConfirmation()
		{
			if (SocialAPI.Workshop != null && this._dataObject != null)
			{
				SocialAPI.Workshop.PublishWorld(this._dataObject, base.GetPublishSettings());
			}
			Main.menuMode = 888;
			Main.MenuUI.SetState(this._previousUIState);
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x0058617C File Offset: 0x0058437C
		protected override List<WorkshopTagOption> GetTagsToShow()
		{
			return SocialAPI.Workshop.SupportedTags.WorldTags;
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x0058618D File Offset: 0x0058438D
		protected override bool TryFindingTags(out FoundWorkshopEntryInfo info)
		{
			return SocialAPI.Workshop.TryGetInfoForWorld(this._dataObject, out info);
		}
	}
}
