namespace Arbitrader.Core.Data
{
    /// <summary>
    /// Defines bid information.
    /// </summary>
    public class Bid
    {
        public string ProviderName { get; set; }

        /// <summary>
        /// The asset being bought by the bid.
        /// </summary>
        public Asset SourceAsset { get; set; }

        /// <summary>
        /// The asset being sold by the bid.
        /// </summary>
        public Asset DestAsset { get; set; }

        /// <summary>
        /// Amount being sold.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Price of the source asset expressed in the dest asset (source asset / dest asset).
        /// </summary>
        public decimal Price { get; set; }
    }
}
