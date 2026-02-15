using System;

namespace Terraria.Physics
{
	// Token: 0x0200007C RID: 124
	public struct BallStepResult
	{
		// Token: 0x06001558 RID: 5464 RVA: 0x004C3CF6 File Offset: 0x004C1EF6
		private BallStepResult(BallState state)
		{
			this.State = state;
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x004C3CFF File Offset: 0x004C1EFF
		public static BallStepResult OutOfBounds()
		{
			return new BallStepResult(BallState.OutOfBounds);
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x004C3D07 File Offset: 0x004C1F07
		public static BallStepResult Moving()
		{
			return new BallStepResult(BallState.Moving);
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x004C3D0F File Offset: 0x004C1F0F
		public static BallStepResult Resting()
		{
			return new BallStepResult(BallState.Resting);
		}

		// Token: 0x040010E6 RID: 4326
		public readonly BallState State;
	}
}
