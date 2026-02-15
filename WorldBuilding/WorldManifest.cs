using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Terraria.WorldBuilding
{
	// Token: 0x02000096 RID: 150
	public class WorldManifest
	{
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060016D8 RID: 5848 RVA: 0x004DC78B File Offset: 0x004DA98B
		// (set) Token: 0x060016D9 RID: 5849 RVA: 0x004DC793 File Offset: 0x004DA993
		public string Version { get; set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060016DA RID: 5850 RVA: 0x004DC79C File Offset: 0x004DA99C
		// (set) Token: 0x060016DB RID: 5851 RVA: 0x004DC7A4 File Offset: 0x004DA9A4
		public string GitSHA { get; set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060016DC RID: 5852 RVA: 0x004DC7B0 File Offset: 0x004DA9B0
		public uint? FinalHash
		{
			get
			{
				if (this.GenPassResults.Count <= 0)
				{
					return null;
				}
				return this.GenPassResults[this.GenPassResults.Count - 1].Hash;
			}
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x004DC7F4 File Offset: 0x004DA9F4
		public static WorldManifest Deserialize(string json)
		{
			try
			{
				if (!string.IsNullOrEmpty(json))
				{
					return JsonConvert.DeserializeObject<WorldManifest>(json, WorldManifest.SerializerSettings);
				}
			}
			catch (Exception value)
			{
				Trace.WriteLine(value);
			}
			return new WorldManifest();
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x004DC838 File Offset: 0x004DAA38
		public string Serialize()
		{
			return JsonConvert.SerializeObject(this, WorldManifest.SerializerSettings);
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x004DC845 File Offset: 0x004DAA45
		public WorldManifest Clone()
		{
			return JsonConvert.DeserializeObject<WorldManifest>(JsonConvert.SerializeObject(this, WorldManifest.SerializerSettings), WorldManifest.SerializerSettings);
		}

		// Token: 0x040011AD RID: 4525
		public List<GenPassResult> GenPassResults = new List<GenPassResult>();

		// Token: 0x040011AE RID: 4526
		public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
		{
			TypeNameHandling = 4
		};
	}
}
