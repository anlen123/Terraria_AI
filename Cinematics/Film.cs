using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Cinematics
{
	// Token: 0x020005B0 RID: 1456
	public class Film
	{
		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06003995 RID: 14741 RVA: 0x0065242E File Offset: 0x0065062E
		public int Frame
		{
			get
			{
				return this._frame;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06003996 RID: 14742 RVA: 0x00652436 File Offset: 0x00650636
		public int FrameCount
		{
			get
			{
				return this._frameCount;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06003997 RID: 14743 RVA: 0x0065243E File Offset: 0x0065063E
		public int AppendPoint
		{
			get
			{
				return this._nextSequenceAppendTime;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06003998 RID: 14744 RVA: 0x00652446 File Offset: 0x00650646
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x0065244E File Offset: 0x0065064E
		public void AddSequence(int start, int duration, FrameEvent frameEvent)
		{
			this._sequences.Add(new Film.Sequence(frameEvent, start, duration));
			this._nextSequenceAppendTime = Math.Max(this._nextSequenceAppendTime, start + duration);
			this._frameCount = Math.Max(this._frameCount, start + duration);
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x0065248B File Offset: 0x0065068B
		public void AppendSequence(int duration, FrameEvent frameEvent)
		{
			this.AddSequence(this._nextSequenceAppendTime, duration, frameEvent);
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x0065249C File Offset: 0x0065069C
		public void AddSequences(int start, int duration, params FrameEvent[] frameEvents)
		{
			foreach (FrameEvent frameEvent in frameEvents)
			{
				this.AddSequence(start, duration, frameEvent);
			}
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x006524C8 File Offset: 0x006506C8
		public void AppendSequences(int duration, params FrameEvent[] frameEvents)
		{
			int nextSequenceAppendTime = this._nextSequenceAppendTime;
			foreach (FrameEvent frameEvent in frameEvents)
			{
				this._sequences.Add(new Film.Sequence(frameEvent, nextSequenceAppendTime, duration));
				this._nextSequenceAppendTime = Math.Max(this._nextSequenceAppendTime, nextSequenceAppendTime + duration);
				this._frameCount = Math.Max(this._frameCount, nextSequenceAppendTime + duration);
			}
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x0065252B File Offset: 0x0065072B
		public void AppendEmptySequence(int duration)
		{
			this.AddSequence(this._nextSequenceAppendTime, duration, new FrameEvent(Film.EmptyFrameEvent));
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x00652546 File Offset: 0x00650746
		public void AppendKeyFrame(FrameEvent frameEvent)
		{
			this.AddKeyFrame(this._nextSequenceAppendTime, frameEvent);
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x00652558 File Offset: 0x00650758
		public void AppendKeyFrames(params FrameEvent[] frameEvents)
		{
			int nextSequenceAppendTime = this._nextSequenceAppendTime;
			foreach (FrameEvent frameEvent in frameEvents)
			{
				this._sequences.Add(new Film.Sequence(frameEvent, nextSequenceAppendTime, 1));
			}
			this._frameCount = Math.Max(this._frameCount, nextSequenceAppendTime + 1);
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x006525A7 File Offset: 0x006507A7
		public void AddKeyFrame(int frame, FrameEvent frameEvent)
		{
			this._sequences.Add(new Film.Sequence(frameEvent, frame, 1));
			this._frameCount = Math.Max(this._frameCount, frame + 1);
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x006525D0 File Offset: 0x006507D0
		public void AddKeyFrames(int frame, params FrameEvent[] frameEvents)
		{
			foreach (FrameEvent frameEvent in frameEvents)
			{
				this.AddKeyFrame(frame, frameEvent);
			}
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x006525FC File Offset: 0x006507FC
		public bool OnUpdate(GameTime gameTime)
		{
			if (this._sequences.Count == 0)
			{
				return false;
			}
			foreach (Film.Sequence sequence in this._sequences)
			{
				int num = this._frame - sequence.Start;
				if (num >= 0 && num < sequence.Duration)
				{
					sequence.Event(new FrameEventData(this._frame, sequence.Start, sequence.Duration));
				}
			}
			int num2 = this._frame + 1;
			this._frame = num2;
			return num2 != this._frameCount;
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x006526B0 File Offset: 0x006508B0
		public virtual void OnBegin()
		{
			this._isActive = true;
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x006526B9 File Offset: 0x006508B9
		public virtual void OnEnd()
		{
			this._isActive = false;
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x00009E06 File Offset: 0x00008006
		private static void EmptyFrameEvent(FrameEventData evt)
		{
		}

		// Token: 0x04005D7D RID: 23933
		private int _frame;

		// Token: 0x04005D7E RID: 23934
		private int _frameCount;

		// Token: 0x04005D7F RID: 23935
		private int _nextSequenceAppendTime;

		// Token: 0x04005D80 RID: 23936
		private bool _isActive;

		// Token: 0x04005D81 RID: 23937
		private List<Film.Sequence> _sequences = new List<Film.Sequence>();

		// Token: 0x020009C1 RID: 2497
		private class Sequence
		{
			// Token: 0x170005A1 RID: 1441
			// (get) Token: 0x06004A31 RID: 18993 RVA: 0x006D2919 File Offset: 0x006D0B19
			public FrameEvent Event
			{
				get
				{
					return this._frameEvent;
				}
			}

			// Token: 0x170005A2 RID: 1442
			// (get) Token: 0x06004A32 RID: 18994 RVA: 0x006D2921 File Offset: 0x006D0B21
			public int Duration
			{
				get
				{
					return this._duration;
				}
			}

			// Token: 0x170005A3 RID: 1443
			// (get) Token: 0x06004A33 RID: 18995 RVA: 0x006D2929 File Offset: 0x006D0B29
			public int Start
			{
				get
				{
					return this._start;
				}
			}

			// Token: 0x06004A34 RID: 18996 RVA: 0x006D2931 File Offset: 0x006D0B31
			public Sequence(FrameEvent frameEvent, int start, int duration)
			{
				this._frameEvent = frameEvent;
				this._start = start;
				this._duration = duration;
			}

			// Token: 0x040076A6 RID: 30374
			private FrameEvent _frameEvent;

			// Token: 0x040076A7 RID: 30375
			private int _duration;

			// Token: 0x040076A8 RID: 30376
			private int _start;
		}
	}
}
