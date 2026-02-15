using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ReLogic.Peripherals.RGB;
using Terraria.GameInput;
using Terraria.Utilities;

namespace Terraria.GameContent
{
	// Token: 0x02000279 RID: 633
	public class ChromaHotkeyPainter
	{
		// Token: 0x1700037D RID: 893
		// (get) Token: 0x0600243E RID: 9278 RVA: 0x0054B9CE File Offset: 0x00549BCE
		public bool PotionAlert
		{
			get
			{
				return this._quickHealAlert != 0;
			}
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x0054B9DC File Offset: 0x00549BDC
		public void CollectBoundKeys()
		{
			foreach (KeyValuePair<string, ChromaHotkeyPainter.PaintKey> keyValuePair in this._keys)
			{
				keyValuePair.Value.Unbind();
			}
			this._keys.Clear();
			foreach (KeyValuePair<string, List<string>> keyValuePair2 in PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus)
			{
				this._keys.Add(keyValuePair2.Key, new ChromaHotkeyPainter.PaintKey(keyValuePair2.Key, keyValuePair2.Value));
			}
			foreach (KeyValuePair<string, ChromaHotkeyPainter.PaintKey> keyValuePair3 in this._keys)
			{
				keyValuePair3.Value.Bind();
			}
			this._wasdKeys = new List<ChromaHotkeyPainter.PaintKey>
			{
				this._keys["Up"],
				this._keys["Down"],
				this._keys["Left"],
				this._keys["Right"]
			};
			this._healKey = this._keys["QuickHeal"];
			this._mountKey = this._keys["QuickMount"];
			this._jumpKey = this._keys["Jump"];
			this._grappleKey = this._keys["Grapple"];
			this._throwKey = this._keys["Throw"];
			this._manaKey = this._keys["QuickMana"];
			this._buffKey = this._keys["QuickBuff"];
			this._smartCursorKey = this._keys["SmartCursor"];
			this._smartSelectKey = this._keys["SmartSelect"];
			this._reactiveKeys.Clear();
			this._xnaKeysInUse.Clear();
			foreach (KeyValuePair<string, ChromaHotkeyPainter.PaintKey> keyValuePair4 in this._keys)
			{
				this._xnaKeysInUse.AddRange(keyValuePair4.Value.GetXNAKeysInUse());
			}
			this._xnaKeysInUse = this._xnaKeysInUse.Distinct<Keys>().ToList<Keys>();
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x00009E06 File Offset: 0x00008006
		[Old("Reactive keys are no longer used so this catch-all method isn't used")]
		public void PressKey(Keys key)
		{
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x0054BCA0 File Offset: 0x00549EA0
		private ChromaHotkeyPainter.ReactiveRGBKey FindReactiveKey(Keys keyTarget)
		{
			return this._reactiveKeys.FirstOrDefault((ChromaHotkeyPainter.ReactiveRGBKey x) => x.XNAKey == keyTarget);
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x0054BCD4 File Offset: 0x00549ED4
		public void Update()
		{
			this._player = Main.LocalPlayer;
			if (!FocusHelper.AllowChroma)
			{
				this.Step_ClearAll();
				return;
			}
			if (this.PotionAlert)
			{
				foreach (KeyValuePair<string, ChromaHotkeyPainter.PaintKey> keyValuePair in this._keys)
				{
					if (keyValuePair.Key != "QuickHeal")
					{
						keyValuePair.Value.SetClear();
					}
				}
				this.Step_QuickHeal();
			}
			else
			{
				this.Step_Movement();
				this.Step_QuickHeal();
			}
			if (Main.InGameUI.CurrentState == Main.ManageControlsMenu)
			{
				this.Step_ClearAll();
				this.Step_KeybindsMenu();
			}
			this.Step_UpdateReactiveKeys();
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x0054BD98 File Offset: 0x00549F98
		private void SetGroupColorBase(List<ChromaHotkeyPainter.PaintKey> keys, Color color)
		{
			foreach (ChromaHotkeyPainter.PaintKey paintKey in keys)
			{
				paintKey.SetSolid(color);
			}
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x0054BDE4 File Offset: 0x00549FE4
		private void SetGroupClear(List<ChromaHotkeyPainter.PaintKey> keys)
		{
			foreach (ChromaHotkeyPainter.PaintKey paintKey in keys)
			{
				paintKey.SetClear();
			}
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x0054BE30 File Offset: 0x0054A030
		private void Step_KeybindsMenu()
		{
			this.SetGroupColorBase(this._wasdKeys, ChromaHotkeyPainter.PainterColors.MovementKeys);
			this._jumpKey.SetSolid(ChromaHotkeyPainter.PainterColors.MovementKeys);
			this._grappleKey.SetSolid(ChromaHotkeyPainter.PainterColors.QuickGrapple);
			this._mountKey.SetSolid(ChromaHotkeyPainter.PainterColors.QuickMount);
			this._quickHealAlert = 0;
			this._healKey.SetSolid(ChromaHotkeyPainter.PainterColors.QuickHealReady);
			this._manaKey.SetSolid(ChromaHotkeyPainter.PainterColors.QuickMana);
			this._throwKey.SetSolid(ChromaHotkeyPainter.PainterColors.Throw);
			this._smartCursorKey.SetSolid(ChromaHotkeyPainter.PainterColors.SmartCursor);
			this._smartSelectKey.SetSolid(ChromaHotkeyPainter.PainterColors.SmartSelect);
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x0054BED8 File Offset: 0x0054A0D8
		private void Step_UpdateReactiveKeys()
		{
			using (List<ChromaHotkeyPainter.ReactiveRGBKey>.Enumerator enumerator = this._reactiveKeys.FindAll((ChromaHotkeyPainter.ReactiveRGBKey x) => x.Expired).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ChromaHotkeyPainter.ReactiveRGBKey key = enumerator.Current;
					key.Clear();
					if (!this._keys.Any((KeyValuePair<string, ChromaHotkeyPainter.PaintKey> x) => x.Value.UsesKey(key.XNAKey)))
					{
						key.Unbind();
					}
				}
			}
			this._reactiveKeys.RemoveAll((ChromaHotkeyPainter.ReactiveRGBKey x) => x.Expired);
			foreach (ChromaHotkeyPainter.ReactiveRGBKey reactiveRGBKey in this._reactiveKeys)
			{
				reactiveRGBKey.Update();
			}
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x0054BFEC File Offset: 0x0054A1EC
		private void Step_ClearAll()
		{
			foreach (KeyValuePair<string, ChromaHotkeyPainter.PaintKey> keyValuePair in this._keys)
			{
				keyValuePair.Value.SetClear();
			}
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x0054C044 File Offset: 0x0054A244
		private void Step_SmartKeys()
		{
			ChromaHotkeyPainter.PaintKey smartCursorKey = this._smartCursorKey;
			ChromaHotkeyPainter.PaintKey smartSelectKey = this._smartSelectKey;
			if (this._player.dead || this._player.frozen || this._player.tongued || this._player.webbed || this._player.stoned || this._player.noItems)
			{
				smartCursorKey.SetClear();
				smartSelectKey.SetClear();
				return;
			}
			if (Main.SmartCursorWanted)
			{
				smartCursorKey.SetSolid(ChromaHotkeyPainter.PainterColors.SmartCursor);
			}
			else
			{
				smartCursorKey.SetClear();
			}
			if (this._player.controlTorch)
			{
				smartSelectKey.SetSolid(ChromaHotkeyPainter.PainterColors.SmartSelect);
				return;
			}
			smartSelectKey.SetClear();
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x0054C0F4 File Offset: 0x0054A2F4
		private void Step_Movement()
		{
			List<ChromaHotkeyPainter.PaintKey> wasdKeys = this._wasdKeys;
			bool flag = this._player.frozen || this._player.tongued || this._player.webbed || this._player.stoned;
			if (this._player.dead)
			{
				this.SetGroupClear(wasdKeys);
				return;
			}
			if (flag)
			{
				this.SetGroupColorBase(wasdKeys, ChromaHotkeyPainter.PainterColors.DangerKeyBlocked);
				return;
			}
			this.SetGroupColorBase(wasdKeys, ChromaHotkeyPainter.PainterColors.MovementKeys);
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x0054C170 File Offset: 0x0054A370
		private void Step_Mount()
		{
			ChromaHotkeyPainter.PaintKey mountKey = this._mountKey;
			if (this._player.QuickMount_GetItemToUse() == null || this._player.dead)
			{
				mountKey.SetClear();
				return;
			}
			if (this._player.frozen || this._player.tongued || this._player.webbed || this._player.stoned || this._player.gravDir == -1f || this._player.noItems)
			{
				mountKey.SetSolid(ChromaHotkeyPainter.PainterColors.DangerKeyBlocked);
				if (this._player.gravDir == -1f)
				{
					mountKey.SetSolid(ChromaHotkeyPainter.PainterColors.DangerKeyBlocked * 0.6f);
				}
				return;
			}
			mountKey.SetSolid(ChromaHotkeyPainter.PainterColors.QuickMount);
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x0054C238 File Offset: 0x0054A438
		private void Step_Grapple()
		{
			ChromaHotkeyPainter.PaintKey grappleKey = this._grappleKey;
			if (this._player.QuickGrapple_GetItemToUse() == null || this._player.dead)
			{
				grappleKey.SetClear();
				return;
			}
			if (this._player.frozen || this._player.tongued || this._player.webbed || this._player.stoned || this._player.noItems)
			{
				grappleKey.SetSolid(ChromaHotkeyPainter.PainterColors.DangerKeyBlocked);
				return;
			}
			grappleKey.SetSolid(ChromaHotkeyPainter.PainterColors.QuickGrapple);
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x0054C2C8 File Offset: 0x0054A4C8
		private void Step_Jump()
		{
			ChromaHotkeyPainter.PaintKey jumpKey = this._jumpKey;
			if (this._player.dead)
			{
				jumpKey.SetClear();
				return;
			}
			if (this._player.frozen || this._player.tongued || this._player.webbed || this._player.stoned)
			{
				jumpKey.SetSolid(ChromaHotkeyPainter.PainterColors.DangerKeyBlocked);
				return;
			}
			jumpKey.SetSolid(ChromaHotkeyPainter.PainterColors.MovementKeys);
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x0054C33C File Offset: 0x0054A53C
		private void Step_QuickHeal()
		{
			ChromaHotkeyPainter.PaintKey healKey = this._healKey;
			if (this._player.QuickHeal_GetItemToUse() == null || this._player.dead)
			{
				healKey.SetClear();
				this._quickHealAlert = 0;
				return;
			}
			if (this._player.potionDelay > 0)
			{
				float lerpValue = Utils.GetLerpValue((float)this._player.potionDelayTime, 0f, (float)this._player.potionDelay, true);
				Color solid = Color.Lerp(ChromaHotkeyPainter.PainterColors.DangerKeyBlocked, ChromaHotkeyPainter.PainterColors.QuickHealCooldown, lerpValue) * lerpValue * lerpValue * lerpValue;
				healKey.SetSolid(solid);
				this._quickHealAlert = 0;
				return;
			}
			if (this._player.statLife == this._player.statLifeMax2)
			{
				healKey.SetClear();
				this._quickHealAlert = 0;
				return;
			}
			if ((float)this._player.statLife <= (float)this._player.statLifeMax2 / 4f)
			{
				if (this._quickHealAlert != 1)
				{
					this._quickHealAlert = 1;
					healKey.SetAlert(Color.Black, ChromaHotkeyPainter.PainterColors.QuickHealReadyUrgent, -1f, 2f);
				}
				return;
			}
			if ((float)this._player.statLife <= (float)this._player.statLifeMax2 / 2f)
			{
				if (this._quickHealAlert != 2)
				{
					this._quickHealAlert = 2;
					healKey.SetAlert(Color.Black, ChromaHotkeyPainter.PainterColors.QuickHealReadyUrgent, -1f, 2f);
				}
				return;
			}
			healKey.SetSolid(ChromaHotkeyPainter.PainterColors.QuickHealReady);
			this._quickHealAlert = 0;
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x0054C4AC File Offset: 0x0054A6AC
		private void Step_QuickMana()
		{
			ChromaHotkeyPainter.PaintKey manaKey = this._manaKey;
			if (this._player.QuickMana_GetItemToUse() == null || this._player.dead || this._player.statMana == this._player.statManaMax2)
			{
				manaKey.SetClear();
				return;
			}
			manaKey.SetSolid(ChromaHotkeyPainter.PainterColors.QuickMana);
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x0054C504 File Offset: 0x0054A704
		private void Step_Throw()
		{
			ChromaHotkeyPainter.PaintKey throwKey = this._throwKey;
			Item heldItem = this._player.HeldItem;
			if (this._player.dead || this._player.HeldItem.favorited || this._player.noThrow > 0)
			{
				throwKey.SetClear();
				return;
			}
			if (this._player.frozen || this._player.tongued || this._player.webbed || this._player.stoned || this._player.noItems)
			{
				throwKey.SetClear();
				return;
			}
			throwKey.SetSolid(ChromaHotkeyPainter.PainterColors.Throw);
		}

		// Token: 0x04004DD4 RID: 19924
		private readonly Dictionary<string, ChromaHotkeyPainter.PaintKey> _keys = new Dictionary<string, ChromaHotkeyPainter.PaintKey>();

		// Token: 0x04004DD5 RID: 19925
		private readonly List<ChromaHotkeyPainter.ReactiveRGBKey> _reactiveKeys = new List<ChromaHotkeyPainter.ReactiveRGBKey>();

		// Token: 0x04004DD6 RID: 19926
		private List<Keys> _xnaKeysInUse = new List<Keys>();

		// Token: 0x04004DD7 RID: 19927
		private Player _player;

		// Token: 0x04004DD8 RID: 19928
		private int _quickHealAlert;

		// Token: 0x04004DD9 RID: 19929
		private List<ChromaHotkeyPainter.PaintKey> _wasdKeys = new List<ChromaHotkeyPainter.PaintKey>();

		// Token: 0x04004DDA RID: 19930
		private ChromaHotkeyPainter.PaintKey _healKey;

		// Token: 0x04004DDB RID: 19931
		private ChromaHotkeyPainter.PaintKey _mountKey;

		// Token: 0x04004DDC RID: 19932
		private ChromaHotkeyPainter.PaintKey _jumpKey;

		// Token: 0x04004DDD RID: 19933
		private ChromaHotkeyPainter.PaintKey _grappleKey;

		// Token: 0x04004DDE RID: 19934
		private ChromaHotkeyPainter.PaintKey _throwKey;

		// Token: 0x04004DDF RID: 19935
		private ChromaHotkeyPainter.PaintKey _manaKey;

		// Token: 0x04004DE0 RID: 19936
		private ChromaHotkeyPainter.PaintKey _buffKey;

		// Token: 0x04004DE1 RID: 19937
		private ChromaHotkeyPainter.PaintKey _smartCursorKey;

		// Token: 0x04004DE2 RID: 19938
		private ChromaHotkeyPainter.PaintKey _smartSelectKey;

		// Token: 0x020007F8 RID: 2040
		private class ReactiveRGBKey
		{
			// Token: 0x1700053B RID: 1339
			// (get) Token: 0x060042A3 RID: 17059 RVA: 0x006BE2B7 File Offset: 0x006BC4B7
			public bool Expired
			{
				get
				{
					return this._expireTime < Main.gameTimeCache.TotalGameTime;
				}
			}

			// Token: 0x060042A4 RID: 17060 RVA: 0x006BE2CE File Offset: 0x006BC4CE
			public ReactiveRGBKey(Keys key, Color color, TimeSpan duration, string whatIsThisKeyFor)
			{
				this._color = color;
				this.XNAKey = key;
				this.WhatIsThisKeyFor = whatIsThisKeyFor;
				this._duration = duration;
				this._startTime = Main.gameTimeCache.TotalGameTime;
			}

			// Token: 0x060042A5 RID: 17061 RVA: 0x006BE304 File Offset: 0x006BC504
			public void Update()
			{
				float amount = (float)Utils.GetLerpValue(this._startTime.TotalSeconds, this._expireTime.TotalSeconds, Main.gameTimeCache.TotalGameTime.TotalSeconds, true);
				this._rgbKey.SetSolid(Color.Lerp(this._color, Color.Black, amount));
			}

			// Token: 0x060042A6 RID: 17062 RVA: 0x006BE35D File Offset: 0x006BC55D
			public void Clear()
			{
				this._rgbKey.Clear();
			}

			// Token: 0x060042A7 RID: 17063 RVA: 0x006BE36A File Offset: 0x006BC56A
			public void Unbind()
			{
				Main.Chroma.UnbindKey(this.XNAKey);
			}

			// Token: 0x060042A8 RID: 17064 RVA: 0x006BE37C File Offset: 0x006BC57C
			public void Bind()
			{
				this._rgbKey = Main.Chroma.BindKey(this.XNAKey, this.WhatIsThisKeyFor);
			}

			// Token: 0x060042A9 RID: 17065 RVA: 0x006BE39A File Offset: 0x006BC59A
			public void Refresh()
			{
				this._startTime = Main.gameTimeCache.TotalGameTime;
				this._expireTime = this._startTime;
				this._expireTime.Add(this._duration);
			}

			// Token: 0x0400715E RID: 29022
			public readonly Keys XNAKey;

			// Token: 0x0400715F RID: 29023
			public readonly string WhatIsThisKeyFor;

			// Token: 0x04007160 RID: 29024
			private readonly Color _color;

			// Token: 0x04007161 RID: 29025
			private readonly TimeSpan _duration;

			// Token: 0x04007162 RID: 29026
			private TimeSpan _startTime;

			// Token: 0x04007163 RID: 29027
			private TimeSpan _expireTime;

			// Token: 0x04007164 RID: 29028
			private RgbKey _rgbKey;
		}

		// Token: 0x020007F9 RID: 2041
		private class PaintKey
		{
			// Token: 0x060042AA RID: 17066 RVA: 0x006BE3CC File Offset: 0x006BC5CC
			public PaintKey(string triggerName, List<string> keys)
			{
				this._triggerName = triggerName;
				this._xnaKeys = new List<Keys>();
				using (List<string>.Enumerator enumerator = keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Keys item;
						if (Enum.TryParse<Keys>(enumerator.Current, true, out item))
						{
							this._xnaKeys.Add(item);
						}
					}
				}
				this._rgbKeys = new List<RgbKey>();
			}

			// Token: 0x060042AB RID: 17067 RVA: 0x006BE44C File Offset: 0x006BC64C
			public void Unbind()
			{
				foreach (RgbKey rgbKey in this._rgbKeys)
				{
					Main.Chroma.UnbindKey(rgbKey.Key);
				}
			}

			// Token: 0x060042AC RID: 17068 RVA: 0x006BE4A8 File Offset: 0x006BC6A8
			public void Bind()
			{
				foreach (Keys keys in this._xnaKeys)
				{
					this._rgbKeys.Add(Main.Chroma.BindKey(keys, this._triggerName));
				}
				this._rgbKeys = this._rgbKeys.Distinct<RgbKey>().ToList<RgbKey>();
			}

			// Token: 0x060042AD RID: 17069 RVA: 0x006BE528 File Offset: 0x006BC728
			public void SetSolid(Color color)
			{
				foreach (RgbKey rgbKey in this._rgbKeys)
				{
					rgbKey.SetSolid(color);
				}
			}

			// Token: 0x060042AE RID: 17070 RVA: 0x006BE57C File Offset: 0x006BC77C
			public void SetClear()
			{
				foreach (RgbKey rgbKey in this._rgbKeys)
				{
					rgbKey.Clear();
				}
			}

			// Token: 0x060042AF RID: 17071 RVA: 0x006BE5CC File Offset: 0x006BC7CC
			public bool UsesKey(Keys key)
			{
				return this._xnaKeys.Contains(key);
			}

			// Token: 0x060042B0 RID: 17072 RVA: 0x006BE5DC File Offset: 0x006BC7DC
			public void SetAlert(Color colorBase, Color colorFlash, float time, float flashesPerSecond)
			{
				if (time == -1f)
				{
					time = 10000f;
				}
				foreach (RgbKey rgbKey in this._rgbKeys)
				{
					rgbKey.SetFlashing(colorBase, colorFlash, time, flashesPerSecond);
				}
			}

			// Token: 0x060042B1 RID: 17073 RVA: 0x006BE640 File Offset: 0x006BC840
			public List<Keys> GetXNAKeysInUse()
			{
				return new List<Keys>(this._xnaKeys);
			}

			// Token: 0x04007165 RID: 29029
			private string _triggerName;

			// Token: 0x04007166 RID: 29030
			private List<Keys> _xnaKeys;

			// Token: 0x04007167 RID: 29031
			private List<RgbKey> _rgbKeys;
		}

		// Token: 0x020007FA RID: 2042
		private static class PainterColors
		{
			// Token: 0x04007168 RID: 29032
			private const float HOTKEY_COLOR_MULTIPLIER = 1f;

			// Token: 0x04007169 RID: 29033
			public static readonly Color MovementKeys = Color.Gray * 1f;

			// Token: 0x0400716A RID: 29034
			public static readonly Color QuickMount = Color.RoyalBlue * 1f;

			// Token: 0x0400716B RID: 29035
			public static readonly Color QuickGrapple = Color.Lerp(Color.RoyalBlue, Color.Blue, 0.5f) * 1f;

			// Token: 0x0400716C RID: 29036
			public static readonly Color QuickHealReady = Color.Pink * 1f;

			// Token: 0x0400716D RID: 29037
			public static readonly Color QuickHealReadyUrgent = Color.DeepPink * 1f;

			// Token: 0x0400716E RID: 29038
			public static readonly Color QuickHealCooldown = Color.HotPink * 0.5f * 1f;

			// Token: 0x0400716F RID: 29039
			public static readonly Color QuickMana = new Color(40, 0, 230) * 1f;

			// Token: 0x04007170 RID: 29040
			public static readonly Color Throw = Color.Red * 0.2f * 1f;

			// Token: 0x04007171 RID: 29041
			public static readonly Color SmartCursor = Color.Gold;

			// Token: 0x04007172 RID: 29042
			public static readonly Color SmartSelect = Color.Goldenrod;

			// Token: 0x04007173 RID: 29043
			public static readonly Color DangerKeyBlocked = Color.Red * 1f;
		}
	}
}
