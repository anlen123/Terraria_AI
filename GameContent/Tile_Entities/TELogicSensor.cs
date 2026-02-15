using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities
{
	// Token: 0x0200041A RID: 1050
	public class TELogicSensor : TileEntityType<TELogicSensor>
	{
		// Token: 0x06003022 RID: 12322 RVA: 0x005B7DA7 File Offset: 0x005B5FA7
		public override void RegisterTileEntityID(int assignedID)
		{
			base.RegisterTileEntityID(assignedID);
			TileEntity._UpdateStart += TELogicSensor.UpdateStartInternal;
			TileEntity._UpdateEnd += TELogicSensor.UpdateEndInternal;
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x005B7DD2 File Offset: 0x005B5FD2
		public override void OnPlaced()
		{
			this.FigureCheckState();
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x005B7DDA File Offset: 0x005B5FDA
		public override bool IsTileValidForEntity(int x, int y)
		{
			return TELogicSensor.ValidTile(x, y);
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x005B7DE3 File Offset: 0x005B5FE3
		private static void UpdateStartInternal()
		{
			TELogicSensor.inUpdateLoop = true;
			TELogicSensor.markedIDsForRemoval.Clear();
			TELogicSensor.playerBox.Clear();
			TELogicSensor.playerBoxFilled = false;
			TELogicSensor.FillPlayerHitboxes();
		}

		// Token: 0x06003026 RID: 12326 RVA: 0x005B7E0C File Offset: 0x005B600C
		private static void FillPlayerHitboxes()
		{
			if (!TELogicSensor.playerBoxFilled)
			{
				for (int i = 0; i < 255; i++)
				{
					Player player = Main.player[i];
					if (player.active && !player.dead && !player.ghost)
					{
						TELogicSensor.playerBox[i] = player.getRect();
					}
				}
				TELogicSensor.playerBoxFilled = true;
			}
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x005B7E68 File Offset: 0x005B6068
		private static void UpdateEndInternal()
		{
			TELogicSensor.inUpdateLoop = false;
			foreach (Tuple<Point16, bool> tuple in TELogicSensor.tripPoints)
			{
				Wiring.blockPlayerTeleportationForOneIteration = tuple.Item2;
				Wiring.HitSwitch((int)tuple.Item1.X, (int)tuple.Item1.Y);
				if (Main.netMode == 2)
				{
					NetMessage.SendData(59, -1, -1, null, (int)tuple.Item1.X, (float)tuple.Item1.Y, 0f, 0f, 0, 0, 0);
				}
			}
			Wiring.blockPlayerTeleportationForOneIteration = false;
			TELogicSensor.tripPoints.Clear();
			using (List<int>.Enumerator enumerator2 = TELogicSensor.markedIDsForRemoval.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					TELogicSensor entity;
					if (TileEntity.TryGet<TELogicSensor>(enumerator2.Current, out entity))
					{
						TileEntity.Remove(entity, false);
					}
				}
			}
			TELogicSensor.markedIDsForRemoval.Clear();
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x005B7F78 File Offset: 0x005B6178
		public override void Update()
		{
			bool state = TELogicSensor.GetState((int)this.Position.X, (int)this.Position.Y, this.logicCheck, this);
			TELogicSensor.LogicCheckType logicCheckType = this.logicCheck;
			if (logicCheckType - TELogicSensor.LogicCheckType.Day > 1)
			{
				if (logicCheckType - TELogicSensor.LogicCheckType.PlayerAbove > 4)
				{
					return;
				}
				if (this.On != state)
				{
					this.ChangeState(state, true);
				}
			}
			else
			{
				if (!this.On && state)
				{
					this.ChangeState(true, true);
				}
				if (this.On && !state)
				{
					this.ChangeState(false, false);
					return;
				}
			}
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x005B7FF8 File Offset: 0x005B61F8
		public void ChangeState(bool onState, bool TripWire)
		{
			if (onState != this.On && !TELogicSensor.SanityCheck((int)this.Position.X, (int)this.Position.Y))
			{
				return;
			}
			Main.tile[(int)this.Position.X, (int)this.Position.Y].frameX = (onState ? 18 : 0);
			this.On = onState;
			if (Main.netMode == 2)
			{
				NetMessage.SendTileSquare(-1, (int)this.Position.X, (int)this.Position.Y, TileChangeType.None);
			}
			if (TripWire && Main.netMode != 1)
			{
				TELogicSensor.tripPoints.Add(Tuple.Create<Point16, bool>(this.Position, this.logicCheck == TELogicSensor.LogicCheckType.PlayerAbove));
			}
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x005B80B0 File Offset: 0x005B62B0
		public static bool ValidTile(int x, int y)
		{
			return Main.tile[x, y].active() && Main.tile[x, y].type == 423 && Main.tile[x, y].frameY % 18 == 0 && Main.tile[x, y].frameX % 18 == 0;
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x005B8117 File Offset: 0x005B6317
		public TELogicSensor()
		{
			this.logicCheck = TELogicSensor.LogicCheckType.None;
			this.On = false;
			this.RequiresUpdates = true;
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x005B8134 File Offset: 0x005B6334
		public static TELogicSensor.LogicCheckType FigureCheckType(int x, int y, out bool on)
		{
			on = false;
			if (!WorldGen.InWorld(x, y, 0))
			{
				return TELogicSensor.LogicCheckType.None;
			}
			Tile tile = Main.tile[x, y];
			if (tile == null)
			{
				return TELogicSensor.LogicCheckType.None;
			}
			TELogicSensor.LogicCheckType logicCheckType = TELogicSensor.LogicCheckType.None;
			switch (tile.frameY / 18)
			{
			case 0:
				logicCheckType = TELogicSensor.LogicCheckType.Day;
				break;
			case 1:
				logicCheckType = TELogicSensor.LogicCheckType.Night;
				break;
			case 2:
				logicCheckType = TELogicSensor.LogicCheckType.PlayerAbove;
				break;
			case 3:
				logicCheckType = TELogicSensor.LogicCheckType.Water;
				break;
			case 4:
				logicCheckType = TELogicSensor.LogicCheckType.Lava;
				break;
			case 5:
				logicCheckType = TELogicSensor.LogicCheckType.Honey;
				break;
			case 6:
				logicCheckType = TELogicSensor.LogicCheckType.Liquid;
				break;
			}
			on = TELogicSensor.GetState(x, y, logicCheckType, null);
			return logicCheckType;
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x005B81B8 File Offset: 0x005B63B8
		public static bool GetState(int x, int y, TELogicSensor.LogicCheckType type, TELogicSensor instance = null)
		{
			switch (type)
			{
			case TELogicSensor.LogicCheckType.Day:
				return Main.dayTime;
			case TELogicSensor.LogicCheckType.Night:
				return !Main.dayTime;
			case TELogicSensor.LogicCheckType.PlayerAbove:
			{
				bool result = false;
				Rectangle value = new Rectangle(x * 16 - 32 - 1, y * 16 - 160 - 1, 82, 162);
				foreach (KeyValuePair<int, Rectangle> keyValuePair in TELogicSensor.playerBox)
				{
					if (keyValuePair.Value.Intersects(value))
					{
						result = true;
						break;
					}
				}
				return result;
			}
			case TELogicSensor.LogicCheckType.Water:
			case TELogicSensor.LogicCheckType.Lava:
			case TELogicSensor.LogicCheckType.Honey:
			case TELogicSensor.LogicCheckType.Liquid:
			{
				if (instance == null)
				{
					return false;
				}
				Tile tile = Main.tile[x, y];
				bool flag = true;
				if (tile == null || tile.liquid == 0)
				{
					flag = false;
				}
				if (!tile.lava() && type == TELogicSensor.LogicCheckType.Lava)
				{
					flag = false;
				}
				if (!tile.honey() && type == TELogicSensor.LogicCheckType.Honey)
				{
					flag = false;
				}
				if ((tile.honey() || tile.lava() || tile.shimmer()) && type == TELogicSensor.LogicCheckType.Water)
				{
					flag = false;
				}
				if (!flag && instance.On)
				{
					if (instance.CountedData == 0)
					{
						instance.CountedData = 15;
					}
					else if (instance.CountedData > 0)
					{
						instance.CountedData--;
					}
					flag = (instance.CountedData > 0);
				}
				return flag;
			}
			default:
				return false;
			}
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x005B8328 File Offset: 0x005B6528
		public void FigureCheckState()
		{
			this.logicCheck = TELogicSensor.FigureCheckType((int)this.Position.X, (int)this.Position.Y, out this.On);
			TELogicSensor.GetFrame((int)this.Position.X, (int)this.Position.Y, this.logicCheck, this.On);
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x005B8384 File Offset: 0x005B6584
		public static void GetFrame(int x, int y, TELogicSensor.LogicCheckType type, bool on)
		{
			Main.tile[x, y].frameX = (on ? 18 : 0);
			switch (type)
			{
			case TELogicSensor.LogicCheckType.Day:
				Main.tile[x, y].frameY = 0;
				return;
			case TELogicSensor.LogicCheckType.Night:
				Main.tile[x, y].frameY = 18;
				return;
			case TELogicSensor.LogicCheckType.PlayerAbove:
				Main.tile[x, y].frameY = 36;
				return;
			case TELogicSensor.LogicCheckType.Water:
				Main.tile[x, y].frameY = 54;
				return;
			case TELogicSensor.LogicCheckType.Lava:
				Main.tile[x, y].frameY = 72;
				return;
			case TELogicSensor.LogicCheckType.Honey:
				Main.tile[x, y].frameY = 90;
				return;
			case TELogicSensor.LogicCheckType.Liquid:
				Main.tile[x, y].frameY = 108;
				return;
			default:
				Main.tile[x, y].frameY = 0;
				return;
			}
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x005B8471 File Offset: 0x005B6671
		public static bool SanityCheck(int x, int y)
		{
			if (!Main.tile[x, y].active() || Main.tile[x, y].type != 423)
			{
				TELogicSensor.Kill(x, y);
				return false;
			}
			return true;
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x005B84A8 File Offset: 0x005B66A8
		public static int Hook_AfterPlacement(int x, int y, int type = 423, int style = 0, int direction = 1, int alternate = 0)
		{
			bool on;
			TELogicSensor.LogicCheckType type2 = TELogicSensor.FigureCheckType(x, y, out on);
			TELogicSensor.GetFrame(x, y, type2, on);
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x, y, TileChangeType.None);
				NetMessage.SendData(87, -1, -1, null, x, (float)y, (float)TileEntityType<TELogicSensor>.EntityTypeID, 0f, 0, 0, 0);
				return -1;
			}
			return TileEntityType<TELogicSensor>.Place(x, y);
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x005B8504 File Offset: 0x005B6704
		public new static void Kill(int x, int y)
		{
			TELogicSensor telogicSensor;
			if (TileEntity.TryGetAt<TELogicSensor>(x, y, out telogicSensor))
			{
				Wiring.blockPlayerTeleportationForOneIteration = (telogicSensor.logicCheck == TELogicSensor.LogicCheckType.PlayerAbove);
				bool flag = false;
				if (telogicSensor.logicCheck == TELogicSensor.LogicCheckType.PlayerAbove && telogicSensor.On)
				{
					flag = true;
				}
				else if (telogicSensor.logicCheck == TELogicSensor.LogicCheckType.Water && telogicSensor.On)
				{
					flag = true;
				}
				else if (telogicSensor.logicCheck == TELogicSensor.LogicCheckType.Lava && telogicSensor.On)
				{
					flag = true;
				}
				else if (telogicSensor.logicCheck == TELogicSensor.LogicCheckType.Honey && telogicSensor.On)
				{
					flag = true;
				}
				else if (telogicSensor.logicCheck == TELogicSensor.LogicCheckType.Liquid && telogicSensor.On)
				{
					flag = true;
				}
				if (flag)
				{
					Wiring.HitSwitch((int)telogicSensor.Position.X, (int)telogicSensor.Position.Y);
					NetMessage.SendData(59, -1, -1, null, (int)telogicSensor.Position.X, (float)telogicSensor.Position.Y, 0f, 0f, 0, 0, 0);
				}
				Wiring.blockPlayerTeleportationForOneIteration = false;
				if (TELogicSensor.inUpdateLoop)
				{
					TELogicSensor.markedIDsForRemoval.Add(telogicSensor.ID);
					return;
				}
				TileEntity.Remove(telogicSensor, false);
			}
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x005B8607 File Offset: 0x005B6807
		public override void WriteExtraData(BinaryWriter writer, bool networkSend)
		{
			if (!networkSend)
			{
				writer.Write((byte)this.logicCheck);
				writer.Write(this.On);
			}
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x005B8625 File Offset: 0x005B6825
		public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
		{
			if (!networkSend)
			{
				this.logicCheck = (TELogicSensor.LogicCheckType)reader.ReadByte();
				this.On = reader.ReadBoolean();
			}
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x005B8644 File Offset: 0x005B6844
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.Position.X,
				"x  ",
				this.Position.Y,
				"y ",
				this.logicCheck
			});
		}

		// Token: 0x040056A2 RID: 22178
		private static Dictionary<int, Rectangle> playerBox = new Dictionary<int, Rectangle>();

		// Token: 0x040056A3 RID: 22179
		private static List<Tuple<Point16, bool>> tripPoints = new List<Tuple<Point16, bool>>();

		// Token: 0x040056A4 RID: 22180
		private static List<int> markedIDsForRemoval = new List<int>();

		// Token: 0x040056A5 RID: 22181
		private static bool inUpdateLoop;

		// Token: 0x040056A6 RID: 22182
		private static bool playerBoxFilled;

		// Token: 0x040056A7 RID: 22183
		public TELogicSensor.LogicCheckType logicCheck;

		// Token: 0x040056A8 RID: 22184
		public bool On;

		// Token: 0x040056A9 RID: 22185
		public int CountedData;

		// Token: 0x02000938 RID: 2360
		public enum LogicCheckType
		{
			// Token: 0x040074EE RID: 29934
			None,
			// Token: 0x040074EF RID: 29935
			Day,
			// Token: 0x040074F0 RID: 29936
			Night,
			// Token: 0x040074F1 RID: 29937
			PlayerAbove,
			// Token: 0x040074F2 RID: 29938
			Water,
			// Token: 0x040074F3 RID: 29939
			Lava,
			// Token: 0x040074F4 RID: 29940
			Honey,
			// Token: 0x040074F5 RID: 29941
			Liquid
		}
	}
}
