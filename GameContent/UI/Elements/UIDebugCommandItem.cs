using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.Testing.ChatCommands;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003F2 RID: 1010
	public class UIDebugCommandItem : UIPanel
	{
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06002E90 RID: 11920 RVA: 0x005AB525 File Offset: 0x005A9725
		// (set) Token: 0x06002E91 RID: 11921 RVA: 0x005AB52D File Offset: 0x005A972D
		public int Order { get; set; }

		// Token: 0x06002E92 RID: 11922 RVA: 0x005AB538 File Offset: 0x005A9738
		public UIDebugCommandItem(IDebugCommand command, int order)
		{
			this.Command = command;
			this.Order = order;
			this.Height.Set(30f, 0f);
			this.Width.Set(0f, 1f);
			base.SetPadding(6f);
			this.BorderColor = Color.Transparent;
			this.BackgroundColor = Color.Transparent;
			this._dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider", 1);
			this._innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", 1);
			this._hoverInfoLabel = new UIText("", 1f, false);
			this._hoverInfoLabel.VAlign = 1f;
			this._hoverInfoLabel.Left.Set(80f, 0f);
			this._hoverInfoLabel.Top.Set(-3f, 0f);
			base.Append(this._hoverInfoLabel);
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x005AB638 File Offset: 0x005A9838
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			string name = this.Command.Name;
			string text = this.Command.Description ?? "";
			string helpText = this.Command.HelpText;
			"Authority:  " + this.Command.Requirements;
			base.DrawSelf(spriteBatch);
			if (base.IsMouseHovering)
			{
				Item item = Main.DisplayAndGetFakeItem(ItemRarityColor.StrongRed10);
				item.SetNameOverride(name);
				item.ToolTip = this._preparedTooltip;
			}
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			Vector2 vector = innerDimensions.Position() - innerDimensions.Position();
			float num = 6f;
			float num2 = vector.X + num;
			float num3 = 21f;
			FontAssets.MouseText.Value.MeasureString(name);
			Color color = Color.White;
			Color color2 = Color.Gold;
			if (!this.CanCurrentlyBeUsed())
			{
				color = Color.DarkGray;
				color2 = Color.DarkGray;
			}
			Utils.DrawBorderString(spriteBatch, name, innerDimensions.Position() + new Vector2(num2 + 6f, vector.Y - 2f), color, 1.1f, 0f, 0f, -1);
			Utils.DrawBorderString(spriteBatch, text, innerDimensions.Position() + new Vector2(num2 + 6f + 180f + 16f, vector.Y + 2f + num3), color2, 0.8f, 0f, 1f, -1);
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x005AB7AB File Offset: 0x005A99AB
		private bool CanCurrentlyBeUsed()
		{
			return (this.Command.Requirements & ~CommandRequirement.SinglePlayer) != (CommandRequirement)0 || Main.netMode == 0;
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x005AB7CC File Offset: 0x005A99CC
		public override int CompareTo(object obj)
		{
			return this.Order.CompareTo(((UIDebugCommandItem)obj).Order);
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x005AB7F4 File Offset: 0x005A99F4
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			this.BackgroundColor = new Color(76, 90, 149);
			this.BorderColor = new Color(50, 60, 86);
			ItemTooltip preparedTooltip = this._preparedTooltip;
			string item = FontAssets.ItemStack.Value.CreateWrappedText((this.Command.Description ?? "").Replace("\n", " "), 480f, Language.ActiveCulture.CultureInfo);
			List<string> list = new List<string>
			{
				item
			};
			list.Add(" ");
			list.Add("Authority:  " + this.Command.Requirements);
			this._preparedTooltip = ItemTooltip.FromHardcodedText(list.ToArray());
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x005AB8C0 File Offset: 0x005A9AC0
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this.BackgroundColor = (this.BorderColor = Color.Transparent);
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x005AB8E8 File Offset: 0x005A9AE8
		public override void LeftClick(UIMouseEvent evt)
		{
			IngameFancyUI.Close(false);
			Main.drawingPlayerChat = true;
			Main.chatText = "/" + this.Command.Name.ToLower() + " ";
			Main.NewText("Chat has been set to \"" + Main.chatText + "\"", byte.MaxValue, byte.MaxValue, 0);
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x005AB94C File Offset: 0x005A9B4C
		private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
		{
			spriteBatch.Draw(this._innerPanelTexture.Value, position, new Rectangle?(new Rectangle(0, 0, 8, this._innerPanelTexture.Height())), Color.White);
			spriteBatch.Draw(this._innerPanelTexture.Value, new Vector2(position.X + 8f, position.Y), new Rectangle?(new Rectangle(8, 0, 8, this._innerPanelTexture.Height())), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(this._innerPanelTexture.Value, new Vector2(position.X + width - 8f, position.Y), new Rectangle?(new Rectangle(16, 0, 8, this._innerPanelTexture.Height())), Color.White);
		}

		// Token: 0x040055A2 RID: 21922
		public readonly IDebugCommand Command;

		// Token: 0x040055A4 RID: 21924
		private readonly Asset<Texture2D> _dividerTexture;

		// Token: 0x040055A5 RID: 21925
		private readonly Asset<Texture2D> _innerPanelTexture;

		// Token: 0x040055A6 RID: 21926
		private readonly UIText _hoverInfoLabel;

		// Token: 0x040055A7 RID: 21927
		private ItemTooltip _preparedTooltip;
	}
}
