using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericHostPractice.Services
{
    /// <summary>
    /// サービスを作ってみるテスト
    /// ここで外から呼ぶメソッドだけ定義していく
    /// 何で本体クラスとインタフェースが1対1なんだろ？
    /// 他のクラスにインジェクションする物だったらインタフェース作るって考えであってる？？
    /// </summary>
    public interface IMyService
    {
        /// <summary>
        /// インジェクション先から
        /// このメソッドを呼び出すことができる
        /// </summary>
        /// <returns></returns>
        void MyServiceMethod();
    }

    /// <summary>
    /// 本体
    /// </summary>
    public class MyService : IMyService
    {
        ILogger<MyService> Logger { get; }
        public MyService(ILogger<MyService> logger)
        {
            // ここでインジェクションしたものをクラスフィールドに保持する
            Logger = logger;
        }
        public void MyServiceMethod()
        {
            Logger.LogTrace("aaaa");
        }
    }
}
