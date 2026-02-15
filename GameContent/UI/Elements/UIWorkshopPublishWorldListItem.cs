using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003E3 RID: 995
	public class UIWorkshopPublishWorldListItem : AWorldListItem
	{
		// Token: 0x06002E2C RID: 11820 RVA: 0x005A8A10 File Offset: 0x005A6C10
		public UIWorkshopPublishWorldListItem(UIState ownerState, WorldFileData data, int orderInList)
		{
			this._ownerState = ownerState;
			this._orderInList = orderInList;
			this._data = data;
			this.LoadTextures();
			this.InitializeAppearance();
			this._worldIcon = base.GetIconElement();
			this._worldIcon.Left.Set(4f, 0f);
			this._worldIcon.VAlign = 0.5f;
			this._worldIcon.OnLeftDoubleClick += this.PublishButtonClick_ImportWorldToLocalFiles;
			base.Append(this._worldIcon);
			this._publishButton = new UIIconTextButton(Language.GetText("Workshop.Publish"), Color.White, "Images/UI/Workshop/Publish", 1f, 0.5f, 10f);
			this._publishButton.HAlign = 1f;
			this._publishButton.VAlign = 1f;
			this._publishButton.OnLeftClick += this.PublishButtonClick_ImportWorldToLocalFiles;
			base.OnLeftDoubleClick += this.PublishButtonClick_ImportWorldToLocalFiles;
			base.Append(this._publishButton);
			this._publishButton.SetSnapPoint("Publish", orderInList, null, null);
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x005A8B49 File Offset: 0x005A6D49
		private void LoadTextures()
		{
			this._innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", 1);
			this._workshopIconTexture = TextureAssets.Extra[243];
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x005A8B72 File Offset: 0x005A6D72
		private void InitializeAppearance()
		{
			this.Height.Set(82f, 0f);
			this.Width.Set(0f, 1f);
			base.SetPadding(6f);
			this.SetColorsToNotHovered();
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x005A7BF9 File Offset: 0x005A5DF9
		private void SetColorsToHovered()
		{
			this.BackgroundColor = new Color(73, 94, 171);
			this.BorderColor = new Color(89, 116, 213);
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x005A7C23 File Offset: 0x005A5E23
		private void SetColorsToNotHovered()
		{
			this.BackgroundColor = new Color(63, 82, 151) * 0.7f;
			this.BorderColor = new Color(89, 116, 213) * 0.7f;
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x005A8BAF File Offset: 0x005A6DAF
		private void PublishButtonClick_ImportWorldToLocalFiles(UIMouseEvent evt, UIElement listeningElement)
		{
			if (listeningElement != evt.Target)
			{
				return;
			}
			Main.MenuUI.SetState(new WorkshopPublishInfoStateForWorld(this._ownerState, this._data));
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x005A8BD8 File Offset: 0x005A6DD8
		public override int CompareTo(object obj)
		{
			UIWorkshopPublishWorldListItem uiworkshopPublishWorldListItem = obj as UIWorkshopPublishWorldListItem;
			if (uiworkshopPublishWorldListItem != null)
			{
				return this._orderInList.CompareTo(uiworkshopPublishWorldListItem._orderInList);
			}
			return base.CompareTo(obj);
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x005A8C08 File Offset: 0x005A6E08
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			this.SetColorsToHovered();
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x005A8C17 File Offset: 0x005A6E17
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this.SetColorsToNotHovered();
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x005A8C28 File Offset: 0x005A6E28
		private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width, float height)
		{
			Utils.DrawSplicedPanel(spriteBatch, this._innerPanelTexture.Value, (int)position.X, (int)position.Y, (int)width, (int)height, 10, 10, 10, 10, Color.White);
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x005A8C68 File Offset: 0x005A6E68
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			CalculatedStyle dimensions = this._worldIcon.GetDimensions();
			float num = dimensions.X + dimensions.Width;
			Color color = this._data.IsValid ? Color.White : Color.Gray;
			string worldName = this._data.GetWorldName(true);
			Utils.DrawBorderString(spriteBatch, worldName, new Vector2(num + 6f, innerDimensions.Y + 3f), color, 1f, 0f, 0f, -1);
			float num2 = (innerDimensions.Width - 22f - dimensions.Width - this._publishButton.GetDimensions().Width) / 2f;
			float height = this._publishButton.GetDimensions().Height;
			Vector2 vector = new Vector2(num + 6f, innerDimensions.Y + innerDimensions.Height - height);
			float num3 = num2;
			this.DrawPanel(spriteBatch, vector, num3, height);
			string text = "";
			Color white = Color.White;
			base.GetDifficulty(out text, out white);
			Vector2 vector2 = FontAssets.MouseText.Value.MeasureString(text);
			float x = vector2.X;
			float y = vector2.Y;
			float x2 = num3 * 0.5f - x * 0.5f;
			float num4 = height * 0.5f - y * 0.5f;
			Utils.DrawBorderString(spriteBatch, text, vector + new Vector2(x2, num4 + 3f), white, 1f, 0f, 0f, -1);
			vector.X += num3 + 5f;
			float num5 = num2;
			if (!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
			{
				num5 += 40f;
			}
			this.DrawPanel(spriteBatch, vector, num5, height);
			string textValue = Language.GetTextValue("UI.WorldSizeFormat", this._data.WorldSizeName);
			Vector2 vector3 = FontAssets.MouseText.Value.MeasureString(textValue);
			float x3 = vector3.X;
			float y2 = vector3.Y;
			float x4 = num5 * 0.5f - x3 * 0.5f;
			float num6 = height * 0.5f - y2 * 0.5f;
			Utils.DrawBorderString(spriteBatch, textValue, vector + new Vector2(x4, num6 + 3f), Color.White, 1f, 0f, 0f, -1);
			vector.X += num5 + 5f;
		}

		// Token: 0x04005520 RID: 21792
		private Asset<Texture2D> _workshopIconTexture;

		// Token: 0x04005521 RID: 21793
		private Asset<Texture2D> _innerPanelTexture;

		// Token: 0x04005522 RID: 21794
		private UIElement _worldIcon;

		// Token: 0x04005523 RID: 21795
		private UIElement _publishButton;

		// Token: 0x04005524 RID: 21796
		private int _orderInList;

		// Token: 0x04005525 RID: 21797
		private UIState _ownerState;
	}
}
