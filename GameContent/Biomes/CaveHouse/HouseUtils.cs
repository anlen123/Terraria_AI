using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse
{
	// Token: 0x0200051C RID: 1308
	public static class HouseUtils
	{
		// Token: 0x06003679 RID: 13945 RVA: 0x006276CC File Offset: 0x006258CC
		public static HouseBuilder CreateBuilder(Point origin, StructureMap structures)
		{
			List<Rectangle> list = HouseUtils.CreateRooms(origin);
			if (list.Count == 0 || !HouseUtils.AreRoomLocationsValid(list))
			{
				return HouseBuilder.Invalid;
			}
			HouseType houseType = HouseUtils.GetHouseType(list);
			if (!HouseUtils.AreRoomsValid(list, structures, houseType))
			{
				return HouseBuilder.Invalid;
			}
			switch (houseType)
			{
			case HouseType.Wood:
				return new WoodHouseBuilder(list);
			case HouseType.Ice:
				return new IceHouseBuilder(list);
			case HouseType.Desert:
				return new DesertHouseBuilder(list);
			case HouseType.Jungle:
				return new JungleHouseBuilder(list);
			case HouseType.Mushroom:
				return new MushroomHouseBuilder(list);
			case HouseType.Granite:
				return new GraniteHouseBuilder(list);
			case HouseType.Marble:
				return new MarbleHouseBuilder(list);
			default:
				return new WoodHouseBuilder(list);
			}
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x00627768 File Offset: 0x00625968
		public static int GetMaxPossibleRoomsInABigAbandonedHouse()
		{
			if (WorldGen.SecretSeed.errorWorld.Enabled)
			{
				return 30;
			}
			return 7;
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x0062777C File Offset: 0x0062597C
		public static int GetRandomizedRoomCountInABigAbandonedHouse()
		{
			int num = 7;
			if (WorldGen.SecretSeed.errorWorld.Enabled)
			{
				num = WorldGen.genRand.Next(7, 31);
			}
			return Math.Max(0, num - WorldGen.genRand.Next(4));
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x006277B8 File Offset: 0x006259B8
		private static List<Rectangle> CreateRooms_BigAbandonedHouses(Point origin)
		{
			List<Rectangle> list = new List<Rectangle>();
			Point point;
			if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(200), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out point) || point == origin)
			{
				return new List<Rectangle>();
			}
			Rectangle rectangle = HouseUtils.FindRoom(point);
			int randomizedRoomCountInABigAbandonedHouse = HouseUtils.GetRandomizedRoomCountInABigAbandonedHouse();
			if (randomizedRoomCountInABigAbandonedHouse == 0)
			{
				return list;
			}
			for (int i = 0; i < randomizedRoomCountInABigAbandonedHouse; i++)
			{
				Rectangle rectangle2 = HouseUtils.FindRoom_BigAbandonedHouses(new Point(rectangle.Center.X, rectangle.Y + 1), rectangle);
				list.Add(rectangle2);
				rectangle = rectangle2;
			}
			for (int j = 0; j < list.Count; j++)
			{
				list[j].Y += 3;
			}
			return list;
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x0062787C File Offset: 0x00625A7C
		private static List<Rectangle> CreateRooms(Point origin)
		{
			if (WorldGen.SecretSeed.GenerateBiggerAbandonedHouses)
			{
				return HouseUtils.CreateRooms_BigAbandonedHouses(origin);
			}
			Point point;
			if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(200), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out point) || point == origin)
			{
				return new List<Rectangle>();
			}
			Rectangle rectangle = HouseUtils.FindRoom(point);
			Rectangle rectangle2 = HouseUtils.FindRoom(new Point(rectangle.Center.X, rectangle.Y + 1));
			Rectangle rectangle3 = HouseUtils.FindRoom(new Point(rectangle.Center.X, rectangle.Y + rectangle.Height + 10));
			rectangle3.Y = rectangle.Y + rectangle.Height - 1;
			double roomSolidPrecentage = HouseUtils.GetRoomSolidPrecentage(rectangle2);
			double roomSolidPrecentage2 = HouseUtils.GetRoomSolidPrecentage(rectangle3);
			rectangle.Y += 3;
			rectangle2.Y += 3;
			rectangle3.Y += 3;
			List<Rectangle> list = new List<Rectangle>();
			if (WorldGen.genRand.NextDouble() > roomSolidPrecentage + 0.2)
			{
				list.Add(rectangle2);
			}
			list.Add(rectangle);
			if (WorldGen.genRand.NextDouble() > roomSolidPrecentage2 + 0.2)
			{
				list.Add(rectangle3);
			}
			return list;
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x006279B8 File Offset: 0x00625BB8
		private static Rectangle FindRoom(Point origin)
		{
			Point point;
			bool flag = WorldUtils.Find(origin, Searches.Chain(new Searches.Left(25), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out point);
			Point point2;
			bool flag2 = WorldUtils.Find(origin, Searches.Chain(new Searches.Right(25), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out point2);
			if (!flag)
			{
				point = new Point(origin.X - 25, origin.Y);
			}
			if (!flag2)
			{
				point2 = new Point(origin.X + 25, origin.Y);
			}
			Rectangle rectangle = new Rectangle(origin.X, origin.Y, 0, 0);
			if (origin.X - point.X > point2.X - origin.X)
			{
				rectangle.X = point.X;
				rectangle.Width = Utils.Clamp<int>(point2.X - point.X, 15, 30);
			}
			else
			{
				rectangle.Width = Utils.Clamp<int>(point2.X - point.X, 15, 30);
				rectangle.X = point2.X - rectangle.Width;
			}
			Point point3;
			bool flag3 = WorldUtils.Find(point, Searches.Chain(new Searches.Up(10), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out point3);
			Point point4;
			bool flag4 = WorldUtils.Find(point2, Searches.Chain(new Searches.Up(10), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out point4);
			if (!flag3)
			{
				point3 = new Point(origin.X, origin.Y - 10);
			}
			if (!flag4)
			{
				point4 = new Point(origin.X, origin.Y - 10);
			}
			rectangle.Height = Utils.Clamp<int>(Math.Max(origin.Y - point3.Y, origin.Y - point4.Y), 8, 12);
			rectangle.Y -= rectangle.Height;
			return rectangle;
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x00627B84 File Offset: 0x00625D84
		private static Rectangle FindRoom_BigAbandonedHouses(Point origin, Rectangle lastRoom)
		{
			int minValue = 15;
			int num = 30;
			int minValue2 = 8;
			int num2 = 12;
			Point point;
			bool flag = WorldUtils.Find(origin, Searches.Chain(new Searches.Left(25), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out point);
			Point point2;
			bool flag2 = WorldUtils.Find(origin, Searches.Chain(new Searches.Right(25), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out point2);
			if (!flag)
			{
				point = new Point(origin.X - 25, origin.Y);
			}
			if (!flag2)
			{
				point2 = new Point(origin.X + 25, origin.Y);
			}
			Rectangle rectangle = new Rectangle(origin.X, origin.Y, 0, 0);
			if (WorldGen.genRand.Next(2) == 0)
			{
				if (point.X < lastRoom.Left)
				{
					point.X = lastRoom.Left;
				}
				rectangle.X = point.X;
				rectangle.Width = WorldGen.genRand.Next(minValue, num + 1);
				if (rectangle.Left <= 10)
				{
					rectangle.X = 10;
				}
				if (rectangle.Right >= Main.maxTilesX - 10)
				{
					rectangle.X = Main.maxTilesX - 10 - rectangle.Width;
				}
			}
			else
			{
				if (point2.X > lastRoom.Right)
				{
					point2.X = lastRoom.Right;
				}
				rectangle.Width = WorldGen.genRand.Next(minValue, num + 1);
				rectangle.X = point2.X - rectangle.Width;
				if (rectangle.Left <= 10)
				{
					rectangle.X = 10;
				}
				if (rectangle.Right >= Main.maxTilesX - 10)
				{
					rectangle.X = Main.maxTilesX - 10 - rectangle.Width;
				}
			}
			rectangle.Height = WorldGen.genRand.Next(minValue2, num2 + 1);
			rectangle.Y -= rectangle.Height;
			return rectangle;
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x00627D6C File Offset: 0x00625F6C
		private static double GetRoomSolidPrecentage(Rectangle room)
		{
			double num = (double)(room.Width * room.Height);
			Ref<int> @ref = new Ref<int>(0);
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsSolid(),
				new Actions.Count(@ref)
			}));
			return (double)@ref.Value / num;
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x00627DDC File Offset: 0x00625FDC
		private static bool AreRoomLocationsValid(IEnumerable<Rectangle> rooms)
		{
			foreach (Rectangle rectangle in rooms)
			{
				if (!WorldGen.InWorld(rectangle, 10))
				{
					return false;
				}
				if (rectangle.Y + rectangle.Height > Main.maxTilesY - 220)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x00627E4C File Offset: 0x0062604C
		private static HouseType GetHouseType(IEnumerable<Rectangle> rooms)
		{
			Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
			foreach (Rectangle rectangle in rooms)
			{
				WorldUtils.Gen(new Point(rectangle.X - 10, rectangle.Y - 10), new Shapes.Rectangle(rectangle.Width + 20, rectangle.Height + 20), new Actions.TileScanner(new ushort[]
				{
					0,
					59,
					147,
					1,
					161,
					53,
					396,
					397,
					368,
					367,
					60,
					70
				}).Output(dictionary));
			}
			List<Tuple<HouseType, int>> list = new List<Tuple<HouseType, int>>
			{
				Tuple.Create<HouseType, int>(HouseType.Wood, dictionary[0] + dictionary[1]),
				Tuple.Create<HouseType, int>(HouseType.Jungle, dictionary[59] + dictionary[60] * 10),
				Tuple.Create<HouseType, int>(HouseType.Mushroom, dictionary[59] + dictionary[70] * 10),
				Tuple.Create<HouseType, int>(HouseType.Ice, dictionary[147] + dictionary[161]),
				Tuple.Create<HouseType, int>(HouseType.Desert, dictionary[397] + dictionary[396] + dictionary[53]),
				Tuple.Create<HouseType, int>(HouseType.Granite, dictionary[368]),
				Tuple.Create<HouseType, int>(HouseType.Marble, dictionary[367])
			};
			Tuple<HouseType, int> tuple = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				if (tuple.Item2 < list[i].Item2)
				{
					tuple = list[i];
				}
			}
			return tuple.Item1;
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x00628008 File Offset: 0x00626208
		private static bool AreRoomsValid(IEnumerable<Rectangle> rooms, StructureMap structures, HouseType style)
		{
			foreach (Rectangle rectangle in rooms)
			{
				Point point;
				if (style != HouseType.Granite && WorldUtils.Find(new Point(rectangle.X - 2, rectangle.Y - 2), Searches.Chain(new Searches.Rectangle(rectangle.Width + 4, rectangle.Height + 4).RequireAll(false), new GenCondition[]
				{
					new Conditions.HasLava()
				}), out point))
				{
					return false;
				}
				if (WorldGen.notTheBees)
				{
					if (!structures.CanPlace(rectangle, HouseUtils.BeelistedTiles, 5))
					{
						return false;
					}
				}
				else if (!structures.CanPlace(rectangle, HouseUtils.BlacklistedTiles, 5))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04005B06 RID: 23302
		private static readonly bool[] BlacklistedTiles = TileID.Sets.Factory.CreateBoolSet(true, new int[]
		{
			225,
			41,
			43,
			44,
			226,
			203,
			112,
			25,
			151,
			21,
			467
		});

		// Token: 0x04005B07 RID: 23303
		private static readonly bool[] BeelistedTiles = TileID.Sets.Factory.CreateBoolSet(true, new int[]
		{
			41,
			43,
			44,
			226,
			203,
			112,
			25,
			151,
			21,
			467
		});
	}
}
