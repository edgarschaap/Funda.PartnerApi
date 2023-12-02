using Domain.Entities;
using Domain.ValueTypes;
using PartnerApi.Client.Dtos.Response;

namespace Adapter.Http.PartnerApi.Mappers;

public static class PropertyObjectDtoToRealtorMapper
{
    public static Realtor ToDomainEntity(this PropertyObjectDto dto)
    {
        return Realtor.New(
                RealtorId.New(dto.RealtorId),
                RealtorName.New(dto.RealtorName)
            );
    }
}