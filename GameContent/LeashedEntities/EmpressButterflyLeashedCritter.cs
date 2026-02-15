using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x02000459 RID: 1113
	public class EmpressButterflyLeashedCritter : FlyLeashedCritter
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x0600326A RID: 12906 RVA: 0x005EF34D File Offset: 0x005ED54D
		private float Opacity
		{
			get
			{
				return Utils.GetLerpValue(60f, 25f, this.fadeAmount, true);
			}
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x005EF365 File Offset: 0x005ED565
		protected override void CopyToDummy()
		{
			base.CopyToDummy();
			LeashedCritter._dummy.ai[2] = this.fadeAmount;
			LeashedCritter._dummy.Opacity = this.Opacity;
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x005EF390 File Offset: 0x005ED590
		protected override void VisualEffects()
		{
			base.VisualEffects();
			Vector3 vector = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.33f % 1f, 1f, 0.5f, byte.MaxValue).ToVector3() * 0.3f;
			vector += Vector3.One * 0.1f;
			Lighting.AddLight(base.Center, vector);
			bool value = Main.LocalPlayer.Center.Distance(base.Center) > 300f;
			this.fadeAmount = MathHelper.Clamp(this.fadeAmount + (float)value.ToDirectionInt(), 0f, 50f);
			if (this.fadeAmount > 0f)
			{
				float opacity = this.Opacity;
				int num = 1;
				for (int i = 0; i < num; i++)
				{
					if (Main.rand.Next(5) == 0)
					{
						float num2 = MathHelper.Lerp(0.9f, 0.6f, opacity);
						Color newColor = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.3f % 1f, 1f, 0.5f, byte.MaxValue) * 0.5f;
						int num3 = Dust.NewDust(this.position, this.width, this.height, 267, 0f, 0f, 0, newColor, 1f);
						Main.dust[num3].position = base.Center + Main.rand.NextVector2Circular((float)this.width, (float)this.height);
						Main.dust[num3].velocity *= Main.rand.NextFloat() * 0.8f;
						Main.dust[num3].velocity += this.velocity * 0.6f;
						Main.dust[num3].noGravity = true;
						Main.dust[num3].fadeIn = 0.6f + Main.rand.NextFloat() * 0.7f * num2;
						Main.dust[num3].scale = 0.35f;
						if (num3 != 6000)
						{
							Dust dust = Dust.CloneDust(num3);
							dust.scale /= 2f;
							dust.fadeIn *= 0.85f;
							dust.color = new Color(255, 255, 255, 255) * 0.5f;
						}
					}
				}
			}
		}

		// Token: 0x040057F9 RID: 22521
		public new static EmpressButterflyLeashedCritter Prototype = new EmpressButterflyLeashedCritter();

		// Token: 0x040057FA RID: 22522
		private float fadeAmount;

		// Token: 0x040057FB RID: 22523
		private const int FadeAwayCap = 50;
	}
}
