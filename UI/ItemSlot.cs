using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Chat;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;

namespace Terraria.UI
{
	// Token: 0x020000FA RID: 250
	public class ItemSlot
	{
		// Token: 0x06001965 RID: 6501 RVA: 0x004E9F68 File Offset: 0x004E8168
		static ItemSlot()
		{
			Color[,] array = new Color[3, 3];
			array[0, 0] = new Color(50, 106, 64);
			array[0, 1] = new Color(46, 106, 98);
			array[0, 2] = new Color(45, 85, 105);
			array[1, 0] = new Color(35, 106, 126);
			array[1, 1] = new Color(50, 89, 140);
			array[1, 2] = new Color(57, 70, 128);
			array[2, 0] = new Color(122, 63, 83);
			array[2, 1] = new Color(104, 46, 85);
			array[2, 2] = new Color(84, 37, 87);
			ItemSlot.LoadoutSlotColors = array;
			ItemSlot.OverdrawGlowSize = 1f;
			ItemSlot.OverdrawGlowColorMultiplier = Color.White;
			ItemSlot._dirtyHack = new Item[0];
			ItemSlot.canFavoriteAt[0] = true;
			ItemSlot.canFavoriteAt[1] = true;
			ItemSlot.canFavoriteAt[2] = true;
			ItemSlot.canFavoriteAt[32] = true;
			ItemSlot.canShareAt[0] = true;
			ItemSlot.canShareAt[1] = true;
			ItemSlot.canShareAt[2] = true;
			ItemSlot.canShareAt[32] = true;
			ItemSlot.canShareAt[15] = true;
			ItemSlot.canShareAt[4] = true;
			ItemSlot.canShareAt[32] = true;
			ItemSlot.canShareAt[5] = true;
			ItemSlot.canShareAt[6] = true;
			ItemSlot.canShareAt[7] = true;
			ItemSlot.canShareAt[27] = true;
			ItemSlot.canShareAt[26] = true;
			ItemSlot.canShareAt[23] = true;
			ItemSlot.canShareAt[24] = true;
			ItemSlot.canShareAt[39] = true;
			ItemSlot.canShareAt[25] = true;
			ItemSlot.canShareAt[38] = true;
			ItemSlot.canShareAt[22] = true;
			ItemSlot.canShareAt[35] = true;
			ItemSlot.canShareAt[3] = true;
			ItemSlot.canShareAt[8] = true;
			ItemSlot.canShareAt[9] = true;
			ItemSlot.canShareAt[10] = true;
			ItemSlot.canShareAt[11] = true;
			ItemSlot.canShareAt[12] = true;
			ItemSlot.canShareAt[33] = true;
			ItemSlot.canShareAt[16] = true;
			ItemSlot.canShareAt[20] = true;
			ItemSlot.canShareAt[18] = true;
			ItemSlot.canShareAt[19] = true;
			ItemSlot.canShareAt[17] = true;
			ItemSlot.canShareAt[29] = true;
			ItemSlot.canShareAt[34] = true;
			ItemSlot.canShareAt[30] = true;
			ItemSlot.canShareAt[41] = true;
			ItemSlot.canShareAt[42] = true;
			ItemSlot.canShareAt[43] = true;
			ItemSlot.canQuickDropAt[0] = true;
			ItemSlot.canQuickDropAt[1] = true;
			ItemSlot.canQuickDropAt[2] = true;
			ItemSlot.canQuickDropAt[6] = true;
			ItemSlot.canQuickDropAt[15] = true;
			ItemSlot.canQuickDropAt[4] = true;
			ItemSlot.canQuickDropAt[32] = true;
			ItemSlot.canQuickDropAt[3] = true;
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06001966 RID: 6502 RVA: 0x004EA291 File Offset: 0x004E8491
		public static bool ShiftInUse
		{
			get
			{
				return Main.keyState.PressingShift() || ItemSlot.ShiftForcedOn;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x004EA2A6 File Offset: 0x004E84A6
		public static bool ControlInUse
		{
			get
			{
				return Main.keyState.PressingControl();
			}
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06001968 RID: 6504 RVA: 0x004EA2B4 File Offset: 0x004E84B4
		// (remove) Token: 0x06001969 RID: 6505 RVA: 0x004EA2E8 File Offset: 0x004E84E8
		public static event ItemSlot.ItemTransferEvent OnItemTransferred;

		// Token: 0x0600196A RID: 6506 RVA: 0x004EA31B File Offset: 0x004E851B
		public static void AnnounceTransfer(ItemSlot.ItemTransferInfo info)
		{
			if (ItemSlot.OnItemTransferred != null)
			{
				ItemSlot.OnItemTransferred(info);
			}
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x004EA330 File Offset: 0x004E8530
		public static void PrepareForChest(Chest chest)
		{
			int maxItems = chest.maxItems;
			if (ItemSlot.inventoryGlowTimeChest.Length < maxItems)
			{
				Array.Resize<int>(ref ItemSlot.inventoryGlowTimeChest, maxItems);
			}
			if (ItemSlot.inventoryGlowHueChest.Length < maxItems)
			{
				Array.Resize<float>(ref ItemSlot.inventoryGlowHueChest, maxItems);
			}
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x004EA370 File Offset: 0x004E8570
		public static void SetGlowForChest(Chest chest)
		{
			ItemSlot.PrepareForChest(chest);
			int maxItems = chest.maxItems;
			for (int i = 0; i < maxItems; i++)
			{
				ItemSlot.SetGlow(i, -1f, true);
			}
			for (int j = 0; j < maxItems; j++)
			{
				CoinSlot.ForceSlotState(j, 3, chest.item[j]);
			}
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x004EA3C0 File Offset: 0x004E85C0
		public static void SetGlow(int index, float hue, bool chest)
		{
			if (!chest)
			{
				ItemSlot.inventoryGlowTime[index] = 300;
				ItemSlot.inventoryGlowHue[index] = hue;
				return;
			}
			if (hue < 0f)
			{
				ItemSlot.inventoryGlowTimeChest[index] = 0;
				ItemSlot.inventoryGlowHueChest[index] = 0f;
				return;
			}
			ItemSlot.inventoryGlowTimeChest[index] = 300;
			ItemSlot.inventoryGlowHueChest[index] = hue;
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x004EA418 File Offset: 0x004E8618
		public static void UpdateInterface()
		{
			if (!Main.playerInventory || Main.player[Main.myPlayer].talkNPC == -1)
			{
				ItemSlot._customCurrencyForSavings = -1;
			}
			for (int i = 0; i < ItemSlot.inventoryGlowTime.Length; i++)
			{
				if (ItemSlot.inventoryGlowTime[i] > 0)
				{
					ItemSlot.inventoryGlowTime[i]--;
					if (ItemSlot.inventoryGlowTime[i] == 0)
					{
						ItemSlot.inventoryGlowHue[i] = 0f;
					}
				}
			}
			for (int j = 0; j < ItemSlot.inventoryGlowTimeChest.Length; j++)
			{
				if (ItemSlot.inventoryGlowTimeChest[j] > 0)
				{
					ItemSlot.inventoryGlowTimeChest[j]--;
					if (ItemSlot.inventoryGlowTimeChest[j] == 0 || ItemSlot.forceClearGlowsOnChest)
					{
						ItemSlot.inventoryGlowHueChest[j] = 0f;
					}
				}
			}
			ItemSlot.forceClearGlowsOnChest = false;
			for (int k = 0; k < ItemSlot.playerSlotPulseEffects.Length; k++)
			{
				ItemSlot.PulseEffect pulseEffect = ItemSlot.playerSlotPulseEffects[k];
				if (pulseEffect.itemInSlot != null)
				{
					ItemSlot.PulseEffect[] array = ItemSlot.playerSlotPulseEffects;
					int num = k;
					int num2 = array[num].time + 1;
					array[num].time = num2;
					if (num2 >= ItemSlot.PulseEffect.EffectDuration || pulseEffect.slotRef.Item.IsAir)
					{
						ItemSlot.playerSlotPulseEffects[k] = default(ItemSlot.PulseEffect);
					}
				}
			}
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x004EA543 File Offset: 0x004E8743
		public static void IndicateBlockedSlot(PlayerItemSlotID.SlotReference slot)
		{
			ItemSlot.AddPulseEffect(slot, new Color(250, 40, 40, 255));
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x004EA560 File Offset: 0x004E8760
		public static void AddPulseEffect(PlayerItemSlotID.SlotReference slot, Color color)
		{
			ItemSlot.PulseEffect pulseEffect = new ItemSlot.PulseEffect(slot, color);
			if (pulseEffect.itemInSlot.IsAir)
			{
				return;
			}
			ItemSlot.playerSlotPulseEffects[slot.SlotId] = pulseEffect;
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x004EA595 File Offset: 0x004E8795
		public static void Handle(ref Item inv, int context = 0, bool allowInteract = true)
		{
			ItemSlot.singleSlotArray[0] = inv;
			ItemSlot.Handle(ItemSlot.singleSlotArray, context, 0, allowInteract);
			inv = ItemSlot.singleSlotArray[0];
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x004EA5B6 File Offset: 0x004E87B6
		public static bool HoverOverrideClick(Item inv, int context = 0)
		{
			ItemSlot.singleSlotArray[0] = inv;
			ItemSlot.OverrideHover(ItemSlot.singleSlotArray, context, 0);
			if (Main.cursorOverride >= 0 && Main.mouseLeftRelease && Main.mouseLeft)
			{
				ItemSlot.OverrideLeftClick(ItemSlot.singleSlotArray, context, 0);
				return true;
			}
			return false;
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x004EA5F2 File Offset: 0x004E87F2
		public static void Handle(Item[] inv, int context = 0, int slot = 0, bool allowInteract = true)
		{
			ItemSlot.OverrideHover(inv, context, slot);
			if (allowInteract)
			{
				ItemSlot.LeftClick(inv, context, slot);
				ItemSlot.RightClick(inv, context, slot);
			}
			ItemSlot.MouseHover(inv, context, slot);
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x004EA618 File Offset: 0x004E8818
		public static void OverrideHover(Item[] inv, int context = 0, int slot = 0)
		{
			if (!PlayerInput.UsingGamepad)
			{
				UILinkPointNavigator.SuggestUsage(ItemSlot.GetGamepadPointForSlot(inv, context, slot));
			}
			if (inv[slot].IsAir)
			{
				return;
			}
			if (Main.keyState.IsKeyDown(Main.FavoriteKey))
			{
				if (Main.drawingPlayerChat && ItemSlot.canShareAt[context])
				{
					Main.cursorOverride = 2;
					return;
				}
				if (ItemSlot.canFavoriteAt[context])
				{
					Main.cursorOverride = 3;
					return;
				}
			}
			ItemSlot.AlternateClickAction? alternateClickAction = ItemSlot.GetAlternateClickAction(inv, context, slot);
			if (alternateClickAction != null)
			{
				Main.cursorOverride = alternateClickAction.Value.cursorOverride;
			}
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x004EA6A0 File Offset: 0x004E88A0
		public static ItemSlot.AlternateClickAction? GetAlternateClickAction(Item[] inv, int context, int slot)
		{
			Item item = inv[slot];
			if (item.IsAir || item.favorited)
			{
				return null;
			}
			bool flag = ItemSlot.Options.DisableLeftShiftTrashCan && !PlayerInput.UsingGamepad;
			if (ItemSlot.ControlInUse && !ItemSlot.Options.DisableQuickTrash && flag)
			{
				if (context <= 4 || context == 7 || context == 32)
				{
					return ItemSlot.AlternateClickAction.GetSellOrTrash(item);
				}
			}
			else if (ItemSlot.ShiftInUse)
			{
				if (Main.LocalPlayer.tileEntityAnchor.IsInValidUseTileEntity())
				{
					ItemSlot.AlternateClickAction? shiftClickAction = Main.LocalPlayer.tileEntityAnchor.GetTileEntity().GetShiftClickAction(inv, context, slot);
					if (shiftClickAction != null)
					{
						return shiftClickAction;
					}
				}
				switch (context)
				{
				case 0:
				case 1:
				case 2:
					if (Main.CreativeMenu.IsShowingResearchMenu())
					{
						if (context == 0)
						{
							return new ItemSlot.AlternateClickAction?(ItemSlot.AlternateClickAction.TransferToChest);
						}
					}
					else if (Main.InReforgeMenu)
					{
						if (context == 0 && item.CanHavePrefixes())
						{
							return new ItemSlot.AlternateClickAction?(ItemSlot.AlternateClickAction.TransferToChest);
						}
					}
					else if (Main.InGuideCraftMenu)
					{
						if (context == 0 && item.material)
						{
							return new ItemSlot.AlternateClickAction?(ItemSlot.AlternateClickAction.TransferToChest);
						}
					}
					else if (Main.player[Main.myPlayer].chest != -1)
					{
						if (ChestUI.TryPlacingInChest(inv, slot, true, context))
						{
							return new ItemSlot.AlternateClickAction?(ItemSlot.AlternateClickAction.TransferToChest);
						}
					}
					else if (!ItemSlot.Options.DisableQuickTrash && !flag)
					{
						return ItemSlot.AlternateClickAction.GetSellOrTrash(item);
					}
					break;
				case 3:
				case 4:
				case 32:
					if (Main.player[Main.myPlayer].ItemSpace(item).CanTakeItemToPersonalInventory)
					{
						return new ItemSlot.AlternateClickAction?(ItemSlot.AlternateClickAction.TransferFromChest);
					}
					break;
				case 5:
				case 6:
				case 7:
				case 29:
					if (Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
					{
						return new ItemSlot.AlternateClickAction?(ItemSlot.AlternateClickAction.TransferToBackpack);
					}
					break;
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 33:
					if (Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
					{
						return new ItemSlot.AlternateClickAction?(ItemSlot.AlternateClickAction.Unequip);
					}
					break;
				}
			}
			return null;
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x004EA904 File Offset: 0x004E8B04
		private static bool OverrideLeftClick(Item[] inv, int context = 0, int slot = 0)
		{
			if (Main.LocalPlayer.tileEntityAnchor.IsInValidUseTileEntity() && ItemSlot.ShiftInUse && Main.LocalPlayer.tileEntityAnchor.GetTileEntity().PerformShiftClickAction(inv, context, slot))
			{
				return true;
			}
			Item item = inv[slot];
			if (Main.cursorOverride == 2)
			{
				if (ChatManager.AddChatText(FontAssets.MouseText.Value, ItemTagHandler.GenerateTag(item), Vector2.One))
				{
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				}
				return true;
			}
			if (Main.cursorOverride == 3)
			{
				if (!ItemSlot.canFavoriteAt[context])
				{
					return false;
				}
				item.favorited = !item.favorited;
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				return true;
			}
			else
			{
				if (Main.cursorOverride == 6)
				{
					Item item2 = item.DeepClone();
					if (!(ItemSlot.TryResearchingItem(ref item2, true) | ItemSlot.TryResearchingItem(ref Main.LocalPlayer.trashItem, false)))
					{
						SoundEngine.PlaySound(SoundID.TrashItem, -1, -1, 0f, 1f);
					}
					SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
					Main.LocalPlayer.trashItem = item.Clone();
					ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(Main.LocalPlayer.trashItem, context, 6, 0));
					item.TurnToAir(false);
					if (context == 3 && Main.netMode == 1)
					{
						NetMessage.SendData(32, -1, -1, null, Main.LocalPlayer.chest, (float)slot, 0f, 0f, 0, 0, 0);
					}
					CoinSlot.ForceSlotState(slot, context, inv[slot]);
					return true;
				}
				if (Main.cursorOverride == 7)
				{
					if (context == 29)
					{
						Item item3 = new Item();
						item3.SetDefaults(inv[slot].type, null);
						item3.stack = (item3.OnlyNeedOneInInventory() ? 1 : item3.maxStack);
						item3.OnCreated(new JourneyDuplicationItemCreationContext());
						Player.GetItemLogger.Start();
						item3 = Main.LocalPlayer.GetItem(item3, GetItemSettings.QuickTransferFromSlot);
						Player.GetItemLogger.Stop();
						ItemSlot.DisplayTransfer_GetItem(inv, context, slot);
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						return true;
					}
					Player.GetItemLogger.Start();
					inv[slot] = Main.LocalPlayer.GetItem(inv[slot], GetItemSettings.QuickTransferFromSlot);
					Player.GetItemLogger.Stop();
					ItemSlot.DisplayTransfer_GetItem(inv, context, slot);
					CoinSlot.ForceSlotState(slot, context, inv[slot]);
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					return true;
				}
				else
				{
					if (Main.cursorOverride == 8)
					{
						Player.GetItemLogger.Start();
						inv[slot] = Main.LocalPlayer.GetItem(inv[slot], GetItemSettings.QuickTransferFromSlot);
						Player.GetItemLogger.Stop();
						ItemSlot.DisplayTransfer_GetItem(inv, context, slot);
						if (Main.player[Main.myPlayer].chest > -1)
						{
							NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)slot, 0f, 0f, 0, 0, 0);
						}
						CoinSlot.ForceSlotState(slot, context, inv[slot]);
						return true;
					}
					if (Main.cursorOverride == 9)
					{
						if (Main.CreativeMenu.IsShowingResearchMenu())
						{
							Main.CreativeMenu.SwapItem(ref inv[slot]);
							SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
							Main.CreativeMenu.SacrificeItemInSacrificeSlot();
						}
						else if (Main.InReforgeMenu)
						{
							if (item.stack <= 1)
							{
								Utils.Swap<Item>(ref inv[slot], ref Main.reforgeItem);
								ItemSlot.DisplayTransfer_TwoWay(inv, slot, context, Main.reforgeItem, 5);
								SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
							}
							else if (Main.reforgeItem.IsAir)
							{
								Main.reforgeItem = item.Clone();
								Main.reforgeItem.stack = 1;
								item.stack--;
								ItemSlot.DisplayTransfer_TwoWay(inv, slot, context, Main.reforgeItem, 5);
								SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
							}
						}
						else if (Main.InGuideCraftMenu)
						{
							Utils.Swap<Item>(ref inv[slot], ref Main.guideItem);
							ItemSlot.DisplayTransfer_TwoWay(inv, slot, context, Main.guideItem, 7);
							SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
						}
						else
						{
							ChestUI.TryPlacingInChest(inv, slot, false, context);
						}
						return true;
					}
					if (Main.cursorOverride == 10)
					{
						Chest chest = Main.instance.shop[Main.npcShop];
						if (Main.LocalPlayer.SellItem(item, -1))
						{
							chest.AddItemToShop(item);
							ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(item, context, 15, 0));
							item.TurnToAir(false);
							SoundEngine.PlaySound(18, -1, -1, 1, 1f, 0f);
						}
						else if (item.value == 0)
						{
							chest.AddItemToShop(item);
							ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(item, context, 15, 0));
							item.TurnToAir(false);
							SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
						}
						return true;
					}
					return false;
				}
			}
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x004EADAC File Offset: 0x004E8FAC
		public static void LeftClick(Item[] inv, int context = 0, int slot = 0)
		{
			if (Main.LocalPlayerHasPendingInventoryActions())
			{
				return;
			}
			Player player = Main.player[Main.myPlayer];
			inv[slot].newAndShiny = false;
			bool flag = Main.mouseLeftRelease && Main.mouseLeft;
			if (flag)
			{
				if (ItemSlot.OverrideLeftClick(inv, context, slot))
				{
					return;
				}
				if (player.itemAnimation != 0 || player.itemTime != 0)
				{
					return;
				}
			}
			int num = ItemSlot.PickItemMovementAction(inv, context, slot, Main.mouseItem);
			if (num != 3 && !flag)
			{
				return;
			}
			if (num == 0)
			{
				bool flag2 = false;
				if (context == 6 && Main.mouseItem.type != 0)
				{
					Item item = Main.mouseItem.DeepClone();
					if (ItemSlot.TryResearchingItem(ref item, true) | ItemSlot.TryResearchingItem(inv, slot, false))
					{
						flag2 = true;
					}
					inv[slot].SetDefaults(0, null);
				}
				Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
				if (inv[slot].stack > 0)
				{
					ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(inv[slot], 21, context, inv[slot].stack));
				}
				else
				{
					ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(Main.mouseItem, context, 21, Main.mouseItem.stack));
				}
				if (inv[slot].stack > 0)
				{
					if (context <= 17)
					{
						if (context == 0)
						{
							AchievementsHelper.NotifyItemPickup(player, inv[slot]);
							goto IL_13F;
						}
						if (context - 8 > 4 && context - 16 > 1)
						{
							goto IL_13F;
						}
					}
					else if (context != 25 && context != 27 && context != 33)
					{
						goto IL_13F;
					}
					AchievementsHelper.HandleOnEquip(player, inv[slot], context);
				}
				IL_13F:
				if (inv[slot].type == 0 || inv[slot].stack < 1)
				{
					inv[slot] = new Item();
				}
				if (Item.CanStack(Main.mouseItem, inv[slot]))
				{
					Utils.Swap<bool>(ref inv[slot].favorited, ref Main.mouseItem.favorited);
					if (inv[slot].stack != inv[slot].maxStack && Main.mouseItem.stack != Main.mouseItem.maxStack)
					{
						if (Main.mouseItem.stack + inv[slot].stack <= Main.mouseItem.maxStack)
						{
							inv[slot].stack += Main.mouseItem.stack;
							Main.mouseItem.stack = 0;
							ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(inv[slot], 21, context, inv[slot].stack));
						}
						else
						{
							int num2 = Main.mouseItem.maxStack - inv[slot].stack;
							inv[slot].stack += num2;
							Main.mouseItem.stack -= num2;
							ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(inv[slot], 21, context, num2));
						}
					}
				}
				if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
				{
					Main.mouseItem = new Item();
				}
				if (Main.mouseItem.type > 0 || inv[slot].type > 0)
				{
					if (context == 6 && Main.mouseItem.type == 0 && !flag2)
					{
						SoundEngine.PlaySound(SoundID.TrashItem, -1, -1, 0f, 1f);
					}
					SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
				}
				if (context == 3 && Main.netMode == 1)
				{
					NetMessage.SendData(32, -1, -1, null, player.chest, (float)slot, 0f, 0f, 0, 0, 0);
				}
				CoinSlot.ForceSlotState(slot, context, inv[slot]);
			}
			else if (num == 1)
			{
				if (Main.mouseItem.stack == 1 && Main.mouseItem.type > 0 && inv[slot].type > 0 && inv[slot].IsNotTheSameAs(Main.mouseItem))
				{
					Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
					SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
					if (inv[slot].stack > 0)
					{
						if (context <= 12)
						{
							if (context == 0)
							{
								AchievementsHelper.NotifyItemPickup(player, inv[slot]);
								goto IL_595;
							}
							if (context - 8 > 4)
							{
								goto IL_595;
							}
						}
						else if (context - 16 > 1 && context != 33)
						{
							goto IL_595;
						}
						AchievementsHelper.HandleOnEquip(player, inv[slot], context);
					}
				}
				else if (Main.mouseItem.type == 0 && inv[slot].type > 0)
				{
					Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
					if (inv[slot].type == 0 || inv[slot].stack < 1)
					{
						inv[slot] = new Item();
					}
					if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
					{
						Main.mouseItem = new Item();
					}
					if (Main.mouseItem.type > 0 || inv[slot].type > 0)
					{
						SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
					}
				}
				else if (Main.mouseItem.type > 0 && inv[slot].type == 0)
				{
					if (Main.mouseItem.stack == 1)
					{
						Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
						if (inv[slot].type == 0 || inv[slot].stack < 1)
						{
							inv[slot] = new Item();
						}
						if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
						{
							Main.mouseItem = new Item();
						}
						if (Main.mouseItem.type > 0 || inv[slot].type > 0)
						{
							SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
						}
					}
					else
					{
						inv[slot] = Main.mouseItem.Clone();
						Main.mouseItem.stack--;
						inv[slot].stack = 1;
						SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
					}
					if (inv[slot].stack > 0)
					{
						if (context <= 17)
						{
							if (context == 0)
							{
								AchievementsHelper.NotifyItemPickup(player, inv[slot]);
								goto IL_595;
							}
							if (context - 8 > 4 && context - 16 > 1)
							{
								goto IL_595;
							}
						}
						else if (context != 25 && context != 27 && context != 33)
						{
							goto IL_595;
						}
						AchievementsHelper.HandleOnEquip(player, inv[slot], context);
					}
				}
				IL_595:
				if ((context == 23 || context == 24 || context == 39) && Main.netMode == 1)
				{
					NetMessage.SendData(121, -1, -1, null, Main.myPlayer, (float)player.tileEntityAnchor.interactEntityID, (float)slot, 0f, 0, 0, 0);
				}
				if (context == 38 && Main.netMode == 1)
				{
					NetMessage.SendData(121, -1, -1, null, Main.myPlayer, (float)player.tileEntityAnchor.interactEntityID, (float)slot, 3f, 0, 0, 0);
				}
				if (context == 26 && Main.netMode == 1)
				{
					NetMessage.SendData(124, -1, -1, null, Main.myPlayer, (float)player.tileEntityAnchor.interactEntityID, (float)slot, 0f, 0, 0, 0);
				}
				CoinSlot.ForceSlotState(slot, context, inv[slot]);
			}
			else if (num == 2)
			{
				if (Main.mouseItem.stack == 1 && Main.mouseItem.dye > 0 && inv[slot].type > 0 && inv[slot].type != Main.mouseItem.type)
				{
					Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
					SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
					if (inv[slot].stack > 0)
					{
						if (context <= 17)
						{
							if (context == 0)
							{
								AchievementsHelper.NotifyItemPickup(player, inv[slot]);
								goto IL_8D7;
							}
							if (context - 8 > 4 && context - 16 > 1)
							{
								goto IL_8D7;
							}
						}
						else if (context != 25 && context != 27 && context != 33)
						{
							goto IL_8D7;
						}
						AchievementsHelper.HandleOnEquip(player, inv[slot], context);
					}
				}
				else if (Main.mouseItem.type == 0 && inv[slot].type > 0)
				{
					Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
					if (inv[slot].type == 0 || inv[slot].stack < 1)
					{
						inv[slot] = new Item();
					}
					if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
					{
						Main.mouseItem = new Item();
					}
					if (Main.mouseItem.type > 0 || inv[slot].type > 0)
					{
						SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
					}
				}
				else if (Main.mouseItem.dye > 0 && inv[slot].type == 0)
				{
					if (Main.mouseItem.stack == 1)
					{
						Utils.Swap<Item>(ref inv[slot], ref Main.mouseItem);
						if (inv[slot].type == 0 || inv[slot].stack < 1)
						{
							inv[slot] = new Item();
						}
						if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
						{
							Main.mouseItem = new Item();
						}
						if (Main.mouseItem.type > 0 || inv[slot].type > 0)
						{
							SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
						}
					}
					else
					{
						Main.mouseItem.stack--;
						inv[slot].SetDefaults(Main.mouseItem.type, null);
						SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
					}
					if (inv[slot].stack > 0)
					{
						if (context <= 17)
						{
							if (context == 0)
							{
								AchievementsHelper.NotifyItemPickup(player, inv[slot]);
								goto IL_8D7;
							}
							if (context - 8 > 4 && context - 16 > 1)
							{
								goto IL_8D7;
							}
						}
						else if (context != 25 && context != 27 && context != 33)
						{
							goto IL_8D7;
						}
						AchievementsHelper.HandleOnEquip(player, inv[slot], context);
					}
				}
				IL_8D7:
				if (context == 25 && Main.netMode == 1)
				{
					NetMessage.SendData(121, -1, -1, null, Main.myPlayer, (float)player.tileEntityAnchor.interactEntityID, (float)slot, 1f, 0, 0, 0);
				}
				if (context == 27 && Main.netMode == 1)
				{
					NetMessage.SendData(124, -1, -1, null, Main.myPlayer, (float)player.tileEntityAnchor.interactEntityID, (float)slot, 1f, 0, 0, 0);
				}
			}
			else if (num == 3)
			{
				ItemSlot.HandleShopSlot(inv, slot, false, true);
			}
			else if (num == 4)
			{
				Chest chest = Main.instance.shop[Main.npcShop];
				if (player.SellItem(Main.mouseItem, -1))
				{
					chest.AddItemToShop(Main.mouseItem);
					Main.mouseItem.SetDefaults(0, null);
					SoundEngine.PlaySound(18, -1, -1, 1, 1f, 0f);
					ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(inv[slot], 21, 15, 0));
				}
				else if (Main.mouseItem.value == 0)
				{
					chest.AddItemToShop(Main.mouseItem);
					Main.mouseItem.SetDefaults(0, null);
					SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
					ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(inv[slot], 21, 15, 0));
				}
				Main.stackSplit = 9999;
			}
			else if (num == 5 && Main.mouseItem.IsAir)
			{
				SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
				Main.mouseItem.SetDefaults(inv[slot].type, null);
				Main.mouseItem.stack = (Main.mouseItem.OnlyNeedOneInInventory() ? 1 : Main.mouseItem.maxStack);
				Main.mouseItem.OnCreated(new JourneyDuplicationItemCreationContext());
				ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(inv[slot], 29, 21, 0));
			}
			if (context > 2 && context != 5 && context != 32)
			{
				inv[slot].favorited = false;
			}
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x004EB860 File Offset: 0x004E9A60
		private static bool TryResearchingItem(Item[] inv, int slot, bool onlySacrificeIfItWouldFinishResearch = false)
		{
			return ItemSlot.TryResearchingItem(ref inv[slot], onlySacrificeIfItWouldFinishResearch);
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x004EB870 File Offset: 0x004E9A70
		private static bool TryResearchingItem(ref Item item, bool onlySacrificeIfItWouldFinishResearch = false)
		{
			if (!Main.IsJourneyMode)
			{
				return false;
			}
			if (item == null || item.IsAir)
			{
				return false;
			}
			int num;
			CreativeUI.ItemSacrificeResult itemSacrificeResult = Main.CreativeMenu.SacrificeItem(ref item, out num, false, onlySacrificeIfItWouldFinishResearch);
			if (itemSacrificeResult == CreativeUI.ItemSacrificeResult.CannotSacrifice)
			{
				return false;
			}
			if (itemSacrificeResult == CreativeUI.ItemSacrificeResult.SacrificedAndDone)
			{
				SoundEngine.PlaySound(64, -1, -1, 1, 1f, 0f);
			}
			else
			{
				SoundEngine.PlaySound(63, -1, -1, 1, 1f, 0f);
			}
			return true;
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x004EB8DC File Offset: 0x004E9ADC
		public static bool ShouldHighlightSlotForMouseItem(int context, int slot, Item checkItem)
		{
			bool result = false;
			if (!checkItem.IsAir)
			{
				if (context == 8)
				{
					result = (!ItemSlot.HasSameItemInSlot(checkItem, new ArraySegment<Item>(Main.LocalPlayer.armor, 10, 3)) && Main.LocalPlayer.armor[slot].type != checkItem.type && !checkItem.vanity && ((checkItem.headSlot > -1 && slot == 0) || (checkItem.bodySlot > -1 && slot == 1) || (checkItem.legSlot > -1 && slot == 2)));
				}
				else if (context == 23)
				{
					result = ((checkItem.headSlot > -1 && slot == 0) || (checkItem.bodySlot > -1 && slot == 1) || (checkItem.legSlot > -1 && slot == 2));
				}
				else if (context == 26)
				{
					result = (checkItem.headSlot > -1);
				}
				else if (context == 9)
				{
					result = (!ItemSlot.HasSameItemInSlot(checkItem, new ArraySegment<Item>(Main.LocalPlayer.armor, 0, 3)) && Main.LocalPlayer.armor[slot].type != checkItem.type && checkItem.vanity && ((checkItem.headSlot > -1 && slot == 10) || (checkItem.vanity && checkItem.bodySlot > -1 && slot == 11) || (checkItem.vanity && checkItem.legSlot > -1 && slot == 12)));
				}
				else if (context == 12)
				{
					result = (Main.LocalPlayer.IsItemSlotUnlockedAndUsable(slot) && checkItem.dye > 0);
				}
				else if (context == 33 || context == 25 || context == 27)
				{
					result = (checkItem.dye > 0);
				}
				else if (context == 16)
				{
					result = (checkItem.mountType == -1 && Main.projHook[checkItem.shoot]);
				}
				else if (context == 17)
				{
					result = (checkItem.mountType != -1 && !MountID.Sets.Cart[checkItem.mountType]);
				}
				else if (context == 39)
				{
					result = (checkItem.mountType != -1);
				}
				else if (context == 19)
				{
					result = (checkItem.buffType > 0 && Main.vanityPet[checkItem.buffType] && !Main.lightPet[checkItem.buffType]);
				}
				else if (context == 18)
				{
					result = (checkItem.mountType != -1 && MountID.Sets.Cart[checkItem.mountType]);
				}
				else if (context == 20)
				{
					result = (checkItem.buffType > 0 && Main.lightPet[checkItem.buffType]);
				}
				else if (context == 10)
				{
					result = (Main.LocalPlayer.armor[slot].type != checkItem.type && !checkItem.vanity && checkItem.accessory && Main.LocalPlayer.IsItemSlotUnlockedAndUsable(slot) && ItemSlot.CanEquipAccessoryInSlot(checkItem, slot));
				}
				else if (context == 24)
				{
					TEDisplayDoll tedisplayDoll = Main.LocalPlayer.tileEntityAnchor.GetTileEntity() as TEDisplayDoll;
					result = (checkItem.accessory && ItemSlot.CanEquipAccessoryInSlot(checkItem, new ArraySegment<Item>(tedisplayDoll.Equipment, 3, 5), slot));
				}
				else if (context == 38)
				{
					result = TEDisplayDoll.AcceptedInWeaponSlot(checkItem);
				}
				else if (context == 11)
				{
					result = (Main.LocalPlayer.armor[slot].type != checkItem.type && checkItem.vanity && checkItem.accessory && Main.LocalPlayer.IsItemSlotUnlockedAndUsable(slot) && ItemSlot.CanEquipAccessoryInVanitySlot(checkItem, slot));
				}
			}
			return result;
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x004EBC3C File Offset: 0x004E9E3C
		public static void GetDimSlotForMouseItem(int context, int slot, Item checkItem, out float itemFade)
		{
			bool flag = false;
			itemFade = 1f;
			if (!checkItem.IsAir)
			{
				if (context == 8)
				{
					flag = ((!ItemID.Sets.DualEquipArmor[checkItem.type] && ItemSlot.HasSameItemInSlot(checkItem, new ArraySegment<Item>(Main.LocalPlayer.armor, 10, 3))) || ((checkItem.headSlot <= -1 || slot != 0) && (checkItem.bodySlot <= -1 || slot != 1) && (checkItem.legSlot <= -1 || slot != 2)));
				}
				else if (context == 23)
				{
					flag = ((checkItem.headSlot <= -1 || slot != 0) && (checkItem.bodySlot <= -1 || slot != 1) && (checkItem.legSlot <= -1 || slot != 2));
				}
				else if (context == 26)
				{
					flag = (checkItem.headSlot <= -1);
				}
				else if (context == 9)
				{
					flag = ((!ItemID.Sets.DualEquipArmor[checkItem.type] && ItemSlot.HasSameItemInSlot(checkItem, new ArraySegment<Item>(Main.LocalPlayer.armor, 0, 3))) || ((checkItem.headSlot <= -1 || slot != 10) && (checkItem.bodySlot <= -1 || slot != 11) && (checkItem.legSlot <= -1 || slot != 12)));
				}
				else if (context == 12)
				{
					flag = (!Main.LocalPlayer.IsItemSlotUnlockedAndUsable(slot) || checkItem.dye <= 0);
				}
				else if (context == 33 || context == 25 || context == 27)
				{
					flag = (checkItem.dye <= 0);
				}
				else if (context == 16)
				{
					flag = (checkItem.mountType != -1 || !Main.projHook[checkItem.shoot]);
				}
				else if (context == 17)
				{
					flag = (checkItem.mountType == -1 || MountID.Sets.Cart[checkItem.mountType]);
				}
				else if (context == 39)
				{
					flag = (checkItem.mountType == -1 || MountID.Sets.Cart[checkItem.mountType]);
				}
				else if (context == 19)
				{
					flag = (checkItem.buffType <= 0 || !Main.vanityPet[checkItem.buffType] || Main.lightPet[checkItem.buffType]);
				}
				else if (context == 18)
				{
					flag = (checkItem.mountType == -1 || !MountID.Sets.Cart[checkItem.mountType]);
				}
				else if (context == 20)
				{
					flag = (checkItem.buffType <= 0 || !Main.lightPet[checkItem.buffType]);
				}
				else if (context == 10)
				{
					flag = (!checkItem.accessory || !Main.LocalPlayer.IsItemSlotUnlockedAndUsable(slot) || !ItemSlot.CanEquipAccessoryInSlot(checkItem, slot));
				}
				else if (context == 24)
				{
					TEDisplayDoll tedisplayDoll = Main.LocalPlayer.tileEntityAnchor.GetTileEntity() as TEDisplayDoll;
					flag = (!checkItem.accessory || !ItemSlot.CanEquipAccessoryInSlot(checkItem, new ArraySegment<Item>(tedisplayDoll.Equipment, 3, 5), slot));
				}
				else if (context == 38)
				{
					flag = !TEDisplayDoll.AcceptedInWeaponSlot(checkItem);
				}
				else if (context == 11)
				{
					flag = (!checkItem.accessory || !Main.LocalPlayer.IsItemSlotUnlockedAndUsable(slot) || !ItemSlot.CanEquipAccessoryInVanitySlot(checkItem, slot));
				}
			}
			if (flag)
			{
				itemFade = 0.5f;
			}
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x004EBF5C File Offset: 0x004EA15C
		public static bool CanEquipAccessoryInSlot(Item checkItem, int slot)
		{
			return checkItem.accessory && ItemSlot.CanEquipAccessoryInSlot(checkItem, new ArraySegment<Item>(Main.LocalPlayer.armor, 3, 7), slot) && !ItemSlot.HasSameItemInSlot(checkItem, new ArraySegment<Item>(Main.LocalPlayer.armor, 13, 7));
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x004EBFA8 File Offset: 0x004EA1A8
		public static bool CanEquipAccessoryInVanitySlot(Item checkItem, int slot)
		{
			return checkItem.accessory && ItemSlot.CanEquipAccessoryInSlot(checkItem, new ArraySegment<Item>(Main.LocalPlayer.armor, 13, 7), slot) && !ItemSlot.HasSameItemInSlot(checkItem, new ArraySegment<Item>(Main.LocalPlayer.armor, 3, 7));
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x004EBFF4 File Offset: 0x004EA1F4
		public static int PickItemMovementAction(Item[] inv, int context, int slot, Item checkItem)
		{
			Player player = Main.player[Main.myPlayer];
			int result = -1;
			if (context == 0)
			{
				result = 0;
			}
			else if (context == 1)
			{
				if (checkItem.type == 0 || checkItem.type == 71 || checkItem.type == 72 || checkItem.type == 73 || checkItem.type == 74)
				{
					result = 0;
				}
			}
			else if (context == 2)
			{
				if (checkItem.FitsAmmoSlot())
				{
					result = 0;
				}
			}
			else if (context == 3)
			{
				result = 0;
			}
			else if (context == 4 || context == 32)
			{
				Item[] item = Main.LocalPlayer.GetCurrentContainer().item;
				if (!ChestUI.IsBlockedFromTransferIntoChest(checkItem, item))
				{
					result = 0;
				}
			}
			else if (context == 5)
			{
				if (checkItem.CanHavePrefixes() || checkItem.type == 0)
				{
					result = 1;
				}
			}
			else if (context == 6)
			{
				result = 0;
			}
			else if (context == 7)
			{
				if (checkItem.material || checkItem.type == 0)
				{
					result = 0;
				}
			}
			else if (context == 8)
			{
				if ((ItemID.Sets.DualEquipArmor[checkItem.type] || !ItemSlot.HasSameItemInSlot(checkItem, new ArraySegment<Item>(Main.LocalPlayer.armor, 10, 3))) && Main.LocalPlayer.armor[slot].type != checkItem.type && (checkItem.type == 0 || (checkItem.headSlot > -1 && slot == 0) || (checkItem.bodySlot > -1 && slot == 1) || (checkItem.legSlot > -1 && slot == 2)))
				{
					result = 1;
				}
			}
			else if (context == 23)
			{
				if (checkItem.type == 0 || (checkItem.headSlot > 0 && slot == 0) || (checkItem.bodySlot > 0 && slot == 1) || (checkItem.legSlot > 0 && slot == 2))
				{
					result = 1;
				}
			}
			else if (context == 26)
			{
				if (checkItem.type == 0 || checkItem.headSlot > 0)
				{
					result = 1;
				}
			}
			else if (context == 9)
			{
				if ((ItemID.Sets.DualEquipArmor[checkItem.type] || !ItemSlot.HasSameItemInSlot(checkItem, new ArraySegment<Item>(Main.LocalPlayer.armor, 0, 3))) && Main.LocalPlayer.armor[slot].type != checkItem.type && (checkItem.type == 0 || (checkItem.headSlot > -1 && slot == 10) || (checkItem.bodySlot > -1 && slot == 11) || (checkItem.legSlot > -1 && slot == 12)))
				{
					result = 1;
				}
			}
			else if (context == 10)
			{
				if (checkItem.type == 0 || ItemSlot.CanEquipAccessoryInSlot(checkItem, slot))
				{
					result = 1;
				}
			}
			else if (context == 24)
			{
				if (checkItem.type == 0 || (checkItem.accessory && ItemSlot.CanEquipAccessoryInSlot(checkItem, new ArraySegment<Item>(inv, 3, 5), slot)))
				{
					result = 1;
				}
			}
			else if (context == 11)
			{
				if (checkItem.type == 0 || ItemSlot.CanEquipAccessoryInVanitySlot(checkItem, slot))
				{
					result = 1;
				}
			}
			else if (context == 12 || context == 25 || context == 27 || context == 33)
			{
				result = 2;
			}
			else if (context == 15)
			{
				if (checkItem.type == 0 && inv[slot].type > 0)
				{
					result = 3;
				}
				else if (checkItem.type == inv[slot].type && checkItem.type > 0 && checkItem.stack < checkItem.maxStack && inv[slot].stack > 0)
				{
					result = 3;
				}
				else if (inv[slot].type == 0 && checkItem.type > 0 && (checkItem.type < 71 || checkItem.type > 74))
				{
					result = 4;
				}
			}
			else if (context == 16)
			{
				if (checkItem.type == 0 || Main.projHook[checkItem.shoot])
				{
					result = 1;
				}
			}
			else if (context == 17)
			{
				if (checkItem.type == 0 || (checkItem.mountType != -1 && !MountID.Sets.Cart[checkItem.mountType]))
				{
					result = 1;
				}
			}
			else if (context == 39)
			{
				if (checkItem.type == 0 || checkItem.mountType != -1)
				{
					result = 1;
				}
			}
			else if (context == 38)
			{
				if (checkItem.type == 0 || TEDisplayDoll.AcceptedInWeaponSlot(checkItem))
				{
					result = 1;
				}
			}
			else if (context == 19)
			{
				if (checkItem.type == 0 || (checkItem.buffType > 0 && Main.vanityPet[checkItem.buffType] && !Main.lightPet[checkItem.buffType]))
				{
					result = 1;
				}
			}
			else if (context == 18)
			{
				if (checkItem.type == 0 || (checkItem.mountType != -1 && MountID.Sets.Cart[checkItem.mountType]))
				{
					result = 1;
				}
			}
			else if (context == 20)
			{
				if (checkItem.type == 0 || (checkItem.buffType > 0 && Main.lightPet[checkItem.buffType]))
				{
					result = 1;
				}
			}
			else if (context == 29 && checkItem.type == 0 && inv[slot].type > 0)
			{
				result = 5;
			}
			if (context == 30)
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x004EC4C4 File Offset: 0x004EA6C4
		public static void RightClick(Item[] inv, int context = 0, int slot = 0)
		{
			if (Main.LocalPlayerHasPendingInventoryActions())
			{
				return;
			}
			Player player = Main.player[Main.myPlayer];
			inv[slot].newAndShiny = false;
			if (player.itemAnimation > 0)
			{
				return;
			}
			if (context == 15)
			{
				ItemSlot.HandleShopSlot(inv, slot, true, false);
				return;
			}
			if (!Main.mouseRight)
			{
				return;
			}
			if (context == 6 || context == 34)
			{
				return;
			}
			if (Main.mouseItem.IsAir || !Item.CanStack(inv[slot], Main.mouseItem))
			{
				if (!PlayerInput.UsingGamepadUI && context == 0 && ItemID.Sets.OpenableBag[inv[slot].type])
				{
					if (Main.mouseRightRelease)
					{
						ItemSlot.TryOpenContainer(inv, context, slot, player);
					}
					return;
				}
				if (context == 9 || context == 11)
				{
					if (Main.mouseRightRelease)
					{
						ItemSlot.SwapVanityEquip(inv, context, slot, player);
					}
					return;
				}
				if ((context == 0 || context == 4 || context == 32 || context == 3) && inv[slot].stack == 1 && !PlayerInput.UsingGamepadUI && (inv[slot].CanBeEquipped() || inv[slot].dye > 0 || ItemID.Sets.HasItemSwap[inv[slot].type]))
				{
					if (Main.mouseRightRelease && context == 0)
					{
						ItemSlot.TryItemSwap(inv[slot]);
					}
					if (Main.mouseRightRelease)
					{
						ItemSlot.SwapEquip(inv, context, slot);
					}
					return;
				}
			}
			if (Main.stackSplit <= 1 && !inv[slot].IsAir)
			{
				int num = Main.superFastStack + 1;
				for (int i = 0; i < num; i++)
				{
					if ((Item.CanStack(Main.mouseItem, inv[slot]) && Main.mouseItem.stack < Main.mouseItem.maxStack) || Main.mouseItem.type == 0)
					{
						ItemSlot.PickupItemIntoMouse(inv, context, slot, player);
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						ItemSlot.RefreshStackSplitCooldown();
					}
				}
			}
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x004EC660 File Offset: 0x004EA860
		public static void PickupItemIntoMouse(Item[] inv, int context, int slot, Player player)
		{
			if (Main.mouseItem.type == 0)
			{
				Main.mouseItem = inv[slot].Clone();
				if (context == 29)
				{
					Main.mouseItem.SetDefaults(Main.mouseItem.type, null);
					Main.mouseItem.OnCreated(new JourneyDuplicationItemCreationContext());
				}
				Main.mouseItem.stack = 0;
				if (inv[slot].favorited && inv[slot].stack == 1)
				{
					Main.mouseItem.favorited = true;
				}
				else
				{
					Main.mouseItem.favorited = false;
				}
				ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(inv[slot], context, 21, 0));
			}
			Main.mouseItem.stack++;
			if (context != 29)
			{
				inv[slot].stack--;
			}
			if (inv[slot].stack <= 0)
			{
				inv[slot] = new Item();
			}
			CoinSlot.ForceSlotState(slot, context, inv[slot]);
			if (context == 3 && Main.netMode == 1)
			{
				NetMessage.SendData(32, -1, -1, null, player.chest, (float)slot, 0f, 0f, 0, 0, 0);
			}
			if ((context == 23 || context == 24 || context == 39) && Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, (float)player.tileEntityAnchor.interactEntityID, (float)slot, 0f, 0, 0, 0);
			}
			if (context == 25 && Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, (float)player.tileEntityAnchor.interactEntityID, (float)slot, 1f, 0, 0, 0);
			}
			if (context == 38 && Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, (float)player.tileEntityAnchor.interactEntityID, (float)slot, 3f, 0, 0, 0);
			}
			if (context == 26 && Main.netMode == 1)
			{
				NetMessage.SendData(124, -1, -1, null, Main.myPlayer, (float)player.tileEntityAnchor.interactEntityID, (float)slot, 0f, 0, 0, 0);
			}
			if (context == 27 && Main.netMode == 1)
			{
				NetMessage.SendData(124, -1, -1, null, Main.myPlayer, (float)player.tileEntityAnchor.interactEntityID, (float)slot, 1f, 0, 0, 0);
			}
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x004EC86B File Offset: 0x004EAA6B
		public static void RefreshStackSplitCooldown()
		{
			if (Main.stackSplit == 0)
			{
				Main.stackSplit = 30;
				return;
			}
			Main.stackSplit = Main.stackDelay;
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x004EC888 File Offset: 0x004EAA88
		private static void TryOpenContainer(Item[] inv, int context, int slot, Player player)
		{
			Item item = inv[slot];
			Player.GetItemLogger.Start();
			bool flag = ItemSlot.TryOpenContainer_GrantItems(item, player);
			Player.GetItemLogger.Stop();
			ItemSlot.DisplayTransfer_GetItem(inv, context, slot);
			if (!flag)
			{
				return;
			}
			item.stack--;
			if (item.stack == 0)
			{
				item.SetDefaults(0, null);
			}
			SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
			Main.stackSplit = 30;
			Main.mouseRightRelease = false;
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x004EC900 File Offset: 0x004EAB00
		private static bool TryOpenContainer_GrantItems(Item item, Player player)
		{
			if (ItemID.Sets.BossBag[item.type])
			{
				player.OpenBossBag(item.type);
			}
			else if (ItemID.Sets.IsFishingCrate[item.type])
			{
				player.OpenFishingCrate(item.type);
			}
			else if (item.type == 3093)
			{
				player.OpenHerbBag(3093);
			}
			else if (item.type == 4345)
			{
				player.OpenCanofWorms(item.type);
			}
			else if (item.type == 4410)
			{
				player.OpenOyster(item.type);
			}
			else if (item.type == 1774)
			{
				player.OpenGoodieBag(1774);
			}
			else if (item.type == 6142)
			{
				player.OpenChilletEgg(6142);
			}
			else if (item.type == 3085)
			{
				if (!player.ConsumeItem(327, false, true))
				{
					return false;
				}
				player.OpenLockBox(3085);
			}
			else if (item.type == 4879)
			{
				if (!player.HasItemInInventoryOrOpenVoidBag(329))
				{
					return false;
				}
				player.OpenShadowLockbox(4879);
			}
			else if (item.type == 1869)
			{
				player.OpenPresent(1869);
			}
			else
			{
				if (item.type != 599 && item.type != 600 && item.type != 601)
				{
					return false;
				}
				player.OpenLegacyPresent(item.type);
			}
			return true;
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x004ECA84 File Offset: 0x004EAC84
		private static void SwapVanityEquip(Item[] inv, int context, int slot, Player player)
		{
			if (Main.npcShop > 0)
			{
				return;
			}
			Item item = inv[slot - 10];
			if (inv[slot].IsAir && item.IsAir)
			{
				return;
			}
			if (context == 11)
			{
				if (!inv[slot].IsAir && !ItemSlot.CanEquipAccessoryInSlot(inv[slot], new ArraySegment<Item>(Main.LocalPlayer.armor, 3, 7), slot - 10))
				{
					return;
				}
				if (!item.IsAir && !ItemSlot.CanEquipAccessoryInSlot(item, new ArraySegment<Item>(Main.LocalPlayer.armor, 13, 7), slot))
				{
					return;
				}
			}
			Utils.Swap<Item>(ref inv[slot], ref inv[slot - 10]);
			SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
			if (inv[slot].stack > 0)
			{
				if (context <= 12)
				{
					if (context == 0)
					{
						AchievementsHelper.NotifyItemPickup(player, inv[slot]);
						return;
					}
					if (context - 8 > 4)
					{
						return;
					}
				}
				else if (context - 16 > 1 && context != 33)
				{
					return;
				}
				AchievementsHelper.HandleOnEquip(player, inv[slot], context);
			}
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x004ECB6C File Offset: 0x004EAD6C
		private static void TryItemSwap(Item item)
		{
			int type = item.type;
			if (type > 5309)
			{
				if (type <= 5391)
				{
					switch (type)
					{
					case 5323:
						break;
					case 5324:
						item.ChangeItemType(5329);
						ItemSlot.AfterItemSwap(type, item.type);
						return;
					case 5325:
						goto IL_E8;
					case 5326:
					case 5327:
					case 5328:
						return;
					case 5329:
						item.ChangeItemType(5330);
						ItemSlot.AfterItemSwap(type, item.type);
						return;
					case 5330:
						item.ChangeItemType(5324);
						ItemSlot.AfterItemSwap(type, item.type);
						return;
					default:
						switch (type)
						{
						case 5358:
							item.ChangeItemType(5360);
							ItemSlot.AfterItemSwap(type, item.type);
							return;
						case 5359:
							item.ChangeItemType(5358);
							ItemSlot.AfterItemSwap(type, item.type);
							return;
						case 5360:
							item.ChangeItemType(5361);
							ItemSlot.AfterItemSwap(type, item.type);
							return;
						case 5361:
							item.ChangeItemType(5359);
							ItemSlot.AfterItemSwap(type, item.type);
							return;
						default:
							if (type != 5391)
							{
								return;
							}
							goto IL_188;
						}
						break;
					}
				}
				else
				{
					if (type == 5437)
					{
						item.ChangeItemType(5358);
						ItemSlot.AfterItemSwap(type, item.type);
						return;
					}
					switch (type)
					{
					case 5453:
						goto IL_1E0;
					case 5454:
						goto IL_20C;
					case 5455:
						break;
					default:
						if (type != 5526)
						{
							return;
						}
						goto IL_2B0;
					}
				}
				item.ChangeItemType((item.type == 5323) ? 5455 : 5323);
				ItemSlot.AfterItemSwap(type, item.type);
				return;
			}
			if (type <= 4346)
			{
				if (type == 2611)
				{
					goto IL_2B0;
				}
				if (type != 4131)
				{
					if (type != 4346)
					{
						return;
					}
					goto IL_188;
				}
			}
			else
			{
				if (type == 4767)
				{
					goto IL_1E0;
				}
				if (type - 5059 <= 1)
				{
					item.ChangeItemType((item.type == 5059) ? 5060 : 5059);
					ItemSlot.AfterItemSwap(type, item.type);
					return;
				}
				if (type != 5309)
				{
					return;
				}
				goto IL_20C;
			}
			IL_E8:
			item.ChangeItemType((item.type == 5325) ? 4131 : 5325);
			ItemSlot.AfterItemSwap(type, item.type);
			return;
			IL_188:
			item.ChangeItemType((item.type == 4346) ? 5391 : 4346);
			ItemSlot.AfterItemSwap(type, item.type);
			return;
			IL_1E0:
			item.ChangeItemType((item.type == 4767) ? 5453 : 4767);
			ItemSlot.AfterItemSwap(type, item.type);
			return;
			IL_20C:
			item.ChangeItemType((item.type == 5309) ? 5454 : 5309);
			ItemSlot.AfterItemSwap(type, item.type);
			return;
			IL_2B0:
			item.ChangeItemType((item.type == 2611) ? 5526 : 2611);
			ItemSlot.AfterItemSwap(type, item.type);
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x004ECE54 File Offset: 0x004EB054
		private static void AfterItemSwap(int oldType, int newType)
		{
			if (newType == 5324 || newType == 5329 || newType == 5330 || newType == 4346 || newType == 5391 || newType == 5358 || newType == 5361 || newType == 5360 || newType == 5359 || newType == 2611 || newType == 5526)
			{
				SoundEngine.PlaySound(22, -1, -1, 1, 1f, 0f);
			}
			else
			{
				SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
			}
			Main.stackSplit = 30;
			Main.mouseRightRelease = false;
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x004ECEF4 File Offset: 0x004EB0F4
		public static void HandleItemPickupAction<TItemInfo>(ItemSlot.ItemPickupAction<TItemInfo> action, TItemInfo entry, int slotItemType, int slotItemStack, ref int stackLimiter)
		{
			if (!Main.mouseItem.IsAir && slotItemType != Main.mouseItem.type)
			{
				return;
			}
			bool flag = stackLimiter == -1;
			if (Main.stackSplit > 1 || flag)
			{
				return;
			}
			if (stackLimiter == 0)
			{
				stackLimiter = slotItemStack;
			}
			int num = Math.Min(Main.superFastStack + 1, stackLimiter);
			stackLimiter -= num;
			if (stackLimiter == 0)
			{
				stackLimiter = -1;
			}
			action(entry, num);
			ItemSlot.RefreshStackSplitCooldown();
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x004ECF6C File Offset: 0x004EB16C
		private static void HandleShopSlot(Item[] inv, int slot, bool rightClickIsValid, bool leftClickIsValid)
		{
			if (Main.cursorOverride == 2)
			{
				return;
			}
			Chest chest = Main.instance.shop[Main.npcShop];
			bool flag = (Main.mouseRight && rightClickIsValid) || (Main.mouseLeft && leftClickIsValid);
			if (Main.stackSplit <= 1 && flag && inv[slot].type > 0 && (Item.CanStack(Main.mouseItem, inv[slot]) || Main.mouseItem.type == 0))
			{
				int num = Main.superFastStack + 1;
				if (ItemSlot.CanBulkBuy(inv[slot]))
				{
					num *= ItemSlot.GetBulkBuyAmount(inv[slot]);
				}
				Player localPlayer = Main.LocalPlayer;
				for (int i = 0; i < num; i++)
				{
					if (Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0)
					{
						long num2;
						long price;
						localPlayer.GetItemExpectedPrice(inv[slot], out num2, out price);
						if (localPlayer.BuyItem(price, inv[slot].shopSpecialCurrency) && inv[slot].stack > 0)
						{
							if (i == 0)
							{
								if (inv[slot].value > 0)
								{
									SoundEngine.PlaySound(18, -1, -1, 1, 1f, 0f);
								}
								else
								{
									SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
								}
							}
							if (Main.mouseItem.type == 0)
							{
								Main.mouseItem.SetDefaults(inv[slot].type, null);
								if (inv[slot].prefix != 0)
								{
									Main.mouseItem.Prefix((int)inv[slot].prefix);
								}
								Main.mouseItem.stack = 0;
							}
							if (!inv[slot].buyOnce && inv[slot].shopSpecialCurrency == -1)
							{
								Main.shopSellbackHelper.Add(inv[slot]);
							}
							Main.mouseItem.stack++;
							ItemSlot.RefreshStackSplitCooldown();
							if (inv[slot].buyOnce)
							{
								Item item = inv[slot];
								int num3 = item.stack - 1;
								item.stack = num3;
								if (num3 <= 0)
								{
									inv[slot].SetDefaults(0, null);
								}
							}
							ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(Main.mouseItem, 15, 21, 0));
						}
					}
				}
			}
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x004ED169 File Offset: 0x004EB369
		public static void Draw(SpriteBatch spriteBatch, ref Item inv, int context, Vector2 position, Color lightColor = default(Color))
		{
			ItemSlot.singleSlotArray[0] = inv;
			ItemSlot.Draw(spriteBatch, ItemSlot.singleSlotArray, context, 0, position, lightColor);
			inv = ItemSlot.singleSlotArray[0];
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x004ED190 File Offset: 0x004EB390
		public static void Draw(SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor = default(Color))
		{
			Player player = Main.player[Main.myPlayer];
			Item item = inv[slot];
			float inventoryScale = Main.inventoryScale;
			Color color = Color.White;
			if (lightColor != Color.Transparent)
			{
				color = lightColor;
			}
			bool flag = false;
			if (context == 36)
			{
				flag = true;
				context = 13;
			}
			else if (context == 13 && slot == player.selectedItem && !player.selectedItemState.HasActiveOverride)
			{
				flag = true;
			}
			bool flag2 = false;
			int num = 0;
			int gamepadPointForSlot = ItemSlot.GetGamepadPointForSlot(inv, context, slot);
			if (PlayerInput.UsingGamepadUI)
			{
				flag2 = (UILinkPointNavigator.CurrentPoint == gamepadPointForSlot);
				if (PlayerInput.SettingsForUI.PreventHighlightsForGamepad)
				{
					flag2 = false;
				}
				if (context == 0)
				{
					num = player.DpadRadial.GetDrawMode(slot);
					if (num > 0 && !PlayerInput.CurrentProfile.UsingDpadHotbar())
					{
						num = 0;
					}
				}
			}
			Texture2D value = TextureAssets.InventoryBack.Value;
			Color color2 = Main.inventoryBack;
			bool flag3 = false;
			bool highlightThingsForMouse = PlayerInput.SettingsForUI.HighlightThingsForMouse;
			if (item.type > 0 && item.stack > 0 && item.favorited && context != 13 && context != 21 && context != 37 && context != 22 && context != 14 && context != 35)
			{
				value = TextureAssets.InventoryBack10.Value;
				if (context == 32)
				{
					value = TextureAssets.InventoryBack19.Value;
				}
			}
			else if (item.type > 0 && item.stack > 0 && ItemSlot.Options.HighlightNewItems && item.newAndShiny && context != 13 && context != 21 && context != 37 && context != 14 && context != 22 && context != 35)
			{
				value = TextureAssets.InventoryBack15.Value;
				float num2 = (float)Main.mouseTextColor / 255f;
				num2 = num2 * 0.2f + 0.8f;
				color2 = color2.MultiplyRGBA(new Color(num2, num2, num2));
			}
			else if (!highlightThingsForMouse && item.type > 0 && item.stack > 0 && num != 0 && context != 13 && context != 21 && context != 37 && context != 22 && context != 35)
			{
				value = TextureAssets.InventoryBack15.Value;
				float num3 = (float)Main.mouseTextColor / 255f;
				num3 = num3 * 0.2f + 0.8f;
				if (num == 1)
				{
					color2 = color2.MultiplyRGBA(new Color(num3, num3 / 2f, num3 / 2f));
				}
				else
				{
					color2 = color2.MultiplyRGBA(new Color(num3 / 2f, num3, num3 / 2f));
				}
			}
			else if (context == 0 && slot < 10)
			{
				value = TextureAssets.InventoryBack9.Value;
			}
			else if (context == 28)
			{
				value = TextureAssets.InventoryBack7.Value;
				color2 = Color.White;
			}
			else if (context == 16 || context == 17 || context == 19 || context == 18 || context == 20 || context == 17)
			{
				value = TextureAssets.InventoryBack3.Value;
			}
			else if (context == 10 || context == 8)
			{
				value = TextureAssets.InventoryBack13.Value;
				color2 = ItemSlot.GetColorByLoadout(slot, context);
			}
			else if (context == 24 || context == 23 || context == 39 || context == 38 || context == 26)
			{
				value = TextureAssets.InventoryBack8.Value;
			}
			else if (context == 11 || context == 9)
			{
				value = TextureAssets.InventoryBack13.Value;
				color2 = ItemSlot.GetColorByLoadout(slot, context);
			}
			else if (context == 25 || context == 27 || context == 33)
			{
				value = TextureAssets.InventoryBack12.Value;
			}
			else if (context == 12)
			{
				value = TextureAssets.InventoryBack13.Value;
				color2 = ItemSlot.GetColorByLoadout(slot, context);
			}
			else if (context == 3)
			{
				value = TextureAssets.InventoryBack5.Value;
			}
			else if (context == 4 || context == 32)
			{
				value = TextureAssets.InventoryBack2.Value;
			}
			else if (context == 7 || context == 5)
			{
				value = TextureAssets.InventoryBack4.Value;
			}
			else if (context == 6)
			{
				value = TextureAssets.InventoryBack7.Value;
			}
			else if (context == 13)
			{
				byte b = 200;
				if (slot == Main.LocalPlayer.selectedItemState.Hotbar)
				{
					value = TextureAssets.InventoryBack20.Value;
					b = byte.MaxValue;
				}
				if (flag)
				{
					value = TextureAssets.InventoryBack14.Value;
					b = byte.MaxValue;
				}
				color2 = new Color((int)b, (int)b, (int)b, (int)b);
			}
			else if (context == 14 || context == 21 || context == 37)
			{
				flag3 = true;
			}
			else if (context == 15)
			{
				value = TextureAssets.InventoryBack6.Value;
			}
			else if (context == 29)
			{
				color2 = new Color(53, 69, 127, 255);
				value = TextureAssets.InventoryBack18.Value;
			}
			else if (context == 34)
			{
				color2 = new Color(25, 44, 65, 180) * 0.9f;
				value = TextureAssets.InventoryBack18.Value;
			}
			else if (context == 30)
			{
				flag3 = !flag2;
			}
			else if (context == 22 || context == 42 || context == 43)
			{
				value = TextureAssets.InventoryBack4.Value;
				if (context == 42 || context == 43)
				{
					color2 = new Color(20, 40, 60, 180) * 0.9f;
					color2 = new Color(16, 36, 56, 180) * 0.9f;
					color2 = Utils.ShiftBlueToCyanTheme(color2);
					value = TextureAssets.InventoryBack18.Value;
					if (slot == 0)
					{
						value = TextureAssets.InventoryBack18.Value;
					}
				}
				if (ItemSlot.DrawGoldBGForCraftingMaterial)
				{
					ItemSlot.DrawGoldBGForCraftingMaterial = false;
					value = TextureAssets.InventoryBack14.Value;
					float num4 = (float)color2.A / 255f;
					if (num4 < 0.7f)
					{
						num4 = Utils.GetLerpValue(0f, 0.7f, num4, true);
					}
					else
					{
						num4 = 1f;
					}
					color2 = Color.White * num4;
				}
			}
			else if (context == 35)
			{
				value = TextureAssets.InventoryBack2.Value;
				if (ItemSlot.DrawGoldBGForCraftingMaterial)
				{
					ItemSlot.DrawGoldBGForCraftingMaterial = false;
					value = TextureAssets.InventoryBack14.Value;
					float num5 = (float)color2.A / 255f;
					if (num5 < 0.7f)
					{
						num5 = Utils.GetLerpValue(0f, 0.7f, num5, true);
					}
					else
					{
						num5 = 1f;
					}
					color2 = Color.White * num5;
				}
			}
			else if (context == 41)
			{
				color2 = new Color(20, 40, 60, 180) * 0.9f;
				color2 = new Color(16, 36, 56, 180) * 0.9f;
				color2 = Utils.ShiftBlueToCyanTheme(color2);
				value = TextureAssets.InventoryBack18.Value;
			}
			if ((context == 0 || context == 2) && ItemSlot.inventoryGlowTime[slot] > 0 && !inv[slot].favorited && !inv[slot].IsAir)
			{
				float scale = Main.invAlpha / 255f;
				Color value2 = new Color(63, 65, 151, 255) * scale;
				Color value3 = Main.hslToRgb(ItemSlot.inventoryGlowHue[slot], 1f, 0.5f, byte.MaxValue) * scale;
				float num6 = (float)ItemSlot.inventoryGlowTime[slot] / 300f;
				num6 *= num6;
				color2 = Color.Lerp(value2, value3, num6 / 2f);
				value = TextureAssets.InventoryBack13.Value;
			}
			if ((context == 4 || context == 32 || context == 3) && ItemSlot.inventoryGlowTimeChest[slot] > 0 && !inv[slot].favorited && !inv[slot].IsAir)
			{
				float scale2 = Main.invAlpha / 255f;
				Color value4 = new Color(130, 62, 102, 255) * scale2;
				if (context == 3)
				{
					value4 = new Color(104, 52, 52, 255) * scale2;
				}
				Color value5 = Main.hslToRgb(ItemSlot.inventoryGlowHueChest[slot], 1f, 0.5f, byte.MaxValue) * scale2;
				float num7 = (float)ItemSlot.inventoryGlowTimeChest[slot] / 300f;
				num7 *= num7;
				color2 = Color.Lerp(value4, value5, num7 / 2f);
				value = TextureAssets.InventoryBack13.Value;
			}
			if (flag2)
			{
				value = TextureAssets.InventoryBack14.Value;
				color2 = Color.White;
				if (item.favorited)
				{
					value = TextureAssets.InventoryBack17.Value;
				}
				if (context == 34)
				{
					color2 = Color.Gray;
				}
			}
			if (context == 41 || context == 43 || context == 42)
			{
				color2 = color2.MultiplyRGBA(lightColor);
			}
			if (context == 28 && Main.MouseScreen.Between(position, position + value.Size() * inventoryScale) && !player.mouseInterface)
			{
				value = TextureAssets.InventoryBack14.Value;
				color2 = Color.White;
			}
			CoinSlot.CoinDrawState coinDrawState;
			CoinSlot.UpdateDrawState(slot, context, item, out coinDrawState);
			float num8 = 1f;
			ItemSlot.GetDimSlotForMouseItem(context, slot, Main.mouseItem, out num8);
			color2 *= num8;
			if (!flag3)
			{
				spriteBatch.Draw(value, position, null, color2, 0f, default(Vector2), inventoryScale, SpriteEffects.None, 0f);
				if (context == 32 || context == 0 || context == 2 || context == 1)
				{
					int num9 = (context == 32) ? (slot + PlayerItemSlotID.Bank4_0) : slot;
					ItemSlot.PulseEffect pulseEffect = ItemSlot.playerSlotPulseEffects[num9];
					if (pulseEffect.IsActive)
					{
						float num10 = (float)ItemSlot.PulseEffect.EffectDuration;
						float num11 = 0.5f;
						float num12 = 3.1415927f;
						float num13 = (float)pulseEffect.time / num10;
						Color color3 = pulseEffect.color * (float)(0.5 + 0.2 * -(float)Math.Cos((double)num13 * 3.141592653589793 * 2.0 * (double)num11 + (double)num12));
						color3 *= 1f - num13 * num13 * num13 * num13;
						spriteBatch.Draw(TextureAssets.InventoryBack21.Value, position, null, color3, 0f, default(Vector2), inventoryScale, SpriteEffects.None, 0f);
					}
				}
				if (context == 41 && ItemSlot.DrawSelectionHighlightForGridSlot)
				{
					spriteBatch.Draw(TextureAssets.InventoryBack24.Value, position, null, Main.inventoryBack, 0f, default(Vector2), inventoryScale, SpriteEffects.None, 0f);
				}
			}
			if (ItemSlot.ShouldHighlightSlotForMouseItem(context, slot, Main.mouseItem))
			{
				Color color4 = color2;
				if (value == TextureAssets.InventoryBack3.Value)
				{
					color4 = new Color(50, 106, 46, (int)color2.A);
				}
				else if (value == TextureAssets.InventoryBack8.Value)
				{
					color4 = new Color(46, 106, 98, (int)color2.A);
				}
				else if (value == TextureAssets.InventoryBack12.Value)
				{
					color4 = new Color(45, 85, 105, (int)color2.A);
				}
				else if (value == TextureAssets.InventoryBack13.Value)
				{
					ItemSlot.TryGetSlotColor(Main.LocalPlayer.CurrentLoadoutIndex, context, out color4);
					color4.A = color2.A;
				}
				color4 *= 2f;
				spriteBatch.Draw(TextureAssets.InventoryBack22.Value, position, null, color4, 0f, default(Vector2), inventoryScale, SpriteEffects.None, 0f);
			}
			int num14 = -1;
			switch (context)
			{
			case 7:
				num14 = 18;
				break;
			case 8:
			case 23:
				if (slot == 0)
				{
					num14 = 0;
				}
				if (slot == 1)
				{
					num14 = 6;
				}
				if (slot == 2)
				{
					num14 = 12;
				}
				break;
			case 9:
				if (slot == 10)
				{
					num14 = 3;
				}
				if (slot == 11)
				{
					num14 = 9;
				}
				if (slot == 12)
				{
					num14 = 15;
				}
				break;
			case 10:
			case 24:
				num14 = 11;
				break;
			case 11:
				num14 = 2;
				break;
			case 12:
			case 25:
			case 27:
			case 33:
				num14 = 1;
				break;
			case 16:
				num14 = 4;
				break;
			case 17:
			case 39:
				num14 = 13;
				break;
			case 18:
				num14 = 7;
				break;
			case 19:
				num14 = 10;
				break;
			case 20:
				num14 = 17;
				break;
			case 26:
				num14 = 0;
				break;
			}
			if ((item.type <= 0 || item.stack <= 0) && num14 != -1)
			{
				float num15 = 0.35f;
				Texture2D value6 = TextureAssets.Extra[54].Value;
				Rectangle rectangle = value6.Frame(3, 7, num14 % 3, num14 / 3, 0, 0);
				rectangle.Width -= 2;
				rectangle.Height -= 2;
				spriteBatch.Draw(value6, position + value.Size() / 2f * inventoryScale, new Rectangle?(rectangle), Color.White * (num15 * num8), 0f, rectangle.Size() / 2f, inventoryScale, SpriteEffects.None, 0f);
			}
			Vector2 vector = value.Size() * inventoryScale;
			bool flag4 = (item.type > 0 && item.stack > 0) || coinDrawState.fadeItem > 0;
			if (flag4)
			{
				ItemSlot.ItemDisplayKey key = new ItemSlot.ItemDisplayKey
				{
					Context = context,
					Slot = slot
				};
				ulong num16;
				if (ItemSlot._nextTickDrawAvailable.TryGetValue(key, out num16) && num16 > Main.EverLastingTicker)
				{
					flag4 = false;
				}
			}
			if (flag4)
			{
				float scale3;
				if (item.IsACoin || coinDrawState.fadeItem > 0)
				{
					scale3 = CoinSlot.DrawItemCoin(spriteBatch, position + vector / 2f - new Vector2(0f, coinDrawState.coinYOffset * inventoryScale), (coinDrawState.fadeItem > 0) ? coinDrawState.fadeItem : item.type, coinDrawState.coinAnimFrame, inventoryScale, 32f, color, num8 * coinDrawState.fadeScale);
				}
				else
				{
					scale3 = ItemSlot.DrawItemIcon(item, context, spriteBatch, position + vector / 2f, inventoryScale, 32f, color, num8, false);
				}
				if (Main.DoGlowingMouseItemDraw)
				{
					float glow = CraftingEffects.GetGlow(item);
					float num17 = glow;
					num17 *= num17;
					bool flag5 = true;
					if (!Main.FlashyEffectsInterface)
					{
						flag5 = false;
					}
					if (glow > 0f && flag5)
					{
						float num18 = Utils.Remap(num17, 1f, 0f, 1f, 2f, true);
						float fromValue = Utils.Remap((float)Main.stackCounter, 0f, 8f, 0f, 1f, true);
						if (Main.superFastStack > 0)
						{
							fromValue = 1f;
						}
						Utils.Remap(fromValue, 0f, 1f, 1f, 0.5f, true);
						num17 *= Utils.Remap(glow, 1f, 0.95f, 0.5f, 1f, true);
						MiscShaderData miscShaderData = GameShaders.Misc["MouseItem"];
						miscShaderData.UseSaturation(num17);
						miscShaderData.UseColor(Main.OurFavoriteColor);
						miscShaderData.Apply(null);
						if (item.IsACoin || coinDrawState.fadeItem > 0)
						{
							scale3 = CoinSlot.DrawItemCoin(spriteBatch, position + vector / 2f - new Vector2(0f, coinDrawState.coinYOffset * inventoryScale), (coinDrawState.fadeItem > 0) ? coinDrawState.fadeItem : item.type, coinDrawState.coinAnimFrame, inventoryScale * num18, 32f, color, num8 * coinDrawState.fadeScale);
						}
						else
						{
							scale3 = ItemSlot.DrawItemIcon(item, context, spriteBatch, position + vector / 2f, inventoryScale * num18, 32f, color, num8, false);
						}
						Main.pixelShader.CurrentTechnique.Passes[0].Apply();
					}
				}
				if (item.type == 5324 || item.type == 5329 || item.type == 5330)
				{
					Vector2 value7 = new Vector2(2f, -6f) * inventoryScale;
					int type = item.type;
					if (type != 5324)
					{
						if (type != 5329)
						{
							if (type == 5330)
							{
								Texture2D value8 = TextureAssets.Extra[257].Value;
								Rectangle rectangle2 = value8.Frame(3, 1, 0, 0, 0, 0);
								spriteBatch.Draw(value8, position + value7 + new Vector2(40f, 40f) * inventoryScale, new Rectangle?(rectangle2), color * num8, 0f, rectangle2.Size() / 2f, 1f, SpriteEffects.None, 0f);
							}
						}
						else
						{
							Texture2D value9 = TextureAssets.Extra[257].Value;
							Rectangle rectangle3 = value9.Frame(3, 1, 1, 0, 0, 0);
							spriteBatch.Draw(value9, position + value7 + new Vector2(40f, 40f) * inventoryScale, new Rectangle?(rectangle3), color * num8, 0f, rectangle3.Size() / 2f, 1f, SpriteEffects.None, 0f);
						}
					}
					else
					{
						Texture2D value10 = TextureAssets.Extra[257].Value;
						Rectangle rectangle4 = value10.Frame(3, 1, 2, 0, 0, 0);
						spriteBatch.Draw(value10, position + value7 + new Vector2(40f, 40f) * inventoryScale, new Rectangle?(rectangle4), color * num8, 0f, rectangle4.Size() / 2f, 1f, SpriteEffects.None, 0f);
					}
				}
				int num19 = -1;
				if (context == 13)
				{
					if (item.DD2Summon)
					{
						for (int i = 0; i < 58; i++)
						{
							if (inv[i].type == 3822)
							{
								num19 += inv[i].stack;
							}
						}
						if (num19 >= 0)
						{
							num19++;
						}
					}
					if (item.useAmmo > 0)
					{
						int useAmmo = item.useAmmo;
						num19 = 0;
						for (int j = 0; j < 58; j++)
						{
							if (inv[j].ammo == useAmmo)
							{
								num19 += inv[j].stack;
							}
						}
					}
					if (item.fishingPole > 0)
					{
						num19 = 0;
						for (int k = 0; k < 58; k++)
						{
							if (inv[k].bait > 0)
							{
								num19 += inv[k].stack;
							}
						}
					}
					if (item.tileWand > 0)
					{
						int tileWand = item.tileWand;
						num19 = 0;
						for (int l = 0; l < 58; l++)
						{
							if (inv[l].type == tileWand)
							{
								num19 += inv[l].stack;
							}
						}
					}
					int num20;
					if (player.TryGetFlexibleWandAvailableUsageCount(item, out num20))
					{
						num19 = num20;
					}
					else if (item.GetFlexibleTileWand() != null && item.GetFlexibleTileWand().ShowsHoverAmmoIcon)
					{
						num19 = 0;
					}
					int type = item.type;
					if (type - 1071 <= 1 || type - 1543 <= 1)
					{
						Item item2 = player.FindPaintOrCoating();
						if (item2 != null)
						{
							num19 = item2.stack;
						}
					}
					if (item.type == 509 || item.type == 851 || item.type == 850 || item.type == 3612 || item.type == 3625 || item.type == 3611)
					{
						num19 = 0;
						for (int m = 0; m < 58; m++)
						{
							if (inv[m].type == 530)
							{
								num19 += inv[m].stack;
							}
						}
					}
				}
				if (num19 != -1)
				{
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, num19.ToString(), position + new Vector2(8f, 30f) * inventoryScale, color, 0f, Vector2.Zero, new Vector2(inventoryScale * 0.8f), -1f, inventoryScale);
				}
				if (context != 37 && (item.stack > 1 || coinDrawState.stackTextDrawFadeOverload > 0f) && num19 == -1)
				{
					Vector2 vector2 = new Vector2(10f, (float)(26 + FontAssets.ItemStack.Value.LineSpacing));
					float num21 = inventoryScale * coinDrawState.stackTextScale;
					if (context == 43)
					{
						vector2 += new Vector2(-5f, 7f);
						num21 *= 1.2f;
					}
					float scale4 = (coinDrawState.stackTextDrawFadeOverload > 0f) ? coinDrawState.stackTextDrawFadeOverload : 1f;
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, item.stack.ToString(), position + vector2 * inventoryScale, color * scale4, 0f, new Vector2(0f, (float)FontAssets.ItemStack.Value.LineSpacing), new Vector2(num21), -1f, num21);
				}
				if (context == 13)
				{
					string text = string.Concat(slot + 1);
					if (text == "10")
					{
						text = "0";
					}
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text, position + new Vector2(8f, 4f) * inventoryScale, color, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
				}
				if (context == 13 && item.potion)
				{
					Vector2 position2 = position + value.Size() * inventoryScale / 2f - TextureAssets.Cd.Value.Size() * inventoryScale / 2f;
					Color color5 = item.GetAlpha(color) * ((float)player.potionDelay / (float)player.potionDelayTime);
					spriteBatch.Draw(TextureAssets.Cd.Value, position2, null, color5, 0f, default(Vector2), scale3, SpriteEffects.None, 0f);
				}
				if (context == 34)
				{
					Vector2 position3 = position + value.Size() * inventoryScale / 2f - TextureAssets.Cd.Value.Size() * inventoryScale / 2f;
					Color color6 = item.GetAlpha(color) * 0.5f;
					spriteBatch.Draw(TextureAssets.Cd.Value, position3, null, color6, 0f, default(Vector2), scale3, SpriteEffects.None, 0f);
				}
				if ((context == 10 || context == 18) && item.expertOnly && !Main.expertMode)
				{
					Vector2 position4 = position + value.Size() * inventoryScale / 2f - TextureAssets.Cd.Value.Size() * inventoryScale / 2f;
					Color white = Color.White;
					spriteBatch.Draw(TextureAssets.Cd.Value, position4, null, white * num8, 0f, default(Vector2), scale3, SpriteEffects.None, 0f);
				}
			}
			else if (context == 6)
			{
				Texture2D value11 = TextureAssets.Trash.Value;
				Vector2 position5 = position + value.Size() * inventoryScale / 2f - value11.Size() * inventoryScale / 2f;
				spriteBatch.Draw(value11, position5, null, new Color(100, 100, 100, 100), 0f, default(Vector2), inventoryScale, SpriteEffects.None, 0f);
			}
			if (context == 0 && slot < 10)
			{
				float num22 = inventoryScale;
				string text2 = string.Concat(slot + 1);
				if (text2 == "10")
				{
					text2 = "0";
				}
				Color baseColor = Main.inventoryBack;
				int num23 = 0;
				if (Main.player[Main.myPlayer].selectedItem == slot)
				{
					baseColor = Color.White;
					baseColor.A = 200;
					num23 -= 2;
					num22 *= 1.4f;
				}
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text2, position + new Vector2(6f, (float)(4 + num23)) * inventoryScale, baseColor, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
			}
			if (gamepadPointForSlot != -1)
			{
				UILinkPointNavigator.SetPosition(gamepadPointForSlot, position + vector * 0.75f);
			}
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x004EEA0C File Offset: 0x004ECC0C
		public static Color GetColorByLoadout(int slot, int context)
		{
			Color color = Color.White;
			Color color2;
			if (ItemSlot.TryGetSlotColor(Main.LocalPlayer.CurrentLoadoutIndex, context, out color2))
			{
				color = color2;
			}
			Color value = new Color(color.ToVector4() * Main.inventoryBack.ToVector4());
			float num = Utils.Remap((float)(Main.timeForVisualEffects - ItemSlot._lastTimeForVisualEffectsThatLoadoutWasChanged), 0f, 30f, 0.5f, 0f, true);
			if (!Main.FlashyEffectsInterface)
			{
				num = 0f;
			}
			return Color.Lerp(value, Color.White, num * num * num);
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x004EEA93 File Offset: 0x004ECC93
		public static void RecordLoadoutChange()
		{
			ItemSlot._lastTimeForVisualEffectsThatLoadoutWasChanged = Main.timeForVisualEffects;
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x004EEAA0 File Offset: 0x004ECCA0
		public static bool TryGetSlotColor(int loadoutIndex, int context, out Color color)
		{
			color = default(Color);
			if (loadoutIndex < 0 || loadoutIndex >= 3)
			{
				return false;
			}
			int num = -1;
			switch (context)
			{
			case 8:
			case 10:
				num = 0;
				break;
			case 9:
			case 11:
				num = 1;
				break;
			case 12:
				num = 2;
				break;
			}
			if (num == -1)
			{
				return false;
			}
			color = ItemSlot.LoadoutSlotColors[loadoutIndex, num];
			return true;
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x004EEB01 File Offset: 0x004ECD01
		public static float ShiftHueByLoadout(float hue, int loadoutIndex)
		{
			return (hue + (float)loadoutIndex / 8f) % 1f;
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x004EEB13 File Offset: 0x004ECD13
		public static Color GetLoadoutColor(int loadoutIndex)
		{
			return Main.hslToRgb(ItemSlot.ShiftHueByLoadout(0.41f, loadoutIndex), 0.7f, 0.5f, byte.MaxValue);
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x004EEB34 File Offset: 0x004ECD34
		public static float DrawItemIcon(Item item, int context, SpriteBatch spriteBatch, Vector2 screenPositionForItemCenter, float scale, float sizeLimit, Color environmentColor, float itemFade = 1f, bool flip = false)
		{
			Color secondColor = Color.White;
			Color secondColor2 = Color.White;
			int type = item.type;
			Main.instance.LoadItem(type);
			Texture2D value = TextureAssets.Item[type].Value;
			Rectangle rectangle = value.Frame(1, 1, 0, 0, 0, 0);
			DrawAnimation drawAnimation = Main.itemAnimations[type];
			if (drawAnimation != null)
			{
				int frameCounterOverride = -1;
				if (type == 5644 && context != 31 && !Main.LocalPlayer.AnyoneToSpectate())
				{
					frameCounterOverride = 0;
				}
				rectangle = drawAnimation.GetFrame(value, frameCounterOverride);
			}
			float num = 1f;
			if (context == 37)
			{
				secondColor = ItemSlot.OverdrawGlowColorMultiplier;
				secondColor2 = ItemSlot.OverdrawGlowColorMultiplier;
				environmentColor = Color.White;
				num = ItemSlot.OverdrawGlowSize;
			}
			Color newColor;
			float num2;
			ItemSlot.DrawItem_GetColorAndScale(item, scale, ref environmentColor, sizeLimit, ref rectangle, out newColor, out num2);
			SpriteEffects effects = SpriteEffects.None;
			if (flip)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Vector2 origin = rectangle.Size() / 2f;
			Color value2 = item.GetAlpha(newColor).MultiplyRGBA(secondColor);
			spriteBatch.Draw(value, screenPositionForItemCenter, new Rectangle?(rectangle), value2 * itemFade, 0f, origin, num2 * num, effects, 0f);
			if (item.color != Color.Transparent)
			{
				Color newColor2 = environmentColor;
				if (context == 13)
				{
					newColor2.A = byte.MaxValue;
				}
				spriteBatch.Draw(value, screenPositionForItemCenter, new Rectangle?(rectangle), item.GetColor(newColor2).MultiplyRGBA(secondColor2) * itemFade, 0f, origin, num2 * num, effects, 0f);
			}
			if (item.glowMask != -1 && item.type != 3779 && item.type != 46 && item.type != 5462)
			{
				Rectangle rectangle2 = rectangle;
				Color color = Color.White;
				int type2 = item.type;
				if (type2 <= 5146)
				{
					if (type2 != 3858)
					{
						if (type2 == 5146)
						{
							color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
						}
					}
					else
					{
						color = new Color(255, 255, 255, 63) * 0.75f;
						rectangle2 = TextureAssets.GlowMask[233].Value.Frame(1, 3, 0, Main.LocalPlayer.miscCounter % 15 / 5, 0, 0);
						rectangle2.Height -= 2;
					}
				}
				else if (type2 != 5462)
				{
					if (type2 != 5669)
					{
						if (type2 - 5670 <= 1)
						{
							color = Item.GetPhaseColor(item.shoot, false);
						}
					}
					else
					{
						float amount = Utils.WrappedLerp(0.5f, 1f, (float)(Main.LocalPlayer.miscCounter % 100) / 100f);
						color = Color.Lerp(color, new Color(180, 85, 30), amount);
						color.A = (byte)item.alpha;
					}
				}
				else
				{
					color = new Color(255, 140, 0, 5);
				}
				if (item.type == 5670 || item.type == 5671)
				{
					spriteBatch.Draw(TextureAssets.GlowMask[(int)item.glowMask].Value, screenPositionForItemCenter, new Rectangle?(rectangle2), color * itemFade, 0f, rectangle2.Size() / 2f, num2 * num, effects, 0f);
					color = Item.GetPhaseColor(item.shoot, true);
				}
				spriteBatch.Draw(TextureAssets.GlowMask[(int)item.glowMask].Value, screenPositionForItemCenter, new Rectangle?(rectangle2), color * itemFade, 0f, rectangle2.Size() / 2f, num2 * num, effects, 0f);
			}
			if (ItemID.Sets.TrapSigned[item.type])
			{
				Vector2 value3 = new Vector2(1f, -1f);
				Vector2 value4 = rectangle.Size() * 0.45f * value3 * num2 * num;
				spriteBatch.Draw(TextureAssets.Wire.Value, screenPositionForItemCenter + value4, new Rectangle?(new Rectangle(4, 58, 8, 8)), environmentColor * itemFade, 0f, new Vector2(4f), num2 * num, SpriteEffects.None, 0f);
			}
			if (ItemID.Sets.DrawUnsafeIndicator[item.type])
			{
				Vector2 value5 = new Vector2(1f, -1f);
				Vector2 value6 = (rectangle.Size() * 0.45f + new Vector2(-4f, -4f)) * value5 * num2 * num;
				Texture2D value7 = TextureAssets.Extra[258].Value;
				Rectangle rectangle3 = value7.Frame(1, 1, 0, 0, 0, 0);
				spriteBatch.Draw(value7, screenPositionForItemCenter + value6, new Rectangle?(rectangle3), environmentColor * itemFade, 0f, rectangle3.Size() / 2f, num2 * num, SpriteEffects.None, 0f);
			}
			return num2;
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x004EF030 File Offset: 0x004ED230
		public static void DrawItem_GetColorAndScale(Item item, float scale, ref Color currentWhite, float sizeLimit, ref Rectangle frame, out Color itemLight, out float finalDrawScale)
		{
			itemLight = currentWhite;
			float num = 1f;
			ItemSlot.GetItemLight(ref itemLight, ref num, item, false, 1f);
			float num2 = 1f;
			if ((float)frame.Width > sizeLimit || (float)frame.Height > sizeLimit)
			{
				if (frame.Width > frame.Height)
				{
					num2 = sizeLimit / (float)frame.Width;
				}
				else
				{
					num2 = sizeLimit / (float)frame.Height;
				}
			}
			if (item.type == 5669 && sizeLimit == 20f)
			{
				num2 = 0.5f;
			}
			finalDrawScale = scale * num2 * num;
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x004EF0C8 File Offset: 0x004ED2C8
		private static int GetGamepadPointForSlot(Item[] inv, int context, int slot)
		{
			Player localPlayer = Main.LocalPlayer;
			int result = -1;
			switch (context)
			{
			case 0:
			case 1:
			case 2:
				result = slot;
				break;
			case 3:
			case 4:
			case 32:
				result = 400 + slot - ChestUI.StartingRowForDrawing * 10;
				break;
			case 5:
				result = 303;
				break;
			case 6:
				result = 300;
				break;
			case 7:
				result = (NewCraftingUI.Visible ? 20020 : 1500);
				break;
			case 8:
			case 9:
			case 10:
			case 11:
			{
				int num = slot;
				if (num % 10 == 9 && !localPlayer.CanDemonHeartAccessoryBeShown())
				{
					num--;
				}
				result = 100 + num;
				break;
			}
			case 12:
				if (inv == localPlayer.dye)
				{
					int num2 = slot;
					if (num2 % 10 == 9 && !localPlayer.CanDemonHeartAccessoryBeShown())
					{
						num2--;
					}
					result = 120 + num2;
				}
				break;
			case 15:
				result = 2700 + slot;
				break;
			case 16:
				result = 184;
				break;
			case 17:
				result = 183;
				break;
			case 18:
				result = 182;
				break;
			case 19:
				result = 180;
				break;
			case 20:
				result = 181;
				break;
			case 22:
				if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig != -1)
				{
					result = 22000 + UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig;
				}
				if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall != -1)
				{
					result = 1500 + UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall + 1;
				}
				break;
			case 23:
			case 24:
			case 39:
				result = 5100 + slot;
				break;
			case 25:
				result = 5109 + slot;
				break;
			case 26:
				result = 5000 + slot;
				break;
			case 27:
				result = 5002 + slot;
				break;
			case 29:
			case 34:
			case 41:
				result = 3000 + slot;
				if (UILinkPointNavigator.Shortcuts.ItemSlotShouldHighlightAsSelected)
				{
					result = UILinkPointNavigator.CurrentPoint;
				}
				break;
			case 30:
				result = 15000 + slot;
				break;
			case 33:
				if (inv == localPlayer.miscDyes)
				{
					result = 185 + slot;
				}
				break;
			case 35:
				if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall != -1)
				{
					result = 12000;
				}
				if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig != -1)
				{
					result = 11100;
				}
				break;
			case 38:
				result = 5118;
				break;
			case 42:
				result = 20000;
				break;
			case 43:
				result = 20001 + UILinkPointNavigator.Shortcuts.NewCraftingUI_MaterialIndex;
				break;
			}
			return result;
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x004EF347 File Offset: 0x004ED547
		public static void MouseHover(int context = 0)
		{
			ItemSlot.MouseHover(Main.HoverItem, context);
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x004EF354 File Offset: 0x004ED554
		public static void MouseHover(Item item, int context = 0)
		{
			ItemSlot.singleSlotArray[0] = item;
			ItemSlot.MouseHover(ItemSlot.singleSlotArray, context, 0);
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x004EF36C File Offset: 0x004ED56C
		public static int GetBulkBuyAmount(Item item)
		{
			int num = 10;
			if (!item.buyOnce)
			{
				return num;
			}
			return Math.Min(num, item.stack);
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x004EF392 File Offset: 0x004ED592
		public static bool CanBulkBuy(Item item)
		{
			return (item.isAShopItem || item.buyOnce) && ItemSlot.ShiftInUse;
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x004EF3B0 File Offset: 0x004ED5B0
		public static int GetBulkCraftAmount(Item item)
		{
			int num = item.maxStack / item.stack;
			if (num < 1)
			{
				num = 1;
			}
			return Math.Min(num, 10);
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x004EF3DC File Offset: 0x004ED5DC
		public static int EstimateDisplayStack(Item item)
		{
			int num = item.buyOnce ? 1 : item.stack;
			if (ItemSlot.CanBulkBuy(item))
			{
				int bulkBuyAmount = ItemSlot.GetBulkBuyAmount(item);
				return num * bulkBuyAmount;
			}
			if (Main.TryingToBulkCraft() && ((item.tooltipContext == 22 && item.tooltipSlot == 0) || item.tooltipContext == 42 || item.tooltipContext == 41))
			{
				return ItemSlot.GetBulkCraftAmount(item) * item.stack;
			}
			return num;
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x004EF44C File Offset: 0x004ED64C
		public static void MouseHover(Item[] inv, int context = 0, int slot = 0)
		{
			if (context == 6 && Main.hoverItemName == null)
			{
				Main.hoverItemName = Lang.inter[3].Value;
			}
			if (!inv[slot].IsAir)
			{
				ItemSlot._customCurrencyForSavings = inv[slot].shopSpecialCurrency;
				Main.hoverItemName = inv[slot].Name;
				if (inv[slot].stack > 1)
				{
					Main.hoverItemName = string.Concat(new object[]
					{
						Main.hoverItemName,
						" (",
						inv[slot].stack,
						")"
					});
				}
				Main.HoverItem = inv[slot].Clone();
				Main.HoverItem.tooltipContext = context;
				Main.HoverItem.tooltipSlot = inv[slot].tooltipSlot;
				if (context == 8)
				{
					Main.HoverItem.wornArmor = true;
					return;
				}
				if (context == 11 || context == 9)
				{
					Main.HoverItem.social = true;
					return;
				}
				if (context == 15)
				{
					Main.HoverItem.buy = true;
					return;
				}
			}
			else
			{
				if (context == 10 || context == 11 || context == 24)
				{
					Main.hoverItemName = Lang.inter[9].Value;
				}
				if (context == 11)
				{
					Main.hoverItemName = Lang.inter[11].Value + " " + Main.hoverItemName;
				}
				if (context == 8 || context == 9 || context == 23 || context == 26)
				{
					if (slot == 0 || slot == 10 || context == 26)
					{
						Main.hoverItemName = Lang.inter[12].Value;
					}
					else if (slot == 1 || slot == 11)
					{
						Main.hoverItemName = Lang.inter[13].Value;
					}
					else if (slot == 2 || slot == 12)
					{
						Main.hoverItemName = Lang.inter[14].Value;
					}
					else if (slot >= 10)
					{
						Main.hoverItemName = Lang.inter[11].Value + " " + Main.hoverItemName;
					}
				}
				if (context == 12 || context == 25 || context == 27 || context == 33)
				{
					Main.hoverItemName = Lang.inter[57].Value;
				}
				if (context == 16)
				{
					Main.hoverItemName = Lang.inter[90].Value;
				}
				if (context == 17 || context == 39)
				{
					Main.hoverItemName = Lang.inter[91].Value;
				}
				if (context == 19)
				{
					Main.hoverItemName = Lang.inter[92].Value;
				}
				if (context == 18)
				{
					Main.hoverItemName = Lang.inter[93].Value;
				}
				if (context == 20)
				{
					Main.hoverItemName = Lang.inter[94].Value;
				}
				if (context == 38)
				{
					Main.hoverItemName = Language.GetTextValue("UI.DisplayDollWeapon");
				}
			}
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x004EF6D3 File Offset: 0x004ED8D3
		public static void ResetInventoryStateCounters()
		{
			ItemSlot.dyeSwapCounter = 0;
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x004EF6DB File Offset: 0x004ED8DB
		public static void SwapEquip(ref Item inv, int context = 0)
		{
			ItemSlot.singleSlotArray[0] = inv;
			ItemSlot.SwapEquip(ItemSlot.singleSlotArray, context, 0);
			inv = ItemSlot.singleSlotArray[0];
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x004EF6FC File Offset: 0x004ED8FC
		public static bool CanSwapEquip(Item item)
		{
			return !item.IsAir && (item.dye > 0 || Main.projHook[item.shoot] || item.mountType != -1 || (item.buffType > 0 && (Main.lightPet[item.buffType] || Main.vanityPet[item.buffType])) || item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1 || item.accessory);
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x004EF780 File Offset: 0x004ED980
		public static void SwapEquip(Item[] inv, int context, int slot)
		{
			Player player = Main.player[Main.myPlayer];
			if (inv[slot].IsAir)
			{
				return;
			}
			if (inv[slot].dye > 0)
			{
				bool flag;
				int toSlot;
				inv[slot] = ItemSlot.DyeSwap(inv[slot], out flag, out toSlot);
				if (flag)
				{
					Main.EquipPageSelected = 0;
					AchievementsHelper.HandleOnEquip(player, inv[slot], 12);
					ItemSlot.DisplayTransfer_TwoWay(inv, context, slot, player.dye, 12, toSlot);
				}
			}
			else if (Main.projHook[inv[slot].shoot])
			{
				bool flag;
				inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 4, out flag);
				if (flag)
				{
					Main.EquipPageSelected = 2;
					AchievementsHelper.HandleOnEquip(player, player.miscEquips[4], 16);
					ItemSlot.DisplayTransfer_TwoWay(inv, context, slot, player.miscEquips, 16, 4);
				}
			}
			else if (inv[slot].mountType != -1 && !MountID.Sets.Cart[inv[slot].mountType])
			{
				bool flag;
				inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 3, out flag);
				if (flag)
				{
					Main.EquipPageSelected = 2;
					AchievementsHelper.HandleOnEquip(player, inv[slot], 17);
					ItemSlot.DisplayTransfer_TwoWay(inv, context, slot, player.miscEquips, 17, 3);
				}
			}
			else if (inv[slot].mountType != -1 && MountID.Sets.Cart[inv[slot].mountType])
			{
				bool flag;
				inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 2, out flag);
				if (flag)
				{
					Main.EquipPageSelected = 2;
					ItemSlot.DisplayTransfer_TwoWay(inv, context, slot, player.miscEquips, 18, 2);
				}
			}
			else if (inv[slot].buffType > 0 && Main.lightPet[inv[slot].buffType])
			{
				bool flag;
				inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 1, out flag);
				if (flag)
				{
					Main.EquipPageSelected = 2;
					ItemSlot.DisplayTransfer_TwoWay(inv, context, slot, player.miscEquips, 20, 1);
				}
			}
			else if (inv[slot].buffType > 0 && Main.vanityPet[inv[slot].buffType])
			{
				bool flag;
				inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 0, out flag);
				if (flag)
				{
					Main.EquipPageSelected = 2;
					ItemSlot.DisplayTransfer_TwoWay(inv, context, slot, player.miscEquips, 19, 0);
				}
			}
			else
			{
				int num = inv[slot].accessory ? 10 : (inv[slot].vanity ? 9 : 8);
				bool flag;
				int num2;
				inv[slot] = ItemSlot.ArmorSwap(inv[slot], out flag, out num2);
				if (flag)
				{
					Main.EquipPageSelected = 0;
					Item item = player.armor[num2];
					AchievementsHelper.HandleOnEquip(player, item, num);
					ItemSlot.DisplayTransfer_TwoWay(inv, context, slot, player.armor, num, num2);
				}
			}
			if (context == 3 && Main.netMode == 1)
			{
				NetMessage.SendData(32, -1, -1, null, player.chest, (float)slot, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x004EFA10 File Offset: 0x004EDC10
		public static void DisplayTransfer_GetItem(Item[] arrayFrom, int fromContext, int fromSlot)
		{
			Vector2 position = UILinkPointNavigator.GetPosition(ItemSlot.GetGamepadPointForSlot(arrayFrom, fromContext, fromSlot));
			foreach (PlayerGetItemLogger.GetItemLoggerEntry getItemLoggerEntry in Player.GetItemLogger.Entries)
			{
				Vector2 position2 = UILinkPointNavigator.GetPosition(ItemSlot.GetGamepadPointForSlot(getItemLoggerEntry.TargetArray, getItemLoggerEntry.TargetItemSlotContext, getItemLoggerEntry.TargetSlot));
				int timeToAnimate = ItemSlot.GetTimeToAnimate(position, position2);
				Item item = getItemLoggerEntry.TargetArray[getItemLoggerEntry.TargetSlot];
				bool flag = ItemSlot.TryDisplayTransfer(ref position, ref position2, item, getItemLoggerEntry.Stack, timeToAnimate);
				bool flag2 = item.stack == getItemLoggerEntry.Stack;
				if (flag && flag2)
				{
					ItemSlot.AddCooldown(getItemLoggerEntry.TargetItemSlotContext, getItemLoggerEntry.TargetSlot, timeToAnimate);
				}
			}
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x004EFAE4 File Offset: 0x004EDCE4
		private static int GetTimeToAnimate(Vector2 startPosition, Vector2 endPosition)
		{
			int num = 15;
			int num2 = (int)Vector2.Distance(startPosition, endPosition) / 20;
			if (num2 > 15)
			{
				num2 = 15;
			}
			return num + num2;
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x004EFB0C File Offset: 0x004EDD0C
		public static void DisplayTransfer_OneWay(Item[] arrayFrom, int fromContext, int fromSlot, Item[] arrayTo, int toContext, int toSlot, int stackSize = 1)
		{
			int gamepadPointForSlot = ItemSlot.GetGamepadPointForSlot(arrayFrom, fromContext, fromSlot);
			int gamepadPointForSlot2 = ItemSlot.GetGamepadPointForSlot(arrayTo, toContext, toSlot);
			Vector2 position = UILinkPointNavigator.GetPosition(gamepadPointForSlot);
			Vector2 position2 = UILinkPointNavigator.GetPosition(gamepadPointForSlot2);
			int timeToAnimate = ItemSlot.GetTimeToAnimate(position, position2);
			bool flag = ItemSlot.TryDisplayTransfer(ref position, ref position2, arrayTo[toSlot], stackSize, timeToAnimate);
			bool flag2 = arrayTo[toSlot].stack == stackSize;
			if (flag && flag2)
			{
				ItemSlot.AddCooldown(toContext, toSlot, timeToAnimate);
			}
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x004EFB70 File Offset: 0x004EDD70
		public static void DisplayTransfer_TwoWay(Item[] arrayFrom, int fromContext, int fromSlot, Item toItem, int toContext)
		{
			int gamepadPointForSlot = ItemSlot.GetGamepadPointForSlot(arrayFrom, fromContext, fromSlot);
			int gamepadPointForSlot2 = ItemSlot.GetGamepadPointForSlot(ItemSlot._dirtyHack, toContext, 0);
			Vector2 position = UILinkPointNavigator.GetPosition(gamepadPointForSlot);
			Vector2 position2 = UILinkPointNavigator.GetPosition(gamepadPointForSlot2);
			int timeToAnimate = ItemSlot.GetTimeToAnimate(position, position2);
			if (ItemSlot.TryDisplayTransfer(ref position, ref position2, toItem, toItem.stack, timeToAnimate))
			{
				ItemSlot.AddCooldown(toContext, 0, timeToAnimate);
			}
			if (ItemSlot.TryDisplayTransfer(ref position2, ref position, arrayFrom[fromSlot], arrayFrom[fromSlot].stack, timeToAnimate))
			{
				ItemSlot.AddCooldown(fromContext, fromSlot, timeToAnimate);
			}
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x004EFBE4 File Offset: 0x004EDDE4
		public static void DisplayTransfer_TwoWay(Item[] arrayFrom, int fromContext, int fromSlot, Item[] arrayTo, int toContext, int toSlot)
		{
			int gamepadPointForSlot = ItemSlot.GetGamepadPointForSlot(arrayFrom, fromContext, fromSlot);
			int gamepadPointForSlot2 = ItemSlot.GetGamepadPointForSlot(arrayTo, toContext, toSlot);
			Vector2 position = UILinkPointNavigator.GetPosition(gamepadPointForSlot);
			Vector2 position2 = UILinkPointNavigator.GetPosition(gamepadPointForSlot2);
			int timeToAnimate = ItemSlot.GetTimeToAnimate(position, position2);
			if (ItemSlot.TryDisplayTransfer(ref position, ref position2, arrayTo[toSlot], arrayTo[toSlot].stack, timeToAnimate))
			{
				ItemSlot.AddCooldown(toContext, toSlot, timeToAnimate);
			}
			if (ItemSlot.TryDisplayTransfer(ref position2, ref position, arrayFrom[fromSlot], arrayFrom[fromSlot].stack, timeToAnimate))
			{
				ItemSlot.AddCooldown(fromContext, fromSlot, timeToAnimate);
			}
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x004EFC5C File Offset: 0x004EDE5C
		private static void AddCooldown(int context, int slot, int time)
		{
			ItemSlot._nextTickDrawAvailable[new ItemSlot.ItemDisplayKey
			{
				Slot = slot,
				Context = context
			}] = Main.EverLastingTicker + (ulong)((long)time);
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		private static bool TryDisplayTransfer(ref Vector2 pointPositionFrom, ref Vector2 pointPositionTo, Item toItem, int stackSize, int animationTime)
		{
			return false;
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x004EFC94 File Offset: 0x004EDE94
		public static bool CanEquipBothAccessories(Item acc1, Item acc2)
		{
			return acc1.type != acc2.type && (acc1.wingSlot <= 0 || acc2.wingSlot <= 0);
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x004EFCBC File Offset: 0x004EDEBC
		public static bool HasIncompatibleAccessory(Item newAcc, ArraySegment<Item> accessories, out int collisionSlot)
		{
			for (int i = 0; i < accessories.Count; i++)
			{
				if (!ItemSlot.CanEquipBothAccessories(accessories.Array[i + accessories.Offset], newAcc))
				{
					collisionSlot = i + accessories.Offset;
					return true;
				}
			}
			collisionSlot = -1;
			return false;
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x004EFD08 File Offset: 0x004EDF08
		public static bool HasSameItemInSlot(Item newItem, ArraySegment<Item> items)
		{
			if (newItem.IsAir)
			{
				return false;
			}
			for (int i = 0; i < items.Count; i++)
			{
				if (items.Array[i + items.Offset].type == newItem.type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x004EFD54 File Offset: 0x004EDF54
		public static bool CanEquipAccessoryInSlot(Item newAcc, ArraySegment<Item> accessories, int slot)
		{
			int num;
			return !ItemSlot.HasIncompatibleAccessory(newAcc, accessories, out num) || slot == num;
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x004EFD74 File Offset: 0x004EDF74
		private static Item DyeSwap(Item item, out bool success, out int targetSlot)
		{
			targetSlot = -1;
			success = false;
			if (item.dye <= 0)
			{
				return item;
			}
			Player player = Main.player[Main.myPlayer];
			while (!player.IsItemSlotUnlockedAndUsable(ItemSlot.dyeSwapCounter))
			{
				ItemSlot.dyeSwapCounter = (ItemSlot.dyeSwapCounter + 1) % 10;
			}
			for (int i = 0; i < 10; i++)
			{
				if (player.IsItemSlotUnlockedAndUsable(i) && player.dye[i].IsAir)
				{
					ItemSlot.dyeSwapCounter = i;
					break;
				}
			}
			SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
			Utils.Swap<Item>(ref item, ref player.dye[ItemSlot.dyeSwapCounter]);
			targetSlot = ItemSlot.dyeSwapCounter;
			ItemSlot.dyeSwapCounter = (ItemSlot.dyeSwapCounter + 1) % 10;
			success = true;
			return item;
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x004EFE30 File Offset: 0x004EE030
		private static Item ArmorSwap(Item item, out bool success, out int targetSlot)
		{
			targetSlot = -1;
			success = false;
			if (item.stack < 1)
			{
				return item;
			}
			if (item.headSlot == -1 && item.bodySlot == -1 && item.legSlot == -1 && !item.accessory)
			{
				return item;
			}
			Player player = Main.player[Main.myPlayer];
			int num = item.vanity ? 10 : 0;
			if (item.headSlot != -1 || item.bodySlot != -1 || item.legSlot != -1)
			{
				if (ItemSlot.HasSameItemInSlot(item, new ArraySegment<Item>(Main.LocalPlayer.armor, 0, 3)) || ItemSlot.HasSameItemInSlot(item, new ArraySegment<Item>(Main.LocalPlayer.armor, 10, 3)))
				{
					return item;
				}
			}
			else if (item.accessory && (item.vanity ? ItemSlot.HasSameItemInSlot(item, new ArraySegment<Item>(player.armor, 3, 7)) : ItemSlot.HasSameItemInSlot(item, new ArraySegment<Item>(player.armor, 13, 7))))
			{
				return item;
			}
			if (item.headSlot != -1)
			{
				targetSlot = num;
			}
			else if (item.bodySlot != -1)
			{
				targetSlot = num + 1;
			}
			else if (item.legSlot != -1)
			{
				targetSlot = num + 2;
			}
			else if (item.accessory)
			{
				ArraySegment<Item> accessories = new ArraySegment<Item>(player.armor, 3 + num, 7);
				if (ItemSlot.HasIncompatibleAccessory(item, accessories, out targetSlot))
				{
					if (!player.IsItemSlotUnlockedAndUsable(targetSlot))
					{
						return item;
					}
				}
				else
				{
					targetSlot = accessories.Offset;
					for (int i = 0; i < accessories.Count; i++)
					{
						int num2 = i + accessories.Offset;
						if (player.IsItemSlotUnlockedAndUsable(num2) && accessories.Array[num2].IsAir)
						{
							targetSlot = num2;
							break;
						}
					}
				}
			}
			if (targetSlot == -1)
			{
				return item;
			}
			item.favorited = false;
			SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
			Utils.Swap<Item>(ref item, ref player.armor[targetSlot]);
			success = true;
			return item;
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x004EFFFE File Offset: 0x004EE1FE
		private static Item EquipSwap(Item item, Item[] inv, int slot, out bool success)
		{
			success = false;
			Player player = Main.player[Main.myPlayer];
			item.favorited = false;
			Item result = inv[slot].Clone();
			inv[slot] = item.Clone();
			SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
			success = true;
			return result;
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x004F0040 File Offset: 0x004EE240
		public static void DrawMoney(SpriteBatch sb, string text, float shopx, float shopy, int[] coinsArray, bool horizontal = false, bool fromSavings = false)
		{
			Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, text, shopx, shopy + 40f, Color.White * ((float)Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, 1f);
			CoinSlot.CoinDrawState coinDrawState = default(CoinSlot.CoinDrawState);
			coinDrawState.coinAnimFrame = 0;
			coinDrawState.coinYOffset = 0f;
			coinDrawState.stackTextScale = 1f;
			if (horizontal)
			{
				for (int i = 0; i < 4; i++)
				{
					Main.instance.LoadItem(74 - i);
					if (i == 0)
					{
						int num = coinsArray[3 - i];
					}
					int num2 = coinsArray[3 - i];
					if (num2 > 999)
					{
						num2 = 999;
					}
					if (fromSavings)
					{
						CoinSlot.UpdateSavings(i, num2, out coinDrawState);
					}
					Vector2 vector = new Vector2(shopx + ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One, -1f).X + (float)(24 * i) + 45f, shopy + 50f);
					CoinSlot.DrawItemCoin(sb, vector + new Vector2(0f, -coinDrawState.coinYOffset), 74 - i, coinDrawState.coinAnimFrame, 1f, 1024f, Color.White, 1f);
					Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.Value, num2.ToString(), vector.X - 11f, vector.Y + (float)FontAssets.ItemStack.Value.LineSpacing * 0.75f, Color.White, Color.Black, new Vector2(0f, (float)FontAssets.ItemStack.Value.LineSpacing), 0.75f * coinDrawState.stackTextScale);
				}
				return;
			}
			for (int j = 0; j < 4; j++)
			{
				Main.instance.LoadItem(74 - j);
				int num3 = (j == 0 && coinsArray[3 - j] > 99) ? -6 : 0;
				int num4 = coinsArray[3 - j];
				if (num4 > 999)
				{
					num4 = 999;
				}
				if (fromSavings)
				{
					CoinSlot.UpdateSavings(j, num4, out coinDrawState);
				}
				CoinSlot.DrawItemCoin(sb, new Vector2(shopx + 11f + (float)(24 * j), shopy + 75f - coinDrawState.coinYOffset), 74 - j, coinDrawState.coinAnimFrame, 1f, 1024f, Color.White, 1f);
				Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.Value, num4.ToString(), shopx + (float)(24 * j) + (float)num3, shopy + 75f + (float)FontAssets.ItemStack.Value.LineSpacing * 0.75f, Color.White, Color.Black, new Vector2(0f, (float)FontAssets.ItemStack.Value.LineSpacing), 0.75f * coinDrawState.stackTextScale);
			}
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x004F030C File Offset: 0x004EE50C
		public static void DrawSavings(SpriteBatch sb, float shopx, float shopy, bool horizontal = false)
		{
			Player player = Main.player[Main.myPlayer];
			if (ItemSlot._customCurrencyForSavings != -1)
			{
				CustomCurrencyManager.DrawSavings(sb, ItemSlot._customCurrencyForSavings, shopx, shopy, horizontal);
				return;
			}
			bool flag;
			long num = Utils.CoinsCount(out flag, player.bank.item, new int[0]);
			long num2 = Utils.CoinsCount(out flag, player.bank2.item, new int[0]);
			long num3 = Utils.CoinsCount(out flag, player.bank3.item, new int[0]);
			long num4 = Utils.CoinsCount(out flag, player.bank4.item, new int[0]);
			long num5 = Utils.CoinsCombineStacks(out flag, new long[]
			{
				num,
				num2,
				num3,
				num4
			});
			if (num5 > 0L)
			{
				Texture2D texture;
				Rectangle r;
				Main.GetItemDrawFrame(4076, out texture, out r);
				Texture2D texture2;
				Rectangle r2;
				Main.GetItemDrawFrame(3813, out texture2, out r2);
				Texture2D texture2D;
				Rectangle rectangle;
				Main.GetItemDrawFrame(346, out texture2D, out rectangle);
				Texture2D texture2D2;
				Rectangle rectangle2;
				Main.GetItemDrawFrame(87, out texture2D2, out rectangle2);
				if (num4 > 0L)
				{
					sb.Draw(texture, Utils.CenteredRectangle(new Vector2(shopx + 70f, shopy + 45f), r.Size() * 0.65f), null, Color.White);
				}
				if (num3 > 0L)
				{
					sb.Draw(texture2, Utils.CenteredRectangle(new Vector2(shopx + 92f, shopy + 45f), r2.Size() * 0.65f), null, Color.White);
				}
				if (num2 > 0L)
				{
					sb.Draw(texture2D, Utils.CenteredRectangle(new Vector2(shopx + 80f, shopy + 50f), texture2D.Size() * 0.65f), null, Color.White);
				}
				if (num > 0L)
				{
					sb.Draw(texture2D2, Utils.CenteredRectangle(new Vector2(shopx + 70f, shopy + 60f), texture2D2.Size() * 0.65f), null, Color.White);
				}
				ItemSlot.DrawMoney(sb, Lang.inter[66].Value, shopx, shopy, Utils.CoinsSplit(num5), horizontal, true);
			}
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x004F0538 File Offset: 0x004EE738
		public static void GetItemLight(ref Color currentColor, Item item, bool outInTheWorld = false, float lightScalar = 1f)
		{
			float num = 1f;
			ItemSlot.GetItemLight(ref currentColor, ref num, item, outInTheWorld, lightScalar);
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x004F0558 File Offset: 0x004EE758
		public static void GetItemLight(ref Color currentColor, int type, bool outInTheWorld = false, float lightScalar = 1f)
		{
			float num = 1f;
			ItemSlot.GetItemLight(ref currentColor, ref num, type, outInTheWorld, lightScalar);
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x004F0577 File Offset: 0x004EE777
		public static void GetItemLight(ref Color currentColor, ref float scale, Item item, bool outInTheWorld = false, float lightScalar = 1f)
		{
			ItemSlot.GetItemLight(ref currentColor, ref scale, item.type, outInTheWorld, lightScalar);
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x004F058C File Offset: 0x004EE78C
		public static Color GetItemLight(ref Color currentColor, ref float scale, int type, bool outInTheWorld = false, float lightScalar = 1f)
		{
			if (type < 0 || type > (int)ItemID.Count)
			{
				return currentColor;
			}
			if (type == 662 || type == 663 || type == 5444 || type == 5450 || type == 5643)
			{
				currentColor.R = (byte)Main.DiscoR;
				currentColor.G = (byte)Main.DiscoG;
				currentColor.B = (byte)Main.DiscoB;
				currentColor.A = byte.MaxValue;
				currentColor *= lightScalar;
			}
			if (type == 5128)
			{
				currentColor.R = (byte)Main.DiscoR;
				currentColor.G = (byte)Main.DiscoG;
				currentColor.B = (byte)Main.DiscoB;
				currentColor.A = byte.MaxValue;
				currentColor *= lightScalar;
			}
			else if (ItemID.Sets.ItemIconPulse[type])
			{
				scale = Main.essScale;
				currentColor.R = (byte)((float)currentColor.R * scale);
				currentColor.G = (byte)((float)currentColor.G * scale);
				currentColor.B = (byte)((float)currentColor.B * scale);
				currentColor.A = (byte)((float)currentColor.A * scale);
				currentColor *= lightScalar;
			}
			else if (type == 58 || type == 184 || type == 4143)
			{
				scale = Main.essScale * 0.25f + 0.75f;
				currentColor.R = (byte)((float)currentColor.R * scale);
				currentColor.G = (byte)((float)currentColor.G * scale);
				currentColor.B = (byte)((float)currentColor.B * scale);
				currentColor.A = (byte)((float)currentColor.A * scale);
				currentColor *= lightScalar;
			}
			return currentColor;
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x004F0754 File Offset: 0x004EE954
		public static void DrawRadialCircular(SpriteBatch sb, Vector2 position, Player.SelectionRadial radial, Item[] items)
		{
			ItemSlot.CircularRadialOpacity = MathHelper.Clamp(ItemSlot.CircularRadialOpacity + ((PlayerInput.UsingGamepad && PlayerInput.Triggers.Current.RadialHotbar) ? 0.25f : -0.15f), 0f, 1f);
			if (ItemSlot.CircularRadialOpacity == 0f)
			{
				return;
			}
			Texture2D value = TextureAssets.HotbarRadial[2].Value;
			float scale = ItemSlot.CircularRadialOpacity * 0.9f;
			float num = ItemSlot.CircularRadialOpacity * 1f;
			float num2 = (float)Main.mouseTextColor / 255f;
			float num3 = 1f - (1f - num2) * (1f - num2);
			num3 *= 0.785f;
			Color value2 = Color.White * num3 * scale;
			value = TextureAssets.HotbarRadial[1].Value;
			float num4 = 6.2831855f / (float)radial.RadialCount;
			float num5 = -1.5707964f;
			for (int i = 0; i < radial.RadialCount; i++)
			{
				int num6 = radial.Bindings[i];
				Vector2 value3 = new Vector2(150f, 0f).RotatedBy((double)(num5 + num4 * (float)i), default(Vector2)) * num;
				float num7 = 0.85f;
				if (radial.SelectedBinding == i)
				{
					num7 = 1.7f;
				}
				sb.Draw(value, position + value3, null, value2 * num7, 0f, value.Size() / 2f, num * num7, SpriteEffects.None, 0f);
				if (num6 != -1)
				{
					float inventoryScale = Main.inventoryScale;
					Main.inventoryScale = num * num7;
					ItemSlot.Draw(sb, items, 14, num6, position + value3 + new Vector2(-26f * num * num7), Color.White);
					Main.inventoryScale = inventoryScale;
				}
			}
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x004F092C File Offset: 0x004EEB2C
		public static void DrawRadialQuicks(SpriteBatch sb, Vector2 position)
		{
			ItemSlot.QuicksRadialOpacity = MathHelper.Clamp(ItemSlot.QuicksRadialOpacity + ((PlayerInput.UsingGamepad && PlayerInput.Triggers.Current.RadialQuickbar) ? 0.25f : -0.15f), 0f, 1f);
			if (ItemSlot.QuicksRadialOpacity == 0f)
			{
				return;
			}
			Player player = Main.player[Main.myPlayer];
			Texture2D value = TextureAssets.HotbarRadial[2].Value;
			Texture2D value2 = TextureAssets.QuicksIcon.Value;
			float scale = ItemSlot.QuicksRadialOpacity * 0.9f;
			float num = ItemSlot.QuicksRadialOpacity * 1f;
			float num2 = (float)Main.mouseTextColor / 255f;
			float num3 = 1f - (1f - num2) * (1f - num2);
			num3 *= 0.785f;
			Color value3 = Color.White * num3 * scale;
			float num4 = 6.2831855f / (float)player.QuicksRadial.RadialCount;
			float num5 = -1.5707964f;
			Item item = player.QuickHeal_GetItemToUse();
			Item item2 = player.QuickMana_GetItemToUse();
			Item item3 = null;
			Item item4 = null;
			if (item == null)
			{
				item = new Item();
				item.SetDefaults(28, null);
			}
			if (item2 == null)
			{
				item2 = new Item();
				item2.SetDefaults(110, null);
			}
			if (item3 == null)
			{
				item3 = new Item();
				item3.SetDefaults(292, null);
			}
			if (item4 == null)
			{
				item4 = new Item();
				item4.SetDefaults(2428, null);
			}
			for (int i = 0; i < player.QuicksRadial.RadialCount; i++)
			{
				Item item5 = item4;
				if (i == 1)
				{
					item5 = item;
				}
				if (i == 2)
				{
					item5 = item3;
				}
				if (i == 3)
				{
					item5 = item2;
				}
				int num6 = player.QuicksRadial.Bindings[i];
				Vector2 value4 = new Vector2(120f, 0f).RotatedBy((double)(num5 + num4 * (float)i), default(Vector2)) * num;
				float num7 = 0.85f;
				if (player.QuicksRadial.SelectedBinding == i)
				{
					num7 = 1.7f;
				}
				sb.Draw(value, position + value4, null, value3 * num7, 0f, value.Size() / 2f, num * num7 * 1.3f, SpriteEffects.None, 0f);
				float inventoryScale = Main.inventoryScale;
				Main.inventoryScale = num * num7;
				ItemSlot.Draw(sb, ref item5, 14, position + value4 + new Vector2(-26f * num * num7), Color.White);
				Main.inventoryScale = inventoryScale;
				sb.Draw(value2, position + value4 + new Vector2(34f, 20f) * 0.85f * num * num7, null, value3 * num7, 0f, value.Size() / 2f, num * num7 * 1.3f, SpriteEffects.None, 0f);
			}
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x004F0C2C File Offset: 0x004EEE2C
		public static void DrawRadialDpad(SpriteBatch sb, Vector2 position)
		{
			if (!PlayerInput.UsingGamepad || !PlayerInput.CurrentProfile.UsingDpadHotbar())
			{
				return;
			}
			Player player = Main.player[Main.myPlayer];
			if (player.chest != -1)
			{
				return;
			}
			Texture2D value = TextureAssets.HotbarRadial[0].Value;
			float num = (float)Main.mouseTextColor / 255f;
			float num2 = 1f - (1f - num) * (1f - num);
			num2 *= 0.785f;
			Color color = Color.White * num2;
			sb.Draw(value, position, null, color, 0f, value.Size() / 2f, Main.inventoryScale, SpriteEffects.None, 0f);
			for (int i = 0; i < 4; i++)
			{
				int num3 = player.DpadRadial.Bindings[i];
				if (num3 != -1)
				{
					ItemSlot.Draw(sb, player.inventory, 14, num3, position + new Vector2((float)(value.Width / 3), 0f).RotatedBy((double)(-1.5707964f + 1.5707964f * (float)i), default(Vector2)) + new Vector2(-26f * Main.inventoryScale), Color.White);
				}
			}
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x004F0D66 File Offset: 0x004EEF66
		public static string GetGamepadInstructions(int context = 0)
		{
			return ItemSlot.GetGamepadInstructions(ItemSlot.singleSlotArray, context, 0);
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x004F0D74 File Offset: 0x004EEF74
		public static string GetGamepadInstructions(ref Item inv, int context = 0)
		{
			ItemSlot.singleSlotArray[0] = inv;
			string gamepadInstructions = ItemSlot.GetGamepadInstructions(ItemSlot.singleSlotArray, context, 0);
			inv = ItemSlot.singleSlotArray[0];
			return gamepadInstructions;
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x004CC333 File Offset: 0x004CA533
		public static bool CanExecuteCommand()
		{
			return PlayerInput.AllowExecutionOfGamepadInstructions;
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x004F0D94 File Offset: 0x004EEF94
		public static string GetGamepadInstructions(Item[] inv, int context = 0, int slot = 0)
		{
			Player player = Main.player[Main.myPlayer];
			string text = "";
			if (inv == null || inv[slot] == null || Main.mouseItem == null)
			{
				return text;
			}
			if (context == 0 || context == 1 || context == 2)
			{
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						text += PlayerInput.BuildCommand(Lang.misc[65].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
						});
						if (inv[slot].type == Main.mouseItem.type && Main.mouseItem.stack < inv[slot].maxStack && inv[slot].maxStack > 1)
						{
							text += PlayerInput.BuildCommand(Lang.misc[55].Value, new List<string>[]
							{
								PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]
							});
						}
					}
					else
					{
						if (context == 0 && player.chest == -1 && PlayerInput.AllowExecutionOfGamepadInstructions)
						{
							player.DpadRadial.ChangeBinding(slot);
						}
						text += PlayerInput.BuildCommand(Lang.misc[54].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
						});
						if (inv[slot].maxStack > 1)
						{
							text += PlayerInput.BuildCommand(Lang.misc[55].Value, new List<string>[]
							{
								PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]
							});
						}
					}
					if (ItemID.Sets.OpenableBag[inv[slot].type])
					{
						text += PlayerInput.BuildCommand(Language.GetTextValue("UI.ActionOpen"), new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
						});
						if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							ItemSlot.TryOpenContainer(inv, context, slot, player);
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
					else if (ItemID.Sets.HasItemSwap[inv[slot].type])
					{
						text += PlayerInput.BuildCommand(Language.GetTextValue("UI.ActionChangeType"), new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
						});
						if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							ItemSlot.TryItemSwap(inv[slot]);
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
					else if (inv[slot].stack == 1 && inv[slot].CanBeEquipped())
					{
						text += PlayerInput.BuildCommand(Lang.misc[67].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
						});
						if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							ItemSlot.SwapEquip(inv, context, slot);
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
					text += PlayerInput.BuildCommand(Lang.misc[83].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]
					});
					if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
					{
						inv[slot].favorited = !inv[slot].favorited;
						PlayerInput.LockGamepadButtons("SmartCursor");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				else if (Main.mouseItem.type > 0)
				{
					text += PlayerInput.BuildCommand(Lang.misc[65].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
					});
				}
			}
			if (context == 3 || context == 4 || context == 32)
			{
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						text += PlayerInput.BuildCommand(Lang.misc[65].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
						});
						if (inv[slot].type == Main.mouseItem.type && Main.mouseItem.stack < inv[slot].maxStack && inv[slot].maxStack > 1)
						{
							text += PlayerInput.BuildCommand(Lang.misc[55].Value, new List<string>[]
							{
								PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]
							});
						}
					}
					else
					{
						text += PlayerInput.BuildCommand(Lang.misc[54].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
						});
						if (inv[slot].maxStack > 1)
						{
							text += PlayerInput.BuildCommand(Lang.misc[55].Value, new List<string>[]
							{
								PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]
							});
						}
					}
					if (inv[slot].stack == 1 && inv[slot].CanBeEquipped())
					{
						text += PlayerInput.BuildCommand(Lang.misc[67].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
						});
						if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							ItemSlot.SwapEquip(inv, context, slot);
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
					if (context == 32)
					{
						text += PlayerInput.BuildCommand(Lang.misc[83].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]
						});
						if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
						{
							inv[slot].favorited = !inv[slot].favorited;
							PlayerInput.LockGamepadButtons("SmartCursor");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
				}
				else if (Main.mouseItem.type > 0)
				{
					text += PlayerInput.BuildCommand(Lang.misc[65].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
					});
				}
			}
			if (context == 15)
			{
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						if (inv[slot].type == Main.mouseItem.type && Main.mouseItem.stack < inv[slot].maxStack && inv[slot].maxStack > 1)
						{
							text += PlayerInput.BuildCommand(Lang.misc[91].Value, new List<string>[]
							{
								PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]
							});
						}
					}
					else
					{
						text += PlayerInput.BuildCommand(Lang.misc[90].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"],
							PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]
						});
					}
				}
				else if (Main.mouseItem.type > 0)
				{
					text += PlayerInput.BuildCommand(Lang.misc[92].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
					});
				}
			}
			if (context == 8 || context == 9 || context == 10 || context == 11 || context == 16 || context == 17 || context == 19 || context == 20)
			{
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						if (Main.mouseItem.stack == 1 && Main.mouseItem.CanBeEquipped())
						{
							text += PlayerInput.BuildCommand(Lang.misc[65].Value, new List<string>[]
							{
								PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
							});
						}
					}
					else
					{
						text += PlayerInput.BuildCommand(Lang.misc[54].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
						});
					}
					if (context == 10)
					{
						bool flag = player.hideVisibleAccessory[slot];
						text += PlayerInput.BuildCommand(Lang.misc[flag ? 77 : 78].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
						});
						if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							player.hideVisibleAccessory[slot] = !player.hideVisibleAccessory[slot];
							SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
							if (Main.netMode == 1)
							{
								NetMessage.SendData(4, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
							}
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
					if ((context == 16 || context == 17 || context == 18 || context == 19 || context == 20) && slot < 2)
					{
						bool flag2 = player.hideMisc[slot];
						text += PlayerInput.BuildCommand(Lang.misc[flag2 ? 77 : 78].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
						});
						if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							if (slot == 0)
							{
								player.TogglePet();
							}
							if (slot == 1)
							{
								player.ToggleLight();
							}
							SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
							if (Main.netMode == 1)
							{
								NetMessage.SendData(4, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
							}
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
				}
				else
				{
					if (Main.mouseItem.type > 0 && Main.mouseItem.CanBeEquipped())
					{
						text += PlayerInput.BuildCommand(Lang.misc[65].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
						});
					}
					if (context == 10)
					{
						bool flag3 = player.hideVisibleAccessory[slot];
						text += PlayerInput.BuildCommand(Lang.misc[flag3 ? 77 : 78].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
						});
						if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							player.hideVisibleAccessory[slot] = !player.hideVisibleAccessory[slot];
							SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
							if (Main.netMode == 1)
							{
								NetMessage.SendData(4, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
							}
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
					if ((context == 16 || context == 17 || context == 18 || context == 19 || context == 20) && slot < 2)
					{
						bool flag4 = player.hideMisc[slot];
						text += PlayerInput.BuildCommand(Lang.misc[flag4 ? 77 : 78].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
						});
						if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							if (slot == 0)
							{
								player.TogglePet();
							}
							if (slot == 1)
							{
								player.ToggleLight();
							}
							Main.mouseLeftRelease = false;
							SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
							if (Main.netMode == 1)
							{
								NetMessage.SendData(4, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
							}
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
				}
			}
			if (context == 12 || context == 25 || context == 27 || context == 33)
			{
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						if (Main.mouseItem.dye > 0)
						{
							text += PlayerInput.BuildCommand(Lang.misc[65].Value, new List<string>[]
							{
								PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
							});
						}
					}
					else
					{
						text += PlayerInput.BuildCommand(Lang.misc[54].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
						});
					}
					if (context == 12 || context == 25 || context == 27 || context == 33)
					{
						int num = -1;
						if (inv == player.dye)
						{
							num = slot;
						}
						if (inv == player.miscDyes)
						{
							num = 10 + slot;
						}
						if (num != -1)
						{
							if (num < 10)
							{
								bool flag5 = player.hideVisibleAccessory[slot];
								text += PlayerInput.BuildCommand(Lang.misc[flag5 ? 77 : 78].Value, new List<string>[]
								{
									PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
								});
								if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
								{
									player.hideVisibleAccessory[slot] = !player.hideVisibleAccessory[slot];
									SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
									if (Main.netMode == 1)
									{
										NetMessage.SendData(4, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
									}
									PlayerInput.LockGamepadButtons("Grapple");
									PlayerInput.SettingsForUI.TryRevertingToMouseMode();
								}
							}
							else
							{
								bool flag6 = player.hideMisc[slot];
								text += PlayerInput.BuildCommand(Lang.misc[flag6 ? 77 : 78].Value, new List<string>[]
								{
									PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
								});
								if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
								{
									player.hideMisc[slot] = !player.hideMisc[slot];
									SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
									if (Main.netMode == 1)
									{
										NetMessage.SendData(4, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
									}
									PlayerInput.LockGamepadButtons("Grapple");
									PlayerInput.SettingsForUI.TryRevertingToMouseMode();
								}
							}
						}
					}
				}
				else if (Main.mouseItem.type > 0 && Main.mouseItem.dye > 0)
				{
					text += PlayerInput.BuildCommand(Lang.misc[65].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
					});
				}
				return text;
			}
			if (context == 18)
			{
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						if (Main.mouseItem.dye > 0)
						{
							text += PlayerInput.BuildCommand(Lang.misc[65].Value, new List<string>[]
							{
								PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
							});
						}
					}
					else
					{
						text += PlayerInput.BuildCommand(Lang.misc[54].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
						});
					}
				}
				else if (Main.mouseItem.type > 0 && Main.mouseItem.dye > 0)
				{
					text += PlayerInput.BuildCommand(Lang.misc[65].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
					});
				}
				bool enabledSuperCart = player.enabledSuperCart;
				text += PlayerInput.BuildCommand(Language.GetTextValue((!enabledSuperCart) ? "UI.EnableSuperCart" : "UI.DisableSuperCart"), new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
				});
				if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
				{
					player.enabledSuperCart = !player.enabledSuperCart;
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					if (Main.netMode == 1)
					{
						NetMessage.SendData(4, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
					}
					PlayerInput.LockGamepadButtons("Grapple");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
				return text;
			}
			if (context == 6)
			{
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						text += PlayerInput.BuildCommand(Lang.misc[74].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
						});
					}
					else
					{
						text += PlayerInput.BuildCommand(Lang.misc[54].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
						});
					}
				}
				else if (Main.mouseItem.type > 0)
				{
					text += PlayerInput.BuildCommand(Lang.misc[74].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
					});
				}
				return text;
			}
			if (context == 5 || context == 7)
			{
				bool flag7 = false;
				if (context == 5)
				{
					flag7 = (Main.mouseItem.CanHavePrefixes() || Main.mouseItem.type == 0);
				}
				if (context == 7)
				{
					flag7 = Main.mouseItem.material;
				}
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						if (flag7)
						{
							text += PlayerInput.BuildCommand(Lang.misc[65].Value, new List<string>[]
							{
								PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
							});
						}
					}
					else
					{
						text += PlayerInput.BuildCommand(Lang.misc[54].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
						});
					}
				}
				else if (Main.mouseItem.type > 0 && flag7)
				{
					text += PlayerInput.BuildCommand(Lang.misc[65].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
					});
				}
			}
			if (!Main.mouseItem.IsAir)
			{
				if (ItemSlot.canQuickDropAt[context])
				{
					text += PlayerInput.BuildCommand(Lang.inter[121].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]
					});
					if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
					{
						player.DropSelectedItem();
						PlayerInput.LockGamepadButtons("SmartSelect");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				else if (player.ItemSpace(Main.mouseItem).CanTakeItemToPersonalInventory)
				{
					text += PlayerInput.BuildCommand(Lang.misc[76].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]
					});
					if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
					{
						Main.mouseItem = player.GetItem(Main.mouseItem, GetItemSettings.ReturnItemShowAsNew);
						PlayerInput.LockGamepadButtons("SmartSelect");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
			}
			else if (context == 22 || context == 42 || context == 43 || context == 41)
			{
				bool flag8 = Player.Settings.CraftingGridControl == Player.Settings.CraftingGridMode.Classic;
				text += PlayerInput.BuildCommand(Language.GetTextValue(CraftingUI.CraftingWindowTextKey), new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]
				});
				if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
				{
					if (flag8)
					{
						NewCraftingUI.Close(true, true);
						if (UILinkPointNavigator.CurrentPage == 10)
						{
							UILinkPointNavigator.ChangePage(9);
						}
						else
						{
							UILinkPointNavigator.ChangePage(10);
						}
					}
					else
					{
						NewCraftingUI.ToggleInInventory(false);
					}
				}
				if (!Main.InGuideCraftMenu && Main.bannerUI.AnyAvailableBanners && context == 22)
				{
					text += PlayerInput.BuildCommand(Language.GetTextValue("UI.CyclePipsToBanners"), new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]
					});
					if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
					{
						Main.TryChangePipsPage(Main.PipPage.Banners);
						UILinkPointNavigator.ChangePage(22);
						PlayerInput.LockGamepadButtons("SmartSelect");
					}
				}
			}
			else if (context == 35)
			{
				text += PlayerInput.BuildCommand(Language.GetTextValue("GameUI.BannersWindow"), new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]
				});
				if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
				{
					if (UILinkPointNavigator.CurrentPage == 23)
					{
						UILinkPointNavigator.ChangePage(22);
					}
					else
					{
						UILinkPointNavigator.ChangePage(23);
					}
				}
				text += PlayerInput.BuildCommand(Language.GetTextValue("UI.CyclePipsToCrafting"), new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]
				});
				if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
				{
					Main.TryChangePipsPage(Main.PipPage.Recipes);
					UILinkPointNavigator.ChangePage(9);
					PlayerInput.LockGamepadButtons("SmartSelect");
				}
			}
			else
			{
				ItemSlot.ShiftForcedOn = true;
				ItemSlot.AlternateClickAction? alternateClickAction = ItemSlot.GetAlternateClickAction(inv, context, slot);
				if (alternateClickAction != null)
				{
					text += PlayerInput.BuildCommand(alternateClickAction.Value.gamepadHintText.Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]
					});
					if (ItemSlot.CanDoSimulatedClickAction() && ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
					{
						bool mouseLeft = Main.mouseLeft;
						int cursorOverride = Main.cursorOverride;
						Main.mouseLeft = true;
						Main.cursorOverride = alternateClickAction.Value.cursorOverride;
						ItemSlot.LeftClick(inv, context, slot);
						Main.cursorOverride = cursorOverride;
						Main.mouseLeft = mouseLeft;
						PlayerInput.LockGamepadButtons("SmartSelect");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				ItemSlot.ShiftForcedOn = false;
			}
			if (!ItemSlot.TryEnteringFastUseMode(inv, context, slot, player, ref text))
			{
				ItemSlot.TryEnteringBuildingMode(inv, context, slot, player, ref text);
			}
			return text;
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x004F247B File Offset: 0x004F067B
		private static bool CanDoSimulatedClickAction()
		{
			return !PlayerInput.SteamDeckIsUsed || UILinkPointNavigator.InUse;
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x004F248C File Offset: 0x004F068C
		private static bool TryEnteringFastUseMode(Item[] inv, int context, int slot, Player player, ref string s)
		{
			int num = 0;
			if (Main.mouseItem.CanBeQuickUsed)
			{
				num = 1;
			}
			if (num == 0 && Main.mouseItem.stack <= 0 && context == 0 && inv[slot].CanBeQuickUsed)
			{
				num = 2;
			}
			if (num > 0)
			{
				s += PlayerInput.BuildCommand(Language.GetTextValue("UI.QuickUseItem"), new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]
				});
				if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.QuickMount)
				{
					if (num == 1)
					{
						PlayerInput.TryEnteringFastUseModeForMouseItem();
					}
					else if (num == 2)
					{
						PlayerInput.TryEnteringFastUseModeForInventorySlot(slot);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x004F2534 File Offset: 0x004F0734
		private static bool TryEnteringBuildingMode(Item[] inv, int context, int slot, Player player, ref string s)
		{
			int num = 0;
			if (ItemSlot.IsABuildingItem(Main.mouseItem))
			{
				num = 1;
			}
			if (num == 0 && Main.mouseItem.stack <= 0 && context == 0 && ItemSlot.IsABuildingItem(inv[slot]))
			{
				num = 2;
			}
			if (num > 0)
			{
				Item item = Main.mouseItem;
				if (num == 1)
				{
					item = Main.mouseItem;
				}
				if (num == 2)
				{
					item = inv[slot];
				}
				if (num != 1 || player.ItemSpace(item).CanTakeItemToPersonalInventory)
				{
					if (item.damage > 0 && item.ammo == 0)
					{
						s += PlayerInput.BuildCommand(Lang.misc[60].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]
						});
					}
					else if (item.createTile >= 0 || item.createWall > 0)
					{
						s += PlayerInput.BuildCommand(Lang.misc[61].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]
						});
					}
					else
					{
						s += PlayerInput.BuildCommand(Lang.misc[63].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]
						});
					}
					if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.QuickMount)
					{
						PlayerInput.EnterBuildingMode();
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x004F26A0 File Offset: 0x004F08A0
		public static string GetQuickCraftGamepadInstructions(Recipe recipe)
		{
			Player localPlayer = Main.LocalPlayer;
			if (!Main.mouseItem.IsAir || !localPlayer.ItemSpace(recipe.createItem).CanTakeItemToPersonalInventory || localPlayer.HasLockedInventory())
			{
				return null;
			}
			if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.Current.Grapple && (Main.stackSplit <= 1 || PlayerInput.Triggers.JustPressed.Grapple))
			{
				if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
				{
					UILinksInitializer.SomeVarsForUILinkers.SequencedCraftingCurrent = Main.recipe[Main.availableRecipe[Main.focusRecipe]];
				}
				ItemSlot.RefreshStackSplitCooldown();
				Main.quickCraftStackSplit = true;
				if (UILinksInitializer.SomeVarsForUILinkers.SequencedCraftingCurrent == Main.recipe[Main.availableRecipe[Main.focusRecipe]])
				{
					CraftingRequests.CraftItem(recipe, 1, true);
				}
			}
			return PlayerInput.BuildCommand(Lang.misc[71].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
			});
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x004F279C File Offset: 0x004F099C
		public static string GetCraftSlotGamepadInstructions()
		{
			if (Main.InGuideCraftMenu)
			{
				return "";
			}
			string text = "";
			Player localPlayer = Main.LocalPlayer;
			Recipe recipe = Main.recipe[Main.availableRecipe[Main.focusRecipe]];
			string quickCraftGamepadInstructions = ItemSlot.GetQuickCraftGamepadInstructions(recipe);
			if (quickCraftGamepadInstructions == null && Main.mouseItem.stack == 1 && Main.mouseItem.CanBeEquipped())
			{
				text += PlayerInput.BuildCommand(Lang.misc[67].Value, new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
				});
				if (ItemSlot.CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
				{
					ItemSlot.SwapEquip(ref Main.mouseItem, 0);
					if (Main.player[Main.myPlayer].ItemSpace(Main.mouseItem).CanTakeItemToPersonalInventory)
					{
						Main.mouseItem = localPlayer.GetItem(Main.mouseItem, GetItemSettings.QuickTransferFromSlot);
					}
					PlayerInput.LockGamepadButtons("Grapple");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
			}
			if (Main.mouseItem.IsAir || (Main.mouseItem.CanHavePrefixes() && Item.CanStack(Main.mouseItem, recipe.createItem) && Main.mouseItem.stack + recipe.createItem.stack <= Main.mouseItem.maxStack))
			{
				text += PlayerInput.BuildCommand(Lang.misc[72].Value, new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"],
					PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]
				});
			}
			if (quickCraftGamepadInstructions != null)
			{
				text += quickCraftGamepadInstructions;
			}
			return text;
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x004F2945 File Offset: 0x004F0B45
		public static bool IsABuildingItem(Item item)
		{
			return item.type > 0 && item.stack > 0 && item.useStyle != 0 && item.useTime > 0;
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x004F296C File Offset: 0x004F0B6C
		public static void SelectEquipPage(Item item)
		{
			Main.EquipPage = -1;
			if (item.IsAir)
			{
				return;
			}
			if (Main.projHook[item.shoot])
			{
				Main.EquipPage = 2;
				return;
			}
			if (item.mountType != -1)
			{
				Main.EquipPage = 2;
				return;
			}
			if (item.buffType > 0 && Main.vanityPet[item.buffType])
			{
				Main.EquipPage = 2;
				return;
			}
			if (item.buffType > 0 && Main.lightPet[item.buffType])
			{
				Main.EquipPage = 2;
				return;
			}
			if (item.dye > 0 && Main.EquipPageSelected == 1)
			{
				Main.EquipPage = 0;
				return;
			}
			if (item.legSlot != -1 || item.headSlot != -1 || item.bodySlot != -1 || item.accessory)
			{
				Main.EquipPage = 0;
			}
		}

		// Token: 0x04001336 RID: 4918
		private static Dictionary<ItemSlot.ItemDisplayKey, ulong> _nextTickDrawAvailable = new Dictionary<ItemSlot.ItemDisplayKey, ulong>();

		// Token: 0x04001337 RID: 4919
		public static bool DrawGoldBGForCraftingMaterial = false;

		// Token: 0x04001338 RID: 4920
		public static bool DrawSelectionHighlightForGridSlot = false;

		// Token: 0x04001339 RID: 4921
		public static bool ShiftForcedOn;

		// Token: 0x0400133B RID: 4923
		private static Item[] singleSlotArray = new Item[1];

		// Token: 0x0400133C RID: 4924
		private static bool[] canFavoriteAt = new bool[ItemSlot.Context.Count];

		// Token: 0x0400133D RID: 4925
		private static bool[] canShareAt = new bool[ItemSlot.Context.Count];

		// Token: 0x0400133E RID: 4926
		private static bool[] canQuickDropAt = new bool[ItemSlot.Context.Count];

		// Token: 0x0400133F RID: 4927
		private static float[] inventoryGlowHue = new float[58];

		// Token: 0x04001340 RID: 4928
		private static int[] inventoryGlowTime = new int[58];

		// Token: 0x04001341 RID: 4929
		private static float[] inventoryGlowHueChest = new float[58];

		// Token: 0x04001342 RID: 4930
		private static int[] inventoryGlowTimeChest = new int[58];

		// Token: 0x04001343 RID: 4931
		private static ItemSlot.PulseEffect[] playerSlotPulseEffects = new ItemSlot.PulseEffect[PlayerItemSlotID.Count];

		// Token: 0x04001344 RID: 4932
		private static int _customCurrencyForSavings = -1;

		// Token: 0x04001345 RID: 4933
		public static bool forceClearGlowsOnChest = false;

		// Token: 0x04001346 RID: 4934
		private static double _lastTimeForVisualEffectsThatLoadoutWasChanged;

		// Token: 0x04001347 RID: 4935
		private static Color[,] LoadoutSlotColors;

		// Token: 0x04001348 RID: 4936
		public static float OverdrawGlowSize;

		// Token: 0x04001349 RID: 4937
		public static Color OverdrawGlowColorMultiplier;

		// Token: 0x0400134A RID: 4938
		private static int dyeSwapCounter;

		// Token: 0x0400134B RID: 4939
		private static Item[] _dirtyHack;

		// Token: 0x0400134C RID: 4940
		public static float CircularRadialOpacity;

		// Token: 0x0400134D RID: 4941
		public static float QuicksRadialOpacity;

		// Token: 0x02000709 RID: 1801
		public class Options
		{
			// Token: 0x04006887 RID: 26759
			public static bool DisableLeftShiftTrashCan = true;

			// Token: 0x04006888 RID: 26760
			public static bool DisableQuickTrash = false;

			// Token: 0x04006889 RID: 26761
			public static bool HighlightNewItems = true;
		}

		// Token: 0x0200070A RID: 1802
		public class Context
		{
			// Token: 0x0400688A RID: 26762
			public const int InventoryItem = 0;

			// Token: 0x0400688B RID: 26763
			public const int InventoryCoin = 1;

			// Token: 0x0400688C RID: 26764
			public const int InventoryAmmo = 2;

			// Token: 0x0400688D RID: 26765
			public const int ChestItem = 3;

			// Token: 0x0400688E RID: 26766
			public const int BankItem = 4;

			// Token: 0x0400688F RID: 26767
			public const int PrefixItem = 5;

			// Token: 0x04006890 RID: 26768
			public const int TrashItem = 6;

			// Token: 0x04006891 RID: 26769
			public const int GuideItem = 7;

			// Token: 0x04006892 RID: 26770
			public const int EquipArmor = 8;

			// Token: 0x04006893 RID: 26771
			public const int EquipArmorVanity = 9;

			// Token: 0x04006894 RID: 26772
			public const int EquipAccessory = 10;

			// Token: 0x04006895 RID: 26773
			public const int EquipAccessoryVanity = 11;

			// Token: 0x04006896 RID: 26774
			public const int EquipDye = 12;

			// Token: 0x04006897 RID: 26775
			public const int HotbarItem = 13;

			// Token: 0x04006898 RID: 26776
			public const int ChatItem = 14;

			// Token: 0x04006899 RID: 26777
			public const int ShopItem = 15;

			// Token: 0x0400689A RID: 26778
			public const int EquipGrapple = 16;

			// Token: 0x0400689B RID: 26779
			public const int EquipMount = 17;

			// Token: 0x0400689C RID: 26780
			public const int EquipMinecart = 18;

			// Token: 0x0400689D RID: 26781
			public const int EquipPet = 19;

			// Token: 0x0400689E RID: 26782
			public const int EquipLight = 20;

			// Token: 0x0400689F RID: 26783
			public const int MouseItem = 21;

			// Token: 0x040068A0 RID: 26784
			public const int CraftingMaterial = 22;

			// Token: 0x040068A1 RID: 26785
			public const int DisplayDollArmor = 23;

			// Token: 0x040068A2 RID: 26786
			public const int DisplayDollAccessory = 24;

			// Token: 0x040068A3 RID: 26787
			public const int DisplayDollDye = 25;

			// Token: 0x040068A4 RID: 26788
			public const int HatRackHat = 26;

			// Token: 0x040068A5 RID: 26789
			public const int HatRackDye = 27;

			// Token: 0x040068A6 RID: 26790
			public const int GoldDebug = 28;

			// Token: 0x040068A7 RID: 26791
			public const int CreativeInfinite = 29;

			// Token: 0x040068A8 RID: 26792
			public const int CreativeSacrifice = 30;

			// Token: 0x040068A9 RID: 26793
			public const int InWorld = 31;

			// Token: 0x040068AA RID: 26794
			public const int VoidItem = 32;

			// Token: 0x040068AB RID: 26795
			public const int EquipMiscDye = 33;

			// Token: 0x040068AC RID: 26796
			public const int CreativeInfiniteLocked = 34;

			// Token: 0x040068AD RID: 26797
			public const int BannerClaiming = 35;

			// Token: 0x040068AE RID: 26798
			public const int HotbarItemSmartSelected = 36;

			// Token: 0x040068AF RID: 26799
			public const int OverdrawGlow = 37;

			// Token: 0x040068B0 RID: 26800
			public const int DisplayDollWeapon = 38;

			// Token: 0x040068B1 RID: 26801
			public const int DisplayDollMount = 39;

			// Token: 0x040068B2 RID: 26802
			public const int InWorldDisplay = 40;

			// Token: 0x040068B3 RID: 26803
			public const int NewCraftingUIRecipe = 41;

			// Token: 0x040068B4 RID: 26804
			public const int NewCraftingUICraftSlot = 42;

			// Token: 0x040068B5 RID: 26805
			public const int NewCraftingUIMaterial = 43;

			// Token: 0x040068B6 RID: 26806
			public static readonly int Count = 44;
		}

		// Token: 0x0200070B RID: 1803
		public struct ItemDisplayKey
		{
			// Token: 0x06003FFA RID: 16378 RVA: 0x0069BF73 File Offset: 0x0069A173
			public bool Equals(ItemSlot.ItemDisplayKey other)
			{
				return this.Context == other.Context && this.Slot == other.Slot;
			}

			// Token: 0x06003FFB RID: 16379 RVA: 0x0069BF93 File Offset: 0x0069A193
			public override bool Equals(object obj)
			{
				return obj is ItemSlot.ItemDisplayKey && this.Equals((ItemSlot.ItemDisplayKey)obj);
			}

			// Token: 0x06003FFC RID: 16380 RVA: 0x0069BFAB File Offset: 0x0069A1AB
			public override int GetHashCode()
			{
				return this.Context * 397 ^ this.Slot;
			}

			// Token: 0x06003FFD RID: 16381 RVA: 0x0069BFC0 File Offset: 0x0069A1C0
			public static bool operator ==(ItemSlot.ItemDisplayKey left, ItemSlot.ItemDisplayKey right)
			{
				return left.Equals(right);
			}

			// Token: 0x06003FFE RID: 16382 RVA: 0x0069BFCA File Offset: 0x0069A1CA
			public static bool operator !=(ItemSlot.ItemDisplayKey left, ItemSlot.ItemDisplayKey right)
			{
				return !left.Equals(right);
			}

			// Token: 0x040068B7 RID: 26807
			public int Context;

			// Token: 0x040068B8 RID: 26808
			public int Slot;
		}

		// Token: 0x0200070C RID: 1804
		public struct AlternateClickAction
		{
			// Token: 0x06003FFF RID: 16383 RVA: 0x0069BFD7 File Offset: 0x0069A1D7
			public AlternateClickAction(int cursorOverride, LocalizedText gamepadHintText)
			{
				this.cursorOverride = cursorOverride;
				this.gamepadHintText = gamepadHintText;
			}

			// Token: 0x06004000 RID: 16384 RVA: 0x0069BFE8 File Offset: 0x0069A1E8
			public static ItemSlot.AlternateClickAction? GetSellOrTrash(Item item)
			{
				if (Main.npcShop <= 0)
				{
					return new ItemSlot.AlternateClickAction?(ItemSlot.AlternateClickAction.Trash);
				}
				if (item.type >= 71 && item.type <= 74)
				{
					return null;
				}
				return new ItemSlot.AlternateClickAction?(ItemSlot.AlternateClickAction.Sell);
			}

			// Token: 0x040068B9 RID: 26809
			public readonly int cursorOverride;

			// Token: 0x040068BA RID: 26810
			public readonly LocalizedText gamepadHintText;

			// Token: 0x040068BB RID: 26811
			public static ItemSlot.AlternateClickAction Trash = new ItemSlot.AlternateClickAction(6, Lang.misc[74]);

			// Token: 0x040068BC RID: 26812
			public static ItemSlot.AlternateClickAction TransferToBackpack = new ItemSlot.AlternateClickAction(7, Lang.misc[76]);

			// Token: 0x040068BD RID: 26813
			public static ItemSlot.AlternateClickAction Unequip = new ItemSlot.AlternateClickAction(7, Lang.misc[68]);

			// Token: 0x040068BE RID: 26814
			public static ItemSlot.AlternateClickAction TransferFromChest = new ItemSlot.AlternateClickAction(8, Lang.misc[76]);

			// Token: 0x040068BF RID: 26815
			public static ItemSlot.AlternateClickAction TransferToChest = new ItemSlot.AlternateClickAction(9, Lang.misc[76]);

			// Token: 0x040068C0 RID: 26816
			public static ItemSlot.AlternateClickAction Sell = new ItemSlot.AlternateClickAction(10, Lang.misc[75]);
		}

		// Token: 0x0200070D RID: 1805
		public struct ItemTransferInfo
		{
			// Token: 0x06004002 RID: 16386 RVA: 0x0069C0B1 File Offset: 0x0069A2B1
			public ItemTransferInfo(Item itemAfter, int fromContext, int toContext, int transferAmount = 0)
			{
				this.ItemType = itemAfter.type;
				this.TransferAmount = itemAfter.stack;
				if (transferAmount != 0)
				{
					this.TransferAmount = transferAmount;
				}
				this.FromContenxt = fromContext;
				this.ToContext = toContext;
			}

			// Token: 0x040068C1 RID: 26817
			public int ItemType;

			// Token: 0x040068C2 RID: 26818
			public int TransferAmount;

			// Token: 0x040068C3 RID: 26819
			public int FromContenxt;

			// Token: 0x040068C4 RID: 26820
			public int ToContext;
		}

		// Token: 0x0200070E RID: 1806
		// (Invoke) Token: 0x06004004 RID: 16388
		public delegate void ItemTransferEvent(ItemSlot.ItemTransferInfo info);

		// Token: 0x0200070F RID: 1807
		public struct PulseEffect
		{
			// Token: 0x06004007 RID: 16391 RVA: 0x0069C0E5 File Offset: 0x0069A2E5
			public PulseEffect(PlayerItemSlotID.SlotReference slotRef, Color color)
			{
				this.color = color;
				this.slotRef = slotRef;
				this.itemInSlot = slotRef.Item;
				this.time = 0;
			}

			// Token: 0x1700050B RID: 1291
			// (get) Token: 0x06004008 RID: 16392 RVA: 0x0069C109 File Offset: 0x0069A309
			public bool IsActive
			{
				get
				{
					return this.itemInSlot != null;
				}
			}

			// Token: 0x040068C5 RID: 26821
			public static readonly int EffectDuration = 40;

			// Token: 0x040068C6 RID: 26822
			public static readonly int NumPulses = 2;

			// Token: 0x040068C7 RID: 26823
			public readonly Color color;

			// Token: 0x040068C8 RID: 26824
			public readonly PlayerItemSlotID.SlotReference slotRef;

			// Token: 0x040068C9 RID: 26825
			public readonly Item itemInSlot;

			// Token: 0x040068CA RID: 26826
			public int time;
		}

		// Token: 0x02000710 RID: 1808
		// (Invoke) Token: 0x0600400B RID: 16395
		public delegate void ItemPickupAction<TItemInfo>(TItemInfo info, int stackToGet);
	}
}
