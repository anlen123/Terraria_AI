using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.Net;
using Terraria.UI;

namespace Terraria.GameContent.Creative
{
	// Token: 0x02000329 RID: 809
	public class CreativePowers
	{
		// Token: 0x02000896 RID: 2198
		public abstract class APerPlayerTogglePower : ICreativePower, IOnPlayerJoining
		{
			// Token: 0x17000544 RID: 1348
			// (get) Token: 0x060044C3 RID: 17603 RVA: 0x006C1F88 File Offset: 0x006C0188
			// (set) Token: 0x060044C4 RID: 17604 RVA: 0x006C1F90 File Offset: 0x006C0190
			public ushort PowerId { get; set; }

			// Token: 0x17000545 RID: 1349
			// (get) Token: 0x060044C5 RID: 17605 RVA: 0x006C1F99 File Offset: 0x006C0199
			// (set) Token: 0x060044C6 RID: 17606 RVA: 0x006C1FA1 File Offset: 0x006C01A1
			public string ServerConfigName { get; set; }

			// Token: 0x17000546 RID: 1350
			// (get) Token: 0x060044C7 RID: 17607 RVA: 0x006C1FAA File Offset: 0x006C01AA
			// (set) Token: 0x060044C8 RID: 17608 RVA: 0x006C1FB2 File Offset: 0x006C01B2
			public PowerPermissionLevel CurrentPermissionLevel { get; set; }

			// Token: 0x17000547 RID: 1351
			// (get) Token: 0x060044C9 RID: 17609 RVA: 0x006C1FBB File Offset: 0x006C01BB
			// (set) Token: 0x060044CA RID: 17610 RVA: 0x006C1FC3 File Offset: 0x006C01C3
			public PowerPermissionLevel DefaultPermissionLevel { get; set; }

			// Token: 0x060044CB RID: 17611 RVA: 0x006C1FCC File Offset: 0x006C01CC
			public bool IsEnabledForPlayer(int playerIndex)
			{
				return this._perPlayerIsEnabled.IndexInRange(playerIndex) && this._perPlayerIsEnabled[playerIndex];
			}

			// Token: 0x060044CC RID: 17612 RVA: 0x006C1FE8 File Offset: 0x006C01E8
			public void DeserializeNetMessage(BinaryReader reader, int userId)
			{
				CreativePowers.APerPlayerTogglePower.SubMessageType subMessageType = (CreativePowers.APerPlayerTogglePower.SubMessageType)reader.ReadByte();
				if (subMessageType == CreativePowers.APerPlayerTogglePower.SubMessageType.SyncEveryone)
				{
					this.Deserialize_SyncEveryone(reader, userId);
					return;
				}
				if (subMessageType != CreativePowers.APerPlayerTogglePower.SubMessageType.SyncOnePlayer)
				{
					return;
				}
				int playerIndex = (int)reader.ReadByte();
				bool state = reader.ReadBoolean();
				if (Main.netMode == 2)
				{
					playerIndex = userId;
					if (!CreativePowersHelper.IsAvailableForPlayer(this, playerIndex))
					{
						return;
					}
				}
				this.SetEnabledState(playerIndex, state);
			}

			// Token: 0x060044CD RID: 17613 RVA: 0x006C2038 File Offset: 0x006C0238
			private void Deserialize_SyncEveryone(BinaryReader reader, int userId)
			{
				int num = (int)Math.Ceiling((double)((float)this._perPlayerIsEnabled.Length / 8f));
				if (Main.netMode == 2 && !CreativePowersHelper.IsAvailableForPlayer(this, userId))
				{
					reader.ReadBytes(num);
					return;
				}
				for (int i = 0; i < num; i++)
				{
					BitsByte bitsByte = reader.ReadByte();
					for (int j = 0; j < 8; j++)
					{
						int num2 = i * 8 + j;
						if (num2 != Main.myPlayer)
						{
							if (num2 >= this._perPlayerIsEnabled.Length)
							{
								break;
							}
							this.SetEnabledState(num2, bitsByte[j]);
						}
					}
				}
			}

			// Token: 0x060044CE RID: 17614 RVA: 0x006C20C8 File Offset: 0x006C02C8
			public void SetEnabledState(int playerIndex, bool state)
			{
				this._perPlayerIsEnabled[playerIndex] = state;
				if (Main.netMode == 2)
				{
					NetPacket packet = NetCreativePowersModule.PreparePacket(this.PowerId, 3);
					packet.Writer.Write(1);
					packet.Writer.Write((byte)playerIndex);
					packet.Writer.Write(state);
					NetManager.Instance.Broadcast(packet, -1);
				}
			}

			// Token: 0x060044CF RID: 17615 RVA: 0x006C2127 File Offset: 0x006C0327
			public void DebugCall()
			{
				this.RequestUse();
			}

			// Token: 0x060044D0 RID: 17616 RVA: 0x006C2130 File Offset: 0x006C0330
			internal void RequestUse()
			{
				NetPacket packet = NetCreativePowersModule.PreparePacket(this.PowerId, 3);
				packet.Writer.Write(1);
				packet.Writer.Write((byte)Main.myPlayer);
				packet.Writer.Write(!this._perPlayerIsEnabled[Main.myPlayer]);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}

			// Token: 0x060044D1 RID: 17617 RVA: 0x006C2190 File Offset: 0x006C0390
			public void Reset()
			{
				for (int i = 0; i < this._perPlayerIsEnabled.Length; i++)
				{
					this._perPlayerIsEnabled[i] = this._defaultToggleState;
				}
			}

			// Token: 0x060044D2 RID: 17618 RVA: 0x006C21C0 File Offset: 0x006C03C0
			public void OnPlayerJoining(int playerIndex)
			{
				int num = (int)Math.Ceiling((double)((float)this._perPlayerIsEnabled.Length / 8f));
				NetPacket packet = NetCreativePowersModule.PreparePacket(this.PowerId, num + 1);
				packet.Writer.Write(0);
				for (int i = 0; i < num; i++)
				{
					BitsByte bb = 0;
					for (int j = 0; j < 8; j++)
					{
						int num2 = i * 8 + j;
						if (num2 >= this._perPlayerIsEnabled.Length)
						{
							break;
						}
						bb[j] = this._perPlayerIsEnabled[num2];
					}
					packet.Writer.Write(bb);
				}
				NetManager.Instance.SendToClient(packet, playerIndex);
			}

			// Token: 0x060044D3 RID: 17619 RVA: 0x006C2268 File Offset: 0x006C0468
			public void ProvidePowerButtons(CreativePowerUIElementRequestInfo info, List<UIElement> elements)
			{
				GroupOptionButton<bool> groupOptionButton = CreativePowersHelper.CreateToggleButton(info);
				CreativePowersHelper.UpdateUnlockStateByPower(this, groupOptionButton, Main.OurFavoriteColor);
				groupOptionButton.Append(CreativePowersHelper.GetIconImage(this._iconLocation));
				groupOptionButton.OnLeftClick += this.button_OnClick;
				groupOptionButton.OnUpdate += this.button_OnUpdate;
				elements.Add(groupOptionButton);
			}

			// Token: 0x060044D4 RID: 17620 RVA: 0x006C22C4 File Offset: 0x006C04C4
			private void button_OnUpdate(UIElement affectedElement)
			{
				bool currentOption = this._perPlayerIsEnabled[Main.myPlayer];
				GroupOptionButton<bool> groupOptionButton = affectedElement as GroupOptionButton<bool>;
				groupOptionButton.SetCurrentOption(currentOption);
				if (affectedElement.IsMouseHovering)
				{
					string textValue = Language.GetTextValue(groupOptionButton.IsSelected ? (this._powerNameKey + "_Enabled") : (this._powerNameKey + "_Disabled"));
					CreativePowersHelper.AddDescriptionIfNeeded(ref textValue, this._powerNameKey + "_Description");
					CreativePowersHelper.AddUnlockTextIfNeeded(ref textValue, this.GetIsUnlocked(), this._powerNameKey + "_Unlock");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref textValue);
					Main.instance.MouseTextNoOverride(textValue, 0, 0, -1, -1, -1, -1, 0);
				}
			}

			// Token: 0x060044D5 RID: 17621 RVA: 0x006C2372 File Offset: 0x006C0572
			private void button_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				if (!this.GetIsUnlocked())
				{
					return;
				}
				if (!CreativePowersHelper.IsAvailableForPlayer(this, Main.myPlayer))
				{
					return;
				}
				this.RequestUse();
			}

			// Token: 0x060044D6 RID: 17622
			public abstract bool GetIsUnlocked();

			// Token: 0x04007280 RID: 29312
			internal string _powerNameKey;

			// Token: 0x04007281 RID: 29313
			internal Point _iconLocation;

			// Token: 0x04007282 RID: 29314
			internal bool _defaultToggleState;

			// Token: 0x04007283 RID: 29315
			private bool[] _perPlayerIsEnabled = new bool[255];

			// Token: 0x02000AD8 RID: 2776
			private enum SubMessageType : byte
			{
				// Token: 0x0400784F RID: 30799
				SyncEveryone,
				// Token: 0x04007850 RID: 30800
				SyncOnePlayer
			}
		}

		// Token: 0x02000897 RID: 2199
		public abstract class APerPlayerSliderPower : ICreativePower, IOnPlayerJoining, IProvideSliderElement, IPowerSubcategoryElement
		{
			// Token: 0x17000548 RID: 1352
			// (get) Token: 0x060044D8 RID: 17624 RVA: 0x006C23A9 File Offset: 0x006C05A9
			// (set) Token: 0x060044D9 RID: 17625 RVA: 0x006C23B1 File Offset: 0x006C05B1
			public ushort PowerId { get; set; }

			// Token: 0x17000549 RID: 1353
			// (get) Token: 0x060044DA RID: 17626 RVA: 0x006C23BA File Offset: 0x006C05BA
			// (set) Token: 0x060044DB RID: 17627 RVA: 0x006C23C2 File Offset: 0x006C05C2
			public string ServerConfigName { get; set; }

			// Token: 0x1700054A RID: 1354
			// (get) Token: 0x060044DC RID: 17628 RVA: 0x006C23CB File Offset: 0x006C05CB
			// (set) Token: 0x060044DD RID: 17629 RVA: 0x006C23D3 File Offset: 0x006C05D3
			public PowerPermissionLevel CurrentPermissionLevel { get; set; }

			// Token: 0x1700054B RID: 1355
			// (get) Token: 0x060044DE RID: 17630 RVA: 0x006C23DC File Offset: 0x006C05DC
			// (set) Token: 0x060044DF RID: 17631 RVA: 0x006C23E4 File Offset: 0x006C05E4
			public PowerPermissionLevel DefaultPermissionLevel { get; set; }

			// Token: 0x060044E0 RID: 17632 RVA: 0x006C23ED File Offset: 0x006C05ED
			public bool GetRemappedSliderValueFor(int playerIndex, out float value)
			{
				value = 0f;
				if (!this._cachePerPlayer.IndexInRange(playerIndex))
				{
					return false;
				}
				value = this.RemapSliderValueToPowerValue(this._cachePerPlayer[playerIndex]);
				return true;
			}

			// Token: 0x060044E1 RID: 17633
			public abstract float RemapSliderValueToPowerValue(float sliderValue);

			// Token: 0x060044E2 RID: 17634 RVA: 0x006C2418 File Offset: 0x006C0618
			public void DeserializeNetMessage(BinaryReader reader, int userId)
			{
				int num = (int)reader.ReadByte();
				float num2 = reader.ReadSingle();
				if (Main.netMode == 2)
				{
					num = userId;
					if (!CreativePowersHelper.IsAvailableForPlayer(this, num))
					{
						return;
					}
				}
				this._cachePerPlayer[num] = num2;
				if (num == Main.myPlayer)
				{
					this._sliderCurrentValueCache = num2;
					this.UpdateInfoFromSliderValueCache();
				}
			}

			// Token: 0x060044E3 RID: 17635
			internal abstract void UpdateInfoFromSliderValueCache();

			// Token: 0x060044E4 RID: 17636 RVA: 0x0069C970 File Offset: 0x0069AB70
			public void ProvidePowerButtons(CreativePowerUIElementRequestInfo info, List<UIElement> elements)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060044E5 RID: 17637 RVA: 0x006C2468 File Offset: 0x006C0668
			public void DebugCall()
			{
				NetPacket packet = NetCreativePowersModule.PreparePacket(this.PowerId, 5);
				packet.Writer.Write((byte)Main.myPlayer);
				packet.Writer.Write(0f);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}

			// Token: 0x060044E6 RID: 17638
			public abstract UIElement ProvideSlider();

			// Token: 0x060044E7 RID: 17639 RVA: 0x006C24B0 File Offset: 0x006C06B0
			internal float GetSliderValue()
			{
				if (Main.netMode == 1 && this._needsToCommitChange)
				{
					return this._currentTargetValue;
				}
				return this._sliderCurrentValueCache;
			}

			// Token: 0x060044E8 RID: 17640 RVA: 0x006C24CF File Offset: 0x006C06CF
			internal void SetValueKeyboard(float value)
			{
				if (value == this._currentTargetValue)
				{
					return;
				}
				if (!CreativePowersHelper.IsAvailableForPlayer(this, Main.myPlayer))
				{
					return;
				}
				this._currentTargetValue = value;
				this._needsToCommitChange = true;
			}

			// Token: 0x060044E9 RID: 17641 RVA: 0x006C24F8 File Offset: 0x006C06F8
			internal void SetValueGamepad()
			{
				float sliderValue = this.GetSliderValue();
				float num = UILinksInitializer.HandleSliderVerticalInput(sliderValue, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				if (num != sliderValue)
				{
					this.SetValueKeyboard(num);
				}
			}

			// Token: 0x060044EA RID: 17642 RVA: 0x006C2537 File Offset: 0x006C0737
			public void PushChangeAndSetSlider(float value)
			{
				if (!CreativePowersHelper.IsAvailableForPlayer(this, Main.myPlayer))
				{
					return;
				}
				value = MathHelper.Clamp(value, 0f, 1f);
				this._sliderCurrentValueCache = value;
				this._currentTargetValue = value;
				this.PushChange(value);
			}

			// Token: 0x060044EB RID: 17643 RVA: 0x006C2570 File Offset: 0x006C0770
			public GroupOptionButton<int> GetOptionButton(CreativePowerUIElementRequestInfo info, int optionIndex, int currentOptionIndex)
			{
				GroupOptionButton<int> groupOptionButton = CreativePowersHelper.CreateCategoryButton<int>(info, optionIndex, currentOptionIndex);
				CreativePowersHelper.UpdateUnlockStateByPower(this, groupOptionButton, CreativePowersHelper.CommonSelectedColor);
				groupOptionButton.Append(CreativePowersHelper.GetIconImage(this._iconLocation));
				groupOptionButton.OnUpdate += this.categoryButton_OnUpdate;
				return groupOptionButton;
			}

			// Token: 0x060044EC RID: 17644 RVA: 0x006C25B8 File Offset: 0x006C07B8
			private void categoryButton_OnUpdate(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					GroupOptionButton<int> groupOptionButton = affectedElement as GroupOptionButton<int>;
					string textValue = Language.GetTextValue(this._powerNameKey + (groupOptionButton.IsSelected ? "_Opened" : "_Closed"));
					CreativePowersHelper.AddDescriptionIfNeeded(ref textValue, this._powerNameKey + "_Description");
					CreativePowersHelper.AddUnlockTextIfNeeded(ref textValue, this.GetIsUnlocked(), this._powerNameKey + "_Unlock");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref textValue);
					Main.instance.MouseTextNoOverride(textValue, 0, 0, -1, -1, -1, -1, 0);
				}
				this.AttemptPushingChange();
			}

			// Token: 0x060044ED RID: 17645 RVA: 0x006C2650 File Offset: 0x006C0850
			private void AttemptPushingChange()
			{
				if (!this._needsToCommitChange)
				{
					return;
				}
				if (DateTime.UtcNow.CompareTo(this._nextTimeWeCanPush) == -1)
				{
					return;
				}
				this.PushChange(this._currentTargetValue);
			}

			// Token: 0x060044EE RID: 17646 RVA: 0x006C268C File Offset: 0x006C088C
			internal void PushChange(float newSliderValue)
			{
				this._needsToCommitChange = false;
				this._sliderCurrentValueCache = newSliderValue;
				this._nextTimeWeCanPush = DateTime.UtcNow;
				NetPacket packet = NetCreativePowersModule.PreparePacket(this.PowerId, 5);
				packet.Writer.Write((byte)Main.myPlayer);
				packet.Writer.Write(newSliderValue);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}

			// Token: 0x060044EF RID: 17647 RVA: 0x006C26EC File Offset: 0x006C08EC
			public virtual void Reset()
			{
				for (int i = 0; i < this._cachePerPlayer.Length; i++)
				{
					this.ResetForPlayer(i);
				}
			}

			// Token: 0x060044F0 RID: 17648 RVA: 0x006C2713 File Offset: 0x006C0913
			public virtual void ResetForPlayer(int playerIndex)
			{
				this._cachePerPlayer[playerIndex] = this._sliderDefaultValue;
				if (playerIndex == Main.myPlayer)
				{
					this._sliderCurrentValueCache = this._sliderDefaultValue;
					this._currentTargetValue = this._sliderDefaultValue;
				}
			}

			// Token: 0x060044F1 RID: 17649 RVA: 0x006C2743 File Offset: 0x006C0943
			public void OnPlayerJoining(int playerIndex)
			{
				this.ResetForPlayer(playerIndex);
			}

			// Token: 0x060044F2 RID: 17650
			public abstract bool GetIsUnlocked();

			// Token: 0x04007288 RID: 29320
			internal Point _iconLocation;

			// Token: 0x04007289 RID: 29321
			internal float _sliderCurrentValueCache;

			// Token: 0x0400728A RID: 29322
			internal string _powerNameKey;

			// Token: 0x0400728B RID: 29323
			internal float[] _cachePerPlayer = new float[256];

			// Token: 0x0400728C RID: 29324
			internal float _sliderDefaultValue;

			// Token: 0x0400728D RID: 29325
			private float _currentTargetValue;

			// Token: 0x0400728E RID: 29326
			private bool _needsToCommitChange;

			// Token: 0x0400728F RID: 29327
			private DateTime _nextTimeWeCanPush = DateTime.UtcNow;
		}

		// Token: 0x02000898 RID: 2200
		public abstract class ASharedButtonPower : ICreativePower
		{
			// Token: 0x1700054C RID: 1356
			// (get) Token: 0x060044F4 RID: 17652 RVA: 0x006C276F File Offset: 0x006C096F
			// (set) Token: 0x060044F5 RID: 17653 RVA: 0x006C2777 File Offset: 0x006C0977
			public ushort PowerId { get; set; }

			// Token: 0x1700054D RID: 1357
			// (get) Token: 0x060044F6 RID: 17654 RVA: 0x006C2780 File Offset: 0x006C0980
			// (set) Token: 0x060044F7 RID: 17655 RVA: 0x006C2788 File Offset: 0x006C0988
			public string ServerConfigName { get; set; }

			// Token: 0x1700054E RID: 1358
			// (get) Token: 0x060044F8 RID: 17656 RVA: 0x006C2791 File Offset: 0x006C0991
			// (set) Token: 0x060044F9 RID: 17657 RVA: 0x006C2799 File Offset: 0x006C0999
			public PowerPermissionLevel CurrentPermissionLevel { get; set; }

			// Token: 0x1700054F RID: 1359
			// (get) Token: 0x060044FA RID: 17658 RVA: 0x006C27A2 File Offset: 0x006C09A2
			// (set) Token: 0x060044FB RID: 17659 RVA: 0x006C27AA File Offset: 0x006C09AA
			public PowerPermissionLevel DefaultPermissionLevel { get; set; }

			// Token: 0x060044FC RID: 17660 RVA: 0x006C27B3 File Offset: 0x006C09B3
			public ASharedButtonPower()
			{
				this.OnCreation();
			}

			// Token: 0x060044FD RID: 17661 RVA: 0x006C27C4 File Offset: 0x006C09C4
			public void RequestUse()
			{
				NetPacket packet = NetCreativePowersModule.PreparePacket(this.PowerId, 0);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}

			// Token: 0x060044FE RID: 17662 RVA: 0x006C27E9 File Offset: 0x006C09E9
			public void DeserializeNetMessage(BinaryReader reader, int userId)
			{
				if (Main.netMode == 2 && !CreativePowersHelper.IsAvailableForPlayer(this, userId))
				{
					return;
				}
				this.UsePower();
			}

			// Token: 0x060044FF RID: 17663
			internal abstract void UsePower();

			// Token: 0x06004500 RID: 17664
			internal abstract void OnCreation();

			// Token: 0x06004501 RID: 17665 RVA: 0x006C2804 File Offset: 0x006C0A04
			public void ProvidePowerButtons(CreativePowerUIElementRequestInfo info, List<UIElement> elements)
			{
				GroupOptionButton<bool> groupOptionButton = CreativePowersHelper.CreateSimpleButton(info);
				CreativePowersHelper.UpdateUnlockStateByPower(this, groupOptionButton, CreativePowersHelper.CommonSelectedColor);
				groupOptionButton.Append(CreativePowersHelper.GetIconImage(this._iconLocation));
				groupOptionButton.OnLeftClick += this.button_OnClick;
				groupOptionButton.OnUpdate += this.button_OnUpdate;
				elements.Add(groupOptionButton);
			}

			// Token: 0x06004502 RID: 17666 RVA: 0x006C2860 File Offset: 0x006C0A60
			private void button_OnUpdate(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string textValue = Language.GetTextValue(this._powerNameKey);
					CreativePowersHelper.AddDescriptionIfNeeded(ref textValue, this._descriptionKey);
					CreativePowersHelper.AddUnlockTextIfNeeded(ref textValue, this.GetIsUnlocked(), this._powerNameKey + "_Unlock");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref textValue);
					Main.instance.MouseTextNoOverride(textValue, 0, 0, -1, -1, -1, -1, 0);
				}
			}

			// Token: 0x06004503 RID: 17667 RVA: 0x006C28C5 File Offset: 0x006C0AC5
			private void button_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				if (!CreativePowersHelper.IsAvailableForPlayer(this, Main.myPlayer))
				{
					return;
				}
				this.RequestUse();
			}

			// Token: 0x06004504 RID: 17668
			public abstract bool GetIsUnlocked();

			// Token: 0x04007294 RID: 29332
			internal Point _iconLocation;

			// Token: 0x04007295 RID: 29333
			internal string _powerNameKey;

			// Token: 0x04007296 RID: 29334
			internal string _descriptionKey;
		}

		// Token: 0x02000899 RID: 2201
		public abstract class ASharedTogglePower : ICreativePower, IOnPlayerJoining
		{
			// Token: 0x17000550 RID: 1360
			// (get) Token: 0x06004505 RID: 17669 RVA: 0x006C28DB File Offset: 0x006C0ADB
			// (set) Token: 0x06004506 RID: 17670 RVA: 0x006C28E3 File Offset: 0x006C0AE3
			public ushort PowerId { get; set; }

			// Token: 0x17000551 RID: 1361
			// (get) Token: 0x06004507 RID: 17671 RVA: 0x006C28EC File Offset: 0x006C0AEC
			// (set) Token: 0x06004508 RID: 17672 RVA: 0x006C28F4 File Offset: 0x006C0AF4
			public string ServerConfigName { get; set; }

			// Token: 0x17000552 RID: 1362
			// (get) Token: 0x06004509 RID: 17673 RVA: 0x006C28FD File Offset: 0x006C0AFD
			// (set) Token: 0x0600450A RID: 17674 RVA: 0x006C2905 File Offset: 0x006C0B05
			public PowerPermissionLevel CurrentPermissionLevel { get; set; }

			// Token: 0x17000553 RID: 1363
			// (get) Token: 0x0600450B RID: 17675 RVA: 0x006C290E File Offset: 0x006C0B0E
			// (set) Token: 0x0600450C RID: 17676 RVA: 0x006C2916 File Offset: 0x006C0B16
			public PowerPermissionLevel DefaultPermissionLevel { get; set; }

			// Token: 0x17000554 RID: 1364
			// (get) Token: 0x0600450D RID: 17677 RVA: 0x006C291F File Offset: 0x006C0B1F
			// (set) Token: 0x0600450E RID: 17678 RVA: 0x006C2927 File Offset: 0x006C0B27
			public bool Enabled { get; private set; }

			// Token: 0x0600450F RID: 17679 RVA: 0x006C2930 File Offset: 0x006C0B30
			public void SetPowerInfo(bool enabled)
			{
				this.Enabled = enabled;
			}

			// Token: 0x06004510 RID: 17680 RVA: 0x006C2939 File Offset: 0x006C0B39
			public void Reset()
			{
				this.Enabled = false;
			}

			// Token: 0x06004511 RID: 17681 RVA: 0x006C2944 File Offset: 0x006C0B44
			public void OnPlayerJoining(int playerIndex)
			{
				NetPacket packet = NetCreativePowersModule.PreparePacket(this.PowerId, 1);
				packet.Writer.Write(this.Enabled);
				NetManager.Instance.SendToClient(packet, playerIndex);
			}

			// Token: 0x06004512 RID: 17682 RVA: 0x006C297C File Offset: 0x006C0B7C
			public void DeserializeNetMessage(BinaryReader reader, int userId)
			{
				bool powerInfo = reader.ReadBoolean();
				if (Main.netMode == 2 && !CreativePowersHelper.IsAvailableForPlayer(this, userId))
				{
					return;
				}
				this.SetPowerInfo(powerInfo);
				if (Main.netMode == 2)
				{
					NetPacket packet = NetCreativePowersModule.PreparePacket(this.PowerId, 1);
					packet.Writer.Write(this.Enabled);
					NetManager.Instance.Broadcast(packet, -1);
				}
			}

			// Token: 0x06004513 RID: 17683 RVA: 0x006C29DC File Offset: 0x006C0BDC
			private void RequestUse()
			{
				NetPacket packet = NetCreativePowersModule.PreparePacket(this.PowerId, 1);
				packet.Writer.Write(!this.Enabled);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}

			// Token: 0x06004514 RID: 17684 RVA: 0x006C2A18 File Offset: 0x006C0C18
			public void ProvidePowerButtons(CreativePowerUIElementRequestInfo info, List<UIElement> elements)
			{
				GroupOptionButton<bool> groupOptionButton = CreativePowersHelper.CreateToggleButton(info);
				CreativePowersHelper.UpdateUnlockStateByPower(this, groupOptionButton, Main.OurFavoriteColor);
				this.CustomizeButton(groupOptionButton);
				groupOptionButton.OnLeftClick += this.button_OnClick;
				groupOptionButton.OnUpdate += this.button_OnUpdate;
				elements.Add(groupOptionButton);
			}

			// Token: 0x06004515 RID: 17685 RVA: 0x006C2A6C File Offset: 0x006C0C6C
			private void button_OnUpdate(UIElement affectedElement)
			{
				bool enabled = this.Enabled;
				GroupOptionButton<bool> groupOptionButton = affectedElement as GroupOptionButton<bool>;
				groupOptionButton.SetCurrentOption(enabled);
				if (affectedElement.IsMouseHovering)
				{
					string buttonTextKey = this.GetButtonTextKey();
					string textValue = Language.GetTextValue(buttonTextKey + (groupOptionButton.IsSelected ? "_Enabled" : "_Disabled"));
					CreativePowersHelper.AddDescriptionIfNeeded(ref textValue, buttonTextKey + "_Description");
					CreativePowersHelper.AddUnlockTextIfNeeded(ref textValue, this.GetIsUnlocked(), buttonTextKey + "_Unlock");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref textValue);
					Main.instance.MouseTextNoOverride(textValue, 0, 0, -1, -1, -1, -1, 0);
				}
			}

			// Token: 0x06004516 RID: 17686 RVA: 0x006C2B01 File Offset: 0x006C0D01
			private void button_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				if (!CreativePowersHelper.IsAvailableForPlayer(this, Main.myPlayer))
				{
					return;
				}
				this.RequestUse();
			}

			// Token: 0x06004517 RID: 17687
			internal abstract void CustomizeButton(UIElement button);

			// Token: 0x06004518 RID: 17688
			internal abstract string GetButtonTextKey();

			// Token: 0x06004519 RID: 17689
			public abstract bool GetIsUnlocked();
		}

		// Token: 0x0200089A RID: 2202
		public abstract class ASharedSliderPower : ICreativePower, IOnPlayerJoining, IProvideSliderElement, IPowerSubcategoryElement
		{
			// Token: 0x17000555 RID: 1365
			// (get) Token: 0x0600451B RID: 17691 RVA: 0x006C2B17 File Offset: 0x006C0D17
			// (set) Token: 0x0600451C RID: 17692 RVA: 0x006C2B1F File Offset: 0x006C0D1F
			public ushort PowerId { get; set; }

			// Token: 0x17000556 RID: 1366
			// (get) Token: 0x0600451D RID: 17693 RVA: 0x006C2B28 File Offset: 0x006C0D28
			// (set) Token: 0x0600451E RID: 17694 RVA: 0x006C2B30 File Offset: 0x006C0D30
			public string ServerConfigName { get; set; }

			// Token: 0x17000557 RID: 1367
			// (get) Token: 0x0600451F RID: 17695 RVA: 0x006C2B39 File Offset: 0x006C0D39
			// (set) Token: 0x06004520 RID: 17696 RVA: 0x006C2B41 File Offset: 0x006C0D41
			public PowerPermissionLevel CurrentPermissionLevel { get; set; }

			// Token: 0x17000558 RID: 1368
			// (get) Token: 0x06004521 RID: 17697 RVA: 0x006C2B4A File Offset: 0x006C0D4A
			// (set) Token: 0x06004522 RID: 17698 RVA: 0x006C2B52 File Offset: 0x006C0D52
			public PowerPermissionLevel DefaultPermissionLevel { get; set; }

			// Token: 0x06004523 RID: 17699 RVA: 0x006C2B5C File Offset: 0x006C0D5C
			public void DeserializeNetMessage(BinaryReader reader, int userId)
			{
				float num = reader.ReadSingle();
				if (Main.netMode == 2 && !CreativePowersHelper.IsAvailableForPlayer(this, userId))
				{
					return;
				}
				this._sliderCurrentValueCache = num;
				this.UpdateInfoFromSliderValueCache();
				if (Main.netMode == 2)
				{
					NetPacket packet = NetCreativePowersModule.PreparePacket(this.PowerId, 4);
					packet.Writer.Write(num);
					NetManager.Instance.Broadcast(packet, -1);
				}
			}

			// Token: 0x06004524 RID: 17700
			internal abstract void UpdateInfoFromSliderValueCache();

			// Token: 0x06004525 RID: 17701 RVA: 0x0069C970 File Offset: 0x0069AB70
			public void ProvidePowerButtons(CreativePowerUIElementRequestInfo info, List<UIElement> elements)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06004526 RID: 17702 RVA: 0x006C2BC0 File Offset: 0x006C0DC0
			public void DebugCall()
			{
				NetPacket packet = NetCreativePowersModule.PreparePacket(this.PowerId, 4);
				packet.Writer.Write(0f);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}

			// Token: 0x06004527 RID: 17703
			public abstract UIElement ProvideSlider();

			// Token: 0x06004528 RID: 17704 RVA: 0x006C2BF6 File Offset: 0x006C0DF6
			internal float GetSliderValue()
			{
				if (Main.netMode == 1 && this._needsToCommitChange)
				{
					return this._currentTargetValue;
				}
				return this.GetSliderValueInner();
			}

			// Token: 0x06004529 RID: 17705 RVA: 0x006C2C15 File Offset: 0x006C0E15
			internal virtual float GetSliderValueInner()
			{
				return this._sliderCurrentValueCache;
			}

			// Token: 0x0600452A RID: 17706 RVA: 0x006C2C1D File Offset: 0x006C0E1D
			internal void SetValueKeyboard(float value)
			{
				if (value == this._currentTargetValue)
				{
					return;
				}
				this.SetValueKeyboardForced(value);
			}

			// Token: 0x0600452B RID: 17707 RVA: 0x006C2C30 File Offset: 0x006C0E30
			internal void SetValueKeyboardForced(float value)
			{
				if (!CreativePowersHelper.IsAvailableForPlayer(this, Main.myPlayer))
				{
					return;
				}
				this._currentTargetValue = value;
				this._needsToCommitChange = true;
			}

			// Token: 0x0600452C RID: 17708 RVA: 0x006C2C50 File Offset: 0x006C0E50
			internal void SetValueGamepad()
			{
				float sliderValue = this.GetSliderValue();
				float num = UILinksInitializer.HandleSliderVerticalInput(sliderValue, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				if (num != sliderValue)
				{
					this.SetValueKeyboard(num);
				}
			}

			// Token: 0x0600452D RID: 17709 RVA: 0x006C2C90 File Offset: 0x006C0E90
			public GroupOptionButton<int> GetOptionButton(CreativePowerUIElementRequestInfo info, int optionIndex, int currentOptionIndex)
			{
				GroupOptionButton<int> groupOptionButton = CreativePowersHelper.CreateCategoryButton<int>(info, optionIndex, currentOptionIndex);
				CreativePowersHelper.UpdateUnlockStateByPower(this, groupOptionButton, CreativePowersHelper.CommonSelectedColor);
				groupOptionButton.Append(CreativePowersHelper.GetIconImage(this._iconLocation));
				groupOptionButton.OnUpdate += this.categoryButton_OnUpdate;
				return groupOptionButton;
			}

			// Token: 0x0600452E RID: 17710 RVA: 0x006C2CD8 File Offset: 0x006C0ED8
			private void categoryButton_OnUpdate(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					GroupOptionButton<int> groupOptionButton = affectedElement as GroupOptionButton<int>;
					string textValue = Language.GetTextValue(this._powerNameKey + (groupOptionButton.IsSelected ? "_Opened" : "_Closed"));
					CreativePowersHelper.AddDescriptionIfNeeded(ref textValue, this._powerNameKey + "_Description");
					CreativePowersHelper.AddUnlockTextIfNeeded(ref textValue, this.GetIsUnlocked(), this._powerNameKey + "_Unlock");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref textValue);
					Main.instance.MouseTextNoOverride(textValue, 0, 0, -1, -1, -1, -1, 0);
				}
				this.AttemptPushingChange();
			}

			// Token: 0x0600452F RID: 17711 RVA: 0x006C2D70 File Offset: 0x006C0F70
			private void AttemptPushingChange()
			{
				if (!this._needsToCommitChange)
				{
					return;
				}
				if (DateTime.UtcNow.CompareTo(this._nextTimeWeCanPush) == -1)
				{
					return;
				}
				this._needsToCommitChange = false;
				this._sliderCurrentValueCache = this._currentTargetValue;
				this._nextTimeWeCanPush = DateTime.UtcNow;
				NetPacket packet = NetCreativePowersModule.PreparePacket(this.PowerId, 4);
				packet.Writer.Write(this._currentTargetValue);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}

			// Token: 0x06004530 RID: 17712 RVA: 0x006C2DE5 File Offset: 0x006C0FE5
			public virtual void Reset()
			{
				this._sliderCurrentValueCache = 0f;
			}

			// Token: 0x06004531 RID: 17713 RVA: 0x006C2DF4 File Offset: 0x006C0FF4
			public void OnPlayerJoining(int playerIndex)
			{
				if (!this._syncToJoiningPlayers)
				{
					return;
				}
				NetPacket packet = NetCreativePowersModule.PreparePacket(this.PowerId, 4);
				packet.Writer.Write(this._sliderCurrentValueCache);
				NetManager.Instance.SendToClient(packet, playerIndex);
			}

			// Token: 0x06004532 RID: 17714
			public abstract bool GetIsUnlocked();

			// Token: 0x040072A0 RID: 29344
			internal Point _iconLocation;

			// Token: 0x040072A1 RID: 29345
			internal float _sliderCurrentValueCache;

			// Token: 0x040072A2 RID: 29346
			internal string _powerNameKey;

			// Token: 0x040072A3 RID: 29347
			internal bool _syncToJoiningPlayers = true;

			// Token: 0x040072A4 RID: 29348
			internal float _currentTargetValue;

			// Token: 0x040072A5 RID: 29349
			private bool _needsToCommitChange;

			// Token: 0x040072A6 RID: 29350
			private DateTime _nextTimeWeCanPush = DateTime.UtcNow;
		}

		// Token: 0x0200089B RID: 2203
		public class GodmodePower : CreativePowers.APerPlayerTogglePower, IPersistentPerPlayerContent
		{
			// Token: 0x06004534 RID: 17716 RVA: 0x006C2E4F File Offset: 0x006C104F
			public GodmodePower()
			{
				this._powerNameKey = "CreativePowers.Godmode";
				this._iconLocation = CreativePowersHelper.CreativePowerIconLocations.Godmode;
			}

			// Token: 0x06004535 RID: 17717 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}

			// Token: 0x06004536 RID: 17718 RVA: 0x006C2E70 File Offset: 0x006C1070
			public void Save(Player player, BinaryWriter writer)
			{
				bool value = base.IsEnabledForPlayer(Main.myPlayer);
				writer.Write(value);
			}

			// Token: 0x06004537 RID: 17719 RVA: 0x006C2E90 File Offset: 0x006C1090
			public void ResetDataForNewPlayer(Player player)
			{
				player.savedPerPlayerFieldsThatArentInThePlayerClass.godmodePowerEnabled = this._defaultToggleState;
			}

			// Token: 0x06004538 RID: 17720 RVA: 0x006C2EA4 File Offset: 0x006C10A4
			public void Load(Player player, BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				bool godmodePowerEnabled = reader.ReadBoolean();
				player.savedPerPlayerFieldsThatArentInThePlayerClass.godmodePowerEnabled = godmodePowerEnabled;
			}

			// Token: 0x06004539 RID: 17721 RVA: 0x006C2EC4 File Offset: 0x006C10C4
			public void ApplyLoadedDataToOutOfPlayerFields(Player player)
			{
				if (player.savedPerPlayerFieldsThatArentInThePlayerClass.godmodePowerEnabled != base.IsEnabledForPlayer(player.whoAmI))
				{
					base.RequestUse();
				}
			}
		}

		// Token: 0x0200089C RID: 2204
		public class FarPlacementRangePower : CreativePowers.APerPlayerTogglePower, IPersistentPerPlayerContent
		{
			// Token: 0x0600453A RID: 17722 RVA: 0x006C2EE5 File Offset: 0x006C10E5
			public FarPlacementRangePower()
			{
				this._powerNameKey = "CreativePowers.InfinitePlacementRange";
				this._iconLocation = CreativePowersHelper.CreativePowerIconLocations.BlockPlacementRange;
				this._defaultToggleState = true;
			}

			// Token: 0x0600453B RID: 17723 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}

			// Token: 0x0600453C RID: 17724 RVA: 0x006C2F0C File Offset: 0x006C110C
			public void Save(Player player, BinaryWriter writer)
			{
				bool value = base.IsEnabledForPlayer(Main.myPlayer);
				writer.Write(value);
			}

			// Token: 0x0600453D RID: 17725 RVA: 0x006C2F2C File Offset: 0x006C112C
			public void ResetDataForNewPlayer(Player player)
			{
				player.savedPerPlayerFieldsThatArentInThePlayerClass.farPlacementRangePowerEnabled = this._defaultToggleState;
			}

			// Token: 0x0600453E RID: 17726 RVA: 0x006C2F40 File Offset: 0x006C1140
			public void Load(Player player, BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				bool farPlacementRangePowerEnabled = reader.ReadBoolean();
				player.savedPerPlayerFieldsThatArentInThePlayerClass.farPlacementRangePowerEnabled = farPlacementRangePowerEnabled;
			}

			// Token: 0x0600453F RID: 17727 RVA: 0x006C2F60 File Offset: 0x006C1160
			public void ApplyLoadedDataToOutOfPlayerFields(Player player)
			{
				if (player.savedPerPlayerFieldsThatArentInThePlayerClass.farPlacementRangePowerEnabled != base.IsEnabledForPlayer(player.whoAmI))
				{
					base.RequestUse();
				}
			}
		}

		// Token: 0x0200089D RID: 2205
		public class StartDayImmediately : CreativePowers.ASharedButtonPower
		{
			// Token: 0x06004540 RID: 17728 RVA: 0x006C2F81 File Offset: 0x006C1181
			internal override void UsePower()
			{
				if (Main.netMode == 1)
				{
					return;
				}
				Main.SkipToTime(0, true);
			}

			// Token: 0x06004541 RID: 17729 RVA: 0x006C2F93 File Offset: 0x006C1193
			internal override void OnCreation()
			{
				this._powerNameKey = "CreativePowers.StartDayImmediately";
				this._descriptionKey = this._powerNameKey + "_Description";
				this._iconLocation = CreativePowersHelper.CreativePowerIconLocations.TimeDawn;
			}

			// Token: 0x06004542 RID: 17730 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}
		}

		// Token: 0x0200089E RID: 2206
		public class StartNightImmediately : CreativePowers.ASharedButtonPower
		{
			// Token: 0x06004544 RID: 17732 RVA: 0x006C2FC9 File Offset: 0x006C11C9
			internal override void UsePower()
			{
				if (Main.netMode == 1)
				{
					return;
				}
				Main.SkipToTime(0, false);
			}

			// Token: 0x06004545 RID: 17733 RVA: 0x006C2FDB File Offset: 0x006C11DB
			internal override void OnCreation()
			{
				this._powerNameKey = "CreativePowers.StartNightImmediately";
				this._descriptionKey = this._powerNameKey + "_Description";
				this._iconLocation = CreativePowersHelper.CreativePowerIconLocations.TimeDusk;
			}

			// Token: 0x06004546 RID: 17734 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}
		}

		// Token: 0x0200089F RID: 2207
		public class StartNoonImmediately : CreativePowers.ASharedButtonPower
		{
			// Token: 0x06004548 RID: 17736 RVA: 0x006C3009 File Offset: 0x006C1209
			internal override void UsePower()
			{
				if (Main.netMode == 1)
				{
					return;
				}
				Main.SkipToTime(27000, true);
			}

			// Token: 0x06004549 RID: 17737 RVA: 0x006C301F File Offset: 0x006C121F
			internal override void OnCreation()
			{
				this._powerNameKey = "CreativePowers.StartNoonImmediately";
				this._descriptionKey = this._powerNameKey + "_Description";
				this._iconLocation = CreativePowersHelper.CreativePowerIconLocations.TimeNoon;
			}

			// Token: 0x0600454A RID: 17738 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}
		}

		// Token: 0x020008A0 RID: 2208
		public class StartMidnightImmediately : CreativePowers.ASharedButtonPower
		{
			// Token: 0x0600454C RID: 17740 RVA: 0x006C304D File Offset: 0x006C124D
			internal override void UsePower()
			{
				if (Main.netMode == 1)
				{
					return;
				}
				Main.SkipToTime(16200, false);
			}

			// Token: 0x0600454D RID: 17741 RVA: 0x006C3063 File Offset: 0x006C1263
			internal override void OnCreation()
			{
				this._powerNameKey = "CreativePowers.StartMidnightImmediately";
				this._descriptionKey = this._powerNameKey + "_Description";
				this._iconLocation = CreativePowersHelper.CreativePowerIconLocations.TimeMidnight;
			}

			// Token: 0x0600454E RID: 17742 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}
		}

		// Token: 0x020008A1 RID: 2209
		public class ModifyTimeRate : CreativePowers.ASharedSliderPower, IPersistentPerWorldContent
		{
			// Token: 0x17000559 RID: 1369
			// (get) Token: 0x06004550 RID: 17744 RVA: 0x006C3091 File Offset: 0x006C1291
			// (set) Token: 0x06004551 RID: 17745 RVA: 0x006C3099 File Offset: 0x006C1299
			public int TargetTimeRate { get; private set; }

			// Token: 0x06004552 RID: 17746 RVA: 0x006C30A2 File Offset: 0x006C12A2
			public ModifyTimeRate()
			{
				this._powerNameKey = "CreativePowers.ModifyTimeRate";
				this._iconLocation = CreativePowersHelper.CreativePowerIconLocations.ModifyTime;
			}

			// Token: 0x06004553 RID: 17747 RVA: 0x006C30C0 File Offset: 0x006C12C0
			public override void Reset()
			{
				this._sliderCurrentValueCache = 0f;
				this.TargetTimeRate = 1;
			}

			// Token: 0x06004554 RID: 17748 RVA: 0x006C30D4 File Offset: 0x006C12D4
			internal override void UpdateInfoFromSliderValueCache()
			{
				this.TargetTimeRate = (int)Math.Round((double)Utils.Remap(this._sliderCurrentValueCache, 0f, 1f, 1f, 24f, true));
			}

			// Token: 0x06004555 RID: 17749 RVA: 0x006C3104 File Offset: 0x006C1304
			public override UIElement ProvideSlider()
			{
				UIVerticalSlider uiverticalSlider = CreativePowersHelper.CreateSlider(new Func<float>(base.GetSliderValue), new Action<float>(base.SetValueKeyboard), new Action(base.SetValueGamepad));
				uiverticalSlider.OnUpdate += this.UpdateSliderAndShowMultiplierMouseOver;
				UIPanel uipanel = new UIPanel();
				uipanel.Width = new StyleDimension(87f, 0f);
				uipanel.Height = new StyleDimension(180f, 0f);
				uipanel.HAlign = 0f;
				uipanel.VAlign = 0.5f;
				uipanel.Append(uiverticalSlider);
				uipanel.OnUpdate += CreativePowersHelper.UpdateUseMouseInterface;
				UIText uitext = new UIText("x24", 1f, false)
				{
					HAlign = 1f,
					VAlign = 0f
				};
				uitext.OnUpdate += this.UpdateMouseOverNoItemText;
				uitext.OnMouseOver += this.Button_OnMouseOver;
				uitext.OnMouseOut += this.Button_OnMouseOut;
				uitext.OnLeftClick += this.topText_OnClick;
				uipanel.Append(uitext);
				UIText uitext2 = new UIText("x12", 1f, false)
				{
					HAlign = 1f,
					VAlign = 0.5f
				};
				uitext2.OnUpdate += this.UpdateMouseOverNoItemText;
				uitext2.OnMouseOver += this.Button_OnMouseOver;
				uitext2.OnMouseOut += this.Button_OnMouseOut;
				uitext2.OnLeftClick += this.middleText_OnClick;
				uipanel.Append(uitext2);
				UIText uitext3 = new UIText("x1", 1f, false)
				{
					HAlign = 1f,
					VAlign = 1f
				};
				uitext3.OnUpdate += this.UpdateMouseOverNoItemText;
				uitext3.OnMouseOver += this.Button_OnMouseOver;
				uitext3.OnMouseOut += this.Button_OnMouseOut;
				uitext3.OnLeftClick += this.bottomText_OnClick;
				uipanel.Append(uitext3);
				return uipanel;
			}

			// Token: 0x06004556 RID: 17750 RVA: 0x006C330D File Offset: 0x006C150D
			private void bottomText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboardForced(0f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x06004557 RID: 17751 RVA: 0x006C332F File Offset: 0x006C152F
			private void middleText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboardForced(0.5f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x06004558 RID: 17752 RVA: 0x006C3351 File Offset: 0x006C1551
			private void topText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboardForced(1f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x06004559 RID: 17753 RVA: 0x006C3374 File Offset: 0x006C1574
			private void Button_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
			{
				UIText uitext = listeningElement as UIText;
				if (uitext != null)
				{
					uitext.ShadowColor = Color.Black;
				}
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x0600455A RID: 17754 RVA: 0x006C33AC File Offset: 0x006C15AC
			private void Button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
			{
				UIText uitext = listeningElement as UIText;
				if (uitext != null)
				{
					uitext.ShadowColor = Main.OurFavoriteColor;
				}
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x0600455B RID: 17755 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}

			// Token: 0x0600455C RID: 17756 RVA: 0x006C33E3 File Offset: 0x006C15E3
			public void Save(BinaryWriter writer)
			{
				writer.Write(this._sliderCurrentValueCache);
			}

			// Token: 0x0600455D RID: 17757 RVA: 0x006C33F1 File Offset: 0x006C15F1
			public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				this._sliderCurrentValueCache = reader.ReadSingle();
				this.UpdateInfoFromSliderValueCache();
			}

			// Token: 0x0600455E RID: 17758 RVA: 0x006C3405 File Offset: 0x006C1605
			public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				reader.ReadSingle();
			}

			// Token: 0x0600455F RID: 17759 RVA: 0x006C3410 File Offset: 0x006C1610
			private void UpdateMouseOverNoItemText(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					Main.instance.MouseTextNoOverride(string.Empty, 0, 0, -1, -1, -1, -1, 0);
				}
			}

			// Token: 0x06004560 RID: 17760 RVA: 0x006C343C File Offset: 0x006C163C
			private void UpdateSliderAndShowMultiplierMouseOver(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string cursorText = "x" + this.TargetTimeRate.ToString();
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref cursorText);
					Main.instance.MouseTextNoOverride(cursorText, 0, 0, -1, -1, -1, -1, 0);
				}
			}
		}

		// Token: 0x020008A2 RID: 2210
		public class DifficultySliderPower : CreativePowers.ASharedSliderPower, IPersistentPerWorldContent
		{
			// Token: 0x1700055A RID: 1370
			// (get) Token: 0x06004561 RID: 17761 RVA: 0x006C3484 File Offset: 0x006C1684
			// (set) Token: 0x06004562 RID: 17762 RVA: 0x006C348C File Offset: 0x006C168C
			public float StrengthMultiplierToGiveNPCs { get; private set; }

			// Token: 0x06004563 RID: 17763 RVA: 0x006C3495 File Offset: 0x006C1695
			public DifficultySliderPower()
			{
				this._powerNameKey = "CreativePowers.DifficultySlider";
				this._iconLocation = CreativePowersHelper.CreativePowerIconLocations.EnemyStrengthSlider;
			}

			// Token: 0x06004564 RID: 17764 RVA: 0x006C34B3 File Offset: 0x006C16B3
			public override void Reset()
			{
				this._sliderCurrentValueCache = 0f;
				this.UpdateInfoFromSliderValueCache();
			}

			// Token: 0x06004565 RID: 17765 RVA: 0x006C34C8 File Offset: 0x006C16C8
			internal override void UpdateInfoFromSliderValueCache()
			{
				if (this._sliderCurrentValueCache <= 0.33f)
				{
					this.StrengthMultiplierToGiveNPCs = Utils.Remap(this._sliderCurrentValueCache, 0f, 0.33f, 0.5f, 1f, true);
				}
				else
				{
					this.StrengthMultiplierToGiveNPCs = Utils.Remap(this._sliderCurrentValueCache, 0.33f, 1f, 1f, 3f, true);
				}
				float strengthMultiplierToGiveNPCs = (float)Math.Round((double)(this.StrengthMultiplierToGiveNPCs * 20f)) / 20f;
				this.StrengthMultiplierToGiveNPCs = strengthMultiplierToGiveNPCs;
			}

			// Token: 0x06004566 RID: 17766 RVA: 0x006C3554 File Offset: 0x006C1754
			public override UIElement ProvideSlider()
			{
				UIVerticalSlider uiverticalSlider = CreativePowersHelper.CreateSlider(new Func<float>(base.GetSliderValue), new Action<float>(base.SetValueKeyboard), new Action(base.SetValueGamepad));
				UIPanel uipanel = new UIPanel();
				uipanel.Width = new StyleDimension(82f, 0f);
				uipanel.Height = new StyleDimension(180f, 0f);
				uipanel.HAlign = 0f;
				uipanel.VAlign = 0.5f;
				uipanel.Append(uiverticalSlider);
				uipanel.OnUpdate += CreativePowersHelper.UpdateUseMouseInterface;
				uiverticalSlider.OnUpdate += this.UpdateSliderColorAndShowMultiplierMouseOver;
				CreativePowers.DifficultySliderPower.AddIndication(uipanel, 0f, "x3", "Images/UI/WorldCreation/IconDifficultyMaster", new UIElement.ElementEvent(this.MouseOver_Master), new UIElement.MouseEvent(this.Click_Master));
				CreativePowers.DifficultySliderPower.AddIndication(uipanel, 0.33333334f, "x2", "Images/UI/WorldCreation/IconDifficultyExpert", new UIElement.ElementEvent(this.MouseOver_Expert), new UIElement.MouseEvent(this.Click_Expert));
				CreativePowers.DifficultySliderPower.AddIndication(uipanel, 0.6666667f, "x1", "Images/UI/WorldCreation/IconDifficultyNormal", new UIElement.ElementEvent(this.MouseOver_Normal), new UIElement.MouseEvent(this.Click_Normal));
				CreativePowers.DifficultySliderPower.AddIndication(uipanel, 1f, "x0.5", "Images/UI/WorldCreation/IconDifficultyCreative", new UIElement.ElementEvent(this.MouseOver_Journey), new UIElement.MouseEvent(this.Click_Journey));
				return uipanel;
			}

			// Token: 0x06004567 RID: 17767 RVA: 0x006C3351 File Offset: 0x006C1551
			private void Click_Master(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboardForced(1f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x06004568 RID: 17768 RVA: 0x006C36AF File Offset: 0x006C18AF
			private void Click_Expert(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboardForced(0.66f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x06004569 RID: 17769 RVA: 0x006C36D1 File Offset: 0x006C18D1
			private void Click_Normal(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboardForced(0.33f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x0600456A RID: 17770 RVA: 0x006C330D File Offset: 0x006C150D
			private void Click_Journey(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboardForced(0f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x0600456B RID: 17771 RVA: 0x006C36F4 File Offset: 0x006C18F4
			private static void AddIndication(UIPanel panel, float yAnchor, string indicationText, string iconImagePath, UIElement.ElementEvent updateEvent, UIElement.MouseEvent clickEvent)
			{
				UIImage uiimage = new UIImage(Main.Assets.Request<Texture2D>(iconImagePath, 1))
				{
					HAlign = 1f,
					VAlign = yAnchor,
					Left = new StyleDimension(4f, 0f),
					Top = new StyleDimension(2f, 0f),
					RemoveFloatingPointsFromDrawPosition = true
				};
				uiimage.OnMouseOut += CreativePowers.DifficultySliderPower.Button_OnMouseOut;
				uiimage.OnMouseOver += CreativePowers.DifficultySliderPower.Button_OnMouseOver;
				if (updateEvent != null)
				{
					uiimage.OnUpdate += updateEvent;
				}
				if (clickEvent != null)
				{
					uiimage.OnLeftClick += clickEvent;
				}
				panel.Append(uiimage);
			}

			// Token: 0x0600456C RID: 17772 RVA: 0x00592D7A File Offset: 0x00590F7A
			private static void Button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x0600456D RID: 17773 RVA: 0x00592D7A File Offset: 0x00590F7A
			private static void Button_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x0600456E RID: 17774 RVA: 0x006C379C File Offset: 0x006C199C
			private void MouseOver_Journey(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string textValue = Language.GetTextValue("UI.Creative");
					Main.instance.MouseTextNoOverride(textValue, 0, 0, -1, -1, -1, -1, 0);
				}
			}

			// Token: 0x0600456F RID: 17775 RVA: 0x006C37D0 File Offset: 0x006C19D0
			private void MouseOver_Normal(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string textValue = Language.GetTextValue("UI.Normal");
					Main.instance.MouseTextNoOverride(textValue, 0, 0, -1, -1, -1, -1, 0);
				}
			}

			// Token: 0x06004570 RID: 17776 RVA: 0x006C3804 File Offset: 0x006C1A04
			private void MouseOver_Expert(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string textValue = Language.GetTextValue("UI.Expert");
					Main.instance.MouseTextNoOverride(textValue, 0, 0, -1, -1, -1, -1, 0);
				}
			}

			// Token: 0x06004571 RID: 17777 RVA: 0x006C3838 File Offset: 0x006C1A38
			private void MouseOver_Master(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string textValue = Language.GetTextValue("UI.Master");
					Main.instance.MouseTextNoOverride(textValue, 0, 0, -1, -1, -1, -1, 0);
				}
			}

			// Token: 0x06004572 RID: 17778 RVA: 0x006C386C File Offset: 0x006C1A6C
			private void UpdateSliderColorAndShowMultiplierMouseOver(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string cursorText = "x" + this.StrengthMultiplierToGiveNPCs.ToString("F2");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref cursorText);
					Main.instance.MouseTextNoOverride(cursorText, 0, 0, -1, -1, -1, -1, 0);
				}
				UIVerticalSlider uiverticalSlider = affectedElement as UIVerticalSlider;
				if (uiverticalSlider == null)
				{
					return;
				}
				uiverticalSlider.EmptyColor = Color.Black;
				Color filledColor;
				if (Main.masterMode)
				{
					filledColor = Main.hcColor;
				}
				else if (Main.expertMode)
				{
					filledColor = Main.mcColor;
				}
				else if (this.StrengthMultiplierToGiveNPCs < 1f)
				{
					filledColor = Main.creativeModeColor;
				}
				else
				{
					filledColor = Color.White;
				}
				uiverticalSlider.FilledColor = filledColor;
			}

			// Token: 0x06004573 RID: 17779 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}

			// Token: 0x06004574 RID: 17780 RVA: 0x006C33E3 File Offset: 0x006C15E3
			public void Save(BinaryWriter writer)
			{
				writer.Write(this._sliderCurrentValueCache);
			}

			// Token: 0x06004575 RID: 17781 RVA: 0x006C33F1 File Offset: 0x006C15F1
			public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				this._sliderCurrentValueCache = reader.ReadSingle();
				this.UpdateInfoFromSliderValueCache();
			}

			// Token: 0x06004576 RID: 17782 RVA: 0x006C3405 File Offset: 0x006C1605
			public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				reader.ReadSingle();
			}
		}

		// Token: 0x020008A3 RID: 2211
		public class ModifyWindDirectionAndStrength : CreativePowers.ASharedSliderPower
		{
			// Token: 0x06004577 RID: 17783 RVA: 0x006C390F File Offset: 0x006C1B0F
			public ModifyWindDirectionAndStrength()
			{
				this._powerNameKey = "CreativePowers.ModifyWindDirectionAndStrength";
				this._iconLocation = CreativePowersHelper.CreativePowerIconLocations.WindDirection;
				this._syncToJoiningPlayers = false;
			}

			// Token: 0x06004578 RID: 17784 RVA: 0x006C3934 File Offset: 0x006C1B34
			internal override void UpdateInfoFromSliderValueCache()
			{
				Main.windSpeedCurrent = (Main.windSpeedTarget = MathHelper.Lerp(-0.8f, 0.8f, this._sliderCurrentValueCache));
			}

			// Token: 0x06004579 RID: 17785 RVA: 0x006C3956 File Offset: 0x006C1B56
			internal override float GetSliderValueInner()
			{
				return Utils.GetLerpValue(-0.8f, 0.8f, Main.windSpeedTarget, false);
			}

			// Token: 0x0600457A RID: 17786 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}

			// Token: 0x0600457B RID: 17787 RVA: 0x006C3970 File Offset: 0x006C1B70
			public override UIElement ProvideSlider()
			{
				UIVerticalSlider uiverticalSlider = CreativePowersHelper.CreateSlider(new Func<float>(base.GetSliderValue), new Action<float>(base.SetValueKeyboard), new Action(base.SetValueGamepad));
				uiverticalSlider.OnUpdate += this.UpdateSliderAndShowMultiplierMouseOver;
				UIPanel uipanel = new UIPanel();
				uipanel.Width = new StyleDimension(132f, 0f);
				uipanel.Height = new StyleDimension(180f, 0f);
				uipanel.HAlign = 0f;
				uipanel.VAlign = 0.5f;
				uipanel.Append(uiverticalSlider);
				uipanel.OnUpdate += CreativePowersHelper.UpdateUseMouseInterface;
				UIText uitext = new UIText(Language.GetText("CreativePowers.WindWest"), 1f, false)
				{
					HAlign = 1f,
					VAlign = 0f
				};
				uitext.OnUpdate += this.UpdateMouseOverNoItemText;
				uitext.OnMouseOut += this.Button_OnMouseOut;
				uitext.OnMouseOver += this.Button_OnMouseOver;
				uitext.OnLeftClick += this.topText_OnClick;
				uipanel.Append(uitext);
				UIText uitext2 = new UIText(Language.GetText("CreativePowers.WindEast"), 1f, false)
				{
					HAlign = 1f,
					VAlign = 1f
				};
				uitext2.OnUpdate += this.UpdateMouseOverNoItemText;
				uitext2.OnMouseOut += this.Button_OnMouseOut;
				uitext2.OnMouseOver += this.Button_OnMouseOver;
				uitext2.OnLeftClick += this.bottomText_OnClick;
				uipanel.Append(uitext2);
				UIText uitext3 = new UIText(Language.GetText("CreativePowers.WindNone"), 1f, false)
				{
					HAlign = 1f,
					VAlign = 0.5f
				};
				uitext3.OnUpdate += this.UpdateMouseOverNoItemText;
				uitext3.OnMouseOut += this.Button_OnMouseOut;
				uitext3.OnMouseOver += this.Button_OnMouseOver;
				uitext3.OnLeftClick += this.middleText_OnClick;
				uipanel.Append(uitext3);
				return uipanel;
			}

			// Token: 0x0600457C RID: 17788 RVA: 0x006C3351 File Offset: 0x006C1551
			private void topText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboardForced(1f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x0600457D RID: 17789 RVA: 0x006C330D File Offset: 0x006C150D
			private void bottomText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboardForced(0f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x0600457E RID: 17790 RVA: 0x006C332F File Offset: 0x006C152F
			private void middleText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboardForced(0.5f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x0600457F RID: 17791 RVA: 0x006C3B88 File Offset: 0x006C1D88
			private void Button_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
			{
				UIText uitext = listeningElement as UIText;
				if (uitext != null)
				{
					uitext.ShadowColor = Color.Black;
				}
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x06004580 RID: 17792 RVA: 0x006C3BC0 File Offset: 0x006C1DC0
			private void Button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
			{
				UIText uitext = listeningElement as UIText;
				if (uitext != null)
				{
					uitext.ShadowColor = Main.OurFavoriteColor;
				}
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x06004581 RID: 17793 RVA: 0x006C3BF8 File Offset: 0x006C1DF8
			private void UpdateMouseOverNoItemText(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					Main.instance.MouseTextNoOverride(string.Empty, 0, 0, -1, -1, -1, -1, 0);
				}
			}

			// Token: 0x06004582 RID: 17794 RVA: 0x006C3C24 File Offset: 0x006C1E24
			private void UpdateSliderAndShowMultiplierMouseOver(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					int num = (int)(Main.windSpeedCurrent * 50f);
					string text = "";
					if (num < 0)
					{
						text += Language.GetTextValue("GameUI.EastWind", Math.Abs(num));
					}
					else if (num > 0)
					{
						text += Language.GetTextValue("GameUI.WestWind", num);
					}
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref text);
					Main.instance.MouseTextNoOverride(text, 0, 0, -1, -1, -1, -1, 0);
				}
			}
		}

		// Token: 0x020008A4 RID: 2212
		public class ModifyRainPower : CreativePowers.ASharedSliderPower
		{
			// Token: 0x06004583 RID: 17795 RVA: 0x006C3CA3 File Offset: 0x006C1EA3
			public ModifyRainPower()
			{
				this._powerNameKey = "CreativePowers.ModifyRainPower";
				this._iconLocation = CreativePowersHelper.CreativePowerIconLocations.RainStrength;
				this._syncToJoiningPlayers = false;
			}

			// Token: 0x06004584 RID: 17796 RVA: 0x006C3CC8 File Offset: 0x006C1EC8
			internal override void UpdateInfoFromSliderValueCache()
			{
				if (this._sliderCurrentValueCache == 0f)
				{
					Main.StopRain(true);
					return;
				}
				Main.StartRain(true, new float?(this._sliderCurrentValueCache), false);
			}

			// Token: 0x06004585 RID: 17797 RVA: 0x006C3CF0 File Offset: 0x006C1EF0
			internal override float GetSliderValueInner()
			{
				return Main.cloudAlpha;
			}

			// Token: 0x06004586 RID: 17798 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}

			// Token: 0x06004587 RID: 17799 RVA: 0x006C3CF8 File Offset: 0x006C1EF8
			public override UIElement ProvideSlider()
			{
				UIVerticalSlider uiverticalSlider = CreativePowersHelper.CreateSlider(new Func<float>(base.GetSliderValue), new Action<float>(base.SetValueKeyboard), new Action(base.SetValueGamepad));
				uiverticalSlider.OnUpdate += this.UpdateSliderAndShowMultiplierMouseOver;
				UIPanel uipanel = new UIPanel();
				uipanel.Width = new StyleDimension(132f, 0f);
				uipanel.Height = new StyleDimension(180f, 0f);
				uipanel.HAlign = 0f;
				uipanel.VAlign = 0.5f;
				uipanel.Append(uiverticalSlider);
				uipanel.OnUpdate += CreativePowersHelper.UpdateUseMouseInterface;
				UIText uitext = new UIText(Language.GetText("CreativePowers.WeatherMonsoon"), 1f, false)
				{
					HAlign = 1f,
					VAlign = 0f
				};
				uitext.OnUpdate += this.UpdateMouseOverNoItemText;
				uitext.OnMouseOut += this.Button_OnMouseOut;
				uitext.OnMouseOver += this.Button_OnMouseOver;
				uitext.OnLeftClick += this.topText_OnClick;
				uipanel.Append(uitext);
				UIText uitext2 = new UIText(Language.GetText("CreativePowers.WeatherClearSky"), 1f, false)
				{
					HAlign = 1f,
					VAlign = 1f
				};
				uitext2.OnUpdate += this.UpdateMouseOverNoItemText;
				uitext2.OnMouseOut += this.Button_OnMouseOut;
				uitext2.OnMouseOver += this.Button_OnMouseOver;
				uitext2.OnLeftClick += this.bottomText_OnClick;
				uipanel.Append(uitext2);
				UIText uitext3 = new UIText(Language.GetText("CreativePowers.WeatherDrizzle"), 1f, false)
				{
					HAlign = 1f,
					VAlign = 0.5f
				};
				uitext3.OnUpdate += this.UpdateMouseOverNoItemText;
				uitext3.OnMouseOut += this.Button_OnMouseOut;
				uitext3.OnMouseOver += this.Button_OnMouseOver;
				uitext3.OnLeftClick += this.middleText_OnClick;
				uipanel.Append(uitext3);
				return uipanel;
			}

			// Token: 0x06004588 RID: 17800 RVA: 0x006C3351 File Offset: 0x006C1551
			private void topText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboardForced(1f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x06004589 RID: 17801 RVA: 0x006C332F File Offset: 0x006C152F
			private void middleText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboardForced(0.5f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x0600458A RID: 17802 RVA: 0x006C330D File Offset: 0x006C150D
			private void bottomText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboardForced(0f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x0600458B RID: 17803 RVA: 0x006C3F10 File Offset: 0x006C2110
			private void Button_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
			{
				UIText uitext = listeningElement as UIText;
				if (uitext != null)
				{
					uitext.ShadowColor = Color.Black;
				}
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x0600458C RID: 17804 RVA: 0x006C3F48 File Offset: 0x006C2148
			private void Button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
			{
				UIText uitext = listeningElement as UIText;
				if (uitext != null)
				{
					uitext.ShadowColor = Main.OurFavoriteColor;
				}
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x0600458D RID: 17805 RVA: 0x006C3F80 File Offset: 0x006C2180
			private void UpdateMouseOverNoItemText(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					Main.instance.MouseTextNoOverride(string.Empty, 0, 0, -1, -1, -1, -1, 0);
				}
			}

			// Token: 0x0600458E RID: 17806 RVA: 0x006C3FAC File Offset: 0x006C21AC
			private void UpdateSliderAndShowMultiplierMouseOver(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string cursorText = Main.maxRaining.ToString("P0");
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref cursorText);
					Main.instance.MouseTextNoOverride(cursorText, 0, 0, -1, -1, -1, -1, 0);
				}
			}
		}

		// Token: 0x020008A5 RID: 2213
		public class FreezeTime : CreativePowers.ASharedTogglePower, IPersistentPerWorldContent
		{
			// Token: 0x0600458F RID: 17807 RVA: 0x006C3FEB File Offset: 0x006C21EB
			internal override void CustomizeButton(UIElement button)
			{
				button.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.FreezeTime));
			}

			// Token: 0x06004590 RID: 17808 RVA: 0x006C3FFD File Offset: 0x006C21FD
			internal override string GetButtonTextKey()
			{
				return "CreativePowers.FreezeTime";
			}

			// Token: 0x06004591 RID: 17809 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}

			// Token: 0x06004592 RID: 17810 RVA: 0x006C4004 File Offset: 0x006C2204
			public void Save(BinaryWriter writer)
			{
				writer.Write(base.Enabled);
			}

			// Token: 0x06004593 RID: 17811 RVA: 0x006C4014 File Offset: 0x006C2214
			public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				bool powerInfo = reader.ReadBoolean();
				base.SetPowerInfo(powerInfo);
			}

			// Token: 0x06004594 RID: 17812 RVA: 0x006C402F File Offset: 0x006C222F
			public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				reader.ReadBoolean();
			}
		}

		// Token: 0x020008A6 RID: 2214
		public class FreezeWindDirectionAndStrength : CreativePowers.ASharedTogglePower, IPersistentPerWorldContent
		{
			// Token: 0x06004596 RID: 17814 RVA: 0x006C4040 File Offset: 0x006C2240
			internal override void CustomizeButton(UIElement button)
			{
				button.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.WindFreeze));
			}

			// Token: 0x06004597 RID: 17815 RVA: 0x006C4052 File Offset: 0x006C2252
			internal override string GetButtonTextKey()
			{
				return "CreativePowers.FreezeWindDirectionAndStrength";
			}

			// Token: 0x06004598 RID: 17816 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}

			// Token: 0x06004599 RID: 17817 RVA: 0x006C4004 File Offset: 0x006C2204
			public void Save(BinaryWriter writer)
			{
				writer.Write(base.Enabled);
			}

			// Token: 0x0600459A RID: 17818 RVA: 0x006C405C File Offset: 0x006C225C
			public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				bool powerInfo = reader.ReadBoolean();
				base.SetPowerInfo(powerInfo);
			}

			// Token: 0x0600459B RID: 17819 RVA: 0x006C402F File Offset: 0x006C222F
			public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				reader.ReadBoolean();
			}
		}

		// Token: 0x020008A7 RID: 2215
		public class FreezeRainPower : CreativePowers.ASharedTogglePower, IPersistentPerWorldContent
		{
			// Token: 0x0600459D RID: 17821 RVA: 0x006C4077 File Offset: 0x006C2277
			internal override void CustomizeButton(UIElement button)
			{
				button.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.RainFreeze));
			}

			// Token: 0x0600459E RID: 17822 RVA: 0x006C4089 File Offset: 0x006C2289
			internal override string GetButtonTextKey()
			{
				return "CreativePowers.FreezeRainPower";
			}

			// Token: 0x0600459F RID: 17823 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}

			// Token: 0x060045A0 RID: 17824 RVA: 0x006C4004 File Offset: 0x006C2204
			public void Save(BinaryWriter writer)
			{
				writer.Write(base.Enabled);
			}

			// Token: 0x060045A1 RID: 17825 RVA: 0x006C4090 File Offset: 0x006C2290
			public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				bool powerInfo = reader.ReadBoolean();
				base.SetPowerInfo(powerInfo);
			}

			// Token: 0x060045A2 RID: 17826 RVA: 0x006C402F File Offset: 0x006C222F
			public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				reader.ReadBoolean();
			}
		}

		// Token: 0x020008A8 RID: 2216
		public class StopBiomeSpreadPower : CreativePowers.ASharedTogglePower, IPersistentPerWorldContent
		{
			// Token: 0x060045A4 RID: 17828 RVA: 0x006C40AB File Offset: 0x006C22AB
			internal override void CustomizeButton(UIElement button)
			{
				button.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.StopBiomeSpread));
			}

			// Token: 0x060045A5 RID: 17829 RVA: 0x006C40BD File Offset: 0x006C22BD
			internal override string GetButtonTextKey()
			{
				return "CreativePowers.StopBiomeSpread";
			}

			// Token: 0x060045A6 RID: 17830 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}

			// Token: 0x060045A7 RID: 17831 RVA: 0x006C4004 File Offset: 0x006C2204
			public void Save(BinaryWriter writer)
			{
				writer.Write(base.Enabled);
			}

			// Token: 0x060045A8 RID: 17832 RVA: 0x006C40C4 File Offset: 0x006C22C4
			public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				bool powerInfo = reader.ReadBoolean();
				base.SetPowerInfo(powerInfo);
			}

			// Token: 0x060045A9 RID: 17833 RVA: 0x006C402F File Offset: 0x006C222F
			public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				reader.ReadBoolean();
			}
		}

		// Token: 0x020008A9 RID: 2217
		public class SpawnRateSliderPerPlayerPower : CreativePowers.APerPlayerSliderPower, IPersistentPerPlayerContent
		{
			// Token: 0x1700055B RID: 1371
			// (get) Token: 0x060045AB RID: 17835 RVA: 0x006C40DF File Offset: 0x006C22DF
			// (set) Token: 0x060045AC RID: 17836 RVA: 0x006C40E7 File Offset: 0x006C22E7
			public float StrengthMultiplierToGiveNPCs { get; private set; }

			// Token: 0x060045AD RID: 17837 RVA: 0x006C40F0 File Offset: 0x006C22F0
			public SpawnRateSliderPerPlayerPower()
			{
				this._powerNameKey = "CreativePowers.NPCSpawnRateSlider";
				this._sliderDefaultValue = 0.5f;
				this._iconLocation = CreativePowersHelper.CreativePowerIconLocations.EnemySpawnRate;
			}

			// Token: 0x060045AE RID: 17838 RVA: 0x006C4119 File Offset: 0x006C2319
			public bool GetShouldDisableSpawnsFor(int playerIndex)
			{
				if (!this._cachePerPlayer.IndexInRange(playerIndex))
				{
					return false;
				}
				if (playerIndex == Main.myPlayer)
				{
					return this._sliderCurrentValueCache == 0f;
				}
				return this._cachePerPlayer[playerIndex] == 0f;
			}

			// Token: 0x060045AF RID: 17839 RVA: 0x00009E06 File Offset: 0x00008006
			internal override void UpdateInfoFromSliderValueCache()
			{
			}

			// Token: 0x060045B0 RID: 17840 RVA: 0x006C4150 File Offset: 0x006C2350
			public override float RemapSliderValueToPowerValue(float sliderValue)
			{
				if (sliderValue < 0.5f)
				{
					return Utils.Remap(sliderValue, 0f, 0.5f, 0.1f, 1f, true);
				}
				return Utils.Remap(sliderValue, 0.5f, 1f, 1f, 10f, true);
			}

			// Token: 0x060045B1 RID: 17841 RVA: 0x006C419C File Offset: 0x006C239C
			public override UIElement ProvideSlider()
			{
				UIVerticalSlider uiverticalSlider = CreativePowersHelper.CreateSlider(new Func<float>(base.GetSliderValue), new Action<float>(base.SetValueKeyboard), new Action(base.SetValueGamepad));
				uiverticalSlider.OnUpdate += this.UpdateSliderAndShowMultiplierMouseOver;
				UIPanel uipanel = new UIPanel();
				uipanel.Width = new StyleDimension(77f, 0f);
				uipanel.Height = new StyleDimension(180f, 0f);
				uipanel.HAlign = 0f;
				uipanel.VAlign = 0.5f;
				uipanel.Append(uiverticalSlider);
				uipanel.OnUpdate += CreativePowersHelper.UpdateUseMouseInterface;
				UIText uitext = new UIText("x10", 1f, false)
				{
					HAlign = 1f,
					VAlign = 0f
				};
				uitext.OnUpdate += this.UpdateMouseOverNoItemText;
				uitext.OnMouseOut += this.Button_OnMouseOut;
				uitext.OnMouseOver += this.Button_OnMouseOver;
				uitext.OnLeftClick += this.topText_OnClick;
				uipanel.Append(uitext);
				UIText uitext2 = new UIText("x1", 1f, false)
				{
					HAlign = 1f,
					VAlign = 0.5f
				};
				uitext2.OnUpdate += this.UpdateMouseOverNoItemText;
				uitext2.OnMouseOut += this.Button_OnMouseOut;
				uitext2.OnMouseOver += this.Button_OnMouseOver;
				uitext2.OnLeftClick += this.middleText_OnClick;
				uipanel.Append(uitext2);
				UIText uitext3 = new UIText("x0", 1f, false)
				{
					HAlign = 1f,
					VAlign = 1f
				};
				uitext3.OnUpdate += this.UpdateMouseOverNoItemText;
				uitext3.OnMouseOut += this.Button_OnMouseOut;
				uitext3.OnMouseOver += this.Button_OnMouseOver;
				uitext3.OnLeftClick += this.bottomText_OnClick;
				uipanel.Append(uitext3);
				return uipanel;
			}

			// Token: 0x060045B2 RID: 17842 RVA: 0x006C43A8 File Offset: 0x006C25A8
			private void Button_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
			{
				UIText uitext = listeningElement as UIText;
				if (uitext != null)
				{
					uitext.ShadowColor = Color.Black;
				}
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x060045B3 RID: 17843 RVA: 0x006C43E0 File Offset: 0x006C25E0
			private void Button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
			{
				UIText uitext = listeningElement as UIText;
				if (uitext != null)
				{
					uitext.ShadowColor = Main.OurFavoriteColor;
				}
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x060045B4 RID: 17844 RVA: 0x006C4417 File Offset: 0x006C2617
			private void topText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboard(1f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x060045B5 RID: 17845 RVA: 0x006C4439 File Offset: 0x006C2639
			private void middleText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboard(0.5f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x060045B6 RID: 17846 RVA: 0x006C445B File Offset: 0x006C265B
			private void bottomText_OnClick(UIMouseEvent evt, UIElement listeningElement)
			{
				base.SetValueKeyboard(0f);
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}

			// Token: 0x060045B7 RID: 17847 RVA: 0x006C4480 File Offset: 0x006C2680
			private void UpdateMouseOverNoItemText(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					Main.instance.MouseTextNoOverride(string.Empty, 0, 0, -1, -1, -1, -1, 0);
				}
			}

			// Token: 0x060045B8 RID: 17848 RVA: 0x006C44AC File Offset: 0x006C26AC
			private void UpdateSliderAndShowMultiplierMouseOver(UIElement affectedElement)
			{
				if (affectedElement.IsMouseHovering)
				{
					string cursorText = "x" + this.RemapSliderValueToPowerValue(base.GetSliderValue()).ToString("F2");
					if (this.GetShouldDisableSpawnsFor(Main.myPlayer))
					{
						cursorText = Language.GetTextValue(this._powerNameKey + "EnemySpawnsDisabled");
					}
					CreativePowersHelper.AddPermissionTextIfNeeded(this, ref cursorText);
					Main.instance.MouseTextNoOverride(cursorText, 0, 0, -1, -1, -1, -1, 0);
				}
			}

			// Token: 0x060045B9 RID: 17849 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool GetIsUnlocked()
			{
				return true;
			}

			// Token: 0x060045BA RID: 17850 RVA: 0x006C4524 File Offset: 0x006C2724
			public void Save(Player player, BinaryWriter writer)
			{
				float sliderCurrentValueCache = this._sliderCurrentValueCache;
				writer.Write(sliderCurrentValueCache);
			}

			// Token: 0x060045BB RID: 17851 RVA: 0x006C453F File Offset: 0x006C273F
			public void ResetDataForNewPlayer(Player player)
			{
				player.savedPerPlayerFieldsThatArentInThePlayerClass.spawnRatePowerSliderValue = this._sliderDefaultValue;
			}

			// Token: 0x060045BC RID: 17852 RVA: 0x006C4554 File Offset: 0x006C2754
			public void Load(Player player, BinaryReader reader, int gameVersionSaveWasMadeOn)
			{
				float spawnRatePowerSliderValue = reader.ReadSingle();
				player.savedPerPlayerFieldsThatArentInThePlayerClass.spawnRatePowerSliderValue = spawnRatePowerSliderValue;
			}

			// Token: 0x060045BD RID: 17853 RVA: 0x006C4574 File Offset: 0x006C2774
			public void ApplyLoadedDataToOutOfPlayerFields(Player player)
			{
				base.PushChangeAndSetSlider(player.savedPerPlayerFieldsThatArentInThePlayerClass.spawnRatePowerSliderValue);
			}
		}
	}
}
