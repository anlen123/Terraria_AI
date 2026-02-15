using System;

namespace Terraria.GameContent
{
	// Token: 0x02000263 RID: 611
	public struct PlayerEyeHelper
	{
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06002386 RID: 9094 RVA: 0x0053EF18 File Offset: 0x0053D118
		// (set) Token: 0x06002387 RID: 9095 RVA: 0x0053EF20 File Offset: 0x0053D120
		public int EyeFrameToShow { get; private set; }

		// Token: 0x06002388 RID: 9096 RVA: 0x0053EF29 File Offset: 0x0053D129
		public void Update(Player player)
		{
			this.SetStateByPlayerInfo(player);
			this.UpdateEyeFrameToShow(player);
			this._timeInState++;
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x0053EF48 File Offset: 0x0053D148
		private void UpdateEyeFrameToShow(Player player)
		{
			PlayerEyeHelper.EyeFrame eyeFrameToShow = PlayerEyeHelper.EyeFrame.EyeOpen;
			switch (this._state)
			{
			case PlayerEyeHelper.EyeState.NormalBlinking:
			{
				int num = this._timeInState % 240 - 234;
				if (num >= 4)
				{
					eyeFrameToShow = PlayerEyeHelper.EyeFrame.EyeHalfClosed;
				}
				else if (num >= 2)
				{
					eyeFrameToShow = PlayerEyeHelper.EyeFrame.EyeClosed;
				}
				else if (num >= 0)
				{
					eyeFrameToShow = PlayerEyeHelper.EyeFrame.EyeHalfClosed;
				}
				else
				{
					eyeFrameToShow = PlayerEyeHelper.EyeFrame.EyeOpen;
				}
				break;
			}
			case PlayerEyeHelper.EyeState.InStorm:
				if (this._timeInState % 120 - 114 >= 0)
				{
					eyeFrameToShow = PlayerEyeHelper.EyeFrame.EyeClosed;
				}
				else
				{
					eyeFrameToShow = PlayerEyeHelper.EyeFrame.EyeHalfClosed;
				}
				break;
			case PlayerEyeHelper.EyeState.InBed:
			{
				PlayerEyeHelper.EyeFrame eyeFrame = this.DoesPlayerCountAsModeratelyDamaged(player) ? PlayerEyeHelper.EyeFrame.EyeHalfClosed : PlayerEyeHelper.EyeFrame.EyeOpen;
				this._timeInState = player.sleeping.timeSleeping;
				if (this._timeInState < 60)
				{
					eyeFrameToShow = eyeFrame;
				}
				else if (this._timeInState < 120)
				{
					eyeFrameToShow = PlayerEyeHelper.EyeFrame.EyeHalfClosed;
				}
				else
				{
					eyeFrameToShow = PlayerEyeHelper.EyeFrame.EyeClosed;
				}
				break;
			}
			case PlayerEyeHelper.EyeState.JustTookDamage:
				eyeFrameToShow = PlayerEyeHelper.EyeFrame.EyeClosed;
				break;
			case PlayerEyeHelper.EyeState.IsModeratelyDamaged:
			case PlayerEyeHelper.EyeState.IsTipsy:
			case PlayerEyeHelper.EyeState.IsPoisoned:
				if (this._timeInState % 120 - 100 >= 0)
				{
					eyeFrameToShow = PlayerEyeHelper.EyeFrame.EyeClosed;
				}
				else
				{
					eyeFrameToShow = PlayerEyeHelper.EyeFrame.EyeHalfClosed;
				}
				break;
			case PlayerEyeHelper.EyeState.IsBlind:
				eyeFrameToShow = PlayerEyeHelper.EyeFrame.EyeClosed;
				break;
			}
			this.EyeFrameToShow = (int)eyeFrameToShow;
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x0053F038 File Offset: 0x0053D238
		private void SetStateByPlayerInfo(Player player)
		{
			if (player.blackout || player.blind)
			{
				this.SwitchToState(PlayerEyeHelper.EyeState.IsBlind, false);
				return;
			}
			if (this._state == PlayerEyeHelper.EyeState.JustTookDamage && this._timeInState < 20)
			{
				return;
			}
			if (player.sleeping.isSleeping)
			{
				bool resetStateTimerEvenIfAlreadyInState = player.itemAnimation > 0;
				this.SwitchToState(PlayerEyeHelper.EyeState.InBed, resetStateTimerEvenIfAlreadyInState);
				return;
			}
			if (this.DoesPlayerCountAsModeratelyDamaged(player))
			{
				this.SwitchToState(PlayerEyeHelper.EyeState.IsModeratelyDamaged, false);
				return;
			}
			if (player.tipsy)
			{
				this.SwitchToState(PlayerEyeHelper.EyeState.IsTipsy, false);
				return;
			}
			if (player.poisoned || player.venom || player.starving)
			{
				this.SwitchToState(PlayerEyeHelper.EyeState.IsPoisoned, false);
				return;
			}
			bool flag = player.ZoneSandstorm || (player.ZoneSnow && Main.IsItRaining);
			if (player.behindBackWall)
			{
				flag = false;
			}
			if (flag)
			{
				this.SwitchToState(PlayerEyeHelper.EyeState.InStorm, false);
				return;
			}
			this.SwitchToState(PlayerEyeHelper.EyeState.NormalBlinking, false);
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x0053F110 File Offset: 0x0053D310
		private void SwitchToState(PlayerEyeHelper.EyeState newState, bool resetStateTimerEvenIfAlreadyInState = false)
		{
			if (this._state == newState && !resetStateTimerEvenIfAlreadyInState)
			{
				return;
			}
			this._state = newState;
			this._timeInState = 0;
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x0053F12D File Offset: 0x0053D32D
		private bool DoesPlayerCountAsModeratelyDamaged(Player player)
		{
			return (float)player.statLife <= (float)player.statLifeMax2 * 0.25f;
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x0053F148 File Offset: 0x0053D348
		public void BlinkBecausePlayerGotHurt()
		{
			this.SwitchToState(PlayerEyeHelper.EyeState.JustTookDamage, true);
		}

		// Token: 0x04004D71 RID: 19825
		private PlayerEyeHelper.EyeState _state;

		// Token: 0x04004D72 RID: 19826
		private int _timeInState;

		// Token: 0x04004D74 RID: 19828
		private const int TimeToActDamaged = 20;

		// Token: 0x020007E7 RID: 2023
		private enum EyeFrame
		{
			// Token: 0x04007102 RID: 28930
			EyeOpen,
			// Token: 0x04007103 RID: 28931
			EyeHalfClosed,
			// Token: 0x04007104 RID: 28932
			EyeClosed
		}

		// Token: 0x020007E8 RID: 2024
		private enum EyeState
		{
			// Token: 0x04007106 RID: 28934
			NormalBlinking,
			// Token: 0x04007107 RID: 28935
			InStorm,
			// Token: 0x04007108 RID: 28936
			InBed,
			// Token: 0x04007109 RID: 28937
			JustTookDamage,
			// Token: 0x0400710A RID: 28938
			IsModeratelyDamaged,
			// Token: 0x0400710B RID: 28939
			IsBlind,
			// Token: 0x0400710C RID: 28940
			IsTipsy,
			// Token: 0x0400710D RID: 28941
			IsPoisoned
		}
	}
}
