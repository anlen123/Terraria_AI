using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace Terraria.Map
{
	// Token: 0x02000179 RID: 377
	public class MapUpdateQueue
	{
		// Token: 0x06001E1A RID: 7706 RVA: 0x0050263C File Offset: 0x0050083C
		public static void Add(Rectangle area)
		{
			if (Main.dedServ || WorldGen.generatingWorld || !Main.mapEnabled)
			{
				return;
			}
			area = WorldUtils.ClampToWorld(area, 0);
			object @lock = MapUpdateQueue._lock;
			lock (@lock)
			{
				MapUpdateQueue._areaUpdateQueue.Add(area);
				for (int i = area.Left; i < area.Right; i++)
				{
					for (int j = area.Top; j < area.Bottom; j++)
					{
						Main.Map.QueueUpdate(i, j);
					}
				}
			}
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x005026DC File Offset: 0x005008DC
		public static void Add(int x, int y)
		{
			if (Main.dedServ || WorldGen.generatingWorld || !Main.mapEnabled)
			{
				return;
			}
			if (!Main.Map.QueueUpdate(x, y))
			{
				return;
			}
			object @lock = MapUpdateQueue._lock;
			lock (@lock)
			{
				if (MapUpdateQueue._updateCount == MapUpdateQueue._updateQueue.Length)
				{
					if (MapUpdateQueue._updateCount >= 262144)
					{
						return;
					}
					Array.Resize<Point16>(ref MapUpdateQueue._updateQueue, MapUpdateQueue._updateCount * 2);
				}
				MapUpdateQueue._updateQueue[MapUpdateQueue._updateCount++] = new Point16(x, y);
			}
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x00502788 File Offset: 0x00500988
		public static void Update()
		{
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			object @lock = MapUpdateQueue._lock;
			lock (@lock)
			{
				MapUpdateQueue.UpdateTiles();
				MapUpdateQueue.UpdateAreas();
			}
			TimeLogger.MapChanges.AddTime(fromTimestamp);
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x005027DC File Offset: 0x005009DC
		private static void UpdateAreas()
		{
			foreach (Rectangle rectangle in MapUpdateQueue._areaUpdateQueue)
			{
				for (int i = rectangle.Left; i < rectangle.Right; i++)
				{
					for (int j = rectangle.Top; j < rectangle.Bottom; j++)
					{
						if (Main.Map.UpdateType(i, j))
						{
							MapRenderer.QueueChange(i, j);
						}
					}
				}
			}
			MapUpdateQueue._areaUpdateQueue.Clear();
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x00502878 File Offset: 0x00500A78
		private static void UpdateTiles()
		{
			for (int i = 0; i < MapUpdateQueue._updateCount; i++)
			{
				Point16 point = MapUpdateQueue._updateQueue[i];
				if (Main.Map.UpdateType((int)point.X, (int)point.Y))
				{
					MapRenderer.QueueChange((int)point.X, (int)point.Y);
				}
			}
			MapUpdateQueue._updateCount = 0;
		}

		// Token: 0x04001677 RID: 5751
		private const int MAX_QUEUED_UPDATES = 262144;

		// Token: 0x04001678 RID: 5752
		private static List<Rectangle> _areaUpdateQueue = new List<Rectangle>();

		// Token: 0x04001679 RID: 5753
		private static Point16[] _updateQueue = new Point16[1024];

		// Token: 0x0400167A RID: 5754
		private static int _updateCount = 0;

		// Token: 0x0400167B RID: 5755
		private static readonly object _lock = new object();
	}
}
