using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert
{
	// Token: 0x02000515 RID: 1301
	public static class ChambersEntrance
	{
		// Token: 0x06003652 RID: 13906 RVA: 0x00625C58 File Offset: 0x00623E58
		public static void Place(DesertDescription description, GenerationProgress progress, float progressMin, float progressMax)
		{
			int num = description.Desert.Center.X + WorldGen.genRand.Next(-40, 41);
			Point position = new Point(num, (int)description.Surface[num]);
			ChambersEntrance.PlaceAt(description, position, progress, progressMin, progressMax);
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x00625CA8 File Offset: 0x00623EA8
		private static void PlaceAt(DesertDescription description, Point position, GenerationProgress progress, float progressMin, float progressMax)
		{
			ShapeData shapeData = new ShapeData();
			Point origin = new Point(position.X, position.Y + 2);
			WorldUtils.Gen(origin, new Shapes.Circle(24, 12), Actions.Chain(new GenAction[]
			{
				new Modifiers.Blotches(2, 0.3),
				new Actions.SetTile(53, false, true, true).Output(shapeData)
			}));
			UnifiedRandom genRand = WorldGen.genRand;
			ShapeData data = new ShapeData();
			int num = description.Hive.Top - position.Y;
			int num2 = (genRand.Next(2) == 0) ? -1 : 1;
			List<ChambersEntrance.PathConnection> list = new List<ChambersEntrance.PathConnection>
			{
				new ChambersEntrance.PathConnection(new Point(position.X + -num2 * 26, position.Y - 8), num2)
			};
			int num3 = genRand.Next(2, 4);
			for (int i = 0; i < num3; i++)
			{
				int num4 = (int)((double)(i + 1) / (double)num3 * (double)num) + genRand.Next(-8, 9);
				int num5 = num2 * genRand.Next(20, 41);
				int num6 = genRand.Next(18, 29);
				WorldUtils.Gen(position, new Shapes.Circle(num6 / 2, 3), Actions.Chain(new GenAction[]
				{
					new Modifiers.Offset(num5, num4),
					new Modifiers.Blotches(2, 0.3),
					new Actions.Clear().Output(data),
					new Actions.PlaceWall(187, true)
				}));
				list.Add(new ChambersEntrance.PathConnection(new Point(num5 + num6 / 2 * -num2 + position.X, num4 + position.Y), -num2));
				num2 *= -1;
			}
			WorldUtils.Gen(position, new ModShapes.OuterOutline(data, true, false), Actions.Chain(new GenAction[]
			{
				new Modifiers.Expand(1),
				new Modifiers.OnlyTiles(new ushort[]
				{
					53
				}),
				new Actions.SetTile(397, false, true, true),
				new Actions.PlaceWall(187, true)
			}));
			GenShapeActionPair pair = new GenShapeActionPair(new Shapes.Rectangle(2, 4), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsSolid(),
				new Modifiers.Blotches(2, 0.3),
				new Actions.Clear(),
				new Modifiers.Expand(1),
				new Actions.PlaceWall(187, true),
				new Modifiers.OnlyTiles(new ushort[]
				{
					53
				}),
				new Actions.SetTile(397, false, true, true)
			}));
			for (int j = 1; j < list.Count; j++)
			{
				progress.Set((double)((float)j / (float)list.Count), (double)progressMin, (double)progressMax);
				ChambersEntrance.PathConnection pathConnection = list[j - 1];
				ChambersEntrance.PathConnection pathConnection2 = list[j];
				double num7 = Math.Abs(pathConnection2.Position.X - pathConnection.Position.X) * 1.5;
				for (double num8 = 0.0; num8 <= 1.0; num8 += 0.02)
				{
					Vector2D vector2D = new Vector2D(pathConnection.Position.X + pathConnection.Direction * num7 * num8, pathConnection.Position.Y);
					Vector2D vector2D2;
					vector2D2..ctor(pathConnection2.Position.X + pathConnection2.Direction * num7 * (1.0 - num8), pathConnection2.Position.Y);
					Vector2D vector2D3 = Vector2D.Lerp(pathConnection.Position, pathConnection2.Position, num8);
					Vector2D vector2D4 = Vector2D.Lerp(vector2D, vector2D3, num8);
					Vector2D vector2D5 = Vector2D.Lerp(vector2D3, vector2D2, num8);
					WorldUtils.Gen(Vector2D.Lerp(vector2D4, vector2D5, num8).ToPoint(), pair);
				}
			}
			WorldUtils.Gen(origin, new Shapes.Rectangle(new Rectangle(-29, -12, 58, 12)), Actions.Chain(new GenAction[]
			{
				new Modifiers.NotInShape(shapeData),
				new Modifiers.Expand(1),
				new Actions.PlaceWall(0, true)
			}));
		}

		// Token: 0x0200099A RID: 2458
		private struct PathConnection
		{
			// Token: 0x06004998 RID: 18840 RVA: 0x006D0D9B File Offset: 0x006CEF9B
			public PathConnection(Point position, int direction)
			{
				this.Position = new Vector2D((double)position.X, (double)position.Y);
				this.Direction = (double)direction;
			}

			// Token: 0x0400763C RID: 30268
			public readonly Vector2D Position;

			// Token: 0x0400763D RID: 30269
			public readonly double Direction;
		}
	}
}
