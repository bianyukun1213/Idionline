using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Idionline;
using Idionline.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;

// 本地化的一个坑：Satellite assemblies are not produced for registered custom cultures when using .NET Core msbuild https://github.com/dotnet/msbuild/issues/1454
// 简中只能用 zh-Hans，繁中只能用 zh-Hant，否则在 Release\net6.0 和 Release\net6.0\publish 下找不到附属程序集。
// 暂时的解决方案：按照《Asp.net core 2.x/3.x 的 Globalization 和 localization 的使用 (一) 使用方法》 https://www.cnblogs.com/kugar/p/12302100.html
// 提到的执行，从 Debug\net6.0 目录下复制 zh-CN、zh-HK 和 zh-TW。

var builder = WebApplication.CreateBuilder(args);
var migrationOptions = new MongoMigrationOptions
{
    MigrationStrategy = new MigrateMongoMigrationStrategy(),
    BackupStrategy = new CollectionMongoBackupStrategy(),
};
var storageOptions = new MongoStorageOptions
{
    MigrationOptions = migrationOptions,
    CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
};
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Services.Configure<IdionlineSettings>(configuration.GetSection("IdionlineSettings"));
builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("zh-CN");
            });
builder.Services.AddLocalization();
builder.Services.AddHangfire(options => options.UseMongoStorage("mongodb://localhost/db_idionline", storageOptions));
builder.Services.AddHangfireServer(options => { options.WorkerCount = 1; });
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddTransient<DataAccess>();
var app = builder.Build();
var support = new List<CultureInfo>()
            {
                new("zh-CN"),
                new("zh-HK"),
                new("zh-TW")
            };
app.UseRequestLocalization(x =>
{
    x.SetDefaultCulture("zh-CN");
    x.SupportedCultures = support;
    x.SupportedUICultures = support;
    x.AddInitialRequestCultureProvider(new AcceptLanguageHeaderRequestCultureProvider());
});
app.UseHangfireDashboard();
app.UseAuthorization();
app.MapControllers();
app.Run();
RecurringJob.AddOrUpdate<DataAccess>("generate_daily_idiom", x => x.GenLI(), Cron.Daily, new RecurringJobOptions() { TimeZone = TimeZoneInfo.Local });
