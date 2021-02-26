using Furion;
using FurionTemplate.Common.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FurionTemplate.Web.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //注入Redis
            services.AddSingleton<IRedisCacheManager, NewLifeRedisCacheManager>();

            services.AddJwt();

            services.AddCorsAccessor();

            services.AddControllers()
                    .AddInjectWithUnifyResult().AddJsonOptions(option =>
                     {
                         //忽略空
                         option.JsonSerializerOptions.IgnoreNullValues = false;
                         //返回json小写
                         option.JsonSerializerOptions.PropertyNamingPolicy = new LowercasePolicy();
                         //时间格式格式化
                         option.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                         option.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());

                     });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseUnifyResultStatusCodes();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCorsAccessor();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseInject(string.Empty);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
