using System;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.UI
{
	// Token: 0x020000EF RID: 239
	public interface INetDiagnosticsUI
	{
		// Token: 0x06001905 RID: 6405
		void Reset();

		// Token: 0x06001906 RID: 6406
		void Draw(SpriteBatch spriteBatch);

		// Token: 0x06001907 RID: 6407
		void CountReadMessage(int messageId, int messageLength);

		// Token: 0x06001908 RID: 6408
		void CountSentMessage(int messageId, int messageLength);

		// Token: 0x06001909 RID: 6409
		void CountReadModuleMessage(int moduleMessageId, int messageLength);

		// Token: 0x0600190A RID: 6410
		void CountSentModuleMessage(int moduleMessageId, int messageLength);

		// Token: 0x0600190B RID: 6411
		void RotateSendRecvCounters();

		// Token: 0x0600190C RID: 6412
		void GetLastSentRecvBytes(out int sent, out int recv);
	}
}
