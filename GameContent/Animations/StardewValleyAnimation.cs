using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.GameContent.Animations
{
	// Token: 0x0200052B RID: 1323
	public class StardewValleyAnimation
	{
		// Token: 0x060036D5 RID: 14037 RVA: 0x0062B2DB File Offset: 0x006294DB
		public StardewValleyAnimation()
		{
			this.ComposeAnimation();
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x0062B2F4 File Offset: 0x006294F4
		private void ComposeAnimation()
		{
			Asset<Texture2D> asset = TextureAssets.Extra[247];
			Rectangle rectangle = asset.Frame(1, 1, 0, 0, 0, 0);
			DrawData data = new DrawData(asset.Value, Vector2.Zero, new Rectangle?(rectangle), Color.White, 0f, rectangle.Size() * new Vector2(0.5f, 0.5f), 1f, SpriteEffects.None, 0f);
			int targetTime = 128;
			int num = 60;
			int num2 = 360;
			int duration = 60;
			int num3 = 4;
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> item = new Segments.SpriteSegment(asset, targetTime, data, Vector2.Zero).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect(new Segments.SpriteSegment.MaskedFadeEffect.GetMatrixAction(this.GetMatrixForAnimation), "StardewValleyFade", 8, num3).WithPanX(new Segments.Panning
			{
				Delay = 128f,
				Duration = (float)(num2 - 120 + num - 60),
				AmountOverTime = 0.55f,
				StartAmount = -0.4f
			}).WithPanY(new Segments.Panning
			{
				StartAmount = 0f
			})).Then(new Actions.Sprites.OutCircleScale(Vector2.Zero)).With(new Actions.Sprites.OutCircleScale(Vector2.One, num)).Then(new Actions.Sprites.Wait(num2)).Then(new Actions.Sprites.OutCircleScale(Vector2.Zero, duration));
			this._segments.Add(item);
			Asset<Texture2D> asset2 = TextureAssets.Extra[249];
			Rectangle rectangle2 = asset2.Frame(1, 8, 0, 0, 0, 0);
			DrawData data2 = new DrawData(asset2.Value, Vector2.Zero, new Rectangle?(rectangle2), Color.White, 0f, rectangle2.Size() * new Vector2(0.5f, 0.5f), 1f, SpriteEffects.None, 0f);
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> item2 = new Segments.SpriteSegment(asset2, targetTime, data2, Vector2.Zero).Then(new Actions.Sprites.OutCircleScale(Vector2.Zero)).With(new Actions.Sprites.OutCircleScale(Vector2.One, num)).With(new Actions.Sprites.SetFrameSequence(100, new Point[]
			{
				new Point(0, 0),
				new Point(0, 1),
				new Point(0, 2),
				new Point(0, 3),
				new Point(0, 4),
				new Point(0, 5),
				new Point(0, 6),
				new Point(0, 7)
			}, num3, 0, 0)).Then(new Actions.Sprites.Wait(num2)).Then(new Actions.Sprites.OutCircleScale(Vector2.Zero, duration));
			this._segments.Add(item2);
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x0062B598 File Offset: 0x00629798
		private Matrix GetMatrixForAnimation()
		{
			return Main.Transform;
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x0062B5A0 File Offset: 0x006297A0
		public void Draw(SpriteBatch spriteBatch, int timeInAnimation, Vector2 positionInScreen)
		{
			GameAnimationSegment gameAnimationSegment = new GameAnimationSegment
			{
				SpriteBatch = spriteBatch,
				AnchorPositionOnScreen = positionInScreen,
				TimeInAnimation = timeInAnimation,
				DisplayOpacity = 1f
			};
			for (int i = 0; i < this._segments.Count; i++)
			{
				this._segments[i].Draw(ref gameAnimationSegment);
			}
		}

		// Token: 0x04005B2D RID: 23341
		private List<IAnimationSegment> _segments = new List<IAnimationSegment>();
	}
}
