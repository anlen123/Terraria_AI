using System;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x0200055B RID: 1371
	public class SettingsForCharacterPreview
	{
		// Token: 0x060037A4 RID: 14244 RVA: 0x0062EC2C File Offset: 0x0062CE2C
		public void ApplyTo(Projectile proj, bool walking)
		{
			proj.position += this.Offset;
			proj.spriteDirection = this.SpriteDirection;
			proj.direction = this.SpriteDirection;
			if (walking)
			{
				this.Selected.ApplyTo(proj);
			}
			else
			{
				this.NotSelected.ApplyTo(proj);
			}
			if (this.CustomAnimation != null)
			{
				this.CustomAnimation(proj, walking);
			}
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x0062EC9A File Offset: 0x0062CE9A
		public SettingsForCharacterPreview WhenSelected(int? startFrame = null, int? frameCount = null, int? delayPerFrame = null, bool? bounceLoop = null)
		{
			SettingsForCharacterPreview.Modify(ref this.Selected, startFrame, frameCount, delayPerFrame, bounceLoop);
			return this;
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x0062ECAD File Offset: 0x0062CEAD
		public SettingsForCharacterPreview WhenNotSelected(int? startFrame = null, int? frameCount = null, int? delayPerFrame = null, bool? bounceLoop = null)
		{
			SettingsForCharacterPreview.Modify(ref this.NotSelected, startFrame, frameCount, delayPerFrame, bounceLoop);
			return this;
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x0062ECC0 File Offset: 0x0062CEC0
		private static void Modify(ref SettingsForCharacterPreview.SelectionBasedSettings target, int? startFrame, int? frameCount, int? delayPerFrame, bool? bounceLoop)
		{
			if (frameCount != null && frameCount.Value < 1)
			{
				frameCount = new int?(1);
			}
			target.StartFrame = ((startFrame != null) ? startFrame.Value : target.StartFrame);
			target.FrameCount = ((frameCount != null) ? frameCount.Value : target.FrameCount);
			target.DelayPerFrame = ((delayPerFrame != null) ? delayPerFrame.Value : target.DelayPerFrame);
			target.BounceLoop = ((bounceLoop != null) ? bounceLoop.Value : target.BounceLoop);
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x0062ED60 File Offset: 0x0062CF60
		public SettingsForCharacterPreview WithOffset(Vector2 offset)
		{
			this.Offset = offset;
			return this;
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x0062ED6A File Offset: 0x0062CF6A
		public SettingsForCharacterPreview WithOffset(float x, float y)
		{
			this.Offset = new Vector2(x, y);
			return this;
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x0062ED7A File Offset: 0x0062CF7A
		public SettingsForCharacterPreview WithSpriteDirection(int spriteDirection)
		{
			this.SpriteDirection = spriteDirection;
			return this;
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x0062ED84 File Offset: 0x0062CF84
		public SettingsForCharacterPreview WithCode(SettingsForCharacterPreview.CustomAnimationCode customAnimation)
		{
			this.CustomAnimation = customAnimation;
			return this;
		}

		// Token: 0x04005BA4 RID: 23460
		public Vector2 Offset;

		// Token: 0x04005BA5 RID: 23461
		public SettingsForCharacterPreview.SelectionBasedSettings Selected;

		// Token: 0x04005BA6 RID: 23462
		public SettingsForCharacterPreview.SelectionBasedSettings NotSelected;

		// Token: 0x04005BA7 RID: 23463
		public int SpriteDirection = 1;

		// Token: 0x04005BA8 RID: 23464
		public SettingsForCharacterPreview.CustomAnimationCode CustomAnimation;

		// Token: 0x020009BA RID: 2490
		// (Invoke) Token: 0x06004A2A RID: 18986
		public delegate void CustomAnimationCode(Projectile proj, bool walking);

		// Token: 0x020009BB RID: 2491
		public struct SelectionBasedSettings
		{
			// Token: 0x06004A2D RID: 18989 RVA: 0x006D2818 File Offset: 0x006D0A18
			public void ApplyTo(Projectile proj)
			{
				if (this.FrameCount == 0)
				{
					return;
				}
				if (proj.frame < this.StartFrame)
				{
					proj.frame = this.StartFrame;
				}
				int num = proj.frame - this.StartFrame;
				int num2 = this.FrameCount * this.DelayPerFrame;
				int num3 = num2;
				if (this.BounceLoop)
				{
					num3 = num2 * 2 - this.DelayPerFrame * 2;
				}
				int num4 = proj.frameCounter + 1;
				proj.frameCounter = num4;
				if (num4 >= num3)
				{
					proj.frameCounter = 0;
				}
				num = proj.frameCounter / this.DelayPerFrame;
				if (this.BounceLoop && num >= this.FrameCount)
				{
					num = this.FrameCount * 2 - num - 2;
				}
				proj.frame = this.StartFrame + num;
			}

			// Token: 0x04007694 RID: 30356
			public int StartFrame;

			// Token: 0x04007695 RID: 30357
			public int FrameCount;

			// Token: 0x04007696 RID: 30358
			public int DelayPerFrame;

			// Token: 0x04007697 RID: 30359
			public bool BounceLoop;
		}
	}
}
