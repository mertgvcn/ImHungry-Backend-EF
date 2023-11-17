namespace ImHungryBackendER.Services.OtherServices.Interfaces
{
    public interface IDbOperationHelperService
    {
        public void MarkModifiedProperties<T>(T entity, ImHungryContext context);
    }
}
