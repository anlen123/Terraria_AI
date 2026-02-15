using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace Terraria
{
	// Token: 0x02000034 RID: 52
	public class CombatText
	{
		// Token: 0x06000316 RID: 790 RVA: 0x00043B60 File Offset: 0x00041D60
		public static int NewText(Rectangle location, Color color, int amount, bool dramatic = false, bool dot = false)
		{
			return CombatText.NewText(location, color, amount.ToString(), dramatic, dot);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00043B74 File Offset: 0x00041D74
		public static int NewText(Rectangle location, Color color, string text, bool dramatic = false, bool dot = false)
		{
			if (Main.netMode == 2)
			{
				return 100;
			}
			for (int i = 0; i < 100; i++)
			{
				if (!Main.combatText[i].active)
				{
					int num = 0;
					if (dramatic)
					{
						num = 1;
					}
					Vector2 vector = FontAssets.CombatText[num].Value.MeasureString(text);
					if (Main.NoPooling)
					{
						Main.combatText[i] = new CombatText();
					}
					Main.combatText[i].alpha = 1f;
					Main.combatText[i].alphaDir = -1;
					Main.combatText[i].active = true;
					Main.combatText[i].scale = 0f;
					Main.combatText[i].rotation = 0f;
					Main.combatText[i].position.X = (float)location.X + (float)location.Width * 0.5f - vector.X * 0.5f;
					Main.combatText[i].position.Y = (float)location.Y + (float)location.Height * 0.25f - vector.Y * 0.5f;
					CombatText combatText = Main.combatText[i];
					combatText.position.X = combatText.position.X + (float)Main.rand.Next(-(int)((double)location.Width * 0.5), (int)((double)location.Width * 0.5) + 1);
					CombatText combatText2 = Main.combatText[i];
					combatText2.position.Y = combatText2.position.Y + (float)Main.rand.Next(-(int)((double)location.Height * 0.5), (int)((double)location.Height * 0.5) + 1);
					Main.combatText[i].color = color;
					Main.combatText[i].text = text;
					Main.combatText[i].velocity.Y = -7f;
					if (Main.player[Main.myPlayer].gravDir == -1f)
					{
						CombatText combatText3 = Main.combatText[i];
						combatText3.velocity.Y = combatText3.velocity.Y * -1f;
						Main.combatText[i].position.Y = (float)location.Y + (float)location.Height * 0.75f + vector.Y * 0.5f;
					}
					Main.combatText[i].lifeTime = 60;
					Main.combatText[i].crit = dramatic;
					Main.combatText[i].dot = dot;
					if (dramatic)
					{
						Main.combatText[i].text = text;
						Main.combatText[i].lifeTime *= 2;
						CombatText combatText4 = Main.combatText[i];
						combatText4.velocity.Y = combatText4.velocity.Y * 2f;
						Main.combatText[i].velocity.X = (float)Main.rand.Next(-25, 26) * 0.05f;
						Main.combatText[i].rotation = (float)(Main.combatText[i].lifeTime / 2) * 0.002f;
						if (Main.combatText[i].velocity.X < 0f)
						{
							Main.combatText[i].rotation *= -1f;
						}
					}
					if (dot)
					{
						Main.combatText[i].velocity.Y = -4f;
						Main.combatText[i].lifeTime = 40;
					}
					return i;
				}
			}
			return 100;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00043EC0 File Offset: 0x000420C0
		public static void clearAll()
		{
			for (int i = 0; i < 100; i++)
			{
				Main.combatText[i].active = false;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00043EE7 File Offset: 0x000420E7
		public static float TargetScale
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00043EF0 File Offset: 0x000420F0
		public void Update()
		{
			if (this.active)
			{
				float targetScale = CombatText.TargetScale;
				this.alpha += (float)this.alphaDir * 0.05f;
				if ((double)this.alpha <= 0.6)
				{
					this.alphaDir = 1;
				}
				if (this.alpha >= 1f)
				{
					this.alpha = 1f;
					this.alphaDir = -1;
				}
				if (this.dot)
				{
					this.velocity.Y = this.velocity.Y + 0.15f;
				}
				else
				{
					this.velocity.Y = this.velocity.Y * 0.92f;
					if (this.crit)
					{
						this.velocity.Y = this.velocity.Y * 0.92f;
					}
				}
				this.velocity.X = this.velocity.X * 0.93f;
				this.position += this.velocity;
				this.lifeTime--;
				if (this.lifeTime <= 0)
				{
					this.scale -= 0.1f * targetScale;
					if ((double)this.scale < 0.1)
					{
						this.active = false;
					}
					this.lifeTime = 0;
					if (this.crit)
					{
						this.alphaDir = -1;
						this.scale += 0.07f * targetScale;
						return;
					}
				}
				else
				{
					if (this.crit)
					{
						if (this.velocity.X < 0f)
						{
							this.rotation += 0.001f;
						}
						else
						{
							this.rotation -= 0.001f;
						}
					}
					if (this.dot)
					{
						this.scale += 0.5f * targetScale;
						if ((double)this.scale > 0.8 * (double)targetScale)
						{
							this.scale = 0.8f * targetScale;
							return;
						}
					}
					else
					{
						if (this.scale < targetScale)
						{
							this.scale += 0.1f * targetScale;
						}
						if (this.scale > targetScale)
						{
							this.scale = targetScale;
						}
					}
				}
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x000440F8 File Offset: 0x000422F8
		public static void UpdateCombatText()
		{
			for (int i = 0; i < 100; i++)
			{
				if (Main.combatText[i].active)
				{
					Main.combatText[i].Update();
				}
			}
		}

		// Token: 0x0400023B RID: 571
		public static readonly Color DamagedFriendly = new Color(255, 80, 90, 255);

		// Token: 0x0400023C RID: 572
		public static readonly Color DamagedFriendlyCrit = new Color(255, 100, 30, 255);

		// Token: 0x0400023D RID: 573
		public static readonly Color DamagedHostile = new Color(255, 160, 80, 255);

		// Token: 0x0400023E RID: 574
		public static readonly Color DamagedHostileCrit = new Color(255, 100, 30, 255);

		// Token: 0x0400023F RID: 575
		public static readonly Color OthersDamagedHostile = CombatText.DamagedHostile * 0.4f;

		// Token: 0x04000240 RID: 576
		public static readonly Color OthersDamagedHostileCrit = CombatText.DamagedHostileCrit * 0.4f;

		// Token: 0x04000241 RID: 577
		public static readonly Color HealLife = new Color(100, 255, 100, 255);

		// Token: 0x04000242 RID: 578
		public static readonly Color HealMana = new Color(100, 100, 255, 255);

		// Token: 0x04000243 RID: 579
		public static readonly Color LifeRegen = new Color(255, 60, 70, 255);

		// Token: 0x04000244 RID: 580
		public static readonly Color LifeRegenNegative = new Color(255, 140, 40, 255);

		// Token: 0x04000245 RID: 581
		public Vector2 position;

		// Token: 0x04000246 RID: 582
		public Vector2 velocity;

		// Token: 0x04000247 RID: 583
		public float alpha;

		// Token: 0x04000248 RID: 584
		public int alphaDir = 1;

		// Token: 0x04000249 RID: 585
		public string text = "";

		// Token: 0x0400024A RID: 586
		public float scale = 1f;

		// Token: 0x0400024B RID: 587
		public float rotation;

		// Token: 0x0400024C RID: 588
		public Color color;

		// Token: 0x0400024D RID: 589
		public bool active;

		// Token: 0x0400024E RID: 590
		public int lifeTime;

		// Token: 0x0400024F RID: 591
		public bool crit;

		// Token: 0x04000250 RID: 592
		public bool dot;
	}
}
