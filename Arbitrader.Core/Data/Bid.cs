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
        /// Amount being bought by the bid.
        /// </summary>
        public decimal SourceAssetAmount { get; set; }

        /// <summary>
        /// Amount being sold by the bid.
        /// </summary>
        public decimal DestAssetAmount { get; set; }

        /// <summary>
        /// Rate of the source asset expressed in the dest asset.
        /// </summary>
        public decimal Rate
        { 
            get
            {
                if (SourceAssetAmount == 0)
                {
                    return decimal.MaxValue;
                }
                else
                {
                    return DestAssetAmount / SourceAssetAmount;
                }
            }
        }
    }
}
