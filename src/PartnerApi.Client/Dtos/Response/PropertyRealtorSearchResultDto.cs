using Newtonsoft.Json;

namespace PartnerApi.Client.Dtos.Response;

public class PropertyRealtorSearchResultDto
{
    [JsonProperty("Objects")]
    public IEnumerable<PropertyObjectDto> PropertyObjects { get; set; }
        
    public PagingDto Paging { get; set; }
    
    [JsonProperty("TotaalAantalObjecten")]
    public int TotalObjects { get; set; }
}

public class PagingDto
{
    [JsonProperty("AantalPaginas")]
    public int TotalPages { get; set; }
    [JsonProperty("HuidigePagina")]
    public int CurrentPage { get; set; }

    public bool IsLastPage => CurrentPage >= TotalPages;

    public int NextPage => IsLastPage ? CurrentPage : CurrentPage++;
}

public class PropertyObjectDto
{
    [JsonProperty("MakelaarId")]
    public int RealtorId { get; set; }
    [JsonProperty("MakelaarNaam")]
    public string RealtorName { get; set; }
}