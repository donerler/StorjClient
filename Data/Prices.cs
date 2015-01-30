using System.Collections.Generic;

namespace StorjClient.Data
{
    public class PriceData
    {
        public List<Price> Prices { get; set; }
    }

    public class Price
    {
        public decimal Amount { get; set; }
        public decimal Cost { get; set; }
    }
}
