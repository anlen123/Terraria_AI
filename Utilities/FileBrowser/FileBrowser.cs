using System;

namespace Terraria.Utilities.FileBrowser
{
	// Token: 0x020000DC RID: 220
	public class FileBrowser
	{
		// Token: 0x0600187C RID: 6268 RVA: 0x004E1EFC File Offset: 0x004E00FC
		public static string OpenFilePanel(string title, string extension)
		{
			ExtensionFilter[] array;
			if (!string.IsNullOrEmpty(extension))
			{
				(array = new ExtensionFilter[1])[0] = new ExtensionFilter("", new string[]
				{
					extension
				});
			}
			else
			{
				array = null;
			}
			ExtensionFilter[] extensions = array;
			return FileBrowser.OpenFilePanel(title, extensions);
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x004E1F3D File Offset: 0x004E013D
		public static string OpenFilePanel(string title, ExtensionFilter[] extensions)
		{
			return FileBrowser._platformWrapper.OpenFilePanel(title, extensions);
		}

		// Token: 0x040012C5 RID: 4805
		private static IFileBrowser _platformWrapper = new NativeFileDialog();
	}
}
