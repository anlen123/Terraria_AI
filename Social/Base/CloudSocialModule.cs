using System;
using System.Collections.Generic;
using Terraria.IO;

namespace Terraria.Social.Base
{
	// Token: 0x02000161 RID: 353
	public abstract class CloudSocialModule : ISocialModule
	{
		// Token: 0x06001D6A RID: 7530 RVA: 0x00500D0D File Offset: 0x004FEF0D
		public virtual void BindTo(Preferences preferences)
		{
			preferences.OnSave += this.Configuration_OnSave;
			preferences.OnLoad += this.Configuration_OnLoad;
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x00500D33 File Offset: 0x004FEF33
		private void Configuration_OnLoad(Preferences preferences)
		{
			this.EnabledByDefault = preferences.Get<bool>("CloudSavingDefault", false);
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x00500D47 File Offset: 0x004FEF47
		private void Configuration_OnSave(Preferences preferences)
		{
			preferences.Put("CloudSavingDefault", this.EnabledByDefault);
		}

		// Token: 0x06001D6D RID: 7533
		public abstract void Initialize();

		// Token: 0x06001D6E RID: 7534
		public abstract void Shutdown();

		// Token: 0x06001D6F RID: 7535
		public abstract IEnumerable<string> GetFiles();

		// Token: 0x06001D70 RID: 7536
		public abstract bool Write(string path, byte[] data, int length);

		// Token: 0x06001D71 RID: 7537
		public abstract void Read(string path, byte[] buffer, int length);

		// Token: 0x06001D72 RID: 7538
		public abstract bool HasFile(string path);

		// Token: 0x06001D73 RID: 7539
		public abstract int GetFileSize(string path);

		// Token: 0x06001D74 RID: 7540
		public abstract bool Delete(string path);

		// Token: 0x06001D75 RID: 7541
		public abstract bool Forget(string path);

		// Token: 0x06001D76 RID: 7542 RVA: 0x00500D60 File Offset: 0x004FEF60
		public byte[] Read(string path)
		{
			byte[] array = new byte[this.GetFileSize(path)];
			this.Read(path, array, array.Length);
			return array;
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x00500D86 File Offset: 0x004FEF86
		public void Read(string path, byte[] buffer)
		{
			this.Read(path, buffer, buffer.Length);
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x00500D93 File Offset: 0x004FEF93
		public bool Write(string path, byte[] data)
		{
			return this.Write(path, data, data.Length);
		}

		// Token: 0x0400163C RID: 5692
		public bool EnabledByDefault;
	}
}
