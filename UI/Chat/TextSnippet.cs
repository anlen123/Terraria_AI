using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.UI.Chat
{
	// Token: 0x0200010D RID: 269
	public class TextSnippet
	{
		// Token: 0x06001A95 RID: 6805 RVA: 0x004F6B20 File Offset: 0x004F4D20
		public TextSnippet(string text = "")
		{
			this.Text = text;
			this.TextOriginal = text;
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x004F6B41 File Offset: 0x004F4D41
		public TextSnippet(string text, Color color)
		{
			this.Text = text;
			this.TextOriginal = text;
			this.Color = color;
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnHover()
		{
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnClick()
		{
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x004F6B69 File Offset: 0x004F4D69
		public virtual Color GetVisibleColor()
		{
			return ChatManager.WaveColor(this.Color);
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x004F6B76 File Offset: 0x004F4D76
		public virtual bool UniqueDraw(bool justCheckingSize, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
		{
			size = Vector2.Zero;
			return false;
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x004F6B84 File Offset: 0x004F4D84
		public virtual TextSnippet CopyMorph(string newText)
		{
			TextSnippet textSnippet = (TextSnippet)base.MemberwiseClone();
			textSnippet.Text = newText;
			return textSnippet;
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x004F6B98 File Offset: 0x004F4D98
		public override string ToString()
		{
			return "Text: " + this.Text + " | OriginalText: " + this.TextOriginal;
		}

		// Token: 0x040014EE RID: 5358
		public string Text;

		// Token: 0x040014EF RID: 5359
		public string TextOriginal;

		// Token: 0x040014F0 RID: 5360
		public Color Color = Color.White;

		// Token: 0x040014F1 RID: 5361
		public bool CheckForHover;

		// Token: 0x040014F2 RID: 5362
		public bool DeleteWhole;
	}
}
