using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics
{
	// Token: 0x020001D6 RID: 470
	public class SpriteViewMatrix
	{
		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x0051C474 File Offset: 0x0051A674
		// (set) Token: 0x06001F9A RID: 8090 RVA: 0x0051C47C File Offset: 0x0051A67C
		public Vector2 Zoom
		{
			get
			{
				return this._zoom;
			}
			set
			{
				if (this._zoom != value)
				{
					this._zoom = value;
					this._needsRebuild = true;
				}
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x0051C49A File Offset: 0x0051A69A
		public Vector2 Translation
		{
			get
			{
				if (this.ShouldRebuild())
				{
					this.Rebuild();
				}
				return this._translation;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06001F9C RID: 8092 RVA: 0x0051C4B0 File Offset: 0x0051A6B0
		public Matrix ZoomMatrix
		{
			get
			{
				if (this.ShouldRebuild())
				{
					this.Rebuild();
				}
				return this._zoomMatrix;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x0051C4C6 File Offset: 0x0051A6C6
		public Matrix TransformationMatrix
		{
			get
			{
				if (this.ShouldRebuild())
				{
					this.Rebuild();
				}
				return this._transformationMatrix;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06001F9E RID: 8094 RVA: 0x0051C4DC File Offset: 0x0051A6DC
		public Matrix NormalizedTransformationMatrix
		{
			get
			{
				if (this.ShouldRebuild())
				{
					this.Rebuild();
				}
				return this._normalizedTransformationMatrix;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06001F9F RID: 8095 RVA: 0x0051C4F2 File Offset: 0x0051A6F2
		public Vector2 RenderZoom
		{
			get
			{
				return new Vector2(this.ZoomMatrix.M11, this.ZoomMatrix.M22);
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06001FA0 RID: 8096 RVA: 0x0051C50F File Offset: 0x0051A70F
		// (set) Token: 0x06001FA1 RID: 8097 RVA: 0x0051C517 File Offset: 0x0051A717
		public SpriteEffects Effects
		{
			get
			{
				return this._effects;
			}
			set
			{
				if (this._effects != value)
				{
					this._effects = value;
					this._needsRebuild = true;
				}
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x0051C530 File Offset: 0x0051A730
		public Matrix EffectMatrix
		{
			get
			{
				if (this.ShouldRebuild())
				{
					this.Rebuild();
				}
				return this._effectMatrix;
			}
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x0051C548 File Offset: 0x0051A748
		public SpriteViewMatrix(GraphicsDevice graphicsDevice)
		{
			this._graphicsDevice = graphicsDevice;
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0051C5A0 File Offset: 0x0051A7A0
		private void Rebuild()
		{
			if (!this._overrideSystemViewport)
			{
				this._viewport = this._graphicsDevice.Viewport;
			}
			Vector2 vector = new Vector2((float)this._viewport.Width, (float)this._viewport.Height);
			Matrix matrix = Matrix.Identity;
			if ((this._effects & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
			{
				matrix *= Matrix.CreateScale(-1f, 1f, 1f) * Matrix.CreateTranslation(vector.X, 0f, 0f);
			}
			if ((this._effects & SpriteEffects.FlipVertically) != SpriteEffects.None)
			{
				matrix *= Matrix.CreateScale(1f, -1f, 1f) * Matrix.CreateTranslation(0f, vector.Y, 0f);
			}
			Vector2 vector2 = Utils.Round(this._zoom / 0.0078125f) * 0.0078125f;
			Vector2 value = vector * 0.5f;
			Vector2 vector3 = Utils.Round(value - value / vector2);
			Matrix matrix2 = Matrix.CreateOrthographicOffCenter(0f, vector.X, vector.Y, 0f, 0f, 1f);
			this._translation = vector3;
			this._zoomMatrix = Matrix.CreateTranslation(-vector3.X, -vector3.Y, 0f) * Matrix.CreateScale(vector2.X, vector2.Y, 1f);
			this._effectMatrix = matrix;
			this._transformationMatrix = matrix * this._zoomMatrix;
			Matrix matrix3 = Matrix.CreateTranslation(0.00390625f, 0.00390625f, 0f);
			this._transformationMatrix *= matrix3;
			this._normalizedTransformationMatrix = Matrix.Invert(matrix) * this._zoomMatrix * matrix2;
			this._needsRebuild = false;
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x0051C774 File Offset: 0x0051A974
		public void SetViewportOverride(Viewport viewport)
		{
			this._viewport = viewport;
			this._overrideSystemViewport = true;
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0051C784 File Offset: 0x0051A984
		public void ClearViewportOverride()
		{
			this._overrideSystemViewport = false;
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x0051C790 File Offset: 0x0051A990
		private bool ShouldRebuild()
		{
			return this._needsRebuild || (!this._overrideSystemViewport && !this._graphicsDevice.IsDisposed && (this._graphicsDevice.Viewport.Width != this._viewport.Width || this._graphicsDevice.Viewport.Height != this._viewport.Height));
		}

		// Token: 0x04004A0E RID: 18958
		private Vector2 _zoom = Vector2.One;

		// Token: 0x04004A0F RID: 18959
		private Vector2 _translation = Vector2.Zero;

		// Token: 0x04004A10 RID: 18960
		private Matrix _zoomMatrix = Matrix.Identity;

		// Token: 0x04004A11 RID: 18961
		private Matrix _transformationMatrix = Matrix.Identity;

		// Token: 0x04004A12 RID: 18962
		private Matrix _normalizedTransformationMatrix = Matrix.Identity;

		// Token: 0x04004A13 RID: 18963
		private SpriteEffects _effects;

		// Token: 0x04004A14 RID: 18964
		private Matrix _effectMatrix;

		// Token: 0x04004A15 RID: 18965
		private GraphicsDevice _graphicsDevice;

		// Token: 0x04004A16 RID: 18966
		private Viewport _viewport;

		// Token: 0x04004A17 RID: 18967
		private bool _overrideSystemViewport;

		// Token: 0x04004A18 RID: 18968
		private bool _needsRebuild = true;

		// Token: 0x04004A19 RID: 18969
		private const float PixelPerfectOffset = 0.00390625f;

		// Token: 0x04004A1A RID: 18970
		private const float PixelPerfectSafeZoomLevelStep = 0.0078125f;
	}
}
