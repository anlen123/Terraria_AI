using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003A8 RID: 936
	public class UIResourcePackInfoMenu : UIState
	{
		// Token: 0x06002AEF RID: 10991 RVA: 0x0058900C File Offset: 0x0058720C
		public UIResourcePackInfoMenu(UIResourcePackSelectionMenu parent, ResourcePack pack)
		{
			this._resourceMenu = parent;
			this._pack = pack;
			this.BuildPage();
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x00589028 File Offset: 0x00587228
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
				Height = new StyleDimension(52f, 0f)
			};
			uielement3.SetPadding(0f);
			uielement2.Append(uielement3);
			UIText uitext = new UIText(this._pack.Name, 0.7f, true)
			{
				TextColor = Color.Gold
			};
			uitext.HAlign = 0.5f;
			uitext.VAlign = 0f;
			uielement3.Append(uitext);
			UIText uitext2 = new UIText(Language.GetTextValue("UI.Author", this._pack.Author), 0.9f, false)
			{
				HAlign = 0f,
				VAlign = 1f
			};
			uitext2.Top.Set(-6f, 0f);
			uielement3.Append(uitext2);
			UIText uitext3 = new UIText(Language.GetTextValue("UI.Version", this._pack.Version.GetFormattedVersion()), 0.9f, false)
			{
				HAlign = 1f,
				VAlign = 1f,
				TextColor = Color.Silver
			};
			uitext3.Top.Set(-6f, 0f);
			uielement3.Append(uitext3);
			Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/Divider", 1);
			UIImage uiimage = new UIImage(asset)
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixels((float)asset.Height()),
				ScaleToFit = true
			};
			uiimage.Top.Set(52f, 0f);
			uiimage.SetPadding(6f);
			uielement2.Append(uiimage);
			UIElement uielement4 = new UIElement
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(-74f, 1f)
			};
			uielement2.Append(uielement4);
			this._container = uielement4;
			UIText item = new UIText(this._pack.Description, 1f, false)
			{
				HAlign = 0.5f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 0f),
				IsWrapped = true,
				WrappedTextBottomPadding = 0f
			};
			UIList uilist = new UIList
			{
				HAlign = 0.5f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				PaddingRight = 20f
			};
			uilist.ListPadding = 5f;
			uilist.Add(item);
			uielement4.Append(uilist);
			this._list = uilist;
			UIScrollbar uiscrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue);
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			this._scrollbar = uiscrollbar;
			uilist.SetScrollbar(uiscrollbar);
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
			uitextPanel.Width.Set(-10f, 0.5f);
			uitextPanel.Height.Set(50f, 0f);
			uitextPanel.VAlign = 1f;
			uitextPanel.HAlign = 0.5f;
			uitextPanel.Top.Set(-45f, 0f);
			uitextPanel.OnMouseOver += UIResourcePackInfoMenu.FadedMouseOver;
			uitextPanel.OnMouseOut += UIResourcePackInfoMenu.FadedMouseOut;
			uitextPanel.OnLeftClick += this.GoBackClick;
			uitextPanel.SetSnapPoint("GoBack", 0, null, null);
			uielement.Append(uitextPanel);
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x00589554 File Offset: 0x00587754
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

		// Token: 0x06002AF2 RID: 10994 RVA: 0x00589602 File Offset: 0x00587802
		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.MenuUI.SetState(this._resourceMenu);
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x00589614 File Offset: 0x00587814
		private static void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x00586A05 File Offset: 0x00584C05
		private static void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x00589669 File Offset: 0x00587869
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			this.SetupGamepadPoints(spriteBatch);
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x0058967C File Offset: 0x0058787C
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

		// Token: 0x04005339 RID: 21305
		private UIResourcePackSelectionMenu _resourceMenu;

		// Token: 0x0400533A RID: 21306
		private ResourcePack _pack;

		// Token: 0x0400533B RID: 21307
		private UIElement _container;

		// Token: 0x0400533C RID: 21308
		private UIList _list;

		// Token: 0x0400533D RID: 21309
		private UIScrollbar _scrollbar;

		// Token: 0x0400533E RID: 21310
		private bool _isScrollbarAttached;

		// Token: 0x0400533F RID: 21311
		private const string _backPointName = "GoBack";

		// Token: 0x04005340 RID: 21312
		private UIGamepadHelper _helper;
	}
}
