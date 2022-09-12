using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace AuthApi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var buildConfiguration = config.Build();

                    var kvURL = buildConfiguration["KeyVaultConfig:KVurl"];
                    var tenantId = buildConfiguration["KeyVaultConfig:TenantId"];
                    var clientId = buildConfiguration["KeyVaultConfig:ClientId"];
                    var clientSecret = buildConfiguration["KeyVaultConfig:ClientSecretId"];

                    var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

                    var client = new SecretClient(new Uri(kvURL), credential);
                    config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
