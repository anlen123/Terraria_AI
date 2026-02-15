using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Steamworks;
using Terraria.DataStructures;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Social.Base;

namespace Terraria.Social.Steam
{
	// Token: 0x02000142 RID: 322
	public class WorkshopSocialModule : WorkshopSocialModule
	{
		// Token: 0x06001C7E RID: 7294 RVA: 0x004FE000 File Offset: 0x004FC200
		public override void Initialize()
		{
			base.Branding = new WorkshopBranding
			{
				ResourcePackBrand = ResourcePack.BrandingType.SteamWorkshop
			};
			this._publisherInstances = new List<WorkshopHelper.UGCBased.APublisherInstance>();
			base.ProgressReporter = new WorkshopProgressReporter(this._publisherInstances);
			base.SupportedTags = new SupportedWorkshopTags();
			this._contentBaseFolder = Main.SavePath + Path.DirectorySeparatorChar.ToString() + "Workshop";
			this._downloader = WorkshopHelper.UGCBased.Downloader.Create();
			this._publishedItems = WorkshopHelper.UGCBased.PublishedItemsFinder.Create();
			WorkshopIssueReporter workshopIssueReporter = new WorkshopIssueReporter();
			workshopIssueReporter.OnNeedToOpenUI += this._issueReporter_OnNeedToOpenUI;
			workshopIssueReporter.OnNeedToNotifyUI += this._issueReporter_OnNeedToNotifyUI;
			base.IssueReporter = workshopIssueReporter;
			UIWorkshopHub.OnWorkshopHubMenuOpened += this.RefreshSubscriptionsAndPublishings;
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x004FE0C0 File Offset: 0x004FC2C0
		private void _issueReporter_OnNeedToNotifyUI()
		{
			Main.IssueReporterIndicator.AttemptLettingPlayerKnow();
			Main.WorkshopPublishingIndicator.Hide();
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x004FE0D6 File Offset: 0x004FC2D6
		private void _issueReporter_OnNeedToOpenUI()
		{
			Main.OpenReportsMenu();
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Shutdown()
		{
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x004FE0DD File Offset: 0x004FC2DD
		public override void LoadEarlyContent()
		{
			this.RefreshSubscriptionsAndPublishings();
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x004FE0E5 File Offset: 0x004FC2E5
		private void RefreshSubscriptionsAndPublishings()
		{
			this._downloader.Refresh(base.IssueReporter);
			this._publishedItems.Refresh();
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x004FE104 File Offset: 0x004FC304
		public override List<string> GetListOfSubscribedWorldPaths()
		{
			return (from folderPath in this._downloader.WorldPaths
			select folderPath + Path.DirectorySeparatorChar.ToString() + "world.wld").ToList<string>();
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x004FE13A File Offset: 0x004FC33A
		public override List<string> GetListOfSubscribedResourcePackPaths()
		{
			return this._downloader.ResourcePackPaths;
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x004FE148 File Offset: 0x004FC348
		public override bool TryGetPath(string pathEnd, out string fullPathFound)
		{
			fullPathFound = null;
			string text = this._downloader.ResourcePackPaths.FirstOrDefault((string x) => x.EndsWith(pathEnd));
			if (text == null)
			{
				return false;
			}
			fullPathFound = text;
			return true;
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x004FE18B File Offset: 0x004FC38B
		private void Forget(WorkshopHelper.UGCBased.APublisherInstance instance)
		{
			this._publisherInstances.Remove(instance);
			this.RefreshSubscriptionsAndPublishings();
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x004FE1A0 File Offset: 0x004FC3A0
		public override void PublishWorld(WorldFileData world, WorkshopItemPublishSettings settings)
		{
			string name = world.Name;
			string textForWorld = this.GetTextForWorld(world);
			string[] usedTagsInternalNames = settings.GetUsedTagsInternalNames();
			string text = this.GetTemporaryFolderPath() + world.GetFileName(false);
			if (!this.MakeTemporaryFolder(text))
			{
				return;
			}
			WorkshopHelper.UGCBased.WorldPublisherInstance worldPublisherInstance = new WorkshopHelper.UGCBased.WorldPublisherInstance(world);
			this._publisherInstances.Add(worldPublisherInstance);
			worldPublisherInstance.PublishContent(this._publishedItems, base.IssueReporter, new WorkshopHelper.UGCBased.APublisherInstance.FinishedPublishingAction(this.Forget), name, textForWorld, text, settings.PreviewImagePath, settings.Publicity, usedTagsInternalNames);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x004FE224 File Offset: 0x004FC424
		private string GetTextForWorld(WorldFileData world)
		{
			string text = "This is \"";
			text += world.Name;
			int worldSizeX = world.WorldSizeX;
			string text2;
			if (worldSizeX != 4200)
			{
				if (worldSizeX != 6400)
				{
					if (worldSizeX != 8400)
					{
						text2 = "custom";
					}
					else
					{
						text2 = "large";
					}
				}
				else
				{
					text2 = "medium";
				}
			}
			else
			{
				text2 = "small";
			}
			string text3;
			switch (world.GameMode)
			{
			case 0:
				text3 = "classic";
				break;
			case 1:
				text3 = "expert";
				break;
			case 2:
				text3 = "master";
				break;
			case 3:
				text3 = "journey";
				break;
			default:
				text3 = "custom";
				break;
			}
			text = string.Concat(new string[]
			{
				text,
				"\", a ",
				text2.ToLower(),
				" ",
				text3.ToLower(),
				" world"
			});
			text = text + " infected by the " + (world.HasCorruption ? "corruption" : "crimson");
			if (world.IsHardMode)
			{
				text += ", in hardmode";
			}
			return text + ".";
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x004FE350 File Offset: 0x004FC550
		public override void PublishResourcePack(ResourcePack resourcePack, WorkshopItemPublishSettings settings)
		{
			if (resourcePack.IsCompressed)
			{
				base.IssueReporter.ReportInstantUploadProblem("Workshop.ReportIssue_CannotPublishZips");
				return;
			}
			string name = resourcePack.Name;
			string text = resourcePack.Description;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = "";
			}
			string[] usedTagsInternalNames = settings.GetUsedTagsInternalNames();
			string fullPath = resourcePack.FullPath;
			WorkshopHelper.UGCBased.ResourcePackPublisherInstance resourcePackPublisherInstance = new WorkshopHelper.UGCBased.ResourcePackPublisherInstance(resourcePack);
			this._publisherInstances.Add(resourcePackPublisherInstance);
			resourcePackPublisherInstance.PublishContent(this._publishedItems, base.IssueReporter, new WorkshopHelper.UGCBased.APublisherInstance.FinishedPublishingAction(this.Forget), name, text, fullPath, settings.PreviewImagePath, settings.Publicity, usedTagsInternalNames);
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x004FE3E4 File Offset: 0x004FC5E4
		private string GetTemporaryFolderPath()
		{
			ulong steamID = SteamUser.GetSteamID().m_SteamID;
			return this._contentBaseFolder + Path.DirectorySeparatorChar.ToString() + steamID.ToString() + Path.DirectorySeparatorChar.ToString();
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x004FE428 File Offset: 0x004FC628
		private bool MakeTemporaryFolder(string temporaryFolderPath)
		{
			bool result = true;
			if (!Utils.TryCreatingDirectory(temporaryFolderPath))
			{
				base.IssueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_CouldNotCreateTemporaryFolder!");
				result = false;
			}
			return result;
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x004FE452 File Offset: 0x004FC652
		public override void ImportDownloadedWorldToLocalSaves(WorldFileData world, string newDisplayName, Action onCompleted)
		{
			Main.menuMode = 10;
			world.CopyToLocal(newDisplayName, onCompleted);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x004FE464 File Offset: 0x004FC664
		public List<IssueReport> GetReports()
		{
			List<IssueReport> list = new List<IssueReport>();
			if (base.IssueReporter != null)
			{
				list.AddRange(base.IssueReporter.GetReports());
			}
			return list;
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x004FE494 File Offset: 0x004FC694
		public override bool TryGetInfoForWorld(WorldFileData world, out FoundWorkshopEntryInfo info)
		{
			info = null;
			string text = this.GetTemporaryFolderPath() + world.GetFileName(false);
			return Directory.Exists(text) && AWorkshopEntry.TryReadingManifest(text + Path.DirectorySeparatorChar.ToString() + "workshop.json", out info);
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x004FE4E4 File Offset: 0x004FC6E4
		public override bool TryGetInfoForResourcePack(ResourcePack resourcePack, out FoundWorkshopEntryInfo info)
		{
			info = null;
			string fullPath = resourcePack.FullPath;
			return Directory.Exists(fullPath) && AWorkshopEntry.TryReadingManifest(fullPath + Path.DirectorySeparatorChar.ToString() + "workshop.json", out info);
		}

		// Token: 0x040015C6 RID: 5574
		private WorkshopHelper.UGCBased.Downloader _downloader;

		// Token: 0x040015C7 RID: 5575
		private WorkshopHelper.UGCBased.PublishedItemsFinder _publishedItems;

		// Token: 0x040015C8 RID: 5576
		private List<WorkshopHelper.UGCBased.APublisherInstance> _publisherInstances;

		// Token: 0x040015C9 RID: 5577
		private string _contentBaseFolder;
	}
}
