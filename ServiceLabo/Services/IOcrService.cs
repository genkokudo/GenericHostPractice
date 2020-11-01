using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;

// C:\Program Files (x86)\Windows Kits\10\UnionMetadata\10.0.16299.0\Windows.winmd を参照に追加
// PM: Install-Package System.Runtime.WindowsRuntime
namespace ServiceLabo.Services
{
    /// <summary>
    /// OCRサービス
    /// </summary>
    public interface IOcrService
    {
        /// <summary>
        /// 指定した画像内に含まれる文字情報を取得します
        /// </summary>
        /// <param name="filename">画像ファイル名</param>
        /// <returns>OCR結果</returns>
        public OcrResult Recognize(string filename);
    }

    /// <summary>
    /// 本体
    /// </summary>
    public class OcrService : IOcrService
    {
        ILogger<OcrService> Logger { get; }

        public OcrService(ILogger<OcrService> logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// 指定した画像内に含まれる文字情報を取得します
        /// </summary>
        /// <param name="filename">画像ファイル名</param>
        /// <returns>OCR結果</returns>
        public OcrResult Recognize(string filename)
        {
            Task<OcrResult> result = OcrMain(filename);
            result.Wait();
            return result.Result;
        }
        private async Task<OcrResult> OcrMain(string filename)
        {
            OcrEngine ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
            var bitmap = await LoadImage(filename);
            var ocrResult = await ocrEngine.RecognizeAsync(bitmap);
            return ocrResult;
        }
        private async Task<SoftwareBitmap> LoadImage(string path)
        {
            var fs = File.OpenRead(path);
            var buf = new byte[fs.Length];
            fs.Read(buf, 0, (int)fs.Length);
            var mem = new MemoryStream(buf);
            mem.Position = 0;

            var stream = await ConvertToRandomAccessStream(mem);
            var bitmap = await LoadImage(stream);
            return bitmap;
        }
        private async Task<SoftwareBitmap> LoadImage(IRandomAccessStream stream)
        {
            var decoder = await BitmapDecoder.CreateAsync(stream);
            var bitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            return bitmap;
        }
        private async Task<IRandomAccessStream> ConvertToRandomAccessStream(MemoryStream memoryStream)
        {
            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            var dw = new DataWriter(outputStream);
            var task = new Task(() => dw.WriteBytes(memoryStream.ToArray()));
            task.Start();
            await task;
            await dw.StoreAsync();
            await outputStream.FlushAsync();
            return randomAccessStream;
        }
    }
}
