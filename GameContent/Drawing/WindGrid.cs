using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x02000446 RID: 1094
	public class WindGrid
	{
		// Token: 0x060031B2 RID: 12722 RVA: 0x005E1EBA File Offset: 0x005E00BA
		public void SetSize(int targetWidth, int targetHeight)
		{
			this._width = Math.Max(this._width, targetWidth);
			this._height = Math.Max(this._height, targetHeight);
			this.ResizeGrid();
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x005E1EE6 File Offset: 0x005E00E6
		public void Update()
		{
			this._gameTime++;
			if (Main.SettingsEnabled_TilesSwayInWind)
			{
				this.ScanPlayers();
			}
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x005E1F04 File Offset: 0x005E0104
		public void GetWindTime(int tileX, int tileY, int timeThreshold, out int windTimeLeft, out int directionX, out int directionY)
		{
			WindGrid.WindCoord windCoord = this._grid[tileX % this._width, tileY % this._height];
			directionX = windCoord.DirectionX;
			directionY = windCoord.DirectionY;
			if (windCoord.Time + timeThreshold < this._gameTime)
			{
				windTimeLeft = 0;
				return;
			}
			windTimeLeft = this._gameTime - windCoord.Time;
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x005E1F64 File Offset: 0x005E0164
		private void ResizeGrid()
		{
			if (this._width <= this._grid.GetLength(0) && this._height <= this._grid.GetLength(1))
			{
				return;
			}
			this._grid = new WindGrid.WindCoord[this._width, this._height];
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x005E1FB4 File Offset: 0x005E01B4
		private void SetWindTime(int tileX, int tileY, int directionX, int directionY)
		{
			int num = tileX % this._width;
			int num2 = tileY % this._height;
			this._grid[num, num2].Time = this._gameTime;
			this._grid[num, num2].DirectionX = directionX;
			this._grid[num, num2].DirectionY = directionY;
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x005E2014 File Offset: 0x005E0214
		private void ScanPlayers()
		{
			if (Main.netMode == 0)
			{
				this.ScanPlayer(Main.myPlayer);
				return;
			}
			if (Main.netMode == 1)
			{
				for (int i = 0; i < 255; i++)
				{
					this.ScanPlayer(i);
				}
			}
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x005E2054 File Offset: 0x005E0254
		private void ScanPlayer(int i)
		{
			Player player = Main.player[i];
			if (!player.active || player.dead || (player.velocity.X == 0f && player.velocity.Y == 0f))
			{
				return;
			}
			if (!Utils.CenteredRectangle(Main.Camera.Center, Main.Camera.UnscaledSize).Intersects(player.Hitbox))
			{
				return;
			}
			if (player.velocity.HasNaNs())
			{
				return;
			}
			int directionX = Math.Sign(player.velocity.X);
			int directionY = Math.Sign(player.velocity.Y);
			foreach (Point point in Collision.GetTilesIn(player.TopLeft, player.BottomRight))
			{
				this.SetWindTime(point.X, point.Y, directionX, directionY);
			}
		}

		// Token: 0x0400578B RID: 22411
		private WindGrid.WindCoord[,] _grid = new WindGrid.WindCoord[1, 1];

		// Token: 0x0400578C RID: 22412
		private int _width = 1;

		// Token: 0x0400578D RID: 22413
		private int _height = 1;

		// Token: 0x0400578E RID: 22414
		private int _gameTime;

		// Token: 0x02000946 RID: 2374
		private struct WindCoord
		{
			// Token: 0x0400752E RID: 29998
			public int Time;

			// Token: 0x0400752F RID: 29999
			public int DirectionX;

			// Token: 0x04007530 RID: 30000
			public int DirectionY;
		}
	}
}
