using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Creative
{
	// Token: 0x02000321 RID: 801
	public static class ItemFilters
	{
		// Token: 0x040050D9 RID: 20697
		private const int framesPerRow = 11;

		// Token: 0x040050DA RID: 20698
		private const int framesPerColumn = 1;

		// Token: 0x040050DB RID: 20699
		private const int frameSizeOffsetX = -2;

		// Token: 0x040050DC RID: 20700
		private const int frameSizeOffsetY = 0;

		// Token: 0x0200087F RID: 2175
		public class BySearch : IItemEntryFilter, IEntryFilter<Item>, ISearchFilter<Item>
		{
			// Token: 0x06004471 RID: 17521 RVA: 0x006C1394 File Offset: 0x006BF594
			public bool FitsFilter(Item entry)
			{
				if (this._search == null)
				{
					return true;
				}
				int num = 1;
				float knockBack = entry.knockBack;
				int stack = entry.stack;
				entry.stack = 1;
				Main.MouseText_DrawItemTooltip_GetLinesInfo(entry, ref this._unusedYoyoLogo, ref this._unusedResearchLine, knockBack, ref num, this._toolTipLines, this._unusedColor);
				entry.stack = stack;
				for (int i = 0; i < num; i++)
				{
					if (this._toolTipLines[i].IndexOf(this._search, StringComparison.OrdinalIgnoreCase) != -1)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06004472 RID: 17522 RVA: 0x006C1410 File Offset: 0x006BF610
			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabSearch";
			}

			// Token: 0x06004473 RID: 17523 RVA: 0x006C1417 File Offset: 0x006BF617
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Rank_Light", 1);
				return new UIImageFramed(asset, asset.Frame(1, 1, 0, 0, 0, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}

			// Token: 0x06004474 RID: 17524 RVA: 0x006C1450 File Offset: 0x006BF650
			public void SetSearch(string searchText)
			{
				this._search = searchText;
			}

			// Token: 0x04007273 RID: 29299
			private const int _tooltipMaxLines = 30;

			// Token: 0x04007274 RID: 29300
			private string[] _toolTipLines = new string[30];

			// Token: 0x04007275 RID: 29301
			private Color[] _unusedColor = new Color[30];

			// Token: 0x04007276 RID: 29302
			private int _unusedYoyoLogo;

			// Token: 0x04007277 RID: 29303
			private int _unusedResearchLine;

			// Token: 0x04007278 RID: 29304
			private string _search;
		}

		// Token: 0x02000880 RID: 2176
		public class BuildingBlock : IItemEntryFilter, IEntryFilter<Item>
		{
			// Token: 0x06004475 RID: 17525 RVA: 0x006C1459 File Offset: 0x006BF659
			public bool FitsFilter(Item entry)
			{
				return entry.createWall != -1 || entry.tileWand != -1 || (entry.createTile != -1 && !Main.tileFrameImportant[entry.createTile]);
			}

			// Token: 0x06004476 RID: 17526 RVA: 0x006C148B File Offset: 0x006BF68B
			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabBlocks";
			}

			// Token: 0x06004477 RID: 17527 RVA: 0x006C1494 File Offset: 0x006BF694
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", 1);
				return new UIImageFramed(asset, asset.Frame(11, 1, 4, 0, 0, 0).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		// Token: 0x02000881 RID: 2177
		public class Furniture : IItemEntryFilter, IEntryFilter<Item>
		{
			// Token: 0x06004479 RID: 17529 RVA: 0x006C14E4 File Offset: 0x006BF6E4
			public bool FitsFilter(Item entry)
			{
				int createTile = entry.createTile;
				return createTile != -1 && Main.tileFrameImportant[createTile];
			}

			// Token: 0x0600447A RID: 17530 RVA: 0x006C150B File Offset: 0x006BF70B
			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabFurniture";
			}

			// Token: 0x0600447B RID: 17531 RVA: 0x006C1514 File Offset: 0x006BF714
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", 1);
				return new UIImageFramed(asset, asset.Frame(11, 1, 7, 0, 0, 0).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		// Token: 0x02000882 RID: 2178
		public class Tools : IItemEntryFilter, IEntryFilter<Item>
		{
			// Token: 0x0600447D RID: 17533 RVA: 0x006C1564 File Offset: 0x006BF764
			public bool FitsFilter(Item entry)
			{
				return entry.pick > 0 || entry.axe > 0 || entry.hammer > 0 || entry.fishingPole > 0 || entry.tileWand != -1 || this._itemIdsThatAreAccepted.Contains(entry.type);
			}

			// Token: 0x0600447E RID: 17534 RVA: 0x006C15BE File Offset: 0x006BF7BE
			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabTools";
			}

			// Token: 0x0600447F RID: 17535 RVA: 0x006C15C8 File Offset: 0x006BF7C8
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", 1);
				return new UIImageFramed(asset, asset.Frame(11, 1, 6, 0, 0, 0).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}

			// Token: 0x04007279 RID: 29305
			private HashSet<int> _itemIdsThatAreAccepted = new HashSet<int>
			{
				213,
				5295,
				509,
				850,
				851,
				3612,
				3625,
				3611,
				510,
				849,
				3620,
				1071,
				1543,
				1072,
				1544,
				1100,
				1545,
				50,
				3199,
				3124,
				5358,
				5359,
				5360,
				5361,
				5437,
				1326,
				5335,
				3384,
				4263,
				4819,
				4262,
				946,
				4707,
				205,
				206,
				207,
				1128,
				3031,
				4820,
				5302,
				5364,
				4460,
				4608,
				4872,
				3032,
				5303,
				5304,
				1991,
				4821,
				3183,
				779,
				5134,
				1299,
				4711,
				4049,
				114,
				5667
			};
		}

		// Token: 0x02000883 RID: 2179
		public class Weapon : IItemEntryFilter, IEntryFilter<Item>
		{
			// Token: 0x06004481 RID: 17537 RVA: 0x006C18DC File Offset: 0x006BFADC
			public bool FitsFilter(Item entry)
			{
				return entry.damage > 0;
			}

			// Token: 0x06004482 RID: 17538 RVA: 0x006C18E7 File Offset: 0x006BFAE7
			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabWeapons";
			}

			// Token: 0x06004483 RID: 17539 RVA: 0x006C18F0 File Offset: 0x006BFAF0
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", 1);
				return new UIImageFramed(asset, asset.Frame(11, 1, 0, 0, 0, 0).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		// Token: 0x02000884 RID: 2180
		public abstract class AArmor
		{
			// Token: 0x06004485 RID: 17541 RVA: 0x006C193D File Offset: 0x006BFB3D
			public bool IsAnArmorThatMatchesSocialState(Item entry, bool shouldBeSocial)
			{
				return (entry.bodySlot != -1 || entry.headSlot != -1 || entry.legSlot != -1) && entry.vanity == shouldBeSocial;
			}
		}

		// Token: 0x02000885 RID: 2181
		public class Armor : ItemFilters.AArmor, IItemEntryFilter, IEntryFilter<Item>
		{
			// Token: 0x06004487 RID: 17543 RVA: 0x006C196D File Offset: 0x006BFB6D
			public bool FitsFilter(Item entry)
			{
				return base.IsAnArmorThatMatchesSocialState(entry, false);
			}

			// Token: 0x06004488 RID: 17544 RVA: 0x006C1977 File Offset: 0x006BFB77
			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabArmor";
			}

			// Token: 0x06004489 RID: 17545 RVA: 0x006C1980 File Offset: 0x006BFB80
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", 1);
				return new UIImageFramed(asset, asset.Frame(11, 1, 2, 0, 0, 0).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		// Token: 0x02000886 RID: 2182
		public class Vanity : ItemFilters.AArmor, IItemEntryFilter, IEntryFilter<Item>
		{
			// Token: 0x0600448B RID: 17547 RVA: 0x006C19D5 File Offset: 0x006BFBD5
			public bool FitsFilter(Item entry)
			{
				return base.IsAnArmorThatMatchesSocialState(entry, true);
			}

			// Token: 0x0600448C RID: 17548 RVA: 0x006C19DF File Offset: 0x006BFBDF
			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabVanity";
			}

			// Token: 0x0600448D RID: 17549 RVA: 0x006C19E8 File Offset: 0x006BFBE8
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", 1);
				return new UIImageFramed(asset, asset.Frame(11, 1, 8, 0, 0, 0).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		// Token: 0x02000887 RID: 2183
		public abstract class AAccessories
		{
			// Token: 0x0600448F RID: 17551 RVA: 0x006C1A38 File Offset: 0x006BFC38
			public bool IsAnAccessoryOfType(Item entry, ItemFilters.AAccessories.AccessoriesCategory categoryType)
			{
				bool flag = ItemFilters.AAccessories.IsMiscEquipment(entry);
				return (flag && categoryType == ItemFilters.AAccessories.AccessoriesCategory.Misc) || (!flag && categoryType == ItemFilters.AAccessories.AccessoriesCategory.NonMisc && entry.accessory);
			}

			// Token: 0x06004490 RID: 17552 RVA: 0x006C1A68 File Offset: 0x006BFC68
			public static bool IsMiscEquipment(Item item)
			{
				return item.mountType != -1 || (item.buffType > 0 && Main.lightPet[item.buffType]) || (item.buffType > 0 && Main.vanityPet[item.buffType]) || Main.projHook[item.shoot];
			}

			// Token: 0x02000AD6 RID: 2774
			public enum AccessoriesCategory
			{
				// Token: 0x0400784B RID: 30795
				Misc,
				// Token: 0x0400784C RID: 30796
				NonMisc
			}
		}

		// Token: 0x02000888 RID: 2184
		public class Accessories : ItemFilters.AAccessories, IItemEntryFilter, IEntryFilter<Item>
		{
			// Token: 0x06004492 RID: 17554 RVA: 0x006C1ABA File Offset: 0x006BFCBA
			public bool FitsFilter(Item entry)
			{
				return base.IsAnAccessoryOfType(entry, ItemFilters.AAccessories.AccessoriesCategory.NonMisc);
			}

			// Token: 0x06004493 RID: 17555 RVA: 0x006C1AC4 File Offset: 0x006BFCC4
			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabAccessories";
			}

			// Token: 0x06004494 RID: 17556 RVA: 0x006C1ACC File Offset: 0x006BFCCC
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", 1);
				return new UIImageFramed(asset, asset.Frame(11, 1, 1, 0, 0, 0).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		// Token: 0x02000889 RID: 2185
		public class MiscAccessories : ItemFilters.AAccessories, IItemEntryFilter, IEntryFilter<Item>
		{
			// Token: 0x06004496 RID: 17558 RVA: 0x006C1B21 File Offset: 0x006BFD21
			public bool FitsFilter(Item entry)
			{
				return base.IsAnAccessoryOfType(entry, ItemFilters.AAccessories.AccessoriesCategory.Misc);
			}

			// Token: 0x06004497 RID: 17559 RVA: 0x006C1B2B File Offset: 0x006BFD2B
			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabAccessoriesMisc";
			}

			// Token: 0x06004498 RID: 17560 RVA: 0x006C1B34 File Offset: 0x006BFD34
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", 1);
				return new UIImageFramed(asset, asset.Frame(11, 1, 9, 0, 0, 0).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		// Token: 0x0200088A RID: 2186
		public class Consumables : IItemEntryFilter, IEntryFilter<Item>
		{
			// Token: 0x0600449A RID: 17562 RVA: 0x006C1B84 File Offset: 0x006BFD84
			public bool FitsFilter(Item entry)
			{
				int type = entry.type;
				if (type == 267 || type == 1307)
				{
					return true;
				}
				bool flag = entry.createTile != -1 || entry.createWall != -1 || entry.tileWand != -1;
				return entry.consumable && !flag;
			}

			// Token: 0x0600449B RID: 17563 RVA: 0x006C1BDA File Offset: 0x006BFDDA
			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabConsumables";
			}

			// Token: 0x0600449C RID: 17564 RVA: 0x006C1BE4 File Offset: 0x006BFDE4
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", 1);
				return new UIImageFramed(asset, asset.Frame(11, 1, 3, 0, 0, 0).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		// Token: 0x0200088B RID: 2187
		public class GameplayItems : IItemEntryFilter, IEntryFilter<Item>
		{
			// Token: 0x0600449E RID: 17566 RVA: 0x006C1C31 File Offset: 0x006BFE31
			public bool FitsFilter(Item entry)
			{
				return ItemID.Sets.SortingPriorityMiscImportants[entry.type] != -1;
			}

			// Token: 0x0600449F RID: 17567 RVA: 0x006C1C45 File Offset: 0x006BFE45
			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabMisc";
			}

			// Token: 0x060044A0 RID: 17568 RVA: 0x006C1C4C File Offset: 0x006BFE4C
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", 1);
				return new UIImageFramed(asset, asset.Frame(11, 1, 5, 0, 0, 0).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		// Token: 0x0200088C RID: 2188
		public class MiscFallback : IItemEntryFilter, IEntryFilter<Item>
		{
			// Token: 0x060044A2 RID: 17570 RVA: 0x006C1C99 File Offset: 0x006BFE99
			public MiscFallback(List<IItemEntryFilter> otherFiltersToCheckAgainst)
			{
				this.otherFiltersToCheckAgainst = otherFiltersToCheckAgainst;
			}

			// Token: 0x060044A3 RID: 17571 RVA: 0x006C1CB8 File Offset: 0x006BFEB8
			public bool FitsFilter(Item entry)
			{
				bool? flag = this._fitsFilterByItemType[entry.type];
				if (flag == null)
				{
					bool?[] fitsFilterByItemType = this._fitsFilterByItemType;
					int type = entry.type;
					flag = new bool?(!this.otherFiltersToCheckAgainst.Any((IItemEntryFilter f) => f.FitsFilter(entry)));
					fitsFilterByItemType[type] = flag;
				}
				return flag.Value;
			}

			// Token: 0x060044A4 RID: 17572 RVA: 0x006C1C45 File Offset: 0x006BFE45
			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabMisc";
			}

			// Token: 0x060044A5 RID: 17573 RVA: 0x006C1D34 File Offset: 0x006BFF34
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", 1);
				return new UIImageFramed(asset, asset.Frame(11, 1, 5, 0, 0, 0).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}

			// Token: 0x0400727A RID: 29306
			private readonly List<IItemEntryFilter> otherFiltersToCheckAgainst;

			// Token: 0x0400727B RID: 29307
			private bool?[] _fitsFilterByItemType = new bool?[(int)ItemID.Count];
		}

		// Token: 0x0200088D RID: 2189
		public class Materials : IItemEntryFilter, IEntryFilter<Item>
		{
			// Token: 0x060044A6 RID: 17574 RVA: 0x006C1D81 File Offset: 0x006BFF81
			public bool FitsFilter(Item entry)
			{
				return entry.material;
			}

			// Token: 0x060044A7 RID: 17575 RVA: 0x006C1D89 File Offset: 0x006BFF89
			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabMaterials";
			}

			// Token: 0x060044A8 RID: 17576 RVA: 0x006C1D90 File Offset: 0x006BFF90
			public UIElement GetImage()
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", 1);
				return new UIImageFramed(asset, asset.Frame(11, 1, 10, 0, 0, 0).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}
	}
}
