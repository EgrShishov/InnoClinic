public class ServicesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateServiceRequest, CreateServiceCommand>()
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.ServiceCategory, src => src.ServiceCategory)
            .Map(dest => dest.ServiceName, src => src.ServiceName)
            .Map(dest => dest.SpecializationId, src => src.SpecializationId)
            .Map(dest => dest.ServicePrice, src => src.ServicePrice);

        config.NewConfig<(int Id, UpdateServiceInfoRequest request), UpdateServiceCommand>()
            .Map(dest => dest.IsActive, src => src.request.IsActive)
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ServiceName, src => src.request.ServiceName)
            .Map(dest => dest.ServicePrice, src => src.request.ServicePrice)
            .Map(dest => dest.ServiceCategory, src => src.request.ServiceCategory);
    }
}
