using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Storage.Streams;

namespace ServiceLabo.Services
{
    /// <summary>
    /// PDF読み込みサービス
    /// </summary>
    public interface IPdfService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath">PDFファイルパス</param>
        /// <param name="pageNo">読み込むページ（0から数える）</param>
        /// <returns>OCR結果</returns>
        public Task<string> RecognizeAsync(string filepath, uint pageNo);
    }

    /// <summary>
    /// 本体
    /// </summary>
    public class PdfService : IPdfService
    {
        ILogger<PdfService> Logger { get; }

        IOcrService OcrService { get; }

        public PdfService(ILogger<PdfService> logger, IOcrService ocrService)
        {
            Logger = logger;
            OcrService = ocrService;
        }

        public async Task<string> RecognizeAsync(string filename, uint pageNo)
        {
            var fullPath = Path.GetFullPath(filename);
            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(fullPath);

            try
            {
                // PDFファイルを読み込む
                var pdfDocument = await PdfDocument.LoadFromFileAsync(file);

                if (pdfDocument != null)
                {
                    // 1ページ目を読み込む
                    using PdfPage page = pdfDocument.GetPage(pageNo);
                    //var image = new BitmapImage();

                    using var stream = new InMemoryRandomAccessStream();
                    await page.RenderToStreamAsync(stream); // ページを画像ストリームにする
                    var image = await OcrService.LoadImage(stream);
                    return await OcrService.BmpToString(image);
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }
    }
}
