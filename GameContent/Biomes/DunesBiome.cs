using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ReLogic.Utilities;
using Terraria.GameContent.Biomes.Desert;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	// Token: 0x02000507 RID: 1287
	public class DunesBiome : MicroBiome
	{
		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x0600360C RID: 13836 RVA: 0x00620E99 File Offset: 0x0061F099
		public int MaximumWidth
		{
			get
			{
				return this._singleDunesWidth.ScaledMaximum * 2;
			}
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x00620EA8 File Offset: 0x0061F0A8
		public override bool Place(Point origin, StructureMap structures, GenerationProgress progress)
		{
			int height = (int)((double)GenBase._random.Next(60, 100) * this._heightScale);
			int height2 = (int)((double)GenBase._random.Next(60, 100) * this._heightScale);
			int random = this._singleDunesWidth.GetRandom(GenBase._random);
			int random2 = this._singleDunesWidth.GetRandom(GenBase._random);
			DunesBiome.DunesDescription description = DunesBiome.DunesDescription.CreateFromPlacement(new Point(origin.X - random / 2 + 30, origin.Y), random, height);
			DunesBiome.DunesDescription description2 = DunesBiome.DunesDescription.CreateFromPlacement(new Point(origin.X + random2 / 2 - 30, origin.Y), random2, height2);
			this.PlaceSingle(description, structures);
			this.PlaceSingle(description2, structures);
			return true;
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x00620F5C File Offset: 0x0061F15C
		private void PlaceSingle(DunesBiome.DunesDescription description, StructureMap structures)
		{
			int num = GenBase._random.Next(3) + 8;
			for (int i = 0; i < num - 1; i++)
			{
				int num2 = (int)(2.0 / (double)num * (double)description.Area.Width);
				int num3 = (int)((double)i / (double)num * (double)description.Area.Width + (double)description.Area.Left) + num2 * 2 / 5;
				num3 += GenBase._random.Next(-5, 6);
				double num4 = (double)i / (double)(num - 2);
				double num5 = 1.0 - Math.Abs(num4 - 0.5) * 2.0;
				DunesBiome.PlaceHill(num3 - num2 / 2, num3 + num2 / 2, (num5 * 0.3 + 0.2) * this._heightScale, description);
			}
			int num6 = GenBase._random.Next(2) + 1;
			for (int j = 0; j < num6; j++)
			{
				int num7 = description.Area.Width / 2;
				int num8 = description.Area.Center.X;
				num8 += GenBase._random.Next(-10, 11);
				DunesBiome.PlaceHill(num8 - num7 / 2, num8 + num7 / 2, 0.8 * this._heightScale, description);
			}
			structures.AddStructure(description.Area, 20);
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x006210CC File Offset: 0x0061F2CC
		private static void PlaceHill(int startX, int endX, double scale, DunesBiome.DunesDescription description)
		{
			Point point = new Point(startX, (int)description.Surface[startX]);
			Point point2 = new Point(endX, (int)description.Surface[endX]);
			Point point3 = new Point((point.X + point2.X) / 2, (point.Y + point2.Y) / 2 - (int)(35.0 * scale));
			int num = (point2.X - point3.X) / 4;
			int minValue = (point2.X - point3.X) / 16;
			if (description.WindDirection == DunesBiome.WindDirection.Left)
			{
				point3.X -= WorldGen.genRand.Next(minValue, num + 1);
			}
			else
			{
				point3.X += WorldGen.genRand.Next(minValue, num + 1);
			}
			Point point4 = new Point(0, (int)(scale * 12.0));
			Point point5 = new Point(point4.X / -2, point4.Y / -2);
			DunesBiome.PlaceCurvedLine(point, point3, (description.WindDirection != DunesBiome.WindDirection.Left) ? point5 : point4, description);
			DunesBiome.PlaceCurvedLine(point3, point2, (description.WindDirection == DunesBiome.WindDirection.Left) ? point5 : point4, description);
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x006211F0 File Offset: 0x0061F3F0
		private static void PlaceCurvedLine(Point startPoint, Point endPoint, Point anchorOffset, DunesBiome.DunesDescription description)
		{
			Point p = new Point((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);
			p.X += anchorOffset.X;
			p.Y += anchorOffset.Y;
			Vector2D vector2D = startPoint.ToVector2D();
			Vector2D vector2D2 = endPoint.ToVector2D();
			Vector2D vector2D3 = p.ToVector2D();
			double num = 0.5 / (vector2D2.X - vector2D.X);
			Point b = new Point(-1, -1);
			for (double num2 = 0.0; num2 <= 1.0; num2 += num)
			{
				Vector2D vector2D4 = Vector2D.Lerp(vector2D, vector2D3, num2);
				Vector2D vector2D5 = Vector2D.Lerp(vector2D3, vector2D2, num2);
				Point point = Vector2D.Lerp(vector2D4, vector2D5, num2).ToPoint();
				if (!(point == b))
				{
					b = point;
					int num3 = description.Area.Width / 2 - Math.Abs(point.X - description.Area.Center.X);
					int num4 = (int)description.Surface[point.X] + (int)(Math.Sqrt((double)num3) * 3.0);
					for (int i = point.Y - 10; i < point.Y; i++)
					{
						if (GenBase._tiles[point.X, i].active() && GenBase._tiles[point.X, i].type != 53)
						{
							GenBase._tiles[point.X, i].ClearEverything();
						}
					}
					for (int j = point.Y; j < num4; j++)
					{
						GenBase._tiles[point.X, j].ResetToType(53);
					}
				}
			}
		}

		// Token: 0x04005AE2 RID: 23266
		[JsonProperty("SingleDunesWidth")]
		private WorldGenRange _singleDunesWidth = WorldGenRange.Empty;

		// Token: 0x04005AE3 RID: 23267
		[JsonProperty("HeightScale")]
		private double _heightScale = 1.0;

		// Token: 0x02000994 RID: 2452
		private class DunesDescription
		{
			// Token: 0x17000595 RID: 1429
			// (get) Token: 0x0600497B RID: 18811 RVA: 0x006D0C32 File Offset: 0x006CEE32
			// (set) Token: 0x0600497C RID: 18812 RVA: 0x006D0C3A File Offset: 0x006CEE3A
			public bool IsValid { get; private set; }

			// Token: 0x17000596 RID: 1430
			// (get) Token: 0x0600497D RID: 18813 RVA: 0x006D0C43 File Offset: 0x006CEE43
			// (set) Token: 0x0600497E RID: 18814 RVA: 0x006D0C4B File Offset: 0x006CEE4B
			public SurfaceMap Surface { get; private set; }

			// Token: 0x17000597 RID: 1431
			// (get) Token: 0x0600497F RID: 18815 RVA: 0x006D0C54 File Offset: 0x006CEE54
			// (set) Token: 0x06004980 RID: 18816 RVA: 0x006D0C5C File Offset: 0x006CEE5C
			public Rectangle Area { get; private set; }

			// Token: 0x17000598 RID: 1432
			// (get) Token: 0x06004981 RID: 18817 RVA: 0x006D0C65 File Offset: 0x006CEE65
			// (set) Token: 0x06004982 RID: 18818 RVA: 0x006D0C6D File Offset: 0x006CEE6D
			public DunesBiome.WindDirection WindDirection { get; private set; }

			// Token: 0x06004983 RID: 18819 RVA: 0x0000357B File Offset: 0x0000177B
			private DunesDescription()
			{
			}

			// Token: 0x06004984 RID: 18820 RVA: 0x006D0C78 File Offset: 0x006CEE78
			public static DunesBiome.DunesDescription CreateFromPlacement(Point origin, int width, int height)
			{
				Rectangle rectangle = new Rectangle(origin.X - width / 2, origin.Y - height / 2, width, height);
				return new DunesBiome.DunesDescription
				{
					Area = rectangle,
					IsValid = true,
					Surface = SurfaceMap.FromArea(rectangle.Left - 20, rectangle.Width + 40),
					WindDirection = ((WorldGen.genRand.Next(2) == 0) ? DunesBiome.WindDirection.Left : DunesBiome.WindDirection.Right)
				};
			}
		}

		// Token: 0x02000995 RID: 2453
		private enum WindDirection
		{
			// Token: 0x04007635 RID: 30261
			Left,
			// Token: 0x04007636 RID: 30262
			Right
		}
	}
}
