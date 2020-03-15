using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GadgetBox
{
	public static class GadgetMethods
	{
		

		

		

		public static void CloseVanillaUIs(this Player player)
		{
			if (player.sign >= 0 || player.talkNPC >= 0)
			{
				player.talkNPC = -1;
				player.sign = -1;
				Main.npcChatCornerItem = 0;
				Main.editSign = false;
				Main.npcChatText = "";
				player.releaseMount = false;
			}
			if (Main.editChest)
			{
				Main.PlaySound(SoundID.MenuTick);
				Main.editChest = false;
				Main.npcChatText = "";
			}
			if (player.editedChestName)
			{
				NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
				player.editedChestName = false;
			}
			if (player.chest != -1)
			{
				player.chest = -1;
				player.flyingPigChest = -1;
				Recipe.FindRecipes();
			}
		}

		public static int ReforgePrice(this Item item, Player player = null)
		{
			if (item == null || item.IsAir)
			{
				return 0;
			}
			if (player == null)
			{
				player = Main.LocalPlayer;
			}
			int reforgePrice = item.value;
			bool canApplyDiscount = true;
			if (ItemLoader.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount))
			{
				if (canApplyDiscount && player.discount)
				{
					reforgePrice = (int)(reforgePrice * 0.8f);
				}
				reforgePrice /= 3;
			}
			return reforgePrice;
		}

		public static void Consume(this Item item, int amount = 1, bool checkConsumable = true)
		{
			if (checkConsumable && !item.consumable)
			{
				return;
			}

			for (int i = 0; i < amount; i++)
			{
				if (ItemLoader.ConsumeItem(item, Main.LocalPlayer) && (item.stack--) <= 0)
				{
					item.TurnToAir();
				}
			}
		}

		public static void PrefixItem(ref Item item, bool silent = false, bool reset = false)
		{
			bool favorited = item.favorited;
			int stack = item.stack;
			Item tempItem = new Item();
			tempItem.netDefaults(item.netID);
			tempItem = tempItem.CloneWithModdedDataFrom(item);
			if (!reset)
			{
				tempItem.Prefix(-2);
			}
			item = tempItem.Clone();
			item.Center = Main.LocalPlayer.Center;
			item.favorited = favorited;
			item.stack = stack;
			if (!reset)
			{
				ItemLoader.PostReforge(item);
			}
			ItemText.NewText(item, item.stack, true, false);
			if (silent)
			{
				return;
			}
			Main.PlaySound(reset ? new LegacySoundStyle(SoundID.Grab, 1) : SoundID.Item37);
		}

		public static bool CanApplyPrefix(this Item item, byte prefix)
		{
			if (prefix == 0 || item.type == 0)
			{
				return false;
			}
			byte choise = prefix;
			float damageMult = 1f;
			float knockBackMult = 1f;
			float useTimeMult = 1f;
			float scaleMult = 1f;
			float shootSpeedMult = 1f;
			float manaMult = 1f;
			int critBonus = 0;
			bool invalid = true;

			ModPrefix modPrefix = ModPrefix.GetPrefix(choise);

			if (modPrefix?.Category == PrefixCategory.Custom)
			{
				invalid = !modPrefix.CanRoll(item);
			}
			else if (item.type == 1 || item.type == 4 || item.type == 6 || item.type == 7 || item.type == 10 || item.type == 24 || item.type == 45 || item.type == 46 || item.type == 65 || item.type == 103 || item.type == 104 || item.type == 121 || item.type == 122 || item.type == 155 || item.type == 190 || item.type == 196 || item.type == 198 || item.type == 199 || item.type == 200 || item.type == 201 || item.type == 202 || item.type == 203 || item.type == 204 || item.type == 213 || item.type == 217 || item.type == 273 || item.type == 367 || item.type == 368 || item.type == 426 || item.type == 482 || item.type == 483 || item.type == 484 || item.type == 653 || item.type == 654 || item.type == 656 || item.type == 657 || item.type == 659 || item.type == 660 || item.type == 671 || item.type == 672 || item.type == 674 || item.type == 675 || item.type == 676 || item.type == 723 || item.type == 724 || item.type == 757 || item.type == 776 || item.type == 777 || item.type == 778 || item.type == 787 || item.type == 795 || item.type == 797 || item.type == 798 || item.type == 799 || item.type == 881 || item.type == 882 || item.type == 921 || item.type == 922 || item.type == 989 || item.type == 990 || item.type == 991 || item.type == 992 || item.type == 993 || item.type == 1123 || item.type == 1166 || item.type == 1185 || item.type == 1188 || item.type == 1192 || item.type == 1195 || item.type == 1199 || item.type == 1202 || item.type == 1222 || item.type == 1223 || item.type == 1224 || item.type == 1226 || item.type == 1227 || item.type == 1230 || item.type == 1233 || item.type == 1234 || item.type == 1294 || item.type == 1304 || item.type == 1305 || item.type == 1306 || item.type == 1320 || item.type == 1327 || item.type == 1506 || item.type == 1507 || item.type == 1786 || item.type == 1826 || item.type == 1827 || item.type == 1909 || item.type == 1917 || item.type == 1928 || item.type == 2176 || item.type == 2273 || item.type == 2608 || item.type == 2341 || item.type == 2330 || item.type == 2320 || item.type == 2516 || item.type == 2517 || item.type == 2746 || item.type == 2745 || item.type == 3063 || item.type == 3018 || item.type == 3211 || item.type == 3013 || item.type == 3258 || item.type == 3106 || item.type == 3065 || item.type == 2880 || item.type == 3481 || item.type == 3482 || item.type == 3483 || item.type == 3484 || item.type == 3485 || item.type == 3487 || item.type == 3488 || item.type == 3489 || item.type == 3490 || item.type == 3491 || item.type == 3493 || item.type == 3494 || item.type == 3495 || item.type == 3496 || item.type == 3497 || item.type == 3498 || item.type == 3500 || item.type == 3501 || item.type == 3502 || item.type == 3503 || item.type == 3504 || item.type == 3505 || item.type == 3506 || item.type == 3507 || item.type == 3508 || item.type == 3509 || item.type == 3511 || item.type == 3512 || item.type == 3513 || item.type == 3514 || item.type == 3515 || item.type == 3517 || item.type == 3518 || item.type == 3519 || item.type == 3520 || item.type == 3521 || item.type == 3522 || item.type == 3523 || item.type == 3524 || item.type == 3525 || (item.type >= 3462 && item.type <= 3466) || (item.type >= 2772 && item.type <= 2786) || (item.type == 3349 || item.type == 3352 || item.type == 3351 || (item.type >= 3764 && item.type <= 3769)) || item.type == 3772 || item.type == 3823 || item.type == 3827 || MeleePrefix(item))
			{
				byte[] valid = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 36, 37, 38, 53, 54, 55, 39, 40, 56, 41, 57, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 59, 60, 61, 81 };

				if (valid.Contains(prefix))
				{
					invalid = false;
				}
				else if (modPrefix != null && (modPrefix.Category == PrefixCategory.AnyWeapon || modPrefix.Category == PrefixCategory.Melee))
				{
					invalid = !modPrefix.CanRoll(item);
				}
			}
			else if (item.type == 162 || item.type == 160 || item.type == 163 || item.type == 220 || item.type == 274 || item.type == 277 || item.type == 280 || item.type == 383 || item.type == 384 || item.type == 385 || item.type == 386 || item.type == 387 || item.type == 388 || item.type == 389 || item.type == 390 || item.type == 406 || item.type == 537 || item.type == 550 || item.type == 579 || item.type == 756 || item.type == 759 || item.type == 801 || item.type == 802 || item.type == 1186 || item.type == 1189 || item.type == 1190 || item.type == 1193 || item.type == 1196 || item.type == 1197 || item.type == 1200 || item.type == 1203 || item.type == 1204 || item.type == 1228 || item.type == 1231 || item.type == 1232 || item.type == 1259 || item.type == 1262 || item.type == 1297 || item.type == 1314 || item.type == 1325 || item.type == 1947 || item.type == 2332 || item.type == 2331 || item.type == 2342 || item.type == 2424 || item.type == 2611 || item.type == 2798 || item.type == 3012 || item.type == 3473 || item.type == 3098 || item.type == 3368 || item.type == 3835 || item.type == 3836 || item.type == 3858 || WeaponPrefix(item))
			{
				int[] valid = new int[] { 36, 37, 38, 53, 54, 55, 39, 40, 56, 41, 57, 59, 60, 61 };
				if (valid.Contains(prefix))
				{
					invalid = false;
				}
				else if (modPrefix?.Category == PrefixCategory.AnyWeapon)
				{
					invalid = !modPrefix.CanRoll(item);
				}
			}
			else if (item.type == 39 || item.type == 44 || item.type == 95 || item.type == 96 || item.type == 98 || item.type == 99 || item.type == 120 || item.type == 164 || item.type == 197 || item.type == 219 || item.type == 266 || item.type == 281 || item.type == 434 || item.type == 435 || item.type == 436 || item.type == 481 || item.type == 506 || item.type == 533 || item.type == 534 || item.type == 578 || item.type == 655 || item.type == 658 || item.type == 661 || item.type == 679 || item.type == 682 || item.type == 725 || item.type == 758 || item.type == 759 || item.type == 760 || item.type == 796 || item.type == 800 || item.type == 905 || item.type == 923 || item.type == 964 || item.type == 986 || item.type == 1156 || item.type == 1187 || item.type == 1194 || item.type == 1201 || item.type == 1229 || item.type == 1254 || item.type == 1255 || item.type == 1258 || item.type == 1265 || item.type == 1319 || item.type == 1553 || item.type == 1782 || item.type == 1784 || item.type == 1835 || item.type == 1870 || item.type == 1910 || item.type == 1929 || item.type == 1946 || item.type == 2223 || item.type == 2269 || item.type == 2270 || item.type == 2624 || item.type == 2515 || item.type == 2747 || item.type == 2796 || item.type == 2797 || item.type == 3052 || item.type == 2888 || item.type == 3019 || item.type == 3029 || item.type == 3007 || item.type == 3008 || item.type == 3210 || item.type == 3107 || item.type == 3245 || item.type == 3475 || item.type == 3540 || item.type == 3854 || item.type == 3859 || item.type == 3821 || item.type == 3480 || item.type == 3486 || item.type == 3492 || item.type == 3498 || item.type == 3504 || item.type == 3510 || item.type == 3516 || item.type == 3350 || item.type == 3546 || item.type == 3788 || RangedPrefix(item))
			{
				int[] valid = new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 58, 36, 37, 38, 53, 54, 55, 39, 40, 56, 41, 57, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 59, 60, 61, 82 };
				if (valid.Contains(prefix))
				{
					invalid = false;
				}
				else if (modPrefix != null && (modPrefix.Category == PrefixCategory.AnyWeapon || modPrefix.Category == PrefixCategory.Ranged))
				{
					invalid = !modPrefix.CanRoll(item);
				}
			}
			else if (item.type == 64 || item.type == 112 || item.type == 113 || item.type == 127 || item.type == 157 || item.type == 165 || item.type == 218 || item.type == 272 || item.type == 494 || item.type == 495 || item.type == 496 || item.type == 514 || item.type == 517 || item.type == 518 || item.type == 519 || item.type == 683 || item.type == 726 || item.type == 739 || item.type == 740 || item.type == 741 || item.type == 742 || item.type == 743 || item.type == 744 || item.type == 788 || item.type == 1121 || item.type == 1155 || item.type == 1157 || item.type == 1178 || item.type == 1244 || item.type == 1256 || item.type == 1260 || item.type == 1264 || item.type == 1266 || item.type == 1295 || item.type == 1296 || item.type == 1308 || item.type == 1309 || item.type == 1313 || item.type == 1336 || item.type == 1444 || item.type == 1445 || item.type == 1446 || item.type == 1572 || item.type == 1801 || item.type == 1802 || item.type == 1930 || item.type == 1931 || item.type == 2188 || item.type == 2622 || item.type == 2621 || item.type == 2584 || item.type == 2551 || item.type == 2366 || item.type == 2535 || item.type == 2365 || item.type == 2364 || item.type == 2623 || item.type == 2750 || item.type == 2795 || item.type == 3053 || item.type == 3051 || item.type == 3209 || item.type == 3014 || item.type == 3105 || item.type == 2882 || item.type == 3269 || item.type == 3006 || item.type == 3377 || item.type == 3069 || item.type == 2749 || item.type == 3249 || item.type == 3476 || item.type == 3474 || item.type == 3531 || item.type == 3541 || item.type == 3542 || item.type == 3569 || item.type == 3570 || item.type == 3571 || item.type == 3779 || item.type == 3787 || item.type == 3531 || item.type == 3852 || item.type == 3870 || item.type == 3824 || item.type == 3818 || item.type == 3829 || item.type == 3832 || item.type == 3825 || item.type == 3819 || item.type == 3830 || item.type == 3833 || item.type == 3826 || item.type == 3820 || item.type == 3831 || item.type == 3834 || MagicPrefix(item))
			{
				int[] valid = new int[] { 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 52, 36, 37, 38, 53, 54, 55, 39, 40, 56, 41, 57, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 59, 60, 61, 83 };
				if (valid.Contains(prefix))
				{
					invalid = false;
				}
				else if (modPrefix != null && (modPrefix.Category == PrefixCategory.AnyWeapon || modPrefix.Category == PrefixCategory.Magic))
				{
					invalid = !modPrefix.CanRoll(item);
				}
			}
			else if (item.type == 55 || item.type == 119 || item.type == 191 || item.type == 284 || item.type == 670 || item.type == 1122 || item.type == 1513 || item.type == 1569 || item.type == 1571 || item.type == 1825 || item.type == 1918 || item.type == 3054 || item.type == 3262 || (item.type >= 3278 && item.type <= 3292) || (item.type >= 3315 && item.type <= 3317) || item.type == 3389 || item.type == 3030 || item.type == 3543 || WeaponPrefix(item))
			{
				int[] valid = new int[] { 36, 37, 38, 53, 54, 55, 39, 40, 56, 41, 57, 59, 60, 61 };
				if (valid.Contains(prefix))
				{
					invalid = false;
				}
				else if (modPrefix?.Category == PrefixCategory.AnyWeapon)
				{
					invalid = !modPrefix.CanRoll(item);
				}
			}
			else
			{
				if (!item.accessory || item.type == 267 || item.type == 562 || item.type == 563 || item.type == 564 || item.type == 565 || item.type == 566 || item.type == 567 || item.type == 568 || item.type == 569 || item.type == 570 || item.type == 571 || item.type == 572 || item.type == 573 || item.type == 574 || item.type == 576 || item.type == 1307 || (item.type >= 1596 && item.type < 1610) || item.vanity)
				{
					return false;
				}
				int[] valid = new int[] { 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80 };
				if (valid.Contains(prefix))
				{
					invalid = false;
				}
				else if (modPrefix?.Category == PrefixCategory.Accessory)
				{
					invalid = !modPrefix.CanRoll(item);
				}
			}
			if (invalid)
			{
				return false;
			}
			switch (choise)
			{
				case 1:
					scaleMult = 1.12f;
					break;
				case 2:
					scaleMult = 1.18f;
					break;
				case 3:
					damageMult = 1.05f;
					critBonus = 2;
					scaleMult = 1.05f;
					break;
				case 4:
					damageMult = 1.1f;
					scaleMult = 1.1f;
					knockBackMult = 1.1f;
					break;
				case 5:
					damageMult = 1.15f;
					break;
				case 6:
					damageMult = 1.1f;
					break;
				case 7:
					scaleMult = 0.82f;
					break;
				case 8:
					knockBackMult = 0.85f;
					damageMult = 0.85f;
					scaleMult = 0.87f;
					break;
				case 9:
					scaleMult = 0.9f;
					break;
				case 10:
					damageMult = 0.85f;
					break;
				case 11:
					useTimeMult = 1.1f;
					knockBackMult = 0.9f;
					scaleMult = 0.9f;
					break;
				case 12:
					knockBackMult = 1.1f;
					damageMult = 1.05f;
					scaleMult = 1.1f;
					useTimeMult = 1.15f;
					break;
				case 13:
					knockBackMult = 0.8f;
					damageMult = 0.9f;
					scaleMult = 1.1f;
					break;
				case 14:
					knockBackMult = 1.15f;
					useTimeMult = 1.1f;
					break;
				case 15:
					knockBackMult = 0.9f;
					useTimeMult = 0.85f;
					break;
				case 16:
					damageMult = 1.1f;
					critBonus = 3;
					break;
				case 17:
					useTimeMult = 0.85f;
					shootSpeedMult = 1.1f;
					break;
				case 18:
					useTimeMult = 0.9f;
					shootSpeedMult = 1.15f;
					break;
				case 19:
					knockBackMult = 1.15f;
					shootSpeedMult = 1.05f;
					break;
				case 20:
					knockBackMult = 1.05f;
					shootSpeedMult = 1.05f;
					damageMult = 1.1f;
					useTimeMult = 0.95f;
					critBonus = 2;
					break;
				case 21:
					knockBackMult = 1.15f;
					damageMult = 1.1f;
					break;
				case 22:
					knockBackMult = 0.9f;
					shootSpeedMult = 0.9f;
					damageMult = 0.85f;
					break;
				case 23:
					useTimeMult = 1.15f;
					shootSpeedMult = 0.9f;
					break;
				case 24:
					useTimeMult = 1.1f;
					knockBackMult = 0.8f;
					break;
				case 25:
					useTimeMult = 1.1f;
					damageMult = 1.15f;
					critBonus = 1;
					break;
				case 26:
					manaMult = 0.85f;
					damageMult = 1.1f;
					break;
				case 27:
					manaMult = 0.85f;
					break;
				case 28:
					manaMult = 0.85f;
					damageMult = 1.15f;
					knockBackMult = 1.05f;
					break;
				case 29:
					manaMult = 1.1f;
					break;
				case 30:
					manaMult = 1.2f;
					damageMult = 0.9f;
					break;
				case 31:
					knockBackMult = 0.9f;
					damageMult = 0.9f;
					break;
				case 32:
					manaMult = 1.15f;
					damageMult = 1.1f;
					break;
				case 33:
					manaMult = 1.1f;
					knockBackMult = 1.1f;
					useTimeMult = 0.9f;
					break;
				case 34:
					manaMult = 0.9f;
					knockBackMult = 1.1f;
					useTimeMult = 1.1f;
					damageMult = 1.1f;
					break;
				case 35:
					manaMult = 1.2f;
					damageMult = 1.15f;
					knockBackMult = 1.15f;
					break;
				case 36:
					critBonus = 3;
					break;
				case 37:
					damageMult = 1.1f;
					critBonus = 3;
					knockBackMult = 1.1f;
					break;
				case 38:
					knockBackMult = 1.15f;
					break;
				case 39:
					damageMult = 0.7f;
					knockBackMult = 0.8f;
					break;
				case 40:
					damageMult = 0.85f;
					break;
				case 41:
					knockBackMult = 0.85f;
					damageMult = 0.9f;
					break;
				case 42:
					useTimeMult = 0.9f;
					break;
				case 43:
					damageMult = 1.1f;
					useTimeMult = 0.9f;
					break;
				case 44:
					useTimeMult = 0.9f;
					critBonus = 3;
					break;
				case 45:
					useTimeMult = 0.95f;
					break;
				case 46:
					critBonus = 3;
					useTimeMult = 0.94f;
					damageMult = 1.07f;
					break;
				case 47:
					useTimeMult = 1.15f;
					break;
				case 48:
					useTimeMult = 1.2f;
					break;
				case 49:
					useTimeMult = 1.08f;
					break;
				case 50:
					damageMult = 0.8f;
					useTimeMult = 1.15f;
					break;
				case 51:
					knockBackMult = 0.9f;
					useTimeMult = 0.9f;
					damageMult = 1.05f;
					critBonus = 2;
					break;
				case 52:
					manaMult = 0.9f;
					damageMult = 0.9f;
					useTimeMult = 0.9f;
					break;
				case 53:
					damageMult = 1.1f;
					break;
				case 54:
					knockBackMult = 1.15f;
					break;
				case 55:
					knockBackMult = 1.15f;
					damageMult = 1.05f;
					break;
				case 56:
					knockBackMult = 0.8f;
					break;
				case 57:
					knockBackMult = 0.9f;
					damageMult = 1.18f;
					break;
				case 58:
					useTimeMult = 0.85f;
					damageMult = 0.85f;
					break;
				case 59:
					knockBackMult = 1.15f;
					damageMult = 1.15f;
					critBonus = 5;
					break;
				case 60:
					damageMult = 1.15f;
					critBonus = 5;
					break;
				case 61:
					critBonus = 5;
					break;
				case 81:
					knockBackMult = 1.15f;
					damageMult = 1.15f;
					critBonus = 5;
					useTimeMult = 0.9f;
					scaleMult = 1.1f;
					break;
				case 82:
					knockBackMult = 1.15f;
					damageMult = 1.15f;
					critBonus = 5;
					useTimeMult = 0.9f;
					shootSpeedMult = 1.1f;
					break;
				case 83:
					knockBackMult = 1.15f;
					damageMult = 1.15f;
					critBonus = 5;
					useTimeMult = 0.9f;
					manaMult = 0.9f;
					break;
				default:
					if (choise >= PrefixID.Count)
					{
						modPrefix?.SetStats(ref damageMult, ref knockBackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
					}
					break;
			}
			if (damageMult != 1f && Math.Round(item.damage * damageMult) == item.damage)
			{
				invalid = true;
			}
			if (useTimeMult != 1f && Math.Round(item.useAnimation * useTimeMult) == item.useAnimation)
			{
				invalid = true;
			}
			if (manaMult != 1f && Math.Round(item.mana * manaMult) == item.mana)
			{
				invalid = true;
			}
			if (knockBackMult != 1f && item.knockBack == 0f)
			{
				invalid = true;
			}
			if (choise >= PrefixID.Count)
			{
				modPrefix?.ValidateItem(item, ref invalid);
			}
			return !invalid;
		}

		private static bool GeneralPrefix(Item item) => item.maxStack == 1 && item.damage > 0 && item.ammo == 0 && !item.accessory;
		private static bool WeaponPrefix(Item item) => item.modItem != null && GeneralPrefix(item) && item.melee && item.noUseGraphic;
		private static bool MeleePrefix(Item item) => item.modItem != null && GeneralPrefix(item) && item.melee && !item.noUseGraphic;
		private static bool RangedPrefix(Item item) => item.modItem != null && GeneralPrefix(item) && (item.ranged || item.thrown);
		private static bool MagicPrefix(Item item) => item.modItem != null && GeneralPrefix(item) && (item.magic || item.summon);

		

		

		

		

		}
}