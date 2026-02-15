using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003A4 RID: 932
	public class UIWorkshopHub : UIState, IHaveBackButtonCommand
	{
		// Token: 0x1400004D RID: 77
		// (add) Token: 0x06002AB0 RID: 10928 RVA: 0x00586AC8 File Offset: 0x00584CC8
		// (remove) Token: 0x06002AB1 RID: 10929 RVA: 0x00586AFC File Offset: 0x00584CFC
		public static event Action OnWorkshopHubMenuOpened;

		// Token: 0x06002AB2 RID: 10930 RVA: 0x00586B2F File Offset: 0x00584D2F
		public UIWorkshopHub(UIState stateToGoBackTo)
		{
			this._previousUIState = stateToGoBackTo;
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x00586B3E File Offset: 0x00584D3E
		public void EnterHub()
		{
			UIWorkshopHub.OnWorkshopHubMenuOpened();
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x00586B4C File Offset: 0x00584D4C
		public override void OnInitialize()
		{
			base.OnInitialize();
			int num = 20;
			int num2 = 250;
			int num3 = 50 + num * 2;
			int num4 = 600;
			int num5 = num4 - num2 - num3;
			UIElement uielement = new UIElement();
			uielement.Width.Set(600f, 0f);
			uielement.Top.Set((float)num2, 0f);
			uielement.Height.Set((float)(num4 - num2), 0f);
			uielement.HAlign = 0.5f;
			int num6 = 154;
			UIPanel uipanel = new UIPanel();
			uipanel.Width.Set(0f, 1f);
			uipanel.Height.Set((float)num5, 0f);
			uipanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
			UIElement uielement2 = new UIElement();
			uielement2.Width.Set(0f, 1f);
			uielement2.Height.Set((float)num6, 0f);
			uielement2.SetPadding(0f);
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.WorkshopHub"), 0.8f, true);
			uitextPanel.HAlign = 0.5f;
			uitextPanel.Top.Set(-46f, 0f);
			uitextPanel.SetPadding(15f);
			uitextPanel.BackgroundColor = new Color(73, 94, 171);
			UITextPanel<LocalizedText> uitextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
			uitextPanel2.Width.Set(-10f, 0.5f);
			uitextPanel2.Height.Set(50f, 0f);
			uitextPanel2.VAlign = 1f;
			uitextPanel2.HAlign = 0f;
			uitextPanel2.Top.Set((float)(-(float)num), 0f);
			uitextPanel2.OnMouseOver += this.FadedMouseOver;
			uitextPanel2.OnMouseOut += this.FadedMouseOut;
			uitextPanel2.OnLeftClick += this.GoBackClick;
			uitextPanel2.SetSnapPoint("Back", 0, null, null);
			uielement.Append(uitextPanel2);
			this._buttonBack = uitextPanel2;
			UITextPanel<LocalizedText> uitextPanel3 = new UITextPanel<LocalizedText>(Language.GetText("Workshop.ReportLogsButton"), 0.7f, true);
			uitextPanel3.Width.Set(-10f, 0.5f);
			uitextPanel3.Height.Set(50f, 0f);
			uitextPanel3.VAlign = 1f;
			uitextPanel3.HAlign = 1f;
			uitextPanel3.Top.Set((float)(-(float)num), 0f);
			uitextPanel3.OnMouseOver += this.FadedMouseOver;
			uitextPanel3.OnMouseOut += this.FadedMouseOut;
			uitextPanel3.OnLeftClick += this.GoLogsClick;
			uitextPanel3.SetSnapPoint("Logs", 0, null, null);
			uielement.Append(uitextPanel3);
			this._buttonLogs = uitextPanel3;
			UIElement uielement3 = this.MakeButton_OpenWorkshopWorldsImportMenu();
			uielement3.HAlign = 0f;
			uielement3.VAlign = 0f;
			uielement2.Append(uielement3);
			uielement3 = this.MakeButton_OpenUseResourcePacksMenu();
			uielement3.HAlign = 1f;
			uielement3.VAlign = 0f;
			uielement2.Append(uielement3);
			uielement3 = this.MakeButton_OpenPublishWorldsMenu();
			uielement3.HAlign = 0f;
			uielement3.VAlign = 1f;
			uielement2.Append(uielement3);
			uielement3 = this.MakeButton_OpenPublishResourcePacksMenu();
			uielement3.HAlign = 1f;
			uielement3.VAlign = 1f;
			uielement2.Append(uielement3);
			UIWorkshopHub.AddHorizontalSeparator(uipanel, (float)(num6 + 6 + 6));
			this.AddDescriptionPanel(uipanel, (float)(num6 + 8 + 6 + 6), (float)(num5 - num6 - 12 - 12 - 8), "desc");
			uipanel.Append(uielement2);
			uielement.Append(uipanel);
			uielement.Append(uitextPanel);
			base.Append(uielement);
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x00586F68 File Offset: 0x00585168
		private UIElement MakeButton_OpenUseResourcePacksMenu()
		{
			UIElement uielement = this.MakeFancyButton("Images/UI/Workshop/HubResourcepacks", "Workshop.HubResourcePacks");
			uielement.OnLeftClick += this.Click_OpenResourcePacksMenu;
			this._buttonUseResourcePacks = uielement;
			return uielement;
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x00586FA0 File Offset: 0x005851A0
		private void Click_OpenResourcePacksMenu(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.OpenResourcePacksMenu(this);
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x00586FC0 File Offset: 0x005851C0
		private UIElement MakeButton_OpenWorkshopWorldsImportMenu()
		{
			UIElement uielement = this.MakeFancyButton("Images/UI/Workshop/HubWorlds", "Workshop.HubWorlds");
			uielement.OnLeftClick += this.Click_OpenWorldImportMenu;
			this._buttonImportWorlds = uielement;
			return uielement;
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x00586FF8 File Offset: 0x005851F8
		private void Click_OpenWorldImportMenu(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.MenuUI.SetState(new UIWorkshopWorldImport(this));
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x00587020 File Offset: 0x00585220
		private UIElement MakeButton_OpenPublishResourcePacksMenu()
		{
			UIElement uielement = this.MakeFancyButton("Images/UI/Workshop/HubPublishResourcepacks", "Workshop.HubPublishResourcePacks");
			uielement.OnLeftClick += this.Click_OpenResourcePackPublishMenu;
			this._buttonPublishResourcePacks = uielement;
			return uielement;
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x00587058 File Offset: 0x00585258
		private void Click_OpenResourcePackPublishMenu(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.MenuUI.SetState(new UIWorkshopSelectResourcePackToPublish(this));
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x00587080 File Offset: 0x00585280
		private UIElement MakeButton_OpenPublishWorldsMenu()
		{
			UIElement uielement = this.MakeFancyButton("Images/UI/Workshop/HubPublishWorlds", "Workshop.HubPublishWorlds");
			uielement.OnLeftClick += this.Click_OpenWorldPublishMenu;
			this._buttonPublishWorlds = uielement;
			return uielement;
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x005870B8 File Offset: 0x005852B8
		private void Click_OpenWorldPublishMenu(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.MenuUI.SetState(new UIWorkshopSelectWorldToPublish(this));
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x005870E0 File Offset: 0x005852E0
		private static void AddHorizontalSeparator(UIElement Container, float accumualtedHeight)
		{
			UIHorizontalSeparator element = new UIHorizontalSeparator(2, true)
			{
				Width = StyleDimension.FromPercent(1f),
				Top = StyleDimension.FromPixels(accumualtedHeight - 8f),
				Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
			};
			Container.Append(element);
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x00587150 File Offset: 0x00585350
		public void ShowOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			LocalizedText localizedText = null;
			if (listeningElement == this._buttonUseResourcePacks)
			{
				localizedText = Language.GetText("Workshop.HubDescriptionUseResourcePacks");
			}
			if (listeningElement == this._buttonPublishResourcePacks)
			{
				localizedText = Language.GetText("Workshop.HubDescriptionPublishResourcePacks");
			}
			if (listeningElement == this._buttonImportWorlds)
			{
				localizedText = Language.GetText("Workshop.HubDescriptionImportWorlds");
			}
			if (listeningElement == this._buttonPublishWorlds)
			{
				localizedText = Language.GetText("Workshop.HubDescriptionPublishWorlds");
			}
			if (localizedText != null)
			{
				this._descriptionText.SetText(localizedText);
			}
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x005871BE File Offset: 0x005853BE
		public void ClearOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			this._descriptionText.SetText(Language.GetText("Workshop.HubDescriptionDefault"));
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x005871D8 File Offset: 0x005853D8
		private void AddDescriptionPanel(UIElement container, float accumulatedHeight, float height, string tagGroup)
		{
			float num = 0f;
			UISlicedImage uislicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1))
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent(-num * 2f, 1f),
				Left = StyleDimension.FromPixels(-num),
				Height = StyleDimension.FromPixelsAndPercent(height, 0f),
				Top = StyleDimension.FromPixels(2f)
			};
			uislicedImage.SetSliceDepths(10);
			uislicedImage.Color = Color.LightGray * 0.7f;
			container.Append(uislicedImage);
			UIText uitext = new UIText(Language.GetText("Workshop.HubDescriptionDefault"), 1f, false)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Top = StyleDimension.FromPixelsAndPercent(5f, 0f)
			};
			uitext.PaddingLeft = 20f;
			uitext.PaddingRight = 20f;
			uitext.PaddingTop = 6f;
			uitext.IsWrapped = true;
			uislicedImage.Append(uitext);
			this._descriptionText = uitext;
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x00587324 File Offset: 0x00585524
		private UIElement MakeFancyButton(string iconImagePath, string textKey)
		{
			UIPanel uipanel = new UIPanel();
			int num = -3;
			int num2 = -3;
			uipanel.Width = StyleDimension.FromPixelsAndPercent((float)num, 0.5f);
			uipanel.Height = StyleDimension.FromPixelsAndPercent((float)num2, 0.5f);
			uipanel.OnMouseOver += this.SetColorsToHovered;
			uipanel.OnMouseOut += this.SetColorsToNotHovered;
			uipanel.BackgroundColor = new Color(63, 82, 151) * 0.7f;
			uipanel.BorderColor = new Color(89, 116, 213) * 0.7f;
			uipanel.SetPadding(6f);
			UIImage uiimage = new UIImage(Main.Assets.Request<Texture2D>(iconImagePath, 1))
			{
				IgnoresMouseInteraction = true,
				VAlign = 0.5f
			};
			uiimage.Left.Set(2f, 0f);
			uipanel.Append(uiimage);
			uipanel.OnMouseOver += this.ShowOptionDescription;
			uipanel.OnMouseOut += this.ClearOptionDescription;
			UIText uitext = new UIText(Language.GetText(textKey), 0.45f, true)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Width = StyleDimension.FromPixelsAndPercent(-80f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Top = StyleDimension.FromPixelsAndPercent(5f, 0f),
				Left = StyleDimension.FromPixels(80f),
				IgnoresMouseInteraction = true,
				TextOriginX = 0f,
				TextOriginY = 0f
			};
			uitext.PaddingLeft = 0f;
			uitext.PaddingRight = 20f;
			uitext.PaddingTop = 10f;
			uitext.IsWrapped = true;
			uipanel.Append(uitext);
			uipanel.SetSnapPoint("Button", 0, null, null);
			return uipanel;
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x00587512 File Offset: 0x00585712
		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			this.HandleBackButtonUsage();
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x0058752F File Offset: 0x0058572F
		private void GoLogsClick(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.IssueReporterIndicator.Hide();
			Main.OpenReportsMenu();
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x00587558 File Offset: 0x00585758
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x00584489 File Offset: 0x00582689
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x005875AD File Offset: 0x005857AD
		private void SetColorsToHovered(UIMouseEvent evt, UIElement listeningElement)
		{
			UIPanel uipanel = (UIPanel)evt.Target;
			uipanel.BackgroundColor = new Color(73, 94, 171);
			uipanel.BorderColor = new Color(89, 116, 213);
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x005875E4 File Offset: 0x005857E4
		private void SetColorsToNotHovered(UIMouseEvent evt, UIElement listeningElement)
		{
			UIPanel uipanel = (UIPanel)evt.Target;
			uipanel.BackgroundColor = new Color(63, 82, 151) * 0.7f;
			uipanel.BorderColor = new Color(89, 116, 213) * 0.7f;
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x00587637 File Offset: 0x00585837
		public void HandleBackButtonUsage()
		{
			if (this._previousUIState == null)
			{
				Main.menuMode = 0;
				return;
			}
			Main.menuMode = 888;
			Main.MenuUI.SetState(this._previousUIState);
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x00587677 File Offset: 0x00585877
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			this.SetupGamepadPoints(spriteBatch);
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x00587688 File Offset: 0x00585888
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
			int num = 3000;
			int idRangeEndExclusive = num;
			UILinkPoint linkPoint = this._helper.GetLinkPoint(idRangeEndExclusive++, this._buttonUseResourcePacks);
			UILinkPoint linkPoint2 = this._helper.GetLinkPoint(idRangeEndExclusive++, this._buttonPublishResourcePacks);
			UILinkPoint linkPoint3 = this._helper.GetLinkPoint(idRangeEndExclusive++, this._buttonImportWorlds);
			UILinkPoint linkPoint4 = this._helper.GetLinkPoint(idRangeEndExclusive++, this._buttonPublishWorlds);
			UILinkPoint linkPoint5 = this._helper.GetLinkPoint(idRangeEndExclusive++, this._buttonBack);
			UILinkPoint linkPoint6 = this._helper.GetLinkPoint(idRangeEndExclusive++, this._buttonLogs);
			this._helper.PairLeftRight(linkPoint3, linkPoint);
			this._helper.PairLeftRight(linkPoint4, linkPoint2);
			this._helper.PairLeftRight(linkPoint5, linkPoint6);
			this._helper.PairUpDown(linkPoint3, linkPoint4);
			this._helper.PairUpDown(linkPoint, linkPoint2);
			this._helper.PairUpDown(linkPoint4, linkPoint5);
			this._helper.PairUpDown(linkPoint2, linkPoint6);
			this._helper.MoveToVisuallyClosestPoint(num, idRangeEndExclusive);
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x005877A2 File Offset: 0x005859A2
		// Note: this type is marked as 'beforefieldinit'.
		static UIWorkshopHub()
		{
			UIWorkshopHub.OnWorkshopHubMenuOpened = delegate()
			{
			};
		}

		// Token: 0x04005318 RID: 21272
		private UIState _previousUIState;

		// Token: 0x04005319 RID: 21273
		private UIText _descriptionText;

		// Token: 0x0400531A RID: 21274
		private UIElement _buttonUseResourcePacks;

		// Token: 0x0400531B RID: 21275
		private UIElement _buttonPublishResourcePacks;

		// Token: 0x0400531C RID: 21276
		private UIElement _buttonImportWorlds;

		// Token: 0x0400531D RID: 21277
		private UIElement _buttonPublishWorlds;

		// Token: 0x0400531E RID: 21278
		private UIElement _buttonBack;

		// Token: 0x0400531F RID: 21279
		private UIElement _buttonLogs;

		// Token: 0x04005320 RID: 21280
		private UIGamepadHelper _helper;
	}
}
