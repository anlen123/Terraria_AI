using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI.Gamepad;

namespace Terraria.UI
{
	// Token: 0x020000DF RID: 223
	public class BannerClaimingUI : IPipsUI
	{
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001887 RID: 6279 RVA: 0x004E23AC File Offset: 0x004E05AC
		// (set) Token: 0x06001888 RID: 6280 RVA: 0x004E23B4 File Offset: 0x004E05B4
		public bool AnyAvailableBanners { get; private set; }

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06001889 RID: 6281 RVA: 0x004E23BD File Offset: 0x004E05BD
		// (set) Token: 0x0600188A RID: 6282 RVA: 0x004E23C4 File Offset: 0x004E05C4
		private float inventoryScale
		{
			get
			{
				return Main.inventoryScale;
			}
			set
			{
				Main.inventoryScale = value;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x0600188B RID: 6283 RVA: 0x004E23CC File Offset: 0x004E05CC
		// (set) Token: 0x0600188C RID: 6284 RVA: 0x004E23D3 File Offset: 0x004E05D3
		private Color inventoryBack
		{
			get
			{
				return Main.inventoryBack;
			}
			set
			{
				Main.inventoryBack = value;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x0600188D RID: 6285 RVA: 0x004E23DB File Offset: 0x004E05DB
		public int FocusedItemType
		{
			get
			{
				if (this._focusedElementIndex >= 0 && this._focusedElementIndex < this._availableItemsCount)
				{
					return this._squashedItemTypesToShow[this._focusedElementIndex].Type;
				}
				return 0;
			}
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x004E240C File Offset: 0x004E060C
		public void DrawBannersList(SpriteBatch spriteBatch, int adjY, int middleY, Color craftingTipColor)
		{
			UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = -1;
			UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
			int num = this.UpdateAndGetClaimableItemsCount();
			BannerSystem.AnyNewClaimableBanners = false;
			if (num <= 0)
			{
				Main.TryChangePipsPage(Main.PipPage.Recipes);
				return;
			}
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, Language.GetTextValue("GameUI.BannersTitle"), new Vector2(76f, (float)(414 + adjY)), craftingTipColor, 0f, default(Vector2), 1f, SpriteEffects.None, 0f, null, null);
			this.DrawBannersList_Move(num);
			if (Main.PipsFastScroll)
			{
				this._currentElementOffset = (float)this._focusedElementIndex;
				Main.PipsFastScroll = false;
			}
			bool flag = Main.mouseLeft || Main.mouseRight;
			if (!flag)
			{
				this._bannerStackRequestLimiter = 0;
			}
			for (int i = 0; i < num; i++)
			{
				float num2 = ((float)i - this._currentElementOffset) * 65f;
				if (Math.Abs(num2) <= (float)middleY)
				{
					BannerClaimingUI.Entry entry = this._squashedItemTypesToShow[i];
					Item item;
					if (ContentSamples.ItemsByType.TryGetValue(entry.Type, out item))
					{
						this.inventoryScale = 100f / (Math.Abs(num2) + 100f);
						if ((double)this.inventoryScale < 0.75)
						{
							this.inventoryScale = 0.75f;
						}
						double num3;
						Color lightColor;
						this.GetItemSlotColors(num2, middleY, 100f, i, out num3, out lightColor);
						int num4 = (int)(46f - 26f * this.inventoryScale);
						int num5 = (int)(410f + num2 * this.inventoryScale - 30f * this.inventoryScale + (float)adjY);
						if (!Main.LocalPlayer.creativeInterface && Main.mouseX >= num4 && (float)Main.mouseX <= (float)num4 + (float)TextureAssets.InventoryBack.Width() * this.inventoryScale && Main.mouseY >= num5 && (float)Main.mouseY <= (float)num5 + (float)TextureAssets.InventoryBack.Height() * this.inventoryScale && !PlayerInput.IgnoreMouseInterface)
						{
							Main.craftingHide = true;
							Main.LocalPlayer.mouseInterface = true;
							if (flag)
							{
								if (this._focusedElementIndex != i)
								{
									if ((Main.mouseLeft && Main.mouseLeftRelease) || (Main.mouseRight && Main.mouseRightRelease))
									{
										this._focusedElementIndex = i;
										this._bannerStackRequestLimiter = -1;
									}
								}
								else if (num2 == 0f)
								{
									this.TryClaimingBanner(entry);
								}
							}
							ItemSlot.MouseHover(item, 35);
						}
						num3 -= 50.0;
						if (num3 < 0.0)
						{
							num3 = 0.0;
						}
						if (i == this._focusedElementIndex)
						{
							UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = 0;
							if (PlayerInput.SettingsForUI.HighlightThingsForMouse)
							{
								ItemSlot.DrawGoldBGForCraftingMaterial = true;
							}
						}
						else
						{
							UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
						}
						Color inventoryBack = this.inventoryBack;
						this.inventoryBack = new Color((int)((byte)num3), (int)((byte)num3), (int)((byte)num3), (int)((byte)num3));
						int stack = item.stack;
						item.stack = entry.Stack;
						ItemSlot.Draw(spriteBatch, ref item, 35, new Vector2((float)num4, (float)num5), lightColor);
						item.stack = stack;
						this.inventoryBack = inventoryBack;
					}
				}
			}
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x004E2714 File Offset: 0x004E0914
		private void DrawBannersList_Move(int claimableItemsCount)
		{
			this._focusedElementIndex = Utils.Clamp<int>(this._focusedElementIndex, 0, claimableItemsCount - 1);
			this._currentElementOffset = Utils.Clamp<float>(this._currentElementOffset, 0f, (float)(claimableItemsCount - 1));
			int num = (int)this._currentElementOffset;
			this._currentElementOffset = MathHelper.Lerp(this._currentElementOffset, (float)this._focusedElementIndex, 0.03f);
			this._currentElementOffset = Utils.MoveTowards(this._currentElementOffset, (float)this._focusedElementIndex, 0.1f);
			if (num != (int)this._currentElementOffset)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x004E27AF File Offset: 0x004E09AF
		private void TryClaimingBanner(BannerClaimingUI.Entry bannerEntry)
		{
			ItemSlot.HandleItemPickupAction<BannerClaimingUI.Entry>(new ItemSlot.ItemPickupAction<BannerClaimingUI.Entry>(this.SendRequestForBanner), bannerEntry, bannerEntry.Type, bannerEntry.Stack, ref this._bannerStackRequestLimiter);
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x004E27D5 File Offset: 0x004E09D5
		private void SendRequestForBanner(BannerClaimingUI.Entry bannerEntry, int itemAmount)
		{
			BannerSystem.RequestBannerClaim(bannerEntry.Index, itemAmount);
			SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x004E27F8 File Offset: 0x004E09F8
		private int UpdateAndGetClaimableItemsCount()
		{
			ushort[] claimableBannerCounts = BannerSystem.GetClaimableBannerCounts();
			BannerClaimingUI.Entry[] squashedItemTypesToShow = this._squashedItemTypesToShow;
			int num = 0;
			Array.Clear(squashedItemTypesToShow, 0, squashedItemTypesToShow.Length);
			for (int i = 1; i < claimableBannerCounts.Length; i++)
			{
				ushort num2 = claimableBannerCounts[i];
				if (num2 > 0)
				{
					squashedItemTypesToShow[num++] = new BannerClaimingUI.Entry
					{
						Index = i,
						Type = BannerSystem.BannerToItem(i),
						Stack = (int)num2
					};
				}
			}
			this.AnyAvailableBanners = (num > 0);
			this._availableItemsCount = num;
			if (!this.AnyAvailableBanners)
			{
				BannerSystem.AnyNewClaimableBanners = false;
			}
			return num;
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x004E288C File Offset: 0x004E0A8C
		private void GetItemSlotColors(float offset, int middleY, float fadeInValue, int recipeIndex, out double inventoryAlpha, out Color inventoryColor2)
		{
			inventoryAlpha = (double)(this.inventoryBack.A + 50);
			double num = 255.0;
			if (Math.Abs(offset) > (float)middleY - fadeInValue)
			{
				inventoryAlpha = (double)(150f * (fadeInValue - (Math.Abs(offset) - ((float)middleY - fadeInValue)))) * 0.01;
				num = (double)(255f * (fadeInValue - (Math.Abs(offset) - ((float)middleY - fadeInValue)))) * 0.01;
			}
			new Color((int)((byte)inventoryAlpha), (int)((byte)inventoryAlpha), (int)((byte)inventoryAlpha), (int)((byte)inventoryAlpha));
			inventoryColor2 = new Color((int)((byte)num), (int)((byte)num), (int)((byte)num), (int)((byte)num));
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x004E2930 File Offset: 0x004E0B30
		public void DrawGridToggle(SpriteBatch spriteBatch, int adjY)
		{
			if (this.UpdateAndGetClaimableItemsCount() == 0)
			{
				if (Main.PipsCurrentPage == Main.PipPage.Banners)
				{
					Main.PipsUseGrid = false;
				}
				return;
			}
			int num = 128;
			int num2 = 450 + adjY;
			if (Main.InGuideCraftMenu)
			{
				num2 -= 150;
			}
			UILinkPointNavigator.SetPosition(11002, new Vector2((float)num, (float)num2));
			bool flag = Main.mouseX > num - 15 && Main.mouseX < num + 15 && Main.mouseY > num2 - 15 && Main.mouseY < num2 + 15 && !PlayerInput.IgnoreMouseInterface;
			if (Main.PipsCurrentPage == Main.PipPage.Banners)
			{
				Utils.DrawSelectedCraftingBarIndicator(spriteBatch, num, num2);
			}
			int num3 = 2 - (Main.PipsCurrentPage == Main.PipPage.Banners && Main.PipsUseGrid).ToInt() * 2 + flag.ToInt();
			Asset<Texture2D> asset = TextureAssets.BannerToggle[num3];
			spriteBatch.Draw(asset.Value, new Vector2((float)num, (float)num2), null, Color.White, 0f, asset.Size() / 2f, 1f, SpriteEffects.None, 0f);
			if (flag)
			{
				Main.instance.MouseTextNoOverride(Language.GetTextValue("GameUI.BannersWindow"), 0, 0, -1, -1, -1, -1, 0);
				Main.player[Main.myPlayer].mouseInterface = true;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					if (!Main.TryChangePipsPage(Main.PipPage.Banners))
					{
						Main.PipsUseGrid = !Main.PipsUseGrid;
					}
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				}
			}
			Main.DoStatefulTickSound(ref Main.GridToggleMouseOverBanners, flag);
			if (BannerSystem.AnyNewClaimableBanners)
			{
				Utils.DrawNotificationIcon(spriteBatch, new Vector2((float)num, (float)(num2 - 7)), 1f, false);
			}
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x004E2ACC File Offset: 0x004E0CCC
		public void DrawBannersGrid(SpriteBatch spriteBatch)
		{
			int num = this.UpdateAndGetClaimableItemsCount();
			BannerSystem.AnyNewClaimableBanners = false;
			UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = -1;
			UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
			int num2 = 42;
			this.inventoryScale = 0.75f;
			int num3 = 340;
			int num4 = 310;
			int num5 = (Main.screenWidth - num4 - 280) / num2;
			int num6 = (Main.screenHeight - num3 - 20) / num2;
			UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow = num5;
			UILinkPointNavigator.Shortcuts.CRAFT_IconsPerColumn = num6;
			int num7 = 0;
			int num8 = 0;
			int num9 = num4;
			int num10 = num3;
			int mouseX = Main.mouseX;
			int mouseY = Main.mouseY;
			int num11 = num4 - 20;
			int num12 = num3 + 2;
			if (this._gridStartingElementIndex > num - num5 * num6)
			{
				this._gridStartingElementIndex = num - num5 * num6;
				if (this._gridStartingElementIndex < 0)
				{
					this._gridStartingElementIndex = 0;
				}
			}
			if (this._gridStartingElementIndex > 0)
			{
				if (mouseX >= num11 && mouseX <= num11 + TextureAssets.CraftUpButton.Width() && mouseY >= num12 && mouseY <= num12 + TextureAssets.CraftUpButton.Height() && !PlayerInput.IgnoreMouseInterface)
				{
					Main.LocalPlayer.mouseInterface = true;
					if (Main.mouseLeftRelease && Main.mouseLeft)
					{
						this._gridStartingElementIndex -= num5;
						if (this._gridStartingElementIndex < 0)
						{
							this._gridStartingElementIndex = 0;
						}
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						Main.mouseLeftRelease = false;
					}
				}
				spriteBatch.Draw(TextureAssets.CraftUpButton.Value, new Vector2((float)num11, (float)num12), null, new Color(200, 200, 200, 200), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
			if (this._gridStartingElementIndex < num - num5 * num6)
			{
				num12 += 20;
				if (mouseX >= num11 && mouseX <= num11 + TextureAssets.CraftUpButton.Width() && mouseY >= num12 && mouseY <= num12 + TextureAssets.CraftUpButton.Height() && !PlayerInput.IgnoreMouseInterface)
				{
					Main.LocalPlayer.mouseInterface = true;
					if (Main.mouseLeftRelease && Main.mouseLeft)
					{
						this._gridStartingElementIndex += num5;
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						if (this._gridStartingElementIndex > num - num5)
						{
							this._gridStartingElementIndex = num - num5;
						}
						Main.mouseLeftRelease = false;
					}
				}
				spriteBatch.Draw(TextureAssets.CraftDownButton.Value, new Vector2((float)num11, (float)num12), null, new Color(200, 200, 200, 200), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
			this._gridStartingElementIndex = Utils.Clamp<int>(this._gridStartingElementIndex, 0, num - 1);
			this._gridSelectionCandidateIndex = Utils.Clamp<int>(this._gridSelectionCandidateIndex, 0, num - 1);
			int num13 = this._gridStartingElementIndex;
			while (num13 < num && num13 < num)
			{
				int num14 = num9;
				int num15 = num10;
				double num16 = (double)(this.inventoryBack.A + 50);
				double num17 = 255.0;
				BannerClaimingUI.Entry entry = this._squashedItemTypesToShow[num13];
				Item item;
				if (ContentSamples.ItemsByType.TryGetValue(entry.Type, out item))
				{
					new Color((int)((byte)num16), (int)((byte)num16), (int)((byte)num16), (int)((byte)num16));
					new Color((int)((byte)num17), (int)((byte)num17), (int)((byte)num17), (int)((byte)num17));
					if (mouseX >= num14 && (float)mouseX <= (float)num14 + (float)TextureAssets.InventoryBack.Width() * this.inventoryScale && mouseY >= num15 && (float)mouseY <= (float)num15 + (float)TextureAssets.InventoryBack.Height() * this.inventoryScale && !PlayerInput.IgnoreMouseInterface)
					{
						Main.LocalPlayer.mouseInterface = true;
						if (Main.mouseLeftRelease && Main.mouseLeft)
						{
							this._focusedElementIndex = num13;
							Main.PipsFastScroll = true;
							Main.PipsUseGrid = false;
							SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
							Main.mouseLeftRelease = false;
							if (PlayerInput.UsingGamepadUI)
							{
								UILinkPointNavigator.ChangePage(22);
								Main.LockCraftingForThisCraftClickDuration();
							}
						}
						Main.craftingHide = true;
						Main.HoverItem = item.Clone();
						Main.HoverItem.stack = entry.Stack;
						ItemSlot.MouseHover(35);
						Main.hoverItemName = item.Name;
						if (item.stack > 1)
						{
							Main.hoverItemName = string.Concat(new object[]
							{
								Main.hoverItemName,
								" (",
								item.stack,
								")"
							});
						}
					}
					if (num > 0)
					{
						num16 -= 50.0;
						if (num16 < 0.0)
						{
							num16 = 0.0;
						}
						UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = ((this._gridSelectionCandidateIndex == num13) ? 0 : -1);
						Color inventoryBack = this.inventoryBack;
						this.inventoryBack = new Color((int)((byte)num16), (int)((byte)num16), (int)((byte)num16), (int)((byte)num16));
						int stack = item.stack;
						item.stack = entry.Stack;
						ItemSlot.Draw(spriteBatch, ref item, 35, new Vector2((float)num14, (float)num15), default(Color));
						item.stack = stack;
						this.inventoryBack = inventoryBack;
					}
					num9 += num2;
					num7++;
					if (num7 >= num5)
					{
						num9 = num4;
						num10 += num2;
						num7 = 0;
						num8++;
						if (num8 >= num6)
						{
							break;
						}
					}
				}
				num13++;
			}
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x004E3035 File Offset: 0x004E1235
		public void ScrollCraftingList(int mouseWheel)
		{
			this._focusedElementIndex += mouseWheel;
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x004E3045 File Offset: 0x004E1245
		public void ScrollCraftingGrid(int mouseWheel, int width)
		{
			this._gridStartingElementIndex -= mouseWheel * width;
			if (mouseWheel > 0)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x004E3070 File Offset: 0x004E1270
		public void NavigatePipsList(int yOffset)
		{
			float currentElementOffset = this._currentElementOffset;
			this._focusedElementIndex = Utils.Clamp<int>(this._focusedElementIndex + yOffset, 0, this._availableItemsCount - 1);
			if (currentElementOffset != this._currentElementOffset)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x004E30BC File Offset: 0x004E12BC
		public void NavigatePipsGrid(int xOffset, int yOffset)
		{
			int craft_IconsPerRow = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow;
			int num = Math.Min(UILinkPointNavigator.Shortcuts.CRAFT_IconsPerColumn, (int)Math.Ceiling((double)((float)this._availableItemsCount / (float)craft_IconsPerRow)));
			int num2 = this._gridSelectionCandidateIndex - this._gridStartingElementIndex;
			int num3 = num2 % craft_IconsPerRow;
			int num4 = num2 / craft_IconsPerRow;
			if (xOffset < 0 && num3 == 0)
			{
				xOffset = 0;
			}
			if (xOffset > 0 && num3 >= craft_IconsPerRow)
			{
				xOffset = 0;
			}
			if (yOffset < 0 && this._gridStartingElementIndex > 0 && num4 == 0)
			{
				this._gridStartingElementIndex -= craft_IconsPerRow;
			}
			if (yOffset > 0 && num4 == craft_IconsPerRow - 1)
			{
				this._gridStartingElementIndex += craft_IconsPerRow;
			}
			this._gridStartingElementIndex = Utils.Clamp<int>(this._gridStartingElementIndex, 0, Math.Max(0, this._availableItemsCount - 1 - craft_IconsPerRow * num));
			int num5 = this._gridSelectionCandidateIndex - this._gridStartingElementIndex;
			num3 = num5 % craft_IconsPerRow;
			num4 = num5 / craft_IconsPerRow;
			if (yOffset < 0 && this._gridStartingElementIndex == 0 && num4 == 0)
			{
				yOffset = 0;
			}
			if (yOffset > 0 && num4 == num - 1)
			{
				yOffset = 0;
			}
			int num6 = (this._availableItemsCount - this._gridStartingElementIndex) % craft_IconsPerRow;
			if (yOffset > 0 && num6 != 0 && num4 == num - 2 && num3 >= num6)
			{
				yOffset = 0;
			}
			int gridSelectionCandidateIndex = this._gridSelectionCandidateIndex;
			int num7 = xOffset + yOffset * craft_IconsPerRow;
			this._gridSelectionCandidateIndex = Utils.Clamp<int>(this._gridSelectionCandidateIndex + num7, 0, this._availableItemsCount - 1);
			if (gridSelectionCandidateIndex != this._gridSelectionCandidateIndex)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x004E320F File Offset: 0x004E140F
		public void ResetGridSelection()
		{
			this._gridSelectionCandidateIndex = 0;
		}

		// Token: 0x040012D4 RID: 4820
		private int _focusedElementIndex;

		// Token: 0x040012D5 RID: 4821
		private int _gridSelectionCandidateIndex;

		// Token: 0x040012D6 RID: 4822
		private float _currentElementOffset;

		// Token: 0x040012D7 RID: 4823
		private int _availableItemsCount;

		// Token: 0x040012D8 RID: 4824
		private int _gridStartingElementIndex;

		// Token: 0x040012DA RID: 4826
		private int _bannerStackRequestLimiter;

		// Token: 0x040012DB RID: 4827
		private BannerClaimingUI.Entry[] _squashedItemTypesToShow = new BannerClaimingUI.Entry[400];

		// Token: 0x020006FE RID: 1790
		private struct Entry
		{
			// Token: 0x04006824 RID: 26660
			public int Index;

			// Token: 0x04006825 RID: 26661
			public int Type;

			// Token: 0x04006826 RID: 26662
			public int Stack;
		}
	}
}
