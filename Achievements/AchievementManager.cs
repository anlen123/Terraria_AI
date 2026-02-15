using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using Terraria.Social;
using Terraria.UI;
using Terraria.Utilities;

namespace Terraria.Achievements
{
	// Token: 0x020005E1 RID: 1505
	public class AchievementManager
	{
		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06003B0C RID: 15116 RVA: 0x00658DB4 File Offset: 0x00656FB4
		// (remove) Token: 0x06003B0D RID: 15117 RVA: 0x00658DEC File Offset: 0x00656FEC
		public event Achievement.AchievementCompleted OnAchievementCompleted;

		// Token: 0x06003B0E RID: 15118 RVA: 0x00658E24 File Offset: 0x00657024
		public AchievementManager()
		{
			if (SocialAPI.Achievements != null)
			{
				this._savePath = SocialAPI.Achievements.GetSavePath();
				this._isCloudSave = true;
				this._cryptoKey = SocialAPI.Achievements.GetEncryptionKey();
				return;
			}
			this._savePath = Main.SavePath + Path.DirectorySeparatorChar.ToString() + "achievements.dat";
			this._isCloudSave = false;
			this._cryptoKey = Encoding.ASCII.GetBytes("RELOGIC-TERRARIA");
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x00658EC5 File Offset: 0x006570C5
		public void Save()
		{
			FileUtilities.ProtectedInvoke(delegate
			{
				this.Save(this._savePath, this._isCloudSave);
			});
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x00658ED8 File Offset: 0x006570D8
		private void Save(string path, bool cloud)
		{
			object ioLock = AchievementManager._ioLock;
			lock (ioLock)
			{
				if (SocialAPI.Achievements != null)
				{
					SocialAPI.Achievements.StoreStats();
				}
				try
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, new RijndaelManaged().CreateEncryptor(this._cryptoKey, this._cryptoKey), CryptoStreamMode.Write))
						{
							using (BsonWriter bsonWriter = new BsonWriter(cryptoStream))
							{
								JsonSerializer.Create(this._serializerSettings).Serialize(bsonWriter, this._achievements);
								bsonWriter.Flush();
								cryptoStream.FlushFinalBlock();
								FileUtilities.WriteAllBytes(path, memoryStream.ToArray(), cloud);
							}
						}
					}
				}
				catch (Exception exception)
				{
					FancyErrorPrinter.ShowFileSavingFailError(exception, this._savePath);
				}
			}
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x00658FE8 File Offset: 0x006571E8
		public List<Achievement> CreateAchievementsList()
		{
			return this._achievements.Values.ToList<Achievement>();
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x00658FFA File Offset: 0x006571FA
		public void Load()
		{
			this.Load(this._savePath, this._isCloudSave);
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x00659010 File Offset: 0x00657210
		private void Load(string path, bool cloud)
		{
			bool flag = false;
			object ioLock = AchievementManager._ioLock;
			lock (ioLock)
			{
				if (!FileUtilities.Exists(path, cloud))
				{
					return;
				}
				byte[] buffer = FileUtilities.ReadAllBytes(path, cloud);
				Dictionary<string, AchievementManager.StoredAchievement> dictionary = null;
				try
				{
					using (MemoryStream memoryStream = new MemoryStream(buffer))
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, new RijndaelManaged().CreateDecryptor(this._cryptoKey, this._cryptoKey), CryptoStreamMode.Read))
						{
							using (BsonReader bsonReader = new BsonReader(cryptoStream))
							{
								dictionary = JsonSerializer.Create(this._serializerSettings).Deserialize<Dictionary<string, AchievementManager.StoredAchievement>>(bsonReader);
							}
						}
					}
				}
				catch (Exception)
				{
					FileUtilities.Delete(path, cloud, false);
					return;
				}
				if (dictionary == null)
				{
					return;
				}
				foreach (KeyValuePair<string, AchievementManager.StoredAchievement> keyValuePair in dictionary)
				{
					if (this._achievements.ContainsKey(keyValuePair.Key))
					{
						this._achievements[keyValuePair.Key].Load(keyValuePair.Value.Conditions);
					}
				}
				if (SocialAPI.Achievements != null)
				{
					foreach (KeyValuePair<string, Achievement> keyValuePair2 in this._achievements)
					{
						if (keyValuePair2.Value.IsCompleted && !SocialAPI.Achievements.IsAchievementCompleted(keyValuePair2.Key))
						{
							flag = true;
							keyValuePair2.Value.ClearProgress();
						}
					}
				}
			}
			if (flag)
			{
				this.Save();
			}
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x0065925C File Offset: 0x0065745C
		public bool Clear(string achievementName)
		{
			if (SocialAPI.Achievements != null)
			{
				return false;
			}
			Achievement achievement;
			if (!this._achievements.TryGetValue(achievementName, out achievement))
			{
				return false;
			}
			achievement.ClearProgress();
			this.Save();
			return true;
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x00659294 File Offset: 0x00657494
		public void ClearAll()
		{
			if (SocialAPI.Achievements != null)
			{
				return;
			}
			foreach (KeyValuePair<string, Achievement> keyValuePair in this._achievements)
			{
				keyValuePair.Value.ClearProgress();
			}
			this.Save();
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x006592FC File Offset: 0x006574FC
		private void AchievementCompleted(Achievement achievement)
		{
			this.Save();
			if (this.OnAchievementCompleted != null)
			{
				this.OnAchievementCompleted(achievement);
			}
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x00659318 File Offset: 0x00657518
		public void Register(Achievement achievement)
		{
			this._achievements.Add(achievement.Name, achievement);
			achievement.OnCompleted += this.AchievementCompleted;
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x0065933E File Offset: 0x0065753E
		public void RegisterIconIndex(string achievementName, int iconIndex)
		{
			this._achievementIconIndexes.Add(achievementName, iconIndex);
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x0065934D File Offset: 0x0065754D
		public void RegisterAchievementCategory(string achievementName, AchievementCategory category)
		{
			this._achievements[achievementName].SetCategory(category);
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x00659364 File Offset: 0x00657564
		public Achievement GetAchievement(string achievementName)
		{
			Achievement result;
			if (this._achievements.TryGetValue(achievementName, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x00659384 File Offset: 0x00657584
		public T GetCondition<T>(string achievementName, string conditionName) where T : AchievementCondition
		{
			return this.GetCondition(achievementName, conditionName) as T;
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x00659398 File Offset: 0x00657598
		public AchievementCondition GetCondition(string achievementName, string conditionName)
		{
			Achievement achievement;
			if (this._achievements.TryGetValue(achievementName, out achievement))
			{
				return achievement.GetCondition(conditionName);
			}
			return null;
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x006593C0 File Offset: 0x006575C0
		public int GetIconIndex(string achievementName)
		{
			int result;
			if (this._achievementIconIndexes.TryGetValue(achievementName, out result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x04005E32 RID: 24114
		private string _savePath;

		// Token: 0x04005E33 RID: 24115
		private bool _isCloudSave;

		// Token: 0x04005E35 RID: 24117
		private Dictionary<string, Achievement> _achievements = new Dictionary<string, Achievement>();

		// Token: 0x04005E36 RID: 24118
		private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings();

		// Token: 0x04005E37 RID: 24119
		private byte[] _cryptoKey;

		// Token: 0x04005E38 RID: 24120
		private Dictionary<string, int> _achievementIconIndexes = new Dictionary<string, int>();

		// Token: 0x04005E39 RID: 24121
		private static object _ioLock = new object();

		// Token: 0x020009CC RID: 2508
		private class StoredAchievement
		{
			// Token: 0x040076B9 RID: 30393
			[JsonProperty]
			public Dictionary<string, JObject> Conditions;
		}
	}
}
