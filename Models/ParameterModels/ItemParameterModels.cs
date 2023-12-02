namespace ImHungryBackendER.Models.ParameterModels
{
    public class GetItemIngredientRequest
    {
        public long ItemId { get; set; }
    }

    public class GetItemIngredientResponse
    {
        public string Name { get; set; }
        public bool isActive { get; set; }
    }
}
