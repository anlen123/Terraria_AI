using System;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x0200053E RID: 1342
	public class DroneCameraTracker
	{
		// Token: 0x0600374B RID: 14155 RVA: 0x0062DD2C File Offset: 0x0062BF2C
		public void Track(Projectile proj)
		{
			this._trackedProjectile = proj;
			this._lastTrackedType = proj.type;
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x0062DD41 File Offset: 0x0062BF41
		public void WorldClear()
		{
			this._lastTrackedType = 0;
			this._trackedProjectile = null;
		}

		// Token: 0x0600374D RID: 14157 RVA: 0x0062DD54 File Offset: 0x0062BF54
		public bool TryTracking(out Vector2 cameraPosition)
		{
			cameraPosition = default(Vector2);
			if (this._trackedProjectile == null || !this._trackedProjectile.active || this._trackedProjectile.type != this._lastTrackedType || this._trackedProjectile.owner != Main.myPlayer || !Main.LocalPlayer.remoteVisionForDrone)
			{
				this._trackedProjectile = null;
				return false;
			}
			cameraPosition = this._trackedProjectile.Center;
			return true;
		}

		// Token: 0x04005B65 RID: 23397
		private Projectile _trackedProjectile;

		// Token: 0x04005B66 RID: 23398
		private int _lastTrackedType;
	}
}
