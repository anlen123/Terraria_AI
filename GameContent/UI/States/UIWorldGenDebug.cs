using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using ReLogic.Content;
using ReLogic.Graphics;
using ReLogic.Threading;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Testing;
using Terraria.UI;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x0200039D RID: 925
	public class UIWorldGenDebug : UIState
	{
		// Token: 0x06002A1B RID: 10779 RVA: 0x00580D9D File Offset: 0x0057EF9D
		private static void SetButtonState(UIWorldGenDebug.UIImageButtonWithExtraIcon button, UIWorldGenDebug.ButtonState state)
		{
			if (state == UIWorldGenDebug.ButtonState.Enabled)
			{
				button.SetVisibility(1f, 0.4f);
			}
			else if (state == UIWorldGenDebug.ButtonState.NotVisible)
			{
				button.SetVisibility(0f, 0f);
			}
			button.IgnoresMouseInteraction = (state > UIWorldGenDebug.ButtonState.Enabled);
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06002A1C RID: 10780 RVA: 0x00580DD2 File Offset: 0x0057EFD2
		public static UIWorldGenDebug ActiveInstance
		{
			get
			{
				return UserInterface.ActiveInstance.CurrentState as UIWorldGenDebug;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06002A1D RID: 10781 RVA: 0x00580DE3 File Offset: 0x0057EFE3
		public static bool IsActive
		{
			get
			{
				return (Main.gameMenu ? Main.MenuUI.CurrentState : Main.InGameUI.CurrentState) is UIWorldGenDebug;
			}
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x00580E0A File Offset: 0x0057F00A
		public static void Open()
		{
			if (Main.gameMenu)
			{
				Main.MenuUI.SetState(new UIWorldGenDebug());
				return;
			}
			IngameFancyUI.OpenUIState(new UIWorldGenDebug());
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x00580E2D File Offset: 0x0057F02D
		public static void Close()
		{
			if (UIWorldGenDebug.ActiveInstance == null)
			{
				return;
			}
			if (Main.gameMenu)
			{
				Main.MenuUI.SetState(new UIWorldLoad());
				return;
			}
			IngameFancyUI.Close(false);
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06002A20 RID: 10784 RVA: 0x00580E54 File Offset: 0x0057F054
		private static WorldGenerator.Controller Controller
		{
			get
			{
				return WorldGenerator.CurrentController;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06002A21 RID: 10785 RVA: 0x00580E5B File Offset: 0x0057F05B
		private static bool CanSubmitActions
		{
			get
			{
				return UIWorldGenDebug.Controller.Paused && UIWorldGenDebug.Controller.CurrentPass == null;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06002A22 RID: 10786 RVA: 0x00580E78 File Offset: 0x0057F078
		public static GenPass CurrentTargetOrLatestPass
		{
			get
			{
				GenPass genPass = UIWorldGenDebug.Controller.PauseAfterPass;
				if (genPass == null)
				{
					genPass = UIWorldGenDebug.Controller.CurrentPass;
				}
				if (genPass == null)
				{
					genPass = UIWorldGenDebug.Controller.LastCompletedPass;
				}
				return genPass;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06002A23 RID: 10787 RVA: 0x00580EB0 File Offset: 0x0057F0B0
		public static GenPass CurrentTargetPass
		{
			get
			{
				GenPass genPass = UIWorldGenDebug.Controller.PauseAfterPass;
				if (genPass == UIWorldGenDebug.Controller.LastCompletedPass)
				{
					genPass = null;
				}
				return genPass;
			}
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x00580ED8 File Offset: 0x0057F0D8
		public UIWorldGenDebug()
		{
			this.NoGamepadSupport = true;
			this.IgnoresMouseInteraction = true;
			UIGenProgressBar progressBar = new UIGenProgressBar
			{
				VAlign = 0f,
				HAlign = 0.5f,
				Top = StyleDimension.FromPixels(20f),
				IgnoresMouseInteraction = true
			};
			base.Append(progressBar);
			UIHeader progressMessage = new UIHeader
			{
				VAlign = 0f,
				HAlign = 0.5f,
				IgnoresMouseInteraction = true
			};
			base.Append(progressMessage);
			base.OnUpdate += delegate(UIElement e)
			{
				progressBar.SetProgress((float)WorldGenerator.CurrentGenerationProgress.TotalProgress, (float)WorldGenerator.CurrentGenerationProgress.Value);
				progressMessage.Text = WorldGenerator.CurrentGenerationProgress.Message;
				if (WorldGenerator.CurrentController.QueuedAbort)
				{
					progressMessage.Text = Language.GetTextValue("UI.Canceling");
				}
				if (WorldGen.Manifest.GenPassResults.Count != this.LassPassIndex)
				{
					this.LassPassIndex = WorldGen.Manifest.GenPassResults.Count;
					this.EnsurePassVisible(this.LassPassIndex);
				}
			};
			this.controlListArea = new UIElement
			{
				Width = StyleDimension.FromPixels(450f),
				Height = StyleDimension.FromPixelsAndPercent(-60f, 1f),
				Top = StyleDimension.FromPixels(30f),
				Left = StyleDimension.FromPixels(10f)
			};
			base.Append(this.controlListArea);
			this.controlPanel = new UIPanel
			{
				Height = StyleDimension.FromPixels(50f)
			};
			this.controlPanel.SetPadding(8f);
			this.controlPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
			UIElement uielement = this.AddButton(this.controlPanel, "Images/UI/Camera_0", delegate
			{
				UIWorldGenDebug.Controller.DeleteAllSnapshots();
			}, () => "Delete all snapshots", () => string.Concat(new object[]
			{
				"Click to clear all snapshots\nEstimated Disk Usage: ",
				WorldGenSnapshot.EstimatedDiskUsage / 1024L / 1024L,
				"MB",
				UIWorldGenDebug.CanSubmitActions ? "" : "\n[c/FFA500:Must be paused to manipulate snapshots]"
			}));
			UIImage element = new UIImage(Main.Assets.Request<Texture2D>("Images/CoolDown", 1))
			{
				ScaleToFit = true,
				Width = new StyleDimension(28f, 0f),
				Height = new StyleDimension(28f, 0f),
				Left = new StyleDimension(3f, 0f),
				Top = new StyleDimension(3f, 0f)
			};
			uielement.Append(element);
			GroupOptionButton<bool> groupOptionButton = this.AddButton(this.controlPanel, "Images/UI/IconReset", delegate
			{
				UIWorldGenDebug.Controller.TryReset();
			}, () => "Reset", delegate
			{
				if (!UIWorldGenDebug.CanSubmitActions)
				{
					return "[c/FFA500:Must be paused to reset]";
				}
				return null;
			});
			groupOptionButton.IconScale = 28f / (float)groupOptionButton.Icon.Width;
			groupOptionButton.IconOffset = new Vector2(2f, 3f);
			GroupOptionButton<bool> groupOptionButton2 = this.AddButton(this.controlPanel, "Images/UI/IconPrev", new Action(this.StepBack), () => "Step Back", () => "Hotkey: Up/Left");
			groupOptionButton2.IconScale = 28f / (float)groupOptionButton2.Icon.Width;
			groupOptionButton2.IconOffset = new Vector2(2f, 3f);
			GroupOptionButton<bool> playPauseButton = this.AddButton(this.controlPanel, "Images/UI/IconPlayPause", delegate
			{
				WorldGenerator.Controller controller = UIWorldGenDebug.Controller;
				controller.Paused = !controller.Paused;
			}, delegate
			{
				if (!WorldGenerator.CurrentController.Paused)
				{
					return "Pause";
				}
				return "Play";
			}, () => "Hotkey: Space");
			playPauseButton.IconScale = 28f / (float)playPauseButton.Icon.Width;
			playPauseButton.IconOffset = new Vector2(3f, 3f);
			playPauseButton.OnUpdate += delegate(UIElement e)
			{
				playPauseButton.SetIconFrame(playPauseButton.Icon.Frame(1, 2, 0, UIWorldGenDebug.Controller.Paused ? 0 : 1, 0, 0));
			};
			GroupOptionButton<bool> groupOptionButton3 = this.AddButton(this.controlPanel, "Images/UI/IconNext", new Action(this.StepForward), () => "Step Forward", () => "Hotkey: Down/Right");
			groupOptionButton3.IconScale = 28f / (float)groupOptionButton3.Icon.Width;
			groupOptionButton3.IconOffset = new Vector2(2f, 3f);
			this.AddButton(this.controlPanel, "Images/Map_0", delegate
			{
				this.ToggleMap();
			}, () => "Toggle Map", () => "Left click to toggle the map display").IconOffset = new Vector2(4f, 5f);
			GroupOptionButton<bool> groupOptionButton4 = this.AddButton(this.controlPanel, "Images/Extra_" + 48, delegate
			{
				this.hideChat = !this.hideChat;
			}, () => "Toggle Chat", () => "Left click to toggle the chat log");
			Asset<Texture2D> asset = Main.Assets.Request<Texture2D>("Images/Extra_" + 48, 1);
			Rectangle rectangle = asset.Frame(8, EmoteBubble.EMOTE_SHEET_VERTICAL_FRAMES, 1, 0, 0, 0);
			groupOptionButton4.IconScale = 28f / (float)rectangle.Width;
			groupOptionButton4.IconOffset = new Vector2(3f, 5f);
			groupOptionButton4.SetIconFrame(rectangle);
			rectangle = asset.Frame(8, EmoteBubble.EMOTE_SHEET_VERTICAL_FRAMES, 4, 3, 0, 0);
			UIImage element2 = new UIImage(asset)
			{
				Frame = new Rectangle?(rectangle),
				ScaleToFit = true,
				Width = new StyleDimension(28f, 0f),
				Height = new StyleDimension(28f, 0f),
				Left = new StyleDimension(2f, 0f),
				Top = new StyleDimension(6f, 0f)
			};
			groupOptionButton4.Append(element2);
			GroupOptionButton<bool> snapshotFrequencyButton = this.AddButton(this.controlPanel, "Images/UI/IconSnapshotFrequency", new Action(this.CycleSnapshotMode), new Func<string>(this.GetSnapshotModeButtonTitle), null);
			snapshotFrequencyButton.OnUpdate += delegate(UIElement e)
			{
				snapshotFrequencyButton.SetIconFrame(snapshotFrequencyButton.Icon.Frame(1, 3, 0, (int)WorldGenerator.CurrentController.SnapshotFrequency, 0, 0));
			};
			GroupOptionButton<bool> mismatchPauseButton = this.AddButton(this.controlPanel, "Images/UI/IconMismatchPause", delegate
			{
				WorldGenerator.Controller controller = UIWorldGenDebug.Controller;
				controller.PauseOnHashMismatch = !controller.PauseOnHashMismatch;
			}, () => "Pause on gen pass change: " + (WorldGenerator.CurrentController.PauseOnHashMismatch ? "On" : "Off"), () => "Stop the generator when the output of a pass is different\nto the last time it was run in the save, or current session");
			mismatchPauseButton.SetColorsBasedOnSelectionState(new Color(152, 175, 235), Colors.InventoryDefaultColor, 1f, 0.7f);
			mismatchPauseButton.OnUpdate += delegate(UIElement e)
			{
				mismatchPauseButton.SetCurrentOption(WorldGenerator.CurrentController.PauseOnHashMismatch);
			};
			string quickLoadCommand = Main.gameMenu ? "/quickload-regen" : "/quickload";
			this.AddButton(this.controlPanel, "Images/UI/IconQuickload", delegate
			{
				DebugUtils.QuickSPMessage(quickLoadCommand);
			}, () => "Save current settings to " + quickLoadCommand, () => "Future launches of the game will automatically load the world\nfrom the most recent snapshot, and run to the current pass");
			GroupOptionButton<bool> groupOptionButton5 = this.AddButton(this.controlPanel, "Images/UI/Bestiary/Icon_Locked", delegate
			{
			}, () => "Controls", () => this.GetControls());
			groupOptionButton5.IconScale = 24f / (float)groupOptionButton5.Icon.Height;
			groupOptionButton5.IconOffset = new Vector2((28f - groupOptionButton5.IconScale * (float)groupOptionButton5.Icon.Width) / 2f + 3f, 3f);
			this.AddButton(this.controlPanel, "Images/UI/Camera_5", delegate
			{
				UIWorldGenDebug.Controller.QueuedAbort = true;
			}, () => "Cancel", null).IconOffset = new Vector2(4f, 4f);
			this.controlListArea.Append(this.controlPanel);
			float num = this.controlPanel.Height.Pixels + 2f;
			this.scrollPanel = new UIPanel
			{
				Width = StyleDimension.FromPixelsAndPercent(300f, 0f),
				Height = StyleDimension.FromPixelsAndPercent(-num, 1f),
				Top = StyleDimension.FromPixels(num),
				Left = this.controlPanel.Left,
				HAlign = 0f,
				VAlign = 0f
			};
			this.scrollPanel.PaddingTop = 8f;
			this.scrollPanel.PaddingBottom = 8f;
			this.scrollPanel.PaddingLeft = 4f;
			this.scrollPanel.PaddingRight = 4f;
			this.controlListArea.Append(this.scrollPanel);
			this.searchBar = new UIWrappedSearchBar(delegate()
			{
				UserInterface.ActiveInstance.SetState(this);
			}, null, UIWrappedSearchBar.ColorTheme.Blue)
			{
				Left = StyleDimension.FromPixels(-2f),
				Top = StyleDimension.FromPixels(-2f),
				Height = StyleDimension.FromPixels(28f),
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				HAlign = 0f
			};
			this.searchBar.OnSearchContentsChanged += delegate(string s)
			{
				this.searchText = s;
			};
			this.searchBar.HideSearchButton();
			this.scrollPanel.Append(this.searchBar);
			num = 30f;
			UIList uilist = new UIList();
			uilist.Top = StyleDimension.FromPixels(num);
			uilist.Width = StyleDimension.FromPixelsAndPercent(-20f, 1f);
			uilist.Height = StyleDimension.FromPixelsAndPercent(-num, 1f);
			uilist.ListPadding = 0f;
			uilist.ManualSortMethod = delegate(List<UIElement> _)
			{
			};
			this.GenPassList = uilist;
			this.scrollPanel.Append(this.GenPassList);
			foreach (GenPass pass in UIWorldGenDebug.Controller.Passes)
			{
				UIWorldGenDebug.GenPassElement item = new UIWorldGenDebug.GenPassElement(this, pass)
				{
					Width = new StyleDimension(-4f, 1f),
					Height = StyleDimension.FromPixels(32f),
					PaddingLeft = 7f
				};
				this.allPasses.Add(item);
				this.GenPassList.Add(item);
			}
			this.scrollbar = new UIScrollbar(UIScrollbar.ColorTheme.Blue)
			{
				Top = StyleDimension.FromPixels(34f),
				Height = StyleDimension.FromPixelsAndPercent(-38f, 1f),
				Left = StyleDimension.FromPixels(-1f),
				HAlign = 1f
			};
			this.scrollbar.SetView(100f, 1000f);
			this.GenPassList.SetScrollbar(this.scrollbar);
			this.scrollPanel.Append(this.scrollbar);
			this.RefreshControlsPosition();
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x00581B14 File Offset: 0x0057FD14
		private void EnsurePassVisible(int passIndex)
		{
			if (passIndex < this.allPasses.Count)
			{
				UIWorldGenDebug.GenPassElement genPassElement = this.allPasses[passIndex];
				if (!this.searchVisible || string.IsNullOrEmpty(this.searchText) || this.MatchesSearch(genPassElement.Pass))
				{
					float height = this.scrollPanel.GetDimensions().Height;
					if (genPassElement.Height.Pixels + genPassElement.Top.Pixels > this.scrollbar.ViewPosition + height - 8f)
					{
						this.scrollbar.ViewPosition = genPassElement.Top.Pixels - (height - 8f) + genPassElement.Height.Pixels;
						return;
					}
					if (genPassElement.Top.Pixels < this.scrollbar.ViewPosition)
					{
						this.scrollbar.ViewPosition = genPassElement.Top.Pixels;
					}
				}
			}
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x00009E06 File Offset: 0x00008006
		private void RefreshControlsPosition()
		{
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x00581BFB File Offset: 0x0057FDFB
		private string GetControls()
		{
			return "[c/FFF014:Space] to pause/resume\n[c/FFF014:R] to rerun current step\n[c/FFF014:Up]/[c/FFF014:Down] or [c/FFF014:Left]/[c/FFF014:Right] to step back/forward\n[c/FFF014:H] to hide UI\n[c/FFF014:M] to toggle map\n[c/FFF014:C] to hide chat log\n";
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x00581C04 File Offset: 0x0057FE04
		private GroupOptionButton<bool> AddButton(UIPanel controlPanel, string assetPath, Action onClick, Func<string> getTitle, Func<string> getDescription = null)
		{
			GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(true, null, null, Color.White, assetPath, 1f, 0.5f, 10f)
			{
				Width = new StyleDimension(34f, 0f),
				Height = new StyleDimension(34f, 0f),
				Left = StyleDimension.FromPixelsAndPercent((float)(36 * controlPanel.Children.Count<UIElement>()), 0f),
				ShowHighlightWhenSelected = false
			};
			groupOptionButton.IconScale = 24f / (float)groupOptionButton.Icon.Width;
			groupOptionButton.IconOffset = new Vector2(3f, 3f);
			groupOptionButton.OnLeftClick += delegate(UIMouseEvent evt, UIElement e)
			{
				onClick();
			};
			groupOptionButton.Append(new UIWorldGenDebug.TooltipElement(getTitle, getDescription));
			controlPanel.Append(groupOptionButton);
			controlPanel.Width = StyleDimension.FromPixelsAndPercent((float)(36 * controlPanel.Children.Count<UIElement>() - 2) + controlPanel.PaddingLeft + controlPanel.PaddingRight, 0f);
			return groupOptionButton;
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x00581D14 File Offset: 0x0057FF14
		private void RangePassClickEvent(UIWorldGenDebug.GenPassElement target, Action<UIWorldGenDebug.GenPassElement> evt)
		{
			if (this._previousRangePassClickEvent != null && this._previousRangePassClickEvent.Item2.Method == evt.Method && this._previousRangePassClickEvent.Item1 != target && this._previousRangePassClickEvent.Item1.Parent == target.Parent && Main.keyState.PressingShift())
			{
				IEnumerable<UIWorldGenDebug.GenPassElement> enumerable = ((UIList)target.Parent.Parent).Cast<UIWorldGenDebug.GenPassElement>();
				UIWorldGenDebug.GenPassElement item = this._previousRangePassClickEvent.Item1;
				int num = 0;
				using (IEnumerator<UIWorldGenDebug.GenPassElement> enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UIWorldGenDebug.GenPassElement genPassElement = enumerator.Current;
						if (genPassElement == item || genPassElement == target)
						{
							num++;
						}
						if (num > 0)
						{
							evt(genPassElement);
						}
						if (num == 2)
						{
							break;
						}
					}
					goto IL_CA;
				}
			}
			evt(target);
			IL_CA:
			this._previousRangePassClickEvent = new Tuple<UIWorldGenDebug.GenPassElement, Action<UIWorldGenDebug.GenPassElement>>(target, evt);
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x00581E08 File Offset: 0x00580008
		private void RangePassClickEventCheckHistory_OnElementClicked(UIElement clicked)
		{
			if (this._previousRangePassClickEvent != null && clicked != this._previousRangePassClickEvent.Item1)
			{
				this._previousRangePassClickEvent = null;
			}
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x00581E28 File Offset: 0x00580028
		public override void OnActivate()
		{
			UIWorldGenDebug.Config.Load();
			if (UIWorldGenDebug.Controller.SnapshotFrequency == WorldGenerator.SnapshotFrequency.None)
			{
				UIWorldGenDebug.Controller.SnapshotFrequency = WorldGenerator.SnapshotFrequency.Automatic;
			}
			Main.menuChat = true;
			if (Main.gameMenu)
			{
				PlayerInput.SetZoom_World();
				Main.mapFullscreenPos = new Vector2((float)(Main.maxTilesX / 2), (float)(Main.maxTilesY / 2));
				Main.mapFullscreenScale = (float)Main.screenWidth / (float)Main.maxTilesX;
			}
			else
			{
				Main.mapFullscreenScale = 2.5f;
				Main.mapFullscreenPos = Main.Camera.Center / 16f;
			}
			this.ToggleMap();
			if (!Main.gameMenu && !DebugOptions.devLightTilesCheat)
			{
				DebugOptions.devLightTilesCheat = true;
				this.disableLightOnClose = true;
			}
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x00581ED6 File Offset: 0x005800D6
		public override void OnDeactivate()
		{
			Main.menuChat = false;
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x00581EE0 File Offset: 0x005800E0
		public override void Update(GameTime gameTime)
		{
			Main.starGame = false;
			Main.LocalPlayer.dead = true;
			if (UIWorldGenDebug.Controller.Paused && this.TestEnumerator != null)
			{
				for (;;)
				{
					if (UIWorldGenDebug.Controller.TryOperateInControlLock(delegate
					{
					}))
					{
						break;
					}
					Thread.Yield();
				}
				if (!this.TestEnumerator.MoveNext())
				{
					this.TestEnumerator = null;
				}
			}
			base.Update(gameTime);
			if (Main.drawingPlayerChat || this.searchBar.IsWritingText)
			{
				this.ignoreEscapeAttempt = 3;
				return;
			}
			int num = this.ignoreEscapeAttempt;
			this.ignoreEscapeAttempt = num - 1;
			if (num <= 0 && PlayerInput.Triggers.JustPressed.Inventory)
			{
				UIWorldGenDebug.Controller.QueuedAbort = true;
			}
			if (UIWorldGenDebug.KeyPressed(Keys.Space))
			{
				WorldGenerator.Controller controller = UIWorldGenDebug.Controller;
				controller.Paused = !controller.Paused;
			}
			if (UIWorldGenDebug.KeyPressed(Keys.R) && UIWorldGenDebug.CanSubmitActions && UIWorldGenDebug.Controller.LastCompletedPass != null)
			{
				UIWorldGenDebug.Controller.TryRunToEndOfPass(UIWorldGenDebug.Controller.LastCompletedPass, !Main.keyState.PressingShift(), true);
			}
			if (UIWorldGenDebug.KeyPressed(Keys.Up) || UIWorldGenDebug.KeyPressed(Keys.Left))
			{
				this.StepBack();
			}
			if (UIWorldGenDebug.KeyPressed(Keys.Down) || UIWorldGenDebug.KeyPressed(Keys.Right))
			{
				this.StepForward();
			}
			if (UIWorldGenDebug.KeyPressed(Keys.C))
			{
				this.hideChat = !this.hideChat;
			}
			if (UIWorldGenDebug.KeyPressed(Keys.H))
			{
				this.ToggleUI();
			}
			if (UIWorldGenDebug.KeyPressed(Keys.M))
			{
				this.ToggleMap();
			}
			PlayerInput.SetZoom_World();
			if (this.showMap)
			{
				if (PlayerInput.Triggers.Current.Up && !Main.oldKeyState.IsKeyDown(Keys.Up))
				{
					Main.mapFullscreenPos.Y = Main.mapFullscreenPos.Y - 1f * (16f / Main.mapFullscreenScale);
				}
				if (PlayerInput.Triggers.Current.Down && !Main.oldKeyState.IsKeyDown(Keys.Down))
				{
					Main.mapFullscreenPos.Y = Main.mapFullscreenPos.Y + 1f * (16f / Main.mapFullscreenScale);
				}
				if (PlayerInput.Triggers.Current.Left && !Main.oldKeyState.IsKeyDown(Keys.Left))
				{
					Main.mapFullscreenPos.X = Main.mapFullscreenPos.X - 1f * (16f / Main.mapFullscreenScale);
				}
				if (PlayerInput.Triggers.Current.Right && !Main.oldKeyState.IsKeyDown(Keys.Right))
				{
					Main.mapFullscreenPos.X = Main.mapFullscreenPos.X + 1f * (16f / Main.mapFullscreenScale);
				}
				if (!UserInterface.ActiveInstance.IsElementUnderMouse())
				{
					Main.mapFullscreenScale *= 1f + (float)(PlayerInput.ScrollWheelDelta / 120) * 0.3f;
				}
				Main.screenPosition = Main.mapFullscreenPos * 16f - Main.Camera.UnscaledSize / 2f;
			}
			else if (!Main.gameMenu)
			{
				Main.DebugCameraPan(PlayerInput.Triggers.Current.Left, PlayerInput.Triggers.Current.Right, PlayerInput.Triggers.Current.Up, PlayerInput.Triggers.Current.Down);
			}
			if (!Main.gameMenu)
			{
				Main.ClampScreenPositionToWorld();
				Main.LocalPlayer.position += Main.screenPosition - Main.PlayerFocusedScreenPosition();
				Main.mapFullscreenPos = Main.Camera.Center / 16f;
			}
			PlayerInput.SetZoom_UI();
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x00582268 File Offset: 0x00580468
		private void ToggleUI()
		{
			this.hideUI = !this.hideUI;
			foreach (UIElement uielement in this.Elements)
			{
				uielement.IgnoresMouseInteraction = this.hideUI;
			}
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x005822D0 File Offset: 0x005804D0
		public void UnhideChat()
		{
			this.hideChat = false;
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x005822D9 File Offset: 0x005804D9
		private void StepBack()
		{
			if (UIWorldGenDebug.CurrentTargetOrLatestPass != null)
			{
				UIWorldGenDebug.Controller.TryResetToPreviousPass(UIWorldGenDebug.CurrentTargetOrLatestPass);
			}
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x005822F4 File Offset: 0x005804F4
		private void StepForward()
		{
			int num = UIWorldGenDebug.Controller.Passes.IndexOf(UIWorldGenDebug.CurrentTargetOrLatestPass);
			GenPass genPass = UIWorldGenDebug.Controller.Passes.Skip(num + 1).FirstOrDefault((GenPass p) => p.Enabled);
			if (genPass != null)
			{
				UIWorldGenDebug.Controller.TryRunToEndOfPass(genPass, true, true);
			}
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x0058235E File Offset: 0x0058055E
		private void CycleSnapshotMode()
		{
			UIWorldGenDebug.Controller.SnapshotFrequency = (UIWorldGenDebug.Controller.SnapshotFrequency + 1) % (WorldGenerator.SnapshotFrequency)3;
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x00582378 File Offset: 0x00580578
		private string GetSnapshotModeButtonTitle()
		{
			switch (WorldGenerator.CurrentController.SnapshotFrequency)
			{
			case WorldGenerator.SnapshotFrequency.Manual:
				return "Create snaphots: Manually";
			case WorldGenerator.SnapshotFrequency.Automatic:
				return "Create snaphots: Automatically";
			case WorldGenerator.SnapshotFrequency.Always:
				return "Create snaphots: After every pass";
			default:
				return "";
			}
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x005823BB File Offset: 0x005805BB
		private static bool KeyPressed(Keys key)
		{
			return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x005823DA File Offset: 0x005805DA
		private bool MatchesSearch(GenPass pass)
		{
			return string.IsNullOrWhiteSpace(this.searchText) || pass.Name.ToLowerInvariant().Contains(this.searchText.Trim().ToLowerInvariant());
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x0058240C File Offset: 0x0058060C
		private void UpdateFilter()
		{
			if (this.searchVisible && !string.IsNullOrEmpty(this.searchText))
			{
				if (!(this.lastSearchText != this.searchText))
				{
					return;
				}
				this.GenPassList.Clear();
				this.lastSearchText = this.searchText;
				using (List<UIWorldGenDebug.GenPassElement>.Enumerator enumerator = this.allPasses.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UIWorldGenDebug.GenPassElement genPassElement = enumerator.Current;
						if (this.MatchesSearch(genPassElement.Pass))
						{
							this.GenPassList.Add(genPassElement);
						}
					}
					return;
				}
			}
			if (this.allPasses.Count != this.GenPassList.Count)
			{
				this.lastSearchText = null;
				this.GenPassList.Clear();
				foreach (UIWorldGenDebug.GenPassElement item in this.allPasses)
				{
					this.GenPassList.Add(item);
				}
			}
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x00582528 File Offset: 0x00580728
		public override void Recalculate()
		{
			if (Main.gameMenu)
			{
				Main.UIScale = Main.UIScaleWanted;
				PlayerInput.SetZoom_UI();
			}
			base.Recalculate();
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x00582546 File Offset: 0x00580746
		protected override void DrawChildren(SpriteBatch spriteBatch)
		{
			if (!this.hideUI)
			{
				base.DrawChildren(spriteBatch);
			}
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x00582558 File Offset: 0x00580758
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Main.starGame = false;
			Main.onlyDrawFancyUI = this.showMap;
			if (this.showMap)
			{
				Main.alreadyGrabbingSunOrMoon = false;
				this.UpdateAndDrawMap();
				Main.instance.DrawFPS();
			}
			if (!this.hideChat || Main.drawingPlayerChat)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(250f, 0f, 0f) * Main.UIScaleMatrix);
				Main.instance.DrawPlayerChat();
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
			}
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x005825F4 File Offset: 0x005807F4
		public override void Draw(SpriteBatch spriteBatch)
		{
			this.UpdateFilter();
			if (Main.gameMenu)
			{
				Main.UIScale = Main.UIScaleWanted;
				PlayerInput.SetZoom_UI();
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
			}
			base.Draw(spriteBatch);
			if (!this.showMap)
			{
				Main.DrawInterface_37_DebugStuff();
			}
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x00582650 File Offset: 0x00580850
		private void ToggleMap()
		{
			this.showMap = !this.showMap;
			Main.onlyDrawFancyUI = this.showMap;
			if (this.showMap)
			{
				this.nextMapSection = Point.Zero;
				this.fullMapScanTimer = Stopwatch.StartNew();
			}
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x0058268C File Offset: 0x0058088C
		private void UpdateAndDrawMap()
		{
			Main.spriteBatch.End();
			if (Main.clearMap)
			{
				Main.Map.Clear();
				MapRenderer.DrawToMap(default(Rectangle));
			}
			PlayerInput.SetZoom_Unscaled();
			Rectangle rectangle = Utils.CenteredRectangle(Main.mapFullscreenPos.ToPoint(), (Main.ScreenSize.ToVector2() / Main.mapFullscreenScale).ToPoint());
			rectangle.Inflate(2, 2);
			rectangle = WorldUtils.ClampToWorld(rectangle, 40);
			Stopwatch stopwatch = Stopwatch.StartNew();
			Point point = this.nextMapSection;
			while (stopwatch.ElapsedMilliseconds < 10L)
			{
				Rectangle sectionStripRect = new Rectangle(this.nextMapSection.X * 200, 0, 200, Main.maxTilesY);
				sectionStripRect = Rectangle.Intersect(sectionStripRect, rectangle);
				bool mapUpdate = false;
				FastParallel.For(sectionStripRect.Left, sectionStripRect.Right, delegate(int x1, int x2, object _)
				{
					bool flag4 = false;
					int top = sectionStripRect.Top;
					int bottom = sectionStripRect.Bottom;
					for (int i = x1; i < x2; i++)
					{
						for (int j = top; j < bottom; j++)
						{
							flag4 |= Main.Map.UpdateLighting(i, j, byte.MaxValue);
						}
					}
					if (flag4)
					{
						mapUpdate = true;
					}
				}, null);
				this.nextMapSection.Y = 0;
				while (this.nextMapSection.Y < Main.maxSectionsY & mapUpdate)
				{
					Rectangle rectangle2 = new Rectangle(this.nextMapSection.X * 200, this.nextMapSection.Y * 150, 200, 150);
					if (rectangle2.Intersects(rectangle))
					{
						MapRenderer.DrawToMap_Section(this.nextMapSection.X, this.nextMapSection.Y);
					}
					this.nextMapSection.Y = this.nextMapSection.Y + 1;
				}
				this.nextMapSection.X = this.nextMapSection.X + 1;
				if (this.nextMapSection.X >= Main.maxSectionsX)
				{
					if (this.lastScanRateUpdate.Elapsed > TimeSpan.FromMilliseconds(200.0))
					{
						this.lastScanRateUpdate.Restart();
						this.fullMapScanPeriod = this.fullMapScanTimer.Elapsed;
					}
					this.fullMapScanTimer.Restart();
					this.nextMapSection.X = 0;
				}
				if (this.nextMapSection.X == point.X)
				{
					break;
				}
			}
			Main.instance.GraphicsDevice.Clear(new Color(100, 100, 255));
			Main.spriteBatch.Begin();
			Main.mapReady = true;
			Main.MapPylonTile = new Point16(-1, -1);
			Main.mapFullscreen = true;
			bool flag = UserInterface.ActiveInstance.MouseCaptured() || UserInterface.ActiveInstance.IsElementUnderMouse();
			bool flag2 = Main.mouseLeft && !flag;
			bool flag3 = Main.mouseRight && !flag;
			Utils.Swap<bool>(ref Main.mouseLeft, ref flag2);
			Utils.Swap<bool>(ref Main.mouseRight, ref flag3);
			Main.instance.DrawMap(new GameTime());
			Utils.Swap<bool>(ref Main.mouseLeft, ref flag2);
			Utils.Swap<bool>(ref Main.mouseRight, ref flag3);
			Main.mapFullscreen = false;
			PlayerInput.SetZoom_UI();
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
			if (Main.showFrameRate)
			{
				double num = Math.Min(1.0 / this.fullMapScanPeriod.TotalSeconds, 60.0);
				string text = string.Format((num >= 10.0) ? "{0:0}" : "{0:0.0}", num);
				text += " map scans/s";
				DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, text, new Vector2((float)(Main.screenWidth - (int)FontAssets.MouseText.Value.MeasureString(text).X), 4f), new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor));
			}
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x00582A53 File Offset: 0x00580C53
		public void RunTest(IEnumerable<object> test)
		{
			this.TestEnumerator = test.GetEnumerator();
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x00582A61 File Offset: 0x00580C61
		private static IEnumerable<bool> TestSetupResetAndCreateSnapshots()
		{
			UIWorldGenDebug.Controller.TryReset();
			UIWorldGenDebug.Controller.SnapshotFrequency = WorldGenerator.SnapshotFrequency.Always;
			UIWorldGenDebug.Controller.PauseOnHashMismatch = true;
			Main.NewText("Creating Snapshots", byte.MaxValue, 100, 0);
			GenPass lastPass = UIWorldGenDebug.Controller.Passes.Last<GenPass>();
			UIWorldGenDebug.Controller.TryRunToEndOfPass(lastPass, false, true);
			yield return true;
			if (UIWorldGenDebug.Controller.LastCompletedPass != lastPass || UIWorldGenDebug.Controller.PausedDueToHashMismatch)
			{
				Main.NewText("Test aborted", byte.MaxValue, 0, 0);
				yield return false;
			}
			UIWorldGenDebug.Controller.SnapshotFrequency = WorldGenerator.SnapshotFrequency.Manual;
			yield break;
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x00582A6A File Offset: 0x00580C6A
		public static IEnumerable<object> TestResetFromPassesAndRegen()
		{
			using (IEnumerator<bool> enumerator = UIWorldGenDebug.TestSetupResetAndCreateSnapshots().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current)
					{
						yield break;
					}
					yield return null;
				}
			}
			IEnumerator<bool> enumerator = null;
			GenPass lastPass = UIWorldGenDebug.Controller.Passes.Last<GenPass>();
			List<GenPass> passes = (from p in UIWorldGenDebug.Controller.Passes
			where p.Enabled
			select p).ToList<GenPass>();
			int num;
			for (int i = 0; i < passes.Count; i = num + 1)
			{
				GenPass pass = passes[i];
				UIWorldGenDebug.Controller.TryReset();
				Main.NewText(string.Format("[{0}/{1}] Running to {2}", i + 1, passes.Count, pass.Name), byte.MaxValue, 100, 0);
				UIWorldGenDebug.Controller.TryRunToEndOfPass(pass, false, true);
				yield return null;
				if (UIWorldGenDebug.Controller.LastCompletedPass != pass || UIWorldGenDebug.Controller.PausedDueToHashMismatch)
				{
					Main.NewText("Test aborted", byte.MaxValue, 0, 0);
					yield break;
				}
				UIWorldGenDebug.Controller.TryReset();
				UIWorldGenDebug.Controller.TryRunToEndOfPass(lastPass, false, true);
				yield return null;
				if (UIWorldGenDebug.Controller.LastCompletedPass != lastPass || UIWorldGenDebug.Controller.PausedDueToHashMismatch)
				{
					Main.NewText("Test aborted", byte.MaxValue, 0, 0);
					yield break;
				}
				pass = null;
				num = i;
			}
			Main.NewText("Test Completed Successfully", 0, byte.MaxValue, 0);
			yield break;
			yield break;
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x00582A73 File Offset: 0x00580C73
		public static IEnumerable<object> TestHiddenTileData()
		{
			using (IEnumerator<bool> enumerator = UIWorldGenDebug.TestSetupResetAndCreateSnapshots().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current)
					{
						yield break;
					}
					yield return null;
				}
			}
			IEnumerator<bool> enumerator = null;
			UIWorldGenDebug.Controller.TryReset();
			foreach (GenPass pass in from p in UIWorldGenDebug.Controller.Passes
			where p.Enabled
			select p)
			{
				TileSnapshot.Create(null);
				TileSnapshot.Restore();
				UIWorldGenDebug.Controller.TryRunToEndOfPass(pass, false, true);
				yield return null;
				if (UIWorldGenDebug.Controller.LastCompletedPass != pass || UIWorldGenDebug.Controller.PausedDueToHashMismatch)
				{
					Main.NewText("Test aborted", byte.MaxValue, 0, 0);
					yield break;
				}
				pass = null;
			}
			IEnumerator<GenPass> enumerator2 = null;
			Main.NewText("Test Completed Successfully", 0, byte.MaxValue, 0);
			yield break;
			yield break;
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x00582A7C File Offset: 0x00580C7C
		public static IEnumerable<object> TestResumeFromSnapshots()
		{
			using (IEnumerator<bool> enumerator = UIWorldGenDebug.TestSetupResetAndCreateSnapshots().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current)
					{
						yield break;
					}
					yield return null;
				}
			}
			IEnumerator<bool> enumerator = null;
			GenPass lastPass = UIWorldGenDebug.Controller.Passes.Last<GenPass>();
			foreach (GenPass pass in (from p in UIWorldGenDebug.Controller.Passes
			where p.Enabled
			select p).Reverse<GenPass>())
			{
				UIWorldGenDebug.Controller.TryRunToEndOfPass(pass, true, true);
				yield return null;
				if (UIWorldGenDebug.Controller.LastCompletedPass != pass || UIWorldGenDebug.Controller.PausedDueToHashMismatch)
				{
					Main.NewText("Test aborted", byte.MaxValue, 0, 0);
					yield break;
				}
				pass = null;
			}
			IEnumerator<GenPass> enumerator2 = null;
			Main.NewText("Single pass rerun test completed successfully", 0, byte.MaxValue, 0);
			foreach (GenPass pass2 in from p in UIWorldGenDebug.Controller.Passes
			where p.Enabled
			select p)
			{
				UIWorldGenDebug.Controller.TryResetToSnapshot(pass2);
				UIWorldGenDebug.Controller.TryRunToEndOfPass(lastPass, false, true);
				yield return null;
				if (UIWorldGenDebug.Controller.LastCompletedPass != lastPass || UIWorldGenDebug.Controller.PausedDueToHashMismatch)
				{
					Main.NewText("Test aborted", byte.MaxValue, 0, 0);
					yield break;
				}
			}
			enumerator2 = null;
			Main.NewText("Load snapshot and run to end test completed successfully", 0, byte.MaxValue, 0);
			foreach (GenPass pass3 in from p in UIWorldGenDebug.Controller.Passes
			where p.Enabled
			select p)
			{
				UIWorldGenDebug.Controller.TryReset();
				UIWorldGenDebug.Controller.TryResetToSnapshot(pass3);
				UIWorldGenDebug.Controller.TryRunToEndOfPass(lastPass, false, true);
				yield return null;
				if (UIWorldGenDebug.Controller.LastCompletedPass != lastPass || UIWorldGenDebug.Controller.PausedDueToHashMismatch)
				{
					Main.NewText("Test aborted", byte.MaxValue, 0, 0);
					yield break;
				}
			}
			enumerator2 = null;
			Main.NewText("Clean load snapshot and run to end test completed successfully", 0, byte.MaxValue, 0);
			yield break;
			yield break;
		}

		// Token: 0x040052CC RID: 21196
		private UIWrappedSearchBar searchBar;

		// Token: 0x040052CD RID: 21197
		private string lastSearchText;

		// Token: 0x040052CE RID: 21198
		private string searchText;

		// Token: 0x040052CF RID: 21199
		private bool showMap;

		// Token: 0x040052D0 RID: 21200
		private bool hideChat;

		// Token: 0x040052D1 RID: 21201
		private bool hideUI;

		// Token: 0x040052D2 RID: 21202
		private bool disableDebugOnClose;

		// Token: 0x040052D3 RID: 21203
		private bool disableLightOnClose;

		// Token: 0x040052D4 RID: 21204
		private IEnumerator<object> TestEnumerator;

		// Token: 0x040052D5 RID: 21205
		private UIElement controlListArea;

		// Token: 0x040052D6 RID: 21206
		private UIPanel controlPanel;

		// Token: 0x040052D7 RID: 21207
		private UIPanel scrollPanel;

		// Token: 0x040052D8 RID: 21208
		private UIScrollbar scrollbar;

		// Token: 0x040052D9 RID: 21209
		private UIList GenPassList;

		// Token: 0x040052DA RID: 21210
		private GroupOptionButton<bool> SearchButton;

		// Token: 0x040052DB RID: 21211
		private List<UIWorldGenDebug.GenPassElement> allPasses = new List<UIWorldGenDebug.GenPassElement>();

		// Token: 0x040052DC RID: 21212
		private bool searchVisible = true;

		// Token: 0x040052DD RID: 21213
		private int LassPassIndex;

		// Token: 0x040052DE RID: 21214
		private Tuple<UIWorldGenDebug.GenPassElement, Action<UIWorldGenDebug.GenPassElement>> _previousRangePassClickEvent;

		// Token: 0x040052DF RID: 21215
		private int ignoreEscapeAttempt;

		// Token: 0x040052E0 RID: 21216
		private Point nextMapSection;

		// Token: 0x040052E1 RID: 21217
		private TimeSpan fullMapScanPeriod;

		// Token: 0x040052E2 RID: 21218
		private Stopwatch fullMapScanTimer;

		// Token: 0x040052E3 RID: 21219
		private Stopwatch lastScanRateUpdate = Stopwatch.StartNew();

		// Token: 0x020008E0 RID: 2272
		private class TooltipElement : UIElement
		{
			// Token: 0x06004693 RID: 18067 RVA: 0x006C71DE File Offset: 0x006C53DE
			public TooltipElement(Func<string> getTitle, Func<string> getDescription = null)
			{
				this._getTitle = getTitle;
				this._getDescription = getDescription;
				this.IgnoresMouseInteraction = true;
			}

			// Token: 0x06004694 RID: 18068 RVA: 0x006C71FC File Offset: 0x006C53FC
			protected override void DrawSelf(SpriteBatch spriteBatch)
			{
				if (!base.Parent.IsMouseHovering)
				{
					return;
				}
				string nameOverride = this._getTitle();
				string text = (this._getDescription == null) ? null : this._getDescription();
				if (text == null)
				{
					text = string.Empty;
				}
				Item item = Main.DisplayAndGetFakeItem(ItemRarityColor.StrongRed10);
				item.SetNameOverride(nameOverride);
				item.ToolTip = ItemTooltip.FromHardcodedText(new string[]
				{
					text
				});
			}

			// Token: 0x0400735A RID: 29530
			private Func<string> _getTitle;

			// Token: 0x0400735B RID: 29531
			private Func<string> _getDescription;
		}

		// Token: 0x020008E1 RID: 2273
		private class Config
		{
			// Token: 0x06004695 RID: 18069 RVA: 0x006C7265 File Offset: 0x006C5465
			public static void Save()
			{
				File.WriteAllText(UIWorldGenDebug.Config.FilePath, JsonConvert.SerializeObject(UIWorldGenDebug.Config.Instance));
			}

			// Token: 0x06004696 RID: 18070 RVA: 0x006C727C File Offset: 0x006C547C
			public static void Load()
			{
				try
				{
					if (File.Exists(UIWorldGenDebug.Config.FilePath))
					{
						UIWorldGenDebug.Config.Instance = JsonConvert.DeserializeObject<UIWorldGenDebug.Config>(File.ReadAllText(UIWorldGenDebug.Config.FilePath));
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x0400735C RID: 29532
			private static readonly string FilePath = Path.Combine(Main.SavePath, "dev-worldgen.json");

			// Token: 0x0400735D RID: 29533
			public static UIWorldGenDebug.Config Instance = new UIWorldGenDebug.Config();

			// Token: 0x0400735E RID: 29534
			public HashSet<string> HighlightedPassNames = new HashSet<string>();
		}

		// Token: 0x020008E2 RID: 2274
		private class UIImageButtonWithExtraIcon : UIImageButton
		{
			// Token: 0x06004699 RID: 18073 RVA: 0x006C72F3 File Offset: 0x006C54F3
			public UIImageButtonWithExtraIcon(Asset<Texture2D> texture, Rectangle? frame = null) : base(texture, frame)
			{
			}

			// Token: 0x0600469A RID: 18074 RVA: 0x006C7308 File Offset: 0x006C5508
			protected override void DrawSelf(SpriteBatch spriteBatch)
			{
				base.DrawSelf(spriteBatch);
				if (this._iconTexture != null)
				{
					Rectangle rectangle = base.GetDimensions().ToRectangle();
					rectangle.Inflate(-2, -2);
					int width;
					int height;
					if (this._iconFrame != null)
					{
						width = this._iconFrame.Value.Width;
						height = this._iconFrame.Value.Height;
					}
					else
					{
						width = this._iconTexture.Value.Width;
						height = this._iconTexture.Value.Height;
					}
					if (width != height)
					{
						if (width < height)
						{
							float num = (float)width / (float)height;
							int num2 = rectangle.Width - (int)((float)rectangle.Width * num);
							rectangle.Width -= num2;
							rectangle.X += num2 / 2;
						}
						else
						{
							float num3 = (float)height / (float)width;
							int num4 = rectangle.Height - (int)((float)rectangle.Height * num3);
							rectangle.Height -= num4;
							rectangle.Y += num4 / 2;
						}
					}
					spriteBatch.Draw(this._iconTexture.Value, rectangle, this._iconFrame, this.IconColor * (base.IsMouseHovering ? this._visibilityActive : this._visibilityInactive));
				}
			}

			// Token: 0x0600469B RID: 18075 RVA: 0x006C7445 File Offset: 0x006C5645
			public void SetIcon(string iconTexturePath)
			{
				if (iconTexturePath != null)
				{
					this._iconTexture = Main.Assets.Request<Texture2D>(iconTexturePath, 1);
					return;
				}
				this._iconTexture = null;
			}

			// Token: 0x0600469C RID: 18076 RVA: 0x006C7464 File Offset: 0x006C5664
			public void SetIconFrame(Rectangle region)
			{
				this._iconFrame = new Rectangle?(region);
			}

			// Token: 0x1700056E RID: 1390
			// (get) Token: 0x0600469D RID: 18077 RVA: 0x006C7472 File Offset: 0x006C5672
			public Texture2D Icon
			{
				get
				{
					if (this._iconTexture == null)
					{
						return null;
					}
					return this._iconTexture.Value;
				}
			}

			// Token: 0x0400735F RID: 29535
			private Rectangle? _iconFrame;

			// Token: 0x04007360 RID: 29536
			private Asset<Texture2D> _iconTexture;

			// Token: 0x04007361 RID: 29537
			public Color IconColor = Color.White;
		}

		// Token: 0x020008E3 RID: 2275
		private class GenPassElement : UIPanel
		{
			// Token: 0x0600469E RID: 18078 RVA: 0x006C748C File Offset: 0x006C568C
			private UIWorldGenDebug.UIImageButtonWithExtraIcon AddButton(string assetPath, string iconAsset, float x, float y, Action onClick, Func<string> getTitle, Func<string> getDescription = null)
			{
				UIWorldGenDebug.UIImageButtonWithExtraIcon uiimageButtonWithExtraIcon = new UIWorldGenDebug.UIImageButtonWithExtraIcon(Main.Assets.Request<Texture2D>(assetPath, 1), null)
				{
					Left = StyleDimension.FromPixelsAndPercent(x, 0f),
					Top = StyleDimension.FromPixelsAndPercent(y, 0f)
				};
				if (!string.IsNullOrEmpty(iconAsset))
				{
					uiimageButtonWithExtraIcon.SetIcon(iconAsset);
				}
				uiimageButtonWithExtraIcon.OnLeftClick += delegate(UIMouseEvent evt, UIElement e)
				{
					onClick();
				};
				if (getTitle != null)
				{
					uiimageButtonWithExtraIcon.Append(new UIWorldGenDebug.TooltipElement(getTitle, getDescription));
				}
				base.Append(uiimageButtonWithExtraIcon);
				return uiimageButtonWithExtraIcon;
			}

			// Token: 0x0600469F RID: 18079 RVA: 0x006C7521 File Offset: 0x006C5721
			protected override void DrawSelf(SpriteBatch spriteBatch)
			{
				this.RefreshColors();
				base.DrawSelf(spriteBatch);
			}

			// Token: 0x060046A0 RID: 18080 RVA: 0x006C7530 File Offset: 0x006C5730
			private static void InitPassIcons()
			{
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Terrain, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(1));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Skyblock, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(26));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.DunesAndPyramidLocations, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(4));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.OceanSand, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(28));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SandPatches, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(169));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Tunnels, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4501));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.MountainCaves, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4510));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.DirtWallBackgrounds, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(30));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.RocksInDirt, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(3));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.DirtInRocks, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(2));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Clay, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(133));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SmallHoles, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4538));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.DirtLayerCaves, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4510));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.RockLayerCaves, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4512));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SurfaceCaves, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4501));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.WavyCaves, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4537));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.IceBiome, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(6));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Grass, UIWorldGenDebug.GenPassElement.PassIconEntry.FromImageFrame("Images/Tiles_3", 5, 45, 1));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Jungle, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(22));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.MudCavesToJungleGrass, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(745));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.DesertBiome, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(3));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.GlowingMushroomPatches, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(24));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Marble, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(29));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Granite, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(30));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.FloatingIslands, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(26));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.DirtToMud, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(176));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Silt, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(424));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.OresAndShinies, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(19));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Webs, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(150));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Underworld, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(33));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.CorruptionAndCrimson, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(7));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Lakes, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(28));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.StoneToIceAndSiltPlusMudIntoSlush, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(1103));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.DualDungeonsDitherSnake, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(32));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Dungeon, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(32));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.MountainCaveOpenings, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(2));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.BeachesAndOceanCleanup, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(27));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Gems, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(178));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.GravitatingSandCleanup, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(169));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.OceanCaves, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(28));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Shimmer, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(5340));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.DirtWallCleanup, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(2));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Pyramids, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(607));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.DirtRockWallRunner, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4501));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.LivingTrees, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(0));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.LivingTreeWalls, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(1723));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.DemonAndCrimsonAltars, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(5467));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SurfaceWaterInJungle, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(22));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.LihzahrdTemple, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(31));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Beehives, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(1126));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.JungleShrines, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(680));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SettleLiquids, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(28));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.RemoveSurfaceWaterAboveSand, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(169));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Oasis, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(27));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.ShellPilesMarblePilesAndSpikePits, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4090));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SmoothWorld, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(1));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Waterfalls, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(2169));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.FragileIceOverIceBiomeWater, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(664));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.CaveWallVariety, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4540));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.LifeCrystals, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(29));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Statues, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(52));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.UndergroundHousesAndBuriedChests, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(306));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SurfaceChests, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(48));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.ChestsInJungleShrines, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(680));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.UnderwaterChests, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(1298));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SpiderCaves, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(34));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.GemCaves, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4644));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.MossAndMossCaves, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4496));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.LihzahrdTemplePart2, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(31));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.CaveWallsInEnclosedSpaces, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4510));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.UndergroundJungleTrees, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(23));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.FloatingIslandHouses, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(26));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.QuickCleanup, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(41));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.PotsGraveyardsAndBoulderPiles, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(222));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Hellforges, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(221));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SpreadingGrassOnSurfaceSunflowersEvilsOnSurfaceAndLavaCleanup, UIWorldGenDebug.GenPassElement.PassIconEntry.FromImageFrame("Images/Tiles_3", 5, 45, 1));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SurfaceOreAndStone, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(19));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.FallenLogsAndWaterFeatures, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(0));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Traps, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(580));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Piles, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(1));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SpawnPoint, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(224));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SurfaceDirtWallsToGrassWalls, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(745));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SpawnStarterNPCs, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(867));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SunflowersPart2, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(63));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Trees, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(0));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.AlchemyHerbs, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(3093));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.DyePlants, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(1109));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.WebsInSpiderCavesAndHoneyPlusSpeleothemsInBeehives, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(150));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.GrassPlantsEvilPlantsAndPumpkinsOnSurface, UIWorldGenDebug.GenPassElement.PassIconEntry.FromImageFrame("Images/Tiles_3", 5, 45, 1));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.GlowingMushroomPlantsUndergroundAndJunglePlants, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(25));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.JunglePlantsPart2, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(23));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Vines, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(3005));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Flowers, UIWorldGenDebug.GenPassElement.PassIconEntry.FromImageFrame("Images/Tiles_3", 33, 45, 1));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.Mushrooms, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(5));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.ExposedGemsInIceBiome, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(182));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.ExposedGemsUnderground, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4400));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.LongMoss, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4496));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.DirtWallsIntoMudWallsInJungleAndJungleMinMax, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4487));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.BeeLarvaInBeehives, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(2108));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SettleLiquidsPart2AndNotTheBees, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(28));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.CactusPalmTreesAndCoral, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(3));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.TileCleanup, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(41));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.LihzahrdAltar, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(31));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.MicroBiomes, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(0));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.LilypadsCattailsBambooAndSeaweed, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(4564));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.SpeleothemsAndGemTrees, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(6));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.BrokenTrapCleanup, UIWorldGenDebug.GenPassElement.PassIconEntry.FromItem(580));
				UIWorldGenDebug.GenPassElement.passIcons.Add(GenPassNameID.FinalCleanup, UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(41));
			}

			// Token: 0x060046A1 RID: 18081 RVA: 0x006C7F2C File Offset: 0x006C612C
			private UIWorldGenDebug.GenPassElement.PassIconEntry GetPassIcon(GenPass pass)
			{
				if (UIWorldGenDebug.GenPassElement.passIcons.Count == 0)
				{
					UIWorldGenDebug.GenPassElement.InitPassIcons();
				}
				UIWorldGenDebug.GenPassElement.PassIconEntry result;
				if (!UIWorldGenDebug.GenPassElement.passIcons.TryGetValue(pass.Name, out result))
				{
					result = UIWorldGenDebug.GenPassElement.PassIconEntry.FromBestiaryIcon(64);
				}
				return result;
			}

			// Token: 0x060046A2 RID: 18082 RVA: 0x006C7F68 File Offset: 0x006C6168
			private UIImage AddIcon()
			{
				UIWorldGenDebug.GenPassElement.PassIconEntry passIcon = this.GetPassIcon(this.Pass);
				return new UIImage(Main.Assets.Request<Texture2D>(passIcon.Icon, 1))
				{
					Width = new StyleDimension((float)passIcon.Width, 0f),
					Height = new StyleDimension((float)passIcon.Height, 0f),
					Top = new StyleDimension((float)((26 - passIcon.Height) / 2), 0f),
					Left = new StyleDimension((float)((26 - passIcon.Width) / 2), 0f),
					Frame = new Rectangle?(passIcon.Region),
					ScaleToFit = true
				};
			}

			// Token: 0x060046A3 RID: 18083 RVA: 0x006C8018 File Offset: 0x006C6218
			public GenPassElement(UIWorldGenDebug parent, GenPass pass)
			{
				UIWorldGenDebug.GenPassElement.<>c__DisplayClass9_0 CS$<>8__locals1 = new UIWorldGenDebug.GenPassElement.<>c__DisplayClass9_0();
				CS$<>8__locals1.pass = pass;
				CS$<>8__locals1.parent = parent;
				base..ctor();
				CS$<>8__locals1.<>4__this = this;
				this.Pass = CS$<>8__locals1.pass;
				base.SetPadding(2f);
				this.Height.Set(96f, 0f);
				base.Append(new UIWorldGenDebug.TooltipElement(new Func<string>(this.GetTitle), new Func<string>(this.GetDescription)));
				UIImage uiimage = this.AddIcon();
				uiimage.IgnoresMouseInteraction = true;
				base.Append(uiimage);
				UIText indexText = new UIText(this.Index.ToString(), 0.5f, false)
				{
					Left = StyleDimension.FromPixels(2f),
					Top = StyleDimension.FromPixels(2f),
					IgnoresMouseInteraction = true
				};
				base.Append(indexText);
				UIText text = new UIText(CS$<>8__locals1.pass.Name, 1f, false)
				{
					Left = StyleDimension.FromPixels(32f),
					Top = StyleDimension.FromPixels(4f),
					IgnoresMouseInteraction = true
				};
				text.OnUpdate += delegate(UIElement e)
				{
					text.TextColor = (CS$<>8__locals1.<>4__this.IsRunning ? Color.Yellow : (CS$<>8__locals1.<>4__this.Skipped ? Color.DarkGray : (CS$<>8__locals1.<>4__this.HasCompleted ? new Color(0, 230, 0) : ((!CS$<>8__locals1.pass.Enabled) ? Color.DarkGray : Color.White))));
					text.TextColor *= (CS$<>8__locals1.parent.MatchesSearch(CS$<>8__locals1.pass) ? 1f : 0.6f);
					indexText.TextColor = text.TextColor;
				};
				base.Append(text);
				this.SetColorsToNotHovered();
				UIWorldGenDebug.UIImageButtonWithExtraIcon snapshotIcon = this.AddButton("Images/UI/ButtonBacking", "Images/UI/Camera_4", 72f, 3f, delegate
				{
					if (Main.keyState.PressingAlt())
					{
						return;
					}
					if (CS$<>8__locals1.<>4__this.Snapshot == null)
					{
						UIWorldGenDebug.Controller.TryCreateSnapshot();
						return;
					}
					if (!CS$<>8__locals1.<>4__this.Snapshot.Outdated)
					{
						UIWorldGenDebug.Controller.TryResetToSnapshot(CS$<>8__locals1.pass);
					}
				}, new Func<string>(this.GetSnapshotButtonTitle), new Func<string>(this.GetSnapshotButtonDescription));
				snapshotIcon.OnUpdate += delegate(UIElement e)
				{
					if (CS$<>8__locals1.<>4__this.Snapshot != null)
					{
						snapshotIcon.SetIcon("Images/UI/Camera_4");
					}
					else if (UIWorldGenDebug.Controller.LastCompletedPass == CS$<>8__locals1.<>4__this.Pass)
					{
						snapshotIcon.SetIcon("Images/UI/Camera_7");
					}
					UIWorldGenDebug.SetButtonState(snapshotIcon, (CS$<>8__locals1.<>4__this.Pass.Enabled && (CS$<>8__locals1.<>4__this.Snapshot != null || UIWorldGenDebug.Controller.LastCompletedPass == CS$<>8__locals1.<>4__this.Pass)) ? UIWorldGenDebug.ButtonState.Enabled : UIWorldGenDebug.ButtonState.NotVisible);
					if (CS$<>8__locals1.<>4__this.Snapshot != null && CS$<>8__locals1.<>4__this.Snapshot.Outdated)
					{
						snapshotIcon.IconColor = Color.PaleVioletRed;
						return;
					}
					snapshotIcon.IconColor = Color.White;
				};
				snapshotIcon.OnRightClick += delegate(UIMouseEvent evt, UIElement e)
				{
					if (CS$<>8__locals1.<>4__this.Snapshot != null)
					{
						UIWorldGenDebug.Controller.DeleteSnapshot(CS$<>8__locals1.pass);
						UserInterface.ActiveInstance.ClearPointers();
					}
				};
				snapshotIcon.Left = new StyleDimension(-28f, 1f);
				base.OnLeftClick += delegate(UIMouseEvent evt, UIElement e)
				{
					if (CS$<>8__locals1.<>4__this != evt.Target)
					{
						return;
					}
					if (Main.keyState.PressingAlt())
					{
						CS$<>8__locals1.<>4__this.ToggleHighlight();
						CS$<>8__locals1.<>4__this.SetColorsToHovered();
						return;
					}
					if (Main.keyState.PressingControl())
					{
						if (!CS$<>8__locals1.pass.Enabled)
						{
							CS$<>8__locals1.parent.RangePassClickEvent(CS$<>8__locals1.<>4__this, delegate(UIWorldGenDebug.GenPassElement x)
							{
								x.Enable();
								x.RefreshColors();
							});
						}
						return;
					}
					if (CS$<>8__locals1.pass.Enabled)
					{
						UIWorldGenDebug.Controller.TryRunToEndOfPass(CS$<>8__locals1.pass, !Main.keyState.PressingShift(), true);
					}
				};
				base.OnRightClick += delegate(UIMouseEvent evt, UIElement e)
				{
					if (Main.keyState.PressingControl() && CS$<>8__locals1.pass.Enabled)
					{
						CS$<>8__locals1.parent.RangePassClickEvent(CS$<>8__locals1.<>4__this, delegate(UIWorldGenDebug.GenPassElement x)
						{
							x.Disable();
							x.RefreshColors();
						});
					}
				};
			}

			// Token: 0x060046A4 RID: 18084 RVA: 0x006C8243 File Offset: 0x006C6443
			private void RefreshColors()
			{
				if (this.Hovered)
				{
					this.SetColorsToHovered();
					return;
				}
				this.SetColorsToNotHovered();
			}

			// Token: 0x060046A5 RID: 18085 RVA: 0x006C825C File Offset: 0x006C645C
			private void SetColorsToHovered()
			{
				this.BackgroundColor = new Color(73, 94, 171);
				this.BorderColor = new Color(89, 116, 213);
				if (this.IsHighlighted)
				{
					this.BackgroundColor = new Color(110, 30, 150);
					this.BorderColor = new Color(171, 53, 255);
				}
				if (UIWorldGenDebug.CurrentTargetPass == this.Pass)
				{
					this.BorderColor = new Color(255, 231, 69);
				}
				if (!this.Pass.Enabled)
				{
					this.BorderColor = new Color(150, 150, 150) * 1f;
					this.BackgroundColor = Color.Lerp(this.BackgroundColor, new Color(120, 120, 120), 0.5f) * 1f;
				}
			}

			// Token: 0x060046A6 RID: 18086 RVA: 0x006C8344 File Offset: 0x006C6544
			private void SetColorsToNotHovered()
			{
				this.BackgroundColor = new Color(63, 82, 151) * 0.7f;
				this.BorderColor = new Color(89, 116, 213) * 0.7f;
				if (this.IsHighlighted)
				{
					this.BackgroundColor = new Color(110, 30, 150) * 0.7f;
					this.BorderColor = new Color(171, 53, 255) * 0.7f;
				}
				if (UIWorldGenDebug.CurrentTargetPass == this.Pass)
				{
					this.BorderColor = new Color(255, 231, 69);
				}
				if (!this.Pass.Enabled)
				{
					this.BorderColor = new Color(127, 127, 127) * 0.7f;
					this.BackgroundColor = Color.Lerp(this.BackgroundColor, new Color(80, 80, 80), 0.5f) * 0.7f;
				}
			}

			// Token: 0x060046A7 RID: 18087 RVA: 0x006C844B File Offset: 0x006C664B
			public override void MouseOver(UIMouseEvent evt)
			{
				this.Hovered = true;
				base.MouseOver(evt);
				this.SetColorsToHovered();
			}

			// Token: 0x060046A8 RID: 18088 RVA: 0x006C8461 File Offset: 0x006C6661
			public override void MouseOut(UIMouseEvent evt)
			{
				this.Hovered = false;
				base.MouseOut(evt);
				this.SetColorsToNotHovered();
			}

			// Token: 0x060046A9 RID: 18089 RVA: 0x006C8478 File Offset: 0x006C6678
			private string GetTitle()
			{
				if (this.Skipped)
				{
					return "Skipped: " + this.Pass.Name;
				}
				if (!this.Pass.Enabled)
				{
					return "Disabled: " + this.Pass.Name;
				}
				return ((!this.HasCompleted) ? "Run" : "Rerun") + " to " + this.Pass.Name;
			}

			// Token: 0x060046AA RID: 18090 RVA: 0x006C84F0 File Offset: 0x006C66F0
			private string GetDescription()
			{
				string str = string.Empty;
				if (this.Pass.Enabled)
				{
					str += "Hold shift to ignore snapshots\n";
					if (!UIWorldGenDebug.CanSubmitActions && (this.HasCompleted || Main.keyState.PressingShift()))
					{
						str += "\n[c/FFA500:Must be paused to rerun or load snapshots]\n";
					}
				}
				if (!this.HasCompleted && !this.Skipped)
				{
					str += "Crtl Left click to enable\n";
					str += "Crtl Right click to disable\n";
					str += "Crtl & Shift to edit ranges\n";
				}
				return str + "Alt click to toggle highlight\n";
			}

			// Token: 0x060046AB RID: 18091 RVA: 0x006C8584 File Offset: 0x006C6784
			private string GetSnapshotButtonTitle()
			{
				if (this.Snapshot != null && this.Snapshot.Outdated)
				{
					return "Snapshot is outdated and will only be used for comparison when the pass is run again";
				}
				if (this.Snapshot != null)
				{
					return "Reset to snapshot";
				}
				if (UIWorldGenDebug.Controller.LastCompletedPass == this.Pass)
				{
					return "Take snapshot";
				}
				return null;
			}

			// Token: 0x060046AC RID: 18092 RVA: 0x006C85D4 File Offset: 0x006C67D4
			private string GetSnapshotButtonDescription()
			{
				if (this.Snapshot != null)
				{
					string text = "Left click to load snapshot\n";
					text += "Right click to delete snapshot\n";
					if (!UIWorldGenDebug.CanSubmitActions)
					{
						text += "\n[c/FFA500:Must be paused to load a snapshot]";
					}
					return text;
				}
				if (UIWorldGenDebug.Controller.LastCompletedPass == this.Pass)
				{
					return "Left click to take snapshot\n";
				}
				return null;
			}

			// Token: 0x060046AD RID: 18093 RVA: 0x006C8629 File Offset: 0x006C6829
			private void Enable()
			{
				Utils.TryOperateInLock(this.Pass, delegate
				{
					if (!this.HasCompleted)
					{
						this.Pass.Enable();
						UIWorldGenDebug.Controller.ForceUpdateProgress();
					}
				});
			}

			// Token: 0x060046AE RID: 18094 RVA: 0x006C8643 File Offset: 0x006C6843
			private void Disable()
			{
				Utils.TryOperateInLock(this.Pass, delegate
				{
					if (!this.HasCompleted)
					{
						this.Pass.Disable();
						UIWorldGenDebug.Controller.ForceUpdateProgress();
						UIWorldGenDebug.Controller.DeleteSnapshot(this.Pass);
					}
				});
			}

			// Token: 0x060046AF RID: 18095 RVA: 0x006C8660 File Offset: 0x006C6860
			private void ToggleHighlight()
			{
				if (this.IsHighlighted)
				{
					UIWorldGenDebug.Config.Instance.HighlightedPassNames.Remove(this.Pass.Name);
				}
				else
				{
					UIWorldGenDebug.Config.Instance.HighlightedPassNames.Add(this.Pass.Name);
				}
				UIWorldGenDebug.Config.Save();
			}

			// Token: 0x1700056F RID: 1391
			// (get) Token: 0x060046B0 RID: 18096 RVA: 0x006C86B2 File Offset: 0x006C68B2
			public int Index
			{
				get
				{
					return UIWorldGenDebug.Controller.Passes.IndexOf(this.Pass);
				}
			}

			// Token: 0x17000570 RID: 1392
			// (get) Token: 0x060046B1 RID: 18097 RVA: 0x006C86C9 File Offset: 0x006C68C9
			public bool IsRunning
			{
				get
				{
					return UIWorldGenDebug.Controller.CurrentPass == this.Pass;
				}
			}

			// Token: 0x17000571 RID: 1393
			// (get) Token: 0x060046B2 RID: 18098 RVA: 0x006C86DD File Offset: 0x006C68DD
			public bool HasCompleted
			{
				get
				{
					return WorldGen.Manifest.GenPassResults.Count > this.Index;
				}
			}

			// Token: 0x17000572 RID: 1394
			// (get) Token: 0x060046B3 RID: 18099 RVA: 0x006C86F6 File Offset: 0x006C68F6
			public bool Skipped
			{
				get
				{
					return this.HasCompleted && WorldGen.Manifest.GenPassResults[this.Index].Skipped;
				}
			}

			// Token: 0x17000573 RID: 1395
			// (get) Token: 0x060046B4 RID: 18100 RVA: 0x006C871C File Offset: 0x006C691C
			public WorldGenSnapshot Snapshot
			{
				get
				{
					return UIWorldGenDebug.Controller.GetSnapshot(this.Pass);
				}
			}

			// Token: 0x17000574 RID: 1396
			// (get) Token: 0x060046B5 RID: 18101 RVA: 0x006C872E File Offset: 0x006C692E
			public bool IsPausedAfterThisPass
			{
				get
				{
					return UIWorldGenDebug.CanSubmitActions && this.HasCompleted && !this.Skipped && WorldGen.Manifest.GenPassResults.Count == this.Index + 1;
				}
			}

			// Token: 0x17000575 RID: 1397
			// (get) Token: 0x060046B6 RID: 18102 RVA: 0x006C8762 File Offset: 0x006C6962
			public bool IsHighlighted
			{
				get
				{
					return UIWorldGenDebug.Config.Instance.HighlightedPassNames.Contains(this.Pass.Name);
				}
			}

			// Token: 0x04007362 RID: 29538
			public readonly GenPass Pass;

			// Token: 0x04007363 RID: 29539
			private bool Hovered;

			// Token: 0x04007364 RID: 29540
			private static Dictionary<string, UIWorldGenDebug.GenPassElement.PassIconEntry> passIcons = new Dictionary<string, UIWorldGenDebug.GenPassElement.PassIconEntry>();

			// Token: 0x02000ADF RID: 2783
			internal struct PassIconEntry
			{
				// Token: 0x06004CDE RID: 19678 RVA: 0x006D9B48 File Offset: 0x006D7D48
				internal static UIWorldGenDebug.GenPassElement.PassIconEntry FromBestiaryIcon(int index)
				{
					string text = "Images/UI/Bestiary/Icon_Tags_Shadow";
					Asset<Texture2D> tex = Main.Assets.Request<Texture2D>(text, 1);
					return new UIWorldGenDebug.GenPassElement.PassIconEntry
					{
						Icon = text,
						Region = tex.Frame(16, 5, index % 16, index / 16, 0, 0),
						Width = 26,
						Height = 26
					};
				}

				// Token: 0x06004CDF RID: 19679 RVA: 0x006D9BA8 File Offset: 0x006D7DA8
				internal static UIWorldGenDebug.GenPassElement.PassIconEntry FromItem(int index)
				{
					string text = "Images/Item_" + index.ToString();
					Asset<Texture2D> asset = Main.Assets.Request<Texture2D>(text, 1);
					Rectangle rectangle = asset.Frame(1, 1, 0, 0, 0, 0);
					int num = (rectangle.Width > rectangle.Height) ? rectangle.Width : asset.Height();
					float num2 = 20f / (float)num;
					if (num2 > 1.2f)
					{
						num2 = 1.2f;
					}
					return new UIWorldGenDebug.GenPassElement.PassIconEntry
					{
						Icon = text,
						Region = rectangle,
						Width = (int)((float)rectangle.Width * num2),
						Height = (int)((float)rectangle.Height * num2)
					};
				}

				// Token: 0x06004CE0 RID: 19680 RVA: 0x006D9C58 File Offset: 0x006D7E58
				internal static UIWorldGenDebug.GenPassElement.PassIconEntry FromImageFrame(string image, int index, int rowCount, int lineCount)
				{
					Asset<Texture2D> asset = Main.Assets.Request<Texture2D>(image, 1);
					Rectangle rectangle = asset.Frame(rowCount, lineCount, index % rowCount, index / rowCount, 0, 0);
					int num = (rectangle.Width > rectangle.Height) ? rectangle.Width : asset.Height();
					float num2 = 20f / (float)num;
					if (num2 > 1.2f)
					{
						num2 = 1.2f;
					}
					return new UIWorldGenDebug.GenPassElement.PassIconEntry
					{
						Icon = image,
						Region = rectangle,
						Width = (int)((float)rectangle.Width * num2),
						Height = (int)((float)rectangle.Height * num2)
					};
				}

				// Token: 0x04007864 RID: 30820
				internal string Icon;

				// Token: 0x04007865 RID: 30821
				internal Rectangle Region;

				// Token: 0x04007866 RID: 30822
				internal int Width;

				// Token: 0x04007867 RID: 30823
				internal int Height;
			}
		}

		// Token: 0x020008E4 RID: 2276
		private enum ButtonState
		{
			// Token: 0x04007366 RID: 29542
			Enabled,
			// Token: 0x04007367 RID: 29543
			NotVisible
		}
	}
}
