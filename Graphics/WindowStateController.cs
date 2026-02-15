using System;
using System.Drawing;
using System.Windows.Forms;
using ReLogic.OS;

namespace Terraria.Graphics
{
	// Token: 0x020001D7 RID: 471
	public class WindowStateController
	{
		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x004F7A7E File Offset: 0x004F5C7E
		public bool CanMoveWindowAcrossScreens
		{
			get
			{
				return Platform.IsWindows;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x0051C803 File Offset: 0x0051AA03
		public string ScreenDeviceName
		{
			get
			{
				if (!Platform.IsWindows)
				{
					return "";
				}
				return Main.instance.Window.ScreenDeviceName;
			}
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x0051C824 File Offset: 0x0051AA24
		public void TryMovingToScreen(string screenDeviceName)
		{
			if (!this.CanMoveWindowAcrossScreens)
			{
				return;
			}
			Rectangle rectangle;
			if (!this.TryGetBounds(screenDeviceName, out rectangle))
			{
				return;
			}
			if (!this.IsVisibleOnAnyScreen(rectangle))
			{
				return;
			}
			Form form = (Form)Control.FromHandle(Main.instance.Window.Handle);
			if (!this.WouldViewFitInScreen(form.Bounds, rectangle))
			{
				return;
			}
			form.Location = new Point(rectangle.Width / 2 - form.Width / 2 + rectangle.X, rectangle.Height / 2 - form.Height / 2 + rectangle.Y);
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x0051C8BC File Offset: 0x0051AABC
		private bool TryGetBounds(string screenDeviceName, out Rectangle bounds)
		{
			bounds = default(Rectangle);
			foreach (Screen screen in Screen.AllScreens)
			{
				if (screen.DeviceName == screenDeviceName)
				{
					bounds = screen.Bounds;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x0051C905 File Offset: 0x0051AB05
		private bool WouldViewFitInScreen(Rectangle view, Rectangle screen)
		{
			return view.Width <= screen.Width && view.Height <= screen.Height;
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x0051C92C File Offset: 0x0051AB2C
		private bool IsVisibleOnAnyScreen(Rectangle rect)
		{
			Screen[] allScreens = Screen.AllScreens;
			for (int i = 0; i < allScreens.Length; i++)
			{
				if (allScreens[i].WorkingArea.IntersectsWith(rect))
				{
					return true;
				}
			}
			return false;
		}
	}
}
