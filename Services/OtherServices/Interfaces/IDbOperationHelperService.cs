namespace ImHungryBackendER.Services.OtherServices.Interfaces
{
    public interface IDbOperationHelperService
    {
        void MarkModifiedProperties<T>(T entity, ImHungryContext context);
    }
}
