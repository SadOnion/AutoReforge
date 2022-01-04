using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace GadgetBox.GadgetUI
{
    internal class UIReforgeLabel : UILeftAlignedLabel
    {
        internal bool recommended;
        internal bool selected;
        internal Item shownItem;

        private static Dictionary<int, Color> rarityColors = new Dictionary<int, Color>()
        {
            [-11] = Colors.RarityAmber,
            [-1] = Colors.RarityTrash,
            [0] = Colors.RarityNormal,
            [1] = Colors.RarityBlue,
            [2] = Colors.RarityGreen,
            [3] = Colors.RarityOrange,
            [4] = Colors.RarityRed,
            [5] = Colors.RarityPink,
            [6] = Colors.RarityPurple,
            [7] = Colors.RarityLime,
            [8] = Colors.RarityYellow,
            [9] = Colors.RarityCyan,
            [10] = new Color(255, 40, 100),
            [11] = new Color(180, 40, 255)
        };

        public UIReforgeLabel(Item shownItem) :
            base(Lang.prefix[shownItem.prefix].Value, Color.White)
        {
            this.shownItem = shownItem;

            if (Text.StartsWith("("))
            {
                Text = Text.Split('(', ')')[1];
            }
        }

        public override int CompareTo(object obj)
        {
            UIReforgeLabel other = obj as UIReforgeLabel;
            int recommendedComp = -recommended.CompareTo(other.recommended);
            int diffSelected = -selected.CompareTo(other.selected);
            int diffValue = -shownItem.value.CompareTo(other.shownItem.value);
            return diffSelected != 0 ? diffSelected : recommendedComp != 0 ? recommendedComp : diffValue != 0 ? diffValue : shownItem.prefix.CompareTo(other.shownItem.prefix);
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            Main.HoverItem = shownItem.Clone();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            BackgroundColor = selected ? Color.LightSkyBlue : Color.CornflowerBlue;
            if (shownItem.expert || shownItem.rare == -12)
            {
                TextColor = Main.DiscoColor;
            }
            else if (rarityColors.ContainsKey(shownItem.rare))
            {
                TextColor = rarityColors[shownItem.rare];
            }
            else
            {
                TextColor = Color.White;
            }

            base.DrawSelf(spriteBatch);

            if (GetDimensions().ToRectangle().Contains(Main.mouseX, Main.mouseY))
            {
                Main.HoverItem = shownItem.Clone();
                Main.hoverItemName = Main.HoverItem.Name;
            }
        }
    }
}