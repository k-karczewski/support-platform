using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SupportPlatform.Database;
using SupportPlatform.Database.Repositories;
using SupportPlatform.Helpers;
using SupportPlatform.Services;
using System.Text;

namespace SupportPlatform
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
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICloudinaryManager, CloudinaryManager>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<UserEntityMapper>();
            services.AddScoped<ReportEntityMapper>();

            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<SupportPlatformDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            IdentityBuilder identityBuilder = services.AddIdentityCore<UserEntity>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
            }).AddErrorDescriber<CustomIdentityErrorDescriber>(); ;

            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(RoleEntity), identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<SupportPlatformDbContext>().AddDefaultTokenProviders();
            identityBuilder.AddUserManager<UserManager<UserEntity>>();
            identityBuilder.AddSignInManager<SignInManager<UserEntity>>();
            identityBuilder.AddRoleManager<RoleManager<RoleEntity>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AuthenticationKeys:DefaultKey").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireClientRole", policy =>
                {
                   policy.RequireRole(RoleNames.Client);
                });

                options.AddPolicy("RequireEmployeeRole", policy =>
                {
                    policy.RequireRole(RoleNames.Employee);
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

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
