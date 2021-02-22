using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Services
{
    public class ServiceResult : IServiceResult
    {
        public ResultType Result { get; set; }
        public ICollection<string> Errors { get; set; } = null;

        public ServiceResult(ResultType result)
        {
            Result = result;
        }

        public ServiceResult(ResultType result, ICollection<string> errors)
        {
            Result = result;
            Errors = errors;
        }
    }

    public class ServiceResult<ReturnType> : ServiceResult, IServiceResult<ReturnType>
    {
        public ReturnType ReturnedObject { get; set; }

        public ServiceResult(ResultType result) : base(result) { }

        public ServiceResult(ResultType result, ReturnType returnedObject) : base(result)
        {
            ReturnedObject = returnedObject;
        }

        public ServiceResult(ResultType result, ICollection<string> errors): base(result, errors) {}
    }
}
