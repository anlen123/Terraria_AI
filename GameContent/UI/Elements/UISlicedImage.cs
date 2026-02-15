using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003ED RID: 1005
	public class UISlicedImage : UIElement
	{
		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06002E75 RID: 11893 RVA: 0x005AA9CE File Offset: 0x005A8BCE
		// (set) Token: 0x06002E76 RID: 11894 RVA: 0x005AA9D6 File Offset: 0x005A8BD6
		public Color Color
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
			}
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x005AA9E0 File Offset: 0x005A8BE0
		public UISlicedImage(Asset<Texture2D> texture)
		{
			this._texture = texture;
			this.Width.Set((float)this._texture.Width(), 0f);
			this.Height.Set((float)this._texture.Height(), 0f);
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x005AAA32 File Offset: 0x005A8C32
		public void SetImage(Asset<Texture2D> texture)
		{
			this._texture = texture;
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x005AAA3C File Offset: 0x005A8C3C
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			Utils.DrawSplicedPanel(spriteBatch, this._texture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, this._leftSliceDepth, this._rightSliceDepth, this._topSliceDepth, this._bottomSliceDepth, this._color);
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x005AAA9B File Offset: 0x005A8C9B
		public void SetSliceDepths(int top, int bottom, int left, int right)
		{
			this._leftSliceDepth = left;
			this._rightSliceDepth = right;
			this._topSliceDepth = top;
			this._bottomSliceDepth = bottom;
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x005AAABA File Offset: 0x005A8CBA
		public void SetSliceDepths(int fluff)
		{
			this._leftSliceDepth = fluff;
			this._rightSliceDepth = fluff;
			this._topSliceDepth = fluff;
			this._bottomSliceDepth = fluff;
		}

		// Token: 0x04005580 RID: 21888
		private Asset<Texture2D> _texture;

		// Token: 0x04005581 RID: 21889
		private Color _color;

		// Token: 0x04005582 RID: 21890
		private int _leftSliceDepth;

		// Token: 0x04005583 RID: 21891
		private int _rightSliceDepth;

		// Token: 0x04005584 RID: 21892
		private int _topSliceDepth;

		// Token: 0x04005585 RID: 21893
		private int _bottomSliceDepth;
	}
}
