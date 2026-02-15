using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Achievements;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003B4 RID: 948
	public class UIAchievementsMenu : UIState
	{
		// Token: 0x06002C9D RID: 11421 RVA: 0x0059CB68 File Offset: 0x0059AD68
		public void InitializePage()
		{
			base.RemoveAllChildren();
			this._categoryButtons.Clear();
			this._achievementElements.Clear();
			this._achievementsList = null;
			bool flag = true;
			int num = flag.ToInt() * 100;
			UIElement uielement = new UIElement();
			uielement.Width.Set(0f, 0.8f);
			uielement.MaxWidth.Set(800f + (float)num, 0f);
			uielement.MinWidth.Set(600f + (float)num, 0f);
			uielement.Top.Set(220f, 0f);
			uielement.Height.Set(-220f, 1f);
			uielement.HAlign = 0.5f;
			this._outerContainer = uielement;
			base.Append(uielement);
			UIPanel uipanel = new UIPanel();
			uipanel.Width.Set(0f, 1f);
			uipanel.Height.Set(-110f, 1f);
			uipanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
			uipanel.PaddingTop = 0f;
			uielement.Append(uipanel);
			this._achievementsList = new UIList();
			this._achievementsList.Width.Set(-25f, 1f);
			this._achievementsList.Height.Set(-50f, 1f);
			this._achievementsList.Top.Set(50f, 0f);
			this._achievementsList.ListPadding = 5f;
			uipanel.Append(this._achievementsList);
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Achievements"), 1f, true);
			uitextPanel.HAlign = 0.5f;
			uitextPanel.Top.Set(-33f, 0f);
			uitextPanel.SetPadding(13f);
			uitextPanel.BackgroundColor = new Color(73, 94, 171);
			uielement.Append(uitextPanel);
			UITextPanel<LocalizedText> uitextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
			uitextPanel2.Width.Set(-10f, 0.5f);
			uitextPanel2.Height.Set(50f, 0f);
			uitextPanel2.VAlign = 1f;
			uitextPanel2.HAlign = 0.5f;
			uitextPanel2.Top.Set(-45f, 0f);
			uitextPanel2.OnMouseOver += this.FadedMouseOver;
			uitextPanel2.OnMouseOut += this.FadedMouseOut;
			uitextPanel2.OnLeftClick += this.GoBackClick;
			uielement.Append(uitextPanel2);
			this._backpanel = uitextPanel2;
			List<Achievement> list = Main.Achievements.CreateAchievementsList();
			for (int i = 0; i < list.Count; i++)
			{
				UIAchievementListItem item = new UIAchievementListItem(list[i], flag);
				this._achievementsList.Add(item);
				this._achievementElements.Add(item);
			}
			UIScrollbar uiscrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue);
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(-50f, 1f);
			uiscrollbar.Top.Set(50f, 0f);
			uiscrollbar.HAlign = 1f;
			uipanel.Append(uiscrollbar);
			this._achievementsList.SetScrollbar(uiscrollbar);
			UIElement uielement2 = new UIElement();
			uielement2.Width.Set(0f, 1f);
			uielement2.Height.Set(32f, 0f);
			uielement2.Top.Set(10f, 0f);
			Asset<Texture2D> texture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Categories", 1);
			for (int j = 0; j < 4; j++)
			{
				UIToggleImage uitoggleImage = new UIToggleImage(texture, 32, 32, new Point(34 * j, 0), new Point(34 * j, 34));
				uitoggleImage.Left.Set((float)(j * 36 + 8), 0f);
				uitoggleImage.SetState(true);
				uitoggleImage.OnLeftClick += this.FilterList;
				this._categoryButtons.Add(uitoggleImage);
				uielement2.Append(uitoggleImage);
			}
			uipanel.Append(uielement2);
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x0059CFB4 File Offset: 0x0059B1B4
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			for (int i = 0; i < this._categoryButtons.Count; i++)
			{
				if (this._categoryButtons[i].IsMouseHovering)
				{
					string textValue;
					switch (i)
					{
					case -1:
						textValue = Language.GetTextValue("Achievements.NoCategory");
						break;
					case 0:
						textValue = Language.GetTextValue("Achievements.SlayerCategory");
						break;
					case 1:
						textValue = Language.GetTextValue("Achievements.CollectorCategory");
						break;
					case 2:
						textValue = Language.GetTextValue("Achievements.ExplorerCategory");
						break;
					case 3:
						textValue = Language.GetTextValue("Achievements.ChallengerCategory");
						break;
					default:
						textValue = Language.GetTextValue("Achievements.NoCategory");
						break;
					}
					float x = FontAssets.MouseText.Value.MeasureString(textValue).X;
					Vector2 vector = new Vector2((float)Main.mouseX, (float)Main.mouseY) + new Vector2(16f);
					if (vector.Y > (float)(Main.screenHeight - 30))
					{
						vector.Y = (float)(Main.screenHeight - 30);
					}
					if (vector.X > (float)Main.screenWidth - x)
					{
						vector.X = (float)(Main.screenWidth - 460);
					}
					Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, textValue, vector.X, vector.Y, new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor), Color.Black, Vector2.Zero, 1f);
					break;
				}
			}
			this.SetupGamepadPoints(spriteBatch);
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x0059D134 File Offset: 0x0059B334
		public void GotoAchievement(Achievement achievement)
		{
			this._achievementsList.Goto(delegate(UIElement element)
			{
				UIAchievementListItem uiachievementListItem = element as UIAchievementListItem;
				return uiachievementListItem != null && uiachievementListItem.GetAchievement() == achievement;
			});
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x0058D0A2 File Offset: 0x0058B2A2
		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.menuMode = 0;
			IngameFancyUI.Close(false);
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x0059D168 File Offset: 0x0059B368
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x00584489 File Offset: 0x00582689
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x0059D1C0 File Offset: 0x0059B3C0
		private void FilterList(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this._achievementsList.Clear();
			foreach (UIAchievementListItem uiachievementListItem in this._achievementElements)
			{
				if (this._categoryButtons[(int)uiachievementListItem.GetAchievement().Category].IsOn)
				{
					this._achievementsList.Add(uiachievementListItem);
				}
			}
			this.Recalculate();
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x0059D25C File Offset: 0x0059B45C
		public override void OnActivate()
		{
			this.InitializePage();
			if (Main.gameMenu)
			{
				this._outerContainer.Top.Set(220f, 0f);
				this._outerContainer.Height.Set(-220f, 1f);
			}
			else
			{
				this._outerContainer.Top.Set(120f, 0f);
				this._outerContainer.Height.Set(-120f, 1f);
			}
			this._achievementsList.UpdateOrder();
			if (PlayerInput.UsingGamepadUI)
			{
				UILinkPointNavigator.ChangePoint(3002);
			}
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x0059D2FC File Offset: 0x0059B4FC
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 3;
			int num = 3000;
			UILinkPointNavigator.SetPosition(num, this._backpanel.GetInnerDimensions().ToRectangle().Center.ToVector2());
			UILinkPointNavigator.SetPosition(num + 1, this._outerContainer.GetInnerDimensions().ToRectangle().Center.ToVector2());
			int num2 = num;
			UILinkPoint uilinkPoint = UILinkPointNavigator.Points[num2];
			uilinkPoint.Unlink();
			uilinkPoint.Up = num2 + 1;
			num2++;
			UILinkPoint uilinkPoint2 = UILinkPointNavigator.Points[num2];
			uilinkPoint2.Unlink();
			uilinkPoint2.Up = num2 + 1;
			uilinkPoint2.Down = num2 - 1;
			for (int i = 0; i < this._categoryButtons.Count; i++)
			{
				num2++;
				UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num2;
				UILinkPointNavigator.SetPosition(num2, this._categoryButtons[i].GetInnerDimensions().ToRectangle().Center.ToVector2());
				UILinkPoint uilinkPoint3 = UILinkPointNavigator.Points[num2];
				uilinkPoint3.Unlink();
				uilinkPoint3.Left = ((i == 0) ? -3 : (num2 - 1));
				uilinkPoint3.Right = ((i == this._categoryButtons.Count - 1) ? -4 : (num2 + 1));
				uilinkPoint3.Down = num;
			}
		}

		// Token: 0x04005410 RID: 21520
		private UIList _achievementsList;

		// Token: 0x04005411 RID: 21521
		private List<UIAchievementListItem> _achievementElements = new List<UIAchievementListItem>();

		// Token: 0x04005412 RID: 21522
		private List<UIToggleImage> _categoryButtons = new List<UIToggleImage>();

		// Token: 0x04005413 RID: 21523
		private UIElement _backpanel;

		// Token: 0x04005414 RID: 21524
		private UIElement _outerContainer;
	}
}
