using AuthApi.Api.Constantes;
using AuthApi.Infra.Context;
using AuthApi.Infra.CrossCutting.IoC;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace AuthApi.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthApi.Api", Version = "v1" });
            });

            AddTokenConfigurations(services);

            services.AddDbContext<AuthContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("AuthContext")
                )
            );

            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<AuthContext>()
                .AddDefaultTokenProviders();

            AddConfiguracaoMediatR(services);

            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthApi.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddTokenConfigurations(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                //só deve ser desabilitado em ambiente de dev,
                //porém em produção e homologação deve ser true?
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                };
            });
        }

        private static void AddConfiguracaoMediatR(IServiceCollection services)
        {
            var assemblyCommands = AppDomain.CurrentDomain.Load("AuthApi.Application");


            services.AddMediatR(assemblyCommands);

        }
    }
}
