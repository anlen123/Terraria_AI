using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.Social;
using Terraria.Social.Base;

namespace Terraria.IO
{
	// Token: 0x0200006B RID: 107
	public class ResourcePackList
	{
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06001496 RID: 5270 RVA: 0x004BB038 File Offset: 0x004B9238
		public IEnumerable<ResourcePack> EnabledPacks
		{
			get
			{
				return from pack in this._resourcePacks
				where pack.IsEnabled
				orderby pack.SortingOrder, pack.Name, pack.Version, pack.FileName
				select pack;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x004BB100 File Offset: 0x004B9300
		public IEnumerable<ResourcePack> DisabledPacks
		{
			get
			{
				return from pack in this._resourcePacks
				where !pack.IsEnabled
				orderby pack.Name, pack.Version, pack.FileName
				select pack;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x004BB1A4 File Offset: 0x004B93A4
		public IEnumerable<ResourcePack> AllPacks
		{
			get
			{
				return from pack in this._resourcePacks
				orderby pack.Name, pack.Version, pack.FileName
				select pack;
			}
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x004BB223 File Offset: 0x004B9423
		public ResourcePackList()
		{
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x004BB236 File Offset: 0x004B9436
		public ResourcePackList(IEnumerable<ResourcePack> resourcePacks)
		{
			this._resourcePacks.AddRange(resourcePacks);
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x004BB258 File Offset: 0x004B9458
		public JArray ToJson()
		{
			List<ResourcePackList.ResourcePackEntry> list = new List<ResourcePackList.ResourcePackEntry>(this._resourcePacks.Count);
			list.AddRange(from pack in this._resourcePacks
			select new ResourcePackList.ResourcePackEntry(pack.FileName, pack.IsEnabled, pack.SortingOrder));
			return JArray.FromObject(list);
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x004BB2AC File Offset: 0x004B94AC
		public static ResourcePackList FromJson(JArray serializedState, IServiceProvider services, string searchPath)
		{
			if (!Directory.Exists(searchPath))
			{
				return new ResourcePackList();
			}
			List<ResourcePack> resourcePacks = new List<ResourcePack>();
			ResourcePackList.CreatePacksFromSavedJson(serializedState, services, searchPath, resourcePacks);
			ResourcePackList.CreatePacksFromZips(services, searchPath, resourcePacks);
			ResourcePackList.CreatePacksFromDirectories(services, searchPath, resourcePacks);
			ResourcePackList.CreatePacksFromWorkshopFolders(services, resourcePacks);
			return new ResourcePackList(resourcePacks);
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x004BB2F4 File Offset: 0x004B94F4
		public static ResourcePackList Publishable(JArray serializedState, IServiceProvider services, string searchPath)
		{
			if (!Directory.Exists(searchPath))
			{
				return new ResourcePackList();
			}
			List<ResourcePack> resourcePacks = new List<ResourcePack>();
			ResourcePackList.CreatePacksFromZips(services, searchPath, resourcePacks);
			ResourcePackList.CreatePacksFromDirectories(services, searchPath, resourcePacks);
			return new ResourcePackList(resourcePacks);
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x004BB32C File Offset: 0x004B952C
		private static void CreatePacksFromSavedJson(JArray serializedState, IServiceProvider services, string searchPath, List<ResourcePack> resourcePacks)
		{
			foreach (ResourcePackList.ResourcePackEntry resourcePackEntry in ResourcePackList.CreatePackEntryListFromJson(serializedState))
			{
				if (resourcePackEntry.FileName != null)
				{
					string text = Path.Combine(searchPath, resourcePackEntry.FileName);
					try
					{
						bool flag = File.Exists(text) || Directory.Exists(text);
						ResourcePack.BrandingType branding = ResourcePack.BrandingType.None;
						string text2;
						if (!flag && SocialAPI.Workshop != null && SocialAPI.Workshop.TryGetPath(resourcePackEntry.FileName, out text2))
						{
							text = text2;
							flag = true;
							branding = SocialAPI.Workshop.Branding.ResourcePackBrand;
						}
						if (flag)
						{
							ResourcePack item = new ResourcePack(services, text, branding)
							{
								IsEnabled = resourcePackEntry.Enabled,
								SortingOrder = resourcePackEntry.SortingOrder
							};
							resourcePacks.Add(item);
						}
					}
					catch (Exception arg)
					{
						Console.WriteLine("Failed to read resource pack {0}: {1}", text, arg);
					}
				}
			}
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x004BB428 File Offset: 0x004B9628
		private static void CreatePacksFromDirectories(IServiceProvider services, string searchPath, List<ResourcePack> resourcePacks)
		{
			foreach (string text in Directory.GetDirectories(searchPath))
			{
				try
				{
					string folderName = Path.GetFileName(text);
					if (resourcePacks.All((ResourcePack pack) => pack.FileName != folderName))
					{
						resourcePacks.Add(new ResourcePack(services, text, ResourcePack.BrandingType.None));
					}
				}
				catch (Exception arg)
				{
					Console.WriteLine("Failed to read resource pack {0}: {1}", text, arg);
				}
			}
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x004BB4A8 File Offset: 0x004B96A8
		private static void CreatePacksFromZips(IServiceProvider services, string searchPath, List<ResourcePack> resourcePacks)
		{
			foreach (string text in Directory.GetFiles(searchPath, "*.zip"))
			{
				try
				{
					string fileName = Path.GetFileName(text);
					if (resourcePacks.All((ResourcePack pack) => pack.FileName != fileName))
					{
						resourcePacks.Add(new ResourcePack(services, text, ResourcePack.BrandingType.None));
					}
				}
				catch (Exception arg)
				{
					Console.WriteLine("Failed to read resource pack {0}: {1}", text, arg);
				}
			}
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x004BB52C File Offset: 0x004B972C
		private static void CreatePacksFromWorkshopFolders(IServiceProvider services, List<ResourcePack> resourcePacks)
		{
			WorkshopSocialModule workshop = SocialAPI.Workshop;
			if (workshop == null)
			{
				return;
			}
			List<string> listOfSubscribedResourcePackPaths = workshop.GetListOfSubscribedResourcePackPaths();
			ResourcePack.BrandingType resourcePackBrand = workshop.Branding.ResourcePackBrand;
			foreach (string text in listOfSubscribedResourcePackPaths)
			{
				try
				{
					string folderName = Path.GetFileName(text);
					if (resourcePacks.All((ResourcePack pack) => pack.FileName != folderName))
					{
						resourcePacks.Add(new ResourcePack(services, text, resourcePackBrand));
					}
				}
				catch (Exception arg)
				{
					Console.WriteLine("Failed to read resource pack {0}: {1}", text, arg);
				}
			}
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x004BB5EC File Offset: 0x004B97EC
		private static IEnumerable<ResourcePackList.ResourcePackEntry> CreatePackEntryListFromJson(JArray serializedState)
		{
			try
			{
				if (serializedState != null && serializedState.Count != 0)
				{
					return serializedState.ToObject<List<ResourcePackList.ResourcePackEntry>>();
				}
			}
			catch (JsonReaderException arg)
			{
				Console.WriteLine("Failed to parse configuration entry for resource pack list. {0}", arg);
			}
			return new List<ResourcePackList.ResourcePackEntry>();
		}

		// Token: 0x04001074 RID: 4212
		private readonly List<ResourcePack> _resourcePacks = new List<ResourcePack>();

		// Token: 0x0200065F RID: 1631
		private struct ResourcePackEntry
		{
			// Token: 0x06003D7D RID: 15741 RVA: 0x00691AAF File Offset: 0x0068FCAF
			public ResourcePackEntry(string name, bool enabled, int sortingOrder)
			{
				this.FileName = name;
				this.Enabled = enabled;
				this.SortingOrder = sortingOrder;
			}

			// Token: 0x0400662B RID: 26155
			public string FileName;

			// Token: 0x0400662C RID: 26156
			public bool Enabled;

			// Token: 0x0400662D RID: 26157
			public int SortingOrder;
		}
	}
}
