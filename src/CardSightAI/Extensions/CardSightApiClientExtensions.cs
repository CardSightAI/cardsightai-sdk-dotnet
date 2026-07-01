using System;
using System.Threading;
using System.Threading.Tasks;
using CardSightAI.Generated;

namespace CardSightAI
{
    /// <summary>
    /// Convenience extension methods over <see cref="ICardSightApiClient"/> that provide strongly-typed
    /// inputs for endpoints whose generated request bodies are loosely typed.
    /// </summary>
    /// <remarks>
    /// A few request bodies are declared in the OpenAPI specification as an <c>anyOf</c> of a single item
    /// or a batch array. The code generator cannot represent that as a class, so it emits an empty
    /// "additional properties" bag (for example <see cref="CreateCollectionCardInput"/> and
    /// <see cref="AddCardToListInput"/>). These helpers let you pass the fully-typed item that the API
    /// actually expects (<see cref="CollectionCardItemInput"/> / <see cref="ListCardItemInput"/>) and
    /// handle mapping it onto the generated body for you.
    /// </remarks>
    public static class CardSightApiClientExtensions
    {
        /// <summary>
        /// Adds a single card to a collection using a strongly-typed <see cref="CollectionCardItemInput"/>.
        /// </summary>
        /// <param name="api">The API client (typically <c>client.Api</c>).</param>
        /// <param name="collectionId">The collection to add the card to.</param>
        /// <param name="card">The card to add, with optional purchase, grade, and parallel details.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The add-cards response.</returns>
        /// <remarks>
        /// Wraps the generated <c>AddCollectionCardsAsync</c>, whose body is loosely typed. To add several
        /// cards, call this once per card (the generated client cannot send the batch-array form).
        /// </remarks>
        public static Task<AddCollectionCardsResponse> AddCollectionCardAsync(
            this ICardSightApiClient api,
            Guid collectionId,
            CollectionCardItemInput card,
            CancellationToken cancellationToken = default)
        {
            if (api is null) throw new ArgumentNullException(nameof(api));
            return api.AddCollectionCardsAsync(ToBody(card), collectionId, cancellationToken);
        }

        /// <summary>
        /// Adds a single card to a list using a strongly-typed <see cref="ListCardItemInput"/>.
        /// </summary>
        /// <param name="api">The API client (typically <c>client.Api</c>).</param>
        /// <param name="listId">The list to add the card to.</param>
        /// <param name="card">The card to add.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The add-cards response.</returns>
        /// <remarks>
        /// Wraps the generated <c>AddCardsToListAsync</c>, whose body is loosely typed. To add several
        /// cards, call this once per card (the generated client cannot send the batch-array form).
        /// </remarks>
        public static Task<AddCardsToListResponse> AddCardToListAsync(
            this ICardSightApiClient api,
            string listId,
            ListCardItemInput card,
            CancellationToken cancellationToken = default)
        {
            if (api is null) throw new ArgumentNullException(nameof(api));
            return api.AddCardsToListAsync(ToBody(card), listId, cancellationToken);
        }

        // Maps the typed collection-card item onto the generated free-form body.
        // The generator models the optional parallelId/gradeId as non-nullable Guids, so an unset value
        // serializes as Guid.Empty; we omit those so "no parallel / no grade" is sent correctly. Optional
        // string fields are omitted when null. Keep in sync with CollectionCardItemInput.
        internal static CreateCollectionCardInput ToBody(CollectionCardItemInput card)
        {
            if (card is null) throw new ArgumentNullException(nameof(card));

            var body = new CreateCollectionCardInput();
            var p = body.AdditionalProperties;

            p["cardId"] = card.CardId;
            if (card.ParallelId != Guid.Empty) p["parallelId"] = card.ParallelId;
            if (card.GradeId != Guid.Empty) p["gradeId"] = card.GradeId;
            p["quantity"] = card.Quantity;
            if (card.BuyPrice != null) p["buyPrice"] = card.BuyPrice;
            if (card.BuyDate != null) p["buyDate"] = card.BuyDate;
            if (card.SellPrice != null) p["sellPrice"] = card.SellPrice;
            if (card.SoldPrice != null) p["soldPrice"] = card.SoldPrice;
            if (card.SoldDate != null) p["soldDate"] = card.SoldDate;

            return body;
        }

        // Maps the typed list-card item onto the generated free-form body.
        internal static AddCardToListInput ToBody(ListCardItemInput card)
        {
            if (card is null) throw new ArgumentNullException(nameof(card));

            var body = new AddCardToListInput();
            if (card.CardId != null) body.AdditionalProperties["cardId"] = card.CardId;
            return body;
        }
    }
}
