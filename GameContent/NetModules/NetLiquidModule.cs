using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	// Token: 0x020002E7 RID: 743
	public class NetLiquidModule : NetModule
	{
		// Token: 0x06002632 RID: 9778 RVA: 0x0055D53C File Offset: 0x0055B73C
		public static NetPacket Serialize(HashSet<int> changes)
		{
			NetPacket result = NetModule.CreatePacket<NetLiquidModule>(65530);
			result.Writer.Write((ushort)changes.Count);
			foreach (int num in changes)
			{
				int num2 = num >> 16 & 65535;
				int num3 = num & 65535;
				result.Writer.Write(num);
				result.Writer.Write(Main.tile[num2, num3].liquid);
				result.Writer.Write(Main.tile[num2, num3].liquidType());
			}
			return result;
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x0055D600 File Offset: 0x0055B800
		public static NetPacket SerializeForPlayer(int playerIndex)
		{
			NetLiquidModule._changesForPlayerCache.Clear();
			foreach (KeyValuePair<Point, NetLiquidModule.ChunkChanges> keyValuePair in NetLiquidModule._changesByChunkCoords)
			{
				if (keyValuePair.Value.BroadcastingCondition(playerIndex))
				{
					NetLiquidModule._changesForPlayerCache.AddRange(keyValuePair.Value.DirtiedPackedTileCoords);
				}
			}
			NetPacket result = NetModule.CreatePacket<NetLiquidModule>(65530);
			result.Writer.Write((ushort)NetLiquidModule._changesForPlayerCache.Count);
			foreach (int num in NetLiquidModule._changesForPlayerCache)
			{
				int num2 = num >> 16 & 65535;
				int num3 = num & 65535;
				result.Writer.Write(num);
				result.Writer.Write(Main.tile[num2, num3].liquid);
				result.Writer.Write(Main.tile[num2, num3].liquidType());
			}
			return result;
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x0055D73C File Offset: 0x0055B93C
		public override bool Deserialize(BinaryReader reader, int userId)
		{
			int num = (int)reader.ReadUInt16();
			for (int i = 0; i < num; i++)
			{
				int num2 = reader.ReadInt32();
				byte liquid = reader.ReadByte();
				byte liquidType = reader.ReadByte();
				int num3 = num2 >> 16 & 65535;
				int num4 = num2 & 65535;
				Tile tile = Main.tile[num3, num4];
				if (tile != null)
				{
					tile.liquid = liquid;
					tile.liquidType((int)liquidType);
				}
			}
			return true;
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x0055D7A9 File Offset: 0x0055B9A9
		public static void CreateAndBroadcastByChunk(HashSet<int> dirtiedPackedTileCoords)
		{
			NetLiquidModule.PrepareChunks(dirtiedPackedTileCoords);
			NetLiquidModule.PrepareAndSendToEachPlayerSeparately();
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x0055D7B8 File Offset: 0x0055B9B8
		private static void PrepareAndSendToEachPlayerSeparately()
		{
			for (int i = 0; i < 256; i++)
			{
				if (Netplay.Clients[i].IsConnected())
				{
					NetManager.Instance.SendToClient(NetLiquidModule.SerializeForPlayer(i), i);
				}
			}
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x0055D7F4 File Offset: 0x0055B9F4
		private static void BroadcastEachChunkSeparately()
		{
			foreach (KeyValuePair<Point, NetLiquidModule.ChunkChanges> keyValuePair in NetLiquidModule._changesByChunkCoords)
			{
				NetManager.Instance.Broadcast(NetLiquidModule.Serialize(keyValuePair.Value.DirtiedPackedTileCoords), new NetManager.BroadcastCondition(keyValuePair.Value.BroadcastingCondition), -1);
			}
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x0055D870 File Offset: 0x0055BA70
		private static void PrepareChunks(HashSet<int> dirtiedPackedTileCoords)
		{
			foreach (KeyValuePair<Point, NetLiquidModule.ChunkChanges> keyValuePair in NetLiquidModule._changesByChunkCoords)
			{
				keyValuePair.Value.DirtiedPackedTileCoords.Clear();
			}
			NetLiquidModule.DistributeChangesIntoChunks(dirtiedPackedTileCoords);
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x0055D8D4 File Offset: 0x0055BAD4
		private static void BroadcastAllChanges(HashSet<int> dirtiedPackedTileCoords)
		{
			NetManager.Instance.Broadcast(NetLiquidModule.Serialize(dirtiedPackedTileCoords), -1);
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x0055D8E8 File Offset: 0x0055BAE8
		private static void DistributeChangesIntoChunks(HashSet<int> dirtiedPackedTileCoords)
		{
			foreach (int num in dirtiedPackedTileCoords)
			{
				int x = num >> 16 & 65535;
				int y = num & 65535;
				Point point;
				point.X = Netplay.GetSectionX(x);
				point.Y = Netplay.GetSectionY(y);
				NetLiquidModule.ChunkChanges chunkChanges;
				if (!NetLiquidModule._changesByChunkCoords.TryGetValue(point, out chunkChanges))
				{
					chunkChanges = new NetLiquidModule.ChunkChanges(point.X, point.Y);
					NetLiquidModule._changesByChunkCoords[point] = chunkChanges;
				}
				chunkChanges.DirtiedPackedTileCoords.Add(num);
			}
		}

		// Token: 0x04005040 RID: 20544
		private static List<int> _changesForPlayerCache = new List<int>();

		// Token: 0x04005041 RID: 20545
		private static Dictionary<Point, NetLiquidModule.ChunkChanges> _changesByChunkCoords = new Dictionary<Point, NetLiquidModule.ChunkChanges>();

		// Token: 0x02000827 RID: 2087
		private class ChunkChanges
		{
			// Token: 0x0600430D RID: 17165 RVA: 0x006BF3FC File Offset: 0x006BD5FC
			public ChunkChanges(int x, int y)
			{
				this.ChunkX = x;
				this.ChunkY = y;
				this.DirtiedPackedTileCoords = new HashSet<int>();
			}

			// Token: 0x0600430E RID: 17166 RVA: 0x006BF41D File Offset: 0x006BD61D
			public bool BroadcastingCondition(int clientIndex)
			{
				return Netplay.Clients[clientIndex].TileSections[this.ChunkX, this.ChunkY];
			}

			// Token: 0x04007226 RID: 29222
			public HashSet<int> DirtiedPackedTileCoords;

			// Token: 0x04007227 RID: 29223
			public int ChunkX;

			// Token: 0x04007228 RID: 29224
			public int ChunkY;
		}
	}
}
