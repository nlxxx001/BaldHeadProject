global using Util;

using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Cache;
using AspNetCoreRateLimit;
using ApiServer.Middlewares;
using ApiServer.Filters;
using ApiServer.Config;
using LeYiXue.Common.Util;
using Util.Helper;

//------------------------------------------------------------------------------------应用创建器------------------------------------------------------------------------------------
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string strEvn = EnvironmentHelper.GetEnvironment(builder.Configuration).ToLower();
if (strEvn == "uat" || strEvn == "production" || strEvn == "dev")
{
    strEvn = "." + strEvn;
}
else
{
    strEvn = "";
}

builder.Configuration.AddJsonFile($"appsettings{strEvn}.json", optional: true, reloadOnChange: true);

//配置限流策略
builder.BaseInit().Services.AddOptions().AddMemoryCache();
    //.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"))
    //.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>()
    //.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>()
    //.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>()
    //.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

bool isOpenUseCrossNginx = bool.Parse(builder.Configuration["Service:OpenUseCrossNginx"] ?? "false");
bool isOnlne = (builder.Configuration["Environment:Type"] ?? "Development") == "Production";

builder.Services.AddControllers().AddJsonOptions(config => {
    //解决文档中样例参数说明中参数首字母变小写的问题
    config.JsonSerializerOptions.PropertyNamingPolicy = null;
});

//配置swagger
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("Common", new OpenApiInfo { Title = "通用接口", Version = "Common" });
    s.SwaggerDoc("Health", new OpenApiInfo { Title = "心跳接口", Version = "Health" });
    s.DocInclusionPredicate((d, a) => a.GroupName.ToUpper() == d.ToUpper());
    string b = Path.GetDirectoryName(typeof(Program).Assembly.Location);
    s.IncludeXmlComments(Path.Combine(b, "API.xml"));
    s.IncludeXmlComments(Path.Combine(b, "DTO.xml"));

    s.AddSecurityDefinition("token", new OpenApiSecurityScheme
    {
        Description = "请输入登录后获取到的 token 以便调试需要token验证的接口",
        Name = "token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference()
                {
                    Id = "token",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
        });
});

//配置拦截器,Json序列化配置,自定义参数验证等
builder.Services.AddControllers(o => o.Filters.AddCustomFilters()).AddNewtonsoftJson(m =>
{
    m.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    m.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    m.SerializerSettings.ContractResolver = new DefaultContractResolver();
}).ConfigureApiBehaviorOptions(a => a.SuppressModelStateInvalidFilter = true);


if (isOnlne && !isOpenUseCrossNginx)//线上并且未启用nginx
{
    //配置端口监听和证书等
    builder.WebHost.ConfigureKestrel(o =>
    {
        o.ListenAnyIP(builder.Configuration["Environment:HttpPort"].ToInt());
        o.ListenAnyIP(builder.Configuration["Environment:HttpsPort"].ToInt(), l => l.UseHttps("7285546__aixueshi.top.pfx", "4hFB8gvT"));
    });
}
//-----------------------------------------------------------------------------------创建应用并启动-----------------------------------------------------------------------------------
WebApplication app = builder.Build();
GlobalStaticService.Init(app.Services, builder.Configuration, args);
app.UseMiddleware<LogIdentityMiddleware>();//日志中间件
//开发调试标识
if (app.Environment.IsDevelopment()) Res.IsTest = true;

////启用中间件验证权限
if (GlobalStaticService.IsUseAuthority)
{
    app.UseMiddleware<AuthorityIdentityMiddleware>();//权限中间件
}

//二级目录，每个应用需要修改成对应的目录信息，替换掉Health
var virtualPath = "";
if (GlobalStaticService.Environment?.ToLower() == "uat")
{
    virtualPath = "/Health";
}

//允许跨域  启用用nginx配置，就不需要配置跨域
if (!GlobalStaticService.OpenUseCrossNginx)
{
    app.UseCors(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
}


if (!isOnlne)
{
    //允许跨域  启用swagger
    app.UseSwagger(s => {
        s.RouteTemplate = "swagger/{documentName}/swagger.json";
        s.PreSerializeFilters.Add((swaggerDoc, httpReq) => {
            swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer {
                Url = virtualPath
            } };
        });
    })
    .UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint($"{virtualPath}/swagger/Health/swagger.json", "Health");
        s.SwaggerEndpoint($"{virtualPath}/swagger/Common/swagger.json", "Common");
        s.EnableFilter();
        s.RoutePrefix = "FTOCUU5XXIDHB0SUPP0NCW4RJ9XA087EJBQPKUQNBCU0NM2XEWXJG3HZXD7Z8H8H7GVG5YQK09ZR45S0H57CEAJUOSORCLUXF1K";
    });

}

//启用接口统计统计
//app.UseCustomMiddleware();

//配置路由  启动程序
app.MapControllers();
if (GlobalStaticService.ServicePort == 0)
{
    //iis部署时候用下面的
    app.Run();
}
else
{
    app.Run($"http://*:{GlobalStaticService.ServicePort}");
}