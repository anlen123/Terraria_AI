using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	// Token: 0x0200050B RID: 1291
	public class CampsiteBiome : MicroBiome
	{
		// Token: 0x0600362B RID: 13867 RVA: 0x00623454 File Offset: 0x00621654
		public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
		{
			Ref<int> @ref = new Ref<int>(0);
			Ref<int> ref2 = new Ref<int>(0);
			WorldUtils.Gen(origin, new Shapes.Circle(10), Actions.Chain(new GenAction[]
			{
				new Actions.Scanner(ref2),
				new Modifiers.IsSolid(),
				new Actions.Scanner(@ref)
			}));
			if (@ref.Value < ref2.Value - 5)
			{
				return false;
			}
			int num = GenBase._random.Next(6, 10);
			int num2 = GenBase._random.Next(1, 5);
			if (!structures.CanPlace(new Rectangle(origin.X - num, origin.Y - num, num * 2, num * 2), 0))
			{
				return false;
			}
			int num3 = num + 3;
			for (int i = origin.X - num3; i <= origin.X + num3; i++)
			{
				for (int j = origin.Y - num3; j <= origin.Y + num3; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && (Main.tileDungeon[(int)tile.type] || TileID.Sets.IsAContainer[(int)tile.type] || tile.type == 226 || tile.type == 237))
					{
						return false;
					}
				}
			}
			ushort type = (ushort)((byte)(196 + WorldGen.genRand.Next(4)));
			for (int k = origin.X - num; k <= origin.X + num; k++)
			{
				for (int l = origin.Y - num; l <= origin.Y + num; l++)
				{
					if (Main.tile[k, l].active())
					{
						int type2 = (int)Main.tile[k, l].type;
						if (type2 == 53 || type2 == 396 || type2 == 397 || type2 == 404)
						{
							type = 171;
						}
						if (type2 == 161 || type2 == 147)
						{
							type = 40;
						}
						if (type2 == 60)
						{
							type = (ushort)((byte)(204 + WorldGen.genRand.Next(4)));
						}
						if (type2 == 367)
						{
							type = 178;
						}
						if (type2 == 368)
						{
							type = 180;
						}
					}
				}
			}
			ShapeData data = new ShapeData();
			WorldUtils.Gen(origin, new Shapes.Slime(num), Actions.Chain(new GenAction[]
			{
				new Modifiers.Blotches(num2, num2, num2, 1, 1.0).Output(data),
				new Modifiers.Offset(0, -2),
				new Modifiers.OnlyTiles(new ushort[]
				{
					53
				}),
				new Actions.SetTile(397, true, true, true),
				new Modifiers.OnlyWalls(new ushort[1]),
				new Actions.PlaceWall(type, true)
			}));
			WorldUtils.Gen(origin, new ModShapes.All(data), Actions.Chain(new GenAction[]
			{
				new Actions.ClearTile(false),
				new Actions.SetLiquid(0, 0),
				new Actions.SetFrames(true),
				new Modifiers.OnlyWalls(new ushort[1]),
				new Actions.PlaceWall(type, true)
			}));
			Point point;
			if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(10), new GenCondition[]
			{
				new Conditions.IsSolid()
			}), out point))
			{
				return false;
			}
			int num4 = point.Y - 1;
			bool flag = GenBase._random.Next() % 2 == 0;
			if (GenBase._random.Next() % 10 != 0)
			{
				int num5 = GenBase._random.Next(1, 4);
				int num6 = flag ? 4 : (-(num >> 1));
				for (int m = 0; m < num5; m++)
				{
					int num7 = GenBase._random.Next(1, 3);
					for (int n = 0; n < num7; n++)
					{
						WorldGen.PlaceTile(origin.X + num6 - m, num4 - n, 332, true, false, -1, 0);
					}
				}
			}
			int num8 = (num - 3) * (flag ? -1 : 1);
			if (GenBase._random.Next() % 10 != 0)
			{
				WorldGen.PlaceTile(origin.X + num8, num4, 186, false, false, -1, 0);
			}
			if (GenBase._random.Next() % 10 != 0)
			{
				if (WorldGen.SecretSeed.rainbowStuff.Enabled)
				{
					WorldGen.PlaceTile(origin.X, num4, 215, true, false, -1, 5);
				}
				else
				{
					WorldGen.PlaceTile(origin.X, num4, 215, true, false, -1, 0);
				}
				if (GenBase._tiles[origin.X, num4].active() && GenBase._tiles[origin.X, num4].type == 215)
				{
					Tile tile2 = GenBase._tiles[origin.X, num4];
					tile2.frameY += 36;
					Tile tile3 = GenBase._tiles[origin.X - 1, num4];
					tile3.frameY += 36;
					Tile tile4 = GenBase._tiles[origin.X + 1, num4];
					tile4.frameY += 36;
					Tile tile5 = GenBase._tiles[origin.X, num4 - 1];
					tile5.frameY += 36;
					Tile tile6 = GenBase._tiles[origin.X - 1, num4 - 1];
					tile6.frameY += 36;
					Tile tile7 = GenBase._tiles[origin.X + 1, num4 - 1];
					tile7.frameY += 36;
				}
			}
			structures.AddProtectedStructure(new Rectangle(origin.X - num, origin.Y - num, num * 2, num * 2), 4);
			return true;
		}
	}
}
