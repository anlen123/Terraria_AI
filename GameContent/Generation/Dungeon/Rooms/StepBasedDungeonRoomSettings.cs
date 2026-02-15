using System;
using ReLogic.Utilities;

namespace Terraria.GameContent.Generation.Dungeon.Rooms
{
	// Token: 0x020004B3 RID: 1203
	public abstract class StepBasedDungeonRoomSettings : DungeonRoomSettings
	{
		// Token: 0x0600344D RID: 13389 RVA: 0x00602C73 File Offset: 0x00600E73
		public override int GetBoundingRadius()
		{
			return (int)((double)this.OverrideStrength * 0.8 + 5.0 + (double)this.OverrideSteps * 0.5 * 1.4);
		}

		// Token: 0x040059F9 RID: 23033
		public int OverrideStrength;

		// Token: 0x040059FA RID: 23034
		public int OverrideSteps;

		// Token: 0x040059FB RID: 23035
		public Vector2D OverrideStartPosition;

		// Token: 0x040059FC RID: 23036
		public Vector2D OverrideEndPosition;

		// Token: 0x040059FD RID: 23037
		public Vector2D OverrideVelocity;

		// Token: 0x040059FE RID: 23038
		public double OverrideInteriorToExteriorRatio;
	}
}
