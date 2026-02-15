using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using ReLogic.Threading;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;

namespace Terraria.Utilities
{
	// Token: 0x020000D3 RID: 211
	public static class TileSnapshot
	{
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06001827 RID: 6183 RVA: 0x004E0A84 File Offset: 0x004DEC84
		// (set) Token: 0x06001828 RID: 6184 RVA: 0x004E0A8B File Offset: 0x004DEC8B
		public static object Context { get; private set; }

		// Token: 0x06001829 RID: 6185 RVA: 0x004E0A93 File Offset: 0x004DEC93
		public static void Create(object context = null)
		{
			TileSnapshot.Context = context;
			TileSnapshot._worldFile = Main.ActiveWorldFileData;
			TileSnapshot.SaveTiles();
			TileSnapshot.SaveTileEntities(true);
			TileSnapshot.SaveChests(true);
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x004E0AB8 File Offset: 0x004DECB8
		private static void SaveTiles()
		{
			Array.Resize<TileSnapshot.TileStruct>(ref TileSnapshot._tiles, Main.maxTilesX * Main.maxTilesY);
			FastParallel.For(0, Main.maxTilesX, delegate(int x0, int x1, object _)
			{
				Tile[,] tile = Main.tile;
				TileSnapshot.TileStruct[] tiles = TileSnapshot._tiles;
				int maxTilesY = Main.maxTilesY;
				for (int i = x0; i < x1; i++)
				{
					for (int j = 0; j < maxTilesY; j++)
					{
						tiles[i * maxTilesY + j] = TileSnapshot.TileStruct.From(tile[i, j]);
					}
				}
			}, null);
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x004E0B08 File Offset: 0x004DED08
		private static void SaveTileEntities(bool clone)
		{
			if (TileSnapshot._tileEntities == null)
			{
				TileSnapshot._tileEntities = new List<TileEntity>(TileEntity.ByID.Count);
			}
			TileSnapshot._tileEntities.Clear();
			object entityCreationLock = TileEntity.EntityCreationLock;
			lock (entityCreationLock)
			{
				foreach (TileEntity tileEntity in TileEntity.ByID.Values)
				{
					TileSnapshot._tileEntities.Add(clone ? TileSnapshot.Clone(tileEntity) : tileEntity);
				}
			}
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x004E0BBC File Offset: 0x004DEDBC
		private static void SaveChests(bool clone)
		{
			if (TileSnapshot._chests == null)
			{
				TileSnapshot._chests = new List<Chest>(8000);
			}
			TileSnapshot._chests.Clear();
			foreach (Chest chest2 in Main.chest)
			{
				if (chest2 != null)
				{
					TileSnapshot._chests.Add(clone ? chest2.CloneWithSeparateItems() : chest2);
				}
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600182D RID: 6189 RVA: 0x004E0C1A File Offset: 0x004DEE1A
		public static bool IsCreated
		{
			get
			{
				return TileSnapshot._tiles != null;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x0600182E RID: 6190 RVA: 0x004E0C24 File Offset: 0x004DEE24
		public static bool SizeMatches
		{
			get
			{
				return TileSnapshot._worldFile.WorldSizeX == Main.ActiveWorldFileData.WorldSizeX && TileSnapshot._worldFile.WorldSizeY == Main.ActiveWorldFileData.WorldSizeY;
			}
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x004E0C54 File Offset: 0x004DEE54
		public static IEnumerable<Point> Compare()
		{
			bool any = false;
			int num;
			for (int x = 0; x < Main.maxTilesX; x = num + 1)
			{
				for (int y = 0; y < Main.maxTilesY; y = num + 1)
				{
					TileSnapshot.TileStruct lhs = TileSnapshot.TileStruct.From(Main.tile[x, y]);
					TileSnapshot.TileStruct rhs = TileSnapshot._tiles[x * Main.maxTilesY + y];
					if (!(lhs == rhs))
					{
						any = true;
						Main.NewText(string.Concat(new object[]
						{
							"Mismatch at ",
							x,
							", ",
							y,
							" world vs snap"
						}), byte.MaxValue, byte.MaxValue, byte.MaxValue);
						Main.NewText(lhs.ToString(), byte.MaxValue, byte.MaxValue, byte.MaxValue);
						Main.NewText(rhs.ToString(), byte.MaxValue, byte.MaxValue, byte.MaxValue);
						yield return new Point(x, y);
					}
					num = y;
				}
				num = x;
			}
			Main.NewText(any ? "No more differences" : "Snapshot matches identically", byte.MaxValue, byte.MaxValue, byte.MaxValue);
			yield break;
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x004E0C5D File Offset: 0x004DEE5D
		private static TileEntity Clone(TileEntity ent)
		{
			TileSnapshot._tempStream.Position = 0L;
			TileEntity.Write(TileSnapshot._tempWriter, ent, false);
			TileSnapshot._tempStream.Position = 0L;
			return TileEntity.Read(TileSnapshot._tempReader, 318, false);
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x004E0C94 File Offset: 0x004DEE94
		public static void Restore()
		{
			TileSnapshot.RestoreTiles();
			TileSnapshot.RestoreTileEntities(TileSnapshot._tileEntities, true);
			TileSnapshot.RestoreChests(TileSnapshot._chests, true);
			if (Main.dedServ)
			{
				NetMessage.ResyncTiles(new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY));
				return;
			}
			Main.sectionManager.SetAllSectionsLoaded();
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x004E0CE4 File Offset: 0x004DEEE4
		private static void RestoreTiles()
		{
			FastParallel.For(0, Main.maxTilesX, delegate(int x0, int x1, object _)
			{
				TileSnapshot.TileStruct[] tiles = TileSnapshot._tiles;
				Tile[,] tile = Main.tile;
				int maxTilesY = Main.maxTilesY;
				for (int i = x0; i < x1; i++)
				{
					for (int j = 0; j < maxTilesY; j++)
					{
						tiles[i * maxTilesY + j].Apply(tile[i, j]);
					}
				}
				Liquid.numLiquid = 0;
				LiquidBuffer.numLiquidBuffer = 0;
			}, null);
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x004E0D14 File Offset: 0x004DEF14
		private static void RestoreTileEntities(List<TileEntity> entities, bool clone)
		{
			object entityCreationLock = TileEntity.EntityCreationLock;
			lock (entityCreationLock)
			{
				LeashedEntity.Clear(true);
				TileEntity.Clear();
				foreach (TileEntity tileEntity in entities)
				{
					TileSnapshot.Restore(clone ? TileSnapshot.Clone(tileEntity) : tileEntity);
				}
			}
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x004E0DA0 File Offset: 0x004DEFA0
		private static void Restore(TileEntity ent)
		{
			ent.ID = TileEntity.TileEntitiesNextID++;
			TileEntity.Add(ent);
			ent.OnWorldLoaded();
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x004E0DC4 File Offset: 0x004DEFC4
		private static void RestoreChests(List<Chest> chests, bool clone)
		{
			Chest.Clear();
			foreach (Chest chest in chests)
			{
				Chest.Assign(clone ? chest.CloneWithSeparateItems() : chest);
			}
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x004E0E24 File Offset: 0x004DF024
		public static void Swap()
		{
			TileSnapshot._worldFile = Main.ActiveWorldFileData;
			TileSnapshot.SwapTiles();
			List<TileEntity> tileEntities = TileSnapshot._tileEntities;
			TileSnapshot._tileEntities = null;
			TileSnapshot.SaveTileEntities(false);
			TileSnapshot.RestoreTileEntities(tileEntities, false);
			List<Chest> chests = TileSnapshot._chests;
			TileSnapshot._chests = null;
			TileSnapshot.SaveChests(false);
			TileSnapshot.RestoreChests(chests, false);
			if (Main.dedServ)
			{
				NetMessage.ResyncTiles(new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY));
				return;
			}
			Main.sectionManager.SetAllSectionsLoaded();
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x004E0E98 File Offset: 0x004DF098
		private static void SwapTiles()
		{
			Array.Resize<TileSnapshot.TileStruct>(ref TileSnapshot._tiles, Main.maxTilesX * Main.maxTilesY);
			FastParallel.For(0, Main.maxTilesX, delegate(int x0, int x1, object _)
			{
				Tile[,] tile = Main.tile;
				TileSnapshot.TileStruct[] tiles = TileSnapshot._tiles;
				int maxTilesY = Main.maxTilesY;
				for (int i = x0; i < x1; i++)
				{
					for (int j = 0; j < maxTilesY; j++)
					{
						Tile tile2 = tile[i, j];
						TileSnapshot.TileStruct tileStruct = TileSnapshot.TileStruct.From(tile2);
						Utils.Swap<TileSnapshot.TileStruct>(ref tiles[i * maxTilesY + j], ref tileStruct);
						tileStruct.Apply(tile2);
					}
				}
			}, null);
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x004E0EE5 File Offset: 0x004DF0E5
		public static void Clear()
		{
			TileSnapshot._tiles = null;
			TileSnapshot._tileEntities = null;
			TileSnapshot._chests = null;
			GC.Collect();
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x004E0F00 File Offset: 0x004DF100
		public static void Save(BinaryWriter writer)
		{
			writer.Write(Marshal.SizeOf(typeof(TileSnapshot.TileStruct)));
			foreach (TileSnapshot.TileStruct tileStruct in TileSnapshot._tiles)
			{
				tileStruct.Write(writer);
			}
			writer.Write(TileSnapshot._tileEntities.Count);
			foreach (TileEntity ent in TileSnapshot._tileEntities)
			{
				TileEntity.Write(writer, ent, false);
			}
			writer.Write(TileSnapshot._chests.Count);
			foreach (Chest chest in TileSnapshot._chests)
			{
				writer.Write(chest.index);
				writer.Write(chest.x);
				writer.Write(chest.y);
				writer.Write(chest.maxItems);
				writer.Write(chest.name);
				for (int j = 0; j < chest.maxItems; j++)
				{
					Item item = chest.item[j];
					if (item.IsAir)
					{
						writer.Write(0);
					}
					else
					{
						writer.Write((ushort)item.type);
						writer.Write((ushort)item.stack);
						writer.Write(item.prefix);
					}
				}
			}
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x004E1090 File Offset: 0x004DF290
		public static void Load(BinaryReader reader, object context = null)
		{
			if (reader.ReadInt32() != Marshal.SizeOf(typeof(TileSnapshot.TileStruct)))
			{
				throw new Exception("TileSnapshot was saved with a different value of #define SNAPSHOT_RUNTIME_DATA");
			}
			TileSnapshot.Context = context;
			TileSnapshot._worldFile = Main.ActiveWorldFileData;
			Array.Resize<TileSnapshot.TileStruct>(ref TileSnapshot._tiles, Main.maxTilesX * Main.maxTilesY);
			for (int i = 0; i < TileSnapshot._tiles.Length; i++)
			{
				TileSnapshot._tiles[i] = TileSnapshot.TileStruct.Read(reader);
			}
			if (TileSnapshot._tileEntities == null)
			{
				TileSnapshot._tileEntities = new List<TileEntity>();
			}
			TileSnapshot._tileEntities.Clear();
			int num = reader.ReadInt32();
			for (int j = 0; j < num; j++)
			{
				TileSnapshot._tileEntities.Add(TileEntity.Read(reader, 318, false));
			}
			if (TileSnapshot._chests == null)
			{
				TileSnapshot._chests = new List<Chest>();
			}
			TileSnapshot._chests.Clear();
			num = reader.ReadInt32();
			for (int k = 0; k < num; k++)
			{
				Chest chest = Chest.CreateOutOfArray(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
				chest.name = reader.ReadString();
				chest.FillWithEmptyInstances();
				for (int l = 0; l < chest.maxItems; l++)
				{
					int num2 = (int)reader.ReadUInt16();
					if (num2 != 0)
					{
						Item item = chest.item[l];
						item.SetDefaults(num2, null);
						item.stack = (int)reader.ReadUInt16();
						item.Prefix((int)reader.ReadByte());
					}
				}
				TileSnapshot._chests.Add(chest);
			}
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x004E120C File Offset: 0x004DF40C
		public static void Save(string path)
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(File.Create(path)))
			{
				TileSnapshot.Save(binaryWriter);
			}
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x004E1248 File Offset: 0x004DF448
		public static void Load(string path, object context = null)
		{
			using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(path)))
			{
				TileSnapshot.Load(binaryReader, context);
			}
		}

		// Token: 0x040012AF RID: 4783
		private static WorldFileData _worldFile;

		// Token: 0x040012B0 RID: 4784
		private static TileSnapshot.TileStruct[] _tiles;

		// Token: 0x040012B1 RID: 4785
		private static List<TileEntity> _tileEntities;

		// Token: 0x040012B2 RID: 4786
		private static List<Chest> _chests;

		// Token: 0x040012B3 RID: 4787
		private static MemoryStream _tempStream = new MemoryStream();

		// Token: 0x040012B4 RID: 4788
		private static BinaryWriter _tempWriter = new BinaryWriter(TileSnapshot._tempStream);

		// Token: 0x040012B5 RID: 4789
		private static BinaryReader _tempReader = new BinaryReader(TileSnapshot._tempStream);

		// Token: 0x020006F5 RID: 1781
		[StructLayout(LayoutKind.Explicit)]
		public struct TileStruct
		{
			// Token: 0x06003FA5 RID: 16293 RVA: 0x00699DD8 File Offset: 0x00697FD8
			public static TileSnapshot.TileStruct From(Tile tile)
			{
				TileSnapshot.TileStruct tileStruct = default(TileSnapshot.TileStruct);
				tileStruct._type = tile.type;
				ushort wall = tile.wall;
				tileStruct._sTileHeader = tile.sTileHeader;
				tileStruct._liquid = tile.liquid;
				tileStruct._bTileHeader = tile.bTileHeader;
				tileStruct._wall_bTileHeader3_packed = (ushort)((int)wall | (int)(tile.bTileHeader3 & 224) << 8);
				if ((tileStruct._sTileHeader & 32) == 0)
				{
					tileStruct._type = 0;
					tileStruct._sTileHeader &= 35808;
				}
				else
				{
					if (Main.tileFrameImportant[(int)tileStruct._type])
					{
						tileStruct._frameX = tile.frameX;
						tileStruct._frameY = tile.frameY;
					}
					if ((tileStruct._sTileHeader & 29696) != 0 && !TileID.Sets.SaveSlopes[(int)tileStruct._type])
					{
						tileStruct._sTileHeader &= 35839;
					}
				}
				if (wall == 0)
				{
					tileStruct._bTileHeader &= 224;
				}
				if (tileStruct._liquid == 0)
				{
					tileStruct._bTileHeader &= 159;
				}
				return tileStruct;
			}

			// Token: 0x06003FA6 RID: 16294 RVA: 0x00699EEC File Offset: 0x006980EC
			public void Apply(Tile tile)
			{
				tile.type = this._type;
				tile.wall = (this._wall_bTileHeader3_packed & 8191);
				tile.sTileHeader = this._sTileHeader;
				tile.frameX = this._frameX;
				tile.frameY = this._frameY;
				tile.liquid = this._liquid;
				tile.bTileHeader = this._bTileHeader;
				tile.bTileHeader2 = 0;
				tile.bTileHeader3 = (byte)(this._wall_bTileHeader3_packed >> 8 & 224);
			}

			// Token: 0x06003FA7 RID: 16295 RVA: 0x00699F70 File Offset: 0x00698170
			public static bool operator ==(TileSnapshot.TileStruct lhs, TileSnapshot.TileStruct rhs)
			{
				return lhs._i0 == rhs._i0 && lhs._i1 == rhs._i1 && lhs._i2 == rhs._i2;
			}

			// Token: 0x06003FA8 RID: 16296 RVA: 0x00699F9E File Offset: 0x0069819E
			public static bool operator !=(TileSnapshot.TileStruct lhs, TileSnapshot.TileStruct rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x06003FA9 RID: 16297 RVA: 0x00699FAA File Offset: 0x006981AA
			public override bool Equals(object obj)
			{
				return obj is TileSnapshot.TileStruct && (TileSnapshot.TileStruct)obj == this;
			}

			// Token: 0x06003FAA RID: 16298 RVA: 0x00699FC7 File Offset: 0x006981C7
			public override int GetHashCode()
			{
				return this._i0 ^ this._i1 ^ this._i2;
			}

			// Token: 0x06003FAB RID: 16299 RVA: 0x00699FE0 File Offset: 0x006981E0
			public override string ToString()
			{
				bool flag = (this._sTileHeader & 32) > 0;
				int num = (int)(this._sTileHeader & 31);
				bool flag2 = (this._sTileHeader & 1024) > 0;
				int num2 = (this._sTileHeader & 28672) >> 12;
				int num3 = (int)(this._wall_bTileHeader3_packed & 8191);
				int num4 = (int)(this._bTileHeader & 31);
				int num5 = (this._bTileHeader & 96) >> 5;
				bool flag3 = (this._sTileHeader & 128) > 0;
				bool flag4 = (this._sTileHeader & 256) > 0;
				bool flag5 = (this._sTileHeader & 512) > 0;
				bool flag6 = (this._bTileHeader & 128) > 0;
				bool flag7 = (this._sTileHeader & 2048) > 0;
				bool flag8 = (this._sTileHeader & 64) > 0;
				bool flag9 = (this._wall_bTileHeader3_packed & 8192) > 0;
				bool flag10 = (this._wall_bTileHeader3_packed & 16384) > 0;
				bool flag11 = (this._sTileHeader & 32768) > 0;
				bool flag12 = (this._wall_bTileHeader3_packed & 32768) > 0;
				string text = "!tile";
				if (flag)
				{
					text = "tile:" + this._type;
					if (num > 0)
					{
						text = text + "c" + num;
					}
					if (flag2)
					{
						text += "h";
					}
					if (num2 != 0)
					{
						text = text + "s" + num2;
					}
					if (Main.tileFrameImportant[(int)this._type])
					{
						text = string.Concat(new object[]
						{
							text,
							" f",
							this._frameX,
							",",
							this._frameY
						});
					}
				}
				string text2 = "!wall";
				if (num3 > 0)
				{
					text2 = "wall:" + num3;
					if (num4 > 0)
					{
						text2 = text2 + "c" + num4;
					}
				}
				string text3 = "!liquid";
				if (this._liquid > 0)
				{
					text3 = TileSnapshot.TileStruct._liquidNames[num5] + ":" + this._liquid;
				}
				return string.Format("{0} {1} {2} flags:{3}{4} {5}{6} {7}{8} {9}{10}{11}{12}", new object[]
				{
					text,
					text2,
					text3,
					flag7 ? 'a' : '0',
					flag8 ? 'x' : '0',
					flag9 ? 'E' : '0',
					flag10 ? 'e' : '0',
					flag12 ? 'F' : '0',
					flag11 ? 'f' : '0',
					flag3 ? 'r' : '0',
					flag4 ? 'b' : '0',
					flag5 ? 'g' : '0',
					flag6 ? 'y' : '0'
				});
			}

			// Token: 0x06003FAC RID: 16300 RVA: 0x0069A2DC File Offset: 0x006984DC
			public void Write(BinaryWriter writer)
			{
				writer.Write(this._i0);
				writer.Write(this._i1);
				writer.Write(this._i2);
			}

			// Token: 0x06003FAD RID: 16301 RVA: 0x0069A304 File Offset: 0x00698504
			public static TileSnapshot.TileStruct Read(BinaryReader reader)
			{
				return new TileSnapshot.TileStruct
				{
					_i0 = reader.ReadInt32(),
					_i1 = reader.ReadInt32(),
					_i2 = reader.ReadInt32()
				};
			}

			// Token: 0x040067E4 RID: 26596
			[FieldOffset(0)]
			private ushort _type;

			// Token: 0x040067E5 RID: 26597
			[FieldOffset(2)]
			private ushort _wall_bTileHeader3_packed;

			// Token: 0x040067E6 RID: 26598
			[FieldOffset(4)]
			private ushort _sTileHeader;

			// Token: 0x040067E7 RID: 26599
			[FieldOffset(6)]
			private short _frameX;

			// Token: 0x040067E8 RID: 26600
			[FieldOffset(8)]
			private short _frameY;

			// Token: 0x040067E9 RID: 26601
			[FieldOffset(10)]
			private byte _liquid;

			// Token: 0x040067EA RID: 26602
			[FieldOffset(11)]
			private byte _bTileHeader;

			// Token: 0x040067EB RID: 26603
			[FieldOffset(0)]
			private int _i0;

			// Token: 0x040067EC RID: 26604
			[FieldOffset(4)]
			private int _i1;

			// Token: 0x040067ED RID: 26605
			[FieldOffset(8)]
			private int _i2;

			// Token: 0x040067EE RID: 26606
			private static string[] _liquidNames = new string[]
			{
				"water",
				"lava",
				"honey",
				"shimmer"
			};
		}
	}
}
