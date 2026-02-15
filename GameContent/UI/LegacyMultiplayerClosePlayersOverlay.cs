using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI
{
	// Token: 0x02000370 RID: 880
	public class LegacyMultiplayerClosePlayersOverlay : IMultiplayerClosePlayersOverlay
	{
		// Token: 0x0600292D RID: 10541 RVA: 0x00578784 File Offset: 0x00576984
		public void Draw()
		{
			int teamNamePlateDistance = Main.teamNamePlateDistance;
			if (teamNamePlateDistance <= 0)
			{
				return;
			}
			SpriteBatch spriteBatch = Main.spriteBatch;
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
			PlayerInput.SetZoom_World();
			int screenWidth = Main.screenWidth;
			int screenHeight = Main.screenHeight;
			Vector2 screenPosition = Main.screenPosition;
			PlayerInput.SetZoom_UI();
			float uiscale = Main.UIScale;
			int num = teamNamePlateDistance * 8;
			Player[] player = Main.player;
			int myPlayer = Main.myPlayer;
			SpriteViewMatrix gameViewMatrix = Main.GameViewMatrix;
			byte mouseTextColor = Main.mouseTextColor;
			Color[] teamColor = Main.teamColor;
			Camera camera = Main.Camera;
			IPlayerRenderer playerRenderer = Main.PlayerRenderer;
			Vector2 screenPosition2 = Main.screenPosition;
			for (int i = 0; i < 255; i++)
			{
				if (player[i].active && myPlayer != i && (!player[i].dead || player[i].ghost) && player[myPlayer].team > 0 && player[myPlayer].team == player[i].team)
				{
					string name = player[i].name;
					Vector2 vector = FontAssets.MouseText.Value.MeasureString(name);
					float num2 = 0f;
					if (player[i].chatOverhead.timeLeft > 0)
					{
						num2 = -vector.Y * uiscale;
					}
					else if (player[i].emoteTime > 0)
					{
						num2 = -vector.Y * uiscale;
					}
					Vector2 vector2 = new Vector2((float)(screenWidth / 2) + screenPosition.X, (float)(screenHeight / 2) + screenPosition.Y);
					Vector2 vector3 = player[i].position;
					vector3 += (vector3 - vector2) * (gameViewMatrix.RenderZoom - Vector2.One);
					float num3 = 0f;
					float num4 = (float)mouseTextColor / 255f;
					Color baseColor = new Color((int)((byte)((float)teamColor[player[i].team].R * num4)), (int)((byte)((float)teamColor[player[i].team].G * num4)), (int)((byte)((float)teamColor[player[i].team].B * num4)), (int)mouseTextColor);
					float num5 = vector3.X + (float)(player[i].width / 2) - vector2.X;
					float num6 = vector3.Y - vector.Y - 2f + num2 - vector2.Y;
					float num7 = (float)Math.Sqrt((double)(num5 * num5 + num6 * num6));
					int num8 = screenHeight;
					if (screenHeight > screenWidth)
					{
						num8 = screenWidth;
					}
					num8 = num8 / 2 - 50;
					if (num8 < 100)
					{
						num8 = 100;
					}
					if (num7 < (float)num8)
					{
						vector.X = vector3.X + (float)(player[i].width / 2) - vector.X / 2f - screenPosition.X;
						vector.Y = vector3.Y - vector.Y - 2f + num2 - screenPosition.Y;
					}
					else
					{
						num3 = num7;
						num7 = (float)num8 / num7;
						vector.X = (float)(screenWidth / 2) + num5 * num7 - vector.X / 2f;
						vector.Y = (float)(screenHeight / 2) + num6 * num7 + 40f * uiscale;
					}
					Vector2 vector4 = FontAssets.MouseText.Value.MeasureString(name);
					vector += vector4 / 2f;
					vector *= 1f / uiscale;
					vector -= vector4 / 2f;
					if (player[myPlayer].gravDir == -1f)
					{
						vector.Y = (float)screenHeight - vector.Y;
					}
					if (num3 > 0f)
					{
						float num9 = 20f;
						float num10 = -27f;
						num10 -= (vector4.X - 85f) / 2f;
						num5 = player[i].Center.X - player[myPlayer].Center.X;
						num6 = player[i].Center.Y - player[myPlayer].Center.Y;
						float num11 = (float)Math.Sqrt((double)(num5 * num5 + num6 * num6));
						if (num11 <= (float)num)
						{
							string textValue = Language.GetTextValue("GameUI.PlayerDistance", (int)(num11 / 16f * 2f));
							Vector2 vector5 = FontAssets.MouseText.Value.MeasureString(textValue);
							vector5.X = vector.X - num10;
							vector5.Y = vector.Y + vector4.Y / 2f - vector5.Y / 2f - num9;
							LegacyMultiplayerClosePlayersOverlay.DrawPlayerName2(spriteBatch, ref baseColor, textValue, ref vector5);
							Color playerHeadBordersColor = Main.GetPlayerHeadBordersColor(player[i]);
							Vector2 position = new Vector2(vector.X, vector.Y - num9);
							position.X -= 22f + num10;
							position.Y += 8f;
							playerRenderer.DrawPlayerHead(camera, player[i], position, 1f, 0.8f, playerHeadBordersColor);
							Vector2 vector6 = vector5 + screenPosition2 + new Vector2(26f, 20f);
							if (player[i].statLife != player[i].statLifeMax2)
							{
								Main.instance.DrawHealthBar(vector6.X, vector6.Y, player[i].statLife, player[i].statLifeMax2, 1f, 1.25f, true);
							}
							ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, name, vector + new Vector2(0f, -40f), baseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
						}
					}
					else
					{
						LegacyMultiplayerClosePlayersOverlay.DrawPlayerName(spriteBatch, name, ref vector, ref baseColor);
					}
				}
			}
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x00578D68 File Offset: 0x00576F68
		private static void DrawPlayerName2(SpriteBatch spriteBatch, ref Color namePlateColor, string npDist, ref Vector2 npDistPos)
		{
			float num = 0.85f;
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, npDist, new Vector2(npDistPos.X - 2f, npDistPos.Y), Color.Black, 0f, default(Vector2), num, SpriteEffects.None, 0f, null, null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, npDist, new Vector2(npDistPos.X + 2f, npDistPos.Y), Color.Black, 0f, default(Vector2), num, SpriteEffects.None, 0f, null, null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, npDist, new Vector2(npDistPos.X, npDistPos.Y - 2f), Color.Black, 0f, default(Vector2), num, SpriteEffects.None, 0f, null, null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, npDist, new Vector2(npDistPos.X, npDistPos.Y + 2f), Color.Black, 0f, default(Vector2), num, SpriteEffects.None, 0f, null, null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, npDist, npDistPos, namePlateColor, 0f, default(Vector2), num, SpriteEffects.None, 0f, null, null);
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x00578EC0 File Offset: 0x005770C0
		private static void DrawPlayerName(SpriteBatch spriteBatch, string namePlate, ref Vector2 namePlatePos, ref Color namePlateColor)
		{
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, namePlate, new Vector2(namePlatePos.X - 2f, namePlatePos.Y), Color.Black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f, null, null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, namePlate, new Vector2(namePlatePos.X + 2f, namePlatePos.Y), Color.Black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f, null, null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, namePlate, new Vector2(namePlatePos.X, namePlatePos.Y - 2f), Color.Black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f, null, null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, namePlate, new Vector2(namePlatePos.X, namePlatePos.Y + 2f), Color.Black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f, null, null);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, namePlate, namePlatePos, namePlateColor, 0f, default(Vector2), 1f, SpriteEffects.None, 0f, null, null);
		}
	}
}
