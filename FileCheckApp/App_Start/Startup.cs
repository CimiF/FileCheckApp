using FileCheckApp.App_Start;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;

namespace FileCheckApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IStateKeeper, StateKeeperSingleJsonFile>();
            services.AddTransient<ICheckSum, MD5CheckSum>();
        }

        public IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            return services.BuildServiceProvider();
        }
    }
}