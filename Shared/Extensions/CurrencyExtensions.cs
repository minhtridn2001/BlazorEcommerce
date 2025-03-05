using System.Globalization;


namespace BlazorEcommerce.Shared.Extensions
{
    public static class CurrencyExtensions
    {
        private static readonly CultureInfo VietnameseCulture = CultureInfo.GetCultureInfo("vi-VN");

        public static string ToVND(this decimal amount)
        {
            return string.Format(VietnameseCulture, "{0:#,##0} VND", amount);
        }
    }
}
