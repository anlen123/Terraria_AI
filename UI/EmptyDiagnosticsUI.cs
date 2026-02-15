using System;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.UI
{
	// Token: 0x020000F0 RID: 240
	public class EmptyDiagnosticsUI : INetDiagnosticsUI
	{
		// Token: 0x0600190D RID: 6413 RVA: 0x00009E06 File Offset: 0x00008006
		public void Reset()
		{
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x00009E06 File Offset: 0x00008006
		public void CountReadMessage(int messageId, int messageLength)
		{
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x00009E06 File Offset: 0x00008006
		public void CountSentMessage(int messageId, int messageLength)
		{
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x00009E06 File Offset: 0x00008006
		public void CountReadModuleMessage(int moduleMessageId, int messageLength)
		{
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x00009E06 File Offset: 0x00008006
		public void CountSentModuleMessage(int moduleMessageId, int messageLength)
		{
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x00009E06 File Offset: 0x00008006
		public void Draw(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x00009E06 File Offset: 0x00008006
		public void RotateSendRecvCounters()
		{
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x004E684C File Offset: 0x004E4A4C
		public void GetLastSentRecvBytes(out int sent, out int recv)
		{
			sent = 0;
			recv = 0;
		}
	}
}
