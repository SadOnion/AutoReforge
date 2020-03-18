using GadgetBox.GadgetUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;

namespace AutoReroll
{
    class UIMoneyPanel : UIPanel
    {
        public bool Visible {get;internal set;}
        Color textColor = Color.White;
        string text = "Leave me";
        public UICounterButton[] CounterButtons { get;private set;} = new UICounterButton[4];
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if(!Visible)
            {
                
                return;
            }
            base.DrawSelf(spriteBatch);
            
        }
        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            if(!Visible)
            {
                
                return;
            }
            
            Utils.DrawBorderString(spriteBatch,text,GetInnerDimensions().Position()-Main.fontMouseText.MeasureString(text).X*Vector2.UnitX,Colors.AlphaDarken(textColor),.85f);
            base.DrawChildren(spriteBatch);
        }
        public int GetMoneyValue()
        {
            return CounterButtons[0].Counter*Item.copper+CounterButtons[1].Counter*Item.silver
                +CounterButtons[2].Counter*Item.gold+CounterButtons[3].Counter*Item.platinum;
        }
        public override void OnInitialize()
        {
            
            MinWidth.Pixels = 150;
			MinHeight.Pixels = 40;
            Width.Pixels = 150;
			Height.Pixels = 40;
           int padding=25;
            UICounterButton platinumButton = new UICounterButton(ModContent.GetTexture(AutoReroll.modName+"/Images/Platinum_Coin"));
            platinumButton.Left.Pixels=25;
            UICounterButton goldButton = new UICounterButton(ModContent.GetTexture(AutoReroll.modName+"/Images/Gold_Coin"));
            goldButton.Left.Pixels=platinumButton.Left.Pixels+padding;
            UICounterButton silverButton = new UICounterButton(ModContent.GetTexture(AutoReroll.modName+"/Images/Silver_Coin"));
            silverButton.Left.Pixels=goldButton.Left.Pixels+padding;
            
            UICounterButton copperButton = new UICounterButton(ModContent.GetTexture(AutoReroll.modName+"/Images/Copper_Coin"));
            copperButton.Left.Pixels=silverButton.Left.Pixels+padding;
            
            CounterButtons[3] = platinumButton;
            CounterButtons[2] = goldButton;
            CounterButtons[1] = silverButton;
            CounterButtons[0] = copperButton;

            Append(platinumButton);
            Append(goldButton);
            Append(silverButton);
            Append(copperButton);
        }
    }
    class UICounterButton : UIImageButton
    {
        public int Counter {get; private set;}
        private UIText counterText;
        public UICounterButton(Texture2D texture) : base(texture)
        {
            OnScrollWheel += (a,b) => ChangeCounter(a.ScrollWheelValue);
            counterText = new UIText(Counter.ToString(),.75f);
            counterText.Top.Pixels=5;
            Append(counterText);
        }
       protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (IsMouseHovering)
			{
				Main.hoverItemName = "Use mouse wheel to change value";
			}
            base.DrawSelf(spriteBatch);
		}
        private void ChangeCounter(int value)
        {
            Counter+= value > 0?1:-1;
            if(Counter < 0) Counter =99;
            if(Counter > 99) Counter = 0;
            counterText.SetText(Counter.ToString());
        }
    }
}
