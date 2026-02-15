using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.ID;

namespace Terraria.WorldBuilding
{
	// Token: 0x020000BB RID: 187
	public class StructureMap
	{
		// Token: 0x0600178E RID: 6030 RVA: 0x004DDD14 File Offset: 0x004DBF14
		public bool CanPlace(Rectangle area, int padding = 0)
		{
			return this.CanPlace(area, TileID.Sets.GeneralPlacementTiles, padding);
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x004DDD24 File Offset: 0x004DBF24
		public bool CanPlace(Rectangle area, bool[] validTiles, int padding = 0)
		{
			object @lock = this._lock;
			bool result;
			lock (@lock)
			{
				if (area.X < 0 || area.Y < 0 || area.X + area.Width > Main.maxTilesX - 1 || area.Y + area.Height > Main.maxTilesY - 1)
				{
					result = false;
				}
				else
				{
					Rectangle rectangle = new Rectangle(area.X - padding, area.Y - padding, area.Width + padding * 2, area.Height + padding * 2);
					for (int i = 0; i < this._protectedStructures.Count; i++)
					{
						if (rectangle.Intersects(this._protectedStructures[i]))
						{
							return false;
						}
					}
					for (int j = rectangle.X; j < rectangle.X + rectangle.Width; j++)
					{
						for (int k = rectangle.Y; k < rectangle.Y + rectangle.Height; k++)
						{
							if (Main.tile[j, k].active())
							{
								ushort type = Main.tile[j, k].type;
								if (!validTiles[(int)type])
								{
									return false;
								}
							}
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x004DDE8C File Offset: 0x004DC08C
		public Rectangle GetBoundingBox()
		{
			object @lock = this._lock;
			Rectangle result;
			lock (@lock)
			{
				if (this._structures.Count == 0)
				{
					result = Rectangle.Empty;
				}
				else
				{
					Point point = new Point(this._structures.Min((Rectangle rect) => rect.Left), this._structures.Min((Rectangle rect) => rect.Top));
					Point point2 = new Point(this._structures.Max((Rectangle rect) => rect.Right), this._structures.Max((Rectangle rect) => rect.Bottom));
					result = new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
				}
			}
			return result;
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x004DDFD0 File Offset: 0x004DC1D0
		public void AddStructure(Rectangle area, int padding = 0)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				area.Inflate(padding, padding);
				this._structures.Add(area);
			}
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x004DE020 File Offset: 0x004DC220
		public void AddProtectedStructure(Rectangle area, int padding = 0)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				area.Inflate(padding, padding);
				this._structures.Add(area);
				this._protectedStructures.Add(area);
			}
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x004DE07C File Offset: 0x004DC27C
		public void Reset()
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._protectedStructures.Clear();
			}
		}

		// Token: 0x0400126C RID: 4716
		[JsonProperty]
		private readonly List<Rectangle> _structures = new List<Rectangle>(2048);

		// Token: 0x0400126D RID: 4717
		[JsonProperty]
		private readonly List<Rectangle> _protectedStructures = new List<Rectangle>(2048);

		// Token: 0x0400126E RID: 4718
		private readonly object _lock = new object();
	}
}
