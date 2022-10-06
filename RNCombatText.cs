using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace RNCombatText
{
	public class RNCombatText : Mod
	{
        private readonly Dictionary<int, string> numberRomanDictionary = new()
        {
            { 1000, "M" },
            { 900, "CM" },
            { 500, "D" },
            { 400, "CD" },
            { 100, "C" },
            { 90, "XC" },
            { 50, "L" },
            { 40, "XL" },
            { 10, "X" },
            { 9, "IX" },
            { 5, "V" },
            { 4, "IV" },
            { 1, "I" }
        };

        private string To(int number)
        {
            StringBuilder roman = new();

            foreach (var item in numberRomanDictionary)
            {
                while (number >= item.Key)
                {
                    roman.Append(item.Value);

                    number -= item.Key;
                }
            }

            return roman.ToString();
        }

        public override void Load()
        {
            On.Terraria.CombatText.NewText_Rectangle_Color_int_bool_bool += CombatText_NewText_Rectangle_Color_int_bool_bool;
            On.Terraria.CombatText.NewText_Rectangle_Color_string_bool_bool += CombatText_NewText_Rectangle_Color_string_bool_bool;
        }

        private int CombatText_NewText_Rectangle_Color_string_bool_bool(On.Terraria.CombatText.orig_NewText_Rectangle_Color_string_bool_bool orig, Microsoft.Xna.Framework.Rectangle location, Microsoft.Xna.Framework.Color color, string text, bool dramatic, bool dot)
        {
            string[] split = text.Split(' ');

            string completedString = "";

            foreach (string newText in split)
            {
                if (int.TryParse(newText, out int value))
                {
                    completedString += To(value);
                }
                else
                {
                    completedString += newText;
                }

                completedString += " ";
            }

            completedString.Trim();

            return orig(location, color, completedString, dramatic, dot);
        }

        private int CombatText_NewText_Rectangle_Color_int_bool_bool(On.Terraria.CombatText.orig_NewText_Rectangle_Color_int_bool_bool orig, Microsoft.Xna.Framework.Rectangle location, Microsoft.Xna.Framework.Color color, int amount, bool dramatic, bool dot)
        {
            return CombatText.NewText(location, color, amount.ToString(), dramatic, dot);
        }
    }
}