using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003FC RID: 1020
	public class UICharacter : UIElement
	{
		// Token: 0x06002ECB RID: 11979 RVA: 0x005AE6B8 File Offset: 0x005AC8B8
		public UICharacter(Player player, bool animated = false, bool hasBackPanel = true, float characterScale = 1f, bool useAClone = false)
		{
			this._player = player;
			if (useAClone)
			{
				this._player = player.SerializedClone();
				this._player.dead = false;
				this._player.PlayerFrame();
			}
			this.Width.Set(59f, 0f);
			this.Height.Set(58f, 0f);
			this._texture = Main.Assets.Request<Texture2D>("Images/UI/PlayerBackground", 1);
			this.UseImmediateMode = true;
			this._animated = animated;
			this._drawsBackPanel = hasBackPanel;
			this._characterScale = characterScale;
			this.OverrideSamplerState = SamplerState.PointClamp;
			this._petProjectiles = UICharacter.NoPets;
			this.PreparePetProjectiles();
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x005AE780 File Offset: 0x005AC980
		private void PreparePetProjectiles()
		{
			if (this._player.hideMisc[0])
			{
				return;
			}
			Item item = this._player.miscEquips[0];
			if (item.IsAir)
			{
				return;
			}
			int shoot = item.shoot;
			this._petProjectiles = new Projectile[]
			{
				this.PreparePetProjectiles_CreatePetProjectileDummy(shoot)
			};
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x005AE7D5 File Offset: 0x005AC9D5
		private Projectile PreparePetProjectiles_CreatePetProjectileDummy(int projectileId)
		{
			Projectile projectile = new Projectile();
			projectile.SetDefaults(projectileId);
			projectile.isAPreviewDummy = true;
			return projectile;
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x005AE7EA File Offset: 0x005AC9EA
		public override void Update(GameTime gameTime)
		{
			if (this._animated)
			{
				this._animationCounter++;
			}
			base.Update(gameTime);
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x005AE80C File Offset: 0x005ACA0C
		private void UpdateAnim()
		{
			if (!this._animated)
			{
				this._player.bodyFrame.Y = (this._player.legFrame.Y = (this._player.headFrame.Y = 0));
				return;
			}
			int num = (int)(Main.GlobalTimeWrappedHourly / 0.07f) % 14 + 6;
			this._player.bodyFrame.Y = (this._player.legFrame.Y = (this._player.headFrame.Y = num * 56));
			this._player.WingFrame(false);
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x005AE8B4 File Offset: 0x005ACAB4
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			if (this._drawsBackPanel)
			{
				spriteBatch.Draw(this._texture.Value, dimensions.Position(), Color.White);
			}
			this._player.ResetEffects();
			this._player.ResetVisibleAccessories();
			this._player.UpdateMiscCounter();
			this._player.UpdateDyes();
			if (this.PrepareAction != null)
			{
				this.PrepareAction();
			}
			this._player.PlayerFrame();
			this.UpdateAnim();
			this.DrawPets(spriteBatch);
			Vector2 playerPosition = this.GetPlayerPosition(ref dimensions);
			Item item = this._player.inventory[this._player.selectedItem];
			this._player.inventory[this._player.selectedItem] = UICharacter._blankItem;
			Main.PlayerRenderer.DrawPlayer(Main.Camera, this._player, playerPosition + Main.screenPosition, 0f, Vector2.Zero, 0f, this._characterScale);
			this._player.inventory[this._player.selectedItem] = item;
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x005AE9D0 File Offset: 0x005ACBD0
		private Vector2 GetPlayerPosition(ref CalculatedStyle dimensions)
		{
			Vector2 result = dimensions.Position() + new Vector2(dimensions.Width * 0.5f - (float)(this._player.width >> 1), dimensions.Height * 0.5f - (float)(this._player.height >> 1));
			if (this._petProjectiles.Length != 0)
			{
				result.X -= 10f;
			}
			return result;
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x005AEA40 File Offset: 0x005ACC40
		public void DrawPets(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			Vector2 playerPosition = this.GetPlayerPosition(ref dimensions);
			for (int i = 0; i < this._petProjectiles.Length; i++)
			{
				Projectile projectile = this._petProjectiles[i];
				Vector2 value = playerPosition + new Vector2(0f, (float)this._player.height) + new Vector2(20f, 0f) + new Vector2(0f, (float)(-(float)projectile.height));
				projectile.position = value + Main.screenPosition;
				projectile.velocity = new Vector2(0.1f, 0f);
				projectile.direction = 1;
				projectile.owner = Main.myPlayer;
				ProjectileID.Sets.CharacterPreviewAnimations[projectile.type].ApplyTo(projectile, this._animated);
				Player player = Main.player[Main.myPlayer];
				Main.player[Main.myPlayer] = this._player;
				Main.instance.DrawProjDirect(projectile, null);
				Main.player[Main.myPlayer] = player;
			}
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, spriteBatch.GraphicsDevice.BlendState, spriteBatch.GraphicsDevice.SamplerStates[0], spriteBatch.GraphicsDevice.DepthStencilState, spriteBatch.GraphicsDevice.RasterizerState, null, Main.UIScaleMatrix);
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x005AEB95 File Offset: 0x005ACD95
		public void SetAnimated(bool animated)
		{
			this._animated = animated;
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06002ED4 RID: 11988 RVA: 0x005AEB9E File Offset: 0x005ACD9E
		public bool IsAnimated
		{
			get
			{
				return this._animated;
			}
		}

		// Token: 0x040055E0 RID: 21984
		private Player _player;

		// Token: 0x040055E1 RID: 21985
		private Projectile[] _petProjectiles;

		// Token: 0x040055E2 RID: 21986
		private Asset<Texture2D> _texture;

		// Token: 0x040055E3 RID: 21987
		private static Item _blankItem = new Item();

		// Token: 0x040055E4 RID: 21988
		private bool _animated;

		// Token: 0x040055E5 RID: 21989
		private bool _drawsBackPanel;

		// Token: 0x040055E6 RID: 21990
		private float _characterScale = 1f;

		// Token: 0x040055E7 RID: 21991
		private int _animationCounter;

		// Token: 0x040055E8 RID: 21992
		public Action PrepareAction;

		// Token: 0x040055E9 RID: 21993
		private static readonly Projectile[] NoPets = new Projectile[0];
	}
}
