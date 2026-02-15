using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.UI
{
	// Token: 0x020000F1 RID: 241
	public class NetDiagnosticsUI : INetDiagnosticsUI
	{
		// Token: 0x06001916 RID: 6422 RVA: 0x004E6854 File Offset: 0x004E4A54
		public void Reset()
		{
			this.bytesRecv = 0;
			this.bytesRecvLast = 0;
			this.bytesSent = 0;
			this.bytesSentLast = 0;
			for (int i = 0; i < this._counterByMessageId.Length; i++)
			{
				this._counterByMessageId[i].Reset();
			}
			this._counterByModuleId.Clear();
			this._counterByMessageId[10].exemptFromBadScoreTest = true;
			this._counterByMessageId[82].exemptFromBadScoreTest = true;
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x004E68DA File Offset: 0x004E4ADA
		public void CountReadMessage(int messageId, int messageLength)
		{
			Interlocked.Add(ref this.bytesRecv, messageLength);
			this._counterByMessageId[messageId].CountReadMessage(messageLength);
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x004E68FB File Offset: 0x004E4AFB
		public void CountSentMessage(int messageId, int messageLength)
		{
			Interlocked.Add(ref this.bytesSent, messageLength);
			this._counterByMessageId[messageId].CountSentMessage(messageLength);
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x004E691C File Offset: 0x004E4B1C
		public void CountReadModuleMessage(int moduleMessageId, int messageLength)
		{
			NetDiagnosticsUI.CounterForMessage value;
			this._counterByModuleId.TryGetValue(moduleMessageId, out value);
			value.CountReadMessage(messageLength);
			this._counterByModuleId[moduleMessageId] = value;
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x004E6950 File Offset: 0x004E4B50
		public void CountSentModuleMessage(int moduleMessageId, int messageLength)
		{
			NetDiagnosticsUI.CounterForMessage value;
			this._counterByModuleId.TryGetValue(moduleMessageId, out value);
			value.CountSentMessage(messageLength);
			this._counterByModuleId[moduleMessageId] = value;
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x004E6981 File Offset: 0x004E4B81
		public void RotateSendRecvCounters()
		{
			this.bytesRecvLast = Interlocked.Exchange(ref this.bytesRecv, 0);
			this.bytesSentLast = Interlocked.Exchange(ref this.bytesSent, 0);
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x004E69AB File Offset: 0x004E4BAB
		public void GetLastSentRecvBytes(out int sent, out int recv)
		{
			sent = this.bytesSentLast;
			recv = this.bytesRecvLast;
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x004E69C4 File Offset: 0x004E4BC4
		public void Draw(SpriteBatch spriteBatch)
		{
			Utils.DrawBorderString(Main.spriteBatch, "Packet Stats (bytes) F8 to hide", new Vector2(800f, 80f), Color.White, 1f, 0f, 0f, -1);
			int num = this._counterByMessageId.Length + this._counterByModuleId.Count;
			for (int i = 0; i <= num / 51; i++)
			{
				Utils.DrawInvBG(spriteBatch, 190 + 400 * i, 110, 390, 683, default(Color));
			}
			Vector2 position;
			for (int j = 0; j < this._counterByMessageId.Length; j++)
			{
				int num2 = j / 51;
				int num3 = j - num2 * 51;
				position.X = (float)(200 + num2 * 400);
				position.Y = (float)(120 + num3 * 13);
				this.DrawCounter(spriteBatch, ref this._counterByMessageId[j], j.ToString(), position);
			}
			int num4 = this._counterByMessageId.Length + 1;
			foreach (KeyValuePair<int, NetDiagnosticsUI.CounterForMessage> keyValuePair in this._counterByModuleId)
			{
				int num5 = num4 / 51;
				int num6 = num4 - num5 * 51;
				position.X = (float)(200 + num5 * 400);
				position.Y = (float)(120 + num6 * 13);
				NetDiagnosticsUI.CounterForMessage value = keyValuePair.Value;
				this.DrawCounter(spriteBatch, ref value, ".." + keyValuePair.Key.ToString(), position);
				num4++;
			}
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x004E6B70 File Offset: 0x004E4D70
		private void DrawCounter(SpriteBatch spriteBatch, ref NetDiagnosticsUI.CounterForMessage counter, string title, Vector2 position)
		{
			if (!counter.exemptFromBadScoreTest)
			{
				if (this._highestFoundReadCount < counter.timesReceived)
				{
					this._highestFoundReadCount = counter.timesReceived;
				}
				if (this._highestFoundReadBytes < counter.bytesReceived)
				{
					this._highestFoundReadBytes = counter.bytesReceived;
				}
			}
			Vector2 pos = position;
			string text = title + ": ";
			float num = Utils.Remap((float)counter.bytesReceived, 0f, (float)this._highestFoundReadBytes, 0f, 1f, true);
			Color color = Main.hslToRgb(0.3f * (1f - num), 1f, 0.5f, byte.MaxValue);
			if (counter.exemptFromBadScoreTest)
			{
				color = Color.White;
			}
			string text2 = text;
			this.DrawText(spriteBatch, text2, pos, color);
			pos.X += 30f;
			text2 = "rx:" + string.Format("{0,0}", counter.timesReceived);
			this.DrawText(spriteBatch, text2, pos, color);
			pos.X += 70f;
			text2 = string.Format("{0,0}", counter.bytesReceived);
			this.DrawText(spriteBatch, text2, pos, color);
			pos.X += 70f;
			text2 = text;
			this.DrawText(spriteBatch, text2, pos, color);
			pos.X += 30f;
			text2 = "tx:" + string.Format("{0,0}", counter.timesSent);
			this.DrawText(spriteBatch, text2, pos, color);
			pos.X += 70f;
			text2 = string.Format("{0,0}", counter.bytesSent);
			this.DrawText(spriteBatch, text2, pos, color);
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x004E6D1C File Offset: 0x004E4F1C
		private void DrawText(SpriteBatch spriteBatch, string text, Vector2 pos, Color color)
		{
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text, pos, color, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f, null, null);
		}

		// Token: 0x04001316 RID: 4886
		private NetDiagnosticsUI.CounterForMessage[] _counterByMessageId = new NetDiagnosticsUI.CounterForMessage[(int)(MessageID.Count + 1)];

		// Token: 0x04001317 RID: 4887
		private Dictionary<int, NetDiagnosticsUI.CounterForMessage> _counterByModuleId = new Dictionary<int, NetDiagnosticsUI.CounterForMessage>();

		// Token: 0x04001318 RID: 4888
		private volatile int bytesRecv;

		// Token: 0x04001319 RID: 4889
		private volatile int bytesRecvLast;

		// Token: 0x0400131A RID: 4890
		private volatile int bytesSent;

		// Token: 0x0400131B RID: 4891
		private volatile int bytesSentLast;

		// Token: 0x0400131C RID: 4892
		private int _highestFoundReadBytes = 1;

		// Token: 0x0400131D RID: 4893
		private int _highestFoundReadCount = 1;

		// Token: 0x02000704 RID: 1796
		private struct CounterForMessage
		{
			// Token: 0x06003FD4 RID: 16340 RVA: 0x0069B2A3 File Offset: 0x006994A3
			public void Reset()
			{
				this.timesReceived = 0;
				this.timesSent = 0;
				this.bytesReceived = 0;
				this.bytesSent = 0;
			}

			// Token: 0x06003FD5 RID: 16341 RVA: 0x0069B2C1 File Offset: 0x006994C1
			public void CountReadMessage(int messageLength)
			{
				this.timesReceived++;
				this.bytesReceived += messageLength;
			}

			// Token: 0x06003FD6 RID: 16342 RVA: 0x0069B2DF File Offset: 0x006994DF
			public void CountSentMessage(int messageLength)
			{
				this.timesSent++;
				this.bytesSent += messageLength;
			}

			// Token: 0x04006865 RID: 26725
			public int timesReceived;

			// Token: 0x04006866 RID: 26726
			public int timesSent;

			// Token: 0x04006867 RID: 26727
			public int bytesReceived;

			// Token: 0x04006868 RID: 26728
			public int bytesSent;

			// Token: 0x04006869 RID: 26729
			public bool exemptFromBadScoreTest;
		}
	}
}
