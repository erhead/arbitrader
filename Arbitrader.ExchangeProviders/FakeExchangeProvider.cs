using System;
using System.Collections.Generic;
using System.Linq;
using Arbitrader.Core;
using Arbitrader.Core.Data;

namespace Arbitrader.ExchangeProviders
{
    public class FakeExchangeProvider : IExchangeProvider
    {
        private const int BidsPerDirection = 100;

        private const decimal PriceDispersionPercent = 10m;

        private const decimal AmountDispersionPercent = 60m;

        private const int DecimalPrecision = 2;

        /// <summary>
        /// The collection of directions for this exchange provider. Each direction is represented by a tuple (SourceAsset, DestAsset, Price).
        /// </summary>
        private List<Tuple<Asset, Asset, decimal>> _directions = new List<Tuple<Asset, Asset, decimal>>();

        private List<Bid> _bids = new List<Bid>();

        private void GenerateBids(Asset sourceAsset, Asset destAsset, decimal price, decimal overallAmount)
        {
            decimal averageAmount = overallAmount / BidsPerDirection;
            decimal priceDispersion = price * PriceDispersionPercent / 100;
            decimal amountDispersion = averageAmount * AmountDispersionPercent / 100;
            Random random = new Random();
            for (var i = 0; i < BidsPerDirection; i++)
            {
                var bid = new Bid
                {
                    ProviderName = ProviderName,
                    SourceAsset = sourceAsset,
                    DestAsset = destAsset,
                    Amount = Math.Round(averageAmount + (decimal)(random.NextDouble() - 0.5) * 2 * amountDispersion, DecimalPrecision),
                    Price = Math.Round(price + (decimal)(random.NextDouble() - 0.5) * 2 * priceDispersion, DecimalPrecision)
                };
                _bids.Add(bid);
            }
        }

        public string ProviderName { get; set; }

        /// <summary>
        /// Add or update exchange direction with predefined bids.
        /// </summary>
        /// <param name="sourceAsset">The asset being exchanged.</param>
        /// <param name="destAsset">What the source asset being exchanged to.</param>
        /// <param name="bids">The collection of bids to populate within the added direction.</param>
        public void AddDirection(Asset sourceAsset, Asset destAsset, decimal price, List<Bid> bids)
        {
            RemoveDirection(sourceAsset, destAsset);
            foreach (var bid in bids)
            {
                _bids.Add(new Bid
                {
                    ProviderName = ProviderName,
                    Amount = bid.Amount,
                    SourceAsset = bid.SourceAsset,
                    DestAsset = bid.DestAsset,
                    Price = bid.Price
                });
            }

            _directions.Add(new Tuple<Asset, Asset, decimal>(sourceAsset, destAsset, price));
        }

        /// <summary>
        /// Add or update exchange direction.
        /// </summary>
        /// <param name="sourceAsset">The asset being exchanged.</param>
        /// <param name="destAsset">What the source asset being exchanged to.</param>
        /// <param name="price">Exchange rate source / destination.</param>
        /// <param name="overallAmount">Summary amount of all generated bids.</param>
        public void AddDirection(Asset sourceAsset, Asset destAsset, decimal price, decimal overallAmount)
        {
            RemoveDirection(sourceAsset, destAsset);
            GenerateBids(sourceAsset, destAsset, price, overallAmount);
            _directions.Add(new Tuple<Asset, Asset, decimal>(sourceAsset, destAsset, price));
        }

        public void RemoveDirection(Asset sourceAsset, Asset destAsset)
        {
            var bidsToRemove = _bids.Where(x => x.SourceAsset == sourceAsset && x.DestAsset == destAsset);
            foreach (Bid bid in bidsToRemove)
            {
                _bids.Remove(bid);
            }

            var directionsToRemove = _directions.Where(x => x.Item1 == sourceAsset && x.Item2 == destAsset);
            foreach(var direction in directionsToRemove)
            {
                _directions.Remove(direction);
            }
        }

        public List<Bid> GetBids(Asset sourceAsset, Asset destAsset)
        {
            return _bids.Where(x => x.SourceAsset == sourceAsset && x.DestAsset == destAsset).ToList();
        }

        public List<Tuple<Asset, Asset, decimal>> GetDirections()
        {
            return _directions.ToList();
        }

        public int Buy(Asset sourceAsset, Asset destAsset, decimal amount)
        {
            throw new NotImplementedException();
        }

        public bool BuyDryRun(Asset sourceAsset, Asset destAsset, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
