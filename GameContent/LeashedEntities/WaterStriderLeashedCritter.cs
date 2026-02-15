using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x02000464 RID: 1124
	internal class WaterStriderLeashedCritter : JumperLeashedCritter
	{
		// Token: 0x0600329C RID: 12956 RVA: 0x005F0490 File Offset: 0x005EE690
		public WaterStriderLeashedCritter()
		{
			this.minWaitTime = 60;
			this.maxWaitTime = 120;
			this.strayingRangeInBlocks = 5;
			this.maxJumpWidth = 32f;
			this.minJumpWidth = 8f;
			this.maxJumpHeight = 0f;
			this.maxJumpDuration = 14f;
			this.jumpCooldown = 15;
			this.canStandOnWater = true;
		}

		// Token: 0x0600329D RID: 12957 RVA: 0x005F04F8 File Offset: 0x005EE6F8
		public override Vector2 GetDrawOffset()
		{
			Vector2 drawOffset = base.GetDrawOffset();
			Point pt = base.Center.ToTileCoordinates();
			for (int i = 0; i < 2; i++)
			{
				pt.Y++;
				byte liquid = Framing.GetTileSafely(pt).liquid;
				if (liquid != 0)
				{
					drawOffset.Y = (float)((byte.MaxValue - liquid) / 16);
					break;
				}
			}
			return drawOffset;
		}

		// Token: 0x04005814 RID: 22548
		public new static WaterStriderLeashedCritter Prototype = new WaterStriderLeashedCritter();
	}
}
