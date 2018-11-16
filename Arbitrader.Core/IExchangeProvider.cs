using Arbitrader.Core.Data;
using System;
using System.Collections.Generic;

namespace Arbitrader.Core
{
    /// <summary>
    /// Defines methods for a class which provider an access to a certain exchange.
    /// </summary>
    public interface IExchangeProvider
    {
        /// <summary>
        /// Returns a provider name which is unique within the system.
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Get supported exchange directions and maximal exchange amount denominated in the source asset.
        /// </summary>
        /// <returns>The tuple consisting of the source asset, the destination asset and maximal amount denominated in the source asset.</returns>
        List<Tuple<Asset, Asset, decimal>> GetDirections();

        /// <summary>
        /// Returns a list of bids for the specified asset pair.
        /// </summary>
        /// <param name="soldAsset">The asset being sold in the bid.</param>
        /// <param name="quoteAsset">The quote asset.</param>
        /// <returns>A list of appropriate bids.</returns>
        List<Bid> GetBids(Asset soldAsset, Asset quoteAsset);

        /// <summary>
        /// Buy specified amount of the specified asset.
        /// </summary>
        /// <param name="boughtAsset">The asset to buy.</param>
        /// <param name="amount">Amount to buy.</param>
        /// <returns>The created transaction ID.</returns>
        int Buy(Asset boughtAsset, decimal amount);

        /// <summary>
        /// Check possibility of buying specified amount of the asset right now.
        /// </summary>
        /// <param name="boughtAsset">The asset to check.</param>
        /// <param name="amount">Amount to check.</param>
        /// <returns><c>true</c>, if the specified operation is possible.</returns>
        bool BuyDryRun(Asset boughtAsset, decimal amount);
    }
}
