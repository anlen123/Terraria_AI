using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Terraria.Social.Base
{
	// Token: 0x02000156 RID: 342
	public abstract class AWorkshopEntry
	{
		// Token: 0x06001D2E RID: 7470 RVA: 0x005007A0 File Offset: 0x004FE9A0
		public static string ReadHeader(string jsonText)
		{
			JToken jtoken;
			if (!JObject.Parse(jsonText).TryGetValue("ContentType", ref jtoken))
			{
				return null;
			}
			return jtoken.ToObject<string>();
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x005007CC File Offset: 0x004FE9CC
		protected static string CreateHeaderJson(string contentTypeName, ulong workshopEntryId, string[] tags, WorkshopItemPublicSettingId publicity, string previewImagePath)
		{
			new JObject();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["WorkshopPublishedVersion"] = 1;
			dictionary["ContentType"] = contentTypeName;
			dictionary["SteamEntryId"] = workshopEntryId;
			if (tags != null && tags.Length != 0)
			{
				dictionary["Tags"] = JArray.FromObject(tags);
			}
			dictionary["Publicity"] = publicity;
			return JsonConvert.SerializeObject(dictionary, AWorkshopEntry.SerializerSettings);
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x00500848 File Offset: 0x004FEA48
		public static bool TryReadingManifest(string filePath, out FoundWorkshopEntryInfo info)
		{
			info = null;
			if (!File.Exists(filePath))
			{
				return false;
			}
			string text = File.ReadAllText(filePath);
			info = new FoundWorkshopEntryInfo();
			Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(text, AWorkshopEntry.SerializerSettings);
			if (dictionary == null)
			{
				return false;
			}
			if (!AWorkshopEntry.TryGet<ulong>(dictionary, "SteamEntryId", out info.workshopEntryId))
			{
				return false;
			}
			int publishedVersion;
			if (!AWorkshopEntry.TryGet<int>(dictionary, "WorkshopPublishedVersion", out publishedVersion))
			{
				publishedVersion = 1;
			}
			info.publishedVersion = publishedVersion;
			JArray jarray;
			if (AWorkshopEntry.TryGet<JArray>(dictionary, "Tags", out jarray))
			{
				info.tags = jarray.ToObject<string[]>();
			}
			int publicity;
			if (AWorkshopEntry.TryGet<int>(dictionary, "Publicity", out publicity))
			{
				info.publicity = (WorkshopItemPublicSettingId)publicity;
			}
			AWorkshopEntry.TryGet<string>(dictionary, "PreviewImagePath", out info.previewImagePath);
			return true;
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x005008F8 File Offset: 0x004FEAF8
		protected static bool TryGet<T>(Dictionary<string, object> dict, string name, out T outputValue)
		{
			outputValue = default(T);
			bool result;
			try
			{
				object obj;
				if (dict.TryGetValue(name, out obj))
				{
					if (obj is T)
					{
						outputValue = (T)((object)obj);
						result = true;
					}
					else if (obj is JObject)
					{
						outputValue = JsonConvert.DeserializeObject<T>(((JObject)obj).ToString());
						result = true;
					}
					else
					{
						outputValue = (T)((object)Convert.ChangeType(obj, typeof(T)));
						result = true;
					}
				}
				else
				{
					result = false;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0400161E RID: 5662
		public const int CurrentWorkshopPublishVersion = 1;

		// Token: 0x0400161F RID: 5663
		public const string ContentTypeName_World = "World";

		// Token: 0x04001620 RID: 5664
		public const string ContentTypeName_ResourcePack = "ResourcePack";

		// Token: 0x04001621 RID: 5665
		protected const string HeaderFileName = "Workshop.json";

		// Token: 0x04001622 RID: 5666
		protected const string ContentTypeJsonCategoryField = "ContentType";

		// Token: 0x04001623 RID: 5667
		protected const string WorkshopPublishedVersionField = "WorkshopPublishedVersion";

		// Token: 0x04001624 RID: 5668
		protected const string WorkshopEntryField = "SteamEntryId";

		// Token: 0x04001625 RID: 5669
		protected const string TagsField = "Tags";

		// Token: 0x04001626 RID: 5670
		protected const string PreviewImageField = "PreviewImagePath";

		// Token: 0x04001627 RID: 5671
		protected const string PublictyField = "Publicity";

		// Token: 0x04001628 RID: 5672
		protected static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
		{
			TypeNameHandling = 0,
			MetadataPropertyHandling = 1,
			Formatting = 1
		};
	}
}
