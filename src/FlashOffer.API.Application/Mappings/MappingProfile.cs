using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Features.OfferRequests.Commands;
using FlashOffer.API.Application.Features.PurchaseRequests.Commands;
using FlashOffer.API.Domain.Entities;
using System.Reflection;

namespace FlashOffer.API.Application.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

		CreateMap<CreateOfferRequestCommand, OfferRequest>();
		CreateMap<OfferRequest, OfferRequestResponseDto>();
		CreateMap<CreatePurchaseRequestCommand, PurchaseRequest>();
		CreateMap<PurchaseRequest, PurchaseRequestResponseDto>();
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
			var methodInfo = type.GetMethod("Mapping") ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");
			methodInfo?.Invoke(instance, new object[] { this });
		}
	}
}