using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Achievements;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameInput;

namespace Terraria.UI
{
	// Token: 0x020000E4 RID: 228
	public class AchievementAdvisor
	{
		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060018C2 RID: 6338 RVA: 0x004E43CC File Offset: 0x004E25CC
		public bool CanDrawAboveCoins
		{
			get
			{
				return Main.screenWidth >= 1000 && !PlayerInput.UsingGamepad && !PlayerInput.SteamDeckIsUsed;
			}
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x004E4400 File Offset: 0x004E2600
		public void LoadContent()
		{
			this._achievementsTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievements", 1);
			this._achievementsBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", 1);
			this._achievementsBorderMouseHoverFatTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders_MouseHover", 1);
			this._achievementsBorderMouseHoverThinTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders_MouseHoverThin", 1);
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x00009E06 File Offset: 0x00008006
		public void Draw(SpriteBatch spriteBatch)
		{
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x004E4468 File Offset: 0x004E2668
		public void DrawOneAchievement(SpriteBatch spriteBatch, Vector2 position, bool large)
		{
			List<AchievementAdvisorCard> bestCards = this.GetBestCards(1);
			if (bestCards.Count < 1)
			{
				return;
			}
			AchievementAdvisorCard hoveredCard = bestCards[0];
			float num = 0.35f;
			if (large)
			{
				num = 0.75f;
			}
			this._hoveredCard = null;
			bool flag;
			this.DrawCard(bestCards[0], spriteBatch, position + new Vector2(8f) * num, num, out flag);
			if (flag)
			{
				this._hoveredCard = hoveredCard;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Main.ingameOptionsWindow = false;
					IngameFancyUI.OpenAchievementsAndGoto(this._hoveredCard.achievement);
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				}
			}
			Main.DoStatefulTickSound(ref Main.achievementAdvisorMouseOver, flag);
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x004E451A File Offset: 0x004E271A
		public void Update()
		{
			this._hoveredCard = null;
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x004E4524 File Offset: 0x004E2724
		public void DrawOptionsPanel(SpriteBatch spriteBatch, Vector2 leftPosition, Vector2 rightPosition)
		{
			List<AchievementAdvisorCard> bestCards = this.GetBestCards(10);
			this._hoveredCard = null;
			int num = bestCards.Count;
			if (num > 5)
			{
				num = 5;
			}
			for (int i = 0; i < num; i++)
			{
				bool flag;
				this.DrawCard(bestCards[i], spriteBatch, leftPosition + new Vector2((float)(42 * i), 0f), 0.5f, out flag);
				if (flag)
				{
					this._hoveredCard = bestCards[i];
				}
			}
			for (int j = 5; j < bestCards.Count; j++)
			{
				bool flag;
				this.DrawCard(bestCards[j], spriteBatch, rightPosition + new Vector2((float)(42 * j), 0f), 0.5f, out flag);
				if (flag)
				{
					this._hoveredCard = bestCards[j];
				}
			}
			if (this._hoveredCard != null)
			{
				if (this._hoveredCard.achievement.IsCompleted)
				{
					this._hoveredCard = null;
					return;
				}
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Main.ingameOptionsWindow = false;
					IngameFancyUI.OpenAchievementsAndGoto(this._hoveredCard.achievement);
				}
			}
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x004E462C File Offset: 0x004E282C
		public void DrawMouseHover()
		{
			if (this._hoveredCard == null)
			{
				return;
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
			PlayerInput.SetZoom_UI();
			Item item = Main.DisplayAndGetFakeItem(ItemRarityColor.StrongRed10);
			item.SetNameOverride(this._hoveredCard.achievement.FriendlyName.Value);
			item.ToolTip = ItemTooltip.FromLanguageKey(this._hoveredCard.achievement.Description.Key);
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x004E46A8 File Offset: 0x004E28A8
		private void DrawCard(AchievementAdvisorCard card, SpriteBatch spriteBatch, Vector2 position, float scale, out bool hovered)
		{
			hovered = false;
			if (Main.MouseScreen.Between(position, position + card.frame.Size() * scale) && !PlayerInput.IgnoreMouseInterface)
			{
				Main.LocalPlayer.mouseInterface = true;
				hovered = true;
			}
			Color white = Color.White;
			if (!hovered)
			{
				white = new Color(220, 220, 220, 220);
			}
			Vector2 value = new Vector2(-4f) * scale;
			Vector2 value2 = new Vector2(-8f) * scale;
			Texture2D value3 = this._achievementsBorderMouseHoverFatTexture.Value;
			if (scale > 0.5f)
			{
				value3 = this._achievementsBorderMouseHoverThinTexture.Value;
				value2 = new Vector2(-5f) * scale;
			}
			Rectangle frame = card.frame;
			frame.X += 528;
			spriteBatch.Draw(this._achievementsTexture.Value, position, new Rectangle?(frame), white, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(this._achievementsBorderTexture.Value, position + value, null, white, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			if (hovered)
			{
				spriteBatch.Draw(value3, position + value2, null, Main.OurFavoriteColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			}
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x004E4820 File Offset: 0x004E2A20
		private List<AchievementAdvisorCard> GetBestCards(int cardsAmount = 10)
		{
			List<AchievementAdvisorCard> list = new List<AchievementAdvisorCard>();
			for (int i = 0; i < this._cards.Count; i++)
			{
				AchievementAdvisorCard achievementAdvisorCard = this._cards[i];
				if (!achievementAdvisorCard.achievement.IsCompleted && achievementAdvisorCard.IsAchievableInWorld())
				{
					list.Add(achievementAdvisorCard);
					if (list.Count >= cardsAmount)
					{
						break;
					}
				}
			}
			return list;
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x004E487C File Offset: 0x004E2A7C
		public void Initialize()
		{
			float num = 1f;
			List<AchievementAdvisorCard> cards = this._cards;
			Achievement achievement = Main.Achievements.GetAchievement("TIMBER");
			float num2 = num;
			num = num2 + 1f;
			cards.Add(new AchievementAdvisorCard(achievement, num2));
			List<AchievementAdvisorCard> cards2 = this._cards;
			Achievement achievement2 = Main.Achievements.GetAchievement("BENCHED");
			float num3 = num;
			num = num3 + 1f;
			cards2.Add(new AchievementAdvisorCard(achievement2, num3));
			List<AchievementAdvisorCard> cards3 = this._cards;
			Achievement achievement3 = Main.Achievements.GetAchievement("OBTAIN_HAMMER");
			float num4 = num;
			num = num4 + 1f;
			cards3.Add(new AchievementAdvisorCard(achievement3, num4));
			List<AchievementAdvisorCard> cards4 = this._cards;
			Achievement achievement4 = Main.Achievements.GetAchievement("NO_HOBO");
			float num5 = num;
			num = num5 + 1f;
			cards4.Add(new AchievementAdvisorCard(achievement4, num5));
			List<AchievementAdvisorCard> cards5 = this._cards;
			Achievement achievement5 = Main.Achievements.GetAchievement("YOU_CAN_DO_IT");
			float num6 = num;
			num = num6 + 1f;
			cards5.Add(new AchievementAdvisorCard(achievement5, num6));
			List<AchievementAdvisorCard> cards6 = this._cards;
			Achievement achievement6 = Main.Achievements.GetAchievement("OOO_SHINY");
			float num7 = num;
			num = num7 + 1f;
			cards6.Add(new AchievementAdvisorCard(achievement6, num7));
			List<AchievementAdvisorCard> cards7 = this._cards;
			Achievement achievement7 = Main.Achievements.GetAchievement("HEAVY_METAL");
			float num8 = num;
			num = num8 + 1f;
			cards7.Add(new AchievementAdvisorCard(achievement7, num8));
			List<AchievementAdvisorCard> cards8 = this._cards;
			Achievement achievement8 = Main.Achievements.GetAchievement("MATCHING_ATTIRE");
			float num9 = num;
			num = num9 + 1f;
			cards8.Add(new AchievementAdvisorCard(achievement8, num9));
			List<AchievementAdvisorCard> cards9 = this._cards;
			Achievement achievement9 = Main.Achievements.GetAchievement("HEART_BREAKER");
			float num10 = num;
			num = num10 + 1f;
			cards9.Add(new AchievementAdvisorCard(achievement9, num10));
			List<AchievementAdvisorCard> cards10 = this._cards;
			Achievement achievement10 = Main.Achievements.GetAchievement("I_AM_LOOT");
			float num11 = num;
			num = num11 + 1f;
			cards10.Add(new AchievementAdvisorCard(achievement10, num11));
			List<AchievementAdvisorCard> cards11 = this._cards;
			Achievement achievement11 = Main.Achievements.GetAchievement("HOLD_ON_TIGHT");
			float num12 = num;
			num = num12 + 1f;
			cards11.Add(new AchievementAdvisorCard(achievement11, num12));
			List<AchievementAdvisorCard> cards12 = this._cards;
			Achievement achievement12 = Main.Achievements.GetAchievement("STAR_POWER");
			float num13 = num;
			num = num13 + 1f;
			cards12.Add(new AchievementAdvisorCard(achievement12, num13));
			List<AchievementAdvisorCard> cards13 = this._cards;
			Achievement achievement13 = Main.Achievements.GetAchievement("EYE_ON_YOU");
			float num14 = num;
			num = num14 + 1f;
			cards13.Add(new AchievementAdvisorCard(achievement13, num14));
			List<AchievementAdvisorCard> cards14 = this._cards;
			Achievement achievement14 = Main.Achievements.GetAchievement("SMASHING_POPPET");
			float num15 = num;
			num = num15 + 1f;
			cards14.Add(new AchievementAdvisorCard(achievement14, num15));
			List<AchievementAdvisorCard> cards15 = this._cards;
			Achievement achievement15 = Main.Achievements.GetAchievement("WHERES_MY_HONEY");
			float num16 = num;
			num = num16 + 1f;
			cards15.Add(new AchievementAdvisorCard(achievement15, num16));
			List<AchievementAdvisorCard> cards16 = this._cards;
			Achievement achievement16 = Main.Achievements.GetAchievement("STING_OPERATION");
			float num17 = num;
			num = num17 + 1f;
			cards16.Add(new AchievementAdvisorCard(achievement16, num17));
			List<AchievementAdvisorCard> cards17 = this._cards;
			Achievement achievement17 = Main.Achievements.GetAchievement("BONED");
			float num18 = num;
			num = num18 + 1f;
			cards17.Add(new AchievementAdvisorCard(achievement17, num18));
			List<AchievementAdvisorCard> cards18 = this._cards;
			Achievement achievement18 = Main.Achievements.GetAchievement("DUNGEON_HEIST");
			float num19 = num;
			num = num19 + 1f;
			cards18.Add(new AchievementAdvisorCard(achievement18, num19));
			List<AchievementAdvisorCard> cards19 = this._cards;
			Achievement achievement19 = Main.Achievements.GetAchievement("ITS_GETTING_HOT_IN_HERE");
			float num20 = num;
			num = num20 + 1f;
			cards19.Add(new AchievementAdvisorCard(achievement19, num20));
			List<AchievementAdvisorCard> cards20 = this._cards;
			Achievement achievement20 = Main.Achievements.GetAchievement("MINER_FOR_FIRE");
			float num21 = num;
			num = num21 + 1f;
			cards20.Add(new AchievementAdvisorCard(achievement20, num21));
			List<AchievementAdvisorCard> cards21 = this._cards;
			Achievement achievement21 = Main.Achievements.GetAchievement("STILL_HUNGRY");
			float num22 = num;
			num = num22 + 1f;
			cards21.Add(new AchievementAdvisorCard(achievement21, num22));
			List<AchievementAdvisorCard> cards22 = this._cards;
			Achievement achievement22 = Main.Achievements.GetAchievement("ITS_HARD");
			float num23 = num;
			num = num23 + 1f;
			cards22.Add(new AchievementAdvisorCard(achievement22, num23));
			List<AchievementAdvisorCard> cards23 = this._cards;
			Achievement achievement23 = Main.Achievements.GetAchievement("BEGONE_EVIL");
			float num24 = num;
			num = num24 + 1f;
			cards23.Add(new AchievementAdvisorCard(achievement23, num24));
			List<AchievementAdvisorCard> cards24 = this._cards;
			Achievement achievement24 = Main.Achievements.GetAchievement("EXTRA_SHINY");
			float num25 = num;
			num = num25 + 1f;
			cards24.Add(new AchievementAdvisorCard(achievement24, num25));
			List<AchievementAdvisorCard> cards25 = this._cards;
			Achievement achievement25 = Main.Achievements.GetAchievement("HEAD_IN_THE_CLOUDS");
			float num26 = num;
			num = num26 + 1f;
			cards25.Add(new AchievementAdvisorCard(achievement25, num26));
			List<AchievementAdvisorCard> cards26 = this._cards;
			Achievement achievement26 = Main.Achievements.GetAchievement("BUCKETS_OF_BOLTS");
			float num27 = num;
			num = num27 + 1f;
			cards26.Add(new AchievementAdvisorCard(achievement26, num27));
			List<AchievementAdvisorCard> cards27 = this._cards;
			Achievement achievement27 = Main.Achievements.GetAchievement("DRAX_ATTAX");
			float num28 = num;
			num = num28 + 1f;
			cards27.Add(new AchievementAdvisorCard(achievement27, num28));
			List<AchievementAdvisorCard> cards28 = this._cards;
			Achievement achievement28 = Main.Achievements.GetAchievement("PHOTOSYNTHESIS");
			float num29 = num;
			num = num29 + 1f;
			cards28.Add(new AchievementAdvisorCard(achievement28, num29));
			List<AchievementAdvisorCard> cards29 = this._cards;
			Achievement achievement29 = Main.Achievements.GetAchievement("GET_A_LIFE");
			float num30 = num;
			num = num30 + 1f;
			cards29.Add(new AchievementAdvisorCard(achievement29, num30));
			List<AchievementAdvisorCard> cards30 = this._cards;
			Achievement achievement30 = Main.Achievements.GetAchievement("THE_GREAT_SOUTHERN_PLANTKILL");
			float num31 = num;
			num = num31 + 1f;
			cards30.Add(new AchievementAdvisorCard(achievement30, num31));
			List<AchievementAdvisorCard> cards31 = this._cards;
			Achievement achievement31 = Main.Achievements.GetAchievement("TEMPLE_RAIDER");
			float num32 = num;
			num = num32 + 1f;
			cards31.Add(new AchievementAdvisorCard(achievement31, num32));
			List<AchievementAdvisorCard> cards32 = this._cards;
			Achievement achievement32 = Main.Achievements.GetAchievement("LIHZAHRDIAN_IDOL");
			float num33 = num;
			num = num33 + 1f;
			cards32.Add(new AchievementAdvisorCard(achievement32, num33));
			List<AchievementAdvisorCard> cards33 = this._cards;
			Achievement achievement33 = Main.Achievements.GetAchievement("ROBBING_THE_GRAVE");
			float num34 = num;
			num = num34 + 1f;
			cards33.Add(new AchievementAdvisorCard(achievement33, num34));
			List<AchievementAdvisorCard> cards34 = this._cards;
			Achievement achievement34 = Main.Achievements.GetAchievement("OBSESSIVE_DEVOTION");
			float num35 = num;
			num = num35 + 1f;
			cards34.Add(new AchievementAdvisorCard(achievement34, num35));
			List<AchievementAdvisorCard> cards35 = this._cards;
			Achievement achievement35 = Main.Achievements.GetAchievement("STAR_DESTROYER");
			float num36 = num;
			num = num36 + 1f;
			cards35.Add(new AchievementAdvisorCard(achievement35, num36));
			List<AchievementAdvisorCard> cards36 = this._cards;
			Achievement achievement36 = Main.Achievements.GetAchievement("CHAMPION_OF_TERRARIA");
			float num37 = num;
			num = num37 + 1f;
			cards36.Add(new AchievementAdvisorCard(achievement36, num37));
			from x in this._cards
			orderby x.order
			select x;
		}

		// Token: 0x040012E5 RID: 4837
		private List<AchievementAdvisorCard> _cards = new List<AchievementAdvisorCard>();

		// Token: 0x040012E6 RID: 4838
		private Asset<Texture2D> _achievementsTexture;

		// Token: 0x040012E7 RID: 4839
		private Asset<Texture2D> _achievementsBorderTexture;

		// Token: 0x040012E8 RID: 4840
		private Asset<Texture2D> _achievementsBorderMouseHoverFatTexture;

		// Token: 0x040012E9 RID: 4841
		private Asset<Texture2D> _achievementsBorderMouseHoverThinTexture;

		// Token: 0x040012EA RID: 4842
		private AchievementAdvisorCard _hoveredCard;
	}
}
