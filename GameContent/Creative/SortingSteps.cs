using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Creative
{
	// Token: 0x02000323 RID: 803
	public static class SortingSteps
	{
		// Token: 0x0200088E RID: 2190
		public abstract class ACreativeItemSortStep : ICreativeItemSortStep, IEntrySortStep<Item>, IComparer<Item>
		{
			// Token: 0x060044AA RID: 17578
			public abstract string GetDisplayNameKey();

			// Token: 0x060044AB RID: 17579
			public abstract int Compare(Item x, Item y);
		}

		// Token: 0x0200088F RID: 2191
		public abstract class AStepByFittingFilter : SortingSteps.ACreativeItemSortStep
		{
			// Token: 0x060044AD RID: 17581 RVA: 0x006C1DE0 File Offset: 0x006BFFE0
			public override int Compare(Item x, Item y)
			{
				int num = this.FitsFilter(x).CompareTo(this.FitsFilter(y));
				if (num == 0)
				{
					num = 1;
				}
				return num;
			}

			// Token: 0x060044AE RID: 17582
			public abstract bool FitsFilter(Item item);

			// Token: 0x060044AF RID: 17583 RVA: 0x006C1E0A File Offset: 0x006C000A
			public virtual int CompareWhenBothFit(Item x, Item y)
			{
				return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
			}
		}

		// Token: 0x02000890 RID: 2192
		public class Blocks : SortingSteps.AStepByFittingFilter
		{
			// Token: 0x060044B1 RID: 17585 RVA: 0x006C1E26 File Offset: 0x006C0026
			public override string GetDisplayNameKey()
			{
				return "CreativePowers.Sort_Blocks";
			}

			// Token: 0x060044B2 RID: 17586 RVA: 0x006C1E2D File Offset: 0x006C002D
			public override bool FitsFilter(Item item)
			{
				return item.createTile >= 0 && !Main.tileFrameImportant[item.createTile];
			}
		}

		// Token: 0x02000891 RID: 2193
		public class Walls : SortingSteps.AStepByFittingFilter
		{
			// Token: 0x060044B4 RID: 17588 RVA: 0x006C1E51 File Offset: 0x006C0051
			public override string GetDisplayNameKey()
			{
				return "CreativePowers.Sort_Walls";
			}

			// Token: 0x060044B5 RID: 17589 RVA: 0x006C1E58 File Offset: 0x006C0058
			public override bool FitsFilter(Item item)
			{
				return item.createWall >= 0;
			}
		}

		// Token: 0x02000892 RID: 2194
		public class PlacableObjects : SortingSteps.AStepByFittingFilter
		{
			// Token: 0x060044B7 RID: 17591 RVA: 0x006C1E66 File Offset: 0x006C0066
			public override string GetDisplayNameKey()
			{
				return "CreativePowers.Sort_PlacableObjects";
			}

			// Token: 0x060044B8 RID: 17592 RVA: 0x006C1E6D File Offset: 0x006C006D
			public override bool FitsFilter(Item item)
			{
				return item.createTile >= 0 && Main.tileFrameImportant[item.createTile];
			}
		}

		// Token: 0x02000893 RID: 2195
		public class ByUnlockStatus : SortingSteps.ACreativeItemSortStep
		{
			// Token: 0x060044BA RID: 17594 RVA: 0x006C1E86 File Offset: 0x006C0086
			public override string GetDisplayNameKey()
			{
				return "CreativePowers.Sort_UnlockedFirst";
			}

			// Token: 0x060044BB RID: 17595 RVA: 0x006C1E90 File Offset: 0x006C0090
			public override int Compare(Item x, Item y)
			{
				ItemsSacrificedUnlocksTracker itemSacrifices = Main.LocalPlayerCreativeTracker.ItemSacrifices;
				bool flag = itemSacrifices.IsNewlyResearched(x.type);
				bool flag2 = itemSacrifices.IsNewlyResearched(y.type);
				if (flag != flag2)
				{
					if (!flag)
					{
						return 1;
					}
					return -1;
				}
				else
				{
					bool flag3 = itemSacrifices.IsFullyResearched(x.type);
					bool flag4 = itemSacrifices.IsFullyResearched(y.type);
					if (flag3 == flag4)
					{
						return 0;
					}
					if (!flag3)
					{
						return 1;
					}
					return -1;
				}
			}
		}

		// Token: 0x02000894 RID: 2196
		public class ByCreativeSortingId : SortingSteps.ACreativeItemSortStep
		{
			// Token: 0x060044BD RID: 17597 RVA: 0x006C1EF5 File Offset: 0x006C00F5
			public override string GetDisplayNameKey()
			{
				return "CreativePowers.Sort_SortingID";
			}

			// Token: 0x060044BE RID: 17598 RVA: 0x006C1EFC File Offset: 0x006C00FC
			public override int Compare(Item x, Item y)
			{
				ContentSamples.CreativeHelper.ItemGroupAndOrderInGroup itemGroupAndOrderInGroup = ContentSamples.ItemCreativeSortingId[x.type];
				ContentSamples.CreativeHelper.ItemGroupAndOrderInGroup itemGroupAndOrderInGroup2 = ContentSamples.ItemCreativeSortingId[y.type];
				int num = itemGroupAndOrderInGroup.Group.CompareTo(itemGroupAndOrderInGroup2.Group);
				if (num == 0)
				{
					num = itemGroupAndOrderInGroup.OrderInGroup.CompareTo(itemGroupAndOrderInGroup2.OrderInGroup);
				}
				return num;
			}
		}

		// Token: 0x02000895 RID: 2197
		public class Alphabetical : SortingSteps.ACreativeItemSortStep
		{
			// Token: 0x060044C0 RID: 17600 RVA: 0x006C1F60 File Offset: 0x006C0160
			public override string GetDisplayNameKey()
			{
				return "CreativePowers.Sort_Alphabetical";
			}

			// Token: 0x060044C1 RID: 17601 RVA: 0x006C1F68 File Offset: 0x006C0168
			public override int Compare(Item x, Item y)
			{
				string name = x.Name;
				string name2 = y.Name;
				return name.CompareTo(name2);
			}
		}
	}
}
