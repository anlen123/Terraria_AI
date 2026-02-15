using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Animations;
using Terraria.GameContent.Skies.CreditsRoll;
using Terraria.Graphics.Effects;

namespace Terraria.GameContent.Skies
{
	// Token: 0x02000449 RID: 1097
	public class CreditsRollSky : CustomSky
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x060031CD RID: 12749 RVA: 0x005E3785 File Offset: 0x005E1985
		public int AmountOfTimeNeededForFullPlay
		{
			get
			{
				return this._endTime;
			}
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x005E378D File Offset: 0x005E198D
		public CreditsRollSky()
		{
			this.EnsureSegmentsAreMade();
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x005E37BC File Offset: 0x005E19BC
		public override void Update(GameTime gameTime)
		{
			if (FocusHelper.PauseSkies)
			{
				return;
			}
			this._currentTime++;
			float num = 0.008333334f;
			if (Main.gameMenu)
			{
				num = 0.06666667f;
			}
			this._opacity = MathHelper.Clamp(this._opacity + num * (float)this._wantsToBeSeen.ToDirectionInt(), 0f, 1f);
			if (this._opacity == 0f && !this._wantsToBeSeen)
			{
				this._isActive = false;
				return;
			}
			bool flag = true;
			if (!Main.CanPlayCreditsRoll())
			{
				flag = false;
			}
			if (this._currentTime >= this._endTime)
			{
				flag = false;
			}
			if (!flag)
			{
				SkyManager.Instance.Deactivate("CreditsRoll", new object[0]);
			}
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x005E3874 File Offset: 0x005E1A74
		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			float num = 1f;
			if (num < minDepth || num > maxDepth)
			{
				return;
			}
			Vector2 anchorPositionOnScreen = Main.ScreenSize.ToVector2() / 2f;
			if (Main.gameMenu)
			{
				anchorPositionOnScreen.Y = 300f;
			}
			GameAnimationSegment gameAnimationSegment = new GameAnimationSegment
			{
				SpriteBatch = spriteBatch,
				AnchorPositionOnScreen = anchorPositionOnScreen,
				TimeInAnimation = this._currentTime,
				DisplayOpacity = this._opacity
			};
			List<IAnimationSegment> list = this._segmentsInGame;
			if (Main.gameMenu)
			{
				list = this._segmentsInMainMenu;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Draw(ref gameAnimationSegment);
			}
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x005E3926 File Offset: 0x005E1B26
		public override bool IsActive()
		{
			return this._isActive;
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x005E392E File Offset: 0x005E1B2E
		public override void Reset()
		{
			this._currentTime = 0;
			this.EnsureSegmentsAreMade();
			this._isActive = false;
			this._wantsToBeSeen = false;
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x005E394B File Offset: 0x005E1B4B
		public override void Activate(Vector2 position, params object[] args)
		{
			this._isActive = true;
			this._wantsToBeSeen = true;
			if (this._opacity == 0f)
			{
				this.EnsureSegmentsAreMade();
				this._currentTime = 0;
			}
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x005E3978 File Offset: 0x005E1B78
		private void EnsureSegmentsAreMade()
		{
			if (this._segmentsInMainMenu.Count > 0 && this._segmentsInGame.Count > 0)
			{
				return;
			}
			this._segmentsInGame.Clear();
			this._composer.FillSegments(this._segmentsInGame, out this._endTime, true);
			this._segmentsInMainMenu.Clear();
			this._composer.FillSegments(this._segmentsInMainMenu, out this._endTime, false);
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x005E39E8 File Offset: 0x005E1BE8
		public override void Deactivate(params object[] args)
		{
			this._wantsToBeSeen = false;
		}

		// Token: 0x04005798 RID: 22424
		private int _endTime;

		// Token: 0x04005799 RID: 22425
		private int _currentTime;

		// Token: 0x0400579A RID: 22426
		private CreditsRollComposer _composer = new CreditsRollComposer();

		// Token: 0x0400579B RID: 22427
		private List<IAnimationSegment> _segmentsInGame = new List<IAnimationSegment>();

		// Token: 0x0400579C RID: 22428
		private List<IAnimationSegment> _segmentsInMainMenu = new List<IAnimationSegment>();

		// Token: 0x0400579D RID: 22429
		private bool _isActive;

		// Token: 0x0400579E RID: 22430
		private bool _wantsToBeSeen;

		// Token: 0x0400579F RID: 22431
		private float _opacity;
	}
}
