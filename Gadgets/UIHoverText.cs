using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace GadgetBox.GadgetUI
{
	internal class UIHoverText : UIElement
	{
		public string HoverText { get; internal set; }

		public UIHoverText()
		{
			HoverText = "";
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (!string.IsNullOrEmpty(HoverText) && IsMouseHovering)
			{
				Main.hoverItemName = HoverText;
			}
		}
	}
}