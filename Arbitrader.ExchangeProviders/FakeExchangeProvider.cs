using System;
using System.Collections.Generic;
using Arbitrader.Core;
using Arbitrader.Core.Data;

namespace Arbitrader.ExchangeProviders
{
    public class FakeExchangeProvider : IExchangeProvider
    {
        public string ProviderName => throw new NotImplementedException();

        public int Buy(Asset boughtAsset, decimal amount)
        {
            throw new NotImplementedException();
        }

        public bool BuyDryRun(Asset boughtAsset, decimal amount)
        {
            throw new NotImplementedException();
        }

        public List<Bid> GetBids(Asset soldAsset, Asset quoteAsset)
        {
            throw new NotImplementedException();
        }

        public List<Tuple<Asset, Asset, decimal>> GetDirections()
        {
            throw new NotImplementedException();
        }
    }
}
