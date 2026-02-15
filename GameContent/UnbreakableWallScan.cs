using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Net;

namespace Terraria.GameContent
{
	// Token: 0x0200024A RID: 586
	public static class UnbreakableWallScan
	{
		// Token: 0x060022F1 RID: 8945 RVA: 0x0053B36A File Offset: 0x0053956A
		public static void Update(Player player)
		{
			int netMode = Main.netMode;
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x0053B374 File Offset: 0x00539574
		public static bool InsideUnbreakableWalls(Point pt)
		{
			int num = 0;
			for (int i = 0; i < UnbreakableWallScan.Directions.Length; i++)
			{
				if (UnbreakableWallScan.LineScan(pt, UnbreakableWallScan.Directions[i]))
				{
					num |= 1 << i;
				}
			}
			for (int j = 0; j < UnbreakableWallScan.Directions.Length; j++)
			{
				if ((num & 31) == 0)
				{
					return false;
				}
				num = ((num << 1 & 255) | num >> 7);
			}
			return true;
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x0053B3DC File Offset: 0x005395DC
		public static bool LineScan(Point pt, Point dir)
		{
			int i = 0;
			while (i < UnbreakableWallScan.ScanDistance)
			{
				if (!WorldGen.InWorld(pt, 0))
				{
					return false;
				}
				Tile tile = Main.tile[pt.X, pt.Y];
				if (tile == null)
				{
					return false;
				}
				if (tile.wall == 350)
				{
					return tile.wallColor() >= 16;
				}
				i++;
				pt.X += dir.X;
				pt.Y += dir.Y;
			}
			return false;
		}

		// Token: 0x04004D1A RID: 19738
		public static readonly int ScanDistance = 250;

		// Token: 0x04004D1B RID: 19739
		public static readonly Point[] Directions = new Point[]
		{
			new Point(1, 0),
			new Point(1, 1),
			new Point(0, 1),
			new Point(-1, 1),
			new Point(-1, 0),
			new Point(-1, -1),
			new Point(0, -1),
			new Point(1, -1)
		};

		// Token: 0x020007D3 RID: 2003
		public class NetModule : Terraria.Net.NetModule
		{
			// Token: 0x06004230 RID: 16944 RVA: 0x006BCA42 File Offset: 0x006BAC42
			public override bool Deserialize(BinaryReader reader, int userId)
			{
				if (Main.netMode != 1)
				{
					return false;
				}
				Main.player[(int)reader.ReadByte()].insideUnbreakableWalls = reader.ReadBoolean();
				return true;
			}

			// Token: 0x06004231 RID: 16945 RVA: 0x006BCA68 File Offset: 0x006BAC68
			internal static void BroadcastChange(Player player)
			{
				NetPacket packet = Terraria.Net.NetModule.CreatePacket<UnbreakableWallScan.NetModule>(65530);
				packet.Writer.Write((byte)player.whoAmI);
				packet.Writer.Write(player.insideUnbreakableWalls);
				NetManager.Instance.Broadcast(packet, -1);
			}
		}
	}
}
