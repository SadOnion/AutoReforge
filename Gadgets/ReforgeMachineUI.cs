using AutoReroll;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
namespace GadgetBox.GadgetUI
{
    internal class ReforgeMachineUI : UIState
    {
        internal UIPanel reforgePanel;
        internal UIItemSlot reforgeSlot;
        internal UIFancyButton reforgeButton;
        internal UIPanel reforgeListPanel;
        internal UIList reforgeList;
        internal UIMoneyPanel moneyPanel;
        private List<int> selectedPrefixes = new List<int>();
        private int reforgePrice;
        private bool autoReforge;
        private double tickCounter;
        private double silenceCounter;
        private int lastItem = ItemID.None;
        private HashSet<int> allowedPrefixes = new HashSet<int>();
        public override void OnInitialize()
        {
            Main.recBigList = false;
            Main.playerInventory = true;
            Main.hidePlayerCraftingMenu = true;

            reforgePanel = new UIReforgePanel(() => reforgeSlot.item, () => reforgePrice);
            reforgePanel.SetPadding(4);
            reforgePanel.Top.Pixels = Main.instance.invBottom + 10;
            reforgePanel.Left.Pixels = 65;
            reforgePanel.MinHeight.Pixels = 300;

            reforgeSlot = new UIItemSlot(0.85f);
            reforgeSlot.Top.Pixels = reforgeSlot.Left.Pixels = 12;
            reforgeSlot.CanClick += () => Main.mouseItem.type == ItemID.None || Main.mouseItem.Prefix(-3);
            reforgeSlot.OnMouseDown += (a, b) => { selectedPrefixes.Clear(); OnItemChanged(); };
            reforgePanel.Append(reforgeSlot);

            moneyPanel = new UIMoneyPanel();
            moneyPanel.Left.Pixels = 170;
            moneyPanel.Top.Pixels = 45;
            moneyPanel.BackgroundColor = Color.Transparent;
            moneyPanel.BorderColor = Color.Transparent;
            moneyPanel.Visible = false;
            reforgePanel.Append(moneyPanel);

            reforgeButton = new UIFancyButton(TextureAssets.Reforge[0].Value, TextureAssets.Reforge[1].Value);
            reforgeButton.Top.Pixels = 20;
            reforgeButton.Left.Pixels = 64;
            reforgeButton.CanClick += CanReforgeItem;
            reforgeButton.OnMouseDown += OnReforgeButtonClick;
            reforgeButton.HoverText = Language.GetTextValue("LegacyInterface.19");
            reforgePanel.Append(reforgeButton);

            reforgeListPanel = new UIPanel();
            reforgeListPanel.Top.Pixels = 80;
            reforgeListPanel.Left.Pixels = 12;
            reforgeListPanel.Width.Set(-24, 1);
            reforgeListPanel.Height.Set(-82, 1);
            reforgeListPanel.SetPadding(6);
            reforgeListPanel.BackgroundColor = Color.CadetBlue;
            reforgePanel.Append(reforgeListPanel);

            reforgeList = new UIList();
            reforgeList.Width.Precent = reforgeList.Height.Precent = 1f;
            reforgeList.Width.Pixels = -24;
            reforgeList.ListPadding = 2;
            reforgeListPanel.Append(reforgeList);

            var reforgeListScrollbar = new FixedUIScrollbar(AutoReroll.AutoReroll.Instance.userInterface);
            reforgeListScrollbar.SetView(100f, 1000f);
            reforgeListScrollbar.Top.Pixels = 4;
            reforgeListScrollbar.Height.Set(-8, 1f);
            reforgeListScrollbar.Left.Set(-20, 1f);
            reforgeListPanel.Append(reforgeListScrollbar);
            reforgeList.SetScrollbar(reforgeListScrollbar);


            Append(reforgePanel);
        }

        public override void OnDeactivate()
        {
            if (!reforgeSlot.item.IsAir)
            {
                Main.LocalPlayer.QuickSpawnClonedItem(reforgeSlot.item, reforgeSlot.item.stack);
                reforgeSlot.item.TurnToAir();
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            bool closeUI = false, silent = false;
            if (Main.LocalPlayer.talkNPC == -1)
            {
                AutoReroll.AutoReroll.Instance.ReforgeMenu = false;
            }
            if (!AutoReroll.AutoReroll.Instance.ReforgeMenu)
            {
                closeUI = true;
            }

            if (closeUI)
            {
                if (!silent)
                {
                    SoundEngine.PlaySound(SoundID.MenuClose);
                }
                AutoReroll.AutoReroll.Instance.userInterface.SetState(null);
                AutoReroll.AutoReroll.Instance.isInReforgeMenu = false;
                AutoReroll.AutoReroll.Instance.ReforgeMenu = false;
                return;
            }

            reforgePrice = reforgeSlot.item.ReforgePrice();
            if (autoReforge)
            {

                tickCounter += gameTime.ElapsedGameTime.TotalMilliseconds;

                silenceCounter += .001f * gameTime.ElapsedGameTime.TotalMilliseconds;

                reforgeButton.Rotation += 10 * .001f * gameTime.ElapsedGameTime.Milliseconds;
                if (selectedPrefixes.Count == 0 || selectedPrefixes.Contains(reforgeSlot.item.prefix) || !CanReforgeItem())
                {
                    autoReforge = false;
                    tickCounter = 0;
                }
                else if (tickCounter > 1000 / AutoReroll.AutoReroll.ForgePerSec)
                {

                    tickCounter = 0;
                    ReforgeItem(silenceCounter < .2f ? true : false);
                    if (silenceCounter > .2f) silenceCounter = 0;
                    if (selectedPrefixes.Contains(reforgeSlot.item.prefix))
                    {
                        autoReforge = false;
                        tickCounter = 0;
                    }
                }
            }
            else if (reforgeButton.Rotation != 0)
            {
                if (reforgeButton.Rotation > MathHelper.TwoPi)
                {
                    reforgeButton.Rotation %= MathHelper.TwoPi;
                }
                reforgeButton.Rotation = MathHelper.TwoPi - reforgeButton.Rotation <= 0.2f ? 0 : reforgeButton.Rotation + 0.2f;

            }
            else
            {
                silenceCounter = .2f;
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Main.hidePlayerCraftingMenu = true;
            if (reforgePanel.ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
                Main.HoverItem.TurnToAir();
                Main.hoverItemName = "";
            }
            reforgeButton.Visible = !reforgeSlot.item.IsAir;
            moneyPanel.Visible = !reforgeSlot.item.IsAir;
        }

        private void OnReforgeButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (autoReforge)
            {
                autoReforge = false;
                tickCounter = 0;
            }
            else if (selectedPrefixes.Count > 0)
            {
                autoReforge = true;
            }
            else
            {
                ReforgeItem(false);
            }
        }

        private bool CanReforgeItem() => !reforgeSlot.item.IsAir && !selectedPrefixes.Contains(reforgeSlot.item.prefix) &&
            Main.LocalPlayer.CanBuyItem(reforgePrice + moneyPanel.GetMoneyValue(), -1) && ItemLoader.PreReforge(reforgeSlot.item);

        private void OnItemChanged()
        {
            reforgeList.Clear();
            if (reforgeSlot.item.IsAir)
            {
                return;
            }
            if (lastItem != reforgeSlot.item.type)
            {
                UpdateAllowedPrefixes();
                lastItem = reforgeSlot.item.type;
            }

            UpdateReforgeList();

        }
        private void UpdateReforgeList()
        {
            Item controlItem = reforgeSlot.item.Clone();

            controlItem.netDefaults(reforgeSlot.item.netID);
            controlItem = controlItem.CloneWithModdedDataFrom(reforgeSlot.item);

            UIReforgeLabel reforgeLabel;
            List<int> tempSelected = new List<int>();
            foreach (var prefix in allowedPrefixes)
            {
                Item tempItem = controlItem.Clone();
                tempItem.Prefix(prefix);
                reforgeLabel = new UIReforgeLabel(tempItem);
                reforgeLabel.OnMouseDown += ChoseReforge;
                reforgeLabel.SetPadding(10);
                if (selectedPrefixes.Contains(prefix))
                {
                    reforgeLabel.selected = true;
                    tempSelected.Add(prefix);
                }
                reforgeList.Add(reforgeLabel);
            }
            selectedPrefixes = tempSelected;
        }
        private void UpdateAllowedPrefixes()
        {
            allowedPrefixes.Clear();
            if (!ItemLoader.PreReforge(reforgeSlot.item))
            {
                return;
            }
            int attempts = 75 * PrefixLoader.PrefixCount;
            for (int i = 0; i < attempts; i++)
            {
                Item tempItem = new Item();
                tempItem = reforgeSlot.item.CloneWithModdedDataFrom(reforgeSlot.item);
                tempItem.netDefaults(reforgeSlot.item.netID);
                tempItem.Prefix(-2);
                if (tempItem.prefix > 0)
                {
                    allowedPrefixes.Add(tempItem.prefix);
                }

            }
            Main.NewText($"Vanilla {allowedPrefixes.Count}");
            Item item = new Item();
            item = reforgeSlot.item.CloneWithModdedDataFrom(reforgeSlot.item);
            item.netDefaults(reforgeSlot.item.netID);
            AddAllowedModPrefixes(item, item.prefix);
        }

        private void AddAllowedModPrefixes(Item item, int num)
        {
            if (GadgetMethods.MagicPrefix(item))
            {
                var prefixes = GadgetMethods.GetValidModedPrefixes(item, PrefixCategory.Magic);
                AddPrefixes(prefixes);
            }

            var anyPrefixes = GadgetMethods.GetValidModedPrefixes(item, PrefixCategory.AnyWeapon);
            AddPrefixes(anyPrefixes);

            if (GadgetMethods.RangedPrefix(item))
            {
                var prefixes = GadgetMethods.GetValidModedPrefixes(item, PrefixCategory.Ranged);
                AddPrefixes(prefixes);
            }
            if (GadgetMethods.MagicPrefix(item))
            {
                var prefixes = GadgetMethods.GetValidModedPrefixes(item, PrefixCategory.Magic);
                AddPrefixes(prefixes);
            }
            if (GadgetMethods.MeleePrefix(item))
            {
                var prefixes = GadgetMethods.GetValidModedPrefixes(item, PrefixCategory.Melee);
                AddPrefixes(prefixes);
            }
            if (GadgetMethods.IsAPrefixableAccessory(item))
            {
                var prefixes = GadgetMethods.GetValidModedPrefixes(item, PrefixCategory.Accessory);
                AddPrefixes(prefixes);
            }
            if (GadgetMethods.GeneralPrefix(item))
            {
                var prefixes = GadgetMethods.GetValidModedPrefixes(item, PrefixCategory.Custom);
                AddPrefixes(prefixes);
            }
        }
        private void AddPrefixes(IEnumerable<int> prefixes)
        {
            foreach (var prefix in prefixes)
            {
                allowedPrefixes.Add(prefix);
            }
        }

        private void ChoseReforge(UIMouseEvent evt, UIElement listeningElement)
        {
            UIReforgeLabel element = ((UIReforgeLabel)listeningElement);
            element.selected = !element.selected;
            if (!selectedPrefixes.Remove(element.shownItem.prefix))
            {
                selectedPrefixes.Add(element.shownItem.prefix);
            }
            reforgeList.UpdateOrder();
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        private void ReforgeItem(bool silent)
        {
            Main.LocalPlayer.BuyItem(reforgePrice, -1);
            GadgetMethods.PrefixItem(ref reforgeSlot.item, silent);
        }
    }
}