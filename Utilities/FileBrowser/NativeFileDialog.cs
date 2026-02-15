using System;
using System.Linq;

namespace Terraria.Utilities.FileBrowser
{
	// Token: 0x020000D9 RID: 217
	public class NativeFileDialog : IFileBrowser
	{
		// Token: 0x06001877 RID: 6263 RVA: 0x004E1E8C File Offset: 0x004E008C
		public string OpenFilePanel(string title, ExtensionFilter[] extensions)
		{
			string[] value = extensions.SelectMany((ExtensionFilter x) => x.Extensions).ToArray<string>();
			string result;
			if (nativefiledialog.NFD_OpenDialog(string.Join(",", value), null, out result) == nativefiledialog.nfdresult_t.NFD_OKAY)
			{
				return result;
			}
			return null;
		}
	}
}
