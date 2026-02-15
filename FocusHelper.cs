using System;
using System.Windows.Forms;
using ReLogic.OS;

namespace Terraria
{
	// Token: 0x02000017 RID: 23
	public static class FocusHelper
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000C238 File Offset: 0x0000A438
		public static bool GameplayActive
		{
			get
			{
				return FocusHelper.IsSelectedApplication && !Main.gamePaused;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x0000C24B File Offset: 0x0000A44B
		public static bool AllowAudioUnfocused
		{
			get
			{
				return Main.SettingPlayWhenUnfocused;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x0000C252 File Offset: 0x0000A452
		public static bool AllowSkyMovement
		{
			get
			{
				return FocusHelper.GameplayActive;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000C252 File Offset: 0x0000A452
		public static bool AllowTileDrawingToEmitEffects
		{
			get
			{
				return FocusHelper.GameplayActive;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x0000C252 File Offset: 0x0000A452
		public static bool AllowPlayerToEmitEffects
		{
			get
			{
				return FocusHelper.GameplayActive;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000BA RID: 186 RVA: 0x0000C252 File Offset: 0x0000A452
		public static bool AllowWorldItemsToEmitEffects
		{
			get
			{
				return FocusHelper.GameplayActive;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000BB RID: 187 RVA: 0x0000C259 File Offset: 0x0000A459
		public static bool PauseSkies
		{
			get
			{
				return !FocusHelper.GameplayActive;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000BC RID: 188 RVA: 0x0000C252 File Offset: 0x0000A452
		public static bool UpdateVisualEffects
		{
			get
			{
				return FocusHelper.GameplayActive;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000BD RID: 189 RVA: 0x0000C259 File Offset: 0x0000A459
		public static bool PauseLiquidRenderer
		{
			get
			{
				return !FocusHelper.GameplayActive;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000BE RID: 190 RVA: 0x0000C252 File Offset: 0x0000A452
		public static bool AllowMiscDustEffects
		{
			get
			{
				return FocusHelper.GameplayActive;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000BF RID: 191 RVA: 0x0000C263 File Offset: 0x0000A463
		public static bool PausePlayerBalloonAnimations
		{
			get
			{
				return !FocusHelper.IsSelectedApplication || (Main.ingameOptionsWindow && Main.autoPause);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000C252 File Offset: 0x0000A452
		public static bool AllowRain
		{
			get
			{
				return FocusHelper.GameplayActive;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x0000C27C File Offset: 0x0000A47C
		public static bool AllowCountingPlayerTime
		{
			get
			{
				bool flag = Main.gamePaused && !FocusHelper.IsSelectedApplication;
				bool result = Main.instance.IsActive && !flag;
				if (Main.gameMenu)
				{
					result = false;
				}
				return result;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x0000C2BA File Offset: 0x0000A4BA
		public static bool UpdateBackgroundThunder
		{
			get
			{
				return FocusHelper.IsSelectedApplication || Main.SettingPlayWhenUnfocused;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000C2CA File Offset: 0x0000A4CA
		public static bool AllowMusic
		{
			get
			{
				return FocusHelper.IsSelectedApplication || FocusHelper.AllowAudioUnfocused;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000C2DA File Offset: 0x0000A4DA
		public static bool PauseSounds
		{
			get
			{
				return !FocusHelper.GameplayActive && Main.netMode == 0;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x0000C2ED File Offset: 0x0000A4ED
		public static bool QuietAmbientSounds
		{
			get
			{
				return !FocusHelper.IsSelectedApplication;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x0000C2F7 File Offset: 0x0000A4F7
		public static bool AllowInputProcessing
		{
			get
			{
				return FocusHelper.IsSelectedApplication;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000C2FE File Offset: 0x0000A4FE
		public static bool AllowInputProcessingForGamepad
		{
			get
			{
				return FocusHelper.IsSelectedApplication || Main.AllowUnfocusedInputOnGamepad;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x0000C2F7 File Offset: 0x0000A4F7
		public static bool AllowUIInputs
		{
			get
			{
				return FocusHelper.IsSelectedApplication;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x0000C2F7 File Offset: 0x0000A4F7
		public static bool AllowGameplayInputs
		{
			get
			{
				return FocusHelper.IsSelectedApplication;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000CA RID: 202 RVA: 0x0000C2F7 File Offset: 0x0000A4F7
		public static bool LetStarsFallInMenu
		{
			get
			{
				return FocusHelper.IsSelectedApplication;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000CB RID: 203 RVA: 0x0000C2F7 File Offset: 0x0000A4F7
		public static bool AllowDontStarveDarknessDamage
		{
			get
			{
				return FocusHelper.IsSelectedApplication;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000CC RID: 204 RVA: 0x0000C2F7 File Offset: 0x0000A4F7
		public static bool AllowChroma
		{
			get
			{
				return FocusHelper.IsSelectedApplication;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000CD RID: 205 RVA: 0x0000C2ED File Offset: 0x0000A4ED
		public static bool AllowTaskbarFlash
		{
			get
			{
				return !FocusHelper.IsSelectedApplication;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000C310 File Offset: 0x0000A510
		public static void UpdateFocus(out bool wantsToPause)
		{
			wantsToPause = false;
			bool flag = !Main.SettingPlayWhenUnfocused;
			FocusHelper.IsSelectedApplication = Main.instance.IsActive;
			if (Platform.IsWindows)
			{
				Form form = Control.FromHandle(Main.instance.Window.Handle) as Form;
				bool flag2 = form.WindowState == FormWindowState.Minimized;
				bool flag3 = Form.ActiveForm == form;
				FocusHelper.IsSelectedApplication = (FocusHelper.IsSelectedApplication || flag3);
				if (flag2)
				{
					FocusHelper.IsSelectedApplication = false;
				}
			}
			if (!FocusHelper.IsSelectedApplication && Main.netMode == 0 && flag)
			{
				if (!Platform.IsOSX)
				{
					Main.instance.IsMouseVisible = true;
				}
				wantsToPause = true;
				return;
			}
			if (!Platform.IsOSX)
			{
				Main.instance.IsMouseVisible = false;
			}
			if (Platform.IsWindows && Main.instance.ReHideCursor)
			{
				Main.instance.IsMouseVisible = false;
				Main.instance.ReHideCursor = false;
				IMouseNotifier mouseNotifier = Platform.Get<IMouseNotifier>();
				if (mouseNotifier != null)
				{
					mouseNotifier.ForceCursorHidden();
				}
			}
		}

		// Token: 0x0400005B RID: 91
		public static bool IsSelectedApplication;
	}
}
