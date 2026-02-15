using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x02000409 RID: 1033
	public class UIWorldListItem : AWorldListItem
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06002F5C RID: 12124 RVA: 0x005B1D15 File Offset: 0x005AFF15
		public bool IsFavorite
		{
			get
			{
				return this._data.IsFavorite;
			}
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x005B1D24 File Offset: 0x005AFF24
		public UIWorldListItem(WorldFileData data, int orderInList, bool canBePlayed, bool hasBeenPlayedByActivePlayer, bool isNewlyGenerated)
		{
			this._orderInList = orderInList;
			this._data = data;
			this._canBePlayed = canBePlayed;
			this._hasBeenPlayedByActivePlayer = hasBeenPlayedByActivePlayer;
			this._isNewlyGenerated = isNewlyGenerated;
			this.LoadTextures();
			this.InitializeAppearance();
			this._worldIcon = base.GetIconElement();
			this._worldIcon.OnLeftDoubleClick += this.PlayGame;
			base.Append(this._worldIcon);
			if (this._data.DefeatedMoonlord)
			{
				UIImage element = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/IconCompletion", 1))
				{
					HAlign = 0.5f,
					VAlign = 0.5f,
					Top = new StyleDimension(-10f, 0f),
					Left = new StyleDimension(-3f, 0f),
					IgnoresMouseInteraction = true
				};
				this._worldIcon.Append(element);
			}
			if (base.GetIcons().Count >= 2 && !this._data.ZenithWorld)
			{
				UIImage element2 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/IconMixedSeed", 1))
				{
					HAlign = 0f,
					VAlign = 1f,
					Top = StyleDimension.FromPixels(0f),
					Left = StyleDimension.FromPixels(0f)
				};
				this._worldIcon.Append(element2);
			}
			float num = 4f;
			UIImageButton uiimageButton = new UIImageButton(this._buttonPlayTexture, null);
			uiimageButton.VAlign = 1f;
			uiimageButton.Left.Set(num, 0f);
			uiimageButton.OnLeftClick += this.PlayGame;
			base.OnLeftDoubleClick += this.PlayGame;
			uiimageButton.OnMouseOver += this.PlayMouseOver;
			uiimageButton.OnMouseOut += this.ButtonMouseOut;
			base.Append(uiimageButton);
			num += 24f;
			UIImageButton uiimageButton2 = new UIImageButton(this._data.IsFavorite ? this._buttonFavoriteActiveTexture : this._buttonFavoriteInactiveTexture, null);
			uiimageButton2.VAlign = 1f;
			uiimageButton2.Left.Set(num, 0f);
			uiimageButton2.OnLeftClick += this.FavoriteButtonClick;
			uiimageButton2.OnMouseOver += this.FavoriteMouseOver;
			uiimageButton2.OnMouseOut += this.ButtonMouseOut;
			uiimageButton2.SetVisibility(1f, this._data.IsFavorite ? 0.8f : 0.4f);
			base.Append(uiimageButton2);
			num += 24f;
			if (SocialAPI.Cloud != null)
			{
				UIImageButton uiimageButton3 = new UIImageButton(this._data.IsCloudSave ? this._buttonCloudActiveTexture : this._buttonCloudInactiveTexture, null);
				uiimageButton3.VAlign = 1f;
				uiimageButton3.Left.Set(num, 0f);
				uiimageButton3.OnLeftClick += this.CloudButtonClick;
				uiimageButton3.OnMouseOver += this.CloudMouseOver;
				uiimageButton3.OnMouseOut += this.ButtonMouseOut;
				uiimageButton3.SetSnapPoint("Cloud", orderInList, null, null);
				base.Append(uiimageButton3);
				num += 24f;
			}
			if (this._data.WorldGeneratorVersion != 0UL)
			{
				UIImageButton uiimageButton4 = new UIImageButton(this._buttonSeedTexture, null);
				uiimageButton4.VAlign = 1f;
				uiimageButton4.Left.Set(num, 0f);
				uiimageButton4.OnLeftClick += this.SeedButtonClick;
				uiimageButton4.OnMouseOver += this.SeedMouseOver;
				uiimageButton4.OnMouseOut += this.ButtonMouseOut;
				uiimageButton4.SetSnapPoint("Seed", orderInList, null, null);
				base.Append(uiimageButton4);
				num += 24f;
			}
			UIImageButton uiimageButton5 = new UIImageButton(this._buttonRenameTexture, null);
			uiimageButton5.VAlign = 1f;
			uiimageButton5.Left.Set(num, 0f);
			uiimageButton5.OnLeftClick += this.RenameButtonClick;
			uiimageButton5.OnMouseOver += this.RenameMouseOver;
			uiimageButton5.OnMouseOut += this.ButtonMouseOut;
			uiimageButton5.SetSnapPoint("Rename", orderInList, null, null);
			base.Append(uiimageButton5);
			num += 24f;
			UIImageButton uiimageButton6 = new UIImageButton(this._buttonDeleteTexture, null);
			uiimageButton6.VAlign = 1f;
			uiimageButton6.HAlign = 1f;
			if (!this._data.IsFavorite)
			{
				uiimageButton6.OnLeftClick += this.DeleteButtonClick;
			}
			uiimageButton6.OnMouseOver += this.DeleteMouseOver;
			uiimageButton6.OnMouseOut += this.DeleteMouseOut;
			this._deleteButton = uiimageButton6;
			base.Append(uiimageButton6);
			num += 4f;
			this._buttonLabel = new UIText("", 1f, false);
			this._buttonLabel.VAlign = 1f;
			this._buttonLabel.Left.Set(num, 0f);
			this._buttonLabel.Top.Set(-3f, 0f);
			base.Append(this._buttonLabel);
			this._deleteButtonLabel = new UIText("", 1f, false);
			this._deleteButtonLabel.VAlign = 1f;
			this._deleteButtonLabel.HAlign = 1f;
			this._deleteButtonLabel.Left.Set(-30f, 0f);
			this._deleteButtonLabel.Top.Set(-3f, 0f);
			base.Append(this._deleteButtonLabel);
			int num2 = 0;
			if (this._hasBeenPlayedByActivePlayer)
			{
				UIImage uiimage = new UIImage(this._hasBeenPlayedByActivePlayerTexture)
				{
					HAlign = 1f,
					Left = new StyleDimension((float)num2, 0f),
					Top = new StyleDimension(-6f, 0f),
					ImageScale = 0.75f,
					UseTextureSizeForOrigin = false
				};
				uiimage.OnMouseOver += this.HasPlayedMouseOver;
				uiimage.OnMouseOut += this.DeleteMouseOut;
				base.Append(uiimage);
				num2 -= 24;
			}
			if (this._isNewlyGenerated)
			{
				UIImage uiimage2 = new UIImage(this._isNewlyGeneratedTexture)
				{
					HAlign = 1f,
					Left = new StyleDimension((float)(num2 - 2), 0f),
					Top = new StyleDimension(-6f, 0f),
					ImageScale = 0.75f,
					UseTextureSizeForOrigin = false
				};
				uiimage2.OnMouseOver += this.NewlyGeneratedMouseOver;
				uiimage2.OnMouseOut += this.DeleteMouseOut;
				base.Append(uiimage2);
				num2 -= 24;
			}
			uiimageButton.SetSnapPoint("Play", orderInList, null, null);
			uiimageButton2.SetSnapPoint("Favorite", orderInList, null, null);
			uiimageButton5.SetSnapPoint("Rename", orderInList, null, null);
			uiimageButton6.SetSnapPoint("Delete", orderInList, null, null);
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x005B24D8 File Offset: 0x005B06D8
		private void LoadTextures()
		{
			this._dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider", 1);
			this._innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", 1);
			this._buttonCloudActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudActive", 1);
			this._buttonCloudInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudInactive", 1);
			this._buttonFavoriteActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteActive", 1);
			this._buttonFavoriteInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteInactive", 1);
			this._buttonPlayTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", 1);
			this._buttonSeedTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonSeed", 1);
			this._buttonRenameTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonRename", 1);
			this._buttonDeleteTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", 1);
			this._hasBeenPlayedByActivePlayerTexture = Main.Assets.Request<Texture2D>("Images/UI/IconPlayedBefore", 1);
			this._isNewlyGeneratedTexture = Main.Assets.Request<Texture2D>("Images/UI/IconNewlyGenerated", 1);
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x005B25ED File Offset: 0x005B07ED
		private void InitializeAppearance()
		{
			this.Height.Set(96f, 0f);
			this.Width.Set(0f, 1f);
			base.SetPadding(6f);
			this.SetColorsToNotHovered();
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x005B262C File Offset: 0x005B082C
		private void SetColorsToHovered()
		{
			this.BackgroundColor = new Color(73, 94, 171);
			this.BorderColor = new Color(89, 116, 213);
			if (!this._canBePlayed)
			{
				this.BorderColor = new Color(150, 150, 150) * 1f;
				this.BackgroundColor = Color.Lerp(this.BackgroundColor, new Color(120, 120, 120), 0.5f) * 1f;
			}
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x005B26B8 File Offset: 0x005B08B8
		private void SetColorsToNotHovered()
		{
			this.BackgroundColor = new Color(63, 82, 151) * 0.7f;
			this.BorderColor = new Color(89, 116, 213) * 0.7f;
			if (!this._canBePlayed)
			{
				this.BorderColor = new Color(127, 127, 127) * 0.7f;
				this.BackgroundColor = Color.Lerp(new Color(63, 82, 151), new Color(80, 80, 80), 0.5f) * 0.7f;
			}
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x005B2757 File Offset: 0x005B0957
		private void RenameMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonLabel.SetText(Language.GetTextValue("UI.Rename"));
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x005B276E File Offset: 0x005B096E
		private void FavoriteMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			if (this._data.IsFavorite)
			{
				this._buttonLabel.SetText(Language.GetTextValue("UI.Unfavorite"));
				return;
			}
			this._buttonLabel.SetText(Language.GetTextValue("UI.Favorite"));
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x005B27A8 File Offset: 0x005B09A8
		private void CloudMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			if (this._data.IsCloudSave)
			{
				this._buttonLabel.SetText(Language.GetTextValue("UI.MoveOffCloud"));
				return;
			}
			this._buttonLabel.SetText(Language.GetTextValue("UI.MoveToCloud"));
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x005B27E2 File Offset: 0x005B09E2
		private void PlayMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonLabel.SetText(Language.GetTextValue("UI.Play"));
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x005B27F9 File Offset: 0x005B09F9
		private void SeedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonLabel.SetText(Language.GetTextValue("UI.CopySeed", this._data.GetFullSeedText(true)));
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x005B281C File Offset: 0x005B0A1C
		private void DeleteMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			if (this._data.IsFavorite)
			{
				this._deleteButtonLabel.SetText(Language.GetTextValue("UI.CannotDeleteFavorited"));
				return;
			}
			this._deleteButtonLabel.SetText(Language.GetTextValue("UI.Delete"));
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x005B2856 File Offset: 0x005B0A56
		private void DeleteMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			this._deleteButtonLabel.SetText("");
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x005B2868 File Offset: 0x005B0A68
		private void NewlyGeneratedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			this._deleteButtonLabel.SetText(Language.GetTextValue("UI.WorldNewlyGenerated"));
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x005B287F File Offset: 0x005B0A7F
		private void HasPlayedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			this._deleteButtonLabel.SetText(Language.GetTextValue("UI.WorldHasBeenPlayed"));
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x005B2896 File Offset: 0x005B0A96
		private void ButtonMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonLabel.SetText("");
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x005B28A8 File Offset: 0x005B0AA8
		private void CloudButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (this._data.IsCloudSave)
			{
				this._data.MoveToLocal();
			}
			else
			{
				this._data.MoveToCloud();
			}
			((UIImageButton)evt.Target).SetImage(this._data.IsCloudSave ? this._buttonCloudActiveTexture : this._buttonCloudInactiveTexture, null);
			if (this._data.IsCloudSave)
			{
				this._buttonLabel.SetText(Language.GetTextValue("UI.MoveOffCloud"));
				return;
			}
			this._buttonLabel.SetText(Language.GetTextValue("UI.MoveToCloud"));
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x005B2948 File Offset: 0x005B0B48
		private void DeleteButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			for (int i = 0; i < Main.WorldList.Count; i++)
			{
				if (Main.WorldList[i] == this._data)
				{
					SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
					Main.selectedWorld = i;
					Main.menuMode = 9;
					return;
				}
			}
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x005B29A0 File Offset: 0x005B0BA0
		private void PlayGame(UIMouseEvent evt, UIElement listeningElement)
		{
			if (listeningElement != evt.Target)
			{
				return;
			}
			if (!this._data.IsValid)
			{
				return;
			}
			if (this.TryMovingToRejectionMenuIfNeeded(this._data.GameMode))
			{
				return;
			}
			this._data.SetAsActive();
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.clrInput();
			Main.GetInputText("", false);
			if (Main.menuMultiplayer && SocialAPI.Network != null)
			{
				Main.menuMode = 889;
			}
			else if (Main.menuMultiplayer)
			{
				Main.menuMode = 30;
			}
			else
			{
				Main.menuMode = 10;
			}
			if (!Main.menuMultiplayer)
			{
				WorldGen.playWorld();
			}
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x005B2A48 File Offset: 0x005B0C48
		private bool TryMovingToRejectionMenuIfNeeded(int worldGameMode)
		{
			if (!GameModeID.IsValid(worldGameMode))
			{
				SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
				Main.statusText = Language.GetTextValue("UI.WorldCannotBeLoadedBecauseItHasAnInvalidGameMode");
				Main.menuMode = 1000000;
				return true;
			}
			bool flag = Main.ActivePlayerFileData.Player.difficulty == 3;
			bool flag2 = worldGameMode == 3;
			if (flag && !flag2)
			{
				SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
				Main.statusText = Language.GetTextValue("UI.PlayerIsCreativeAndWorldIsNotCreative");
				Main.menuMode = 1000000;
				return true;
			}
			if (!flag && flag2)
			{
				SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
				Main.statusText = Language.GetTextValue("UI.PlayerIsNotCreativeAndWorldIsCreative");
				Main.menuMode = 1000000;
				return true;
			}
			return false;
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x005B2B14 File Offset: 0x005B0D14
		private void RenameButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.clrInput();
			UIVirtualKeyboard state = new UIVirtualKeyboard(Lang.menu[48].Value, this._data.GetWorldName(false), new UIVirtualKeyboard.KeyboardSubmitEvent(this.OnFinishedSettingName), new Action(this.GoBackHere), 0, true, 27);
			Main.MenuUI.SetState(state);
			UIList uilist = base.Parent.Parent as UIList;
			if (uilist != null)
			{
				uilist.UpdateOrder();
			}
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x005B2B9C File Offset: 0x005B0D9C
		private void OnFinishedSettingName(string name)
		{
			string newDisplayName = name.Trim();
			Main.menuMode = 10;
			this._data.Rename(newDisplayName);
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x005B2BC3 File Offset: 0x005B0DC3
		private void GoBackHere()
		{
			Main.GoToWorldSelect();
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x005B2BCC File Offset: 0x005B0DCC
		private void FavoriteButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			this._data.ToggleFavorite();
			((UIImageButton)evt.Target).SetImage(this._data.IsFavorite ? this._buttonFavoriteActiveTexture : this._buttonFavoriteInactiveTexture, null);
			((UIImageButton)evt.Target).SetVisibility(1f, this._data.IsFavorite ? 0.8f : 0.4f);
			if (this._data.IsFavorite)
			{
				this._buttonLabel.SetText(Language.GetTextValue("UI.Unfavorite"));
				this._deleteButton.OnLeftClick -= this.DeleteButtonClick;
			}
			else
			{
				this._buttonLabel.SetText(Language.GetTextValue("UI.Favorite"));
				this._deleteButton.OnLeftClick += this.DeleteButtonClick;
			}
			UIList uilist = base.Parent.Parent as UIList;
			if (uilist != null)
			{
				uilist.UpdateOrder();
			}
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x005B2CC7 File Offset: 0x005B0EC7
		private void SeedButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			Platform.Get<IClipboard>().Value = this._data.GetFullSeedText(false);
			this._buttonLabel.SetText(Language.GetTextValue("UI.SeedCopied"));
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x005B2CF4 File Offset: 0x005B0EF4
		public override int CompareTo(object obj)
		{
			UIWorldListItem uiworldListItem = obj as UIWorldListItem;
			if (uiworldListItem != null)
			{
				return this._orderInList.CompareTo(uiworldListItem._orderInList);
			}
			return base.CompareTo(obj);
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x005B2D24 File Offset: 0x005B0F24
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			this.SetColorsToHovered();
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x005B2D33 File Offset: 0x005B0F33
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this.SetColorsToNotHovered();
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x005B2D44 File Offset: 0x005B0F44
		private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
		{
			spriteBatch.Draw(this._innerPanelTexture.Value, position, new Rectangle?(new Rectangle(0, 0, 8, this._innerPanelTexture.Height())), Color.White);
			spriteBatch.Draw(this._innerPanelTexture.Value, new Vector2(position.X + 8f, position.Y), new Rectangle?(new Rectangle(8, 0, 8, this._innerPanelTexture.Height())), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(this._innerPanelTexture.Value, new Vector2(position.X + width - 8f, position.Y), new Rectangle?(new Rectangle(16, 0, 8, this._innerPanelTexture.Height())), Color.White);
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x005B2E34 File Offset: 0x005B1034
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			CalculatedStyle dimensions = this._worldIcon.GetDimensions();
			float num = dimensions.X + dimensions.Width;
			Color color = Color.White;
			string text = this._data.GetWorldName(true);
			if (!this._data.IsValid)
			{
				color = Color.Gray;
				string name = StatusID.Search.GetName(this._data.LoadStatus);
				text = "(" + name + ") " + text;
			}
			Utils.DrawBorderString(spriteBatch, text, new Vector2(num + 6f, dimensions.Y - 2f), color, 1f, 0f, 0f, -1);
			spriteBatch.Draw(this._dividerTexture.Value, new Vector2(num, innerDimensions.Y + 21f), null, Color.White, 0f, Vector2.Zero, new Vector2((base.GetDimensions().X + base.GetDimensions().Width - num) / 8f, 1f), SpriteEffects.None, 0f);
			Vector2 vector = new Vector2(num + 6f, innerDimensions.Y + 29f);
			float num2 = 100f;
			this.DrawPanel(spriteBatch, vector, num2);
			string text2;
			Color color2;
			base.GetDifficulty(out text2, out color2);
			float x = FontAssets.MouseText.Value.MeasureString(text2).X;
			float x2 = num2 * 0.5f - x * 0.5f;
			Utils.DrawBorderString(spriteBatch, text2, vector + new Vector2(x2, 3f), color2, 1f, 0f, 0f, -1);
			vector.X += num2 + 5f;
			float num3 = 150f;
			if (!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
			{
				num3 += 40f;
			}
			this.DrawPanel(spriteBatch, vector, num3);
			string textValue = Language.GetTextValue("UI.WorldSizeFormat", this._data.WorldSizeName);
			float x3 = FontAssets.MouseText.Value.MeasureString(textValue).X;
			float x4 = num3 * 0.5f - x3 * 0.5f;
			Utils.DrawBorderString(spriteBatch, textValue, vector + new Vector2(x4, 3f), Color.White, 1f, 0f, 0f, -1);
			vector.X += num3 + 5f;
			float num4 = innerDimensions.X + innerDimensions.Width - vector.X;
			this.DrawPanel(spriteBatch, vector, num4);
			string arg;
			if (GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
			{
				arg = this._data.CreationTime.ToString("d MMMM yyyy");
			}
			else
			{
				arg = this._data.CreationTime.ToShortDateString();
			}
			string textValue2 = Language.GetTextValue("UI.WorldCreatedFormat", arg);
			float x5 = FontAssets.MouseText.Value.MeasureString(textValue2).X;
			float x6 = num4 * 0.5f - x5 * 0.5f;
			Utils.DrawBorderString(spriteBatch, textValue2, vector + new Vector2(x6, 3f), Color.White, 1f, 0f, 0f, -1);
			vector.X += num4 + 5f;
		}

		// Token: 0x04005647 RID: 22087
		private Asset<Texture2D> _dividerTexture;

		// Token: 0x04005648 RID: 22088
		private Asset<Texture2D> _innerPanelTexture;

		// Token: 0x04005649 RID: 22089
		private UIElement _worldIcon;

		// Token: 0x0400564A RID: 22090
		private UIText _buttonLabel;

		// Token: 0x0400564B RID: 22091
		private UIText _deleteButtonLabel;

		// Token: 0x0400564C RID: 22092
		private Asset<Texture2D> _buttonCloudActiveTexture;

		// Token: 0x0400564D RID: 22093
		private Asset<Texture2D> _buttonCloudInactiveTexture;

		// Token: 0x0400564E RID: 22094
		private Asset<Texture2D> _buttonFavoriteActiveTexture;

		// Token: 0x0400564F RID: 22095
		private Asset<Texture2D> _buttonFavoriteInactiveTexture;

		// Token: 0x04005650 RID: 22096
		private Asset<Texture2D> _buttonPlayTexture;

		// Token: 0x04005651 RID: 22097
		private Asset<Texture2D> _buttonSeedTexture;

		// Token: 0x04005652 RID: 22098
		private Asset<Texture2D> _buttonRenameTexture;

		// Token: 0x04005653 RID: 22099
		private Asset<Texture2D> _buttonDeleteTexture;

		// Token: 0x04005654 RID: 22100
		private Asset<Texture2D> _hasBeenPlayedByActivePlayerTexture;

		// Token: 0x04005655 RID: 22101
		private Asset<Texture2D> _isNewlyGeneratedTexture;

		// Token: 0x04005656 RID: 22102
		private UIImageButton _deleteButton;

		// Token: 0x04005657 RID: 22103
		private int _orderInList;

		// Token: 0x04005658 RID: 22104
		private bool _canBePlayed;

		// Token: 0x04005659 RID: 22105
		private bool _hasBeenPlayedByActivePlayer;

		// Token: 0x0400565A RID: 22106
		private bool _isNewlyGenerated;
	}
}
