using System;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x0200045B RID: 1115
	public class FireflyLeashedCritter : FlyLeashedCritter
	{
		// Token: 0x06003272 RID: 12914 RVA: 0x005EF6FF File Offset: 0x005ED8FF
		protected override void CopyToDummy()
		{
			base.CopyToDummy();
			LeashedCritter._dummy.localAI[2] = (float)(this.lightOn ? 1 : 0);
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x005EF720 File Offset: 0x005ED920
		protected override void VisualEffects()
		{
			base.VisualEffects();
			this.UpdateTimer();
			if (this.lightOn && this.timer > 3)
			{
				this.AddLight();
			}
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x005EF748 File Offset: 0x005ED948
		private void AddLight()
		{
			int i = (int)base.Center.X / 16;
			int j = (int)base.Center.Y / 16;
			float scale = LeashedCritter._dummy.scale;
			int npcType = this.npcType;
			if (npcType == 355)
			{
				Lighting.AddLight(i, j, 0.109500006f * scale, 0.15f * scale, 0.0615f * scale);
				return;
			}
			if (npcType == 358)
			{
				Lighting.AddLight(i, j, 0.10124999f * scale, 0.21374999f * scale, 0.225f * scale);
				return;
			}
			if (npcType != 654)
			{
				return;
			}
			Lighting.AddLight(i, j, 0.225f * scale, 0.105000004f * scale, 0.060000002f * scale);
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x005EF7F8 File Offset: 0x005ED9F8
		private void UpdateTimer()
		{
			int num = this.timer - 1;
			this.timer = num;
			if (num > 0)
			{
				return;
			}
			this.timer = 0;
			if (!this.lightOn && Main.dayTime && (double)(this.position.Y / 16f) < Main.worldSurface + 10.0)
			{
				return;
			}
			this.lightOn = !this.lightOn;
			this.timer = (this.lightOn ? Main.rand.Next(10, 30) : Main.rand.Next(30, 180));
		}

		// Token: 0x040057FD RID: 22525
		public new static FireflyLeashedCritter Prototype = new FireflyLeashedCritter();

		// Token: 0x040057FE RID: 22526
		private bool lightOn;

		// Token: 0x040057FF RID: 22527
		private int timer;
	}
}
