using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria
{
	// Token: 0x0200002E RID: 46
	public static class Minecart
	{
		// Token: 0x06000221 RID: 545 RVA: 0x0002C264 File Offset: 0x0002A464
		public static void Initialize()
		{
			if (!Main.dedServ && (float)TextureAssets.MinecartMount.Width() != Minecart.Customization.Default.MinecartTextureWidth)
			{
				throw new Exception("Be sure to update Minecart.textureWidth to match the actual texture size of " + Minecart.Customization.Default.MinecartTextureWidth + ".");
			}
			Minecart._rightSideConnection = new int[36];
			Minecart._leftSideConnection = new int[36];
			Minecart._trackType = new int[36];
			Minecart._boostLeft = new bool[36];
			Minecart._texturePosition = new Vector2[40];
			Minecart._tileHeight = new int[36][];
			for (int i = 0; i < 36; i++)
			{
				int[] array = new int[8];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = 5;
				}
				Minecart._tileHeight[i] = array;
			}
			int num = 0;
			Minecart._leftSideConnection[num] = -1;
			Minecart._rightSideConnection[num] = -1;
			Minecart._tileHeight[num][0] = -4;
			Minecart._tileHeight[num][7] = -4;
			Minecart._texturePosition[num] = new Vector2(0f, 0f);
			num++;
			Minecart._leftSideConnection[num] = 1;
			Minecart._rightSideConnection[num] = 1;
			Minecart._texturePosition[num] = new Vector2(1f, 0f);
			num++;
			Minecart._leftSideConnection[num] = -1;
			Minecart._rightSideConnection[num] = 1;
			for (int k = 0; k < 4; k++)
			{
				Minecart._tileHeight[num][k] = -1;
			}
			Minecart._texturePosition[num] = new Vector2(2f, 1f);
			num++;
			Minecart._leftSideConnection[num] = 1;
			Minecart._rightSideConnection[num] = -1;
			for (int l = 4; l < 8; l++)
			{
				Minecart._tileHeight[num][l] = -1;
			}
			Minecart._texturePosition[num] = new Vector2(3f, 1f);
			num++;
			Minecart._leftSideConnection[num] = 2;
			Minecart._rightSideConnection[num] = 1;
			Minecart._tileHeight[num][0] = 1;
			Minecart._tileHeight[num][1] = 2;
			Minecart._tileHeight[num][2] = 3;
			Minecart._tileHeight[num][3] = 3;
			Minecart._tileHeight[num][4] = 4;
			Minecart._tileHeight[num][5] = 4;
			Minecart._texturePosition[num] = new Vector2(0f, 2f);
			num++;
			Minecart._leftSideConnection[num] = 1;
			Minecart._rightSideConnection[num] = 2;
			Minecart._tileHeight[num][2] = 4;
			Minecart._tileHeight[num][3] = 4;
			Minecart._tileHeight[num][4] = 3;
			Minecart._tileHeight[num][5] = 3;
			Minecart._tileHeight[num][6] = 2;
			Minecart._tileHeight[num][7] = 1;
			Minecart._texturePosition[num] = new Vector2(1f, 2f);
			num++;
			Minecart._leftSideConnection[num] = 1;
			Minecart._rightSideConnection[num] = 0;
			Minecart._tileHeight[num][4] = 6;
			Minecart._tileHeight[num][5] = 6;
			Minecart._tileHeight[num][6] = 7;
			Minecart._tileHeight[num][7] = 8;
			Minecart._texturePosition[num] = new Vector2(0f, 1f);
			num++;
			Minecart._leftSideConnection[num] = 0;
			Minecart._rightSideConnection[num] = 1;
			Minecart._tileHeight[num][0] = 8;
			Minecart._tileHeight[num][1] = 7;
			Minecart._tileHeight[num][2] = 6;
			Minecart._tileHeight[num][3] = 6;
			Minecart._texturePosition[num] = new Vector2(1f, 1f);
			num++;
			Minecart._leftSideConnection[num] = 0;
			Minecart._rightSideConnection[num] = 2;
			for (int m = 0; m < 8; m++)
			{
				Minecart._tileHeight[num][m] = 8 - m;
			}
			Minecart._texturePosition[num] = new Vector2(0f, 3f);
			num++;
			Minecart._leftSideConnection[num] = 2;
			Minecart._rightSideConnection[num] = 0;
			for (int n = 0; n < 8; n++)
			{
				Minecart._tileHeight[num][n] = n + 1;
			}
			Minecart._texturePosition[num] = new Vector2(1f, 3f);
			num++;
			Minecart._leftSideConnection[num] = 2;
			Minecart._rightSideConnection[num] = -1;
			Minecart._tileHeight[num][0] = 1;
			Minecart._tileHeight[num][1] = 2;
			for (int num2 = 2; num2 < 8; num2++)
			{
				Minecart._tileHeight[num][num2] = -1;
			}
			Minecart._texturePosition[num] = new Vector2(4f, 1f);
			num++;
			Minecart._leftSideConnection[num] = -1;
			Minecart._rightSideConnection[num] = 2;
			Minecart._tileHeight[num][6] = 2;
			Minecart._tileHeight[num][7] = 1;
			for (int num3 = 0; num3 < 6; num3++)
			{
				Minecart._tileHeight[num][num3] = -1;
			}
			Minecart._texturePosition[num] = new Vector2(5f, 1f);
			num++;
			Minecart._leftSideConnection[num] = 0;
			Minecart._rightSideConnection[num] = -1;
			Minecart._tileHeight[num][0] = 8;
			Minecart._tileHeight[num][1] = 7;
			Minecart._tileHeight[num][2] = 6;
			for (int num4 = 3; num4 < 8; num4++)
			{
				Minecart._tileHeight[num][num4] = -1;
			}
			Minecart._texturePosition[num] = new Vector2(6f, 1f);
			num++;
			Minecart._leftSideConnection[num] = -1;
			Minecart._rightSideConnection[num] = 0;
			Minecart._tileHeight[num][5] = 6;
			Minecart._tileHeight[num][6] = 7;
			Minecart._tileHeight[num][7] = 8;
			for (int num5 = 0; num5 < 5; num5++)
			{
				Minecart._tileHeight[num][num5] = -1;
			}
			Minecart._texturePosition[num] = new Vector2(7f, 1f);
			num++;
			Minecart._leftSideConnection[num] = -1;
			Minecart._rightSideConnection[num] = 1;
			Minecart._tileHeight[num][0] = -4;
			Minecart._texturePosition[num] = new Vector2(2f, 0f);
			num++;
			Minecart._leftSideConnection[num] = 1;
			Minecart._rightSideConnection[num] = -1;
			Minecart._tileHeight[num][7] = -4;
			Minecart._texturePosition[num] = new Vector2(3f, 0f);
			num++;
			Minecart._leftSideConnection[num] = 2;
			Minecart._rightSideConnection[num] = -1;
			for (int num6 = 0; num6 < 6; num6++)
			{
				Minecart._tileHeight[num][num6] = num6 + 1;
			}
			Minecart._tileHeight[num][6] = -3;
			Minecart._tileHeight[num][7] = -3;
			Minecart._texturePosition[num] = new Vector2(4f, 0f);
			num++;
			Minecart._leftSideConnection[num] = -1;
			Minecart._rightSideConnection[num] = 2;
			Minecart._tileHeight[num][0] = -3;
			Minecart._tileHeight[num][1] = -3;
			for (int num7 = 2; num7 < 8; num7++)
			{
				Minecart._tileHeight[num][num7] = 8 - num7;
			}
			Minecart._texturePosition[num] = new Vector2(5f, 0f);
			num++;
			Minecart._leftSideConnection[num] = 0;
			Minecart._rightSideConnection[num] = -1;
			for (int num8 = 0; num8 < 6; num8++)
			{
				Minecart._tileHeight[num][num8] = 8 - num8;
			}
			Minecart._tileHeight[num][6] = -3;
			Minecart._tileHeight[num][7] = -3;
			Minecart._texturePosition[num] = new Vector2(6f, 0f);
			num++;
			Minecart._leftSideConnection[num] = -1;
			Minecart._rightSideConnection[num] = 0;
			Minecart._tileHeight[num][0] = -3;
			Minecart._tileHeight[num][1] = -3;
			for (int num9 = 2; num9 < 8; num9++)
			{
				Minecart._tileHeight[num][num9] = num9 + 1;
			}
			Minecart._texturePosition[num] = new Vector2(7f, 0f);
			num++;
			Minecart._leftSideConnection[num] = -1;
			Minecart._rightSideConnection[num] = -1;
			Minecart._tileHeight[num][0] = -4;
			Minecart._tileHeight[num][7] = -4;
			Minecart._trackType[num] = 1;
			Minecart._texturePosition[num] = new Vector2(0f, 4f);
			num++;
			Minecart._leftSideConnection[num] = 1;
			Minecart._rightSideConnection[num] = 1;
			Minecart._trackType[num] = 1;
			Minecart._texturePosition[num] = new Vector2(1f, 4f);
			num++;
			Minecart._leftSideConnection[num] = -1;
			Minecart._rightSideConnection[num] = 1;
			Minecart._tileHeight[num][0] = -4;
			Minecart._trackType[num] = 1;
			Minecart._texturePosition[num] = new Vector2(0f, 5f);
			num++;
			Minecart._leftSideConnection[num] = 1;
			Minecart._rightSideConnection[num] = -1;
			Minecart._tileHeight[num][7] = -4;
			Minecart._trackType[num] = 1;
			Minecart._texturePosition[num] = new Vector2(1f, 5f);
			num++;
			Minecart._leftSideConnection[num] = -1;
			Minecart._rightSideConnection[num] = 1;
			for (int num10 = 0; num10 < 6; num10++)
			{
				Minecart._tileHeight[num][num10] = -2;
			}
			Minecart._texturePosition[num] = new Vector2(2f, 2f);
			num++;
			Minecart._leftSideConnection[num] = 1;
			Minecart._rightSideConnection[num] = -1;
			for (int num11 = 2; num11 < 8; num11++)
			{
				Minecart._tileHeight[num][num11] = -2;
			}
			Minecart._texturePosition[num] = new Vector2(3f, 2f);
			num++;
			Minecart._leftSideConnection[num] = 2;
			Minecart._rightSideConnection[num] = -1;
			Minecart._tileHeight[num][0] = 1;
			Minecart._tileHeight[num][1] = 2;
			for (int num12 = 2; num12 < 8; num12++)
			{
				Minecart._tileHeight[num][num12] = -2;
			}
			Minecart._texturePosition[num] = new Vector2(4f, 2f);
			num++;
			Minecart._leftSideConnection[num] = -1;
			Minecart._rightSideConnection[num] = 2;
			Minecart._tileHeight[num][6] = 2;
			Minecart._tileHeight[num][7] = 1;
			for (int num13 = 0; num13 < 6; num13++)
			{
				Minecart._tileHeight[num][num13] = -2;
			}
			Minecart._texturePosition[num] = new Vector2(5f, 2f);
			num++;
			Minecart._leftSideConnection[num] = 0;
			Minecart._rightSideConnection[num] = -1;
			Minecart._tileHeight[num][0] = 8;
			Minecart._tileHeight[num][1] = 7;
			Minecart._tileHeight[num][2] = 6;
			for (int num14 = 3; num14 < 8; num14++)
			{
				Minecart._tileHeight[num][num14] = -2;
			}
			Minecart._texturePosition[num] = new Vector2(6f, 2f);
			num++;
			Minecart._leftSideConnection[num] = -1;
			Minecart._rightSideConnection[num] = 0;
			Minecart._tileHeight[num][5] = 6;
			Minecart._tileHeight[num][6] = 7;
			Minecart._tileHeight[num][7] = 8;
			for (int num15 = 0; num15 < 5; num15++)
			{
				Minecart._tileHeight[num][num15] = -2;
			}
			Minecart._texturePosition[num] = new Vector2(7f, 2f);
			num++;
			Minecart._leftSideConnection[num] = 1;
			Minecart._rightSideConnection[num] = 1;
			Minecart._trackType[num] = 2;
			Minecart._boostLeft[num] = false;
			Minecart._texturePosition[num] = new Vector2(2f, 3f);
			num++;
			Minecart._leftSideConnection[num] = 1;
			Minecart._rightSideConnection[num] = 1;
			Minecart._trackType[num] = 2;
			Minecart._boostLeft[num] = true;
			Minecart._texturePosition[num] = new Vector2(3f, 3f);
			num++;
			Minecart._leftSideConnection[num] = 0;
			Minecart._rightSideConnection[num] = 2;
			for (int num16 = 0; num16 < 8; num16++)
			{
				Minecart._tileHeight[num][num16] = 8 - num16;
			}
			Minecart._trackType[num] = 2;
			Minecart._boostLeft[num] = false;
			Minecart._texturePosition[num] = new Vector2(4f, 3f);
			num++;
			Minecart._leftSideConnection[num] = 2;
			Minecart._rightSideConnection[num] = 0;
			for (int num17 = 0; num17 < 8; num17++)
			{
				Minecart._tileHeight[num][num17] = num17 + 1;
			}
			Minecart._trackType[num] = 2;
			Minecart._boostLeft[num] = true;
			Minecart._texturePosition[num] = new Vector2(5f, 3f);
			num++;
			Minecart._leftSideConnection[num] = 0;
			Minecart._rightSideConnection[num] = 2;
			for (int num18 = 0; num18 < 8; num18++)
			{
				Minecart._tileHeight[num][num18] = 8 - num18;
			}
			Minecart._trackType[num] = 2;
			Minecart._boostLeft[num] = true;
			Minecart._texturePosition[num] = new Vector2(6f, 3f);
			num++;
			Minecart._leftSideConnection[num] = 2;
			Minecart._rightSideConnection[num] = 0;
			for (int num19 = 0; num19 < 8; num19++)
			{
				Minecart._tileHeight[num][num19] = num19 + 1;
			}
			Minecart._trackType[num] = 2;
			Minecart._boostLeft[num] = false;
			Minecart._texturePosition[num] = new Vector2(7f, 3f);
			num++;
			Minecart._texturePosition[36] = new Vector2(0f, 6f);
			Minecart._texturePosition[37] = new Vector2(1f, 6f);
			Minecart._texturePosition[39] = new Vector2(0f, 7f);
			Minecart._texturePosition[38] = new Vector2(1f, 7f);
			for (int num20 = 0; num20 < Minecart._texturePosition.Length; num20++)
			{
				Minecart._texturePosition[num20] = Minecart._texturePosition[num20] * 18f;
			}
			for (int num21 = 0; num21 < Minecart._tileHeight.Length; num21++)
			{
				int[] array2 = Minecart._tileHeight[num21];
				for (int num22 = 0; num22 < array2.Length; num22++)
				{
					if (array2[num22] >= 0)
					{
						array2[num22] = (8 - array2[num22]) * 2;
					}
				}
			}
			int[] array3 = new int[36];
			Minecart._trackSwitchOptions = new int[64][];
			for (int num23 = 0; num23 < 64; num23++)
			{
				int num24 = 0;
				for (int num25 = 1; num25 < 256; num25 <<= 1)
				{
					if ((num23 & num25) == num25)
					{
						num24++;
					}
				}
				int num26 = 0;
				int num27 = 0;
				while (num27 < 36)
				{
					array3[num27] = -1;
					int num28 = 0;
					switch (Minecart._leftSideConnection[num27])
					{
					case 0:
						num28 |= 1;
						break;
					case 1:
						num28 |= 2;
						break;
					case 2:
						num28 |= 4;
						break;
					}
					switch (Minecart._rightSideConnection[num27])
					{
					case 0:
						num28 |= 8;
						break;
					case 1:
						num28 |= 16;
						break;
					case 2:
						num28 |= 32;
						break;
					}
					if (num24 < 2)
					{
						if (num23 == num28)
						{
							goto IL_E41;
						}
					}
					else if (num28 != 0 && (num23 & num28) == num28)
					{
						goto IL_E41;
					}
					IL_E4D:
					num27++;
					continue;
					IL_E41:
					array3[num27] = num27;
					num26++;
					goto IL_E4D;
				}
				if (num26 != 0)
				{
					int[] array4 = new int[num26];
					int num29 = 0;
					for (int num30 = 0; num30 < 36; num30++)
					{
						if (array3[num30] != -1)
						{
							array4[num29] = array3[num30];
							num29++;
						}
					}
					Minecart._trackSwitchOptions[num23] = array4;
				}
			}
			Minecart._firstPressureFrame = -1;
			Minecart._firstLeftBoostFrame = -1;
			Minecart._firstRightBoostFrame = -1;
			for (int num31 = 0; num31 < Minecart._trackType.Length; num31++)
			{
				int num32 = Minecart._trackType[num31];
				if (num32 != 1)
				{
					if (num32 == 2)
					{
						if (Minecart._boostLeft[num31])
						{
							if (Minecart._firstLeftBoostFrame == -1)
							{
								Minecart._firstLeftBoostFrame = (short)num31;
							}
						}
						else if (Minecart._firstRightBoostFrame == -1)
						{
							Minecart._firstRightBoostFrame = (short)num31;
						}
					}
				}
				else if (Minecart._firstPressureFrame == -1)
				{
					Minecart._firstPressureFrame = (short)num31;
				}
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0002D199 File Offset: 0x0002B399
		public static bool IsPressurePlate(Tile tile)
		{
			return tile != null && (tile.active() && tile.type == 314 && (tile.frameX == 20 || tile.frameX == 21));
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0002D1CC File Offset: 0x0002B3CC
		public static BitsByte TrackCollision(Player Player, ref Vector2 Position, ref Vector2 Velocity, ref Vector2 lastBoost, int Width, int Height, bool followDown, bool followUp, int fallStart, bool trackOnly, Mount.MountDelegatesData delegatesData)
		{
			if (followDown && followUp)
			{
				followDown = false;
				followUp = false;
			}
			Minecart.Customization minecartSettings = Player.MinecartSettings;
			Vector2 vector = new Vector2((float)(Width / 2) - minecartSettings.MinecartTextureWidth / 2f, (float)(Height / 2));
			Vector2 vector2 = Position + new Vector2((float)(Width / 2) - minecartSettings.MinecartTextureWidth / 2f, (float)(Height / 2)) + minecartSettings.MagnetOffset;
			Vector2 vector3 = Velocity;
			float num = vector3.Length();
			vector3.Normalize();
			Vector2 vector4 = vector2;
			Tile tile = null;
			bool flag = false;
			bool flag2 = true;
			int num2 = -1;
			int num3 = -1;
			int num4 = -1;
			Minecart.TrackState trackState = Minecart.TrackState.NoTrack;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			Vector2 vector5 = Vector2.Zero;
			Vector2 value = Vector2.Zero;
			BitsByte result = default(BitsByte);
			for (;;)
			{
				int num5 = (int)(vector4.X / 16f);
				int num6 = (int)(vector4.Y / 16f);
				int num7 = (int)vector4.X % 16 / 2;
				if (flag2)
				{
					num4 = num7;
				}
				bool flag7 = num7 != num4;
				if ((trackState == Minecart.TrackState.OnBack || trackState == Minecart.TrackState.OnTrack || trackState == Minecart.TrackState.OnFront) && num5 != num2)
				{
					int num8;
					if (trackState == Minecart.TrackState.OnBack)
					{
						num8 = (int)tile.BackTrack();
					}
					else
					{
						num8 = (int)tile.FrontTrack();
					}
					int num9;
					if (vector3.X < 0f)
					{
						num9 = Minecart._leftSideConnection[num8];
					}
					else
					{
						num9 = Minecart._rightSideConnection[num8];
					}
					switch (num9)
					{
					case 0:
						num6--;
						vector4.Y -= 2f;
						break;
					case 2:
						num6++;
						vector4.Y += 2f;
						break;
					}
				}
				Minecart.TrackState trackState2 = Minecart.TrackState.NoTrack;
				bool flag8 = false;
				if (num5 != num2 || num6 != num3)
				{
					if (flag2)
					{
						flag2 = false;
					}
					else
					{
						flag8 = true;
					}
					tile = Main.tile[num5, num6];
					if (tile == null)
					{
						tile = new Tile();
						Main.tile[num5, num6] = tile;
					}
					flag = (tile.nactive() && tile.type == 314);
				}
				if (flag)
				{
					Minecart.TrackState trackState3 = Minecart.TrackState.NoTrack;
					int num10 = (int)tile.FrontTrack();
					int num11 = (int)tile.BackTrack();
					int num12 = Minecart._tileHeight[num10][num7];
					switch (num12)
					{
					case -4:
						if (trackState == Minecart.TrackState.OnFront)
						{
							if (trackOnly)
							{
								vector4 -= value;
								num = 0f;
								trackState2 = Minecart.TrackState.OnFront;
								flag6 = true;
							}
							else
							{
								trackState2 = Minecart.TrackState.NoTrack;
								flag5 = true;
							}
						}
						break;
					case -3:
						if (trackState == Minecart.TrackState.OnFront)
						{
							trackState = Minecart.TrackState.NoTrack;
							Matrix matrix;
							if (Velocity.X > 0f)
							{
								if (Minecart._leftSideConnection[num10] == 2)
								{
									matrix = Matrix.CreateRotationZ(-0.7853982f);
								}
								else
								{
									matrix = Matrix.CreateRotationZ(0.7853982f);
								}
							}
							else if (Minecart._rightSideConnection[num10] == 2)
							{
								matrix = Matrix.CreateRotationZ(0.7853982f);
							}
							else
							{
								matrix = Matrix.CreateRotationZ(-0.7853982f);
							}
							vector5 = Vector2.Transform(new Vector2(Velocity.X, 0f), matrix);
							vector5.X = Velocity.X;
							flag4 = true;
							num = 0f;
						}
						break;
					case -2:
						if (trackState == Minecart.TrackState.OnFront)
						{
							if (trackOnly)
							{
								vector4 -= value;
								num = 0f;
								trackState2 = Minecart.TrackState.OnFront;
								flag6 = true;
							}
							else
							{
								if (vector3.X < 0f)
								{
									float num13 = (float)(num5 * 16 + (num7 + 1) * 2) - vector4.X;
									vector4.X += num13;
									num += num13 / vector3.X;
								}
								vector3.X = -vector3.X;
								result[1] = true;
								trackState2 = Minecart.TrackState.OnFront;
							}
						}
						break;
					case -1:
						if (trackState == Minecart.TrackState.OnFront)
						{
							vector4 -= value;
							num = 0f;
							trackState2 = Minecart.TrackState.OnFront;
							flag6 = true;
						}
						break;
					default:
					{
						float num14 = (float)(num6 * 16 + num12);
						if (num5 != num2 && trackState == Minecart.TrackState.NoTrack && vector4.Y > num14 && vector4.Y - num14 < 2f)
						{
							flag8 = false;
							trackState = Minecart.TrackState.AboveFront;
						}
						Minecart.TrackState trackState4;
						if (vector4.Y < num14)
						{
							trackState4 = Minecart.TrackState.AboveTrack;
						}
						else if (vector4.Y > num14)
						{
							trackState4 = Minecart.TrackState.BelowTrack;
						}
						else
						{
							trackState4 = Minecart.TrackState.OnTrack;
						}
						if (num11 != -1)
						{
							float num15 = (float)(num6 * 16 + Minecart._tileHeight[num11][num7]);
							if (vector4.Y < num15)
							{
								trackState3 = Minecart.TrackState.AboveTrack;
							}
							else if (vector4.Y > num15)
							{
								trackState3 = Minecart.TrackState.BelowTrack;
							}
							else
							{
								trackState3 = Minecart.TrackState.OnTrack;
							}
						}
						switch (trackState4)
						{
						case Minecart.TrackState.AboveTrack:
							switch (trackState3)
							{
							case Minecart.TrackState.AboveTrack:
								trackState2 = Minecart.TrackState.AboveTrack;
								break;
							case Minecart.TrackState.OnTrack:
								trackState2 = Minecart.TrackState.OnBack;
								break;
							case Minecart.TrackState.BelowTrack:
								trackState2 = Minecart.TrackState.AboveFront;
								break;
							default:
								trackState2 = Minecart.TrackState.AboveFront;
								break;
							}
							break;
						case Minecart.TrackState.OnTrack:
							if (trackState3 == Minecart.TrackState.OnTrack)
							{
								trackState2 = Minecart.TrackState.OnTrack;
							}
							else
							{
								trackState2 = Minecart.TrackState.OnFront;
							}
							break;
						case Minecart.TrackState.BelowTrack:
							switch (trackState3)
							{
							case Minecart.TrackState.AboveTrack:
								trackState2 = Minecart.TrackState.AboveBack;
								break;
							case Minecart.TrackState.OnTrack:
								trackState2 = Minecart.TrackState.OnBack;
								break;
							case Minecart.TrackState.BelowTrack:
								trackState2 = Minecart.TrackState.BelowTrack;
								break;
							default:
								trackState2 = Minecart.TrackState.BelowTrack;
								break;
							}
							break;
						}
						break;
					}
					}
				}
				if (!flag8)
				{
					if (trackState != trackState2)
					{
						bool flag9 = false;
						if (flag7 || vector3.Y > 0f)
						{
							switch (trackState)
							{
							case Minecart.TrackState.AboveTrack:
								switch (trackState2)
								{
								case Minecart.TrackState.AboveTrack:
									trackState2 = Minecart.TrackState.OnTrack;
									break;
								case Minecart.TrackState.AboveFront:
									trackState2 = Minecart.TrackState.OnBack;
									break;
								case Minecart.TrackState.AboveBack:
									trackState2 = Minecart.TrackState.OnFront;
									break;
								}
								break;
							case Minecart.TrackState.OnTrack:
							{
								int num16 = Minecart._tileHeight[(int)tile.FrontTrack()][num7];
								int num17 = Minecart._tileHeight[(int)tile.BackTrack()][num7];
								if (followDown)
								{
									if (num16 < num17)
									{
										trackState2 = Minecart.TrackState.OnBack;
									}
									else
									{
										trackState2 = Minecart.TrackState.OnFront;
									}
								}
								else if (followUp)
								{
									if (num16 < num17)
									{
										trackState2 = Minecart.TrackState.OnFront;
									}
									else
									{
										trackState2 = Minecart.TrackState.OnBack;
									}
								}
								else
								{
									trackState2 = Minecart.TrackState.OnFront;
								}
								flag9 = true;
								break;
							}
							case Minecart.TrackState.AboveFront:
								if (trackState2 == Minecart.TrackState.BelowTrack)
								{
									trackState2 = Minecart.TrackState.OnFront;
								}
								break;
							case Minecart.TrackState.AboveBack:
								if (trackState2 == Minecart.TrackState.BelowTrack)
								{
									trackState2 = Minecart.TrackState.OnBack;
								}
								break;
							case Minecart.TrackState.OnFront:
								trackState2 = Minecart.TrackState.OnFront;
								flag9 = true;
								break;
							case Minecart.TrackState.OnBack:
								trackState2 = Minecart.TrackState.OnBack;
								flag9 = true;
								break;
							}
							int num18 = -1;
							if (trackState2 != Minecart.TrackState.OnTrack && trackState2 != Minecart.TrackState.OnFront)
							{
								if (trackState2 == Minecart.TrackState.OnBack)
								{
									num18 = (int)tile.BackTrack();
								}
							}
							else
							{
								num18 = (int)tile.FrontTrack();
							}
							if (num18 != -1)
							{
								if (!flag9 && Velocity.Y > Player.defaultGravity)
								{
									int num19 = (int)(Position.Y / 16f);
									if (fallStart < num19 - 1)
									{
										delegatesData.MinecartLandingSound(Player, Position, Width, Height);
										Minecart.WheelSparks(delegatesData.MinecartDust, Position, Width, Height, 10, minecartSettings);
									}
								}
								if (trackState == Minecart.TrackState.AboveFront && Minecart._trackType[num18] == 1)
								{
									flag3 = true;
								}
								vector3.Y = 0f;
								vector4.Y = (float)(num6 * 16 + Minecart._tileHeight[num18][num7]);
							}
						}
					}
				}
				else if (trackState2 == Minecart.TrackState.OnFront || trackState2 == Minecart.TrackState.OnBack || trackState2 == Minecart.TrackState.OnTrack)
				{
					if (flag && Minecart._trackType[(int)tile.FrontTrack()] == 1)
					{
						flag3 = true;
					}
					vector3.Y = 0f;
				}
				if (trackState2 == Minecart.TrackState.OnFront)
				{
					int num20 = (int)tile.FrontTrack();
					if (Minecart._trackType[num20] == 2 && lastBoost.X == 0f && lastBoost.Y == 0f)
					{
						lastBoost = new Vector2((float)num5, (float)num6);
						if (Minecart._boostLeft[num20])
						{
							result[4] = true;
						}
						else
						{
							result[5] = true;
						}
					}
				}
				num4 = num7;
				trackState = trackState2;
				num2 = num5;
				num3 = num6;
				if (num <= 0f)
				{
					break;
				}
				float num21 = vector4.X % 2f;
				float num22 = vector4.Y % 2f;
				float num23 = 3f;
				float num24 = 3f;
				if (vector3.X < 0f)
				{
					num23 = num21 + 0.125f;
				}
				else if (vector3.X > 0f)
				{
					num23 = 2f - num21;
				}
				if (vector3.Y < 0f)
				{
					num24 = num22 + 0.125f;
				}
				else if (vector3.Y > 0f)
				{
					num24 = 2f - num22;
				}
				if (num23 == 3f && num24 == 3f)
				{
					goto IL_85F;
				}
				float num25 = Math.Abs(num23 / vector3.X);
				float num26 = Math.Abs(num24 / vector3.Y);
				float num27 = (num25 < num26) ? num25 : num26;
				if (num27 > num)
				{
					value = vector3 * num;
					num = 0f;
				}
				else
				{
					value = vector3 * num27;
					num -= num27;
				}
				vector4 += value;
			}
			if (lastBoost.X != (float)num2 || lastBoost.Y != (float)num3)
			{
				lastBoost = Vector2.Zero;
			}
			IL_85F:
			if (flag3)
			{
				result[3] = true;
			}
			if (flag5)
			{
				Velocity.X = vector4.X - vector2.X;
				Velocity.Y = Player.defaultGravity;
			}
			else if (flag4)
			{
				result[2] = true;
				Velocity = vector5;
			}
			else if (result[1])
			{
				Velocity.X = -Velocity.X;
				Position.X = vector4.X - minecartSettings.MagnetOffset.X - vector.X - Velocity.X;
				if (vector3.Y == 0f)
				{
					Velocity.Y = 0f;
				}
			}
			else
			{
				if (flag6)
				{
					Velocity.X = vector4.X - vector2.X;
				}
				if (vector3.Y == 0f)
				{
					Velocity.Y = 0f;
				}
			}
			Position.Y += vector4.Y - vector2.Y - Velocity.Y;
			Position.Y = (float)Math.Round((double)Position.Y, 2);
			if (trackState == Minecart.TrackState.OnTrack || trackState - Minecart.TrackState.OnFront <= 1)
			{
				result[0] = true;
			}
			return result;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0002DB5C File Offset: 0x0002BD5C
		public static bool FrameTrack(int i, int j, bool pound, bool mute = false)
		{
			if (Minecart._trackType == null)
			{
				return false;
			}
			Tile tile = Main.tile[i, j];
			if (tile == null)
			{
				tile = new Tile();
				Main.tile[i, j] = tile;
			}
			if (mute && tile.type != 314)
			{
				return false;
			}
			int nearbyTilesSetLookupIndex = Minecart.GetNearbyTilesSetLookupIndex(i, j);
			int num = (int)tile.FrontTrack();
			int num2 = (int)tile.BackTrack();
			int num3;
			if (num >= 0 && num < Minecart._trackType.Length)
			{
				num3 = Minecart._trackType[num];
			}
			else
			{
				num3 = 0;
			}
			int num4 = -1;
			int num5 = -1;
			int[] array = Minecart._trackSwitchOptions[nearbyTilesSetLookupIndex];
			if (array != null)
			{
				if (!pound)
				{
					int num6 = -1;
					int num7 = -1;
					bool flag = false;
					for (int k = 0; k < array.Length; k++)
					{
						int num8 = array[k];
						if (num2 == array[k])
						{
							num5 = k;
						}
						if (Minecart._trackType[num8] == num3)
						{
							if (Minecart._leftSideConnection[num8] == -1 || Minecart._rightSideConnection[num8] == -1)
							{
								if (num == array[k])
								{
									num4 = k;
									flag = true;
								}
								if (num6 == -1)
								{
									num6 = k;
								}
							}
							else
							{
								if (num == array[k])
								{
									num4 = k;
									flag = false;
								}
								if (num7 == -1)
								{
									num7 = k;
								}
							}
						}
					}
					if (num7 != -1)
					{
						if (num4 == -1 || flag)
						{
							num4 = num7;
						}
					}
					else
					{
						if (num4 == -1)
						{
							if (num3 == 2)
							{
								return false;
							}
							if (num3 == 1)
							{
								return false;
							}
							num4 = num6;
						}
						num5 = -1;
					}
				}
				else
				{
					for (int l = 0; l < array.Length; l++)
					{
						if (num == array[l])
						{
							num4 = l;
						}
						if (num2 == array[l])
						{
							num5 = l;
						}
					}
					int num9 = 0;
					int num10 = 0;
					for (int m = 0; m < array.Length; m++)
					{
						if (Minecart._trackType[array[m]] == num3)
						{
							if (Minecart._leftSideConnection[array[m]] == -1 || Minecart._rightSideConnection[array[m]] == -1)
							{
								num10++;
							}
							else
							{
								num9++;
							}
						}
					}
					if (num9 < 2 && num10 < 2)
					{
						return false;
					}
					bool flag2 = num9 == 0;
					bool flag3 = false;
					if (!flag2)
					{
						while (!flag3)
						{
							num5++;
							if (num5 >= array.Length)
							{
								num5 = -1;
								break;
							}
							if ((Minecart._leftSideConnection[array[num5]] != Minecart._leftSideConnection[array[num4]] || Minecart._rightSideConnection[array[num5]] != Minecart._rightSideConnection[array[num4]]) && Minecart._trackType[array[num5]] == num3 && Minecart._leftSideConnection[array[num5]] != -1 && Minecart._rightSideConnection[array[num5]] != -1)
							{
								flag3 = true;
							}
						}
					}
					if (!flag3)
					{
						for (;;)
						{
							num4++;
							if (num4 >= array.Length)
							{
								break;
							}
							if (Minecart._trackType[array[num4]] == num3 && (Minecart._leftSideConnection[array[num4]] == -1 || Minecart._rightSideConnection[array[num4]] == -1) == flag2)
							{
								goto IL_2EA;
							}
						}
						num4 = -1;
						for (;;)
						{
							num4++;
							if (Minecart._trackType[array[num4]] == num3)
							{
								if ((Minecart._leftSideConnection[array[num4]] == -1 || Minecart._rightSideConnection[array[num4]] == -1) == flag2)
								{
									break;
								}
							}
						}
					}
				}
				IL_2EA:
				bool flag4 = false;
				if (num4 == -2)
				{
					if (tile.FrontTrack() != Minecart._firstPressureFrame)
					{
						flag4 = true;
					}
				}
				else if (num4 == -1)
				{
					if (tile.FrontTrack() != 0)
					{
						flag4 = true;
					}
				}
				else if ((int)tile.FrontTrack() != array[num4])
				{
					flag4 = true;
				}
				if (num5 == -1)
				{
					if (tile.BackTrack() != -1)
					{
						flag4 = true;
					}
				}
				else if ((int)tile.BackTrack() != array[num5])
				{
					flag4 = true;
				}
				if (num4 == -2)
				{
					tile.FrontTrack(Minecart._firstPressureFrame);
				}
				else if (num4 == -1)
				{
					tile.FrontTrack(0);
				}
				else
				{
					tile.FrontTrack((short)array[num4]);
				}
				if (num5 == -1)
				{
					tile.BackTrack(-1);
				}
				else
				{
					tile.BackTrack((short)array[num5]);
				}
				if (pound && flag4 && !mute)
				{
					WorldGen.KillTile(i, j, true, false, false);
				}
				return true;
			}
			if (pound)
			{
				return false;
			}
			tile.FrontTrack(0);
			tile.BackTrack(-1);
			return false;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0002DF10 File Offset: 0x0002C110
		private static int GetNearbyTilesSetLookupIndex(int i, int j)
		{
			int num = 0;
			if (Main.tile[i - 1, j - 1] != null && Main.tile[i - 1, j - 1].type == 314)
			{
				num++;
			}
			if (Main.tile[i - 1, j] != null && Main.tile[i - 1, j].type == 314)
			{
				num += 2;
			}
			if (Main.tile[i - 1, j + 1] != null && Main.tile[i - 1, j + 1].type == 314)
			{
				num += 4;
			}
			if (Main.tile[i + 1, j - 1] != null && Main.tile[i + 1, j - 1].type == 314)
			{
				num += 8;
			}
			if (Main.tile[i + 1, j] != null && Main.tile[i + 1, j].type == 314)
			{
				num += 16;
			}
			if (Main.tile[i + 1, j + 1] != null && Main.tile[i + 1, j + 1].type == 314)
			{
				num += 32;
			}
			return num;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0002E048 File Offset: 0x0002C248
		public static bool GetOnTrack(int tileX, int tileY, ref Vector2 Position, int Width, int Height, Minecart.Customization settings)
		{
			Tile tile = Main.tile[tileX, tileY];
			if (tile.type != 314)
			{
				return false;
			}
			Vector2 value = new Vector2((float)(Width / 2) - settings.MinecartTextureWidth / 2f, (float)(Height / 2));
			Vector2 vector = Position + value + settings.MagnetOffset;
			int num = (int)vector.X % 16 / 2;
			int num2 = -1;
			int num3 = 0;
			for (int i = num; i < 8; i++)
			{
				num3 = Minecart._tileHeight[(int)tile.frameX][i];
				if (num3 >= 0)
				{
					num2 = i;
					break;
				}
			}
			if (num2 == -1)
			{
				for (int j = num - 1; j >= 0; j--)
				{
					num3 = Minecart._tileHeight[(int)tile.frameX][j];
					if (num3 >= 0)
					{
						num2 = j;
						break;
					}
				}
			}
			if (num2 == -1)
			{
				return false;
			}
			vector.X = (float)(tileX * 16 + num2 * 2);
			vector.Y = (float)(tileY * 16 + num3);
			vector -= settings.MagnetOffset;
			vector -= value;
			Position = vector;
			return true;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0002E160 File Offset: 0x0002C360
		public static bool OnTrack(Vector2 Position, int Width, int Height, Minecart.Customization settings)
		{
			Vector2 vector = Position + new Vector2((float)(Width / 2) - settings.MinecartTextureWidth / 2f, (float)(Height / 2)) + settings.MagnetOffset;
			int num = (int)(vector.X / 16f);
			int num2 = (int)(vector.Y / 16f);
			return Main.tile[num, num2] != null && Main.tile[num, num2].type == 314;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0002E1DC File Offset: 0x0002C3DC
		public static float TrackRotation(Player player, ref float rotation, Vector2 Position, int Width, int Height, bool followDown, bool followUp, Mount.MountDelegatesData delegatesData)
		{
			Vector2 vector;
			Vector2 vector2;
			Minecart.GetWheelsPositions(player, Position, Width, Height, followDown, followUp, delegatesData, out vector, out vector2);
			float num = vector2.Y - vector.Y;
			float num2 = vector2.X - vector.X;
			float num3 = num / num2;
			float num4 = vector.Y + (Position.X - vector.X) * num3;
			float num5 = (Position.X - (float)((int)Position.X)) * num3;
			rotation = (float)Math.Atan2((double)num, (double)num2);
			return num4 - Position.Y + num5;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0002E260 File Offset: 0x0002C460
		public static void GetWheelsPositions(Player player, Vector2 Position, int Width, int Height, bool followDown, bool followUp, Mount.MountDelegatesData delegatesData, out Vector2 leftWheel, out Vector2 rightWheel)
		{
			leftWheel = Position;
			rightWheel = Position;
			Vector2 zero = Vector2.Zero;
			Minecart.Customization minecartSettings = player.MinecartSettings;
			Vector2 value = minecartSettings.WheelOffset * new Vector2(-1f, 1f);
			Minecart.TrackCollision(player, ref leftWheel, ref value, ref zero, Width, Height, followDown, followUp, 0, true, delegatesData);
			leftWheel += value;
			value = minecartSettings.WheelOffset;
			Minecart.TrackCollision(player, ref rightWheel, ref value, ref zero, Width, Height, followDown, followUp, 0, true, delegatesData);
			rightWheel += value;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0002E300 File Offset: 0x0002C500
		public static void HitTrackSwitch(Vector2 Position, int Width, int Height, Minecart.Customization settings)
		{
			Vector2 magnetPosition = Minecart.GetMagnetPosition(Position, Width, Height, settings);
			int num = (int)(magnetPosition.X / 16f);
			int num2 = (int)(magnetPosition.Y / 16f);
			Wiring.HitSwitch(num, num2);
			NetMessage.SendData(59, -1, -1, null, num, (float)num2, 0f, 0f, 0, 0, 0);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0002E354 File Offset: 0x0002C554
		public static Vector2 GetMagnetPosition(Vector2 Position, int Width, int Height, Minecart.Customization settings)
		{
			new Vector2((float)(Width / 2) - settings.MinecartTextureWidth / 2f, (float)(Height / 2));
			return Position + new Vector2((float)(Width / 2) - settings.MinecartTextureWidth / 2f, (float)(Height / 2)) + settings.MagnetOffset;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0002E3A8 File Offset: 0x0002C5A8
		public static void FlipSwitchTrack(int i, int j)
		{
			Tile tileTrack = Main.tile[i, j];
			short num = tileTrack.FrontTrack();
			if (num == -1)
			{
				return;
			}
			int num2 = Minecart._trackType[(int)num];
			if (num2 != 0)
			{
				if (num2 != 2)
				{
					return;
				}
				Minecart.FrameTrack(i, j, true, true);
				NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
			}
			else if (tileTrack.BackTrack() != -1)
			{
				tileTrack.FrontTrack(tileTrack.BackTrack());
				tileTrack.BackTrack(num);
				NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
				return;
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0002E418 File Offset: 0x0002C618
		public static void TrackColors(int i, int j, Tile trackTile, out int frontColor, out int backColor)
		{
			if (trackTile.type == 314)
			{
				frontColor = (int)trackTile.color();
				backColor = frontColor;
				if (trackTile.frameY == -1)
				{
					return;
				}
				int num = Minecart._leftSideConnection[(int)trackTile.frameX];
				int num2 = Minecart._rightSideConnection[(int)trackTile.frameX];
				int num3 = Minecart._leftSideConnection[(int)trackTile.frameY];
				int num4 = Minecart._rightSideConnection[(int)trackTile.frameY];
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				int num8 = 0;
				for (int k = 0; k < 4; k++)
				{
					int num9;
					switch (k)
					{
					case 1:
						num9 = num2;
						break;
					case 2:
						num9 = num3;
						break;
					case 3:
						num9 = num4;
						break;
					default:
						num9 = num;
						break;
					}
					int num10;
					switch (num9)
					{
					case 0:
						num10 = -1;
						break;
					case 1:
						num10 = 0;
						break;
					case 2:
						num10 = 1;
						break;
					default:
						num10 = 0;
						break;
					}
					Tile tile;
					if (k % 2 == 0)
					{
						tile = Main.tile[i - 1, j + num10];
					}
					else
					{
						tile = Main.tile[i + 1, j + num10];
					}
					int num11;
					if (tile == null || !tile.active() || tile.type != 314)
					{
						num11 = 0;
					}
					else
					{
						num11 = (int)tile.color();
					}
					switch (k)
					{
					case 1:
						num6 = num11;
						break;
					case 2:
						num7 = num11;
						break;
					case 3:
						num8 = num11;
						break;
					default:
						num5 = num11;
						break;
					}
				}
				if (num == num3)
				{
					if (num6 != 0)
					{
						frontColor = num6;
					}
					else if (num5 != 0)
					{
						frontColor = num5;
					}
					if (num8 != 0)
					{
						backColor = num8;
						return;
					}
					if (num7 != 0)
					{
						backColor = num7;
						return;
					}
				}
				else if (num2 == num4)
				{
					if (num5 != 0)
					{
						frontColor = num5;
					}
					else if (num6 != 0)
					{
						frontColor = num6;
					}
					if (num7 != 0)
					{
						backColor = num7;
						return;
					}
					if (num8 != 0)
					{
						backColor = num8;
						return;
					}
				}
				else
				{
					if (num6 == 0)
					{
						if (num5 != 0)
						{
							frontColor = num5;
						}
					}
					else if (num5 != 0)
					{
						frontColor = ((num2 <= num) ? num6 : num5);
					}
					if (num8 == 0)
					{
						if (num7 != 0)
						{
							backColor = num7;
							return;
						}
					}
					else if (num7 != 0)
					{
						backColor = ((num4 <= num3) ? num8 : num7);
						return;
					}
				}
			}
			else
			{
				frontColor = 0;
				backColor = 0;
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0002E60B File Offset: 0x0002C80B
		public static bool DrawLeftDecoration(int frameID)
		{
			return frameID >= 0 && frameID < 36 && Minecart._leftSideConnection[frameID] == 2;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0002E622 File Offset: 0x0002C822
		public static bool DrawRightDecoration(int frameID)
		{
			return frameID >= 0 && frameID < 36 && Minecart._rightSideConnection[frameID] == 2;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0002E639 File Offset: 0x0002C839
		public static bool DrawBumper(int frameID)
		{
			return frameID >= 0 && frameID < 36 && (Minecart._tileHeight[frameID][0] == -1 || Minecart._tileHeight[frameID][7] == -1);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0002E660 File Offset: 0x0002C860
		public static bool DrawBouncyBumper(int frameID)
		{
			return frameID >= 0 && frameID < 36 && (Minecart._tileHeight[frameID][0] == -2 || Minecart._tileHeight[frameID][7] == -2);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0002E68C File Offset: 0x0002C88C
		public static void PlaceTrack(Tile trackCache, int style)
		{
			trackCache.active(true);
			trackCache.type = 314;
			trackCache.frameY = -1;
			switch (style)
			{
			case 0:
				trackCache.frameX = -1;
				return;
			case 1:
				trackCache.frameX = Minecart._firstPressureFrame;
				return;
			case 2:
				trackCache.frameX = Minecart._firstLeftBoostFrame;
				return;
			case 3:
				trackCache.frameX = Minecart._firstRightBoostFrame;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0002E6F4 File Offset: 0x0002C8F4
		public static int GetTrackItem(Tile trackCache)
		{
			switch (Minecart._trackType[(int)trackCache.frameX])
			{
			case 0:
				return 2340;
			case 1:
				return 2492;
			case 2:
				return 2739;
			default:
				return 0;
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0002E738 File Offset: 0x0002C938
		public static Rectangle GetSourceRect(int frameID, int animationFrame = 0)
		{
			if (frameID < 0 || frameID >= 40)
			{
				return new Rectangle(0, 0, 0, 0);
			}
			Vector2 vector = Minecart._texturePosition[frameID];
			Rectangle result = new Rectangle((int)vector.X, (int)vector.Y, 16, 16);
			if (frameID < 36 && Minecart._trackType[frameID] == 2)
			{
				result.Y += 18 * animationFrame;
			}
			return result;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0002E79C File Offset: 0x0002C99C
		public static bool GetAreExpectationsForSidesMet(Point tileCoords, int? expectedYOffsetForLeft, int? expectedYOffsetForRight)
		{
			Tile tileTrack = Main.tile[tileCoords.X, tileCoords.Y];
			if (expectedYOffsetForLeft != null)
			{
				short num = tileTrack.FrontTrack();
				int num2 = Minecart.ConvertOffsetYToTrackConnectionValue(expectedYOffsetForLeft.Value);
				if (Minecart._leftSideConnection[(int)num] != num2)
				{
					return false;
				}
			}
			if (expectedYOffsetForRight != null)
			{
				short num3 = tileTrack.FrontTrack();
				int num4 = Minecart.ConvertOffsetYToTrackConnectionValue(expectedYOffsetForRight.Value);
				if (Minecart._rightSideConnection[(int)num3] != num4)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0002E818 File Offset: 0x0002CA18
		public static void TryFittingTileOrientation(Point tileCoords, int? expectedYOffsetForLeft, int? expectedYOffsetForRight)
		{
			int nearbyTilesSetLookupIndex = Minecart.GetNearbyTilesSetLookupIndex(tileCoords.X, tileCoords.Y);
			int[] array = Minecart._trackSwitchOptions[nearbyTilesSetLookupIndex];
			if (array == null)
			{
				return;
			}
			Tile tileSafely = Framing.GetTileSafely(tileCoords);
			int num = Minecart._trackType[(int)tileSafely.FrontTrack()];
			int? num2 = null;
			int i = 0;
			while (i < array.Length)
			{
				int num3 = array[i];
				int num4 = Minecart._leftSideConnection[num3];
				int num5 = Minecart._rightSideConnection[num3];
				int num6 = Minecart._trackType[num3];
				if (expectedYOffsetForLeft == null)
				{
					goto IL_86;
				}
				int num7 = Minecart.ConvertOffsetYToTrackConnectionValue(expectedYOffsetForLeft.Value);
				if (Minecart._leftSideConnection[num3] == num7)
				{
					goto IL_86;
				}
				IL_BF:
				i++;
				continue;
				IL_86:
				if (expectedYOffsetForRight != null)
				{
					int num8 = Minecart.ConvertOffsetYToTrackConnectionValue(expectedYOffsetForRight.Value);
					if (Minecart._rightSideConnection[num3] != num8)
					{
						goto IL_BF;
					}
				}
				if (Minecart._trackType[num3] == num)
				{
					num2 = new int?(num3);
					break;
				}
				goto IL_BF;
			}
			if (num2 != null)
			{
				tileSafely.FrontTrack((short)num2.Value);
				NetMessage.SendTileSquare(-1, tileCoords.X, tileCoords.Y, TileChangeType.None);
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0002E91E File Offset: 0x0002CB1E
		private static int ConvertOffsetYToTrackConnectionValue(int offsetY)
		{
			if (offsetY == -1)
			{
				return 0;
			}
			if (offsetY != 1)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0002E92F File Offset: 0x0002CB2F
		private static int ConvertTrackConnectionValueToOffsetY(int trackConnectionValue)
		{
			if (trackConnectionValue == 0)
			{
				return -1;
			}
			if (trackConnectionValue != 2)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0002E940 File Offset: 0x0002CB40
		public static void WheelSparks(Action<Vector2> DustAction, Vector2 Position, int Width, int Height, int sparkCount, Minecart.Customization settings)
		{
			Vector2 value = new Vector2((float)(Width / 2) - settings.MinecartTextureWidth / 2f, (float)(Height / 2));
			Vector2 obj = Position + value + settings.MagnetOffset;
			for (int i = 0; i < sparkCount; i++)
			{
				DustAction(obj);
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0002E992 File Offset: 0x0002CB92
		private static short FrontTrack(this Tile tileTrack)
		{
			return tileTrack.frameX;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0002E99A File Offset: 0x0002CB9A
		private static void FrontTrack(this Tile tileTrack, short trackID)
		{
			tileTrack.frameX = trackID;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0002E9A3 File Offset: 0x0002CBA3
		private static short BackTrack(this Tile tileTrack)
		{
			return tileTrack.frameY;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0002E9AB File Offset: 0x0002CBAB
		private static void BackTrack(this Tile tileTrack, short trackID)
		{
			tileTrack.frameY = trackID;
		}

		// Token: 0x0400019B RID: 411
		private const int TotalFrames = 36;

		// Token: 0x0400019C RID: 412
		public const int LeftDownDecoration = 36;

		// Token: 0x0400019D RID: 413
		public const int RightDownDecoration = 37;

		// Token: 0x0400019E RID: 414
		public const int BouncyBumperDecoration = 38;

		// Token: 0x0400019F RID: 415
		public const int RegularBumperDecoration = 39;

		// Token: 0x040001A0 RID: 416
		public const int Flag_OnTrack = 0;

		// Token: 0x040001A1 RID: 417
		public const int Flag_BouncyBumper = 1;

		// Token: 0x040001A2 RID: 418
		public const int Flag_UsedRamp = 2;

		// Token: 0x040001A3 RID: 419
		public const int Flag_HitSwitch = 3;

		// Token: 0x040001A4 RID: 420
		public const int Flag_BoostLeft = 4;

		// Token: 0x040001A5 RID: 421
		public const int Flag_BoostRight = 5;

		// Token: 0x040001A6 RID: 422
		private const int NoConnection = -1;

		// Token: 0x040001A7 RID: 423
		private const int TopConnection = 0;

		// Token: 0x040001A8 RID: 424
		private const int MiddleConnection = 1;

		// Token: 0x040001A9 RID: 425
		private const int BottomConnection = 2;

		// Token: 0x040001AA RID: 426
		private const int BumperEnd = -1;

		// Token: 0x040001AB RID: 427
		private const int BouncyEnd = -2;

		// Token: 0x040001AC RID: 428
		private const int RampEnd = -3;

		// Token: 0x040001AD RID: 429
		private const int OpenEnd = -4;

		// Token: 0x040001AE RID: 430
		public const float BoosterSpeed = 4f;

		// Token: 0x040001AF RID: 431
		private const int Type_Normal = 0;

		// Token: 0x040001B0 RID: 432
		private const int Type_Pressure = 1;

		// Token: 0x040001B1 RID: 433
		private const int Type_Booster = 2;

		// Token: 0x040001B2 RID: 434
		private static int[] _leftSideConnection;

		// Token: 0x040001B3 RID: 435
		private static int[] _rightSideConnection;

		// Token: 0x040001B4 RID: 436
		private static int[] _trackType;

		// Token: 0x040001B5 RID: 437
		private static bool[] _boostLeft;

		// Token: 0x040001B6 RID: 438
		private static Vector2[] _texturePosition;

		// Token: 0x040001B7 RID: 439
		private static short _firstPressureFrame;

		// Token: 0x040001B8 RID: 440
		private static short _firstLeftBoostFrame;

		// Token: 0x040001B9 RID: 441
		private static short _firstRightBoostFrame;

		// Token: 0x040001BA RID: 442
		private static int[][] _trackSwitchOptions;

		// Token: 0x040001BB RID: 443
		private static int[][] _tileHeight;

		// Token: 0x020005F8 RID: 1528
		private enum TrackState
		{
			// Token: 0x04006393 RID: 25491
			NoTrack = -1,
			// Token: 0x04006394 RID: 25492
			AboveTrack,
			// Token: 0x04006395 RID: 25493
			OnTrack,
			// Token: 0x04006396 RID: 25494
			BelowTrack,
			// Token: 0x04006397 RID: 25495
			AboveFront,
			// Token: 0x04006398 RID: 25496
			AboveBack,
			// Token: 0x04006399 RID: 25497
			OnFront,
			// Token: 0x0400639A RID: 25498
			OnBack
		}

		// Token: 0x020005F9 RID: 1529
		public struct Customization
		{
			// Token: 0x170004C3 RID: 1219
			// (get) Token: 0x06003B96 RID: 15254 RVA: 0x00659C44 File Offset: 0x00657E44
			public static Minecart.Customization Default
			{
				get
				{
					return new Minecart.Customization
					{
						MinecartTextureWidth = 50f,
						MagnetOffset = new Vector2(25f, 26f),
						WheelOffset = new Vector2(12f, 0f)
					};
				}
			}

			// Token: 0x0400639B RID: 25499
			public float MinecartTextureWidth;

			// Token: 0x0400639C RID: 25500
			public Vector2 WheelOffset;

			// Token: 0x0400639D RID: 25501
			public Vector2 MagnetOffset;
		}
	}
}
