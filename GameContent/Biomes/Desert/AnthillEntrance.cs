using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert
{
	// Token: 0x02000514 RID: 1300
	public static class AnthillEntrance
	{
		// Token: 0x06003650 RID: 13904 RVA: 0x006258CC File Offset: 0x00623ACC
		public static void Place(DesertDescription description, GenerationProgress progress, float progressMin, float progressMax)
		{
			int num = WorldGen.genRand.Next(2, 4);
			for (int i = 0; i < num; i++)
			{
				progress.Set((double)((float)i / (float)num), (double)progressMin, (double)progressMax);
				int holeRadius = WorldGen.genRand.Next(15, 18);
				int num2 = (int)((double)(i + 1) / (double)(num + 1) * (double)description.Surface.Width);
				num2 += description.Desert.Left;
				int y = (int)description.Surface[num2];
				AnthillEntrance.PlaceAt(description, new Point(num2, y), holeRadius);
			}
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x00625958 File Offset: 0x00623B58
		private static void PlaceAt(DesertDescription description, Point position, int holeRadius)
		{
			ShapeData data = new ShapeData();
			Point point = new Point(position.X, position.Y + 6);
			WorldUtils.Gen(point, new Shapes.Tail((double)(holeRadius * 2), new Vector2D(0.0, (double)(-(double)holeRadius) * 1.5)), Actions.Chain(new GenAction[]
			{
				new Actions.SetTile(53, false, true, true).Output(data)
			}));
			GenShapeActionPair genShapeActionPair = new GenShapeActionPair(new Shapes.Rectangle(1, 1), Actions.Chain(new GenAction[]
			{
				new Modifiers.Blotches(2, 0.3),
				new Modifiers.IsSolid(),
				new Actions.Clear(),
				new Actions.PlaceWall(187, true)
			}));
			GenShapeActionPair genShapeActionPair2 = new GenShapeActionPair(new Shapes.Rectangle(1, 1), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsSolid(),
				new Actions.Clear(),
				new Actions.PlaceWall(187, true)
			}));
			GenShapeActionPair pair = new GenShapeActionPair(new Shapes.Circle(2, 3), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsSolid(),
				new Actions.SetTile(397, false, true, true),
				new Actions.PlaceWall(187, true)
			}));
			GenShapeActionPair pair2 = new GenShapeActionPair(new Shapes.Circle(holeRadius, 3), Actions.Chain(new GenAction[]
			{
				new Modifiers.SkipWalls(new ushort[]
				{
					187
				}),
				new Actions.SetTile(53, false, true, true)
			}));
			GenShapeActionPair pair3 = new GenShapeActionPair(new Shapes.Circle(holeRadius - 2, 3), Actions.Chain(new GenAction[]
			{
				new Actions.PlaceWall(187, true)
			}));
			int num = position.X;
			for (int i = position.Y - holeRadius - 3; i < description.Hive.Top + (position.Y - description.Desert.Top) * 2 + 12; i++)
			{
				WorldUtils.Gen(new Point(num, i), (i < position.Y) ? genShapeActionPair2 : genShapeActionPair);
				WorldUtils.Gen(new Point(num, i), pair);
				if (i % 3 == 0 && i >= position.Y)
				{
					num += WorldGen.genRand.Next(-1, 2);
					WorldUtils.Gen(new Point(num, i), genShapeActionPair);
					if (i >= position.Y + 5)
					{
						WorldUtils.Gen(new Point(num, i), pair2);
						WorldUtils.Gen(new Point(num, i), pair3);
					}
					WorldUtils.Gen(new Point(num, i), pair);
				}
			}
			WorldUtils.Gen(new Point(point.X, point.Y - (int)((double)holeRadius * 1.5) + 3), new Shapes.Circle(holeRadius / 2, holeRadius / 3), Actions.Chain(new GenAction[]
			{
				Actions.Chain(new GenAction[]
				{
					new Actions.ClearTile(false),
					new Modifiers.Expand(1),
					new Actions.PlaceWall(0, true)
				})
			}));
			WorldUtils.Gen(point, new ModShapes.All(data), new Actions.Smooth(false));
		}
	}
}
