using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.Localization;

namespace Terraria.DataStructures
{
	// Token: 0x0200058A RID: 1418
	public class TitleLinkButton
	{
		// Token: 0x06003804 RID: 14340 RVA: 0x0062F828 File Offset: 0x0062DA28
		public void Draw(SpriteBatch spriteBatch, Vector2 anchorPosition)
		{
			Rectangle r = this.Image.Frame(1, 1, 0, 0, 0, 0);
			if (this.FrameWhenNotSelected != null)
			{
				r = this.FrameWhenNotSelected.Value;
			}
			Vector2 vector = r.Size();
			Vector2 vector2 = anchorPosition - vector / 2f;
			bool flag = false;
			if (Main.MouseScreen.Between(vector2, vector2 + vector))
			{
				Main.LocalPlayer.mouseInterface = true;
				flag = true;
				this.DrawTooltip();
				this.TryClicking();
			}
			Rectangle? rectangle = flag ? this.FrameWehnSelected : this.FrameWhenNotSelected;
			Rectangle rectangle2 = this.Image.Frame(1, 1, 0, 0, 0, 0);
			if (rectangle != null)
			{
				rectangle2 = rectangle.Value;
			}
			Texture2D value = this.Image.Value;
			spriteBatch.Draw(value, anchorPosition, new Rectangle?(rectangle2), Color.White, 0f, rectangle2.Size() / 2f, 1f, SpriteEffects.None, 0f);
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x0062F924 File Offset: 0x0062DB24
		private void DrawTooltip()
		{
			Item fakeItem = TitleLinkButton._fakeItem;
			fakeItem.SetDefaults(0, null);
			string textValue = Language.GetTextValue(this.TooltipTextKey);
			fakeItem.SetNameOverride(textValue);
			fakeItem.type = 1;
			fakeItem.scale = 0f;
			fakeItem.rare = 8;
			fakeItem.value = -1;
			Main.HoverItem = TitleLinkButton._fakeItem;
			Main.instance.MouseText("", 0, 0, -1, -1, -1, -1, 0);
			Main.mouseText = true;
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x0062F996 File Offset: 0x0062DB96
		private void TryClicking()
		{
			if (PlayerInput.IgnoreMouseInterface)
			{
				return;
			}
			if (!Main.mouseLeft || !Main.mouseLeftRelease)
			{
				return;
			}
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.mouseLeftRelease = false;
			this.OpenLink();
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x0062F9D0 File Offset: 0x0062DBD0
		private void OpenLink()
		{
			try
			{
				Platform.Get<IPathService>().OpenURL(this.LinkUrl);
			}
			catch
			{
				Console.WriteLine("Failed to open link?!");
			}
		}

		// Token: 0x04005C1A RID: 23578
		private static Item _fakeItem = new Item();

		// Token: 0x04005C1B RID: 23579
		public string TooltipTextKey;

		// Token: 0x04005C1C RID: 23580
		public string LinkUrl;

		// Token: 0x04005C1D RID: 23581
		public Asset<Texture2D> Image;

		// Token: 0x04005C1E RID: 23582
		public Rectangle? FrameWhenNotSelected;

		// Token: 0x04005C1F RID: 23583
		public Rectangle? FrameWehnSelected;
	}
}
