using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003CF RID: 975
	public class UIParticleLayer : UIElement
	{
		// Token: 0x06002D8C RID: 11660 RVA: 0x005A41DD File Offset: 0x005A23DD
		public UIParticleLayer()
		{
			this.IgnoresMouseInteraction = true;
			this.ParticleSystem = new ParticleRenderer();
			base.OnUpdate += this.ParticleSystemUpdate;
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x005A4209 File Offset: 0x005A2409
		private void ParticleSystemUpdate(UIElement affectedElement)
		{
			this.ParticleSystem.Update();
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x005A4218 File Offset: 0x005A2418
		public override void Recalculate()
		{
			base.Recalculate();
			Rectangle r = base.GetDimensions().ToRectangle();
			this.ParticleSystem.Settings.AnchorPosition = r.TopLeft() + this.AnchorPositionOffsetByPercents * r.Size() + this.AnchorPositionOffsetByPixels;
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x005A4271 File Offset: 0x005A2471
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			this.ParticleSystem.Draw(spriteBatch);
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x005A427F File Offset: 0x005A247F
		public void AddParticle(IParticle particle)
		{
			this.ParticleSystem.Add(particle);
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x005A428D File Offset: 0x005A248D
		public void ClearParticles()
		{
			this.ParticleSystem.Clear();
		}

		// Token: 0x040054C0 RID: 21696
		public ParticleRenderer ParticleSystem;

		// Token: 0x040054C1 RID: 21697
		public Vector2 AnchorPositionOffsetByPercents;

		// Token: 0x040054C2 RID: 21698
		public Vector2 AnchorPositionOffsetByPixels;
	}
}
