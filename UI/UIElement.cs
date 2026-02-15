using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent.UI.Elements;

namespace Terraria.UI
{
	// Token: 0x020000FB RID: 251
	public class UIElement : IComparable
	{
		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060019C1 RID: 6593 RVA: 0x004F2A2A File Offset: 0x004F0C2A
		// (set) Token: 0x060019C2 RID: 6594 RVA: 0x004F2A32 File Offset: 0x004F0C32
		public UIElement Parent { get; private set; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060019C3 RID: 6595 RVA: 0x004F2A3B File Offset: 0x004F0C3B
		// (set) Token: 0x060019C4 RID: 6596 RVA: 0x004F2A43 File Offset: 0x004F0C43
		public int UniqueId { get; private set; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060019C5 RID: 6597 RVA: 0x004F2A4C File Offset: 0x004F0C4C
		public IEnumerable<UIElement> Children
		{
			get
			{
				return this.Elements;
			}
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060019C6 RID: 6598 RVA: 0x004F2A54 File Offset: 0x004F0C54
		// (remove) Token: 0x060019C7 RID: 6599 RVA: 0x004F2A8C File Offset: 0x004F0C8C
		public event UIElement.MouseEvent OnLeftMouseDown;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060019C8 RID: 6600 RVA: 0x004F2AC4 File Offset: 0x004F0CC4
		// (remove) Token: 0x060019C9 RID: 6601 RVA: 0x004F2AFC File Offset: 0x004F0CFC
		public event UIElement.MouseEvent OnLeftMouseUp;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060019CA RID: 6602 RVA: 0x004F2B34 File Offset: 0x004F0D34
		// (remove) Token: 0x060019CB RID: 6603 RVA: 0x004F2B6C File Offset: 0x004F0D6C
		public event UIElement.MouseEvent OnLeftClick;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060019CC RID: 6604 RVA: 0x004F2BA4 File Offset: 0x004F0DA4
		// (remove) Token: 0x060019CD RID: 6605 RVA: 0x004F2BDC File Offset: 0x004F0DDC
		public event UIElement.MouseEvent OnLeftDoubleClick;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060019CE RID: 6606 RVA: 0x004F2C14 File Offset: 0x004F0E14
		// (remove) Token: 0x060019CF RID: 6607 RVA: 0x004F2C4C File Offset: 0x004F0E4C
		public event UIElement.MouseEvent OnRightMouseDown;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x060019D0 RID: 6608 RVA: 0x004F2C84 File Offset: 0x004F0E84
		// (remove) Token: 0x060019D1 RID: 6609 RVA: 0x004F2CBC File Offset: 0x004F0EBC
		public event UIElement.MouseEvent OnRightMouseUp;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x060019D2 RID: 6610 RVA: 0x004F2CF4 File Offset: 0x004F0EF4
		// (remove) Token: 0x060019D3 RID: 6611 RVA: 0x004F2D2C File Offset: 0x004F0F2C
		public event UIElement.MouseEvent OnRightClick;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060019D4 RID: 6612 RVA: 0x004F2D64 File Offset: 0x004F0F64
		// (remove) Token: 0x060019D5 RID: 6613 RVA: 0x004F2D9C File Offset: 0x004F0F9C
		public event UIElement.MouseEvent OnRightDoubleClick;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060019D6 RID: 6614 RVA: 0x004F2DD4 File Offset: 0x004F0FD4
		// (remove) Token: 0x060019D7 RID: 6615 RVA: 0x004F2E0C File Offset: 0x004F100C
		public event UIElement.MouseEvent OnMouseOver;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x060019D8 RID: 6616 RVA: 0x004F2E44 File Offset: 0x004F1044
		// (remove) Token: 0x060019D9 RID: 6617 RVA: 0x004F2E7C File Offset: 0x004F107C
		public event UIElement.MouseEvent OnMouseOut;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x060019DA RID: 6618 RVA: 0x004F2EB4 File Offset: 0x004F10B4
		// (remove) Token: 0x060019DB RID: 6619 RVA: 0x004F2EEC File Offset: 0x004F10EC
		public event UIElement.ScrollWheelEvent OnScrollWheel;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x060019DC RID: 6620 RVA: 0x004F2F24 File Offset: 0x004F1124
		// (remove) Token: 0x060019DD RID: 6621 RVA: 0x004F2F5C File Offset: 0x004F115C
		public event UIElement.ElementEvent OnUpdate;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x060019DE RID: 6622 RVA: 0x004F2F94 File Offset: 0x004F1194
		// (remove) Token: 0x060019DF RID: 6623 RVA: 0x004F2FCC File Offset: 0x004F11CC
		public event UIElement.DrawEvent OnDraw;

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060019E0 RID: 6624 RVA: 0x004F3001 File Offset: 0x004F1201
		// (set) Token: 0x060019E1 RID: 6625 RVA: 0x004F3009 File Offset: 0x004F1209
		public bool IsMouseHovering { get; private set; }

		// Token: 0x060019E2 RID: 6626 RVA: 0x004F3014 File Offset: 0x004F1214
		public UIElement()
		{
			this.UniqueId = UIElement._idCounter++;
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x004F3074 File Offset: 0x004F1274
		public void SetSnapPoint(string name, int id, Vector2? anchor = null, Vector2? offset = null)
		{
			if (anchor == null)
			{
				anchor = new Vector2?(new Vector2(0.5f));
			}
			if (offset == null)
			{
				offset = new Vector2?(Vector2.Zero);
			}
			this._snapPoint = new SnapPoint(name, id, anchor.Value, offset.Value);
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x004F30CB File Offset: 0x004F12CB
		public bool GetSnapPoint(out SnapPoint point)
		{
			point = this._snapPoint;
			if (this._snapPoint != null)
			{
				this._snapPoint.Calculate(this);
			}
			return this._snapPoint != null;
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x004F30F4 File Offset: 0x004F12F4
		public virtual void ExecuteRecursively(UIElement.UIElementAction action)
		{
			action(this);
			foreach (UIElement uielement in this.Elements)
			{
				uielement.ExecuteRecursively(action);
			}
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void DrawSelf(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x004F314C File Offset: 0x004F134C
		protected virtual void DrawChildren(SpriteBatch spriteBatch)
		{
			foreach (UIElement uielement in this.Elements)
			{
				uielement.Draw(spriteBatch);
			}
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x004F31A0 File Offset: 0x004F13A0
		public void Append(UIElement element)
		{
			element.Remove();
			element.Parent = this;
			this.Elements.Add(element);
			element.Recalculate();
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x004F31C1 File Offset: 0x004F13C1
		public void Remove()
		{
			if (this.Parent != null)
			{
				this.Parent.RemoveChild(this);
			}
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x004F31D7 File Offset: 0x004F13D7
		public void RemoveChild(UIElement child)
		{
			this.Elements.Remove(child);
			child.Parent = null;
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x004F31F0 File Offset: 0x004F13F0
		public void RemoveAllChildren()
		{
			foreach (UIElement uielement in this.Elements)
			{
				uielement.Parent = null;
			}
			this.Elements.Clear();
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x004F324C File Offset: 0x004F144C
		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (this.OnDraw != null)
			{
				this.OnDraw(this, spriteBatch);
			}
			bool overflowHidden = this.OverflowHidden;
			bool useImmediateMode = this.UseImmediateMode;
			RasterizerState rasterizerState = spriteBatch.GraphicsDevice.RasterizerState;
			Rectangle scissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
			SamplerState anisotropicClamp = SamplerState.AnisotropicClamp;
			if (useImmediateMode || this.OverrideSamplerState != null)
			{
				spriteBatch.End();
				spriteBatch.Begin(useImmediateMode ? SpriteSortMode.Immediate : SpriteSortMode.Deferred, BlendState.AlphaBlend, (this.OverrideSamplerState != null) ? this.OverrideSamplerState : anisotropicClamp, DepthStencilState.None, UIElement.OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);
				this.DrawSelf(spriteBatch);
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, UIElement.OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);
			}
			else
			{
				this.DrawSelf(spriteBatch);
			}
			if (overflowHidden)
			{
				spriteBatch.End();
				Rectangle clippingRectangle = this.GetClippingRectangle(spriteBatch);
				spriteBatch.GraphicsDevice.ScissorRectangle = clippingRectangle;
				spriteBatch.GraphicsDevice.RasterizerState = UIElement.OverflowHiddenRasterizerState;
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, UIElement.OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);
			}
			this.DrawChildren(spriteBatch);
			if (overflowHidden)
			{
				spriteBatch.End();
				spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
				spriteBatch.GraphicsDevice.RasterizerState = rasterizerState;
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, rasterizerState, null, Main.UIScaleMatrix);
			}
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x004F339C File Offset: 0x004F159C
		public virtual void Update(GameTime gameTime)
		{
			if (this.OnUpdate != null)
			{
				this.OnUpdate(this);
			}
			foreach (UIElement uielement in this.Elements)
			{
				uielement.Update(gameTime);
			}
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x004F3404 File Offset: 0x004F1604
		public Rectangle GetClippingRectangle(SpriteBatch spriteBatch)
		{
			Vector2 vector = new Vector2(this._innerDimensions.X, this._innerDimensions.Y);
			Vector2 vector2 = new Vector2(this._innerDimensions.Width, this._innerDimensions.Height) + vector;
			vector = Vector2.Transform(vector, Main.UIScaleMatrix);
			vector2 = Vector2.Transform(vector2, Main.UIScaleMatrix);
			Rectangle rectangle = new Rectangle((int)vector.X, (int)vector.Y, (int)(vector2.X - vector.X), (int)(vector2.Y - vector.Y));
			int num = (int)((float)Main.screenWidth * Main.UIScale);
			int num2 = (int)((float)Main.screenHeight * Main.UIScale);
			rectangle.X = Utils.Clamp<int>(rectangle.X, 0, num);
			rectangle.Y = Utils.Clamp<int>(rectangle.Y, 0, num2);
			rectangle.Width = Utils.Clamp<int>(rectangle.Width, 0, num - rectangle.X);
			rectangle.Height = Utils.Clamp<int>(rectangle.Height, 0, num2 - rectangle.Y);
			Rectangle scissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
			int num3 = Utils.Clamp<int>(rectangle.Left, scissorRectangle.Left, scissorRectangle.Right);
			int num4 = Utils.Clamp<int>(rectangle.Top, scissorRectangle.Top, scissorRectangle.Bottom);
			int num5 = Utils.Clamp<int>(rectangle.Right, scissorRectangle.Left, scissorRectangle.Right);
			int num6 = Utils.Clamp<int>(rectangle.Bottom, scissorRectangle.Top, scissorRectangle.Bottom);
			return new Rectangle(num3, num4, num5 - num3, num6 - num4);
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x004F35A8 File Offset: 0x004F17A8
		public virtual List<SnapPoint> GetSnapPoints()
		{
			List<SnapPoint> list = new List<SnapPoint>();
			SnapPoint item;
			if (this.GetSnapPoint(out item))
			{
				list.Add(item);
			}
			foreach (UIElement uielement in this.Elements)
			{
				list.AddRange(uielement.GetSnapPoints());
			}
			return list;
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x004F3618 File Offset: 0x004F1818
		public virtual void Recalculate()
		{
			CalculatedStyle parentDimensions;
			if (this.Parent != null)
			{
				parentDimensions = this.Parent.GetInnerDimensions();
			}
			else
			{
				parentDimensions = UserInterface.ActiveInstance.GetDimensions();
			}
			if (this.Parent != null && this.Parent is UIList)
			{
				parentDimensions.Height = float.MaxValue;
			}
			CalculatedStyle dimensionsBasedOnParentDimensions = this.GetDimensionsBasedOnParentDimensions(parentDimensions);
			this._outerDimensions = dimensionsBasedOnParentDimensions;
			dimensionsBasedOnParentDimensions.X += this.MarginLeft;
			dimensionsBasedOnParentDimensions.Y += this.MarginTop;
			dimensionsBasedOnParentDimensions.Width -= this.MarginLeft + this.MarginRight;
			dimensionsBasedOnParentDimensions.Height -= this.MarginTop + this.MarginBottom;
			this._dimensions = dimensionsBasedOnParentDimensions;
			dimensionsBasedOnParentDimensions.X += this.PaddingLeft;
			dimensionsBasedOnParentDimensions.Y += this.PaddingTop;
			dimensionsBasedOnParentDimensions.Width -= this.PaddingLeft + this.PaddingRight;
			dimensionsBasedOnParentDimensions.Height -= this.PaddingTop + this.PaddingBottom;
			this._innerDimensions = dimensionsBasedOnParentDimensions;
			this.RecalculateChildren();
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x004F3730 File Offset: 0x004F1930
		private CalculatedStyle GetDimensionsBasedOnParentDimensions(CalculatedStyle parentDimensions)
		{
			CalculatedStyle calculatedStyle;
			calculatedStyle.X = this.Left.GetValue(parentDimensions.Width) + parentDimensions.X;
			calculatedStyle.Y = this.Top.GetValue(parentDimensions.Height) + parentDimensions.Y;
			float value = this.MinWidth.GetValue(parentDimensions.Width);
			float value2 = this.MaxWidth.GetValue(parentDimensions.Width);
			float value3 = this.MinHeight.GetValue(parentDimensions.Height);
			float value4 = this.MaxHeight.GetValue(parentDimensions.Height);
			calculatedStyle.Width = MathHelper.Clamp(this.Width.GetValue(parentDimensions.Width), value, value2);
			calculatedStyle.Height = MathHelper.Clamp(this.Height.GetValue(parentDimensions.Height), value3, value4);
			calculatedStyle.Width += this.MarginLeft + this.MarginRight;
			calculatedStyle.Height += this.MarginTop + this.MarginBottom;
			calculatedStyle.X += parentDimensions.Width * this.HAlign - calculatedStyle.Width * this.HAlign;
			calculatedStyle.Y += parentDimensions.Height * this.VAlign - calculatedStyle.Height * this.VAlign;
			return calculatedStyle;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x004F3880 File Offset: 0x004F1A80
		public UIElement GetElementAt(Vector2 point)
		{
			UIElement uielement = null;
			for (int i = this.Elements.Count - 1; i >= 0; i--)
			{
				UIElement uielement2 = this.Elements[i];
				if (!uielement2.IgnoresMouseInteraction && uielement2.ContainsPoint(point))
				{
					uielement = uielement2;
					if (!uielement2.PassThroughMouseInteraction)
					{
						break;
					}
				}
			}
			if (uielement != null)
			{
				return uielement.GetElementAt(point);
			}
			if (this.IgnoresMouseInteraction)
			{
				return null;
			}
			if (this.ContainsPoint(point))
			{
				return this;
			}
			return null;
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x004F38F0 File Offset: 0x004F1AF0
		public virtual bool ContainsPoint(Vector2 point)
		{
			return point.X > this._dimensions.X && point.Y > this._dimensions.Y && point.X < this._dimensions.X + this._dimensions.Width && point.Y < this._dimensions.Y + this._dimensions.Height;
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x004F3963 File Offset: 0x004F1B63
		public virtual Rectangle GetViewCullingArea()
		{
			return this._dimensions.ToRectangle();
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x004F3970 File Offset: 0x004F1B70
		public void SetPadding(float pixels)
		{
			this.PaddingBottom = pixels;
			this.PaddingLeft = pixels;
			this.PaddingRight = pixels;
			this.PaddingTop = pixels;
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x004F3990 File Offset: 0x004F1B90
		public virtual void RecalculateChildren()
		{
			foreach (UIElement uielement in this.Elements)
			{
				uielement.Recalculate();
			}
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x004F39E0 File Offset: 0x004F1BE0
		public CalculatedStyle GetInnerDimensions()
		{
			return this._innerDimensions;
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x004F39E8 File Offset: 0x004F1BE8
		public CalculatedStyle GetDimensions()
		{
			return this._dimensions;
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x004F39F0 File Offset: 0x004F1BF0
		public CalculatedStyle GetOuterDimensions()
		{
			return this._outerDimensions;
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x004F39F8 File Offset: 0x004F1BF8
		public void CopyStyle(UIElement element)
		{
			this.Top = element.Top;
			this.Left = element.Left;
			this.Width = element.Width;
			this.Height = element.Height;
			this.PaddingBottom = element.PaddingBottom;
			this.PaddingLeft = element.PaddingLeft;
			this.PaddingRight = element.PaddingRight;
			this.PaddingTop = element.PaddingTop;
			this.HAlign = element.HAlign;
			this.VAlign = element.VAlign;
			this.MinWidth = element.MinWidth;
			this.MaxWidth = element.MaxWidth;
			this.MinHeight = element.MinHeight;
			this.MaxHeight = element.MaxHeight;
			this.Recalculate();
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x004F3AB3 File Offset: 0x004F1CB3
		public virtual void LeftMouseDown(UIMouseEvent evt)
		{
			if (this.OnLeftMouseDown != null)
			{
				this.OnLeftMouseDown(evt, this);
			}
			if (this.Parent != null)
			{
				this.Parent.LeftMouseDown(evt);
			}
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x004F3ADE File Offset: 0x004F1CDE
		public virtual void LeftMouseUp(UIMouseEvent evt)
		{
			if (this.OnLeftMouseUp != null)
			{
				this.OnLeftMouseUp(evt, this);
			}
			if (this.Parent != null)
			{
				this.Parent.LeftMouseUp(evt);
			}
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x004F3B09 File Offset: 0x004F1D09
		public virtual void LeftClick(UIMouseEvent evt)
		{
			if (this.OnLeftClick != null)
			{
				this.OnLeftClick(evt, this);
			}
			if (this.Parent != null)
			{
				this.Parent.LeftClick(evt);
			}
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x004F3B34 File Offset: 0x004F1D34
		public virtual void LeftDoubleClick(UIMouseEvent evt)
		{
			if (this.OnLeftDoubleClick != null)
			{
				this.OnLeftDoubleClick(evt, this);
			}
			if (this.Parent != null)
			{
				this.Parent.LeftDoubleClick(evt);
			}
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x004F3B5F File Offset: 0x004F1D5F
		public virtual void RightMouseDown(UIMouseEvent evt)
		{
			if (this.OnRightMouseDown != null)
			{
				this.OnRightMouseDown(evt, this);
			}
			if (this.Parent != null)
			{
				this.Parent.RightMouseDown(evt);
			}
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x004F3B8A File Offset: 0x004F1D8A
		public virtual void RightMouseUp(UIMouseEvent evt)
		{
			if (this.OnRightMouseUp != null)
			{
				this.OnRightMouseUp(evt, this);
			}
			if (this.Parent != null)
			{
				this.Parent.RightMouseUp(evt);
			}
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x004F3BB5 File Offset: 0x004F1DB5
		public virtual void RightClick(UIMouseEvent evt)
		{
			if (this.OnRightClick != null)
			{
				this.OnRightClick(evt, this);
			}
			if (this.Parent != null)
			{
				this.Parent.RightClick(evt);
			}
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x004F3BE0 File Offset: 0x004F1DE0
		public virtual void RightDoubleClick(UIMouseEvent evt)
		{
			if (this.OnRightDoubleClick != null)
			{
				this.OnRightDoubleClick(evt, this);
			}
			if (this.Parent != null)
			{
				this.Parent.RightDoubleClick(evt);
			}
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x004F3C0B File Offset: 0x004F1E0B
		public virtual void MouseOver(UIMouseEvent evt)
		{
			this.IsMouseHovering = true;
			if (this.OnMouseOver != null)
			{
				this.OnMouseOver(evt, this);
			}
			if (this.Parent != null)
			{
				this.Parent.MouseOver(evt);
			}
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x004F3C3D File Offset: 0x004F1E3D
		public virtual void MouseOut(UIMouseEvent evt)
		{
			this.IsMouseHovering = false;
			if (this.OnMouseOut != null)
			{
				this.OnMouseOut(evt, this);
			}
			if (this.Parent != null)
			{
				this.Parent.MouseOut(evt);
			}
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x004F3C6F File Offset: 0x004F1E6F
		public virtual void ScrollWheel(UIScrollWheelEvent evt)
		{
			if (this.OnScrollWheel != null)
			{
				this.OnScrollWheel(evt, this);
			}
			if (this.Parent != null)
			{
				this.Parent.ScrollWheel(evt);
			}
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x004F3C9C File Offset: 0x004F1E9C
		public void Activate()
		{
			if (!this._isInitialized)
			{
				this.Initialize();
			}
			this.OnActivate();
			foreach (UIElement uielement in this.Elements)
			{
				uielement.Activate();
			}
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnActivate()
		{
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x004F3D00 File Offset: 0x004F1F00
		[Conditional("DEBUG")]
		public void DrawDebugHitbox(BasicDebugDrawer drawer, float colorIntensity = 0f)
		{
			if (this.IsMouseHovering)
			{
				colorIntensity += 0.1f;
			}
			Color color = Main.hslToRgb(colorIntensity, colorIntensity, 0.5f, byte.MaxValue);
			CalculatedStyle innerDimensions = this.GetInnerDimensions();
			drawer.DrawLine(innerDimensions.Position(), innerDimensions.Position() + new Vector2(innerDimensions.Width, 0f), 2f, color);
			drawer.DrawLine(innerDimensions.Position() + new Vector2(innerDimensions.Width, 0f), innerDimensions.Position() + new Vector2(innerDimensions.Width, innerDimensions.Height), 2f, color);
			drawer.DrawLine(innerDimensions.Position() + new Vector2(innerDimensions.Width, innerDimensions.Height), innerDimensions.Position() + new Vector2(0f, innerDimensions.Height), 2f, color);
			drawer.DrawLine(innerDimensions.Position() + new Vector2(0f, innerDimensions.Height), innerDimensions.Position(), 2f, color);
			foreach (UIElement uielement in this.Elements)
			{
			}
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x004F3E60 File Offset: 0x004F2060
		public void Deactivate()
		{
			this.OnDeactivate();
			foreach (UIElement uielement in this.Elements)
			{
				uielement.Deactivate();
			}
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnDeactivate()
		{
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x004F3EB8 File Offset: 0x004F20B8
		public void Initialize()
		{
			this.OnInitialize();
			this._isInitialized = true;
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnInitialize()
		{
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public virtual int CompareTo(object obj)
		{
			return 0;
		}

		// Token: 0x0400134F RID: 4943
		protected readonly List<UIElement> Elements = new List<UIElement>();

		// Token: 0x04001351 RID: 4945
		public StyleDimension Top;

		// Token: 0x04001352 RID: 4946
		public StyleDimension Left;

		// Token: 0x04001353 RID: 4947
		public StyleDimension Width;

		// Token: 0x04001354 RID: 4948
		public StyleDimension Height;

		// Token: 0x04001355 RID: 4949
		public StyleDimension MaxWidth = StyleDimension.Fill;

		// Token: 0x04001356 RID: 4950
		public StyleDimension MaxHeight = StyleDimension.Fill;

		// Token: 0x04001357 RID: 4951
		public StyleDimension MinWidth = StyleDimension.Empty;

		// Token: 0x04001358 RID: 4952
		public StyleDimension MinHeight = StyleDimension.Empty;

		// Token: 0x04001366 RID: 4966
		private bool _isInitialized;

		// Token: 0x04001367 RID: 4967
		public bool IgnoresMouseInteraction;

		// Token: 0x04001368 RID: 4968
		public bool PassThroughMouseInteraction;

		// Token: 0x04001369 RID: 4969
		public bool OverflowHidden;

		// Token: 0x0400136A RID: 4970
		public SamplerState OverrideSamplerState;

		// Token: 0x0400136B RID: 4971
		public float PaddingTop;

		// Token: 0x0400136C RID: 4972
		public float PaddingLeft;

		// Token: 0x0400136D RID: 4973
		public float PaddingRight;

		// Token: 0x0400136E RID: 4974
		public float PaddingBottom;

		// Token: 0x0400136F RID: 4975
		public float MarginTop;

		// Token: 0x04001370 RID: 4976
		public float MarginLeft;

		// Token: 0x04001371 RID: 4977
		public float MarginRight;

		// Token: 0x04001372 RID: 4978
		public float MarginBottom;

		// Token: 0x04001373 RID: 4979
		public float HAlign;

		// Token: 0x04001374 RID: 4980
		public float VAlign;

		// Token: 0x04001375 RID: 4981
		private CalculatedStyle _innerDimensions;

		// Token: 0x04001376 RID: 4982
		private CalculatedStyle _dimensions;

		// Token: 0x04001377 RID: 4983
		private CalculatedStyle _outerDimensions;

		// Token: 0x04001378 RID: 4984
		private static readonly RasterizerState OverflowHiddenRasterizerState = new RasterizerState
		{
			CullMode = CullMode.None,
			ScissorTestEnable = true
		};

		// Token: 0x04001379 RID: 4985
		public bool UseImmediateMode;

		// Token: 0x0400137A RID: 4986
		private SnapPoint _snapPoint;

		// Token: 0x0400137C RID: 4988
		private static int _idCounter = 0;

		// Token: 0x02000711 RID: 1809
		// (Invoke) Token: 0x0600400F RID: 16399
		public delegate void MouseEvent(UIMouseEvent evt, UIElement listeningElement);

		// Token: 0x02000712 RID: 1810
		// (Invoke) Token: 0x06004013 RID: 16403
		public delegate void ScrollWheelEvent(UIScrollWheelEvent evt, UIElement listeningElement);

		// Token: 0x02000713 RID: 1811
		// (Invoke) Token: 0x06004017 RID: 16407
		public delegate void ElementEvent(UIElement affectedElement);

		// Token: 0x02000714 RID: 1812
		// (Invoke) Token: 0x0600401B RID: 16411
		public delegate void DrawEvent(UIElement affectedElement, SpriteBatch sb);

		// Token: 0x02000715 RID: 1813
		// (Invoke) Token: 0x0600401F RID: 16415
		public delegate void UIElementAction(UIElement element);
	}
}
