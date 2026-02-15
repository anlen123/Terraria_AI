using System;
using System.Windows.Forms;

namespace Terraria.Utilities
{
	// Token: 0x020000C5 RID: 197
	public static class MessageBox
	{
		// Token: 0x060017D6 RID: 6102 RVA: 0x004DFD33 File Offset: 0x004DDF33
		public static DialogResult Show(string message, string title, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
		{
			return (DialogResult)MessageBox.Show(message, title, (MessageBoxButtons)buttons, (MessageBoxIcon)icon);
		}
	}
}
