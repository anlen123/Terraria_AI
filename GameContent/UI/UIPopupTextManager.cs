using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;

namespace Terraria.GameContent.UI
{
	// Token: 0x0200036C RID: 876
	public class UIPopupTextManager
	{
		// Token: 0x06002919 RID: 10521 RVA: 0x00577A78 File Offset: 0x00575C78
		public void ResetText(UIPopupText text)
		{
			text.scale = 0f;
			text.rotation = 0f;
			text.alpha = 1f;
			text.alphaDir = -1;
			text.framesSinceSpawn = 0;
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x00577AAC File Offset: 0x00575CAC
		public int NewText(UIAdvancedPopupRequest request)
		{
			if (!Main.showItemText)
			{
				return -1;
			}
			if (Main.netMode == 2)
			{
				return -1;
			}
			int num = this.FindNextItemTextSlot();
			if (num >= 0)
			{
				string text = request.Text;
				Vector2 vector = FontAssets.MouseText.Value.MeasureString(text);
				UIPopupText uipopupText = this.popupText[num];
				this.ResetText(uipopupText);
				uipopupText.active = true;
				uipopupText.position = request.Position;
				if (request.Alignment >= UIPopupTextAlignment.BottomLeft)
				{
					UIPopupText uipopupText2 = uipopupText;
					uipopupText2.position.Y = uipopupText2.position.Y - vector.Y;
				}
				else if (request.Alignment >= UIPopupTextAlignment.MidLeft)
				{
					UIPopupText uipopupText3 = uipopupText;
					uipopupText3.position.Y = uipopupText3.position.Y - vector.Y / 2f;
				}
				int num2 = (int)(request.Alignment % UIPopupTextAlignment.MidLeft);
				if (num2 != 1)
				{
					if (num2 == 2)
					{
						UIPopupText uipopupText4 = uipopupText;
						uipopupText4.position.X = uipopupText4.position.X - vector.X;
					}
				}
				else
				{
					UIPopupText uipopupText5 = uipopupText;
					uipopupText5.position.X = uipopupText5.position.X - vector.X / 2f;
				}
				uipopupText.name = text;
				uipopupText.velocity = request.Velocity;
				uipopupText.lifeTime = request.DurationInFrames;
				uipopupText.context = request.Context;
				uipopupText.color = request.Color;
				uipopupText.PrepareDisplayText();
			}
			return num;
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x00577BE0 File Offset: 0x00575DE0
		private int FindNextItemTextSlot()
		{
			int num = -1;
			for (int i = 0; i < 20; i++)
			{
				if (!this.popupText[i].active)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				double num2 = (double)Main.bottomWorld;
				for (int j = 0; j < 20; j++)
				{
					if (num2 > (double)this.popupText[j].position.Y)
					{
						num = j;
						num2 = (double)this.popupText[j].position.Y;
					}
				}
			}
			return num;
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x00577C54 File Offset: 0x00575E54
		public void UpdateItemText()
		{
			int num = 0;
			for (int i = 0; i < 20; i++)
			{
				if (this.popupText[i].active)
				{
					num++;
					this.popupText[i].Update(i, this);
				}
			}
			this.numActive = num;
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x00577C9C File Offset: 0x00575E9C
		public void ClearAll()
		{
			for (int i = 0; i < 20; i++)
			{
				this.popupText[i] = new UIPopupText();
			}
			this.numActive = 0;
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x00577CCC File Offset: 0x00575ECC
		public void DrawItemTextPopups(float scaleTarget)
		{
			SpriteBatch spriteBatch = Main.spriteBatch;
			for (int i = 0; i < 20; i++)
			{
				UIPopupText uipopupText = this.popupText[i];
				if (uipopupText.active)
				{
					string displayText = uipopupText.displayText;
					Vector2 vector = FontAssets.MouseText.Value.MeasureString(displayText);
					Vector2 vector2 = new Vector2(vector.X * 0.5f, vector.Y * 0.5f);
					float num = scaleTarget;
					float num2 = uipopupText.scale / num;
					int num3 = (int)(255f - 255f * num2);
					float num4 = (float)uipopupText.color.R;
					float num5 = (float)uipopupText.color.G;
					float num6 = (float)uipopupText.color.B;
					float num7 = (float)uipopupText.color.A;
					num4 *= num2 * uipopupText.alpha * 0.3f;
					float alpha = uipopupText.alpha;
					float alpha2 = uipopupText.alpha;
					num7 *= num2 * uipopupText.alpha;
					Color color = Color.Black;
					float scale = 1f;
					Texture2D texture2D = null;
					if (uipopupText.context == UIPopupTextContext.SpecialSeed)
					{
						color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.6f % 1f, 1f, 0.6f, byte.MaxValue) * 0.6f;
						num *= 0.5f;
						scale = 0.8f;
					}
					int num8 = 40;
					Utils.EaseOutCirc((double)Utils.Remap((float)uipopupText.framesSinceSpawn, 0f, (float)num8, 0f, 1f, true));
					float num9 = (float)num3 / 255f;
					for (int j = 0; j < 5; j++)
					{
						Color color2 = color;
						float num10 = 0f;
						float num11 = 0f;
						if (j == 0)
						{
							num10 -= num * 2f;
						}
						else if (j == 1)
						{
							num10 += num * 2f;
						}
						else if (j == 2)
						{
							num11 -= num * 2f;
						}
						else if (j == 3)
						{
							num11 += num * 2f;
						}
						else
						{
							color2 = uipopupText.color * num2 * uipopupText.alpha * scale;
						}
						if (j < 4)
						{
							num7 = (float)uipopupText.color.A * num2 * uipopupText.alpha;
							color2 = new Color(0, 0, 0, (int)num7);
						}
						if (color != Color.Black && j < 4)
						{
							num10 *= 1.3f + 1.3f * num9;
							num11 *= 1.3f + 1.3f * num9;
						}
						float num12 = uipopupText.position.X + num10;
						float num13 = uipopupText.position.Y + num11;
						if (color != Color.Black && j < 4)
						{
							Color color3 = color;
							color3.A = (byte)MathHelper.Lerp(60f, 127f, Utils.GetLerpValue(0f, 255f, num7, true));
							DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, displayText, new Vector2(num12 + vector2.X, num13 + vector2.Y), Color.Lerp(color2, color3, 0.5f), uipopupText.rotation, vector2, uipopupText.scale, SpriteEffects.None, 0f, null, null);
							DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, displayText, new Vector2(num12 + vector2.X, num13 + vector2.Y), color3, uipopupText.rotation, vector2, uipopupText.scale, SpriteEffects.None, 0f, null, null);
						}
						else
						{
							DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, displayText, new Vector2(num12 + vector2.X, num13 + vector2.Y), color2, uipopupText.rotation, vector2, uipopupText.scale, SpriteEffects.None, 0f, null, null);
						}
						if (texture2D != null)
						{
							float scale2 = (1.3f - num9) * uipopupText.scale * 0.7f;
							Vector2 value = new Vector2(num12 + vector2.X, num13 + vector2.Y);
							Color value2 = color * 0.6f;
							if (j == 4)
							{
								value2 = Color.White * 0.6f;
							}
							value2.A = (byte)((float)value2.A * 0.5f);
							int num14 = 25;
							spriteBatch.Draw(texture2D, value + new Vector2(vector2.X * -0.5f - (float)num14 - texture2D.Size().X / 2f, 0f), null, value2 * uipopupText.scale, 0f, texture2D.Size() / 2f, scale2, SpriteEffects.None, 0f);
							spriteBatch.Draw(texture2D, value + new Vector2(vector2.X * 0.5f + (float)num14 + texture2D.Size().X / 2f, 0f), null, value2 * uipopupText.scale, 0f, texture2D.Size() / 2f, scale2, SpriteEffects.None, 0f);
						}
					}
				}
			}
		}

		// Token: 0x040051A2 RID: 20898
		public const int maxItemText = 20;

		// Token: 0x040051A3 RID: 20899
		public UIPopupText[] popupText = new UIPopupText[20];

		// Token: 0x040051A4 RID: 20900
		public int numActive;
	}
}
