using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.Graphics.Shaders
{
	// Token: 0x020001E6 RID: 486
	public class ShaderData
	{
		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600206C RID: 8300 RVA: 0x00521D23 File Offset: 0x0051FF23
		public Effect Shader
		{
			get
			{
				if (this._shader != null)
				{
					return this._shader.Value;
				}
				return null;
			}
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x00521D3A File Offset: 0x0051FF3A
		public ShaderData(Asset<Effect> shader, string passName)
		{
			this._passName = passName;
			this._shader = shader;
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x00521D50 File Offset: 0x0051FF50
		public virtual void Apply()
		{
			if (this._effect == null || this._effect != this.Shader)
			{
				this._effect = this.Shader;
				this._effectPass = this.Shader.CurrentTechnique.Passes[this._passName];
			}
			this._effectPass.Apply();
		}

		// Token: 0x04004AE2 RID: 19170
		private readonly Asset<Effect> _shader;

		// Token: 0x04004AE3 RID: 19171
		private readonly string _passName;

		// Token: 0x04004AE4 RID: 19172
		private Effect _effect;

		// Token: 0x04004AE5 RID: 19173
		private EffectPass _effectPass;

		// Token: 0x0200079F RID: 1951
		public class EffectParameter<T>
		{
			// Token: 0x06004199 RID: 16793 RVA: 0x006BADC2 File Offset: 0x006B8FC2
			private EffectParameter(Action<T> setValue)
			{
				this._setValue = setValue;
			}

			// Token: 0x0600419A RID: 16794 RVA: 0x006BADD1 File Offset: 0x006B8FD1
			public void SetValue(T value)
			{
				if (this._hasValue && EqualityComparer<T>.Default.Equals(this._value, value))
				{
					return;
				}
				this._hasValue = true;
				this._value = value;
				this._setValue(value);
			}

			// Token: 0x0600419B RID: 16795 RVA: 0x006BAE09 File Offset: 0x006B9009
			public static ShaderData.EffectParameter<T> Get(EffectParameter param)
			{
				if (param == null)
				{
					return null;
				}
				return (ShaderData.EffectParameter<T>)ShaderData.EffectParameter<T>._cachedParameters.GetValue(param, new ConditionalWeakTable<EffectParameter, object>.CreateValueCallback(ShaderData.EffectParameter<T>._Create));
			}

			// Token: 0x0600419C RID: 16796 RVA: 0x006BAE2C File Offset: 0x006B902C
			private static object _Create(EffectParameter param)
			{
				ShaderData.EffectParameter<Matrix> effectParameter = (typeof(T) == typeof(Matrix)) ? new ShaderData.EffectParameter<Matrix>(new Action<Matrix>(param.SetValue)) : ((typeof(T) == typeof(Quaternion)) ? new ShaderData.EffectParameter<Quaternion>(new Action<Quaternion>(param.SetValue)) : ((typeof(T) == typeof(Vector4)) ? new ShaderData.EffectParameter<Vector4>(new Action<Vector4>(param.SetValue)) : ((typeof(T) == typeof(Vector3)) ? new ShaderData.EffectParameter<Vector3>(new Action<Vector3>(param.SetValue)) : ((typeof(T) == typeof(Vector2)) ? new ShaderData.EffectParameter<Vector2>(new Action<Vector2>(param.SetValue)) : ((typeof(T) == typeof(float)) ? new ShaderData.EffectParameter<float>(new Action<float>(param.SetValue)) : ((typeof(T) == typeof(int)) ? new ShaderData.EffectParameter<int>(new Action<int>(param.SetValue)) : ((typeof(T) == typeof(bool)) ? new ShaderData.EffectParameter<bool>(new Action<bool>(param.SetValue)) : ((typeof(T) == typeof(string)) ? new ShaderData.EffectParameter<string>(new Action<string>(param.SetValue)) : ((typeof(T) == typeof(Texture)) ? new ShaderData.EffectParameter<Texture>(new Action<Texture>(param.SetValue)) : null)))))))));
				if (effectParameter == null)
				{
					throw new ArgumentOutOfRangeException("Unsupported type: " + typeof(T));
				}
				return effectParameter;
			}

			// Token: 0x04007042 RID: 28738
			private readonly Action<T> _setValue;

			// Token: 0x04007043 RID: 28739
			private T _value;

			// Token: 0x04007044 RID: 28740
			private bool _hasValue;

			// Token: 0x04007045 RID: 28741
			private static ConditionalWeakTable<EffectParameter, object> _cachedParameters = new ConditionalWeakTable<EffectParameter, object>();
		}
	}
}
