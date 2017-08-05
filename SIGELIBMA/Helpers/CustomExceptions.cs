using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGELIBMA.Helpers
{


    //public class CustomExceptions : Exception, ICustomExceptions
    //{
    //    #region Public Serializable properties.
    //    [DataMember]
    //    public int ErrorCode { get; set; }
    //    [DataMember]
    //    public string ErrorMessage { get; set; }
    //    [DataMember]
    //    public string user { get; set; }

    //    string reasonPhrase = "ApiException";

    //    [DataMember]
    //    public string ReasonPhrase
    //    {
    //        get { return this.reasonPhrase; }

    //        set { this.reasonPhrase = value; }
    //    }
    //    #endregion
    //}  

    public class EmptyParametersException : Exception
    {
        public EmptyParametersException()
            : base() { }

        public EmptyParametersException(string message)
            : base(message) { }

        public EmptyParametersException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class InvalidFormatException : Exception
    {
        public InvalidFormatException()
            : base() { }

        public InvalidFormatException(string message)
            : base(message) { }

        public InvalidFormatException(string message, Exception innerException)
            : base(message, innerException) { }
    }


    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base() { }

        public NotFoundException(string message)
            : base(message) { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}