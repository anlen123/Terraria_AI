using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse
{
	// Token: 0x02000526 RID: 1318
	public class WoodHouseBuilder : HouseBuilder
	{
		// Token: 0x060036CF RID: 14031 RVA: 0x0062B098 File Offset: 0x00629298
		public WoodHouseBuilder(IEnumerable<Rectangle> rooms) : base(HouseType.Wood, rooms)
		{
			base.TileType = 30;
			base.WallType = 27;
			base.BeamType = 124;
			base.PlatformStyle = 0;
			base.DoorStyle = 0;
			base.TableStyle = 0;
			base.WorkbenchStyle = 0;
			base.PianoStyle = 0;
			base.BookcaseStyle = 0;
			base.ChairStyle = 0;
			base.ChestStyle = 1;
			base.PotentiallyConvertToSeedHouse();
			base.PotentiallyConvertToRainbowBrick();
			base.PotentiallyConvertToRainbowMossBlock();
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x0062B110 File Offset: 0x00629310
		protected override void AgeRoom(Rectangle room)
		{
			for (int i = 0; i < room.Width * room.Height / 16; i++)
			{
				int x = WorldGen.genRand.Next(1, room.Width - 1) + room.X;
				int y = WorldGen.genRand.Next(1, room.Height - 1) + room.Y;
				WorldUtils.Gen(new Point(x, y), new Shapes.Rectangle(2, 2), Actions.Chain(new GenAction[]
				{
					new Modifiers.Dither(0.5),
					new Modifiers.Blotches(2, 2, 0.3),
					new Modifiers.IsEmpty(),
					new Actions.SetTile(51, true, true, true)
				}));
			}
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new GenAction[]
			{
				new Modifiers.Dither(0.85),
				new Modifiers.Blotches(2, 0.3),
				new Modifiers.OnlyWalls(new ushort[]
				{
					base.WallType
				}),
				new Modifiers.SkipTiles(this.SkipTilesDuringWallAging),
				((double)room.Y > Main.worldSurface) ? new Actions.ClearWall(true) : new Actions.PlaceWall(2, true)
			}));
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new GenAction[]
			{
				new Modifiers.Dither(0.95),
				new Modifiers.OnlyTiles(new ushort[]
				{
					30,
					321,
					158
				}),
				new Actions.ClearTile(true)
			}));
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x0062B2CB File Offset: 0x006294CB
		public override void Place(HouseBuilderContext context, StructureMap structures)
		{
			base.Place(context, structures);
			base.RainbowifyOnTenthAnniversaryWorlds();
		}
	}
}
