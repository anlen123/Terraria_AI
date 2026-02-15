using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003FD RID: 1021
	public class UICharacterListItem : UIPanel
	{
		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06002ED6 RID: 11990 RVA: 0x005AEBBD File Offset: 0x005ACDBD
		public bool IsFavorite
		{
			get
			{
				return this._data.IsFavorite;
			}
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x005AEBCC File Offset: 0x005ACDCC
		public UICharacterListItem(PlayerFileData data, int orderInList)
		{
			this.BorderColor = new Color(89, 116, 213) * 0.7f;
			this._dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider", 1);
			this._innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", 1);
			this._buttonCloudActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudActive", 1);
			this._buttonCloudInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonCloudInactive", 1);
			this._buttonFavoriteActiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteActive", 1);
			this._buttonFavoriteInactiveTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteInactive", 1);
			this._buttonPlayTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", 1);
			this._buttonRenameTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonRename", 1);
			this._buttonDeleteTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", 1);
			this.Height.Set(96f, 0f);
			this.Width.Set(0f, 1f);
			base.SetPadding(6f);
			this._data = data;
			this._orderInList = orderInList;
			this._playerPanel = new UICharacter(data.Player, false, true, 1f, true);
			this._playerPanel.Left.Set(4f, 0f);
			this._playerPanel.OnLeftDoubleClick += this.PlayGame;
			base.OnLeftDoubleClick += this.PlayGame;
			base.Append(this._playerPanel);
			float num = 4f;
			UIImageButton uiimageButton = new UIImageButton(this._buttonPlayTexture, null);
			uiimageButton.VAlign = 1f;
			uiimageButton.Left.Set(num, 0f);
			uiimageButton.OnLeftClick += this.PlayGame;
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
				base.Append(uiimageButton3);
				uiimageButton3.SetSnapPoint("Cloud", orderInList, null, null);
				num += 24f;
			}
			UIImageButton uiimageButton4 = new UIImageButton(this._buttonRenameTexture, null);
			uiimageButton4.VAlign = 1f;
			uiimageButton4.Left.Set(num, 0f);
			uiimageButton4.OnLeftClick += this.RenameButtonClick;
			uiimageButton4.OnMouseOver += this.RenameMouseOver;
			uiimageButton4.OnMouseOut += this.ButtonMouseOut;
			base.Append(uiimageButton4);
			num += 24f;
			UIImageButton uiimageButton5 = new UIImageButton(this._buttonDeleteTexture, null);
			uiimageButton5.VAlign = 1f;
			uiimageButton5.HAlign = 1f;
			if (!this._data.IsFavorite)
			{
				uiimageButton5.OnLeftClick += this.DeleteButtonClick;
			}
			uiimageButton5.OnMouseOver += this.DeleteMouseOver;
			uiimageButton5.OnMouseOut += this.DeleteMouseOut;
			this._deleteButton = uiimageButton5;
			base.Append(uiimageButton5);
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
			uiimageButton.SetSnapPoint("Play", orderInList, null, null);
			uiimageButton2.SetSnapPoint("Favorite", orderInList, null, null);
			uiimageButton4.SetSnapPoint("Rename", orderInList, null, null);
			uiimageButton5.SetSnapPoint("Delete", orderInList, null, null);
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x005AF1B6 File Offset: 0x005AD3B6
		private void RenameMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonLabel.SetText(Language.GetTextValue("UI.Rename"));
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x005AF1CD File Offset: 0x005AD3CD
		private void FavoriteMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			if (this._data.IsFavorite)
			{
				this._buttonLabel.SetText(Language.GetTextValue("UI.Unfavorite"));
				return;
			}
			this._buttonLabel.SetText(Language.GetTextValue("UI.Favorite"));
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x005AF207 File Offset: 0x005AD407
		private void CloudMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			if (this._data.IsCloudSave)
			{
				this._buttonLabel.SetText(Language.GetTextValue("UI.MoveOffCloud"));
				return;
			}
			this._buttonLabel.SetText(Language.GetTextValue("UI.MoveToCloud"));
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x005AF241 File Offset: 0x005AD441
		private void PlayMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonLabel.SetText(Language.GetTextValue("UI.Play"));
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x005AF258 File Offset: 0x005AD458
		private void DeleteMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			if (this._data.IsFavorite)
			{
				this._deleteButtonLabel.SetText(Language.GetTextValue("UI.CannotDeleteFavorited"));
				return;
			}
			this._deleteButtonLabel.SetText(Language.GetTextValue("UI.Delete"));
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x005AF292 File Offset: 0x005AD492
		private void DeleteMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			this._deleteButtonLabel.SetText("");
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x005AF2A4 File Offset: 0x005AD4A4
		private void ButtonMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonLabel.SetText("");
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x005AF2B8 File Offset: 0x005AD4B8
		private void RenameButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.clrInput();
			UIVirtualKeyboard state = new UIVirtualKeyboard(Lang.menu[45].Value, this._data.Name, new UIVirtualKeyboard.KeyboardSubmitEvent(this.OnFinishedSettingName), new Action(this.GoBackHere), 0, true, 20);
			Main.MenuUI.SetState(state);
			UIList uilist = base.Parent.Parent as UIList;
			if (uilist != null)
			{
				uilist.UpdateOrder();
			}
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x005AF340 File Offset: 0x005AD540
		private void OnFinishedSettingName(string name)
		{
			string newName = name.Trim();
			Main.menuMode = 10;
			this._data.Rename(newName);
			Main.OpenCharacterSelectUI();
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x005AF36C File Offset: 0x005AD56C
		private void GoBackHere()
		{
			Main.OpenCharacterSelectUI();
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x005AF374 File Offset: 0x005AD574
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

		// Token: 0x06002EE3 RID: 12003 RVA: 0x005AF414 File Offset: 0x005AD614
		private void DeleteButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			for (int i = 0; i < Main.PlayerList.Count; i++)
			{
				if (Main.PlayerList[i] == this._data)
				{
					SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
					Main.selectedPlayer = i;
					Main.menuMode = 5;
					return;
				}
			}
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x005AF46B File Offset: 0x005AD66B
		private void PlayGame(UIMouseEvent evt, UIElement listeningElement)
		{
			if (listeningElement != evt.Target)
			{
				return;
			}
			if (this._data.Player.loadStatus == StatusID.Ok)
			{
				Main.SelectPlayer(this._data);
			}
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x005AF49C File Offset: 0x005AD69C
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

		// Token: 0x06002EE6 RID: 12006 RVA: 0x005AF598 File Offset: 0x005AD798
		public override int CompareTo(object obj)
		{
			UICharacterListItem uicharacterListItem = obj as UICharacterListItem;
			if (uicharacterListItem != null)
			{
				return this._orderInList.CompareTo(uicharacterListItem._orderInList);
			}
			return base.CompareTo(obj);
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x005AF5C8 File Offset: 0x005AD7C8
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			this.BackgroundColor = new Color(73, 94, 171);
			this.BorderColor = new Color(89, 116, 213);
			this._playerPanel.SetAnimated(true);
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x005AF608 File Offset: 0x005AD808
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this.BackgroundColor = new Color(63, 82, 151) * 0.7f;
			this.BorderColor = new Color(89, 116, 213) * 0.7f;
			this._playerPanel.SetAnimated(false);
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x005AF664 File Offset: 0x005AD864
		private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
		{
			spriteBatch.Draw(this._innerPanelTexture.Value, position, new Rectangle?(new Rectangle(0, 0, 8, this._innerPanelTexture.Height())), Color.White);
			spriteBatch.Draw(this._innerPanelTexture.Value, new Vector2(position.X + 8f, position.Y), new Rectangle?(new Rectangle(8, 0, 8, this._innerPanelTexture.Height())), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(this._innerPanelTexture.Value, new Vector2(position.X + width - 8f, position.Y), new Rectangle?(new Rectangle(16, 0, 8, this._innerPanelTexture.Height())), Color.White);
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x005AF754 File Offset: 0x005AD954
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			CalculatedStyle dimensions = this._playerPanel.GetDimensions();
			float num = dimensions.X + dimensions.Width;
			Color color = Color.White;
			string text = this._data.Name;
			if (this._data.Player.loadStatus != StatusID.Ok)
			{
				color = Color.Gray;
				string name = StatusID.Search.GetName(this._data.Player.loadStatus);
				text = "(" + name + ") " + text;
			}
			Utils.DrawBorderString(spriteBatch, text, new Vector2(num + 6f, dimensions.Y - 2f), color, 1f, 0f, 0f, -1);
			spriteBatch.Draw(this._dividerTexture.Value, new Vector2(num, innerDimensions.Y + 21f), null, Color.White, 0f, Vector2.Zero, new Vector2((base.GetDimensions().X + base.GetDimensions().Width - num) / 8f, 1f), SpriteEffects.None, 0f);
			Vector2 vector = new Vector2(num + 6f, innerDimensions.Y + 29f);
			float num2 = 200f;
			Vector2 vector2 = vector;
			this.DrawPanel(spriteBatch, vector2, num2);
			spriteBatch.Draw(TextureAssets.Heart.Value, vector2 + new Vector2(5f, 2f), Color.White);
			vector2.X += 10f + (float)TextureAssets.Heart.Width();
			Utils.DrawBorderString(spriteBatch, this._data.Player.statLifeMax + Language.GetTextValue("GameUI.PlayerLifeMax"), vector2 + new Vector2(0f, 3f), Color.White, 1f, 0f, 0f, -1);
			vector2.X += 65f;
			spriteBatch.Draw(TextureAssets.Mana.Value, vector2 + new Vector2(5f, 2f), Color.White);
			vector2.X += 10f + (float)TextureAssets.Mana.Width();
			Utils.DrawBorderString(spriteBatch, this._data.Player.statManaMax + Language.GetTextValue("GameUI.PlayerManaMax"), vector2 + new Vector2(0f, 3f), Color.White, 1f, 0f, 0f, -1);
			vector.X += num2 + 5f;
			Vector2 vector3 = vector;
			float num3 = 140f;
			if (GameCulture.FromCultureName(GameCulture.CultureName.Russian).IsActive)
			{
				num3 = 180f;
			}
			this.DrawPanel(spriteBatch, vector3, num3);
			string text2 = "";
			Color color2 = Color.White;
			switch (this._data.Player.difficulty)
			{
			case 0:
				text2 = Language.GetTextValue("UI.Softcore");
				break;
			case 1:
				text2 = Language.GetTextValue("UI.Mediumcore");
				color2 = Main.mcColor;
				break;
			case 2:
				text2 = Language.GetTextValue("UI.Hardcore");
				color2 = Main.hcColor;
				break;
			case 3:
				text2 = Language.GetTextValue("UI.Creative");
				color2 = Main.creativeModeColor;
				break;
			}
			vector3 += new Vector2(num3 * 0.5f - FontAssets.MouseText.Value.MeasureString(text2).X * 0.5f, 3f);
			Utils.DrawBorderString(spriteBatch, text2, vector3, color2, 1f, 0f, 0f, -1);
			vector.X += num3 + 5f;
			Vector2 vector4 = vector;
			float num4 = innerDimensions.X + innerDimensions.Width - vector4.X;
			this.DrawPanel(spriteBatch, vector4, num4);
			TimeSpan playTime = this._data.GetPlayTime();
			int num5 = playTime.Days * 24 + playTime.Hours;
			string text3 = ((num5 < 10) ? "0" : "") + num5 + playTime.ToString("\\:mm\\:ss");
			vector4 += new Vector2(num4 * 0.5f - FontAssets.MouseText.Value.MeasureString(text3).X * 0.5f, 3f);
			Utils.DrawBorderString(spriteBatch, text3, vector4, Color.White, 1f, 0f, 0f, -1);
		}

		// Token: 0x040055EA RID: 21994
		private PlayerFileData _data;

		// Token: 0x040055EB RID: 21995
		private Asset<Texture2D> _dividerTexture;

		// Token: 0x040055EC RID: 21996
		private Asset<Texture2D> _innerPanelTexture;

		// Token: 0x040055ED RID: 21997
		private UICharacter _playerPanel;

		// Token: 0x040055EE RID: 21998
		private UIText _buttonLabel;

		// Token: 0x040055EF RID: 21999
		private UIText _deleteButtonLabel;

		// Token: 0x040055F0 RID: 22000
		private Asset<Texture2D> _buttonCloudActiveTexture;

		// Token: 0x040055F1 RID: 22001
		private Asset<Texture2D> _buttonCloudInactiveTexture;

		// Token: 0x040055F2 RID: 22002
		private Asset<Texture2D> _buttonFavoriteActiveTexture;

		// Token: 0x040055F3 RID: 22003
		private Asset<Texture2D> _buttonFavoriteInactiveTexture;

		// Token: 0x040055F4 RID: 22004
		private Asset<Texture2D> _buttonPlayTexture;

		// Token: 0x040055F5 RID: 22005
		private Asset<Texture2D> _buttonRenameTexture;

		// Token: 0x040055F6 RID: 22006
		private Asset<Texture2D> _buttonDeleteTexture;

		// Token: 0x040055F7 RID: 22007
		private UIImageButton _deleteButton;

		// Token: 0x040055F8 RID: 22008
		private int _orderInList;
	}
}
