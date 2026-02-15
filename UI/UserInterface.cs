using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameInput;

namespace Terraria.UI
{
	// Token: 0x02000101 RID: 257
	public class UserInterface
	{
		// Token: 0x06001A14 RID: 6676 RVA: 0x004F3F45 File Offset: 0x004F2145
		public void ClearPointers()
		{
			this.LeftMouse.Clear();
			this.RightMouse.Clear();
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x004F3F5D File Offset: 0x004F215D
		public bool MouseCaptured()
		{
			return (this.LeftMouse.WasDown && this.LeftMouse.LastDown != null) || (this.RightMouse.WasDown && this.RightMouse.LastDown != null);
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x004F3F98 File Offset: 0x004F2198
		public void ResetLasts()
		{
			if (this._lastElementHover != null)
			{
				this._lastElementHover.MouseOut(new UIMouseEvent(this._lastElementHover, this.MousePosition));
			}
			this.ClearPointers();
			this._lastElementHover = null;
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x004F3FCB File Offset: 0x004F21CB
		public void EscapeElements()
		{
			this.ResetLasts();
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06001A18 RID: 6680 RVA: 0x004F3FD3 File Offset: 0x004F21D3
		public UIState CurrentState
		{
			get
			{
				return this._currentState;
			}
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x004F3FDC File Offset: 0x004F21DC
		public UserInterface()
		{
			UserInterface.InputPointerCache inputPointerCache = new UserInterface.InputPointerCache();
			inputPointerCache.MouseDownEvent = delegate(UIElement element, UIMouseEvent evt)
			{
				element.LeftMouseDown(evt);
			};
			inputPointerCache.MouseUpEvent = delegate(UIElement element, UIMouseEvent evt)
			{
				element.LeftMouseUp(evt);
			};
			inputPointerCache.ClickEvent = delegate(UIElement element, UIMouseEvent evt)
			{
				element.LeftClick(evt);
			};
			inputPointerCache.DoubleClickEvent = delegate(UIElement element, UIMouseEvent evt)
			{
				element.LeftDoubleClick(evt);
			};
			this.LeftMouse = inputPointerCache;
			UserInterface.InputPointerCache inputPointerCache2 = new UserInterface.InputPointerCache();
			inputPointerCache2.MouseDownEvent = delegate(UIElement element, UIMouseEvent evt)
			{
				element.RightMouseDown(evt);
			};
			inputPointerCache2.MouseUpEvent = delegate(UIElement element, UIMouseEvent evt)
			{
				element.RightMouseUp(evt);
			};
			inputPointerCache2.ClickEvent = delegate(UIElement element, UIMouseEvent evt)
			{
				element.RightClick(evt);
			};
			inputPointerCache2.DoubleClickEvent = delegate(UIElement element, UIMouseEvent evt)
			{
				element.RightDoubleClick(evt);
			};
			this.RightMouse = inputPointerCache2;
			base..ctor();
			UserInterface.ActiveInstance = this;
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x004F413E File Offset: 0x004F233E
		public void Use()
		{
			if (UserInterface.ActiveInstance != this)
			{
				UserInterface.ActiveInstance = this;
				this.Recalculate();
				return;
			}
			UserInterface.ActiveInstance = this;
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x004F415B File Offset: 0x004F235B
		private void ImmediatelyUpdateInputPointers()
		{
			this.LeftMouse.WasDown = Main.mouseLeft;
			this.RightMouse.WasDown = Main.mouseRight;
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x004F4180 File Offset: 0x004F2380
		private void ResetState()
		{
			if (!Main.dedServ)
			{
				this.GetMousePosition();
				this.ImmediatelyUpdateInputPointers();
				if (this._lastElementHover != null)
				{
					this._lastElementHover.MouseOut(new UIMouseEvent(this._lastElementHover, this.MousePosition));
				}
			}
			this.ClearPointers();
			this._lastElementHover = null;
			this._clickDisabledTimeRemaining = Math.Max(this._clickDisabledTimeRemaining, 200.0);
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x004F41EB File Offset: 0x004F23EB
		private void GetMousePosition()
		{
			this.MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x004F4204 File Offset: 0x004F2404
		public void Update(GameTime time)
		{
			if (this._currentState == null)
			{
				return;
			}
			bool flag = FocusHelper.AllowUIInputs;
			if (!Main.gameMenu && PlayerInput.IgnoreMouseInterface)
			{
				flag = false;
			}
			this.GetMousePosition();
			UIElement uielement = flag ? this._currentState.GetElementAt(this.MousePosition) : null;
			this._clickDisabledTimeRemaining = Math.Max(0.0, this._clickDisabledTimeRemaining - time.ElapsedGameTime.TotalMilliseconds);
			bool flag2 = this._clickDisabledTimeRemaining > 0.0;
			if (uielement != this._lastElementHover)
			{
				if (this._lastElementHover != null)
				{
					this._lastElementHover.MouseOut(new UIMouseEvent(this._lastElementHover, this.MousePosition));
				}
				if (uielement != null)
				{
					uielement.MouseOver(new UIMouseEvent(uielement, this.MousePosition));
				}
				this._lastElementHover = uielement;
			}
			if (!flag2)
			{
				this.HandleClick(this.LeftMouse, time, Main.mouseLeft && flag, uielement);
				this.HandleClick(this.RightMouse, time, Main.mouseRight && flag, uielement);
			}
			if (PlayerInput.ScrollWheelDeltaForUI != 0)
			{
				if (uielement != null)
				{
					uielement.ScrollWheel(new UIScrollWheelEvent(uielement, this.MousePosition, PlayerInput.ScrollWheelDeltaForUI));
				}
				PlayerInput.ScrollWheelDeltaForUI = 0;
			}
			if (this._currentState != null)
			{
				this._currentState.Update(time);
			}
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x004F4338 File Offset: 0x004F2538
		private void HandleClick(UserInterface.InputPointerCache cache, GameTime time, bool isDown, UIElement mouseElement)
		{
			if (isDown && !cache.WasDown && mouseElement != null)
			{
				cache.LastDown = mouseElement;
				cache.MouseDownEvent(mouseElement, new UIMouseEvent(mouseElement, this.MousePosition));
				if (cache.LastClicked == mouseElement && time.TotalGameTime.TotalMilliseconds - cache.LastTimeDown < 500.0)
				{
					cache.DoubleClickEvent(mouseElement, new UIMouseEvent(mouseElement, this.MousePosition));
					cache.LastClicked = null;
				}
				cache.LastTimeDown = time.TotalGameTime.TotalMilliseconds;
			}
			else if (!isDown && cache.WasDown && cache.LastDown != null)
			{
				UIElement lastDown = cache.LastDown;
				if (lastDown.ContainsPoint(this.MousePosition))
				{
					cache.ClickEvent(lastDown, new UIMouseEvent(lastDown, this.MousePosition));
					cache.LastClicked = cache.LastDown;
				}
				cache.MouseUpEvent(lastDown, new UIMouseEvent(lastDown, this.MousePosition));
				cache.LastDown = null;
			}
			cache.WasDown = isDown;
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x004F4452 File Offset: 0x004F2652
		public void Draw(SpriteBatch spriteBatch, GameTime time)
		{
			this.Use();
			if (this._currentState != null)
			{
				if (this._isStateDirty)
				{
					this._currentState.Recalculate();
					this._isStateDirty = false;
				}
				this._currentState.Draw(spriteBatch);
			}
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x004F4488 File Offset: 0x004F2688
		public void DrawDebugHitbox(BasicDebugDrawer drawer)
		{
			UIState currentState = this._currentState;
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x004F4494 File Offset: 0x004F2694
		public void SetState(UIState state)
		{
			if (state == this._currentState)
			{
				return;
			}
			if (state != null)
			{
				this.AddToHistory(state);
			}
			if (this._currentState != null)
			{
				if (this._lastElementHover != null)
				{
					this._lastElementHover.MouseOut(new UIMouseEvent(this._lastElementHover, this.MousePosition));
				}
				this._currentState.Deactivate();
			}
			this._currentState = state;
			this.ResetState();
			if (state != null)
			{
				this._isStateDirty = true;
				state.Activate();
				state.Recalculate();
			}
			this.IsVisible = (this._currentState != null);
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x004F4520 File Offset: 0x004F2720
		public void GoBack()
		{
			if (this._history.Count < 2)
			{
				return;
			}
			UIState state = this._history[this._history.Count - 2];
			this._history.RemoveRange(this._history.Count - 2, 2);
			this.SetState(state);
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x004F4575 File Offset: 0x004F2775
		private void AddToHistory(UIState state)
		{
			this._history.Add(state);
			if (this._history.Count > 32)
			{
				this._history.RemoveRange(0, 4);
			}
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x004F459F File Offset: 0x004F279F
		public void Recalculate()
		{
			if (this._currentState != null)
			{
				this._currentState.Recalculate();
			}
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x004F45B4 File Offset: 0x004F27B4
		public CalculatedStyle GetDimensions()
		{
			Vector2 originalScreenSize = PlayerInput.OriginalScreenSize;
			return new CalculatedStyle(0f, 0f, originalScreenSize.X / Main.UIScale, originalScreenSize.Y / Main.UIScale);
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x004F45EE File Offset: 0x004F27EE
		internal void RefreshState()
		{
			if (this._currentState != null)
			{
				this._currentState.Deactivate();
			}
			this.ResetState();
			this._currentState.Activate();
			this._currentState.Recalculate();
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x004F461F File Offset: 0x004F281F
		public bool IsElementUnderMouse()
		{
			return this.IsVisible && this._lastElementHover != null && !(this._lastElementHover is UIState);
		}

		// Token: 0x04001381 RID: 4993
		private const double DOUBLE_CLICK_TIME = 500.0;

		// Token: 0x04001382 RID: 4994
		private const double STATE_CHANGE_CLICK_DISABLE_TIME = 200.0;

		// Token: 0x04001383 RID: 4995
		private const int MAX_HISTORY_SIZE = 32;

		// Token: 0x04001384 RID: 4996
		private const int HISTORY_PRUNE_SIZE = 4;

		// Token: 0x04001385 RID: 4997
		public static UserInterface ActiveInstance = new UserInterface();

		// Token: 0x04001386 RID: 4998
		private List<UIState> _history = new List<UIState>();

		// Token: 0x04001387 RID: 4999
		private UserInterface.InputPointerCache LeftMouse;

		// Token: 0x04001388 RID: 5000
		private UserInterface.InputPointerCache RightMouse;

		// Token: 0x04001389 RID: 5001
		public Vector2 MousePosition;

		// Token: 0x0400138A RID: 5002
		private UIElement _lastElementHover;

		// Token: 0x0400138B RID: 5003
		private double _clickDisabledTimeRemaining;

		// Token: 0x0400138C RID: 5004
		private bool _isStateDirty;

		// Token: 0x0400138D RID: 5005
		public bool IsVisible;

		// Token: 0x0400138E RID: 5006
		private UIState _currentState;

		// Token: 0x02000716 RID: 1814
		// (Invoke) Token: 0x06004023 RID: 16419
		private delegate void MouseElementEvent(UIElement element, UIMouseEvent evt);

		// Token: 0x02000717 RID: 1815
		private class InputPointerCache
		{
			// Token: 0x06004026 RID: 16422 RVA: 0x0069C123 File Offset: 0x0069A323
			public void Clear()
			{
				this.LastClicked = null;
				this.LastDown = null;
				this.LastTimeDown = 0.0;
			}

			// Token: 0x040068CB RID: 26827
			public double LastTimeDown;

			// Token: 0x040068CC RID: 26828
			public bool WasDown;

			// Token: 0x040068CD RID: 26829
			public UIElement LastDown;

			// Token: 0x040068CE RID: 26830
			public UIElement LastClicked;

			// Token: 0x040068CF RID: 26831
			public UserInterface.MouseElementEvent MouseDownEvent;

			// Token: 0x040068D0 RID: 26832
			public UserInterface.MouseElementEvent MouseUpEvent;

			// Token: 0x040068D1 RID: 26833
			public UserInterface.MouseElementEvent ClickEvent;

			// Token: 0x040068D2 RID: 26834
			public UserInterface.MouseElementEvent DoubleClickEvent;
		}
	}
}
