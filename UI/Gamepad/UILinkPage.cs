using System;
using System.Collections.Generic;

namespace Terraria.UI.Gamepad
{
	// Token: 0x02000105 RID: 261
	public class UILinkPage
	{
		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06001A2D RID: 6701 RVA: 0x004F4934 File Offset: 0x004F2B34
		// (remove) Token: 0x06001A2E RID: 6702 RVA: 0x004F496C File Offset: 0x004F2B6C
		public event Action<int, int> ReachEndEvent;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06001A2F RID: 6703 RVA: 0x004F49A4 File Offset: 0x004F2BA4
		// (remove) Token: 0x06001A30 RID: 6704 RVA: 0x004F49DC File Offset: 0x004F2BDC
		public event Action TravelEvent;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06001A31 RID: 6705 RVA: 0x004F4A14 File Offset: 0x004F2C14
		// (remove) Token: 0x06001A32 RID: 6706 RVA: 0x004F4A4C File Offset: 0x004F2C4C
		public event Action LeaveEvent;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06001A33 RID: 6707 RVA: 0x004F4A84 File Offset: 0x004F2C84
		// (remove) Token: 0x06001A34 RID: 6708 RVA: 0x004F4ABC File Offset: 0x004F2CBC
		public event Action EnterEvent;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06001A35 RID: 6709 RVA: 0x004F4AF4 File Offset: 0x004F2CF4
		// (remove) Token: 0x06001A36 RID: 6710 RVA: 0x004F4B2C File Offset: 0x004F2D2C
		public event Action UpdateEvent;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06001A37 RID: 6711 RVA: 0x004F4B64 File Offset: 0x004F2D64
		// (remove) Token: 0x06001A38 RID: 6712 RVA: 0x004F4B9C File Offset: 0x004F2D9C
		public event Func<bool> IsValidEvent;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06001A39 RID: 6713 RVA: 0x004F4BD4 File Offset: 0x004F2DD4
		// (remove) Token: 0x06001A3A RID: 6714 RVA: 0x004F4C0C File Offset: 0x004F2E0C
		public event Func<bool> CanEnterEvent;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06001A3B RID: 6715 RVA: 0x004F4C44 File Offset: 0x004F2E44
		// (remove) Token: 0x06001A3C RID: 6716 RVA: 0x004F4C7C File Offset: 0x004F2E7C
		public event Action<int> OnPageMoveAttempt;

		// Token: 0x06001A3D RID: 6717 RVA: 0x004F4CB1 File Offset: 0x004F2EB1
		public UILinkPage()
		{
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x004F4CD2 File Offset: 0x004F2ED2
		public UILinkPage(int id)
		{
			this.ID = id;
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x004F4CFA File Offset: 0x004F2EFA
		public void Update()
		{
			if (this.UpdateEvent != null)
			{
				this.UpdateEvent();
			}
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x004F4D0F File Offset: 0x004F2F0F
		public void Leave()
		{
			if (this.LeaveEvent != null)
			{
				this.LeaveEvent();
			}
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x004F4D24 File Offset: 0x004F2F24
		public void Enter()
		{
			if (this.EnterEvent != null)
			{
				this.EnterEvent();
			}
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x004F4D39 File Offset: 0x004F2F39
		public bool IsValid()
		{
			return this.IsValidEvent == null || this.IsValidEvent();
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x004F4D50 File Offset: 0x004F2F50
		public bool CanEnter()
		{
			return this.CanEnterEvent == null || this.CanEnterEvent();
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x004F4D67 File Offset: 0x004F2F67
		public void TravelUp()
		{
			this.Travel(this.LinkMap[this.CurrentPoint].Up);
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x004F4D85 File Offset: 0x004F2F85
		public void TravelDown()
		{
			this.Travel(this.LinkMap[this.CurrentPoint].Down);
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x004F4DA3 File Offset: 0x004F2FA3
		public void TravelLeft()
		{
			this.Travel(this.LinkMap[this.CurrentPoint].Left);
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x004F4DC1 File Offset: 0x004F2FC1
		public void TravelRight()
		{
			this.Travel(this.LinkMap[this.CurrentPoint].Right);
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x004F4DDF File Offset: 0x004F2FDF
		public void SwapPageLeft()
		{
			if (this.OnPageMoveAttempt != null)
			{
				this.OnPageMoveAttempt(-1);
			}
			UILinkPointNavigator.ChangePage(this.PageOnLeft);
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x004F4E00 File Offset: 0x004F3000
		public void SwapPageRight()
		{
			if (this.OnPageMoveAttempt != null)
			{
				this.OnPageMoveAttempt(1);
			}
			UILinkPointNavigator.ChangePage(this.PageOnRight);
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x004F4E24 File Offset: 0x004F3024
		private void Travel(int next)
		{
			if (next < 0)
			{
				if (this.ReachEndEvent != null)
				{
					this.ReachEndEvent(this.CurrentPoint, next);
					if (this.TravelEvent != null)
					{
						this.TravelEvent();
						return;
					}
				}
			}
			else
			{
				UILinkPointNavigator.ChangePoint(next);
				if (this.TravelEvent != null)
				{
					this.TravelEvent();
				}
			}
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06001A4B RID: 6731 RVA: 0x004F4E7C File Offset: 0x004F307C
		// (remove) Token: 0x06001A4C RID: 6732 RVA: 0x004F4EB4 File Offset: 0x004F30B4
		public event Func<string> OnSpecialInteracts;

		// Token: 0x06001A4D RID: 6733 RVA: 0x004F4EE9 File Offset: 0x004F30E9
		public string SpecialInteractions()
		{
			if (this.OnSpecialInteracts != null)
			{
				return this.OnSpecialInteracts();
			}
			return string.Empty;
		}

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06001A4E RID: 6734 RVA: 0x004F4F04 File Offset: 0x004F3104
		// (remove) Token: 0x06001A4F RID: 6735 RVA: 0x004F4F3C File Offset: 0x004F313C
		public event Func<string> OnSpecialInteractsLate;

		// Token: 0x06001A50 RID: 6736 RVA: 0x004F4F71 File Offset: 0x004F3171
		public string SpecialInteractionsLate()
		{
			if (this.OnSpecialInteractsLate != null)
			{
				return this.OnSpecialInteractsLate();
			}
			return string.Empty;
		}

		// Token: 0x040014B1 RID: 5297
		public int ID;

		// Token: 0x040014B2 RID: 5298
		public int PageOnLeft = -1;

		// Token: 0x040014B3 RID: 5299
		public int PageOnRight = -1;

		// Token: 0x040014B4 RID: 5300
		public int DefaultPoint;

		// Token: 0x040014B5 RID: 5301
		public int CurrentPoint;

		// Token: 0x040014B6 RID: 5302
		public Dictionary<int, UILinkPoint> LinkMap = new Dictionary<int, UILinkPoint>();
	}
}
