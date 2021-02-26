using Furion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace FurionTemplate.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSqlSugar(new ConnectionConfig
            {
                ConnectionString = App.Configuration["AppInfo:Name"],//连接符字串
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute//从特性读取主键自增信息
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
