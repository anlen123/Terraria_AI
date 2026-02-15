using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent
{
	// Token: 0x0200024B RID: 587
	public class ChumBucketProjectileHelper
	{
		// Token: 0x060022F5 RID: 8949 RVA: 0x0053B4F2 File Offset: 0x005396F2
		public void OnPreUpdateAllProjectiles()
		{
			Utils.Swap<Dictionary<Point, int>>(ref this._chumCountsPendingForThisFrame, ref this._chumCountsFromLastFrame);
			this._chumCountsPendingForThisFrame.Clear();
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x0053B510 File Offset: 0x00539710
		public void AddChumLocation(Vector2 spot)
		{
			Point key = spot.ToTileCoordinates();
			int num = 0;
			this._chumCountsPendingForThisFrame.TryGetValue(key, out num);
			num++;
			this._chumCountsPendingForThisFrame[key] = num;
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x0053B548 File Offset: 0x00539748
		public int GetChumsInLocation(Point tileCoords)
		{
			int result = 0;
			this._chumCountsFromLastFrame.TryGetValue(tileCoords, out result);
			return result;
		}

		// Token: 0x04004D1C RID: 19740
		private Dictionary<Point, int> _chumCountsPendingForThisFrame = new Dictionary<Point, int>();

		// Token: 0x04004D1D RID: 19741
		private Dictionary<Point, int> _chumCountsFromLastFrame = new Dictionary<Point, int>();
	}
}
