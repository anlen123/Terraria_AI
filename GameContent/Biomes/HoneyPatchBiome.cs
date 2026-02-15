using System;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	// Token: 0x02000510 RID: 1296
	public class HoneyPatchBiome : MicroBiome
	{
		// Token: 0x06003647 RID: 13895 RVA: 0x0062468C File Offset: 0x0062288C
		public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
		{
			if (GenBase._tiles[origin.X, origin.Y].active() && WorldGen.SolidTile(origin.X, origin.Y, false))
			{
				return false;
			}
			Point point;
			if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(80), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out point))
			{
				return false;
			}
			point.Y += 2;
			Ref<int> @ref = new Ref<int>(0);
			Ref<int> ref2 = new Ref<int>(0);
			Ref<int> ref3 = new Ref<int>(0);
			WorldUtils.Gen(point, new Shapes.Circle(15), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsSolid(),
				new Actions.Scanner(@ref),
				new Modifiers.OnlyTiles(new ushort[]
				{
					60,
					59
				}),
				new Actions.Scanner(ref2),
				new Modifiers.OnlyTiles(new ushort[]
				{
					60
				}),
				new Actions.Scanner(ref3)
			}));
			if ((double)ref2.Value / (double)@ref.Value < 0.75 || ref3.Value < 2)
			{
				return false;
			}
			@ref = new Ref<int>(0);
			WorldUtils.Gen(point, new Shapes.Circle(8), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsSolid(),
				new Actions.Scanner(@ref)
			}));
			if (@ref.Value < 20)
			{
				return false;
			}
			if (!structures.CanPlace(new Rectangle(point.X - 8, point.Y - 8, 16, 16), 0))
			{
				return false;
			}
			if (HoneyPatchBiome.TooCloseToImportantLocations(point))
			{
				return false;
			}
			WorldUtils.Gen(point, new Shapes.Circle(8), Actions.Chain(new GenAction[]
			{
				new Modifiers.RadialDither(0.0, 10.0),
				new Modifiers.IsSolid(),
				new Actions.SetTile(229, true, true, true)
			}));
			ShapeData data = new ShapeData();
			WorldUtils.Gen(point, new Shapes.Circle(4, 3), Actions.Chain(new GenAction[]
			{
				new Modifiers.Blotches(2, 0.3),
				new Modifiers.IsSolid(),
				new Actions.ClearTile(true),
				new Modifiers.RectangleMask(-6, 6, 0, 3).Output(data),
				new Actions.SetLiquid(2, byte.MaxValue)
			}));
			WorldUtils.Gen(new Point(point.X, point.Y + 1), new ModShapes.InnerOutline(data, true), Actions.Chain(new GenAction[]
			{
				new Modifiers.IsEmpty(),
				new Modifiers.RectangleMask(-6, 6, 1, 3),
				new Actions.SetTile(59, true, true, true)
			}));
			WorldUtils.Gen(new Point(point.X, point.Y), new ModShapes.All(data), Actions.Chain(new GenAction[]
			{
				new Modifiers.Expand(1),
				new Modifiers.IsBelowHeight(point.Y, true),
				new Modifiers.IsNotSolid(),
				new Modifiers.NoLiquid(2),
				new Actions.SetTile(229, true, true, true)
			}));
			structures.AddProtectedStructure(new Rectangle(point.X - 8, point.Y - 8, 16, 16), 0);
			return true;
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x0062498C File Offset: 0x00622B8C
		private static bool TooCloseToImportantLocations(Point origin)
		{
			int x = origin.X;
			int y = origin.Y;
			if (y >= Main.UnderworldLayer - 30)
			{
				return true;
			}
			int num = 150;
			for (int i = x - num; i < x + num; i += 10)
			{
				if (i > 0 && i <= Main.maxTilesX - 1)
				{
					for (int j = y - num; j < y + num; j += 10)
					{
						if (j > 0 && j <= Main.maxTilesY - 1)
						{
							if (Main.tile[i, j].active() && Main.tile[i, j].type == 226)
							{
								return true;
							}
							if (Main.tile[i, j].wall == 83 || Main.tile[i, j].wall == 3 || Main.tile[i, j].wall == 87)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}
	}
}
