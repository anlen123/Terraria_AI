using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.Events
{
	// Token: 0x020004FD RID: 1277
	public class ScreenDarkness
	{
		// Token: 0x060035B8 RID: 13752 RVA: 0x0061C6A8 File Offset: 0x0061A8A8
		public static void Update(SceneState sceneState, SceneMetrics metrics)
		{
			float target = 0f;
			float amount = 0.016666668f;
			Vector2 center = metrics.Center;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == 370 && Main.npc[i].Distance(center) < 3000f && (Main.npc[i].ai[0] >= 10f || (Main.npc[i].ai[0] == 9f && Main.npc[i].ai[2] > 120f)))
				{
					target = 0.95f;
					ScreenDarkness.frontColor = new Color(0, 0, 120) * 0.3f;
					amount = 0.03f;
				}
				if (Main.npc[i].active && Main.npc[i].type == 113 && Main.npc[i].Distance(center) < 3000f)
				{
					float num = Utils.Remap(Main.npc[i].Distance(center), 2000f, 3000f, 1f, 0f, true);
					target = Main.npc[i].localAI[1] * num;
					amount = 1f;
					ScreenDarkness.frontColor = Color.Black;
				}
			}
			sceneState.MoveTowards(ref ScreenDarkness.screenObstruction, target, amount);
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x0061C804 File Offset: 0x0061AA04
		public static void DrawBack(SpriteBatch spriteBatch)
		{
			if (ScreenDarkness.screenObstruction == 0f)
			{
				return;
			}
			Color color = Color.Black * ScreenDarkness.screenObstruction;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x0061C864 File Offset: 0x0061AA64
		public static void DrawFront(SpriteBatch spriteBatch)
		{
			if (ScreenDarkness.screenObstruction == 0f)
			{
				return;
			}
			Color color = ScreenDarkness.frontColor * ScreenDarkness.screenObstruction;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
		}

		// Token: 0x04005AB9 RID: 23225
		public static float screenObstruction;

		// Token: 0x04005ABA RID: 23226
		public static Color frontColor = new Color(0, 0, 120);
	}
}
