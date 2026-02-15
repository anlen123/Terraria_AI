using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using Terraria.Localization;

namespace Terraria.IO
{
	// Token: 0x02000073 RID: 115
	public class Preferences
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060014FE RID: 5374 RVA: 0x004BC8CC File Offset: 0x004BAACC
		// (remove) Token: 0x060014FF RID: 5375 RVA: 0x004BC904 File Offset: 0x004BAB04
		public event Action<Preferences> OnSave;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06001500 RID: 5376 RVA: 0x004BC93C File Offset: 0x004BAB3C
		// (remove) Token: 0x06001501 RID: 5377 RVA: 0x004BC974 File Offset: 0x004BAB74
		public event Action<Preferences> OnLoad;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06001502 RID: 5378 RVA: 0x004BC9AC File Offset: 0x004BABAC
		// (remove) Token: 0x06001503 RID: 5379 RVA: 0x004BC9E4 File Offset: 0x004BABE4
		public event Preferences.TextProcessAction OnProcessText;

		// Token: 0x06001504 RID: 5380 RVA: 0x004BCA1C File Offset: 0x004BAC1C
		public Preferences(string path, bool parseAllTypes = false, bool useBson = false)
		{
			this._path = path;
			this.UseBson = useBson;
			if (parseAllTypes)
			{
				this._serializerSettings = new JsonSerializerSettings
				{
					TypeNameHandling = 4,
					MetadataPropertyHandling = 1,
					Formatting = 1
				};
				return;
			}
			this._serializerSettings = new JsonSerializerSettings
			{
				Formatting = 1
			};
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x004BCA8C File Offset: 0x004BAC8C
		public bool Load()
		{
			object @lock = this._lock;
			bool result;
			lock (@lock)
			{
				if (!File.Exists(this._path))
				{
					result = false;
				}
				else
				{
					try
					{
						if (!this.UseBson)
						{
							string text = File.ReadAllText(this._path);
							this._data = JsonConvert.DeserializeObject<Dictionary<string, object>>(text, this._serializerSettings);
						}
						else
						{
							using (FileStream fileStream = File.OpenRead(this._path))
							{
								using (BsonReader bsonReader = new BsonReader(fileStream))
								{
									JsonSerializer jsonSerializer = JsonSerializer.Create(this._serializerSettings);
									this._data = jsonSerializer.Deserialize<Dictionary<string, object>>(bsonReader);
								}
							}
						}
						if (this._data == null)
						{
							this._data = new Dictionary<string, object>();
						}
						if (this.OnLoad != null)
						{
							this.OnLoad(this);
						}
						result = true;
					}
					catch (Exception)
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x004BCBA4 File Offset: 0x004BADA4
		public bool Save(bool canCreateFile = true)
		{
			object @lock = this._lock;
			bool result;
			lock (@lock)
			{
				try
				{
					if (this.OnSave != null)
					{
						this.OnSave(this);
					}
					if (!canCreateFile && !File.Exists(this._path))
					{
						return false;
					}
					Directory.GetParent(this._path).Create();
					if (File.Exists(this._path))
					{
						File.SetAttributes(this._path, FileAttributes.Normal);
					}
					if (!this.UseBson)
					{
						string contents = JsonConvert.SerializeObject(this._data, this._serializerSettings);
						if (this.OnProcessText != null)
						{
							this.OnProcessText(ref contents);
						}
						File.WriteAllText(this._path, contents);
						File.SetAttributes(this._path, FileAttributes.Normal);
					}
					else
					{
						using (FileStream fileStream = File.Create(this._path))
						{
							using (BsonWriter bsonWriter = new BsonWriter(fileStream))
							{
								File.SetAttributes(this._path, FileAttributes.Normal);
								JsonSerializer.Create(this._serializerSettings).Serialize(bsonWriter, this._data);
							}
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(Language.GetTextValue("Error.UnableToWritePreferences", this._path));
					Console.WriteLine(ex.ToString());
					return false;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x004BCD5C File Offset: 0x004BAF5C
		public void Clear()
		{
			this._data.Clear();
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x004BCD6C File Offset: 0x004BAF6C
		public void Put(string name, object value)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._data[name] = value;
				if (this.AutoSave)
				{
					this.Save(true);
				}
			}
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x004BCDC4 File Offset: 0x004BAFC4
		public bool Contains(string name)
		{
			object @lock = this._lock;
			bool result;
			lock (@lock)
			{
				result = this._data.ContainsKey(name);
			}
			return result;
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x004BCE0C File Offset: 0x004BB00C
		public T Get<T>(string name, T defaultValue)
		{
			object @lock = this._lock;
			T result;
			lock (@lock)
			{
				try
				{
					object obj;
					if (this._data.TryGetValue(name, out obj))
					{
						if (obj is T)
						{
							result = (T)((object)obj);
						}
						else if (obj is JObject)
						{
							result = JsonConvert.DeserializeObject<T>(((JObject)obj).ToString());
						}
						else if (typeof(T).IsEnum)
						{
							result = (T)((object)Convert.ChangeType(obj, Enum.GetUnderlyingType(typeof(T))));
						}
						else
						{
							result = (T)((object)Convert.ChangeType(obj, typeof(T)));
						}
					}
					else
					{
						result = defaultValue;
					}
				}
				catch
				{
					result = defaultValue;
				}
			}
			return result;
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x004BCEDC File Offset: 0x004BB0DC
		public void Get<T>(string name, ref T currentValue)
		{
			currentValue = this.Get<T>(name, currentValue);
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x004BCEF1 File Offset: 0x004BB0F1
		public List<string> GetAllKeys()
		{
			return this._data.Keys.ToList<string>();
		}

		// Token: 0x040010AC RID: 4268
		private Dictionary<string, object> _data = new Dictionary<string, object>();

		// Token: 0x040010AD RID: 4269
		private readonly string _path;

		// Token: 0x040010AE RID: 4270
		private readonly JsonSerializerSettings _serializerSettings;

		// Token: 0x040010AF RID: 4271
		public readonly bool UseBson;

		// Token: 0x040010B0 RID: 4272
		private readonly object _lock = new object();

		// Token: 0x040010B1 RID: 4273
		public bool AutoSave;

		// Token: 0x02000667 RID: 1639
		// (Invoke) Token: 0x06003D9C RID: 15772
		public delegate void TextProcessAction(ref string text);
	}
}
