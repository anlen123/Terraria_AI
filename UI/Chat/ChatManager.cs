using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;
using Terraria.Localization;
using Terraria.Testing.ChatCommands;

namespace Terraria.UI.Chat
{
	// Token: 0x0200010B RID: 267
	public static class ChatManager
	{
		// Token: 0x06001A7E RID: 6782 RVA: 0x004F60DC File Offset: 0x004F42DC
		public static Color WaveColor(Color color)
		{
			float num = (float)Main.mouseTextColor / 255f;
			color = Color.Lerp(color, Color.Black, 1f - num);
			color.A = Main.mouseTextColor;
			return color;
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x004F6118 File Offset: 0x004F4318
		public static void ConvertNormalSnippets(List<TextSnippet> snippets)
		{
			for (int i = 0; i < snippets.Count; i++)
			{
				TextSnippet textSnippet = snippets[i];
				if (textSnippet.GetType() == typeof(TextSnippet))
				{
					snippets[i] = new PlainTagHandler.PlainSnippet(textSnippet.Text, textSnippet.Color);
				}
			}
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x004F6170 File Offset: 0x004F4370
		public static void Register<T>(params string[] names) where T : ITagHandler, new()
		{
			T t = Activator.CreateInstance<T>();
			for (int i = 0; i < names.Length; i++)
			{
				ChatManager._handlers[names[i].ToLower()] = t;
			}
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x004F61AC File Offset: 0x004F43AC
		private static ITagHandler GetHandler(string tagName)
		{
			string key = tagName.ToLower();
			if (ChatManager._handlers.ContainsKey(key))
			{
				return ChatManager._handlers[key];
			}
			return null;
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x004F61DA File Offset: 0x004F43DA
		public static bool MayNeedParsing(string text)
		{
			return text.IndexOf('\r') >= 0 || ChatManager.Regexes.Format.IsMatch(text);
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x004F61F4 File Offset: 0x004F43F4
		public static List<TextSnippet> ParseMessage(string text, Color baseColor)
		{
			text = text.Replace("\r", "");
			MatchCollection matchCollection = ChatManager.Regexes.Format.Matches(text);
			List<TextSnippet> list = new List<TextSnippet>();
			int num = 0;
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				if (match.Index > num)
				{
					list.Add(new TextSnippet(text.Substring(num, match.Index - num), baseColor));
				}
				num = match.Index + match.Length;
				string value = match.Groups["tag"].Value;
				string text2 = match.Groups["text"].Value.Replace("\\]", "]");
				string value2 = match.Groups["options"].Value;
				ITagHandler handler = ChatManager.GetHandler(value);
				if (handler != null)
				{
					list.Add(handler.Parse(text2, baseColor, value2));
					list[list.Count - 1].TextOriginal = match.ToString();
				}
				else
				{
					list.Add(new TextSnippet(text2, baseColor));
				}
			}
			if (text.Length > num)
			{
				list.Add(new TextSnippet(text.Substring(num, text.Length - num), baseColor));
			}
			return list;
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x004F6360 File Offset: 0x004F4560
		public static bool AddChatText(DynamicSpriteFont font, string text, Vector2 baseScale)
		{
			int num = Main.screenWidth - 330;
			if (ChatManager.GetStringSize(font, Main.chatText + text, baseScale, -1f).X > (float)num)
			{
				return false;
			}
			Main.chatText += text;
			return true;
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x004F63B2 File Offset: 0x004F45B2
		public static IEnumerable<PositionedSnippet> LayoutSnippets(DynamicSpriteFont font, IEnumerable<TextSnippet> snippets, Vector2 scale, float maxWidth = -1f)
		{
			int line = 0;
			Vector2 pos = Vector2.Zero;
			float uniqueDrawScale = Math.Min(scale.X, scale.Y);
			int i = 0;
			foreach (TextSnippet snippet in snippets)
			{
				Vector2 size;
				int num;
				if (snippet.UniqueDraw(true, out size, null, default(Vector2), default(Color), uniqueDrawScale))
				{
					if (maxWidth >= 0f && pos.X + size.X > maxWidth)
					{
						pos.X = 0f;
						pos.Y += (float)font.LineSpacing * scale.Y;
						num = line;
						line = num + 1;
					}
					yield return new PositionedSnippet(snippet, i, line, pos, size);
					pos.X += size.X;
				}
				else
				{
					string text = font.CreateWrappedText(snippet.Text, scale.X, maxWidth, pos.X, Language.ActiveCulture.CultureInfo);
					int num2 = 0;
					for (;;)
					{
						int sep = text.IndexOf('\n', num2);
						int num3 = ((sep < 0) ? text.Length : sep) - num2;
						if (num3 > 0)
						{
							string text2 = text.Substring(num2, num3);
							size = font.MeasureString(text2) * scale;
							yield return new PositionedSnippet(snippet.CopyMorph(text2), i, line, pos, size);
							pos.X += size.X;
						}
						if (sep < 0)
						{
							break;
						}
						pos.X = 0f;
						pos.Y += (float)font.LineSpacing * scale.Y;
						num = line;
						line = num + 1;
						num2 = sep + 1;
					}
					text = null;
				}
				num = i;
				i = num + 1;
				size = default(Vector2);
				snippet = null;
			}
			IEnumerator<TextSnippet> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x004F63D7 File Offset: 0x004F45D7
		public static Vector2 GetStringSize(DynamicSpriteFont font, string text, Vector2 baseScale, float maxWidth = -1f)
		{
			return ChatManager.GetStringSize(font, ChatManager.ParseMessage(text, Color.White), baseScale, maxWidth);
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x004F63EC File Offset: 0x004F45EC
		public static Vector2 GetStringSize(DynamicSpriteFont font, IEnumerable<TextSnippet> snippets, Vector2 scale, float maxWidth = -1f)
		{
			return ChatManager.GetStringSize(ChatManager.LayoutSnippets(font, snippets, scale, maxWidth));
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x004F63FC File Offset: 0x004F45FC
		public static Vector2 GetStringSize(IEnumerable<PositionedSnippet> snippets)
		{
			Vector2 zero = Vector2.Zero;
			foreach (PositionedSnippet positionedSnippet in snippets)
			{
				zero.X = Math.Max(zero.X, positionedSnippet.Position.X + positionedSnippet.Size.X);
				zero.Y = Math.Max(zero.Y, positionedSnippet.Position.Y + positionedSnippet.Size.Y);
			}
			return zero;
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x004F6498 File Offset: 0x004F4698
		public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, IEnumerable<TextSnippet> snippets, Vector2 position, Color shadowColor, float rotation, Vector2 origin, Vector2 scale, float maxWidth = -1f, float spread = 2f)
		{
			List<PositionedSnippet> snippets2 = ChatManager.LayoutSnippets(font, snippets, scale, maxWidth).ToList<PositionedSnippet>();
			ChatManager.DrawColorCodedStringShadow(spriteBatch, font, snippets2, position, shadowColor, rotation, origin, scale, spread);
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x004F64CC File Offset: 0x004F46CC
		public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, List<PositionedSnippet> snippets, Vector2 position, Color shadowColor, float rotation, Vector2 origin, Vector2 scale, float spread = 2f)
		{
			for (int i = 0; i < ChatManager.ShadowDirections.Length; i++)
			{
				int num;
				ChatManager.DrawColorCodedString(spriteBatch, font, snippets, position + ChatManager.ShadowDirections[i] * spread, rotation, origin, scale, out num, new Color?(shadowColor));
			}
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x004F651C File Offset: 0x004F471C
		public static void DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, IEnumerable<TextSnippet> snippets, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 scale, out int hoveredSnippet, float maxWidth = -1f, bool ignoreColors = false)
		{
			ChatManager.DrawColorCodedString(spriteBatch, font, ChatManager.LayoutSnippets(font, snippets, scale, maxWidth), position, rotation, origin, scale, out hoveredSnippet, ignoreColors ? new Color?(baseColor) : null);
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x004F655C File Offset: 0x004F475C
		public static void DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, IEnumerable<TextSnippet> snippets, Vector2 position, float rotation, Vector2 origin, Vector2 scale, out int hoveredSnippet, float maxWidth = -1f)
		{
			ChatManager.DrawColorCodedString(spriteBatch, font, ChatManager.LayoutSnippets(font, snippets, scale, maxWidth), position, rotation, origin, scale, out hoveredSnippet, null);
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x004F6590 File Offset: 0x004F4790
		public static void DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, IEnumerable<PositionedSnippet> snippets, Vector2 position, float rotation, Vector2 origin, Vector2 scale, out int hoveredSnippet, Color? colorOverride = null)
		{
			hoveredSnippet = -1;
			Vector2 vec = new Vector2((float)Main.mouseX, (float)Main.mouseY);
			float scale2 = Math.Min(scale.X, scale.Y);
			foreach (PositionedSnippet positionedSnippet in snippets)
			{
				Vector2 vector = position + positionedSnippet.Position;
				TextSnippet snippet = positionedSnippet.Snippet;
				Color color = (colorOverride != null) ? colorOverride.Value : snippet.GetVisibleColor();
				Vector2 vector2;
				if (!snippet.UniqueDraw(false, out vector2, spriteBatch, vector, color, scale2))
				{
					DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, snippet.Text, vector, color, rotation, origin, scale, SpriteEffects.None, 0f);
				}
				if (vec.Between(vector, vector + positionedSnippet.Size))
				{
					hoveredSnippet = positionedSnippet.OrigIndex;
				}
			}
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x004F6684 File Offset: 0x004F4884
		public static void DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, float rotation, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth = -1f, float spread = 2f)
		{
			ChatManager.DrawColorCodedStringShadow(spriteBatch, font, snippets, position, Color.Black, rotation, origin, baseScale, maxWidth, spread);
			ChatManager.DrawColorCodedString(spriteBatch, font, snippets, position, rotation, origin, baseScale, out hoveredSnippet, maxWidth);
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x004F66BC File Offset: 0x004F48BC
		public static void DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth = -1f, float spread = 2f)
		{
			ChatManager.DrawColorCodedStringShadow(spriteBatch, font, snippets, position, color.MultiplyRGBA(Color.Black), rotation, origin, baseScale, maxWidth, spread);
			ChatManager.DrawColorCodedString(spriteBatch, font, snippets, position, color, rotation, origin, baseScale, out hoveredSnippet, maxWidth, true);
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x004F6700 File Offset: 0x004F4900
		public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
		{
			for (int i = 0; i < ChatManager.ShadowDirections.Length; i++)
			{
				ChatManager.DrawColorCodedString(spriteBatch, font, text, position + ChatManager.ShadowDirections[i] * spread, baseColor, rotation, origin, baseScale, maxWidth, true);
			}
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x004F674C File Offset: 0x004F494C
		public static Vector2 DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, bool ignoreColors = false)
		{
			Vector2 vector = position;
			Vector2 vector2 = vector;
			string[] array = text.Split(new char[]
			{
				'\n'
			});
			float x = font.MeasureString(" ").X;
			Color color = baseColor;
			float num = 1f;
			float num2 = 0f;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				foreach (string text2 in array2[i].Split(new char[]
				{
					':'
				}))
				{
					if (text2.StartsWith("sss"))
					{
						if (text2.StartsWith("sss1"))
						{
							if (!ignoreColors)
							{
								color = Color.Red;
							}
						}
						else if (text2.StartsWith("sss2"))
						{
							if (!ignoreColors)
							{
								color = Color.Blue;
							}
						}
						else if (text2.StartsWith("sssr") && !ignoreColors)
						{
							color = Color.White;
						}
					}
					else
					{
						string[] array4 = text2.Split(new char[]
						{
							' '
						});
						for (int k = 0; k < array4.Length; k++)
						{
							if (k != 0)
							{
								vector.X += x * baseScale.X * num;
							}
							if (maxWidth > 0f)
							{
								float num3 = font.MeasureString(array4[k]).X * baseScale.X * num;
								if (vector.X - position.X + num3 > maxWidth)
								{
									vector.X = position.X;
									vector.Y += (float)font.LineSpacing * num2 * baseScale.Y;
									vector2.Y = Math.Max(vector2.Y, vector.Y);
									num2 = 0f;
								}
							}
							if (num2 < num)
							{
								num2 = num;
							}
							DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, array4[k], vector, color, rotation, origin, baseScale * num, SpriteEffects.None, 0f);
							vector.X += font.MeasureString(array4[k]).X * baseScale.X * num;
							vector2.X = Math.Max(vector2.X, vector.X);
						}
					}
				}
				vector.X = position.X;
				vector.Y += (float)font.LineSpacing * num2 * baseScale.Y;
				vector2.Y = Math.Max(vector2.Y, vector.Y);
				num2 = 0f;
			}
			return vector2;
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x004F69D4 File Offset: 0x004F4BD4
		public static void DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 scale, float maxWidth = -1f, float spread = 2f)
		{
			Color color = baseColor.MultiplyRGBA(Color.Black);
			if (maxWidth < 0f && !ChatManager.MayNeedParsing(text))
			{
				foreach (Vector2 value in ChatManager.ShadowDirections)
				{
					DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, text, position + value * spread, color, rotation, origin, scale, SpriteEffects.None, 0f);
				}
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, text, position, baseColor, rotation, origin, scale, SpriteEffects.None, 0f);
				return;
			}
			List<TextSnippet> snippets = ChatManager.ParseMessage(text, baseColor);
			ChatManager.ConvertNormalSnippets(snippets);
			List<PositionedSnippet> snippets2 = ChatManager.LayoutSnippets(font, snippets, scale, maxWidth).ToList<PositionedSnippet>();
			ChatManager.DrawColorCodedStringShadow(spriteBatch, font, snippets2, position, color, rotation, origin, scale, spread);
			int num;
			ChatManager.DrawColorCodedString(spriteBatch, font, snippets2, position, rotation, origin, scale, out num, null);
		}

		// Token: 0x040014EA RID: 5354
		public static readonly DebugCommandProcessor DebugCommands = new DebugCommandProcessor();

		// Token: 0x040014EB RID: 5355
		public static readonly ChatCommandProcessor Commands = new ChatCommandProcessor();

		// Token: 0x040014EC RID: 5356
		private static ConcurrentDictionary<string, ITagHandler> _handlers = new ConcurrentDictionary<string, ITagHandler>();

		// Token: 0x040014ED RID: 5357
		public static readonly Vector2[] ShadowDirections = new Vector2[]
		{
			-Vector2.UnitX,
			Vector2.UnitX,
			-Vector2.UnitY,
			Vector2.UnitY
		};

		// Token: 0x0200071A RID: 1818
		public static class Regexes
		{
			// Token: 0x040068FC RID: 26876
			public static readonly Regex Format = new Regex("(?<!\\\\)\\[(?<tag>[a-zA-Z]{1,10})(\\/(?<options>[^:]+))?:(?<text>.+?)(?<!\\\\)\\]", RegexOptions.Compiled | RegexOptions.Singleline);
		}
	}
}
