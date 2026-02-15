using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders
{
	// Token: 0x020001E2 RID: 482
	public class HairShaderData : ShaderData
	{
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x0600202F RID: 8239 RVA: 0x00520C9F File Offset: 0x0051EE9F
		public bool ShaderDisabled
		{
			get
			{
				return this._shaderDisabled;
			}
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x00520CA8 File Offset: 0x0051EEA8
		public HairShaderData(Asset<Effect> shader, string passName) : base(shader, passName)
		{
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x00520CF4 File Offset: 0x0051EEF4
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
			this.uDirection = base.Shader.GetParameter("uDirection");
			this.uSourceRect = base.Shader.GetParameter("uSourceRect");
			this.uDrawPosition = base.Shader.GetParameter("uDrawPosition");
			this.uTargetPosition = base.Shader.GetParameter("uTargetPosition");
			this.uImageSize0 = base.Shader.GetParameter("uImageSize0");
			this.uImageSize1 = base.Shader.GetParameter("uImageSize1");
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x00520E18 File Offset: 0x0051F018
		public virtual void Apply(Player player, DrawData? drawData = null)
		{
			if (this._shaderDisabled)
			{
				return;
			}
			this.CheckCachedParameters();
			this.uColor.SetValue(this._uColor);
			this.uSaturation.SetValue(this._uSaturation);
			this.uSecondaryColor.SetValue(this._uSecondaryColor);
			this.uTime.SetValue(Main.GlobalTimeWrappedHourly);
			this.uOpacity.SetValue(this._uOpacity);
			this.uTargetPosition.SetValue(this._uTargetPosition);
			if (drawData != null)
			{
				DrawData value = drawData.Value;
				Vector4 value2 = new Vector4((float)value.sourceRect.Value.X, (float)value.sourceRect.Value.Y, (float)value.sourceRect.Value.Width, (float)value.sourceRect.Value.Height);
				this.uSourceRect.SetValue(value2);
				this.uDrawPosition.SetValue(value.position);
				this.uImageSize0.SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
			}
			else
			{
				this.uSourceRect.SetValue(new Vector4(0f, 0f, 4f, 4f));
			}
			if (this._uImage != null)
			{
				Main.graphics.GraphicsDevice.Textures[1] = this._uImage.Value;
				this.uImageSize1.SetValue(new Vector2((float)this._uImage.Width(), (float)this._uImage.Height()));
			}
			if (player != null)
			{
				this.uDirection.SetValue((float)player.direction);
			}
			this.Apply();
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x00520FD2 File Offset: 0x0051F1D2
		public virtual Color GetColor(Player player, Color lightColor)
		{
			return new Color(lightColor.ToVector4() * player.hairColor.ToVector4());
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x00520FF0 File Offset: 0x0051F1F0
		public HairShaderData UseColor(float r, float g, float b)
		{
			return this.UseColor(new Vector3(r, g, b));
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x00521000 File Offset: 0x0051F200
		public HairShaderData UseColor(Color color)
		{
			return this.UseColor(color.ToVector3());
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x0052100F File Offset: 0x0051F20F
		public HairShaderData UseColor(Vector3 color)
		{
			this._uColor = color;
			return this;
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x00521019 File Offset: 0x0051F219
		public HairShaderData UseImage(string path)
		{
			if (!Main.dedServ)
			{
				this._uImage = Main.Assets.Request<Texture2D>(path, 1);
			}
			return this;
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x00521035 File Offset: 0x0051F235
		public HairShaderData UseOpacity(float alpha)
		{
			this._uOpacity = alpha;
			return this;
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x0052103F File Offset: 0x0051F23F
		public HairShaderData UseSecondaryColor(float r, float g, float b)
		{
			return this.UseSecondaryColor(new Vector3(r, g, b));
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x0052104F File Offset: 0x0051F24F
		public HairShaderData UseSecondaryColor(Color color)
		{
			return this.UseSecondaryColor(color.ToVector3());
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x0052105E File Offset: 0x0051F25E
		public HairShaderData UseSecondaryColor(Vector3 color)
		{
			this._uSecondaryColor = color;
			return this;
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00521068 File Offset: 0x0051F268
		public HairShaderData UseSaturation(float saturation)
		{
			this._uSaturation = saturation;
			return this;
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x00521072 File Offset: 0x0051F272
		public HairShaderData UseTargetPosition(Vector2 position)
		{
			this._uTargetPosition = position;
			return this;
		}

		// Token: 0x04004A95 RID: 19093
		protected Vector3 _uColor = Vector3.One;

		// Token: 0x04004A96 RID: 19094
		protected Vector3 _uSecondaryColor = Vector3.One;

		// Token: 0x04004A97 RID: 19095
		protected float _uSaturation = 1f;

		// Token: 0x04004A98 RID: 19096
		protected float _uOpacity = 1f;

		// Token: 0x04004A99 RID: 19097
		protected Asset<Texture2D> _uImage;

		// Token: 0x04004A9A RID: 19098
		protected bool _shaderDisabled;

		// Token: 0x04004A9B RID: 19099
		private Vector2 _uTargetPosition = Vector2.One;

		// Token: 0x04004A9C RID: 19100
		private Effect _effect;

		// Token: 0x04004A9D RID: 19101
		private ShaderData.EffectParameter<Vector3> uColor;

		// Token: 0x04004A9E RID: 19102
		private ShaderData.EffectParameter<float> uSaturation;

		// Token: 0x04004A9F RID: 19103
		private ShaderData.EffectParameter<Vector3> uSecondaryColor;

		// Token: 0x04004AA0 RID: 19104
		private ShaderData.EffectParameter<float> uTime;

		// Token: 0x04004AA1 RID: 19105
		private ShaderData.EffectParameter<float> uOpacity;

		// Token: 0x04004AA2 RID: 19106
		private ShaderData.EffectParameter<float> uDirection;

		// Token: 0x04004AA3 RID: 19107
		private ShaderData.EffectParameter<Vector4> uSourceRect;

		// Token: 0x04004AA4 RID: 19108
		private ShaderData.EffectParameter<Vector2> uDrawPosition;

		// Token: 0x04004AA5 RID: 19109
		private ShaderData.EffectParameter<Vector2> uTargetPosition;

		// Token: 0x04004AA6 RID: 19110
		private ShaderData.EffectParameter<Vector2> uImageSize0;

		// Token: 0x04004AA7 RID: 19111
		private ShaderData.EffectParameter<Vector2> uImageSize1;
	}
}
