using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	// Token: 0x02000285 RID: 645
	public class CustomFloatCondition : AchievementCondition
	{
		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060024D3 RID: 9427 RVA: 0x00551F80 File Offset: 0x00550180
		// (set) Token: 0x060024D4 RID: 9428 RVA: 0x00551F88 File Offset: 0x00550188
		public float Value
		{
			get
			{
				return this._value;
			}
			set
			{
				float num = Utils.Clamp<float>(value, 0f, this._maxValue);
				if (this._tracker != null)
				{
					((ConditionFloatTracker)this._tracker).SetValue(num, true);
				}
				this._value = num;
				if (this._value == this._maxValue)
				{
					this.Complete();
				}
			}
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x00551FDC File Offset: 0x005501DC
		private CustomFloatCondition(string name, float maxValue) : base(name)
		{
			this._maxValue = maxValue;
			this._value = 0f;
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x00551FF7 File Offset: 0x005501F7
		public override void Clear()
		{
			this._value = 0f;
			base.Clear();
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x0055200A File Offset: 0x0055020A
		public override void Load(JObject state)
		{
			base.Load(state);
			this._value = (float)state["Value"];
			if (this._tracker != null)
			{
				((ConditionFloatTracker)this._tracker).SetValue(this._value, false);
			}
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x00552049 File Offset: 0x00550249
		protected override IAchievementTracker CreateAchievementTracker()
		{
			return new ConditionFloatTracker(this._maxValue);
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x00552056 File Offset: 0x00550256
		public static AchievementCondition Create(string name, float maxValue)
		{
			return new CustomFloatCondition(name, maxValue);
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x0055205F File Offset: 0x0055025F
		public override void Complete()
		{
			if (this._tracker != null)
			{
				((ConditionFloatTracker)this._tracker).SetValue(this._maxValue, true);
			}
			this._value = this._maxValue;
			base.Complete();
		}

		// Token: 0x04004F47 RID: 20295
		[JsonProperty("Value")]
		private float _value;

		// Token: 0x04004F48 RID: 20296
		private float _maxValue;
	}
}
