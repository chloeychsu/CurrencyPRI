using AutoMapper;

namespace CurrencyApi;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Currency, CurrenciesDto>().IncludeMembers(x => x.Language)
        .ForMember(d=>d.Updated,o=>o.MapFrom(s=>s.UpdatedUTC.ToLocalTime().ToString("yyyy/MM/dd hh:mm:ss")));
        CreateMap<ICollection<Translation>, CurrenciesDto>()
       .ForMember(d => d.CH_Name,
                  o => o.MapFrom(s => s.FirstOrDefault(x => x.Language == "zh-TW").Text ?? ""));
        CreateMap<CreateCurrencyDto,Currency>();
    }
}
