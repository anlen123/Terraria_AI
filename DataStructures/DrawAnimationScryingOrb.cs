using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures
{
	// Token: 0x02000592 RID: 1426
	public class DrawAnimationScryingOrb : DrawAnimation
	{
		// Token: 0x0600383F RID: 14399 RVA: 0x00630D58 File Offset: 0x0062EF58
		public override void Update()
		{
			int num = this.FrameCounter + 1;
			this.FrameCounter = num;
			if (num < this.TicksPerFrame)
			{
				return;
			}
			this.FrameCounter = 0;
			num = this._nextStateCounter - 1;
			this._nextStateCounter = num;
			if (num <= 0)
			{
				if (this._state == DrawAnimationScryingOrb.State.Moving)
				{
					this._state = ((Main.rand.Next(4) == 0) ? DrawAnimationScryingOrb.State.FrozenCenter : DrawAnimationScryingOrb.State.Frozen);
					this._nextStateCounter = Main.rand.Next(7, 10);
					return;
				}
				this._state = DrawAnimationScryingOrb.State.Moving;
				this._nextStateCounter = Main.rand.Next(3, 9);
				if (Main.rand.Next(4) == 0)
				{
					this._dir *= -1;
					return;
				}
			}
			else if (this._state == DrawAnimationScryingOrb.State.Moving)
			{
				this.Frame += this._dir;
				if (this.Frame >= this.FrameCount)
				{
					this.Frame = 1;
					return;
				}
				if (this.Frame <= 0)
				{
					this.Frame = this.FrameCount - 1;
				}
			}
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x00630E50 File Offset: 0x0062F050
		public override Rectangle GetFrame(Texture2D texture, int frameCounterOverride = -1)
		{
			int frameY = (this._state == DrawAnimationScryingOrb.State.FrozenCenter) ? 0 : this.Frame;
			if (frameCounterOverride >= 0)
			{
				frameY = frameCounterOverride;
			}
			return texture.Frame(1, this.FrameCount, 0, frameY, 0, -2);
		}

		// Token: 0x04005C4B RID: 23627
		private DrawAnimationScryingOrb.State _state;

		// Token: 0x04005C4C RID: 23628
		private int _nextStateCounter;

		// Token: 0x04005C4D RID: 23629
		private int _dir = 1;

		// Token: 0x020009BF RID: 2495
		private enum State
		{
			// Token: 0x0400769E RID: 30366
			Moving,
			// Token: 0x0400769F RID: 30367
			Frozen,
			// Token: 0x040076A0 RID: 30368
			FrozenCenter
		}
	}
}
