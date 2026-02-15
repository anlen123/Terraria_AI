using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.GameContent.Generation;
using Terraria.GameContent.Generation.Dungeon;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	// Token: 0x0200050F RID: 1295
	public class EnchantedSwordBiome : MicroBiome
	{
		// Token: 0x06003645 RID: 13893 RVA: 0x006240C4 File Offset: 0x006222C4
		public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
		{
			Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
			WorldUtils.Gen(new Point(origin.X - 25, origin.Y - 25), new Shapes.Rectangle(50, 50), new Actions.TileScanner(new ushort[]
			{
				0,
				1
			}).Output(dictionary));
			int num = dictionary[0] + dictionary[1];
			if (WorldGen.SecretSeed.errorWorld.Enabled)
			{
				if (num < 625)
				{
					return false;
				}
			}
			else if (num < 1250)
			{
				return false;
			}
			int num2 = 55;
			if (WorldGen.SecretSeed.errorWorld.Enabled)
			{
				num2 = 105;
			}
			if (origin.Y <= num2)
			{
				return false;
			}
			int num3 = origin.Y - num2;
			int num4 = 50;
			if (num3 < num4)
			{
				num4 = num3;
			}
			Point point;
			if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Up(num3), new GenCondition[]
			{
				new Conditions.IsSolid().AreaOr(1, num4).Not()
			}), out point) || point.Y <= num2)
			{
				if (!WorldGen.SecretSeed.errorWorld.Enabled)
				{
					return false;
				}
				point.Y = origin.Y - 100;
			}
			Point point2;
			if (WorldUtils.Find(origin, Searches.Chain(new Searches.Up(origin.Y - point.Y), new GenCondition[]
			{
				new Conditions.IsTile(new ushort[]
				{
					53
				})
			}), out point2) && !WorldGen.SecretSeed.errorWorld.Enabled)
			{
				return false;
			}
			point.Y += 50;
			ShapeData shapeData = new ShapeData();
			ShapeData shapeData2 = new ShapeData();
			Point point3 = new Point(origin.X, origin.Y + 20);
			Point point4 = new Point(origin.X, origin.Y + 30);
			bool[] array = new bool[TileID.Sets.GeneralPlacementTiles.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = TileID.Sets.GeneralPlacementTiles[i];
			}
			array[21] = false;
			array[467] = false;
			double num5 = 0.8 + GenBase._random.NextDouble() * 0.5;
			Rectangle rectangle = new Rectangle(point3.X - (int)(20.0 * num5), point3.Y - 20, (int)(40.0 * num5), 40);
			if (!structures.CanPlace(rectangle, array, 0))
			{
				return false;
			}
			Rectangle rectangle2 = new Rectangle(origin.X, point.Y + 10, 1, origin.Y - point.Y - 9);
			if (!structures.CanPlace(rectangle2, array, 2))
			{
				return false;
			}
			if (WorldGen.SecretSeed.dualDungeons.Enabled && (DungeonUtils.IntersectsAnyPotentialDungeonBounds(rectangle, false) || DungeonUtils.IntersectsAnyPotentialDungeonBounds(rectangle2, false)))
			{
				return false;
			}
			WorldUtils.Gen(point3, new Shapes.Slime(20, num5, 1.0), Actions.Chain(new GenAction[]
			{
				new Modifiers.Blotches(2, 0.4),
				new Actions.ClearTile(true).Output(shapeData)
			}));
			WorldUtils.Gen(point4, new Shapes.Mound(14, 14), Actions.Chain(new GenAction[]
			{
				new Modifiers.Blotches(2, 1, 0.8),
				new Actions.SetTile(0, false, true, true),
				new Actions.SetFrames(true).Output(shapeData2)
			}));
			shapeData.Subtract(shapeData2, point3, point4);
			WorldUtils.Gen(point3, new ModShapes.InnerOutline(shapeData, true), Actions.Chain(new GenAction[]
			{
				new Actions.SetTile(2, false, true, true),
				new Actions.SetFrames(true)
			}));
			WorldUtils.Gen(point3, new ModShapes.All(shapeData), Actions.Chain(new GenAction[]
			{
				new Modifiers.RectangleMask(-40, 40, 0, 40),
				new Modifiers.IsEmpty(),
				new Actions.SetLiquid(0, byte.MaxValue)
			}));
			WorldUtils.Gen(point3, new ModShapes.All(shapeData), Actions.Chain(new GenAction[]
			{
				new Actions.PlaceWall(68, true),
				new Modifiers.OnlyTiles(new ushort[]
				{
					2
				}),
				new Modifiers.Offset(0, 1),
				new ActionVines(3, 5, 382)
			}));
			if (GenBase._random.NextDouble() <= this._chanceOfEntrance || WorldGen.tenthAnniversaryWorldGen)
			{
				ShapeData data = new ShapeData();
				WorldUtils.Gen(new Point(origin.X, point.Y + 10), new Shapes.Rectangle(1, origin.Y - point.Y - 9), Actions.Chain(new GenAction[]
				{
					new Modifiers.Blotches(2, 0.2),
					new Modifiers.SkipTiles(new ushort[]
					{
						191,
						192
					}),
					new Actions.ClearTile(false).Output(data),
					new Modifiers.Expand(1),
					new Modifiers.OnlyTiles(new ushort[]
					{
						53
					}),
					new Actions.SetTile(397, false, true, true).Output(data)
				}));
				WorldUtils.Gen(new Point(origin.X, point.Y + 10), new ModShapes.All(data), new Actions.SetFrames(true));
			}
			if (GenBase._random.NextDouble() <= this._chanceOfRealSword)
			{
				WorldGen.PlaceTile(point4.X, point4.Y - 15, 187, true, false, -1, 17);
			}
			else
			{
				WorldGen.PlaceTile(point4.X, point4.Y - 15, 186, true, false, -1, 15);
			}
			WorldUtils.Gen(point4, new ModShapes.All(shapeData2), Actions.Chain(new GenAction[]
			{
				new Modifiers.Offset(0, -1),
				new Modifiers.OnlyTiles(new ushort[]
				{
					2
				}),
				new Modifiers.Offset(0, -1),
				new ActionGrass()
			}));
			structures.AddProtectedStructure(new Rectangle(point3.X - (int)(20.0 * num5), point3.Y - 20, (int)(40.0 * num5), 40), 10);
			return true;
		}

		// Token: 0x04005AF4 RID: 23284
		[JsonProperty("ChanceOfEntrance")]
		private double _chanceOfEntrance;

		// Token: 0x04005AF5 RID: 23285
		[JsonProperty("ChanceOfRealSword")]
		private double _chanceOfRealSword;
	}
}
