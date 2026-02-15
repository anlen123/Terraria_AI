using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003AB RID: 939
	public struct UIGamepadHelper
	{
		// Token: 0x06002B46 RID: 11078 RVA: 0x0058BEE0 File Offset: 0x0058A0E0
		public UILinkPoint[,] CreateUILinkPointGrid(ref int currentID, List<SnapPoint> pointsForGrid, int pointsPerLine, UILinkPoint topLinkPoint, UILinkPoint leftLinkPoint, UILinkPoint rightLinkPoint, UILinkPoint bottomLinkPoint)
		{
			int num = (int)Math.Ceiling((double)((float)pointsForGrid.Count / (float)pointsPerLine));
			UILinkPoint[,] array = new UILinkPoint[pointsPerLine, num];
			for (int i = 0; i < pointsForGrid.Count; i++)
			{
				int num2 = i % pointsPerLine;
				int num3 = i / pointsPerLine;
				UILinkPoint[,] array2 = array;
				int num4 = num2;
				int num5 = num3;
				int num6 = currentID;
				currentID = num6 + 1;
				array2[num4, num5] = this.MakeLinkPointFromSnapPoint(num6, pointsForGrid[i]);
			}
			for (int j = 0; j < array.GetLength(0); j++)
			{
				for (int k = 0; k < array.GetLength(1); k++)
				{
					UILinkPoint uilinkPoint = array[j, k];
					if (uilinkPoint != null)
					{
						if (j < array.GetLength(0) - 1)
						{
							UILinkPoint uilinkPoint2 = array[j + 1, k];
							if (uilinkPoint2 != null)
							{
								this.PairLeftRight(uilinkPoint, uilinkPoint2);
							}
						}
						if (k < array.GetLength(1) - 1)
						{
							UILinkPoint uilinkPoint3 = array[j, k + 1];
							if (uilinkPoint3 != null)
							{
								this.PairUpDown(uilinkPoint, uilinkPoint3);
							}
						}
						if (leftLinkPoint != null && j == 0)
						{
							uilinkPoint.Left = leftLinkPoint.ID;
						}
						if (topLinkPoint != null && k == 0)
						{
							uilinkPoint.Up = topLinkPoint.ID;
						}
						if (rightLinkPoint != null && j == pointsPerLine - 1)
						{
							uilinkPoint.Right = rightLinkPoint.ID;
						}
						if (bottomLinkPoint != null && k == num - 1)
						{
							uilinkPoint.Down = bottomLinkPoint.ID;
						}
					}
				}
			}
			return array;
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x0058C040 File Offset: 0x0058A240
		public void LinkVerticalStrips(UILinkPoint[] stripOnLeft, UILinkPoint[] stripOnRight, int leftStripStartOffset)
		{
			if (stripOnLeft == null || stripOnRight == null)
			{
				return;
			}
			int num = Math.Max(stripOnLeft.Length, stripOnRight.Length);
			int num2 = Math.Min(stripOnLeft.Length, stripOnRight.Length);
			for (int i = 0; i < leftStripStartOffset; i++)
			{
				this.PairLeftRight(stripOnLeft[i], stripOnRight[0]);
			}
			for (int j = 0; j < num2; j++)
			{
				this.PairLeftRight(stripOnLeft[j + leftStripStartOffset], stripOnRight[j]);
			}
			for (int k = num2; k < num; k++)
			{
				if (stripOnLeft.Length > k)
				{
					stripOnLeft[k].Right = stripOnRight[stripOnRight.Length - 1].ID;
				}
				if (stripOnRight.Length > k)
				{
					stripOnRight[k].Left = stripOnLeft[stripOnLeft.Length - 1].ID;
				}
			}
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x0058C0E8 File Offset: 0x0058A2E8
		public void LinkVerticalStripRightSideToSingle(UILinkPoint[] strip, UILinkPoint theSingle)
		{
			if (strip == null || theSingle == null)
			{
				return;
			}
			int num = Math.Max(strip.Length, 1);
			int num2 = Math.Min(strip.Length, 1);
			for (int i = 0; i < num2; i++)
			{
				this.PairLeftRight(strip[i], theSingle);
			}
			for (int j = num2; j < num; j++)
			{
				if (strip.Length > j)
				{
					strip[j].Right = theSingle.ID;
				}
			}
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x0058C148 File Offset: 0x0058A348
		public void RemovePointsOutOfView(List<SnapPoint> pts, UIElement containerPanel, SpriteBatch spriteBatch)
		{
			float scaleFactor = 1f / Main.UIScale;
			Rectangle clippingRectangle = containerPanel.GetClippingRectangle(spriteBatch);
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

		// Token: 0x06002B4A RID: 11082 RVA: 0x0058C1BC File Offset: 0x0058A3BC
		public void LinkHorizontalStripBottomSideToSingle(UILinkPoint[] strip, UILinkPoint theSingle)
		{
			if (strip == null || theSingle == null)
			{
				return;
			}
			for (int i = strip.Length - 1; i >= 0; i--)
			{
				this.PairUpDown(strip[i], theSingle);
			}
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x0058C1EC File Offset: 0x0058A3EC
		public void LinkHorizontalStripUpSideToSingle(UILinkPoint[] strip, UILinkPoint theSingle)
		{
			if (strip == null || theSingle == null)
			{
				return;
			}
			for (int i = strip.Length - 1; i >= 0; i--)
			{
				this.PairUpDown(theSingle, strip[i]);
			}
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x0058C21A File Offset: 0x0058A41A
		public void LinkVerticalStripBottomSideToSingle(UILinkPoint[] strip, UILinkPoint theSingle)
		{
			if (strip == null || theSingle == null)
			{
				return;
			}
			this.PairUpDown(strip[strip.Length - 1], theSingle);
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x0058C234 File Offset: 0x0058A434
		public UILinkPoint[] CreateUILinkStripVertical(ref int currentID, List<SnapPoint> currentStrip)
		{
			UILinkPoint[] array = new UILinkPoint[currentStrip.Count];
			for (int i = 0; i < currentStrip.Count; i++)
			{
				UILinkPoint[] array2 = array;
				int num = i;
				int num2 = currentID;
				currentID = num2 + 1;
				array2[num] = this.MakeLinkPointFromSnapPoint(num2, currentStrip[i]);
			}
			for (int j = 0; j < currentStrip.Count - 1; j++)
			{
				this.PairUpDown(array[j], array[j + 1]);
			}
			return array;
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x0058C29C File Offset: 0x0058A49C
		public UILinkPoint[] CreateUILinkStripHorizontal(ref int currentID, List<SnapPoint> currentStrip)
		{
			UILinkPoint[] array = new UILinkPoint[currentStrip.Count];
			for (int i = 0; i < currentStrip.Count; i++)
			{
				UILinkPoint[] array2 = array;
				int num = i;
				int num2 = currentID;
				currentID = num2 + 1;
				array2[num] = this.MakeLinkPointFromSnapPoint(num2, currentStrip[i]);
			}
			for (int j = 0; j < currentStrip.Count - 1; j++)
			{
				this.PairLeftRight(array[j], array[j + 1]);
			}
			return array;
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x0058C304 File Offset: 0x0058A504
		public void TryMovingBackIntoCreativeGridIfOutOfIt(int start, int currentID)
		{
			List<UILinkPoint> list = new List<UILinkPoint>();
			for (int i = start; i < currentID; i++)
			{
				list.Add(UILinkPointNavigator.Points[i]);
			}
			if (PlayerInput.UsingGamepadUI && UILinkPointNavigator.CurrentPoint >= currentID)
			{
				this.MoveToVisuallyClosestPoint(list);
			}
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x0058C34C File Offset: 0x0058A54C
		public void MoveToVisuallyClosestPoint(List<UILinkPoint> lostrefpoints)
		{
			Dictionary<int, UILinkPoint> points = UILinkPointNavigator.Points;
			Vector2 mouseScreen = Main.MouseScreen;
			UILinkPoint uilinkPoint = null;
			foreach (UILinkPoint uilinkPoint2 in lostrefpoints)
			{
				if (uilinkPoint == null || Vector2.Distance(mouseScreen, uilinkPoint.Position) > Vector2.Distance(mouseScreen, uilinkPoint2.Position))
				{
					uilinkPoint = uilinkPoint2;
				}
			}
			if (uilinkPoint != null)
			{
				UILinkPointNavigator.ChangePoint(uilinkPoint.ID);
			}
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x0058C3D0 File Offset: 0x0058A5D0
		public List<SnapPoint> GetOrderedPointsByCategoryName(List<SnapPoint> pts, string name)
		{
			return (from x in pts
			where x.Name == name
			orderby x.Id
			select x).ToList<SnapPoint>();
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x0058C425 File Offset: 0x0058A625
		public void PairLeftRight(UILinkPoint leftSide, UILinkPoint rightSide)
		{
			if (leftSide != null)
			{
				leftSide.Right = ((rightSide == null) ? -1 : rightSide.ID);
			}
			if (rightSide != null)
			{
				rightSide.Left = ((leftSide == null) ? -1 : leftSide.ID);
			}
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x0058C451 File Offset: 0x0058A651
		public void PairUpDown(UILinkPoint upSide, UILinkPoint downSide)
		{
			if (upSide != null)
			{
				upSide.Down = ((downSide == null) ? -1 : downSide.ID);
			}
			if (downSide != null)
			{
				downSide.Up = ((upSide == null) ? -1 : upSide.ID);
			}
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x0058AF55 File Offset: 0x00589155
		public UILinkPoint MakeLinkPointFromSnapPoint(int id, SnapPoint snap)
		{
			UILinkPointNavigator.SetPosition(id, snap.Position);
			UILinkPoint uilinkPoint = UILinkPointNavigator.Points[id];
			uilinkPoint.Unlink();
			return uilinkPoint;
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x0058C480 File Offset: 0x0058A680
		public UILinkPoint GetLinkPoint(int id, UIElement element)
		{
			SnapPoint snap;
			if (element.GetSnapPoint(out snap))
			{
				return this.MakeLinkPointFromSnapPoint(id, snap);
			}
			return null;
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x0058C4A4 File Offset: 0x0058A6A4
		public UILinkPoint TryMakeLinkPoint(ref int id, SnapPoint snap)
		{
			if (snap == null)
			{
				return null;
			}
			int num = id;
			id = num + 1;
			return this.MakeLinkPointFromSnapPoint(num, snap);
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x0058C4C8 File Offset: 0x0058A6C8
		public UILinkPoint[] GetVerticalStripFromCategoryName(ref int currentID, List<SnapPoint> pts, string categoryName)
		{
			List<SnapPoint> orderedPointsByCategoryName = this.GetOrderedPointsByCategoryName(pts, categoryName);
			UILinkPoint[] result = null;
			if (orderedPointsByCategoryName.Count > 0)
			{
				result = this.CreateUILinkStripVertical(ref currentID, orderedPointsByCategoryName);
			}
			return result;
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x0058C4F4 File Offset: 0x0058A6F4
		public void MoveToVisuallyClosestPoint(int idRangeStartInclusive, int idRangeEndExclusive)
		{
			if (UILinkPointNavigator.CurrentPoint >= idRangeStartInclusive && UILinkPointNavigator.CurrentPoint < idRangeEndExclusive)
			{
				return;
			}
			Dictionary<int, UILinkPoint> points = UILinkPointNavigator.Points;
			Vector2 mouseScreen = Main.MouseScreen;
			UILinkPoint uilinkPoint = null;
			for (int i = idRangeStartInclusive; i < idRangeEndExclusive; i++)
			{
				UILinkPoint uilinkPoint2;
				if (!points.TryGetValue(i, out uilinkPoint2))
				{
					return;
				}
				if (uilinkPoint == null || Vector2.Distance(mouseScreen, uilinkPoint.Position) > Vector2.Distance(mouseScreen, uilinkPoint2.Position))
				{
					uilinkPoint = uilinkPoint2;
				}
			}
			if (uilinkPoint != null)
			{
				UILinkPointNavigator.ChangePoint(uilinkPoint.ID);
			}
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x0058C568 File Offset: 0x0058A768
		public void CullPointsOutOfElementArea(SpriteBatch spriteBatch, List<SnapPoint> pointsAtMiddle, UIElement container)
		{
			float scaleFactor = 1f / Main.UIScale;
			Rectangle clippingRectangle = container.GetClippingRectangle(spriteBatch);
			Vector2 minimum = clippingRectangle.TopLeft() * scaleFactor;
			Vector2 maximum = clippingRectangle.BottomRight() * scaleFactor;
			for (int i = 0; i < pointsAtMiddle.Count; i++)
			{
				if (!pointsAtMiddle[i].Position.Between(minimum, maximum))
				{
					pointsAtMiddle.Remove(pointsAtMiddle[i]);
					i--;
				}
			}
		}
	}
}
