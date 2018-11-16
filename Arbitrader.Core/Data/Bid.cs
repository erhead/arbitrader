namespace Arbitrader.Core.Data
{
    /// <summary>
    /// Defines bid information.
    /// </summary>
    public class Bid
    {
        public string ProviderName { get; set; }

        /// <summary>
        /// The asset for the Quantity field.
        /// </summary>
        public Asset BaseAsset { get; set; }

        /// <summary>
        /// The asset for the Price field.
        /// </summary>
        public Asset QuoteAsset { get; set; }

        /// <summary>
        /// Quantity being sold.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// The price for one unit of the sold asset expressed in the quote asset.
        /// </summary>
        public decimal Price { get; set; }
    }
}
