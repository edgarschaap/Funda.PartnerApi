using Adapter.Persistence.InMemory;
using Domain.Entities;
using Domain.UseCases;
using Domain.ValueTypes;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.Tests.Unit.UseCases;

[TestFixture]
public class GetTopRealtorsWithPropertiesForSearchKeyUseCaseTests
{
    private PartnerApiRealtorRepositoryInMemory _realtorRepositoryInMemory;

    [SetUp]
    public void SetUp()
    {
        _realtorRepositoryInMemory = new PartnerApiRealtorRepositoryInMemory();
    }

    [Test]
    public Task CanConstruct_DoesntThrowArgumentNullException()
    {
        // Arrange
        var action = () => CreateSut();

        // Act & Assert
        action.Should().NotThrow<ArgumentNullException>();

        return Task.CompletedTask;
    }
    
    [Test]
    public async Task GivenNoRealtorsForKeyAvailable_ShouldReturnEmptyList()
    {
        // Arrange
        var sut = CreateSut();
        var searchKeys = new List<SearchKey> { SearchKey.New("NoResultsKey") };
        
        // Act
        IEnumerable<RealtorsProperties> result = await sut.ExecuteAsync(searchKeys, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GivenThreeRealtorsWithMultipleObjects_ShouldReturnOrderedByObjectCount()
    {
        // Arrange
        var searchKeys = new List<SearchKey> { SearchKey.New("Huizen") };
        var realtors = new List<Realtor>
        {
            Realtor.New(RealtorId.New(1), RealtorName.New("Voorma & Walch")),
            Realtor.New(RealtorId.New(2), RealtorName.New("Van Trigt Makelaars")),
            Realtor.New(RealtorId.New(2), RealtorName.New("Van Trigt Makelaars")),
            Realtor.New(RealtorId.New(2), RealtorName.New("Van Trigt Makelaars")),
            Realtor.New(RealtorId.New(3), RealtorName.New("Floberg Makelaardij")),
            Realtor.New(RealtorId.New(3), RealtorName.New("Floberg Makelaardij")),
            Realtor.New(RealtorId.New(1), RealtorName.New("Voorma & Walch")),
            Realtor.New(RealtorId.New(1), RealtorName.New("Voorma & Walch")),
            Realtor.New(RealtorId.New(1), RealtorName.New("Voorma & Walch")),
        };

        foreach (var realtor in realtors)
        {
            _realtorRepositoryInMemory.AddRealtorForKeys(realtor, searchKeys);
        }

        var sut = CreateSut();

        // Act
        List<RealtorsProperties> result = (await sut.ExecuteAsync(searchKeys, CancellationToken.None)).ToList();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Count().Should().Be(3);
        result.First().Should()
            .BeEquivalentTo(RealtorsProperties.New(RealtorName.New("Voorma & Walch"), PropertyCount.New(4)));
        result.Last().Should()
            .BeEquivalentTo(RealtorsProperties.New(RealtorName.New("Floberg Makelaardij"), PropertyCount.New(2)));
    }

    [Test]
    public async Task GivenMoreThanTenRealtorsWithMultipleObjects_ShouldOnlyReturnTopTen()
    {
        // Arrange
        var searchKeys = new List<SearchKey> { SearchKey.New("Huizen") };
        var realtors = new List<Realtor>
        {
            Realtor.New(RealtorId.New(1), RealtorName.New("Voorma & Walch")),
            Realtor.New(RealtorId.New(2), RealtorName.New("Van Trigt Makelaars")),
            Realtor.New(RealtorId.New(2), RealtorName.New("Van Trigt Makelaars")),
            Realtor.New(RealtorId.New(2), RealtorName.New("Van Trigt Makelaars")),
            Realtor.New(RealtorId.New(3), RealtorName.New("Floberg Makelaardij")),
            Realtor.New(RealtorId.New(3), RealtorName.New("Floberg Makelaardij")),
            Realtor.New(RealtorId.New(1), RealtorName.New("Voorma & Walch")),
            Realtor.New(RealtorId.New(1), RealtorName.New("Voorma & Walch")),
            Realtor.New(RealtorId.New(1), RealtorName.New("Voorma & Walch")),
            Realtor.New(RealtorId.New(4), RealtorName.New("Van Straaten ERA Makelaardij")),
            Realtor.New(RealtorId.New(5), RealtorName.New("Siewe Makelaars")),
            Realtor.New(RealtorId.New(6), RealtorName.New("Avenue 't Gooi Makelaars")),
            Realtor.New(RealtorId.New(7), RealtorName.New("Kappelle Makelaars")),
            Realtor.New(RealtorId.New(8), RealtorName.New("Gerbert Hansman Makelaardij")),
            Realtor.New(RealtorId.New(9), RealtorName.New("De Ruiter Makelaars")),
            Realtor.New(RealtorId.New(10), RealtorName.New("Schuitema Makelaars")),
            Realtor.New(RealtorId.New(10), RealtorName.New("Schuitema Makelaars")),
            Realtor.New(RealtorId.New(11), RealtorName.New("Van Westerloo Makelaardij")),
            Realtor.New(RealtorId.New(12), RealtorName.New("Keizerskroon Makelaars")),
            Realtor.New(RealtorId.New(13), RealtorName.New("Noké Vastgoed")),
            Realtor.New(RealtorId.New(13), RealtorName.New("Noké Vastgoed")),
        };

        foreach (var realtor in realtors)
        {
            _realtorRepositoryInMemory.AddRealtorForKeys(realtor, searchKeys);
        }

        var sut = CreateSut();

        // Act
        List<RealtorsProperties> result = (await sut.ExecuteAsync(searchKeys, CancellationToken.None)).ToList();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Count().Should().Be(10);
    }
    
    public GetTopRealtorsWithPropertiesForSearchKeyUseCase CreateSut()
    {
        return new GetTopRealtorsWithPropertiesForSearchKeyUseCase(_realtorRepositoryInMemory);
    }
}