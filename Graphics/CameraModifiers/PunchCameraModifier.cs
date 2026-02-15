using System;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.CameraModifiers
{
	// Token: 0x0200021D RID: 541
	public class PunchCameraModifier : ICameraModifier
	{
		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060021BC RID: 8636 RVA: 0x00531B28 File Offset: 0x0052FD28
		// (set) Token: 0x060021BD RID: 8637 RVA: 0x00531B30 File Offset: 0x0052FD30
		public string UniqueIdentity { get; private set; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060021BE RID: 8638 RVA: 0x00531B39 File Offset: 0x0052FD39
		// (set) Token: 0x060021BF RID: 8639 RVA: 0x00531B41 File Offset: 0x0052FD41
		public bool Finished { get; private set; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060021C0 RID: 8640 RVA: 0x00531B4A File Offset: 0x0052FD4A
		// (set) Token: 0x060021C1 RID: 8641 RVA: 0x00531B52 File Offset: 0x0052FD52
		public bool IsAScreenShake { get; set; }

		// Token: 0x060021C2 RID: 8642 RVA: 0x00531B5C File Offset: 0x0052FD5C
		public PunchCameraModifier(Vector2 startPosition, Vector2 direction, float strength, float vibrationCyclesPerSecond, int frames, float distanceFalloff = -1f, string uniqueIdentity = null)
		{
			this._startPosition = startPosition;
			this._direction = direction;
			this._strength = strength;
			this._vibrationCyclesPerSecond = vibrationCyclesPerSecond;
			this._framesToLast = frames;
			this._distanceFalloff = distanceFalloff;
			this.UniqueIdentity = uniqueIdentity;
			this.IsAScreenShake = true;
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x00531BAC File Offset: 0x0052FDAC
		public void Update(ref CameraInfo cameraInfo)
		{
			float scaleFactor = (float)Math.Cos((double)((float)this._framesLasted / 60f * this._vibrationCyclesPerSecond * 6.2831855f));
			float scaleFactor2 = Utils.Remap((float)this._framesLasted, 0f, (float)this._framesToLast, 1f, 0f, true);
			float scaleFactor3 = Utils.Remap(Vector2.Distance(this._startPosition, cameraInfo.OriginalCameraCenter), 0f, this._distanceFalloff, 1f, 0f, true);
			if (this._distanceFalloff == -1f)
			{
				scaleFactor3 = 1f;
			}
			cameraInfo.CameraPosition += this._direction * scaleFactor * this._strength * scaleFactor2 * scaleFactor3;
			this._framesLasted++;
			if (this._framesLasted >= this._framesToLast)
			{
				this.Finished = true;
			}
		}

		// Token: 0x04004C1D RID: 19485
		private int _framesToLast;

		// Token: 0x04004C1E RID: 19486
		private Vector2 _startPosition;

		// Token: 0x04004C1F RID: 19487
		private Vector2 _direction;

		// Token: 0x04004C20 RID: 19488
		private float _distanceFalloff;

		// Token: 0x04004C21 RID: 19489
		private float _strength;

		// Token: 0x04004C22 RID: 19490
		private float _vibrationCyclesPerSecond;

		// Token: 0x04004C23 RID: 19491
		private int _framesLasted;
	}
}
