using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Helpers
{

    /// <summary>  
    /// IApiExceptions Interface  
    /// </summary>  
    public interface ICustomExceptions
    {
        /// <summary>  
        /// IApiExceptions Interface  
        /// </summary>  

        int ErrorCode { get; set; }

        string ErrorMessage { get; set; }

        string User { get; set; }

        string Method { get; set; }

        string InnerException { get; set; }
    }  
}