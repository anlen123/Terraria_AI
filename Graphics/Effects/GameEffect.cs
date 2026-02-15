using System;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.Effects
{
	// Token: 0x020001F0 RID: 496
	public abstract class GameEffect
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x060020A6 RID: 8358 RVA: 0x0052292F File Offset: 0x00520B2F
		public bool IsLoaded
		{
			get
			{
				return this._isLoaded;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060020A7 RID: 8359 RVA: 0x00522937 File Offset: 0x00520B37
		public EffectPriority Priority
		{
			get
			{
				return this._priority;
			}
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x0052293F File Offset: 0x00520B3F
		public void Load()
		{
			if (this._isLoaded)
			{
				return;
			}
			this._isLoaded = true;
			this.OnLoad();
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnLoad()
		{
		}

		// Token: 0x060020AA RID: 8362
		public abstract bool IsVisible();

		// Token: 0x060020AB RID: 8363
		public abstract void Activate(Vector2 position, params object[] args);

		// Token: 0x060020AC RID: 8364
		public abstract void Deactivate(params object[] args);

		// Token: 0x04004B07 RID: 19207
		public float Opacity;

		// Token: 0x04004B08 RID: 19208
		protected bool _isLoaded;

		// Token: 0x04004B09 RID: 19209
		protected EffectPriority _priority;
	}
}
