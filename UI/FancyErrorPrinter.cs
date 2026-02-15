using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ReLogic.Content;
using Terraria.Utilities;

namespace Terraria.UI
{
	// Token: 0x020000E7 RID: 231
	public class FancyErrorPrinter
	{
		// Token: 0x060018D4 RID: 6356 RVA: 0x004E4FC4 File Offset: 0x004E31C4
		public static void ShowFailedToLoadAssetError(Exception exception, string filePath)
		{
			bool flag = false;
			if (exception is UnauthorizedAccessException)
			{
				flag = true;
			}
			if (exception is FileNotFoundException)
			{
				flag = true;
			}
			if (exception is DirectoryNotFoundException)
			{
				flag = true;
			}
			if (exception is AssetLoadException)
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Failed to load asset: \"" + filePath.Replace("/", "\\") + "\"!");
			List<string> list = new List<string>();
			list.Add("Try verifying integrity of game files via Steam, the asset may be missing.");
			list.Add("If you are using an Anti-virus, please make sure it does not block Terraria in any way.");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Suggestions:");
			FancyErrorPrinter.AppendSuggestions(stringBuilder, list);
			stringBuilder.AppendLine();
			FancyErrorPrinter.IncludeOriginalMessage(stringBuilder, exception);
			FancyErrorPrinter.ShowTheBox(stringBuilder.ToString());
			Console.WriteLine(stringBuilder.ToString());
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x004E5084 File Offset: 0x004E3284
		public static void ShowFileSavingFailError(Exception exception, string filePath)
		{
			bool flag = false;
			if (exception is UnauthorizedAccessException)
			{
				flag = true;
			}
			if (exception is FileNotFoundException)
			{
				flag = true;
			}
			if (exception is DirectoryNotFoundException)
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Failed to create the file: \"" + filePath.Replace("/", "\\") + "\"!");
			List<string> list = new List<string>();
			list.Add("If you are using an Anti-virus, please make sure it does not block Terraria in any way.");
			list.Add("Try making sure your `Documents/My Games/Terraria` folder is not set to 'read-only'.");
			list.Add("Try verifying integrity of game files via Steam.");
			if (filePath.ToLower().Contains("onedrive"))
			{
				list.Add("Try updating OneDrive.");
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Suggestions:");
			FancyErrorPrinter.AppendSuggestions(stringBuilder, list);
			stringBuilder.AppendLine();
			FancyErrorPrinter.IncludeOriginalMessage(stringBuilder, exception);
			FancyErrorPrinter.ShowTheBox(stringBuilder.ToString());
			Console.WriteLine(stringBuilder.ToString());
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x004E5164 File Offset: 0x004E3364
		public static void ShowDirectoryCreationFailError(Exception exception, string folderPath)
		{
			bool flag = false;
			if (exception is UnauthorizedAccessException)
			{
				flag = true;
			}
			if (exception is FileNotFoundException)
			{
				flag = true;
			}
			if (exception is DirectoryNotFoundException)
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Failed to create the folder: \"" + folderPath.Replace("/", "\\") + "\"!");
			List<string> list = new List<string>();
			list.Add("If you are using an Anti-virus, please make sure it does not block Terraria in any way.");
			list.Add("Try making sure your `Documents/My Games/Terraria` folder is not set to 'read-only'.");
			list.Add("Try verifying integrity of game files via Steam.");
			if (folderPath.ToLower().Contains("onedrive"))
			{
				list.Add("Try updating OneDrive.");
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Suggestions:");
			FancyErrorPrinter.AppendSuggestions(stringBuilder, list);
			stringBuilder.AppendLine();
			FancyErrorPrinter.IncludeOriginalMessage(stringBuilder, exception);
			FancyErrorPrinter.ShowTheBox(stringBuilder.ToString());
			Console.WriteLine(exception);
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x004E523C File Offset: 0x004E343C
		private static void IncludeOriginalMessage(StringBuilder text, Exception exception)
		{
			text.AppendLine("The original Error below");
			text.Append(exception);
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x004E5254 File Offset: 0x004E3454
		private static void AppendSuggestions(StringBuilder text, List<string> suggestions)
		{
			for (int i = 0; i < suggestions.Count; i++)
			{
				string str = suggestions[i];
				text.AppendLine((i + 1).ToString() + ". " + str);
			}
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x004E5297 File Offset: 0x004E3497
		private static void ShowTheBox(string preparedMessage)
		{
			if (!Main.dedServ)
			{
				MessageBox.Show(preparedMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
