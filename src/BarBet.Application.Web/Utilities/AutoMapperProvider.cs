using AutoMapper;

namespace BarBet.Application.Web.Utilities;

internal static class AutoMapperProvider
{
    private static IMapper? _mapper;

    public static IMapper Mapper
    {
        get
        {
            return _mapper ??=
                new MapperConfiguration(config => config.AddMaps(typeof(AutoMapperProvider).Assembly))
                    .CreateMapper();
        }
    }
}