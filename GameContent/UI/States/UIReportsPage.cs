using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003A3 RID: 931
	public class UIReportsPage : UIState
	{
		// Token: 0x06002AA5 RID: 10917 RVA: 0x0058624C File Offset: 0x0058444C
		public UIReportsPage(UIState stateToGoBackTo, int menuIdToGoBackTo, List<IProvideReports> reporters)
		{
			this._previousUIState = stateToGoBackTo;
			this._menuIdToGoBackTo = menuIdToGoBackTo;
			this._reporters = reporters;
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x00586269 File Offset: 0x00584469
		public override void OnInitialize()
		{
			this.BuildPage();
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x00586274 File Offset: 0x00584474
		private void BuildPage()
		{
			base.RemoveAllChildren();
			UIElement uielement = new UIElement();
			uielement.Width.Set(0f, 0.8f);
			uielement.MaxWidth.Set(500f, 0f);
			uielement.MinWidth.Set(300f, 0f);
			uielement.Top.Set(230f, 0f);
			uielement.Height.Set(-uielement.Top.Pixels, 1f);
			uielement.HAlign = 0.5f;
			base.Append(uielement);
			UIPanel uipanel = new UIPanel();
			uipanel.Width.Set(0f, 1f);
			uipanel.Height.Set(-110f, 1f);
			uipanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
			uielement.Append(uipanel);
			UIElement uielement2 = new UIElement
			{
				Width = StyleDimension.Fill,
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f)
			};
			uipanel.Append(uielement2);
			UIElement uielement3 = new UIElement
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(28f, 0f)
			};
			uielement3.SetPadding(0f);
			uielement2.Append(uielement3);
			uielement3.Append(new UIText(Language.GetTextValue("UI.ReportsPage"), 0.7f, true)
			{
				HAlign = 0.5f,
				VAlign = 0f
			});
			UIElement uielement4 = new UIElement
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(-40f, 1f),
				Top = new StyleDimension(-2f, 0f)
			};
			uielement2.Append(uielement4);
			this._container = uielement4;
			float num = 0f;
			UISlicedImage uislicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1))
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent(-num * 2f, 1f),
				Left = StyleDimension.FromPixels(-num),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Top = StyleDimension.FromPixels(2f)
			};
			uislicedImage.SetSliceDepths(10);
			uislicedImage.Color = Color.LightGray * 0.5f;
			uielement4.Append(uislicedImage);
			UIList uilist = new UIList
			{
				HAlign = 0.5f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				PaddingRight = 20f
			};
			uilist.ListPadding = 40f;
			uilist.ManualSortMethod = new Action<List<UIElement>>(this.ManualIfnoSortingMethod);
			UIElement item = new UIElement();
			uilist.Add(item);
			this.PopulateLogs(uilist);
			uielement4.Append(uilist);
			this._list = uilist;
			UIScrollbar uiscrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue);
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			this._scrollbar = uiscrollbar;
			uilist.SetScrollbar(uiscrollbar);
			uiscrollbar.GoToBottom();
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
			uitextPanel.Width.Set(-10f, 0.5f);
			uitextPanel.Height.Set(50f, 0f);
			uitextPanel.VAlign = 1f;
			uitextPanel.HAlign = 0.5f;
			uitextPanel.Top.Set(-45f, 0f);
			uitextPanel.OnMouseOver += UIReportsPage.FadedMouseOver;
			uitextPanel.OnMouseOut += UIReportsPage.FadedMouseOut;
			uitextPanel.OnLeftClick += this.GoBackClick;
			uitextPanel.SetSnapPoint("GoBack", 0, null, null);
			uielement.Append(uitextPanel);
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x00009E06 File Offset: 0x00008006
		private void ManualIfnoSortingMethod(List<UIElement> list)
		{
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x005866DC File Offset: 0x005848DC
		private void PopulateLogs(UIList listContents)
		{
			List<IssueReport> list = (from report in this._reporters.SelectMany((IProvideReports reporter) => reporter.GetReports())
			orderby report.timeReported
			select report).ToList<IssueReport>();
			if (list.Count == 0)
			{
				UIText item = new UIText(Language.GetTextValue("Workshop.ReportLogsInitialMessage"), 1f, false)
				{
					HAlign = 0f,
					VAlign = 0f,
					Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
					Height = StyleDimension.FromPixelsAndPercent(0f, 0f),
					IsWrapped = true,
					WrappedTextBottomPadding = 0f,
					TextOriginX = 0.5f,
					TextColor = Color.Gray
				};
				listContents.Add(item);
			}
			for (int i = 0; i < list.Count; i++)
			{
				UIText uitext = new UIText(list[i].reportText, 1f, false)
				{
					HAlign = 0f,
					VAlign = 0f,
					Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
					Height = StyleDimension.FromPixelsAndPercent(0f, 0f),
					IsWrapped = true,
					WrappedTextBottomPadding = 0f,
					TextOriginX = 0f
				};
				listContents.Add(uitext);
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Divider", 1);
				UIImage element = new UIImage(asset)
				{
					Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
					Height = StyleDimension.FromPixels((float)asset.Height()),
					ScaleToFit = true,
					VAlign = 1f
				};
				uitext.Append(element);
			}
			UIElement item2 = new UIElement();
			listContents.Add(item2);
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x005868D0 File Offset: 0x00584AD0
		public override void Recalculate()
		{
			if (this._scrollbar != null)
			{
				if (this._isScrollbarAttached && !this._scrollbar.CanScroll)
				{
					this._container.RemoveChild(this._scrollbar);
					this._isScrollbarAttached = false;
					this._list.Width.Set(0f, 1f);
				}
				else if (!this._isScrollbarAttached && this._scrollbar.CanScroll)
				{
					this._container.Append(this._scrollbar);
					this._isScrollbarAttached = true;
					this._list.Width.Set(-25f, 1f);
				}
			}
			base.Recalculate();
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x0058697E File Offset: 0x00584B7E
		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.MenuUI.SetState(this._previousUIState);
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			Main.menuMode = this._menuIdToGoBackTo;
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x005869B0 File Offset: 0x00584BB0
		private static void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x00586A05 File Offset: 0x00584C05
		private static void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x00586A44 File Offset: 0x00584C44
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			this.SetupGamepadPoints(spriteBatch);
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x00586A54 File Offset: 0x00584C54
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 1;
			int num = 3000;
			int idRangeEndExclusive = num;
			List<SnapPoint> snapPoints = this.GetSnapPoints();
			for (int i = 0; i < snapPoints.Count; i++)
			{
				SnapPoint snapPoint = snapPoints[i];
				string name = snapPoint.Name;
				if (name == "GoBack")
				{
					this._helper.MakeLinkPointFromSnapPoint(idRangeEndExclusive++, snapPoint);
				}
			}
			this._helper.MoveToVisuallyClosestPoint(num, idRangeEndExclusive);
		}

		// Token: 0x0400530E RID: 21262
		private UIState _previousUIState;

		// Token: 0x0400530F RID: 21263
		private int _menuIdToGoBackTo;

		// Token: 0x04005310 RID: 21264
		private UIElement _container;

		// Token: 0x04005311 RID: 21265
		private UIList _list;

		// Token: 0x04005312 RID: 21266
		private UIScrollbar _scrollbar;

		// Token: 0x04005313 RID: 21267
		private bool _isScrollbarAttached;

		// Token: 0x04005314 RID: 21268
		private const string _backPointName = "GoBack";

		// Token: 0x04005315 RID: 21269
		private List<IProvideReports> _reporters;

		// Token: 0x04005316 RID: 21270
		private UIGamepadHelper _helper;
	}
}
