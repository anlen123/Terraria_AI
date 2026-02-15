using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics
{
	// Token: 0x020001CC RID: 460
	public struct FinalFractalHelper
	{
		// Token: 0x06001F60 RID: 8032 RVA: 0x0051AC98 File Offset: 0x00518E98
		public static int GetRandomProfileIndex()
		{
			List<int> list = FinalFractalHelper._fractalProfiles.Keys.ToList<int>();
			int index = Main.rand.Next(list.Count);
			if (list[index] == 4956)
			{
				list.RemoveAt(index);
				index = Main.rand.Next(list.Count);
			}
			return list[index];
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x0051ACF4 File Offset: 0x00518EF4
		public void Draw(Projectile proj)
		{
			FinalFractalHelper.FinalFractalProfile finalFractalProfile = FinalFractalHelper.GetFinalFractalProfile((int)proj.ai[1]);
			MiscShaderData miscShaderData = GameShaders.Misc["FinalFractal"];
			int num = 4;
			int num2 = 0;
			int num3 = 0;
			int num4 = 4;
			miscShaderData.UseShaderSpecificData(new Vector4((float)num, (float)num2, (float)num3, (float)num4));
			miscShaderData.UseImage0("Images/Extra_" + 201);
			miscShaderData.UseImage1("Images/Extra_" + 193);
			miscShaderData.Apply(null);
			FinalFractalHelper._vertexStrip.PrepareStrip(proj.oldPos, proj.oldRot, finalFractalProfile.colorMethod, finalFractalProfile.widthMethod, -Main.screenPosition + proj.Size / 2f, new int?(proj.oldPos.Length), true);
			FinalFractalHelper._vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x0051ADF8 File Offset: 0x00518FF8
		public static FinalFractalHelper.FinalFractalProfile GetFinalFractalProfile(int usedSwordId)
		{
			FinalFractalHelper.FinalFractalProfile defaultProfile;
			if (!FinalFractalHelper._fractalProfiles.TryGetValue(usedSwordId, out defaultProfile))
			{
				defaultProfile = FinalFractalHelper._defaultProfile;
			}
			return defaultProfile;
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x0051AE1C File Offset: 0x0051901C
		private Color StripColors(float progressOnStrip)
		{
			Color result = Color.Lerp(Color.White, Color.Violet, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip, false));
			result.A /= 2;
			return result;
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x0051AE77 File Offset: 0x00519077
		private float StripWidth(float progressOnStrip)
		{
			return 50f;
		}

		// Token: 0x040049F1 RID: 18929
		public const int TotalIllusions = 4;

		// Token: 0x040049F2 RID: 18930
		public const int FramesPerImportantTrail = 15;

		// Token: 0x040049F3 RID: 18931
		private static VertexStrip _vertexStrip = new VertexStrip();

		// Token: 0x040049F4 RID: 18932
		private static Dictionary<int, FinalFractalHelper.FinalFractalProfile> _fractalProfiles = new Dictionary<int, FinalFractalHelper.FinalFractalProfile>
		{
			{
				65,
				new FinalFractalHelper.FinalFractalProfile(48f, new Color(236, 62, 192))
			},
			{
				1123,
				new FinalFractalHelper.FinalFractalProfile(48f, Main.OurFavoriteColor)
			},
			{
				46,
				new FinalFractalHelper.FinalFractalProfile(48f, new Color(122, 66, 191))
			},
			{
				121,
				new FinalFractalHelper.FinalFractalProfile(76f, new Color(254, 158, 35))
			},
			{
				190,
				new FinalFractalHelper.FinalFractalProfile(70f, new Color(107, 203, 0))
			},
			{
				368,
				new FinalFractalHelper.FinalFractalProfile(70f, new Color(236, 200, 19))
			},
			{
				674,
				new FinalFractalHelper.FinalFractalProfile(70f, new Color(236, 200, 19))
			},
			{
				273,
				new FinalFractalHelper.FinalFractalProfile(70f, new Color(179, 54, 201))
			},
			{
				675,
				new FinalFractalHelper.FinalFractalProfile(70f, new Color(179, 54, 201))
			},
			{
				2880,
				new FinalFractalHelper.FinalFractalProfile(70f, new Color(84, 234, 245))
			},
			{
				989,
				new FinalFractalHelper.FinalFractalProfile(48f, new Color(91, 158, 232))
			},
			{
				1826,
				new FinalFractalHelper.FinalFractalProfile(76f, new Color(252, 95, 4))
			},
			{
				3063,
				new FinalFractalHelper.FinalFractalProfile(76f, new Color(254, 194, 250))
			},
			{
				3065,
				new FinalFractalHelper.FinalFractalProfile(70f, new Color(237, 63, 133))
			},
			{
				757,
				new FinalFractalHelper.FinalFractalProfile(70f, new Color(80, 222, 122))
			},
			{
				155,
				new FinalFractalHelper.FinalFractalProfile(70f, new Color(56, 78, 210))
			},
			{
				795,
				new FinalFractalHelper.FinalFractalProfile(70f, new Color(237, 28, 36))
			},
			{
				3018,
				new FinalFractalHelper.FinalFractalProfile(80f, new Color(143, 215, 29))
			},
			{
				4144,
				new FinalFractalHelper.FinalFractalProfile(45f, new Color(178, 255, 180))
			},
			{
				3507,
				new FinalFractalHelper.FinalFractalProfile(45f, new Color(235, 166, 135))
			},
			{
				4956,
				new FinalFractalHelper.FinalFractalProfile(86f, new Color(178, 255, 180))
			}
		};

		// Token: 0x040049F5 RID: 18933
		private static FinalFractalHelper.FinalFractalProfile _defaultProfile = new FinalFractalHelper.FinalFractalProfile(50f, Color.White);

		// Token: 0x0200078A RID: 1930
		// (Invoke) Token: 0x0600415A RID: 16730
		public delegate void SpawnDustMethod(Vector2 centerPosition, float rotation, Vector2 velocity);

		// Token: 0x0200078B RID: 1931
		public struct FinalFractalProfile
		{
			// Token: 0x0600415D RID: 16733 RVA: 0x006B8D9C File Offset: 0x006B6F9C
			public FinalFractalProfile(float fullBladeLength, Color color)
			{
				this.trailWidth = fullBladeLength / 2f;
				this.trailColor = color;
				this.widthMethod = null;
				this.colorMethod = null;
				this.dustMethod = null;
				this.widthMethod = new VertexStrip.StripHalfWidthFunction(this.StripWidth);
				this.colorMethod = new VertexStrip.StripColorFunction(this.StripColors);
				this.dustMethod = new FinalFractalHelper.SpawnDustMethod(this.StripDust);
			}

			// Token: 0x0600415E RID: 16734 RVA: 0x006B8E28 File Offset: 0x006B7028
			private void StripDust(Vector2 centerPosition, float rotation, Vector2 velocity)
			{
				if (Main.rand.Next(9) == 0)
				{
					int num = Main.rand.Next(1, 4);
					for (int i = 0; i < num; i++)
					{
						Dust dust = Dust.NewDustPerfect(centerPosition, 278, null, 100, Color.Lerp(this.trailColor, Color.White, Main.rand.NextFloat() * 0.3f), 1f);
						dust.scale = 0.4f;
						dust.fadeIn = 0.4f + Main.rand.NextFloat() * 0.3f;
						dust.noGravity = true;
						dust.velocity += rotation.ToRotationVector2() * (3f + Main.rand.NextFloat() * 4f);
					}
				}
			}

			// Token: 0x0600415F RID: 16735 RVA: 0x006B8F00 File Offset: 0x006B7100
			private Color StripColors(float progressOnStrip)
			{
				Color result = this.trailColor * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip, false));
				result.A /= 2;
				return result;
			}

			// Token: 0x06004160 RID: 16736 RVA: 0x006B8F41 File Offset: 0x006B7141
			private float StripWidth(float progressOnStrip)
			{
				return this.trailWidth;
			}

			// Token: 0x04006FEF RID: 28655
			public float trailWidth;

			// Token: 0x04006FF0 RID: 28656
			public Color trailColor;

			// Token: 0x04006FF1 RID: 28657
			public FinalFractalHelper.SpawnDustMethod dustMethod;

			// Token: 0x04006FF2 RID: 28658
			public VertexStrip.StripColorFunction colorMethod;

			// Token: 0x04006FF3 RID: 28659
			public VertexStrip.StripHalfWidthFunction widthMethod;
		}
	}
}
