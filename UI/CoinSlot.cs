using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.UI
{
	// Token: 0x020000DD RID: 221
	public class CoinSlot
	{
		// Token: 0x0600187F RID: 6271 RVA: 0x004E1F4B File Offset: 0x004E014B
		public static void UpdateSavings(int slot, int count, out CoinSlot.CoinDrawState drawState)
		{
			CoinSlot.Savings[slot].UpdateState(71 + slot, count, CoinSlot.SavingsCoinJumpScale, out drawState);
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x004E1F68 File Offset: 0x004E0168
		public static float DrawItemCoin(SpriteBatch spriteBatch, Vector2 screenPositionForItemCenter, int coinType, int coinFrame, float scale, float sizeLimit, Color itemColor, float itemFade = 1f)
		{
			int num = coinType - 71;
			Texture2D value = TextureAssets.Coin[num].Value;
			Rectangle rectangle = value.Frame(1, 8, 0, coinFrame, 0, 0);
			Color white = Color.White;
			Color white2 = Color.White;
			float num2 = 1f;
			if ((float)rectangle.Width > sizeLimit || (float)rectangle.Height > sizeLimit)
			{
				if (rectangle.Width > rectangle.Height)
				{
					num2 = sizeLimit / (float)rectangle.Width;
				}
				else
				{
					num2 = sizeLimit / (float)rectangle.Height;
				}
			}
			float num3 = scale * num2;
			SpriteEffects effects = SpriteEffects.None;
			Vector2 origin = rectangle.Size() / 2f;
			Color value2 = ContentSamples.ItemsByType[coinType].GetAlpha(itemColor).MultiplyRGBA(white);
			spriteBatch.Draw(value, screenPositionForItemCenter, new Rectangle?(rectangle), value2 * itemFade, 0f, origin, num3, effects, 0f);
			return num3;
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x004E2044 File Offset: 0x004E0244
		public static void UpdateSlotAnims()
		{
			for (int i = 0; i < CoinSlot.Savings.Length; i++)
			{
				CoinSlot.Savings[i].UpdateAnim();
			}
			for (int j = 0; j < CoinSlot.ChestEntries.Length; j++)
			{
				CoinSlot.ChestEntries[j].UpdateAnim();
			}
			for (int k = 0; k < CoinSlot.InventoryEntries.Length; k++)
			{
				CoinSlot.InventoryEntries[k].UpdateAnim();
			}
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x004E20B8 File Offset: 0x004E02B8
		public static void ForceSlotState(int slot, int context, Item item)
		{
			if (context <= 2)
			{
				CoinSlot.InventoryEntries[slot].ForceState(item.type, item.stack);
				return;
			}
			if (context - 3 > 1)
			{
				return;
			}
			CoinSlot.ChestEntries[slot].ForceState(item.type, item.stack);
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x004E210C File Offset: 0x004E030C
		public static void UpdateDrawState(int slot, int context, Item item, out CoinSlot.CoinDrawState drawState)
		{
			if (context <= 2)
			{
				CoinSlot.InventoryEntries[slot].UpdateState(item.type, item.stack, CoinSlot.ItemSlotCoinJumpScale, out drawState);
				return;
			}
			if (context - 3 > 1)
			{
				drawState.fadeItem = 0;
				drawState.fadeScale = 1f;
				drawState.coinAnimFrame = 0;
				drawState.coinYOffset = 0f;
				drawState.stackTextScale = 1f;
				drawState.stackTextDrawFadeOverload = -1f;
				return;
			}
			CoinSlot.ChestEntries[slot].UpdateState(item.type, item.stack, CoinSlot.ItemSlotCoinJumpScale, out drawState);
		}

		// Token: 0x040012C6 RID: 4806
		private static float[] FadeAnimKeys = new float[]
		{
			0.3f,
			0.4f,
			0.5f,
			0.6f,
			0.7f,
			0.8f,
			0.9f,
			1f,
			1f,
			1f,
			1f,
			1f,
			1f,
			1f,
			1f,
			1f,
			1f,
			1f,
			1f,
			1f,
			1f
		};

		// Token: 0x040012C7 RID: 4807
		private static float[] TextAnimKeys = new float[]
		{
			1f,
			1.0107f,
			1.0391f,
			1.0791f,
			1.125f,
			1.1709f,
			1.2109f,
			1.2393f,
			1.25f,
			1.2393f,
			1.2109f,
			1.1709f,
			1.125f,
			1.0791f,
			1.0391f,
			1.0107f,
			1f
		};

		// Token: 0x040012C8 RID: 4808
		private static float[] JumpAnimKeys = new float[]
		{
			0f,
			0.23748f,
			0.43408f,
			0.59366f,
			0.72007f,
			0.81717f,
			0.88881f,
			0.93885f,
			0.97115f,
			0.98955f,
			0.99793f,
			1f,
			1f,
			1f,
			0.99793f,
			0.98955f,
			0.97115f,
			0.93885f,
			0.88881f,
			0.81717f,
			0.72007f,
			0.59366f,
			0.43408f,
			0.23748f,
			0f
		};

		// Token: 0x040012C9 RID: 4809
		private static int JumpApex = 12;

		// Token: 0x040012CA RID: 4810
		private static int JumpTrigger_TextAnimRangeStart = 9;

		// Token: 0x040012CB RID: 4811
		private static int JumpTrigger_TextAnimRangeEnd = 13;

		// Token: 0x040012CC RID: 4812
		private static float ItemSlotCoinJumpScale = 10f;

		// Token: 0x040012CD RID: 4813
		private static float SavingsCoinJumpScale = 10f;

		// Token: 0x040012CE RID: 4814
		private static int JumpAnimHoldTime = 12;

		// Token: 0x040012CF RID: 4815
		private static int SpinAnimRangeStart = 9;

		// Token: 0x040012D0 RID: 4816
		private static int SpinAnimRangeEnd = 13;

		// Token: 0x040012D1 RID: 4817
		private static CoinSlot.CoinEntry[] Savings = new CoinSlot.CoinEntry[4];

		// Token: 0x040012D2 RID: 4818
		private static CoinSlot.CoinEntry[] ChestEntries = new CoinSlot.CoinEntry[200];

		// Token: 0x040012D3 RID: 4819
		private static CoinSlot.CoinEntry[] InventoryEntries = new CoinSlot.CoinEntry[59];

		// Token: 0x020006FC RID: 1788
		private struct CoinEntry
		{
			// Token: 0x06003FBF RID: 16319 RVA: 0x0069A6EC File Offset: 0x006988EC
			public void ForceState(int itemType, int itemStack)
			{
				this.Type = itemType;
				this.Stack = itemStack;
				this.TextAnimFrame = 0;
				this.JumpAnimFrame = 0;
				this.JumpAnimHold = 0;
				this.SpinAnimFrame = 0;
				this.FadeItemType = 0;
			}

			// Token: 0x06003FC0 RID: 16320 RVA: 0x0069A720 File Offset: 0x00698920
			public void UpdateState(int itemType, int itemStack, float jumpScale, out CoinSlot.CoinDrawState drawState)
			{
				if (this.Type != itemType || this.DrawActive == 0)
				{
					bool flag = true;
					if (itemType != 0 && this.FadeItemType == itemType && this.DrawActive != 0)
					{
						flag = false;
					}
					if (itemType == 0 && this.DrawActive != 0 && ItemID.Sets.CommonCoin[this.Type])
					{
						this.FadeItemType = this.Type;
					}
					else
					{
						this.FadeItemType = 0;
					}
					if (this.FadeItemType != 0)
					{
						flag = false;
					}
					this.Type = itemType;
					if (this.DrawActive == 0)
					{
						this.Stack = itemStack;
					}
					if (flag)
					{
						this.TextAnimFrame = 0;
						this.JumpAnimFrame = 0;
						this.JumpAnimHold = 0;
						this.SpinAnimFrame = 0;
					}
				}
				this.DrawActive = 2;
				if (ItemID.Sets.CommonCoin[this.Type] || this.FadeItemType != 0)
				{
					if (this.Stack != itemStack)
					{
						this.Stack = itemStack;
						if (this.TextAnimFrame == 0)
						{
							this.TextAnimFrame = CoinSlot.TextAnimKeys.Length - 1;
						}
					}
					if (this.TextAnimFrame >= CoinSlot.JumpTrigger_TextAnimRangeStart && this.TextAnimFrame <= CoinSlot.JumpTrigger_TextAnimRangeEnd)
					{
						this.JumpAnimHold = CoinSlot.JumpAnimHoldTime;
						if (this.JumpAnimFrame == 0)
						{
							this.JumpAnimFrame = CoinSlot.JumpAnimKeys.Length - 1;
						}
					}
				}
				drawState.stackTextScale = CoinSlot.TextAnimKeys[this.TextAnimFrame];
				drawState.coinYOffset = CoinSlot.JumpAnimKeys[this.JumpAnimFrame] * jumpScale;
				drawState.coinAnimFrame = this.SpinAnimFrame / 2;
				drawState.fadeItem = this.FadeItemType;
				drawState.fadeScale = 1f;
				if (this.FadeItemType != 0)
				{
					if (this.TextAnimFrame > 0 || this.JumpAnimFrame >= CoinSlot.JumpApex || this.JumpAnimFrame >= CoinSlot.FadeAnimKeys.Length)
					{
						drawState.stackTextDrawFadeOverload = 1f;
					}
					else
					{
						drawState.stackTextDrawFadeOverload = CoinSlot.FadeAnimKeys[this.JumpAnimFrame];
					}
					drawState.fadeScale = drawState.stackTextDrawFadeOverload;
					return;
				}
				if (this.Stack != 1 || (this.TextAnimFrame <= 0 && this.JumpAnimFrame == 0))
				{
					drawState.stackTextDrawFadeOverload = -1f;
					return;
				}
				if (this.TextAnimFrame > 0 || this.JumpAnimFrame >= CoinSlot.JumpApex || this.JumpAnimFrame >= CoinSlot.FadeAnimKeys.Length)
				{
					drawState.stackTextDrawFadeOverload = 1f;
					return;
				}
				drawState.stackTextDrawFadeOverload = CoinSlot.FadeAnimKeys[this.JumpAnimFrame];
			}

			// Token: 0x06003FC1 RID: 16321 RVA: 0x0069A960 File Offset: 0x00698B60
			public void UpdateAnim()
			{
				if (this.DrawActive > 0)
				{
					this.DrawActive--;
				}
				if (this.FadeItemType > 0 && this.JumpAnimFrame == 0 && this.TextAnimFrame == 0)
				{
					this.FadeItemType = 0;
				}
				if (this.TextAnimFrame > 0)
				{
					this.TextAnimFrame--;
				}
				if (this.JumpAnimHold > 0)
				{
					this.JumpAnimHold--;
				}
				if (this.JumpAnimFrame > 0)
				{
					if (this.JumpAnimHold > 0)
					{
						if (this.JumpAnimFrame != CoinSlot.JumpApex)
						{
							if (this.JumpAnimFrame < CoinSlot.JumpApex)
							{
								this.JumpAnimFrame = CoinSlot.JumpApex + CoinSlot.JumpApex - this.JumpAnimFrame;
							}
							this.JumpAnimFrame--;
						}
					}
					else
					{
						this.JumpAnimFrame--;
					}
				}
				if (this.JumpAnimFrame >= CoinSlot.SpinAnimRangeStart && this.JumpAnimFrame <= CoinSlot.SpinAnimRangeEnd)
				{
					this.SpinAnimFrame = (this.SpinAnimFrame + 1) % 14;
					return;
				}
				if (this.SpinAnimFrame != 0)
				{
					this.SpinAnimFrame = (this.SpinAnimFrame + 1) % 14;
				}
			}

			// Token: 0x04006816 RID: 26646
			public int Type;

			// Token: 0x04006817 RID: 26647
			public int Stack;

			// Token: 0x04006818 RID: 26648
			public int TextAnimFrame;

			// Token: 0x04006819 RID: 26649
			public int JumpAnimFrame;

			// Token: 0x0400681A RID: 26650
			public int SpinAnimFrame;

			// Token: 0x0400681B RID: 26651
			public int DrawActive;

			// Token: 0x0400681C RID: 26652
			public int JumpAnimHold;

			// Token: 0x0400681D RID: 26653
			public int FadeItemType;
		}

		// Token: 0x020006FD RID: 1789
		public struct CoinDrawState
		{
			// Token: 0x0400681E RID: 26654
			public int coinAnimFrame;

			// Token: 0x0400681F RID: 26655
			public float coinYOffset;

			// Token: 0x04006820 RID: 26656
			public float stackTextScale;

			// Token: 0x04006821 RID: 26657
			public float stackTextDrawFadeOverload;

			// Token: 0x04006822 RID: 26658
			public int fadeItem;

			// Token: 0x04006823 RID: 26659
			public float fadeScale;
		}
	}
}
