using Terraria;

namespace AutoReroll
{
    class PrefixUtils
    {
        public static bool GeneralPrefix(Item item)
		{
			if (item.maxStack == 1 && item.damage > 0 && item.ammo == 0)
			{
				return !item.accessory;
			}
			return false;
		}
		public static bool MeleePrefix(Item item)
		{
			if (GeneralPrefix(item) && item.melee)
			{
				return !item.noUseGraphic;
			}
			return false;
		}
		public static bool ModMeleePrefix(Item item)
		{
			if (item.modItem != null && GeneralPrefix(item) && item.melee)
			{
				return !item.noUseGraphic;
			}
			return false;
		}
		public static bool WeaponPrefix(Item item)
		{
			if (GeneralPrefix(item) && item.melee)
			{
				return item.noUseGraphic;
			}
			return false;
		}
		public static bool ModWeaponPrefix(Item item)
		{
			if (GeneralPrefix(item) && item.melee)
			{
				return item.noUseGraphic;
			}
			return false;
		}
		public static bool RangedPrefix(Item item)
		{
			if (GeneralPrefix(item))
			{
				if (!item.ranged)
				{
					return item.thrown;
				}
				return true;
			}
			return false;
		}
		public static bool ModRangedPrefix(Item item)
		{
			if (item.modItem != null && GeneralPrefix(item))
			{
				if (!item.ranged)
				{
					return item.thrown;
				}
				return true;
			}
			return false;
		}
		public static bool MagicPrefix(Item item)
		{
			if (GeneralPrefix(item))
			{
				return item.magic;
			}
			return false;
		}
		public static bool ModMagicPrefix(Item item)
		{
			if (item.modItem != null && GeneralPrefix(item))
			{
				return item.magic;
			}
			return false;
		}
		public static bool ModSummonPrefix(Item item)
		{
			if (item.modItem != null && GeneralPrefix(item))
			{
				return item.summon;
			}
			return false;
		}
		public static bool SummonPrefix(Item item)
		{
			if (GeneralPrefix(item))
			{
				return item.summon;
			}
			return false;
		}
    }
}
