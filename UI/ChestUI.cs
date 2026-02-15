using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using ReLogic.Localization.IME;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;

namespace Terraria.UI
{
	// Token: 0x020000F9 RID: 249
	public class ChestUI
	{
		// Token: 0x0600194F RID: 6479 RVA: 0x004E75AC File Offset: 0x004E57AC
		public static void UpdateHover(int ID, bool hovering)
		{
			if (hovering)
			{
				if (!ChestUI.ButtonHovered[ID])
				{
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				}
				ChestUI.ButtonHovered[ID] = true;
				ChestUI.ButtonScale[ID] += 0.05f;
				if (ChestUI.ButtonScale[ID] > 1f)
				{
					ChestUI.ButtonScale[ID] = 1f;
					return;
				}
			}
			else
			{
				ChestUI.ButtonHovered[ID] = false;
				ChestUI.ButtonScale[ID] -= 0.05f;
				if (ChestUI.ButtonScale[ID] < 0.75f)
				{
					ChestUI.ButtonScale[ID] = 0.75f;
				}
			}
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x004E7648 File Offset: 0x004E5848
		public static void Draw(SpriteBatch spritebatch)
		{
			if (Main.player[Main.myPlayer].chest != -1 && !Main.PipsUseGrid && !NewCraftingUI.Visible)
			{
				Main.inventoryScale = 0.755f;
				if (Utils.FloatIntersect((float)Main.mouseX, (float)Main.mouseY, 0f, 0f, 73f, (float)Main.instance.invBottom, 560f * Main.inventoryScale, 224f * Main.inventoryScale))
				{
					Main.player[Main.myPlayer].mouseInterface = true;
				}
				ChestUI.DrawName(spritebatch);
				ChestUI.DrawButtons(spritebatch);
				ChestUI.DrawSlots(spritebatch);
				return;
			}
			for (int i = 0; i < ChestUI.ButtonID.Count; i++)
			{
				ChestUI.ButtonScale[i] = 0.75f;
				ChestUI.ButtonHovered[i] = false;
			}
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x004E770C File Offset: 0x004E590C
		private static void DrawName(SpriteBatch spritebatch)
		{
			Player player = Main.player[Main.myPlayer];
			string text = string.Empty;
			if (Main.editChest)
			{
				text = Main.npcChatText;
			}
			else if (player.chest > -1 && Main.chest[player.chest] != null)
			{
				Chest chest = Main.chest[player.chest];
				if (chest.name != "")
				{
					text = chest.name;
				}
				else
				{
					Tile tile = Main.tile[player.chestX, player.chestY];
					if (tile.type == 21)
					{
						text = Lang.chestType[(int)(tile.frameX / 36)].Value;
					}
					else if (tile.type == 467 && tile.frameX / 36 == 4)
					{
						text = Lang.GetItemNameValue(3988);
					}
					else if (tile.type == 467)
					{
						text = Lang.chestType2[(int)(tile.frameX / 36)].Value;
					}
					else if (tile.type == 88)
					{
						text = Lang.dresserType[(int)(tile.frameX / 54)].Value;
					}
				}
			}
			else if (player.chest == -2)
			{
				text = Lang.inter[32].Value;
			}
			else if (player.chest == -3)
			{
				text = Lang.inter[33].Value;
			}
			else if (player.chest == -4)
			{
				text = Lang.GetItemNameValue(3813);
			}
			else if (player.chest == -5)
			{
				text = Lang.GetItemNameValue(4076);
			}
			Color baseColor = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
			baseColor = Color.White * (1f - (255f - (float)Main.mouseTextColor) / 255f * 0.5f);
			baseColor.A = byte.MaxValue;
			DynamicSpriteFont value = FontAssets.MouseText.Value;
			Vector2 vector = new Vector2(504f, (float)Main.instance.invBottom);
			ChatManager.DrawColorCodedStringWithShadow(spritebatch, value, text, vector, baseColor, 0f, Vector2.Zero, Vector2.One, -1f, 1.5f);
			if (Main.editChest)
			{
				vector.X += value.MeasureString(text).X;
				Main.instance.SetIMEPanelAnchor(vector + new Vector2(0f, 56f), 0f);
				string compositionString = Platform.Get<IImeService>().CompositionString;
				if (compositionString != null && compositionString.Length > 0)
				{
					ChatManager.DrawColorCodedStringWithShadow(spritebatch, value, compositionString, vector, Main.imeCompositionStringColor, 0f, Vector2.Zero, Vector2.One, -1f, 1.5f);
					vector.X += value.MeasureString(compositionString).X;
				}
				Main instance = Main.instance;
				int num = instance.textBlinkerCount + 1;
				instance.textBlinkerCount = num;
				if (num >= 20)
				{
					Main.instance.textBlinkerState = ((Main.instance.textBlinkerState == 0) ? 1 : 0);
					Main.instance.textBlinkerCount = 0;
				}
				if (Main.instance.textBlinkerState == 1)
				{
					ChatManager.DrawColorCodedStringWithShadow(spritebatch, value, "|", vector, baseColor, 0f, Vector2.Zero, Vector2.One, -1f, 1.5f);
				}
			}
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x004E7A4C File Offset: 0x004E5C4C
		private static void DrawButtons(SpriteBatch spritebatch)
		{
			for (int i = 0; i < ChestUI.ButtonID.Count; i++)
			{
				ChestUI.DrawButton(spritebatch, i, 506, Main.instance.invBottom + 40);
			}
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x004E7A84 File Offset: 0x004E5C84
		private static void DrawButton(SpriteBatch spriteBatch, int ID, int X, int Y)
		{
			Player player = Main.player[Main.myPlayer];
			if ((ID == 5 && player.chest < -1) || (ID == 6 && !Main.editChest))
			{
				ChestUI.UpdateHover(ID, false);
				return;
			}
			Y += ID * 26;
			float num = ChestUI.ButtonScale[ID];
			string text = "";
			switch (ID)
			{
			case 0:
				text = Lang.inter[29].Value;
				break;
			case 1:
				text = Lang.inter[30].Value;
				break;
			case 2:
				text = Lang.inter[31].Value;
				break;
			case 3:
				text = Lang.inter[82].Value;
				break;
			case 4:
				text = Lang.inter[122].Value;
				break;
			case 5:
				text = Lang.inter[Main.editChest ? 47 : 61].Value;
				break;
			case 6:
				text = Lang.inter[63].Value;
				break;
			}
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(text);
			Color baseColor = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor) * num;
			baseColor = Color.White * 0.97f * (1f - (255f - (float)Main.mouseTextColor) / 255f * 0.5f);
			baseColor.A = byte.MaxValue;
			int num2 = (int)(vector.X * num / 2f);
			X += num2;
			bool flag = Utils.FloatIntersect((float)Main.mouseX, (float)Main.mouseY, 0f, 0f, (float)(X - num2), (float)(Y - 12), vector.X * num, 24f);
			if (ChestUI.ButtonHovered[ID])
			{
				flag = Utils.FloatIntersect((float)Main.mouseX, (float)Main.mouseY, 0f, 0f, (float)(X - num2 - 10), (float)(Y - 12), vector.X * num + 16f, 24f);
			}
			if (flag)
			{
				baseColor = Main.OurFavoriteColor;
			}
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text, new Vector2((float)X, (float)Y), baseColor, 0f, vector / 2f, new Vector2(num), -1f, 1.5f);
			vector *= num;
			switch (ID)
			{
			case 0:
				UILinkPointNavigator.SetPosition(500, new Vector2((float)X - vector.X * num / 2f * 0.8f, (float)Y));
				break;
			case 1:
				UILinkPointNavigator.SetPosition(501, new Vector2((float)X - vector.X * num / 2f * 0.8f, (float)Y));
				break;
			case 2:
				UILinkPointNavigator.SetPosition(502, new Vector2((float)X - vector.X * num / 2f * 0.8f, (float)Y));
				break;
			case 3:
				UILinkPointNavigator.SetPosition(503, new Vector2((float)X - vector.X * num / 2f * 0.8f, (float)Y));
				break;
			case 4:
				UILinkPointNavigator.SetPosition(505, new Vector2((float)X - vector.X * num / 2f * 0.8f, (float)Y));
				break;
			case 5:
				UILinkPointNavigator.SetPosition(504, new Vector2((float)X, (float)Y));
				break;
			case 6:
				UILinkPointNavigator.SetPosition(504, new Vector2((float)X, (float)Y));
				break;
			}
			if (!flag)
			{
				ChestUI.UpdateHover(ID, false);
				return;
			}
			ChestUI.UpdateHover(ID, true);
			if (!PlayerInput.IgnoreMouseInterface)
			{
				player.mouseInterface = true;
				if (!Main.mouseLeft || !Main.mouseLeftRelease)
				{
					return;
				}
				switch (ID)
				{
				case 0:
					ChestUI.LootAll();
					return;
				case 1:
					ChestUI.DepositAll();
					return;
				case 2:
					ChestUI.QuickStack(false);
					return;
				case 3:
					ChestUI.Restock();
					return;
				case 4:
					ItemSorting.SortChest();
					break;
				case 5:
					ChestUI.RenameChest();
					return;
				case 6:
					ChestUI.RenameChestCancel();
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x004E7E68 File Offset: 0x004E6068
		public static void Scroll(int mouseWheel)
		{
			int startingRowForDrawing = ChestUI.StartingRowForDrawing;
			if (mouseWheel > 0)
			{
				ChestUI.StartingRowForDrawing--;
			}
			else
			{
				ChestUI.StartingRowForDrawing++;
			}
			ChestUI.StartingRowForDrawing = Utils.Clamp<int>(ChestUI.StartingRowForDrawing, 0, ChestUI.LastHighestChestRow);
			if (startingRowForDrawing != ChestUI.StartingRowForDrawing)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x004E7ECC File Offset: 0x004E60CC
		private static void DrawSlots(SpriteBatch spriteBatch)
		{
			int num = 10;
			int num2 = 4;
			Player player = Main.player[Main.myPlayer];
			int context = 0;
			Chest chest = null;
			if (player.chest > -1)
			{
				context = 3;
				chest = Main.chest[player.chest];
			}
			if (player.chest == -2)
			{
				context = 4;
				chest = player.bank;
			}
			if (player.chest == -3)
			{
				context = 4;
				chest = player.bank2;
			}
			if (player.chest == -4)
			{
				context = 4;
				chest = player.bank3;
			}
			if (player.chest == -5)
			{
				context = 32;
				chest = player.bank4;
			}
			Item[] item = chest.item;
			int maxItems = chest.maxItems;
			Main.inventoryScale = 0.755f;
			Rectangle lastChestDisplayRectangle = new Rectangle(73, Main.instance.invBottom, (int)((float)(num * 56) * Main.inventoryScale), (int)((float)(num2 * 56) * Main.inventoryScale));
			ChestUI.LastChestDisplayRectangle = lastChestDisplayRectangle;
			if (lastChestDisplayRectangle.Contains(Main.mouseX, Main.mouseY) && !PlayerInput.IgnoreMouseInterface)
			{
				player.mouseInterface = true;
			}
			int num3 = (int)Math.Max(0.0, Math.Ceiling((double)((float)maxItems / (float)num)) - 4.0);
			ChestUI.StartingRowForDrawing = Utils.Clamp<int>(ChestUI.StartingRowForDrawing, 0, num3);
			ChestUI.LastHighestChestRow = num3;
			ItemSlot.PrepareForChest(chest);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					int num4 = i + j * num + ChestUI.StartingRowForDrawing * num;
					if (num4 < item.Length)
					{
						int num5 = (int)(73f + (float)(i * 56) * Main.inventoryScale);
						int num6 = (int)((float)Main.instance.invBottom + (float)(j * 56) * Main.inventoryScale);
						new Color(100, 100, 100, 100);
						if (Utils.FloatIntersect((float)Main.mouseX, (float)Main.mouseY, 0f, 0f, (float)num5, (float)num6, (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale, (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
						{
							player.mouseInterface = true;
							ItemSlot.Handle(item, context, num4, true);
						}
						ItemSlot.Draw(spriteBatch, item, context, num4, new Vector2((float)num5, (float)num6), default(Color));
					}
				}
			}
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x004E8114 File Offset: 0x004E6314
		public static void LootAll()
		{
			Player player = Main.player[Main.myPlayer];
			GetItemSettings settings = (player.chest > -1) ? GetItemSettings.LootAllFromChest : GetItemSettings.LootAllFromBank;
			if (player.chest > -1)
			{
				Chest chest = Main.chest[player.chest];
				for (int i = 0; i < chest.maxItems; i++)
				{
					if (chest.item[i].type > 0)
					{
						Player.GetItemLogger.Start();
						chest.item[i] = player.GetItem(chest.item[i], settings);
						Player.GetItemLogger.Stop();
						ItemSlot.DisplayTransfer_GetItem(chest.item, 3, i);
						if (Main.netMode == 1)
						{
							NetMessage.SendData(32, -1, -1, null, player.chest, (float)i, 0f, 0f, 0, 0, 0);
						}
					}
				}
				return;
			}
			if (player.chest == -3)
			{
				for (int j = 0; j < player.bank2.maxItems; j++)
				{
					if (player.bank2.item[j].type > 0)
					{
						Player.GetItemLogger.Start();
						player.bank2.item[j] = player.GetItem(player.bank2.item[j], settings);
						Player.GetItemLogger.Stop();
						ItemSlot.DisplayTransfer_GetItem(player.bank2.item, 4, j);
					}
				}
				return;
			}
			if (player.chest == -4)
			{
				for (int k = 0; k < player.bank3.maxItems; k++)
				{
					if (player.bank3.item[k].type > 0)
					{
						Player.GetItemLogger.Start();
						player.bank3.item[k] = player.GetItem(player.bank3.item[k], settings);
						Player.GetItemLogger.Stop();
						ItemSlot.DisplayTransfer_GetItem(player.bank3.item, 4, k);
					}
				}
				return;
			}
			if (player.chest == -5)
			{
				for (int l = 0; l < player.bank4.maxItems; l++)
				{
					if (player.bank4.item[l].type > 0 && !player.bank4.item[l].favorited)
					{
						Player.GetItemLogger.Start();
						player.bank4.item[l] = player.GetItem(player.bank4.item[l], settings);
						Player.GetItemLogger.Stop();
						ItemSlot.DisplayTransfer_GetItem(player.bank4.item, 32, l);
					}
				}
				return;
			}
			for (int m = 0; m < player.bank.maxItems; m++)
			{
				if (player.bank.item[m].type > 0)
				{
					Player.GetItemLogger.Start();
					player.bank.item[m] = player.GetItem(player.bank.item[m], settings);
					Player.GetItemLogger.Stop();
					ItemSlot.DisplayTransfer_GetItem(player.bank.item, 4, m);
				}
			}
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x004E8404 File Offset: 0x004E6604
		private static void DepositAll_IntoWorldChest(Player player, Chest chest, int playerInventorySlot)
		{
			for (int i = 0; i < chest.maxItems; i++)
			{
				if (chest.item[i].stack < chest.item[i].maxStack && Item.CanStack(player.inventory[playerInventorySlot], chest.item[i]))
				{
					int num = player.inventory[playerInventorySlot].stack;
					if (player.inventory[playerInventorySlot].stack + chest.item[i].stack > chest.item[i].maxStack)
					{
						num = chest.item[i].maxStack - chest.item[i].stack;
					}
					player.inventory[playerInventorySlot].stack -= num;
					chest.item[i].stack += num;
					SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
					if (player.inventory[playerInventorySlot].stack <= 0)
					{
						player.inventory[playerInventorySlot].SetDefaults(0, null);
						if (Main.netMode == 1)
						{
							NetMessage.SendData(32, -1, -1, null, player.chest, (float)i, 0f, 0f, 0, 0, 0);
							return;
						}
						break;
					}
					else
					{
						if (chest.item[i].type == 0)
						{
							chest.item[i] = player.inventory[playerInventorySlot].Clone();
							player.inventory[playerInventorySlot].SetDefaults(0, null);
						}
						if (Main.netMode == 1)
						{
							NetMessage.SendData(32, -1, -1, null, player.chest, (float)i, 0f, 0f, 0, 0, 0);
						}
					}
				}
			}
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x004E8598 File Offset: 0x004E6798
		private static void DepositAll_IntoLocalChest(Player player, Chest chest, int p)
		{
			for (int i = 0; i < chest.maxItems; i++)
			{
				if (chest.item[i].stack < chest.item[i].maxStack && Item.CanStack(player.inventory[p], chest.item[i]))
				{
					int num = player.inventory[p].stack;
					if (player.inventory[p].stack + chest.item[i].stack > chest.item[i].maxStack)
					{
						num = chest.item[i].maxStack - chest.item[i].stack;
					}
					player.inventory[p].stack -= num;
					chest.item[i].stack += num;
					SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
					if (player.inventory[p].stack <= 0)
					{
						player.inventory[p].SetDefaults(0, null);
						return;
					}
					if (chest.item[i].type == 0)
					{
						chest.item[i] = player.inventory[p].Clone();
						player.inventory[p].SetDefaults(0, null);
					}
				}
			}
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x004E86DC File Offset: 0x004E68DC
		public static void DepositAll()
		{
			Player player = Main.player[Main.myPlayer];
			if (player.chest > -1)
			{
				ChestUI.MoveCoins(player.inventory, Main.chest[player.chest]);
			}
			else if (player.chest == -3)
			{
				ChestUI.MoveCoins(player.inventory, player.bank2);
			}
			else if (player.chest == -4)
			{
				ChestUI.MoveCoins(player.inventory, player.bank3);
			}
			else if (player.chest == -5)
			{
				ChestUI.MoveCoins(player.inventory, player.bank4);
			}
			else
			{
				ChestUI.MoveCoins(player.inventory, player.bank);
			}
			for (int i = 49; i >= 10; i--)
			{
				if (player.inventory[i].stack > 0 && player.inventory[i].type > 0 && !player.inventory[i].favorited)
				{
					if (player.inventory[i].maxStack > 1)
					{
						Chest currentContainer = player.GetCurrentContainer();
						if (player.chest > -1)
						{
							ChestUI.DepositAll_IntoWorldChest(player, currentContainer, i);
						}
						else
						{
							ChestUI.DepositAll_IntoLocalChest(player, currentContainer, i);
						}
					}
					if (player.inventory[i].stack > 0)
					{
						if (player.chest > -1)
						{
							int j = 0;
							while (j < Main.chest[player.chest].maxItems)
							{
								if (Main.chest[player.chest].item[j].stack == 0)
								{
									SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
									Main.chest[player.chest].item[j] = player.inventory[i].Clone();
									player.inventory[i].SetDefaults(0, null);
									ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, i, Main.chest[player.chest].item, 3, j, Main.chest[player.chest].item[j].stack);
									if (Main.netMode == 1)
									{
										NetMessage.SendData(32, -1, -1, null, player.chest, (float)j, 0f, 0f, 0, 0, 0);
										break;
									}
									break;
								}
								else
								{
									j++;
								}
							}
						}
						else if (player.chest == -3)
						{
							for (int k = 0; k < player.bank2.maxItems; k++)
							{
								if (player.bank2.item[k].stack == 0)
								{
									SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
									player.bank2.item[k] = player.inventory[i].Clone();
									player.inventory[i].SetDefaults(0, null);
									ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, i, player.bank2.item, 4, k, player.bank2.item[k].stack);
									break;
								}
							}
						}
						else if (player.chest == -4)
						{
							for (int l = 0; l < player.bank3.maxItems; l++)
							{
								if (player.bank3.item[l].stack == 0)
								{
									SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
									player.bank3.item[l] = player.inventory[i].Clone();
									player.inventory[i].SetDefaults(0, null);
									ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, i, player.bank3.item, 4, l, player.bank3.item[l].stack);
									break;
								}
							}
						}
						else if (player.chest == -5)
						{
							for (int m = 0; m < player.bank4.maxItems; m++)
							{
								if (player.bank4.item[m].stack == 0)
								{
									SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
									player.bank4.item[m] = player.inventory[i].Clone();
									player.inventory[i].SetDefaults(0, null);
									ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, i, player.bank4.item, 32, m, player.bank4.item[m].stack);
									break;
								}
							}
						}
						else
						{
							for (int n = 0; n < player.bank.maxItems; n++)
							{
								if (player.bank.item[n].stack == 0)
								{
									SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
									player.bank.item[n] = player.inventory[i].Clone();
									player.inventory[i].SetDefaults(0, null);
									ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, i, player.bank.item, 4, n, player.bank.item[n].stack);
									break;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x004E8BDC File Offset: 0x004E6DDC
		public static void QuickStack(bool voidStack = false)
		{
			Player player = Main.player[Main.myPlayer];
			Item[] array = player.inventory;
			if (voidStack)
			{
				array = player.bank4.item;
			}
			Vector2 center = player.Center;
			if (!voidStack && player.chest == -5)
			{
				ChestUI.MoveCoins(array, player.bank4);
			}
			else if (player.chest == -4)
			{
				ChestUI.MoveCoins(array, player.bank3);
			}
			else if (player.chest == -3)
			{
				ChestUI.MoveCoins(array, player.bank2);
			}
			else if (player.chest == -2)
			{
				ChestUI.MoveCoins(array, player.bank);
			}
			Chest currentContainer = player.GetCurrentContainer();
			Item[] item = currentContainer.item;
			int toContext = 3;
			if (voidStack || player.chest == -5)
			{
				toContext = 32;
			}
			else if (player.chest < -1)
			{
				toContext = 4;
			}
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			List<int> list4 = new List<int>();
			bool[] array2 = new bool[item.Length];
			for (int i = 0; i < currentContainer.maxItems; i++)
			{
				if (item[i].type > 0 && item[i].stack > 0 && (item[i].type < 71 || item[i].type > 74))
				{
					list2.Add(i);
					list.Add(item[i].type);
				}
				if (item[i].type == 0 || item[i].stack <= 0)
				{
					list3.Add(i);
				}
			}
			int num = 50;
			int num2 = 10;
			if (player.chest <= -2)
			{
				num += 4;
			}
			if (voidStack)
			{
				num2 = 0;
				num = player.bank4.maxItems;
			}
			for (int j = num2; j < num; j++)
			{
				if (list.Contains(array[j].type) && !array[j].favorited)
				{
					dictionary.Add(j, array[j].type);
				}
			}
			for (int k = 0; k < list2.Count; k++)
			{
				int num3 = list2[k];
				foreach (KeyValuePair<int, int> keyValuePair in dictionary)
				{
					if (Item.CanStack(array[keyValuePair.Key], item[num3]))
					{
						int num4 = array[keyValuePair.Key].stack;
						int num5 = item[num3].maxStack - item[num3].stack;
						if (num5 == 0)
						{
							break;
						}
						if (num4 > num5)
						{
							num4 = num5;
						}
						SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
						item[num3].stack += num4;
						array[keyValuePair.Key].stack -= num4;
						if (array[keyValuePair.Key].stack == 0)
						{
							array[keyValuePair.Key].SetDefaults(0, null);
						}
						array2[num3] = true;
					}
				}
			}
			foreach (KeyValuePair<int, int> keyValuePair2 in dictionary)
			{
				if (array[keyValuePair2.Key].stack == 0)
				{
					list4.Add(keyValuePair2.Key);
				}
			}
			foreach (int key in list4)
			{
				dictionary.Remove(key);
			}
			for (int l = 0; l < list3.Count; l++)
			{
				int num6 = list3[l];
				bool flag = true;
				foreach (KeyValuePair<int, int> keyValuePair3 in dictionary)
				{
					if (array[keyValuePair3.Key].stack != 0 && (flag || Item.CanStack(array[keyValuePair3.Key], item[num6])))
					{
						SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
						if (flag)
						{
							item[num6] = array[keyValuePair3.Key];
							array[keyValuePair3.Key] = new Item();
							ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, keyValuePair3.Key, item, toContext, num6, 1);
						}
						else
						{
							int num7 = array[keyValuePair3.Key].stack;
							int num8 = item[num6].maxStack - item[num6].stack;
							if (num8 == 0)
							{
								break;
							}
							if (num7 > num8)
							{
								num7 = num8;
							}
							item[num6].stack += num7;
							array[keyValuePair3.Key].stack -= num7;
							if (array[keyValuePair3.Key].stack == 0)
							{
								array[keyValuePair3.Key] = new Item();
							}
							ItemSlot.DisplayTransfer_OneWay(player.inventory, 0, keyValuePair3.Key, item, toContext, num6, 1);
						}
						array2[num6] = true;
						flag = false;
					}
				}
			}
			if (Main.netMode == 1 && player.chest >= 0)
			{
				for (int m = 0; m < array2.Length; m++)
				{
					NetMessage.SendData(32, -1, -1, null, player.chest, (float)m, 0f, 0f, 0, 0, 0);
				}
			}
			list.Clear();
			list2.Clear();
			list3.Clear();
			dictionary.Clear();
			list4.Clear();
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x004E9198 File Offset: 0x004E7398
		public static void RenameChest()
		{
			Player player = Main.player[Main.myPlayer];
			if (!Main.editChest)
			{
				IngameFancyUI.OpenVirtualKeyboard(2);
				return;
			}
			ChestUI.RenameChestSubmit(player);
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x004E91C8 File Offset: 0x004E73C8
		public static void RenameChestSubmit(Player player)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			Main.editChest = false;
			int chest = player.chest;
			if (chest < 0)
			{
				return;
			}
			if (Main.npcChatText == Main.defaultChestName)
			{
				Main.npcChatText = "";
			}
			if (Main.chest[chest].name != Main.npcChatText)
			{
				Main.chest[chest].name = Main.npcChatText;
				if (Main.netMode == 1)
				{
					player.editedChestName = true;
				}
			}
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x004E9250 File Offset: 0x004E7450
		public static void RenameChestCancel()
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			Main.editChest = false;
			Main.npcChatText = string.Empty;
			Main.blockKey = Keys.Escape.ToString();
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x004E9298 File Offset: 0x004E7498
		public static void Restock()
		{
			Player player = Main.player[Main.myPlayer];
			Item[] inventory = player.inventory;
			Item[] item = player.bank.item;
			if (player.chest > -1)
			{
				item = Main.chest[player.chest].item;
			}
			else if (player.chest == -2)
			{
				item = player.bank.item;
			}
			else if (player.chest == -3)
			{
				item = player.bank2.item;
			}
			else if (player.chest == -4)
			{
				item = player.bank3.item;
			}
			else if (player.chest == -5)
			{
				item = player.bank4.item;
			}
			HashSet<int> hashSet = new HashSet<int>();
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			for (int i = 57; i >= 0; i--)
			{
				Item item2 = inventory[i];
				if ((i < 50 || i >= 54) && (item2.type < 71 || item2.type > 74))
				{
					if (item2.stack == 0 || item2.type == 0 || item2.type == 0)
					{
						list2.Add(i);
					}
					else if (item2.maxStack > 1 && (!item2.favorited || !item2.OnlyNeedOneInInventory()))
					{
						hashSet.Add(item2.type);
						if (item2.stack < item2.maxStack)
						{
							list.Add(i);
						}
					}
				}
			}
			bool flag = false;
			for (int j = 0; j < item.Length; j++)
			{
				if (item[j].stack >= 1 && item[j].prefix == 0 && hashSet.Contains(item[j].type))
				{
					bool flag2 = false;
					for (int k = 0; k < list.Count; k++)
					{
						int num = list[k];
						int context = 0;
						if (num >= 50)
						{
							context = 2;
						}
						if (Item.CanStack(inventory[num], item[j]) && ItemSlot.PickItemMovementAction(inventory, context, num, item[j]) != -1)
						{
							int num2 = item[j].stack;
							if (inventory[num].maxStack - inventory[num].stack < num2)
							{
								num2 = inventory[num].maxStack - inventory[num].stack;
							}
							inventory[num].stack += num2;
							item[j].stack -= num2;
							flag = true;
							if (inventory[num].stack == inventory[num].maxStack)
							{
								if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
								{
									NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)j, 0f, 0f, 0, 0, 0);
								}
								list.RemoveAt(k);
								k--;
							}
							if (item[j].stack == 0)
							{
								item[j] = new Item();
								flag2 = true;
								if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
								{
									NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)j, 0f, 0f, 0, 0, 0);
									break;
								}
								break;
							}
						}
					}
					if (!flag2 && list2.Count > 0 && item[j].ammo != 0)
					{
						for (int l = 0; l < list2.Count; l++)
						{
							int context2 = 0;
							if (list2[l] >= 50)
							{
								context2 = 2;
							}
							if (ItemSlot.PickItemMovementAction(inventory, context2, list2[l], item[j]) != -1)
							{
								Utils.Swap<Item>(ref inventory[list2[l]], ref item[j]);
								if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
								{
									NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)j, 0f, 0f, 0, 0, 0);
								}
								list.Add(list2[l]);
								list2.RemoveAt(l);
								flag = true;
								break;
							}
						}
					}
				}
			}
			if (flag)
			{
				SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
			}
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x004E96C7 File Offset: 0x004E78C7
		public static long MoveCoins(Item[] pInv, Chest chest)
		{
			return ChestUI.MoveCoins(pInv, chest.item, chest.maxItems);
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x004E96DC File Offset: 0x004E78DC
		public static long MoveCoins(Item[] pInv, Item[] cInv, int chestMaxItems)
		{
			bool flag = false;
			int[] array = new int[4];
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			bool flag2 = false;
			int[] array2 = new int[chestMaxItems];
			bool flag3;
			long num = Utils.CoinsCount(out flag3, pInv, new int[0]);
			for (int i = 0; i < cInv.Length; i++)
			{
				array2[i] = -1;
				if (cInv[i].stack < 1 || cInv[i].type < 1)
				{
					list2.Add(i);
					cInv[i] = new Item();
				}
				if (cInv[i] != null && cInv[i].stack > 0)
				{
					int num2 = 0;
					if (cInv[i].type == 71)
					{
						num2 = 1;
					}
					if (cInv[i].type == 72)
					{
						num2 = 2;
					}
					if (cInv[i].type == 73)
					{
						num2 = 3;
					}
					if (cInv[i].type == 74)
					{
						num2 = 4;
					}
					array2[i] = num2 - 1;
					if (num2 > 0)
					{
						array[num2 - 1] += cInv[i].stack;
						list2.Add(i);
						cInv[i] = new Item();
						flag2 = true;
					}
				}
			}
			if (!flag2)
			{
				return 0L;
			}
			for (int j = 0; j < pInv.Length; j++)
			{
				if (j != 58 && pInv[j] != null && pInv[j].stack > 0 && !pInv[j].favorited)
				{
					int num3 = 0;
					if (pInv[j].type == 71)
					{
						num3 = 1;
					}
					if (pInv[j].type == 72)
					{
						num3 = 2;
					}
					if (pInv[j].type == 73)
					{
						num3 = 3;
					}
					if (pInv[j].type == 74)
					{
						num3 = 4;
					}
					if (num3 > 0)
					{
						flag = true;
						array[num3 - 1] += pInv[j].stack;
						list.Add(j);
						pInv[j] = new Item();
					}
				}
			}
			for (int k = 0; k < 3; k++)
			{
				while (array[k] >= 100)
				{
					array[k] -= 100;
					array[k + 1]++;
				}
			}
			for (int l = 0; l < chestMaxItems; l++)
			{
				if (array2[l] >= 0 && cInv[l].type == 0)
				{
					int num4 = l;
					int num5 = array2[l];
					if (array[num5] > 0)
					{
						cInv[num4].SetDefaults(71 + num5, null);
						cInv[num4].stack = array[num5];
						if (cInv[num4].stack > cInv[num4].maxStack)
						{
							cInv[num4].stack = cInv[num4].maxStack;
						}
						array[num5] -= cInv[num4].stack;
						array2[l] = -1;
					}
					if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
					{
						NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)num4, 0f, 0f, 0, 0, 0);
					}
					list2.Remove(num4);
				}
			}
			for (int m = 0; m < chestMaxItems; m++)
			{
				if (array2[m] >= 0 && cInv[m].type == 0)
				{
					int num6 = m;
					int n = 3;
					while (n >= 0)
					{
						if (array[n] > 0)
						{
							cInv[num6].SetDefaults(71 + n, null);
							cInv[num6].stack = array[n];
							if (cInv[num6].stack > cInv[num6].maxStack)
							{
								cInv[num6].stack = cInv[num6].maxStack;
							}
							array[n] -= cInv[num6].stack;
							array2[m] = -1;
							break;
						}
						if (array[n] == 0)
						{
							n--;
						}
					}
					if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
					{
						NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)num6, 0f, 0f, 0, 0, 0);
					}
					list2.Remove(num6);
				}
			}
			while (list2.Count > 0)
			{
				int num7 = list2[0];
				int num8 = 3;
				while (num8 >= 0)
				{
					if (array[num8] > 0)
					{
						cInv[num7].SetDefaults(71 + num8, null);
						cInv[num7].stack = array[num8];
						if (cInv[num7].stack > cInv[num7].maxStack)
						{
							cInv[num7].stack = cInv[num7].maxStack;
						}
						array[num8] -= cInv[num7].stack;
						break;
					}
					if (array[num8] == 0)
					{
						num8--;
					}
				}
				if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
				{
					NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)list2[0], 0f, 0f, 0, 0, 0);
				}
				list2.RemoveAt(0);
			}
			int num9 = 3;
			while (num9 >= 0 && list.Count > 0)
			{
				int num10 = list[0];
				if (array[num9] > 0)
				{
					pInv[num10].SetDefaults(71 + num9, null);
					pInv[num10].stack = array[num9];
					if (pInv[num10].stack > pInv[num10].maxStack)
					{
						pInv[num10].stack = pInv[num10].maxStack;
					}
					array[num9] -= pInv[num10].stack;
					flag = false;
					list.RemoveAt(0);
				}
				if (array[num9] == 0)
				{
					num9--;
				}
			}
			if (flag)
			{
				SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
			}
			bool flag4;
			long num11 = Utils.CoinsCount(out flag4, pInv, new int[0]);
			if (flag3 || flag4)
			{
				return 0L;
			}
			return num - num11;
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x004E9C74 File Offset: 0x004E7E74
		public static bool TryPlacingInChest(Item[] inv, int slot, bool justCheck, int itemSlotContext)
		{
			Item item = inv[slot];
			bool flag = Main.LocalPlayer.chest > -1 && Main.netMode == 1;
			Chest currentContainer = Main.LocalPlayer.GetCurrentContainer();
			Item[] item2 = currentContainer.item;
			if (ChestUI.IsBlockedFromTransferIntoChest(item, item2))
			{
				return false;
			}
			Player player = Main.player[Main.myPlayer];
			bool flag2 = false;
			if (item.maxStack > 1)
			{
				for (int i = 0; i < currentContainer.maxItems; i++)
				{
					if (item2[i].stack < item2[i].maxStack && Item.CanStack(item, item2[i]))
					{
						int num = item.stack;
						if (item.stack + item2[i].stack > item2[i].maxStack)
						{
							num = item2[i].maxStack - item2[i].stack;
						}
						if (justCheck)
						{
							flag2 = (flag2 || num > 0);
							break;
						}
						item.stack -= num;
						item2[i].stack += num;
						SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
						if (item.stack <= 0)
						{
							item.SetDefaults(0, null);
							if (flag)
							{
								NetMessage.SendData(32, -1, -1, null, player.chest, (float)i, 0f, 0f, 0, 0, 0);
								break;
							}
							break;
						}
						else
						{
							if (item2[i].type == 0)
							{
								item2[i] = item.Clone();
								item.SetDefaults(0, null);
							}
							if (flag)
							{
								NetMessage.SendData(32, -1, -1, null, player.chest, (float)i, 0f, 0f, 0, 0, 0);
							}
						}
					}
				}
			}
			if (item.stack > 0)
			{
				int toContext = 3;
				int j = 0;
				while (j < currentContainer.maxItems)
				{
					if (item2[j].stack == 0)
					{
						if (justCheck)
						{
							flag2 = true;
							break;
						}
						SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
						item2[j] = item.Clone();
						item.SetDefaults(0, null);
						ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(item2[j], itemSlotContext, toContext, 0));
						ItemSlot.DisplayTransfer_OneWay(inv, itemSlotContext, slot, item2, toContext, j, item2[j].stack);
						if (flag)
						{
							NetMessage.SendData(32, -1, -1, null, player.chest, (float)j, 0f, 0f, 0, 0, 0);
							break;
						}
						break;
					}
					else
					{
						j++;
					}
				}
			}
			return flag2;
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x004E9ECC File Offset: 0x004E80CC
		public static bool IsBlockedFromTransferIntoChest(Item item, Item[] container)
		{
			return (item.type == 3213 && item.favorited && container == Main.LocalPlayer.bank.item) || ((item.type == 4131 || item.type == 5325) && item.favorited && container == Main.LocalPlayer.bank4.item);
		}

		// Token: 0x0400132F RID: 4911
		public const float buttonScaleMinimum = 0.75f;

		// Token: 0x04001330 RID: 4912
		public const float buttonScaleMaximum = 1f;

		// Token: 0x04001331 RID: 4913
		public static float[] ButtonScale = new float[ChestUI.ButtonID.Count];

		// Token: 0x04001332 RID: 4914
		public static bool[] ButtonHovered = new bool[ChestUI.ButtonID.Count];

		// Token: 0x04001333 RID: 4915
		public static int StartingRowForDrawing = 0;

		// Token: 0x04001334 RID: 4916
		public static int LastHighestChestRow = 0;

		// Token: 0x04001335 RID: 4917
		public static Rectangle LastChestDisplayRectangle;

		// Token: 0x02000708 RID: 1800
		public class ButtonID
		{
			// Token: 0x0400687F RID: 26751
			public const int LootAll = 0;

			// Token: 0x04006880 RID: 26752
			public const int DepositAll = 1;

			// Token: 0x04006881 RID: 26753
			public const int QuickStack = 2;

			// Token: 0x04006882 RID: 26754
			public const int Restock = 3;

			// Token: 0x04006883 RID: 26755
			public const int Sort = 4;

			// Token: 0x04006884 RID: 26756
			public const int RenameChest = 5;

			// Token: 0x04006885 RID: 26757
			public const int RenameChestCancel = 6;

			// Token: 0x04006886 RID: 26758
			public static readonly int Count = 7;
		}
	}
}
