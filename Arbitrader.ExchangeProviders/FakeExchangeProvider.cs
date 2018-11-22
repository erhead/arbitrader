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

        private const decimal RateDispersionPercent = 10m;

        private const decimal AmountDispersionPercent = 60m;

        private const int DecimalPrecision = 2;

        /// <summary>
        /// The collection of directions for this exchange provider. Each direction is represented by a tuple (SourceAsset, DestAsset, Rate).
        /// </summary>
        private List<Tuple<Asset, Asset, decimal>> _directions = new List<Tuple<Asset, Asset, decimal>>();

        private List<Bid> _bids = new List<Bid>();

        private List<Transaction> _transactions = new List<Transaction>();

        private void GenerateBids(Asset sourceAsset, Asset destAsset, decimal rate, decimal overallAmount)
        {
            decimal averageAmount = overallAmount / BidsPerDirection;
            decimal rateDispersion = rate * RateDispersionPercent / 100;
            decimal amountDispersion = averageAmount * AmountDispersionPercent / 100;
            Random random = new Random();
            for (var i = 0; i < BidsPerDirection; i++)
            {
                var currentRate = Math.Round(rate + (decimal)(random.NextDouble() - 0.5) * 2 * rateDispersion, DecimalPrecision);
                var destAssetAmount = Math.Round(averageAmount + (decimal)(random.NextDouble() - 0.5) * 2 * amountDispersion, DecimalPrecision);
                var bid = new Bid
                {
                    ProviderName = ProviderName,
                    SourceAsset = sourceAsset,
                    DestAsset = destAsset,
                    DestAssetAmount = destAssetAmount,
                    SourceAssetAmount = destAssetAmount / rate
                };
                _bids.Add(bid);
            }
        }

        /// <summary>
        /// Checks if specified purchase is feasible with current bids.
        /// </summary>
        /// <param name="sourceAsset">The source asset.</param>
        /// <param name="destAsset">The dest asset.</param>
        /// <param name="destAssetAmount">Exchange ratge of the source asset expressed in the dest asset.</param>
        /// <param name="maxSourceAssetAmount">Optional maximal amount of the source asset.</param>
        /// <returns>Number of bids to take from the top of the list to satisfy the request.</returns>
        private int CheckBuyingIsAllowed(Asset sourceAsset, Asset destAsset, decimal destAssetAmount, decimal? maxSourceAssetAmount)
        {
            if (maxSourceAssetAmount != null)
            {
                throw new NotImplementedException("Source asset amount limitation is not implemented");
            }

            if (!_directions.Any(x => x.Item1 == sourceAsset && x.Item2 == destAsset))
            {
                throw new InvalidOperationException($"Exchange direction {sourceAsset} -> {destAsset} is not supported by {ProviderName}");
            }

            if (destAssetAmount < 0)
            {
                throw new ArgumentException("Dest asset amount cannot be less than zero");
            }

            var bids = GetBids(sourceAsset, destAsset);

            decimal currentSum = 0;
            int bidsToTake = 0;
            while (currentSum < destAssetAmount && bidsToTake <= bids.Count)
            {
                bidsToTake++;
                currentSum += bids[bidsToTake - 1].DestAssetAmount;
            }

            if (bidsToTake > bids.Count)
            {
                throw new InvalidOperationException("Requested amount is greater than available amount");
            }

            return bidsToTake;
        }

        public IIdProvider IdProvider { get; set; }

        public string ProviderName { get; set; }

        /// <summary>
        /// Add or update exchange direction with predefined bids.
        /// </summary>
        /// <param name="sourceAsset">The asset being exchanged.</param>
        /// <param name="destAsset">What the source asset being exchanged to.</param>
        /// <param name="rate">Source asset rate expressed in the dest asset.</param>
        /// <param name="bids">The collection of bids to populate within the added direction.</param>
        public void AddDirection(Asset sourceAsset, Asset destAsset, decimal rate, List<Bid> bids)
        {
            RemoveDirection(sourceAsset, destAsset);
            foreach (var bid in bids)
            {
                _bids.Add(new Bid
                {
                    ProviderName = ProviderName,
                    SourceAsset = bid.SourceAsset,
                    DestAsset = bid.DestAsset,
                    SourceAssetAmount = bid.SourceAssetAmount,
                    DestAssetAmount = bid.DestAssetAmount
                });
            }

            _directions.Add(new Tuple<Asset, Asset, decimal>(sourceAsset, destAsset, rate));
        }

        /// <summary>
        /// Add or update exchange direction.
        /// </summary>
        /// <param name="sourceAsset">The asset being exchanged.</param>
        /// <param name="destAsset">What the source asset being exchanged to.</param>
        /// <param name="rate">Exchange rate source / destination.</param>
        /// <param name="overallAmount">Summary amount of all generated bids.</param>
        public void AddDirection(Asset sourceAsset, Asset destAsset, decimal rate, decimal overallAmount)
        {
            RemoveDirection(sourceAsset, destAsset);
            GenerateBids(sourceAsset, destAsset, rate, overallAmount);
            _directions.Add(new Tuple<Asset, Asset, decimal>(sourceAsset, destAsset, rate));
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

        /// <summary>
        /// Get the bids matching the specified asset pair.
        /// </summary>
        /// <param name="sourceAsset">The source asset.</param>
        /// <param name="destAsset">The dest asset.</param>
        /// <returns>The list of bids sorted by the source asset rate descending (more profitable first).</returns>
        public List<Bid> GetBids(Asset sourceAsset, Asset destAsset)
        {
            var result = _bids.Where(x => x.SourceAsset == sourceAsset && x.DestAsset == destAsset).ToList();
            result.Sort((x, y) => Decimal.Compare(y.Rate, x.Rate));
            return result;
        }

        public List<Tuple<Asset, Asset, decimal>> GetDirections()
        {
            return _directions.ToList();
        }

        public int Buy(Asset sourceAsset, Asset destAsset, decimal destAssetAmount, decimal? maxSourceAssetAmount)
        {
            int bidsToTake = CheckBuyingIsAllowed(sourceAsset, destAsset, destAssetAmount, maxSourceAssetAmount);

            var bids = GetBids(sourceAsset, destAsset);
            var takenBids = bids.Take(bidsToTake);
            decimal resultingRate = takenBids.Aggregate(0m, (s, bid) => s + bid.Rate * bid.DestAssetAmount) / takenBids.Sum(bid => bid.DestAssetAmount);

            var transaction = new Transaction
            {
                Id = IdProvider.GenerateIntId(nameof(Transaction)),
                SourceAsset = sourceAsset,
                DestAsset = destAsset,
                DestAssetAmount = destAssetAmount,
                SourceAssetAmount = destAssetAmount / resultingRate,
                Status = TransactionStatus.Success
            };
            _transactions.Add(transaction);
            return transaction.Id;
        }

        public bool BuyDryRun(Asset sourceAsset, Asset destAsset, decimal destAssetAmount, decimal? maxSourceAssetAmount)
        {
            try
            {
                CheckBuyingIsAllowed(sourceAsset, destAsset, destAssetAmount, maxSourceAssetAmount);
                return true;
            }
            catch (InvalidOperationException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }
        }
    }
}
