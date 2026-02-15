using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x02000404 RID: 1028
	public class UIScrollbar : UIElement
	{
		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06002F18 RID: 12056 RVA: 0x005B09DD File Offset: 0x005AEBDD
		// (set) Token: 0x06002F19 RID: 12057 RVA: 0x005B09E5 File Offset: 0x005AEBE5
		public float ViewPosition
		{
			get
			{
				return this._viewPosition;
			}
			set
			{
				this._viewPosition = MathHelper.Clamp(value, 0f, this._maxViewSize - this._viewSize);
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06002F1A RID: 12058 RVA: 0x005B0A05 File Offset: 0x005AEC05
		public bool CanScroll
		{
			get
			{
				return this._maxViewSize != this._viewSize;
			}
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x005B0A18 File Offset: 0x005AEC18
		public void GoToBottom()
		{
			this.ViewPosition = this._maxViewSize - this._viewSize;
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x005B0A30 File Offset: 0x005AEC30
		public UIScrollbar(UIScrollbar.ColorTheme theme = UIScrollbar.ColorTheme.Blue)
		{
			this._theme = theme;
			this.Width.Set(20f, 0f);
			this.MaxWidth.Set(20f, 0f);
			string text = "Images/UI/Scrollbar";
			if (this._theme == UIScrollbar.ColorTheme.Cyan)
			{
				text = "Images/UI/Scrollbar2";
			}
			this._texture = Main.Assets.Request<Texture2D>(text, 1);
			this._innerTexture = Main.Assets.Request<Texture2D>("Images/UI/ScrollbarInner", 1);
			this.PaddingTop = 5f;
			this.PaddingBottom = 5f;
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x005B0ADD File Offset: 0x005AECDD
		public void SetView(float viewSize, float maxViewSize)
		{
			viewSize = MathHelper.Clamp(viewSize, 0f, maxViewSize);
			this._viewPosition = MathHelper.Clamp(this._viewPosition, 0f, maxViewSize - viewSize);
			this._viewSize = viewSize;
			this._maxViewSize = maxViewSize;
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x005B09DD File Offset: 0x005AEBDD
		public float GetValue()
		{
			return this._viewPosition;
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x005B0B14 File Offset: 0x005AED14
		private Rectangle GetHandleRectangle()
		{
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			if (this._maxViewSize == 0f && this._viewSize == 0f)
			{
				this._viewSize = 1f;
				this._maxViewSize = 1f;
			}
			return new Rectangle((int)innerDimensions.X, (int)(innerDimensions.Y + innerDimensions.Height * (this._viewPosition / this._maxViewSize)) - 3, 20, (int)(innerDimensions.Height * (this._viewSize / this._maxViewSize)) + 7);
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x005B0B9C File Offset: 0x005AED9C
		private void DrawBar(SpriteBatch spriteBatch, Texture2D texture, Rectangle dimensions, Color color)
		{
			spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y - 6, dimensions.Width, 6), new Rectangle?(new Rectangle(0, 0, texture.Width, 6)), color);
			spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height), new Rectangle?(new Rectangle(0, 6, texture.Width, 4)), color);
			spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y + dimensions.Height, dimensions.Width, 6), new Rectangle?(new Rectangle(0, texture.Height - 6, texture.Width, 6)), color);
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x005B0C5C File Offset: 0x005AEE5C
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (this.AutoHide && !this.CanScroll)
			{
				return;
			}
			CalculatedStyle dimensions = base.GetDimensions();
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			if (this._isDragging)
			{
				float num = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - this._dragYOffset;
				this._viewPosition = MathHelper.Clamp(num / innerDimensions.Height * this._maxViewSize, 0f, this._maxViewSize - this._viewSize);
			}
			Rectangle handleRectangle = this.GetHandleRectangle();
			Vector2 mousePosition = UserInterface.ActiveInstance.MousePosition;
			bool isHoveringOverHandle = this._isHoveringOverHandle;
			this._isHoveringOverHandle = handleRectangle.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y));
			if (!isHoveringOverHandle && this._isHoveringOverHandle && FocusHelper.AllowUIInputs)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
			this.DrawBar(spriteBatch, this._texture.Value, dimensions.ToRectangle(), Color.White);
			this.DrawBar(spriteBatch, this._innerTexture.Value, handleRectangle, Color.White * ((this._isDragging || this._isHoveringOverHandle) ? 1f : 0.85f));
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x005B0D94 File Offset: 0x005AEF94
		public override void LeftMouseDown(UIMouseEvent evt)
		{
			base.LeftMouseDown(evt);
			if (evt.Target == this)
			{
				Rectangle handleRectangle = this.GetHandleRectangle();
				if (handleRectangle.Contains(new Point((int)evt.MousePosition.X, (int)evt.MousePosition.Y)))
				{
					this._isDragging = true;
					this._dragYOffset = evt.MousePosition.Y - (float)handleRectangle.Y;
					return;
				}
				CalculatedStyle innerDimensions = base.GetInnerDimensions();
				float num = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - (float)(handleRectangle.Height >> 1);
				this._viewPosition = MathHelper.Clamp(num / innerDimensions.Height * this._maxViewSize, 0f, this._maxViewSize - this._viewSize);
			}
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x005B0E56 File Offset: 0x005AF056
		public override void LeftMouseUp(UIMouseEvent evt)
		{
			base.LeftMouseUp(evt);
			this._isDragging = false;
		}

		// Token: 0x0400561B RID: 22043
		private float _viewPosition;

		// Token: 0x0400561C RID: 22044
		private float _viewSize = 1f;

		// Token: 0x0400561D RID: 22045
		private float _maxViewSize = 20f;

		// Token: 0x0400561E RID: 22046
		private bool _isDragging;

		// Token: 0x0400561F RID: 22047
		private bool _isHoveringOverHandle;

		// Token: 0x04005620 RID: 22048
		private float _dragYOffset;

		// Token: 0x04005621 RID: 22049
		public bool AutoHide;

		// Token: 0x04005622 RID: 22050
		private Asset<Texture2D> _texture;

		// Token: 0x04005623 RID: 22051
		private Asset<Texture2D> _innerTexture;

		// Token: 0x04005624 RID: 22052
		private UIScrollbar.ColorTheme _theme;

		// Token: 0x02000935 RID: 2357
		public enum ColorTheme
		{
			// Token: 0x040074E8 RID: 29928
			Blue,
			// Token: 0x040074E9 RID: 29929
			Cyan
		}
	}
}
