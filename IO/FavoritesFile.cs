using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Terraria.UI;
using Terraria.Utilities;

namespace Terraria.IO
{
	// Token: 0x0200006D RID: 109
	public class FavoritesFile
	{
		// Token: 0x060014B1 RID: 5297 RVA: 0x004BB783 File Offset: 0x004B9983
		public FavoritesFile(string path, bool isCloud)
		{
			this.Path = path;
			this.IsCloudSave = isCloud;
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x004BB7B4 File Offset: 0x004B99B4
		public void SaveFavorite(FileData fileData)
		{
			if (!this._data.ContainsKey(fileData.Type))
			{
				this._data.Add(fileData.Type, new Dictionary<string, bool>());
			}
			this._data[fileData.Type][fileData.GetFileName(true)] = fileData.IsFavorite;
			this.Save();
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x004BB813 File Offset: 0x004B9A13
		public void ClearEntry(FileData fileData)
		{
			if (!this._data.ContainsKey(fileData.Type))
			{
				return;
			}
			this._data[fileData.Type].Remove(fileData.GetFileName(true));
			this.Save();
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x004BB850 File Offset: 0x004B9A50
		public bool IsFavorite(FileData fileData)
		{
			if (!this._data.ContainsKey(fileData.Type))
			{
				return false;
			}
			string fileName = fileData.GetFileName(true);
			bool flag;
			return this._data[fileData.Type].TryGetValue(fileName, out flag) && flag;
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x004BB898 File Offset: 0x004B9A98
		public void Save()
		{
			try
			{
				string s = JsonConvert.SerializeObject(this._data, 1);
				byte[] bytes = this._ourEncoder.GetBytes(s);
				FileUtilities.WriteAllBytes(this.Path, bytes, this.IsCloudSave);
			}
			catch (Exception exception)
			{
				FancyErrorPrinter.ShowFileSavingFailError(exception, this.Path);
				throw;
			}
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x004BB8F4 File Offset: 0x004B9AF4
		public void Load()
		{
			if (!FileUtilities.Exists(this.Path, this.IsCloudSave))
			{
				this._data.Clear();
				return;
			}
			try
			{
				byte[] bytes = FileUtilities.ReadAllBytes(this.Path, this.IsCloudSave);
				string @string;
				try
				{
					@string = this._ourEncoder.GetString(bytes);
				}
				catch
				{
					@string = Encoding.ASCII.GetString(bytes);
				}
				this._data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, bool>>>(@string);
				if (this._data == null)
				{
					this._data = new Dictionary<string, Dictionary<string, bool>>();
				}
			}
			catch (Exception)
			{
				Console.WriteLine("Unable to load favorites.json file ({0} : {1})", this.Path, this.IsCloudSave ? "Cloud Save" : "Local Save");
			}
		}

		// Token: 0x04001077 RID: 4215
		public readonly string Path;

		// Token: 0x04001078 RID: 4216
		public readonly bool IsCloudSave;

		// Token: 0x04001079 RID: 4217
		private Dictionary<string, Dictionary<string, bool>> _data = new Dictionary<string, Dictionary<string, bool>>();

		// Token: 0x0400107A RID: 4218
		private UTF8Encoding _ourEncoder = new UTF8Encoding(true, true);
	}
}
