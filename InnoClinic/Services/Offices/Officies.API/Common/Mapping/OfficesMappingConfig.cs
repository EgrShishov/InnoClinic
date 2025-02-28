public class OfficesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateOfficeRequest, CreateOfficeCommand>()
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.HouseNumber, src => src.HouseNumber)
            .Map(dest => dest.OfficeNumber, src => src.OfficeNumber)
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.Photo, src => src.Photo)
            .Map(dest => dest.RegistryPhoneNumber, src => src.RegistryPhoneNumber)
            .Map(dest => dest.Street, src => src.Street);        
        
        config.NewConfig<(UpdateOfficeRequest request, string OfficeId), UpdateOfficeCommand>()
            .Map(dest => dest.OfficeId, src => src.OfficeId)
            .Map(dest => dest.City, src => src.request.City)
            .Map(dest => dest.HouseNumber, src => src.request.HouseNumber)
            .Map(dest => dest.OfficeNumber, src => src.request.OfficeNumber)
            .Map(dest => dest.IsActive, src => src.request.IsActive)
            .Map(dest => dest.Photo, src => src.request.Photo)
            .Map(dest => dest.RegistryPhoneNumber, src => src.request.RegistryPhoneNumber)
            .Map(dest => dest.Street, src => src.request.Street);

    }
}
