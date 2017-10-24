using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

using AutoMapper;

using packt_asp_net_core_and_angular2.Data;
using packt_asp_net_core_and_angular2.Classes;
using packt_asp_net_core_and_angular2.Data.Users;
using AspNet.Security.OpenIdConnect.Primitives;

namespace packt_asp_net_core_and_angular2
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseOpenIddict();
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(conf => {
                conf.User.RequireUniqueEmail = true;
                conf.Password.RequireNonAlphanumeric = false;
                conf.Cookies.ApplicationCookie.AutomaticChallenge = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opts => {
                opts.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                opts.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                opts.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            services.AddSingleton<DbSeeder>();

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            // Register the OpenIddict services.
            services.AddOpenIddict(options =>
            {
                // Register the Entity Framework stores.
                options.AddEntityFrameworkCoreStores<ApplicationDbContext>();

                // Register the ASP.NET Core MVC binder used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                options.AddMvcBinders();

                // Enable the authorization, logout, userinfo, and introspection endpoints.
                options.EnableAuthorizationEndpoint("/api/connect/authorize")
                       .EnableLogoutEndpoint("/connect/logout")
                       .EnableIntrospectionEndpoint("/connect/introspect")
                       .EnableUserinfoEndpoint("/api/userinfo")
                       .EnableTokenEndpoint("/api/connect/token");

                // Note: the sample only uses the implicit code flow but you can enable
                // the other flows if you need to support implicit, password or client credentials.
                options.AllowImplicitFlow();
                options.AllowPasswordFlow();
                options.AllowAuthorizationCodeFlow();
                options.AllowRefreshTokenFlow();

                // During development, you can disable the HTTPS requirement.
                options.DisableHttpsRequirement();

                // Register a new ephemeral key, that is discarded when the application
                // shuts down. Tokens signed using this key are automatically invalidated.
                // This method should only be used during development.
                options.AddEphemeralSigningKey();

                // On production, using a X.509 certificate stored in the machine store is recommended.
                // You can generate a self-signed certificate using Pluralsight's self-cert utility:
                // https://s3.amazonaws.com/pluralsight-free/keith-brown/samples/SelfCert.zip
                //
                // options.AddSigningCertificate("7D2A741FE34CC2C7369237A5F2078988E17A6A75");
                //
                // Alternatively, you can also store the certificate as an embedded .pfx resource
                // directly in this assembly or in a file published alongside this project:
                //
                // options.AddSigningCertificate(
                //     assembly: typeof(Startup).GetTypeInfo().Assembly,
                //     resource: "AuthorizationServer.Certificate.pfx",
                //     password: "OpenIddict");

                // Note: to use JWT access tokens instead of the default
                // encrypted format, the following line is required:
                //
                options.UseJsonWebTokens();
            });

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
                              ILoggerFactory loggerFactory, DbSeeder seeder)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var options = new RewriteOptions()
                .AddIISUrlRewrite(env.ContentRootFileProvider, "IISUrlRewrite.xml");
            app.UseRewriter(options);

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context => {
                    context.Context.Response.Headers["Cache-Control"] = Configuration["StaticFiles:Headers:Cache-Control"];
                    context.Context.Response.Headers["Pragma"] = Configuration["StaticFiles:Headers:Pragma"];
                    context.Context.Response.Headers["Expires"]= Configuration["StaticFiles:Headers:Expires"];
                }
            });

            // Add a custom Jwt Provider to generate Tokens
            // app.UseJwtProvider();
            // Add the Jwt Bearer Header Authentication to validate Tokens
            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                Authority = "http://localhost:5000/",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    // IssuerSigningKey = JwtTokenProvider.SecurityKey,
                    // ValidateIssuerSigningKey = true,
                    // ValidIssuer = JwtTokenProvider.Issuer,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }
            });

            app.UseOpenIddict();

            app.UseMvc();

            try
            {
                seeder.SeedAsync().Wait();
            }
            catch (AggregateException e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}
