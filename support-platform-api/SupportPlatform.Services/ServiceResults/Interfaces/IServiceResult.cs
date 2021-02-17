using System.Collections.Generic;

namespace SupportPlatform.Services
{
    public interface IServiceResult
    {
        ResultType Result { get; set; }
        ICollection<string> Errors { get; set; }
    }

    public interface IServiceResult<ReturnType> : IServiceResult
    {
        ReturnType ReturnedObject { get; set; }
    }
}
