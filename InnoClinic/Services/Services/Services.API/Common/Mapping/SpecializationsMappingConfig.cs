public class SpecializationsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateSpecializationRequest, CreateSpecializationCommand>()
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.SpecializationName, src => src.SpecializationName);

        config.NewConfig<(UpdateSpecializationRequest request, int Id), UpdateSpecializationCommand>()
            .Map(dest => dest.IsActive, src => src.request.IsActive)
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.SpecializationName, src => src.request.SpecializationName);

        config.NewConfig<(int Id, bool IsActive), ChangeServiceStatusCommand>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Status, src => src.IsActive);
    }
}
