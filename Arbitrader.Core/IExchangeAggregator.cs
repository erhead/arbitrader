using Arbitrader.Core.Data;
using System;
using System.Collections.Generic;

namespace Arbitrader.Core
{
    public interface IExchangeAggregator
    {
        void AddExchangeProvider(IExchangeProvider exchangeProvider);

        void RemoveExchangeProvider(string name);

        IEnumerable<IExchangeProvider> ExchangeProviders { get; }

        List<Tuple<string, Asset, Asset, decimal>> GetAllExchangeDirections();

        List<Bid> GetAllBids(Asset sourceAsset, Asset destAsset);

        int Buy(string providerName, Asset sourceAsset, Asset destAsset, decimal destAssetAmount, decimal? maxSourceAssetAmount);

        bool BuyDryRun(string providerName, Asset sourceAsset, Asset destAsset, decimal destAssetAmount, decimal? maxSourceAssetAmount);
    }
}
