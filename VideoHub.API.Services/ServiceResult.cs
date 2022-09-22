using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoHub.API.Services
{
    public class ServiceResult
    {
        public bool Valid { get; set; }
        public string Message { get; set; }

        public ServiceResult()
        {
            Valid = true;
        }
    }

    public class ServiceResult<T> : ServiceResult where T : class
    {
        public T Result { get; set; }
    }
}
