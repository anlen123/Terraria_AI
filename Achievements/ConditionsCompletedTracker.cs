using System;
using System.Collections.Generic;

namespace Terraria.Achievements
{
	// Token: 0x020005E3 RID: 1507
	public class ConditionsCompletedTracker : ConditionIntTracker
	{
		// Token: 0x06003B2B RID: 15147 RVA: 0x006594C6 File Offset: 0x006576C6
		public void AddCondition(AchievementCondition condition)
		{
			this._maxValue++;
			condition.OnComplete += this.OnConditionCompleted;
			this._conditions.Add(condition);
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x006594F4 File Offset: 0x006576F4
		private void OnConditionCompleted(AchievementCondition condition)
		{
			base.SetValue(Math.Min(this._value + 1, this._maxValue), true);
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x00659510 File Offset: 0x00657710
		protected override void Load()
		{
			for (int i = 0; i < this._conditions.Count; i++)
			{
				if (this._conditions[i].IsCompleted)
				{
					this._value++;
				}
			}
		}

		// Token: 0x04005E3E RID: 24126
		private List<AchievementCondition> _conditions = new List<AchievementCondition>();
	}
}
