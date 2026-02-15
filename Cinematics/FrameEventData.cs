using System;

namespace Terraria.Cinematics
{
	// Token: 0x020005AE RID: 1454
	public struct FrameEventData
	{
		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06003989 RID: 14729 RVA: 0x006523BD File Offset: 0x006505BD
		public int AbsoluteFrame
		{
			get
			{
				return this._absoluteFrame;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x0600398A RID: 14730 RVA: 0x006523C5 File Offset: 0x006505C5
		public int Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x0600398B RID: 14731 RVA: 0x006523CD File Offset: 0x006505CD
		public int Duration
		{
			get
			{
				return this._duration;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x0600398C RID: 14732 RVA: 0x006523D5 File Offset: 0x006505D5
		public int Frame
		{
			get
			{
				return this._absoluteFrame - this._start;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x0600398D RID: 14733 RVA: 0x006523E4 File Offset: 0x006505E4
		public bool IsFirstFrame
		{
			get
			{
				return this._start == this._absoluteFrame;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x0600398E RID: 14734 RVA: 0x006523F4 File Offset: 0x006505F4
		public bool IsLastFrame
		{
			get
			{
				return this.Remaining == 0;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x0600398F RID: 14735 RVA: 0x006523FF File Offset: 0x006505FF
		public int Remaining
		{
			get
			{
				return this._start + this._duration - this._absoluteFrame - 1;
			}
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x00652417 File Offset: 0x00650617
		public FrameEventData(int absoluteFrame, int start, int duration)
		{
			this._absoluteFrame = absoluteFrame;
			this._start = start;
			this._duration = duration;
		}

		// Token: 0x04005D7A RID: 23930
		private int _absoluteFrame;

		// Token: 0x04005D7B RID: 23931
		private int _start;

		// Token: 0x04005D7C RID: 23932
		private int _duration;
	}
}
