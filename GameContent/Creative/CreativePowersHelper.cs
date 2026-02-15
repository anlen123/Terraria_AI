using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Creative
{
	// Token: 0x0200031E RID: 798
	public class CreativePowersHelper
	{
		// Token: 0x06002789 RID: 10121 RVA: 0x00567BDA File Offset: 0x00565DDA
		private static Asset<Texture2D> GetPowerIconAsset(string path)
		{
			return Main.Assets.Request<Texture2D>(path, 1);
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x00567BE8 File Offset: 0x00565DE8
		public static UIImageFramed GetIconImage(Point iconLocation)
		{
			Asset<Texture2D> powerIconAsset = CreativePowersHelper.GetPowerIconAsset("Images/UI/Creative/Infinite_Powers");
			return new UIImageFramed(powerIconAsset, powerIconAsset.Frame(21, 1, iconLocation.X, iconLocation.Y, 0, 0))
			{
				MarginLeft = 4f,
				MarginTop = 4f,
				VAlign = 0.5f,
				HAlign = 1f,
				IgnoresMouseInteraction = true
			};
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x00567C50 File Offset: 0x00565E50
		public static GroupOptionButton<bool> CreateToggleButton(CreativePowerUIElementRequestInfo info)
		{
			GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(true, null, null, Color.White, null, 0.8f, 0.5f, 10f);
			groupOptionButton.Width = new StyleDimension((float)info.PreferredButtonWidth, 0f);
			groupOptionButton.Height = new StyleDimension((float)info.PreferredButtonHeight, 0f);
			groupOptionButton.ShowHighlightWhenSelected = false;
			groupOptionButton.SetCurrentOption(false);
			groupOptionButton.SetColorsBasedOnSelectionState(new Color(152, 175, 235), Colors.InventoryDefaultColor, 1f, 0.7f);
			groupOptionButton.SetColorsBasedOnSelectionState(Main.OurFavoriteColor, Colors.InventoryDefaultColor, 1f, 0.7f);
			return groupOptionButton;
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x00567CFC File Offset: 0x00565EFC
		public static GroupOptionButton<bool> CreateSimpleButton(CreativePowerUIElementRequestInfo info)
		{
			GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(true, null, null, Color.White, null, 0.8f, 0.5f, 10f);
			groupOptionButton.Width = new StyleDimension((float)info.PreferredButtonWidth, 0f);
			groupOptionButton.Height = new StyleDimension((float)info.PreferredButtonHeight, 0f);
			groupOptionButton.ShowHighlightWhenSelected = false;
			groupOptionButton.SetCurrentOption(false);
			groupOptionButton.SetColorsBasedOnSelectionState(new Color(152, 175, 235), Colors.InventoryDefaultColor, 1f, 0.7f);
			return groupOptionButton;
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x00567D8C File Offset: 0x00565F8C
		public static GroupOptionButton<T> CreateCategoryButton<T>(CreativePowerUIElementRequestInfo info, T option, T currentOption) where T : IConvertible, IEquatable<T>
		{
			GroupOptionButton<T> groupOptionButton = new GroupOptionButton<T>(option, null, null, Color.White, null, 0.8f, 0.5f, 10f);
			groupOptionButton.Width = new StyleDimension((float)info.PreferredButtonWidth, 0f);
			groupOptionButton.Height = new StyleDimension((float)info.PreferredButtonHeight, 0f);
			groupOptionButton.ShowHighlightWhenSelected = false;
			groupOptionButton.SetCurrentOption(currentOption);
			groupOptionButton.SetColorsBasedOnSelectionState(new Color(152, 175, 235), Colors.InventoryDefaultColor, 1f, 0.7f);
			return groupOptionButton;
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x00567E1C File Offset: 0x0056601C
		public static void AddPermissionTextIfNeeded(ICreativePower power, ref string originalText)
		{
			if (!CreativePowersHelper.IsAvailableForPlayer(power, Main.myPlayer))
			{
				string textValue = Language.GetTextValue("CreativePowers.CantUsePowerBecauseOfNoPermissionFromServer");
				originalText = originalText + "\n" + textValue;
			}
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x00567E50 File Offset: 0x00566050
		public static void AddDescriptionIfNeeded(ref string originalText, string descriptionKey)
		{
			if (!CreativePowerSettings.ShouldPowersBeElaborated)
			{
				return;
			}
			string textValue = Language.GetTextValue(descriptionKey);
			originalText = originalText + "\n" + textValue;
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x00567E7C File Offset: 0x0056607C
		public static void AddUnlockTextIfNeeded(ref string originalText, bool needed, string descriptionKey)
		{
			if (needed)
			{
				return;
			}
			string textValue = Language.GetTextValue(descriptionKey);
			originalText = originalText + "\n" + textValue;
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x00567EA4 File Offset: 0x005660A4
		public static UIVerticalSlider CreateSlider(Func<float> GetSliderValueMethod, Action<float> SetValueKeyboardMethod, Action SetValueGamepadMethod)
		{
			return new UIVerticalSlider(GetSliderValueMethod, SetValueKeyboardMethod, SetValueGamepadMethod, Color.Red)
			{
				Width = new StyleDimension(12f, 0f),
				Height = new StyleDimension(-10f, 1f),
				Left = new StyleDimension(6f, 0f),
				HAlign = 0f,
				VAlign = 0.5f,
				EmptyColor = Color.OrangeRed,
				FilledColor = Color.CornflowerBlue
			};
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x00567F29 File Offset: 0x00566129
		public static void UpdateUseMouseInterface(UIElement affectedElement)
		{
			if (affectedElement.IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x00567F40 File Offset: 0x00566140
		public static void UpdateUnlockStateByPower(ICreativePower power, UIElement button, Color colorWhenSelected)
		{
			IGroupOptionButton asButton = button as IGroupOptionButton;
			if (asButton == null)
			{
				return;
			}
			button.OnUpdate += delegate(UIElement element)
			{
				CreativePowersHelper.UpdateUnlockStateByPowerInternal(power, colorWhenSelected, asButton);
			};
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x00567F88 File Offset: 0x00566188
		public static bool IsAvailableForPlayer(ICreativePower power, int playerIndex)
		{
			switch (power.CurrentPermissionLevel)
			{
			default:
				return false;
			case PowerPermissionLevel.CanBeChangedByHostAlone:
				return Main.netMode == 0 || Main.countsAsHostForGameplay[playerIndex];
			case PowerPermissionLevel.CanBeChangedByEveryone:
				return true;
			}
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x00567FC4 File Offset: 0x005661C4
		private static void UpdateUnlockStateByPowerInternal(ICreativePower power, Color colorWhenSelected, IGroupOptionButton asButton)
		{
			bool isUnlocked = power.GetIsUnlocked();
			bool flag = !CreativePowersHelper.IsAvailableForPlayer(power, Main.myPlayer);
			asButton.SetBorderColor(flag ? Color.DimGray : Color.White);
			if (flag)
			{
				asButton.SetColorsBasedOnSelectionState(new Color(60, 60, 60), new Color(60, 60, 60), 0.7f, 0.7f);
				return;
			}
			if (isUnlocked)
			{
				asButton.SetColorsBasedOnSelectionState(colorWhenSelected, Colors.InventoryDefaultColor, 1f, 0.7f);
				return;
			}
			asButton.SetColorsBasedOnSelectionState(Color.Crimson, Color.Red, 0.7f, 0.7f);
		}

		// Token: 0x040050CC RID: 20684
		public const int TextureIconColumns = 21;

		// Token: 0x040050CD RID: 20685
		public const int TextureIconRows = 1;

		// Token: 0x040050CE RID: 20686
		public static Color CommonSelectedColor = new Color(152, 175, 235);

		// Token: 0x0200087C RID: 2172
		public class CreativePowerIconLocations
		{
			// Token: 0x04007255 RID: 29269
			public static readonly Point Unassigned = new Point(0, 0);

			// Token: 0x04007256 RID: 29270
			public static readonly Point Deprecated = new Point(0, 0);

			// Token: 0x04007257 RID: 29271
			public static readonly Point ItemDuplication = new Point(0, 0);

			// Token: 0x04007258 RID: 29272
			public static readonly Point ItemResearch = new Point(1, 0);

			// Token: 0x04007259 RID: 29273
			public static readonly Point TimeCategory = new Point(2, 0);

			// Token: 0x0400725A RID: 29274
			public static readonly Point WeatherCategory = new Point(3, 0);

			// Token: 0x0400725B RID: 29275
			public static readonly Point EnemyStrengthSlider = new Point(4, 0);

			// Token: 0x0400725C RID: 29276
			public static readonly Point GameEvents = new Point(5, 0);

			// Token: 0x0400725D RID: 29277
			public static readonly Point Godmode = new Point(6, 0);

			// Token: 0x0400725E RID: 29278
			public static readonly Point BlockPlacementRange = new Point(7, 0);

			// Token: 0x0400725F RID: 29279
			public static readonly Point StopBiomeSpread = new Point(8, 0);

			// Token: 0x04007260 RID: 29280
			public static readonly Point EnemySpawnRate = new Point(9, 0);

			// Token: 0x04007261 RID: 29281
			public static readonly Point FreezeTime = new Point(10, 0);

			// Token: 0x04007262 RID: 29282
			public static readonly Point TimeDawn = new Point(11, 0);

			// Token: 0x04007263 RID: 29283
			public static readonly Point TimeNoon = new Point(12, 0);

			// Token: 0x04007264 RID: 29284
			public static readonly Point TimeDusk = new Point(13, 0);

			// Token: 0x04007265 RID: 29285
			public static readonly Point TimeMidnight = new Point(14, 0);

			// Token: 0x04007266 RID: 29286
			public static readonly Point WindDirection = new Point(15, 0);

			// Token: 0x04007267 RID: 29287
			public static readonly Point WindFreeze = new Point(16, 0);

			// Token: 0x04007268 RID: 29288
			public static readonly Point RainStrength = new Point(17, 0);

			// Token: 0x04007269 RID: 29289
			public static readonly Point RainFreeze = new Point(18, 0);

			// Token: 0x0400726A RID: 29290
			public static readonly Point ModifyTime = new Point(19, 0);

			// Token: 0x0400726B RID: 29291
			public static readonly Point PersonalCategory = new Point(20, 0);
		}
	}
}
