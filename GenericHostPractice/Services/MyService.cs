using GenericHostPractice.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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

        public MyService(ILogger<MyService> logger, IOptions<DefaultParameters> options)
        {
            // ここでインジェクションしたものをクラスフィールドに保持する
            Logger = logger;
            Options = options.Value;
        }
        public void MyServiceMethod()
        {
            Logger.LogTrace("IOptionsで設定内容を表示。");
            Logger.LogTrace($"{Options.DefaultUser.Name}:{Options.DefaultUser.Age}");
            Logger.LogTrace("MyServiceを実行しました。");
        }
    }
}
