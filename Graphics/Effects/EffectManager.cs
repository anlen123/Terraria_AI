using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.Effects
{
	// Token: 0x020001EB RID: 491
	public abstract class EffectManager<T> where T : GameEffect
	{
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06002085 RID: 8325 RVA: 0x00522043 File Offset: 0x00520243
		public bool IsLoaded
		{
			get
			{
				return this._isLoaded;
			}
		}

		// Token: 0x1700032A RID: 810
		public T this[string key]
		{
			get
			{
				T result;
				if (this._effects.TryGetValue(key, out result))
				{
					return result;
				}
				return default(T);
			}
			set
			{
				this.Bind(key, value);
			}
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x0052207E File Offset: 0x0052027E
		public void Bind(string name, T effect)
		{
			this._effects[name] = effect;
			if (this._isLoaded)
			{
				effect.Load();
			}
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x005220A0 File Offset: 0x005202A0
		public void Load()
		{
			if (this._isLoaded)
			{
				return;
			}
			this._isLoaded = true;
			foreach (T t in this._effects.Values)
			{
				t.Load();
			}
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x0052210C File Offset: 0x0052030C
		public T Activate(string name, Vector2 position = default(Vector2), params object[] args)
		{
			if (!this._effects.ContainsKey(name))
			{
				throw new MissingEffectException(string.Concat(new object[]
				{
					"Unable to find effect named: ",
					name,
					". Type: ",
					typeof(T),
					"."
				}));
			}
			T t = this._effects[name];
			this.OnActivate(t, position);
			t.Activate(position, args);
			return t;
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x00522184 File Offset: 0x00520384
		public void Deactivate(string name, params object[] args)
		{
			if (!this._effects.ContainsKey(name))
			{
				throw new MissingEffectException(string.Concat(new object[]
				{
					"Unable to find effect named: ",
					name,
					". Type: ",
					typeof(T),
					"."
				}));
			}
			T t = this._effects[name];
			this.OnDeactivate(t);
			t.Deactivate(args);
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnActivate(T effect, Vector2 position)
		{
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnDeactivate(T effect)
		{
		}

		// Token: 0x04004AF4 RID: 19188
		protected bool _isLoaded;

		// Token: 0x04004AF5 RID: 19189
		protected Dictionary<string, T> _effects = new Dictionary<string, T>();
	}
}
