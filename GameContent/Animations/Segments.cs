using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.Animations
{
	// Token: 0x0200052A RID: 1322
	public class Segments
	{
		// Token: 0x04005B2C RID: 23340
		private const float PixelsToRollUpPerFrame = 0.5f;

		// Token: 0x020009A0 RID: 2464
		public class LocalizedTextSegment : IAnimationSegment
		{
			// Token: 0x1700059A RID: 1434
			// (get) Token: 0x060049A5 RID: 18853 RVA: 0x006D143B File Offset: 0x006CF63B
			public float DedicatedTimeNeeded
			{
				get
				{
					return 240f;
				}
			}

			// Token: 0x060049A6 RID: 18854 RVA: 0x006D1442 File Offset: 0x006CF642
			public LocalizedTextSegment(float timeInAnimation, string textKey)
			{
				this._text = Language.GetText(textKey);
				this._timeToShowPeak = timeInAnimation;
			}

			// Token: 0x060049A7 RID: 18855 RVA: 0x006D145D File Offset: 0x006CF65D
			public LocalizedTextSegment(float timeInAnimation, LocalizedText textObject, Vector2 anchorOffset)
			{
				this._text = textObject;
				this._timeToShowPeak = timeInAnimation;
				this._anchorOffset = anchorOffset;
			}

			// Token: 0x060049A8 RID: 18856 RVA: 0x006D147C File Offset: 0x006CF67C
			public void Draw(ref GameAnimationSegment info)
			{
				float num = 250f;
				float num2 = 250f;
				int timeInAnimation = info.TimeInAnimation;
				float num3 = Utils.GetLerpValue(this._timeToShowPeak - num, this._timeToShowPeak, (float)timeInAnimation, true) * Utils.GetLerpValue(this._timeToShowPeak + num2, this._timeToShowPeak, (float)timeInAnimation, true);
				if (num3 <= 0f)
				{
					return;
				}
				float num4 = this._timeToShowPeak - (float)timeInAnimation;
				Vector2 vector = info.AnchorPositionOnScreen + new Vector2(0f, num4 * 0.5f);
				vector += this._anchorOffset;
				Vector2 baseScale = new Vector2(0.7f);
				float num5 = Main.GlobalTimeWrappedHourly * 0.02f % 1f;
				if (num5 < 0f)
				{
					num5 += 1f;
				}
				Color value = Main.hslToRgb(num5, 1f, 0.5f, byte.MaxValue);
				string value2 = this._text.Value;
				Vector2 vector2 = FontAssets.DeathText.Value.MeasureString(value2);
				vector2 *= 0.5f;
				float scale = 1f - (1f - num3) * (1f - num3);
				ChatManager.DrawColorCodedStringShadow(info.SpriteBatch, FontAssets.DeathText.Value, value2, vector, value * scale * scale * 0.25f * info.DisplayOpacity, 0f, vector2, baseScale, -1f, 2f);
				ChatManager.DrawColorCodedString(info.SpriteBatch, FontAssets.DeathText.Value, value2, vector, Color.White * scale * info.DisplayOpacity, 0f, vector2, baseScale, -1f, false);
			}

			// Token: 0x04007648 RID: 30280
			private const int PixelsForALine = 120;

			// Token: 0x04007649 RID: 30281
			private LocalizedText _text;

			// Token: 0x0400764A RID: 30282
			private float _timeToShowPeak;

			// Token: 0x0400764B RID: 30283
			private Vector2 _anchorOffset;
		}

		// Token: 0x020009A1 RID: 2465
		public abstract class AnimationSegmentWithActions<T> : IAnimationSegment
		{
			// Token: 0x1700059B RID: 1435
			// (get) Token: 0x060049A9 RID: 18857 RVA: 0x006D162C File Offset: 0x006CF82C
			public float DedicatedTimeNeeded
			{
				get
				{
					return (float)this._dedicatedTimeNeeded;
				}
			}

			// Token: 0x060049AA RID: 18858 RVA: 0x006D1635 File Offset: 0x006CF835
			public AnimationSegmentWithActions(int targetTime)
			{
				this._targetTime = targetTime;
				this._dedicatedTimeNeeded = 0;
			}

			// Token: 0x060049AB RID: 18859 RVA: 0x006D1658 File Offset: 0x006CF858
			protected void ProcessActions(T obj, float localTimeForObject)
			{
				for (int i = 0; i < this._actions.Count; i++)
				{
					this._actions[i].ApplyTo(obj, localTimeForObject);
				}
			}

			// Token: 0x060049AC RID: 18860 RVA: 0x006D1690 File Offset: 0x006CF890
			public Segments.AnimationSegmentWithActions<T> Then(IAnimationSegmentAction<T> act)
			{
				this.Bind(act);
				act.SetDelay((float)this._dedicatedTimeNeeded);
				this._actions.Add(act);
				this._lastDedicatedTimeNeeded = this._dedicatedTimeNeeded;
				this._dedicatedTimeNeeded += act.ExpectedLengthOfActionInFrames;
				return this;
			}

			// Token: 0x060049AD RID: 18861 RVA: 0x006D16DD File Offset: 0x006CF8DD
			public Segments.AnimationSegmentWithActions<T> With(IAnimationSegmentAction<T> act)
			{
				this.Bind(act);
				act.SetDelay((float)this._lastDedicatedTimeNeeded);
				this._actions.Add(act);
				return this;
			}

			// Token: 0x060049AE RID: 18862
			protected abstract void Bind(IAnimationSegmentAction<T> act);

			// Token: 0x060049AF RID: 18863
			public abstract void Draw(ref GameAnimationSegment info);

			// Token: 0x0400764C RID: 30284
			private int _dedicatedTimeNeeded;

			// Token: 0x0400764D RID: 30285
			private int _lastDedicatedTimeNeeded;

			// Token: 0x0400764E RID: 30286
			protected int _targetTime;

			// Token: 0x0400764F RID: 30287
			private List<IAnimationSegmentAction<T>> _actions = new List<IAnimationSegmentAction<T>>();
		}

		// Token: 0x020009A2 RID: 2466
		public class PlayerSegment : Segments.AnimationSegmentWithActions<Player>
		{
			// Token: 0x060049B0 RID: 18864 RVA: 0x006D1700 File Offset: 0x006CF900
			public PlayerSegment(int targetTime, Vector2 anchorOffset, Vector2 normalizedHitboxOrigin) : base(targetTime)
			{
				this._player = new Player();
				this._anchorOffset = anchorOffset;
				this._normalizedOriginForHitbox = normalizedHitboxOrigin;
			}

			// Token: 0x060049B1 RID: 18865 RVA: 0x006D1722 File Offset: 0x006CF922
			public Segments.PlayerSegment UseShaderEffect(Segments.PlayerSegment.IShaderEffect shaderEffect)
			{
				this._shaderEffect = shaderEffect;
				return this;
			}

			// Token: 0x060049B2 RID: 18866 RVA: 0x006D172C File Offset: 0x006CF92C
			protected override void Bind(IAnimationSegmentAction<Player> act)
			{
				act.BindTo(this._player);
			}

			// Token: 0x060049B3 RID: 18867 RVA: 0x006D173C File Offset: 0x006CF93C
			public override void Draw(ref GameAnimationSegment info)
			{
				if ((float)info.TimeInAnimation > (float)this._targetTime + base.DedicatedTimeNeeded)
				{
					return;
				}
				if (info.TimeInAnimation < this._targetTime)
				{
					return;
				}
				this.ResetPlayerAnimation(ref info);
				float localTimeForObject = (float)(info.TimeInAnimation - this._targetTime);
				base.ProcessActions(this._player, localTimeForObject);
				if (info.DisplayOpacity == 0f)
				{
					return;
				}
				this._player.ResetEffects();
				this._player.ResetVisibleAccessories();
				this._player.UpdateMiscCounter();
				this._player.UpdateDyes();
				this._player.PlayerFrame();
				this._player.socialIgnoreLight = true;
				this._player.position += Main.screenPosition;
				this._player.position -= new Vector2((float)(this._player.width / 2), (float)this._player.height);
				this._player.opacityForAnimation *= info.DisplayOpacity;
				Item item = this._player.inventory[this._player.selectedItem];
				this._player.inventory[this._player.selectedItem] = Segments.PlayerSegment._blankItem;
				float shadow = 1f - this._player.opacityForAnimation;
				shadow = 0f;
				if (this._shaderEffect != null)
				{
					this._shaderEffect.BeforeDrawing(ref info);
				}
				Main.PlayerRenderer.DrawPlayer(Main.Camera, this._player, this._player.position, 0f, this._player.fullRotationOrigin, shadow, 1f);
				if (this._shaderEffect != null)
				{
					this._shaderEffect.AfterDrawing(ref info);
				}
				this._player.inventory[this._player.selectedItem] = item;
			}

			// Token: 0x060049B4 RID: 18868 RVA: 0x006D190E File Offset: 0x006CFB0E
			private void ResetPlayerAnimation(ref GameAnimationSegment info)
			{
				this._player.CopyVisuals(Main.LocalPlayer);
				this._player.position = info.AnchorPositionOnScreen + this._anchorOffset;
				this._player.opacityForAnimation = 1f;
			}

			// Token: 0x04007650 RID: 30288
			private Player _player;

			// Token: 0x04007651 RID: 30289
			private Vector2 _anchorOffset;

			// Token: 0x04007652 RID: 30290
			private Vector2 _normalizedOriginForHitbox;

			// Token: 0x04007653 RID: 30291
			private Segments.PlayerSegment.IShaderEffect _shaderEffect;

			// Token: 0x04007654 RID: 30292
			private static Item _blankItem = new Item();

			// Token: 0x02000AEE RID: 2798
			public interface IShaderEffect
			{
				// Token: 0x06004D14 RID: 19732
				void BeforeDrawing(ref GameAnimationSegment info);

				// Token: 0x06004D15 RID: 19733
				void AfterDrawing(ref GameAnimationSegment info);
			}

			// Token: 0x02000AEF RID: 2799
			public class ImmediateSpritebatchForPlayerDyesEffect : Segments.PlayerSegment.IShaderEffect
			{
				// Token: 0x06004D16 RID: 19734 RVA: 0x006DA24D File Offset: 0x006D844D
				public void BeforeDrawing(ref GameAnimationSegment info)
				{
					info.SpriteBatch.End();
					info.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.CurrentFrameFlags.Hacks.CurrentBackgroundMatrixForCreditsRoll);
				}

				// Token: 0x06004D17 RID: 19735 RVA: 0x006DA280 File Offset: 0x006D8480
				public void AfterDrawing(ref GameAnimationSegment info)
				{
					info.SpriteBatch.End();
					info.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.CurrentFrameFlags.Hacks.CurrentBackgroundMatrixForCreditsRoll);
				}
			}
		}

		// Token: 0x020009A3 RID: 2467
		public class NPCSegment : Segments.AnimationSegmentWithActions<NPC>
		{
			// Token: 0x060049B6 RID: 18870 RVA: 0x006D1958 File Offset: 0x006CFB58
			public NPCSegment(int targetTime, int npcId, Vector2 anchorOffset, Vector2 normalizedNPCHitboxOrigin) : base(targetTime)
			{
				this._npc = new NPC();
				this._npc.IsABestiaryIconDummy = true;
				this._npc.SetDefaults(npcId, new NPCSpawnParams
				{
					playerCountForMultiplayerDifficultyOverride = new int?(1),
					difficultyOverride = new float?(GameDifficultyLevel.Classic)
				});
				this._anchorOffset = anchorOffset;
				this._normalizedOriginForHitbox = normalizedNPCHitboxOrigin;
			}

			// Token: 0x060049B7 RID: 18871 RVA: 0x006D19C5 File Offset: 0x006CFBC5
			protected override void Bind(IAnimationSegmentAction<NPC> act)
			{
				act.BindTo(this._npc);
			}

			// Token: 0x060049B8 RID: 18872 RVA: 0x006D19D4 File Offset: 0x006CFBD4
			public override void Draw(ref GameAnimationSegment info)
			{
				if ((float)info.TimeInAnimation > (float)this._targetTime + base.DedicatedTimeNeeded)
				{
					return;
				}
				if (info.TimeInAnimation < this._targetTime)
				{
					return;
				}
				this.ResetNPCAnimation(ref info);
				float localTimeForObject = (float)(info.TimeInAnimation - this._targetTime);
				base.ProcessActions(this._npc, localTimeForObject);
				if (this._npc.alpha >= 255)
				{
					return;
				}
				this._npc.FindFrame();
				ITownNPCProfile townNPCProfile;
				if (TownNPCProfiles.Instance.GetProfile(this._npc.type, out townNPCProfile))
				{
					TextureAssets.Npc[this._npc.type] = townNPCProfile.GetTextureNPCShouldUse(this._npc);
				}
				this._npc.Opacity *= info.DisplayOpacity;
				Main.instance.DrawNPCDirect(info.SpriteBatch, this._npc, this._npc.behindTiles, Vector2.Zero);
			}

			// Token: 0x060049B9 RID: 18873 RVA: 0x006D1AC0 File Offset: 0x006CFCC0
			private void ResetNPCAnimation(ref GameAnimationSegment info)
			{
				this._npc.position = info.AnchorPositionOnScreen + this._anchorOffset - this._npc.Size * this._normalizedOriginForHitbox;
				this._npc.alpha = 0;
				this._npc.velocity = Vector2.Zero;
			}

			// Token: 0x04007655 RID: 30293
			private NPC _npc;

			// Token: 0x04007656 RID: 30294
			private Vector2 _anchorOffset;

			// Token: 0x04007657 RID: 30295
			private Vector2 _normalizedOriginForHitbox;
		}

		// Token: 0x020009A4 RID: 2468
		public class LooseSprite
		{
			// Token: 0x060049BA RID: 18874 RVA: 0x006D1B20 File Offset: 0x006CFD20
			public LooseSprite(DrawData data, Asset<Texture2D> asset)
			{
				this._originalDrawData = data;
				this._asset = asset;
				this.Reset();
			}

			// Token: 0x060049BB RID: 18875 RVA: 0x006D1B3C File Offset: 0x006CFD3C
			public void Reset()
			{
				this._originalDrawData.texture = this._asset.Value;
				this.CurrentDrawData = this._originalDrawData;
				this.CurrentOpacity = 1f;
			}

			// Token: 0x04007658 RID: 30296
			private DrawData _originalDrawData;

			// Token: 0x04007659 RID: 30297
			private Asset<Texture2D> _asset;

			// Token: 0x0400765A RID: 30298
			public DrawData CurrentDrawData;

			// Token: 0x0400765B RID: 30299
			public float CurrentOpacity;
		}

		// Token: 0x020009A5 RID: 2469
		public class SpriteSegment : Segments.AnimationSegmentWithActions<Segments.LooseSprite>
		{
			// Token: 0x060049BC RID: 18876 RVA: 0x006D1B6B File Offset: 0x006CFD6B
			public SpriteSegment(Asset<Texture2D> asset, int targetTime, DrawData data, Vector2 anchorOffset) : base(targetTime)
			{
				this._sprite = new Segments.LooseSprite(data, asset);
				this._anchorOffset = anchorOffset;
			}

			// Token: 0x060049BD RID: 18877 RVA: 0x006D1B89 File Offset: 0x006CFD89
			protected override void Bind(IAnimationSegmentAction<Segments.LooseSprite> act)
			{
				act.BindTo(this._sprite);
			}

			// Token: 0x060049BE RID: 18878 RVA: 0x006D1B97 File Offset: 0x006CFD97
			public Segments.SpriteSegment UseShaderEffect(Segments.SpriteSegment.IShaderEffect shaderEffect)
			{
				this._shaderEffect = shaderEffect;
				return this;
			}

			// Token: 0x060049BF RID: 18879 RVA: 0x006D1BA4 File Offset: 0x006CFDA4
			public override void Draw(ref GameAnimationSegment info)
			{
				if ((float)info.TimeInAnimation > (float)this._targetTime + base.DedicatedTimeNeeded)
				{
					return;
				}
				if (info.TimeInAnimation < this._targetTime)
				{
					return;
				}
				this.ResetSpriteAnimation(ref info);
				float localTimeForObject = (float)(info.TimeInAnimation - this._targetTime);
				base.ProcessActions(this._sprite, localTimeForObject);
				DrawData currentDrawData = this._sprite.CurrentDrawData;
				currentDrawData.position += info.AnchorPositionOnScreen + this._anchorOffset;
				currentDrawData.color *= this._sprite.CurrentOpacity * info.DisplayOpacity;
				if (this._shaderEffect != null)
				{
					this._shaderEffect.BeforeDrawing(ref info, ref currentDrawData);
				}
				currentDrawData.Draw(info.SpriteBatch);
				if (this._shaderEffect != null)
				{
					this._shaderEffect.AfterDrawing(ref info, ref currentDrawData);
				}
			}

			// Token: 0x060049C0 RID: 18880 RVA: 0x006D1C91 File Offset: 0x006CFE91
			private void ResetSpriteAnimation(ref GameAnimationSegment info)
			{
				this._sprite.Reset();
			}

			// Token: 0x0400765C RID: 30300
			private Segments.LooseSprite _sprite;

			// Token: 0x0400765D RID: 30301
			private Vector2 _anchorOffset;

			// Token: 0x0400765E RID: 30302
			private Segments.SpriteSegment.IShaderEffect _shaderEffect;

			// Token: 0x02000AF0 RID: 2800
			public interface IShaderEffect
			{
				// Token: 0x06004D19 RID: 19737
				void BeforeDrawing(ref GameAnimationSegment info, ref DrawData drawData);

				// Token: 0x06004D1A RID: 19738
				void AfterDrawing(ref GameAnimationSegment info, ref DrawData drawData);
			}

			// Token: 0x02000AF1 RID: 2801
			public class MaskedFadeEffect : Segments.SpriteSegment.IShaderEffect
			{
				// Token: 0x06004D1B RID: 19739 RVA: 0x006DA2B4 File Offset: 0x006D84B4
				public MaskedFadeEffect(Segments.SpriteSegment.MaskedFadeEffect.GetMatrixAction fetchMatrixMethod = null, string shaderKey = "MaskedFade", int verticalFrameCount = 1, int verticalFrameWait = 1)
				{
					this._fetchMatrix = fetchMatrixMethod;
					this._shaderKey = shaderKey;
					this._verticalFrameCount = verticalFrameCount;
					if (verticalFrameWait < 1)
					{
						verticalFrameWait = 1;
					}
					this._verticalFrameWait = verticalFrameWait;
					if (this._fetchMatrix == null)
					{
						this._fetchMatrix = new Segments.SpriteSegment.MaskedFadeEffect.GetMatrixAction(this.DefaultFetchMatrix);
					}
				}

				// Token: 0x06004D1C RID: 19740 RVA: 0x006DA306 File Offset: 0x006D8506
				private Matrix DefaultFetchMatrix()
				{
					return Main.CurrentFrameFlags.Hacks.CurrentBackgroundMatrixForCreditsRoll;
				}

				// Token: 0x06004D1D RID: 19741 RVA: 0x006DA310 File Offset: 0x006D8510
				public void BeforeDrawing(ref GameAnimationSegment info, ref DrawData drawData)
				{
					info.SpriteBatch.End();
					info.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, this._fetchMatrix());
					MiscShaderData miscShaderData = GameShaders.Misc[this._shaderKey];
					int num = info.TimeInAnimation / this._verticalFrameWait % this._verticalFrameCount;
					miscShaderData.UseShaderSpecificData(new Vector4(1f / (float)this._verticalFrameCount, (float)num / (float)this._verticalFrameCount, this._panX.GetPanAmount((float)info.TimeInAnimation), this._panY.GetPanAmount((float)info.TimeInAnimation)));
					miscShaderData.Apply(new DrawData?(drawData));
				}

				// Token: 0x06004D1E RID: 19742 RVA: 0x006DA3D0 File Offset: 0x006D85D0
				public Segments.SpriteSegment.MaskedFadeEffect WithPanX(Segments.Panning panning)
				{
					this._panX = panning;
					return this;
				}

				// Token: 0x06004D1F RID: 19743 RVA: 0x006DA3DA File Offset: 0x006D85DA
				public Segments.SpriteSegment.MaskedFadeEffect WithPanY(Segments.Panning panning)
				{
					this._panY = panning;
					return this;
				}

				// Token: 0x06004D20 RID: 19744 RVA: 0x006DA3E4 File Offset: 0x006D85E4
				public void AfterDrawing(ref GameAnimationSegment info, ref DrawData drawData)
				{
					Main.pixelShader.CurrentTechnique.Passes[0].Apply();
					info.SpriteBatch.End();
					info.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, this._fetchMatrix());
				}

				// Token: 0x0400788D RID: 30861
				private readonly string _shaderKey;

				// Token: 0x0400788E RID: 30862
				private readonly int _verticalFrameCount;

				// Token: 0x0400788F RID: 30863
				private readonly int _verticalFrameWait;

				// Token: 0x04007890 RID: 30864
				private Segments.Panning _panX;

				// Token: 0x04007891 RID: 30865
				private Segments.Panning _panY;

				// Token: 0x04007892 RID: 30866
				private Segments.SpriteSegment.MaskedFadeEffect.GetMatrixAction _fetchMatrix;

				// Token: 0x02000B1C RID: 2844
				// (Invoke) Token: 0x06004DB9 RID: 19897
				public delegate Matrix GetMatrixAction();
			}
		}

		// Token: 0x020009A6 RID: 2470
		public struct Panning
		{
			// Token: 0x060049C1 RID: 18881 RVA: 0x006D1CA0 File Offset: 0x006CFEA0
			public float GetPanAmount(float time)
			{
				float num = MathHelper.Clamp((time - this.Delay) / this.Duration, 0f, 1f);
				return this.StartAmount + num * this.AmountOverTime;
			}

			// Token: 0x0400765F RID: 30303
			public float AmountOverTime;

			// Token: 0x04007660 RID: 30304
			public float StartAmount;

			// Token: 0x04007661 RID: 30305
			public float Delay;

			// Token: 0x04007662 RID: 30306
			public float Duration;
		}

		// Token: 0x020009A7 RID: 2471
		public class EmoteSegment : IAnimationSegment
		{
			// Token: 0x1700059C RID: 1436
			// (get) Token: 0x060049C2 RID: 18882 RVA: 0x006D1CDB File Offset: 0x006CFEDB
			// (set) Token: 0x060049C3 RID: 18883 RVA: 0x006D1CE3 File Offset: 0x006CFEE3
			public float DedicatedTimeNeeded { get; private set; }

			// Token: 0x060049C4 RID: 18884 RVA: 0x006D1CEC File Offset: 0x006CFEEC
			public EmoteSegment(int emoteId, int targetTime, int timeToPlay, Vector2 position, SpriteEffects drawEffect, Vector2 velocity = default(Vector2))
			{
				this._emoteId = emoteId;
				this._targetTime = targetTime;
				this._effect = drawEffect;
				this._offset = position;
				this._velocity = velocity;
				this.DedicatedTimeNeeded = (float)timeToPlay;
			}

			// Token: 0x060049C5 RID: 18885 RVA: 0x006D1D24 File Offset: 0x006CFF24
			public void Draw(ref GameAnimationSegment info)
			{
				int num = info.TimeInAnimation - this._targetTime;
				if (num < 0)
				{
					return;
				}
				if ((float)num >= this.DedicatedTimeNeeded)
				{
					return;
				}
				Vector2 vector = info.AnchorPositionOnScreen + this._offset + this._velocity * (float)num;
				vector = vector.Floor();
				bool flag = num < 6 || (float)num >= this.DedicatedTimeNeeded - 6f;
				Texture2D value = TextureAssets.Extra[48].Value;
				Rectangle rectangle = value.Frame(8, EmoteBubble.EMOTE_SHEET_VERTICAL_FRAMES, flag ? 0 : 1, 0, 0, 0);
				Vector2 origin = new Vector2((float)(rectangle.Width / 2), (float)rectangle.Height);
				SpriteEffects spriteEffects = this._effect;
				info.SpriteBatch.Draw(value, vector, new Rectangle?(rectangle), Color.White * info.DisplayOpacity, 0f, origin, 1f, spriteEffects, 0f);
				if (!flag)
				{
					int emoteId = this._emoteId;
					if ((emoteId == 87 || emoteId == 89) && (spriteEffects & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
					{
						spriteEffects &= ~SpriteEffects.FlipHorizontally;
						vector.X += 4f;
					}
					info.SpriteBatch.Draw(value, vector, new Rectangle?(this.GetFrame(num % 20)), Color.White, 0f, origin, 1f, spriteEffects, 0f);
				}
			}

			// Token: 0x060049C6 RID: 18886 RVA: 0x006D1E7C File Offset: 0x006D007C
			private Rectangle GetFrame(int wrappedTime)
			{
				int num = (wrappedTime >= 10) ? 1 : 0;
				return TextureAssets.Extra[48].Value.Frame(8, EmoteBubble.EMOTE_SHEET_VERTICAL_FRAMES, this._emoteId % 4 * 2 + num, this._emoteId / 4 + 1, 0, 0);
			}

			// Token: 0x04007664 RID: 30308
			private int _targetTime;

			// Token: 0x04007665 RID: 30309
			private Vector2 _offset;

			// Token: 0x04007666 RID: 30310
			private SpriteEffects _effect;

			// Token: 0x04007667 RID: 30311
			private int _emoteId;

			// Token: 0x04007668 RID: 30312
			private Vector2 _velocity;
		}
	}
}
