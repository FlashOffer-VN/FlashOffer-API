using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Features.PurchaseRequests.Commands;
using System.Reflection;

namespace FlashOffer.API.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
		CreateMap<CreatePurchaseRequestDto, CreatePurchaseRequestCommand>();
	}

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            // Tên interface generic trong Reflection phải có dấu backtick (arity 1) để tìm đúng method
            var methodInfo = type.GetMethod("Mapping") ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");

            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}
