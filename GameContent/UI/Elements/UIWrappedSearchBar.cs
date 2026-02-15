using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003C8 RID: 968
	public class UIWrappedSearchBar : UIElement
	{
		// Token: 0x1400004E RID: 78
		// (add) Token: 0x06002D40 RID: 11584 RVA: 0x005A2430 File Offset: 0x005A0630
		// (remove) Token: 0x06002D41 RID: 11585 RVA: 0x005A2468 File Offset: 0x005A0668
		public event Action<string> OnSearchContentsChanged;

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06002D42 RID: 11586 RVA: 0x005A249D File Offset: 0x005A069D
		public bool HasContents
		{
			get
			{
				return this._searchBar.HasContents;
			}
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x005A24AA File Offset: 0x005A06AA
		public void SetContents(string contents, bool forced = false)
		{
			this._searchBar.SetContents(contents, forced);
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06002D44 RID: 11588 RVA: 0x005A24B9 File Offset: 0x005A06B9
		public bool IsWritingText
		{
			get
			{
				return this._searchBar.IsWritingText;
			}
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x005A24C6 File Offset: 0x005A06C6
		public void ToggleTakingText()
		{
			this._searchBar.ToggleTakingText();
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06002D46 RID: 11590 RVA: 0x005A24D3 File Offset: 0x005A06D3
		// (set) Token: 0x06002D47 RID: 11591 RVA: 0x005A24E0 File Offset: 0x005A06E0
		public int MaxInputLength
		{
			get
			{
				return this._searchBar.MaxInputLength;
			}
			set
			{
				this._searchBar.MaxInputLength = value;
			}
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x005A24EE File Offset: 0x005A06EE
		public void SetSearchSnapPoint(string name, int id, Vector2? anchor = null, Vector2? offset = null)
		{
			this._searchButton.SetSnapPoint(name, id, anchor, offset);
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x005A2500 File Offset: 0x005A0700
		public UIWrappedSearchBar(Action goBackFromVirtualKeyboard, LocalizedText emptyText = null, UIWrappedSearchBar.ColorTheme theme = UIWrappedSearchBar.ColorTheme.Blue)
		{
			this._theme = theme;
			this._goBackFromVirtualKeyboard = goBackFromVirtualKeyboard;
			this._emptyText = ((emptyText != null) ? emptyText : Language.GetText("UI.PlayerNameSlot"));
			this.Height = new StyleDimension(24f, 0f);
			this.Width = new StyleDimension(0f, 1f);
			base.SetPadding(0f);
			this.AddSearchBar();
			this.SetContents(null, true);
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x005A257A File Offset: 0x005A077A
		public void HideSearchButton()
		{
			base.RemoveChild(this._searchButton);
			this._searchBoxPanel.Width = new StyleDimension(-3f, 1f);
			this.Recalculate();
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x005A25A8 File Offset: 0x005A07A8
		private void AddSearchBar()
		{
			string text = "Images/UI/Bestiary/Button_Search";
			if (this._theme == UIWrappedSearchBar.ColorTheme.Red)
			{
				text = "Images/UI/Bestiary/Button_Search_2";
			}
			UIImageButton uiimageButton = new UIImageButton(Main.Assets.Request<Texture2D>(text, 1), null)
			{
				VAlign = 0.5f
			};
			this._searchButton = uiimageButton;
			uiimageButton.OnLeftClick += this.Click_SearchArea;
			uiimageButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search_Border", 1), null);
			uiimageButton.SetVisibility(1f, 1f);
			base.Append(uiimageButton);
			UIPanel uipanel = new UIPanel
			{
				Width = new StyleDimension(-uiimageButton.Width.Pixels - 3f, 1f),
				Height = new StyleDimension(0f, 1f),
				VAlign = 0.5f,
				HAlign = 1f
			};
			this._searchBoxPanel = uipanel;
			uipanel.BackgroundColor = new Color(35, 40, 83);
			uipanel.BorderColor = new Color(35, 40, 83);
			if (this._theme == UIWrappedSearchBar.ColorTheme.Red)
			{
				uipanel.BackgroundColor = Utils.ShiftBlueToCyanTheme(uipanel.BackgroundColor);
				uipanel.BorderColor = Utils.ShiftBlueToCyanTheme(uipanel.BorderColor);
			}
			uipanel.SetPadding(0f);
			base.Append(uipanel);
			UISearchBar uisearchBar = new UISearchBar(this._emptyText, 0.8f)
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(0f, 1f),
				HAlign = 0f,
				VAlign = 0.5f,
				Left = new StyleDimension(0f, 0f),
				IgnoresMouseInteraction = true
			};
			this._searchBar = uisearchBar;
			uipanel.OnLeftClick += this.Click_SearchArea;
			uipanel.OnRightClick += this.SearchBox_OnRightClick;
			uisearchBar.OnContentsChanged += this.UpdateSearchContents;
			uipanel.Append(uisearchBar);
			uisearchBar.OnStartTakingInput += this.OnStartTakingInput;
			uisearchBar.OnEndTakingInput += this.OnEndTakingInput;
			uisearchBar.OnNeedingVirtualKeyboard += this.OpenVirtualKeyboardWhenNeeded;
			UIImageButton uiimageButton2 = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel", 1), null)
			{
				HAlign = 1f,
				VAlign = 0.5f,
				Left = new StyleDimension(-2f, 0f)
			};
			uiimageButton2.OnMouseOver += this.searchCancelButton_OnMouseOver;
			uiimageButton2.OnLeftClick += this.searchCancelButton_OnClick;
			uipanel.Append(uiimageButton2);
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x005A285B File Offset: 0x005A0A5B
		private void searchCancelButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (this.HasContents)
			{
				this.SetContents(null, true);
				SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
				return;
			}
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x00592D7A File Offset: 0x00590F7A
		private void searchCancelButton_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x005A2898 File Offset: 0x005A0A98
		private void OpenVirtualKeyboardWhenNeeded()
		{
			UIVirtualKeyboard uivirtualKeyboard = new UIVirtualKeyboard(this._emptyText.Value, this._searchString, new UIVirtualKeyboard.KeyboardSubmitEvent(this.SubmitVirtualText), new Action(this.GoBackFromVirtualKeyboard), 0, true, this.MaxInputLength);
			if (this.CustomOpenVirtualKeyboard != null)
			{
				this.CustomOpenVirtualKeyboard(uivirtualKeyboard);
				return;
			}
			UserInterface.ActiveInstance.SetState(uivirtualKeyboard);
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x005A28FC File Offset: 0x005A0AFC
		private void SubmitVirtualText(string text)
		{
			this.SetContents(text.Trim(), false);
			this.GoBackFromVirtualKeyboard();
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x005A2911 File Offset: 0x005A0B11
		private void GoBackFromVirtualKeyboard()
		{
			this._searchBar.ToggleTakingText();
			this._goBackFromVirtualKeyboard();
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x005A2929 File Offset: 0x005A0B29
		private void OnStartTakingInput()
		{
			this._searchBoxPanel.BorderColor = Main.OurFavoriteColor;
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x005A293B File Offset: 0x005A0B3B
		private void OnEndTakingInput()
		{
			this._searchBoxPanel.BorderColor = new Color(35, 40, 83);
			if (this._theme == UIWrappedSearchBar.ColorTheme.Red)
			{
				this._searchBoxPanel.BorderColor = Utils.ShiftBlueToCyanTheme(this._searchBoxPanel.BorderColor);
			}
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x005A2977 File Offset: 0x005A0B77
		private void UpdateSearchContents(string contents)
		{
			this._searchString = contents;
			if (this.OnSearchContentsChanged != null)
			{
				this.OnSearchContentsChanged(contents);
			}
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x005A2994 File Offset: 0x005A0B94
		private void Click_SearchArea(UIMouseEvent evt, UIElement listeningElement)
		{
			if (evt.Target.Parent == this._searchBoxPanel)
			{
				return;
			}
			this.ToggleTakingText();
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x005A29B0 File Offset: 0x005A0BB0
		private void SearchBox_OnRightClick(UIMouseEvent evt, UIElement listeningElement)
		{
			this.SetContents(null, true);
			if (!this.IsWritingText)
			{
				this.ToggleTakingText();
			}
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x005A29C8 File Offset: 0x005A0BC8
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (this.IsWritingText && FocusHelper.AllowUIInputs && (Main.mouseLeft || Main.mouseRight))
			{
				if (!this.Elements.Any((UIElement e) => e.IsMouseHovering))
				{
					this.ToggleTakingText();
				}
			}
		}

		// Token: 0x0400548E RID: 21646
		private Action _goBackFromVirtualKeyboard;

		// Token: 0x0400548F RID: 21647
		private LocalizedText _emptyText;

		// Token: 0x04005490 RID: 21648
		private UISearchBar _searchBar;

		// Token: 0x04005491 RID: 21649
		private UIPanel _searchBoxPanel;

		// Token: 0x04005492 RID: 21650
		private UIElement _searchButton;

		// Token: 0x04005493 RID: 21651
		private string _searchString;

		// Token: 0x04005495 RID: 21653
		public Action<UIState> CustomOpenVirtualKeyboard;

		// Token: 0x04005496 RID: 21654
		private UIWrappedSearchBar.ColorTheme _theme;

		// Token: 0x02000922 RID: 2338
		public enum ColorTheme
		{
			// Token: 0x040074B4 RID: 29876
			Blue,
			// Token: 0x040074B5 RID: 29877
			Red
		}
	}
}
