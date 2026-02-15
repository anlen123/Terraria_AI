using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003F3 RID: 1011
	public class UIHorizontalSeparator : UIElement
	{
		// Token: 0x06002E9A RID: 11930 RVA: 0x005ABA3C File Offset: 0x005A9C3C
		public UIHorizontalSeparator(int EdgeWidth = 2, bool highlightSideUp = true)
		{
			this.Color = Color.White;
			if (highlightSideUp)
			{
				this._texture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Separator1", 1);
			}
			else
			{
				this._texture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Separator2", 1);
			}
			this.Width.Set((float)this._texture.Width(), 0f);
			this.Height.Set((float)this._texture.Height(), 0f);
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x005ABAC4 File Offset: 0x005A9CC4
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			Utils.DrawPanel(this._texture.Value, this.EdgeWidth, 0, spriteBatch, dimensions.Position(), dimensions.Width, this.Color);
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public override bool ContainsPoint(Vector2 point)
		{
			return false;
		}

		// Token: 0x040055A8 RID: 21928
		private Asset<Texture2D> _texture;

		// Token: 0x040055A9 RID: 21929
		public Color Color;

		// Token: 0x040055AA RID: 21930
		public int EdgeWidth;
	}
}
