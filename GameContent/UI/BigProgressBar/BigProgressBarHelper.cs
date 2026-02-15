using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x0200038B RID: 907
	public class BigProgressBarHelper
	{
		// Token: 0x060029C3 RID: 10691 RVA: 0x0057E2C4 File Offset: 0x0057C4C4
		public static void DrawBareBonesBar(SpriteBatch spriteBatch, float lifePercent)
		{
			Rectangle rectangle = Utils.CenteredRectangle(Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f), new Vector2(400f, 20f));
			Rectangle destinationRectangle = rectangle;
			destinationRectangle.Inflate(2, 2);
			Texture2D value = TextureAssets.MagicPixel.Value;
			Rectangle value2 = new Rectangle(0, 0, 1, 1);
			Rectangle rectangle2 = rectangle;
			rectangle2.Width = (int)((float)rectangle2.Width * lifePercent);
			spriteBatch.Draw(value, destinationRectangle, new Rectangle?(value2), Color.White * 0.6f);
			spriteBatch.Draw(value, rectangle, new Rectangle?(value2), Color.Black * 0.6f);
			spriteBatch.Draw(value, rectangle2, new Rectangle?(value2), Color.LimeGreen * 0.5f);
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x0057E3A8 File Offset: 0x0057C5A8
		public static void DrawFancyBar(SpriteBatch spriteBatch, float lifeAmount, float lifeMax, Texture2D barIconTexture, Rectangle barIconFrame)
		{
			Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/UI_BossBar", 1).Value;
			Point point = new Point(456, 22);
			Point point2 = new Point(32, 24);
			int verticalFrames = 6;
			Rectangle value2 = value.Frame(1, verticalFrames, 0, 3, 0, 0);
			Color color = Color.White * 0.2f;
			float num = lifeAmount / lifeMax;
			int num2 = (int)((float)point.X * num);
			num2 -= num2 % 2;
			Rectangle rectangle = value.Frame(1, verticalFrames, 0, 2, 0, 0);
			rectangle.X += point2.X;
			rectangle.Y += point2.Y;
			rectangle.Width = 2;
			rectangle.Height = point.Y;
			Rectangle value3 = value.Frame(1, verticalFrames, 0, 1, 0, 0);
			value3.X += point2.X;
			value3.Y += point2.Y;
			value3.Width = 2;
			value3.Height = point.Y;
			Rectangle rectangle2 = Utils.CenteredRectangle(Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f), point.ToVector2());
			Vector2 vector = rectangle2.TopLeft() - point2.ToVector2();
			spriteBatch.Draw(value, vector, new Rectangle?(value2), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(value, rectangle2.TopLeft(), new Rectangle?(rectangle), Color.White, 0f, Vector2.Zero, new Vector2((float)(num2 / rectangle.Width), 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(value, rectangle2.TopLeft() + new Vector2((float)(num2 - 2), 0f), new Rectangle?(value3), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Rectangle value4 = value.Frame(1, verticalFrames, 0, 0, 0, 0);
			spriteBatch.Draw(value, vector, new Rectangle?(value4), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Vector2 value5 = new Vector2(4f, 20f) + new Vector2(26f, 28f) / 2f;
			spriteBatch.Draw(barIconTexture, vector + value5, new Rectangle?(barIconFrame), Color.White, 0f, barIconFrame.Size() / 2f, 1f, SpriteEffects.None, 0f);
			if (BigProgressBarSystem.ShowText)
			{
				BigProgressBarHelper.DrawHealthText(spriteBatch, rectangle2, lifeAmount, lifeMax);
			}
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x0057E65C File Offset: 0x0057C85C
		private static void DrawHealthText(SpriteBatch spriteBatch, Rectangle area, float current, float max)
		{
			DynamicSpriteFont value = FontAssets.ItemStack.Value;
			Vector2 vector = area.Center.ToVector2();
			vector.Y += 1f;
			string text = "/";
			Vector2 vector2 = value.MeasureString(text);
			Utils.DrawBorderStringFourWay(spriteBatch, value, text, vector.X, vector.Y, Color.White, Color.Black, vector2 * 0.5f, 1f);
			text = ((int)current).ToString();
			vector2 = value.MeasureString(text);
			Utils.DrawBorderStringFourWay(spriteBatch, value, text, vector.X - 5f, vector.Y, Color.White, Color.Black, vector2 * new Vector2(1f, 0.5f), 1f);
			text = ((int)max).ToString();
			vector2 = value.MeasureString(text);
			Utils.DrawBorderStringFourWay(spriteBatch, value, text, vector.X + 5f, vector.Y, Color.White, Color.Black, vector2 * new Vector2(0f, 0.5f), 1f);
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x0057E774 File Offset: 0x0057C974
		public static void DrawFancyBar(SpriteBatch spriteBatch, float lifeAmount, float lifeMax, Texture2D barIconTexture, Rectangle barIconFrame, float shieldCurrent, float shieldMax)
		{
			Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/UI_BossBar", 1).Value;
			Point point = new Point(456, 22);
			Point point2 = new Point(32, 24);
			int verticalFrames = 6;
			Rectangle value2 = value.Frame(1, verticalFrames, 0, 3, 0, 0);
			Color color = Color.White * 0.2f;
			float num = lifeAmount / lifeMax;
			int num2 = (int)((float)point.X * num);
			num2 -= num2 % 2;
			Rectangle rectangle = value.Frame(1, verticalFrames, 0, 2, 0, 0);
			rectangle.X += point2.X;
			rectangle.Y += point2.Y;
			rectangle.Width = 2;
			rectangle.Height = point.Y;
			Rectangle value3 = value.Frame(1, verticalFrames, 0, 1, 0, 0);
			value3.X += point2.X;
			value3.Y += point2.Y;
			value3.Width = 2;
			value3.Height = point.Y;
			float num3 = shieldCurrent / shieldMax;
			int num4 = (int)((float)point.X * num3);
			num4 -= num4 % 2;
			Rectangle rectangle2 = value.Frame(1, verticalFrames, 0, 5, 0, 0);
			rectangle2.X += point2.X;
			rectangle2.Y += point2.Y;
			rectangle2.Width = 2;
			rectangle2.Height = point.Y;
			Rectangle value4 = value.Frame(1, verticalFrames, 0, 4, 0, 0);
			value4.X += point2.X;
			value4.Y += point2.Y;
			value4.Width = 2;
			value4.Height = point.Y;
			Rectangle rectangle3 = Utils.CenteredRectangle(Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f), point.ToVector2());
			Vector2 vector = rectangle3.TopLeft() - point2.ToVector2();
			spriteBatch.Draw(value, vector, new Rectangle?(value2), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(value, rectangle3.TopLeft(), new Rectangle?(rectangle), Color.White, 0f, Vector2.Zero, new Vector2((float)(num2 / rectangle.Width), 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(value, rectangle3.TopLeft() + new Vector2((float)(num2 - 2), 0f), new Rectangle?(value3), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(value, rectangle3.TopLeft(), new Rectangle?(rectangle2), Color.White, 0f, Vector2.Zero, new Vector2((float)(num4 / rectangle2.Width), 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(value, rectangle3.TopLeft() + new Vector2((float)(num4 - 2), 0f), new Rectangle?(value4), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Rectangle value5 = value.Frame(1, verticalFrames, 0, 0, 0, 0);
			spriteBatch.Draw(value, vector, new Rectangle?(value5), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Vector2 value6 = new Vector2(4f, 20f) + barIconFrame.Size() / 2f;
			spriteBatch.Draw(barIconTexture, vector + value6, new Rectangle?(barIconFrame), Color.White, 0f, barIconFrame.Size() / 2f, 1f, SpriteEffects.None, 0f);
			if (BigProgressBarSystem.ShowText)
			{
				if (shieldCurrent > 0f)
				{
					BigProgressBarHelper.DrawHealthText(spriteBatch, rectangle3, shieldCurrent, shieldMax);
					return;
				}
				BigProgressBarHelper.DrawHealthText(spriteBatch, rectangle3, lifeAmount, lifeMax);
			}
		}

		// Token: 0x040052A3 RID: 21155
		private const string _bossBarTexturePath = "Images/UI/UI_BossBar";
	}
}
