using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Terraria.ID;

namespace Terraria.GameContent.Metadata
{
	// Token: 0x0200028E RID: 654
	public static class TileMaterials
	{
		// Token: 0x06002516 RID: 9494 RVA: 0x00552978 File Offset: 0x00550B78
		static TileMaterials()
		{
			TileMaterials._materialsByName = TileMaterials.DeserializeEmbeddedResource<Dictionary<string, TileMaterial>>("Terraria.GameContent.Metadata.MaterialData.Materials.json");
			TileMaterial tileMaterial = TileMaterials._materialsByName["Default"];
			for (int i = 0; i < TileMaterials.MaterialsByTileId.Length; i++)
			{
				TileMaterials.MaterialsByTileId[i] = tileMaterial;
			}
			foreach (KeyValuePair<string, string> keyValuePair in TileMaterials.DeserializeEmbeddedResource<Dictionary<string, string>>("Terraria.GameContent.Metadata.MaterialData.Tiles.json"))
			{
				string key = keyValuePair.Key;
				string value = keyValuePair.Value;
				TileMaterials.SetForTileId((ushort)TileID.Search.GetId(key), TileMaterials._materialsByName[value]);
			}
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x00552A40 File Offset: 0x00550C40
		private static T DeserializeEmbeddedResource<T>(string path)
		{
			T result;
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
			{
				using (StreamReader streamReader = new StreamReader(manifestResourceStream))
				{
					result = JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd());
				}
			}
			return result;
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x00552AA0 File Offset: 0x00550CA0
		public static void SetForTileId(ushort tileId, TileMaterial material)
		{
			TileMaterials.MaterialsByTileId[(int)tileId] = material;
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x00552AAA File Offset: 0x00550CAA
		public static TileMaterial GetByTileId(ushort tileId)
		{
			return TileMaterials.MaterialsByTileId[(int)tileId];
		}

		// Token: 0x04004F65 RID: 20325
		private static Dictionary<string, TileMaterial> _materialsByName;

		// Token: 0x04004F66 RID: 20326
		private static readonly TileMaterial[] MaterialsByTileId = new TileMaterial[(int)TileID.Count];
	}
}
