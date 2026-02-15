using System;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	// Token: 0x02000503 RID: 1283
	public class SpikePitBiome : MicroBiome
	{
		// Token: 0x060035E8 RID: 13800 RVA: 0x0061E240 File Offset: 0x0061C440
		public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
		{
			if (WorldGen.SolidTile(origin.X, origin.Y, false) && GenBase._tiles[origin.X, origin.Y].wall == 3)
			{
				return false;
			}
			if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(100), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out origin))
			{
				return false;
			}
			ushort num = 1;
			if (WorldGen.notTheBees)
			{
				num = 225;
			}
			Point point;
			if (!WorldUtils.Find(new Point(origin.X - 4, origin.Y), Searches.Chain(new Searches.Down(5), new GenCondition[]
			{
				new Conditions.IsTile(new ushort[]
				{
					num
				}).AreaAnd(8, 1)
			}), out point))
			{
				return false;
			}
			ShapeData shapeData = new ShapeData();
			ShapeData shapeData2 = new ShapeData();
			ShapeData shapeData3 = new ShapeData();
			for (int i = 0; i < 4; i++)
			{
				WorldUtils.Gen(origin, new Shapes.Circle(GenBase._random.Next(8, 10) + i), Actions.Chain(new GenAction[]
				{
					new Modifiers.Offset(0, 5 * i + 5),
					new Modifiers.Blotches(3, 0.3).Output(shapeData)
				}));
			}
			for (int j = 0; j < 4; j++)
			{
				WorldUtils.Gen(origin, new Shapes.Circle(GenBase._random.Next(6, 7) + j), Actions.Chain(new GenAction[]
				{
					new Modifiers.Offset(0, 2 * j + 12),
					new Modifiers.Blotches(3, 0.3).Output(shapeData2)
				}));
			}
			for (int k = 0; k < 4; k++)
			{
				WorldUtils.Gen(origin, new Shapes.Circle(GenBase._random.Next(4, 5) + k / 2), Actions.Chain(new GenAction[]
				{
					new Modifiers.Offset(0, (int)(7.5 * (double)k) - 10),
					new Modifiers.Blotches(3, 0.3).Output(shapeData3)
				}));
			}
			ShapeData shapeData4 = new ShapeData(shapeData2);
			shapeData2.Subtract(shapeData3, origin, origin);
			shapeData4.Subtract(shapeData2, origin, origin);
			Rectangle bounds = ShapeData.GetBounds(origin, new ShapeData[]
			{
				shapeData,
				shapeData3
			});
			if (!structures.CanPlace(bounds, 2))
			{
				return false;
			}
			WorldUtils.Gen(origin, new ModShapes.All(shapeData), Actions.Chain(new GenAction[]
			{
				new Actions.SetTile(num, true, true, true)
			}));
			WorldUtils.Gen(origin, new ModShapes.All(shapeData3), new Actions.ClearTile(true));
			WorldUtils.Gen(origin, new ModShapes.All(shapeData4), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsTouchingAir(true),
				new Modifiers.IsTouching(true, new ushort[]
				{
					num
				}),
				new Actions.SetTile(48, true, true, true)
			}));
			WorldUtils.Gen(origin, new ModShapes.All(shapeData4), Actions.Chain(new GenAction[]
			{
				new Modifiers.Checkerboard(2),
				new Modifiers.IsTouchingAir(true),
				new Modifiers.IsTouching(false, new ushort[]
				{
					48
				}),
				new Actions.SetTile(48, true, true, true)
			}));
			structures.AddProtectedStructure(bounds, 2);
			return true;
		}
	}
}
