using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrader.Core.Data
{
    public class OrderItem
    {
        public string ProviderName { get; set; }

        public Asset SourceAsset { get; set; }

        public Asset DestAsset { get; set; }

        public decimal DestAssetAmount { get; set; }

        public decimal? MaxSourceAssetAmount { get; set; }
    }
}
