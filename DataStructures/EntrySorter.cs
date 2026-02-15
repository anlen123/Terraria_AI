using System;
using System.Collections.Generic;
using Terraria.Localization;

namespace Terraria.DataStructures
{
	// Token: 0x02000544 RID: 1348
	public class EntrySorter<TEntryType, TStepType> : IComparer<TEntryType> where TEntryType : new() where TStepType : IEntrySortStep<TEntryType>
	{
		// Token: 0x0600375E RID: 14174 RVA: 0x0062DFF9 File Offset: 0x0062C1F9
		public void AddSortSteps(List<TStepType> sortSteps)
		{
			this.Steps.AddRange(sortSteps);
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x0062E008 File Offset: 0x0062C208
		public int Compare(TEntryType x, TEntryType y)
		{
			int num = 0;
			if (this._prioritizedStep != -1)
			{
				TStepType tstepType = this.Steps[this._prioritizedStep];
				num = tstepType.Compare(x, y);
				if (num != 0)
				{
					return num;
				}
			}
			for (int i = 0; i < this.Steps.Count; i++)
			{
				if (i != this._prioritizedStep)
				{
					TStepType tstepType = this.Steps[i];
					num = tstepType.Compare(x, y);
					if (num != 0)
					{
						return num;
					}
				}
			}
			return num;
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x0062E089 File Offset: 0x0062C289
		public void SetPrioritizedStepIndex(int index)
		{
			this._prioritizedStep = index;
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x0062E094 File Offset: 0x0062C294
		public string GetDisplayName()
		{
			TStepType tstepType = this.Steps[this._prioritizedStep];
			return Language.GetTextValue(tstepType.GetDisplayNameKey());
		}

		// Token: 0x04005B72 RID: 23410
		public List<TStepType> Steps = new List<TStepType>();

		// Token: 0x04005B73 RID: 23411
		private int _prioritizedStep;
	}
}
