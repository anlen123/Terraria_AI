using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Utilities;

namespace Terraria.GameContent
{
	// Token: 0x0200026E RID: 622
	public class TreeTopsInfo
	{
		// Token: 0x060023F9 RID: 9209 RVA: 0x005493EC File Offset: 0x005475EC
		public void Save(BinaryWriter writer)
		{
			writer.Write(this._variations.Length);
			for (int i = 0; i < this._variations.Length; i++)
			{
				writer.Write(this._variations[i]);
			}
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x00549428 File Offset: 0x00547628
		public void Load(BinaryReader reader, int loadVersion)
		{
			if (loadVersion < 211)
			{
				this.CopyExistingWorldInfo();
				return;
			}
			int num = reader.ReadInt32();
			int num2 = 0;
			while (num2 < num && num2 < this._variations.Length)
			{
				this._variations[num2] = reader.ReadInt32();
				num2++;
			}
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x00549470 File Offset: 0x00547670
		public void SyncSend(BinaryWriter writer)
		{
			for (int i = 0; i < this._variations.Length; i++)
			{
				writer.Write((byte)this._variations[i]);
			}
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x005494A0 File Offset: 0x005476A0
		public void SyncReceive(BinaryReader reader)
		{
			for (int i = 0; i < this._variations.Length; i++)
			{
				int num = this._variations[i];
				this._variations[i] = (int)reader.ReadByte();
				if (this._variations[i] != num)
				{
					this.DoTreeFX(i);
				}
			}
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x005494E9 File Offset: 0x005476E9
		public int GetTreeStyle(int areaId)
		{
			return this._variations[areaId];
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x005494F4 File Offset: 0x005476F4
		public void RandomizeTreeStyleBasedOnWorldPosition(UnifiedRandom rand, Vector2 worldPosition)
		{
			Point point = new Point((int)(worldPosition.X / 16f), (int)(worldPosition.Y / 16f) + 1);
			Tile tileSafely = Framing.GetTileSafely(point);
			if (!tileSafely.active())
			{
				return;
			}
			int num = -1;
			if (tileSafely.type == 70)
			{
				num = 11;
			}
			else if (tileSafely.type == 53 && WorldGen.oceanDepths(point.X, point.Y))
			{
				num = 10;
			}
			else if (tileSafely.type == 23)
			{
				num = 4;
			}
			else if (tileSafely.type == 199)
			{
				num = 8;
			}
			else if (tileSafely.type == 109 || tileSafely.type == 492)
			{
				num = 7;
			}
			else if (tileSafely.type == 53)
			{
				num = 9;
			}
			else if (tileSafely.type == 147)
			{
				num = 6;
			}
			else if (tileSafely.type == 60)
			{
				num = 5;
			}
			else if (tileSafely.type == 633)
			{
				num = 12;
			}
			else if (tileSafely.type == 2 || tileSafely.type == 477)
			{
				if (point.X < Main.treeX[0])
				{
					num = 0;
				}
				else if (point.X < Main.treeX[1])
				{
					num = 1;
				}
				else if (point.X < Main.treeX[2])
				{
					num = 2;
				}
				else
				{
					num = 3;
				}
			}
			if (num > -1)
			{
				this.RandomizeTreeStyle(rand, num);
			}
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x00549650 File Offset: 0x00547850
		public void RandomizeTreeStyle(UnifiedRandom rand, int areaId)
		{
			int num = this._variations[areaId];
			bool flag = false;
			while (this._variations[areaId] == num)
			{
				switch (areaId)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					this._variations[areaId] = rand.Next(6);
					break;
				case 4:
					this._variations[areaId] = rand.Next(5);
					break;
				case 5:
					this._variations[areaId] = rand.Next(6);
					break;
				case 6:
					this._variations[areaId] = rand.NextFromList(new int[]
					{
						0,
						1,
						2,
						21,
						22,
						3,
						31,
						32,
						4,
						41,
						42,
						5,
						6,
						7
					});
					break;
				case 7:
					this._variations[areaId] = rand.Next(5);
					break;
				case 8:
					this._variations[areaId] = rand.Next(6);
					break;
				case 9:
					this._variations[areaId] = rand.Next(5);
					break;
				case 10:
					this._variations[areaId] = rand.Next(6);
					break;
				case 11:
					this._variations[areaId] = rand.Next(4);
					break;
				case 12:
					this._variations[areaId] = rand.Next(6);
					break;
				default:
					flag = true;
					break;
				}
				if (flag)
				{
					break;
				}
			}
			if (num != this._variations[areaId])
			{
				if (Main.netMode == 2)
				{
					NetMessage.SendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				this.DoTreeFX(areaId);
			}
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x00009E06 File Offset: 0x00008006
		private void DoTreeFX(int areaID)
		{
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x005497BA File Offset: 0x005479BA
		public void CopyExistingWorldInfoForWorldGeneration()
		{
			this.CopyExistingWorldInfo();
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x005497C4 File Offset: 0x005479C4
		private void CopyExistingWorldInfo()
		{
			this._variations[0] = Main.treeStyle[0];
			this._variations[1] = Main.treeStyle[1];
			this._variations[2] = Main.treeStyle[2];
			this._variations[3] = Main.treeStyle[3];
			this._variations[4] = WorldGen.corruptBG;
			this._variations[5] = WorldGen.jungleBG;
			this._variations[6] = WorldGen.snowBG;
			this._variations[7] = WorldGen.hallowBG;
			this._variations[8] = WorldGen.crimsonBG;
			this._variations[9] = WorldGen.desertBG;
			this._variations[10] = WorldGen.oceanBG;
			this._variations[11] = WorldGen.mushroomBG;
			this._variations[12] = WorldGen.underworldBG;
		}

		// Token: 0x04004DAA RID: 19882
		private int[] _variations = new int[TreeTopsInfo.AreaId.Count];

		// Token: 0x020007F0 RID: 2032
		public class AreaId
		{
			// Token: 0x0400712C RID: 28972
			public const int Forest1 = 0;

			// Token: 0x0400712D RID: 28973
			public const int Forest2 = 1;

			// Token: 0x0400712E RID: 28974
			public const int Forest3 = 2;

			// Token: 0x0400712F RID: 28975
			public const int Forest4 = 3;

			// Token: 0x04007130 RID: 28976
			public const int Corruption = 4;

			// Token: 0x04007131 RID: 28977
			public const int Jungle = 5;

			// Token: 0x04007132 RID: 28978
			public const int Snow = 6;

			// Token: 0x04007133 RID: 28979
			public const int Hallow = 7;

			// Token: 0x04007134 RID: 28980
			public const int Crimson = 8;

			// Token: 0x04007135 RID: 28981
			public const int Desert = 9;

			// Token: 0x04007136 RID: 28982
			public const int Ocean = 10;

			// Token: 0x04007137 RID: 28983
			public const int GlowingMushroom = 11;

			// Token: 0x04007138 RID: 28984
			public const int Underworld = 12;

			// Token: 0x04007139 RID: 28985
			public static readonly int Count = 13;
		}
	}
}
