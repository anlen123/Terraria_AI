using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.Localization;
using Terraria.Social;

namespace Terraria.Achievements
{
	// Token: 0x020005DD RID: 1501
	[JsonObject(1)]
	public class Achievement
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06003AE7 RID: 15079 RVA: 0x00658868 File Offset: 0x00656A68
		public AchievementCategory Category
		{
			get
			{
				return this._category;
			}
		}

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x06003AE8 RID: 15080 RVA: 0x00658870 File Offset: 0x00656A70
		// (remove) Token: 0x06003AE9 RID: 15081 RVA: 0x006588A8 File Offset: 0x00656AA8
		public event Achievement.AchievementCompleted OnCompleted;

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06003AEA RID: 15082 RVA: 0x006588DD File Offset: 0x00656ADD
		public bool HasTracker
		{
			get
			{
				return this._tracker != null;
			}
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x006588E8 File Offset: 0x00656AE8
		public IAchievementTracker GetTracker()
		{
			return this._tracker;
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06003AEC RID: 15084 RVA: 0x006588F0 File Offset: 0x00656AF0
		public bool IsCompleted
		{
			get
			{
				return this._completedCount == this._conditions.Count;
			}
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x00658908 File Offset: 0x00656B08
		public Achievement(string name)
		{
			this.Name = name;
			this.FriendlyName = Language.GetText("Achievements." + name + "_Name");
			this.Description = Language.GetText("Achievements." + name + "_Description");
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x00658978 File Offset: 0x00656B78
		public void ClearProgress()
		{
			this._completedCount = 0;
			foreach (KeyValuePair<string, AchievementCondition> keyValuePair in this._conditions)
			{
				keyValuePair.Value.Clear();
			}
			if (this._tracker != null)
			{
				this._tracker.Clear();
			}
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x006589EC File Offset: 0x00656BEC
		public void Load(Dictionary<string, JObject> conditions)
		{
			foreach (KeyValuePair<string, JObject> keyValuePair in conditions)
			{
				AchievementCondition achievementCondition;
				if (this._conditions.TryGetValue(keyValuePair.Key, out achievementCondition))
				{
					achievementCondition.Load(keyValuePair.Value);
					if (achievementCondition.IsCompleted)
					{
						this._completedCount++;
					}
				}
			}
			if (this._tracker != null)
			{
				this._tracker.Load();
			}
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x00658A80 File Offset: 0x00656C80
		public void AddCondition(AchievementCondition condition)
		{
			this._conditions[condition.Name] = condition;
			condition.OnComplete += this.OnConditionComplete;
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x00658AA8 File Offset: 0x00656CA8
		private void OnConditionComplete(AchievementCondition condition)
		{
			this._completedCount++;
			if (this._completedCount == this._conditions.Count)
			{
				if (this._tracker == null && SocialAPI.Achievements != null)
				{
					SocialAPI.Achievements.CompleteAchievement(this.Name);
				}
				if (this.OnCompleted != null)
				{
					this.OnCompleted(this);
				}
			}
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x00658B09 File Offset: 0x00656D09
		private void UseTracker(IAchievementTracker tracker)
		{
			tracker.ReportAs("STAT_" + this.Name);
			this._tracker = tracker;
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x00658B28 File Offset: 0x00656D28
		public void UseTrackerFromCondition(string conditionName)
		{
			this.UseTracker(this.GetConditionTracker(conditionName));
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x00658B38 File Offset: 0x00656D38
		public void UseConditionsCompletedTracker()
		{
			ConditionsCompletedTracker conditionsCompletedTracker = new ConditionsCompletedTracker();
			foreach (KeyValuePair<string, AchievementCondition> keyValuePair in this._conditions)
			{
				conditionsCompletedTracker.AddCondition(keyValuePair.Value);
			}
			this.UseTracker(conditionsCompletedTracker);
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x00658BA0 File Offset: 0x00656DA0
		public void UseConditionsCompletedTracker(params string[] conditions)
		{
			ConditionsCompletedTracker conditionsCompletedTracker = new ConditionsCompletedTracker();
			foreach (string key in conditions)
			{
				conditionsCompletedTracker.AddCondition(this._conditions[key]);
			}
			this.UseTracker(conditionsCompletedTracker);
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x00658BDE File Offset: 0x00656DDE
		public void ClearTracker()
		{
			this._tracker = null;
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x00658BE7 File Offset: 0x00656DE7
		private IAchievementTracker GetConditionTracker(string name)
		{
			return this._conditions[name].GetAchievementTracker();
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x00658BFC File Offset: 0x00656DFC
		public void AddConditions(params AchievementCondition[] conditions)
		{
			for (int i = 0; i < conditions.Length; i++)
			{
				this.AddCondition(conditions[i]);
			}
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x00658C20 File Offset: 0x00656E20
		public AchievementCondition GetCondition(string conditionName)
		{
			AchievementCondition result;
			if (this._conditions.TryGetValue(conditionName, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x00658C40 File Offset: 0x00656E40
		public void SetCategory(AchievementCategory category)
		{
			this._category = category;
		}

		// Token: 0x04005E24 RID: 24100
		private static int _totalAchievements;

		// Token: 0x04005E25 RID: 24101
		public readonly string Name;

		// Token: 0x04005E26 RID: 24102
		public readonly LocalizedText FriendlyName;

		// Token: 0x04005E27 RID: 24103
		public readonly LocalizedText Description;

		// Token: 0x04005E28 RID: 24104
		public readonly int Id = Achievement._totalAchievements++;

		// Token: 0x04005E29 RID: 24105
		private AchievementCategory _category;

		// Token: 0x04005E2A RID: 24106
		private IAchievementTracker _tracker;

		// Token: 0x04005E2C RID: 24108
		[JsonProperty("Conditions")]
		private Dictionary<string, AchievementCondition> _conditions = new Dictionary<string, AchievementCondition>();

		// Token: 0x04005E2D RID: 24109
		private int _completedCount;

		// Token: 0x020009CA RID: 2506
		// (Invoke) Token: 0x06004A56 RID: 19030
		public delegate void AchievementCompleted(Achievement achievement);
	}
}
