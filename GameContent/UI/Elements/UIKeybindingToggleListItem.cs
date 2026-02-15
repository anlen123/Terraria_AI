using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003F7 RID: 1015
	public class UIKeybindingToggleListItem : UIElement
	{
		// Token: 0x06002EA6 RID: 11942 RVA: 0x005ACED8 File Offset: 0x005AB0D8
		public UIKeybindingToggleListItem(Func<string> getText, Func<bool> getStatus, Color color)
		{
			this._color = color;
			this._toggleTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle", 1);
			Func<string> textDisplayFunction;
			if (getText == null)
			{
				textDisplayFunction = (() => "???");
			}
			else
			{
				textDisplayFunction = getText;
			}
			this._TextDisplayFunction = textDisplayFunction;
			Func<bool> isOnFunction;
			if (getStatus == null)
			{
				isOnFunction = (() => false);
			}
			else
			{
				isOnFunction = getStatus;
			}
			this._IsOnFunction = isOnFunction;
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x005ACF60 File Offset: 0x005AB160
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float num = 6f;
			base.DrawSelf(spriteBatch);
			CalculatedStyle dimensions = base.GetDimensions();
			float num2 = dimensions.Width + 1f;
			Vector2 vector = new Vector2(dimensions.X, dimensions.Y);
			bool flag = false;
			Vector2 scale = new Vector2(0.8f);
			Color color = flag ? Color.Gold : (base.IsMouseHovering ? Color.White : Color.Silver);
			color = Color.Lerp(color, Color.White, base.IsMouseHovering ? 0.5f : 0f);
			Color color2 = base.IsMouseHovering ? this._color : this._color.MultiplyRGBA(new Color(180, 180, 180));
			Vector2 position = vector;
			Utils.DrawSettingsPanel(spriteBatch, position, num2, color2);
			position.X += 8f;
			position.Y += 2f + num;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, this._TextDisplayFunction(), position, color, 0f, Vector2.Zero, scale, num2, 2f);
			position.X -= 17f;
			Rectangle rectangle = new Rectangle(this._IsOnFunction() ? ((this._toggleTexture.Width() - 2) / 2 + 2) : 0, 0, (this._toggleTexture.Width() - 2) / 2, this._toggleTexture.Height());
			Vector2 vector2 = new Vector2((float)rectangle.Width, 0f);
			position = new Vector2(dimensions.X + dimensions.Width - vector2.X - 10f, dimensions.Y + 2f + num);
			spriteBatch.Draw(this._toggleTexture.Value, position, new Rectangle?(rectangle), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
		}

		// Token: 0x040055B7 RID: 21943
		private Color _color;

		// Token: 0x040055B8 RID: 21944
		private Func<string> _TextDisplayFunction;

		// Token: 0x040055B9 RID: 21945
		private Func<bool> _IsOnFunction;

		// Token: 0x040055BA RID: 21946
		private Asset<Texture2D> _toggleTexture;
	}
}
