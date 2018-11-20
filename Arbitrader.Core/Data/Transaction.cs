namespace Arbitrader.Core.Data
{
    public class Transaction
    {
        public int Id { get; set; }

        public Asset SourceAsset { get; set; }

        public Asset DestAsset { get; set; }

        public decimal SourceAssetAmount { get; set; }

        public decimal DestAssetAmount { get; set; }

        public decimal? Rate
        {
            get
            {
                if (SourceAssetAmount == 0)
                {
                    return null;
                }
                else 
                {
                    return DestAssetAmount / SourceAssetAmount;
                }
            }
        }

        public TransactionStatus Status { get; set; }
    }
}
