using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x02000400 RID: 1024
	public class UIImageFramed : UIElement, IColorable
	{
		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06002EF9 RID: 12025 RVA: 0x005B0076 File Offset: 0x005AE276
		// (set) Token: 0x06002EFA RID: 12026 RVA: 0x005B007E File Offset: 0x005AE27E
		public Color Color { get; set; }

		// Token: 0x06002EFB RID: 12027 RVA: 0x005B0088 File Offset: 0x005AE288
		public UIImageFramed(Asset<Texture2D> texture, Rectangle frame)
		{
			this._texture = texture;
			this._frame = frame;
			this.Width.Set((float)this._frame.Width, 0f);
			this.Height.Set((float)this._frame.Height, 0f);
			this.Color = Color.White;
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x005B00EC File Offset: 0x005AE2EC
		public void SetImage(Asset<Texture2D> texture, Rectangle frame)
		{
			this._texture = texture;
			this._frame = frame;
			this.Width.Set((float)this._frame.Width, 0f);
			this.Height.Set((float)this._frame.Height, 0f);
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x005B0140 File Offset: 0x005AE340
		public void SetFrame(Rectangle frame)
		{
			this._frame = frame;
			this.Width.Set((float)this._frame.Width, 0f);
			this.Height.Set((float)this._frame.Height, 0f);
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x005B018C File Offset: 0x005AE38C
		public void SetFrame(int frameCountHorizontal, int frameCountVertical, int frameX, int frameY, int sizeOffsetX, int sizeOffsetY)
		{
			this.SetFrame(this._texture.Frame(frameCountHorizontal, frameCountVertical, frameX, frameY, 0, 0).OffsetSize(sizeOffsetX, sizeOffsetY));
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x005B01B0 File Offset: 0x005AE3B0
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			spriteBatch.Draw(this._texture.Value, dimensions.Position(), new Rectangle?(this._frame), this.Color);
		}

		// Token: 0x0400560C RID: 22028
		private Asset<Texture2D> _texture;

		// Token: 0x0400560D RID: 22029
		private Rectangle _frame;
	}
}
