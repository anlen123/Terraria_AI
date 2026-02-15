using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Terraria.Utilities;

namespace Terraria.Testing
{
	// Token: 0x02000111 RID: 273
	public static class GitStatus
	{
		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x004F74A4 File Offset: 0x004F56A4
		public static string GitSHA
		{
			get
			{
				GitStatus.Init();
				return GitStatus._gitSHA;
			}
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x004F74B0 File Offset: 0x004F56B0
		private static void Init()
		{
			if (GitStatus._init)
			{
				return;
			}
			GitStatus._init = true;
			if (!GitStatus.HasGitFolder())
			{
				return;
			}
			try
			{
				GitStatus._gitSHA = GitStatus.GitRevParse();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "git command failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x004F7508 File Offset: 0x004F5708
		private static string GitRevParse()
		{
			string result;
			using (Process process = new Process())
			{
				process.StartInfo = new ProcessStartInfo("git", "rev-parse HEAD")
				{
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				};
				process.Start();
				string text = process.StandardOutput.ReadToEnd().Trim();
				if (!Regex.IsMatch(text, "^[0-9a-f]+$"))
				{
					throw new Exception(text);
				}
				result = text;
			}
			return result;
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x004F7590 File Offset: 0x004F5790
		private static bool HasGitFolder()
		{
			try
			{
				string directoryName = Path.GetDirectoryName(Path.GetFullPath("."));
				while (!Directory.Exists(Path.Combine(directoryName, ".git")))
				{
					if ((directoryName = Path.GetDirectoryName(directoryName)) == null)
					{
						return false;
					}
				}
				return true;
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x04001500 RID: 5376
		private static string _gitSHA = "";

		// Token: 0x04001501 RID: 5377
		private static bool _init;
	}
}
