using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.Graphics.Shaders
{
	// Token: 0x020001E3 RID: 483
	public class ScreenShaderData : ShaderData
	{
		// Token: 0x17000324 RID: 804
		// (get) Token: 0x0600203E RID: 8254 RVA: 0x0052107C File Offset: 0x0051F27C
		public float Intensity
		{
			get
			{
				return this._uIntensity;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x0600203F RID: 8255 RVA: 0x00521084 File Offset: 0x0051F284
		public float CombinedOpacity
		{
			get
			{
				return this._uOpacity * this._globalOpacity;
			}
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x00521094 File Offset: 0x0051F294
		public ScreenShaderData(string passName) : base(Main.ScreenShaderRef, passName)
		{
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x00521170 File Offset: 0x0051F370
		public ScreenShaderData(Asset<Effect> shader, string passName) : base(shader, passName)
		{
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void Update(GameTime gameTime)
		{
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x00521248 File Offset: 0x0051F448
		public static Vector2 UnscaledScreenPosition
		{
			get
			{
				Matrix effectMatrix = Main.GameViewMatrix.EffectMatrix;
				Matrix transformationMatrix = Main.GameViewMatrix.TransformationMatrix;
				return Main.screenPosition + new Vector2(effectMatrix.M41 - transformationMatrix.M41, effectMatrix.M42 - transformationMatrix.M42) / new Vector2(transformationMatrix.M11, transformationMatrix.M22);
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06002044 RID: 8260 RVA: 0x005212AA File Offset: 0x0051F4AA
		public static Vector2 UnscaledScreenSize
		{
			get
			{
				return new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / Main.GameViewMatrix.RenderZoom;
			}
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x005212CC File Offset: 0x0051F4CC
		private void CheckCachedParameters()
		{
			if (this._effect != null && this._effect == base.Shader)
			{
				return;
			}
			this._effect = base.Shader;
			this.uColor = base.Shader.GetParameter("uColor");
			this.uOpacity = base.Shader.GetParameter("uOpacity");
			this.uSecondaryColor = base.Shader.GetParameter("uSecondaryColor");
			this.uTime = base.Shader.GetParameter("uTime");
			this.uScreenResolution = base.Shader.GetParameter("uScreenResolution");
			this.uScreenPosition = base.Shader.GetParameter("uScreenPosition");
			this.uTargetPosition = base.Shader.GetParameter("uTargetPosition");
			this.uImageOffset = base.Shader.GetParameter("uImageOffset");
			this.uSceneSize = base.Shader.GetParameter("uSceneSize");
			this.uSceneOffset = base.Shader.GetParameter("uSceneOffset");
			this.uIntensity = base.Shader.GetParameter("uIntensity");
			this.uProgress = base.Shader.GetParameter("uProgress");
			this.uDirection = base.Shader.GetParameter("uDirection");
			this.uZoom = base.Shader.GetParameter("uZoom");
			this.uMultiChunkScene = base.Shader.GetParameter("uMultiChunkScene");
			for (int i = 0; i < this.uImageSize.Length; i++)
			{
				this.uImageSize[i] = base.Shader.GetParameter("uImageSize" + i);
			}
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x0052147C File Offset: 0x0051F67C
		public override void Apply()
		{
			this.CheckCachedParameters();
			Vector2 value = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
			this.uColor.SetValue(this._uColor);
			this.uOpacity.SetValue(this.CombinedOpacity);
			this.uSecondaryColor.SetValue(this._uSecondaryColor);
			this.uTime.SetValue(Main.GlobalTimeWrappedHourly);
			this.uScreenResolution.SetValue(ScreenShaderData.UnscaledScreenSize);
			this.uScreenPosition.SetValue(ScreenShaderData.UnscaledScreenPosition - value);
			this.uTargetPosition.SetValue(this._uTargetPosition - value);
			this.uImageOffset.SetValue(this._uImageOffset);
			this.uSceneSize.SetValue(this._uSceneSize);
			this.uSceneOffset.SetValue(this._uSceneOffset);
			this.uIntensity.SetValue(this._uIntensity);
			this.uProgress.SetValue(this._uProgress);
			this.uDirection.SetValue(this._uDirection);
			this.uZoom.SetValue(Main.GameViewMatrix.RenderZoom);
			this.uMultiChunkScene.SetValue(ScreenShaderData.MultiChunkCapture);
			this.uImageSize[0].SetValue(this._uImageSize0);
			for (int i = 0; i < this._uAssetImages.Length; i++)
			{
				Texture2D texture2D = this._uCustomImages[i];
				if (this._uAssetImages[i] != null && this._uAssetImages[i].IsLoaded)
				{
					texture2D = this._uAssetImages[i].Value;
				}
				if (texture2D != null)
				{
					Main.graphics.GraphicsDevice.Textures[i + 1] = texture2D;
					int width = texture2D.Width;
					int height = texture2D.Height;
					if (this._samplerStates[i] != null)
					{
						Main.graphics.GraphicsDevice.SamplerStates[i + 1] = this._samplerStates[i];
					}
					else if (Utils.IsPowerOfTwo(width) && Utils.IsPowerOfTwo(height))
					{
						Main.graphics.GraphicsDevice.SamplerStates[i + 1] = SamplerState.LinearWrap;
					}
					else
					{
						Main.graphics.GraphicsDevice.SamplerStates[i + 1] = SamplerState.AnisotropicClamp;
					}
					this.uImageSize[i + 1].SetValue(new Vector2((float)width, (float)height) * this._imageScales[i]);
				}
			}
			base.Apply();
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x005216DC File Offset: 0x0051F8DC
		public ScreenShaderData UseImageOffset(Vector2 offset)
		{
			this._uImageOffset = offset;
			return this;
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x005216E6 File Offset: 0x0051F8E6
		public ScreenShaderData UseIntensity(float intensity)
		{
			this._uIntensity = intensity;
			return this;
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x005216F0 File Offset: 0x0051F8F0
		public ScreenShaderData UseColor(float r, float g, float b)
		{
			return this.UseColor(new Vector3(r, g, b));
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x00521700 File Offset: 0x0051F900
		public ScreenShaderData UseProgress(float progress)
		{
			this._uProgress = progress;
			return this;
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x0052170A File Offset: 0x0051F90A
		public ScreenShaderData UseImage(Texture2D image, int index = 0, SamplerState samplerState = null)
		{
			this._samplerStates[index] = samplerState;
			this._uAssetImages[index] = null;
			this._uCustomImages[index] = image;
			return this;
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x00521728 File Offset: 0x0051F928
		public ScreenShaderData UseImage(string path, int index = 0, SamplerState samplerState = null)
		{
			this._uAssetImages[index] = Main.Assets.Request<Texture2D>(path, 1);
			this._uCustomImages[index] = null;
			this._samplerStates[index] = samplerState;
			return this;
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x00521751 File Offset: 0x0051F951
		public ScreenShaderData UseSceneSize(Vector2 size)
		{
			this._uSceneSize = size;
			return this;
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x0052175B File Offset: 0x0051F95B
		public ScreenShaderData UseSceneOffset(Vector2 size)
		{
			this._uSceneOffset = size;
			return this;
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x00521765 File Offset: 0x0051F965
		public ScreenShaderData UseImageSize0(Vector2 size)
		{
			this._uImageSize0 = size;
			return this;
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x0052176F File Offset: 0x0051F96F
		public ScreenShaderData UseColor(Color color)
		{
			return this.UseColor(color.ToVector3());
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x0052177E File Offset: 0x0051F97E
		public ScreenShaderData UseColor(Vector3 color)
		{
			this._uColor = color;
			return this;
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x00521788 File Offset: 0x0051F988
		public ScreenShaderData UseDirection(Vector2 direction)
		{
			this._uDirection = direction;
			return this;
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x00521792 File Offset: 0x0051F992
		public ScreenShaderData UseGlobalOpacity(float opacity)
		{
			this._globalOpacity = opacity;
			return this;
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x0052179C File Offset: 0x0051F99C
		public ScreenShaderData UseTargetPosition(Vector2 position)
		{
			this._uTargetPosition = position;
			return this;
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x005217A6 File Offset: 0x0051F9A6
		public ScreenShaderData UseSecondaryColor(float r, float g, float b)
		{
			return this.UseSecondaryColor(new Vector3(r, g, b));
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x005217B6 File Offset: 0x0051F9B6
		public ScreenShaderData UseSecondaryColor(Color color)
		{
			return this.UseSecondaryColor(color.ToVector3());
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x005217C5 File Offset: 0x0051F9C5
		public ScreenShaderData UseSecondaryColor(Vector3 color)
		{
			this._uSecondaryColor = color;
			return this;
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x005217CF File Offset: 0x0051F9CF
		public ScreenShaderData UseOpacity(float opacity)
		{
			this._uOpacity = opacity;
			return this;
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x005217D9 File Offset: 0x0051F9D9
		public ScreenShaderData UseImageScale(Vector2 scale, int index = 0)
		{
			this._imageScales[index] = scale;
			return this;
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x00520C92 File Offset: 0x0051EE92
		public virtual ScreenShaderData GetSecondaryShader(Player player)
		{
			return this;
		}

		// Token: 0x04004AA8 RID: 19112
		private Vector3 _uColor = Vector3.One;

		// Token: 0x04004AA9 RID: 19113
		private Vector3 _uSecondaryColor = Vector3.One;

		// Token: 0x04004AAA RID: 19114
		private float _uOpacity = 1f;

		// Token: 0x04004AAB RID: 19115
		private float _globalOpacity = 1f;

		// Token: 0x04004AAC RID: 19116
		private float _uIntensity = 1f;

		// Token: 0x04004AAD RID: 19117
		private Vector2 _uTargetPosition = Vector2.One;

		// Token: 0x04004AAE RID: 19118
		private Vector2 _uDirection = new Vector2(0f, 1f);

		// Token: 0x04004AAF RID: 19119
		private float _uProgress;

		// Token: 0x04004AB0 RID: 19120
		private Vector2 _uImageOffset = Vector2.Zero;

		// Token: 0x04004AB1 RID: 19121
		private Vector2 _uSceneSize;

		// Token: 0x04004AB2 RID: 19122
		private Vector2 _uSceneOffset;

		// Token: 0x04004AB3 RID: 19123
		private Vector2 _uImageSize0;

		// Token: 0x04004AB4 RID: 19124
		private Asset<Texture2D>[] _uAssetImages = new Asset<Texture2D>[3];

		// Token: 0x04004AB5 RID: 19125
		private Texture2D[] _uCustomImages = new Texture2D[3];

		// Token: 0x04004AB6 RID: 19126
		private SamplerState[] _samplerStates = new SamplerState[3];

		// Token: 0x04004AB7 RID: 19127
		private Vector2[] _imageScales = new Vector2[]
		{
			Vector2.One,
			Vector2.One,
			Vector2.One
		};

		// Token: 0x04004AB8 RID: 19128
		public static bool MultiChunkCapture;

		// Token: 0x04004AB9 RID: 19129
		private Effect _effect;

		// Token: 0x04004ABA RID: 19130
		private ShaderData.EffectParameter<Vector3> uColor;

		// Token: 0x04004ABB RID: 19131
		private ShaderData.EffectParameter<float> uOpacity;

		// Token: 0x04004ABC RID: 19132
		private ShaderData.EffectParameter<Vector3> uSecondaryColor;

		// Token: 0x04004ABD RID: 19133
		private ShaderData.EffectParameter<float> uTime;

		// Token: 0x04004ABE RID: 19134
		private ShaderData.EffectParameter<Vector2> uScreenResolution;

		// Token: 0x04004ABF RID: 19135
		private ShaderData.EffectParameter<Vector2> uScreenPosition;

		// Token: 0x04004AC0 RID: 19136
		private ShaderData.EffectParameter<Vector2> uTargetPosition;

		// Token: 0x04004AC1 RID: 19137
		private ShaderData.EffectParameter<Vector2> uImageOffset;

		// Token: 0x04004AC2 RID: 19138
		private ShaderData.EffectParameter<Vector2> uSceneSize;

		// Token: 0x04004AC3 RID: 19139
		private ShaderData.EffectParameter<Vector2> uSceneOffset;

		// Token: 0x04004AC4 RID: 19140
		private ShaderData.EffectParameter<float> uIntensity;

		// Token: 0x04004AC5 RID: 19141
		private ShaderData.EffectParameter<float> uProgress;

		// Token: 0x04004AC6 RID: 19142
		private ShaderData.EffectParameter<Vector2> uDirection;

		// Token: 0x04004AC7 RID: 19143
		private ShaderData.EffectParameter<Vector2> uZoom;

		// Token: 0x04004AC8 RID: 19144
		private ShaderData.EffectParameter<Vector2>[] uImageSize = new ShaderData.EffectParameter<Vector2>[4];

		// Token: 0x04004AC9 RID: 19145
		private ShaderData.EffectParameter<bool> uMultiChunkScene;
	}
}
