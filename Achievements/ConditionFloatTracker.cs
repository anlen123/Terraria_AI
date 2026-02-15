using System;
using Terraria.Social;

namespace Terraria.Achievements
{
	// Token: 0x020005DF RID: 1503
	public class ConditionFloatTracker : AchievementTracker<float>
	{
		// Token: 0x06003B04 RID: 15108 RVA: 0x00658D33 File Offset: 0x00656F33
		public ConditionFloatTracker(float maxValue) : base(TrackerType.Float)
		{
			this._maxValue = maxValue;
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x00658D43 File Offset: 0x00656F43
		public ConditionFloatTracker() : base(TrackerType.Float)
		{
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x00658D4C File Offset: 0x00656F4C
		public override void ReportUpdate()
		{
			if (SocialAPI.Achievements != null && this._name != null)
			{
				SocialAPI.Achievements.UpdateFloatStat(this._name, this._value);
			}
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x00009E06 File Offset: 0x00008006
		protected override void Load()
		{
		}
	}
}
