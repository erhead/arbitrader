using Arbitrader.Core.Data;
using Arbitrader.Core.ExchangeProvider;
using System;
using System.Collections.Generic;

namespace Arbitrader.Core.ExchangeAggregator
{
    public interface IExchangeAggregator
    {
        void AddExchangeProvider(IExchangeProvider exchangeProvider);

        void RemoveExchangeProvider(string name);

        IEnumerable<IExchangeProvider> ExchangeProviders { get; }

        List<Tuple<string, Asset, Asset, decimal>> GetAllExchangeDirections();

        List<Bid> GetAllBids(Asset soldAsset, Asset quoteAsset);

        int Buy(string providerName, Asset boughtAsset, decimal amount);

        bool BuyDryRun(string providerName, Asset boughtAsset, decimal amount);
    }
}
