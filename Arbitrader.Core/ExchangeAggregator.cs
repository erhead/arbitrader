using System;
using System.Collections.Generic;
using System.Linq;
using Arbitrader.Core.Data;

namespace Arbitrader.Core
{
    public class ExchangeAggregator : IExchangeAggregator
    {
        private Dictionary<string, IExchangeProvider> _exchangeProviders = new Dictionary<string, IExchangeProvider>();

        public ExchangeAggregator(IEnumerable<IExchangeProvider> exchangeProviders)
        {
            foreach (var provider in exchangeProviders)
            {
                _exchangeProviders.Add(provider.ProviderName, provider);
            }
        }

        public IEnumerable<IExchangeProvider> ExchangeProviders => _exchangeProviders.Values.ToList();

        public void AddExchangeProvider(IExchangeProvider exchangeProvider)
        {
            if (_exchangeProviders.ContainsKey(exchangeProvider.ProviderName))
            {
                throw new InvalidOperationException($"The provider with name '{exchangeProvider.ProviderName}' already exists");
            }

            _exchangeProviders.Add(exchangeProvider.ProviderName, exchangeProvider);
        }

        public void RemoveExchangeProvider(string name)
        {
            if (!_exchangeProviders.ContainsKey(name))
            {
                throw new InvalidOperationException($"The provider with name '{name}' does not exist");
            }

            _exchangeProviders.Remove(name);
        }

        public int Buy(string providerName, Asset sourceAsset, Asset destAsset, decimal destAssetAmount, decimal? sourceAssetMaxAmount)
        {
            if (!_exchangeProviders.ContainsKey(providerName))
            {
                throw new InvalidOperationException($"An exchange provider with name '{providerName} does not exist.'");
            }

            var provider = _exchangeProviders[providerName];
            return provider.Buy(sourceAsset, destAsset, destAssetAmount, sourceAssetMaxAmount);
        }

        public bool BuyDryRun(string providerName, Asset sourceAsset, Asset destAsset, decimal destAssetAmount, decimal? sourceAssetMaxAmount)
        {
            if (!_exchangeProviders.ContainsKey(providerName))
            {
                throw new InvalidOperationException($"An exchange provider with name '{providerName} does not exist.'");
            }

            var provider = _exchangeProviders[providerName];
            return provider.BuyDryRun(sourceAsset, destAsset, destAssetAmount, sourceAssetMaxAmount);
        }

        public List<Bid> GetAllBids(Asset sourceAsset, Asset destAsset)
        {
            var result = new List<Bid>();
            result.AddRange(_exchangeProviders.Values.SelectMany(p => p.GetBids(sourceAsset, destAsset)));
            
            // Most profitable first.
            result.Sort((x, y) => Decimal.Compare(y.Rate, x.Rate));
            return result;
        }

        public List<Tuple<string, Asset, Asset, decimal>> GetAllExchangeDirections()
        {
            var result = new List<Tuple<string, Asset, Asset, decimal>>();
            result.AddRange(
                _exchangeProviders.Values.SelectMany(
                    p => p.GetDirections().Select(direction => new Tuple<string, Asset, Asset, decimal>(p.ProviderName, direction.Item1, direction.Item2, direction.Item3))));
            return result;
            
        }
    }
}
