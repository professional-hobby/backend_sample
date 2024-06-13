namespace FlowerSpotAPI.CustomMiddlewares
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                //TODO: Put here code to Log the Exception

                httpContext.Response.StatusCode = 500;
            }
        }
    }
}
