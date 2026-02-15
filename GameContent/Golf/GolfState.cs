using System;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent.Golf
{
	// Token: 0x0200031A RID: 794
	public class GolfState
	{
		// Token: 0x0600275F RID: 10079 RVA: 0x00566D3A File Offset: 0x00564F3A
		private void UpdateScoreTime()
		{
			if (this.golfScoreTime < this.golfScoreTimeMax)
			{
				this.golfScoreTime++;
			}
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x00566D58 File Offset: 0x00564F58
		public void ResetScoreTime()
		{
			this.golfScoreTime = 0;
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x00566D61 File Offset: 0x00564F61
		public void SetScoreTime()
		{
			this.golfScoreTime = this.golfScoreTimeMax;
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06002762 RID: 10082 RVA: 0x00566D6F File Offset: 0x00564F6F
		public float ScoreAdjustment
		{
			get
			{
				return (float)this.golfScoreTime / (float)this.golfScoreTimeMax;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06002763 RID: 10083 RVA: 0x00566D80 File Offset: 0x00564F80
		public bool ShouldScoreHole
		{
			get
			{
				return this.golfScoreTime >= this.golfScoreDelay;
			}
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x00566D94 File Offset: 0x00564F94
		public bool TryGetCameraTrackingPosition(out Vector2 cameraPosition)
		{
			Projectile lastHitBall = this.GetLastHitBall();
			if (lastHitBall != null && this._waitingForBallToSettle)
			{
				cameraPosition = lastHitBall.Center;
				return true;
			}
			if (this._lastRecordedBallTime + 2.0 >= Main.gameTimeCache.TotalGameTime.TotalSeconds && lastHitBall == null && this._lastRecordedBallLocation != null)
			{
				cameraPosition = this._lastRecordedBallLocation.Value;
				return true;
			}
			cameraPosition = default(Vector2);
			return false;
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x00566E10 File Offset: 0x00565010
		public void WorldClear()
		{
			this._lastHitGolfBall = null;
			this._lastRecordedBallLocation = null;
			this._lastRecordedBallTime = 0.0;
			this._lastRecordedSwingCount = 0;
			this._waitingForBallToSettle = false;
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x00566E42 File Offset: 0x00565042
		public void CancelBallTracking()
		{
			this._waitingForBallToSettle = false;
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x00566E4C File Offset: 0x0056504C
		public void RecordSwing(Projectile golfBall)
		{
			this._lastSwingPosition = golfBall.position;
			this._lastHitGolfBall = golfBall;
			this._lastRecordedSwingCount = (int)golfBall.ai[1];
			this._waitingForBallToSettle = true;
			int golfBallId = this.GetGolfBallId(golfBall);
			if (this._hitRecords[golfBallId] == null || this._lastRecordedSwingCount == 1)
			{
				this._hitRecords[golfBallId] = new GolfBallTrackRecord();
			}
			this._hitRecords[golfBallId].RecordHit(golfBall.position);
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x00566EBD File Offset: 0x005650BD
		private int GetGolfBallId(Projectile golfBall)
		{
			return golfBall.whoAmI;
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x00566EC8 File Offset: 0x005650C8
		public Projectile GetLastHitBall()
		{
			if (this._lastHitGolfBall == null || !this._lastHitGolfBall.active || !ProjectileID.Sets.IsAGolfBall[this._lastHitGolfBall.type] || this._lastHitGolfBall.owner != Main.myPlayer || this._lastRecordedSwingCount != (int)this._lastHitGolfBall.ai[1])
			{
				return null;
			}
			return this._lastHitGolfBall;
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x00566F30 File Offset: 0x00565130
		public void Update()
		{
			this.UpdateScoreTime();
			Projectile lastHitBall = this.GetLastHitBall();
			if (lastHitBall == null)
			{
				this._waitingForBallToSettle = false;
				return;
			}
			if (this._waitingForBallToSettle)
			{
				this._waitingForBallToSettle = ((int)lastHitBall.localAI[1] == 1);
			}
			bool flag = false;
			int type = Main.LocalPlayer.HeldItem.type;
			if (type == 3611)
			{
				flag = true;
			}
			if (!Item.IsAGolfingItem(Main.LocalPlayer.HeldItem) && !flag)
			{
				this._waitingForBallToSettle = false;
			}
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x00566FA8 File Offset: 0x005651A8
		public void RecordBallInfo(Projectile golfBall)
		{
			if (this.GetLastHitBall() != golfBall || !this._waitingForBallToSettle)
			{
				return;
			}
			this._lastRecordedBallLocation = new Vector2?(golfBall.Center);
			this._lastRecordedBallTime = Main.gameTimeCache.TotalGameTime.TotalSeconds;
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x00566FF0 File Offset: 0x005651F0
		public void LandBall(Projectile golfBall)
		{
			int golfBallId = this.GetGolfBallId(golfBall);
			GolfBallTrackRecord golfBallTrackRecord = this._hitRecords[golfBallId];
			if (golfBallTrackRecord == null)
			{
				return;
			}
			golfBallTrackRecord.RecordHit(golfBall.position);
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x00567020 File Offset: 0x00565220
		public int GetGolfBallScore(Projectile golfBall)
		{
			int golfBallId = this.GetGolfBallId(golfBall);
			GolfBallTrackRecord golfBallTrackRecord = this._hitRecords[golfBallId];
			if (golfBallTrackRecord == null)
			{
				return 0;
			}
			return (int)((float)golfBallTrackRecord.GetAccumulatedScore() * this.ScoreAdjustment);
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x00567054 File Offset: 0x00565254
		public void ResetGolfBall()
		{
			Projectile lastHitBall = this.GetLastHitBall();
			if (lastHitBall == null)
			{
				return;
			}
			if (Vector2.Distance(lastHitBall.position, this._lastSwingPosition) < 1f)
			{
				return;
			}
			lastHitBall.position = this._lastSwingPosition;
			lastHitBall.velocity = Vector2.Zero;
			lastHitBall.ai[1] += 1f;
			lastHitBall.netUpdate2 = true;
			this._lastRecordedSwingCount = (int)lastHitBall.ai[1];
		}

		// Token: 0x040050B8 RID: 20664
		private const int BALL_RETURN_PENALTY = 1;

		// Token: 0x040050B9 RID: 20665
		private int golfScoreTime;

		// Token: 0x040050BA RID: 20666
		private int golfScoreTimeMax = 3600;

		// Token: 0x040050BB RID: 20667
		private int golfScoreDelay = 90;

		// Token: 0x040050BC RID: 20668
		private double _lastRecordedBallTime;

		// Token: 0x040050BD RID: 20669
		private Vector2? _lastRecordedBallLocation;

		// Token: 0x040050BE RID: 20670
		private bool _waitingForBallToSettle;

		// Token: 0x040050BF RID: 20671
		private Vector2 _lastSwingPosition;

		// Token: 0x040050C0 RID: 20672
		private Projectile _lastHitGolfBall;

		// Token: 0x040050C1 RID: 20673
		private int _lastRecordedSwingCount;

		// Token: 0x040050C2 RID: 20674
		private GolfBallTrackRecord[] _hitRecords = new GolfBallTrackRecord[1000];
	}
}
