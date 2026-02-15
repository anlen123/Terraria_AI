using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent
{
	// Token: 0x02000266 RID: 614
	public class SpelunkerProjectileHelper
	{
		// Token: 0x060023D1 RID: 9169 RVA: 0x00547A14 File Offset: 0x00545C14
		public void OnPreUpdateAllProjectiles()
		{
			this._clampBox = new Rectangle(2, 2, Main.maxTilesX - 2, Main.maxTilesY - 2);
			int num = this._frameCounter + 1;
			this._frameCounter = num;
			if (num >= 10)
			{
				this._frameCounter = 0;
				this._tilesChecked.Clear();
				this._positionsChecked.Clear();
			}
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x00547A6E File Offset: 0x00545C6E
		public void AddSpotToCheck(Vector2 spot)
		{
			if (this._positionsChecked.Add(spot))
			{
				this.CheckSpot(spot);
			}
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x00547A88 File Offset: 0x00545C88
		private void CheckSpot(Vector2 Center)
		{
			int num = (int)Center.X / 16;
			int num2 = (int)Center.Y / 16;
			int num3 = Utils.Clamp<int>(num - 30, this._clampBox.Left, this._clampBox.Right);
			int num4 = Utils.Clamp<int>(num + 30, this._clampBox.Left, this._clampBox.Right);
			int num5 = Utils.Clamp<int>(num2 - 30, this._clampBox.Top, this._clampBox.Bottom);
			int num6 = Utils.Clamp<int>(num2 + 30, this._clampBox.Top, this._clampBox.Bottom);
			Point item = default(Point);
			Vector2 position = default(Vector2);
			for (int i = num3; i <= num4; i++)
			{
				for (int j = num5; j <= num6; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.active() && Main.IsTileSpelunkable(tile))
					{
						Vector2 vector = new Vector2((float)(num - i), (float)(num2 - j));
						if (vector.Length() <= 30f)
						{
							item.X = i;
							item.Y = j;
							if (this._tilesChecked.Add(item) && Main.rand.Next(4) == 0)
							{
								position.X = (float)(i * 16);
								position.Y = (float)(j * 16);
								Dust dust = Dust.NewDustDirect(position, 16, 16, 204, 0f, 0f, 150, default(Color), 0.3f);
								dust.fadeIn = 0.75f;
								dust.velocity *= 0.1f;
								dust.noLight = true;
							}
						}
					}
				}
			}
		}

		// Token: 0x04004D8A RID: 19850
		private HashSet<Vector2> _positionsChecked = new HashSet<Vector2>();

		// Token: 0x04004D8B RID: 19851
		private HashSet<Point> _tilesChecked = new HashSet<Point>();

		// Token: 0x04004D8C RID: 19852
		private Rectangle _clampBox;

		// Token: 0x04004D8D RID: 19853
		private int _frameCounter;
	}
}
