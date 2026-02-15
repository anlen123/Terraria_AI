using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.Net;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.Creative
{
	// Token: 0x0200031F RID: 799
	public class CreativeUI
	{
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06002798 RID: 10136 RVA: 0x00568075 File Offset: 0x00566275
		// (set) Token: 0x06002799 RID: 10137 RVA: 0x0056807D File Offset: 0x0056627D
		public bool Enabled { get; private set; }

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x0600279A RID: 10138 RVA: 0x00568086 File Offset: 0x00566286
		public bool Blocked
		{
			get
			{
				return Main.LocalPlayer.talkNPC != -1 || (!NewCraftingUI.Visible && (Main.LocalPlayer.chest != -1 || Main.LocalPlayer.tileEntityAnchor.IsInValidUseTileEntity()));
			}
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x005680C0 File Offset: 0x005662C0
		public CreativeUI()
		{
			for (int i = 0; i < this._itemSlotsForUI.Length; i++)
			{
				this._itemSlotsForUI[i] = new Item();
			}
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x0056810C File Offset: 0x0056630C
		public void Initialize()
		{
			this._buttonTexture = Main.Assets.Request<Texture2D>("Images/UI/Creative/Journey_Toggle", 1);
			this._buttonBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Creative/Journey_Toggle_MouseOver", 1);
			this._uiState = new UICreativePowersMenu();
			this._powersUI.SetState(this._uiState);
			this._initialized = true;
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x00568168 File Offset: 0x00566368
		public void Update(GameTime gameTime)
		{
			if (!this.Enabled)
			{
				return;
			}
			if (!Main.playerInventory)
			{
				return;
			}
			this._powersUI.Update(gameTime);
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x00568188 File Offset: 0x00566388
		public void Draw(SpriteBatch spriteBatch)
		{
			if (!this._initialized)
			{
				this.Initialize();
			}
			if (Main.LocalPlayer.difficulty != 3)
			{
				this.Enabled = false;
				return;
			}
			if (this.Blocked)
			{
				return;
			}
			Vector2 location = new Vector2(28f, 267f);
			Vector2 value = new Vector2(353f, 258f);
			new Vector2(40f, 267f);
			value + new Vector2(50f, 50f);
			if (Main.screenHeight < 650 && this.Enabled)
			{
				location.X += 52f * Main.inventoryScale;
			}
			this.DrawToggleButton(spriteBatch, location);
			if (!this.Enabled)
			{
				return;
			}
			this._powersUI.Draw(spriteBatch, Main.gameTimeCache);
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x00568252 File Offset: 0x00566452
		public UIElement ProvideItemSlotElement(int itemSlotContext)
		{
			if (itemSlotContext != 0)
			{
				return null;
			}
			return new UIItemSlot(this._itemSlotsForUI, itemSlotContext, 30);
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x00568267 File Offset: 0x00566467
		public Item GetItemByIndex(int itemSlotContext)
		{
			if (itemSlotContext != 0)
			{
				return null;
			}
			return this._itemSlotsForUI[itemSlotContext];
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x00568276 File Offset: 0x00566476
		public void SetItembyIndex(Item item, int itemSlotContext)
		{
			if (itemSlotContext == 0)
			{
				this._itemSlotsForUI[itemSlotContext] = item;
			}
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x00568284 File Offset: 0x00566484
		private void DrawToggleButton(SpriteBatch spritebatch, Vector2 location)
		{
			Vector2 vector = this._buttonTexture.Size();
			Rectangle hitbox = Utils.CenteredRectangle(location + vector / 2f, vector);
			UILinkPointNavigator.SetPosition(311, hitbox.Center.ToVector2());
			spritebatch.Draw(this._buttonTexture.Value, location, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Main.LocalPlayer.creativeInterface = false;
			if (hitbox.Contains(Main.MouseScreen.ToPoint()) && !PlayerInput.IgnoreMouseInterface)
			{
				Main.LocalPlayer.creativeInterface = true;
				Main.LocalPlayer.mouseInterface = true;
				if (this.Enabled)
				{
					Main.instance.MouseTextNoOverride(Language.GetTextValue("CreativePowers.PowersMenuOpen"), 0, 0, -1, -1, -1, -1, 0);
				}
				else
				{
					Main.instance.MouseTextNoOverride(Language.GetTextValue("CreativePowers.PowersMenuClosed"), 0, 0, -1, -1, -1, -1, 0);
				}
				spritebatch.Draw(this._buttonBorderTexture.Value, location, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					this.ToggleMenu();
				}
			}
			Main.DoStatefulTickSound(ref Main.CreativeMenuMouseOver, Main.LocalPlayer.creativeInterface);
			if (Main.LocalPlayerCreativeTracker.ItemSacrifices.AnyNewUnlocksFromTeammates)
			{
				Utils.DrawNotificationIcon(spritebatch, hitbox, 1f, false);
			}
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x005683FC File Offset: 0x005665FC
		public void SwapItem(ref Item item)
		{
			Utils.Swap<Item>(ref item, ref this._itemSlotsForUI[0]);
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x00568410 File Offset: 0x00566610
		public void CloseMenu()
		{
			if (!this.Enabled)
			{
				return;
			}
			this.Enabled = false;
			this.StopPlayingSacrificeAnimations();
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x00568428 File Offset: 0x00566628
		public void ResumeMenuFromGamepadSearch()
		{
			this.Enabled = true;
			this.GamepadMoveToSearchButtonHack = true;
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x00568438 File Offset: 0x00566638
		public void ToggleMenu()
		{
			this.Enabled = !this.Enabled;
			this._powersUI.EscapeElements();
			UISliderBase.EscapeElements();
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			if (this.Enabled)
			{
				NewCraftingUI.Close(true, true);
				Main.LocalPlayer.chest = -1;
				Main.LocalPlayer.tileEntityAnchor.Clear();
				UILinkPointNavigator.ChangePoint(10000);
				return;
			}
			if (this._itemSlotsForUI[0].stack > 0)
			{
				Main.LocalPlayer.GetOrDropItem(this._itemSlotsForUI[0], GetItemSettings.ReturnItemFromSlot);
				this.StopPlayingSacrificeAnimations();
			}
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x005684DA File Offset: 0x005666DA
		public bool IsShowingResearchMenu()
		{
			return this.Enabled && this._uiState != null && this._uiState.IsShowingResearchMenu;
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x005684F9 File Offset: 0x005666F9
		public void SacrificeItemInSacrificeSlot()
		{
			if (this._uiState == null)
			{
				return;
			}
			this._uiState.SacrificeWhatsInResearchMenu();
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x0056850F File Offset: 0x0056670F
		public void StopPlayingSacrificeAnimations()
		{
			if (this._uiState == null)
			{
				return;
			}
			this._uiState.StopPlayingResearchAnimations();
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x00568528 File Offset: 0x00566728
		public bool ShouldDrawSacrificeArea()
		{
			if (!this._itemSlotsForUI[0].IsAir)
			{
				return true;
			}
			Item mouseItem = Main.mouseItem;
			int num;
			return !mouseItem.IsAir && CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(mouseItem.type, out num) && Main.LocalPlayerCreativeTracker.ItemSacrifices.GetSacrificeCount(mouseItem.type) < num;
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x00568588 File Offset: 0x00566788
		public bool GetSacrificeNumbers(out int itemIdChecked, out int amountWeHave, out int amountNeededTotal)
		{
			amountWeHave = 0;
			amountNeededTotal = 0;
			itemIdChecked = 0;
			Item item = this._itemSlotsForUI[0];
			if (!item.IsAir)
			{
				itemIdChecked = item.type;
			}
			return Main.LocalPlayerCreativeTracker.ItemSacrifices.TryGetSacrificeNumbers(item.type, out amountWeHave, out amountNeededTotal);
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x005685D3 File Offset: 0x005667D3
		public CreativeUI.ItemSacrificeResult SacrificeItem(out int amountWeSacrificed)
		{
			return this.SacrificeItem(ref this._itemSlotsForUI[0], out amountWeSacrificed, true, false);
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x005685EC File Offset: 0x005667EC
		public CreativeUI.ItemSacrificeResult SacrificeItem(ref Item item, out int amountWeSacrificed, bool spawnExcessItem = true, bool onlySacrificeIfItWouldFinishResearch = false)
		{
			int num = 0;
			int num2 = 0;
			amountWeSacrificed = 0;
			if (!Main.LocalPlayerCreativeTracker.ItemSacrifices.TryGetSacrificeNumbers(item.type, out num2, out num))
			{
				return CreativeUI.ItemSacrificeResult.CannotSacrifice;
			}
			int num3 = Utils.Clamp<int>(num - num2, 0, num);
			if (num3 == 0)
			{
				return CreativeUI.ItemSacrificeResult.CannotSacrifice;
			}
			int num4 = Math.Min(num3, item.stack);
			bool flag = num4 == num3;
			if (onlySacrificeIfItWouldFinishResearch && !flag)
			{
				return CreativeUI.ItemSacrificeResult.CannotSacrifice;
			}
			NetPacket packet = NetCreativeUnlocksPlayerReportModule.SerializeSacrificeRequest(Main.myPlayer, item.type, num4);
			NetManager.Instance.SendToServer(packet);
			if (!Main.ServerSideCharacter)
			{
				Main.LocalPlayerCreativeTracker.ItemSacrifices.RegisterItemSacrifice(item.type, num4, null);
			}
			item.stack -= num4;
			if (item.stack <= 0)
			{
				item.TurnToAir(false);
			}
			amountWeSacrificed = num4;
			if (item.stack > 0 && spawnExcessItem)
			{
				item = Main.LocalPlayer.GetItem(item, GetItemSettings.ReturnItemFromSlot);
			}
			if (!flag)
			{
				return CreativeUI.ItemSacrificeResult.SacrificedButNotDone;
			}
			return CreativeUI.ItemSacrificeResult.SacrificedAndDone;
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x005686D8 File Offset: 0x005668D8
		public void Reset()
		{
			for (int i = 0; i < this._itemSlotsForUI.Length; i++)
			{
				this._itemSlotsForUI[i].TurnToAir(false);
			}
			this._initialized = false;
			this.Enabled = false;
		}

		// Token: 0x040050CF RID: 20687
		public const int ItemSlotIndexes_SacrificeItem = 0;

		// Token: 0x040050D0 RID: 20688
		public const int ItemSlotIndexes_Count = 1;

		// Token: 0x040050D2 RID: 20690
		private bool _initialized;

		// Token: 0x040050D3 RID: 20691
		private Asset<Texture2D> _buttonTexture;

		// Token: 0x040050D4 RID: 20692
		private Asset<Texture2D> _buttonBorderTexture;

		// Token: 0x040050D5 RID: 20693
		private Item[] _itemSlotsForUI = new Item[1];

		// Token: 0x040050D6 RID: 20694
		private UserInterface _powersUI = new UserInterface();

		// Token: 0x040050D7 RID: 20695
		public bool GamepadMoveToSearchButtonHack;

		// Token: 0x040050D8 RID: 20696
		private UICreativePowersMenu _uiState;

		// Token: 0x0200087E RID: 2174
		public enum ItemSacrificeResult
		{
			// Token: 0x04007270 RID: 29296
			CannotSacrifice,
			// Token: 0x04007271 RID: 29297
			SacrificedButNotDone,
			// Token: 0x04007272 RID: 29298
			SacrificedAndDone
		}
	}
}
