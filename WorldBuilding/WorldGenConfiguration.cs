using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.IO;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000BE RID: 190
	public class WorldGenConfiguration : GameConfiguration
	{
		// Token: 0x060017A4 RID: 6052 RVA: 0x004DEEC0 File Offset: 0x004DD0C0
		public WorldGenConfiguration(JObject configurationRoot) : base(configurationRoot)
		{
			this._biomeRoot = (((JObject)configurationRoot["Biomes"]) ?? new JObject());
			this._passRoot = (((JObject)configurationRoot["Passes"]) ?? new JObject());
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x004DEF12 File Offset: 0x004DD112
		public T CreateBiome<T>() where T : MicroBiome, new()
		{
			return this.CreateBiome<T>(typeof(T).Name);
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x004DEF2C File Offset: 0x004DD12C
		public T CreateBiome<T>(string name) where T : MicroBiome, new()
		{
			JToken jtoken;
			if (this._biomeRoot.TryGetValue(name, ref jtoken))
			{
				return jtoken.ToObject<T>();
			}
			return Activator.CreateInstance<T>();
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x004DEF58 File Offset: 0x004DD158
		public GameConfiguration GetPassConfiguration(string name)
		{
			JToken jtoken;
			if (this._passRoot.TryGetValue(name, ref jtoken))
			{
				return new GameConfiguration((JObject)jtoken);
			}
			return new GameConfiguration(new JObject());
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x004DEF8C File Offset: 0x004DD18C
		public static WorldGenConfiguration FromEmbeddedPath(string path)
		{
			WorldGenConfiguration result;
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
			{
				using (StreamReader streamReader = new StreamReader(manifestResourceStream))
				{
					result = new WorldGenConfiguration(JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd()));
				}
			}
			return result;
		}

		// Token: 0x0400127A RID: 4730
		private readonly JObject _biomeRoot;

		// Token: 0x0400127B RID: 4731
		private readonly JObject _passRoot;
	}
}
