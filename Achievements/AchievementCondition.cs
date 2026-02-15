using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Terraria.Achievements
{
	// Token: 0x020005DE RID: 1502
	[JsonObject(1)]
	public abstract class AchievementCondition
	{
		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06003AFB RID: 15099 RVA: 0x00658C4C File Offset: 0x00656E4C
		// (remove) Token: 0x06003AFC RID: 15100 RVA: 0x00658C84 File Offset: 0x00656E84
		public event AchievementCondition.AchievementUpdate OnComplete;

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06003AFD RID: 15101 RVA: 0x00658CB9 File Offset: 0x00656EB9
		public bool IsCompleted
		{
			get
			{
				return this._isCompleted;
			}
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x00658CC1 File Offset: 0x00656EC1
		protected AchievementCondition(string name)
		{
			this.Name = name;
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x00658CD0 File Offset: 0x00656ED0
		public virtual void Load(JObject state)
		{
			this._isCompleted = (bool)state["Completed"];
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x00658CE8 File Offset: 0x00656EE8
		public virtual void Clear()
		{
			this._isCompleted = false;
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x00658CF1 File Offset: 0x00656EF1
		public virtual void Complete()
		{
			if (this._isCompleted)
			{
				return;
			}
			this._isCompleted = true;
			if (this.OnComplete != null)
			{
				this.OnComplete(this);
			}
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x000762F3 File Offset: 0x000744F3
		protected virtual IAchievementTracker CreateAchievementTracker()
		{
			return null;
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x00658D17 File Offset: 0x00656F17
		public IAchievementTracker GetAchievementTracker()
		{
			if (this._tracker == null)
			{
				this._tracker = this.CreateAchievementTracker();
			}
			return this._tracker;
		}

		// Token: 0x04005E2E RID: 24110
		public readonly string Name;

		// Token: 0x04005E30 RID: 24112
		protected IAchievementTracker _tracker;

		// Token: 0x04005E31 RID: 24113
		[JsonProperty("Completed")]
		private bool _isCompleted;

		// Token: 0x020009CB RID: 2507
		// (Invoke) Token: 0x06004A5A RID: 19034
		public delegate void AchievementUpdate(AchievementCondition condition);
	}
}
