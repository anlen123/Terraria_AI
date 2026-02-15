using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria
{
	// Token: 0x0200003D RID: 61
	public class RecipeGroup
	{
		// Token: 0x060004B8 RID: 1208 RVA: 0x0012BFA1 File Offset: 0x0012A1A1
		private static Func<string> WithDefaultCombineFormat(string key)
		{
			LocalizedText text = Language.GetText(key);
			return () => RecipeGroup.DefaultCombineFormat.Format(text);
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0012BFBF File Offset: 0x0012A1BF
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x0012BFC7 File Offset: 0x0012A1C7
		public int RegisteredId { get; private set; }

		// Token: 0x060004BB RID: 1211 RVA: 0x0012BFD0 File Offset: 0x0012A1D0
		public RecipeGroup(string groupDescriptorKey, params int[] validItems) : this(RecipeGroup.WithDefaultCombineFormat(groupDescriptorKey), validItems)
		{
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0012BFE0 File Offset: 0x0012A1E0
		public RecipeGroup(Func<string> getName, params int[] validItems)
		{
			this.RegisteredId = -1;
			this.GetText = getName;
			foreach (int itemID in validItems)
			{
				this.Add(itemID, null);
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0012C034 File Offset: 0x0012A234
		public RecipeGroup Add(int itemID, Func<bool> isPreferred = null)
		{
			this.ValidItems.Add(itemID);
			this.Items.Add(itemID);
			return this;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0012C050 File Offset: 0x0012A250
		internal void SortDecraftingEntries()
		{
			this.DecraftItemId = (from e in this.Items
			orderby ContentSamples.ItemsByType[e].value
			select e).First<int>();
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0012C087 File Offset: 0x0012A287
		public override string ToString()
		{
			return this.GetText();
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0012C094 File Offset: 0x0012A294
		public RecipeGroup Register()
		{
			if (this.RegisteredId >= 0)
			{
				throw new Exception("Already registered");
			}
			int num = RecipeGroup.nextRecipeGroupIndex++;
			this.RegisteredId = num;
			RecipeGroup.recipeGroups.Add(num, this);
			return this;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0012C0D8 File Offset: 0x0012A2D8
		public int CountUsableItems(Dictionary<int, int> itemStacksAvailable)
		{
			int num = 0;
			foreach (int key in this.ValidItems)
			{
				int num2;
				if (itemStacksAvailable.TryGetValue(key, out num2))
				{
					num += num2;
				}
			}
			return num;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0012C138 File Offset: 0x0012A338
		public int GetGroupFakeItemId()
		{
			return this.RegisteredId + RecipeGroup.FakeItemIdOffset;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0012C146 File Offset: 0x0012A346
		public bool Contains(int itemType)
		{
			return this.ValidItems.Contains(itemType);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0012C154 File Offset: 0x0012A354
		public int GetPlaceholderItemType()
		{
			return this.Items[0];
		}

		// Token: 0x040002EA RID: 746
		public static readonly int FakeItemIdOffset = 1000000;

		// Token: 0x040002EB RID: 747
		public static LocalizedText DefaultCombineFormat = Language.GetText("CombineFormat.RecipeGroup");

		// Token: 0x040002ED RID: 749
		public Func<string> GetText;

		// Token: 0x040002EE RID: 750
		public HashSet<int> ValidItems = new HashSet<int>();

		// Token: 0x040002EF RID: 751
		public List<int> Items = new List<int>();

		// Token: 0x040002F0 RID: 752
		public int DecraftItemId;

		// Token: 0x040002F1 RID: 753
		public static Dictionary<int, RecipeGroup> recipeGroups = new Dictionary<int, RecipeGroup>();

		// Token: 0x040002F2 RID: 754
		public static int nextRecipeGroupIndex;
	}
}
