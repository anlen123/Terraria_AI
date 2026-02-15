using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Capture
{
	// Token: 0x020001DD RID: 477
	public class CaptureManager : IDisposable
	{
		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06001FFC RID: 8188 RVA: 0x005200C9 File Offset: 0x0051E2C9
		public bool IsCapturing
		{
			get
			{
				return !Main.dedServ && this._camera.IsCapturing;
			}
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x005200DF File Offset: 0x0051E2DF
		public CaptureManager()
		{
			this._interface = new CaptureInterface();
			if (!Main.dedServ)
			{
				this._camera = new CaptureCamera(Main.instance.GraphicsDevice);
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06001FFE RID: 8190 RVA: 0x0052010E File Offset: 0x0051E30E
		// (set) Token: 0x06001FFF RID: 8191 RVA: 0x0052011B File Offset: 0x0051E31B
		public bool Active
		{
			get
			{
				return this._interface.Active;
			}
			set
			{
				if (Main.CaptureModeDisabled)
				{
					return;
				}
				if (this._interface.Active != value)
				{
					this._interface.ToggleCamera(value);
				}
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06002000 RID: 8192 RVA: 0x0052013F File Offset: 0x0051E33F
		public bool UsingMap
		{
			get
			{
				return this.Active && this._interface.UsingMap();
			}
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x00520156 File Offset: 0x0051E356
		public void Scrolling()
		{
			this._interface.Scrolling();
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x00520163 File Offset: 0x0051E363
		public void Update(CaptureInterface.SelectionContext context)
		{
			this._interface.Update(context);
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x00520171 File Offset: 0x0051E371
		public void Draw(SpriteBatch sb)
		{
			this._interface.Draw(sb);
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x0052017F File Offset: 0x0051E37F
		public float GetProgress()
		{
			return this._camera.GetProgress();
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x0052018C File Offset: 0x0051E38C
		public void Capture()
		{
			CaptureSettings settings = new CaptureSettings
			{
				Area = new Rectangle(2660, 100, 1000, 1000),
				UseScaling = false
			};
			this.Capture(settings);
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x005201C9 File Offset: 0x0051E3C9
		public void Capture(CaptureSettings settings)
		{
			this._camera.Capture(settings);
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x005201D7 File Offset: 0x0051E3D7
		public void DrawTick()
		{
			this._interface.UpdateCameraCountdown();
			this._camera.DrawTick();
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x005201EF File Offset: 0x0051E3EF
		public void Dispose()
		{
			this._camera.Dispose();
		}

		// Token: 0x04004A69 RID: 19049
		public static CaptureManager Instance = new CaptureManager();

		// Token: 0x04004A6A RID: 19050
		private CaptureInterface _interface;

		// Token: 0x04004A6B RID: 19051
		private CaptureCamera _camera;
	}
}
