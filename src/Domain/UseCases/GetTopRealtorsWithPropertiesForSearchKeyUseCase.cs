using System.Collections;
using Domain.Entities;
using Domain.Ports;
using Domain.ValueTypes;

namespace Domain.UseCases;

public class GetTopRealtorsWithPropertiesForSearchKeyUseCase
{
    private const int TopRankedAmount = 10;
    private readonly IRealtorRepository _realtorRepository;

    public GetTopRealtorsWithPropertiesForSearchKeyUseCase(IRealtorRepository realtorRepository)
    {
        _realtorRepository = realtorRepository ?? throw new ArgumentNullException(nameof(realtorRepository));
    }

    public async Task<IEnumerable<RealtorsProperties>> ExecuteAsync(IEnumerable<SearchKey> searchKeys, CancellationToken cancellationToken)
    {
        var realtors = await _realtorRepository.GetRealtorsForKeysAsync(searchKeys, cancellationToken);

        var realtorProperties = realtors
            .GroupBy(realtor => new { Id = realtor.RealtorId, Name = realtor.RealtorName })
            .OrderByDescending(oby => oby.Count())
            .Select(orderedRealtor => RealtorsProperties.New(orderedRealtor.Key.Name, PropertyCount.New(orderedRealtor.Count())))
            .Take(TopRankedAmount);

        return realtorProperties;
    }
}