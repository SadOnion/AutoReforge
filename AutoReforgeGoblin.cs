using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoReroll
{
    class AutoReforgeGoblin : GlobalNPC
    {
        /*
        public override bool PreChatButtonClicked(NPC npc, bool firstButton)
        {
            if(npc.type == NPCID.GoblinTinkerer && firstButton==false)
            {
                GadgetBox.GadgetMethods.CloseVanillaUIs(Main.LocalPlayer);
                Main.playerInventory=true;
                AutoReroll.Instance.isInReforgeMenu=true;
                return false;
            }
            return true;
        }
        */
    }
}
