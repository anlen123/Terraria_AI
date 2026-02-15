using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Utilities;

namespace Terraria.Audio
{
	// Token: 0x020005DC RID: 1500
	public static class SoundEngine
	{
		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06003AD4 RID: 15060 RVA: 0x00658527 File Offset: 0x00656727
		// (set) Token: 0x06003AD5 RID: 15061 RVA: 0x0065852E File Offset: 0x0065672E
		public static bool IsAudioSupported { get; private set; }

		// Token: 0x06003AD6 RID: 15062 RVA: 0x00658538 File Offset: 0x00656738
		public static IAudioSystem Initialize()
		{
			SoundEngine.IsAudioSupported = SoundEngine.TestAudioSupport();
			try
			{
				if (SoundEngine.IsAudioSupported)
				{
					return new LegacyAudioSystem();
				}
			}
			catch (Exception)
			{
				SoundEngine.IsAudioSupported = false;
			}
			return new DisabledAudioSystem();
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x00658580 File Offset: 0x00656780
		public static void Load(IServiceProvider services)
		{
			if (!SoundEngine.IsAudioSupported)
			{
				return;
			}
			SoundEngine.LegacySoundPlayer = new LegacySoundPlayer(services);
			SoundEngine.SoundPlayer = new SoundPlayer();
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x006585A0 File Offset: 0x006567A0
		public static void Update()
		{
			if (!SoundEngine.IsAudioSupported)
			{
				return;
			}
			if (Main.audioSystem != null)
			{
				Main.audioSystem.UpdateAudioEngine();
			}
			SoundInstanceGarbageCollector.Update();
			bool pauseSounds = FocusHelper.PauseSounds;
			if (!SoundEngine.AreSoundsPaused && pauseSounds)
			{
				SoundEngine.SoundPlayer.PauseAll();
			}
			else if (SoundEngine.AreSoundsPaused && !pauseSounds)
			{
				SoundEngine.SoundPlayer.ResumeAll();
			}
			SoundEngine.AreSoundsPaused = pauseSounds;
			SoundEngine.SoundPlayer.Update();
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x0065860D File Offset: 0x0065680D
		public static void Reload()
		{
			if (!SoundEngine.IsAudioSupported)
			{
				return;
			}
			if (SoundEngine.LegacySoundPlayer != null)
			{
				SoundEngine.LegacySoundPlayer.Reload();
			}
			if (SoundEngine.SoundPlayer != null)
			{
				SoundEngine.SoundPlayer.Reload();
			}
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x00658639 File Offset: 0x00656839
		public static void PlaySound(int type, Vector2 position, int style = 1, float pitchOffset = 0f)
		{
			SoundEngine.PlaySound(type, (int)position.X, (int)position.Y, style, 1f, pitchOffset);
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x00658657 File Offset: 0x00656857
		public static SoundEffectInstance PlaySound(LegacySoundStyle type, Vector2 position, float pitchOffset = 0f, float volumeScale = 1f)
		{
			return SoundEngine.PlaySound(type, (int)position.X, (int)position.Y, pitchOffset, volumeScale);
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x0065866F File Offset: 0x0065686F
		public static SoundEffectInstance PlaySound(LegacySoundStyle type, int x = -1, int y = -1, float pitchOffset = 0f, float volumeScale = 1f)
		{
			if (type == null)
			{
				return null;
			}
			return SoundEngine.PlaySound(type.SoundId, x, y, type.Style, type.Volume * volumeScale, pitchOffset + type.GetRandomPitch());
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x0065869A File Offset: 0x0065689A
		public static SoundEffectInstance PlaySound(int type, int x = -1, int y = -1, int Style = 1, float volumeScale = 1f, float pitchOffset = 0f)
		{
			if (Main.dedServ || !SoundEngine.IsAudioSupported)
			{
				return null;
			}
			return SoundEngine.LegacySoundPlayer.PlaySound(type, x, y, Style, volumeScale, pitchOffset);
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x006586BE File Offset: 0x006568BE
		public static ActiveSound GetActiveSound(SlotId id)
		{
			if (Main.dedServ || !SoundEngine.IsAudioSupported)
			{
				return null;
			}
			return SoundEngine.SoundPlayer.GetActiveSound(id);
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x006586DC File Offset: 0x006568DC
		public static SlotId PlayTrackedSound(SoundStyle style, Vector2 position, SoundPlayOverrides overrides = default(SoundPlayOverrides))
		{
			if (Main.dedServ || !SoundEngine.IsAudioSupported)
			{
				return SlotId.Invalid;
			}
			if (style.MaxTrackedInstances > 0 && SoundEngine.SoundPlayer.GetActiveSoundCount(style) >= style.MaxTrackedInstances)
			{
				return SlotId.Invalid;
			}
			return SoundEngine.SoundPlayer.Play(style, position, overrides);
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x0065872C File Offset: 0x0065692C
		public static SlotId PlayTrackedLoopedSound(SoundStyle style, Vector2 position, ActiveSound.LoopedPlayCondition loopingCondition = null, SoundPlayOverrides overrides = default(SoundPlayOverrides))
		{
			if (Main.dedServ || !SoundEngine.IsAudioSupported)
			{
				return SlotId.Invalid;
			}
			return SoundEngine.SoundPlayer.PlayLooped(style, position, loopingCondition, overrides);
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x00658750 File Offset: 0x00656950
		public static SlotId PlayTrackedSound(SoundStyle style)
		{
			if (Main.dedServ || !SoundEngine.IsAudioSupported)
			{
				return SlotId.Invalid;
			}
			return SoundEngine.SoundPlayer.Play(style);
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x00658771 File Offset: 0x00656971
		public static void StopTrackedSounds()
		{
			if (!Main.dedServ && SoundEngine.IsAudioSupported)
			{
				SoundEngine.SoundPlayer.StopAll();
			}
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x0065878B File Offset: 0x0065698B
		public static SoundEffect GetTrackableSoundByStyleId(int id)
		{
			if (Main.dedServ || !SoundEngine.IsAudioSupported)
			{
				return null;
			}
			return SoundEngine.LegacySoundPlayer.GetTrackableSoundByStyleId(id);
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x006587A8 File Offset: 0x006569A8
		public static void StopAmbientSounds()
		{
			if (Main.dedServ || !SoundEngine.IsAudioSupported)
			{
				return;
			}
			if (SoundEngine.LegacySoundPlayer != null)
			{
				SoundEngine.LegacySoundPlayer.StopAmbientSounds();
			}
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x006587CA File Offset: 0x006569CA
		public static ActiveSound FindActiveSound(SoundStyle style)
		{
			if (Main.dedServ || !SoundEngine.IsAudioSupported)
			{
				return null;
			}
			return SoundEngine.SoundPlayer.FindActiveSound(style);
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x006587E8 File Offset: 0x006569E8
		private static bool TestAudioSupport()
		{
			byte[] buffer = new byte[]
			{
				82,
				73,
				70,
				70,
				158,
				0,
				0,
				0,
				87,
				65,
				86,
				69,
				102,
				109,
				116,
				32,
				16,
				0,
				0,
				0,
				1,
				0,
				1,
				0,
				68,
				172,
				0,
				0,
				136,
				88,
				1,
				0,
				2,
				0,
				16,
				0,
				76,
				73,
				83,
				84,
				26,
				0,
				0,
				0,
				73,
				78,
				70,
				79,
				73,
				83,
				70,
				84,
				14,
				0,
				0,
				0,
				76,
				97,
				118,
				102,
				53,
				54,
				46,
				52,
				48,
				46,
				49,
				48,
				49,
				0,
				100,
				97,
				116,
				97,
				88,
				0,
				0,
				0,
				0,
				0,
				126,
				4,
				240,
				8,
				64,
				13,
				95,
				17,
				67,
				21,
				217,
				24,
				23,
				28,
				240,
				30,
				94,
				33,
				84,
				35,
				208,
				36,
				204,
				37,
				71,
				38,
				64,
				38,
				183,
				37,
				180,
				36,
				58,
				35,
				79,
				33,
				1,
				31,
				86,
				28,
				92,
				25,
				37,
				22,
				185,
				18,
				42,
				15,
				134,
				11,
				222,
				7,
				68,
				4,
				196,
				0,
				112,
				253,
				86,
				250,
				132,
				247,
				6,
				245,
				230,
				242,
				47,
				241,
				232,
				239,
				25,
				239,
				194,
				238,
				231,
				238,
				139,
				239,
				169,
				240,
				61,
				242,
				67,
				244,
				180,
				246
			};
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(buffer))
				{
					SoundEffect.FromStream(memoryStream);
				}
			}
			catch (NoAudioHardwareException)
			{
				Console.WriteLine("No audio hardware found. Disabling all audio.");
				return false;
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x04005E20 RID: 24096
		public static LegacySoundPlayer LegacySoundPlayer;

		// Token: 0x04005E21 RID: 24097
		public static SoundPlayer SoundPlayer;

		// Token: 0x04005E22 RID: 24098
		public static bool AreSoundsPaused;
	}
}
