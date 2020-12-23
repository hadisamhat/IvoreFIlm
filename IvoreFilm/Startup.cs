using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using IvoreFilm.Helpers;
using IvoreFilm.Helpers.Authorization;
using IvoreFilm.Helpers.ImageHelper;
using IvoreFilm.Models;
using IvoreFilm.Models.Service;
using IvoreFilm.Repositories.MovieRepository;
using IvoreFilm.Repositories.SearchRepository;
using IvoreFilm.Repositories.SeriesRepository;
using IvoreFilm.Repositories.UserRepository;
using IvoreFilm.Repositories.WatchListRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace IvoreFilm
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(
                options => options.EnableEndpointRouting = false
            );
            services.AddTransient<IWatchListRepository, WatchListRepository>();
            services.AddTransient<ISearchRepository, SearchRepository>();
            services.AddTransient<NewtonsoftJsonSerializer, NewtonsoftJsonSerializer>();
            services.AddTransient<ISerieRepository, SeriesRepository>();
            services.AddTransient<IMovieRepository, MovieRepository>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IImageHelper, ImageHelper>();
            services.AddAutoMapper(AppDomain.CurrentDomain.Load("IvoreFilm"));
            services.AddDbContext<IvoreFilmContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "IvoreFilm API",
                    Description = "IvoreFilm ASP.NET Core Web API"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme, 
                                Id = "Bearer" 
                            }
                        },
                        new string[] {}
 
                    }
                });
              
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            
            app.UseAuthorization();
            
            loggerFactory.AddFile("Logs/MyLogs-{Date}.txt");
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IvoreFilm V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}