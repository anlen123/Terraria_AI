using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics.Effects
{
	// Token: 0x020001ED RID: 493
	public class Filter : GameEffect
	{
		// Token: 0x0600208F RID: 8335 RVA: 0x0052220C File Offset: 0x0052040C
		public Filter(ScreenShaderData shader, EffectPriority priority = EffectPriority.VeryLow)
		{
			this._shader = shader;
			this._priority = priority;
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x00522222 File Offset: 0x00520422
		public void Update(GameTime gameTime)
		{
			this._shader.UseGlobalOpacity(this.Opacity);
			this._shader.Update(gameTime);
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x00522242 File Offset: 0x00520442
		public void Apply(Vector2 textureSize, Vector2 sceneSize, Vector2 sceneOffset)
		{
			this._shader.UseSceneSize(sceneSize).UseSceneOffset(sceneOffset).UseImageSize0(textureSize).Apply();
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x00522261 File Offset: 0x00520461
		public ScreenShaderData GetShader()
		{
			return this._shader;
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x00522269 File Offset: 0x00520469
		public override void Activate(Vector2 position, params object[] args)
		{
			this._shader.UseGlobalOpacity(this.Opacity);
			this._shader.UseTargetPosition(position);
			this.Active = true;
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x00522291 File Offset: 0x00520491
		public override void Deactivate(params object[] args)
		{
			this.Active = false;
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x0052229A File Offset: 0x0052049A
		public bool IsInUse()
		{
			return this.Active || this.Opacity > 0f;
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x005222B3 File Offset: 0x005204B3
		public bool IsActive()
		{
			return this.Active;
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x005222BB File Offset: 0x005204BB
		public override bool IsVisible()
		{
			return this.GetShader().CombinedOpacity > 0f && !this.IsHidden;
		}

		// Token: 0x04004AFC RID: 19196
		public bool Active;

		// Token: 0x04004AFD RID: 19197
		private ScreenShaderData _shader;

		// Token: 0x04004AFE RID: 19198
		public bool IsHidden;
	}
}
