using System;
using Newtonsoft.Json;
using Xunit;
using CardSightAI;
using CardSightAI.Generated;

namespace CardSightAI.Tests
{
    public class CardSightApiClientExtensionsTests
    {
        [Fact]
        public void ToBody_CollectionCard_MapsSetFields()
        {
            var cardId = Guid.NewGuid();
            var gradeId = Guid.NewGuid();

            var body = CardSightApiClientExtensions.ToBody(new CollectionCardItemInput
            {
                CardId = cardId,
                GradeId = gradeId,
                Quantity = 2,
                BuyPrice = "50.00",
            });

            var p = body.AdditionalProperties;
            Assert.Equal(cardId, p["cardId"]);
            Assert.Equal(gradeId, p["gradeId"]);
            Assert.Equal(2d, p["quantity"]);
            Assert.Equal("50.00", p["buyPrice"]);
        }

        [Fact]
        public void ToBody_CollectionCard_OmitsEmptyGuidsAndNulls()
        {
            // ParallelId/GradeId left unset (Guid.Empty) and optional strings left null
            var body = CardSightApiClientExtensions.ToBody(new CollectionCardItemInput
            {
                CardId = Guid.NewGuid(),
                Quantity = 1,
            });

            var p = body.AdditionalProperties;
            Assert.False(p.ContainsKey("parallelId")); // empty Guid omitted
            Assert.False(p.ContainsKey("gradeId"));     // empty Guid omitted
            Assert.False(p.ContainsKey("buyPrice"));    // null omitted
            Assert.False(p.ContainsKey("soldDate"));    // null omitted
        }

        [Fact]
        public void ToBody_CollectionCard_SerializesToFlatJson()
        {
            var body = CardSightApiClientExtensions.ToBody(new CollectionCardItemInput
            {
                CardId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Quantity = 1,
            });

            var json = JsonConvert.SerializeObject(body);

            // The free-form body serializes its extension data as a flat object
            Assert.Contains("\"cardId\":\"11111111-1111-1111-1111-111111111111\"", json);
            Assert.Contains("\"quantity\":1.0", json);
            Assert.DoesNotContain("parallelId", json);
        }

        [Fact]
        public void ToBody_ListCard_MapsCardId()
        {
            var body = CardSightApiClientExtensions.ToBody(new ListCardItemInput { CardId = "card-123" });
            Assert.Equal("card-123", body.AdditionalProperties["cardId"]);
        }

        [Fact]
        public void ToBody_NullCard_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                CardSightApiClientExtensions.ToBody((CollectionCardItemInput)null!));
        }

        [Fact]
        public void AddCollectionCardAsync_NullApi_Throws()
        {
            ICardSightApiClient api = null!;
            // The guard throws synchronously (before the Task is created), so use the Action overload.
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = api.AddCollectionCardAsync(Guid.NewGuid(), new CollectionCardItemInput { CardId = Guid.NewGuid() });
            });
        }
    }
}
