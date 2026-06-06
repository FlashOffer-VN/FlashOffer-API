namespace FlashOffer.API.Application.Common.Mappings;

public interface IMapFrom<T>
{
    void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap(typeof(T), GetType()).ReverseMap();
    }
}
