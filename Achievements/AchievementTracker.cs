using System;
using Terraria.Social;

namespace Terraria.Achievements
{
	// Token: 0x020005E2 RID: 1506
	public abstract class AchievementTracker<T> : IAchievementTracker
	{
		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06003B20 RID: 15136 RVA: 0x00659400 File Offset: 0x00657600
		public T Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06003B21 RID: 15137 RVA: 0x00659408 File Offset: 0x00657608
		public T MaxValue
		{
			get
			{
				return this._maxValue;
			}
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x00659410 File Offset: 0x00657610
		protected AchievementTracker(TrackerType type)
		{
			this._type = type;
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x0065941F File Offset: 0x0065761F
		void IAchievementTracker.ReportAs(string name)
		{
			this._name = name;
		}

		// Token: 0x06003B24 RID: 15140 RVA: 0x00659428 File Offset: 0x00657628
		TrackerType IAchievementTracker.GetTrackerType()
		{
			return this._type;
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x00659430 File Offset: 0x00657630
		void IAchievementTracker.Clear()
		{
			this.SetValue(default(T), true);
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x00659450 File Offset: 0x00657650
		public void SetValue(T newValue, bool reportUpdate = true)
		{
			if (!newValue.Equals(this._value))
			{
				this._value = newValue;
				if (reportUpdate)
				{
					this.ReportUpdate();
					if (this._value.Equals(this._maxValue))
					{
						this.OnComplete();
					}
				}
			}
		}

		// Token: 0x06003B27 RID: 15143
		public abstract void ReportUpdate();

		// Token: 0x06003B28 RID: 15144
		protected abstract void Load();

		// Token: 0x06003B29 RID: 15145 RVA: 0x006594AB File Offset: 0x006576AB
		void IAchievementTracker.Load()
		{
			this.Load();
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x006594B3 File Offset: 0x006576B3
		protected void OnComplete()
		{
			if (SocialAPI.Achievements != null)
			{
				SocialAPI.Achievements.StoreStats();
			}
		}

		// Token: 0x04005E3A RID: 24122
		protected T _value;

		// Token: 0x04005E3B RID: 24123
		protected T _maxValue;

		// Token: 0x04005E3C RID: 24124
		protected string _name;

		// Token: 0x04005E3D RID: 24125
		private TrackerType _type;
	}
}
