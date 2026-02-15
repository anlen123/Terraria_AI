using System;
using Steamworks;
using Terraria.Social.Base;

namespace Terraria.Social.Steam
{
	// Token: 0x02000143 RID: 323
	public class OverlaySocialModule : OverlaySocialModule
	{
		// Token: 0x06001C92 RID: 7314 RVA: 0x004FE530 File Offset: 0x004FC730
		public override void Initialize()
		{
			this._gamepadTextInputDismissed = Callback<GamepadTextInputDismissed_t>.Create(new Callback<GamepadTextInputDismissed_t>.DispatchDelegate(this.OnGamepadTextInputDismissed));
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Shutdown()
		{
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x004FE549 File Offset: 0x004FC749
		public override bool IsGamepadTextInputActive()
		{
			return this._gamepadTextInputActive;
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x004FE551 File Offset: 0x004FC751
		public override bool ShowGamepadTextInput(string description, uint maxLength, bool multiLine = false, string existingText = "", bool password = false)
		{
			if (this._gamepadTextInputActive)
			{
				return false;
			}
			bool flag = SteamUtils.ShowGamepadTextInput(password ? 1 : 0, multiLine ? 1 : 0, description, maxLength, existingText);
			if (flag)
			{
				this._gamepadTextInputActive = true;
			}
			return flag;
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x004FE580 File Offset: 0x004FC780
		public override string GetGamepadText()
		{
			uint enteredGamepadTextLength = SteamUtils.GetEnteredGamepadTextLength();
			string result;
			SteamUtils.GetEnteredGamepadTextInput(ref result, enteredGamepadTextLength);
			return result;
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x004FE59D File Offset: 0x004FC79D
		private void OnGamepadTextInputDismissed(GamepadTextInputDismissed_t result)
		{
			this._gamepadTextInputActive = false;
		}

		// Token: 0x040015CA RID: 5578
		private Callback<GamepadTextInputDismissed_t> _gamepadTextInputDismissed;

		// Token: 0x040015CB RID: 5579
		private bool _gamepadTextInputActive;
	}
}
