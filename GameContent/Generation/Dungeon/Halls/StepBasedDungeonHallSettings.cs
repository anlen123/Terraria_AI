using System;

namespace Terraria.GameContent.Generation.Dungeon.Halls
{
	// Token: 0x020004C3 RID: 1219
	public abstract class StepBasedDungeonHallSettings : DungeonHallSettings
	{
		// Token: 0x04005A29 RID: 23081
		public int OverrideStrength;

		// Token: 0x04005A2A RID: 23082
		public int OverrideSteps;

		// Token: 0x04005A2B RID: 23083
		public bool ForceHorizontal;

		// Token: 0x04005A2C RID: 23084
		public double OverrideInteriorToExteriorRatio;
	}
}
