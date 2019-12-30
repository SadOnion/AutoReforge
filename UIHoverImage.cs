using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace BestModifierRoll
{
    public class UIHoverImage : UIImage
	{
		private string HoverText;

		public UIHoverImage(Texture2D texture, string hoverText) : base(texture) {
			HoverText = hoverText;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);

			if (IsMouseHovering) {
				Main.hoverItemName = HoverText;
			}
		}
		public void SetHoverText(string newText)
		{
			HoverText = newText;
		}
	}
}
