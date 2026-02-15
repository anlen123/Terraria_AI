using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;
using Terraria.Graphics.Light;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Terraria.GameContent.Shaders
{
	// Token: 0x02000298 RID: 664
	public class WaterShaderData : ScreenShaderData
	{
		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06002530 RID: 9520 RVA: 0x00553498 File Offset: 0x00551698
		// (remove) Token: 0x06002531 RID: 9521 RVA: 0x005534D0 File Offset: 0x005516D0
		public event Action<TileBatch> OnWaveDraw;

		// Token: 0x06002532 RID: 9522 RVA: 0x00553508 File Offset: 0x00551708
		public WaterShaderData(string passName) : base(passName)
		{
			Main.OnRenderTargetsInitialized += this.InitRenderTargets;
			Main.OnRenderTargetsReleased += this.ReleaseRenderTargets;
			this._rippleShapeTexture = Main.Assets.Request<Texture2D>("Images/Misc/Ripples", 1);
			Main.OnPreDraw += this.PreDraw;
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x005535CC File Offset: 0x005517CC
		public override void Update(GameTime gameTime)
		{
			this._useViscosityFilter = (Main.WaveQuality >= 3);
			this._useProjectileWaves = (Main.WaveQuality >= 3);
			this._usePlayerWaves = (Main.WaveQuality >= 2);
			this._useRippleWaves = (Main.WaveQuality >= 2);
			this._useCustomWaves = (Main.WaveQuality >= 2);
			if (FocusHelper.PauseLiquidRenderer)
			{
				return;
			}
			this._progress += (float)gameTime.ElapsedGameTime.TotalSeconds * base.Intensity * 0.75f;
			this._progress %= 86400f;
			if (this._useProjectileWaves || this._useRippleWaves || this._useCustomWaves || this._usePlayerWaves)
			{
				this._queuedSteps++;
			}
			base.Update(gameTime);
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x005536A8 File Offset: 0x005518A8
		private void StepLiquids()
		{
			this._isWaveBufferDirty = true;
			Vector2 vector = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
			Vector2 vector2 = vector - Main.screenPosition;
			TileBatch tileBatch = Main.tileBatch;
			GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
			graphicsDevice.SetRenderTarget(this._distortionTarget);
			if (this._clearNextFrame)
			{
				graphicsDevice.Clear(new Color(0.5f, 0.5f, 0f, 1f));
				this._clearNextFrame = false;
			}
			this.DrawWaves();
			graphicsDevice.SetRenderTarget(this._distortionTargetSwap);
			graphicsDevice.Clear(new Color(0.5f, 0.5f, 0.5f, 1f));
			Main.tileBatch.Begin();
			vector2 *= 0.25f;
			vector2.X = (float)Math.Floor((double)vector2.X);
			vector2.Y = (float)Math.Floor((double)vector2.Y);
			Vector2 vector3 = vector2 - this._lastDistortionDrawOffset;
			this._lastDistortionDrawOffset = vector2;
			tileBatch.Draw(this._distortionTarget, new Vector4(vector3.X, vector3.Y, (float)this._distortionTarget.Width, (float)this._distortionTarget.Height), new VertexColors(Color.White));
			GameShaders.Misc["WaterProcessor"].Apply(new DrawData?(new DrawData(this._distortionTarget, Vector2.Zero, Color.White)));
			tileBatch.End();
			RenderTarget2D distortionTarget = this._distortionTarget;
			this._distortionTarget = this._distortionTargetSwap;
			this._distortionTargetSwap = distortionTarget;
			if (this._useViscosityFilter)
			{
				LiquidRenderer.Instance.SetWaveMaskData(ref this._viscosityMaskChain[this._activeViscosityMask]);
				tileBatch.Begin();
				Rectangle cachedDrawArea = LiquidRenderer.Instance.GetCachedDrawArea();
				Rectangle rectangle = new Rectangle(0, 0, cachedDrawArea.Height, cachedDrawArea.Width);
				Vector4 vector4 = new Vector4((float)(cachedDrawArea.X + cachedDrawArea.Width), (float)cachedDrawArea.Y, (float)cachedDrawArea.Height, (float)cachedDrawArea.Width);
				vector4 *= 16f;
				vector4.X -= vector.X;
				vector4.Y -= vector.Y;
				vector4 *= 0.25f;
				vector4.X += vector2.X;
				vector4.Y += vector2.Y;
				graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
				tileBatch.Draw(this._viscosityMaskChain[this._activeViscosityMask], vector4, new Rectangle?(rectangle), new VertexColors(Color.White), rectangle.Size(), SpriteEffects.FlipHorizontally, -1.5707964f);
				tileBatch.End();
				this._activeViscosityMask++;
				this._activeViscosityMask %= this._viscosityMaskChain.Length;
			}
			graphicsDevice.SetRenderTarget(null);
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x0055399C File Offset: 0x00551B9C
		private void DrawWaves()
		{
			Vector2 screenPosition = Main.screenPosition;
			Vector2 value = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
			Vector2 value2 = -this._lastDistortionDrawOffset / 0.25f + value;
			TileBatch tileBatch = Main.tileBatch;
			GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
			Vector2 dimensions = new Vector2((float)Main.screenWidth, (float)Main.screenHeight);
			Vector2 value3 = new Vector2(16f, 16f);
			tileBatch.Begin();
			GameShaders.Misc["WaterDistortionObject"].Apply(null);
			if (this._useNPCWaves)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					if (Main.npc[i] != null && Main.npc[i].active && (Main.npc[i].wet || Main.npc[i].wetCount != 0) && Collision.CheckAABBvAABBCollision(screenPosition, dimensions, Main.npc[i].position - value3, Main.npc[i].Size + value3))
					{
						NPC npc = Main.npc[i];
						Vector2 vector = npc.Center - value2;
						Vector2 vector2 = npc.velocity.RotatedBy((double)(-(double)npc.rotation), default(Vector2)) / new Vector2((float)npc.height, (float)npc.width);
						float num = vector2.LengthSquared();
						num = num * 0.3f + 0.7f * num * (1024f / (float)(npc.height * npc.width));
						num = Math.Min(num, 0.08f);
						num += (npc.velocity - npc.oldVelocity).Length() * 0.5f;
						vector2.Normalize();
						Vector2 velocity = npc.velocity;
						velocity.Normalize();
						vector -= velocity * 10f;
						if (!this._useViscosityFilter && (npc.honeyWet || npc.lavaWet))
						{
							num *= 0.3f;
						}
						if (npc.wet)
						{
							tileBatch.Draw(TextureAssets.MagicPixel.Value, new Vector4(vector.X, vector.Y, (float)npc.width * 2f, (float)npc.height * 2f) * 0.25f, null, new VertexColors(new Color(vector2.X * 0.5f + 0.5f, vector2.Y * 0.5f + 0.5f, 0.5f * num)), new Vector2((float)TextureAssets.MagicPixel.Width() / 2f, (float)TextureAssets.MagicPixel.Height() / 2f), SpriteEffects.None, npc.rotation);
						}
						if (npc.wetCount != 0)
						{
							num = npc.velocity.Length();
							num = 0.195f * (float)Math.Sqrt((double)num);
							float scaleFactor = 5f;
							if (!npc.wet)
							{
								scaleFactor = -20f;
							}
							this.QueueRipple(npc.Center + velocity * scaleFactor, new Color(0.5f, (npc.wet ? num : (-num)) * 0.5f + 0.5f, 0f, 1f) * 0.5f, new Vector2((float)npc.width, (float)npc.height * ((float)npc.wetCount / 9f)) * MathHelper.Clamp(num * 10f, 0f, 1f), RippleShape.Circle, 0f);
						}
					}
				}
			}
			if (this._usePlayerWaves)
			{
				for (int j = 0; j < 255; j++)
				{
					if (Main.player[j] != null && Main.player[j].active && (Main.player[j].wet || Main.player[j].wetCount != 0) && Collision.CheckAABBvAABBCollision(screenPosition, dimensions, Main.player[j].position - value3, Main.player[j].Size + value3))
					{
						Player player = Main.player[j];
						Vector2 vector3 = player.Center - value2;
						float num2 = player.velocity.Length();
						num2 = 0.05f * (float)Math.Sqrt((double)num2);
						Vector2 velocity2 = player.velocity;
						velocity2.Normalize();
						vector3 -= velocity2 * 10f;
						if (!this._useViscosityFilter && (player.honeyWet || player.lavaWet))
						{
							num2 *= 0.3f;
						}
						if (player.wet)
						{
							tileBatch.Draw(TextureAssets.MagicPixel.Value, new Vector4(vector3.X - (float)player.width * 2f * 0.5f, vector3.Y - (float)player.height * 2f * 0.5f, (float)player.width * 2f, (float)player.height * 2f) * 0.25f, new VertexColors(new Color(velocity2.X * 0.5f + 0.5f, velocity2.Y * 0.5f + 0.5f, 0.5f * num2)));
						}
						if (player.wetCount != 0)
						{
							float scaleFactor2 = 5f;
							if (!player.wet)
							{
								scaleFactor2 = -20f;
							}
							num2 *= 3f;
							this.QueueRipple(player.Center + velocity2 * scaleFactor2, player.wet ? num2 : (-num2), new Vector2((float)player.width, (float)player.height * ((float)player.wetCount / 9f)) * MathHelper.Clamp(num2 * 10f, 0f, 1f), RippleShape.Circle, 0f);
						}
					}
				}
			}
			if (this._useProjectileWaves)
			{
				for (int k = 0; k < 1000; k++)
				{
					Projectile projectile = Main.projectile[k];
					if (projectile.wet && !projectile.lavaWet)
					{
						bool flag = !projectile.honeyWet;
					}
					bool flag2 = projectile.lavaWet;
					bool flag3 = projectile.honeyWet;
					bool flag4 = projectile.wet;
					if (projectile.ignoreWater)
					{
						flag4 = true;
					}
					if (projectile != null && projectile.active && ProjectileID.Sets.CanDistortWater[projectile.type] && flag4 && !ProjectileID.Sets.NoLiquidDistortion[projectile.type] && Collision.CheckAABBvAABBCollision(screenPosition, dimensions, projectile.position - value3, projectile.Size + value3))
					{
						if (projectile.ignoreWater)
						{
							bool flag5 = Collision.LavaCollision(projectile.position, projectile.width, projectile.height);
							flag2 = Collision.WetCollision(projectile.position, projectile.width, projectile.height);
							flag3 = Collision.honey;
							flag4 = (flag5 || flag2 || flag3);
							if (!flag4)
							{
								goto IL_879;
							}
						}
						Vector2 vector4 = projectile.Center - value2;
						float num3 = projectile.velocity.Length();
						num3 = 2f * (float)Math.Sqrt((double)(0.05f * num3));
						Vector2 velocity3 = projectile.velocity;
						velocity3.Normalize();
						if (!this._useViscosityFilter && (flag3 || flag2))
						{
							num3 *= 0.3f;
						}
						float num4 = Math.Max(12f, (float)projectile.width * 0.75f);
						float num5 = Math.Max(12f, (float)projectile.height * 0.75f);
						tileBatch.Draw(TextureAssets.MagicPixel.Value, new Vector4(vector4.X - num4 * 0.5f, vector4.Y - num5 * 0.5f, num4, num5) * 0.25f, new VertexColors(new Color(velocity3.X * 0.5f + 0.5f, velocity3.Y * 0.5f + 0.5f, num3 * 0.5f)));
					}
					IL_879:;
				}
			}
			tileBatch.End();
			if (this._useRippleWaves)
			{
				tileBatch.Begin();
				for (int l = 0; l < this._rippleQueueCount; l++)
				{
					Vector2 vector5 = this._rippleQueue[l].Position - value2;
					Vector2 size = this._rippleQueue[l].Size;
					Rectangle sourceRectangle = this._rippleQueue[l].SourceRectangle;
					Texture2D value4 = this._rippleShapeTexture.Value;
					tileBatch.Draw(value4, new Vector4(vector5.X, vector5.Y, size.X, size.Y) * 0.25f, new Rectangle?(sourceRectangle), new VertexColors(this._rippleQueue[l].WaveData), new Vector2((float)(sourceRectangle.Width / 2), (float)(sourceRectangle.Height / 2)), SpriteEffects.None, this._rippleQueue[l].Rotation);
				}
				tileBatch.End();
			}
			this._rippleQueueCount = 0;
			if (this._useCustomWaves && this.OnWaveDraw != null)
			{
				tileBatch.Begin();
				this.OnWaveDraw(tileBatch);
				tileBatch.End();
			}
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x0055436C File Offset: 0x0055256C
		private void PreDraw(GameTime gameTime)
		{
			this.ValidateRenderTargets();
			if (!this._usingRenderTargets || !Main.IsGraphicsDeviceAvailable)
			{
				return;
			}
			if (this._useProjectileWaves || this._useRippleWaves || this._useCustomWaves || this._usePlayerWaves)
			{
				for (int i = 0; i < Math.Min(this._queuedSteps, 2); i++)
				{
					this.StepLiquids();
				}
			}
			else if (this._isWaveBufferDirty || this._clearNextFrame)
			{
				GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
				graphicsDevice.SetRenderTarget(this._distortionTarget);
				graphicsDevice.Clear(new Color(0.5f, 0.5f, 0f, 1f));
				this._clearNextFrame = false;
				this._isWaveBufferDirty = false;
				graphicsDevice.SetRenderTarget(null);
			}
			this._queuedSteps = 0;
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x00554430 File Offset: 0x00552630
		public override void Apply()
		{
			if (!this._usingRenderTargets || !Main.IsGraphicsDeviceAvailable)
			{
				return;
			}
			base.UseProgress(this._progress);
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			Vector2 vector = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
			Vector2 unscaledScreenPosition = ScreenShaderData.UnscaledScreenPosition;
			Vector2 value = (Main.drawToScreen ? Vector2.Zero : vector) - unscaledScreenPosition;
			base.UseImage(this.DrawRipples ? this._distortionTarget : this._noDistortionTexture, 1, null);
			base.UseImage(Main.waterTarget.Texture, 2, SamplerState.PointClamp);
			base.UseTargetPosition(unscaledScreenPosition + vector - Main.waterTarget.Position);
			base.UseImageOffset(-(value - this._lastDistortionDrawOffset / 0.25f));
			base.Apply();
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x00554520 File Offset: 0x00552720
		private void ValidateRenderTargets()
		{
			int backBufferWidth = Main.instance.GraphicsDevice.PresentationParameters.BackBufferWidth;
			int backBufferHeight = Main.instance.GraphicsDevice.PresentationParameters.BackBufferHeight;
			bool flag = !Main.drawToScreen;
			if (this._usingRenderTargets && !flag)
			{
				this.ReleaseRenderTargets();
				return;
			}
			if (!this._usingRenderTargets && flag)
			{
				this.InitRenderTargets(backBufferWidth, backBufferHeight);
				return;
			}
			if (this._usingRenderTargets && flag && (this._distortionTarget.IsContentLost || this._distortionTargetSwap.IsContentLost))
			{
				this._clearNextFrame = true;
			}
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x005545B4 File Offset: 0x005527B4
		private void InitRenderTargets(int width, int height)
		{
			this._lastScreenWidth = width;
			this._lastScreenHeight = height;
			width = (int)((float)width * 0.25f);
			height = (int)((float)height * 0.25f);
			try
			{
				this._noDistortionTexture = new Texture2D(Main.instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
				this._noDistortionTexture.SetData<Color>(new Color[]
				{
					new Color(0.5f, 0.5f, 0f, 1f)
				});
				this._distortionTarget = new RenderTarget2D(Main.instance.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
				this._distortionTargetSwap = new RenderTarget2D(Main.instance.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
				this._usingRenderTargets = true;
				this._clearNextFrame = true;
			}
			catch (Exception arg)
			{
				Lighting.Mode = LightMode.Retro;
				this._usingRenderTargets = false;
				Console.WriteLine("Failed to create water distortion render targets. " + arg);
			}
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x005546A8 File Offset: 0x005528A8
		private void ReleaseRenderTargets()
		{
			try
			{
				if (this._distortionTarget != null)
				{
					this._distortionTarget.Dispose();
				}
				if (this._distortionTargetSwap != null)
				{
					this._distortionTargetSwap.Dispose();
				}
			}
			catch (Exception arg)
			{
				Console.WriteLine("Error disposing of water distortion render targets. " + arg);
			}
			this._distortionTarget = null;
			this._distortionTargetSwap = null;
			this._usingRenderTargets = false;
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x00554718 File Offset: 0x00552918
		public void QueueRipple(Vector2 position, float strength = 1f, RippleShape shape = RippleShape.Square, float rotation = 0f)
		{
			float g = strength * 0.5f + 0.5f;
			float scale = Math.Min(Math.Abs(strength), 1f);
			this.QueueRipple(position, new Color(0.5f, g, 0f, 1f) * scale, new Vector2(4f * Math.Max(Math.Abs(strength), 1f)), shape, rotation);
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x00554784 File Offset: 0x00552984
		public void QueueRipple(Vector2 position, float strength, Vector2 size, RippleShape shape = RippleShape.Square, float rotation = 0f)
		{
			float g = strength * 0.5f + 0.5f;
			float scale = Math.Min(Math.Abs(strength), 1f);
			this.QueueRipple(position, new Color(0.5f, g, 0f, 1f) * scale, size, shape, rotation);
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x005547D8 File Offset: 0x005529D8
		public void QueueRipple(Vector2 position, Color waveData, Vector2 size, RippleShape shape = RippleShape.Square, float rotation = 0f)
		{
			if (!this._useRippleWaves || Main.drawToScreen)
			{
				this._rippleQueueCount = 0;
				return;
			}
			if (this._rippleQueueCount < this._rippleQueue.Length)
			{
				WaterShaderData.Ripple[] rippleQueue = this._rippleQueue;
				int rippleQueueCount = this._rippleQueueCount;
				this._rippleQueueCount = rippleQueueCount + 1;
				rippleQueue[rippleQueueCount] = new WaterShaderData.Ripple(position, waveData, size, shape, rotation);
			}
		}

		// Token: 0x04004F73 RID: 20339
		private const float DISTORTION_BUFFER_SCALE = 0.25f;

		// Token: 0x04004F74 RID: 20340
		private const float WAVE_FRAMERATE = 0.016666668f;

		// Token: 0x04004F75 RID: 20341
		private const int MAX_RIPPLES_QUEUED = 200;

		// Token: 0x04004F77 RID: 20343
		public bool DrawRipples = true;

		// Token: 0x04004F78 RID: 20344
		public bool _useViscosityFilter = true;

		// Token: 0x04004F79 RID: 20345
		private RenderTarget2D _distortionTarget;

		// Token: 0x04004F7A RID: 20346
		private RenderTarget2D _distortionTargetSwap;

		// Token: 0x04004F7B RID: 20347
		private Texture2D _noDistortionTexture;

		// Token: 0x04004F7C RID: 20348
		private bool _usingRenderTargets;

		// Token: 0x04004F7D RID: 20349
		private Vector2 _lastDistortionDrawOffset = Vector2.Zero;

		// Token: 0x04004F7E RID: 20350
		private float _progress;

		// Token: 0x04004F7F RID: 20351
		private WaterShaderData.Ripple[] _rippleQueue = new WaterShaderData.Ripple[200];

		// Token: 0x04004F80 RID: 20352
		private int _rippleQueueCount;

		// Token: 0x04004F81 RID: 20353
		private int _lastScreenWidth;

		// Token: 0x04004F82 RID: 20354
		private int _lastScreenHeight;

		// Token: 0x04004F83 RID: 20355
		public bool _useProjectileWaves = true;

		// Token: 0x04004F84 RID: 20356
		private bool _useNPCWaves = true;

		// Token: 0x04004F85 RID: 20357
		private bool _usePlayerWaves = true;

		// Token: 0x04004F86 RID: 20358
		private bool _useRippleWaves = true;

		// Token: 0x04004F87 RID: 20359
		private bool _useCustomWaves = true;

		// Token: 0x04004F88 RID: 20360
		private bool _clearNextFrame = true;

		// Token: 0x04004F89 RID: 20361
		private Texture2D[] _viscosityMaskChain = new Texture2D[3];

		// Token: 0x04004F8A RID: 20362
		private int _activeViscosityMask;

		// Token: 0x04004F8B RID: 20363
		private Asset<Texture2D> _rippleShapeTexture;

		// Token: 0x04004F8C RID: 20364
		private bool _isWaveBufferDirty = true;

		// Token: 0x04004F8D RID: 20365
		private int _queuedSteps;

		// Token: 0x04004F8E RID: 20366
		private const int MAX_QUEUED_STEPS = 2;

		// Token: 0x0200080F RID: 2063
		private struct Ripple
		{
			// Token: 0x1700053C RID: 1340
			// (get) Token: 0x060042E1 RID: 17121 RVA: 0x006BE83D File Offset: 0x006BCA3D
			public Rectangle SourceRectangle
			{
				get
				{
					return WaterShaderData.Ripple.RIPPLE_SHAPE_SOURCE_RECTS[(int)this.Shape];
				}
			}

			// Token: 0x060042E2 RID: 17122 RVA: 0x006BE84F File Offset: 0x006BCA4F
			public Ripple(Vector2 position, Color waveData, Vector2 size, RippleShape shape, float rotation)
			{
				this.Position = position;
				this.WaveData = waveData;
				this.Size = size;
				this.Shape = shape;
				this.Rotation = rotation;
			}

			// Token: 0x040071AD RID: 29101
			private static readonly Rectangle[] RIPPLE_SHAPE_SOURCE_RECTS = new Rectangle[]
			{
				new Rectangle(0, 0, 0, 0),
				new Rectangle(1, 1, 62, 62),
				new Rectangle(1, 65, 62, 62)
			};

			// Token: 0x040071AE RID: 29102
			public readonly Vector2 Position;

			// Token: 0x040071AF RID: 29103
			public readonly Color WaveData;

			// Token: 0x040071B0 RID: 29104
			public readonly Vector2 Size;

			// Token: 0x040071B1 RID: 29105
			public readonly RippleShape Shape;

			// Token: 0x040071B2 RID: 29106
			public readonly float Rotation;
		}
	}
}
