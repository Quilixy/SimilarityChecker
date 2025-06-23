namespace api.Helpers
{
    public static class NumberToWordsConverter
    {
        private static readonly string[] Units = 
            { "sıfır", "bir", "iki", "üç", "dört", "beş", "altı", "yedi", "sekiz", "dokuz" };

        private static readonly string[] Tens = 
            { "", "on", "yirmi", "otuz", "kırk", "elli", "altmış", "yetmiş", "seksen", "doksan" };

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return Units[0];

            if (number < 0)
                return "eksi " + NumberToWords(Math.Abs(number));

            string words = "";

            #region Binler
                if ((number / 1000) > 0)
                {
                    if ((number / 1000) == 1)
                        words += "bin";
                    else
                        words += Units[number / 1000] + "bin";
                    number %= 1000;
                }
            #endregion

            #region Yüzler
            if ((number / 100) > 0)
            {
                if ((number / 100) == 1)
                    words += "yüz";
                else
                    words += Units[number / 100] + "yüz";
                number %= 100;
            }
            #endregion

            #region Onlar
            if (number > 0)
            {
                if (number < 10)
                    words += Units[number];
                else
                    words += Tens[number / 10] + Units[number % 10];
            }
            #endregion

            
            return words;
        }
    }
}