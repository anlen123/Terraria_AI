using System;
using System.Collections.Generic;
using Terraria.IO;

namespace Terraria.Social.Base
{
	// Token: 0x0200015D RID: 349
	public abstract class WorkshopSocialModule : ISocialModule
	{
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001D4C RID: 7500 RVA: 0x00500C6D File Offset: 0x004FEE6D
		// (set) Token: 0x06001D4D RID: 7501 RVA: 0x00500C75 File Offset: 0x004FEE75
		public WorkshopBranding Branding { get; protected set; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001D4E RID: 7502 RVA: 0x00500C7E File Offset: 0x004FEE7E
		// (set) Token: 0x06001D4F RID: 7503 RVA: 0x00500C86 File Offset: 0x004FEE86
		public AWorkshopProgressReporter ProgressReporter { get; protected set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001D50 RID: 7504 RVA: 0x00500C8F File Offset: 0x004FEE8F
		// (set) Token: 0x06001D51 RID: 7505 RVA: 0x00500C97 File Offset: 0x004FEE97
		public AWorkshopTagsCollection SupportedTags { get; protected set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001D52 RID: 7506 RVA: 0x00500CA0 File Offset: 0x004FEEA0
		// (set) Token: 0x06001D53 RID: 7507 RVA: 0x00500CA8 File Offset: 0x004FEEA8
		public WorkshopIssueReporter IssueReporter { get; protected set; }

		// Token: 0x06001D54 RID: 7508
		public abstract void Initialize();

		// Token: 0x06001D55 RID: 7509
		public abstract void Shutdown();

		// Token: 0x06001D56 RID: 7510
		public abstract void PublishWorld(WorldFileData world, WorkshopItemPublishSettings settings);

		// Token: 0x06001D57 RID: 7511
		public abstract void PublishResourcePack(ResourcePack resourcePack, WorkshopItemPublishSettings settings);

		// Token: 0x06001D58 RID: 7512
		public abstract bool TryGetInfoForWorld(WorldFileData world, out FoundWorkshopEntryInfo info);

		// Token: 0x06001D59 RID: 7513
		public abstract bool TryGetInfoForResourcePack(ResourcePack resourcePack, out FoundWorkshopEntryInfo info);

		// Token: 0x06001D5A RID: 7514
		public abstract void LoadEarlyContent();

		// Token: 0x06001D5B RID: 7515
		public abstract List<string> GetListOfSubscribedResourcePackPaths();

		// Token: 0x06001D5C RID: 7516
		public abstract List<string> GetListOfSubscribedWorldPaths();

		// Token: 0x06001D5D RID: 7517
		public abstract bool TryGetPath(string pathEnd, out string fullPathFound);

		// Token: 0x06001D5E RID: 7518
		public abstract void ImportDownloadedWorldToLocalSaves(WorldFileData world, string newDisplayName, Action onCompleted);
	}
}
