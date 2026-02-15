using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
	// Token: 0x02000385 RID: 901
	public class ItemTagHandler : ITagHandler
	{
		// Token: 0x060029AF RID: 10671 RVA: 0x0057DC6C File Offset: 0x0057BE6C
		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			Item item = new Item();
			int type;
			if (int.TryParse(text, out type))
			{
				item.SetDefaults(type, null);
			}
			if (item.type <= 0)
			{
				return new TextSnippet(text);
			}
			item.stack = 1;
			if (options != null)
			{
				string[] array = options.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].Length != 0)
					{
						char c = array[i][0];
						int value2;
						if (c != 'p')
						{
							int value;
							if ((c == 's' || c == 'x') && int.TryParse(array[i].Substring(1), out value))
							{
								item.stack = Utils.Clamp<int>(value, 1, item.maxStack);
							}
						}
						else if (int.TryParse(array[i].Substring(1), out value2))
						{
							item.Prefix((int)((byte)Utils.Clamp<int>(value2, 0, PrefixID.Count)));
						}
					}
				}
			}
			string str = "";
			if (item.stack > 1)
			{
				str = " (" + item.stack + ")";
			}
			return new ItemTagHandler.ItemSnippet(item)
			{
				Text = "[" + item.AffixName() + str + "]",
				CheckForHover = true,
				DeleteWhole = true
			};
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x0057DDAC File Offset: 0x0057BFAC
		public static string GenerateTag(Item I)
		{
			string text = "[i";
			if (I.prefix != 0)
			{
				text = text + "/p" + I.prefix;
			}
			if (I.stack != 1)
			{
				text = text + "/s" + I.stack;
			}
			return string.Concat(new object[]
			{
				text,
				":",
				I.type,
				"]"
			});
		}

		// Token: 0x020008DD RID: 2269
		private class ItemSnippet : TextSnippet
		{
			// Token: 0x0600468A RID: 18058 RVA: 0x006C7061 File Offset: 0x006C5261
			public ItemSnippet(Item item) : base("")
			{
				this._item = item;
				this.Color = ItemRarity.GetColor(item.rare);
			}

			// Token: 0x0600468B RID: 18059 RVA: 0x006C7088 File Offset: 0x006C5288
			public override void OnHover()
			{
				Main.HoverItem = this._item.Clone();
				Main.instance.MouseText(this._item.Name, this._item.rare, 0, -1, -1, -1, -1, 0);
			}

			// Token: 0x0600468C RID: 18060 RVA: 0x006C70CC File Offset: 0x006C52CC
			public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
			{
				if (Main.netMode != 2 && !Main.dedServ)
				{
					Main.instance.LoadItem(this._item.type);
				}
				scale *= 0.75f;
				if (!justCheckingString && color != Color.Black)
				{
					float inventoryScale = Main.inventoryScale;
					Main.inventoryScale = scale;
					ItemSlot.Draw(spriteBatch, ref this._item, 14, position - new Vector2(10f) * Main.inventoryScale, Color.White);
					Main.inventoryScale = inventoryScale;
				}
				size = new Vector2(32f) * scale;
				return true;
			}

			// Token: 0x04007357 RID: 29527
			private Item _item;
		}
	}
}
