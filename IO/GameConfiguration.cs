using System;
using Newtonsoft.Json.Linq;

namespace Terraria.IO
{
	// Token: 0x02000074 RID: 116
	public class GameConfiguration
	{
		// Token: 0x0600150D RID: 5389 RVA: 0x004BCF03 File Offset: 0x004BB103
		public GameConfiguration(JObject configurationRoot)
		{
			this._root = configurationRoot;
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x004BCF12 File Offset: 0x004BB112
		public T Get<T>(string entry)
		{
			return this._root[entry].ToObject<T>();
		}

		// Token: 0x040010B5 RID: 4277
		private readonly JObject _root;
	}
}
