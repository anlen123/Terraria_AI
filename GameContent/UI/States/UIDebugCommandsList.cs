using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Testing.ChatCommands;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003AF RID: 943
	public class UIDebugCommandsList : UIState
	{
		// Token: 0x06002C1F RID: 11295 RVA: 0x005959AD File Offset: 0x00593BAD
		public UIDebugCommandsList()
		{
			this.BuildPage();
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x00009E06 File Offset: 0x00008006
		public override void OnDeactivate()
		{
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x005959D4 File Offset: 0x00593BD4
		private void BuildPage()
		{
			base.RemoveAllChildren();
			UIElement uielement = new UIElement();
			uielement.Width.Set(0f, 0.8f);
			uielement.MaxWidth.Set(800f, 0f);
			uielement.MinWidth.Set(600f, 0f);
			uielement.Top.Set(220f, 0f);
			uielement.Height.Set(-220f, 1f);
			uielement.HAlign = 0.5f;
			base.Append(uielement);
			UIPanel uipanel = new UIPanel();
			uipanel.Width.Set(0f, 1f);
			uipanel.Height.Set(-110f, 1f);
			uipanel.BackgroundColor = new Color(33, 43, 79) * 0.95f;
			uielement.Append(uipanel);
			UIWrappedSearchBar uiwrappedSearchBar = new UIWrappedSearchBar(delegate()
			{
				UserInterface.ActiveInstance.SetState(this);
			}, null, UIWrappedSearchBar.ColorTheme.Blue)
			{
				Width = new StyleDimension(200f, 0f),
				Top = new StyleDimension(20f, 0f)
			};
			uiwrappedSearchBar.OnSearchContentsChanged += this.searchbar_OnSearchContentsChanged;
			uipanel.Append(uiwrappedSearchBar);
			this._commandsList.Width.Set(-25f, 1f);
			this._commandsList.Height.Set(-60f, 1f);
			this._commandsList.VAlign = 1f;
			this._commandsList.ListPadding = 5f;
			uipanel.Append(this._commandsList);
			UITextPanel<string> uitextPanel = new UITextPanel<string>("Debug Commands", 1f, true);
			uitextPanel.HAlign = 0.5f;
			uitextPanel.Top.Set(-33f, 0f);
			uitextPanel.SetPadding(13f);
			uitextPanel.BackgroundColor = new Color(73, 94, 171);
			uielement.Append(uitextPanel);
			UITextPanel<LocalizedText> uitextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, true);
			uitextPanel2.Width.Set(-10f, 0.5f);
			uitextPanel2.Height.Set(50f, 0f);
			uitextPanel2.VAlign = 1f;
			uitextPanel2.HAlign = 0.5f;
			uitextPanel2.Top.Set(-45f, 0f);
			uitextPanel2.OnMouseOver += UIDebugCommandsList.FadedMouseOver;
			uitextPanel2.OnMouseOut += UIDebugCommandsList.FadedMouseOut;
			uitextPanel2.OnLeftClick += this.GoBackClick;
			uielement.Append(uitextPanel2);
			UIScrollbar uiscrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue);
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			uipanel.Append(uiscrollbar);
			this._commandsList.SetScrollbar(uiscrollbar);
			this.PopulateCommandsList();
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x00595CD0 File Offset: 0x00593ED0
		private void searchbar_OnSearchContentsChanged(string searchContents)
		{
			if (searchContents == null)
			{
				searchContents = string.Empty;
			}
			string text = searchContents.ToLowerInvariant().Trim();
			bool flag = string.IsNullOrWhiteSpace(text);
			this._commandsList.Clear();
			foreach (UIDebugCommandItem uidebugCommandItem in this._commands)
			{
				if (flag || UIDebugCommandsList.DoesCommandMatchSearch(text, uidebugCommandItem))
				{
					this._commandsList.Add(uidebugCommandItem);
				}
			}
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x00595D5C File Offset: 0x00593F5C
		private static bool DoesCommandMatchSearch(string lowerContents, UIDebugCommandItem command)
		{
			IDebugCommand command2 = command.Command;
			return command2.Name.ToLowerInvariant().Contains(lowerContents) || (command2.Description != null && command2.Description.ToLowerInvariant().Contains(lowerContents)) || (command2.HelpText != null && command2.HelpText.ToLowerInvariant().Contains(lowerContents));
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x00595DC0 File Offset: 0x00593FC0
		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			IngameFancyUI.Close(false);
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x00595DC8 File Offset: 0x00593FC8
		private static void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x00586A05 File Offset: 0x00584C05
		private static void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x00595E20 File Offset: 0x00594020
		private void PopulateCommandsList()
		{
			List<IDebugCommand> list = ChatManager.DebugCommands.Commands.ToList<IDebugCommand>();
			list.Sort((IDebugCommand x, IDebugCommand y) => StringComparer.OrdinalIgnoreCase.Compare(x.Name, y.Name));
			int num = 0;
			foreach (IDebugCommand command in list)
			{
				UIDebugCommandItem item = new UIDebugCommandItem(command, num++);
				this._commands.Add(item);
				this._commandsList.Add(item);
			}
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x00595EC0 File Offset: 0x005940C0
		private static void DrawMouseOver()
		{
			Item item = new Item();
			item.SetDefaults(0, null);
			item.SetNameOverride("Dev Commands");
			item.type = 1;
			item.scale = 0f;
			item.rare = 10;
			Main.HoverItem = item;
			Main.instance.MouseText("", 0, 0, -1, -1, -1, -1, 0);
			Main.mouseText = true;
		}

		// Token: 0x040053B7 RID: 21431
		private readonly UIList _commandsList = new UIList();

		// Token: 0x040053B8 RID: 21432
		private readonly List<UIDebugCommandItem> _commands = new List<UIDebugCommandItem>();
	}
}
