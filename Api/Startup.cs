using System;
using System.Text;
using Api.Middlewares;
using AutoMapper;
using Data;
using Domain;
using FluentValidation.AspNetCore;
using Infrastructure.Interfaces;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Languages.Commands;
using Services.Mappings;
using Services.Translations.Queries;
using Swashbuckle.AspNetCore.Swagger;

namespace Api
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
        
            services.AddCors();
            services.AddMediatR(typeof(Details).Assembly);
            services.AddDbContext<DataContext>(opts =>
            {
                opts.EnableDetailedErrors();
                opts.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddControllers()
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Create>());


            services.AddSwaggerGen(opt =>
                {
                    opt.SwaggerDoc("v1",

                        new OpenApiInfo
                        {
                            Title = "VocabularyUp API",
                            Description = "Api for improving english vocabulary",
                            Version = "v1"
                        });
                    opt.CustomSchemaIds(t=>t.FullName);
           
                });

            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            
            
            

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            var builder = services.AddIdentityCore<User>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddEntityFrameworkStores<DataContext>();
            identityBuilder.AddSignInManager<SignInManager<User>>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890veryhard"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        //asagidaki iki setr token expire olandan sonre
                        //logini engellemek ucundu ve 401 unauthorise mesaji gonderir, bulari elave etmesek token
                        //expire olsa bele sayta daxil olmaq olurdu
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<RestErrorHandlingMiddleware>();
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(
                builder => builder
                    .WithOrigins(
                        "http://localhost:8080",
                        "http://localhost:8081",
                        "http://localhost:8082")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            var swaggerOptions = new Options.SwaggerOptions();
            
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            
            app.UseSwagger(opt => { opt.RouteTemplate = swaggerOptions.JsonRoute
                ;});
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerOptions.UIEndpoint,swaggerOptions.Description);
            } );
            // app.UseSwagger();
            // app.UseSwaggerUI(options =>
            // {
            //     options.SwaggerEndpoint("/swagger/v1/swagger.json","VocabularyUp API");
            // } );
        }
    }
}