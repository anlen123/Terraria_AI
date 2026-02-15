using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003A7 RID: 935
	public class UIWorkshopWorldImport : UIState, IHaveBackButtonCommand
	{
		// Token: 0x06002AE2 RID: 10978 RVA: 0x005887C4 File Offset: 0x005869C4
		public UIWorkshopWorldImport(UIState uiStateToGoBackTo)
		{
			this._uiStateToGoBackTo = uiStateToGoBackTo;
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x005887E0 File Offset: 0x005869E0
		public override void OnInitialize()
		{
			UIElement uielement = new UIElement();
			uielement.Width.Set(0f, 0.8f);
			uielement.MaxWidth.Set(650f, 0f);
			uielement.Top.Set(220f, 0f);
			uielement.Height.Set(-220f, 1f);
			uielement.HAlign = 0.5f;
			UIPanel uipanel = new UIPanel();
			uipanel.Width.Set(0f, 1f);
			uipanel.Height.Set(-110f, 1f);
			uipanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
			uielement.Append(uipanel);
			this._containerPanel = uipanel;
			this._worldList = new UIList();
			this._worldList.Width.Set(0f, 1f);
			this._worldList.Height.Set(0f, 1f);
			this._worldList.ListPadding = 5f;
			uipanel.Append(this._worldList);
			this._scrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue);
			this._scrollbar.SetView(100f, 1000f);
			this._scrollbar.Height.Set(0f, 1f);
			this._scrollbar.HAlign = 1f;
			this._worldList.SetScrollbar(this._scrollbar);
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.WorkshopImportWorld"), 0.8f, true);
			uitextPanel.HAlign = 0.5f;
			uitextPanel.Top.Set(-40f, 0f);
			uitextPanel.SetPadding(15f);
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
			this._backPanel = uitextPanel2;
			base.Append(uielement);
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x00588A74 File Offset: 0x00586C74
		public override void Recalculate()
		{
			if (this._scrollbar != null)
			{
				if (this._isScrollbarAttached && !this._scrollbar.CanScroll)
				{
					this._containerPanel.RemoveChild(this._scrollbar);
					this._isScrollbarAttached = false;
					this._worldList.Width.Set(0f, 1f);
				}
				else if (!this._isScrollbarAttached && this._scrollbar.CanScroll)
				{
					this._containerPanel.Append(this._scrollbar);
					this._isScrollbarAttached = true;
					this._worldList.Width.Set(-25f, 1f);
				}
			}
			base.Recalculate();
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x00588B22 File Offset: 0x00586D22
		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			this.HandleBackButtonUsage();
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x00588B2A File Offset: 0x00586D2A
		public void HandleBackButtonUsage()
		{
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			Main.MenuUI.SetState(this._uiStateToGoBackTo);
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x00588B54 File Offset: 0x00586D54
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x00587B9D File Offset: 0x00585D9D
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x00588BA9 File Offset: 0x00586DA9
		public override void OnActivate()
		{
			Main.LoadWorlds();
			this.UpdateWorkshopWorldList();
			this.UpdateWorldsList();
			if (PlayerInput.UsingGamepadUI)
			{
				UILinkPointNavigator.ChangePoint(3000 + ((this._worldList.Count == 0) ? 1 : 2));
			}
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x00588BE0 File Offset: 0x00586DE0
		public void UpdateWorkshopWorldList()
		{
			UIWorkshopWorldImport.WorkshopWorldList.Clear();
			if (SocialAPI.Workshop != null)
			{
				foreach (string file in SocialAPI.Workshop.GetListOfSubscribedWorldPaths())
				{
					UIWorkshopWorldImport.WorkshopWorldList.Add(WorldFile.GetAllMetadata(file, false));
				}
			}
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x00588C54 File Offset: 0x00586E54
		private void UpdateWorldsList()
		{
			this._worldList.Clear();
			IEnumerable<WorldFileData> enumerable = from x in new List<WorldFileData>(UIWorkshopWorldImport.WorkshopWorldList)
			orderby x.IsFavorite descending, x.Name, x.GetFileName(true)
			select x;
			int num = 0;
			foreach (WorldFileData data in enumerable)
			{
				this._worldList.Add(new UIWorkshopImportWorldListItem(this, data, num++));
			}
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x00588D30 File Offset: 0x00586F30
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (this.skipDraw)
			{
				this.skipDraw = false;
				return;
			}
			base.Draw(spriteBatch);
			this.SetupGamepadPoints(spriteBatch);
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x00588D50 File Offset: 0x00586F50
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
			int num = 3000;
			UILinkPointNavigator.SetPosition(num, this._backPanel.GetInnerDimensions().ToRectangle().Center.ToVector2());
			int num2 = num;
			UILinkPoint uilinkPoint = UILinkPointNavigator.Points[num2];
			uilinkPoint.Unlink();
			float scaleFactor = 1f / Main.UIScale;
			Rectangle clippingRectangle = this._containerPanel.GetClippingRectangle(spriteBatch);
			Vector2 minimum = clippingRectangle.TopLeft() * scaleFactor;
			Vector2 maximum = clippingRectangle.BottomRight() * scaleFactor;
			List<SnapPoint> snapPoints = this.GetSnapPoints();
			for (int i = 0; i < snapPoints.Count; i++)
			{
				if (!snapPoints[i].Position.Between(minimum, maximum))
				{
					snapPoints.Remove(snapPoints[i]);
					i--;
				}
			}
			SnapPoint[,] array = new SnapPoint[this._worldList.Count, 1];
			foreach (SnapPoint snapPoint in from a in snapPoints
			where a.Name == "Import"
			select a)
			{
				array[snapPoint.Id, 0] = snapPoint;
			}
			num2 = num + 2;
			int[] array2 = new int[this._worldList.Count];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = -1;
			}
			for (int k = 0; k < 1; k++)
			{
				int num3 = -1;
				for (int l = 0; l < array.GetLength(0); l++)
				{
					if (array[l, k] != null)
					{
						uilinkPoint = UILinkPointNavigator.Points[num2];
						uilinkPoint.Unlink();
						UILinkPointNavigator.SetPosition(num2, array[l, k].Position);
						if (num3 != -1)
						{
							uilinkPoint.Up = num3;
							UILinkPointNavigator.Points[num3].Down = num2;
						}
						if (array2[l] != -1)
						{
							uilinkPoint.Left = array2[l];
							UILinkPointNavigator.Points[array2[l]].Right = num2;
						}
						uilinkPoint.Down = num;
						if (k == 0)
						{
							UILinkPointNavigator.Points[num].Up = (UILinkPointNavigator.Points[num + 1].Up = num2);
						}
						num3 = num2;
						array2[l] = num2;
						UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num2;
						num2++;
					}
				}
			}
			if (PlayerInput.UsingGamepadUI && this._worldList.Count == 0 && UILinkPointNavigator.CurrentPoint > 3001)
			{
				UILinkPointNavigator.ChangePoint(3001);
			}
		}

		// Token: 0x04005330 RID: 21296
		private UIList _worldList;

		// Token: 0x04005331 RID: 21297
		private UITextPanel<LocalizedText> _backPanel;

		// Token: 0x04005332 RID: 21298
		private UIPanel _containerPanel;

		// Token: 0x04005333 RID: 21299
		private UIScrollbar _scrollbar;

		// Token: 0x04005334 RID: 21300
		private bool _isScrollbarAttached;

		// Token: 0x04005335 RID: 21301
		private List<Tuple<string, bool>> favoritesCache = new List<Tuple<string, bool>>();

		// Token: 0x04005336 RID: 21302
		private UIState _uiStateToGoBackTo;

		// Token: 0x04005337 RID: 21303
		public static List<WorldFileData> WorkshopWorldList = new List<WorldFileData>();

		// Token: 0x04005338 RID: 21304
		private bool skipDraw;
	}
}
