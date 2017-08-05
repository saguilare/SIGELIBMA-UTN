using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Http.Tracing;
using SIGELIBMA.Helpers;
using System.Net;
using System.Web.Mvc.Filters;
using System.Text;
using System.Web;


namespace SIGELIBMA.Filters
{

    public class ExceptionFilter : FilterAttribute, IExceptionFilter
        {
            public void OnException(ExceptionContext context)
            {

                var message = new StringBuilder();
                NLogger logger = new NLogger();

                if (context.Exception is EmptyParametersException)
                {
                    message.Append("Empty/invalid parameter found. ");
                }
                else if (context.Exception is InvalidFormatException)
                {
                    message.Append("Empty/invalid format parameter found. ");
                }
                else if (context.Exception is NotFoundException)
                {
                    message.Append("Value not found in method. ");
                }
                else
                {
                    message.Append("Error found in CAG app. ");
                }


                message.Append("Error: " + context.Exception.Message + Environment.NewLine);
                message.Append("Source: " + context.Exception.Source + Environment.NewLine);
                message.Append("StackTrace: " + context.Exception.StackTrace + Environment.NewLine);
                message.Append("Inner Exception: " + context.Exception.InnerException + Environment.NewLine);

                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogFatal(context.Exception.ToString());
            }
        }
    }
