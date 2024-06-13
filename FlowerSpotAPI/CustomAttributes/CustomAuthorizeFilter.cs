using FlowerSpotCore.InterfacesServices;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FlowerSpotCore.ModelsServices;

namespace FlowerSpotAPI.CustomAttributes
{
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IUsersService _usersService;

        public CustomAuthorizeFilter(IHttpContextAccessor HttpContextAccessor, IUsersService UsersLogic)
        {
            _httpContextAccessor = HttpContextAccessor;
            _usersService = UsersLogic;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_httpContextAccessor.HttpContext!.Request.Headers["Authorization"].Any())
            {
                JwtTokenModel? data = _usersService.ValidateTokenAndGetData(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim());
                if (data != null && data.userId != null)
                {
                    _httpContextAccessor.HttpContext.Items.Add(new KeyValuePair<object, object?>("UserID", (int)data.userId.Value));
                }
                else
                {
                    context.Result = new UnauthorizedObjectResult(string.Empty);
                    return;
                }
            }
            else
            {
                context.Result = new UnauthorizedObjectResult(string.Empty);
                return;
            }
        }
    }
}
