using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures
{
	// Token: 0x02000591 RID: 1425
	public class DrawAnimationVertical : DrawAnimation
	{
		// Token: 0x0600383C RID: 14396 RVA: 0x00630BE8 File Offset: 0x0062EDE8
		public DrawAnimationVertical(int ticksperframe, int frameCount, bool pingPong = false)
		{
			this.Frame = 0;
			this.FrameCounter = 0;
			this.FrameCount = frameCount;
			this.TicksPerFrame = ticksperframe;
			this.PingPong = pingPong;
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x00630C14 File Offset: 0x0062EE14
		public override void Update()
		{
			if (this.NotActuallyAnimating)
			{
				return;
			}
			int num = this.FrameCounter + 1;
			this.FrameCounter = num;
			if (num >= this.TicksPerFrame)
			{
				this.FrameCounter = 0;
				if (this.PingPong)
				{
					num = this.Frame + 1;
					this.Frame = num;
					if (num >= this.FrameCount * 2 - 2)
					{
						this.Frame = 0;
						return;
					}
				}
				else
				{
					num = this.Frame + 1;
					this.Frame = num;
					if (num >= this.FrameCount)
					{
						this.Frame = 0;
					}
				}
			}
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x00630C98 File Offset: 0x0062EE98
		public override Rectangle GetFrame(Texture2D texture, int frameCounterOverride = -1)
		{
			if (frameCounterOverride != -1)
			{
				int num = frameCounterOverride / this.TicksPerFrame;
				int num2 = this.FrameCount;
				if (this.PingPong)
				{
					num2 = num2 * 2 - 1;
				}
				int num3 = num % num2;
				if (this.PingPong && num3 >= this.FrameCount)
				{
					num3 = this.FrameCount * 2 - 2 - num3;
				}
				Rectangle result = texture.Frame(1, this.FrameCount, 0, num3, 0, 0);
				result.Height -= 2;
				return result;
			}
			int frameY = this.Frame;
			if (this.PingPong && this.Frame >= this.FrameCount)
			{
				frameY = this.FrameCount * 2 - 2 - this.Frame;
			}
			Rectangle result2 = texture.Frame(1, this.FrameCount, 0, frameY, 0, 0);
			result2.Height -= 2;
			return result2;
		}

		// Token: 0x04005C49 RID: 23625
		public bool PingPong;

		// Token: 0x04005C4A RID: 23626
		public bool NotActuallyAnimating;
	}
}
