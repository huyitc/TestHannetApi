using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hannet.Api.Extentsions;
using Hannet.Data;
using Hannet.Data.Repository;
using Hannet.Model.Models;
using Hannet.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Linq;

namespace Hannet.Api
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
            //authenticate
            services.AddAuthorization();
            services.AddJwtAuthentication(services.GetApplicationSettings(this.Configuration),Configuration);
            //cấu hình IDentity
            services.AddIdentity();
            //add database
            services.AddDatabase(this.Configuration);
            //sử dụng call stored
            services.AddDbContext<HannetDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("HannetSolutionDB")));
            //cấu hình automaper
            services.AddAutoMapper
                (typeof(MappingProfile));
            //Cấu hình autofac  
            services.AddAutofac();

            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwagger();
           /* services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hannet.Api", Version = "v1" });
            });*/
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ContainerModule());
            builder.RegisterInstance(AutoMapperConfig.Initialize()).SingleInstance();
            builder.RegisterAssemblyTypes(typeof(DeviceRepository).Assembly)
               .Where(t => t.Name.EndsWith("Repository"))
                .As(x => x.GetInterfaces().FirstOrDefault(t => t.Name.EndsWith("Repository")));
            builder.RegisterAssemblyTypes(typeof(DeviceService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .As(x => x.GetInterfaces().FirstOrDefault(t => t.Name.EndsWith("Service")));
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hannet.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = new { error = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));
            app.UseSwaggerUI()
            .UseRouting()
            .UseCors(options => options
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader())
            .UseAuthentication();
            //app.UseStatusCodePagesWithReExecute("/api/errors/{0}");
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
