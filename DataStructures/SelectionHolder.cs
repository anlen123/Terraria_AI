using System;
using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;
using Terraria.IO;

namespace Terraria.DataStructures
{
	// Token: 0x02000559 RID: 1369
	public abstract class SelectionHolder<TCycleType> where TCycleType : class, IConfigKeyHolder
	{
		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06003795 RID: 14229 RVA: 0x0062EAAE File Offset: 0x0062CCAE
		// (set) Token: 0x06003796 RID: 14230 RVA: 0x0062EAB6 File Offset: 0x0062CCB6
		public string ActiveSelectionKeyName { get; private set; }

		// Token: 0x06003797 RID: 14231 RVA: 0x0062EABF File Offset: 0x0062CCBF
		public void BindTo(Preferences preferences)
		{
			preferences.OnLoad += this.Wrapped_Configuration_OnLoad;
			preferences.OnSave += this.Configuration_Save;
		}

		// Token: 0x06003798 RID: 14232
		protected abstract void Configuration_Save(Preferences obj);

		// Token: 0x06003799 RID: 14233
		protected abstract void Configuration_OnLoad(Preferences obj);

		// Token: 0x0600379A RID: 14234 RVA: 0x0062EAE6 File Offset: 0x0062CCE6
		protected void Wrapped_Configuration_OnLoad(Preferences obj)
		{
			this.Configuration_OnLoad(obj);
			if (this.LoadedContent)
			{
				this.SetActiveMinimapFromLoadedConfigKey();
			}
		}

		// Token: 0x0600379B RID: 14235
		protected abstract void PopulateOptionsAndLoadContent(AssetRequestMode mode);

		// Token: 0x0600379C RID: 14236 RVA: 0x0062EAFD File Offset: 0x0062CCFD
		public void LoadContent(AssetRequestMode mode)
		{
			this.PopulateOptionsAndLoadContent(mode);
			this.LoadedContent = true;
			this.SetActiveMinimapFromLoadedConfigKey();
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x0062EB14 File Offset: 0x0062CD14
		public void CycleSelection()
		{
			TCycleType lastFrame = default(TCycleType);
			this.Options.Values.FirstOrDefault(delegate(TCycleType frame)
			{
				if (frame == this.ActiveSelection)
				{
					return true;
				}
				lastFrame = frame;
				return false;
			});
			if (lastFrame == null)
			{
				lastFrame = this.Options.Values.Last<TCycleType>();
			}
			this.SetActiveFrame(lastFrame);
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x0062EB86 File Offset: 0x0062CD86
		public void SetActiveMinimapFromLoadedConfigKey()
		{
			this.SetActiveFrame(this.ActiveSelectionConfigKey);
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x0062EB94 File Offset: 0x0062CD94
		private void SetActiveFrame(string frameName)
		{
			TCycleType tcycleType = this.Options.FirstOrDefault((KeyValuePair<string, TCycleType> pair) => pair.Key == frameName).Value;
			if (tcycleType == null)
			{
				tcycleType = this.Options.Values.First<TCycleType>();
			}
			this.SetActiveFrame(tcycleType);
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x0062EBEE File Offset: 0x0062CDEE
		private void SetActiveFrame(TCycleType frame)
		{
			this.ActiveSelection = frame;
			this.ActiveSelectionConfigKey = frame.ConfigKey;
			this.ActiveSelectionKeyName = frame.NameKey;
		}

		// Token: 0x04005B9F RID: 23455
		protected Dictionary<string, TCycleType> Options = new Dictionary<string, TCycleType>();

		// Token: 0x04005BA0 RID: 23456
		protected TCycleType ActiveSelection;

		// Token: 0x04005BA1 RID: 23457
		protected string ActiveSelectionConfigKey;

		// Token: 0x04005BA2 RID: 23458
		protected bool LoadedContent;
	}
}
