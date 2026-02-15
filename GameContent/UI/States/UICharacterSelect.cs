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
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003B6 RID: 950
	public class UICharacterSelect : UIState
	{
		// Token: 0x06002CB7 RID: 11447 RVA: 0x0059E190 File Offset: 0x0059C390
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
			this._containerPanel = uipanel;
			uielement.Append(uipanel);
			this._playerList = new UIList();
			this._playerList.Width.Set(0f, 1f);
			this._playerList.Height.Set(0f, 1f);
			this._playerList.ListPadding = 5f;
			uipanel.Append(this._playerList);
			this._scrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue);
			this._scrollbar.SetView(100f, 1000f);
			this._scrollbar.Height.Set(0f, 1f);
			this._scrollbar.HAlign = 1f;
			this._playerList.SetScrollbar(this._scrollbar);
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.SelectPlayer"), 0.8f, true);
			uitextPanel.HAlign = 0.5f;
			uitextPanel.Top.Set(-40f, 0f);
			uitextPanel.SetPadding(15f);
			uitextPanel.BackgroundColor = new Color(73, 94, 171);
			uielement.Append(uitextPanel);
			UITextPanel<LocalizedText> uitextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
			uitextPanel2.Width.Set(-10f, 0.5f);
			uitextPanel2.Height.Set(50f, 0f);
			uitextPanel2.VAlign = 1f;
			uitextPanel2.Top.Set(-45f, 0f);
			uitextPanel2.OnMouseOver += this.FadedMouseOver;
			uitextPanel2.OnMouseOut += this.FadedMouseOut;
			uitextPanel2.OnLeftClick += this.GoBackClick;
			uitextPanel2.SetSnapPoint("Back", 0, null, null);
			uielement.Append(uitextPanel2);
			this._backPanel = uitextPanel2;
			UITextPanel<LocalizedText> uitextPanel3 = new UITextPanel<LocalizedText>(Language.GetText("UI.New"), 0.7f, true);
			uitextPanel3.CopyStyle(uitextPanel2);
			uitextPanel3.HAlign = 1f;
			uitextPanel3.OnMouseOver += this.FadedMouseOver;
			uitextPanel3.OnMouseOut += this.FadedMouseOut;
			uitextPanel3.OnLeftClick += this.NewCharacterClick;
			uielement.Append(uitextPanel3);
			uitextPanel2.SetSnapPoint("New", 0, null, null);
			this._newPanel = uitextPanel3;
			base.Append(uielement);
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x0059E4D0 File Offset: 0x0059C6D0
		public override void Recalculate()
		{
			if (this._scrollbar != null)
			{
				if (this._isScrollbarAttached && !this._scrollbar.CanScroll)
				{
					this._containerPanel.RemoveChild(this._scrollbar);
					this._isScrollbarAttached = false;
					this._playerList.Width.Set(0f, 1f);
				}
				else if (!this._isScrollbarAttached && this._scrollbar.CanScroll)
				{
					this._containerPanel.Append(this._scrollbar);
					this._isScrollbarAttached = true;
					this._playerList.Width.Set(-25f, 1f);
				}
			}
			base.Recalculate();
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x0059E57E File Offset: 0x0059C77E
		private void NewCharacterClick(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.PendingPlayer = new Player();
			Main.menuMode = 888;
			Main.MenuUI.SetState(new UICharacterCreation(Main.PendingPlayer));
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x0059E5BD File Offset: 0x0059C7BD
		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			Main.menuMode = 0;
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x0059E5DC File Offset: 0x0059C7DC
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x00587B9D File Offset: 0x00585D9D
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x0059E631 File Offset: 0x0059C831
		public override void OnActivate()
		{
			Main.LoadPlayers();
			Main.ActivePlayerFileData = new PlayerFileData();
			this.UpdatePlayersList();
			if (PlayerInput.UsingGamepadUI)
			{
				UILinkPointNavigator.ChangePoint(3000 + ((this._playerList.Count == 0) ? 1 : 2));
			}
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x0059E66C File Offset: 0x0059C86C
		private void UpdatePlayersList()
		{
			this._playerList.Clear();
			IEnumerable<PlayerFileData> enumerable = from x in Main.PlayerList
			orderby x.IsFavorite descending, x.LastPlayed descending, x.Name, x.GetFileName(true)
			select x;
			int num = 0;
			foreach (PlayerFileData data in enumerable)
			{
				this._playerList.Add(new UICharacterListItem(data, num++));
			}
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x0059E764 File Offset: 0x0059C964
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (this.skipDraw)
			{
				this.skipDraw = false;
				return;
			}
			if (this.UpdateFavoritesCache())
			{
				this.skipDraw = true;
				Main.MenuUI.Draw(spriteBatch, new GameTime());
			}
			base.Draw(spriteBatch);
			this.SetupGamepadPoints(spriteBatch);
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x0059E7A4 File Offset: 0x0059C9A4
		private bool UpdateFavoritesCache()
		{
			List<PlayerFileData> list = new List<PlayerFileData>(Main.PlayerList);
			list.Sort(delegate(PlayerFileData x, PlayerFileData y)
			{
				if (x.IsFavorite && !y.IsFavorite)
				{
					return -1;
				}
				if (!x.IsFavorite && y.IsFavorite)
				{
					return 1;
				}
				if (x.Name.CompareTo(y.Name) != 0)
				{
					return x.Name.CompareTo(y.Name);
				}
				return x.GetFileName(true).CompareTo(y.GetFileName(true));
			});
			bool flag = false;
			if (!flag && list.Count != this.favoritesCache.Count)
			{
				flag = true;
			}
			if (!flag)
			{
				for (int i = 0; i < this.favoritesCache.Count; i++)
				{
					Tuple<string, bool> tuple = this.favoritesCache[i];
					if (!(list[i].Name == tuple.Item1) || list[i].IsFavorite != tuple.Item2)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				this.favoritesCache.Clear();
				foreach (PlayerFileData playerFileData in list)
				{
					this.favoritesCache.Add(Tuple.Create<string, bool>(playerFileData.Name, playerFileData.IsFavorite));
				}
				this.UpdatePlayersList();
			}
			return flag;
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x0059E8C4 File Offset: 0x0059CAC4
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 1;
			int num = 3000;
			UILinkPointNavigator.SetPosition(num, this._backPanel.GetInnerDimensions().ToRectangle().Center.ToVector2());
			UILinkPointNavigator.SetPosition(num + 1, this._newPanel.GetInnerDimensions().ToRectangle().Center.ToVector2());
			int num2 = num;
			UILinkPoint uilinkPoint = UILinkPointNavigator.Points[num2];
			uilinkPoint.Unlink();
			uilinkPoint.Right = num2 + 1;
			num2 = num + 1;
			uilinkPoint = UILinkPointNavigator.Points[num2];
			uilinkPoint.Unlink();
			uilinkPoint.Left = num2 - 1;
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
			int num3 = 5;
			SnapPoint[,] array = new SnapPoint[this._playerList.Count, num3];
			foreach (SnapPoint snapPoint in from a in snapPoints
			where a.Name == "Play"
			select a)
			{
				array[snapPoint.Id, 0] = snapPoint;
			}
			foreach (SnapPoint snapPoint2 in from a in snapPoints
			where a.Name == "Favorite"
			select a)
			{
				array[snapPoint2.Id, 1] = snapPoint2;
			}
			foreach (SnapPoint snapPoint3 in from a in snapPoints
			where a.Name == "Cloud"
			select a)
			{
				array[snapPoint3.Id, 2] = snapPoint3;
			}
			foreach (SnapPoint snapPoint4 in from a in snapPoints
			where a.Name == "Rename"
			select a)
			{
				array[snapPoint4.Id, 3] = snapPoint4;
			}
			foreach (SnapPoint snapPoint5 in from a in snapPoints
			where a.Name == "Delete"
			select a)
			{
				array[snapPoint5.Id, 4] = snapPoint5;
			}
			num2 = num + 2;
			int[] array2 = new int[this._playerList.Count];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = -1;
			}
			for (int k = 0; k < num3; k++)
			{
				int num4 = -1;
				for (int l = 0; l < array.GetLength(0); l++)
				{
					if (array[l, k] != null)
					{
						uilinkPoint = UILinkPointNavigator.Points[num2];
						uilinkPoint.Unlink();
						UILinkPointNavigator.SetPosition(num2, array[l, k].Position);
						if (num4 != -1)
						{
							uilinkPoint.Up = num4;
							UILinkPointNavigator.Points[num4].Down = num2;
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
						num4 = num2;
						array2[l] = num2;
						UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num2;
						num2++;
					}
				}
			}
			if (PlayerInput.UsingGamepadUI && this._playerList.Count == 0 && UILinkPointNavigator.CurrentPoint > 3001)
			{
				UILinkPointNavigator.ChangePoint(3001);
			}
		}

		// Token: 0x0400541E RID: 21534
		private UIList _playerList;

		// Token: 0x0400541F RID: 21535
		private UITextPanel<LocalizedText> _backPanel;

		// Token: 0x04005420 RID: 21536
		private UITextPanel<LocalizedText> _newPanel;

		// Token: 0x04005421 RID: 21537
		private UIPanel _containerPanel;

		// Token: 0x04005422 RID: 21538
		private UIScrollbar _scrollbar;

		// Token: 0x04005423 RID: 21539
		private bool _isScrollbarAttached;

		// Token: 0x04005424 RID: 21540
		private List<Tuple<string, bool>> favoritesCache = new List<Tuple<string, bool>>();

		// Token: 0x04005425 RID: 21541
		private bool skipDraw;
	}
}
