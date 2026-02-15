using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003F5 RID: 1013
	public class UIKeybindingSimpleListItem : UIElement
	{
		// Token: 0x06002EA2 RID: 11938 RVA: 0x005ACA06 File Offset: 0x005AAC06
		public UIKeybindingSimpleListItem(Func<string> getText, Color color)
		{
			this._color = color;
			Func<string> getTextFunction;
			if (getText == null)
			{
				getTextFunction = (() => "???");
			}
			else
			{
				getTextFunction = getText;
			}
			this._GetTextFunction = getTextFunction;
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x005ACA40 File Offset: 0x005AAC40
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float num = 6f;
			base.DrawSelf(spriteBatch);
			CalculatedStyle dimensions = base.GetDimensions();
			float num2 = dimensions.Width + 1f;
			Vector2 vector = new Vector2(dimensions.X, dimensions.Y);
			Vector2 vector2 = new Vector2(0.8f);
			Color color = base.IsMouseHovering ? Color.White : Color.Silver;
			color = Color.Lerp(color, Color.White, base.IsMouseHovering ? 0.5f : 0f);
			Color color2 = base.IsMouseHovering ? this._color : this._color.MultiplyRGBA(new Color(180, 180, 180));
			Vector2 position = vector;
			Utils.DrawSettings2Panel(spriteBatch, position, num2, color2);
			position.X += 8f;
			position.Y += 2f + num;
			string text = this._GetTextFunction();
			Vector2 stringSize = ChatManager.GetStringSize(FontAssets.ItemStack.Value, text, vector2, -1f);
			position.X = dimensions.X + dimensions.Width / 2f - stringSize.X / 2f;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text, position, color, 0f, Vector2.Zero, vector2, num2, 2f);
		}

		// Token: 0x040055AE RID: 21934
		private Color _color;

		// Token: 0x040055AF RID: 21935
		private Func<string> _GetTextFunction;
	}
}
