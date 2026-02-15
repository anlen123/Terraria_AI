using System;
using System.Collections.Generic;
using Steamworks;
using Terraria.Social.Base;

namespace Terraria.Social.Steam
{
	// Token: 0x02000145 RID: 325
	public class CloudSocialModule : CloudSocialModule
	{
		// Token: 0x06001CA8 RID: 7336 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Initialize()
		{
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Shutdown()
		{
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x004FE768 File Offset: 0x004FC968
		public override IEnumerable<string> GetFiles()
		{
			object obj = this.ioLock;
			IEnumerable<string> result;
			lock (obj)
			{
				int fileCount = SteamRemoteStorage.GetFileCount();
				List<string> list = new List<string>(fileCount);
				for (int i = 0; i < fileCount; i++)
				{
					int num;
					list.Add(SteamRemoteStorage.GetFileNameAndSize(i, ref num));
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x004FE7D4 File Offset: 0x004FC9D4
		public override bool Write(string path, byte[] data, int length)
		{
			object obj = this.ioLock;
			bool result;
			lock (obj)
			{
				UGCFileWriteStreamHandle_t ugcfileWriteStreamHandle_t = SteamRemoteStorage.FileWriteStreamOpen(path);
				bool flag2 = false;
				uint num = 0U;
				while ((ulong)num < (ulong)((long)length))
				{
					int num2 = (int)Math.Min(1024L, (long)length - (long)((ulong)num));
					Array.Copy(data, (long)((ulong)num), this.writeBuffer, 0L, (long)num2);
					if (!SteamRemoteStorage.FileWriteStreamWriteChunk(ugcfileWriteStreamHandle_t, this.writeBuffer, num2))
					{
						flag2 = true;
						break;
					}
					num += 1024U;
				}
				if (flag2)
				{
					SteamRemoteStorage.FileWriteStreamCancel(ugcfileWriteStreamHandle_t);
					result = false;
				}
				else
				{
					result = SteamRemoteStorage.FileWriteStreamClose(ugcfileWriteStreamHandle_t);
				}
			}
			return result;
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x004FE884 File Offset: 0x004FCA84
		public override int GetFileSize(string path)
		{
			object obj = this.ioLock;
			int fileSize;
			lock (obj)
			{
				fileSize = SteamRemoteStorage.GetFileSize(path);
			}
			return fileSize;
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x004FE8C8 File Offset: 0x004FCAC8
		public override void Read(string path, byte[] buffer, int size)
		{
			object obj = this.ioLock;
			lock (obj)
			{
				SteamRemoteStorage.FileRead(path, buffer, size);
			}
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x004FE90C File Offset: 0x004FCB0C
		public override bool HasFile(string path)
		{
			object obj = this.ioLock;
			bool result;
			lock (obj)
			{
				result = SteamRemoteStorage.FileExists(path);
			}
			return result;
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x004FE950 File Offset: 0x004FCB50
		public override bool Delete(string path)
		{
			object obj = this.ioLock;
			bool result;
			lock (obj)
			{
				result = SteamRemoteStorage.FileDelete(path);
			}
			return result;
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x004FE994 File Offset: 0x004FCB94
		public override bool Forget(string path)
		{
			object obj = this.ioLock;
			bool result;
			lock (obj)
			{
				result = SteamRemoteStorage.FileForget(path);
			}
			return result;
		}

		// Token: 0x040015D1 RID: 5585
		private const uint WRITE_CHUNK_SIZE = 1024U;

		// Token: 0x040015D2 RID: 5586
		private object ioLock = new object();

		// Token: 0x040015D3 RID: 5587
		private byte[] writeBuffer = new byte[1024];
	}
}
