using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse
{
	// Token: 0x0200051F RID: 1311
	public class HouseBuilder
	{
		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06003689 RID: 13961 RVA: 0x0062860C File Offset: 0x0062680C
		// (set) Token: 0x0600368A RID: 13962 RVA: 0x00628614 File Offset: 0x00626814
		public double ChestChance { get; set; }

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x0600368B RID: 13963 RVA: 0x0062861D File Offset: 0x0062681D
		// (set) Token: 0x0600368C RID: 13964 RVA: 0x00628625 File Offset: 0x00626825
		public ushort TileType { get; protected set; }

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x0600368D RID: 13965 RVA: 0x0062862E File Offset: 0x0062682E
		// (set) Token: 0x0600368E RID: 13966 RVA: 0x00628636 File Offset: 0x00626836
		public ushort WallType { get; protected set; }

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x0600368F RID: 13967 RVA: 0x0062863F File Offset: 0x0062683F
		// (set) Token: 0x06003690 RID: 13968 RVA: 0x00628647 File Offset: 0x00626847
		public ushort BeamType { get; protected set; }

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06003691 RID: 13969 RVA: 0x00628650 File Offset: 0x00626850
		// (set) Token: 0x06003692 RID: 13970 RVA: 0x00628658 File Offset: 0x00626858
		public byte BeamPaint { get; protected set; }

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06003693 RID: 13971 RVA: 0x00628661 File Offset: 0x00626861
		// (set) Token: 0x06003694 RID: 13972 RVA: 0x00628669 File Offset: 0x00626869
		public int PlatformStyle { get; protected set; }

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06003695 RID: 13973 RVA: 0x00628672 File Offset: 0x00626872
		// (set) Token: 0x06003696 RID: 13974 RVA: 0x0062867A File Offset: 0x0062687A
		public int DoorStyle { get; protected set; }

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06003697 RID: 13975 RVA: 0x00628683 File Offset: 0x00626883
		// (set) Token: 0x06003698 RID: 13976 RVA: 0x0062868B File Offset: 0x0062688B
		public int TableStyle { get; protected set; }

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06003699 RID: 13977 RVA: 0x00628694 File Offset: 0x00626894
		// (set) Token: 0x0600369A RID: 13978 RVA: 0x0062869C File Offset: 0x0062689C
		public bool UsesTables2 { get; protected set; }

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x0600369B RID: 13979 RVA: 0x006286A5 File Offset: 0x006268A5
		// (set) Token: 0x0600369C RID: 13980 RVA: 0x006286AD File Offset: 0x006268AD
		public int WorkbenchStyle { get; protected set; }

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x0600369D RID: 13981 RVA: 0x006286B6 File Offset: 0x006268B6
		// (set) Token: 0x0600369E RID: 13982 RVA: 0x006286BE File Offset: 0x006268BE
		public int PianoStyle { get; protected set; }

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x0600369F RID: 13983 RVA: 0x006286C7 File Offset: 0x006268C7
		// (set) Token: 0x060036A0 RID: 13984 RVA: 0x006286CF File Offset: 0x006268CF
		public int BookcaseStyle { get; protected set; }

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x060036A1 RID: 13985 RVA: 0x006286D8 File Offset: 0x006268D8
		// (set) Token: 0x060036A2 RID: 13986 RVA: 0x006286E0 File Offset: 0x006268E0
		public int ChairStyle { get; protected set; }

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x060036A3 RID: 13987 RVA: 0x006286E9 File Offset: 0x006268E9
		// (set) Token: 0x060036A4 RID: 13988 RVA: 0x006286F1 File Offset: 0x006268F1
		public int ChestStyle { get; protected set; }

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x060036A5 RID: 13989 RVA: 0x006286FA File Offset: 0x006268FA
		// (set) Token: 0x060036A6 RID: 13990 RVA: 0x00628702 File Offset: 0x00626902
		public bool UsesContainers2 { get; protected set; }

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x060036A7 RID: 13991 RVA: 0x0062870B File Offset: 0x0062690B
		// (set) Token: 0x060036A8 RID: 13992 RVA: 0x00628713 File Offset: 0x00626913
		public ReadOnlyCollection<Rectangle> Rooms { get; private set; }

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x060036A9 RID: 13993 RVA: 0x0062871C File Offset: 0x0062691C
		public Rectangle TopRoom
		{
			get
			{
				return this.Rooms.First<Rectangle>();
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x060036AA RID: 13994 RVA: 0x00628729 File Offset: 0x00626929
		public Rectangle BottomRoom
		{
			get
			{
				return this.Rooms.Last<Rectangle>();
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x060036AB RID: 13995 RVA: 0x004DD195 File Offset: 0x004DB395
		private UnifiedRandom _random
		{
			get
			{
				return WorldGen.genRand;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x060036AC RID: 13996 RVA: 0x004DD19C File Offset: 0x004DB39C
		private Tile[,] _tiles
		{
			get
			{
				return Main.tile;
			}
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x00628736 File Offset: 0x00626936
		private HouseBuilder()
		{
			this.IsValid = false;
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x0062875C File Offset: 0x0062695C
		protected HouseBuilder(HouseType type, IEnumerable<Rectangle> rooms)
		{
			this.Type = type;
			this.IsValid = true;
			List<Rectangle> list = rooms.ToList<Rectangle>();
			list.Sort((Rectangle lhs, Rectangle rhs) => lhs.Top.CompareTo(rhs.Top));
			this.Rooms = list.AsReadOnly();
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void AgeRoom(Rectangle room)
		{
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x006287CC File Offset: 0x006269CC
		public void PotentiallyConvertToRainbowMossBlock()
		{
			if (WorldGen.SecretSeed.rainbowStuff.Enabled && WorldGen.genRand.Next(2) == 0)
			{
				this.TileType = 692;
				this.WallType = 346;
				this.PlatformStyle = 43;
				this.DoorStyle = 44;
			}
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x00628818 File Offset: 0x00626A18
		public void PotentiallyConvertToRainbowBrick()
		{
			if (Main.tenthAnniversaryWorld)
			{
				if (Main.getGoodWorld)
				{
					if (WorldGen.genRand.Next(7) == 0)
					{
						this.TileType = 160;
						this.WallType = 44;
						return;
					}
				}
				else if (WorldGen.genRand.Next(2) == 0)
				{
					this.TileType = 160;
					this.WallType = 44;
				}
			}
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x00628874 File Offset: 0x00626A74
		public void RainbowifyOnTenthAnniversaryWorlds()
		{
			if (!Main.tenthAnniversaryWorld || (this.TileType == 160 && WorldGen.genRand.Next(2) == 0))
			{
				return;
			}
			foreach (Rectangle rectangle in this.Rooms)
			{
				WorldUtils.Gen(new Point(rectangle.X, rectangle.Y), new Shapes.Rectangle(rectangle.Width, rectangle.Height), new Actions.SetTileAndWallRainbowPaint());
			}
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x0062890C File Offset: 0x00626B0C
		public void PotentiallyConvertToSeedHouse()
		{
			if (WorldGen.SecretSeed.errorWorld.Enabled)
			{
				this.PlatformStyle = WorldGen.genRand.Next(49);
				this.DoorStyle = WorldGen.genRand.Next(49);
				this.TableStyle = WorldGen.genRand.Next(35);
				this.WorkbenchStyle = WorldGen.genRand.Next(44);
				this.PianoStyle = WorldGen.genRand.Next(39);
				this.BookcaseStyle = WorldGen.genRand.Next(40);
				this.ChairStyle = WorldGen.genRand.Next(44);
				switch (WorldGen.genRand.Next(20))
				{
				default:
					this.TileType = 159;
					this.WallType = 43;
					return;
				case 1:
					this.TileType = 422;
					this.WallType = 225;
					return;
				case 2:
					this.TileType = 194;
					this.WallType = 75;
					return;
				case 3:
					this.TileType = 541;
					this.WallType = 318;
					this.PlatformStyle = 48;
					return;
				case 4:
					this.TileType = 137;
					this.WallType = 147;
					return;
				case 5:
					this.TileType = 48;
					this.WallType = 245;
					return;
				case 6:
					this.TileType = 370;
					this.WallType = 182;
					return;
				case 7:
					this.TileType = 140;
					this.WallType = 33;
					return;
				case 8:
					this.TileType = 347;
					this.WallType = 174;
					return;
				case 9:
					this.TileType = 508;
					this.WallType = 243;
					return;
				case 10:
					this.TileType = 507;
					this.WallType = 242;
					return;
				case 11:
					this.TileType = 546;
					this.WallType = 167;
					return;
				case 12:
					this.TileType = 329;
					this.WallType = 169;
					return;
				case 13:
					this.TileType = 326;
					this.WallType = 136;
					return;
				case 14:
					this.TileType = 327;
					this.WallType = 137;
					return;
				case 15:
					this.TileType = 345;
					this.WallType = 172;
					return;
				case 16:
					this.TileType = 708;
					this.WallType = 347;
					return;
				case 17:
					this.TileType = 501;
					this.WallType = 238;
					return;
				case 18:
					this.TileType = 272;
					this.WallType = 225;
					return;
				case 19:
					this.TileType = 421;
					this.WallType = 225;
					return;
				}
			}
			else
			{
				if (WorldGen.genRand.NextFloat() > 0.4f)
				{
					return;
				}
				bool flag = this.Type == HouseType.Wood;
				bool flag2 = this.Type == HouseType.Desert;
				bool flag3 = this.Type == HouseType.Jungle;
				bool flag4 = this.Type == HouseType.Ice;
				List<ushort> list = new List<ushort>();
				if (flag3 && Main.notTheBeesWorld && Main.tenthAnniversaryWorld)
				{
					list.Add(562);
					list.Add(563);
					list.Add(229);
				}
				if ((flag || flag4) && Main.drunkWorld && Main.tenthAnniversaryWorld)
				{
					if (flag4)
					{
						list.Add(197);
					}
					else
					{
						list.Add(193);
					}
				}
				if (flag4 && WorldGen.SecretSeed.worldIsFrozen.Enabled && WorldGen.genRand.Next(3) == 0)
				{
					list.Add(145);
					list.Add(146);
				}
				if (flag2 && Main.remixWorld && Main.getGoodWorld)
				{
					list.Add(188);
				}
				if (list.Count <= 0)
				{
					return;
				}
				ushort num = list[WorldGen.genRand.Next(list.Count)];
				if (num <= 193)
				{
					if (num <= 146)
					{
						if (num == 145)
						{
							this.TileType = num;
							this.WallType = 29;
							this.BeamType = 574;
							this.BeamPaint = 26;
							return;
						}
						if (num != 146)
						{
							return;
						}
						this.TileType = num;
						this.WallType = 30;
						this.BeamType = 574;
						this.BeamPaint = 26;
						return;
					}
					else
					{
						if (num == 188)
						{
							this.TileType = num;
							this.WallType = 72;
							this.BeamType = 124;
							this.BeamPaint = 17;
							this.PlatformStyle = 25;
							this.DoorStyle = 4;
							this.TableStyle = 30;
							this.UsesTables2 = false;
							this.WorkbenchStyle = 5;
							this.PianoStyle = 17;
							this.BookcaseStyle = 6;
							this.ChairStyle = 6;
							this.ChestStyle = 42;
							this.UsesContainers2 = false;
							return;
						}
						if (num != 193)
						{
							return;
						}
						this.TileType = num;
						this.WallType = 76;
						this.BeamType = 124;
						this.BeamPaint = 19;
						this.PlatformStyle = 20;
						this.DoorStyle = 31;
						this.TableStyle = 29;
						this.UsesTables2 = false;
						this.WorkbenchStyle = 8;
						this.PianoStyle = 24;
						this.BookcaseStyle = 26;
						this.ChairStyle = 31;
						this.ChestStyle = 34;
						this.UsesContainers2 = false;
						return;
					}
				}
				else if (num <= 229)
				{
					if (num == 197)
					{
						this.TileType = num;
						this.WallType = 76;
						this.BeamType = 574;
						this.BeamPaint = 26;
						this.PlatformStyle = 20;
						this.DoorStyle = 31;
						this.TableStyle = 29;
						this.UsesTables2 = false;
						this.WorkbenchStyle = 8;
						this.PianoStyle = 24;
						this.BookcaseStyle = 26;
						this.ChairStyle = 31;
						this.ChestStyle = 34;
						this.UsesContainers2 = false;
						return;
					}
					if (num != 229)
					{
						return;
					}
					this.TileType = num;
					this.WallType = 86;
					this.BeamType = 575;
					this.BeamPaint = 15;
					this.PlatformStyle = 24;
					this.DoorStyle = 22;
					this.TableStyle = 19;
					this.UsesTables2 = false;
					this.WorkbenchStyle = 19;
					this.PianoStyle = 9;
					this.BookcaseStyle = 9;
					this.ChairStyle = 22;
					this.ChestStyle = 29;
					this.UsesContainers2 = false;
					return;
				}
				else
				{
					if (num == 562)
					{
						this.TileType = num;
						this.WallType = 312;
						this.BeamType = 575;
						this.BeamPaint = 16;
						this.PlatformStyle = 44;
						this.DoorStyle = 45;
						this.TableStyle = 8;
						this.UsesTables2 = true;
						this.WorkbenchStyle = 40;
						this.PianoStyle = 39;
						this.BookcaseStyle = 40;
						this.ChairStyle = 44;
						this.ChestStyle = 11;
						this.UsesContainers2 = true;
						return;
					}
					if (num != 563)
					{
						return;
					}
					this.TileType = num;
					this.WallType = 313;
					this.BeamType = 575;
					this.BeamPaint = 16;
					this.PlatformStyle = 44;
					this.DoorStyle = 45;
					this.TableStyle = 8;
					this.UsesTables2 = true;
					this.WorkbenchStyle = 40;
					this.PianoStyle = 39;
					this.BookcaseStyle = 40;
					this.ChairStyle = 44;
					this.ChestStyle = 11;
					this.UsesContainers2 = true;
					return;
				}
			}
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x00629030 File Offset: 0x00627230
		public void PaintSeedHouses()
		{
			if (this.TileType == 197 && Main.drunkWorld && Main.tenthAnniversaryWorld)
			{
				foreach (Rectangle rectangle in this.Rooms)
				{
					WorldUtils.Gen(new Point(rectangle.X, rectangle.Y), new Shapes.Rectangle(rectangle.Width, rectangle.Height), Actions.Chain(new GenAction[]
					{
						new Modifiers.OnlyTiles(new ushort[]
						{
							19,
							10,
							11,
							14,
							18,
							87,
							101,
							15,
							21
						}),
						new Actions.SetTilePaint(7)
					}));
					WorldUtils.Gen(new Point(rectangle.X, rectangle.Y), new Shapes.Rectangle(rectangle.Width, rectangle.Height), Actions.Chain(new GenAction[]
					{
						new Modifiers.OnlyWalls(new ushort[]
						{
							this.WallType
						}),
						new Actions.SetWallPaint(7)
					}));
				}
			}
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x0062914C File Offset: 0x0062734C
		public virtual void Place(HouseBuilderContext context, StructureMap structures)
		{
			this.PlaceEmptyRooms();
			foreach (Rectangle area in this.Rooms)
			{
				structures.AddProtectedStructure(area, 8);
			}
			this.PlaceStairs();
			this.PlaceDoors();
			this.PlacePlatforms();
			this.PlaceSupportBeams();
			this.PlaceBiomeSpecificPriorityTool(context);
			this.FillRooms();
			foreach (Rectangle room in this.Rooms)
			{
				this.AgeRoom(room);
			}
			this.PlaceChests();
			this.PlaceBiomeSpecificTool(context);
			this.PaintSeedHouses();
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x00629214 File Offset: 0x00627414
		private void PlaceEmptyRooms()
		{
			foreach (Rectangle rectangle in this.Rooms)
			{
				WorldUtils.Gen(new Point(rectangle.X, rectangle.Y), new Shapes.Rectangle(rectangle.Width, rectangle.Height), Actions.Chain(new GenAction[]
				{
					new Actions.SetTileKeepWall(this.TileType, false, true),
					new Actions.SetFrames(true)
				}));
				WorldUtils.Gen(new Point(rectangle.X + 1, rectangle.Y + 1), new Shapes.Rectangle(rectangle.Width - 2, rectangle.Height - 2), Actions.Chain(new GenAction[]
				{
					new Actions.ClearTile(true),
					new Actions.PlaceWall(this.WallType, true)
				}));
			}
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x00629300 File Offset: 0x00627500
		private void FillRooms()
		{
			int x = 14;
			if (this.UsesTables2)
			{
				x = 469;
			}
			Point[] choices = new Point[]
			{
				new Point(x, this.TableStyle),
				new Point(16, 0),
				new Point(18, this.WorkbenchStyle),
				new Point(86, 0),
				new Point(87, this.PianoStyle),
				new Point(94, 0),
				new Point(101, this.BookcaseStyle)
			};
			foreach (Rectangle rectangle in this.Rooms)
			{
				int num = rectangle.Width / 8;
				int num2 = rectangle.Width / (num + 1);
				int num3 = this._random.Next(2);
				for (int i = 0; i < num; i++)
				{
					int num4 = (i + 1) * num2 + rectangle.X;
					int num5 = i + num3 % 2;
					if (num5 != 0)
					{
						if (num5 == 1)
						{
							int num6 = rectangle.Y + 1;
							WorldGen.PlaceTile(num4, num6, 34, true, false, -1, this._random.Next(6));
							for (int j = -1; j < 2; j++)
							{
								for (int k = 0; k < 3; k++)
								{
									Tile tile = this._tiles[j + num4, k + num6];
									tile.frameX += 54;
								}
							}
						}
					}
					else
					{
						int num6 = rectangle.Y + Math.Min(rectangle.Height / 2, rectangle.Height - 5);
						PaintingEntry paintingEntry = (this.Type == HouseType.Desert) ? WorldGen.RandHousePictureDesert() : WorldGen.RandHousePicture();
						WorldGen.PlaceTile(num4, num6, paintingEntry.tileType, true, false, -1, paintingEntry.style);
					}
				}
				int l = rectangle.Width / 8 + 3;
				WorldGen.SetupStatueList();
				while (l > 0)
				{
					int num7 = this._random.Next(rectangle.Width - 3) + 1 + rectangle.X;
					int num8 = rectangle.Y + rectangle.Height - 2;
					switch (this._random.Next(4))
					{
					case 0:
						WorldGen.PlaceSmallPile(num7, num8, this._random.Next(31, 34), 1, 185);
						break;
					case 1:
						WorldGen.PlaceTile(num7, num8, 186, true, false, -1, this._random.Next(22, 26));
						break;
					case 2:
					{
						int num9 = this._random.Next(2, GenVars.statueList.Length);
						WorldGen.PlaceTile(num7, num8, (int)GenVars.statueList[num9].X, true, false, -1, (int)GenVars.statueList[num9].Y);
						if (GenVars.StatuesWithTraps.Contains(num9))
						{
							WorldGen.PlaceStatueTrap(num7, num8);
						}
						break;
					}
					case 3:
					{
						Point point = Utils.SelectRandom<Point>(this._random, choices);
						WorldGen.PlaceTile(num7, num8, point.X, true, false, -1, point.Y);
						break;
					}
					}
					l--;
				}
			}
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x00629658 File Offset: 0x00627858
		private void PlaceStairs()
		{
			foreach (Tuple<Point, Point> tuple in this.CreateStairsList())
			{
				Point item = tuple.Item1;
				Point item2 = tuple.Item2;
				int num = (item2.X > item.X) ? 1 : -1;
				ShapeData shapeData = new ShapeData();
				for (int i = 0; i < item2.Y - item.Y; i++)
				{
					shapeData.Add(num * (i + 1), i);
				}
				WorldUtils.Gen(item, new ModShapes.All(shapeData), Actions.Chain(new GenAction[]
				{
					new Actions.PlaceTile(19, this.PlatformStyle),
					new Actions.SetSlope((num == 1) ? 1 : 2),
					new Actions.SetFrames(true)
				}));
				WorldUtils.Gen(new Point(item.X + ((num == 1) ? 1 : -4), item.Y - 1), new Shapes.Rectangle(4, 1), Actions.Chain(new GenAction[]
				{
					new Actions.Clear(),
					new Actions.PlaceWall(this.WallType, true),
					new Actions.PlaceTile(19, this.PlatformStyle),
					new Actions.SetFrames(true)
				}));
			}
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x006297B0 File Offset: 0x006279B0
		private List<Tuple<Point, Point>> CreateStairsList()
		{
			List<Tuple<Point, Point>> list = new List<Tuple<Point, Point>>();
			for (int i = 1; i < this.Rooms.Count; i++)
			{
				Rectangle rectangle = this.Rooms[i];
				Rectangle rectangle2 = this.Rooms[i - 1];
				int num = rectangle2.X - rectangle.X;
				int num2 = rectangle.X + rectangle.Width - (rectangle2.X + rectangle2.Width);
				if (num > num2)
				{
					list.Add(new Tuple<Point, Point>(new Point(rectangle.X + rectangle.Width - 1, rectangle.Y + 1), new Point(rectangle.X + rectangle.Width - rectangle.Height + 1, rectangle.Y + rectangle.Height - 1)));
				}
				else
				{
					list.Add(new Tuple<Point, Point>(new Point(rectangle.X, rectangle.Y + 1), new Point(rectangle.X + rectangle.Height - 1, rectangle.Y + rectangle.Height - 1)));
				}
			}
			return list;
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x006298C0 File Offset: 0x00627AC0
		private void PlaceDoors()
		{
			foreach (Point point in this.CreateDoorList())
			{
				WorldUtils.Gen(point, new Shapes.Rectangle(1, 3), new Actions.ClearTile(true));
				WorldGen.PlaceTile(point.X, point.Y, 10, true, true, -1, this.DoorStyle);
			}
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x00629940 File Offset: 0x00627B40
		private List<Point> CreateDoorList()
		{
			List<Point> list = new List<Point>();
			foreach (Rectangle rectangle in this.Rooms)
			{
				int y;
				if (HouseBuilder.FindSideExit(new Rectangle(rectangle.X + rectangle.Width, rectangle.Y + 1, 1, rectangle.Height - 2), false, out y))
				{
					list.Add(new Point(rectangle.X + rectangle.Width - 1, y));
				}
				if (HouseBuilder.FindSideExit(new Rectangle(rectangle.X, rectangle.Y + 1, 1, rectangle.Height - 2), true, out y))
				{
					list.Add(new Point(rectangle.X, y));
				}
			}
			return list;
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x00629A14 File Offset: 0x00627C14
		private void PlacePlatforms()
		{
			foreach (Point origin in this.CreatePlatformsList())
			{
				WorldUtils.Gen(origin, new Shapes.Rectangle(3, 1), Actions.Chain(new GenAction[]
				{
					new Actions.ClearMetadata(),
					new Actions.PlaceTile(19, this.PlatformStyle),
					new Actions.SetFrames(true)
				}));
			}
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x00629A98 File Offset: 0x00627C98
		private List<Point> CreatePlatformsList()
		{
			List<Point> list = new List<Point>();
			Rectangle topRoom = this.TopRoom;
			Rectangle bottomRoom = this.BottomRoom;
			int x;
			if (HouseBuilder.FindVerticalExit(new Rectangle(topRoom.X + 2, topRoom.Y, topRoom.Width - 4, 1), true, out x))
			{
				list.Add(new Point(x, topRoom.Y));
			}
			if (HouseBuilder.FindVerticalExit(new Rectangle(bottomRoom.X + 2, bottomRoom.Y + bottomRoom.Height - 1, bottomRoom.Width - 4, 1), false, out x))
			{
				list.Add(new Point(x, bottomRoom.Y + bottomRoom.Height - 1));
			}
			return list;
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x00629B3C File Offset: 0x00627D3C
		private void PlaceSupportBeams()
		{
			foreach (Rectangle rectangle in this.CreateSupportBeamList())
			{
				if (rectangle.Height > 1 && this._tiles[rectangle.X, rectangle.Y - 1].type != 19)
				{
					WorldUtils.Gen(new Point(rectangle.X, rectangle.Y), new Shapes.Rectangle(rectangle.Width, rectangle.Height), Actions.Chain(new GenAction[]
					{
						new Actions.SetTileKeepWall(this.BeamType, false, true),
						new Actions.SetFrames(true),
						new Actions.SetTilePaint(this.BeamPaint)
					}));
					Tile tile = this._tiles[rectangle.X, rectangle.Y + rectangle.Height];
					tile.slope(0);
					tile.halfBrick(false);
				}
			}
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x00629C44 File Offset: 0x00627E44
		private List<Rectangle> CreateSupportBeamList()
		{
			List<Rectangle> list = new List<Rectangle>();
			int num = this.Rooms.Min((Rectangle room) => room.Left);
			int num2 = this.Rooms.Max((Rectangle room) => room.Right) - 1;
			int num3 = 6;
			while (num3 > 4 && (num2 - num) % num3 != 0)
			{
				num3--;
			}
			for (int i = num; i <= num2; i += num3)
			{
				for (int j = 0; j < this.Rooms.Count; j++)
				{
					Rectangle rectangle = this.Rooms[j];
					if (i >= rectangle.X && i < rectangle.X + rectangle.Width)
					{
						int num4 = rectangle.Y + rectangle.Height;
						int num5 = 50;
						for (int k = j + 1; k < this.Rooms.Count; k++)
						{
							if (i >= this.Rooms[k].X && i < this.Rooms[k].X + this.Rooms[k].Width)
							{
								num5 = Math.Min(num5, this.Rooms[k].Y - num4);
							}
						}
						if (num5 > 0)
						{
							Point point;
							bool flag = WorldUtils.Find(new Point(i, num4), Searches.Chain(new Searches.Down(num5), new GenCondition[]
							{
								new Conditions.IsSolid()
							}), out point);
							if (num5 < 50 && !WorldGen.SecretSeed.GenerateBiggerAbandonedHouses)
							{
								flag = true;
								point = new Point(i, num4 + num5);
							}
							if (flag)
							{
								list.Add(new Rectangle(i, num4, 1, point.Y - num4));
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x00629E28 File Offset: 0x00628028
		private static bool FindVerticalExit(Rectangle wall, bool isUp, out int exitX)
		{
			Point point;
			bool result = WorldUtils.Find(new Point(wall.X + wall.Width - 3, wall.Y + (isUp ? -5 : 0)), Searches.Chain(new Searches.Left(wall.Width - 3), new GenCondition[]
			{
				new Conditions.IsSolid().Not().AreaOr(3, 5)
			}), out point);
			exitX = point.X;
			return result;
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x00629E94 File Offset: 0x00628094
		private static bool FindSideExit(Rectangle wall, bool isLeft, out int exitY)
		{
			Point point;
			bool result = WorldUtils.Find(new Point(wall.X + (isLeft ? -4 : 0), wall.Y + wall.Height - 3), Searches.Chain(new Searches.Up(wall.Height - 3), new GenCondition[]
			{
				new Conditions.IsSolid().Not().AreaOr(4, 3)
			}), out point);
			exitY = point.Y;
			return result;
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x00629F00 File Offset: 0x00628100
		private void PlaceChests()
		{
			if (this._random.NextDouble() > this.ChestChance)
			{
				return;
			}
			bool flag = false;
			foreach (Rectangle rectangle in this.Rooms)
			{
				int num = rectangle.Height - 1 + rectangle.Y;
				bool flag2 = num > (int)Main.worldSurface;
				ushort chestTileType = (flag2 && this.UsesContainers2) ? 467 : 21;
				int chestStyle = flag2 ? this.ChestStyle : 0;
				int num2 = 0;
				while (num2 < 10 && !(flag = WorldGen.AddBuriedChest(this._random.Next(2, rectangle.Width - 2) + rectangle.X, num, 0, false, chestStyle, false, chestTileType)))
				{
					num2++;
				}
				if (flag)
				{
					break;
				}
				int num3 = rectangle.X + 2;
				while (num3 <= rectangle.X + rectangle.Width - 2 && !(flag = WorldGen.AddBuriedChest(num3, num, 0, false, chestStyle, false, chestTileType)))
				{
					num3++;
				}
				if (flag)
				{
					break;
				}
			}
			if (!flag)
			{
				foreach (Rectangle rectangle2 in this.Rooms)
				{
					int num4 = rectangle2.Y - 1;
					bool flag3 = num4 > (int)Main.worldSurface;
					ushort chestTileType2 = (flag3 && this.UsesContainers2) ? 467 : 21;
					int chestStyle2 = flag3 ? this.ChestStyle : 0;
					int num5 = 0;
					while (num5 < 10 && !(flag = WorldGen.AddBuriedChest(this._random.Next(2, rectangle2.Width - 2) + rectangle2.X, num4, 0, false, chestStyle2, false, chestTileType2)))
					{
						num5++;
					}
					if (flag)
					{
						break;
					}
					int num6 = rectangle2.X + 2;
					while (num6 <= rectangle2.X + rectangle2.Width - 2 && !(flag = WorldGen.AddBuriedChest(num6, num4, 0, false, chestStyle2, false, chestTileType2)))
					{
						num6++;
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				for (int i = 0; i < 1000; i++)
				{
					int i2 = this._random.Next(this.Rooms[0].X - 30, this.Rooms[0].X + 30);
					int num7 = this._random.Next(this.Rooms[0].Y - 30, this.Rooms[0].Y + 30);
					bool flag4 = num7 > (int)Main.worldSurface;
					ushort chestTileType3 = (flag4 && this.UsesContainers2) ? 467 : 21;
					int chestStyle3 = flag4 ? this.ChestStyle : 0;
					if (flag = WorldGen.AddBuriedChest(i2, num7, 0, false, chestStyle3, false, chestTileType3))
					{
						break;
					}
				}
			}
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x0062A1E4 File Offset: 0x006283E4
		private void PlaceBiomeSpecificPriorityTool(HouseBuilderContext context)
		{
			if (this.Type == HouseType.Desert && GenVars.extraBastStatueCount < GenVars.extraBastStatueCountMax)
			{
				bool flag = false;
				foreach (Rectangle rectangle in this.Rooms)
				{
					int num = rectangle.Height - 2 + rectangle.Y;
					if (WorldGen.remixWorldGen && (double)num > Main.rockLayer)
					{
						return;
					}
					for (int i = 0; i < 10; i++)
					{
						int num2 = this._random.Next(2, rectangle.Width - 2) + rectangle.X;
						WorldGen.PlaceTile(num2, num, 506, true, true, -1, 0);
						if (flag = (this._tiles[num2, num].active() && this._tiles[num2, num].type == 506))
						{
							break;
						}
					}
					if (flag)
					{
						break;
					}
					int num3 = rectangle.X + 2;
					while (num3 <= rectangle.X + rectangle.Width - 2 && !(flag = WorldGen.PlaceTile(num3, num, 506, true, true, -1, 0)))
					{
						num3++;
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					foreach (Rectangle rectangle2 in this.Rooms)
					{
						int num4 = rectangle2.Y - 1;
						for (int j = 0; j < 10; j++)
						{
							int num5 = this._random.Next(2, rectangle2.Width - 2) + rectangle2.X;
							WorldGen.PlaceTile(num5, num4, 506, true, true, -1, 0);
							if (flag = (this._tiles[num5, num4].active() && this._tiles[num5, num4].type == 506))
							{
								break;
							}
						}
						if (flag)
						{
							break;
						}
						int num6 = rectangle2.X + 2;
						while (num6 <= rectangle2.X + rectangle2.Width - 2 && !(flag = WorldGen.PlaceTile(num6, num4, 506, true, true, -1, 0)))
						{
							num6++;
						}
						if (flag)
						{
							break;
						}
					}
				}
				if (flag)
				{
					GenVars.extraBastStatueCount++;
				}
			}
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x0062A44C File Offset: 0x0062864C
		private void PlaceBiomeSpecificTool(HouseBuilderContext context)
		{
			if (this.Type == HouseType.Jungle && context.SharpenerCount < this._random.Next(2, 5))
			{
				bool flag = false;
				foreach (Rectangle rectangle in this.Rooms)
				{
					int num = rectangle.Height - 2 + rectangle.Y;
					for (int i = 0; i < 10; i++)
					{
						int num2 = this._random.Next(2, rectangle.Width - 2) + rectangle.X;
						WorldGen.PlaceTile(num2, num, 377, true, true, -1, 0);
						if (flag = (this._tiles[num2, num].active() && this._tiles[num2, num].type == 377))
						{
							break;
						}
					}
					if (flag)
					{
						break;
					}
					int num3 = rectangle.X + 2;
					while (num3 <= rectangle.X + rectangle.Width - 2 && !(flag = WorldGen.PlaceTile(num3, num, 377, true, true, -1, 0)))
					{
						num3++;
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					context.SharpenerCount++;
				}
			}
			if (this.Type == HouseType.Desert && context.ExtractinatorCount < this._random.Next(2, 5))
			{
				ushort num4 = 219;
				if (WorldGen.SecretSeed.errorWorld.Enabled)
				{
					num4 = 642;
				}
				bool flag2 = false;
				foreach (Rectangle rectangle2 in this.Rooms)
				{
					int num5 = rectangle2.Height - 2 + rectangle2.Y;
					for (int j = 0; j < 10; j++)
					{
						int num6 = this._random.Next(2, rectangle2.Width - 2) + rectangle2.X;
						WorldGen.PlaceTile(num6, num5, (int)num4, true, true, -1, 0);
						if (flag2 = (this._tiles[num6, num5].active() && this._tiles[num6, num5].type == num4))
						{
							break;
						}
					}
					if (flag2)
					{
						break;
					}
					int num7 = rectangle2.X + 2;
					while (num7 <= rectangle2.X + rectangle2.Width - 2 && !(flag2 = WorldGen.PlaceTile(num7, num5, (int)num4, true, true, -1, 0)))
					{
						num7++;
					}
					if (flag2)
					{
						break;
					}
				}
				if (flag2)
				{
					context.ExtractinatorCount++;
				}
			}
		}

		// Token: 0x04005B08 RID: 23304
		private const int VERTICAL_EXIT_WIDTH = 3;

		// Token: 0x04005B09 RID: 23305
		public static readonly HouseBuilder Invalid = new HouseBuilder();

		// Token: 0x04005B0A RID: 23306
		public readonly HouseType Type;

		// Token: 0x04005B0B RID: 23307
		public readonly bool IsValid;

		// Token: 0x04005B1C RID: 23324
		protected ushort[] SkipTilesDuringWallAging = new ushort[]
		{
			245,
			246,
			240,
			241,
			242
		};
	}
}
