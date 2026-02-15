using System;

namespace Terraria.Utilities
{
	// Token: 0x020000D0 RID: 208
	public static class PlatformUtilities
	{
		// Token: 0x0600181F RID: 6175 RVA: 0x004E099B File Offset: 0x004DEB9B
		public static void SavePng(string path, int width, int height, byte[] data)
		{
			throw new NotSupportedException("Use Bitmap to save png images on windows");
		}
	}
}
