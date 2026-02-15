using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Golf
{
	// Token: 0x02000318 RID: 792
	public class GolfBallTrackRecord
	{
		// Token: 0x0600274D RID: 10061 RVA: 0x00566534 File Offset: 0x00564734
		public void RecordHit(Vector2 position)
		{
			this._hitLocations.Add(position);
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x00566544 File Offset: 0x00564744
		public int GetAccumulatedScore()
		{
			double num;
			int num2;
			this.GetTrackInfo(out num, out num2);
			int num3 = (int)(num / 16.0);
			int num4 = num2 + 2;
			return num3 / num4;
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x00566570 File Offset: 0x00564770
		private void GetTrackInfo(out double totalDistancePassed, out int hitsMade)
		{
			hitsMade = 0;
			totalDistancePassed = 0.0;
			int i = 0;
			while (i < this._hitLocations.Count - 1)
			{
				totalDistancePassed += (double)Vector2.Distance(this._hitLocations[i], this._hitLocations[i + 1]);
				i++;
				hitsMade++;
			}
		}

		// Token: 0x040050B1 RID: 20657
		private List<Vector2> _hitLocations = new List<Vector2>();
	}
}
