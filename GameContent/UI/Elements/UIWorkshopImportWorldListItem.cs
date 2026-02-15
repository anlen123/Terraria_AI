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
	// Token: 0x020003E1 RID: 993
	public class UIWorkshopImportWorldListItem : AWorldListItem
	{
		// Token: 0x06002E10 RID: 11792 RVA: 0x005A79E8 File Offset: 0x005A5BE8
		public UIWorkshopImportWorldListItem(UIState ownerState, WorldFileData data, int orderInList)
		{
			this._ownerState = ownerState;
			this._orderInList = orderInList;
			this._data = data;
			this.LoadTextures();
			this.InitializeAppearance();
			this._worldIcon = base.GetIconElement();
			this._worldIcon.Left.Set(4f, 0f);
			this._worldIcon.OnLeftDoubleClick += this.ImportButtonClick_ImportWorldToLocalFiles;
			base.Append(this._worldIcon);
			float num = 4f;
			UIImageButton uiimageButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", 1), null);
			uiimageButton.VAlign = 1f;
			uiimageButton.Left.Set(num, 0f);
			uiimageButton.OnLeftClick += this.ImportButtonClick_ImportWorldToLocalFiles;
			base.OnLeftDoubleClick += this.ImportButtonClick_ImportWorldToLocalFiles;
			uiimageButton.OnMouseOver += this.PlayMouseOver;
			uiimageButton.OnMouseOut += this.ButtonMouseOut;
			base.Append(uiimageButton);
			num += 24f;
			this._buttonLabel = new UIText("", 1f, false);
			this._buttonLabel.VAlign = 1f;
			this._buttonLabel.Left.Set(num, 0f);
			this._buttonLabel.Top.Set(-3f, 0f);
			base.Append(this._buttonLabel);
			uiimageButton.SetSnapPoint("Import", orderInList, null, null);
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x005A7B7D File Offset: 0x005A5D7D
		private void LoadTextures()
		{
			this._dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider", 1);
			this._innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", 1);
			this._workshopIconTexture = TextureAssets.Extra[243];
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x005A7BBC File Offset: 0x005A5DBC
		private void InitializeAppearance()
		{
			this.Height.Set(96f, 0f);
			this.Width.Set(0f, 1f);
			base.SetPadding(6f);
			this.SetColorsToNotHovered();
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x005A7BF9 File Offset: 0x005A5DF9
		private void SetColorsToHovered()
		{
			this.BackgroundColor = new Color(73, 94, 171);
			this.BorderColor = new Color(89, 116, 213);
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x005A7C23 File Offset: 0x005A5E23
		private void SetColorsToNotHovered()
		{
			this.BackgroundColor = new Color(63, 82, 151) * 0.7f;
			this.BorderColor = new Color(89, 116, 213) * 0.7f;
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x005A7C61 File Offset: 0x005A5E61
		private void PlayMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonLabel.SetText(Language.GetTextValue("UI.Import"));
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x005A7C78 File Offset: 0x005A5E78
		private void ButtonMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			this._buttonLabel.SetText("");
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x005A7C8C File Offset: 0x005A5E8C
		private void ImportButtonClick_ImportWorldToLocalFiles(UIMouseEvent evt, UIElement listeningElement)
		{
			if (listeningElement != evt.Target)
			{
				return;
			}
			if (!this._data.IsValid)
			{
				return;
			}
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.clrInput();
			UIVirtualKeyboard state = new UIVirtualKeyboard(Language.GetTextValue("Workshop.EnterNewNameForImportedWorld"), this._data.Name, new UIVirtualKeyboard.KeyboardSubmitEvent(this.OnFinishedSettingName), new Action(this.GoBackHere), 0, true, 27);
			Main.MenuUI.SetState(state);
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x005A7D10 File Offset: 0x005A5F10
		private void OnFinishedSettingName(string name)
		{
			string newDisplayName = name.Trim();
			if (SocialAPI.Workshop != null)
			{
				SocialAPI.Workshop.ImportDownloadedWorldToLocalSaves(this._data, newDisplayName, new Action(this.GoBackHere));
			}
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x005A7D48 File Offset: 0x005A5F48
		private void GoBackHere()
		{
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			Main.menuMode = 888;
			Main.MenuUI.SetState(this._ownerState);
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x005A7D7C File Offset: 0x005A5F7C
		public override int CompareTo(object obj)
		{
			UIWorkshopImportWorldListItem uiworkshopImportWorldListItem = obj as UIWorkshopImportWorldListItem;
			if (uiworkshopImportWorldListItem != null)
			{
				return this._orderInList.CompareTo(uiworkshopImportWorldListItem._orderInList);
			}
			return base.CompareTo(obj);
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x005A7DAC File Offset: 0x005A5FAC
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			this.SetColorsToHovered();
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x005A7DBB File Offset: 0x005A5FBB
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this.SetColorsToNotHovered();
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x005A7DCC File Offset: 0x005A5FCC
		private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
		{
			spriteBatch.Draw(this._innerPanelTexture.Value, position, new Rectangle?(new Rectangle(0, 0, 8, this._innerPanelTexture.Height())), Color.White);
			spriteBatch.Draw(this._innerPanelTexture.Value, new Vector2(position.X + 8f, position.Y), new Rectangle?(new Rectangle(8, 0, 8, this._innerPanelTexture.Height())), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(this._innerPanelTexture.Value, new Vector2(position.X + width - 8f, position.Y), new Rectangle?(new Rectangle(16, 0, 8, this._innerPanelTexture.Height())), Color.White);
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x005A7EBC File Offset: 0x005A60BC
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
			if (text != null)
			{
				Utils.DrawBorderString(spriteBatch, text, new Vector2(num + 6f, dimensions.Y - 2f), color, 1f, 0f, 0f, -1);
			}
			spriteBatch.Draw(this._workshopIconTexture.Value, new Vector2(base.GetDimensions().X + base.GetDimensions().Width - (float)this._workshopIconTexture.Width() - 3f, base.GetDimensions().Y + 2f), new Rectangle?(this._workshopIconTexture.Frame(1, 1, 0, 0, 0, 0)), Color.White);
			spriteBatch.Draw(this._dividerTexture.Value, new Vector2(num, innerDimensions.Y + 21f), null, Color.White, 0f, Vector2.Zero, new Vector2((base.GetDimensions().X + base.GetDimensions().Width - num) / 8f, 1f), SpriteEffects.None, 0f);
			Vector2 vector = new Vector2(num + 6f, innerDimensions.Y + 29f);
			float num2 = 100f;
			this.DrawPanel(spriteBatch, vector, num2);
			string text2 = "";
			Color white = Color.White;
			base.GetDifficulty(out text2, out white);
			float x = FontAssets.MouseText.Value.MeasureString(text2).X;
			float x2 = num2 * 0.5f - x * 0.5f;
			Utils.DrawBorderString(spriteBatch, text2, vector + new Vector2(x2, 3f), white, 1f, 0f, 0f, -1);
			vector.X += num2 + 5f;
			if (this._data._worldSizeName != null)
			{
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
			}
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

		// Token: 0x0400550C RID: 21772
		private Asset<Texture2D> _dividerTexture;

		// Token: 0x0400550D RID: 21773
		private Asset<Texture2D> _workshopIconTexture;

		// Token: 0x0400550E RID: 21774
		private Asset<Texture2D> _innerPanelTexture;

		// Token: 0x0400550F RID: 21775
		private UIElement _worldIcon;

		// Token: 0x04005510 RID: 21776
		private UIText _buttonLabel;

		// Token: 0x04005511 RID: 21777
		private int _orderInList;

		// Token: 0x04005512 RID: 21778
		public UIState _ownerState;
	}
}
