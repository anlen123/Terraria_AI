using System;
using System.Collections.Generic;
using rail;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000124 RID: 292
	public class CloudSocialModule : CloudSocialModule
	{
		// Token: 0x06001B69 RID: 7017 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Initialize()
		{
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Shutdown()
		{
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x004FA2C4 File Offset: 0x004F84C4
		public override IEnumerable<string> GetFiles()
		{
			object obj = this.ioLock;
			IEnumerable<string> result;
			lock (obj)
			{
				uint fileCount = rail_api.RailFactory().RailStorageHelper().GetFileCount();
				List<string> list = new List<string>((int)fileCount);
				ulong num = 0UL;
				for (uint num2 = 0U; num2 < fileCount; num2 += 1U)
				{
					string item;
					rail_api.RailFactory().RailStorageHelper().GetFileNameAndSize(num2, ref item, ref num);
					list.Add(item);
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x004FA34C File Offset: 0x004F854C
		public override bool Write(string path, byte[] data, int length)
		{
			object obj = this.ioLock;
			bool result;
			lock (obj)
			{
				bool flag2 = true;
				IRailFile railFile;
				if (rail_api.RailFactory().RailStorageHelper().IsFileExist(path))
				{
					railFile = rail_api.RailFactory().RailStorageHelper().OpenFile(path);
				}
				else
				{
					railFile = rail_api.RailFactory().RailStorageHelper().CreateFile(path);
				}
				if (railFile != null)
				{
					railFile.Write(data, (uint)length);
					railFile.Close();
				}
				else
				{
					flag2 = false;
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x004FA3DC File Offset: 0x004F85DC
		public override int GetFileSize(string path)
		{
			object obj = this.ioLock;
			int result;
			lock (obj)
			{
				IRailFile railFile = rail_api.RailFactory().RailStorageHelper().OpenFile(path);
				if (railFile != null)
				{
					int size = (int)railFile.GetSize();
					railFile.Close();
					result = size;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x004FA444 File Offset: 0x004F8644
		public override void Read(string path, byte[] buffer, int size)
		{
			object obj = this.ioLock;
			lock (obj)
			{
				IRailFile railFile = rail_api.RailFactory().RailStorageHelper().OpenFile(path);
				if (railFile != null)
				{
					railFile.Read(buffer, (uint)size);
					railFile.Close();
				}
			}
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x004FA4A4 File Offset: 0x004F86A4
		public override bool HasFile(string path)
		{
			object obj = this.ioLock;
			bool result;
			lock (obj)
			{
				result = rail_api.RailFactory().RailStorageHelper().IsFileExist(path);
			}
			return result;
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x004FA4F0 File Offset: 0x004F86F0
		public override bool Delete(string path)
		{
			object obj = this.ioLock;
			bool result;
			lock (obj)
			{
				RailResult railResult = rail_api.RailFactory().RailStorageHelper().RemoveFile(path);
				result = (railResult == 0);
			}
			return result;
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x004FA544 File Offset: 0x004F8744
		public override bool Forget(string path)
		{
			return this.Delete(path);
		}

		// Token: 0x04001569 RID: 5481
		private object ioLock = new object();
	}
}
