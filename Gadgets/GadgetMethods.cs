using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GadgetBox
{
    public static class GadgetMethods
    {
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
            PopupText.NewText(PopupTextContext.ItemReforge, item, item.stack, true, false);
            if (silent)
            {
                return;
            }
            SoundEngine.PlaySound(reset ? new LegacySoundStyle(SoundID.Grab, 1) : SoundID.Item37);
        }
    }
}