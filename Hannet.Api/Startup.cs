using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hannet.Api.Extentsions;
using Hannet.Data;
using Hannet.Data.Repository;
using Hannet.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            
            //sử dụng call stored
            services.AddDbContext<HannetDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("HannetSolutionDB")));
            //cấu hình automaper
          
            services.AddAutoMapper
                (typeof(MappingProfile));
            //Cấu hình autofac
            services.AddAutofac();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hannet.Api", Version = "v1" });
            });
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
