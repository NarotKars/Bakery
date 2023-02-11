using OnlineStore.Exceptions;
using System.Data.SqlClient;
using System.Net;

namespace OnlineStore
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (SqlException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(e.Message);
            }
            catch(RESTException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                await context.Response.WriteAsync(e.Message);
            }
            catch (AggregateException e)
            {
                foreach (var exception in e.InnerExceptions)
                {
                    if (exception.GetType().Name == nameof(ArgumentException))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await context.Response.WriteAsync(e.Message);
                    }
                    else if (exception.GetType().Name == nameof(SqlException))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(e.Message);
            }
        }
    }
}
