using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003AC RID: 940
	public class UIEmotesMenu : UIState
	{
		// Token: 0x06002B5A RID: 11098 RVA: 0x0058C5DC File Offset: 0x0058A7DC
		public override void OnActivate()
		{
			this.InitializePage();
			if (Main.gameMenu)
			{
				this._outerContainer.Top.Set(220f, 0f);
				this._outerContainer.Height.Set(-220f, 1f);
				return;
			}
			this._outerContainer.Top.Set(120f, 0f);
			this._outerContainer.Height.Set(-120f, 1f);
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x0058C660 File Offset: 0x0058A860
		public void InitializePage()
		{
			base.RemoveAllChildren();
			UIElement uielement = new UIElement();
			uielement.Width.Set(590f, 0f);
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
			this._container = uipanel;
			UIList uilist = new UIList();
			uilist.Width.Set(-25f, 1f);
			uilist.Height.Set(-50f, 1f);
			uilist.Top.Set(50f, 0f);
			uilist.HAlign = 0.5f;
			uilist.ListPadding = 14f;
			uipanel.Append(uilist);
			this._list = uilist;
			UIScrollbar uiscrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue);
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(-20f, 1f);
			uiscrollbar.HAlign = 1f;
			uiscrollbar.VAlign = 1f;
			uiscrollbar.Top = StyleDimension.FromPixels(-5f);
			uilist.SetScrollbar(uiscrollbar);
			this._scrollBar = uiscrollbar;
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
			uitextPanel.Width.Set(-10f, 0.5f);
			uitextPanel.Height.Set(50f, 0f);
			uitextPanel.VAlign = 1f;
			uitextPanel.HAlign = 0.5f;
			uitextPanel.Top.Set(-45f, 0f);
			uitextPanel.OnMouseOver += this.FadedMouseOver;
			uitextPanel.OnMouseOut += this.FadedMouseOut;
			uitextPanel.OnLeftClick += this.GoBackClick;
			uitextPanel.SetSnapPoint("Back", 0, null, null);
			uielement.Append(uitextPanel);
			this._backPanel = uitextPanel;
			int num = 0;
			this.TryAddingList(Language.GetText("UI.EmoteCategoryGeneral"), ref num, 10, this.GetEmotesGeneral());
			this.TryAddingList(Language.GetText("UI.EmoteCategoryRPS"), ref num, 10, this.GetEmotesRPS());
			this.TryAddingList(Language.GetText("UI.EmoteCategoryItems"), ref num, 11, this.GetEmotesItems());
			this.TryAddingList(Language.GetText("UI.EmoteCategoryBiomesAndEvents"), ref num, 8, this.GetEmotesBiomesAndEvents());
			this.TryAddingList(Language.GetText("UI.EmoteCategoryTownNPCs"), ref num, 9, this.GetEmotesTownNPCs());
			this.TryAddingList(Language.GetText("UI.EmoteCategoryCritters"), ref num, 7, this.GetEmotesCritters());
			this.TryAddingList(Language.GetText("UI.EmoteCategoryBosses"), ref num, 8, this.GetEmotesBosses());
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x0058C990 File Offset: 0x0058AB90
		private void TryAddingList(LocalizedText title, ref int currentGroupIndex, int maxEmotesPerRow, List<int> emoteIds)
		{
			if (emoteIds == null)
			{
				return;
			}
			if (emoteIds.Count == 0)
			{
				return;
			}
			UIList list = this._list;
			int num = currentGroupIndex;
			currentGroupIndex = num + 1;
			list.Add(new EmotesGroupListItem(title, num, maxEmotesPerRow, emoteIds.ToArray()));
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x0058C9D0 File Offset: 0x0058ABD0
		private List<int> GetEmotesGeneral()
		{
			return new List<int>
			{
				0,
				1,
				2,
				3,
				15,
				136,
				94,
				16,
				135,
				134,
				137,
				138,
				139,
				17,
				87,
				88,
				89,
				91,
				92,
				93,
				8,
				9,
				10,
				11,
				14,
				100,
				146,
				147,
				148
			};
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x0058CAE0 File Offset: 0x0058ACE0
		private List<int> GetEmotesRPS()
		{
			return new List<int>
			{
				36,
				37,
				38,
				33,
				34,
				35
			};
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x0058CB18 File Offset: 0x0058AD18
		private List<int> GetEmotesItems()
		{
			return new List<int>
			{
				7,
				73,
				74,
				75,
				76,
				131,
				77,
				78,
				79,
				80,
				81,
				82,
				83,
				84,
				85,
				86,
				90,
				132,
				126,
				127,
				128,
				129,
				149
			};
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x0058CBF0 File Offset: 0x0058ADF0
		private List<int> GetEmotesBiomesAndEvents()
		{
			return new List<int>
			{
				22,
				23,
				24,
				25,
				26,
				27,
				28,
				29,
				30,
				31,
				32,
				18,
				19,
				20,
				21,
				99,
				4,
				5,
				6,
				95,
				96,
				97,
				98
			};
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x0058CCB8 File Offset: 0x0058AEB8
		private List<int> GetEmotesTownNPCs()
		{
			return new List<int>
			{
				101,
				102,
				103,
				104,
				105,
				106,
				107,
				108,
				109,
				110,
				111,
				112,
				113,
				114,
				115,
				116,
				117,
				118,
				119,
				120,
				121,
				122,
				123,
				124,
				125,
				130,
				140,
				141,
				142,
				145
			};
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x0058CDCC File Offset: 0x0058AFCC
		private List<int> GetEmotesCritters()
		{
			List<int> list = new List<int>();
			list.AddRange(new int[]
			{
				12,
				13,
				61,
				62,
				63
			});
			list.AddRange(new int[]
			{
				67,
				68,
				69,
				70
			});
			list.Add(72);
			if (NPC.downedGoblins)
			{
				list.Add(64);
			}
			if (NPC.downedFrost)
			{
				list.Add(66);
			}
			if (NPC.downedPirates)
			{
				list.Add(65);
			}
			if (NPC.downedMartians)
			{
				list.Add(71);
			}
			return list;
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x0058CE54 File Offset: 0x0058B054
		private List<int> GetEmotesBosses()
		{
			List<int> list = new List<int>();
			if (NPC.downedBoss1)
			{
				list.Add(39);
			}
			if (NPC.downedBoss2)
			{
				list.Add(40);
				list.Add(41);
			}
			if (NPC.downedSlimeKing)
			{
				list.Add(51);
			}
			if (NPC.downedDeerclops)
			{
				list.Add(150);
			}
			if (NPC.downedQueenBee)
			{
				list.Add(42);
			}
			if (NPC.downedBoss3)
			{
				list.Add(43);
			}
			if (Main.hardMode)
			{
				list.Add(44);
			}
			if (NPC.downedQueenSlime)
			{
				list.Add(144);
			}
			if (NPC.downedMechBoss1)
			{
				list.Add(45);
			}
			if (NPC.downedMechBoss3)
			{
				list.Add(46);
			}
			if (NPC.downedMechBoss2)
			{
				list.Add(47);
			}
			if (NPC.downedPlantBoss)
			{
				list.Add(48);
			}
			if (NPC.downedGolemBoss)
			{
				list.Add(49);
			}
			if (NPC.downedFishron)
			{
				list.Add(50);
			}
			if (NPC.downedEmpressOfLight)
			{
				list.Add(143);
			}
			if (NPC.downedAncientCultist)
			{
				list.Add(52);
			}
			if (NPC.downedMoonlord)
			{
				list.Add(53);
			}
			if (NPC.downedHalloweenTree)
			{
				list.Add(54);
			}
			if (NPC.downedHalloweenKing)
			{
				list.Add(55);
			}
			if (NPC.downedChristmasTree)
			{
				list.Add(56);
			}
			if (NPC.downedChristmasIceQueen)
			{
				list.Add(57);
			}
			if (NPC.downedChristmasSantank)
			{
				list.Add(58);
			}
			if (NPC.downedPirates)
			{
				list.Add(59);
			}
			if (NPC.downedMartians)
			{
				list.Add(60);
			}
			if (DD2Event.DownedInvasionAnyDifficulty)
			{
				list.Add(133);
			}
			return list;
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x0058CFF4 File Offset: 0x0058B1F4
		public override void Recalculate()
		{
			if (this._scrollBar != null)
			{
				if (this._isScrollbarAttached && !this._scrollBar.CanScroll)
				{
					this._container.RemoveChild(this._scrollBar);
					this._isScrollbarAttached = false;
					this._list.Width.Set(0f, 1f);
				}
				else if (!this._isScrollbarAttached && this._scrollBar.CanScroll)
				{
					this._container.Append(this._scrollBar);
					this._isScrollbarAttached = true;
					this._list.Width.Set(-25f, 1f);
				}
			}
			base.Recalculate();
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x0058D0A2 File Offset: 0x0058B2A2
		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.menuMode = 0;
			IngameFancyUI.Close(false);
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x0058D0B0 File Offset: 0x0058B2B0
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x00584489 File Offset: 0x00582689
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002B68 RID: 11112 RVA: 0x0058D105 File Offset: 0x0058B305
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			this.SetupGamepadPoints2(spriteBatch);
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x0058D118 File Offset: 0x0058B318
		private void SetupGamepadPoints2(SpriteBatch spriteBatch)
		{
			int num = 7;
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 1;
			int num2;
			int id = num2 = 3001;
			List<SnapPoint> snapPoints = this.GetSnapPoints();
			this.RemoveSnapPointsOutOfScreen(spriteBatch, snapPoints);
			UILinkPointNavigator.SetPosition(id, this._backPanel.GetInnerDimensions().ToRectangle().Center.ToVector2());
			UILinkPoint uilinkPoint = UILinkPointNavigator.Points[num2];
			uilinkPoint.Unlink();
			uilinkPoint.Up = num2 + 1;
			UILinkPoint uilinkPoint2 = uilinkPoint;
			num2++;
			int num3 = 0;
			List<List<SnapPoint>> list = new List<List<SnapPoint>>();
			for (int i = 0; i < num; i++)
			{
				List<SnapPoint> emoteGroup = this.GetEmoteGroup(snapPoints, i);
				if (emoteGroup.Count > 0)
				{
					list.Add(emoteGroup);
				}
				num3 += (int)Math.Ceiling((double)((float)emoteGroup.Count / 14f));
			}
			SnapPoint[,] array = new SnapPoint[14, num3];
			int num4 = 0;
			for (int j = 0; j < list.Count; j++)
			{
				List<SnapPoint> list2 = list[j];
				for (int k = 0; k < list2.Count; k++)
				{
					int num5 = num4 + k / 14;
					int num6 = k % 14;
					array[num6, num5] = list2[k];
				}
				num4 += (int)Math.Ceiling((double)((float)list2.Count / 14f));
			}
			int[,] array2 = new int[14, num3];
			int up = 0;
			for (int l = 0; l < array.GetLength(1); l++)
			{
				for (int m = 0; m < array.GetLength(0); m++)
				{
					SnapPoint snapPoint = array[m, l];
					if (snapPoint != null)
					{
						UILinkPointNavigator.Points[num2].Unlink();
						UILinkPointNavigator.SetPosition(num2, snapPoint.Position);
						array2[m, l] = num2;
						if (m == 0)
						{
							up = num2;
						}
						num2++;
					}
				}
			}
			uilinkPoint2.Up = up;
			for (int n = 0; n < array.GetLength(1); n++)
			{
				for (int num7 = 0; num7 < array.GetLength(0); num7++)
				{
					int num8 = array2[num7, n];
					if (num8 != 0)
					{
						UILinkPoint uilinkPoint3 = UILinkPointNavigator.Points[num8];
						if (this.TryGetPointOnGrid(array2, num7, n, -1, 0))
						{
							uilinkPoint3.Left = array2[num7 - 1, n];
						}
						else
						{
							uilinkPoint3.Left = uilinkPoint3.ID;
							for (int num9 = num7; num9 < array.GetLength(0); num9++)
							{
								if (this.TryGetPointOnGrid(array2, num9, n, 0, 0))
								{
									uilinkPoint3.Left = array2[num9, n];
								}
							}
						}
						if (this.TryGetPointOnGrid(array2, num7, n, 1, 0))
						{
							uilinkPoint3.Right = array2[num7 + 1, n];
						}
						else
						{
							uilinkPoint3.Right = uilinkPoint3.ID;
							for (int num10 = num7; num10 >= 0; num10--)
							{
								if (this.TryGetPointOnGrid(array2, num10, n, 0, 0))
								{
									uilinkPoint3.Right = array2[num10, n];
								}
							}
						}
						if (this.TryGetPointOnGrid(array2, num7, n, 0, -1))
						{
							uilinkPoint3.Up = array2[num7, n - 1];
						}
						else
						{
							uilinkPoint3.Up = uilinkPoint3.ID;
							for (int num11 = n - 1; num11 >= 0; num11--)
							{
								if (this.TryGetPointOnGrid(array2, num7, num11, 0, 0))
								{
									uilinkPoint3.Up = array2[num7, num11];
									break;
								}
							}
						}
						if (this.TryGetPointOnGrid(array2, num7, n, 0, 1))
						{
							uilinkPoint3.Down = array2[num7, n + 1];
						}
						else
						{
							uilinkPoint3.Down = uilinkPoint3.ID;
							for (int num12 = n + 1; num12 < array.GetLength(1); num12++)
							{
								if (this.TryGetPointOnGrid(array2, num7, num12, 0, 0))
								{
									uilinkPoint3.Down = array2[num7, num12];
									break;
								}
							}
							if (uilinkPoint3.Down == uilinkPoint3.ID)
							{
								uilinkPoint3.Down = uilinkPoint2.ID;
							}
						}
					}
				}
			}
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x0058D514 File Offset: 0x0058B714
		private bool TryGetPointOnGrid(int[,] grid, int x, int y, int offsetX, int offsetY)
		{
			return x + offsetX >= 0 && x + offsetX < grid.GetLength(0) && y + offsetY >= 0 && y + offsetY < grid.GetLength(1) && grid[x + offsetX, y + offsetY] != 0;
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x0058D564 File Offset: 0x0058B764
		private void RemoveSnapPointsOutOfScreen(SpriteBatch spriteBatch, List<SnapPoint> pts)
		{
			float scaleFactor = 1f / Main.UIScale;
			Rectangle clippingRectangle = this._container.GetClippingRectangle(spriteBatch);
			Vector2 minimum = clippingRectangle.TopLeft() * scaleFactor;
			Vector2 maximum = clippingRectangle.BottomRight() * scaleFactor;
			for (int i = 0; i < pts.Count; i++)
			{
				if (!pts[i].Position.Between(minimum, maximum))
				{
					pts.Remove(pts[i]);
					i--;
				}
			}
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x0058D5DC File Offset: 0x0058B7DC
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 1;
			int num = 3001;
			int num2 = num;
			List<SnapPoint> snapPoints = this.GetSnapPoints();
			UILinkPointNavigator.SetPosition(num, this._backPanel.GetInnerDimensions().ToRectangle().Center.ToVector2());
			UILinkPoint uilinkPoint = UILinkPointNavigator.Points[num2];
			uilinkPoint.Unlink();
			uilinkPoint.Up = num2 + 1;
			UILinkPoint uilinkPoint2 = uilinkPoint;
			num2++;
			float scaleFactor = 1f / Main.UIScale;
			Rectangle clippingRectangle = this._container.GetClippingRectangle(spriteBatch);
			Vector2 minimum = clippingRectangle.TopLeft() * scaleFactor;
			Vector2 maximum = clippingRectangle.BottomRight() * scaleFactor;
			for (int i = 0; i < snapPoints.Count; i++)
			{
				if (!snapPoints[i].Position.Between(minimum, maximum))
				{
					snapPoints.Remove(snapPoints[i]);
					i--;
				}
			}
			int num3 = 0;
			int num4 = 7;
			List<List<SnapPoint>> list = new List<List<SnapPoint>>();
			for (int j = 0; j < num4; j++)
			{
				List<SnapPoint> emoteGroup = this.GetEmoteGroup(snapPoints, j);
				if (emoteGroup.Count > 0)
				{
					list.Add(emoteGroup);
				}
			}
			List<SnapPoint>[] array = list.ToArray();
			for (int k = 0; k < array.Length; k++)
			{
				List<SnapPoint> list2 = array[k];
				int num5 = list2.Count / 14;
				if (list2.Count % 14 > 0)
				{
					num5++;
				}
				int num6 = 14;
				if (list2.Count % 14 != 0)
				{
					num6 = list2.Count % 14;
				}
				for (int l = 0; l < list2.Count; l++)
				{
					uilinkPoint = UILinkPointNavigator.Points[num2];
					uilinkPoint.Unlink();
					UILinkPointNavigator.SetPosition(num2, list2[l].Position);
					int num7 = 14;
					if (l / 14 == num5 - 1 && list2.Count % 14 != 0)
					{
						num7 = list2.Count % 14;
					}
					int num8 = l % 14;
					uilinkPoint.Left = num2 - 1;
					uilinkPoint.Right = num2 + 1;
					uilinkPoint.Up = num2 - 14;
					uilinkPoint.Down = num2 + 14;
					if (num8 == num7 - 1)
					{
						uilinkPoint.Right = num2 - num7 + 1;
					}
					if (num8 == 0)
					{
						uilinkPoint.Left = num2 + num7 - 1;
					}
					if (num8 == 0)
					{
						uilinkPoint2.Up = num2;
					}
					if (l < 14)
					{
						if (num3 == 0)
						{
							uilinkPoint.Up = -1;
						}
						else
						{
							uilinkPoint.Up = num2 - 14;
							if (num8 >= num3)
							{
								uilinkPoint.Up -= 14;
							}
							int num9 = k - 1;
							while (num9 > 0 && array[num9].Count <= num8)
							{
								uilinkPoint.Up -= 14;
								num9--;
							}
						}
					}
					int down = num;
					if (k == array.Length - 1)
					{
						if (l / 14 < num5 - 1 && num8 >= list2.Count % 14)
						{
							uilinkPoint.Down = down;
						}
						if (l / 14 == num5 - 1)
						{
							uilinkPoint.Down = down;
						}
					}
					else if (l / 14 == num5 - 1)
					{
						uilinkPoint.Down = num2 + 14;
						int num10 = k + 1;
						while (num10 < array.Length && array[num10].Count <= num8)
						{
							uilinkPoint.Down += 14;
							num10++;
						}
						if (k == array.Length - 1)
						{
							uilinkPoint.Down = down;
						}
					}
					else if (num8 >= num6)
					{
						uilinkPoint.Down = num2 + 14 + 14;
						int num11 = k + 1;
						while (num11 < array.Length && array[num11].Count <= num8)
						{
							uilinkPoint.Down += 14;
							num11++;
						}
					}
					num2++;
				}
				num3 = num6;
				int num12 = 14 - num3;
				num2 += num12;
			}
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x0058D98C File Offset: 0x0058BB8C
		private List<SnapPoint> GetEmoteGroup(List<SnapPoint> ptsOnPage, int groupIndex)
		{
			string groupName = "Group " + groupIndex;
			List<SnapPoint> list = (from a in ptsOnPage
			where a.Name == groupName
			select a).ToList<SnapPoint>();
			list.Sort(new Comparison<SnapPoint>(this.SortPoints));
			return list;
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x0058D9E0 File Offset: 0x0058BBE0
		private int SortPoints(SnapPoint a, SnapPoint b)
		{
			return a.Id.CompareTo(b.Id);
		}

		// Token: 0x04005364 RID: 21348
		private UIElement _outerContainer;

		// Token: 0x04005365 RID: 21349
		private UIElement _backPanel;

		// Token: 0x04005366 RID: 21350
		private UIElement _container;

		// Token: 0x04005367 RID: 21351
		private UIList _list;

		// Token: 0x04005368 RID: 21352
		private UIScrollbar _scrollBar;

		// Token: 0x04005369 RID: 21353
		private bool _isScrollbarAttached;
	}
}
