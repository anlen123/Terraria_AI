using System;
using Terraria.Social;

namespace Terraria.Achievements
{
	// Token: 0x020005E0 RID: 1504
	public class ConditionIntTracker : AchievementTracker<int>
	{
		// Token: 0x06003B08 RID: 15112 RVA: 0x00658D73 File Offset: 0x00656F73
		public ConditionIntTracker() : base(TrackerType.Int)
		{
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x00658D7C File Offset: 0x00656F7C
		public ConditionIntTracker(int maxValue) : base(TrackerType.Int)
		{
			this._maxValue = maxValue;
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x00658D8C File Offset: 0x00656F8C
		public override void ReportUpdate()
		{
			if (SocialAPI.Achievements != null && this._name != null)
			{
				SocialAPI.Achievements.UpdateIntStat(this._name, this._value);
			}
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x00009E06 File Offset: 0x00008006
		protected override void Load()
		{
		}
	}
}
