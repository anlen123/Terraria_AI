using System;

namespace Terraria.GameContent
{
	// Token: 0x02000274 RID: 628
	public struct WellFedHelper
	{
		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x0054A904 File Offset: 0x00548B04
		public int TimeLeft
		{
			get
			{
				return this._timeLeftRank1 + this._timeLeftRank2 + this._timeLeftRank3;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x0054A91A File Offset: 0x00548B1A
		public int Rank
		{
			get
			{
				if (this._timeLeftRank3 > 0)
				{
					return 3;
				}
				if (this._timeLeftRank2 > 0)
				{
					return 2;
				}
				if (this._timeLeftRank1 > 0)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x0054A940 File Offset: 0x00548B40
		public void Eat(int foodRank, int foodBuffTime)
		{
			int num = foodBuffTime;
			if (foodRank >= 3)
			{
				this.AddTimeTo(ref this._timeLeftRank3, ref num, 72000);
			}
			if (foodRank >= 2)
			{
				this.AddTimeTo(ref this._timeLeftRank2, ref num, 72000);
			}
			if (foodRank >= 1)
			{
				this.AddTimeTo(ref this._timeLeftRank1, ref num, 72000);
			}
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x0054A994 File Offset: 0x00548B94
		public void Update()
		{
			this.ReduceTimeLeft();
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x0054A99C File Offset: 0x00548B9C
		public void Clear()
		{
			this._timeLeftRank1 = 0;
			this._timeLeftRank2 = 0;
			this._timeLeftRank3 = 0;
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x0054A9B4 File Offset: 0x00548BB4
		private void AddTimeTo(ref int foodTimeCounter, ref int timeLeftToAdd, int counterMaximumTime)
		{
			if (timeLeftToAdd == 0)
			{
				return;
			}
			int num = timeLeftToAdd;
			if (foodTimeCounter + num > counterMaximumTime)
			{
				num = counterMaximumTime - foodTimeCounter;
			}
			foodTimeCounter += num;
			timeLeftToAdd -= num;
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x0054A9E4 File Offset: 0x00548BE4
		private void ReduceTimeLeft()
		{
			if (this._timeLeftRank3 > 0)
			{
				this._timeLeftRank3--;
				return;
			}
			if (this._timeLeftRank2 > 0)
			{
				this._timeLeftRank2--;
				return;
			}
			if (this._timeLeftRank1 > 0)
			{
				this._timeLeftRank1--;
			}
		}

		// Token: 0x04004DBA RID: 19898
		private const int MAXIMUM_TIME_LEFT_PER_COUNTER = 72000;

		// Token: 0x04004DBB RID: 19899
		private int _timeLeftRank1;

		// Token: 0x04004DBC RID: 19900
		private int _timeLeftRank2;

		// Token: 0x04004DBD RID: 19901
		private int _timeLeftRank3;
	}
}
