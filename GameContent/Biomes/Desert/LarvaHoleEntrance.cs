using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert
{
	// Token: 0x02000518 RID: 1304
	public static class LarvaHoleEntrance
	{
		// Token: 0x0600366D RID: 13933 RVA: 0x00626CCC File Offset: 0x00624ECC
		public static void Place(DesertDescription description, GenerationProgress progress, float progressMin, float progressMax)
		{
			int num = WorldGen.genRand.Next(2, 4);
			for (int i = 0; i < num; i++)
			{
				progress.Set((double)((float)i / (float)num), (double)progressMin, (double)progressMax);
				int holeRadius = WorldGen.genRand.Next(13, 16);
				int num2 = (int)((double)(i + 1) / (double)(num + 1) * (double)description.Surface.Width);
				num2 += description.Desert.Left;
				int y = (int)description.Surface[num2];
				LarvaHoleEntrance.PlaceAt(description, new Point(num2, y), holeRadius);
			}
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x00626D58 File Offset: 0x00624F58
		private static void PlaceAt(DesertDescription description, Point position, int holeRadius)
		{
			ShapeData data = new ShapeData();
			WorldUtils.Gen(position, new Shapes.Rectangle(new Rectangle(-holeRadius, -holeRadius * 2, holeRadius * 2, holeRadius * 2)), new Actions.Clear().Output(data));
			WorldUtils.Gen(position, new Shapes.Tail((double)(holeRadius * 2), new Vector2D(0.0, (double)holeRadius * 1.5)), Actions.Chain(new GenAction[]
			{
				new Actions.Clear().Output(data)
			}));
			WorldUtils.Gen(position, new ModShapes.All(data), Actions.Chain(new GenAction[]
			{
				new Modifiers.Offset(0, 1),
				new Modifiers.Expand(1),
				new Modifiers.IsSolid(),
				new Actions.Smooth(true)
			}));
			GenShapeActionPair pair = new GenShapeActionPair(new Shapes.Rectangle(1, 1), Actions.Chain(new GenAction[]
			{
				new Modifiers.Blotches(2, 0.3),
				new Modifiers.IsSolid(),
				new Actions.Clear(),
				new Actions.PlaceWall(187, true)
			}));
			GenShapeActionPair pair2 = new GenShapeActionPair(new Shapes.Circle(2, 3), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsSolid(),
				new Actions.SetTile(397, false, true, true),
				new Actions.PlaceWall(187, true)
			}));
			int num = position.X;
			for (int i = position.Y + (int)((double)holeRadius * 1.5); i < description.Hive.Top + (position.Y - description.Desert.Top) * 2 + 12; i++)
			{
				WorldUtils.Gen(new Point(num, i), pair);
				WorldUtils.Gen(new Point(num, i), pair2);
				if (i % 3 == 0)
				{
					num += WorldGen.genRand.Next(-1, 2);
					WorldUtils.Gen(new Point(num, i), pair);
					WorldUtils.Gen(new Point(num, i), pair2);
				}
			}
			WorldUtils.Gen(new Point(position.X, position.Y + 2), new ModShapes.All(data), new Actions.PlaceWall(0, true));
		}
	}
}
