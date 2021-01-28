using System;
using System.Linq;
using System.Text;
using AutoMapper;
using TestChat.Api.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace TestChat.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AutoMapperConfiguration.Initialize();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region authentication
            services.AddAuthorization(options =>{});

            services.AddCors();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer( options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    //ValidAudience = "the audience you want to validate",
                    ValidateIssuer = false,
                    //ValidIssuer = "the isser you want to validate",

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsATestChatApp")),

                    ValidateLifetime = true, //validate the expiration and not before values in the token

                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
            });

            #endregion

            services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                options.JsonSerializerOptions.IgnoreNullValues = true;
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                var resolver = options.SerializerSettings.ContractResolver;
                if (resolver != null)
                {
                    (resolver as DefaultContractResolver).NamingStrategy = null;
                }
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddCors(options => options.AddPolicy(
                "CorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyHeader()
                               .AllowAnyMethod()
                               .SetIsOriginAllowed((host) => true)
                               .AllowCredentials()
                               .WithOrigins("http://localhost:4200", "https://localhost:4200");
                    }
                )
            );

            services.AddSignalR();

            #region DI

            #region DbContext
            //services.AddDbContext<TestChat.Data.Contexts.ChatDbContext>(options => options.UseMySql(Configuration.GetConnectionString("MySql"), x => x.MigrationsAssembly("TestChat.Data")));
            services.AddDbContext<TestChat.Data.Contexts.ChatDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Sql"), x => x.MigrationsAssembly("TestChat.Data")));
            #endregion DbContext

            #region UnitOfWork
            services.AddScoped<TestChat.Core.Repositories.IUnitOfWork, TestChat.Data.Repositories.UnitOfWork>();
            #endregion UnitOfWork

            #region TestChat Repository & Service
            #region Repository
            services.AddTransient<TestChat.Core.Repositories.IUserRepository, TestChat.Data.Repositories.UserRepository>();
            services.AddTransient<TestChat.Core.Repositories.IUserChatRepository, TestChat.Data.Repositories.UserChatRepository>();
            #endregion Repository

            #region Service
            services.AddTransient<TestChat.Core.Services.IUserService, TestChat.Services.Services.UserService>();
            services.AddTransient<TestChat.Core.Services.IUserChatService, TestChat.Services.Services.UserChatService>();
            #endregion Service
            #endregion TestChat Repository &Service          

            #endregion DI

            services.AddHttpContextAccessor();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // No consent check needed here
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            //app.UseCors(s => s.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();
           
            app.UseCookiePolicy();
        
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatsocket").RequireCors("CorsPolicy");
            });
        }
    }
}