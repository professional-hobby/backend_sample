using FlowerSpotCore.InterfacesRepository;
using FlowerSpotCore.InterfacesServices;
using FlowerSpotCore.InterfacesServices.ExternalAPIs;
using FlowerSpotData.PostgreSQL;
using FlowerSpotLogic;
using FlowerSpotQuoteOfTheDayProvider;
using FlowerSpotRepository;

namespace FlowerSpotAPI
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddScoped<IAllRepositories, AllRepositories>();

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IFlowersService, FlowersService>();
            services.AddScoped<ISightingsService, SightingsService>();
            services.AddScoped<ILikesService, LikesService>();

            services.AddSingleton<IQuoteOfTheDayService, QuoteOfTheDayService>();

            services.AddHttpContextAccessor();
        }
    }
}
