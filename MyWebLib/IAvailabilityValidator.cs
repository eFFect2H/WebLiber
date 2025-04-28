namespace MyWebLib
{
    public interface IAvailabilityValidator<TDto>
    {
        Task<bool> IsAvailableAsync(TDto dto);
    }
}
