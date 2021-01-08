using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using DevTalk.Customers.Auth;
using DevTalk.Customers.Domain.Contracts;
using DevTalk.Customers.Domain.Entities.Customer;
using DevTalk.Customers.Infrastructure.DbContexts;
using DevTalk.Customers.Infrastructure.Queries;
using DevTalk.Customers.Infrastructure.SqlMappings;
using DevTalk.Customers.Middleware;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace DevTalk.Customers
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

            //DB
            services.AddDbContext<CustomerDbContext>(options => options.UseSqlServer(@"Data Source=reyes-devtalk.database.windows.net;Initial Catalog=Customers;Integrated Security=False;User Id=robertreyes22;Password=&JF760@2dTd&;Encrypt=True;TrustServerCertificate=False;MultipleActiveResultSets=True", x => x.MigrationsAssembly("DevTalk.Customers")));
            services.AddScoped<IDbContext, CustomerDbContext>();

            //Dapper queries
            services.AddScoped<IQueries<Customer>, CustomerQueries>();

            //MediatR
            services.AddMediatR(typeof(Startup));

            //Auth0
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "https://dev-3y955wo3.us.auth0.com/";
                options.Audience = "DevTalk";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Customer", policy => policy.Requirements.Add(new HasScopeRequirement("Customer", "https://dev-3y955wo3.us.auth0.com/")));
            });

            //Mass Transit
            services.AddMassTransit(x =>
            {
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.SelectBasicTier();
                    cfg.Host("Endpoint=sb://reyes-devtalk.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Wf1TqXpzwL+i34UvcdU2yGwqicIXYKUWrzDBEIqQ5Zo=");
                });
            });

            services.AddMassTransitHostedService();

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Customer API",
                    Description = "Manages customers for shopping app"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //custom middleware
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customers API"));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
