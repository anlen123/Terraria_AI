using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003E5 RID: 997
	public class EmoteButton : UIElement
	{
		// Token: 0x06002E3A RID: 11834 RVA: 0x005A9270 File Offset: 0x005A7470
		public EmoteButton(int emoteIndex)
		{
			this._texture = Main.Assets.Request<Texture2D>("Images/Extra_" + 48, 1);
			this._textureBorder = Main.Assets.Request<Texture2D>("Images/UI/EmoteBubbleBorder", 1);
			this._emoteIndex = emoteIndex;
			Rectangle frame = this.GetFrame();
			this.Width.Set((float)frame.Width, 0f);
			this.Height.Set((float)frame.Height, 0f);
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x005A92F8 File Offset: 0x005A74F8
		private Rectangle GetFrame()
		{
			int num = (this._frameCounter >= 10) ? 1 : 0;
			return this._texture.Frame(8, EmoteBubble.EMOTE_SHEET_VERTICAL_FRAMES, this._emoteIndex % 4 * 2 + num, this._emoteIndex / 4 + 1, 0, 0);
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x005A9340 File Offset: 0x005A7540
		private void UpdateFrame()
		{
			int num = this._frameCounter + 1;
			this._frameCounter = num;
			if (num >= 20)
			{
				this._frameCounter = 0;
			}
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x005A9369 File Offset: 0x005A7569
		public override void Update(GameTime gameTime)
		{
			this.UpdateFrame();
			base.Update(gameTime);
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x005A9378 File Offset: 0x005A7578
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			Vector2 vector = dimensions.Position() + new Vector2(dimensions.Width, dimensions.Height) / 2f;
			Rectangle frame = this.GetFrame();
			Rectangle value = frame;
			value.X = this._texture.Width() / 8;
			value.Y = 0;
			Vector2 origin = frame.Size() / 2f;
			Color white = Color.White;
			Color color = Color.Black;
			if (this._hovered)
			{
				color = Main.OurFavoriteColor;
			}
			spriteBatch.Draw(this._texture.Value, vector, new Rectangle?(value), white, 0f, origin, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(this._texture.Value, vector, new Rectangle?(frame), white, 0f, origin, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(this._textureBorder.Value, vector - Vector2.One * 2f, null, color, 0f, origin, 1f, SpriteEffects.None, 0f);
			if (this._hovered)
			{
				string name = EmoteID.Search.GetName(this._emoteIndex);
				string cursorText = "/" + Language.GetTextValue("EmojiName." + name);
				Main.instance.MouseText(cursorText, 0, 0, -1, -1, -1, -1, 0);
			}
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x005A94EE File Offset: 0x005A76EE
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this._hovered = true;
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x005A9513 File Offset: 0x005A7713
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this._hovered = false;
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x005A9523 File Offset: 0x005A7723
		public override void LeftClick(UIMouseEvent evt)
		{
			base.LeftClick(evt);
			EmoteBubble.MakeLocalPlayerEmote(this._emoteIndex);
			IngameFancyUI.Close(false);
		}

		// Token: 0x04005537 RID: 21815
		private Asset<Texture2D> _texture;

		// Token: 0x04005538 RID: 21816
		private Asset<Texture2D> _textureBorder;

		// Token: 0x04005539 RID: 21817
		private int _emoteIndex;

		// Token: 0x0400553A RID: 21818
		private bool _hovered;

		// Token: 0x0400553B RID: 21819
		private int _frameCounter;
	}
}
