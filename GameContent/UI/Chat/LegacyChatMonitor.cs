using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
	// Token: 0x02000380 RID: 896
	public class LegacyChatMonitor : IChatMonitor
	{
		// Token: 0x0600298F RID: 10639 RVA: 0x0057CEE0 File Offset: 0x0057B0E0
		public LegacyChatMonitor()
		{
			this.showCount = 10;
			this.numChatLines = 500;
			this.chatLength = 600;
			this.chatLine = new ChatLine[this.numChatLines];
			for (int i = 0; i < this.numChatLines; i++)
			{
				this.chatLine[i] = new ChatLine();
			}
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x0057CF40 File Offset: 0x0057B140
		public void Clear()
		{
			for (int i = 0; i < this.numChatLines; i++)
			{
				this.chatLine[i] = new ChatLine();
			}
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x0057CF6B File Offset: 0x0057B16B
		public void ResetOffset()
		{
			this.startChatLine = 0;
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x0057CF74 File Offset: 0x0057B174
		public void Update()
		{
			for (int i = 0; i < this.numChatLines; i++)
			{
				this.chatLine[i].UpdateTimeLeft();
			}
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x0057CFA0 File Offset: 0x0057B1A0
		public void Offset(int linesOffset)
		{
			this.showCount = (int)((float)(Main.screenHeight / 3) / FontAssets.MouseText.Value.MeasureString("1").Y) - 1;
			if (linesOffset == 1)
			{
				this.startChatLine++;
				if (this.startChatLine + this.showCount >= this.numChatLines - 1)
				{
					this.startChatLine = this.numChatLines - this.showCount - 1;
				}
				if (this.chatLine[this.startChatLine + this.showCount].originalText == "")
				{
					this.startChatLine--;
					return;
				}
			}
			else if (linesOffset == -1)
			{
				this.startChatLine--;
				if (this.startChatLine < 0)
				{
					this.startChatLine = 0;
				}
			}
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x0057D06C File Offset: 0x0057B26C
		public void NewText(string newText, byte R = 255, byte G = 255, byte B = 255)
		{
			this.NewTextMultiline(newText, false, new Color((int)R, (int)G, (int)B), -1);
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x0057D080 File Offset: 0x0057B280
		public void NewTextInternal(string newText, byte R = 255, byte G = 255, byte B = 255, bool force = false)
		{
			int num = 80;
			if (!force && newText.Length > num)
			{
				string text = this.TrimIntoMultipleLines(R, G, B, num, newText);
				if (text.Length > 0)
				{
					this.NewTextInternal(text, R, G, B, true);
				}
				return;
			}
			for (int i = this.numChatLines - 1; i > 0; i--)
			{
				this.chatLine[i].Copy(this.chatLine[i - 1]);
			}
			this.chatLine[0].color = new Color((int)R, (int)G, (int)B);
			this.chatLine[0].originalText = newText;
			this.chatLine[0].parsedText = ChatManager.ParseMessage(this.chatLine[0].originalText, this.chatLine[0].color).ToArray();
			this.chatLine[0].showTime = this.chatLength;
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x0057D16C File Offset: 0x0057B36C
		private string TrimIntoMultipleLines(byte R, byte G, byte B, int maxTextSize, string oldText)
		{
			while (oldText.Length > maxTextSize)
			{
				int num = maxTextSize;
				int num2 = num;
				while (oldText.Substring(num2, 1) != " ")
				{
					num2--;
					if (num2 < 1)
					{
						break;
					}
				}
				if (num2 == 0)
				{
					while (oldText.Substring(num, 1) != " ")
					{
						num++;
						if (num >= oldText.Length - 1)
						{
							break;
						}
					}
				}
				else
				{
					num = num2;
				}
				if (num >= oldText.Length - 1)
				{
					num = oldText.Length;
				}
				string text = Utils.TrimUserString(oldText, num);
				this.NewTextInternal(text, R, G, B, true);
				oldText = oldText.Substring(text.Length);
				if (oldText.Length > 0)
				{
					while (oldText.Substring(0, 1) == " ")
					{
						oldText = oldText.Substring(1);
					}
				}
			}
			return oldText;
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x0057D244 File Offset: 0x0057B444
		public void NewTextMultiline(string text, bool force = false, Color c = default(Color), int WidthLimit = -1)
		{
			if (c == default(Color))
			{
				c = Color.White;
			}
			List<List<TextSnippet>> list;
			if (WidthLimit != -1)
			{
				list = Utils.WordwrapStringSmart(text, c, FontAssets.MouseText.Value, (float)WidthLimit, 10);
			}
			else
			{
				list = Utils.WordwrapStringSmart(text, c, FontAssets.MouseText.Value, (float)Main.ChatLineWidthLimit, 10);
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.NewText(list[i]);
			}
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x0057D2C0 File Offset: 0x0057B4C0
		public void NewText(List<TextSnippet> snippets)
		{
			for (int i = this.numChatLines - 1; i > 0; i--)
			{
				this.chatLine[i].Copy(this.chatLine[i - 1]);
			}
			this.chatLine[0].originalText = "this is a hack because draw checks length is higher than 0";
			this.chatLine[0].parsedText = snippets.ToArray();
			this.chatLine[0].showTime = this.chatLength;
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x0057D344 File Offset: 0x0057B544
		public void DrawChat(bool drawingPlayerChat)
		{
			int num = this.startChatLine;
			int num2 = this.startChatLine + this.showCount;
			if (num2 >= this.numChatLines)
			{
				num2 = --this.numChatLines;
				num = num2 - this.showCount;
			}
			int num3 = 0;
			int num4 = -1;
			int num5 = -1;
			for (int i = num; i < num2; i++)
			{
				if (drawingPlayerChat || (this.chatLine[i].showTime > 0 && this.chatLine[i].parsedText.Length != 0))
				{
					int num6 = -1;
					ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, this.chatLine[i].parsedText, new Vector2(88f, (float)(Main.screenHeight - 30 - 28 - num3 * 21)), 0f, Vector2.Zero, Vector2.One, out num6, -1f, 2f);
					if (num6 >= 0 && this.chatLine[i].parsedText[num6].CheckForHover)
					{
						num4 = i;
						num5 = num6;
					}
				}
				num3++;
			}
			if (num4 > -1)
			{
				this.chatLine[num4].parsedText[num5].OnHover();
				Main.LocalPlayer.mouseInterface = true;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					this.chatLine[num4].parsedText[num5].OnClick();
				}
			}
		}

		// Token: 0x0400527D RID: 21117
		private int numChatLines;

		// Token: 0x0400527E RID: 21118
		private ChatLine[] chatLine;

		// Token: 0x0400527F RID: 21119
		private int chatLength;

		// Token: 0x04005280 RID: 21120
		private int showCount;

		// Token: 0x04005281 RID: 21121
		private int startChatLine;
	}
}
