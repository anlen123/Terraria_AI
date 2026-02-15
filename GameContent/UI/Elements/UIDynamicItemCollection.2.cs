using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003C6 RID: 966
	public abstract class UIDynamicItemCollection<TEntry> : UIDynamicItemCollection
	{
		// Token: 0x06002D30 RID: 11568 RVA: 0x005A1EA4 File Offset: 0x005A00A4
		public UIDynamicItemCollection()
		{
			this.Width = new StyleDimension(0f, 1f);
			this.HAlign = 0.5f;
			this.UpdateSize();
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06002D31 RID: 11569 RVA: 0x005A1EF3 File Offset: 0x005A00F3
		public int Count
		{
			get
			{
				return this._contents.Count;
			}
		}

		// Token: 0x06002D32 RID: 11570
		protected abstract Item GetItem(TEntry entry);

		// Token: 0x06002D33 RID: 11571 RVA: 0x005A1F00 File Offset: 0x005A0100
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Main.inventoryScale = 0.84615386f;
			int startX;
			int startY;
			int num;
			int num2;
			this.GetGridParameters(out startX, out startY, out num, out num2);
			int num3 = this._itemsPerLine;
			Vector2 v = Main.MouseScreen;
			if (PlayerInput.UsingGamepad)
			{
				v = UILinkPointNavigator.GetPosition(UILinkPointNavigator.CurrentPoint);
			}
			for (int i = num; i < num2; i++)
			{
				TEntry entry = this._contents[i];
				Rectangle itemSlotHitbox = this.GetItemSlotHitbox(startX, startY, num, i);
				if (TextureAssets.Item[this.GetItem(entry).type].State == null)
				{
					num3--;
				}
				bool hovering = base.IsMouseHovering && itemSlotHitbox.Contains(v.ToPoint()) && !PlayerInput.IgnoreMouseInterface;
				this.DrawSlot(spriteBatch, entry, itemSlotHitbox.TopLeft(), hovering);
				if (num3 <= 0)
				{
					break;
				}
			}
			int num4 = 0;
			while (num4 < this._contents.Count && num3 > 0)
			{
				Item item = this.GetItem(this._contents[(num4 + num2) % this._contents.Count]);
				if (TextureAssets.Item[item.type].State == null)
				{
					Main.instance.LoadItem(item.type);
					num3 -= 4;
				}
				num4++;
			}
		}

		// Token: 0x06002D34 RID: 11572
		protected abstract void DrawSlot(SpriteBatch spriteBatch, TEntry entry, Vector2 pos, bool hovering);

		// Token: 0x06002D35 RID: 11573 RVA: 0x005A2040 File Offset: 0x005A0240
		private Rectangle GetItemSlotHitbox(int startX, int startY, int startItemIndex, int i)
		{
			int num = i - startItemIndex;
			int num2 = num % this._itemsPerLine;
			int num3 = num / this._itemsPerLine;
			return new Rectangle(startX + num2 * 44, startY + num3 * 44, 44, 44);
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x005A2078 File Offset: 0x005A0278
		private void GetGridParameters(out int startX, out int startY, out int startItemIndex, out int endItemIndex)
		{
			Rectangle rectangle = base.GetDimensions().ToRectangle();
			Rectangle viewCullingArea = base.Parent.GetViewCullingArea();
			int x = rectangle.Center.X;
			startX = x - (int)((float)(44 * this._itemsPerLine) * 0.5f);
			startY = rectangle.Top;
			startItemIndex = 0;
			endItemIndex = this._contents.Count;
			int num = (Math.Min(viewCullingArea.Top, rectangle.Top) - viewCullingArea.Top) / 44;
			startY += -num * 44;
			startItemIndex += -num * this._itemsPerLine;
			int num2 = (int)Math.Ceiling((double)((float)viewCullingArea.Height / 44f)) * this._itemsPerLine;
			if (endItemIndex > num2 + startItemIndex + this._itemsPerLine)
			{
				endItemIndex = num2 + startItemIndex + this._itemsPerLine;
			}
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x005A2151 File Offset: 0x005A0351
		public override void Recalculate()
		{
			base.Recalculate();
			this.UpdateSize();
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x005A215F File Offset: 0x005A035F
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (base.IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x005A217B File Offset: 0x005A037B
		public void SetContentsToShow(List<TEntry> itemsToShow)
		{
			this._contents.Clear();
			this._contents.AddRange(itemsToShow);
			this.UpdateSize();
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x005A219A File Offset: 0x005A039A
		public int GetItemsPerLine()
		{
			return this._itemsPerLine;
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x005A21A4 File Offset: 0x005A03A4
		public override List<SnapPoint> GetSnapPoints()
		{
			List<SnapPoint> list = new List<SnapPoint>();
			int startX;
			int startY;
			int num;
			int num2;
			this.GetGridParameters(out startX, out startY, out num, out num2);
			int itemsPerLine = this._itemsPerLine;
			Rectangle viewCullingArea = base.Parent.GetViewCullingArea();
			int num3 = num2 - num;
			while (this._dummySnapPoints.Count < num3)
			{
				this._dummySnapPoints.Add(new SnapPoint("DynamicItemCollectionSlot", 0, Vector2.Zero, Vector2.Zero));
			}
			int num4 = 0;
			Vector2 value = base.GetDimensions().Position();
			for (int i = num; i < num2; i++)
			{
				Point center = this.GetItemSlotHitbox(startX, startY, num, i).Center;
				if (viewCullingArea.Contains(center))
				{
					SnapPoint snapPoint = this._dummySnapPoints[num4];
					snapPoint.ThisIsAHackThatChangesTheSnapPointsInfo(Vector2.Zero, center.ToVector2() - value, i);
					snapPoint.Calculate(this);
					num4++;
					list.Add(snapPoint);
				}
			}
			foreach (UIElement uielement in this.Elements)
			{
				list.AddRange(uielement.GetSnapPoints());
			}
			return list;
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x005A22E8 File Offset: 0x005A04E8
		public void UpdateSize()
		{
			int num = base.GetDimensions().ToRectangle().Width / 44;
			this._itemsPerLine = num;
			int num2 = (int)Math.Ceiling((double)((float)this._contents.Count / (float)num));
			this.MinHeight.Set((float)(44 * num2), 0f);
		}

		// Token: 0x04005486 RID: 21638
		private List<TEntry> _contents = new List<TEntry>();

		// Token: 0x04005487 RID: 21639
		private int _itemsPerLine;

		// Token: 0x04005488 RID: 21640
		private const int sizePerEntryX = 44;

		// Token: 0x04005489 RID: 21641
		private const int sizePerEntryY = 44;

		// Token: 0x0400548A RID: 21642
		private List<SnapPoint> _dummySnapPoints = new List<SnapPoint>();
	}
}
