using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.Events
{
	// Token: 0x020004FE RID: 1278
	public class ScreenObstruction
	{
		// Token: 0x060035BD RID: 13757 RVA: 0x0061C8D4 File Offset: 0x0061AAD4
		public static void Update(SceneState sceneState, SceneMetrics metrics)
		{
			float num = 0f;
			float amount = 0.1f;
			if (metrics.PerspectivePlayer.insideUnbreakableWalls)
			{
				int progressPlayerCanSafelyMatch = DangerousDungeonCurse.GetProgressPlayerCanSafelyMatch();
				int num2 = DangerousDungeonCurse.GetProgressPlayerNeedsToMatch(metrics.PerspectivePlayer) - progressPlayerCanSafelyMatch;
				if (num2 > 0)
				{
					float max = 0.9f;
					num = Utils.Clamp<float>(0.4f * (float)num2, 0f, max);
					amount = (ScreenObstruction.lastSpeed = 0.01f);
				}
			}
			if (metrics.PerspectivePlayer.headcovered)
			{
				num = 0.95f;
				amount = (ScreenObstruction.lastSpeed = 0.3f);
			}
			if (num == 0f && ScreenObstruction.screenObstruction != 0f)
			{
				amount = ScreenObstruction.lastSpeed;
			}
			else
			{
				ScreenObstruction.lastSpeed = amount;
			}
			sceneState.MoveTowards(ref ScreenObstruction.screenObstruction, num, amount);
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x0061C988 File Offset: 0x0061AB88
		public static void Draw(SpriteBatch spriteBatch)
		{
			if (ScreenObstruction.screenObstruction == 0f)
			{
				return;
			}
			Color color = Color.Black * ScreenObstruction.screenObstruction;
			int num = TextureAssets.Extra[49].Width();
			int num2 = 10;
			Rectangle rect = Main.SceneMetrics.PerspectivePlayer.getRect();
			rect.Inflate((num - rect.Width) / 2, (num - rect.Height) / 2 + num2 / 2);
			rect.Offset(-(int)Main.screenPosition.X, -(int)Main.screenPosition.Y + (int)Main.player[Main.myPlayer].gfxOffY - num2);
			Rectangle destinationRectangle = Rectangle.Union(new Rectangle(0, 0, 1, 1), new Rectangle(rect.Right - 1, rect.Top - 1, 1, 1));
			Rectangle destinationRectangle2 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, 0, 1, 1), new Rectangle(rect.Right, rect.Bottom - 1, 1, 1));
			Rectangle destinationRectangle3 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left, rect.Bottom, 1, 1));
			Rectangle destinationRectangle4 = Rectangle.Union(new Rectangle(0, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left - 1, rect.Top, 1, 1));
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle, new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle2, new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle3, new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle4, new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
			spriteBatch.Draw(TextureAssets.Extra[49].Value, rect, color);
		}

		// Token: 0x04005ABB RID: 23227
		public static float lastSpeed = 0.1f;

		// Token: 0x04005ABC RID: 23228
		public static float screenObstruction;
	}
}
