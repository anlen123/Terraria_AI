using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders
{
	// Token: 0x020001E4 RID: 484
	public class ArmorShaderData : ShaderData
	{
		// Token: 0x0600205C RID: 8284 RVA: 0x005217EC File Offset: 0x0051F9EC
		public ArmorShaderData(Asset<Effect> shader, string passName) : base(shader, passName)
		{
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x00521838 File Offset: 0x0051FA38
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
			this.uTargetPosition = base.Shader.GetParameter("uTargetPosition");
			this.uSourceRect = base.Shader.GetParameter("uSourceRect");
			this.uLegacyArmorSourceRect = base.Shader.GetParameter("uLegacyArmorSourceRect");
			this.uLegacyArmorSheetSize = base.Shader.GetParameter("uLegacyArmorSheetSize");
			this.uDrawPosition = base.Shader.GetParameter("uDrawPosition");
			this.uRotation = base.Shader.GetParameter("uRotation");
			this.uDirection = base.Shader.GetParameter("uDirection");
			this.uImageSize0 = base.Shader.GetParameter("uImageSize0");
			this.uImageSize1 = base.Shader.GetParameter("uImageSize1");
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x0052199C File Offset: 0x0051FB9C
		public virtual void Apply(Entity entity, DrawData? drawData = null)
		{
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
				Vector4 value2;
				if (value.sourceRect != null)
				{
					value2 = new Vector4((float)value.sourceRect.Value.X, (float)value.sourceRect.Value.Y, (float)value.sourceRect.Value.Width, (float)value.sourceRect.Value.Height);
				}
				else
				{
					value2 = new Vector4(0f, 0f, (float)value.texture.Width, (float)value.texture.Height);
				}
				this.uSourceRect.SetValue(value2);
				this.uLegacyArmorSourceRect.SetValue(value2);
				this.uDrawPosition.SetValue(value.position);
				this.uImageSize0.SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
				this.uLegacyArmorSheetSize.SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
				this.uRotation.SetValue(value.rotation * (((value.effect & SpriteEffects.FlipHorizontally) != SpriteEffects.None) ? -1f : 1f));
				this.uDirection.SetValue((float)(((value.effect & SpriteEffects.FlipHorizontally) != SpriteEffects.None) ? -1 : 1));
			}
			else
			{
				Vector4 value3 = new Vector4(0f, 0f, 4f, 4f);
				this.uSourceRect.SetValue(value3);
				this.uLegacyArmorSourceRect.SetValue(value3);
				this.uRotation.SetValue(0f);
			}
			if (this._uImage != null)
			{
				Main.graphics.GraphicsDevice.Textures[1] = this._uImage.Value;
				this.uImageSize1.SetValue(new Vector2((float)this._uImage.Width(), (float)this._uImage.Height()));
			}
			if (entity != null)
			{
				this.uDirection.SetValue((float)entity.direction);
			}
			Player player = entity as Player;
			if (player != null)
			{
				Rectangle bodyFrame = player.bodyFrame;
				this.uLegacyArmorSourceRect.SetValue(new Vector4((float)bodyFrame.X, (float)bodyFrame.Y, (float)bodyFrame.Width, (float)bodyFrame.Height));
				this.uLegacyArmorSheetSize.SetValue(new Vector2(40f, 1120f));
			}
			this.Apply();
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x00521C77 File Offset: 0x0051FE77
		public ArmorShaderData UseColor(float r, float g, float b)
		{
			return this.UseColor(new Vector3(r, g, b));
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x00521C87 File Offset: 0x0051FE87
		public ArmorShaderData UseColor(Color color)
		{
			return this.UseColor(color.ToVector3());
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x00521C96 File Offset: 0x0051FE96
		public ArmorShaderData UseColor(Vector3 color)
		{
			this._uColor = color;
			return this;
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x00521CA0 File Offset: 0x0051FEA0
		public ArmorShaderData UseImage(string path)
		{
			if (!Main.dedServ)
			{
				this._uImage = Main.Assets.Request<Texture2D>(path, 1);
			}
			return this;
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x00521CBC File Offset: 0x0051FEBC
		public ArmorShaderData UseOpacity(float alpha)
		{
			this._uOpacity = alpha;
			return this;
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x00521CC6 File Offset: 0x0051FEC6
		public ArmorShaderData UseTargetPosition(Vector2 position)
		{
			this._uTargetPosition = position;
			return this;
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x00521CD0 File Offset: 0x0051FED0
		public ArmorShaderData UseSecondaryColor(float r, float g, float b)
		{
			return this.UseSecondaryColor(new Vector3(r, g, b));
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x00521CE0 File Offset: 0x0051FEE0
		public ArmorShaderData UseSecondaryColor(Color color)
		{
			return this.UseSecondaryColor(color.ToVector3());
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x00521CEF File Offset: 0x0051FEEF
		public ArmorShaderData UseSecondaryColor(Vector3 color)
		{
			this._uSecondaryColor = color;
			return this;
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x00521CF9 File Offset: 0x0051FEF9
		public ArmorShaderData UseSaturation(float saturation)
		{
			this._uSaturation = saturation;
			return this;
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x00520C92 File Offset: 0x0051EE92
		public virtual ArmorShaderData GetSecondaryShader(Entity entity)
		{
			return this;
		}

		// Token: 0x04004ACA RID: 19146
		private Vector3 _uColor = Vector3.One;

		// Token: 0x04004ACB RID: 19147
		private Vector3 _uSecondaryColor = Vector3.One;

		// Token: 0x04004ACC RID: 19148
		private float _uSaturation = 1f;

		// Token: 0x04004ACD RID: 19149
		private float _uOpacity = 1f;

		// Token: 0x04004ACE RID: 19150
		private Asset<Texture2D> _uImage;

		// Token: 0x04004ACF RID: 19151
		private Vector2 _uTargetPosition = Vector2.One;

		// Token: 0x04004AD0 RID: 19152
		private Effect _effect;

		// Token: 0x04004AD1 RID: 19153
		private ShaderData.EffectParameter<Vector3> uColor;

		// Token: 0x04004AD2 RID: 19154
		private ShaderData.EffectParameter<float> uSaturation;

		// Token: 0x04004AD3 RID: 19155
		private ShaderData.EffectParameter<Vector3> uSecondaryColor;

		// Token: 0x04004AD4 RID: 19156
		private ShaderData.EffectParameter<float> uTime;

		// Token: 0x04004AD5 RID: 19157
		private ShaderData.EffectParameter<float> uOpacity;

		// Token: 0x04004AD6 RID: 19158
		private ShaderData.EffectParameter<Vector2> uTargetPosition;

		// Token: 0x04004AD7 RID: 19159
		private ShaderData.EffectParameter<Vector4> uSourceRect;

		// Token: 0x04004AD8 RID: 19160
		private ShaderData.EffectParameter<Vector4> uLegacyArmorSourceRect;

		// Token: 0x04004AD9 RID: 19161
		private ShaderData.EffectParameter<Vector2> uLegacyArmorSheetSize;

		// Token: 0x04004ADA RID: 19162
		private ShaderData.EffectParameter<Vector2> uDrawPosition;

		// Token: 0x04004ADB RID: 19163
		private ShaderData.EffectParameter<float> uRotation;

		// Token: 0x04004ADC RID: 19164
		private ShaderData.EffectParameter<float> uDirection;

		// Token: 0x04004ADD RID: 19165
		private ShaderData.EffectParameter<Vector2> uImageSize0;

		// Token: 0x04004ADE RID: 19166
		private ShaderData.EffectParameter<Vector2> uImageSize1;
	}
}
