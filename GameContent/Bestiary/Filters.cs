using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000330 RID: 816
	public static class Filters
	{
		// Token: 0x020008AF RID: 2223
		public class BySearch : IBestiaryEntryFilter, IEntryFilter<BestiaryEntry>, ISearchFilter<BestiaryEntry>
		{
			// Token: 0x1700055C RID: 1372
			// (get) Token: 0x060045D2 RID: 17874 RVA: 0x006C4853 File Offset: 0x006C2A53
			public bool? ForcedDisplay
			{
				get
				{
					return new bool?(true);
				}
			}

			// Token: 0x060045D4 RID: 17876 RVA: 0x006C485C File Offset: 0x006C2A5C
			public bool FitsFilter(BestiaryEntry entry)
			{
				if (this._search == null)
				{
					return true;
				}
				BestiaryUICollectionInfo entryUICollectionInfo = entry.UIInfoProvider.GetEntryUICollectionInfo();
				for (int i = 0; i < entry.Info.Count; i++)
				{
					IProvideSearchFilterString provideSearchFilterString = entry.Info[i] as IProvideSearchFilterString;
					if (provideSearchFilterString != null)
					{
						string searchString = provideSearchFilterString.GetSearchString(ref entryUICollectionInfo);
						if (searchString != null && searchString.ToLower().IndexOf(this._search, StringComparison.OrdinalIgnoreCase) != -1)
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x060045D5 RID: 17877 RVA: 0x006C48CE File Offset: 0x006C2ACE
			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.IfSearched";
			}

			// Token: 0x060045D6 RID: 17878 RVA: 0x006C1417 File Offset: 0x006BF617
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Rank_Light", 1);
				return new UIImageFramed(asset, asset.Frame(1, 1, 0, 0, 0, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}

			// Token: 0x060045D7 RID: 17879 RVA: 0x006C48D5 File Offset: 0x006C2AD5
			public void SetSearch(string searchText)
			{
				this._search = searchText;
			}

			// Token: 0x040072F5 RID: 29429
			private string _search;
		}

		// Token: 0x020008B0 RID: 2224
		public class ByUnlockState : IBestiaryEntryFilter, IEntryFilter<BestiaryEntry>
		{
			// Token: 0x1700055D RID: 1373
			// (get) Token: 0x060045D8 RID: 17880 RVA: 0x006C4853 File Offset: 0x006C2A53
			public bool? ForcedDisplay
			{
				get
				{
					return new bool?(true);
				}
			}

			// Token: 0x060045D9 RID: 17881 RVA: 0x006C48E0 File Offset: 0x006C2AE0
			public bool FitsFilter(BestiaryEntry entry)
			{
				BestiaryUICollectionInfo entryUICollectionInfo = entry.UIInfoProvider.GetEntryUICollectionInfo();
				return entry.Icon.GetUnlockState(entryUICollectionInfo);
			}

			// Token: 0x060045DA RID: 17882 RVA: 0x006C4905 File Offset: 0x006C2B05
			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.IfUnlocked";
			}

			// Token: 0x060045DB RID: 17883 RVA: 0x006C490C File Offset: 0x006C2B0C
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow", 1);
				return new UIImageFramed(asset, asset.Frame(16, 5, 14, 3, 0, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		// Token: 0x020008B1 RID: 2225
		public class ByRareCreature : IBestiaryEntryFilter, IEntryFilter<BestiaryEntry>
		{
			// Token: 0x1700055E RID: 1374
			// (get) Token: 0x060045DD RID: 17885 RVA: 0x006C4948 File Offset: 0x006C2B48
			public bool? ForcedDisplay
			{
				get
				{
					return null;
				}
			}

			// Token: 0x060045DE RID: 17886 RVA: 0x006C4960 File Offset: 0x006C2B60
			public bool FitsFilter(BestiaryEntry entry)
			{
				for (int i = 0; i < entry.Info.Count; i++)
				{
					if (entry.Info[i] is RareSpawnBestiaryInfoElement)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060045DF RID: 17887 RVA: 0x006C4999 File Offset: 0x006C2B99
			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.IsRare";
			}

			// Token: 0x060045E0 RID: 17888 RVA: 0x006C1417 File Offset: 0x006BF617
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Rank_Light", 1);
				return new UIImageFramed(asset, asset.Frame(1, 1, 0, 0, 0, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		// Token: 0x020008B2 RID: 2226
		public class ByBoss : IBestiaryEntryFilter, IEntryFilter<BestiaryEntry>
		{
			// Token: 0x1700055F RID: 1375
			// (get) Token: 0x060045E2 RID: 17890 RVA: 0x006C49A0 File Offset: 0x006C2BA0
			public bool? ForcedDisplay
			{
				get
				{
					return null;
				}
			}

			// Token: 0x060045E3 RID: 17891 RVA: 0x006C49B8 File Offset: 0x006C2BB8
			public bool FitsFilter(BestiaryEntry entry)
			{
				for (int i = 0; i < entry.Info.Count; i++)
				{
					if (entry.Info[i] is BossBestiaryInfoElement)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060045E4 RID: 17892 RVA: 0x006C49F1 File Offset: 0x006C2BF1
			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.IsBoss";
			}

			// Token: 0x060045E5 RID: 17893 RVA: 0x006C49F8 File Offset: 0x006C2BF8
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow", 1);
				return new UIImageFramed(asset, asset.Frame(16, 5, 15, 3, 0, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		// Token: 0x020008B3 RID: 2227
		public class ByInfoElement : IBestiaryEntryFilter, IEntryFilter<BestiaryEntry>
		{
			// Token: 0x17000560 RID: 1376
			// (get) Token: 0x060045E7 RID: 17895 RVA: 0x006C4A34 File Offset: 0x006C2C34
			public bool? ForcedDisplay
			{
				get
				{
					return null;
				}
			}

			// Token: 0x060045E8 RID: 17896 RVA: 0x006C4A4A File Offset: 0x006C2C4A
			public ByInfoElement(IBestiaryInfoElement element)
			{
				this._element = element;
			}

			// Token: 0x060045E9 RID: 17897 RVA: 0x006C4A59 File Offset: 0x006C2C59
			public bool FitsFilter(BestiaryEntry entry)
			{
				return entry.Info.Contains(this._element);
			}

			// Token: 0x060045EA RID: 17898 RVA: 0x006C4A6C File Offset: 0x006C2C6C
			public string GetDisplayNameKey()
			{
				IFilterInfoProvider filterInfoProvider = this._element as IFilterInfoProvider;
				if (filterInfoProvider == null)
				{
					return null;
				}
				return filterInfoProvider.GetDisplayNameKey();
			}

			// Token: 0x060045EB RID: 17899 RVA: 0x006C4A90 File Offset: 0x006C2C90
			public UIElement GetImage()
			{
				IFilterInfoProvider filterInfoProvider = this._element as IFilterInfoProvider;
				if (filterInfoProvider == null)
				{
					return null;
				}
				return filterInfoProvider.GetFilterImage();
			}

			// Token: 0x040072F6 RID: 29430
			private IBestiaryInfoElement _element;
		}
	}
}
