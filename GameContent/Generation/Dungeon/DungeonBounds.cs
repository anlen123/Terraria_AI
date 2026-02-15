using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ReLogic.Utilities;
using Terraria.Utilities;

namespace Terraria.GameContent.Generation.Dungeon
{
	// Token: 0x02000491 RID: 1169
	public class DungeonBounds
	{
		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x0600337D RID: 13181 RVA: 0x005F889B File Offset: 0x005F6A9B
		public Rectangle Hitbox
		{
			get
			{
				if (this._hitbox != null)
				{
					return this._hitbox.Value;
				}
				return Rectangle.Empty;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x0600337E RID: 13182 RVA: 0x005F88BB File Offset: 0x005F6ABB
		public int X
		{
			get
			{
				return this._boundsLeft;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x0600337F RID: 13183 RVA: 0x005F88C3 File Offset: 0x005F6AC3
		public int Y
		{
			get
			{
				return this._boundsTop;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06003380 RID: 13184 RVA: 0x005F88CB File Offset: 0x005F6ACB
		public int Width
		{
			get
			{
				return this._boundsRight - this._boundsLeft;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06003381 RID: 13185 RVA: 0x005F88DA File Offset: 0x005F6ADA
		public int Height
		{
			get
			{
				return this._boundsBottom - this._boundsTop;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06003382 RID: 13186 RVA: 0x005F88E9 File Offset: 0x005F6AE9
		public int Size
		{
			get
			{
				if (this.Width <= this.Height)
				{
					return this.Height;
				}
				return this.Width;
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06003383 RID: 13187 RVA: 0x005F88BB File Offset: 0x005F6ABB
		// (set) Token: 0x06003384 RID: 13188 RVA: 0x005F8906 File Offset: 0x005F6B06
		public int Left
		{
			get
			{
				return this._boundsLeft;
			}
			set
			{
				this._boundsLeft = (int)MathHelper.Clamp((float)value, 10f, (float)(Main.maxTilesX - 10));
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06003385 RID: 13189 RVA: 0x005F8924 File Offset: 0x005F6B24
		// (set) Token: 0x06003386 RID: 13190 RVA: 0x005F892C File Offset: 0x005F6B2C
		public int Right
		{
			get
			{
				return this._boundsRight;
			}
			set
			{
				this._boundsRight = (int)MathHelper.Clamp((float)value, 10f, (float)(Main.maxTilesX - 10));
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06003387 RID: 13191 RVA: 0x005F88C3 File Offset: 0x005F6AC3
		// (set) Token: 0x06003388 RID: 13192 RVA: 0x005F894A File Offset: 0x005F6B4A
		public int Top
		{
			get
			{
				return this._boundsTop;
			}
			set
			{
				this._boundsTop = (int)MathHelper.Clamp((float)value, 10f, (float)(Main.maxTilesY - 10));
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06003389 RID: 13193 RVA: 0x005F8968 File Offset: 0x005F6B68
		// (set) Token: 0x0600338A RID: 13194 RVA: 0x005F8970 File Offset: 0x005F6B70
		public int Bottom
		{
			get
			{
				return this._boundsBottom;
			}
			set
			{
				this._boundsBottom = (int)MathHelper.Clamp((float)value, 10f, (float)(Main.maxTilesY - 10));
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x0600338B RID: 13195 RVA: 0x005F898E File Offset: 0x005F6B8E
		public Point Center
		{
			get
			{
				return new Point((this.Left + this.Right) / 2, (this.Top + this.Bottom) / 2);
			}
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x005F89B3 File Offset: 0x005F6BB3
		public Point RandomPointInBounds(UnifiedRandom genRand)
		{
			return new Point(genRand.Next(this.Left, this.Right + 1), genRand.Next(this.Top, this.Bottom + 1));
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x005F89E2 File Offset: 0x005F6BE2
		public void Inflate(int amount)
		{
			this.SetBounds(this.Left - amount, this.Top - amount, this.Right + amount, this.Bottom + amount);
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x005F8A0A File Offset: 0x005F6C0A
		public void Shrink(int amount)
		{
			this.SetBounds(this.Left + amount, this.Top + amount, this.Right - amount, this.Bottom - amount);
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x005F8A32 File Offset: 0x005F6C32
		public bool ContainsWithFluff(Vector2 point, int fluff)
		{
			if (fluff == 0)
			{
				return this.Contains((int)point.X, (int)point.Y);
			}
			return this.ContainsWithFluff((int)point.X, (int)point.Y, fluff);
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x005F8A61 File Offset: 0x005F6C61
		public bool ContainsWithFluff(Vector2D point, int fluff)
		{
			if (fluff == 0)
			{
				return this.Contains((int)point.X, (int)point.Y);
			}
			return this.ContainsWithFluff((int)point.X, (int)point.Y, fluff);
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x005F8A90 File Offset: 0x005F6C90
		public bool ContainsWithFluff(Point point, int fluff)
		{
			if (fluff == 0)
			{
				return this.Contains(point.X, point.Y);
			}
			return this.ContainsWithFluff(point.X, point.Y, fluff);
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x005F8ABC File Offset: 0x005F6CBC
		public bool ContainsWithFluff(int x, int y, int fluff)
		{
			if (fluff == 0)
			{
				return this.Contains(x, y);
			}
			if (this._hitbox == null)
			{
				return false;
			}
			Rectangle rectangle = new Rectangle(this._hitbox.Value.Left - fluff, this._hitbox.Value.Top - fluff, this._hitbox.Value.Width + fluff * 2, this._hitbox.Value.Height + fluff * 2);
			return rectangle.Contains(x, y);
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x005F8B46 File Offset: 0x005F6D46
		public bool Contains(Vector2D point)
		{
			return this.Contains((int)point.X, (int)point.Y);
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x005F8B5C File Offset: 0x005F6D5C
		public bool Contains(Point point)
		{
			return this.Contains(point.X, point.Y);
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x005F8B70 File Offset: 0x005F6D70
		public bool Contains(int x, int y)
		{
			return this._hitbox != null && this._hitbox.Value.Contains(x, y);
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x005F8BA1 File Offset: 0x005F6DA1
		public bool Intersects(DungeonBounds bounds)
		{
			return bounds.HasHitbox() && this.Intersects(bounds.Hitbox);
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x005F8BBC File Offset: 0x005F6DBC
		public bool Intersects(Rectangle hitbox)
		{
			return this._hitbox != null && this._hitbox.Value.Intersects(hitbox);
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x005F8BEC File Offset: 0x005F6DEC
		public bool IntersectsWithLineThreePointCheck(Point startPoint, Point endPoint)
		{
			return this.IntersectsWithLineThreePointCheck(startPoint.ToVector2D(), endPoint.ToVector2D());
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x005F8C00 File Offset: 0x005F6E00
		public bool IntersectsWithLineThreePointCheck(int startPointX, int startPointY, int endPointX, int endPointY)
		{
			return this.IntersectsWithLineThreePointCheck(new Vector2D((double)startPointX, (double)startPointY), new Vector2D((double)endPointX, (double)endPointY));
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x005F8C1C File Offset: 0x005F6E1C
		public bool IntersectsWithLineThreePointCheck(Vector2D startPoint, Vector2D endPoint)
		{
			return this._hitbox != null && (this.Contains(startPoint) || this.Contains(endPoint) || this.Contains((startPoint + endPoint) / 2.0));
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x005F8C6A File Offset: 0x005F6E6A
		public bool HasHitbox()
		{
			return this._hitbox != null;
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x005F8C77 File Offset: 0x005F6E77
		public void SetBoundsLeft(int minX)
		{
			this.Left = minX;
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x005F8C80 File Offset: 0x005F6E80
		public void SetBoundsRight(int maxX)
		{
			this.Right = maxX;
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x005F8C89 File Offset: 0x005F6E89
		public void SetBoundsTop(int minY)
		{
			this.Top = minY;
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x005F8C92 File Offset: 0x005F6E92
		public void SetBoundsBottom(int maxY)
		{
			this.Bottom = maxY;
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x005F8C9B File Offset: 0x005F6E9B
		public void SetBounds(Rectangle rect)
		{
			this.SetBounds(rect.Left, rect.Top, rect.Right, rect.Bottom);
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x005F8CBF File Offset: 0x005F6EBF
		public void SetBounds(int minX, int minY, int maxX, int maxY)
		{
			this.Left = minX;
			this.Right = maxX;
			this.Top = minY;
			this.Bottom = maxY;
			this.CalculateHitbox();
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x005F8CE8 File Offset: 0x005F6EE8
		public void UpdateBounds(int x, int y)
		{
			if (x < this._boundsLeft)
			{
				this.Left = x;
			}
			if (x > this._boundsRight)
			{
				this.Right = x;
			}
			if (y < this._boundsTop)
			{
				this.Top = y;
			}
			if (y > this._boundsBottom)
			{
				this.Bottom = y;
			}
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x005F8D38 File Offset: 0x005F6F38
		public void UpdateBounds(DungeonBounds bounds)
		{
			if (this.Width == 0 || this.Height == 0)
			{
				this.SetBounds(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
				return;
			}
			this.UpdateBounds(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x005F8D94 File Offset: 0x005F6F94
		public void UpdateBounds(int minX, int minY, int maxX, int maxY)
		{
			if (minX < this._boundsLeft)
			{
				this.Left = minX;
			}
			if (maxX > this._boundsRight)
			{
				this.Right = maxX;
			}
			if (minY < this._boundsTop)
			{
				this.Top = minY;
			}
			if (maxY > this._boundsBottom)
			{
				this.Bottom = maxY;
			}
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x005F8DE4 File Offset: 0x005F6FE4
		public Rectangle CalculateHitbox()
		{
			if (this.Right <= this.Left)
			{
				this.Right = this.Left + 1;
			}
			if (this.Bottom <= this.Top)
			{
				this.Bottom = this.Top + 1;
			}
			this._hitbox = new Rectangle?(new Rectangle(this.X, this.Y, this.Width, this.Height));
			return this._hitbox.Value;
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x005F8E5C File Offset: 0x005F705C
		public void Reset()
		{
			this._hitbox = null;
			this.Left = 0;
			this.Right = 0;
			this.Top = 0;
			this.Bottom = 0;
		}

		// Token: 0x040058C4 RID: 22724
		[JsonProperty]
		private Rectangle? _hitbox;

		// Token: 0x040058C5 RID: 22725
		private int _boundsLeft;

		// Token: 0x040058C6 RID: 22726
		private int _boundsRight;

		// Token: 0x040058C7 RID: 22727
		private int _boundsTop;

		// Token: 0x040058C8 RID: 22728
		private int _boundsBottom;
	}
}
