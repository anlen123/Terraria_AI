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
using Terraria.Map;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003B5 RID: 949
	public class UIWorldSelect : UIState
	{
		// Token: 0x06002CA7 RID: 11431 RVA: 0x0059D460 File Offset: 0x0059B660
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
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.SelectWorld"), 0.8f, true);
			uitextPanel.HAlign = 0.5f;
			uitextPanel.Top.Set(-40f, 0f);
			uitextPanel.SetPadding(15f);
			uitextPanel.BackgroundColor = new Color(73, 94, 171);
			uielement.Append(uitextPanel);
			UITextPanel<LocalizedText> uitextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
			uitextPanel2.Width.Set(-10f, 0.5f);
			uitextPanel2.Height.Set(50f, 0f);
			uitextPanel2.VAlign = 1f;
			uitextPanel2.HAlign = 0f;
			uitextPanel2.Top.Set(-45f, 0f);
			uitextPanel2.OnMouseOver += this.FadedMouseOver;
			uitextPanel2.OnMouseOut += this.FadedMouseOut;
			uitextPanel2.OnLeftClick += this.GoBackClick;
			uielement.Append(uitextPanel2);
			this._backPanel = uitextPanel2;
			UITextPanel<LocalizedText> uitextPanel3 = new UITextPanel<LocalizedText>(Language.GetText("UI.New"), 0.7f, true);
			uitextPanel3.CopyStyle(uitextPanel2);
			uitextPanel3.HAlign = 1f;
			uitextPanel3.OnMouseOver += this.FadedMouseOver;
			uitextPanel3.OnMouseOut += this.FadedMouseOut;
			uitextPanel3.OnLeftClick += this.NewWorldClick;
			uielement.Append(uitextPanel3);
			this._newPanel = uitextPanel3;
			base.Append(uielement);
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x0059D768 File Offset: 0x0059B968
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

		// Token: 0x06002CA9 RID: 11433 RVA: 0x0059D818 File Offset: 0x0059BA18
		private void NewWorldClick(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.newWorldName = Lang.gen[57].Value + " " + (Main.WorldList.Count + 1);
			Main.menuMode = 888;
			Main.MenuUI.SetState(new UIWorldCreation());
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x0059D880 File Offset: 0x0059BA80
		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			Main.menuMode = (Main.menuMultiplayer ? 12 : 1);
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x0059D8A8 File Offset: 0x0059BAA8
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x00587B9D File Offset: 0x00585D9D
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x0059D8FD File Offset: 0x0059BAFD
		public override void OnActivate()
		{
			Main.LoadWorlds();
			this.UpdateWorldsList();
			if (PlayerInput.UsingGamepadUI)
			{
				UILinkPointNavigator.ChangePoint(3000 + ((this._worldList.Count == 0) ? 1 : 2));
			}
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x0059D92D File Offset: 0x0059BB2D
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			UIWorldSelect.NewlyGeneratedWorld = null;
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x0059D93C File Offset: 0x0059BB3C
		private void UpdateWorldsList()
		{
			this._worldList.Clear();
			IEnumerable<WorldFileData> enumerable = Main.WorldList.OrderByDescending(new Func<WorldFileData, bool>(UIWorldSelect.CanWorldBeJoinedByActivePlayer)).ThenByDescending(new Func<WorldFileData, bool>(UIWorldSelect.IsNewlyGenerated)).ThenByDescending((WorldFileData x) => x.IsFavorite).ThenByDescending(new Func<WorldFileData, bool>(UIWorldSelect.HasWorldBeenPlayedByActivePlayer)).ThenByDescending((WorldFileData x) => x.LastPlayed).ThenBy((WorldFileData x) => x.Name).ThenBy((WorldFileData x) => x.GetFileName(true));
			int num = 0;
			foreach (WorldFileData worldFileData in enumerable)
			{
				this._worldList.Add(new UIWorldListItem(worldFileData, num++, UIWorldSelect.CanWorldBeJoinedByActivePlayer(worldFileData), UIWorldSelect.HasWorldBeenPlayedByActivePlayer(worldFileData), UIWorldSelect.IsNewlyGenerated(worldFileData)));
			}
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x0059DA7C File Offset: 0x0059BC7C
		private static bool IsNewlyGenerated(WorldFileData file)
		{
			return UIWorldSelect.NewlyGeneratedWorld != null && file.Path == UIWorldSelect.NewlyGeneratedWorld.Path && file.IsCloudSave == UIWorldSelect.NewlyGeneratedWorld.IsCloudSave;
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x0059DAB0 File Offset: 0x0059BCB0
		private static bool CanWorldBeJoinedByActivePlayer(WorldFileData file)
		{
			bool flag = Main.ActivePlayerFileData.Player.difficulty == 3;
			bool flag2 = file.GameMode == 3;
			return flag == flag2;
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x0059DADC File Offset: 0x0059BCDC
		private static bool HasWorldBeenPlayedByActivePlayer(WorldFileData file)
		{
			string text;
			return WorldMap.TryGetMapPath(Main.ActivePlayerFileData, file, out text);
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x0059DAF6 File Offset: 0x0059BCF6
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

		// Token: 0x06002CB4 RID: 11444 RVA: 0x0059DB38 File Offset: 0x0059BD38
		private bool UpdateFavoritesCache()
		{
			List<WorldFileData> list = new List<WorldFileData>(Main.WorldList);
			list.Sort(delegate(WorldFileData x, WorldFileData y)
			{
				if (x.IsFavorite && !y.IsFavorite)
				{
					return -1;
				}
				if (!x.IsFavorite && y.IsFavorite)
				{
					return 1;
				}
				if (x.Name == null)
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
				foreach (WorldFileData worldFileData in list)
				{
					this.favoritesCache.Add(Tuple.Create<string, bool>(worldFileData.Name, worldFileData.IsFavorite));
				}
				this.UpdateWorldsList();
			}
			return flag;
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x0059DC58 File Offset: 0x0059BE58
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 2;
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
			SnapPoint[,] array = new SnapPoint[this._worldList.Count, 6];
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
			where a.Name == "Seed"
			select a)
			{
				array[snapPoint4.Id, 3] = snapPoint4;
			}
			foreach (SnapPoint snapPoint5 in from a in snapPoints
			where a.Name == "Rename"
			select a)
			{
				array[snapPoint5.Id, 4] = snapPoint5;
			}
			foreach (SnapPoint snapPoint6 in from a in snapPoints
			where a.Name == "Delete"
			select a)
			{
				array[snapPoint6.Id, 5] = snapPoint6;
			}
			num2 = num + 2;
			int[] array2 = new int[this._worldList.Count];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = -1;
			}
			for (int k = 0; k < array.GetLength(1); k++)
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

		// Token: 0x04005415 RID: 21525
		public static WorldFileData NewlyGeneratedWorld;

		// Token: 0x04005416 RID: 21526
		private UIList _worldList;

		// Token: 0x04005417 RID: 21527
		private UITextPanel<LocalizedText> _backPanel;

		// Token: 0x04005418 RID: 21528
		private UITextPanel<LocalizedText> _newPanel;

		// Token: 0x04005419 RID: 21529
		private UIPanel _containerPanel;

		// Token: 0x0400541A RID: 21530
		private UIScrollbar _scrollbar;

		// Token: 0x0400541B RID: 21531
		private bool _isScrollbarAttached;

		// Token: 0x0400541C RID: 21532
		private List<Tuple<string, bool>> favoritesCache = new List<Tuple<string, bool>>();

		// Token: 0x0400541D RID: 21533
		private bool skipDraw;
	}
}
