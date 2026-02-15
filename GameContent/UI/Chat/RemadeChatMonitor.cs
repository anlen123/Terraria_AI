using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
	// Token: 0x02000381 RID: 897
	public class RemadeChatMonitor : IChatMonitor
	{
		// Token: 0x0600299A RID: 10650 RVA: 0x0057D49B File Offset: 0x0057B69B
		public RemadeChatMonitor()
		{
			this._showCount = 10;
			this._startChatLine = 0;
			this._messages = new List<ChatMessageContainer>();
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x0057D4BD File Offset: 0x0057B6BD
		public void NewText(string newText, byte R = 255, byte G = 255, byte B = 255)
		{
			this.AddNewMessage(newText, new Color((int)R, (int)G, (int)B), -1);
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x0057D4D0 File Offset: 0x0057B6D0
		public void NewTextMultiline(string text, bool force = false, Color c = default(Color), int WidthLimit = -1)
		{
			this.AddNewMessage(text, c, WidthLimit);
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x0057D4DC File Offset: 0x0057B6DC
		public void AddNewMessage(string text, Color color, int widthLimitInPixels = -1)
		{
			Trace.WriteLine("[chat] " + text);
			ChatMessageContainer chatMessageContainer = new ChatMessageContainer();
			chatMessageContainer.SetContents(text, color, widthLimitInPixels);
			this._messages.Insert(0, chatMessageContainer);
			while (this._messages.Count > 500)
			{
				this._messages.RemoveAt(this._messages.Count - 1);
			}
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x0057D544 File Offset: 0x0057B744
		public void DrawChat(bool drawingPlayerChat)
		{
			int num = this._startChatLine;
			int num2 = 0;
			int num3 = 0;
			while (num > 0 && num2 < this._messages.Count)
			{
				int num4 = Math.Min(num, this._messages[num2].LineCount);
				num -= num4;
				num3 += num4;
				if (num3 == this._messages[num2].LineCount)
				{
					num3 = 0;
					num2++;
				}
			}
			int num5 = 0;
			int? num6 = null;
			int snippetIndex = -1;
			int? num7 = null;
			int num8 = -1;
			while (num5 < this._showCount && num2 < this._messages.Count)
			{
				ChatMessageContainer chatMessageContainer = this._messages[num2];
				if (!chatMessageContainer.Prepared || !(drawingPlayerChat | chatMessageContainer.CanBeShownWhenChatIsClosed))
				{
					break;
				}
				TextSnippet[] snippetWithInversedIndex = chatMessageContainer.GetSnippetWithInversedIndex(num3);
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, snippetWithInversedIndex, new Vector2(88f, (float)(Main.screenHeight - 30 - 28 - num5 * 22)), 0f, Vector2.Zero, Vector2.One, out num8, -1f, 2f);
				if (num8 >= 0)
				{
					num7 = new int?(num8);
					num6 = new int?(num2);
					snippetIndex = num3;
				}
				num5++;
				num3++;
				if (num3 >= chatMessageContainer.LineCount)
				{
					num3 = 0;
					num2++;
				}
			}
			if (num6 != null && num7 != null)
			{
				TextSnippet[] snippetWithInversedIndex2 = this._messages[num6.Value].GetSnippetWithInversedIndex(snippetIndex);
				snippetWithInversedIndex2[num7.Value].OnHover();
				Main.LocalPlayer.mouseInterface = true;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					snippetWithInversedIndex2[num7.Value].OnClick();
				}
			}
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x0057D6F7 File Offset: 0x0057B8F7
		public void Clear()
		{
			this._messages.Clear();
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x0057D704 File Offset: 0x0057B904
		public void Update()
		{
			if (this._lastChatWidthLimit != Main.ChatLineWidthLimit)
			{
				this._lastChatWidthLimit = Main.ChatLineWidthLimit;
				foreach (ChatMessageContainer chatMessageContainer in this._messages)
				{
					chatMessageContainer.OnWidthLimitChanged();
				}
			}
			foreach (ChatMessageContainer chatMessageContainer2 in this._messages)
			{
				chatMessageContainer2.Update();
			}
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x0057D7AC File Offset: 0x0057B9AC
		public void Offset(int linesOffset)
		{
			this._startChatLine += linesOffset;
			this.ClampMessageIndex();
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x0057D7C4 File Offset: 0x0057B9C4
		private void ClampMessageIndex()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = this._startChatLine + this._showCount;
			while (num < num4 && num2 < this._messages.Count)
			{
				int num5 = Math.Min(num4 - num, this._messages[num2].LineCount);
				num += num5;
				if (num < num4)
				{
					num2++;
					num3 = 0;
				}
				else
				{
					num3 = num5;
				}
			}
			int num6 = this._showCount;
			while (num6 > 0 && num > 0)
			{
				num3--;
				num6--;
				num--;
				if (num3 < 0)
				{
					num2--;
					if (num2 == -1)
					{
						break;
					}
					num3 = this._messages[num2].LineCount - 1;
				}
			}
			this._startChatLine = num;
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x0057D870 File Offset: 0x0057BA70
		public void ResetOffset()
		{
			this._startChatLine = 0;
		}

		// Token: 0x04005282 RID: 21122
		private const int MaxMessages = 500;

		// Token: 0x04005283 RID: 21123
		private int _showCount;

		// Token: 0x04005284 RID: 21124
		private int _startChatLine;

		// Token: 0x04005285 RID: 21125
		private List<ChatMessageContainer> _messages;

		// Token: 0x04005286 RID: 21126
		private int _lastChatWidthLimit;
	}
}
