using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Steamworks;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
	// Token: 0x02000382 RID: 898
	public class GlyphTagHandler : ITagHandler
	{
		// Token: 0x060029A4 RID: 10660 RVA: 0x0057D879 File Offset: 0x0057BA79
		public static TextSnippet GetGlyph(string keyName)
		{
			return new GlyphTagHandler.GlyphSnippet(keyName);
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x0057D884 File Offset: 0x0057BA84
		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			int num;
			if (!int.TryParse(text, out num) || num >= 26)
			{
				return new TextSnippet(text);
			}
			return new GlyphTagHandler.GlyphSnippet(num)
			{
				DeleteWhole = true,
				Text = "[g:" + num + "]"
			};
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x0057D8D0 File Offset: 0x0057BAD0
		public static string GenerateTag(int index)
		{
			string text = "[g";
			return string.Concat(new object[]
			{
				text,
				":",
				index,
				"]"
			});
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x0057D910 File Offset: 0x0057BB10
		public static string GenerateTag(string keyname)
		{
			int index;
			if (GlyphTagHandler.GlyphIndexes.TryGetValue(keyname, out index))
			{
				return GlyphTagHandler.GenerateTag(index);
			}
			return keyname;
		}

		// Token: 0x04005287 RID: 21127
		private const int GlyphsPerLine = 25;

		// Token: 0x04005288 RID: 21128
		private const int MaxGlyphs = 26;

		// Token: 0x04005289 RID: 21129
		public static float GlyphsScale = 1f;

		// Token: 0x0400528A RID: 21130
		public const int DefaultGlyphStyle = -1;

		// Token: 0x0400528B RID: 21131
		public static int GlyphStyle = -1;

		// Token: 0x0400528C RID: 21132
		private static Dictionary<string, int> GlyphIndexes = new Dictionary<string, int>
		{
			{
				Buttons.A.ToString(),
				0
			},
			{
				Buttons.B.ToString(),
				1
			},
			{
				Buttons.Back.ToString(),
				4
			},
			{
				Buttons.DPadDown.ToString(),
				15
			},
			{
				Buttons.DPadLeft.ToString(),
				14
			},
			{
				Buttons.DPadRight.ToString(),
				13
			},
			{
				Buttons.DPadUp.ToString(),
				16
			},
			{
				Buttons.LeftShoulder.ToString(),
				6
			},
			{
				Buttons.LeftStick.ToString(),
				10
			},
			{
				Buttons.LeftThumbstickDown.ToString(),
				20
			},
			{
				Buttons.LeftThumbstickLeft.ToString(),
				17
			},
			{
				Buttons.LeftThumbstickRight.ToString(),
				18
			},
			{
				Buttons.LeftThumbstickUp.ToString(),
				19
			},
			{
				Buttons.LeftTrigger.ToString(),
				8
			},
			{
				Buttons.RightShoulder.ToString(),
				7
			},
			{
				Buttons.RightStick.ToString(),
				11
			},
			{
				Buttons.RightThumbstickDown.ToString(),
				24
			},
			{
				Buttons.RightThumbstickLeft.ToString(),
				21
			},
			{
				Buttons.RightThumbstickRight.ToString(),
				22
			},
			{
				Buttons.RightThumbstickUp.ToString(),
				23
			},
			{
				Buttons.RightTrigger.ToString(),
				9
			},
			{
				Buttons.Start.ToString(),
				5
			},
			{
				Buttons.X.ToString(),
				2
			},
			{
				Buttons.Y.ToString(),
				3
			},
			{
				"RightStickAxis",
				12
			},
			{
				"LR",
				25
			}
		};

		// Token: 0x020008D8 RID: 2264
		public class GlyphXboxTagHandler : ITagHandler
		{
			// Token: 0x0600467E RID: 18046 RVA: 0x006C6DA4 File Offset: 0x006C4FA4
			TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
			{
				int num;
				if (!int.TryParse(text, out num) || num >= 26)
				{
					return new TextSnippet(text);
				}
				return new GlyphTagHandler.GlyphSnippet(num)
				{
					ForcedStyle = 0,
					DeleteWhole = true,
					Text = "[gx:" + num + "]"
				};
			}
		}

		// Token: 0x020008D9 RID: 2265
		public class GlyphPSTagHandler : ITagHandler
		{
			// Token: 0x06004680 RID: 18048 RVA: 0x006C6DF8 File Offset: 0x006C4FF8
			TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
			{
				int num;
				if (!int.TryParse(text, out num) || num >= 26)
				{
					return new TextSnippet(text);
				}
				return new GlyphTagHandler.GlyphSnippet(num)
				{
					ForcedStyle = 1,
					DeleteWhole = true,
					Text = "[gp:" + num + "]"
				};
			}
		}

		// Token: 0x020008DA RID: 2266
		public class GlyphSwitchTagHandler : ITagHandler
		{
			// Token: 0x06004682 RID: 18050 RVA: 0x006C6E4C File Offset: 0x006C504C
			TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
			{
				int num;
				if (!int.TryParse(text, out num) || num >= 26)
				{
					return new TextSnippet(text);
				}
				return new GlyphTagHandler.GlyphSnippet(num)
				{
					ForcedStyle = 2,
					DeleteWhole = true,
					Text = "[gn:" + num + "]"
				};
			}
		}

		// Token: 0x020008DB RID: 2267
		public class GlyphSnippet : TextSnippet
		{
			// Token: 0x06004684 RID: 18052 RVA: 0x006C6E9E File Offset: 0x006C509E
			public GlyphSnippet(int index) : base("")
			{
				this._glyphIndex = index;
				this.Color = Color.White;
			}

			// Token: 0x06004685 RID: 18053 RVA: 0x006C6EC4 File Offset: 0x006C50C4
			public GlyphSnippet(string keyName) : base("")
			{
				GlyphTagHandler.GlyphIndexes.TryGetValue(keyName, out this._glyphIndex);
				this.Color = Color.White;
			}

			// Token: 0x06004686 RID: 18054 RVA: 0x006C6EF8 File Offset: 0x006C50F8
			private static int GetAutoRow()
			{
				SteamInput.RunFrame(true);
				int result = 0;
				InputHandle_t controllerForGamepadIndex = SteamInput.GetControllerForGamepadIndex(0);
				if (controllerForGamepadIndex.m_InputHandle != 0UL)
				{
					switch (SteamInput.GetInputTypeForHandle(controllerForGamepadIndex))
					{
					case 5:
					case 12:
					case 13:
						result = 1;
						break;
					case 8:
					case 9:
					case 10:
						result = 2;
						break;
					}
				}
				return result;
			}

			// Token: 0x06004687 RID: 18055 RVA: 0x006C6F58 File Offset: 0x006C5158
			public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
			{
				scale *= GlyphTagHandler.GlyphsScale;
				if (!justCheckingString && color != Color.Black)
				{
					int num = this.ForcedStyle;
					int num2;
					if (num == -1)
					{
						num2 = GlyphTagHandler.GlyphStyle;
						if (num2 == -1)
						{
							num = GlyphTagHandler.GlyphSnippet.GetAutoRow();
						}
						else
						{
							num = GlyphTagHandler.GlyphStyle;
						}
					}
					int frameX = this._glyphIndex;
					num2 = this._glyphIndex;
					if (num2 == 25)
					{
						frameX = ((Main.GlobalTimeWrappedHourly % 0.6f < 0.3f) ? 17 : 18);
					}
					Texture2D value = TextureAssets.TextGlyph[0].Value;
					spriteBatch.Draw(value, position, new Rectangle?(value.Frame(25, 3, frameX, num, 0, 0)), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
				}
				size = new Vector2(26f) * scale;
				return true;
			}

			// Token: 0x04007354 RID: 29524
			public int ForcedStyle = -1;

			// Token: 0x04007355 RID: 29525
			private int _glyphIndex;
		}
	}
}
