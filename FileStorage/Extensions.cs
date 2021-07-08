using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Sdaia.FileStorage.Controllers;
using Sdaia.FileStorage.FileStorageClient;
using Sdaia.FileStorage.Model;

namespace Sdaia.FileStorage
{
    public static class Extensions
    {
        public static IServiceCollection AddFileStorage(this IServiceCollection services, Action<FileStorageOptions> configureOptions)
        {
            var options = new FileStorageOptions();
            configureOptions(options);
            var fileStorageControllerAssembly = typeof(FileController).Assembly;
            var mvcBuilder = services.AddMvc();
            mvcBuilder.AddApplicationPart(fileStorageControllerAssembly).AddControllersAsServices();
            services.AddHttpContextAccessor();
            services.AddScoped<IFileStorageApiClient, FileStorageApiClient>();
            return services;
        }
    }
}
