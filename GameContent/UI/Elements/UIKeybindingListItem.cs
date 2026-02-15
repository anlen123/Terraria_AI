using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Chat;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003F4 RID: 1012
	public class UIKeybindingListItem : UIElement
	{
		// Token: 0x06002E9D RID: 11933 RVA: 0x005ABB03 File Offset: 0x005A9D03
		public UIKeybindingListItem(string bind, InputMode mode, Color color)
		{
			this._keybind = bind;
			this._inputmode = mode;
			this._color = color;
			base.OnLeftClick += this.OnClickMethod;
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x005ABB32 File Offset: 0x005A9D32
		public void OnClickMethod(UIMouseEvent evt, UIElement listeningElement)
		{
			if (PlayerInput.ListeningTrigger != this._keybind)
			{
				if (PlayerInput.CurrentProfile.AllowEditting)
				{
					PlayerInput.ListenFor(this._keybind, this._inputmode);
					return;
				}
				PlayerInput.ListenFor(null, this._inputmode);
			}
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x005ABB70 File Offset: 0x005A9D70
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float num = 6f;
			base.DrawSelf(spriteBatch);
			CalculatedStyle dimensions = base.GetDimensions();
			float num2 = dimensions.Width + 1f;
			Vector2 vector = new Vector2(dimensions.X, dimensions.Y);
			bool flag = PlayerInput.ListeningTrigger == this._keybind;
			Vector2 vector2 = new Vector2(0.8f);
			Color color = flag ? Color.Gold : (base.IsMouseHovering ? Color.White : Color.Silver);
			color = Color.Lerp(color, Color.White, base.IsMouseHovering ? 0.5f : 0f);
			Color color2 = base.IsMouseHovering ? this._color : this._color.MultiplyRGBA(new Color(180, 180, 180));
			Vector2 vector3 = vector;
			Utils.DrawSettingsPanel(spriteBatch, vector3, num2, color2);
			vector3.X += 8f;
			vector3.Y += 2f + num;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, this.GetFriendlyName(), vector3, color, 0f, Vector2.Zero, vector2, num2, 2f);
			vector3.X -= 17f;
			List<string> list = PlayerInput.CurrentProfile.InputModes[this._inputmode].KeyStatus[this._keybind];
			string text = this.GenInput(list);
			if (string.IsNullOrEmpty(text))
			{
				text = Lang.menu[195].Value;
				if (!flag)
				{
					color = new Color(80, 80, 80);
				}
			}
			Vector2 stringSize = ChatManager.GetStringSize(FontAssets.ItemStack.Value, text, vector2, -1f);
			vector3 = new Vector2(dimensions.X + dimensions.Width - stringSize.X - 10f, dimensions.Y + 2f + num);
			if (this._inputmode == InputMode.XBoxGamepad || this._inputmode == InputMode.XBoxGamepadUI)
			{
				vector3 += new Vector2(0f, -3f);
			}
			GlyphTagHandler.GlyphsScale = 0.85f;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text, vector3, color, 0f, Vector2.Zero, vector2, num2, 2f);
			GlyphTagHandler.GlyphsScale = 1f;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x005ABDB8 File Offset: 0x005A9FB8
		private string GenInput(List<string> list)
		{
			if (list.Count == 0)
			{
				return "";
			}
			string text = "";
			InputMode inputmode = this._inputmode;
			if (inputmode > InputMode.Mouse)
			{
				if (inputmode - InputMode.XBoxGamepad <= 1)
				{
					text = GlyphTagHandler.GenerateTag(list[0]);
					for (int i = 1; i < list.Count; i++)
					{
						text = text + "/" + GlyphTagHandler.GenerateTag(list[i]);
					}
				}
			}
			else
			{
				text = list[0];
				for (int j = 1; j < list.Count; j++)
				{
					text = text + "/" + list[j];
				}
			}
			return text;
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x005ABE50 File Offset: 0x005AA050
		private string GetFriendlyName()
		{
			string keybind = this._keybind;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(keybind);
			if (num <= 2155837676U)
			{
				if (num <= 1227701228U)
				{
					if (num <= 564091680U)
					{
						if (num <= 341426238U)
						{
							if (num != 135974533U)
							{
								if (num != 236909357U)
								{
									if (num == 341426238U)
									{
										if (keybind == "MouseRight")
										{
											return Lang.menu[163].Value;
										}
									}
								}
								else if (keybind == "Jump")
								{
									return Lang.menu[152].Value;
								}
							}
							else if (keybind == "QuickBuff")
							{
								return Lang.menu[157].Value;
							}
						}
						else if (num <= 540272591U)
						{
							if (num != 513712005U)
							{
								if (num == 540272591U)
								{
									if (keybind == "Throw")
									{
										return Lang.menu[153].Value;
									}
								}
							}
							else if (keybind == "Right")
							{
								return Lang.menu[151].Value;
							}
						}
						else if (num != 553555557U)
						{
							if (num == 564091680U)
							{
								if (keybind == "ViewZoomIn")
								{
									return Language.GetTextValue("UI.ZoomIn");
								}
							}
						}
						else if (keybind == "ViewZoomOut")
						{
							return Language.GetTextValue("UI.ZoomOut");
						}
					}
					else if (num <= 1123244352U)
					{
						if (num != 731985058U)
						{
							if (num != 1038438905U)
							{
								if (num == 1123244352U)
								{
									if (keybind == "Up")
									{
										return Lang.menu[148].Value;
									}
								}
							}
							else if (keybind == "QuickMount")
							{
								return Lang.menu[158].Value;
							}
						}
						else if (keybind == "MapAlphaUp")
						{
							return Lang.menu[171].Value;
						}
					}
					else if (num <= 1194145990U)
					{
						if (num != 1160590752U)
						{
							if (num == 1194145990U)
							{
								if (keybind == "Hotbar3")
								{
									return Lang.menu[178].Value;
								}
							}
						}
						else if (keybind == "Hotbar1")
						{
							return Lang.menu[176].Value;
						}
					}
					else if (num != 1210923609U)
					{
						if (num == 1227701228U)
						{
							if (keybind == "Hotbar5")
							{
								return Lang.menu[180].Value;
							}
						}
					}
					else if (keybind == "Hotbar2")
					{
						return Lang.menu[177].Value;
					}
				}
				else if (num <= 1791478331U)
				{
					if (num <= 1278034085U)
					{
						if (num != 1244478847U)
						{
							if (num != 1261256466U)
							{
								if (num == 1278034085U)
								{
									if (keybind == "Hotbar6")
									{
										return Lang.menu[181].Value;
									}
								}
							}
							else if (keybind == "Hotbar7")
							{
								return Lang.menu[182].Value;
							}
						}
						else if (keybind == "Hotbar4")
						{
							return Lang.menu[179].Value;
						}
					}
					else if (num <= 1311589323U)
					{
						if (num != 1294811704U)
						{
							if (num == 1311589323U)
							{
								if (keybind == "Hotbar8")
								{
									return Lang.menu[183].Value;
								}
							}
						}
						else if (keybind == "Hotbar9")
						{
							return Lang.menu[184].Value;
						}
					}
					else if (num != 1750693397U)
					{
						if (num == 1791478331U)
						{
							if (keybind == "LockOn")
							{
								return Lang.menu[231].Value;
							}
						}
					}
					else if (keybind == "Dash")
					{
						return Language.GetTextValue("UI.Dash");
					}
				}
				else if (num <= 2016360712U)
				{
					if (num != 1825695843U)
					{
						if (num != 1982550448U)
						{
							if (num == 2016360712U)
							{
								if (keybind == "PreviousLoadout")
								{
									return Language.GetTextValue("UI.PreviousLoadout");
								}
							}
						}
						else if (keybind == "Hotbar10")
						{
							return Lang.menu[185].Value;
						}
					}
					else if (keybind == "MapAlphaDown")
					{
						return Lang.menu[170].Value;
					}
				}
				else if (num <= 2122282438U)
				{
					if (num != 2105504819U)
					{
						if (num == 2122282438U)
						{
							if (keybind == "DpadSnap2")
							{
								return Lang.menu[192].Value;
							}
						}
					}
					else if (keybind == "DpadSnap1")
					{
						return Lang.menu[191].Value;
					}
				}
				else if (num != 2139060057U)
				{
					if (num == 2155837676U)
					{
						if (keybind == "DpadSnap4")
						{
							return Lang.menu[194].Value;
						}
					}
				}
				else if (keybind == "DpadSnap3")
				{
					return Lang.menu[193].Value;
				}
			}
			else if (num <= 3191803782U)
			{
				if (num <= 2761510965U)
				{
					if (num <= 2419048118U)
					{
						if (num != 2385492880U)
						{
							if (num != 2402270499U)
							{
								if (num == 2419048118U)
								{
									if (keybind == "Loadout1")
									{
										return Language.GetTextValue("UI.Loadout1");
									}
								}
							}
							else if (keybind == "Loadout2")
							{
								return Language.GetTextValue("UI.Loadout2");
							}
						}
						else if (keybind == "Loadout3")
						{
							return Language.GetTextValue("UI.Loadout3");
						}
					}
					else if (num <= 2457286800U)
					{
						if (num != 2424138507U)
						{
							if (num == 2457286800U)
							{
								if (keybind == "Left")
								{
									return Lang.menu[150].Value;
								}
							}
						}
						else if (keybind == "HotbarMinus")
						{
							return Lang.menu[174].Value;
						}
					}
					else if (num != 2651901706U)
					{
						if (num == 2761510965U)
						{
							if (keybind == "Down")
							{
								return Lang.menu[149].Value;
							}
						}
					}
					else if (keybind == "ArmorSetAbility")
					{
						return Language.GetTextValue("UI.ArmorSetAbility");
					}
				}
				else if (num <= 3036053321U)
				{
					if (num != 2772734821U)
					{
						if (num != 2944436841U)
						{
							if (num == 3036053321U)
							{
								if (keybind == "DpadRadial4")
								{
									return Lang.menu[189].Value;
								}
							}
						}
						else if (keybind == "MapZoomIn")
						{
							return Lang.menu[168].Value;
						}
					}
					else if (keybind == "MouseLeft")
					{
						return Lang.menu[162].Value;
					}
				}
				else if (num <= 3069608559U)
				{
					if (num != 3052830940U)
					{
						if (num == 3069608559U)
						{
							if (keybind == "DpadRadial2")
							{
								return Lang.menu[187].Value;
							}
						}
					}
					else if (keybind == "DpadRadial3")
					{
						return Lang.menu[188].Value;
					}
				}
				else if (num != 3086386178U)
				{
					if (num == 3191803782U)
					{
						if (keybind == "QuickHeal")
						{
							return Lang.menu[159].Value;
						}
					}
				}
				else if (keybind == "DpadRadial1")
				{
					return Lang.menu[186].Value;
				}
			}
			else if (num <= 3779252424U)
			{
				if (num <= 3369262303U)
				{
					if (num != 3275635460U)
					{
						if (num != 3313084571U)
						{
							if (num == 3369262303U)
							{
								if (keybind == "Inventory")
								{
									return Lang.menu[154].Value;
								}
							}
						}
						else if (keybind == "QuickMana")
						{
							return Lang.menu[156].Value;
						}
					}
					else if (keybind == "Grapple")
					{
						return Lang.menu[155].Value;
					}
				}
				else if (num <= 3568872111U)
				{
					if (num != 3568722032U)
					{
						if (num == 3568872111U)
						{
							if (keybind == "ToggleCreativeMenu")
							{
								return Language.GetTextValue("UI.ToggleCreativeMenu");
							}
						}
					}
					else if (keybind == "SmartCursor")
					{
						return Lang.menu[161].Value;
					}
				}
				else if (num != 3675592426U)
				{
					if (num == 3779252424U)
					{
						if (keybind == "NextLoadout")
						{
							return Language.GetTextValue("UI.NextLoadout");
						}
					}
				}
				else if (keybind == "MapZoomOut")
				{
					return Lang.menu[169].Value;
				}
			}
			else if (num <= 4118463989U)
			{
				if (num != 3843014342U)
				{
					if (num != 3952285921U)
					{
						if (num == 4118463989U)
						{
							if (keybind == "ToggleCameraMode")
							{
								return Language.GetTextValue("UI.ToggleCameraMode");
							}
						}
					}
					else if (keybind == "HotbarPlus")
					{
						return Lang.menu[175].Value;
					}
				}
				else if (keybind == "RadialHotbar")
				{
					return Lang.menu[190].Value;
				}
			}
			else if (num <= 4154136266U)
			{
				if (num != 4142436984U)
				{
					if (num == 4154136266U)
					{
						if (keybind == "SmartSelect")
						{
							return Lang.menu[160].Value;
						}
					}
				}
				else if (keybind == "MapFull")
				{
					return Lang.menu[173].Value;
				}
			}
			else if (num != 4217003918U)
			{
				if (num == 4257986450U)
				{
					if (keybind == "MapStyle")
					{
						return Lang.menu[172].Value;
					}
				}
			}
			else if (keybind == "RadialQuickbar")
			{
				return Lang.menu[244].Value;
			}
			return this._keybind;
		}

		// Token: 0x040055AB RID: 21931
		private InputMode _inputmode;

		// Token: 0x040055AC RID: 21932
		private Color _color;

		// Token: 0x040055AD RID: 21933
		private string _keybind;
	}
}
