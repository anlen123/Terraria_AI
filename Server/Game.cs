using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Server
{
	// Token: 0x02000069 RID: 105
	public class Game : IDisposable
	{
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x000762F3 File Offset: 0x000744F3
		public GameComponentCollection Components
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x000762F3 File Offset: 0x000744F3
		// (set) Token: 0x06001457 RID: 5207 RVA: 0x00009E06 File Offset: 0x00008006
		public ContentManager Content
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x000762F3 File Offset: 0x000744F3
		public GraphicsDevice GraphicsDevice
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x004BAB41 File Offset: 0x004B8D41
		// (set) Token: 0x0600145A RID: 5210 RVA: 0x00009E06 File Offset: 0x00008006
		public TimeSpan InactiveSleepTime
		{
			get
			{
				return TimeSpan.Zero;
			}
			set
			{
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x000379F1 File Offset: 0x00035BF1
		public bool IsActive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x000379F1 File Offset: 0x00035BF1
		// (set) Token: 0x0600145D RID: 5213 RVA: 0x00009E06 File Offset: 0x00008006
		public bool IsFixedTimeStep
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		// (set) Token: 0x0600145F RID: 5215 RVA: 0x00009E06 File Offset: 0x00008006
		public bool IsMouseVisible
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06001460 RID: 5216 RVA: 0x000762F3 File Offset: 0x000744F3
		public LaunchParameters LaunchParameters
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x000762F3 File Offset: 0x000744F3
		public GameServiceContainer Services
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06001462 RID: 5218 RVA: 0x004BAB41 File Offset: 0x004B8D41
		// (set) Token: 0x06001463 RID: 5219 RVA: 0x00009E06 File Offset: 0x00008006
		public TimeSpan TargetElapsedTime
		{
			get
			{
				return TimeSpan.Zero;
			}
			set
			{
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x000762F3 File Offset: 0x000744F3
		public GameWindow Window
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06001465 RID: 5221 RVA: 0x004BAB48 File Offset: 0x004B8D48
		// (remove) Token: 0x06001466 RID: 5222 RVA: 0x004BAB80 File Offset: 0x004B8D80
		public event EventHandler<EventArgs> Activated;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06001467 RID: 5223 RVA: 0x004BABB8 File Offset: 0x004B8DB8
		// (remove) Token: 0x06001468 RID: 5224 RVA: 0x004BABF0 File Offset: 0x004B8DF0
		public event EventHandler<EventArgs> Deactivated;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06001469 RID: 5225 RVA: 0x004BAC28 File Offset: 0x004B8E28
		// (remove) Token: 0x0600146A RID: 5226 RVA: 0x004BAC60 File Offset: 0x004B8E60
		public event EventHandler<EventArgs> Disposed;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600146B RID: 5227 RVA: 0x004BAC98 File Offset: 0x004B8E98
		// (remove) Token: 0x0600146C RID: 5228 RVA: 0x004BACD0 File Offset: 0x004B8ED0
		public event EventHandler<EventArgs> Exiting;

		// Token: 0x0600146D RID: 5229 RVA: 0x000379F1 File Offset: 0x00035BF1
		protected virtual bool BeginDraw()
		{
			return true;
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void BeginRun()
		{
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x00009E06 File Offset: 0x00008006
		public void Dispose()
		{
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void Draw(GameTime gameTime)
		{
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void EndDraw()
		{
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void EndRun()
		{
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x00009E06 File Offset: 0x00008006
		public void Exit()
		{
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void Initialize()
		{
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void LoadContent()
		{
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void OnActivated(object sender, EventArgs args)
		{
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void OnDeactivated(object sender, EventArgs args)
		{
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void OnExiting(object sender, EventArgs args)
		{
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x00009E06 File Offset: 0x00008006
		public void ResetElapsedTime()
		{
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00009E06 File Offset: 0x00008006
		public void Run()
		{
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x00009E06 File Offset: 0x00008006
		public void RunOneFrame()
		{
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x000379F1 File Offset: 0x00035BF1
		protected virtual bool ShowMissingRequirementMessage(Exception exception)
		{
			return true;
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x00009E06 File Offset: 0x00008006
		public void SuppressDraw()
		{
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x00009E06 File Offset: 0x00008006
		public void Tick()
		{
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void UnloadContent()
		{
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void Update(GameTime gameTime)
		{
		}
	}
}
