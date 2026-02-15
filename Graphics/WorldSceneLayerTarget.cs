using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics
{
	// Token: 0x020001CA RID: 458
	public class WorldSceneLayerTarget
	{
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06001F55 RID: 8021 RVA: 0x0051A8BE File Offset: 0x00518ABE
		public Texture2D Texture
		{
			get
			{
				return this._target;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06001F56 RID: 8022 RVA: 0x0051A8C6 File Offset: 0x00518AC6
		public Vector2 Position
		{
			get
			{
				return this._position;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06001F57 RID: 8023 RVA: 0x0051A8D0 File Offset: 0x00518AD0
		public bool IsPartiallyOffscreen
		{
			get
			{
				if (this._position == Vector2.Zero)
				{
					return true;
				}
				Vector2 value = new Vector2((float)this._target.Width, (float)this._target.Height);
				Vector2 vector = this.Position + value / 2f - Main.Camera.Center;
				Vector2 vector2 = (value - Main.Camera.ScaledSize) / 2f;
				return Math.Abs(vector.X) > vector2.X || Math.Abs(vector.Y) > vector2.Y;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06001F58 RID: 8024 RVA: 0x0051A979 File Offset: 0x00518B79
		public bool IsContentLost
		{
			get
			{
				return this._target.IsContentLost;
			}
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0051A986 File Offset: 0x00518B86
		public WorldSceneLayerTarget(GraphicsDevice graphicsDevice, int width, int height)
		{
			this._target = new RenderTarget2D(graphicsDevice, width, height, false, graphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None);
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x0051A9AC File Offset: 0x00518BAC
		public void UpdateContent(Action render)
		{
			Vector2 screenPosition = Main.screenPosition;
			Point screenSize = Main.ScreenSize;
			Vector2 zoom = Main.GameViewMatrix.Zoom;
			Vector2 center = Main.Camera.Center;
			Main.screenWidth = this._target.Width - Main.offScreenRange * 2;
			Main.screenHeight = this._target.Height - Main.offScreenRange * 2;
			Main.screenPosition = Utils.Round(center - Main.ScreenSize.ToVector2() / 2f);
			Main.GameViewMatrix.Zoom = Vector2.One;
			GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
			RenderTargetBinding[] renderTargets = graphicsDevice.GetRenderTargets();
			graphicsDevice.SetRenderTarget(this._target);
			graphicsDevice.Clear(Color.Transparent);
			this._position = Main.screenPosition - new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
			render();
			graphicsDevice.SetRenderTargets(renderTargets);
			Main.screenPosition = screenPosition;
			Main.screenWidth = screenSize.X;
			Main.screenHeight = screenSize.Y;
			Main.GameViewMatrix.Zoom = zoom;
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x0051AAB6 File Offset: 0x00518CB6
		public void Dispose()
		{
			this._target.Dispose();
		}

		// Token: 0x040049EF RID: 18927
		private readonly RenderTarget2D _target;

		// Token: 0x040049F0 RID: 18928
		private Vector2 _position;
	}
}
