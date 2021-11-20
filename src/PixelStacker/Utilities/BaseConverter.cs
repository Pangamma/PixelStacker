using System;
using System.Linq;

namespace PixelStacker.Utilities
{
    public class BaseConverter
    {
        private const string ALPHABET = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz५६७८९॰ॱ১২৩৪৫৬৭৮৯ৰৱ৲৳৴৵৶৷৸৹৺੦੧੨੩੪੫੬੭੮੯દધનપફૐ૧૨૩૪૫૬૭૮૯૰୦୧୨୪୫୬୭୮୯୰ୱ୲୳୴ஃౚ౦౧౨౩౪౫౬౭౮౯౸౹౺౻౼౽౾ಅಆಇಈಉಊಋಌಎಏಐಒಓಔಕಖಗಘಙಚಛಜಝಞಟಠಡಢಣತಥದಧನಪಫಬಭಮಯರಱಲಳವಶಷಸಹ಼ಽೞೠೡ೦೧೨೩೪೫೬೭೮അആഇഈഉഊഋഌഎ";
        public static int ConvertFromBaseX(string input, int baseX)
        {
            if (2 > baseX || baseX > ALPHABET.Length)
                throw new ArgumentOutOfRangeException(nameof(baseX), "Value must be between 2 and " + ALPHABET.Length);

            string strBase = ALPHABET;
            int intValue = 0;
            int intPower = 1;
            var chars = Enumerable.Reverse(input.ToCharArray());
            foreach (char c in chars)
            {
                var intPosValue = strBase.IndexOf(c);
                intValue += intPosValue * intPower;
                intPower *= baseX;
            }
            return intValue;
        }

        public static string ConvertToBaseX(int input, int baseX)
        {
            if (2 > baseX || baseX > ALPHABET.Length)
                throw new ArgumentOutOfRangeException(nameof(baseX), "Value must be between 2 and " + ALPHABET.Length);

            string strBase = ALPHABET;
            string strValue = "";
            int intPower = baseX;
            while ((int)input != 0)
            {
                var intMod = (int)(input % intPower);
                input /= intPower;
                strValue = strBase[intMod] + strValue;
            }

            return strValue;
        }
    }
}
