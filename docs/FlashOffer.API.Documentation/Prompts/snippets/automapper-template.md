# AutoMapper Mapping Template

// Add to MappingProfile.cs

CreateMap<{EntityName}, {EntityName}Dto>();
CreateMap<Create{EntityName}Dto, {EntityName}>();
CreateMap<Update{EntityName}Dto, {EntityName}>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
    .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
    .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
