using System;
using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;
using ReLogic.Content.Sources;
using Terraria.Audio;
using Terraria.IO;
using Terraria.Localization;

namespace Terraria.GameContent
{
	// Token: 0x02000276 RID: 630
	public class AssetSourceController
	{
		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06002428 RID: 9256 RVA: 0x0054AF64 File Offset: 0x00549164
		// (remove) Token: 0x06002429 RID: 9257 RVA: 0x0054AF9C File Offset: 0x0054919C
		public event Action<ResourcePackList> OnResourcePackChange;

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x0600242A RID: 9258 RVA: 0x0054AFD1 File Offset: 0x005491D1
		// (set) Token: 0x0600242B RID: 9259 RVA: 0x0054AFD9 File Offset: 0x005491D9
		public ResourcePackList ActiveResourcePackList { get; private set; }

		// Token: 0x0600242C RID: 9260 RVA: 0x0054AFE2 File Offset: 0x005491E2
		public AssetSourceController(IAssetRepository assetRepository, IEnumerable<IContentSource> staticSources)
		{
			this._assetRepository = assetRepository;
			this._staticSources = staticSources.ToList<IContentSource>();
			this.ActiveResourcePackList = new ResourcePackList();
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x0054B008 File Offset: 0x00549208
		public void Refresh()
		{
			foreach (ResourcePack resourcePack in this.ActiveResourcePackList.AllPacks)
			{
				resourcePack.GetContentSource().Refresh();
			}
			this.UseResourcePacks(this.ActiveResourcePackList);
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0054B068 File Offset: 0x00549268
		public void UseResourcePacks(ResourcePackList resourcePacks)
		{
			if (this.OnResourcePackChange != null)
			{
				this.OnResourcePackChange(resourcePacks);
			}
			this.ActiveResourcePackList = resourcePacks;
			List<IContentSource> list = new List<IContentSource>(from pack in resourcePacks.EnabledPacks
			orderby pack.SortingOrder
			select pack.GetContentSource());
			list.AddRange(this._staticSources);
			foreach (IContentSource contentSource in list)
			{
				contentSource.ClearRejections();
			}
			List<IContentSource> list2 = new List<IContentSource>();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list2.Add(list[i]);
			}
			this._assetRepository.SetSources(list, 1);
			LanguageManager.Instance.UseSources(list2);
			Main.audioSystem.UseSources(list2);
			SoundEngine.Reload();
			Main.changeTheTitle = true;
		}

		// Token: 0x04004DC3 RID: 19907
		private readonly List<IContentSource> _staticSources;

		// Token: 0x04004DC4 RID: 19908
		private readonly IAssetRepository _assetRepository;
	}
}
