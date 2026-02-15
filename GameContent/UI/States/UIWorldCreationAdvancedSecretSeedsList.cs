using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x0200039C RID: 924
	public class UIWorldCreationAdvancedSecretSeedsList : UIState, IHaveBackButtonCommand
	{
		// Token: 0x06002A04 RID: 10756 RVA: 0x0057FABE File Offset: 0x0057DCBE
		public UIWorldCreationAdvancedSecretSeedsList(UIWorldCreationAdvanced state, UIWorldCreation state2)
		{
			this._creationState = state;
			this._creationState2 = state2;
			this.BuildPage();
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x0057FAF0 File Offset: 0x0057DCF0
		private void BuildPage()
		{
			this.SeedDust.Clear();
			this.SeedParticleSystem.Clear();
			base.RemoveAllChildren();
			UIElement uielement = new UIElement
			{
				Width = StyleDimension.FromPixels(500f),
				Height = StyleDimension.FromPixelsAndPercent(-200f, 1f),
				Top = StyleDimension.FromPixels(202f),
				HAlign = 0.5f,
				VAlign = 0f
			};
			uielement.MaxHeight = StyleDimension.FromPixels(400f);
			uielement.SetPadding(0f);
			base.Append(uielement);
			UIPanel uipanel = new UIPanel
			{
				Width = StyleDimension.FromPercent(1f),
				Height = StyleDimension.FromPixelsAndPercent(-102f, 1f),
				BackgroundColor = new Color(33, 43, 79) * 0.8f
			};
			uipanel.SetPadding(0f);
			uielement.Append(uipanel);
			this.MakeBackAndCreatebuttons(uielement);
			int num = 56;
			int num2 = 4;
			UIElement uielement2 = new UIElement
			{
				Top = StyleDimension.FromPixelsAndPercent((float)num2, 0f),
				Width = StyleDimension.FromPixelsAndPercent(-20f, 1f),
				Left = StyleDimension.FromPixelsAndPercent(2f, 0f),
				Height = StyleDimension.FromPixelsAndPercent((float)(-(float)num2 - num), 1f),
				HAlign = 0.5f
			};
			uielement2.SetPadding(0f);
			uielement2.PaddingTop = 8f;
			uielement2.PaddingBottom = 12f;
			uipanel.Append(uielement2);
			this._worldList = new UIList();
			this._worldList.Width.Set(0f, 1f);
			this._worldList.Height.Set(0f, 1f);
			this._worldList.ListPadding = 5f;
			uielement2.Append(this._worldList);
			this._containerPanel = uielement2;
			this._scrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue);
			this._scrollbar.SetView(100f, 1000f);
			this._scrollbar.Height.Set(0f, 1f);
			this._scrollbar.HAlign = 1f;
			this._worldList.SetScrollbar(this._scrollbar);
			List<WorldGen.SecretSeed> seedsForInterface = SecretSeedsTracker.SeedsForInterface;
			this._worldList.ManualSortMethod = new Action<List<UIElement>>(this.CustomSort);
			int num3 = 0;
			foreach (WorldGen.SecretSeed secretSeed in seedsForInterface)
			{
				GroupOptionButton<WorldGen.SecretSeed> groupOptionButton = new GroupOptionButton<WorldGen.SecretSeed>(secretSeed, null, Language.GetText(secretSeed.Localization), Color.White, null, 1f, 0.5f, 10f)
				{
					Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
					Height = new StyleDimension(40f, 0f),
					HAlign = 0f
				};
				groupOptionButton.SetSnapPoint("Seed", num3++, null, null);
				UIElement uielement3 = new UIElement();
				groupOptionButton.Append(uielement3);
				groupOptionButton.SetTextWithoutLocalization(secretSeed.TextThatWasUsedToUnlock, 1f, Color.White, 0f, 10f);
				groupOptionButton.OnLeftMouseDown += this.ClickSecretSeed;
				groupOptionButton.OnMouseOver += this.MouseOverSeed;
				groupOptionButton.OnMouseOut += this.MouseOutSeed;
				groupOptionButton.SetCurrentOption(secretSeed.Enabled ? secretSeed : null);
				uielement3.OnDraw += this.DrawGlowRing;
				this._worldList.Add(groupOptionButton);
			}
			UIElement uielement4 = new UIElement
			{
				Width = StyleDimension.FromPixelsAndPercent(-20f, 1f),
				Height = StyleDimension.FromPixelsAndPercent((float)(num + num2), 0f),
				HAlign = 0.5f,
				VAlign = 1f
			};
			uielement4.SetPadding(0f);
			uielement4.PaddingBottom = 12f;
			uipanel.Append(uielement4);
			this.AddDescriptionPanel(uielement4, (float)num, "desc");
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x0057FF48 File Offset: 0x0057E148
		private void AddDescriptionPanel(UIElement container, float accumulatedHeight, string tagGroup)
		{
			float num = 0f;
			UISlicedImage uislicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1))
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent(-num * 2f, 1f),
				Left = StyleDimension.FromPixels(-num),
				Height = StyleDimension.FromPixelsAndPercent(accumulatedHeight, 0f),
				Top = StyleDimension.FromPixels(2f)
			};
			uislicedImage.SetSliceDepths(10);
			uislicedImage.Color = Color.LightGray * 0.7f;
			container.Append(uislicedImage);
			UIText uitext = new UIText(Language.GetText("UI.WorldDescriptionDefault"), 0.7f, false)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Top = StyleDimension.FromPixelsAndPercent(2f, 0f)
			};
			uitext.IsWrapped = true;
			uitext.PaddingLeft = 20f;
			uitext.PaddingRight = 20f;
			uitext.PaddingTop = 4f;
			uislicedImage.Append(uitext);
			this._descriptionText = uitext;
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x00580092 File Offset: 0x0057E292
		public void MouseOutSeed(UIMouseEvent evt, UIElement listeningElement)
		{
			this.ClearOptionDescription(evt, listeningElement);
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x0058009C File Offset: 0x0057E29C
		public void MouseOverSeed(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<WorldGen.SecretSeed> groupOptionButton = evt.Target as GroupOptionButton<WorldGen.SecretSeed>;
			if (groupOptionButton == null)
			{
				return;
			}
			bool isSelected = groupOptionButton.IsSelected;
			if (Main.mouseLeft)
			{
				listeningElement.LeftMouseDown(evt);
			}
			this.ShowOptionDescription(evt, listeningElement);
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x005800D8 File Offset: 0x0057E2D8
		public void DrawGlowRing(UIElement listeningElement, SpriteBatch spriteBatch)
		{
			GroupOptionButton<WorldGen.SecretSeed> groupOptionButton = (GroupOptionButton<WorldGen.SecretSeed>)listeningElement.Parent;
			if (groupOptionButton.OptionValue.Enabled)
			{
				Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/IconRandomSeed", 1);
				CalculatedStyle dimensions = groupOptionButton.GetDimensions();
				Vector2 position = dimensions.ToRectangle().TopRight() + new Vector2(-22f, 22f);
				Texture2D value = asset.Value;
				Rectangle rectangle = new Rectangle(0, 0, 4, 4);
				Vector2 origin = new Vector2((float)value.Width * 0.45f, (float)value.Height * 0.95f);
				float rotation = 0.25f * (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 1.3f + dimensions.Position().Y));
				origin = rectangle.Size() / 2f;
				float num = 1.5f;
				float num2 = num + 1f;
				Math.Sin((double)(Main.GlobalTimeWrappedHourly * 1.3f + dimensions.Position().Y * 0.00153178f));
				num = 1f;
				rotation = 0f;
				rectangle = value.Frame(1, 1, 0, 0, 0, 0);
				origin = rectangle.Size() / 2f;
				spriteBatch.Draw(value, position, new Rectangle?(rectangle), Color.White, rotation, origin, num, SpriteEffects.None, 0f);
			}
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x00580232 File Offset: 0x0057E432
		private void CustomSort(List<UIElement> items)
		{
			items.Sort(delegate(UIElement a, UIElement b)
			{
				GroupOptionButton<WorldGen.SecretSeed> groupOptionButton = a as GroupOptionButton<WorldGen.SecretSeed>;
				GroupOptionButton<WorldGen.SecretSeed> groupOptionButton2 = b as GroupOptionButton<WorldGen.SecretSeed>;
				if (groupOptionButton != null && groupOptionButton2 == null)
				{
					return -1;
				}
				if (groupOptionButton == null && groupOptionButton2 != null)
				{
					return 1;
				}
				return groupOptionButton.OptionValue.TextThatWasUsedToUnlock.CompareTo(groupOptionButton2.OptionValue.TextThatWasUsedToUnlock);
			});
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x0058025C File Offset: 0x0057E45C
		private void ClickSecretSeed(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<WorldGen.SecretSeed> groupOptionButton = (GroupOptionButton<WorldGen.SecretSeed>)listeningElement;
			WorldGen.SecretSeed optionValue = groupOptionButton.OptionValue;
			if (optionValue.Enabled)
			{
				groupOptionButton.SetCurrentOption(null);
				WorldGen.SecretSeed.Disable(optionValue);
				this._creationState2.RemoveSeedFromSeedMenu(optionValue.TextThatWasUsedToUnlock);
				return;
			}
			groupOptionButton.SetCurrentOption(optionValue);
			WorldGen.SecretSeed.Enable(optionValue, true);
			this._creationState2.AddSeedFromSeedmenu(optionValue.TextThatWasUsedToUnlock);
			this.SpawnParticles(groupOptionButton);
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x005802C4 File Offset: 0x0057E4C4
		private void SpawnParticles(GroupOptionButton<WorldGen.SecretSeed> element)
		{
			CalculatedStyle dimensions = element.GetDimensions();
			dimensions.Center();
			this.Spawn_RainbowRodHit(new ParticleOrchestraSettings
			{
				PositionInWorld = dimensions.Position() + new Vector2(dimensions.Width - 20f, dimensions.Height / 2f),
				MovementVector = new Vector2(0f, 16f) + Main.rand.NextVector2Circular(10f, 2f)
			});
			float num = 8f;
			int num2 = 0;
			while ((float)num2 < num + 1f)
			{
				this.Spawn_BestReforge(new ParticleOrchestraSettings
				{
					PositionInWorld = dimensions.Position() + new Vector2(0f, dimensions.Height / 2f) + new Vector2(dimensions.Width * (1f / num) * (float)num2, 0f)
				});
				num2++;
			}
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x005803C0 File Offset: 0x0057E5C0
		private void Spawn_RainbowRodHit(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = 6f;
			float num3 = Main.rand.NextFloat();
			for (float num4 = 0f; num4 < 1f; num4 += 1f / num2)
			{
				Vector2 vector = settings.MovementVector * Main.rand.NextFloatDirection() * 0.15f;
				Vector2 vector2 = new Vector2(Main.rand.NextFloat() * 0.4f + 0.4f);
				float f = num + Main.rand.NextFloat() * 6.2831855f;
				float rotation = 1.5707964f;
				Vector2 vector3 = 1.5f * vector2;
				float divider = 60f;
				Vector2 value = Main.rand.NextVector2Circular(8f, 8f) * vector2;
				PrettySparkleParticle prettySparkleParticle = new PrettySparkleParticle();
				prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / divider) - vector * 1f / 60f;
				prettySparkleParticle.ColorTint = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.33f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				prettySparkleParticle.ColorTint.A = 0;
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2;
				this.SeedParticleSystem.Add(prettySparkleParticle);
				prettySparkleParticle = new PrettySparkleParticle();
				prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / divider) - vector * 1f / 60f;
				prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2 * 0.6f;
				this.SeedParticleSystem.Add(prettySparkleParticle);
			}
			for (int i = 0; i < 12; i++)
			{
				Color newColor = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.12f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				Dust dust = this.SeedDust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor, 1f);
				dust.velocity = Main.rand.NextVector2Circular(1f, 1f);
				dust.velocity += settings.MovementVector * Main.rand.NextFloatDirection() * 0.5f;
				dust.noGravity = true;
				dust.scale = 0.6f + Main.rand.NextFloat() * 0.9f;
				dust.fadeIn = 0.7f + Main.rand.NextFloat() * 0.8f;
				if (dust.dustIndex != 200 && dust.type != 0)
				{
					Dust dust2 = this.SeedDust.CloneDust(dust);
					dust2.scale /= 2f;
					dust2.fadeIn *= 0.75f;
					dust2.color = new Color(255, 255, 255, 255);
				}
			}
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x005807AC File Offset: 0x0057E9AC
		private void Spawn_BestReforge(ParticleOrchestraSettings settings)
		{
			Vector2 accelerationPerFrame = new Vector2(0f, 0.16350001f);
			Asset<Texture2D> textureAsset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Research_Spark", 1);
			for (int i = 0; i < 2; i++)
			{
				Vector2 value = Main.rand.NextVector2Circular(3f, 4f);
				Vector2 value2 = new Vector2(0f, Main.rand.NextFloatDirection() * 20f);
				this.SeedParticleSystem.Add(new CreativeSacrificeParticle(textureAsset, null, settings.MovementVector + value, settings.PositionInWorld + value2)
				{
					AccelerationPerFrame = accelerationPerFrame,
					ScaleOffsetPerFrame = -0.016666668f
				});
			}
			float num = Main.rand.NextFloat();
			for (int j = 0; j < 3; j++)
			{
				Color newColor = Main.hslToRgb((num + Main.rand.NextFloat() * 0.12f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				Dust dust = this.SeedDust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor, 1f);
				dust.velocity = Main.rand.NextVector2Circular(1f, 1f);
				dust.velocity += settings.MovementVector * Main.rand.NextFloatDirection() * 0.5f;
				dust.noGravity = true;
				dust.scale = 0.6f + Main.rand.NextFloat() * 0.9f;
				dust.fadeIn = 0.7f + Main.rand.NextFloat() * 0.8f;
				Vector2 value3 = new Vector2(0f, Main.rand.NextFloatDirection() * 20f);
				dust.position += value3;
				if (dust.dustIndex != 200 && dust.type != 0)
				{
					Dust dust2 = this.SeedDust.CloneDust(dust);
					dust2.scale /= 2f;
					dust2.fadeIn *= 0.75f;
					dust2.color = new Color(255, 255, 255, 255);
				}
			}
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x00580A14 File Offset: 0x0057EC14
		public override void Recalculate()
		{
			if (this._scrollbar != null)
			{
				if (this._isScrollbarAttached && !this._scrollbar.CanScroll)
				{
					this._containerPanel.RemoveChild(this._scrollbar);
					this._isScrollbarAttached = false;
					this._worldList.Width.Set(0f, 1f);
				}
				else if (!this._isScrollbarAttached && this._scrollbar.CanScroll)
				{
					this._containerPanel.Append(this._scrollbar);
					this._isScrollbarAttached = true;
					this._worldList.Width.Set(-25f, 1f);
				}
			}
			base.Recalculate();
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x00580AC4 File Offset: 0x0057ECC4
		private void MakeBackAndCreatebuttons(UIElement outerContainer)
		{
			UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Apply"), 0.65f, true)
			{
				Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
				Height = StyleDimension.FromPixels(50f),
				VAlign = 1f,
				HAlign = 0.5f,
				Top = StyleDimension.FromPixels(-43f)
			};
			uitextPanel.OnMouseOver += this.FadedMouseOver;
			uitextPanel.OnMouseOut += this.FadedMouseOut;
			uitextPanel.OnLeftMouseDown += this.Click_GoBack;
			uitextPanel.SetSnapPoint("Back", 0, null, null);
			outerContainer.Append(uitextPanel);
			this._backButton = uitextPanel;
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x00580B94 File Offset: 0x0057ED94
		private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
		{
			this.GoBack();
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x00580B9C File Offset: 0x0057ED9C
		private void GoBack()
		{
			SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			Main.MenuUI.SetState(this._creationState);
			this._creationState.RefreshSecretSeedButton();
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x00580BD0 File Offset: 0x0057EDD0
		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
			this.ShowOptionDescription(evt, listeningElement);
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x00580C30 File Offset: 0x0057EE30
		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
			this.ClearOptionDescription(evt, listeningElement);
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x00580B94 File Offset: 0x0057ED94
		public void HandleBackButtonUsage()
		{
			this.GoBack();
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x00580C82 File Offset: 0x0057EE82
		public void ClearOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			this._descriptionText.SetText(Language.GetText("UI.WorldDescriptionDefault"));
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x00580C9C File Offset: 0x0057EE9C
		public void ShowOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			LocalizedText localizedText = null;
			GroupOptionButton<WorldGen.SecretSeed> groupOptionButton = listeningElement as GroupOptionButton<WorldGen.SecretSeed>;
			if (groupOptionButton != null)
			{
				localizedText = groupOptionButton.Description;
			}
			if (localizedText != null)
			{
				this._descriptionText.SetText(localizedText);
			}
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x00580CCB File Offset: 0x0057EECB
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			this.SetupGamepadPoints(spriteBatch);
			this.DrawSeedSystems(spriteBatch);
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x00580CE2 File Offset: 0x0057EEE2
		public void DrawSeedSystems(SpriteBatch spriteBatch)
		{
			this.SeedDust.UpdateDust();
			this.SeedDust.DrawDust();
			this.SeedParticleSystem.Update();
			this.SeedParticleSystem.Draw(spriteBatch);
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x00580D14 File Offset: 0x0057EF14
		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
			int num = 3000;
			int idRangeEndExclusive = num;
			this.GetSnapPoints();
			UILinkPoint linkPoint = this._helper.GetLinkPoint(idRangeEndExclusive++, this._backButton);
			List<SnapPoint> snapPoints = this._worldList.GetSnapPoints();
			UILinkPoint[,] array = this._helper.CreateUILinkPointGrid(ref idRangeEndExclusive, snapPoints, 1, null, null, null, linkPoint);
			UILinkPoint upSide = array[0, array.GetLength(1) - 1];
			this._helper.PairUpDown(upSide, linkPoint);
			this._helper.MoveToVisuallyClosestPoint(num, idRangeEndExclusive);
		}

		// Token: 0x040052C1 RID: 21185
		private UIWorldCreationAdvanced _creationState;

		// Token: 0x040052C2 RID: 21186
		private UIElement _backButton;

		// Token: 0x040052C3 RID: 21187
		private UIList _worldList;

		// Token: 0x040052C4 RID: 21188
		private UIElement _containerPanel;

		// Token: 0x040052C5 RID: 21189
		private UIScrollbar _scrollbar;

		// Token: 0x040052C6 RID: 21190
		private bool _isScrollbarAttached;

		// Token: 0x040052C7 RID: 21191
		private UIWorldCreation _creationState2;

		// Token: 0x040052C8 RID: 21192
		private ParticleRenderer SeedParticleSystem = new ParticleRenderer();

		// Token: 0x040052C9 RID: 21193
		private UIDust SeedDust = new UIDust();

		// Token: 0x040052CA RID: 21194
		private UIText _descriptionText;

		// Token: 0x040052CB RID: 21195
		private UIGamepadHelper _helper;
	}
}
