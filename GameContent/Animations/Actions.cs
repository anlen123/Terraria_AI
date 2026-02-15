using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Animations
{
	// Token: 0x0200052D RID: 1325
	public class Actions
	{
		// Token: 0x020009A8 RID: 2472
		public class Players
		{
			// Token: 0x02000AF2 RID: 2802
			public interface IPlayerAction : IAnimationSegmentAction<Player>
			{
			}

			// Token: 0x02000AF3 RID: 2803
			public class Fade : Actions.Players.IPlayerAction, IAnimationSegmentAction<Player>
			{
				// Token: 0x06004D21 RID: 19745 RVA: 0x006DA442 File Offset: 0x006D8642
				public Fade(float opacityTarget)
				{
					this._duration = 0;
					this._opacityTarget = opacityTarget;
				}

				// Token: 0x06004D22 RID: 19746 RVA: 0x006DA458 File Offset: 0x006D8658
				public Fade(float opacityTarget, int duration)
				{
					this._duration = duration;
					this._opacityTarget = opacityTarget;
				}

				// Token: 0x06004D23 RID: 19747 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(Player obj)
				{
				}

				// Token: 0x170005C5 RID: 1477
				// (get) Token: 0x06004D24 RID: 19748 RVA: 0x006DA46E File Offset: 0x006D866E
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D25 RID: 19749 RVA: 0x006DA476 File Offset: 0x006D8676
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004D26 RID: 19750 RVA: 0x006DA480 File Offset: 0x006D8680
				public void ApplyTo(Player obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					if (this._duration == 0)
					{
						obj.opacityForAnimation = this._opacityTarget;
						return;
					}
					float num = localTimeForObj - this._delay;
					if (num > (float)this._duration)
					{
						num = (float)this._duration;
					}
					obj.opacityForAnimation = MathHelper.Lerp(obj.opacityForAnimation, this._opacityTarget, Utils.GetLerpValue(0f, (float)this._duration, num, true));
				}

				// Token: 0x04007893 RID: 30867
				private int _duration;

				// Token: 0x04007894 RID: 30868
				private float _opacityTarget;

				// Token: 0x04007895 RID: 30869
				private float _delay;
			}

			// Token: 0x02000AF4 RID: 2804
			public class Wait : Actions.Players.IPlayerAction, IAnimationSegmentAction<Player>
			{
				// Token: 0x06004D27 RID: 19751 RVA: 0x006DA4F1 File Offset: 0x006D86F1
				public Wait(int durationInFrames)
				{
					this._duration = durationInFrames;
				}

				// Token: 0x06004D28 RID: 19752 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(Player obj)
				{
				}

				// Token: 0x170005C6 RID: 1478
				// (get) Token: 0x06004D29 RID: 19753 RVA: 0x006DA500 File Offset: 0x006D8700
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D2A RID: 19754 RVA: 0x006DA508 File Offset: 0x006D8708
				public void ApplyTo(Player obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					obj.velocity = Vector2.Zero;
				}

				// Token: 0x06004D2B RID: 19755 RVA: 0x006DA51F File Offset: 0x006D871F
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x04007896 RID: 30870
				private int _duration;

				// Token: 0x04007897 RID: 30871
				private float _delay;
			}

			// Token: 0x02000AF5 RID: 2805
			public class LookAt : Actions.Players.IPlayerAction, IAnimationSegmentAction<Player>
			{
				// Token: 0x06004D2C RID: 19756 RVA: 0x006DA528 File Offset: 0x006D8728
				public LookAt(int direction)
				{
					this._direction = direction;
				}

				// Token: 0x06004D2D RID: 19757 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(Player obj)
				{
				}

				// Token: 0x170005C7 RID: 1479
				// (get) Token: 0x06004D2E RID: 19758 RVA: 0x001DA9FB File Offset: 0x001D8BFB
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return 0;
					}
				}

				// Token: 0x06004D2F RID: 19759 RVA: 0x006DA537 File Offset: 0x006D8737
				public void ApplyTo(Player obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					obj.direction = this._direction;
				}

				// Token: 0x06004D30 RID: 19760 RVA: 0x006DA54F File Offset: 0x006D874F
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x04007898 RID: 30872
				private int _direction;

				// Token: 0x04007899 RID: 30873
				private float _delay;
			}

			// Token: 0x02000AF6 RID: 2806
			public class MoveWithAcceleration : Actions.Players.IPlayerAction, IAnimationSegmentAction<Player>
			{
				// Token: 0x06004D31 RID: 19761 RVA: 0x006DA558 File Offset: 0x006D8758
				public MoveWithAcceleration(Vector2 offsetPerFrame, Vector2 accelerationPerFrame, int durationInFrames)
				{
					this._accelerationPerFrame = accelerationPerFrame;
					this._offsetPerFrame = offsetPerFrame;
					this._duration = durationInFrames;
				}

				// Token: 0x06004D32 RID: 19762 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(Player obj)
				{
				}

				// Token: 0x170005C8 RID: 1480
				// (get) Token: 0x06004D33 RID: 19763 RVA: 0x006DA575 File Offset: 0x006D8775
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D34 RID: 19764 RVA: 0x006DA57D File Offset: 0x006D877D
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004D35 RID: 19765 RVA: 0x006DA588 File Offset: 0x006D8788
				public void ApplyTo(Player obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					float num = localTimeForObj - this._delay;
					if (num > (float)this._duration)
					{
						num = (float)this._duration;
					}
					Vector2 value = this._offsetPerFrame * num + this._accelerationPerFrame * (num * num * 0.5f);
					obj.position += value;
					obj.velocity = this._offsetPerFrame + this._accelerationPerFrame * num;
					if (this._offsetPerFrame.X != 0f)
					{
						obj.direction = ((this._offsetPerFrame.X > 0f) ? 1 : -1);
					}
				}

				// Token: 0x0400789A RID: 30874
				private Vector2 _offsetPerFrame;

				// Token: 0x0400789B RID: 30875
				private Vector2 _accelerationPerFrame;

				// Token: 0x0400789C RID: 30876
				private int _duration;

				// Token: 0x0400789D RID: 30877
				private float _delay;
			}
		}

		// Token: 0x020009A9 RID: 2473
		public class NPCs
		{
			// Token: 0x02000AF7 RID: 2807
			public interface INPCAction : IAnimationSegmentAction<NPC>
			{
			}

			// Token: 0x02000AF8 RID: 2808
			public class Fade : Actions.NPCs.INPCAction, IAnimationSegmentAction<NPC>
			{
				// Token: 0x06004D36 RID: 19766 RVA: 0x006DA63D File Offset: 0x006D883D
				public Fade(int alphaPerFrame)
				{
					this._duration = 0;
					this._alphaPerFrame = alphaPerFrame;
				}

				// Token: 0x06004D37 RID: 19767 RVA: 0x006DA653 File Offset: 0x006D8853
				public Fade(int alphaPerFrame, int duration)
				{
					this._duration = duration;
					this._alphaPerFrame = alphaPerFrame;
				}

				// Token: 0x06004D38 RID: 19768 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(NPC obj)
				{
				}

				// Token: 0x170005C9 RID: 1481
				// (get) Token: 0x06004D39 RID: 19769 RVA: 0x006DA669 File Offset: 0x006D8869
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D3A RID: 19770 RVA: 0x006DA671 File Offset: 0x006D8871
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004D3B RID: 19771 RVA: 0x006DA67C File Offset: 0x006D887C
				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					if (this._duration == 0)
					{
						obj.alpha = Utils.Clamp<int>(obj.alpha + this._alphaPerFrame, 0, 255);
						return;
					}
					float num = localTimeForObj - this._delay;
					if (num > (float)this._duration)
					{
						num = (float)this._duration;
					}
					obj.alpha = Utils.Clamp<int>(obj.alpha + (int)num * this._alphaPerFrame, 0, 255);
				}

				// Token: 0x0400789E RID: 30878
				private int _duration;

				// Token: 0x0400789F RID: 30879
				private int _alphaPerFrame;

				// Token: 0x040078A0 RID: 30880
				private float _delay;
			}

			// Token: 0x02000AF9 RID: 2809
			public class ShowItem : Actions.NPCs.INPCAction, IAnimationSegmentAction<NPC>
			{
				// Token: 0x06004D3C RID: 19772 RVA: 0x006DA6F6 File Offset: 0x006D88F6
				public ShowItem(int durationInFrames, int itemIdToShow)
				{
					this._duration = durationInFrames;
					this._itemIdToShow = itemIdToShow;
				}

				// Token: 0x06004D3D RID: 19773 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(NPC obj)
				{
				}

				// Token: 0x170005CA RID: 1482
				// (get) Token: 0x06004D3E RID: 19774 RVA: 0x006DA70C File Offset: 0x006D890C
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D3F RID: 19775 RVA: 0x006DA714 File Offset: 0x006D8914
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004D40 RID: 19776 RVA: 0x006DA720 File Offset: 0x006D8920
				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					float num = localTimeForObj - this._delay;
					if (num > (float)this._duration)
					{
						this.FixNPCIfWasHoldingItem(obj);
						return;
					}
					obj.velocity = Vector2.Zero;
					obj.frameCounter = (double)num;
					obj.ai[0] = 23f;
					obj.ai[1] = (float)this._duration - num;
					obj.ai[2] = (float)this._itemIdToShow;
				}

				// Token: 0x06004D41 RID: 19777 RVA: 0x006DA794 File Offset: 0x006D8994
				private void FixNPCIfWasHoldingItem(NPC obj)
				{
					if (obj.ai[0] == 23f)
					{
						obj.frameCounter = 0.0;
						obj.ai[0] = 0f;
						obj.ai[1] = 0f;
						obj.ai[2] = 0f;
					}
				}

				// Token: 0x040078A1 RID: 30881
				private int _itemIdToShow;

				// Token: 0x040078A2 RID: 30882
				private int _duration;

				// Token: 0x040078A3 RID: 30883
				private float _delay;
			}

			// Token: 0x02000AFA RID: 2810
			public class Move : Actions.NPCs.INPCAction, IAnimationSegmentAction<NPC>
			{
				// Token: 0x06004D42 RID: 19778 RVA: 0x006DA7E6 File Offset: 0x006D89E6
				public Move(Vector2 offsetPerFrame, int durationInFrames)
				{
					this._offsetPerFrame = offsetPerFrame;
					this._duration = durationInFrames;
				}

				// Token: 0x06004D43 RID: 19779 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(NPC obj)
				{
				}

				// Token: 0x170005CB RID: 1483
				// (get) Token: 0x06004D44 RID: 19780 RVA: 0x006DA7FC File Offset: 0x006D89FC
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D45 RID: 19781 RVA: 0x006DA804 File Offset: 0x006D8A04
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004D46 RID: 19782 RVA: 0x006DA810 File Offset: 0x006D8A10
				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					float num = localTimeForObj - this._delay;
					if (num > (float)this._duration)
					{
						num = (float)this._duration;
					}
					obj.position += this._offsetPerFrame * num;
					obj.velocity = this._offsetPerFrame;
					if (this._offsetPerFrame.X != 0f)
					{
						obj.direction = (obj.spriteDirection = ((this._offsetPerFrame.X > 0f) ? 1 : -1));
					}
				}

				// Token: 0x040078A4 RID: 30884
				private Vector2 _offsetPerFrame;

				// Token: 0x040078A5 RID: 30885
				private int _duration;

				// Token: 0x040078A6 RID: 30886
				private float _delay;
			}

			// Token: 0x02000AFB RID: 2811
			public class MoveWithAcceleration : Actions.NPCs.INPCAction, IAnimationSegmentAction<NPC>
			{
				// Token: 0x06004D47 RID: 19783 RVA: 0x006DA8A2 File Offset: 0x006D8AA2
				public MoveWithAcceleration(Vector2 offsetPerFrame, Vector2 accelerationPerFrame, int durationInFrames)
				{
					this._accelerationPerFrame = accelerationPerFrame;
					this._offsetPerFrame = offsetPerFrame;
					this._duration = durationInFrames;
				}

				// Token: 0x06004D48 RID: 19784 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(NPC obj)
				{
				}

				// Token: 0x170005CC RID: 1484
				// (get) Token: 0x06004D49 RID: 19785 RVA: 0x006DA8BF File Offset: 0x006D8ABF
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D4A RID: 19786 RVA: 0x006DA8C7 File Offset: 0x006D8AC7
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004D4B RID: 19787 RVA: 0x006DA8D0 File Offset: 0x006D8AD0
				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					float num = localTimeForObj - this._delay;
					if (num > (float)this._duration)
					{
						num = (float)this._duration;
					}
					Vector2 value = this._offsetPerFrame * num + this._accelerationPerFrame * (num * num * 0.5f);
					obj.position += value;
					obj.velocity = this._offsetPerFrame + this._accelerationPerFrame * num;
					if (this._offsetPerFrame.X != 0f)
					{
						obj.direction = (obj.spriteDirection = ((this._offsetPerFrame.X > 0f) ? 1 : -1));
					}
				}

				// Token: 0x040078A7 RID: 30887
				private Vector2 _offsetPerFrame;

				// Token: 0x040078A8 RID: 30888
				private Vector2 _accelerationPerFrame;

				// Token: 0x040078A9 RID: 30889
				private int _duration;

				// Token: 0x040078AA RID: 30890
				private float _delay;
			}

			// Token: 0x02000AFC RID: 2812
			public class MoveWithRotor : Actions.NPCs.INPCAction, IAnimationSegmentAction<NPC>
			{
				// Token: 0x06004D4C RID: 19788 RVA: 0x006DA98E File Offset: 0x006D8B8E
				public MoveWithRotor(Vector2 radialOffset, float rotationPerFrame, Vector2 resultMultiplier, int durationInFrames)
				{
					this._radialOffset = rotationPerFrame;
					this._offsetPerFrame = radialOffset;
					this._resultMultiplier = resultMultiplier;
					this._duration = durationInFrames;
				}

				// Token: 0x06004D4D RID: 19789 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(NPC obj)
				{
				}

				// Token: 0x170005CD RID: 1485
				// (get) Token: 0x06004D4E RID: 19790 RVA: 0x006DA9B3 File Offset: 0x006D8BB3
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D4F RID: 19791 RVA: 0x006DA9BB File Offset: 0x006D8BBB
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004D50 RID: 19792 RVA: 0x006DA9C4 File Offset: 0x006D8BC4
				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					float num = localTimeForObj - this._delay;
					if (num > (float)this._duration)
					{
						num = (float)this._duration;
					}
					Vector2 value = this._offsetPerFrame.RotatedBy((double)(this._radialOffset * num), default(Vector2)) * this._resultMultiplier;
					obj.position += value;
				}

				// Token: 0x040078AB RID: 30891
				private Vector2 _offsetPerFrame;

				// Token: 0x040078AC RID: 30892
				private Vector2 _resultMultiplier;

				// Token: 0x040078AD RID: 30893
				private float _radialOffset;

				// Token: 0x040078AE RID: 30894
				private int _duration;

				// Token: 0x040078AF RID: 30895
				private float _delay;
			}

			// Token: 0x02000AFD RID: 2813
			public class DoBunnyRestAnimation : Actions.NPCs.INPCAction, IAnimationSegmentAction<NPC>
			{
				// Token: 0x06004D51 RID: 19793 RVA: 0x006DAA31 File Offset: 0x006D8C31
				public DoBunnyRestAnimation(int durationInFrames)
				{
					this._duration = durationInFrames;
				}

				// Token: 0x06004D52 RID: 19794 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(NPC obj)
				{
				}

				// Token: 0x170005CE RID: 1486
				// (get) Token: 0x06004D53 RID: 19795 RVA: 0x006DAA40 File Offset: 0x006D8C40
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D54 RID: 19796 RVA: 0x006DAA48 File Offset: 0x006D8C48
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004D55 RID: 19797 RVA: 0x006DAA54 File Offset: 0x006D8C54
				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					float num = localTimeForObj - this._delay;
					if (num > (float)this._duration)
					{
						num = (float)this._duration;
					}
					Rectangle frame = obj.frame;
					int num2 = 10;
					int i = (int)num;
					while (i > 4)
					{
						i -= 4;
						num2++;
						if (num2 > 13)
						{
							num2 = 13;
						}
					}
					obj.ai[0] = 21f;
					obj.ai[1] = 31f;
					obj.frameCounter = (double)i;
					obj.frame.Y = num2 * frame.Height;
				}

				// Token: 0x040078B0 RID: 30896
				private int _duration;

				// Token: 0x040078B1 RID: 30897
				private float _delay;
			}

			// Token: 0x02000AFE RID: 2814
			public class Wait : Actions.NPCs.INPCAction, IAnimationSegmentAction<NPC>
			{
				// Token: 0x06004D56 RID: 19798 RVA: 0x006DAADE File Offset: 0x006D8CDE
				public Wait(int durationInFrames)
				{
					this._duration = durationInFrames;
				}

				// Token: 0x06004D57 RID: 19799 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(NPC obj)
				{
				}

				// Token: 0x170005CF RID: 1487
				// (get) Token: 0x06004D58 RID: 19800 RVA: 0x006DAAED File Offset: 0x006D8CED
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D59 RID: 19801 RVA: 0x006DAAF5 File Offset: 0x006D8CF5
				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					obj.velocity = Vector2.Zero;
				}

				// Token: 0x06004D5A RID: 19802 RVA: 0x006DAB0C File Offset: 0x006D8D0C
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x040078B2 RID: 30898
				private int _duration;

				// Token: 0x040078B3 RID: 30899
				private float _delay;
			}

			// Token: 0x02000AFF RID: 2815
			public class Blink : Actions.NPCs.INPCAction, IAnimationSegmentAction<NPC>
			{
				// Token: 0x06004D5B RID: 19803 RVA: 0x006DAB15 File Offset: 0x006D8D15
				public Blink(int durationInFrames)
				{
					this._duration = durationInFrames;
				}

				// Token: 0x06004D5C RID: 19804 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(NPC obj)
				{
				}

				// Token: 0x170005D0 RID: 1488
				// (get) Token: 0x06004D5D RID: 19805 RVA: 0x006DAB24 File Offset: 0x006D8D24
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D5E RID: 19806 RVA: 0x006DAB2C File Offset: 0x006D8D2C
				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					obj.velocity = Vector2.Zero;
					obj.ai[0] = 0f;
					if (localTimeForObj > this._delay + (float)this._duration)
					{
						return;
					}
					obj.ai[0] = 1001f;
				}

				// Token: 0x06004D5F RID: 19807 RVA: 0x006DAB7A File Offset: 0x006D8D7A
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x040078B4 RID: 30900
				private int _duration;

				// Token: 0x040078B5 RID: 30901
				private float _delay;
			}

			// Token: 0x02000B00 RID: 2816
			public class LookAt : Actions.NPCs.INPCAction, IAnimationSegmentAction<NPC>
			{
				// Token: 0x06004D60 RID: 19808 RVA: 0x006DAB83 File Offset: 0x006D8D83
				public LookAt(int direction)
				{
					this._direction = direction;
				}

				// Token: 0x06004D61 RID: 19809 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(NPC obj)
				{
				}

				// Token: 0x170005D1 RID: 1489
				// (get) Token: 0x06004D62 RID: 19810 RVA: 0x001DA9FB File Offset: 0x001D8BFB
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return 0;
					}
				}

				// Token: 0x06004D63 RID: 19811 RVA: 0x006DAB94 File Offset: 0x006D8D94
				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					obj.direction = (obj.spriteDirection = this._direction);
				}

				// Token: 0x06004D64 RID: 19812 RVA: 0x006DABC0 File Offset: 0x006D8DC0
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x040078B6 RID: 30902
				private int _direction;

				// Token: 0x040078B7 RID: 30903
				private float _delay;
			}

			// Token: 0x02000B01 RID: 2817
			public class PartyHard : Actions.NPCs.INPCAction, IAnimationSegmentAction<NPC>
			{
				// Token: 0x06004D66 RID: 19814 RVA: 0x006DABC9 File Offset: 0x006D8DC9
				public void BindTo(NPC obj)
				{
					obj.ForcePartyHatOn = true;
					obj.UpdateAltTexture();
				}

				// Token: 0x170005D2 RID: 1490
				// (get) Token: 0x06004D67 RID: 19815 RVA: 0x001DA9FB File Offset: 0x001D8BFB
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return 0;
					}
				}

				// Token: 0x06004D68 RID: 19816 RVA: 0x00009E06 File Offset: 0x00008006
				public void ApplyTo(NPC obj, float localTimeForObj)
				{
				}

				// Token: 0x06004D69 RID: 19817 RVA: 0x00009E06 File Offset: 0x00008006
				public void SetDelay(float delay)
				{
				}
			}

			// Token: 0x02000B02 RID: 2818
			public class ForceAltTexture : Actions.NPCs.INPCAction, IAnimationSegmentAction<NPC>
			{
				// Token: 0x06004D6A RID: 19818 RVA: 0x006DABD8 File Offset: 0x006D8DD8
				public ForceAltTexture(int altTexture)
				{
					this._altTexture = altTexture;
				}

				// Token: 0x06004D6B RID: 19819 RVA: 0x006DABE7 File Offset: 0x006D8DE7
				public void BindTo(NPC obj)
				{
					obj.altTexture = this._altTexture;
				}

				// Token: 0x170005D3 RID: 1491
				// (get) Token: 0x06004D6C RID: 19820 RVA: 0x001DA9FB File Offset: 0x001D8BFB
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return 0;
					}
				}

				// Token: 0x06004D6D RID: 19821 RVA: 0x00009E06 File Offset: 0x00008006
				public void ApplyTo(NPC obj, float localTimeForObj)
				{
				}

				// Token: 0x06004D6E RID: 19822 RVA: 0x00009E06 File Offset: 0x00008006
				public void SetDelay(float delay)
				{
				}

				// Token: 0x040078B8 RID: 30904
				private int _altTexture;
			}

			// Token: 0x02000B03 RID: 2819
			public class Variant : Actions.NPCs.INPCAction, IAnimationSegmentAction<NPC>
			{
				// Token: 0x06004D6F RID: 19823 RVA: 0x006DABF5 File Offset: 0x006D8DF5
				public Variant(int variant)
				{
					this._variant = variant;
				}

				// Token: 0x06004D70 RID: 19824 RVA: 0x006DAC04 File Offset: 0x006D8E04
				public void BindTo(NPC obj)
				{
					obj.townNpcVariationIndex = this._variant;
				}

				// Token: 0x170005D4 RID: 1492
				// (get) Token: 0x06004D71 RID: 19825 RVA: 0x001DA9FB File Offset: 0x001D8BFB
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return 0;
					}
				}

				// Token: 0x06004D72 RID: 19826 RVA: 0x00009E06 File Offset: 0x00008006
				public void ApplyTo(NPC obj, float localTimeForObj)
				{
				}

				// Token: 0x06004D73 RID: 19827 RVA: 0x00009E06 File Offset: 0x00008006
				public void SetDelay(float delay)
				{
				}

				// Token: 0x040078B9 RID: 30905
				private int _variant;
			}

			// Token: 0x02000B04 RID: 2820
			public class ZombieKnockOnDoor : Actions.NPCs.INPCAction, IAnimationSegmentAction<NPC>
			{
				// Token: 0x06004D74 RID: 19828 RVA: 0x006DAC12 File Offset: 0x006D8E12
				public ZombieKnockOnDoor(int durationInFrames)
				{
					this._duration = durationInFrames;
				}

				// Token: 0x06004D75 RID: 19829 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(NPC obj)
				{
				}

				// Token: 0x170005D5 RID: 1493
				// (get) Token: 0x06004D76 RID: 19830 RVA: 0x006DAC4B File Offset: 0x006D8E4B
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D77 RID: 19831 RVA: 0x006DAC53 File Offset: 0x006D8E53
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004D78 RID: 19832 RVA: 0x006DAC5C File Offset: 0x006D8E5C
				public void ApplyTo(NPC obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					float num = localTimeForObj - this._delay;
					if (num > (float)this._duration)
					{
						num = (float)this._duration;
					}
					if ((int)num % 60 / 4 <= 3)
					{
						obj.position += this.bumpOffset;
						obj.velocity = this.bumpVelocity;
						return;
					}
					obj.position -= this.bumpOffset;
					obj.velocity = Vector2.Zero;
				}

				// Token: 0x040078BA RID: 30906
				private int _duration;

				// Token: 0x040078BB RID: 30907
				private float _delay;

				// Token: 0x040078BC RID: 30908
				private Vector2 bumpOffset = new Vector2(-1f, 0f);

				// Token: 0x040078BD RID: 30909
				private Vector2 bumpVelocity = new Vector2(0.75f, 0f);
			}
		}

		// Token: 0x020009AA RID: 2474
		public class Sprites
		{
			// Token: 0x02000B05 RID: 2821
			public interface ISpriteAction : IAnimationSegmentAction<Segments.LooseSprite>
			{
			}

			// Token: 0x02000B06 RID: 2822
			public class Fade : Actions.Sprites.ISpriteAction, IAnimationSegmentAction<Segments.LooseSprite>
			{
				// Token: 0x06004D79 RID: 19833 RVA: 0x006DACDE File Offset: 0x006D8EDE
				public Fade(float opacityTarget)
				{
					this._duration = 0;
					this._opacityTarget = opacityTarget;
				}

				// Token: 0x06004D7A RID: 19834 RVA: 0x006DACF4 File Offset: 0x006D8EF4
				public Fade(float opacityTarget, int duration)
				{
					this._duration = duration;
					this._opacityTarget = opacityTarget;
				}

				// Token: 0x06004D7B RID: 19835 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(Segments.LooseSprite obj)
				{
				}

				// Token: 0x170005D6 RID: 1494
				// (get) Token: 0x06004D7C RID: 19836 RVA: 0x006DAD0A File Offset: 0x006D8F0A
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D7D RID: 19837 RVA: 0x006DAD12 File Offset: 0x006D8F12
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004D7E RID: 19838 RVA: 0x006DAD1C File Offset: 0x006D8F1C
				public void ApplyTo(Segments.LooseSprite obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					if (this._duration == 0)
					{
						obj.CurrentOpacity = this._opacityTarget;
						return;
					}
					float num = localTimeForObj - this._delay;
					if (num > (float)this._duration)
					{
						num = (float)this._duration;
					}
					obj.CurrentOpacity = MathHelper.Lerp(obj.CurrentOpacity, this._opacityTarget, Utils.GetLerpValue(0f, (float)this._duration, num, true));
				}

				// Token: 0x040078BE RID: 30910
				private int _duration;

				// Token: 0x040078BF RID: 30911
				private float _opacityTarget;

				// Token: 0x040078C0 RID: 30912
				private float _delay;
			}

			// Token: 0x02000B07 RID: 2823
			public abstract class AScale : Actions.Sprites.ISpriteAction, IAnimationSegmentAction<Segments.LooseSprite>
			{
				// Token: 0x06004D7F RID: 19839 RVA: 0x006DAD8D File Offset: 0x006D8F8D
				public AScale(Vector2 scaleTarget)
				{
					this.Duration = 0;
					this._scaleTarget = scaleTarget;
				}

				// Token: 0x06004D80 RID: 19840 RVA: 0x006DADA3 File Offset: 0x006D8FA3
				public AScale(Vector2 scaleTarget, int duration)
				{
					this.Duration = duration;
					this._scaleTarget = scaleTarget;
				}

				// Token: 0x06004D81 RID: 19841 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(Segments.LooseSprite obj)
				{
				}

				// Token: 0x170005D7 RID: 1495
				// (get) Token: 0x06004D82 RID: 19842 RVA: 0x006DADB9 File Offset: 0x006D8FB9
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this.Duration;
					}
				}

				// Token: 0x06004D83 RID: 19843 RVA: 0x006DADC1 File Offset: 0x006D8FC1
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004D84 RID: 19844 RVA: 0x006DADCC File Offset: 0x006D8FCC
				public void ApplyTo(Segments.LooseSprite obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					if (this.Duration == 0)
					{
						obj.CurrentDrawData.scale = this._scaleTarget;
						return;
					}
					float num = localTimeForObj - this._delay;
					if (num > (float)this.Duration)
					{
						num = (float)this.Duration;
					}
					float progress = this.GetProgress(num);
					obj.CurrentDrawData.scale = Vector2.Lerp(obj.CurrentDrawData.scale, this._scaleTarget, progress);
				}

				// Token: 0x06004D85 RID: 19845
				protected abstract float GetProgress(float durationInFramesToApply);

				// Token: 0x040078C1 RID: 30913
				protected int Duration;

				// Token: 0x040078C2 RID: 30914
				private Vector2 _scaleTarget;

				// Token: 0x040078C3 RID: 30915
				private float _delay;
			}

			// Token: 0x02000B08 RID: 2824
			public class LinearScale : Actions.Sprites.AScale
			{
				// Token: 0x06004D86 RID: 19846 RVA: 0x006DAE42 File Offset: 0x006D9042
				public LinearScale(Vector2 scaleTarget) : base(scaleTarget)
				{
				}

				// Token: 0x06004D87 RID: 19847 RVA: 0x006DAE4B File Offset: 0x006D904B
				public LinearScale(Vector2 scaleTarget, int duration) : base(scaleTarget, duration)
				{
				}

				// Token: 0x06004D88 RID: 19848 RVA: 0x006DAE55 File Offset: 0x006D9055
				protected override float GetProgress(float durationInFramesToApply)
				{
					return Utils.GetLerpValue(0f, (float)this.Duration, durationInFramesToApply, true);
				}
			}

			// Token: 0x02000B09 RID: 2825
			public class OutCircleScale : Actions.Sprites.AScale
			{
				// Token: 0x06004D89 RID: 19849 RVA: 0x006DAE42 File Offset: 0x006D9042
				public OutCircleScale(Vector2 scaleTarget) : base(scaleTarget)
				{
				}

				// Token: 0x06004D8A RID: 19850 RVA: 0x006DAE4B File Offset: 0x006D904B
				public OutCircleScale(Vector2 scaleTarget, int duration) : base(scaleTarget, duration)
				{
				}

				// Token: 0x06004D8B RID: 19851 RVA: 0x006DAE6C File Offset: 0x006D906C
				protected override float GetProgress(float durationInFramesToApply)
				{
					float num = Utils.GetLerpValue(0f, (float)this.Duration, durationInFramesToApply, true);
					num -= 1f;
					return (float)Math.Sqrt((double)(1f - num * num));
				}
			}

			// Token: 0x02000B0A RID: 2826
			public class Wait : Actions.Sprites.ISpriteAction, IAnimationSegmentAction<Segments.LooseSprite>
			{
				// Token: 0x06004D8C RID: 19852 RVA: 0x006DAEA5 File Offset: 0x006D90A5
				public Wait(int durationInFrames)
				{
					this._duration = durationInFrames;
				}

				// Token: 0x06004D8D RID: 19853 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(Segments.LooseSprite obj)
				{
				}

				// Token: 0x170005D8 RID: 1496
				// (get) Token: 0x06004D8E RID: 19854 RVA: 0x006DAEB4 File Offset: 0x006D90B4
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D8F RID: 19855 RVA: 0x006DAEBC File Offset: 0x006D90BC
				public void ApplyTo(Segments.LooseSprite obj, float localTimeForObj)
				{
					float delay = this._delay;
				}

				// Token: 0x06004D90 RID: 19856 RVA: 0x006DAEC7 File Offset: 0x006D90C7
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x040078C4 RID: 30916
				private int _duration;

				// Token: 0x040078C5 RID: 30917
				private float _delay;
			}

			// Token: 0x02000B0B RID: 2827
			public class SimulateGravity : Actions.Sprites.ISpriteAction, IAnimationSegmentAction<Segments.LooseSprite>
			{
				// Token: 0x06004D91 RID: 19857 RVA: 0x006DAED0 File Offset: 0x006D90D0
				public SimulateGravity(Vector2 initialVelocity, Vector2 gravityPerFrame, float rotationPerFrame, int duration)
				{
					this._duration = duration;
					this._initialVelocity = initialVelocity;
					this._gravityPerFrame = gravityPerFrame;
					this._rotationPerFrame = rotationPerFrame;
				}

				// Token: 0x06004D92 RID: 19858 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(Segments.LooseSprite obj)
				{
				}

				// Token: 0x170005D9 RID: 1497
				// (get) Token: 0x06004D93 RID: 19859 RVA: 0x006DAEF5 File Offset: 0x006D90F5
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D94 RID: 19860 RVA: 0x006DAEFD File Offset: 0x006D90FD
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004D95 RID: 19861 RVA: 0x006DAF08 File Offset: 0x006D9108
				public void ApplyTo(Segments.LooseSprite obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					float num = localTimeForObj - this._delay;
					if (num > (float)this._duration)
					{
						num = (float)this._duration;
					}
					Vector2 value = this._initialVelocity * num + this._gravityPerFrame * (num * num);
					obj.CurrentDrawData.position = obj.CurrentDrawData.position + value;
					obj.CurrentDrawData.rotation = obj.CurrentDrawData.rotation + this._rotationPerFrame * num;
				}

				// Token: 0x040078C6 RID: 30918
				private int _duration;

				// Token: 0x040078C7 RID: 30919
				private float _delay;

				// Token: 0x040078C8 RID: 30920
				private Vector2 _initialVelocity;

				// Token: 0x040078C9 RID: 30921
				private Vector2 _gravityPerFrame;

				// Token: 0x040078CA RID: 30922
				private float _rotationPerFrame;
			}

			// Token: 0x02000B0C RID: 2828
			public class SetFrame : Actions.Sprites.ISpriteAction, IAnimationSegmentAction<Segments.LooseSprite>
			{
				// Token: 0x06004D96 RID: 19862 RVA: 0x006DAF8D File Offset: 0x006D918D
				public SetFrame(int frameX, int frameY, int paddingX = 2, int paddingY = 2)
				{
					this._targetFrameX = frameX;
					this._targetFrameY = frameY;
					this._paddingX = paddingX;
					this._paddingY = paddingY;
				}

				// Token: 0x06004D97 RID: 19863 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(Segments.LooseSprite obj)
				{
				}

				// Token: 0x170005DA RID: 1498
				// (get) Token: 0x06004D98 RID: 19864 RVA: 0x001DA9FB File Offset: 0x001D8BFB
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return 0;
					}
				}

				// Token: 0x06004D99 RID: 19865 RVA: 0x006DAFB2 File Offset: 0x006D91B2
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004D9A RID: 19866 RVA: 0x006DAFBC File Offset: 0x006D91BC
				public void ApplyTo(Segments.LooseSprite obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					Rectangle value = obj.CurrentDrawData.sourceRect.Value;
					value.X = (value.Width + this._paddingX) * this._targetFrameX;
					value.Y = (value.Height + this._paddingY) * this._targetFrameY;
					obj.CurrentDrawData.sourceRect = new Rectangle?(value);
				}

				// Token: 0x040078CB RID: 30923
				private int _targetFrameX;

				// Token: 0x040078CC RID: 30924
				private int _targetFrameY;

				// Token: 0x040078CD RID: 30925
				private int _paddingX;

				// Token: 0x040078CE RID: 30926
				private int _paddingY;

				// Token: 0x040078CF RID: 30927
				private float _delay;
			}

			// Token: 0x02000B0D RID: 2829
			public class SetFrameSequence : Actions.Sprites.ISpriteAction, IAnimationSegmentAction<Segments.LooseSprite>
			{
				// Token: 0x06004D9B RID: 19867 RVA: 0x006DB02B File Offset: 0x006D922B
				public SetFrameSequence(int duration, Point[] frameIndices, int timePerFrame, int paddingX = 2, int paddingY = 2) : this(frameIndices, timePerFrame, paddingX, paddingY)
				{
					this._duration = duration;
					this._loop = true;
				}

				// Token: 0x06004D9C RID: 19868 RVA: 0x006DB047 File Offset: 0x006D9247
				public SetFrameSequence(Point[] frameIndices, int timePerFrame, int paddingX = 2, int paddingY = 2)
				{
					this._frameIndices = frameIndices;
					this._timePerFrame = timePerFrame;
					this._paddingX = paddingX;
					this._paddingY = paddingY;
					this._duration = this._timePerFrame * this._frameIndices.Length;
				}

				// Token: 0x06004D9D RID: 19869 RVA: 0x00009E06 File Offset: 0x00008006
				public void BindTo(Segments.LooseSprite obj)
				{
				}

				// Token: 0x170005DB RID: 1499
				// (get) Token: 0x06004D9E RID: 19870 RVA: 0x006DB081 File Offset: 0x006D9281
				public int ExpectedLengthOfActionInFrames
				{
					get
					{
						return this._duration;
					}
				}

				// Token: 0x06004D9F RID: 19871 RVA: 0x006DB089 File Offset: 0x006D9289
				public void SetDelay(float delay)
				{
					this._delay = delay;
				}

				// Token: 0x06004DA0 RID: 19872 RVA: 0x006DB094 File Offset: 0x006D9294
				public void ApplyTo(Segments.LooseSprite obj, float localTimeForObj)
				{
					if (localTimeForObj < this._delay)
					{
						return;
					}
					Rectangle value = obj.CurrentDrawData.sourceRect.Value;
					int num2;
					if (this._loop)
					{
						int num = this._frameIndices.Length;
						num2 = (int)(localTimeForObj % (float)(this._timePerFrame * num)) / this._timePerFrame;
						if (num2 >= num)
						{
							num2 = num - 1;
						}
					}
					else
					{
						float num3 = localTimeForObj - this._delay;
						if (num3 > (float)this._duration)
						{
							num3 = (float)this._duration;
						}
						num2 = (int)(num3 / (float)this._timePerFrame);
						if (num2 >= this._frameIndices.Length)
						{
							num2 = this._frameIndices.Length - 1;
						}
					}
					Point point = this._frameIndices[num2];
					value.X = (value.Width + this._paddingX) * point.X;
					value.Y = (value.Height + this._paddingY) * point.Y;
					obj.CurrentDrawData.sourceRect = new Rectangle?(value);
				}

				// Token: 0x040078D0 RID: 30928
				private Point[] _frameIndices;

				// Token: 0x040078D1 RID: 30929
				private int _timePerFrame;

				// Token: 0x040078D2 RID: 30930
				private int _paddingX;

				// Token: 0x040078D3 RID: 30931
				private int _paddingY;

				// Token: 0x040078D4 RID: 30932
				private float _delay;

				// Token: 0x040078D5 RID: 30933
				private int _duration;

				// Token: 0x040078D6 RID: 30934
				private bool _loop;
			}
		}
	}
}
