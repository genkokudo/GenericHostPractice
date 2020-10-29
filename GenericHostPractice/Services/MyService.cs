using GenericHostPractice.Infrastructure.Database;
using GenericHostPractice.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericHostPractice.Services
{
    /// <summary>
    /// サービス
    /// </summary>
    public interface IMyService
    {
        /// <summary>
        /// インジェクション先から
        /// このメソッドを呼び出すことができる
        /// </summary>
        /// <returns></returns>
        public void MyServiceMethod();
    }

    /// <summary>
    /// 本体
    /// </summary>
    public class MyService : IMyService
    {
        ILogger<MyService> Logger { get; }
        DefaultParameters Options { get; }
        AppDbContext DbContext { get; }

        public MyService(ILogger<MyService> logger, IOptions<DefaultParameters> options, AppDbContext dbContext)
        {
            // ここでインジェクションしたものをクラスフィールドに保持する
            Logger = logger;
            Options = options.Value;
            DbContext = dbContext;
        }
        public void MyServiceMethod()
        {
            Logger.LogTrace("IOptionsで設定内容を表示。");
            Logger.LogTrace($"{Options.DefaultUser.Name}:{Options.DefaultUser.Age}");
            Logger.LogTrace("DBの内容を表示。");
            var tests = DbContext.Tests.ToList();
            foreach (var test in tests)
            {
                Logger.LogTrace($"{test.Name}:{test.Id}");
            }
            Logger.LogTrace("MyServiceを実行しました。");
        }
    }
}
