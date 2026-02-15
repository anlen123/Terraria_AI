using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	// Token: 0x02000512 RID: 1298
	public class MiningExplosivesBiome : MicroBiome
	{
		// Token: 0x0600364C RID: 13900 RVA: 0x006252C4 File Offset: 0x006234C4
		public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
		{
			if (WorldGen.SolidTile(origin.X, origin.Y, false))
			{
				return false;
			}
			if (Main.tile[origin.X, origin.Y].wall == 216 || Main.tile[origin.X, origin.Y].wall == 187)
			{
				return false;
			}
			ushort type = Utils.SelectRandom<ushort>(GenBase._random, new ushort[]
			{
				(GenVars.goldBar == 19) ? 8 : 169,
				(GenVars.silverBar == 21) ? 9 : 168,
				(GenVars.ironBar == 22) ? 6 : 167,
				(GenVars.copperBar == 20) ? 7 : 166
			});
			double num = GenBase._random.NextDouble() * 2.0 - 1.0;
			if (!WorldUtils.Find(origin, Searches.Chain((num > 0.0) ? new Searches.Right(40) : new Searches.Left(40), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out origin))
			{
				return false;
			}
			if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(80), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out origin))
			{
				return false;
			}
			ShapeData shapeData = new ShapeData();
			Ref<int> @ref = new Ref<int>(0);
			Ref<int> ref2 = new Ref<int>(0);
			WorldUtils.Gen(origin, new ShapeRunner(10.0, 20, new Vector2D(num, 1.0)).Output(shapeData), Actions.Chain(new GenAction[]
			{
				new Modifiers.Blotches(2, 0.3),
				new Actions.Scanner(@ref),
				new Modifiers.IsSolid(),
				new Actions.Scanner(ref2)
			}));
			if (ref2.Value < @ref.Value / 2)
			{
				return false;
			}
			Rectangle area = new Rectangle(origin.X - 15, origin.Y - 10, 30, 20);
			if (!structures.CanPlace(area, 0))
			{
				return false;
			}
			WorldUtils.Gen(origin, new ModShapes.All(shapeData), new Actions.SetTile(type, true, true, true));
			WorldUtils.Gen(new Point(origin.X - (int)(num * -5.0), origin.Y - 5), new Shapes.Circle(5), Actions.Chain(new GenAction[]
			{
				new Modifiers.Blotches(2, 0.3),
				new Actions.ClearTile(true)
			}));
			Point point;
			bool flag = true & WorldUtils.Find(new Point(origin.X - ((num > 0.0) ? 3 : -3), origin.Y - 3), Searches.Chain(new Searches.Down(10), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out point);
			int num2 = (GenBase._random.Next(4) == 0) ? 3 : 7;
			Point point2;
			if (!(flag & WorldUtils.Find(new Point(origin.X - ((num > 0.0) ? (-num2) : num2), origin.Y - 3), Searches.Chain(new Searches.Down(10), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out point2)))
			{
				return false;
			}
			point.Y--;
			point2.Y--;
			Tile tile = GenBase._tiles[point.X, point.Y + 1];
			tile.slope(0);
			tile.halfBrick(false);
			for (int i = -1; i <= 1; i++)
			{
				WorldUtils.ClearTile(point2.X + i, point2.Y, false);
				Tile tile2 = GenBase._tiles[point2.X + i, point2.Y + 1];
				if (!WorldGen.SolidOrSlopedTile(tile2))
				{
					tile2.ResetToType(1);
					tile2.active(true);
				}
				tile2.slope(0);
				tile2.halfBrick(false);
				WorldUtils.TileFrame(point2.X + i, point2.Y + 1, true);
			}
			WorldGen.PlaceTile(point.X, point.Y, 141, false, false, -1, 0);
			WorldGen.PlaceTile(point2.X, point2.Y, 411, true, true, -1, 0);
			WorldUtils.WireLine(point, point2);
			structures.AddProtectedStructure(area, 5);
			return true;
		}
	}
}
