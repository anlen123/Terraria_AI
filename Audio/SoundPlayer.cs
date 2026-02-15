using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;

namespace Terraria.Audio
{
	// Token: 0x020005D8 RID: 1496
	public class SoundPlayer
	{
		// Token: 0x06003AB9 RID: 15033 RVA: 0x00657F94 File Offset: 0x00656194
		public SlotId Play(SoundStyle style, Vector2 position, SoundPlayOverrides overrides = default(SoundPlayOverrides))
		{
			if (Main.dedServ || style == null || !style.IsTrackable)
			{
				return SlotId.Invalid;
			}
			if (Vector2.DistanceSquared(Main.screenPosition + new Vector2((float)(Main.screenWidth / 2), (float)(Main.screenHeight / 2)), position) > 100000000f)
			{
				return SlotId.Invalid;
			}
			ActiveSound activeSound = new ActiveSound(style, position, overrides);
			return this._trackedSounds.Add(activeSound);
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x00658000 File Offset: 0x00656200
		public SlotId PlayLooped(SoundStyle style, Vector2 position, ActiveSound.LoopedPlayCondition loopingCondition, SoundPlayOverrides overrides = default(SoundPlayOverrides))
		{
			if (Main.dedServ || style == null || !style.IsTrackable)
			{
				return SlotId.Invalid;
			}
			if (Vector2.DistanceSquared(Main.screenPosition + new Vector2((float)(Main.screenWidth / 2), (float)(Main.screenHeight / 2)), position) > 100000000f)
			{
				return SlotId.Invalid;
			}
			ActiveSound activeSound = new ActiveSound(style, position, loopingCondition, overrides);
			return this._trackedSounds.Add(activeSound);
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x0065806E File Offset: 0x0065626E
		public void Reload()
		{
			this.StopAll();
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x00658078 File Offset: 0x00656278
		public SlotId Play(SoundStyle style)
		{
			if (Main.dedServ || style == null || !style.IsTrackable)
			{
				return SlotId.Invalid;
			}
			ActiveSound activeSound = new ActiveSound(style);
			return this._trackedSounds.Add(activeSound);
		}

		// Token: 0x06003ABD RID: 15037 RVA: 0x006580B0 File Offset: 0x006562B0
		public ActiveSound GetActiveSound(SlotId id)
		{
			if (!this._trackedSounds.Has(id))
			{
				return null;
			}
			return this._trackedSounds[id];
		}

		// Token: 0x06003ABE RID: 15038 RVA: 0x006580D0 File Offset: 0x006562D0
		public void PauseAll()
		{
			foreach (SlotVector<ActiveSound>.ItemPair itemPair in this._trackedSounds)
			{
				itemPair.Value.Pause();
			}
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x00658120 File Offset: 0x00656320
		public void ResumeAll()
		{
			foreach (SlotVector<ActiveSound>.ItemPair itemPair in this._trackedSounds)
			{
				itemPair.Value.Resume();
			}
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x00658170 File Offset: 0x00656370
		public void StopAll()
		{
			foreach (SlotVector<ActiveSound>.ItemPair itemPair in this._trackedSounds)
			{
				itemPair.Value.Stop();
			}
			this._trackedSounds.Clear();
		}

		// Token: 0x06003AC1 RID: 15041 RVA: 0x006581CC File Offset: 0x006563CC
		public void Update()
		{
			foreach (SlotVector<ActiveSound>.ItemPair itemPair in this._trackedSounds)
			{
				try
				{
					itemPair.Value.Update();
					if (!itemPair.Value.IsPlaying)
					{
						this._trackedSounds.Remove(itemPair.Id);
					}
				}
				catch
				{
					this._trackedSounds.Remove(itemPair.Id);
				}
			}
		}

		// Token: 0x06003AC2 RID: 15042 RVA: 0x00658260 File Offset: 0x00656460
		public int GetActiveSoundCount(SoundStyle style)
		{
			int num = 0;
			foreach (SlotVector<ActiveSound>.ItemPair itemPair in this._trackedSounds)
			{
				ActiveSound value = itemPair.Value;
				if (value.Style == style && value.IsPlaying)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x006582C4 File Offset: 0x006564C4
		public ActiveSound FindActiveSound(SoundStyle style)
		{
			foreach (SlotVector<ActiveSound>.ItemPair itemPair in this._trackedSounds)
			{
				if (itemPair.Value.Style == style)
				{
					return itemPair.Value;
				}
			}
			return null;
		}

		// Token: 0x04005E13 RID: 24083
		private readonly SlotVector<ActiveSound> _trackedSounds = new SlotVector<ActiveSound>(4096);
	}
}
