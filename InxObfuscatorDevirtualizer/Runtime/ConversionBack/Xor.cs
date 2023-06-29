namespace InxObfuscatorDevirtualizer.Runtime.ConversionBack
{
    public class XoringShit
    {
        public static string Xoring(string inputString)
        {
            char c = 'ع';
            string text = "";
            int length = inputString.Length;
            for (int i = 0; i < length; i++)
            {
                text += char.ToString((char)(inputString[i] ^ c));
            }
            return text;
        }
    }
}