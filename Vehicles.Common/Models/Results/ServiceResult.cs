namespace Vehicles.Common.Models.Results
{
    public class ServiceResult<T> : ServiceResult
    {
        public T? ResultValue { get; set; }
    }

    public class ServiceResult 
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
