using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using ReLogic.OS;
using Terraria.Social;

namespace Terraria.Utilities
{
	// Token: 0x020000D7 RID: 215
	public static class FileUtilities
	{
		// Token: 0x0600185D RID: 6237 RVA: 0x004E193A File Offset: 0x004DFB3A
		public static bool Exists(string path, bool cloud)
		{
			if (cloud && SocialAPI.Cloud != null)
			{
				return SocialAPI.Cloud.HasFile(path);
			}
			return File.Exists(path);
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x004E1958 File Offset: 0x004DFB58
		public static void Delete(string path, bool cloud, bool forceDeleteFile = false)
		{
			if (cloud && SocialAPI.Cloud != null)
			{
				SocialAPI.Cloud.Delete(path);
				return;
			}
			if (forceDeleteFile)
			{
				File.Delete(path);
				return;
			}
			Platform.Get<IPathService>().MoveToRecycleBin(path);
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x004E1986 File Offset: 0x004DFB86
		public static string GetFullPath(string path, bool cloud)
		{
			if (!cloud)
			{
				return Path.GetFullPath(path);
			}
			return path;
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x004E1994 File Offset: 0x004DFB94
		public static void Copy(string source, string destination, bool cloud)
		{
			if (!cloud)
			{
				try
				{
					File.Copy(source, destination, true);
				}
				catch (IOException ex)
				{
					if (ex.GetType() != typeof(IOException))
					{
						throw;
					}
					using (FileStream fileStream = File.OpenRead(source))
					{
						using (FileStream fileStream2 = File.Create(destination))
						{
							fileStream.CopyTo(fileStream2);
						}
					}
				}
				return;
			}
			if (SocialAPI.Cloud == null)
			{
				return;
			}
			SocialAPI.Cloud.Write(destination, SocialAPI.Cloud.Read(source));
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x004E1A40 File Offset: 0x004DFC40
		public static void Move(string source, string destination, bool cloud)
		{
			if (!cloud)
			{
				try
				{
					if (File.Exists(destination))
					{
						File.Delete(destination);
					}
					File.Move(source, destination);
					return;
				}
				catch (IOException)
				{
				}
			}
			FileUtilities.Copy(source, destination, cloud);
			FileUtilities.Delete(source, cloud, true);
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x004E1A8C File Offset: 0x004DFC8C
		public static int GetFileSize(string path, bool cloud)
		{
			if (cloud && SocialAPI.Cloud != null)
			{
				return SocialAPI.Cloud.GetFileSize(path);
			}
			return (int)new FileInfo(path).Length;
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x004E1AB0 File Offset: 0x004DFCB0
		public static void Read(string path, byte[] buffer, int length, bool cloud)
		{
			if (cloud && SocialAPI.Cloud != null)
			{
				SocialAPI.Cloud.Read(path, buffer, length);
				return;
			}
			using (FileStream fileStream = File.OpenRead(path))
			{
				fileStream.Read(buffer, 0, length);
			}
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x004E1B04 File Offset: 0x004DFD04
		public static byte[] ReadAllBytes(string path, bool cloud)
		{
			if (cloud && SocialAPI.Cloud != null)
			{
				return SocialAPI.Cloud.Read(path);
			}
			return File.ReadAllBytes(path);
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x004E1B22 File Offset: 0x004DFD22
		public static bool WriteAllBytes(string path, byte[] data, bool cloud)
		{
			return FileUtilities.Write(path, data, data.Length, cloud);
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x004E1B30 File Offset: 0x004DFD30
		public static bool Write(string path, byte[] data, int length, bool cloud)
		{
			if (cloud)
			{
				return SocialAPI.Cloud != null && SocialAPI.Cloud.Write(path, data, length);
			}
			string parentFolderPath = FileUtilities.GetParentFolderPath(path, true);
			if (parentFolderPath != "")
			{
				Utils.TryCreatingDirectory(parentFolderPath);
			}
			FileUtilities.RemoveReadOnlyAttribute(path);
			using (FileStream fileStream = File.Open(path, FileMode.Create))
			{
				while (fileStream.Position < (long)length)
				{
					fileStream.Write(data, (int)fileStream.Position, Math.Min(length - (int)fileStream.Position, 2048));
				}
			}
			return true;
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x004E1BCC File Offset: 0x004DFDCC
		public static void RemoveReadOnlyAttribute(string path)
		{
			if (!File.Exists(path))
			{
				return;
			}
			try
			{
				FileAttributes fileAttributes = File.GetAttributes(path);
				if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				{
					fileAttributes &= ~FileAttributes.ReadOnly;
					File.SetAttributes(path, fileAttributes);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x004E1C14 File Offset: 0x004DFE14
		public static bool MoveToCloud(string localPath, string cloudPath)
		{
			if (SocialAPI.Cloud == null)
			{
				return false;
			}
			bool flag = FileUtilities.WriteAllBytes(cloudPath, FileUtilities.ReadAllBytes(localPath, false), true);
			if (flag)
			{
				FileUtilities.Delete(localPath, false, false);
			}
			return flag;
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x004E1C38 File Offset: 0x004DFE38
		public static bool MoveToLocal(string cloudPath, string localPath)
		{
			if (SocialAPI.Cloud == null)
			{
				return false;
			}
			if (FileUtilities.WriteAllBytes(localPath, FileUtilities.ReadAllBytes(cloudPath, true), false))
			{
				FileUtilities.Delete(cloudPath, true, false);
			}
			return true;
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x004E1C5C File Offset: 0x004DFE5C
		public static bool CopyToLocal(string cloudPath, string localPath)
		{
			if (SocialAPI.Cloud == null)
			{
				return false;
			}
			FileUtilities.WriteAllBytes(localPath, FileUtilities.ReadAllBytes(cloudPath, true), false);
			return true;
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x004E1C78 File Offset: 0x004DFE78
		public static string GetFileName(string path, bool includeExtension = true)
		{
			Match match = FileUtilities.FileNameRegex.Match(path);
			if (match == null || match.Groups["fileName"] == null)
			{
				return "";
			}
			includeExtension &= (match.Groups["extension"] != null);
			return match.Groups["fileName"].Value + (includeExtension ? match.Groups["extension"].Value : "");
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x004E1CFC File Offset: 0x004DFEFC
		public static string GetParentFolderPath(string path, bool includeExtension = true)
		{
			Match match = FileUtilities.FileNameRegex.Match(path);
			if (match == null || match.Groups["path"] == null)
			{
				return "";
			}
			return match.Groups["path"].Value;
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x004E1D48 File Offset: 0x004DFF48
		public static void CopyFolder(string sourcePath, string destinationPath)
		{
			Directory.CreateDirectory(destinationPath);
			string[] array = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
			for (int i = 0; i < array.Length; i++)
			{
				Directory.CreateDirectory(array[i].Replace(sourcePath, destinationPath));
			}
			foreach (string text in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
			{
				File.Copy(text, text.Replace(sourcePath, destinationPath), true);
			}
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x004E1DB4 File Offset: 0x004DFFB4
		public static void ProtectedInvoke(Action action)
		{
			bool isBackground = Thread.CurrentThread.IsBackground;
			try
			{
				Thread.CurrentThread.IsBackground = false;
				action();
			}
			finally
			{
				Thread.CurrentThread.IsBackground = isBackground;
			}
		}

		// Token: 0x040012C0 RID: 4800
		private static Regex FileNameRegex = new Regex("^(?<path>.*[\\\\\\/])?(?:$|(?<fileName>.+?)(?:(?<extension>\\.[^.]*$)|$))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
	}
}
