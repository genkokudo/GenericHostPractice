using GenericHostPractice.Infrastructure.Database;
using GenericHostPractice.Infrastructure.Settings;
using GenericHostPractice.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Hosting;
using System;

namespace GenericHostPractice
{
    class Program
    {
        #region Main
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                // ホストを作成して実行
                var host = CreateWebHostBuilder(args).Build();

                // DBに初期値を登録
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        // DBをマイグレーションする
                        var context = services.GetRequiredService<AppDbContext>();
                        context.Database.Migrate();
                        // マスタデータの初期化が必要な場合、こういうクラスを作成する
                        //DbInitializer.Initialize(context);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "An error occurred while seeding the database.");
                    }
                }

                host.Run();
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
        #endregion

        #region CreateWebHostBuilder
        /// <summary>
        /// マイグレーションのためにこのようなメソッドを作成する必要がある
        /// ホストを作成する
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            // 作成した設定を保持
            IConfiguration configuration = null;

            // 環境変数から開発環境か本番系かを取得
            // hostContext.HostingEnvironment.EnvironmentNameだと何故か常にProductionになってしまう
            var envName = SystemConstants.EnvDevelopment;
            envName = Environment.GetEnvironmentVariable(SystemConstants.EnxEnv) ?? envName;   // マイグレーションでは無視される

            return new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    // 設定ファイルと環境変数を読み込んで保持
                    // ここで設定したらIConfiguration configurationでインジェクションできる
                    // 階層的な項目はconfiguration.GetValueの引数で、":"で区切って指定
                    // AddJsonFileで存在しなくても良い場合はoptional = trueを指定
                    configuration = configApp
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.{envName}.json")
                        .AddEnvironmentVariables(prefix: SystemConstants.PrefixEnv) // "DOTNETCORE_"が付く環境変数を読み込む
                        .Build();
                })
                .ConfigureServices((services) =>
                {
                    // サービス処理のDI        
                    // 設定ファイルの読み込み
                    services.Configure<DefaultParameters>(configuration.GetSection("UserSettings"));
                    // ログ
                    services.AddLogging();
                    // テスト（シングルトンで追加する方法）
                    //services.AddSingleton<IMyService, MyService>();
                    // テスト（呼び出すたびにインスタンス作成する方法）
                    services.AddTransient<IMyService, MyService>();
                    // メインロジック
                    // IHostedServiceを実装すると、AddHostedServiceで指定することで動かせる。
                    services.AddHostedService<Application>();

                    // DB接続
                    var constr = configuration.GetValue<string>($"ConnectionStrings:{SystemConstants.Connection}");
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(constr, assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
                    );
                })
                .ConfigureLogging((context, config) =>
                {
                    // ここで設定すると、ILogger<各クラス名> loggerでインジェクションできる
                    // NLogを追加
                    config.ClearProviders();
                    config.SetMinimumLevel(LogLevel.Trace);  //これがないと正常動作しない
                })
                // NLog を有効にする
                .UseNLog();
        }
        #endregion


    }
}
