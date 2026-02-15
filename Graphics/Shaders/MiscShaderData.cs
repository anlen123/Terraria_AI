using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders
{
	// Token: 0x020001E1 RID: 481
	public class MiscShaderData : ShaderData
	{
		// Token: 0x06002018 RID: 8216 RVA: 0x005205B8 File Offset: 0x0051E7B8
		public MiscShaderData(Asset<Effect> shader, string passName) : base(shader, passName)
		{
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x00520604 File Offset: 0x0051E804
		private void CheckCachedParameters()
		{
			if (this._effect != null && this._effect == base.Shader)
			{
				return;
			}
			this._effect = base.Shader;
			this.uColor = base.Shader.GetParameter("uColor");
			this.uSaturation = base.Shader.GetParameter("uSaturation");
			this.uSecondaryColor = base.Shader.GetParameter("uSecondaryColor");
			this.uTime = base.Shader.GetParameter("uTime");
			this.uOpacity = base.Shader.GetParameter("uOpacity");
			this.uShaderSpecificData = base.Shader.GetParameter("uShaderSpecificData");
			this.uSourceRect = base.Shader.GetParameter("uSourceRect");
			this.uDrawPosition = base.Shader.GetParameter("uDrawPosition");
			this.uImageSize0 = base.Shader.GetParameter("uImageSize0");
			this.uImageSize1 = base.Shader.GetParameter("uImageSize1");
			this.uImageSize2 = base.Shader.GetParameter("uImageSize2");
			this.MatrixTransform = base.Shader.GetParameter("MatrixTransform");
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x0052073C File Offset: 0x0051E93C
		public virtual void Apply(DrawData? drawData = null)
		{
			this.CheckCachedParameters();
			this.uColor.SetValue(this._uColor);
			this.uSaturation.SetValue(this._uSaturation);
			this.uSecondaryColor.SetValue(this._uSecondaryColor);
			this.uTime.SetValue(Main.GlobalTimeWrappedHourly);
			this.uOpacity.SetValue(this._uOpacity);
			this.uShaderSpecificData.SetValue(this._shaderSpecificData);
			if (drawData != null)
			{
				DrawData value = drawData.Value;
				Vector4 zero = Vector4.Zero;
				if (drawData.Value.sourceRect != null)
				{
					zero = new Vector4((float)value.sourceRect.Value.X, (float)value.sourceRect.Value.Y, (float)value.sourceRect.Value.Width, (float)value.sourceRect.Value.Height);
				}
				this.uSourceRect.SetValue(zero);
				this.uDrawPosition.SetValue(value.position);
				this.uImageSize0.SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
			}
			else
			{
				this.uSourceRect.SetValue(new Vector4(0f, 0f, 4f, 4f));
			}
			SamplerState value2 = SamplerState.LinearWrap;
			if (this._customSamplerState != null)
			{
				value2 = this._customSamplerState;
			}
			Texture texture = (this._uImage0 != null) ? this._uImage0.Value : this._uImage0Tex;
			if (texture != null)
			{
				Main.graphics.GraphicsDevice.Textures[0] = texture;
				Main.graphics.GraphicsDevice.SamplerStates[0] = value2;
				if (texture is Texture2D)
				{
					this.uImageSize0.SetValue(((Texture2D)texture).Size());
				}
			}
			texture = ((this._uImage1 != null) ? this._uImage1.Value : this._uImage1Tex);
			if (texture != null)
			{
				Main.graphics.GraphicsDevice.Textures[1] = texture;
				Main.graphics.GraphicsDevice.SamplerStates[1] = value2;
				if (texture is Texture2D)
				{
					this.uImageSize1.SetValue(((Texture2D)texture).Size());
				}
			}
			texture = ((this._uImage2 != null) ? this._uImage2.Value : this._uImage2Tex);
			if (texture != null)
			{
				Main.graphics.GraphicsDevice.Textures[2] = texture;
				Main.graphics.GraphicsDevice.SamplerStates[2] = value2;
				if (texture is Texture2D)
				{
					this.uImageSize2.SetValue(((Texture2D)texture).Size());
				}
			}
			if (this._useProjectionMatrix)
			{
				this.MatrixTransform.SetValue(Main.GameViewMatrix.NormalizedTransformationMatrix);
			}
			if (this._transformMatrix != null)
			{
				this.MatrixTransform.SetValue(this._transformMatrix.Value);
			}
			base.Apply();
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x00520A35 File Offset: 0x0051EC35
		public MiscShaderData UseColor(float r, float g, float b)
		{
			return this.UseColor(new Vector3(r, g, b));
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x00520A45 File Offset: 0x0051EC45
		public MiscShaderData UseColor(Color color)
		{
			return this.UseColor(color.ToVector3());
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x00520A54 File Offset: 0x0051EC54
		public MiscShaderData UseColor(Vector3 color)
		{
			this._uColor = color;
			return this;
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x00520A5E File Offset: 0x0051EC5E
		public MiscShaderData UseSamplerState(SamplerState state)
		{
			this._customSamplerState = state;
			return this;
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x00520A68 File Offset: 0x0051EC68
		public MiscShaderData UseImage0(string path)
		{
			if (Main.dedServ)
			{
				return this;
			}
			this._uImage0Tex = null;
			this._uImage0 = Main.Assets.Request<Texture2D>(path, 1);
			return this;
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x00520A8D File Offset: 0x0051EC8D
		public MiscShaderData UseImage1(string path)
		{
			if (Main.dedServ)
			{
				return this;
			}
			this._uImage1Tex = null;
			this._uImage1 = Main.Assets.Request<Texture2D>(path, 1);
			return this;
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x00520AB2 File Offset: 0x0051ECB2
		public MiscShaderData UseImage2(string path)
		{
			if (Main.dedServ)
			{
				return this;
			}
			this._uImage2Tex = null;
			this._uImage2 = Main.Assets.Request<Texture2D>(path, 1);
			return this;
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x00520AD7 File Offset: 0x0051ECD7
		public MiscShaderData UseImage0(Texture texture)
		{
			if (Main.dedServ)
			{
				return this;
			}
			this._uImage0Tex = texture;
			this._uImage0 = null;
			return this;
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x00520AF1 File Offset: 0x0051ECF1
		public MiscShaderData UseImage1(Texture texture)
		{
			if (Main.dedServ)
			{
				return this;
			}
			this._uImage1Tex = texture;
			this._uImage1 = null;
			return this;
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x00520B0B File Offset: 0x0051ED0B
		public MiscShaderData UseImage2(Texture texture)
		{
			if (Main.dedServ)
			{
				return this;
			}
			this._uImage2Tex = texture;
			this._uImage2 = null;
			return this;
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x00520B25 File Offset: 0x0051ED25
		private static bool IsPowerOfTwo(int n)
		{
			return (int)Math.Ceiling(Math.Log((double)n) / Math.Log(2.0)) == (int)Math.Floor(Math.Log((double)n) / Math.Log(2.0));
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x00520B61 File Offset: 0x0051ED61
		public MiscShaderData UseOpacity(float alpha)
		{
			this._uOpacity = alpha;
			return this;
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x00520B6B File Offset: 0x0051ED6B
		public MiscShaderData UseSecondaryColor(float r, float g, float b)
		{
			return this.UseSecondaryColor(new Vector3(r, g, b));
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x00520B7B File Offset: 0x0051ED7B
		public MiscShaderData UseSecondaryColor(Color color)
		{
			return this.UseSecondaryColor(color.ToVector3());
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x00520B8A File Offset: 0x0051ED8A
		public MiscShaderData UseSecondaryColor(Vector3 color)
		{
			this._uSecondaryColor = color;
			return this;
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x00520B94 File Offset: 0x0051ED94
		public MiscShaderData UseProjectionMatrix(bool doUse)
		{
			this._useProjectionMatrix = doUse;
			return this;
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x00520BA0 File Offset: 0x0051EDA0
		public MiscShaderData UseSpriteTransformMatrix(Matrix? transform)
		{
			if (transform == null)
			{
				this._transformMatrix = null;
				return this;
			}
			Viewport viewport = Main.graphics.GraphicsDevice.Viewport;
			float num = (viewport.Width > 0) ? (1f / (float)viewport.Width) : 0f;
			float num2 = (viewport.Height > 0) ? (-1f / (float)viewport.Height) : 0f;
			Matrix matrix = new Matrix
			{
				M11 = num * 2f,
				M22 = num2 * 2f,
				M33 = 1f,
				M44 = 1f,
				M41 = -1f - num,
				M42 = 1f - num2
			};
			this._transformMatrix = new Matrix?(transform.Value * matrix);
			return this;
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x00520C88 File Offset: 0x0051EE88
		public MiscShaderData UseSaturation(float saturation)
		{
			this._uSaturation = saturation;
			return this;
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x00520C92 File Offset: 0x0051EE92
		public virtual MiscShaderData GetSecondaryShader(Entity entity)
		{
			return this;
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x00520C95 File Offset: 0x0051EE95
		public MiscShaderData UseShaderSpecificData(Vector4 specificData)
		{
			this._shaderSpecificData = specificData;
			return this;
		}

		// Token: 0x04004A7A RID: 19066
		private Vector3 _uColor = Vector3.One;

		// Token: 0x04004A7B RID: 19067
		private Vector3 _uSecondaryColor = Vector3.One;

		// Token: 0x04004A7C RID: 19068
		private float _uSaturation = 1f;

		// Token: 0x04004A7D RID: 19069
		private float _uOpacity = 1f;

		// Token: 0x04004A7E RID: 19070
		private Asset<Texture2D> _uImage0;

		// Token: 0x04004A7F RID: 19071
		private Asset<Texture2D> _uImage1;

		// Token: 0x04004A80 RID: 19072
		private Asset<Texture2D> _uImage2;

		// Token: 0x04004A81 RID: 19073
		private Texture _uImage0Tex;

		// Token: 0x04004A82 RID: 19074
		private Texture _uImage1Tex;

		// Token: 0x04004A83 RID: 19075
		private Texture _uImage2Tex;

		// Token: 0x04004A84 RID: 19076
		private bool _useProjectionMatrix;

		// Token: 0x04004A85 RID: 19077
		private Vector4 _shaderSpecificData = Vector4.Zero;

		// Token: 0x04004A86 RID: 19078
		private SamplerState _customSamplerState;

		// Token: 0x04004A87 RID: 19079
		private Matrix? _transformMatrix;

		// Token: 0x04004A88 RID: 19080
		private Effect _effect;

		// Token: 0x04004A89 RID: 19081
		private ShaderData.EffectParameter<Vector3> uColor;

		// Token: 0x04004A8A RID: 19082
		private ShaderData.EffectParameter<float> uSaturation;

		// Token: 0x04004A8B RID: 19083
		private ShaderData.EffectParameter<Vector3> uSecondaryColor;

		// Token: 0x04004A8C RID: 19084
		private ShaderData.EffectParameter<float> uTime;

		// Token: 0x04004A8D RID: 19085
		private ShaderData.EffectParameter<float> uOpacity;

		// Token: 0x04004A8E RID: 19086
		private ShaderData.EffectParameter<Vector4> uShaderSpecificData;

		// Token: 0x04004A8F RID: 19087
		private ShaderData.EffectParameter<Vector4> uSourceRect;

		// Token: 0x04004A90 RID: 19088
		private ShaderData.EffectParameter<Vector2> uDrawPosition;

		// Token: 0x04004A91 RID: 19089
		private ShaderData.EffectParameter<Vector2> uImageSize0;

		// Token: 0x04004A92 RID: 19090
		private ShaderData.EffectParameter<Vector2> uImageSize1;

		// Token: 0x04004A93 RID: 19091
		private ShaderData.EffectParameter<Vector2> uImageSize2;

		// Token: 0x04004A94 RID: 19092
		private ShaderData.EffectParameter<Matrix> MatrixTransform;
	}
}
