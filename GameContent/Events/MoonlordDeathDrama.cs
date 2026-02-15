using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;

namespace Terraria.GameContent.Events
{
	// Token: 0x020004FC RID: 1276
	public class MoonlordDeathDrama
	{
		// Token: 0x060035AF RID: 13743 RVA: 0x0061C148 File Offset: 0x0061A348
		public static void Update(SceneState sceneState, SceneMetrics metrics)
		{
			for (int i = 0; i < MoonlordDeathDrama._pieces.Count; i++)
			{
				MoonlordDeathDrama.MoonlordPiece moonlordPiece = MoonlordDeathDrama._pieces[i];
				moonlordPiece.Update();
				if (moonlordPiece.Dead)
				{
					MoonlordDeathDrama._pieces.Remove(moonlordPiece);
					i--;
				}
			}
			for (int j = 0; j < MoonlordDeathDrama._explosions.Count; j++)
			{
				MoonlordDeathDrama.MoonlordExplosion moonlordExplosion = MoonlordDeathDrama._explosions[j];
				moonlordExplosion.Update();
				if (moonlordExplosion.Dead)
				{
					MoonlordDeathDrama._explosions.Remove(moonlordExplosion);
					j--;
				}
			}
			bool flag = false;
			for (int k = 0; k < MoonlordDeathDrama._lightSources.Count; k++)
			{
				if (metrics.Center.Distance(MoonlordDeathDrama._lightSources[k]) < 2000f)
				{
					flag = true;
					break;
				}
			}
			MoonlordDeathDrama._lightSources.Clear();
			if (!flag)
			{
				MoonlordDeathDrama.requestedLight = 0f;
			}
			sceneState.MoveTowards(ref MoonlordDeathDrama.whitening, MoonlordDeathDrama.requestedLight, 0.02f);
			MoonlordDeathDrama.requestedLight = 0f;
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x0061C24C File Offset: 0x0061A44C
		public static void DrawPieces(SpriteBatch spriteBatch)
		{
			Rectangle playerScreen = Utils.CenteredRectangle(Main.screenPosition + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) * 0.5f, new Vector2((float)(Main.screenWidth + 1000), (float)(Main.screenHeight + 1000)));
			for (int i = 0; i < MoonlordDeathDrama._pieces.Count; i++)
			{
				if (MoonlordDeathDrama._pieces[i].InDrawRange(playerScreen))
				{
					MoonlordDeathDrama._pieces[i].Draw(spriteBatch);
				}
			}
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x0061C2DC File Offset: 0x0061A4DC
		public static void DrawExplosions(SpriteBatch spriteBatch)
		{
			Rectangle playerScreen = Utils.CenteredRectangle(Main.screenPosition + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) * 0.5f, new Vector2((float)(Main.screenWidth + 1000), (float)(Main.screenHeight + 1000)));
			for (int i = 0; i < MoonlordDeathDrama._explosions.Count; i++)
			{
				if (MoonlordDeathDrama._explosions[i].InDrawRange(playerScreen))
				{
					MoonlordDeathDrama._explosions[i].Draw(spriteBatch);
				}
			}
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x0061C36C File Offset: 0x0061A56C
		public static void DrawWhite(SpriteBatch spriteBatch)
		{
			if (MoonlordDeathDrama.whitening == 0f)
			{
				return;
			}
			Color color = Color.White * MoonlordDeathDrama.whitening;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x0061C3CC File Offset: 0x0061A5CC
		public static void ThrowPieces(Vector2 MoonlordCoreCenter, int DramaSeed)
		{
			UnifiedRandom r = new UnifiedRandom(DramaSeed);
			Vector2 value = Vector2.UnitY.RotatedBy((double)(r.NextFloat() * 1.5707964f - 0.7853982f + 3.1415927f), default(Vector2));
			MoonlordDeathDrama._pieces.Add(new MoonlordDeathDrama.MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Spine", 1).Value, new Vector2(64f, 150f), MoonlordCoreCenter + new Vector2(0f, 50f), value * 6f, 0f, r.NextFloat() * 0.1f - 0.05f));
			value = Vector2.UnitY.RotatedBy((double)(r.NextFloat() * 1.5707964f - 0.7853982f + 3.1415927f), default(Vector2));
			MoonlordDeathDrama._pieces.Add(new MoonlordDeathDrama.MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Shoulder", 1).Value, new Vector2(40f, 120f), MoonlordCoreCenter + new Vector2(50f, -120f), value * 10f, 0f, r.NextFloat() * 0.1f - 0.05f));
			value = Vector2.UnitY.RotatedBy((double)(r.NextFloat() * 1.5707964f - 0.7853982f + 3.1415927f), default(Vector2));
			MoonlordDeathDrama._pieces.Add(new MoonlordDeathDrama.MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Torso", 1).Value, new Vector2(192f, 252f), MoonlordCoreCenter, value * 8f, 0f, r.NextFloat() * 0.1f - 0.05f));
			value = Vector2.UnitY.RotatedBy((double)(r.NextFloat() * 1.5707964f - 0.7853982f + 3.1415927f), default(Vector2));
			MoonlordDeathDrama._pieces.Add(new MoonlordDeathDrama.MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Head", 1).Value, new Vector2(138f, 185f), MoonlordCoreCenter - new Vector2(0f, 200f), value * 12f, 0f, r.NextFloat() * 0.1f - 0.05f));
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x0061C628 File Offset: 0x0061A828
		public static void AddExplosion(Vector2 spot)
		{
			MoonlordDeathDrama._explosions.Add(new MoonlordDeathDrama.MoonlordExplosion(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Explosion", 1).Value, spot, Main.rand.Next(2, 4)));
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x0061C65B File Offset: 0x0061A85B
		public static void RequestLight(float light, Vector2 spot)
		{
			MoonlordDeathDrama._lightSources.Add(spot);
			if (light > 1f)
			{
				light = 1f;
			}
			if (MoonlordDeathDrama.requestedLight < light)
			{
				MoonlordDeathDrama.requestedLight = light;
			}
		}

		// Token: 0x04005AB4 RID: 23220
		private static List<MoonlordDeathDrama.MoonlordPiece> _pieces = new List<MoonlordDeathDrama.MoonlordPiece>();

		// Token: 0x04005AB5 RID: 23221
		private static List<MoonlordDeathDrama.MoonlordExplosion> _explosions = new List<MoonlordDeathDrama.MoonlordExplosion>();

		// Token: 0x04005AB6 RID: 23222
		private static List<Vector2> _lightSources = new List<Vector2>();

		// Token: 0x04005AB7 RID: 23223
		private static float whitening;

		// Token: 0x04005AB8 RID: 23224
		private static float requestedLight;

		// Token: 0x0200098A RID: 2442
		public class MoonlordPiece
		{
			// Token: 0x06004961 RID: 18785 RVA: 0x006D071F File Offset: 0x006CE91F
			public MoonlordPiece(Texture2D pieceTexture, Vector2 textureOrigin, Vector2 centerPos, Vector2 velocity, float rot, float angularVelocity)
			{
				this._texture = pieceTexture;
				this._origin = textureOrigin;
				this._position = centerPos;
				this._velocity = velocity;
				this._rotation = rot;
				this._rotationVelocity = angularVelocity;
			}

			// Token: 0x06004962 RID: 18786 RVA: 0x006D0754 File Offset: 0x006CE954
			public void Update()
			{
				this._velocity.Y = this._velocity.Y + 0.3f;
				this._rotation += this._rotationVelocity;
				this._rotationVelocity *= 0.99f;
				this._position += this._velocity;
			}

			// Token: 0x06004963 RID: 18787 RVA: 0x006D07B4 File Offset: 0x006CE9B4
			public void Draw(SpriteBatch sp)
			{
				Color light = this.GetLight();
				sp.Draw(this._texture, this._position - Main.screenPosition, null, light, this._rotation, this._origin, 1f, SpriteEffects.None, 0f);
			}

			// Token: 0x17000591 RID: 1425
			// (get) Token: 0x06004964 RID: 18788 RVA: 0x006D0808 File Offset: 0x006CEA08
			public bool Dead
			{
				get
				{
					return this._position.Y > (float)(Main.maxTilesY * 16) - 480f || this._position.X < 480f || this._position.X >= (float)(Main.maxTilesX * 16) - 480f;
				}
			}

			// Token: 0x06004965 RID: 18789 RVA: 0x006D0864 File Offset: 0x006CEA64
			public bool InDrawRange(Rectangle playerScreen)
			{
				return playerScreen.Contains(this._position.ToPoint());
			}

			// Token: 0x06004966 RID: 18790 RVA: 0x006D0878 File Offset: 0x006CEA78
			public Color GetLight()
			{
				Vector3 vector = Vector3.Zero;
				float num = 0f;
				int num2 = 5;
				Point point = this._position.ToTileCoordinates();
				for (int i = point.X - num2; i <= point.X + num2; i++)
				{
					for (int j = point.Y - num2; j <= point.Y + num2; j++)
					{
						vector += Lighting.GetColor(i, j).ToVector3();
						num += 1f;
					}
				}
				if (num == 0f)
				{
					return Color.White;
				}
				return new Color(vector / num);
			}

			// Token: 0x0400760A RID: 30218
			private Texture2D _texture;

			// Token: 0x0400760B RID: 30219
			private Vector2 _position;

			// Token: 0x0400760C RID: 30220
			private Vector2 _velocity;

			// Token: 0x0400760D RID: 30221
			private Vector2 _origin;

			// Token: 0x0400760E RID: 30222
			private float _rotation;

			// Token: 0x0400760F RID: 30223
			private float _rotationVelocity;
		}

		// Token: 0x0200098B RID: 2443
		public class MoonlordExplosion
		{
			// Token: 0x06004967 RID: 18791 RVA: 0x006D0918 File Offset: 0x006CEB18
			public MoonlordExplosion(Texture2D pieceTexture, Vector2 centerPos, int frameSpeed)
			{
				this._texture = pieceTexture;
				this._position = centerPos;
				this._frameSpeed = frameSpeed;
				this._frameCounter = 0;
				this._frame = this._texture.Frame(1, 7, 0, 0, 0, 0);
				this._origin = this._frame.Size() / 2f;
			}

			// Token: 0x06004968 RID: 18792 RVA: 0x006D0979 File Offset: 0x006CEB79
			public void Update()
			{
				this._frameCounter++;
				this._frame = this._texture.Frame(1, 7, 0, this._frameCounter / this._frameSpeed, 0, 0);
			}

			// Token: 0x06004969 RID: 18793 RVA: 0x006D09AC File Offset: 0x006CEBAC
			public void Draw(SpriteBatch sp)
			{
				Color light = this.GetLight();
				sp.Draw(this._texture, this._position - Main.screenPosition, new Rectangle?(this._frame), light, 0f, this._origin, 1f, SpriteEffects.None, 0f);
			}

			// Token: 0x17000592 RID: 1426
			// (get) Token: 0x0600496A RID: 18794 RVA: 0x006D0A00 File Offset: 0x006CEC00
			public bool Dead
			{
				get
				{
					return this._position.Y > (float)(Main.maxTilesY * 16) - 480f || this._position.X < 480f || this._position.X >= (float)(Main.maxTilesX * 16) - 480f || this._frameCounter >= this._frameSpeed * 7;
				}
			}

			// Token: 0x0600496B RID: 18795 RVA: 0x006D0A6C File Offset: 0x006CEC6C
			public bool InDrawRange(Rectangle playerScreen)
			{
				return playerScreen.Contains(this._position.ToPoint());
			}

			// Token: 0x0600496C RID: 18796 RVA: 0x006D0A80 File Offset: 0x006CEC80
			public Color GetLight()
			{
				return new Color(255, 255, 255, 127);
			}

			// Token: 0x04007610 RID: 30224
			private Texture2D _texture;

			// Token: 0x04007611 RID: 30225
			private Vector2 _position;

			// Token: 0x04007612 RID: 30226
			private Vector2 _origin;

			// Token: 0x04007613 RID: 30227
			private Rectangle _frame;

			// Token: 0x04007614 RID: 30228
			private int _frameCounter;

			// Token: 0x04007615 RID: 30229
			private int _frameSpeed;
		}
	}
}
