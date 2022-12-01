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

//------------------------------------------------------------------------------------Ӧ�ô�����------------------------------------------------------------------------------------
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

//������������
builder.BaseInit().Services.AddOptions().AddMemoryCache();
    //.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"))
    //.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>()
    //.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>()
    //.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>()
    //.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

bool isOpenUseCrossNginx = bool.Parse(builder.Configuration["Service:OpenUseCrossNginx"] ?? "false");
bool isOnlne = (builder.Configuration["Environment:Type"] ?? "Development") == "Production";

builder.Services.AddControllers().AddJsonOptions(config => {
    //����ĵ�����������˵���в�������ĸ��Сд������
    config.JsonSerializerOptions.PropertyNamingPolicy = null;
});

//����swagger
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("Common", new OpenApiInfo { Title = "ͨ�ýӿ�", Version = "Common" });
    s.SwaggerDoc("Health", new OpenApiInfo { Title = "�����ӿ�", Version = "Health" });
    s.DocInclusionPredicate((d, a) => a.GroupName.ToUpper() == d.ToUpper());
    string b = Path.GetDirectoryName(typeof(Program).Assembly.Location);
    s.IncludeXmlComments(Path.Combine(b, "API.xml"));
    s.IncludeXmlComments(Path.Combine(b, "DTO.xml"));

    s.AddSecurityDefinition("token", new OpenApiSecurityScheme
    {
        Description = "�������¼���ȡ���� token �Ա������Ҫtoken��֤�Ľӿ�",
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

//����������,Json���л�����,�Զ��������֤��
builder.Services.AddControllers(o => o.Filters.AddCustomFilters()).AddNewtonsoftJson(m =>
{
    m.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    m.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    m.SerializerSettings.ContractResolver = new DefaultContractResolver();
}).ConfigureApiBehaviorOptions(a => a.SuppressModelStateInvalidFilter = true);


if (isOnlne && !isOpenUseCrossNginx)//���ϲ���δ����nginx
{
    //���ö˿ڼ�����֤���
    builder.WebHost.ConfigureKestrel(o =>
    {
        o.ListenAnyIP(builder.Configuration["Environment:HttpPort"].ToInt());
        o.ListenAnyIP(builder.Configuration["Environment:HttpsPort"].ToInt(), l => l.UseHttps("7285546__aixueshi.top.pfx", "4hFB8gvT"));
    });
}
//-----------------------------------------------------------------------------------����Ӧ�ò�����-----------------------------------------------------------------------------------
WebApplication app = builder.Build();
GlobalStaticService.Init(app.Services, builder.Configuration, args);
app.UseMiddleware<LogIdentityMiddleware>();//��־�м��
//�������Ա�ʶ
if (app.Environment.IsDevelopment()) Res.IsTest = true;

////�����м����֤Ȩ��
if (GlobalStaticService.IsUseAuthority)
{
    app.UseMiddleware<AuthorityIdentityMiddleware>();//Ȩ���м��
}

//����Ŀ¼��ÿ��Ӧ����Ҫ�޸ĳɶ�Ӧ��Ŀ¼��Ϣ���滻��Health
var virtualPath = "";
if (GlobalStaticService.Environment?.ToLower() == "uat")
{
    virtualPath = "/Health";
}

//�������  ������nginx���ã��Ͳ���Ҫ���ÿ���
if (!GlobalStaticService.OpenUseCrossNginx)
{
    app.UseCors(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
}


if (!isOnlne)
{
    //�������  ����swagger
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

//���ýӿ�ͳ��ͳ��
//app.UseCustomMiddleware();

//����·��  ��������
app.MapControllers();
if (GlobalStaticService.ServicePort == 0)
{
    //iis����ʱ���������
    app.Run();
}
else
{
    app.Run($"http://*:{GlobalStaticService.ServicePort}");
}