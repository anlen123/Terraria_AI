using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x02000402 RID: 1026
	public class UIList : UIElement, IEnumerable<UIElement>, IEnumerable
	{
		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06002F02 RID: 12034 RVA: 0x005B01ED File Offset: 0x005AE3ED
		public int Count
		{
			get
			{
				return this._items.Count;
			}
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x005B01FC File Offset: 0x005AE3FC
		public UIList()
		{
			this._innerList.OverflowHidden = false;
			this._innerList.Width.Set(0f, 1f);
			this._innerList.Height.Set(0f, 1f);
			this.OverflowHidden = true;
			base.Append(this._innerList);
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x005B0283 File Offset: 0x005AE483
		public float GetTotalHeight()
		{
			return this._innerListHeight;
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x005B028C File Offset: 0x005AE48C
		public void Goto(UIList.ElementSearchMethod searchMethod)
		{
			for (int i = 0; i < this._items.Count; i++)
			{
				if (searchMethod(this._items[i]))
				{
					this._scrollbar.ViewPosition = this._items[i].Top.Pixels;
					return;
				}
			}
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x005B02E5 File Offset: 0x005AE4E5
		public virtual void Add(UIElement item)
		{
			this._items.Add(item);
			this._innerList.Append(item);
			this.UpdateOrder();
			this._innerList.Recalculate();
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x005B0310 File Offset: 0x005AE510
		public virtual bool Remove(UIElement item)
		{
			this._innerList.RemoveChild(item);
			this.UpdateOrder();
			return this._items.Remove(item);
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x005B0330 File Offset: 0x005AE530
		public virtual void Clear()
		{
			this._innerList.RemoveAllChildren();
			this._items.Clear();
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x005B0348 File Offset: 0x005AE548
		public override void Recalculate()
		{
			base.Recalculate();
			this.UpdateScrollbar();
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x005B0356 File Offset: 0x005AE556
		public override void ScrollWheel(UIScrollWheelEvent evt)
		{
			base.ScrollWheel(evt);
			if (this._scrollbar != null)
			{
				this._scrollbar.ViewPosition -= (float)evt.ScrollWheelValue;
			}
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x005B0380 File Offset: 0x005AE580
		public override void RecalculateChildren()
		{
			base.RecalculateChildren();
			float num = 0f;
			for (int i = 0; i < this._items.Count; i++)
			{
				float num2 = (this._items.Count == 1) ? 0f : this.ListPadding;
				this._items[i].Top.Set(num, 0f);
				this._items[i].Recalculate();
				CalculatedStyle outerDimensions = this._items[i].GetOuterDimensions();
				num += outerDimensions.Height + num2;
			}
			this._innerListHeight = num;
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x005B041C File Offset: 0x005AE61C
		private void UpdateScrollbar()
		{
			if (this._scrollbar == null)
			{
				return;
			}
			float height = base.GetInnerDimensions().Height;
			this._scrollbar.SetView(height, this._innerListHeight);
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x005B0450 File Offset: 0x005AE650
		public void SetScrollbar(UIScrollbar scrollbar)
		{
			this._scrollbar = scrollbar;
			this.UpdateScrollbar();
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x005B045F File Offset: 0x005AE65F
		public void UpdateOrder()
		{
			if (this.ManualSortMethod != null)
			{
				this.ManualSortMethod(this._items);
			}
			else
			{
				this._items.Sort(new Comparison<UIElement>(this.SortMethod));
			}
			this.UpdateScrollbar();
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x005B0499 File Offset: 0x005AE699
		public int SortMethod(UIElement item1, UIElement item2)
		{
			return item1.CompareTo(item2);
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x005B04A4 File Offset: 0x005AE6A4
		public override List<SnapPoint> GetSnapPoints()
		{
			List<SnapPoint> list = new List<SnapPoint>();
			SnapPoint item;
			if (base.GetSnapPoint(out item))
			{
				list.Add(item);
			}
			foreach (UIElement uielement in this._items)
			{
				list.AddRange(uielement.GetSnapPoints());
			}
			return list;
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x005B0514 File Offset: 0x005AE714
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (this._scrollbar != null)
			{
				this._innerList.Top.Set(-this._scrollbar.GetValue(), 0f);
			}
			this.Recalculate();
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x005B0545 File Offset: 0x005AE745
		public IEnumerator<UIElement> GetEnumerator()
		{
			return ((IEnumerable<UIElement>)this._items).GetEnumerator();
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x005B0545 File Offset: 0x005AE745
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<UIElement>)this._items).GetEnumerator();
		}

		// Token: 0x0400560F RID: 22031
		protected List<UIElement> _items = new List<UIElement>();

		// Token: 0x04005610 RID: 22032
		protected UIScrollbar _scrollbar;

		// Token: 0x04005611 RID: 22033
		private UIElement _innerList = new UIList.UIInnerList();

		// Token: 0x04005612 RID: 22034
		private float _innerListHeight;

		// Token: 0x04005613 RID: 22035
		public float ListPadding = 5f;

		// Token: 0x04005614 RID: 22036
		public Action<List<UIElement>> ManualSortMethod;

		// Token: 0x02000933 RID: 2355
		// (Invoke) Token: 0x06004811 RID: 18449
		public delegate bool ElementSearchMethod(UIElement element);

		// Token: 0x02000934 RID: 2356
		private class UIInnerList : UIElement
		{
			// Token: 0x06004814 RID: 18452 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool ContainsPoint(Vector2 point)
			{
				return true;
			}

			// Token: 0x06004815 RID: 18453 RVA: 0x006CB1F0 File Offset: 0x006C93F0
			protected override void DrawChildren(SpriteBatch spriteBatch)
			{
				Vector2 position = base.Parent.GetDimensions().Position();
				Vector2 dimensions = new Vector2(base.Parent.GetDimensions().Width, base.Parent.GetDimensions().Height);
				foreach (UIElement uielement in this.Elements)
				{
					Vector2 position2 = uielement.GetDimensions().Position();
					Vector2 dimensions2 = new Vector2(uielement.GetDimensions().Width, uielement.GetDimensions().Height);
					if (Collision.CheckAABBvAABBCollision(position, dimensions, position2, dimensions2))
					{
						uielement.Draw(spriteBatch);
					}
				}
			}

			// Token: 0x06004816 RID: 18454 RVA: 0x006CB2C0 File Offset: 0x006C94C0
			public override Rectangle GetViewCullingArea()
			{
				return base.Parent.GetDimensions().ToRectangle();
			}
		}
	}
}
