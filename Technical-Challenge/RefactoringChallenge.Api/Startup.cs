using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RefactoringChallenge.Common;
using RefactoringChallenge.Entities;
using RefactoringChallenge.Services.Order;

namespace RefactoringChallenge
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
            //services.AddDbContext<NorthwindDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<NorthwindDbContext>(options => options.UseInMemoryDatabase(
                                                                    Configuration.GetValue<string>("InMemeoryDB:DatabaseName")));

            services.AddSingleton(TypeAdapterConfig.GlobalSettings);
            services.AddScoped<IMapper, ServiceMapper>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddControllers();

            services.AddSwaggerGen();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler(err => err.UseGlobalExceptionHandler(env));

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
