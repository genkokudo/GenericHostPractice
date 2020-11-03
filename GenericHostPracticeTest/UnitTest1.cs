using ChainingAssertion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace GenericHostPracticeTest
{
    public class UnitTest1
    {
        ///// <summary>
        ///// ログ
        ///// </summary>
        //private readonly ILogger Logger;

        ///// <summary>
        ///// デバッグメッセージの出力
        ///// </summary>
        //private readonly ITestOutputHelper Output;

        //#region 固定データ
        ///// <summary>
        ///// テストデータ
        ///// 時間足
        ///// </summary>
        //private static readonly List<MTimeScale> TimeScales = new List<MTimeScale>
        //    {
        //        new MTimeScale() { Id = 1, DisplayName = "1分", SecondsValue = 60 },
        //        new MTimeScale() { Id = 2, DisplayName = "3分", SecondsValue = 180 },
        //        new MTimeScale() { Id = 3, DisplayName = "5分", SecondsValue = 300 },
        //        new MTimeScale() { Id = 4, DisplayName = "10分", SecondsValue = 600 },
        //        new MTimeScale() { Id = 5, DisplayName = "30分", SecondsValue = 1800 },
        //        new MTimeScale() { Id = 6, DisplayName = "1時間", SecondsValue = 3600 },
        //        new MTimeScale() { Id = 7, DisplayName = "2時間", SecondsValue = 7200 },
        //        new MTimeScale() { Id = 8, DisplayName = "4時間", SecondsValue = 14400 },
        //        new MTimeScale() { Id = 9, DisplayName = "12時間", SecondsValue = 43200 },
        //        new MTimeScale() { Id = 10, DisplayName = "1日", SecondsValue = 86400 }
        //    };

        ///// <summary>
        ///// テストデータ
        ///// 板
        ///// ※通貨設定してないので他のテストで使うとき注意
        ///// </summary>
        //private static readonly List<MBoard> Boards = new List<MBoard>
        //    {
        //        new MBoard() { MBoardId = 11, Name = "BITFLYER_BTC_JPY", DisplayName = "bitFlyer現物BTC" },
        //        new MBoard() { MBoardId = 12, Name = "BITFLYER_FX_BTC_JPY", DisplayName = "bitFlyerFX" },
        //        new MBoard() { MBoardId = 13, Name = "BITFLYER_ETH_BTC", DisplayName = "bitFlyer現物ETH" },
        //        new MBoard() { MBoardId = 14, Name = "BITFLYER_BCH_BTC", DisplayName = "bitFlyer現物BCH" }
        //    };

        ///// <summary>
        ///// テストデータ
        ///// 通貨
        ///// </summary>
        //private static readonly List<MCurrency> Currencies = new List<MCurrency>
        //    {
        //        new MCurrency() { MCurrencyId = 1, Name = "JPY", DisplayName = "円" },
        //        new MCurrency() { MCurrencyId = 2, Name = "BTC", DisplayName = "BTC" },
        //        new MCurrency() { MCurrencyId = 3, Name = "ETH", DisplayName = "Ethereum" },
        //        new MCurrency() { MCurrencyId = 4, Name = "BCH", DisplayName = "Bitcoin Cash" }
        //    };
        //#endregion

        //#region トランザクションテストデータ

        ///// <summary>
        ///// 1分足用Tickerテスト用データ
        ///// </summary>
        //private static readonly List<Ticker> Tickers1m = new List<Ticker>
        //    {
        //        // Tickerを入力していく
        //        // 最初のTickerが入力されたら、ローソクが作成される
        //        // 1分足なので、開始と終了時間は1分ごとに切り捨て
        //        new Ticker() { Id = 1,
        //            BestAsk = 801000, BestAskSize = 0.01M, TotalAskDepth = 5000,
        //            BestBid = 800000, BestBidSize = 0.01M, TotalBidDepth = 5000,
        //            Ltp = 800000,
        //            TickId = 100,
        //            Timestamp = DateTime.Parse("08/18/2018 07:22:16"),
        //            Volume = 120000, VolumeByProduct = 120000
        //        },
        //        new Ticker() { Id = 2,
        //            BestAsk = 802000, BestAskSize = 0.01M, TotalAskDepth = 5000,
        //            BestBid = 800000, BestBidSize = 0.01M, TotalBidDepth = 5000,
        //            Ltp = 802000,
        //            TickId = 100,
        //            Timestamp = DateTime.Parse("08/18/2018 07:22:18"),
        //            Volume = 120000, VolumeByProduct = 120000
        //        },
        //        // 1分経過していない場合、ローソクリストに加えない（境界値テスト）
        //        new Ticker() { Id = 3,
        //            BestAsk = 802000, BestAskSize = 0.01M, TotalAskDepth = 5000,
        //            BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
        //            Ltp = 799000,
        //            TickId = 100,
        //            Timestamp = DateTime.Parse("08/18/2018 07:22:59"),
        //            Volume = 120000, VolumeByProduct = 120000
        //        },
        //        // 1分経過したらローソクリストに加える
        //        // 1分経過した後のTicker価格はローソクリストのローソクに影響しない
        //        // 1分経過した後のTicker価格は新しいローソクに反映する
        //        // 1分経過したらローソクをDBに登録する
        //        new Ticker() { Id = 4,
        //            BestAsk = 803000, BestAskSize = 0.01M, TotalAskDepth = 5000,
        //            BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
        //            Ltp = 803000,
        //            TickId = 100,
        //            Timestamp = DateTime.Parse("08/18/2018 07:23:00"),
        //            Volume = 120000, VolumeByProduct = 120000
        //        },
        //        // 5分経過したらローソク5本になっており、DBに全て登録されている
        //        // 間のローソクはMin = Maxになっている
        //        new Ticker() { Id = 5,
        //            BestAsk = 805000, BestAskSize = 0.01M, TotalAskDepth = 5000,
        //            BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
        //            Ltp = 805000,
        //            TickId = 100,
        //            Timestamp = DateTime.Parse("08/18/2018 07:27:00"),
        //            Volume = 120000, VolumeByProduct = 120000
        //        },
        //        // 101分経過したらメモリの最初のローソクを削除する
        //        new Ticker() { Id = 6,
        //            BestAsk = 806000, BestAskSize = 0.01M, TotalAskDepth = 5000,
        //            BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
        //            Ltp = 806000,
        //            TickId = 100,
        //            Timestamp = DateTime.Parse("08/18/2018 09:03:00"),
        //            Volume = 120000, VolumeByProduct = 120000
        //        }
        //    };

        ///// <summary>
        ///// 5分足用Tickerテスト用データ
        ///// </summary>
        //private static readonly List<Ticker> Tickers5m = new List<Ticker>
        //    {
        //        // 親にTickerを入力していく
        //        // 最初のTickerが入力されたら、ローソクが作成される
        //        // 5分足なので、開始と終了時間は5分ごとに切り捨て
        //        new Ticker() { Id = 1,
        //            BestAsk = 801000, BestAskSize = 0.01M, TotalAskDepth = 5000,
        //            BestBid = 800000, BestBidSize = 0.01M, TotalBidDepth = 5000,
        //            Ltp = 800000,
        //            TickId = 100,
        //            Timestamp = DateTime.Parse("08/18/2018 07:22:16"),
        //            Volume = 120000, VolumeByProduct = 120000
        //        },
        //        // 1分経過したらローソクが更新される
        //        new Ticker() { Id = 2,
        //            BestAsk = 802000, BestAskSize = 0.01M, TotalAskDepth = 5000,
        //            BestBid = 800000, BestBidSize = 0.01M, TotalBidDepth = 5000,
        //            Ltp = 802000,
        //            TickId = 100,
        //            Timestamp = DateTime.Parse("08/18/2018 07:23:00"),
        //            Volume = 120000, VolumeByProduct = 120000
        //        },
        //        // 5分経過していない場合、ローソクリストに加えない（境界値テスト）
        //        new Ticker() { Id = 3,
        //            BestAsk = 802000, BestAskSize = 0.01M, TotalAskDepth = 5000,
        //            BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
        //            Ltp = 799000,
        //            TickId = 100,
        //            Timestamp = DateTime.Parse("08/18/2018 07:24:59"),
        //            Volume = 120000, VolumeByProduct = 120000
        //        },
        //        // 5分経過したらローソクリストに加える
        //        // 5分経過した後のTicker価格はローソクリストのローソクに影響しない
        //        // 5分経過した後のTicker価格は新しいローソクに反映する
        //        // 5分経過したらローソクをDBに登録する
        //        new Ticker() { Id = 4,
        //            BestAsk = 803000, BestAskSize = 0.01M, TotalAskDepth = 5000,
        //            BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
        //            Ltp = 803000,
        //            TickId = 100,
        //            Timestamp = DateTime.Parse("08/18/2018 07:25:00"),
        //            Volume = 120000, VolumeByProduct = 120000
        //        },
        //        // 15分経過したらローソク3本になっており、DBに全て登録されている
        //        // 間のローソクはMin = Maxになっている
        //        new Ticker() { Id = 5,
        //            BestAsk = 805000, BestAskSize = 0.01M, TotalAskDepth = 5000,
        //            BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
        //            Ltp = 805000,
        //            TickId = 100,
        //            Timestamp = DateTime.Parse("08/18/2018 07:35:00"),
        //            Volume = 120000, VolumeByProduct = 120000
        //        },
        //        // 505分経過したらDBの最初のローソクを削除する
        //        new Ticker() { Id = 6,
        //            BestAsk = 806000, BestAskSize = 0.01M, TotalAskDepth = 5000,
        //            BestBid = 799000, BestBidSize = 0.01M, TotalBidDepth = 5000,
        //            Ltp = 806000,
        //            TickId = 100,
        //            Timestamp = DateTime.Parse("08/18/2018 15:45:00"),
        //            Volume = 120000, VolumeByProduct = 120000
        //        }
        //    };

        //#endregion

        //#region テストデータ
        ///// <summary>
        ///// CandleMaker生成のテストデータ
        ///// ※Theoryの理解用に作成した、本当はFactの方が適切
        ///// </summary>
        //public static IEnumerable<object[]> CandleMakerTestDataProp
        //{
        //    get
        //    {
        //        yield return new object[] { TimeScales, Boards[0] };
        //        yield return new object[] { TimeScales, Boards[1] };
        //    }
        //}

        //#endregion

        ///// <summary>
        ///// コンストラクタ
        ///// </summary>
        ///// <param name="output"></param>
        //public CandleMakerTest(ITestOutputHelper output)
        //{
        //    Output = output;

        //    // ロガー作成
        //    var serviceProvider = new ServiceCollection()
        //        .AddLogging()
        //        .BuildServiceProvider();

        //    var factory = serviceProvider.GetService<ILoggerFactory>().AddNLog();

        //    Logger = factory.CreateLogger<CandleMakerTest>();
        //}

        //[Theory(DisplayName = "CandleMaker作成")]
        //[MemberData(nameof(CandleMakerTestDataProp))]
        //[Trait("Category", "CandleMakerの作成")]
        //public void Test1(List<MTimeScale> timeScales, MBoard board)
        //{
        //    // InMemoryDBを使うオプションを作成
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(databaseName: "Test1")
        //        .Options;

        //    CandleMaker data = null;
        //    using (var dbContext = new ApplicationDbContext(options)) // InMemoryDBを使うオプション設定を渡す
        //    {
        //        // インスタンス作成
        //        data = CandleMaker.MakeGeneration(Logger, dbContext, timeScales, board);
        //    }

        //    // 名前の確認
        //    data.TimeScale.DisplayName.Is("1分");

        //    // この時間足データの場合、一番親の子（2代目）が2つあること
        //    data.Children.Count.Is(2);

        //    // 約数があって一番大きい秒数の子が親から辿れること
        //    // 表示名が正しく設定されていること
        //    data.Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].TimeScale.DisplayName.Is("1日");
        //}

        //[Fact(DisplayName = "CandleMaker親のテスト")]
        //[Trait("Category", "CandleMakerの動作")]
        //public void Test2()
        //{
        //    // InMemoryDBを使うオプションを作成
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(databaseName: "Test2")
        //        .Options;

        //    CandleMaker data = null;
        //    using (var dbContext = new ApplicationDbContext(options)) // InMemoryDBを使うオプション設定を渡す
        //    {
        //        // インスタンス作成
        //        data = CandleMaker.MakeGeneration(Logger, dbContext, TimeScales, Boards[0]);

        //        // Tickerを入力していく
        //        data.Update(Tickers1m[0]); // "08/18/2018 07:22:16" 800000
        //        // 最初のTickerが入力されたら、ローソクが作成される
        //        data.CurrentCandle.IsNotNull();
        //        data.CurrentCandle.Begin.Is(800000);
        //        data.CurrentCandle.End.Is(800000);
        //        data.CurrentCandle.Min.Is(800000);
        //        data.CurrentCandle.Max.Is(800000);
        //        // 1分足なので、開始と終了時間は1分ごとに切り捨て
        //        data.CurrentCandle.BeginTime.Is(DateTime.Parse("08/18/2018 07:22:00"));
        //        data.CurrentCandle.EndTime.Is(DateTime.Parse("08/18/2018 07:23:00"));

        //        // ローソク更新確認:高値更新
        //        data.Update(Tickers1m[1]);  // "08/18/2018 07:22:18" 802000
        //        data.CurrentCandle.Begin.Is(800000);
        //        data.CurrentCandle.End.Is(802000);
        //        data.CurrentCandle.Min.Is(800000);
        //        data.CurrentCandle.Max.Is(802000);

        //        // ローソク更新確認:安値更新
        //        // 1分経過していない場合、ローソクリストに加えない（境界値テスト）
        //        data.Update(Tickers1m[2]);  // "08/18/2018 07:22:59" 799000
        //        data.CurrentCandle.Begin.Is(800000);
        //        data.CurrentCandle.End.Is(799000);
        //        data.CurrentCandle.Min.Is(799000);
        //        data.CurrentCandle.Max.Is(802000);
        //        data.CandleList.Count.Is(0);

        //        // 1分経過したらローソクリストに加える
        //        // 1分経過した後のTicker価格はローソクリストのローソクに影響しない
        //        // 1分経過した後のTicker価格は新しいローソクに反映する
        //        data.Update(Tickers1m[3]); // "08/18/2018 07:23:00" 803000
        //        data.CandleList.Count.Is(1);
        //        data.CandleList[0].Begin.Is(800000);
        //        data.CandleList[0].End.Is(799000);
        //        data.CandleList[0].Min.Is(799000);
        //        data.CandleList[0].Max.Is(802000);
        //        data.CurrentCandle.Begin.Is(803000);
        //        data.CurrentCandle.End.Is(803000);
        //        data.CurrentCandle.Min.Is(803000);
        //        data.CurrentCandle.Max.Is(803000);

        //        // 5分経過したらローソク5本になっている
        //        // 間のローソクはMin = Maxになっている
        //        data.Update(Tickers1m[4]); // "08/18/2018 07:27:00" 805000
        //        data.CandleList.Count.Is(5);
        //        data.CandleList[3].Begin.Is(803000);
        //        data.CandleList[3].End.Is(803000);
        //        data.CandleList[3].Min.Is(803000);
        //        data.CandleList[3].Max.Is(803000);
        //        data.CandleList[4].Begin.Is(803000);
        //        data.CandleList[4].End.Is(803000);
        //        data.CandleList[4].Min.Is(803000);
        //        data.CandleList[4].Max.Is(803000);
        //        data.CurrentCandle.Begin.Is(805000);
        //        data.CurrentCandle.End.Is(805000);
        //        data.CurrentCandle.Min.Is(805000);
        //        data.CurrentCandle.Max.Is(805000);

        //        // 101分経過したらメモリの最初のローソクを削除する
        //        data.Update(Tickers1m[5]); // "08/18/2018 09:08:00" 806000
        //        data.CandleList.Count.Is(SystemConstants.MaxCandle);
        //        data.CandleList[0].BeginTime.Is(DateTime.Parse("08/18/2018 07:23:00"));
        //    }
        //}

        //[Fact(DisplayName = "CandleMaker子のテスト")]
        //[Trait("Category", "CandleMakerの動作")]
        //public void Test3()
        //{
        //    // InMemoryDBを使うオプションを作成
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(databaseName: "Test3")
        //        .Options;

        //    CandleMaker data = null;
        //    using (var dbContext = new ApplicationDbContext(options)) // InMemoryDBを使うオプション設定を渡す
        //    {
        //        // インスタンス作成
        //        data = CandleMaker.MakeGeneration(Logger, dbContext, TimeScales, Boards[0]);

        //        // 親にTickerを入力していく
        //        data.Update(Tickers5m[0]);  // "08/18/2018 07:22:16" 800000
        //        // 最初のTickerが入力されたら、ローソクが作成される
        //        // 5分足なので、開始と終了時間は5分ごとに切り捨て
        //        data.Children[0].CurrentCandle.BeginTime.Is(DateTime.Parse("08/18/2018 07:20:00"));
        //        data.Children[0].CurrentCandle.EndTime.Is(DateTime.Parse("08/18/2018 07:25:00"));
        //        data.Children[0].CurrentCandle.Begin.Is(800000);
        //        data.Children[0].CurrentCandle.End.Is(800000);
        //        data.Children[0].CurrentCandle.Min.Is(800000);
        //        data.Children[0].CurrentCandle.Max.Is(800000);

        //        // 1分経過したらローソクが更新される
        //        data.Update(Tickers5m[1]);  // "08/18/2018 07:23:00" 802000
        //        data.Children[0].CurrentCandle.Begin.Is(800000);
        //        data.Children[0].CurrentCandle.End.Is(802000);
        //        data.Children[0].CurrentCandle.Min.Is(800000);
        //        data.Children[0].CurrentCandle.Max.Is(802000);

        //        // 5分経過していない場合、ローソクリストに加えない（境界値テスト）
        //        // 安値更新
        //        data.Update(Tickers5m[2]);  // "08/18/2018 07:24:59" 799000
        //        data.Children[0].CurrentCandle.Begin.Is(800000);
        //        data.Children[0].CurrentCandle.End.Is(799000);
        //        data.Children[0].CurrentCandle.Min.Is(799000);
        //        data.Children[0].CurrentCandle.Max.Is(802000);
        //        data.Children[0].CandleList.Count.Is(0);

        //        // 5分経過したらローソクリストに加える
        //        // 5分経過した後のTicker価格はローソクリストのローソクに影響しない
        //        // 5分経過した後のTicker価格は新しいローソクに反映する
        //        data.Update(Tickers5m[3]);  // "08/18/2018 07:25:00" 803000
        //        data.Children[0].CandleList[0].Begin.Is(800000);
        //        data.Children[0].CandleList[0].End.Is(799000);
        //        data.Children[0].CandleList[0].Min.Is(799000);
        //        data.Children[0].CandleList[0].Max.Is(802000);
        //        data.Children[0].CurrentCandle.Begin.Is(803000);
        //        data.Children[0].CurrentCandle.End.Is(803000);
        //        data.Children[0].CurrentCandle.Min.Is(803000);
        //        data.Children[0].CurrentCandle.Max.Is(803000);
        //        data.Children[0].CandleList.Count.Is(1);

        //        // 15分経過したらローソク3本になっている
        //        // 間のローソクはMin = Maxになっている
        //        data.Update(Tickers5m[4]);  // "08/18/2018 07:35:00" 805000
        //        data.Children[0].CandleList.Count.Is(3);
        //        data.Children[0].CandleList[1].Begin.Is(803000);
        //        data.Children[0].CandleList[1].End.Is(803000);
        //        data.Children[0].CandleList[1].Min.Is(803000);
        //        data.Children[0].CandleList[1].Max.Is(803000);
        //        data.Children[0].CandleList[2].Begin.Is(803000);
        //        data.Children[0].CandleList[2].End.Is(803000);
        //        data.Children[0].CandleList[2].Min.Is(803000);
        //        data.Children[0].CandleList[2].Max.Is(803000);
        //        data.Children[0].CurrentCandle.Begin.Is(805000);
        //        data.Children[0].CurrentCandle.End.Is(805000);
        //        data.Children[0].CurrentCandle.Min.Is(805000);
        //        data.Children[0].CurrentCandle.Max.Is(805000);

        //        // 505分経過したらメモリの最初のローソクを削除する
        //        data.Update(Tickers5m[5]);  // "08/18/2018 09:08:00" 806000
        //        data.Children[0].CandleList.Count.Is(SystemConstants.MaxCandle);
        //        data.Children[0].CandleList[0].BeginTime.Is(DateTime.Parse("08/18/2018 07:25:00"));
        //    }
        //}

        //#region xUnitの基本機能
        //[Fact(DisplayName = "xUnitでNLogを使用する")]
        //[Trait("Category", "xUnitの基本機能")]
        //public void HelloLogger()
        //{
        //    Logger.LogTrace("World Trace");
        //    Logger.LogDebug("World Debug");
        //    Logger.LogWarning("World Warn");
        //    Logger.LogError("★★World Error★★");
        //}

        //[Fact(DisplayName = "初めてのInMemoryDatabase")]
        //[Trait("Category", "xUnitの基本機能")]
        //public void AddDatabase()
        //{
        //    // InMemoryDBを使うオプションを作成
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(databaseName: "TestDatabase")
        //        .Options;

        //    // 何かデータを追加。using ステートメントで一度 DB との接続を切る。
        //    using (var context = new ApplicationDbContext(options)) // InMemoryDBを使うオプション設定を渡す
        //    {
        //        foreach (var item in Currencies)
        //        {
        //            context.MCurrencies.Add(item);
        //        }

        //        context.SaveChanges(SystemConstants.SystemName);
        //    }
        //    // 切断

        //    // 再接続してInMemoryDBから値を取り出す
        //    using (var context = new ApplicationDbContext(options))
        //    {
        //        Assert.Equal(4, context.MCurrencies.Count());
        //        var currencie = context.MCurrencies.First(e => e.MCurrencyId == 1);
        //        Assert.Equal("JPY", currencie.Name);
        //        Assert.Equal(SystemConstants.SystemName, currencie.CreatedBy);
        //        Assert.Equal(SystemConstants.SystemName, currencie.UpdatedBy);
        //    }
        //}
        //#endregion
    }
}
