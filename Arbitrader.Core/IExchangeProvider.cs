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
        /// <param name="sourceAsset">The asset being bought in the bid.</param>
        /// <param name="destAsset">The asset being sold in the bid.</param>
        /// <returns>A list of appropriate bids.</returns>
        List<Bid> GetBids(Asset sourceAsset, Asset destAsset);

        /// <summary>
        /// Buy specified amount of the specified asset.
        /// </summary>
        /// <param name="sourceAsset">The asset to sell.</param>
        /// <param name="destAsset">The asset to buy.</param>
        /// <param name="destAssetAmount">Amount of the dest asset to buy.</param>
        /// <param name="maxSourceAssetAmount">Optional maximal amount of the source asset.</param>
        /// <returns>The created transaction ID.</returns>
        int Buy(Asset sourceAsset, Asset destAsset, decimal destAssetAmount, decimal? maxSourceAssetAmount);

        /// <summary>
        /// Check possibility of buying specified amount of the asset right now.
        /// </summary>
        /// <param name="sourceAsset">The asset to sell.</param>
        /// <param name="destAsset">The asset to buy.</param>
        /// <param name="destAssetAmount">Amount of the dest asset to buy.</param>
        /// <param name="maxSourceAssetAmount">Optional maximal amount of the source asset.</param>
        /// <returns><c>true</c>, if the specified operation is possible.</returns>
        bool BuyDryRun(Asset sourceAsset, Asset destAsset, decimal destAssetAmount, decimal? maxSourceAssetAmount);
    }
}
