
using GadgetBox;
using GadgetBox.GadgetUI;
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
		public static AutoReroll Instance;
		public UserInterface userInterface;

		public static Mod Thorium {get;private set;}
		public static Mod Calamity {get;private set;}
		public  static int ForgePerSec=10;
		
		public bool isInReforgeMenu;
		public bool ReforgeMenu;
		private int lastSeenScreenWidth;
		private int lastSeenScreenHeight;
		private bool lastFocus;

		public override void Load()
		{
			Instance = this;
			if (!Main.dedServ) {
			userInterface = new UserInterface();
				Thorium = ModLoader.GetMod("ThoriumMod");
				Calamity = ModLoader.GetMod("CalamityMod");
			}
		}
		public override void PostSetupContent()
		{
			ModCompat.Load();
		}
		public override void Unload()
		{
			ModCompat.Unload();
			Instance=null;
			Thorium = null;
			Calamity=null;

		}
		public override void UpdateUI(GameTime gameTime)
		{

			if (ReforgeMenu && isInReforgeMenu==false)
			{
				userInterface.SetState(new ReforgeMachineUI());
				isInReforgeMenu = true;
			}
			if(userInterface != null)
			{
				userInterface.Update(gameTime);
			}
			
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			 if (mouseTextIndex != -1) {
				
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
    				"BestModifierRoll: MyInterface",
    				delegate
   	 				{		
    				if (Main.playerInventory && !Main.recBigList)
						{
							if (lastSeenScreenWidth != Main.screenWidth || lastSeenScreenHeight != Main.screenHeight || !lastFocus && Main.hasFocus)
							{	
								userInterface.Recalculate();
								lastSeenScreenWidth = Main.screenWidth;
								lastSeenScreenHeight = Main.screenHeight;
							}
							
							userInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
						}
    			
   				return true;
    	},
   		InterfaceScaleType.UI));
  }
	}
		
	}
	
	
}