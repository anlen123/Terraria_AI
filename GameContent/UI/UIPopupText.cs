using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.UI
{
	// Token: 0x0200036D RID: 877
	public class UIPopupText
	{
		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06002920 RID: 10528 RVA: 0x00043EE7 File Offset: 0x000420E7
		public float TargetScale
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x005781FE File Offset: 0x005763FE
		public void PrepareDisplayText()
		{
			this.displayText = this.name;
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x0057820C File Offset: 0x0057640C
		public void Update(int whoAmI, UIPopupTextManager manager)
		{
			if (this.active)
			{
				this.framesSinceSpawn++;
				float targetScale = this.TargetScale;
				this.alpha += (float)this.alphaDir * 0.01f;
				if ((double)this.alpha <= 0.7)
				{
					this.alpha = 0.7f;
					this.alphaDir = 1;
				}
				if (this.alpha >= 1f)
				{
					this.alpha = 1f;
					this.alphaDir = -1;
				}
				bool flag = false;
				Vector2 textHitbox = this.GetTextHitbox();
				Rectangle rectangle = new Rectangle((int)(this.position.X - textHitbox.X / 2f), (int)(this.position.Y - textHitbox.Y / 2f), (int)textHitbox.X, (int)textHitbox.Y);
				for (int i = 0; i < 20; i++)
				{
					UIPopupText uipopupText = manager.popupText[i];
					if (uipopupText.active && i != whoAmI)
					{
						Vector2 textHitbox2 = uipopupText.GetTextHitbox();
						Rectangle value = new Rectangle((int)(uipopupText.position.X - textHitbox2.X / 2f), (int)(uipopupText.position.Y - textHitbox2.Y / 2f), (int)textHitbox2.X, (int)textHitbox2.Y);
						if (rectangle.Intersects(value) && (this.position.Y < uipopupText.position.Y || (this.position.Y == uipopupText.position.Y && whoAmI < i)))
						{
							flag = true;
							int num = manager.numActive;
							if (num > 3)
							{
								num = 3;
							}
							uipopupText.lifeTime = UIPopupText.activeTime + 15 * num;
							this.lifeTime = UIPopupText.activeTime + 15 * num;
						}
					}
				}
				if (!flag)
				{
					if (this.context != UIPopupTextContext.SpecialSeed || (this.scale != targetScale && this.lifeTime > 0))
					{
						this.velocity.Y = this.velocity.Y * 0.86f;
						if (this.scale == targetScale)
						{
							this.velocity.Y = this.velocity.Y * 0.4f;
						}
					}
				}
				else if (this.velocity.Y > -6f)
				{
					this.velocity.Y = this.velocity.Y - 0.2f;
				}
				else
				{
					this.velocity.Y = this.velocity.Y * 0.86f;
				}
				this.velocity.X = this.velocity.X * 0.93f;
				this.position += this.velocity;
				this.lifeTime--;
				if (this.lifeTime <= 0)
				{
					this.scale -= 0.03f * targetScale;
					if ((double)this.scale < 0.1 * (double)targetScale)
					{
						this.active = false;
					}
					this.lifeTime = 0;
					return;
				}
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

		// Token: 0x06002923 RID: 10531 RVA: 0x0057851C File Offset: 0x0057671C
		public Vector2 GetTextHitbox()
		{
			string text = this.displayText;
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(text);
			vector *= this.scale;
			vector.Y *= 0.8f;
			return vector;
		}

		// Token: 0x040051A5 RID: 20901
		public Vector2 position;

		// Token: 0x040051A6 RID: 20902
		public Vector2 velocity;

		// Token: 0x040051A7 RID: 20903
		public float alpha;

		// Token: 0x040051A8 RID: 20904
		public int alphaDir = 1;

		// Token: 0x040051A9 RID: 20905
		public string name;

		// Token: 0x040051AA RID: 20906
		public string displayText;

		// Token: 0x040051AB RID: 20907
		public float scale = 1f;

		// Token: 0x040051AC RID: 20908
		public float rotation;

		// Token: 0x040051AD RID: 20909
		public Color color;

		// Token: 0x040051AE RID: 20910
		public bool active;

		// Token: 0x040051AF RID: 20911
		public int lifeTime;

		// Token: 0x040051B0 RID: 20912
		public int framesSinceSpawn;

		// Token: 0x040051B1 RID: 20913
		public static int activeTime = 60;

		// Token: 0x040051B2 RID: 20914
		public UIPopupTextContext context;
	}
}
