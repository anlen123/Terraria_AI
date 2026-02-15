using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x0200039E RID: 926
	public class UITextWrappingTest : UIState
	{
		// Token: 0x06002A47 RID: 10823 RVA: 0x00582ABC File Offset: 0x00580CBC
		public UITextWrappingTest()
		{
			UIPanel uipanel = new UIPanel
			{
				Top = StyleDimension.FromPixels(100f),
				Left = StyleDimension.FromPixelsAndPercent(-400f, 0.5f),
				Width = StyleDimension.FromPixels(300f),
				Height = StyleDimension.FromPixels(40f),
				BackgroundColor = new Color(43, 56, 101),
				BorderColor = Color.Transparent
			};
			this.modeText = new UIText(this.mode.ToString(), 0.8f, false)
			{
				TextOriginX = 0f,
				Width = StyleDimension.FromPercent(1f),
				Height = StyleDimension.FromPercent(1f)
			};
			uipanel.Append(this.modeText);
			uipanel.OnLeftClick += delegate(UIMouseEvent e, UIElement sender)
			{
				this.CycleMode(1);
			};
			uipanel.OnRightClick += delegate(UIMouseEvent e, UIElement sender)
			{
				this.CycleMode(-1);
			};
			base.Append(uipanel);
			this.scaleText = new UIText(this.ScaleText, 0.8f, false)
			{
				TextOriginX = 0f,
				Top = StyleDimension.FromPixels(150f),
				Left = StyleDimension.FromPixelsAndPercent(-400f, 0.5f),
				Width = StyleDimension.FromPixels(300f),
				Height = StyleDimension.FromPixels(40f)
			};
			base.Append(this.scaleText);
			this.langText = new UIText(this.LangText, 0.8f, false)
			{
				TextOriginX = 1f,
				HAlign = 1f,
				Top = StyleDimension.FromPixels(150f),
				Left = StyleDimension.FromPixelsAndPercent(400f, -0.5f),
				Width = StyleDimension.FromPixels(300f),
				Height = StyleDimension.FromPixels(40f)
			};
			base.Append(this.langText);
			UIList uilist = new UIList();
			uilist.Top = StyleDimension.FromPixels(200f);
			uilist.Left = StyleDimension.FromPixelsAndPercent(-400f, 0.5f);
			uilist.Width = StyleDimension.FromPixels(300f);
			uilist.Height = StyleDimension.FromPixelsAndPercent(-200f, 1f);
			uilist.ListPadding = 5f;
			uilist.ManualSortMethod = delegate(List<UIElement> _)
			{
			};
			this.list = uilist;
			this.list.SetPadding(0f);
			base.Append(this.list);
			UIScrollbar uiscrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue);
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(-20f, 1f);
			uiscrollbar.HAlign = 1f;
			uiscrollbar.VAlign = 0.5f;
			uiscrollbar.Left.Set(6f, 0f);
			this.list.SetScrollbar(uiscrollbar);
			this.ResetList();
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x00582DBD File Offset: 0x00580FBD
		private string ScaleText
		{
			get
			{
				return "Up/Down to change scale. Current: " + this.scale + "%";
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06002A49 RID: 10825 RVA: 0x00582DD9 File Offset: 0x00580FD9
		private string LangText
		{
			get
			{
				return "Current Language: " + Language.ActiveCulture.CultureInfo.DisplayName;
			}
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x00582DF4 File Offset: 0x00580FF4
		private void CycleMode(int offset)
		{
			int length = Enum.GetValues(typeof(UITextWrappingTest.Mode)).Length;
			this.mode = (this.mode + offset + length) % (UITextWrappingTest.Mode)length;
			this.ResetList();
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x00582E30 File Offset: 0x00581030
		private void ResetList()
		{
			this.modeText.SetText(this.mode.ToString());
			this.list.Clear();
			this.list.Add(this.MakeElement("A test string in english.\nSecond line.\n\n^ Double line break\nLooooooooooooooonglinewithnospaces"));
			this.list.Add(this.MakeElement("Ends with newline\n"));
			this.list.Add(this.MakeElement("Non-breaking space: с\u00a0микротранзакциями\n"));
			this.list.Add(this.MakeElement("Thin\u2009Space\nHair\u200aSpace\nZero​Width​Space"));
			this.list.Add(this.NewSeparator());
			this.list.Add(this.MakeElement("せいなる スライムが がったいして できた 生き物。ごうまんで 力づよく きらめく けっしょうに おおわれている。つばさが 生える という うわさも ある。"));
			this.list.Add(this.MakeElement("정화된 슬라임들이 모두 통합되어, 눈부신 수정으로 장식된 거만하고 압도적인 힘이 되었습니다. 날개가 돋아난다는 소문도 있습니다. "));
			this.list.Add(this.MakeElement("Святые слизни объединяются в величественную всесокрушающую массу, украшенную превосходными кристаллами. Говорят, она даже может отрастить крылья."));
			this.list.Add(this.MakeElement("神圣史莱姆合并成了一种高傲的粉碎性力量，这种力量佩戴着闪耀的水晶。传说她会长出翅膀。"));
			this.list.Add(this.MakeElement("神聖史萊姆融合後，會點綴著閃耀的水晶，擁有傲視一切的粉碎性力量。傳說她會長出翅膀。"));
			this.list.Add(this.NewSeparator());
			this.list.Add(this.MakeElement("fullwidth terminators。bang！comma，fullstop。rcomma、colon：question？"));
			this.list.Add(this.MakeElement("Chinese separation〈聖聖聖聖〉《聖聖》「聖聖」『聖聖』【聖聖〔聖聖】〖聖聖〗!%),.:;?]}$100,25.24%"));
			this.list.Add(this.NewSeparator());
			this.list.Add(this.MakeElement(new LocalizedText("", "Keybind glyph support {InputTrigger_UseOrAttack} and {InputTrigger_InteractWithTile}").Value));
			this.list.Add(this.MakeElement("[c/FF0000:SomeRedText] [c/00FF00:SomeGreenText] [c/0000FF:SomeBlueText]"));
			this.list.Add(this.MakeElement("[c/FF0000:SomeRedText][c/00FF00:SomeGreenText][c/0000FF:SomeBlueText]"));
			this.list.Add(this.MakeElement("[c/0000FF:Long colored text, with escaped square brackets [\\] inside]"));
			this.list.Add(this.MakeElement("Items[i:1][i:2][i:3][i:4][i:5][i:6][i:7][i:100][i:1000]"));
			this.list.Add(this.MakeElement("ItemsOnSeparateLines\n[i:1]\n[i:2]\n[i:3]"));
			this.list.Add(this.MakeElement("Items and text [i:1] then stuff [i:2] and some more [i:3] etc"));
			this.list.Add(this.MakeElement("nospacebetweenitems[i:6]andtext[i:7]nospacebetweenitems[i:8]andtext[i:9]"));
			this.list.Add(this.MakeElement("[g:0][g:1][g:2][g:3][g:4][g:5][g:6][g:7][g:8][g:9][g:10][g:11][g:12][g:13][g:14][g:15][g:16][g:17][g:18][g:19][g:20][g:21][g:22][g:23][g:24][g:25]"));
			this.list.Add(this.MakeElement(Language.GetTextValue("Achievements.Completed", "[a:TRANSMUTE_ITEM]")));
			this.list.Add(this.MakeElement("[a:TO_INFINITY_AND_BEYOND][a:PURIFY_ENTIRE_WORLD][a:TO_INFINITY_AND_BEYOND][a:TRANSMUTE_ITEM][a:OBTAIN_HAMMER][a:BENCHED][a:HEAVY_METAL][a:GET_GOLDEN_DELIGHT][a:MINER_FOR_FIRE][a:HEAD_IN_THE_CLOUDS][a:GET_TERRASPARK_BOOTS]"));
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x00583094 File Offset: 0x00581294
		private UIElement NewSeparator()
		{
			return new UIHorizontalSeparator(2, true)
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Color = new Color(89, 116, 213, 255) * 0.9f
			};
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x005830E0 File Offset: 0x005812E0
		private UIElement MakeElement(string value)
		{
			UIElement container = new UIPanel
			{
				Width = StyleDimension.FromPercent(1f),
				Height = StyleDimension.FromPixels((float)(50 * this.scale)),
				BackgroundColor = new Color(43, 56, 101),
				BorderColor = Color.Transparent
			};
			container.SetPadding(UITextWrappingTest.TextPadding);
			if (this.mode == UITextWrappingTest.Mode.UIText)
			{
				UIText text = new UIText(value, (float)this.scale / 100f, false)
				{
					TextOriginX = 0f,
					HAlign = 0f,
					VAlign = 0f,
					Width = StyleDimension.FromPercent(1f),
					Height = StyleDimension.FromPercent(1f),
					IsWrapped = true
				};
				text.OnInternalTextChange += delegate()
				{
					container.Height = new StyleDimension(text.MinHeight.Pixels, 0f);
				};
				container.Append(text);
			}
			else
			{
				UITextWrappingTest.TestElement text = new UITextWrappingTest.TestElement(value, (float)this.scale / 100f, this.mode)
				{
					Width = StyleDimension.FromPercent(1f)
				};
				UITextWrappingTest.TestElement text2 = text;
				text2.OnHeightUpdate = (Action)Delegate.Combine(text2.OnHeightUpdate, new Action(delegate()
				{
					container.Height = new StyleDimension(text.MinHeight.Pixels + container.PaddingTop + container.PaddingBottom, 0f);
				}));
				container.Append(text);
			}
			return container;
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x00583278 File Offset: 0x00581478
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			CalculatedStyle dimensions = this.list.GetDimensions();
			int x = (int)(dimensions.X + UITextWrappingTest.TextPadding);
			int x2 = (int)(dimensions.X + dimensions.Width - UITextWrappingTest.TextPadding);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(x, (int)dimensions.Y, 1, (int)dimensions.Height), Color.Green);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(x2, (int)dimensions.Y, 1, (int)dimensions.Height), Color.Green);
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x00583310 File Offset: 0x00581510
		public override void Update(GameTime gameTime)
		{
			if (Main.keyState.IsKeyDown(Keys.Escape))
			{
				SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
				Main.menuMode = 0;
			}
			int num = 0;
			if (Main.keyState.IsKeyDown(Keys.Down) && Main.oldKeyState.IsKeyUp(Keys.Down))
			{
				num = -10;
			}
			if (Main.keyState.IsKeyDown(Keys.Up) && Main.oldKeyState.IsKeyUp(Keys.Up))
			{
				num = 10;
			}
			if (num != 0)
			{
				this.scale = Utils.Clamp<int>(this.scale + num, 50, 150);
				this.ResetList();
				this.scaleText.SetText(this.ScaleText);
			}
			this.langText.SetText(this.LangText);
			if (Main.mouseLeft)
			{
				Point point = Main.MouseScreen.ToPoint();
				CalculatedStyle dimensions = this.list.GetDimensions();
				if ((float)point.X > dimensions.X && (float)point.Y > dimensions.Y)
				{
					this.list.Width = StyleDimension.FromPixels((float)point.X - dimensions.X);
				}
			}
			base.Update(gameTime);
		}

		// Token: 0x040052E4 RID: 21220
		private static readonly float TextPadding = 12f;

		// Token: 0x040052E5 RID: 21221
		private UIList list;

		// Token: 0x040052E6 RID: 21222
		private UIText modeText;

		// Token: 0x040052E7 RID: 21223
		private UIText scaleText;

		// Token: 0x040052E8 RID: 21224
		private UIText langText;

		// Token: 0x040052E9 RID: 21225
		private UITextWrappingTest.Mode mode;

		// Token: 0x040052EA RID: 21226
		private int scale = 100;

		// Token: 0x020008ED RID: 2285
		private enum Mode
		{
			// Token: 0x040073AE RID: 29614
			UIText,
			// Token: 0x040073AF RID: 29615
			SignsAndNPCChat,
			// Token: 0x040073B0 RID: 29616
			WordwrapStringLegacy,
			// Token: 0x040073B1 RID: 29617
			DrawColorCodedStringWithShadow,
			// Token: 0x040073B2 RID: 29618
			DrawColorCodedStringLegacy,
			// Token: 0x040073B3 RID: 29619
			MultilineChat
		}

		// Token: 0x020008EE RID: 2286
		private class TestElement : UIElement
		{
			// Token: 0x0600470F RID: 18191 RVA: 0x006C97E0 File Offset: 0x006C79E0
			public TestElement(string text, float scale, UITextWrappingTest.Mode mode)
			{
				this.text = text;
				this.scale = scale;
				this.mode = mode;
			}

			// Token: 0x06004710 RID: 18192 RVA: 0x006C9800 File Offset: 0x006C7A00
			protected override void DrawSelf(SpriteBatch spriteBatch)
			{
				Vector2 vector = base.GetDimensions().Position();
				float num = base.GetInnerDimensions().Width;
				if (num <= 0f)
				{
					num = 1000f;
				}
				switch (this.mode)
				{
				case UITextWrappingTest.Mode.SignsAndNPCChat:
				{
					int num2;
					string[] array = Utils.WordwrapString(this.text, FontAssets.MouseText.Value, (int)(num / this.scale), 10, out num2);
					float num3 = 30f * this.scale;
					this.MinHeight.Set((float)num2 * num3, 0f);
					this.OnHeightUpdate();
					for (int i = 0; i < num2; i++)
					{
						Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, array[i], vector.X, vector.Y + (float)i * num3, Color.White, Color.Black, Vector2.Zero, this.scale);
					}
					return;
				}
				case UITextWrappingTest.Mode.WordwrapStringLegacy:
				{
					int num4;
					string[] array2 = Utils.WordwrapStringLegacy(this.text, FontAssets.MouseText.Value, (int)(num / this.scale), 10, out num4);
					float num5 = 30f * this.scale;
					this.MinHeight.Set((float)num4 * num5, 0f);
					this.OnHeightUpdate();
					for (int j = 0; j < num4; j++)
					{
						Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, array2[j], vector.X, vector.Y + (float)j * num5, Color.White, Color.Black, Vector2.Zero, this.scale);
					}
					return;
				}
				case UITextWrappingTest.Mode.DrawColorCodedStringWithShadow:
				{
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, this.text, vector, Color.White, 0f, Vector2.Zero, new Vector2(this.scale), num, 2f);
					Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, this.text, new Vector2(this.scale), num);
					this.MinHeight.Set(stringSize.Y, 0f);
					this.OnHeightUpdate();
					return;
				}
				case UITextWrappingTest.Mode.DrawColorCodedStringLegacy:
				{
					ChatManager.DrawColorCodedStringShadow(spriteBatch, FontAssets.MouseText.Value, this.text, vector, Color.Black, 0f, Vector2.Zero, new Vector2(this.scale), num, 2f);
					ChatManager.DrawColorCodedString(spriteBatch, FontAssets.MouseText.Value, this.text, vector, Color.White, 0f, Vector2.Zero, new Vector2(this.scale), num, false);
					Vector2 stringSize2 = ChatManager.GetStringSize(FontAssets.MouseText.Value, this.text, new Vector2(this.scale), num);
					this.MinHeight.Set(stringSize2.Y, 0f);
					this.OnHeightUpdate();
					return;
				}
				case UITextWrappingTest.Mode.MultilineChat:
				{
					List<List<TextSnippet>> list = Utils.WordwrapStringSmart(this.text, Color.White, FontAssets.MouseText.Value, (float)((int)(num / this.scale)), 10);
					float num6 = 30f * this.scale;
					this.MinHeight.Set((float)list.Count * num6, 0f);
					this.OnHeightUpdate();
					for (int k = 0; k < list.Count; k++)
					{
						int num7;
						ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, list[k].ToArray(), vector + new Vector2(0f, (float)k * num6), 0f, Vector2.Zero, new Vector2(this.scale), out num7, -1f, 2f);
					}
					return;
				}
				default:
					return;
				}
			}

			// Token: 0x040073B4 RID: 29620
			private readonly string text;

			// Token: 0x040073B5 RID: 29621
			private readonly float scale;

			// Token: 0x040073B6 RID: 29622
			private readonly UITextWrappingTest.Mode mode;

			// Token: 0x040073B7 RID: 29623
			public Action OnHeightUpdate;
		}
	}
}
