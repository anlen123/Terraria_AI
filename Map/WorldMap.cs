using System;
using System.IO;
using Terraria.IO;
using Terraria.Social;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.Map
{
	// Token: 0x02000183 RID: 387
	public class WorldMap
	{
		// Token: 0x170002F5 RID: 757
		public MapTile this[int x, int y]
		{
			get
			{
				return this._tiles[x, y];
			}
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x0050ECAC File Offset: 0x0050CEAC
		public WorldMap(int maxWidth, int maxHeight)
		{
			this.MaxWidth = maxWidth;
			this.MaxHeight = maxHeight;
			this._tiles = new MapTile[this.MaxWidth, this.MaxHeight];
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x0050ECD9 File Offset: 0x0050CED9
		public void ConsumeUpdate(int x, int y)
		{
			this._tiles[x, y].IsChanged = false;
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x0050ECEE File Offset: 0x0050CEEE
		public void Update(int x, int y, byte light)
		{
			this._tiles[x, y] = MapHelper.CreateMapTile(x, y, light, 0);
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x0050ED06 File Offset: 0x0050CF06
		public void SetTile(int x, int y, ref MapTile tile)
		{
			this._tiles[x, y] = tile;
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x0050ED1B File Offset: 0x0050CF1B
		public bool IsRevealed(int x, int y)
		{
			return this._tiles[x, y].Light > 0;
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x0050ED34 File Offset: 0x0050CF34
		public bool UpdateLighting(int x, int y, byte light)
		{
			MapTile mapTile = this._tiles[x, y];
			if (light == 0 && mapTile.Light == 0)
			{
				return false;
			}
			MapTile mapTile2 = MapHelper.CreateMapTile(x, y, Math.Max(mapTile.Light, light), 0);
			if (mapTile2.Equals(mapTile))
			{
				return false;
			}
			this._tiles[x, y] = mapTile2;
			return true;
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x0050ED8C File Offset: 0x0050CF8C
		public bool UpdateType(int x, int y)
		{
			return this.UpdateType(x, y, ref this._tiles[x, y]);
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x0050EDA4 File Offset: 0x0050CFA4
		private bool UpdateType(int x, int y, ref MapTile mapTile)
		{
			if (!mapTile.UpdateQueued)
			{
				return false;
			}
			mapTile.UpdateQueued = false;
			if (mapTile.Light == 0)
			{
				return false;
			}
			if (!Main.sectionManager.TileLoaded(x, y))
			{
				return false;
			}
			bool flag = MapHelper.IsBackground((int)mapTile.Type);
			MapTile mapTile2 = MapHelper.CreateMapTile(x, y, mapTile.Light, (int)(flag ? mapTile.Type : 0));
			if (mapTile2.Equals(mapTile))
			{
				return false;
			}
			mapTile = mapTile2;
			return true;
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x0050EE1B File Offset: 0x0050D01B
		internal bool QueueUpdate(int x, int y)
		{
			return this.QueueUpdate(ref this._tiles[x, y]);
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x0050EE30 File Offset: 0x0050D030
		private bool QueueUpdate(ref MapTile mapTile)
		{
			if (mapTile.Light == 0 || mapTile.UpdateQueued)
			{
				return false;
			}
			mapTile.UpdateQueued = true;
			return true;
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x0050EE4C File Offset: 0x0050D04C
		public void UnlockMapSection(int sectionX, int sectionY)
		{
			int num = sectionX * 200;
			int num2 = num + 200;
			int num3 = sectionY * 150;
			int num4 = num3 + 150;
			int num5 = 40;
			num = Utils.Clamp<int>(num, num5, Main.maxTilesX - num5);
			num2 = Utils.Clamp<int>(num2, num5, Main.maxTilesX - num5);
			num3 = Utils.Clamp<int>(num3, num5, Main.maxTilesY - num5);
			num4 = Utils.Clamp<int>(num4, num5, Main.maxTilesY - num5);
			if (DebugOptions.unlockMap == 2)
			{
				for (int i = num; i < num2; i++)
				{
					for (int j = num3; j < num4; j++)
					{
						this.UnlockMapTilePretty(i, j);
					}
				}
				return;
			}
			for (int k = num; k < num2; k++)
			{
				for (int l = num3; l < num4; l++)
				{
					this.UpdateLighting(k, l, byte.MaxValue);
				}
			}
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x0050EF24 File Offset: 0x0050D124
		public void UnlockMapTilePretty(int x, int y)
		{
			if (!WorldGen.InWorld(x, y, 12))
			{
				return;
			}
			if (WorldGen.SolidTile(x, y, false))
			{
				return;
			}
			int num = 5;
			float num2 = 255f;
			Tile tileSafely = Framing.GetTileSafely(x, y);
			if (tileSafely.liquid > 0 && !tileSafely.lava())
			{
				return;
			}
			if (tileSafely.wall > 0)
			{
				num2 *= 0.8f;
			}
			if ((double)y >= Main.worldSurface)
			{
				num2 *= 0.7f;
			}
			for (int i = -num; i <= num; i++)
			{
				for (int j = -num; j <= num; j++)
				{
					int x2 = x + i;
					int y2 = y + j;
					float num3 = (float)(num - Math.Abs(i) - Math.Abs(j));
					if (num3 >= 0f)
					{
						this.UpdateLighting(x2, y2, (byte)(num2 * (num3 / (float)num)));
					}
				}
			}
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x0050EFEC File Offset: 0x0050D1EC
		public void Load()
		{
			Lighting.Clear();
			bool isCloudSave = Main.ActivePlayerFileData.IsCloudSave;
			if (isCloudSave && SocialAPI.Cloud == null)
			{
				return;
			}
			if (!Main.mapEnabled)
			{
				return;
			}
			string text;
			if (!WorldMap.TryGetMapPath(Main.ActivePlayerFileData, Main.ActiveWorldFileData, out text))
			{
				Main.MapFileMetadata = FileMetadata.FromCurrentSettings(FileType.Map);
				return;
			}
			using (MemoryStream memoryStream = new MemoryStream(FileUtilities.ReadAllBytes(text, isCloudSave)))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					try
					{
						int num = binaryReader.ReadInt32();
						bool flag = (num & 32768) == 32768;
						num &= -32769;
						if (num <= 318)
						{
							if (flag)
							{
								MapHelper.LoadMapVersionCompressed(binaryReader, num);
							}
							else if (num <= 91)
							{
								MapHelper.LoadMapVersion1(binaryReader, num);
							}
							else
							{
								MapHelper.LoadMapVersion2(binaryReader, num);
							}
							this.ClearEdges();
							Main.clearMap = true;
							Main.loadMap = true;
							Main.loadMapLock = true;
							Main.refreshMap = false;
						}
					}
					catch (Exception value)
					{
						using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", true))
						{
							streamWriter.WriteLine(DateTime.Now);
							streamWriter.WriteLine(value);
							streamWriter.WriteLine("");
						}
						if (!isCloudSave)
						{
							File.Copy(text, text + ".bad", true);
						}
						this.Clear();
					}
				}
			}
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x0050F170 File Offset: 0x0050D370
		public static bool TryGetMapPath(PlayerFileData playerFileData, WorldFileData worldFileData, out string mapPath)
		{
			string text = playerFileData.Path.Substring(0, playerFileData.Path.Length - 4);
			mapPath = text + Path.DirectorySeparatorChar.ToString() + worldFileData.MapFileName + ".map";
			if (worldFileData.UseGuidAsMapName && !FileUtilities.Exists(mapPath, playerFileData.IsCloudSave))
			{
				mapPath = string.Concat(new object[]
				{
					text,
					Path.DirectorySeparatorChar.ToString(),
					worldFileData.WorldId,
					".map"
				});
			}
			return FileUtilities.Exists(mapPath, playerFileData.IsCloudSave);
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x0050F212 File Offset: 0x0050D412
		public void Save()
		{
			MapHelper.SaveMap();
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x0050F21C File Offset: 0x0050D41C
		public void Clear()
		{
			for (int i = 0; i < this.MaxWidth; i++)
			{
				for (int j = 0; j < this.MaxHeight; j++)
				{
					this._tiles[i, j].Clear();
				}
			}
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x0050F260 File Offset: 0x0050D460
		public void ClearEdges()
		{
			for (int i = 0; i < this.MaxWidth; i++)
			{
				for (int j = 0; j < 40; j++)
				{
					this._tiles[i, j].Clear();
				}
			}
			for (int k = 0; k < this.MaxWidth; k++)
			{
				for (int l = this.MaxHeight - 40; l < this.MaxHeight; l++)
				{
					this._tiles[k, l].Clear();
				}
			}
			for (int m = 0; m < 40; m++)
			{
				for (int n = 40; n < this.MaxHeight - 40; n++)
				{
					this._tiles[m, n].Clear();
				}
			}
			for (int num = this.MaxWidth - 40; num < this.MaxWidth; num++)
			{
				for (int num2 = 40; num2 < this.MaxHeight - 40; num2++)
				{
					this._tiles[num, num2].Clear();
				}
			}
		}

		// Token: 0x040016BE RID: 5822
		public readonly int MaxWidth;

		// Token: 0x040016BF RID: 5823
		public readonly int MaxHeight;

		// Token: 0x040016C0 RID: 5824
		public const int BlackEdgeWidth = 40;

		// Token: 0x040016C1 RID: 5825
		private MapTile[,] _tiles;
	}
}
