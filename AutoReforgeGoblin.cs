﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace AutoReroll
{
    class AutoReforgeGoblin : GlobalNPC
    {

        public override bool PreChatButtonClicked(NPC npc, bool firstButton)
        {
            if (npc.type == NPCID.GoblinTinkerer && firstButton == false && !AutoReroll.UseDefaultReforgeMenu)
            {
                Main.npcChatText = "";
                AutoReroll.Instance.ReforgeMenu = true;
                return false;
            }
            return true;
        }


    }
}
