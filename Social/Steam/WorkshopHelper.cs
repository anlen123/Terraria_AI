using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;
using Terraria.IO;
using Terraria.Social.Base;
using Terraria.Utilities;

namespace Terraria.Social.Steam
{
	// Token: 0x02000140 RID: 320
	public class WorkshopHelper
	{
		// Token: 0x02000737 RID: 1847
		public class UGCBased
		{
			// Token: 0x04006977 RID: 26999
			public const string ManifestFileName = "workshop.json";

			// Token: 0x02000A86 RID: 2694
			public struct SteamWorkshopItem
			{
				// Token: 0x0400772E RID: 30510
				public string ContentFolderPath;

				// Token: 0x0400772F RID: 30511
				public string Description;

				// Token: 0x04007730 RID: 30512
				public string PreviewImagePath;

				// Token: 0x04007731 RID: 30513
				public string[] Tags;

				// Token: 0x04007732 RID: 30514
				public string Title;

				// Token: 0x04007733 RID: 30515
				public ERemoteStoragePublishedFileVisibility? Visibility;
			}

			// Token: 0x02000A87 RID: 2695
			public class Downloader
			{
				// Token: 0x170005BE RID: 1470
				// (get) Token: 0x06004B9B RID: 19355 RVA: 0x006D7574 File Offset: 0x006D5774
				// (set) Token: 0x06004B9C RID: 19356 RVA: 0x006D757C File Offset: 0x006D577C
				public List<string> ResourcePackPaths { get; private set; }

				// Token: 0x170005BF RID: 1471
				// (get) Token: 0x06004B9D RID: 19357 RVA: 0x006D7585 File Offset: 0x006D5785
				// (set) Token: 0x06004B9E RID: 19358 RVA: 0x006D758D File Offset: 0x006D578D
				public List<string> WorldPaths { get; private set; }

				// Token: 0x06004B9F RID: 19359 RVA: 0x006D7596 File Offset: 0x006D5796
				public Downloader()
				{
					this.ResourcePackPaths = new List<string>();
					this.WorldPaths = new List<string>();
				}

				// Token: 0x06004BA0 RID: 19360 RVA: 0x006D75B4 File Offset: 0x006D57B4
				public static WorkshopHelper.UGCBased.Downloader Create()
				{
					return new WorkshopHelper.UGCBased.Downloader();
				}

				// Token: 0x06004BA1 RID: 19361 RVA: 0x006D75BC File Offset: 0x006D57BC
				public List<string> GetListOfSubscribedItemsPaths()
				{
					PublishedFileId_t[] array = new PublishedFileId_t[SteamUGC.GetNumSubscribedItems()];
					SteamUGC.GetSubscribedItems(array, (uint)array.Length);
					ulong num = 0UL;
					string empty = string.Empty;
					uint num2 = 0U;
					List<string> list = new List<string>();
					PublishedFileId_t[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						if (SteamUGC.GetItemInstallInfo(array2[i], ref num, ref empty, 1024U, ref num2))
						{
							list.Add(empty);
						}
					}
					return list;
				}

				// Token: 0x06004BA2 RID: 19362 RVA: 0x006D7626 File Offset: 0x006D5826
				public bool Prepare(WorkshopIssueReporter issueReporter)
				{
					return this.Refresh(issueReporter);
				}

				// Token: 0x06004BA3 RID: 19363 RVA: 0x006D7630 File Offset: 0x006D5830
				public bool Refresh(WorkshopIssueReporter issueReporter)
				{
					this.ResourcePackPaths.Clear();
					this.WorldPaths.Clear();
					foreach (string text in this.GetListOfSubscribedItemsPaths())
					{
						if (text != null)
						{
							try
							{
								string path = text + Path.DirectorySeparatorChar.ToString() + "workshop.json";
								if (File.Exists(path))
								{
									string a = AWorkshopEntry.ReadHeader(File.ReadAllText(path));
									if (!(a == "World"))
									{
										if (a == "ResourcePack")
										{
											this.ResourcePackPaths.Add(text);
										}
									}
									else
									{
										this.WorldPaths.Add(text);
									}
								}
							}
							catch (Exception exception)
							{
								issueReporter.ReportDownloadProblem("Workshop.ReportIssue_FailedToLoadSubscribedFile", text, exception);
								return false;
							}
						}
					}
					return true;
				}
			}

			// Token: 0x02000A88 RID: 2696
			public class PublishedItemsFinder
			{
				// Token: 0x06004BA4 RID: 19364 RVA: 0x006D772C File Offset: 0x006D592C
				public bool HasItemOfId(ulong id)
				{
					return this._items.ContainsKey(id);
				}

				// Token: 0x06004BA5 RID: 19365 RVA: 0x006D773A File Offset: 0x006D593A
				public static WorkshopHelper.UGCBased.PublishedItemsFinder Create()
				{
					WorkshopHelper.UGCBased.PublishedItemsFinder publishedItemsFinder = new WorkshopHelper.UGCBased.PublishedItemsFinder();
					publishedItemsFinder.LoadHooks();
					return publishedItemsFinder;
				}

				// Token: 0x06004BA6 RID: 19366 RVA: 0x006D7747 File Offset: 0x006D5947
				private void LoadHooks()
				{
					this.OnSteamUGCQueryCompletedCallResult = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.OnSteamUGCQueryCompleted));
					this.OnSteamUGCRequestUGCDetailsResultCallResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(new CallResult<SteamUGCRequestUGCDetailsResult_t>.APIDispatchDelegate(this.OnSteamUGCRequestUGCDetailsResult));
				}

				// Token: 0x06004BA7 RID: 19367 RVA: 0x006D7777 File Offset: 0x006D5977
				public void Prepare()
				{
					this.Refresh();
				}

				// Token: 0x06004BA8 RID: 19368 RVA: 0x006D7780 File Offset: 0x006D5980
				public void Refresh()
				{
					this.m_UGCQueryHandle = SteamUGC.CreateQueryUserUGCRequest(SteamUser.GetSteamID().GetAccountID(), 0, -1, 0, SteamUtils.GetAppID(), SteamUtils.GetAppID(), 1U);
					CoreSocialModule.SetSkipPulsing(true);
					SteamAPICall_t steamAPICall_t = SteamUGC.SendQueryUGCRequest(this.m_UGCQueryHandle);
					this.OnSteamUGCQueryCompletedCallResult.Set(steamAPICall_t, new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.OnSteamUGCQueryCompleted));
					CoreSocialModule.SetSkipPulsing(false);
				}

				// Token: 0x06004BA9 RID: 19369 RVA: 0x006D77E4 File Offset: 0x006D59E4
				private void OnSteamUGCQueryCompleted(SteamUGCQueryCompleted_t pCallback, bool bIOFailure)
				{
					this._items.Clear();
					if (bIOFailure || pCallback.m_eResult != 1)
					{
						SteamUGC.ReleaseQueryUGCRequest(this.m_UGCQueryHandle);
						return;
					}
					for (uint num = 0U; num < pCallback.m_unNumResultsReturned; num += 1U)
					{
						SteamUGCDetails_t steamUGCDetails_t;
						SteamUGC.GetQueryUGCResult(this.m_UGCQueryHandle, num, ref steamUGCDetails_t);
						ulong publishedFileId = steamUGCDetails_t.m_nPublishedFileId.m_PublishedFileId;
						WorkshopHelper.UGCBased.SteamWorkshopItem value = new WorkshopHelper.UGCBased.SteamWorkshopItem
						{
							Title = steamUGCDetails_t.m_rgchTitle,
							Description = steamUGCDetails_t.m_rgchDescription
						};
						this._items.Add(publishedFileId, value);
					}
					SteamUGC.ReleaseQueryUGCRequest(this.m_UGCQueryHandle);
				}

				// Token: 0x06004BAA RID: 19370 RVA: 0x00009E06 File Offset: 0x00008006
				private void OnSteamUGCRequestUGCDetailsResult(SteamUGCRequestUGCDetailsResult_t pCallback, bool bIOFailure)
				{
				}

				// Token: 0x04007736 RID: 30518
				private Dictionary<ulong, WorkshopHelper.UGCBased.SteamWorkshopItem> _items = new Dictionary<ulong, WorkshopHelper.UGCBased.SteamWorkshopItem>();

				// Token: 0x04007737 RID: 30519
				private UGCQueryHandle_t m_UGCQueryHandle;

				// Token: 0x04007738 RID: 30520
				private CallResult<SteamUGCQueryCompleted_t> OnSteamUGCQueryCompletedCallResult;

				// Token: 0x04007739 RID: 30521
				private CallResult<SteamUGCRequestUGCDetailsResult_t> OnSteamUGCRequestUGCDetailsResultCallResult;
			}

			// Token: 0x02000A89 RID: 2697
			public abstract class APublisherInstance
			{
				// Token: 0x06004BAC RID: 19372 RVA: 0x006D789C File Offset: 0x006D5A9C
				public void PublishContent(WorkshopHelper.UGCBased.PublishedItemsFinder finder, WorkshopIssueReporter issueReporter, WorkshopHelper.UGCBased.APublisherInstance.FinishedPublishingAction endAction, string itemTitle, string itemDescription, string contentFolderPath, string previewImagePath, WorkshopItemPublicSettingId publicity, string[] tags)
				{
					this._issueReporter = issueReporter;
					this._endAction = endAction;
					this._createItemHook = CallResult<CreateItemResult_t>.Create(new CallResult<CreateItemResult_t>.APIDispatchDelegate(this.CreateItemResult));
					this._updateItemHook = CallResult<SubmitItemUpdateResult_t>.Create(new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(this.UpdateItemResult));
					ERemoteStoragePublishedFileVisibility visibility = this.GetVisibility(publicity);
					this._entryData = new WorkshopHelper.UGCBased.SteamWorkshopItem
					{
						Title = itemTitle,
						Description = itemDescription,
						ContentFolderPath = contentFolderPath,
						Tags = tags,
						PreviewImagePath = previewImagePath,
						Visibility = new ERemoteStoragePublishedFileVisibility?(visibility)
					};
					ulong? num = null;
					FoundWorkshopEntryInfo foundWorkshopEntryInfo;
					if (AWorkshopEntry.TryReadingManifest(contentFolderPath + Path.DirectorySeparatorChar.ToString() + "workshop.json", out foundWorkshopEntryInfo))
					{
						num = new ulong?(foundWorkshopEntryInfo.workshopEntryId);
					}
					if (num != null && finder.HasItemOfId(num.Value))
					{
						this._publishedFileID = new PublishedFileId_t(num.Value);
						this.PreventUpdatingCertainThings();
						this.UpdateItem();
						return;
					}
					this.CreateItem();
				}

				// Token: 0x06004BAD RID: 19373 RVA: 0x006D79AF File Offset: 0x006D5BAF
				private void PreventUpdatingCertainThings()
				{
					this._entryData.Title = null;
					this._entryData.Description = null;
				}

				// Token: 0x06004BAE RID: 19374 RVA: 0x006D79C9 File Offset: 0x006D5BC9
				private ERemoteStoragePublishedFileVisibility GetVisibility(WorkshopItemPublicSettingId publicityId)
				{
					switch (publicityId)
					{
					default:
						return 2;
					case WorkshopItemPublicSettingId.FriendsOnly:
						return 1;
					case WorkshopItemPublicSettingId.Public:
						return 0;
					}
				}

				// Token: 0x06004BAF RID: 19375 RVA: 0x006D79E4 File Offset: 0x006D5BE4
				private void CreateItem()
				{
					CoreSocialModule.SetSkipPulsing(true);
					SteamAPICall_t steamAPICall_t = SteamUGC.CreateItem(SteamUtils.GetAppID(), 0);
					this._createItemHook.Set(steamAPICall_t, new CallResult<CreateItemResult_t>.APIDispatchDelegate(this.CreateItemResult));
					CoreSocialModule.SetSkipPulsing(false);
				}

				// Token: 0x06004BB0 RID: 19376 RVA: 0x006D7A24 File Offset: 0x006D5C24
				private void CreateItemResult(CreateItemResult_t param, bool bIOFailure)
				{
					if (param.m_bUserNeedsToAcceptWorkshopLegalAgreement)
					{
						this._issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_UserDidNotAcceptWorkshopTermsOfService");
						this._endAction(this);
						return;
					}
					if (param.m_eResult == 1)
					{
						this._publishedFileID = param.m_nPublishedFileId;
						this.UpdateItem();
						return;
					}
					this._issueReporter.ReportDelayedUploadProblemWithoutKnownReason("Workshop.ReportIssue_FailedToPublish_WithoutKnownReason", param.m_eResult.ToString());
					this._endAction(this);
				}

				// Token: 0x06004BB1 RID: 19377
				protected abstract string GetHeaderText();

				// Token: 0x06004BB2 RID: 19378
				protected abstract void PrepareContentForUpdate();

				// Token: 0x06004BB3 RID: 19379 RVA: 0x006D7AA0 File Offset: 0x006D5CA0
				private void UpdateItem()
				{
					string headerText = this.GetHeaderText();
					if (!this.TryWritingManifestToFolder(this._entryData.ContentFolderPath, headerText))
					{
						this._endAction(this);
						return;
					}
					this.PrepareContentForUpdate();
					UGCUpdateHandle_t ugcupdateHandle_t = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), this._publishedFileID);
					if (this._entryData.Title != null)
					{
						SteamUGC.SetItemTitle(ugcupdateHandle_t, this._entryData.Title);
					}
					if (this._entryData.Description != null)
					{
						SteamUGC.SetItemDescription(ugcupdateHandle_t, this._entryData.Description);
					}
					SteamUGC.SetItemContent(ugcupdateHandle_t, this._entryData.ContentFolderPath);
					SteamUGC.SetItemTags(ugcupdateHandle_t, this._entryData.Tags, false);
					if (this._entryData.PreviewImagePath != null)
					{
						SteamUGC.SetItemPreview(ugcupdateHandle_t, this._entryData.PreviewImagePath);
					}
					if (this._entryData.Visibility != null)
					{
						SteamUGC.SetItemVisibility(ugcupdateHandle_t, this._entryData.Visibility.Value);
					}
					CoreSocialModule.SetSkipPulsing(true);
					SteamAPICall_t steamAPICall_t = SteamUGC.SubmitItemUpdate(ugcupdateHandle_t, "");
					this._updateHandle = ugcupdateHandle_t;
					this._updateItemHook.Set(steamAPICall_t, new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(this.UpdateItemResult));
					CoreSocialModule.SetSkipPulsing(false);
				}

				// Token: 0x06004BB4 RID: 19380 RVA: 0x006D7BD0 File Offset: 0x006D5DD0
				private void UpdateItemResult(SubmitItemUpdateResult_t param, bool bIOFailure)
				{
					if (param.m_bUserNeedsToAcceptWorkshopLegalAgreement)
					{
						this._issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_UserDidNotAcceptWorkshopTermsOfService");
						this._endAction(this);
						return;
					}
					EResult eResult = param.m_eResult;
					if (eResult <= 9)
					{
						if (eResult == 1)
						{
							SteamFriends.ActivateGameOverlayToWebPage("steam://url/CommunityFilePage/" + this._publishedFileID.m_PublishedFileId, 0);
							goto IL_F5;
						}
						if (eResult == 8)
						{
							this._issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_InvalidParametersForPublishing");
							goto IL_F5;
						}
						if (eResult == 9)
						{
							this._issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_CouldNotFindFolderToUpload");
							goto IL_F5;
						}
					}
					else
					{
						if (eResult == 15)
						{
							this._issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_AccessDeniedBecauseUserDoesntOwnLicenseForApp");
							goto IL_F5;
						}
						if (eResult == 25)
						{
							this._issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_LimitExceeded");
							goto IL_F5;
						}
						if (eResult == 33)
						{
							this._issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_SteamFileLockFailed");
							goto IL_F5;
						}
					}
					this._issueReporter.ReportDelayedUploadProblemWithoutKnownReason("Workshop.ReportIssue_FailedToPublish_WithoutKnownReason", param.m_eResult.ToString());
					IL_F5:
					this._endAction(this);
				}

				// Token: 0x06004BB5 RID: 19381 RVA: 0x006D7CE0 File Offset: 0x006D5EE0
				private bool TryWritingManifestToFolder(string folderPath, string manifestText)
				{
					string path = folderPath + Path.DirectorySeparatorChar.ToString() + "workshop.json";
					bool result = true;
					try
					{
						File.WriteAllText(path, manifestText);
					}
					catch (Exception exception)
					{
						this._issueReporter.ReportManifestCreationProblem("Workshop.ReportIssue_CouldNotCreateResourcePackManifestFile", exception);
						result = false;
					}
					return result;
				}

				// Token: 0x06004BB6 RID: 19382 RVA: 0x006D7D38 File Offset: 0x006D5F38
				public bool TryGetProgress(out float progress)
				{
					progress = 0f;
					if (this._updateHandle == default(UGCUpdateHandle_t))
					{
						return false;
					}
					ulong num;
					ulong num2;
					SteamUGC.GetItemUpdateProgress(this._updateHandle, ref num, ref num2);
					if (num2 == 0UL)
					{
						return false;
					}
					progress = (float)(num / num2);
					return true;
				}

				// Token: 0x0400773A RID: 30522
				protected WorkshopItemPublicSettingId _publicity;

				// Token: 0x0400773B RID: 30523
				protected WorkshopHelper.UGCBased.SteamWorkshopItem _entryData;

				// Token: 0x0400773C RID: 30524
				protected PublishedFileId_t _publishedFileID;

				// Token: 0x0400773D RID: 30525
				private UGCUpdateHandle_t _updateHandle;

				// Token: 0x0400773E RID: 30526
				private CallResult<CreateItemResult_t> _createItemHook;

				// Token: 0x0400773F RID: 30527
				private CallResult<SubmitItemUpdateResult_t> _updateItemHook;

				// Token: 0x04007740 RID: 30528
				private WorkshopHelper.UGCBased.APublisherInstance.FinishedPublishingAction _endAction;

				// Token: 0x04007741 RID: 30529
				private WorkshopIssueReporter _issueReporter;

				// Token: 0x02000B10 RID: 2832
				// (Invoke) Token: 0x06004DA4 RID: 19876
				public delegate void FinishedPublishingAction(WorkshopHelper.UGCBased.APublisherInstance instance);
			}

			// Token: 0x02000A8A RID: 2698
			public class ResourcePackPublisherInstance : WorkshopHelper.UGCBased.APublisherInstance
			{
				// Token: 0x06004BB8 RID: 19384 RVA: 0x006D7D84 File Offset: 0x006D5F84
				public ResourcePackPublisherInstance(ResourcePack resourcePack)
				{
					this._resourcePack = resourcePack;
				}

				// Token: 0x06004BB9 RID: 19385 RVA: 0x006D7D93 File Offset: 0x006D5F93
				protected override string GetHeaderText()
				{
					return TexturePackWorkshopEntry.GetHeaderTextFor(this._resourcePack, this._publishedFileID.m_PublishedFileId, this._entryData.Tags, this._publicity, this._entryData.PreviewImagePath);
				}

				// Token: 0x06004BBA RID: 19386 RVA: 0x00009E06 File Offset: 0x00008006
				protected override void PrepareContentForUpdate()
				{
				}

				// Token: 0x04007742 RID: 30530
				private ResourcePack _resourcePack;
			}

			// Token: 0x02000A8B RID: 2699
			public class WorldPublisherInstance : WorkshopHelper.UGCBased.APublisherInstance
			{
				// Token: 0x06004BBB RID: 19387 RVA: 0x006D7DC7 File Offset: 0x006D5FC7
				public WorldPublisherInstance(WorldFileData world)
				{
					this._world = world;
				}

				// Token: 0x06004BBC RID: 19388 RVA: 0x006D7DD6 File Offset: 0x006D5FD6
				protected override string GetHeaderText()
				{
					return WorldWorkshopEntry.GetHeaderTextFor(this._world, this._publishedFileID.m_PublishedFileId, this._entryData.Tags, this._publicity, this._entryData.PreviewImagePath);
				}

				// Token: 0x06004BBD RID: 19389 RVA: 0x006D7E0C File Offset: 0x006D600C
				protected override void PrepareContentForUpdate()
				{
					if (this._world.IsCloudSave)
					{
						FileUtilities.CopyToLocal(this._world.Path, this._entryData.ContentFolderPath + Path.DirectorySeparatorChar.ToString() + "world.wld");
						return;
					}
					FileUtilities.Copy(this._world.Path, this._entryData.ContentFolderPath + Path.DirectorySeparatorChar.ToString() + "world.wld", false);
				}

				// Token: 0x04007743 RID: 30531
				private WorldFileData _world;
			}
		}
	}
}
