using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ObjectData;

namespace Terraria
{
	// Token: 0x02000011 RID: 17
	public class Chest : IFixLoadedData
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00003615 File Offset: 0x00001815
		public static void Clear()
		{
			Array.Clear(Main.chest, 0, Main.chest.Length);
			Chest._chestsByCoords.Clear();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003633 File Offset: 0x00001833
		public static Chest CreateWorldChest(int index, int x, int y)
		{
			Chest chest = new Chest(index, x, y, false, 40);
			chest.FillWithEmptyInstances();
			Chest.Assign(chest);
			return chest;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000364C File Offset: 0x0000184C
		public static void Assign(Chest chest)
		{
			Main.chest[chest.index] = chest;
			Chest._chestsByCoords[new Point(chest.x, chest.y)] = chest;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003678 File Offset: 0x00001878
		public void Resize(int newSize)
		{
			int num = this.maxItems;
			this.maxItems = newSize;
			Array.Resize<Item>(ref this.item, newSize);
			if (this._itemsGotSet)
			{
				for (int i = num; i < newSize; i++)
				{
					this.item[i] = new Item();
				}
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000036C0 File Offset: 0x000018C0
		public static void RemoveChest(int chestIndex)
		{
			Chest chest = Main.chest[chestIndex];
			if (chest != null)
			{
				Chest._chestsByCoords.Remove(new Point(chest.x, chest.y));
			}
			Main.chest[chestIndex] = null;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000036FC File Offset: 0x000018FC
		public static Chest CreateBank(int index)
		{
			return new Chest(index, 0, 0, true, 40);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003709 File Offset: 0x00001909
		public static Chest CreateShop()
		{
			return new Chest(0, 0, 0, false, 40);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003716 File Offset: 0x00001916
		public static Chest CreateOutOfArray(int index, int x, int y, int maxItems)
		{
			return new Chest(index, x, y, false, maxItems);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003724 File Offset: 0x00001924
		public void FillWithEmptyInstances()
		{
			for (int i = 0; i < this.maxItems; i++)
			{
				this.item[i] = new Item();
			}
			this._itemsGotSet = true;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003758 File Offset: 0x00001958
		public bool IsEmpty()
		{
			for (int i = 0; i < this.maxItems; i++)
			{
				if (!this.item[i].IsAir)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003788 File Offset: 0x00001988
		public bool IsLockedOrInUse()
		{
			return !this.bankChest && (Chest.IsPlayerInChest(this.index) || Chest.IsLocked(this.x, this.y));
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000037B4 File Offset: 0x000019B4
		private Chest(int index = 0, int x = 0, int y = 0, bool bank = false, int maxItems = 40)
		{
			this.maxItems = maxItems;
			this.item = new Item[maxItems];
			this.index = index;
			this.x = x;
			this.y = y;
			this.bankChest = bank;
			this.name = string.Empty;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003804 File Offset: 0x00001A04
		public override string ToString()
		{
			int num = 0;
			for (int i = 0; i < this.item.Length; i++)
			{
				if (this.item[i].stack > 0)
				{
					num++;
				}
			}
			return string.Format("{{X: {0}, Y: {1}, Count: {2}}}", this.x, this.y, num);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003860 File Offset: 0x00001A60
		public Chest CloneWithSeparateItems()
		{
			Chest chest = new Chest(this.index, this.x, this.y, this.bankChest, this.maxItems)
			{
				name = this.name
			};
			for (int i = 0; i < this.maxItems; i++)
			{
				chest.item[i] = this.item[i].Clone();
				chest._itemsGotSet = true;
			}
			return chest;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000038CC File Offset: 0x00001ACC
		public static void Initialize()
		{
			int[] array = Chest.chestTypeToIcon;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = WorldGen.GetItemDrop_Chests(i, false);
			}
			array[2] = 327;
			array[4] = 329;
			array[23] = 1533;
			array[24] = 1534;
			array[25] = 1535;
			array[26] = 1536;
			array[27] = 1537;
			array[36] = 327;
			array[38] = 327;
			array[40] = 327;
			int[] array2 = Chest.chestTypeToIcon2;
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = WorldGen.GetItemDrop_Chests(j, true);
			}
			array2[13] = 4714;
			int[] array3 = Chest.dresserTypeToIcon;
			for (int k = 0; k < Chest.dresserTypeToIcon.Length; k++)
			{
				array3[k] = WorldGen.GetItemDrop_Dressers(k);
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000039A4 File Offset: 0x00001BA4
		public static bool IsPlayerInChest(int i)
		{
			for (int j = 0; j < 255; j++)
			{
				if (Main.player[j].chest == i)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000039D4 File Offset: 0x00001BD4
		public static List<int> GetCurrentlyOpenChests()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].chest > -1)
				{
					list.Add(Main.player[i].chest);
				}
			}
			return list;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003A19 File Offset: 0x00001C19
		public static bool IsLocked(int x, int y)
		{
			return Chest.IsLocked(Main.tile[x, y]);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003A2C File Offset: 0x00001C2C
		public static bool IsLocked(Tile t)
		{
			return t == null || (t.type == 21 && ((t.frameX >= 72 && t.frameX <= 106) || (t.frameX >= 144 && t.frameX <= 178) || (t.frameX >= 828 && t.frameX <= 1006) || (t.frameX >= 1296 && t.frameX <= 1330) || (t.frameX >= 1368 && t.frameX <= 1402) || (t.frameX >= 1440 && t.frameX <= 1474))) || (t.type == 467 && t.frameX / 36 == 13);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003B04 File Offset: 0x00001D04
		public static void VisualizeChestTransfer(Vector2 position, Vector2 chestPosition, int itemType, Chest.ItemTransferVisualizationSettings settings)
		{
			BitsByte bb = new BitsByte(settings.RandomizeStartPosition, settings.RandomizeEndPosition, settings.TransitionIn, settings.Fullbright, false, false, false, false);
			ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.ItemTransfer, new ParticleOrchestraSettings
			{
				PositionInWorld = position,
				MovementVector = chestPosition - position,
				UniqueInfoPiece = (itemType | (int)bb << 24)
			});
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003B6C File Offset: 0x00001D6C
		public static void VisualizeChestTransfer_CoinsBatch(Vector2 position, Vector2 chestPosition, long coinsMoved, Chest.ItemTransferVisualizationSettings settings)
		{
			int[] array = Utils.CoinsSplit(coinsMoved);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] >= 1)
				{
					Chest.VisualizeChestTransfer(position, chestPosition, 71 + i, settings);
				}
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003BA0 File Offset: 0x00001DA0
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003BA8 File Offset: 0x00001DA8
		public static bool Unlock(int X, int Y)
		{
			if (Main.tile[X, Y] == null || Main.tile[X + 1, Y] == null || Main.tile[X, Y + 1] == null || Main.tile[X + 1, Y + 1] == null)
			{
				return false;
			}
			short num = 0;
			int type = 0;
			Tile tileSafely = Framing.GetTileSafely(X, Y);
			int type2 = (int)tileSafely.type;
			int num2 = (int)(tileSafely.frameX / 36);
			if (type2 == 21)
			{
				if (num2 <= 4)
				{
					if (num2 == 2)
					{
						num = 36;
						type = 11;
						AchievementsHelper.NotifyProgressionEvent(19);
						goto IL_F9;
					}
					if (num2 == 4)
					{
						num = 36;
						type = 11;
						goto IL_F9;
					}
				}
				else if (num2 - 23 > 4)
				{
					switch (num2)
					{
					case 36:
					case 38:
					case 40:
						num = 36;
						type = 11;
						goto IL_F9;
					}
				}
				else
				{
					if (!NPC.downedPlantBoss)
					{
						return false;
					}
					num = 180;
					type = 11;
					AchievementsHelper.NotifyProgressionEvent(20);
					goto IL_F9;
				}
				return false;
			}
			if (type2 == 467)
			{
				if (num2 != 13)
				{
					return false;
				}
				if (!NPC.downedPlantBoss)
				{
					return false;
				}
				num = 36;
				type = 11;
				AchievementsHelper.NotifyProgressionEvent(20);
			}
			IL_F9:
			SoundEngine.PlaySound(22, X * 16, Y * 16, 1, 1f, 0f);
			for (int i = X; i <= X + 1; i++)
			{
				for (int j = Y; j <= Y + 1; j++)
				{
					Tile tileSafely2 = Framing.GetTileSafely(i, j);
					if ((int)tileSafely2.type == type2)
					{
						Tile tile = tileSafely2;
						tile.frameX -= num;
						Main.tile[i, j] = tileSafely2;
						for (int k = 0; k < 4; k++)
						{
							Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, type, 0f, 0f, 0, default(Color), 1f);
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003D78 File Offset: 0x00001F78
		public static bool Lock(int X, int Y)
		{
			if (Main.tile[X, Y] == null || Main.tile[X + 1, Y] == null || Main.tile[X, Y + 1] == null || Main.tile[X + 1, Y + 1] == null)
			{
				return false;
			}
			short num = 0;
			Tile tileSafely = Framing.GetTileSafely(X, Y);
			int type = (int)tileSafely.type;
			int num2 = (int)(tileSafely.frameX / 36);
			if (type == 21)
			{
				if (num2 <= 3)
				{
					if (num2 == 1)
					{
						num = 36;
						goto IL_DA;
					}
					if (num2 == 3)
					{
						num = 36;
						goto IL_DA;
					}
				}
				else if (num2 - 18 > 4)
				{
					switch (num2)
					{
					case 35:
					case 37:
					case 39:
						num = 36;
						goto IL_DA;
					}
				}
				else
				{
					if (!NPC.downedPlantBoss)
					{
						return false;
					}
					num = 180;
					goto IL_DA;
				}
				return false;
			}
			if (type == 467)
			{
				if (num2 != 12)
				{
					return false;
				}
				if (!NPC.downedPlantBoss)
				{
					return false;
				}
				num = 36;
				AchievementsHelper.NotifyProgressionEvent(20);
			}
			IL_DA:
			SoundEngine.PlaySound(22, X * 16, Y * 16, 1, 1f, 0f);
			for (int i = X; i <= X + 1; i++)
			{
				for (int j = Y; j <= Y + 1; j++)
				{
					Tile tileSafely2 = Framing.GetTileSafely(i, j);
					if ((int)tileSafely2.type == type)
					{
						Tile tile = tileSafely2;
						tile.frameX += num;
						Main.tile[i, j] = tileSafely2;
					}
				}
			}
			return true;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003ED0 File Offset: 0x000020D0
		public static int UsingChest(int i)
		{
			if (Main.chest[i] != null)
			{
				for (int j = 0; j < 255; j++)
				{
					if (Main.player[j].active && Main.player[j].chest == i)
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003F18 File Offset: 0x00002118
		public static int FindChest(int X, int Y)
		{
			Chest chest;
			if (Chest._chestsByCoords.TryGetValue(new Point(X, Y), out chest))
			{
				return chest.index;
			}
			return -1;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003F44 File Offset: 0x00002144
		public static int FindEmptyChest(int x, int y, int type = 21, int style = 0, int direction = 1, int alternate = 0)
		{
			int num = -1;
			for (int i = 0; i < 8000; i++)
			{
				Chest chest = Main.chest[i];
				if (chest != null)
				{
					if (chest.x == x && chest.y == y)
					{
						return -1;
					}
				}
				else if (num == -1)
				{
					num = i;
				}
			}
			return num;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003F8C File Offset: 0x0000218C
		public static bool NearOtherChests(int x, int y)
		{
			for (int i = x - 25; i < x + 25; i++)
			{
				for (int j = y - 8; j < y + 8; j++)
				{
					Tile tileSafely = Framing.GetTileSafely(i, j);
					if (tileSafely.active() && TileID.Sets.BasicChest[(int)tileSafely.type])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003FDC File Offset: 0x000021DC
		public static int AfterPlacement_Hook(int x, int y, int type = 21, int style = 0, int direction = 1, int alternate = 0)
		{
			Point16 point = new Point16(x, y);
			TileObjectData.OriginToTopLeft(type, style, ref point);
			int num = Chest.FindEmptyChest((int)point.X, (int)point.Y, 21, 0, 1, 0);
			if (num == -1)
			{
				return -1;
			}
			if (Main.netMode != 1)
			{
				Chest.CreateWorldChest(num, (int)point.X, (int)point.Y);
			}
			else if (type == 21)
			{
				NetMessage.SendData(34, -1, -1, null, 0, (float)x, (float)y, (float)style, 0, 0, 0);
			}
			else if (type == 467)
			{
				NetMessage.SendData(34, -1, -1, null, 4, (float)x, (float)y, (float)style, 0, 0, 0);
			}
			else
			{
				NetMessage.SendData(34, -1, -1, null, 2, (float)x, (float)y, (float)style, 0, 0, 0);
			}
			return num;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004084 File Offset: 0x00002284
		public static int CreateChest(int X, int Y, int id = -1)
		{
			int num = id;
			if (num == -1)
			{
				num = Chest.FindEmptyChest(X, Y, 21, 0, 1, 0);
				if (num == -1)
				{
					return -1;
				}
				if (Main.netMode == 1)
				{
					return num;
				}
			}
			Chest.CreateWorldChest(num, X, Y);
			return num;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000040C0 File Offset: 0x000022C0
		public static bool CanDestroyChest(int X, int Y)
		{
			Chest chest;
			if (!Chest._chestsByCoords.TryGetValue(new Point(X, Y), out chest))
			{
				return true;
			}
			for (int i = 0; i < chest.maxItems; i++)
			{
				if (chest.item[i] != null && chest.item[i].type > 0 && chest.item[i].stack > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004124 File Offset: 0x00002324
		public static bool DestroyChest(int X, int Y)
		{
			Chest chest;
			if (!Chest._chestsByCoords.TryGetValue(new Point(X, Y), out chest))
			{
				return true;
			}
			for (int i = 0; i < chest.maxItems; i++)
			{
				if (chest.item[i] != null && chest.item[i].type > 0 && chest.item[i].stack > 0)
				{
					return false;
				}
			}
			int num = chest.index;
			Chest.RemoveChest(num);
			if (Main.player[Main.myPlayer].chest == num)
			{
				Main.player[Main.myPlayer].chest = -1;
			}
			return true;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000041B8 File Offset: 0x000023B8
		public static void DestroyChestDirect(int X, int Y, int id)
		{
			if (id < 0 || id >= Main.chest.Length)
			{
				return;
			}
			try
			{
				Chest chest = Main.chest[id];
				if (chest != null)
				{
					if (chest.x == X && chest.y == Y)
					{
						Chest.RemoveChest(id);
						if (Main.player[Main.myPlayer].chest == id)
						{
							Main.player[Main.myPlayer].chest = -1;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004238 File Offset: 0x00002438
		public void AddItemToShop(Item newItem)
		{
			int num = Main.shopSellbackHelper.Remove(newItem);
			if (num >= newItem.stack)
			{
				return;
			}
			for (int i = 0; i < 39; i++)
			{
				if (this.item[i] == null || this.item[i].type == 0)
				{
					this.item[i] = newItem.Clone();
					this.item[i].favorited = false;
					this.item[i].buyOnce = true;
					this.item[i].stack -= num;
					int value = this.item[i].value;
					return;
				}
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000042D4 File Offset: 0x000024D4
		public static void SetupTravelShop_AddToShop(int itemID, ref int added, ref int count)
		{
			if (itemID == 0)
			{
				return;
			}
			added++;
			Main.travelShop[count] = itemID;
			count++;
			if (itemID == 2260)
			{
				Main.travelShop[count] = 2261;
				count++;
				Main.travelShop[count] = 2262;
				count++;
			}
			if (itemID == 5680)
			{
				Main.travelShop[count] = 5681;
				count++;
				Main.travelShop[count] = 5682;
				count++;
			}
			if (itemID == 4555)
			{
				Main.travelShop[count] = 4556;
				count++;
				Main.travelShop[count] = 4557;
				count++;
			}
			if (itemID == 4321)
			{
				Main.travelShop[count] = 4322;
				count++;
			}
			if (itemID == 4323)
			{
				Main.travelShop[count] = 4324;
				count++;
				Main.travelShop[count] = 4365;
				count++;
			}
			if (itemID == 5390)
			{
				Main.travelShop[count] = 5386;
				count++;
				Main.travelShop[count] = 5387;
				count++;
			}
			if (itemID == 4666)
			{
				Main.travelShop[count] = 4664;
				count++;
				Main.travelShop[count] = 4665;
				count++;
			}
			if (itemID == 3637)
			{
				count--;
				switch (Main.rand.Next(6))
				{
				case 0:
				{
					int[] travelShop = Main.travelShop;
					int num = count;
					count = num + 1;
					travelShop[num] = 3637;
					int[] travelShop2 = Main.travelShop;
					num = count;
					count = num + 1;
					travelShop2[num] = 3642;
					return;
				}
				case 1:
				{
					int[] travelShop3 = Main.travelShop;
					int num = count;
					count = num + 1;
					travelShop3[num] = 3621;
					int[] travelShop4 = Main.travelShop;
					num = count;
					count = num + 1;
					travelShop4[num] = 3622;
					return;
				}
				case 2:
				{
					int[] travelShop5 = Main.travelShop;
					int num = count;
					count = num + 1;
					travelShop5[num] = 3634;
					int[] travelShop6 = Main.travelShop;
					num = count;
					count = num + 1;
					travelShop6[num] = 3639;
					return;
				}
				case 3:
				{
					int[] travelShop7 = Main.travelShop;
					int num = count;
					count = num + 1;
					travelShop7[num] = 3633;
					int[] travelShop8 = Main.travelShop;
					num = count;
					count = num + 1;
					travelShop8[num] = 3638;
					return;
				}
				case 4:
				{
					int[] travelShop9 = Main.travelShop;
					int num = count;
					count = num + 1;
					travelShop9[num] = 3635;
					int[] travelShop10 = Main.travelShop;
					num = count;
					count = num + 1;
					travelShop10[num] = 3640;
					return;
				}
				case 5:
				{
					int[] travelShop11 = Main.travelShop;
					int num = count;
					count = num + 1;
					travelShop11[num] = 3636;
					int[] travelShop12 = Main.travelShop;
					num = count;
					count = num + 1;
					travelShop12[num] = 3641;
					break;
				}
				default:
					return;
				}
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000455C File Offset: 0x0000275C
		public static bool SetupTravelShop_CanAddItemToShop(int it)
		{
			if (it == 0)
			{
				return false;
			}
			for (int i = 0; i < Main.travelShop.Length; i++)
			{
				if (Main.travelShop[i] == it)
				{
					return false;
				}
				if (it == 3637)
				{
					int num = Main.travelShop[i];
					if (num - 3621 <= 1 || num - 3633 <= 9)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000045B4 File Offset: 0x000027B4
		public static void SetupTravelShop_GetPainting(Player playerWithHighestLuck, int[] rarity, ref int it, int minimumRarity = 0)
		{
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && !Main.dontStarveWorld)
			{
				it = 5121;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && !Main.dontStarveWorld)
			{
				it = 5122;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && !Main.dontStarveWorld)
			{
				it = 5124;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && !Main.dontStarveWorld)
			{
				it = 5123;
			}
			if (minimumRarity > 2)
			{
				return;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMoonlord)
			{
				it = 3596;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMartians)
			{
				it = 2865;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMartians)
			{
				it = 2866;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMartians)
			{
				it = 2867;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
			{
				it = 3055;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
			{
				it = 3056;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
			{
				it = 3057;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
			{
				it = 3058;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
			{
				it = 3059;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMoonlord)
			{
				it = 5243;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 5530;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 5633;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 5636;
			}
			if (minimumRarity > 1)
			{
				return;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0 && Main.dontStarveWorld)
			{
				it = 5121;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0 && Main.dontStarveWorld)
			{
				it = 5122;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0 && Main.dontStarveWorld)
			{
				it = 5124;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0 && Main.dontStarveWorld)
			{
				it = 5123;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5225;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5229;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5232;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5389;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5233;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5241;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5244;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5487;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5242;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5531;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000489C File Offset: 0x00002A9C
		public static void SetupTravelShop_AdjustSlotRarities(int slotItemAttempts, ref int[] rarity)
		{
			if (rarity[5] > 1 && slotItemAttempts > 4700)
			{
				rarity[5] = 1;
			}
			if (rarity[4] > 1 && slotItemAttempts > 4600)
			{
				rarity[4] = 1;
			}
			if (rarity[3] > 1 && slotItemAttempts > 4500)
			{
				rarity[3] = 1;
			}
			if (rarity[2] > 1 && slotItemAttempts > 4400)
			{
				rarity[2] = 1;
			}
			if (rarity[1] > 1 && slotItemAttempts > 4300)
			{
				rarity[1] = 1;
			}
			if (rarity[0] > 1 && slotItemAttempts > 4200)
			{
				rarity[0] = 1;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004924 File Offset: 0x00002B24
		public static void SetupTravelShop_GetItem(Player playerWithHighestLuck, int[] rarity, ref int it, int minimumRarity = 0)
		{
			if (minimumRarity <= 4 && playerWithHighestLuck.RollLuck(rarity[4]) == 0)
			{
				it = 3309;
			}
			if (minimumRarity <= 3 && playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 3314;
			}
			if (playerWithHighestLuck.RollLuck(rarity[5]) == 0)
			{
				it = 1987;
			}
			if (minimumRarity > 4)
			{
				return;
			}
			if (playerWithHighestLuck.RollLuck(rarity[4]) == 0 && Main.hardMode)
			{
				it = 2270;
			}
			if (playerWithHighestLuck.RollLuck(rarity[4]) == 0 && Main.hardMode)
			{
				it = 4760;
			}
			if (playerWithHighestLuck.RollLuck(rarity[4]) == 0)
			{
				it = 2278;
			}
			if (playerWithHighestLuck.RollLuck(rarity[4]) == 0)
			{
				it = 2271;
			}
			if (minimumRarity > 3)
			{
				return;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
			{
				it = 2223;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 2272;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 2276;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 2284;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 2285;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 2286;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 2287;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 4744;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && NPC.downedBoss3)
			{
				it = 2296;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 3628;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && Main.hardMode)
			{
				it = 4091;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 4603;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 4604;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 5297;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 4605;
			}
			if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
			{
				it = 4550;
			}
			if (minimumRarity > 2)
			{
				return;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 5680;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 2268;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && WorldGen.shadowOrbSmashed)
			{
				it = 2269;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 1988;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 2275;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 2279;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 2277;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 4555;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 4321;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 4323;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 5390;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 4549;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 4561;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 4774;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 5136;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 5305;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 4562;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 4558;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 4559;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 4563;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
			{
				it = 4666;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && (NPC.downedDeerclops || NPC.downedSlimeKing || NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedQueenBee || Main.hardMode))
			{
				it = 4347;
				if (Main.hardMode)
				{
					it = 4348;
				}
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedBoss1)
			{
				it = 3262;
			}
			if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedMechBossAny)
			{
				it = 3284;
			}
			if (minimumRarity > 1)
			{
				return;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5600;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 2267;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 2214;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 2215;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 2216;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 2217;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 3624;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 2273;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 2274;
			}
			if (minimumRarity > 0)
			{
				return;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2266;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2281 + Main.rand.Next(3);
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2258;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2242;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2260;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 3637;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 4420;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 3119;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 3118;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 3099;
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004E90 File Offset: 0x00003090
		public static void SetupTravelShop()
		{
			for (int i = 0; i < Main.travelShop.Length; i++)
			{
				Main.travelShop[i] = 0;
			}
			Player playerWithHighestLuck = Player.GetPlayerWithHighestLuck();
			int num = Main.rand.Next(4, 7);
			if (playerWithHighestLuck.RollLuck(4) == 0)
			{
				num++;
			}
			if (playerWithHighestLuck.RollLuck(8) == 0)
			{
				num++;
			}
			if (playerWithHighestLuck.RollLuck(16) == 0)
			{
				num++;
			}
			if (playerWithHighestLuck.RollLuck(32) == 0)
			{
				num++;
			}
			if (Main.expertMode && playerWithHighestLuck.RollLuck(2) == 0)
			{
				num++;
			}
			if (NPC.peddlersSatchelWasUsed)
			{
				num++;
			}
			if (Main.tenthAnniversaryWorld)
			{
				if (!Main.getGoodWorld)
				{
					num++;
				}
				num++;
			}
			int num2 = 0;
			int j = 0;
			int[] array = new int[]
			{
				100,
				200,
				300,
				400,
				500,
				600
			};
			int[] rarity = array;
			int k = 0;
			if (Main.hardMode)
			{
				int num3 = 0;
				while (k < 5000)
				{
					k++;
					Chest.SetupTravelShop_AdjustSlotRarities(k, ref rarity);
					Chest.SetupTravelShop_GetItem(playerWithHighestLuck, rarity, ref num3, 2);
					if (Chest.SetupTravelShop_CanAddItemToShop(num3))
					{
						Chest.SetupTravelShop_AddToShop(num3, ref j, ref num2);
						break;
					}
				}
			}
			while (j < num)
			{
				int num4 = 0;
				Chest.SetupTravelShop_GetItem(playerWithHighestLuck, array, ref num4, 0);
				if (Chest.SetupTravelShop_CanAddItemToShop(num4))
				{
					Chest.SetupTravelShop_AddToShop(num4, ref j, ref num2);
				}
			}
			rarity = array;
			k = 0;
			int num5 = 0;
			while (k < 5000)
			{
				k++;
				Chest.SetupTravelShop_AdjustSlotRarities(k, ref rarity);
				Chest.SetupTravelShop_GetPainting(playerWithHighestLuck, rarity, ref num5, 0);
				if (Chest.SetupTravelShop_CanAddItemToShop(num5))
				{
					Chest.SetupTravelShop_AddToShop(num5, ref j, ref num2);
					return;
				}
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000500C File Offset: 0x0000320C
		public void SetupShop(int type)
		{
			ShoppingSettings currentShoppingSettings = Main.LocalPlayer.currentShoppingSettings;
			Item[] array = this.item;
			for (int i = 0; i < this.maxItems; i++)
			{
				array[i] = new Item();
			}
			int num = 0;
			if (type == 1)
			{
				array[num].SetDefaults(88, null);
				num++;
				array[num].SetDefaults(87, null);
				num++;
				array[num].SetDefaults(35, null);
				num++;
				array[num].SetDefaults(1991, null);
				num++;
				array[num].SetDefaults(3509, null);
				num++;
				array[num].SetDefaults(3506, null);
				num++;
				array[num].SetDefaults(8, null);
				num++;
				if (Main.notTheBeesWorld && !Main.remixWorld)
				{
					array[num].SetDefaults(4388, null);
					num++;
				}
				array[num].SetDefaults(28, null);
				num++;
				if (Main.hardMode)
				{
					array[num].SetDefaults(188, null);
					num++;
				}
				array[num].SetDefaults(110, null);
				num++;
				if (Main.hardMode)
				{
					array[num].SetDefaults(189, null);
					num++;
				}
				array[num].SetDefaults(40, null);
				num++;
				array[num].SetDefaults(42, null);
				num++;
				array[num].SetDefaults(965, null);
				num++;
				if (Main.player[Main.myPlayer].ZoneSnow)
				{
					array[num].SetDefaults(967, null);
					num++;
				}
				if (Main.player[Main.myPlayer].ZoneJungle || (Main.tenthAnniversaryWorld && Main.notTheBeesWorld && !Main.remixWorld))
				{
					array[num].SetDefaults(33, null);
					num++;
				}
				if (Main.dayTime && Main.IsItAHappyWindyDay)
				{
					array[num++].SetDefaults(4074, null);
				}
				if (Main.bloodMoon)
				{
					array[num].SetDefaults(279, null);
					num++;
				}
				if (!Main.dayTime)
				{
					array[num++].SetDefaults(282, null);
				}
				if (BirthdayParty.PartyIsUp)
				{
					array[num++].SetDefaults(5643, null);
				}
				if (NPC.downedBoss3)
				{
					array[num].SetDefaults(346, null);
					num++;
				}
				if (Main.hardMode)
				{
					array[num].SetDefaults(488, null);
					num++;
				}
				for (int j = 0; j < 58; j++)
				{
					if (Main.player[Main.myPlayer].inventory[j].type == 930)
					{
						array[num].SetDefaults(931, null);
						num++;
						array[num].SetDefaults(1614, null);
						num++;
						break;
					}
				}
				array[num].SetDefaults(1786, null);
				num++;
				if (Main.hardMode)
				{
					array[num].SetDefaults(1348, null);
					num++;
				}
				if (Main.hardMode)
				{
					array[num].SetDefaults(3198, null);
					num++;
				}
				if (NPC.downedBoss2 || NPC.downedBoss3 || Main.hardMode)
				{
					array[num++].SetDefaults(4063, null);
					array[num++].SetDefaults(4673, null);
				}
				if (Main.player[Main.myPlayer].HasItem(3107))
				{
					array[num].SetDefaults(3108, null);
					num++;
				}
			}
			else if (type == 2)
			{
				array[num].SetDefaults(97, null);
				num++;
				if (Main.bloodMoon || Main.hardMode)
				{
					if (WorldGen.SavedOreTiers.Silver == 168)
					{
						array[num].SetDefaults(4915, null);
						num++;
					}
					else
					{
						array[num].SetDefaults(278, null);
						num++;
					}
				}
				if ((NPC.downedBoss2 && !Main.dayTime) || Main.hardMode)
				{
					array[num].SetDefaults(47, null);
					num++;
				}
				array[num].SetDefaults(95, null);
				num++;
				array[num].SetDefaults(98, null);
				num++;
				if (Main.player[Main.myPlayer].ZoneGraveyard && NPC.downedBoss3)
				{
					array[num++].SetDefaults(4703, null);
				}
				if (!Main.dayTime)
				{
					array[num].SetDefaults(324, null);
					num++;
				}
				if (Main.hardMode)
				{
					array[num].SetDefaults(534, null);
					num++;
				}
				if (Main.hardMode)
				{
					array[num].SetDefaults(1432, null);
					num++;
				}
				if (Main.hardMode)
				{
					array[num].SetDefaults(2177, null);
					num++;
				}
				if (Main.player[Main.myPlayer].HasItem(1258))
				{
					array[num].SetDefaults(1261, null);
					num++;
				}
				if (Main.player[Main.myPlayer].HasItem(1835))
				{
					array[num].SetDefaults(1836, null);
					num++;
				}
				if (Main.player[Main.myPlayer].HasItem(3107))
				{
					array[num].SetDefaults(3108, null);
					num++;
				}
				if (Main.player[Main.myPlayer].HasItem(1782))
				{
					array[num].SetDefaults(1783, null);
					num++;
				}
				if (Main.player[Main.myPlayer].HasItem(1784))
				{
					array[num].SetDefaults(1785, null);
					num++;
				}
				if (Main.halloween)
				{
					array[num].SetDefaults(1736, null);
					num++;
					array[num].SetDefaults(1737, null);
					num++;
					array[num].SetDefaults(1738, null);
					num++;
				}
			}
			else if (type == 3)
			{
				if (Main.bloodMoon)
				{
					if (WorldGen.crimson)
					{
						if (!Main.remixWorld || (Main.tenthAnniversaryWorld && !Main.getGoodWorld))
						{
							array[num].SetDefaults(2886, null);
							num++;
						}
						array[num].SetDefaults(2171, null);
						num++;
						array[num].SetDefaults(4508, null);
						num++;
					}
					else
					{
						if (!Main.remixWorld || (Main.tenthAnniversaryWorld && !Main.getGoodWorld))
						{
							array[num].SetDefaults(67, null);
							num++;
						}
						array[num].SetDefaults(59, null);
						num++;
						array[num].SetDefaults(4504, null);
						num++;
					}
				}
				else
				{
					if (!Main.remixWorld || Main.infectedSeed || (Main.tenthAnniversaryWorld && !Main.getGoodWorld))
					{
						array[num].SetDefaults(66, null);
						num++;
					}
					array[num].SetDefaults(62, null);
					num++;
					array[num].SetDefaults(63, null);
					num++;
					array[num].SetDefaults(745, null);
					num++;
				}
				if (Main.hardMode && Main.player[Main.myPlayer].ZoneGraveyard)
				{
					if (WorldGen.crimson)
					{
						array[num].SetDefaults(59, null);
					}
					else
					{
						array[num].SetDefaults(2171, null);
					}
					num++;
				}
				array[num].SetDefaults(27, null);
				num++;
				array[num].SetDefaults(5309, null);
				num++;
				array[num].SetDefaults(114, null);
				num++;
				array[num].SetDefaults(1828, null);
				num++;
				array[num].SetDefaults(747, null);
				num++;
				if (Main.hardMode)
				{
					array[num].SetDefaults(746, null);
					num++;
				}
				if (Main.hardMode)
				{
					array[num].SetDefaults(369, null);
					num++;
				}
				if (Main.hardMode)
				{
					array[num].SetDefaults(4505, null);
					num++;
				}
				if (Main.player[Main.myPlayer].ZoneUnderworldHeight)
				{
					array[num++].SetDefaults(5214, null);
				}
				else if (Main.player[Main.myPlayer].ZoneGlowshroom)
				{
					array[num++].SetDefaults(194, null);
				}
				if (Main.halloween)
				{
					array[num].SetDefaults(1853, null);
					num++;
					array[num].SetDefaults(1854, null);
					num++;
				}
				array[num++].SetDefaults(3215, null);
				array[num++].SetDefaults(3216, null);
				array[num++].SetDefaults(3219, null);
				if (WorldGen.crimson)
				{
					array[num++].SetDefaults(3218, null);
				}
				else
				{
					array[num++].SetDefaults(3217, null);
				}
				array[num++].SetDefaults(3220, null);
				array[num++].SetDefaults(3221, null);
				array[num++].SetDefaults(3222, null);
				array[num++].SetDefaults(4047, null);
				array[num++].SetDefaults(4045, null);
				array[num++].SetDefaults(4044, null);
				array[num++].SetDefaults(4043, null);
				array[num++].SetDefaults(4042, null);
				array[num++].SetDefaults(4046, null);
				array[num++].SetDefaults(4041, null);
				array[num++].SetDefaults(4241, null);
				array[num++].SetDefaults(4048, null);
				if (Main.hardMode)
				{
					switch (Main.moonPhase / 2)
					{
					case 0:
						array[num++].SetDefaults(4430, null);
						array[num++].SetDefaults(4431, null);
						array[num++].SetDefaults(4432, null);
						break;
					case 1:
						array[num++].SetDefaults(4433, null);
						array[num++].SetDefaults(4434, null);
						array[num++].SetDefaults(4435, null);
						break;
					case 2:
						array[num++].SetDefaults(4436, null);
						array[num++].SetDefaults(4437, null);
						array[num++].SetDefaults(4438, null);
						break;
					default:
						array[num++].SetDefaults(4439, null);
						array[num++].SetDefaults(4440, null);
						array[num++].SetDefaults(4441, null);
						break;
					}
				}
				else
				{
					switch (Main.moonPhase / 2)
					{
					case 0:
						array[num++].SetDefaults(4430, null);
						array[num++].SetDefaults(4431, null);
						break;
					case 1:
						array[num++].SetDefaults(4433, null);
						array[num++].SetDefaults(4434, null);
						break;
					case 2:
						array[num++].SetDefaults(4436, null);
						array[num++].SetDefaults(4437, null);
						break;
					default:
						array[num++].SetDefaults(4439, null);
						array[num++].SetDefaults(4440, null);
						break;
					}
				}
				if (!Main.hardMode && Main.vampireSeed && Main.infectedSeed)
				{
					array[num++].SetDefaults(8, null);
					if (WorldGen.crimson)
					{
						array[num++].SetDefaults(4386, null);
					}
					else
					{
						array[num++].SetDefaults(4385, null);
					}
				}
			}
			else if (type == 4)
			{
				array[num].SetDefaults(168, null);
				num++;
				array[num].SetDefaults(166, null);
				num++;
				if ((NPC.downedBoss1 || NPC.downedSlimeKing) && !Main.dayTime)
				{
					array[num].SetDefaults(5542, null);
					num++;
				}
				array[num].SetDefaults(167, null);
				num++;
				if (Main.hardMode)
				{
					array[num].SetDefaults(265, null);
					num++;
				}
				array[num++].SetDefaults(5481, null);
				array[num++].SetDefaults(5464, null);
				if (Main.hardMode && NPC.downedPlantBoss && NPC.downedPirates)
				{
					array[num].SetDefaults(937, null);
					num++;
				}
				if (Main.hardMode)
				{
					array[num].SetDefaults(1347, null);
					num++;
				}
				for (int k = 0; k < 58; k++)
				{
					if (Main.player[Main.myPlayer].inventory[k].type == 4827)
					{
						array[num].SetDefaults(4827, null);
						num++;
						break;
					}
				}
				for (int l = 0; l < 58; l++)
				{
					if (Main.player[Main.myPlayer].inventory[l].type == 4824)
					{
						array[num].SetDefaults(4824, null);
						num++;
						break;
					}
				}
				for (int m = 0; m < 58; m++)
				{
					if (Main.player[Main.myPlayer].inventory[m].type == 4825)
					{
						array[num].SetDefaults(4825, null);
						num++;
						break;
					}
				}
				for (int n = 0; n < 58; n++)
				{
					if (Main.player[Main.myPlayer].inventory[n].type == 4826)
					{
						array[num].SetDefaults(4826, null);
						num++;
						break;
					}
				}
			}
			else if (type == 5)
			{
				array[num].SetDefaults(254, null);
				num++;
				array[num].SetDefaults(981, null);
				num++;
				if (Main.player[Main.myPlayer].ZoneGraveyard)
				{
					array[num].SetDefaults(5577, null);
					num++;
				}
				else if (Main.dayTime)
				{
					array[num].SetDefaults(242, null);
					num++;
				}
				if (Main.moonPhase == 0)
				{
					array[num].SetDefaults(245, null);
					num++;
					array[num].SetDefaults(246, null);
					num++;
					if (!Main.dayTime)
					{
						array[num++].SetDefaults(1288, null);
						array[num++].SetDefaults(1289, null);
					}
				}
				else if (Main.moonPhase == 1)
				{
					array[num].SetDefaults(325, null);
					num++;
					array[num].SetDefaults(326, null);
					num++;
				}
				array[num].SetDefaults(269, null);
				num++;
				array[num].SetDefaults(270, null);
				num++;
				array[num].SetDefaults(271, null);
				num++;
				if (NPC.downedClown)
				{
					array[num].SetDefaults(503, null);
					num++;
					array[num].SetDefaults(504, null);
					num++;
					array[num].SetDefaults(505, null);
					num++;
				}
				if (Main.bloodMoon)
				{
					array[num].SetDefaults(322, null);
					num++;
					if (!Main.dayTime)
					{
						array[num++].SetDefaults(3362, null);
						array[num++].SetDefaults(3363, null);
					}
				}
				if (NPC.downedAncientCultist)
				{
					if (Main.dayTime)
					{
						array[num++].SetDefaults(2856, null);
						array[num++].SetDefaults(2858, null);
					}
					else
					{
						array[num++].SetDefaults(2857, null);
						array[num++].SetDefaults(2859, null);
					}
				}
				if (NPC.AnyNPCs(441))
				{
					array[num++].SetDefaults(3242, null);
					array[num++].SetDefaults(3243, null);
					array[num++].SetDefaults(3244, null);
				}
				if (Main.player[Main.myPlayer].ZoneGraveyard)
				{
					array[num++].SetDefaults(4685, null);
					array[num++].SetDefaults(4686, null);
					array[num++].SetDefaults(4704, null);
					array[num++].SetDefaults(4705, null);
					array[num++].SetDefaults(4706, null);
					array[num++].SetDefaults(4707, null);
					array[num++].SetDefaults(4708, null);
					array[num++].SetDefaults(4709, null);
				}
				if (Main.player[Main.myPlayer].ZoneSnow)
				{
					array[num].SetDefaults(1429, null);
					num++;
				}
				if (Main.halloween)
				{
					array[num].SetDefaults(1740, null);
					num++;
				}
				if (Main.hardMode)
				{
					if (Main.moonPhase == 2)
					{
						array[num].SetDefaults(869, null);
						num++;
					}
					if (Main.moonPhase == 3)
					{
						array[num].SetDefaults(4994, null);
						num++;
						array[num].SetDefaults(4997, null);
						num++;
					}
					if (Main.moonPhase == 4)
					{
						array[num].SetDefaults(864, null);
						num++;
						array[num].SetDefaults(865, null);
						num++;
					}
					if (Main.moonPhase == 5)
					{
						array[num].SetDefaults(4995, null);
						num++;
						array[num].SetDefaults(4998, null);
						num++;
					}
					if (Main.moonPhase == 6)
					{
						array[num].SetDefaults(873, null);
						num++;
						array[num].SetDefaults(874, null);
						num++;
						array[num].SetDefaults(875, null);
						num++;
					}
					if (Main.moonPhase == 7)
					{
						array[num].SetDefaults(4996, null);
						num++;
						array[num].SetDefaults(4999, null);
						num++;
					}
				}
				if (NPC.downedFrost)
				{
					if (Main.dayTime)
					{
						array[num].SetDefaults(1275, null);
						num++;
					}
					else
					{
						array[num].SetDefaults(1276, null);
						num++;
					}
				}
				if (Main.halloween)
				{
					array[num++].SetDefaults(3246, null);
					array[num++].SetDefaults(3247, null);
				}
				if (BirthdayParty.PartyIsUp)
				{
					array[num++].SetDefaults(3730, null);
					array[num++].SetDefaults(3731, null);
					array[num++].SetDefaults(3733, null);
					array[num++].SetDefaults(3734, null);
					array[num++].SetDefaults(3735, null);
				}
				int golferScoreAccumulated = Main.LocalPlayer.golferScoreAccumulated;
				if (num < 38 && golferScoreAccumulated >= 2000)
				{
					array[num++].SetDefaults(4744, null);
				}
				array[num++].SetDefaults(5308, null);
				if (num < 38)
				{
					array[num++].SetDefaults(5630, null);
				}
			}
			else if (type == 6)
			{
				array[num].SetDefaults(128, null);
				num++;
				array[num].SetDefaults(486, null);
				num++;
				array[num].SetDefaults(398, null);
				num++;
				array[num].SetDefaults(84, null);
				num++;
				array[num].SetDefaults(407, null);
				num++;
				array[num].SetDefaults(161, null);
				num++;
				array[num++].SetDefaults(5324, null);
			}
			else if (type == 7)
			{
				array[num].SetDefaults(487, null);
				num++;
				array[num].SetDefaults(496, null);
				num++;
				array[num].SetDefaults(500, null);
				num++;
				array[num].SetDefaults(507, null);
				num++;
				array[num].SetDefaults(508, null);
				num++;
				array[num].SetDefaults(531, null);
				num++;
				array[num].SetDefaults(149, null);
				num++;
				array[num].SetDefaults(576, null);
				num++;
				array[num].SetDefaults(3186, null);
				num++;
				if (Main.hardMode && Main.bloodMoon)
				{
					array[num++].SetDefaults(5461, null);
				}
				if (Main.halloween)
				{
					array[num].SetDefaults(1739, null);
					num++;
				}
			}
			else if (type == 8)
			{
				array[num].SetDefaults(509, null);
				num++;
				array[num].SetDefaults(850, null);
				num++;
				array[num].SetDefaults(851, null);
				num++;
				array[num].SetDefaults(3612, null);
				num++;
				array[num].SetDefaults(510, null);
				num++;
				array[num].SetDefaults(530, null);
				num++;
				array[num].SetDefaults(513, null);
				num++;
				array[num].SetDefaults(538, null);
				num++;
				array[num].SetDefaults(529, null);
				num++;
				array[num].SetDefaults(541, null);
				num++;
				array[num].SetDefaults(542, null);
				num++;
				array[num].SetDefaults(543, null);
				num++;
				array[num].SetDefaults(852, null);
				num++;
				array[num].SetDefaults(853, null);
				num++;
				array[num++].SetDefaults(4261, null);
				array[num++].SetDefaults(3707, null);
				array[num].SetDefaults(2739, null);
				num++;
				array[num].SetDefaults(849, null);
				num++;
				array[num++].SetDefaults(1263, null);
				array[num++].SetDefaults(3616, null);
				array[num++].SetDefaults(3725, null);
				array[num++].SetDefaults(2799, null);
				array[num++].SetDefaults(3619, null);
				array[num++].SetDefaults(3627, null);
				array[num++].SetDefaults(3629, null);
				array[num++].SetDefaults(585, null);
				array[num++].SetDefaults(584, null);
				array[num++].SetDefaults(583, null);
				array[num++].SetDefaults(4484, null);
				array[num++].SetDefaults(4485, null);
				if (Main.player[Main.myPlayer].ZoneGraveyard)
				{
					array[num].SetDefaults(4409, null);
					num++;
				}
				if (NPC.AnyNPCs(369) && (Main.moonPhase == 1 || Main.moonPhase == 3 || Main.moonPhase == 5 || Main.moonPhase == 7))
				{
					array[num].SetDefaults(2295, null);
					num++;
				}
			}
			else if (type == 9)
			{
				array[num].SetDefaults(588, null);
				num++;
				array[num].SetDefaults(589, null);
				num++;
				array[num].SetDefaults(590, null);
				num++;
				array[num].SetDefaults(597, null);
				num++;
				array[num].SetDefaults(598, null);
				num++;
				array[num].SetDefaults(596, null);
				num++;
				for (int num2 = 1873; num2 < 1906; num2++)
				{
					array[num].SetDefaults(num2, null);
					num++;
				}
			}
			else if (type == 10)
			{
				if (NPC.downedMechBossAny)
				{
					array[num].SetDefaults(756, null);
					num++;
					array[num].SetDefaults(787, null);
					num++;
				}
				array[num].SetDefaults(868, null);
				num++;
				if (NPC.downedPlantBoss)
				{
					array[num].SetDefaults(1551, null);
					num++;
				}
				array[num].SetDefaults(1181, null);
				num++;
				array[num++].SetDefaults(5231, null);
				if (!Main.remixWorld || (Main.tenthAnniversaryWorld && !Main.getGoodWorld))
				{
					array[num++].SetDefaults(783, null);
				}
			}
			else if (type == 11)
			{
				if (!Main.remixWorld || (Main.tenthAnniversaryWorld && !Main.getGoodWorld))
				{
					array[num++].SetDefaults(779, null);
				}
				if (Main.moonPhase >= 4 && Main.hardMode)
				{
					array[num++].SetDefaults(748, null);
				}
				else
				{
					array[num++].SetDefaults(839, null);
					array[num++].SetDefaults(840, null);
					array[num++].SetDefaults(841, null);
				}
				if (NPC.downedGolemBoss)
				{
					array[num++].SetDefaults(948, null);
				}
				if (Main.hardMode)
				{
					array[num++].SetDefaults(3623, null);
				}
				array[num++].SetDefaults(3603, null);
				array[num++].SetDefaults(3604, null);
				array[num++].SetDefaults(3607, null);
				array[num++].SetDefaults(3605, null);
				array[num++].SetDefaults(3606, null);
				array[num++].SetDefaults(3608, null);
				array[num++].SetDefaults(3618, null);
				array[num++].SetDefaults(3602, null);
				array[num++].SetDefaults(3663, null);
				array[num++].SetDefaults(3609, null);
				array[num++].SetDefaults(3610, null);
				if (Main.hardMode || !Main.getGoodWorld)
				{
					array[num++].SetDefaults(995, null);
				}
				if (NPC.downedBoss1 && NPC.downedBoss2 && NPC.downedBoss3)
				{
					array[num++].SetDefaults(2203, null);
				}
				if (WorldGen.crimson)
				{
					array[num++].SetDefaults(2193, null);
				}
				else
				{
					array[num++].SetDefaults(4142, null);
				}
				if (Main.player[Main.myPlayer].ZoneGraveyard)
				{
					array[num++].SetDefaults(2192, null);
				}
				bool zoneJungle = Main.player[Main.myPlayer].ZoneJungle;
				if (zoneJungle)
				{
					array[num++].SetDefaults(2204, null);
				}
				if (zoneJungle && NPC.downedGolemBoss)
				{
					array[num++].SetDefaults(2195, null);
				}
				if (Main.player[Main.myPlayer].ZoneSnow)
				{
					array[num++].SetDefaults(2198, null);
				}
				if ((double)(Main.player[Main.myPlayer].position.Y / 16f) < Main.worldSurface * 0.3499999940395355)
				{
					array[num++].SetDefaults(2197, null);
				}
				if (!Main.remixWorld || (Main.tenthAnniversaryWorld && !Main.getGoodWorld))
				{
					if (Main.eclipse || Main.bloodMoon)
					{
						if (WorldGen.crimson)
						{
							array[num++].SetDefaults(784, null);
						}
						else
						{
							array[num++].SetDefaults(782, null);
						}
					}
					else if (Main.player[Main.myPlayer].ZoneHallow)
					{
						array[num++].SetDefaults(781, null);
					}
					else
					{
						array[num++].SetDefaults(780, null);
					}
					if (NPC.downedMoonlord)
					{
						array[num++].SetDefaults(5392, null);
						array[num++].SetDefaults(5393, null);
						array[num++].SetDefaults(5394, null);
					}
				}
				if (Main.hardMode)
				{
					array[num++].SetDefaults(1344, null);
					array[num++].SetDefaults(4472, null);
				}
				if (Main.halloween)
				{
					array[num++].SetDefaults(1742, null);
				}
			}
			else if (type == 12)
			{
				array[num++].SetDefaults(1120, null);
				array[num++].SetDefaults(5920, null);
				if (Main.halloween)
				{
					array[num++].SetDefaults(3248, null);
					array[num++].SetDefaults(1741, null);
				}
				array[num++].SetDefaults(1037, null);
				array[num++].SetDefaults(2874, null);
				if (Main.netMode == 1)
				{
					array[num++].SetDefaults(1969, null);
				}
				if (Main.moonPhase == 0)
				{
					array[num++].SetDefaults(2871, null);
					array[num++].SetDefaults(2872, null);
				}
				if (!Main.dayTime && Main.bloodMoon)
				{
					array[num++].SetDefaults(4663, null);
				}
				if (Main.player[Main.myPlayer].ZoneGraveyard)
				{
					array[num++].SetDefaults(4662, null);
				}
			}
			else if (type == 13)
			{
				array[num].SetDefaults(859, null);
				num++;
				if (Main.LocalPlayer.golferScoreAccumulated > 500)
				{
					array[num++].SetDefaults(4743, null);
				}
				array[num].SetDefaults(1000, null);
				num++;
				array[num].SetDefaults(1168, null);
				num++;
				if (Main.dayTime)
				{
					array[num].SetDefaults(1449, null);
					num++;
				}
				else
				{
					array[num].SetDefaults(4552, null);
					num++;
				}
				array[num].SetDefaults(1345, null);
				num++;
				array[num].SetDefaults(1450, null);
				num++;
				array[num++].SetDefaults(3253, null);
				array[num++].SetDefaults(4553, null);
				array[num++].SetDefaults(2700, null);
				array[num++].SetDefaults(2738, null);
				array[num++].SetDefaults(4470, null);
				array[num++].SetDefaults(4681, null);
				if (Main.player[Main.myPlayer].ZoneGraveyard)
				{
					array[num++].SetDefaults(4682, null);
				}
				if (LanternNight.LanternsUp)
				{
					array[num++].SetDefaults(4702, null);
				}
				if (Main.player[Main.myPlayer].HasItem(3548))
				{
					array[num].SetDefaults(3548, null);
					num++;
				}
				if (NPC.AnyNPCs(229))
				{
					array[num++].SetDefaults(3369, null);
				}
				if (NPC.downedGolemBoss)
				{
					array[num++].SetDefaults(3546, null);
				}
				if (Main.hardMode)
				{
					array[num].SetDefaults(3214, null);
					num++;
					array[num].SetDefaults(2868, null);
					num++;
					array[num].SetDefaults(970, null);
					num++;
					array[num].SetDefaults(971, null);
					num++;
					array[num].SetDefaults(972, null);
					num++;
					array[num].SetDefaults(973, null);
					num++;
				}
				array[num++].SetDefaults(4791, null);
				array[num++].SetDefaults(3747, null);
				array[num++].SetDefaults(3732, null);
				array[num++].SetDefaults(3742, null);
				if (BirthdayParty.PartyIsUp)
				{
					array[num++].SetDefaults(3749, null);
					array[num++].SetDefaults(3746, null);
					array[num++].SetDefaults(3739, null);
					array[num++].SetDefaults(3740, null);
					array[num++].SetDefaults(3741, null);
					array[num++].SetDefaults(3737, null);
					array[num++].SetDefaults(3738, null);
					array[num++].SetDefaults(3736, null);
					array[num++].SetDefaults(3745, null);
					array[num++].SetDefaults(3744, null);
					array[num++].SetDefaults(3743, null);
				}
			}
			else if (type == 14)
			{
				array[num].SetDefaults(771, null);
				num++;
				if (Main.bloodMoon)
				{
					array[num].SetDefaults(772, null);
					num++;
				}
				if (!Main.dayTime || Main.eclipse)
				{
					array[num].SetDefaults(773, null);
					num++;
				}
				if (Main.eclipse)
				{
					array[num].SetDefaults(774, null);
					num++;
				}
				if (NPC.downedMartians)
				{
					array[num++].SetDefaults(4445, null);
					if (Main.bloodMoon || Main.eclipse)
					{
						array[num++].SetDefaults(4446, null);
					}
				}
				if (Main.hardMode)
				{
					array[num].SetDefaults(4459, null);
					num++;
				}
				if (Main.hardMode)
				{
					array[num].SetDefaults(760, null);
					num++;
				}
				if (Main.hardMode)
				{
					array[num].SetDefaults(1346, null);
					num++;
				}
				if (Main.hardMode)
				{
					array[num++].SetDefaults(5452, null);
					array[num++].SetDefaults(5451, null);
					array[num++].SetDefaults(5738, null);
				}
				array[num].SetDefaults(5598, null);
				num++;
				array[num].SetDefaults(5599, null);
				num++;
				if (Main.player[Main.myPlayer].ZoneGraveyard)
				{
					array[num].SetDefaults(4409, null);
					num++;
				}
				if (Main.player[Main.myPlayer].ZoneGraveyard)
				{
					array[num].SetDefaults(4392, null);
					num++;
				}
				if (Main.halloween)
				{
					array[num].SetDefaults(1743, null);
					num++;
					array[num].SetDefaults(1744, null);
					num++;
					array[num].SetDefaults(1745, null);
					num++;
				}
				if (NPC.downedMartians)
				{
					array[num++].SetDefaults(2862, null);
					array[num++].SetDefaults(3109, null);
				}
				if (Main.player[Main.myPlayer].HasItem(3384) || Main.player[Main.myPlayer].HasItem(3664))
				{
					array[num].SetDefaults(3664, null);
					num++;
				}
				array[num].SetDefaults(5928, null);
				num++;
			}
			else if (type == 15)
			{
				array[num].SetDefaults(1071, null);
				num++;
				array[num].SetDefaults(1072, null);
				num++;
				array[num].SetDefaults(1100, null);
				num++;
				for (int num3 = 1073; num3 <= 1084; num3++)
				{
					array[num].SetDefaults(num3, null);
					num++;
				}
				array[num].SetDefaults(1097, null);
				num++;
				array[num].SetDefaults(1099, null);
				num++;
				array[num].SetDefaults(1098, null);
				num++;
				array[num].SetDefaults(1966, null);
				num++;
				if (Main.hardMode)
				{
					array[num].SetDefaults(1967, null);
					num++;
					array[num].SetDefaults(1968, null);
					num++;
				}
				if (Main.player[Main.myPlayer].ZoneGraveyard)
				{
					array[num].SetDefaults(4668, null);
					num++;
					if (NPC.downedPlantBoss || NPC.AnyNPCs(124))
					{
						array[num].SetDefaults(5344, null);
						num++;
					}
				}
			}
			else if (type == 25)
			{
				if (Main.xMas)
				{
					int num4 = 1948;
					while (num4 <= 1957 && num < 39)
					{
						array[num].SetDefaults(num4, null);
						num4++;
						num++;
					}
				}
				int num5 = 2158;
				while (num5 <= 2160 && num < 39)
				{
					array[num].SetDefaults(num5, null);
					num5++;
					num++;
				}
				int num6 = 2008;
				while (num6 <= 2014 && num < 39)
				{
					array[num].SetDefaults(num6, null);
					num6++;
					num++;
				}
				if (!Main.player[Main.myPlayer].ZoneGraveyard)
				{
					array[num].SetDefaults(1490, null);
					num++;
					if (Main.moonPhase <= 1)
					{
						array[num].SetDefaults(1481, null);
						num++;
					}
					else if (Main.moonPhase <= 3)
					{
						array[num].SetDefaults(1482, null);
						num++;
					}
					else if (Main.moonPhase <= 5)
					{
						array[num].SetDefaults(1483, null);
						num++;
					}
					else
					{
						array[num].SetDefaults(1484, null);
						num++;
					}
				}
				if (Main.player[Main.myPlayer].ShoppingZone_Forest)
				{
					array[num].SetDefaults(5245, null);
					num++;
				}
				if (Main.player[Main.myPlayer].ZoneCrimson)
				{
					array[num].SetDefaults(1492, null);
					num++;
				}
				if (Main.player[Main.myPlayer].ZoneCorrupt)
				{
					array[num].SetDefaults(1488, null);
					num++;
				}
				if (Main.player[Main.myPlayer].ZoneHallow)
				{
					array[num].SetDefaults(1489, null);
					num++;
				}
				if (Main.player[Main.myPlayer].ZoneJungle)
				{
					array[num].SetDefaults(1486, null);
					num++;
				}
				if (Main.player[Main.myPlayer].ZoneSnow)
				{
					array[num].SetDefaults(5491, null);
					num++;
					array[num].SetDefaults(1487, null);
					num++;
				}
				if (Main.player[Main.myPlayer].ZoneDesert)
				{
					array[num].SetDefaults(1491, null);
					num++;
				}
				if (Main.bloodMoon)
				{
					array[num].SetDefaults(1493, null);
					num++;
				}
				if (!Main.player[Main.myPlayer].ZoneGraveyard)
				{
					if ((double)(Main.player[Main.myPlayer].position.Y / 16f) < Main.worldSurface * 0.3499999940395355)
					{
						array[num].SetDefaults(1485, null);
						num++;
					}
					if ((double)(Main.player[Main.myPlayer].position.Y / 16f) < Main.worldSurface * 0.3499999940395355 && Main.hardMode)
					{
						array[num].SetDefaults(1494, null);
						num++;
					}
				}
				if (Main.IsItStorming)
				{
					array[num].SetDefaults(5251, null);
					num++;
				}
				if (Main.player[Main.myPlayer].ZoneGraveyard)
				{
					array[num].SetDefaults(4723, null);
					num++;
					array[num].SetDefaults(4724, null);
					num++;
					array[num].SetDefaults(4725, null);
					num++;
					array[num].SetDefaults(4726, null);
					num++;
					array[num].SetDefaults(4727, null);
					num++;
					array[num].SetDefaults(5257, null);
					num++;
					array[num].SetDefaults(4728, null);
					num++;
					array[num].SetDefaults(4729, null);
					num++;
				}
			}
			else if (type == 16)
			{
				array[num++].SetDefaults(1430, null);
				array[num++].SetDefaults(986, null);
				if (NPC.AnyNPCs(108))
				{
					array[num++].SetDefaults(2999, null);
				}
				if (!Main.dayTime)
				{
					array[num++].SetDefaults(1158, null);
				}
				if (Main.hardMode && NPC.downedPlantBoss)
				{
					array[num++].SetDefaults(1159, null);
					array[num++].SetDefaults(1160, null);
					array[num++].SetDefaults(1161, null);
					if (Main.player[Main.myPlayer].ZoneJungle)
					{
						array[num++].SetDefaults(1167, null);
					}
					array[num++].SetDefaults(1339, null);
				}
				if (Main.hardMode && Main.player[Main.myPlayer].ZoneJungle)
				{
					array[num++].SetDefaults(1171, null);
					if (!Main.dayTime && NPC.downedPlantBoss)
					{
						array[num++].SetDefaults(1162, null);
					}
				}
				array[num++].SetDefaults(909, null);
				array[num++].SetDefaults(910, null);
				array[num++].SetDefaults(940, null);
				array[num++].SetDefaults(941, null);
				array[num++].SetDefaults(942, null);
				array[num++].SetDefaults(943, null);
				array[num++].SetDefaults(944, null);
				array[num++].SetDefaults(945, null);
				array[num++].SetDefaults(4922, null);
				array[num++].SetDefaults(4417, null);
				if (Main.player[Main.myPlayer].HasItem(1835))
				{
					array[num++].SetDefaults(1836, null);
				}
				if (Main.player[Main.myPlayer].HasItem(1258))
				{
					array[num++].SetDefaults(1261, null);
				}
				if (Main.halloween)
				{
					array[num++].SetDefaults(1791, null);
				}
			}
			else if (type == 17)
			{
				array[num].SetDefaults(928, null);
				num++;
				array[num].SetDefaults(929, null);
				num++;
				array[num].SetDefaults(876, null);
				num++;
				array[num].SetDefaults(877, null);
				num++;
				array[num].SetDefaults(878, null);
				num++;
				array[num++].SetDefaults(2434, null);
				if (Main.player[Main.myPlayer].ZoneGraveyard)
				{
					array[num++].SetDefaults(5926, null);
				}
				int num7 = (int)((Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f);
				if ((double)(Main.screenPosition.Y / 16f) < Main.worldSurface + 10.0 && (num7 < 380 || num7 > Main.maxTilesX - 380))
				{
					array[num].SetDefaults(1180, null);
					num++;
				}
				if (Main.hardMode && NPC.downedMechBossAny && NPC.AnyNPCs(208))
				{
					array[num].SetDefaults(1337, null);
					num++;
				}
			}
			else if (type == 18)
			{
				array[num].SetDefaults(1990, null);
				num++;
				array[num].SetDefaults(1979, null);
				num++;
				if (Main.player[Main.myPlayer].statLifeMax >= 400)
				{
					array[num].SetDefaults(1977, null);
					num++;
				}
				if (Main.player[Main.myPlayer].statManaMax >= 200)
				{
					array[num].SetDefaults(1978, null);
					num++;
				}
				long num8 = 0L;
				for (int num9 = 0; num9 < 54; num9++)
				{
					if (Main.player[Main.myPlayer].inventory[num9].type == 71)
					{
						num8 += (long)Main.player[Main.myPlayer].inventory[num9].stack;
					}
					if (Main.player[Main.myPlayer].inventory[num9].type == 72)
					{
						num8 += (long)(Main.player[Main.myPlayer].inventory[num9].stack * 100);
					}
					if (Main.player[Main.myPlayer].inventory[num9].type == 73)
					{
						num8 += (long)(Main.player[Main.myPlayer].inventory[num9].stack * 10000);
					}
					if (Main.player[Main.myPlayer].inventory[num9].type == 74)
					{
						num8 += (long)(Main.player[Main.myPlayer].inventory[num9].stack * 1000000);
					}
				}
				if (num8 >= 1000000L)
				{
					array[num].SetDefaults(1980, null);
					num++;
				}
				if ((Main.moonPhase % 2 == 0 && Main.dayTime) || (Main.moonPhase % 2 == 1 && !Main.dayTime))
				{
					array[num].SetDefaults(1981, null);
					num++;
				}
				if (Main.player[Main.myPlayer].team != 0 && Main.netMode == 1)
				{
					array[num].SetDefaults(1982, null);
					num++;
				}
				if (Main.hardMode)
				{
					array[num].SetDefaults(1983, null);
					num++;
				}
				if (NPC.AnyNPCs(208))
				{
					array[num].SetDefaults(1984, null);
					num++;
				}
				if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
				{
					array[num].SetDefaults(1985, null);
					num++;
				}
				if (Main.hardMode && NPC.downedMechBossAny)
				{
					array[num].SetDefaults(1986, null);
					num++;
				}
				if (Main.hardMode && NPC.downedMartians)
				{
					array[num].SetDefaults(2863, null);
					num++;
					array[num].SetDefaults(3259, null);
					num++;
				}
				array[num++].SetDefaults(5104, null);
			}
			else if (type == 19)
			{
				Player localPlayer = Main.LocalPlayer;
				if (localPlayer.HasItemInAnyInventory(5667) || localPlayer.HasItemInAnyInventory(5663) || localPlayer.HasItemInAnyInventory(5664) || localPlayer.HasItemInAnyInventory(5665) || localPlayer.HasItemInAnyInventory(5666))
				{
					array[num].SetDefaults(5735, null);
					num++;
					array[num].SetDefaults(5736, null);
					num++;
				}
				for (int num10 = 0; num10 < this.maxItems; num10++)
				{
					if (Main.travelShop[num10] != 0)
					{
						array[num].SetDefaults(Main.travelShop[num10], null);
						num++;
					}
				}
			}
			else if (type == 20)
			{
				if (Main.moonPhase == 0)
				{
					array[num].SetDefaults(284, null);
					num++;
				}
				if (Main.moonPhase == 1)
				{
					array[num].SetDefaults(946, null);
					num++;
				}
				if (Main.moonPhase == 2 && !Main.remixWorld)
				{
					array[num].SetDefaults(3069, null);
					num++;
				}
				if (Main.moonPhase == 2 && Main.remixWorld)
				{
					array[num].SetDefaults(517, null);
					num++;
				}
				if (Main.moonPhase == 3)
				{
					array[num].SetDefaults(4341, null);
					num++;
				}
				if (Main.moonPhase == 4)
				{
					array[num].SetDefaults(285, null);
					num++;
				}
				if (Main.moonPhase == 5)
				{
					array[num].SetDefaults(953, null);
					num++;
				}
				if (Main.moonPhase == 6)
				{
					array[num].SetDefaults(3068, null);
					num++;
				}
				if (Main.moonPhase == 7)
				{
					array[num].SetDefaults(3084, null);
					num++;
				}
				if (Main.moonPhase % 2 == 0)
				{
					array[num].SetDefaults(3001, null);
					num++;
				}
				if (Main.moonPhase % 2 != 0)
				{
					array[num].SetDefaults(28, null);
					num++;
				}
				if (Main.moonPhase % 2 != 0 && Main.hardMode)
				{
					array[num].SetDefaults(188, null);
					num++;
				}
				if (!Main.dayTime || Main.moonPhase == 0)
				{
					array[num].SetDefaults(3002, null);
					num++;
					if (Main.player[Main.myPlayer].HasItem(930))
					{
						array[num].SetDefaults(5377, null);
						num++;
					}
				}
				else if (Main.dayTime && Main.moonPhase != 0)
				{
					array[num].SetDefaults(282, null);
					num++;
				}
				if (Main.time % 60.0 * 60.0 * 6.0 <= 10800.0)
				{
					array[num].SetDefaults(3004, null);
				}
				else
				{
					array[num].SetDefaults(8, null);
				}
				num++;
				if (Main.moonPhase == 0 || Main.moonPhase == 1 || Main.moonPhase == 4 || Main.moonPhase == 5)
				{
					array[num].SetDefaults(3003, null);
				}
				else
				{
					array[num].SetDefaults(40, null);
				}
				num++;
				if (Main.moonPhase % 4 == 0)
				{
					array[num++].SetDefaults(3310, null);
				}
				else if (Main.moonPhase % 4 == 1)
				{
					array[num++].SetDefaults(3313, null);
				}
				else if (Main.moonPhase % 4 == 2)
				{
					array[num++].SetDefaults(3312, null);
				}
				else
				{
					array[num++].SetDefaults(3311, null);
				}
				if (Main.moonPhase == 1 || Main.moonPhase == 2)
				{
					array[num++].SetDefaults(5640, null);
				}
				else if (Main.moonPhase == 3 || Main.moonPhase == 5)
				{
					array[num++].SetDefaults(5641, null);
				}
				else if (Main.moonPhase == 6 || Main.moonPhase == 7)
				{
					array[num++].SetDefaults(5642, null);
				}
				array[num].SetDefaults(166, null);
				num++;
				array[num].SetDefaults(965, null);
				num++;
				if (Main.hardMode)
				{
					if (Main.moonPhase < 4)
					{
						array[num].SetDefaults(3316, null);
					}
					else
					{
						array[num].SetDefaults(3315, null);
					}
					num++;
					array[num].SetDefaults(3334, null);
					num++;
					if (NPC.downedMechBossAny)
					{
						array[num].SetDefaults(5540, null);
						num++;
					}
					if (Main.bloodMoon)
					{
						array[num].SetDefaults(3258, null);
						num++;
					}
				}
				if (Main.moonPhase == 0 && !Main.dayTime)
				{
					array[num].SetDefaults(3043, null);
					num++;
				}
				if (!Main.player[Main.myPlayer].ateArtisanBread && Main.moonPhase >= 3 && Main.moonPhase <= 5)
				{
					array[num].SetDefaults(5326, null);
					num++;
				}
			}
			else if (type == 21)
			{
				bool flag = Main.hardMode && NPC.downedMechBossAny;
				object obj = Main.hardMode && NPC.downedGolemBoss;
				array[num].SetDefaults(353, null);
				num++;
				array[num].SetDefaults(3828, null);
				object obj2 = obj;
				if (obj2 != null)
				{
					array[num].shopCustomPrice = new int?(Item.buyPrice(0, 4, 0, 0));
				}
				else if (flag)
				{
					array[num].shopCustomPrice = new int?(Item.buyPrice(0, 1, 0, 0));
				}
				else
				{
					array[num].shopCustomPrice = new int?(Item.buyPrice(0, 0, 25, 0));
				}
				num++;
				array[num].SetDefaults(3816, null);
				num++;
				array[num].SetDefaults(3813, null);
				array[num].shopCustomPrice = new int?(50);
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				num = 10;
				array[num].SetDefaults(3818, null);
				array[num].shopCustomPrice = new int?(5);
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3824, null);
				array[num].shopCustomPrice = new int?(5);
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3832, null);
				array[num].shopCustomPrice = new int?(5);
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3829, null);
				array[num].shopCustomPrice = new int?(5);
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				if (flag)
				{
					num = 20;
					array[num].SetDefaults(3819, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3825, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3833, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3830, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				}
				if (obj2 != null)
				{
					num = 30;
					array[num].SetDefaults(3820, null);
					array[num].shopCustomPrice = new int?(60);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3826, null);
					array[num].shopCustomPrice = new int?(60);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3834, null);
					array[num].shopCustomPrice = new int?(60);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3831, null);
					array[num].shopCustomPrice = new int?(60);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				}
				if (flag)
				{
					num = 4;
					array[num].SetDefaults(3800, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3801, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3802, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					num = 14;
					array[num].SetDefaults(3797, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3798, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3799, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					num = 24;
					array[num].SetDefaults(3803, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3804, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3805, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					num = 34;
					array[num].SetDefaults(3806, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3807, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3808, null);
					array[num].shopCustomPrice = new int?(15);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
				}
				if (obj2 != null)
				{
					num = 7;
					array[num].SetDefaults(3871, null);
					array[num].shopCustomPrice = new int?(50);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3872, null);
					array[num].shopCustomPrice = new int?(50);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3873, null);
					array[num].shopCustomPrice = new int?(50);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					num = 17;
					array[num].SetDefaults(3874, null);
					array[num].shopCustomPrice = new int?(50);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3875, null);
					array[num].shopCustomPrice = new int?(50);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3876, null);
					array[num].shopCustomPrice = new int?(50);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					num = 27;
					array[num].SetDefaults(3877, null);
					array[num].shopCustomPrice = new int?(50);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3878, null);
					array[num].shopCustomPrice = new int?(50);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3879, null);
					array[num].shopCustomPrice = new int?(50);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					num = 37;
					array[num].SetDefaults(3880, null);
					array[num].shopCustomPrice = new int?(50);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3881, null);
					array[num].shopCustomPrice = new int?(50);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
					array[num].SetDefaults(3882, null);
					array[num].shopCustomPrice = new int?(50);
					array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
					num++;
				}
				if (obj2 != null)
				{
					num = 40;
				}
				else if (flag)
				{
					num = 30;
				}
				else
				{
					num = 4;
				}
			}
			else if (type == 22)
			{
				array[num++].SetDefaults(4587, null);
				array[num++].SetDefaults(4590, null);
				array[num++].SetDefaults(4589, null);
				array[num++].SetDefaults(4588, null);
				array[num++].SetDefaults(4083, null);
				array[num++].SetDefaults(4084, null);
				array[num++].SetDefaults(4085, null);
				array[num++].SetDefaults(4086, null);
				array[num++].SetDefaults(4087, null);
				array[num++].SetDefaults(4088, null);
				int golferScoreAccumulated2 = Main.LocalPlayer.golferScoreAccumulated;
				if (golferScoreAccumulated2 > 500)
				{
					array[num].SetDefaults(4039, null);
					num++;
					array[num].SetDefaults(4094, null);
					num++;
					array[num].SetDefaults(4093, null);
					num++;
					array[num].SetDefaults(4092, null);
					num++;
				}
				array[num++].SetDefaults(4089, null);
				array[num++].SetDefaults(3989, null);
				array[num++].SetDefaults(4095, null);
				array[num++].SetDefaults(4040, null);
				array[num++].SetDefaults(4319, null);
				array[num++].SetDefaults(4320, null);
				if (golferScoreAccumulated2 > 1000)
				{
					array[num].SetDefaults(4591, null);
					num++;
					array[num].SetDefaults(4594, null);
					num++;
					array[num].SetDefaults(4593, null);
					num++;
					array[num].SetDefaults(4592, null);
					num++;
				}
				array[num++].SetDefaults(4135, null);
				array[num++].SetDefaults(4138, null);
				array[num++].SetDefaults(4136, null);
				array[num++].SetDefaults(4137, null);
				array[num++].SetDefaults(4049, null);
				if (golferScoreAccumulated2 > 500)
				{
					array[num].SetDefaults(4265, null);
					num++;
				}
				if (golferScoreAccumulated2 > 2000)
				{
					array[num].SetDefaults(4595, null);
					num++;
					array[num].SetDefaults(4598, null);
					num++;
					array[num].SetDefaults(4597, null);
					num++;
					array[num].SetDefaults(4596, null);
					num++;
					if (NPC.downedBoss3)
					{
						array[num].SetDefaults(4264, null);
						num++;
					}
				}
				if (golferScoreAccumulated2 > 500)
				{
					array[num].SetDefaults(4599, null);
					num++;
				}
				if (golferScoreAccumulated2 >= 1000)
				{
					array[num].SetDefaults(4600, null);
					num++;
				}
				if (golferScoreAccumulated2 >= 2000)
				{
					array[num].SetDefaults(4601, null);
					num++;
				}
				if (golferScoreAccumulated2 >= 2000)
				{
					if (Main.moonPhase == 0 || Main.moonPhase == 1)
					{
						array[num].SetDefaults(4658, null);
						num++;
					}
					else if (Main.moonPhase == 2 || Main.moonPhase == 3)
					{
						array[num].SetDefaults(4659, null);
						num++;
					}
					else if (Main.moonPhase == 4 || Main.moonPhase == 5)
					{
						array[num].SetDefaults(4660, null);
						num++;
					}
					else if (Main.moonPhase == 6 || Main.moonPhase == 7)
					{
						array[num].SetDefaults(4661, null);
						num++;
					}
				}
			}
			else if (type == 23)
			{
				BestiaryUnlockProgressReport bestiaryProgressReport = Main.GetBestiaryProgressReport();
				if (Chest.BestiaryGirl_IsFairyTorchAvailable())
				{
					array[num++].SetDefaults(4776, null);
				}
				array[num++].SetDefaults(4767, null);
				if (Main.moonPhase == 0 && !Main.dayTime)
				{
					array[num++].SetDefaults(5253, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.45f)
				{
					array[num++].SetDefaults(5635, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.1f)
				{
					array[num++].SetDefaults(4759, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.03f)
				{
					array[num++].SetDefaults(4672, null);
				}
				array[num++].SetDefaults(4829, null);
				if (bestiaryProgressReport.CompletionPercent >= 0.25f)
				{
					array[num++].SetDefaults(4830, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.45f)
				{
					array[num++].SetDefaults(4910, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.3f)
				{
					array[num++].SetDefaults(4871, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.3f)
				{
					array[num++].SetDefaults(4907, null);
				}
				if (NPC.downedTowerSolar)
				{
					array[num++].SetDefaults(4677, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.1f)
				{
					array[num++].SetDefaults(4676, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.3f)
				{
					array[num++].SetDefaults(4762, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.25f)
				{
					array[num++].SetDefaults(4716, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.3f)
				{
					array[num++].SetDefaults(4785, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.3f)
				{
					array[num++].SetDefaults(4786, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.3f)
				{
					array[num++].SetDefaults(4787, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.3f && Main.hardMode)
				{
					array[num++].SetDefaults(4788, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.25f)
				{
					array[num++].SetDefaults(4763, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.4f)
				{
					array[num++].SetDefaults(4955, null);
				}
				if (Main.hardMode && Main.bloodMoon)
				{
					array[num++].SetDefaults(4736, null);
				}
				if (NPC.downedPlantBoss)
				{
					array[num++].SetDefaults(4701, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.5f)
				{
					array[num++].SetDefaults(4765, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.5f)
				{
					array[num++].SetDefaults(4766, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.5f)
				{
					array[num++].SetDefaults(5285, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.5f)
				{
					array[num++].SetDefaults(4777, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 0.7f)
				{
					array[num++].SetDefaults(4735, null);
				}
				if (bestiaryProgressReport.CompletionPercent >= 1f)
				{
					array[num++].SetDefaults(4951, null);
				}
				if (BirthdayParty.PartyIsUp)
				{
					array[num++].SetDefaults(5466, null);
				}
				switch (Main.moonPhase)
				{
				case 0:
				case 1:
					array[num++].SetDefaults(4768, null);
					array[num++].SetDefaults(4769, null);
					break;
				case 2:
				case 3:
					array[num++].SetDefaults(4770, null);
					array[num++].SetDefaults(4771, null);
					break;
				case 4:
				case 5:
					array[num++].SetDefaults(4772, null);
					array[num++].SetDefaults(4773, null);
					break;
				case 6:
				case 7:
					array[num++].SetDefaults(4560, null);
					array[num++].SetDefaults(4775, null);
					break;
				}
				if (Main.vampireSeed && !Main.infectedSeed)
				{
					array[num++].SetDefaults(8, null);
				}
			}
			else if (type == 24)
			{
				array[num++].SetDefaults(5071, null);
				array[num++].SetDefaults(5072, null);
				array[num++].SetDefaults(5073, null);
				array[num++].SetDefaults(5076, null);
				array[num++].SetDefaults(5077, null);
				array[num++].SetDefaults(5078, null);
				array[num++].SetDefaults(5079, null);
				array[num++].SetDefaults(5080, null);
				array[num++].SetDefaults(5081, null);
				array[num++].SetDefaults(5082, null);
				array[num++].SetDefaults(5083, null);
				array[num++].SetDefaults(5084, null);
				array[num++].SetDefaults(5085, null);
				array[num++].SetDefaults(5086, null);
				array[num++].SetDefaults(5087, null);
				array[num++].SetDefaults(5310, null);
				array[num++].SetDefaults(5222, null);
				array[num++].SetDefaults(5228, null);
				if (NPC.downedSlimeKing && NPC.downedQueenSlime)
				{
					array[num++].SetDefaults(5266, null);
				}
				if (Main.hardMode && NPC.downedMoonlord)
				{
					array[num++].SetDefaults(5044, null);
				}
				if (Main.tenthAnniversaryWorld)
				{
					array[num++].SetDefaults(1309, null);
					array[num++].SetDefaults(1859, null);
					array[num++].SetDefaults(1358, null);
					if (Main.player[Main.myPlayer].ZoneDesert)
					{
						array[num++].SetDefaults(857, null);
					}
					if (Main.bloodMoon)
					{
						array[num++].SetDefaults(4144, null);
					}
					if (Main.hardMode && NPC.downedPirates)
					{
						if (Main.moonPhase == 0 || Main.moonPhase == 1)
						{
							array[num++].SetDefaults(2584, null);
						}
						if (Main.moonPhase == 2 || Main.moonPhase == 3)
						{
							array[num++].SetDefaults(854, null);
						}
						if (Main.moonPhase == 4 || Main.moonPhase == 5)
						{
							array[num++].SetDefaults(855, null);
						}
						if (Main.moonPhase == 6 || Main.moonPhase == 7)
						{
							array[num++].SetDefaults(905, null);
						}
					}
				}
				array[num++].SetDefaults(5088, null);
			}
			bool flag2 = type != 19 && type != 20 && type != 21;
			bool flag3 = TeleportPylonsSystem.DoesPositionHaveEnoughNPCs(2, Main.LocalPlayer.Center.ToTileCoordinates16());
			if (flag2 && flag3 && !Main.player[Main.myPlayer].ZoneCorrupt && !Main.player[Main.myPlayer].ZoneCrimson)
			{
				if (!Main.player[Main.myPlayer].ZoneSnow && !Main.player[Main.myPlayer].ZoneDesert && !Main.player[Main.myPlayer].ZoneBeach && !Main.player[Main.myPlayer].ZoneJungle && !Main.player[Main.myPlayer].ZoneHallow && !Main.player[Main.myPlayer].ZoneGlowshroom)
				{
					if (Main.remixWorld)
					{
						if ((double)(Main.player[Main.myPlayer].Center.Y / 16f) > Main.rockLayer && Main.player[Main.myPlayer].Center.Y / 16f < (float)(Main.maxTilesY - 350) && num < 39)
						{
							array[num++].SetDefaults(4876, null);
						}
					}
					else if ((double)(Main.player[Main.myPlayer].Center.Y / 16f) < Main.worldSurface && num < 39)
					{
						array[num++].SetDefaults(4876, null);
					}
				}
				if (Main.player[Main.myPlayer].ZoneSnow && num < 39)
				{
					array[num++].SetDefaults(4920, null);
				}
				if (Main.player[Main.myPlayer].ZoneDesert && num < 39)
				{
					array[num++].SetDefaults(4919, null);
				}
				if (Main.player[Main.myPlayer].ZoneUnderworldHeight)
				{
					if (num < 39)
					{
						array[num++].SetDefaults(5652, null);
					}
				}
				else if (Main.remixWorld)
				{
					if (!Main.player[Main.myPlayer].ZoneSnow && !Main.player[Main.myPlayer].ZoneDesert && !Main.player[Main.myPlayer].ZoneBeach && !Main.player[Main.myPlayer].ZoneJungle && !Main.player[Main.myPlayer].ZoneHallow && (double)(Main.player[Main.myPlayer].Center.Y / 16f) >= Main.worldSurface && num < 39)
					{
						array[num++].SetDefaults(4917, null);
					}
				}
				else if (!Main.player[Main.myPlayer].ZoneSnow && !Main.player[Main.myPlayer].ZoneDesert && !Main.player[Main.myPlayer].ZoneBeach && !Main.player[Main.myPlayer].ZoneJungle && !Main.player[Main.myPlayer].ZoneHallow && !Main.player[Main.myPlayer].ZoneGlowshroom && (double)(Main.player[Main.myPlayer].Center.Y / 16f) >= Main.worldSurface && num < 39)
				{
					array[num++].SetDefaults(4917, null);
				}
				bool flag4 = Main.player[Main.myPlayer].ZoneBeach && (double)Main.player[Main.myPlayer].position.Y < Main.worldSurface * 16.0;
				if (Main.remixWorld)
				{
					float num11 = Main.player[Main.myPlayer].position.X / 16f;
					float num12 = Main.player[Main.myPlayer].position.Y / 16f;
					flag4 |= (((double)num11 < (double)Main.maxTilesX * 0.43 || (double)num11 > (double)Main.maxTilesX * 0.57) && (double)num12 > Main.rockLayer && num12 < (float)(Main.maxTilesY - 350));
				}
				if (flag4 && num < 39)
				{
					array[num++].SetDefaults(4918, null);
				}
				if (Main.player[Main.myPlayer].ZoneJungle && num < 39)
				{
					array[num++].SetDefaults(4875, null);
				}
				if (Main.player[Main.myPlayer].ZoneHallow && num < 39)
				{
					array[num++].SetDefaults(4916, null);
				}
				if (Main.player[Main.myPlayer].ZoneGlowshroom && (!Main.remixWorld || Main.player[Main.myPlayer].Center.Y / 16f < (float)(Main.maxTilesY - 200)) && num < 39)
				{
					array[num++].SetDefaults(4921, null);
				}
			}
			for (int num13 = 0; num13 < num; num13++)
			{
				array[num13].isAShopItem = true;
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00009A07 File Offset: 0x00007C07
		private static bool BestiaryGirl_IsFairyTorchAvailable()
		{
			return Chest.DidDiscoverBestiaryEntry(585) && Chest.DidDiscoverBestiaryEntry(584) && Chest.DidDiscoverBestiaryEntry(583);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00009A34 File Offset: 0x00007C34
		private static bool DidDiscoverBestiaryEntry(int npcId)
		{
			return Main.BestiaryDB.FindEntryByNPCID(npcId).UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00009A54 File Offset: 0x00007C54
		public static void AskForChestToEatItem(Vector2 worldPosition, int duration)
		{
			Point point = worldPosition.ToTileCoordinates();
			int num = Chest.FindChest(point.X, point.Y);
			if (num == -1)
			{
				return;
			}
			Chest chest = Main.chest[num];
			chest.eatingAnimationTime = Math.Max(duration, chest.eatingAnimationTime);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00009A99 File Offset: 0x00007C99
		private static RoomCheckParticle GetNewParticle()
		{
			return new RoomCheckParticle();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00009AA0 File Offset: 0x00007CA0
		public static void IndicateBlockedChest(int chestIndex)
		{
			Chest chest = Main.chest[chestIndex];
			if (chest == null)
			{
				return;
			}
			RoomCheckParticle roomCheckParticle = Chest._particlePool.RequestParticle();
			roomCheckParticle.SetBasicInfo(TextureAssets.Cd, null, Vector2.Zero, new Vector2((float)(chest.x * 16 + 16), (float)(chest.y * 16 + 16)));
			float num = 60f;
			roomCheckParticle.Delay = 0;
			float num2 = num - (float)roomCheckParticle.Delay;
			roomCheckParticle.SetTypeInfo(num2, false);
			roomCheckParticle.FadeInNormalizedTime = Utils.Remap(num - 55f, (float)roomCheckParticle.Delay, num, 0f, 1f, true);
			roomCheckParticle.FadeOutNormalizedTime = Utils.Remap(num - 20f, (float)roomCheckParticle.Delay, num, 0f, 1f, true);
			roomCheckParticle.ColorTint = new Color(255, 40, 40, 255);
			roomCheckParticle.Scale = Vector2.One * 1f;
			roomCheckParticle.Velocity = Vector2.UnitY * 0.1f;
			roomCheckParticle.AccelerationPerFrame = Vector2.UnitY * -0.4f / num2;
			Main.ParticleSystem_World_OverPlayers.Add(roomCheckParticle);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00009BD0 File Offset: 0x00007DD0
		public static void UpdateChestFrames()
		{
			int num = 8000;
			Chest._chestInUse.Clear();
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active && Main.player[i].chest >= 0 && Main.player[i].chest < num)
				{
					Chest._chestInUse.Add(Main.player[i].chest);
				}
			}
			for (int j = 0; j < num; j++)
			{
				Chest chest = Main.chest[j];
				if (chest != null)
				{
					if (Chest._chestInUse.Contains(j))
					{
						chest.frameCounter++;
					}
					else
					{
						chest.frameCounter--;
					}
					if (chest.eatingAnimationTime == 9 && chest.frame == 1)
					{
						SoundEngine.PlaySound(7, new Vector2((float)(chest.x * 16 + 16), (float)(chest.y * 16 + 16)), 1, 0f);
					}
					if (chest.eatingAnimationTime > 0)
					{
						chest.eatingAnimationTime--;
					}
					if (chest.frameCounter < chest.eatingAnimationTime)
					{
						chest.frameCounter = chest.eatingAnimationTime;
					}
					if (chest.frameCounter < 0)
					{
						chest.frameCounter = 0;
					}
					if (chest.frameCounter > 10)
					{
						chest.frameCounter = 10;
					}
					if (chest.frameCounter == 0)
					{
						chest.frame = 0;
					}
					else if (chest.frameCounter == 10)
					{
						chest.frame = 2;
					}
					else
					{
						chest.frame = 1;
					}
				}
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00009D4C File Offset: 0x00007F4C
		public void FixLoadedData()
		{
			Item[] array = this.item;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].FixAgainstExploit();
			}
		}

		// Token: 0x04000029 RID: 41
		public const float chestStackRange = 600f;

		// Token: 0x0400002A RID: 42
		public const int maxChestTypes = 52;

		// Token: 0x0400002B RID: 43
		public static int[] chestTypeToIcon = new int[52];

		// Token: 0x0400002C RID: 44
		public const int maxChestTypes2 = 38;

		// Token: 0x0400002D RID: 45
		public static int[] chestTypeToIcon2 = new int[38];

		// Token: 0x0400002E RID: 46
		public const int maxDresserTypes = 65;

		// Token: 0x0400002F RID: 47
		public static int[] dresserTypeToIcon = new int[65];

		// Token: 0x04000030 RID: 48
		public const int DefaultMaxItems = 40;

		// Token: 0x04000031 RID: 49
		public const int AbsoluteMaxItemsWeCanEverReachInAChestForNow = 200;

		// Token: 0x04000032 RID: 50
		public int maxItems;

		// Token: 0x04000033 RID: 51
		public const int MaxNameLength = 20;

		// Token: 0x04000034 RID: 52
		public Item[] item;

		// Token: 0x04000035 RID: 53
		public readonly int x;

		// Token: 0x04000036 RID: 54
		public readonly int y;

		// Token: 0x04000037 RID: 55
		public readonly int index;

		// Token: 0x04000038 RID: 56
		public bool bankChest;

		// Token: 0x04000039 RID: 57
		public string name;

		// Token: 0x0400003A RID: 58
		public int frameCounter;

		// Token: 0x0400003B RID: 59
		public int frame;

		// Token: 0x0400003C RID: 60
		public int eatingAnimationTime;

		// Token: 0x0400003D RID: 61
		private bool _itemsGotSet;

		// Token: 0x0400003E RID: 62
		private static Dictionary<Point, Chest> _chestsByCoords = new Dictionary<Point, Chest>();

		// Token: 0x0400003F RID: 63
		private static ParticlePool<RoomCheckParticle> _particlePool = new ParticlePool<RoomCheckParticle>(100, new ParticlePool<RoomCheckParticle>.ParticleInstantiator(Chest.GetNewParticle));

		// Token: 0x04000040 RID: 64
		private static HashSet<int> _chestInUse = new HashSet<int>();

		// Token: 0x020005EA RID: 1514
		public struct ItemTransferVisualizationSettings
		{
			// Token: 0x04006331 RID: 25393
			public bool RandomizeStartPosition;

			// Token: 0x04006332 RID: 25394
			public bool RandomizeEndPosition;

			// Token: 0x04006333 RID: 25395
			public bool TransitionIn;

			// Token: 0x04006334 RID: 25396
			public bool Fullbright;

			// Token: 0x04006335 RID: 25397
			public static Chest.ItemTransferVisualizationSettings PlayerToChest = new Chest.ItemTransferVisualizationSettings
			{
				RandomizeStartPosition = true,
				RandomizeEndPosition = true,
				TransitionIn = true,
				Fullbright = true
			};

			// Token: 0x04006336 RID: 25398
			public static Chest.ItemTransferVisualizationSettings Hopper = new Chest.ItemTransferVisualizationSettings
			{
				RandomizeEndPosition = true
			};
		}
	}
}
