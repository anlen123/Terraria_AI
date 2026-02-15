using System;
using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;
using Terraria.IO;

namespace Terraria.GameContent.UI.ResourceSets
{
	// Token: 0x020003BD RID: 957
	public class PlayerResourceSetsManager
	{
		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06002CFB RID: 11515 RVA: 0x005A1223 File Offset: 0x0059F423
		// (set) Token: 0x06002CFC RID: 11516 RVA: 0x005A122B File Offset: 0x0059F42B
		public string ActiveSetKeyName { get; private set; }

		// Token: 0x06002CFD RID: 11517 RVA: 0x005A1234 File Offset: 0x0059F434
		public void BindTo(Preferences preferences)
		{
			preferences.OnLoad += this.Configuration_OnLoad;
			preferences.OnSave += this.Configuration_OnSave;
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x005A125A File Offset: 0x0059F45A
		private void Configuration_OnLoad(Preferences obj)
		{
			this._activeSetConfigKey = obj.Get<string>("PlayerResourcesSet", "New");
			if (this._loadedContent)
			{
				this.SetActiveFromLoadedConfigKey();
			}
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x005A1280 File Offset: 0x0059F480
		private void Configuration_OnSave(Preferences obj)
		{
			obj.Put("PlayerResourcesSet", this._activeSetConfigKey);
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x005A1294 File Offset: 0x0059F494
		public void LoadContent(AssetRequestMode mode)
		{
			this._sets["New"] = new FancyClassicPlayerResourcesDisplaySet("New", "New", "FancyClassic", mode);
			this._sets["Default"] = new ClassicPlayerResourcesDisplaySet("Default", "Default");
			this._sets["HorizontalBarsWithFullText"] = new HorizontalBarsPlayerResourcesDisplaySet("HorizontalBarsWithFullText", "HorizontalBarsWithFullText", "HorizontalBars", mode);
			this._sets["HorizontalBarsWithText"] = new HorizontalBarsPlayerResourcesDisplaySet("HorizontalBarsWithText", "HorizontalBarsWithText", "HorizontalBars", mode);
			this._sets["HorizontalBars"] = new HorizontalBarsPlayerResourcesDisplaySet("HorizontalBars", "HorizontalBars", "HorizontalBars", mode);
			this._sets["NewWithText"] = new FancyClassicPlayerResourcesDisplaySet("NewWithText", "NewWithText", "FancyClassic", mode);
			this._loadedContent = true;
			this.SetActiveFromLoadedConfigKey();
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x005A1386 File Offset: 0x0059F586
		public void SetActiveFromLoadedConfigKey()
		{
			this.SetActive(this._activeSetConfigKey);
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x005A1394 File Offset: 0x0059F594
		private void SetActive(string name)
		{
			IPlayerResourcesDisplaySet playerResourcesDisplaySet = this._sets.FirstOrDefault((KeyValuePair<string, IPlayerResourcesDisplaySet> pair) => pair.Key == name).Value;
			if (playerResourcesDisplaySet == null)
			{
				playerResourcesDisplaySet = this._sets.Values.First<IPlayerResourcesDisplaySet>();
			}
			this.SetActiveFrame(playerResourcesDisplaySet);
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x005A13E9 File Offset: 0x0059F5E9
		private void SetActiveFrame(IPlayerResourcesDisplaySet set)
		{
			this._activeSet = set;
			this._activeSetConfigKey = set.ConfigKey;
			this.ActiveSetKeyName = set.NameKey;
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x005A140A File Offset: 0x0059F60A
		public void TryToHoverOverResources()
		{
			this._activeSet.TryToHover();
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x005A1417 File Offset: 0x0059F617
		public void Draw()
		{
			this._activeSet.Draw();
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x005A1424 File Offset: 0x0059F624
		public void CycleResourceSet()
		{
			IPlayerResourcesDisplaySet lastFrame = null;
			this._sets.Values.FirstOrDefault(delegate(IPlayerResourcesDisplaySet frame)
			{
				if (frame == this._activeSet)
				{
					return true;
				}
				lastFrame = frame;
				return false;
			});
			if (lastFrame == null)
			{
				lastFrame = this._sets.Values.Last<IPlayerResourcesDisplaySet>();
			}
			this.SetActiveFrame(lastFrame);
		}

		// Token: 0x0400545D RID: 21597
		private Dictionary<string, IPlayerResourcesDisplaySet> _sets = new Dictionary<string, IPlayerResourcesDisplaySet>();

		// Token: 0x0400545E RID: 21598
		private IPlayerResourcesDisplaySet _activeSet;

		// Token: 0x0400545F RID: 21599
		private string _activeSetConfigKey;

		// Token: 0x04005460 RID: 21600
		private bool _loadedContent;
	}
}
