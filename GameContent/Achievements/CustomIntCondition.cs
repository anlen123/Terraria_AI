using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	// Token: 0x02000286 RID: 646
	public class CustomIntCondition : AchievementCondition
	{
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060024DB RID: 9435 RVA: 0x00552092 File Offset: 0x00550292
		// (set) Token: 0x060024DC RID: 9436 RVA: 0x0055209C File Offset: 0x0055029C
		public int Value
		{
			get
			{
				return this._value;
			}
			set
			{
				int num = Utils.Clamp<int>(value, 0, this._maxValue);
				if (this._tracker != null)
				{
					((ConditionIntTracker)this._tracker).SetValue(num, true);
				}
				this._value = num;
				if (this._value == this._maxValue)
				{
					this.Complete();
				}
			}
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x005520EC File Offset: 0x005502EC
		private CustomIntCondition(string name, int maxValue) : base(name)
		{
			this._maxValue = maxValue;
			this._value = 0;
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x00552103 File Offset: 0x00550303
		public override void Clear()
		{
			this._value = 0;
			base.Clear();
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x00552112 File Offset: 0x00550312
		public override void Load(JObject state)
		{
			base.Load(state);
			this._value = (int)state["Value"];
			if (this._tracker != null)
			{
				((ConditionIntTracker)this._tracker).SetValue(this._value, false);
			}
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x00552150 File Offset: 0x00550350
		protected override IAchievementTracker CreateAchievementTracker()
		{
			return new ConditionIntTracker(this._maxValue);
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x0055215D File Offset: 0x0055035D
		public static AchievementCondition Create(string name, int maxValue)
		{
			return new CustomIntCondition(name, maxValue);
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x00552166 File Offset: 0x00550366
		public override void Complete()
		{
			if (this._tracker != null)
			{
				((ConditionIntTracker)this._tracker).SetValue(this._maxValue, true);
			}
			this._value = this._maxValue;
			base.Complete();
		}

		// Token: 0x04004F49 RID: 20297
		[JsonProperty("Value")]
		private int _value;

		// Token: 0x04004F4A RID: 20298
		private int _maxValue;
	}
}
