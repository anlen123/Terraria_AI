using System;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.IO;

namespace Terraria.GameContent.UI.ResourceSets
{
	// Token: 0x020003BC RID: 956
	public class PlayerResourceSetsManager2 : SelectionHolder<IPlayerResourcesDisplaySet>
	{
		// Token: 0x06002CF5 RID: 11509 RVA: 0x005A10EB File Offset: 0x0059F2EB
		protected override void Configuration_Save(Preferences obj)
		{
			obj.Put("PlayerResourcesSet", this.ActiveSelectionConfigKey);
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x005A10FE File Offset: 0x0059F2FE
		protected override void Configuration_OnLoad(Preferences obj)
		{
			this.ActiveSelectionConfigKey = Main.Configuration.Get<string>("PlayerResourcesSet", "New");
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x005A111C File Offset: 0x0059F31C
		protected override void PopulateOptionsAndLoadContent(AssetRequestMode mode)
		{
			this.Options["New"] = new FancyClassicPlayerResourcesDisplaySet("New", "New", "FancyClassic", mode);
			this.Options["Default"] = new ClassicPlayerResourcesDisplaySet("Default", "Default");
			this.Options["HorizontalBarsWithFullText"] = new HorizontalBarsPlayerResourcesDisplaySet("HorizontalBarsWithFullText", "HorizontalBarsWithFullText", "HorizontalBars", mode);
			this.Options["HorizontalBarsWithText"] = new HorizontalBarsPlayerResourcesDisplaySet("HorizontalBarsWithText", "HorizontalBarsWithText", "HorizontalBars", mode);
			this.Options["HorizontalBars"] = new HorizontalBarsPlayerResourcesDisplaySet("HorizontalBars", "HorizontalBars", "HorizontalBars", mode);
			this.Options["NewWithText"] = new FancyClassicPlayerResourcesDisplaySet("NewWithText", "NewWithText", "FancyClassic", mode);
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x005A1201 File Offset: 0x0059F401
		public void TryToHoverOverResources()
		{
			this.ActiveSelection.TryToHover();
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x005A120E File Offset: 0x0059F40E
		public void Draw()
		{
			this.ActiveSelection.Draw();
		}
	}
}
