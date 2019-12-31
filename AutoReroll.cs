
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Timers;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace AutoReroll
{
	public class AutoReroll : Mod
	{
		public const string modName = "AutoReroll";
		internal UserInterface userInterface;
		private GameTime lastUpdateUiGameTime;
		internal GoblinUI myUi;
		public static Mod Thorium {get;private set;}
		public static Mod Calamity {get;private set;}
		public static int ForgePerSec=10;
		public AutoReroll()
		{
		}
		bool buttonActive;
		
		public override void Load()
		{
			if (!Main.dedServ) {
			userInterface = new UserInterface();
    
				myUi = new GoblinUI();
				myUi.Activate(); // Activate calls Initialize() on the UIState if not initialized, then calls OnActivate and then calls Activate on every child element
				Thorium = ModLoader.GetMod("ThoriumMod");
				Calamity = ModLoader.GetMod("CalamityMod");
			}
		}
		public override void UpdateUI(GameTime gameTime)
		{
			lastUpdateUiGameTime = gameTime;
			
			if (userInterface?.CurrentState != null) 
			{
  				userInterface.Update(gameTime);
				if(!Main.InReforgeMenu){userInterface.SetState(null); buttonActive=false;}
				else
				{
					
					if(Main.reforgeItem.type == 0 && buttonActive==true)
						{
						userInterface.SetState(null);
						buttonActive = false;
						}
				}
			}
			else
			{
				if(Main.reforgeItem.type != 0 && buttonActive == false)
					{
						userInterface.SetState(myUi);
						buttonActive=true;
					}
			}
			
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			 if (mouseTextIndex != -1) {
				
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
    				"BestModifierRoll: MyInterface",
    				delegate
   	 				{		
    				if ( lastUpdateUiGameTime != null && userInterface?.CurrentState != null) {
    				userInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);
    			}
   				return true;
    	},
   		InterfaceScaleType.UI));
  }
}
	}
	
	
}