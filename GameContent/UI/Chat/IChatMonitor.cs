using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.UI.Chat
{
	// Token: 0x0200037F RID: 895
	public interface IChatMonitor
	{
		// Token: 0x06002988 RID: 10632
		void NewText(string newText, byte R = 255, byte G = 255, byte B = 255);

		// Token: 0x06002989 RID: 10633
		void NewTextMultiline(string text, bool force = false, Color c = default(Color), int WidthLimit = -1);

		// Token: 0x0600298A RID: 10634
		void DrawChat(bool drawingPlayerChat);

		// Token: 0x0600298B RID: 10635
		void Clear();

		// Token: 0x0600298C RID: 10636
		void Update();

		// Token: 0x0600298D RID: 10637
		void Offset(int linesOffset);

		// Token: 0x0600298E RID: 10638
		void ResetOffset();
	}
}
