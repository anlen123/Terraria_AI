using System;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent
{
	// Token: 0x02000251 RID: 593
	public abstract class ARenderTargetContentByRequest : INeedRenderTargetContent
	{
		// Token: 0x17000375 RID: 885
		// (get) Token: 0x0600231C RID: 8988 RVA: 0x0053BF5B File Offset: 0x0053A15B
		public bool IsReady
		{
			get
			{
				return this._wasPrepared;
			}
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x0053BF63 File Offset: 0x0053A163
		public void Request()
		{
			this._wasRequested = true;
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x0053BF6C File Offset: 0x0053A16C
		public RenderTarget2D GetTarget()
		{
			return this._target;
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x0053BF74 File Offset: 0x0053A174
		public void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			this._wasPrepared = false;
			if (!this._wasRequested)
			{
				return;
			}
			this._wasRequested = false;
			this.HandleUseRequest(device, spriteBatch);
		}

		// Token: 0x06002320 RID: 8992
		protected abstract void HandleUseRequest(GraphicsDevice device, SpriteBatch spriteBatch);

		// Token: 0x06002321 RID: 8993 RVA: 0x0053BF98 File Offset: 0x0053A198
		protected void PrepareARenderTarget_AndListenToEvents(ref RenderTarget2D target, GraphicsDevice device, int neededWidth, int neededHeight, RenderTargetUsage usage)
		{
			if (target == null || target.IsDisposed || target.Width != neededWidth || target.Height != neededHeight)
			{
				if (target != null)
				{
					target.ContentLost -= this.target_ContentLost;
					target.Disposing -= this.target_Disposing;
				}
				target = new RenderTarget2D(device, neededWidth, neededHeight, false, device.PresentationParameters.BackBufferFormat, DepthFormat.None, 0, usage);
				target.ContentLost += this.target_ContentLost;
				target.Disposing += this.target_Disposing;
			}
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x0053C032 File Offset: 0x0053A232
		private void target_Disposing(object sender, EventArgs e)
		{
			this._wasPrepared = false;
			this._target = null;
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x0053C042 File Offset: 0x0053A242
		private void target_ContentLost(object sender, EventArgs e)
		{
			this._wasPrepared = false;
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x0053C04B File Offset: 0x0053A24B
		public void Reset()
		{
			this._wasPrepared = false;
			this._wasRequested = false;
			this._target = null;
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x0053C064 File Offset: 0x0053A264
		protected void PrepareARenderTarget_WithoutListeningToEvents(ref RenderTarget2D target, GraphicsDevice device, int neededWidth, int neededHeight, RenderTargetUsage usage)
		{
			if (target == null || target.IsDisposed || target.Width != neededWidth || target.Height != neededHeight)
			{
				target = new RenderTarget2D(device, neededWidth, neededHeight, false, device.PresentationParameters.BackBufferFormat, DepthFormat.None, 0, usage);
			}
		}

		// Token: 0x04004D2D RID: 19757
		protected RenderTarget2D _target;

		// Token: 0x04004D2E RID: 19758
		protected bool _wasPrepared;

		// Token: 0x04004D2F RID: 19759
		private bool _wasRequested;
	}
}
