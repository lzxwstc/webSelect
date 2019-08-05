using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using JIYITECH.WebApi.Configs;
using JIYITECH.WebApi.Entities;
using JIYITECH.WebApi.Factories;
using JIYITECH.WebApi.Repositories;
using JIYITECH.WebApi.Reposotories;
using JIYITECH.WebApi.Services;
using JIYITECH.WebApi.Tests;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;

namespace JIYITECH.WebApi
{
    public class Startup
    {
        private readonly string rootPath = System.Environment.CurrentDirectory;
        public static ILoggerRepository repository { get; set; }
        public IConfiguration Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            // 根据运行环境选择配置文件
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = configurationBuilder.Build();
            repository = LogManager.CreateRepository("NETCoreRepository");
            // 指定配置文件
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 依赖注入
            // 整个应用程序生命周期以内只创建一个实例
            services.AddSingleton(Configuration);
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            // 每一次GetService都会创建一个新的实例
            services.AddTransient<IDbTransaction, SqlTransaction>();
            services.AddTransient<IUnitOfWork>(provider => new UnitOfWork(Configuration["AppConfig:SQLConnectionStrings"]));
            //services.AddTransient<IUserRepository, UserRepository>();
            //services.AddTransient<IMenuRepository, MenuRepository>();
            //services.AddTransient<IRoleRepository, RoleRepository>();
            //services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            //services.AddTransient<IPermissionRepository, PermissionRepository>();
            //services.AddTransient<IRolePermissionRepository, RolePermissionRepository>();
            //services.AddTransient<IDayDayUpRepository, DayDayUpRepository>();
            //services.AddTransient<IReportsRepository, ReportsRepository>();
            //集中注册服务
            foreach (var item in GetClassName("Service"))
            {
                foreach (var typeArray in item.Value)
                {
                    services.AddScoped(typeArray, item.Key);
                }
            }
            // 静态类静态属性赋值
            SqlServerTableFactory.connectionString = Configuration["AppConfig:SQLConnectionStrings"];
            Demo.connectionString = Configuration["AppConfig:SQLConnectionStrings"];
            // 配置文件注入
            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));
            // swagger配置
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info { Title = "JIYITECH.WebApi API文档", Version = "v1" });
                // 获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                // 为 Swagger JSON and UI设置xml文档注释路径
                var xmlPath = Path.Combine(basePath, "JIYITECH.WebApi.xml");
                x.IncludeXmlComments(xmlPath);
                // 为swagger添加token支持
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                x.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                x.AddSecurityRequirement(security);
            });
            // api权限策略注册
            services.AddAuthorization(x =>
            {
                x.AddPolicy(Permissions.UserCreate, policy => policy.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserCreate)));
                x.AddPolicy(Permissions.UserRead, policy => policy.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserRead)));
                x.AddPolicy(Permissions.UserUpdate, policy => policy.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserUpdate)));
                x.AddPolicy(Permissions.UserDelete, policy => policy.AddRequirements(new PermissionAuthorizationRequirement(Permissions.UserDelete)));
            });
            // jwt鉴权
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  options.RequireHttpsMetadata = false;
                  // 可在鉴权事件触发时进行监听
                  options.Events = new JwtBearerEvents
                  {
                      // 鉴权失败
                      OnAuthenticationFailed = context =>
                      {
                          Console.WriteLine("OnAuthenticationFailed: " +
                              context.Exception.Message);
                          return Task.CompletedTask;
                      },
                      // 鉴权成功
                      OnTokenValidated = context =>
                      {
                          Console.WriteLine("OnTokenValidated: " +
                              context.SecurityToken);
                          return Task.CompletedTask;
                      },
                      // 收到鉴权请求
                      OnMessageReceived = context =>
                      {
                          return Task.CompletedTask;
                      }
                  };
                  // token解析
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      NameClaimType = JwtClaimTypes.Name,
                      RoleClaimType = JwtClaimTypes.Role,
                      ValidIssuer = "http://localhost",
                      ValidAudience = "api",
                      ValidateLifetime = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["AppConfig:TokenSecret"]))
                  };
              });
            // mvc
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // 根据实体类生成数据表
            //SqlServerTableFactory.GenerateTable<Menu>();
            //SqlServerTableFactory.GenerateTable<User>();
            //SqlServerTableFactory.GenerateTable<Role>();
            //SqlServerTableFactory.GenerateTable<Permission>();
            //SqlServerTableFactory.GenerateTable<UserRole>();
            //SqlServerTableFactory.GenerateTable<RolePermission>();
            //SqlServerTableFactory.GenerateTable<DayDayUp>();
            //SqlServerTableFactory.GenerateTable<Reports>();
            // 测试代码
            // Demo.TestDapper();
        }

        /// <summary>  
        /// 获取程序集中的实现类对应的多个接口
        /// </summary>  
        /// <param name="assemblyName">程序集</param>
        public Dictionary<Type, Type[]> GetClassName(string assemblyName)
        {
            if (!String.IsNullOrEmpty(assemblyName))
            {

                Assembly assembly = Assembly.GetExecutingAssembly();
                List<Type> ts = assembly.GetTypes().ToList();

                var result = new Dictionary<Type, Type[]>();
                foreach (var item in ts.Where(s => !s.IsInterface && s.Name.Contains(assemblyName)))
                {
                    var interfaceType = item.GetInterfaces();
                    result.Add(item, interfaceType);
                }
                return result;
            }
            return new Dictionary<Type, Type[]>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // 根据运行环境确定错误页面
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // swagger启用
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "更新日期 2019-04-26");
            });

            // NLog 打印日志 ，原Log4Net 仍保留 
            loggerFactory.AddNLog();
            env.ConfigureNLog("NLog.config");             
            // 测试打印
            try
            {
                string test = null;
                test.ToString();
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                LogHelper.Info(new { a=4});
                // throw;
            }

            // 重定向
            app.UseHttpsRedirection();
            // jwt启用
            app.UseAuthentication();
            // crow跨域启用
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            // mvc启用
            app.UseMvc();

        }
    }
}
