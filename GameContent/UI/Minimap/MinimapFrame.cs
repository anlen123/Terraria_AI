using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;

namespace Terraria.GameContent.UI.Minimap
{
	// Token: 0x020003C3 RID: 963
	public class MinimapFrame : IConfigKeyHolder
	{
		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x005A1AE6 File Offset: 0x0059FCE6
		// (set) Token: 0x06002D17 RID: 11543 RVA: 0x005A1AEE File Offset: 0x0059FCEE
		public string ConfigKey { get; set; }

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06002D18 RID: 11544 RVA: 0x005A1AF7 File Offset: 0x0059FCF7
		// (set) Token: 0x06002D19 RID: 11545 RVA: 0x005A1AFF File Offset: 0x0059FCFF
		public string NameKey { get; set; }

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06002D1A RID: 11546 RVA: 0x005A1B08 File Offset: 0x0059FD08
		// (set) Token: 0x06002D1B RID: 11547 RVA: 0x005A1B10 File Offset: 0x0059FD10
		public Vector2 MinimapPosition { get; set; }

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06002D1C RID: 11548 RVA: 0x005A1B19 File Offset: 0x0059FD19
		// (set) Token: 0x06002D1D RID: 11549 RVA: 0x005A1B2C File Offset: 0x0059FD2C
		private Vector2 FramePosition
		{
			get
			{
				return this.MinimapPosition + this._frameOffset;
			}
			set
			{
				this.MinimapPosition = value - this._frameOffset;
			}
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x005A1B40 File Offset: 0x0059FD40
		public MinimapFrame(Asset<Texture2D> frameTexture, Vector2 frameOffset)
		{
			this._frameTexture = frameTexture;
			this._frameOffset = frameOffset;
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x005A1B56 File Offset: 0x0059FD56
		public void SetResetButton(Asset<Texture2D> hoverTexture, Vector2 position)
		{
			this._resetButton = new MinimapFrame.Button(hoverTexture, position, delegate()
			{
				this.ResetZoom();
			});
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x005A1B71 File Offset: 0x0059FD71
		private void ResetZoom()
		{
			Main.mapMinimapScale = 1.05f;
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x005A1B7D File Offset: 0x0059FD7D
		public void SetZoomInButton(Asset<Texture2D> hoverTexture, Vector2 position)
		{
			this._zoomInButton = new MinimapFrame.Button(hoverTexture, position, delegate()
			{
				this.ZoomInButton();
			});
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x005A1B98 File Offset: 0x0059FD98
		private void ZoomInButton()
		{
			Main.mapMinimapScale *= 1.025f;
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x005A1BAA File Offset: 0x0059FDAA
		public void SetZoomOutButton(Asset<Texture2D> hoverTexture, Vector2 position)
		{
			this._zoomOutButton = new MinimapFrame.Button(hoverTexture, position, delegate()
			{
				this.ZoomOutButton();
			});
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x005A1BC5 File Offset: 0x0059FDC5
		private void ZoomOutButton()
		{
			Main.mapMinimapScale *= 0.975f;
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x005A1BD8 File Offset: 0x0059FDD8
		public void Update()
		{
			MinimapFrame.Button button = null;
			if (this._zoomInButton.IsHighlighted)
			{
				button = this._zoomInButton;
			}
			if (this._zoomOutButton.IsHighlighted)
			{
				button = this._zoomOutButton;
			}
			if (this._resetButton.IsHighlighted)
			{
				button = this._resetButton;
			}
			this._zoomInButton.IsHighlighted = false;
			this._zoomOutButton.IsHighlighted = false;
			this._resetButton.IsHighlighted = false;
			MinimapFrame.Button buttonUnderMouse = this.GetButtonUnderMouse();
			if (buttonUnderMouse != null && !PlayerInput.IgnoreMouseInterface && !Main.LocalPlayer.controlTorch)
			{
				buttonUnderMouse.IsHighlighted = true;
				Main.LocalPlayer.mouseInterface = true;
				if (button != buttonUnderMouse)
				{
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				}
				if (Main.mouseLeft)
				{
					buttonUnderMouse.Click();
					if (Main.mouseLeftRelease)
					{
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					}
				}
			}
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x005A1CB8 File Offset: 0x0059FEB8
		public void DrawBackground(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)this.MinimapPosition.X - 6, (int)this.MinimapPosition.Y - 6, 244, 244), Color.Black * Main.mapMinimapAlpha);
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x005A1D10 File Offset: 0x0059FF10
		public void DrawForeground(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this._frameTexture.Value, this.FramePosition, Color.White);
			this._zoomInButton.Draw(spriteBatch, this.FramePosition);
			this._zoomOutButton.Draw(spriteBatch, this.FramePosition);
			this._resetButton.Draw(spriteBatch, this.FramePosition);
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x005A1D70 File Offset: 0x0059FF70
		private MinimapFrame.Button GetButtonUnderMouse()
		{
			Vector2 testPoint = new Vector2((float)Main.mouseX, (float)Main.mouseY);
			if (this._zoomInButton.IsTouchingPoint(testPoint, this.FramePosition))
			{
				return this._zoomInButton;
			}
			if (this._zoomOutButton.IsTouchingPoint(testPoint, this.FramePosition))
			{
				return this._zoomOutButton;
			}
			if (this._resetButton.IsTouchingPoint(testPoint, this.FramePosition))
			{
				return this._resetButton;
			}
			return null;
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x00009E06 File Offset: 0x00008006
		[Conditional("DEBUG")]
		private void ValidateState()
		{
		}

		// Token: 0x04005476 RID: 21622
		private const float DEFAULT_ZOOM = 1.05f;

		// Token: 0x04005477 RID: 21623
		private const float ZOOM_OUT_MULTIPLIER = 0.975f;

		// Token: 0x04005478 RID: 21624
		private const float ZOOM_IN_MULTIPLIER = 1.025f;

		// Token: 0x0400547C RID: 21628
		private readonly Asset<Texture2D> _frameTexture;

		// Token: 0x0400547D RID: 21629
		private readonly Vector2 _frameOffset;

		// Token: 0x0400547E RID: 21630
		private MinimapFrame.Button _resetButton;

		// Token: 0x0400547F RID: 21631
		private MinimapFrame.Button _zoomInButton;

		// Token: 0x04005480 RID: 21632
		private MinimapFrame.Button _zoomOutButton;

		// Token: 0x02000921 RID: 2337
		private class Button
		{
			// Token: 0x1700057E RID: 1406
			// (get) Token: 0x060047DF RID: 18399 RVA: 0x006CAFA9 File Offset: 0x006C91A9
			private Vector2 Size
			{
				get
				{
					return new Vector2((float)this._hoverTexture.Width(), (float)this._hoverTexture.Height());
				}
			}

			// Token: 0x060047E0 RID: 18400 RVA: 0x006CAFC8 File Offset: 0x006C91C8
			public Button(Asset<Texture2D> hoverTexture, Vector2 position, Action mouseDownCallback)
			{
				this._position = position;
				this._hoverTexture = hoverTexture;
				this._onMouseDown = mouseDownCallback;
			}

			// Token: 0x060047E1 RID: 18401 RVA: 0x006CAFE5 File Offset: 0x006C91E5
			public void Click()
			{
				this._onMouseDown();
			}

			// Token: 0x060047E2 RID: 18402 RVA: 0x006CAFF2 File Offset: 0x006C91F2
			public void Draw(SpriteBatch spriteBatch, Vector2 parentPosition)
			{
				if (!this.IsHighlighted)
				{
					return;
				}
				spriteBatch.Draw(this._hoverTexture.Value, this._position + parentPosition, Color.White);
			}

			// Token: 0x060047E3 RID: 18403 RVA: 0x006CB020 File Offset: 0x006C9220
			public bool IsTouchingPoint(Vector2 testPoint, Vector2 parentPosition)
			{
				Vector2 value = this._position + parentPosition + this.Size * 0.5f;
				Vector2 vector = Vector2.Max(this.Size, new Vector2(22f, 22f)) * 0.5f;
				Vector2 vector2 = testPoint - value;
				return Math.Abs(vector2.X) < vector.X && Math.Abs(vector2.Y) < vector.Y;
			}

			// Token: 0x040074AF RID: 29871
			public bool IsHighlighted;

			// Token: 0x040074B0 RID: 29872
			private readonly Vector2 _position;

			// Token: 0x040074B1 RID: 29873
			private readonly Asset<Texture2D> _hoverTexture;

			// Token: 0x040074B2 RID: 29874
			private readonly Action _onMouseDown;
		}
	}
}
