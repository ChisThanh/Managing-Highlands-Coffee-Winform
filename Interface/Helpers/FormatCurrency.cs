namespace Interface.Helpers
{
    static public class FormatCurrency
    {
        public static string FormatAmount(int amount)
        {
            if (amount >= 1000000000)
            {
                double trillions = amount / 1000000000.0;
                return trillions.ToString("0.##") + "Tỉ";
            }
            else if (amount >= 1000000)
            {
                double trillions = amount / 1000000.0;
                return trillions.ToString("0.##") + "Tr";
            }
            else if (amount >= 1000)
            {
                double thousands = amount / 1000.0;
                return thousands.ToString("0.##") + "K";
            }
            else
            {
                return amount + " VND";
            }
        }
    }
}