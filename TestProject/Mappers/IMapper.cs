namespace Work.Mappers;

public interface IMapper<TModel, TDto>
{
    public TModel ToModel(TDto dto);
    
    public TDto ToDto(TModel model);
}