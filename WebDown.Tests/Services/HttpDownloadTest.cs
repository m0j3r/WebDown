namespace HttpDownload.UnitTests.Services;

using System.Threading.Tasks;
using HttpDownload.Services;
using Microsoft.Extensions.Logging;
using Moq;

public class WebDown_Test
{
    [Theory]
    [InlineData("https://dldir1.qq.com/qqfile/qq/PCQQ9.7.1/QQ9.7.1.28940.exe")]
    public async Task GetDownloadFileSize_inQQ_getFileSizeAsync(string url)
    {
        HttpClient httpClient = new();

        var logger = LoggerHelper.LoggerMock<HttpDownload>();

        HttpDownload httpDown = new(httpClient, logger.Object);

        var result = await httpDown.GetDownloadFileSize(url);

        Assert.Equal(220379144, result);
    }

    [Theory]
    [InlineData("")]
    public async Task GetDownloadFileSize_InUrlError_GetNegative(string url)
    {
        HttpClient httpClient = new();

        var logger = LoggerHelper.LoggerMock<HttpDownload>();

        HttpDownload webDown = new(httpClient, logger.Object);

        var result = await webDown.GetDownloadFileSize(url);

        Assert.Equal(-1, result);
    }

    [Theory]
    [InlineData("")]
    public async Task GetDownloadFileSize_InUrlError_OutLoggerError(string url)
    {
        HttpClient httpClient = new();

        var logger = LoggerHelper.LoggerMock<HttpDownload>();

        HttpDownload webDown = new(httpClient, logger.Object);

        await webDown.GetDownloadFileSize(url);

        logger.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Theory]
    [InlineData("https://dldir1.qq.com/qqfile/qq/PCQQ9.7.9/QQ9.7.9.29065.exe")]
    public async Task GetDownloadFileMd5_InQQ_OutMd5(string url)
    {
        HttpClient httpClient = new();

        var logger = LoggerHelper.LoggerMock<HttpDownload>();

        HttpDownload webDown = new(httpClient, logger.Object);

        var result = await webDown.GetDownloadFileMd5(url);

        Assert.Equal("15f3ce0b9803d5c58b3916e7b376e47d", result);
    }

    [Theory]
    [InlineData("")]
    public async Task GetDownloadFileMd5_InErrorUrl_OutEmpty(string url)
    {
        HttpClient httpClient = new();

        var logger = LoggerHelper.LoggerMock<HttpDownload>();

        HttpDownload webDown = new(httpClient, logger.Object);

        var result = await webDown.GetDownloadFileMd5(url);

        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("https://dldir1.qq.com/qqfile/qq/PCQQ9.7.9/QQ9.7.9.29065.exe")]
    public void GetDownFilename_InUrl_OutName(string url)
    {
        HttpClient httpClient = new();

        var logger = LoggerHelper.LoggerMock<HttpDownload>();

        HttpDownload webDown = new(httpClient, logger.Object);

        var result = webDown.GetDownFilename(url);

        Assert.Equal("QQ9.7.9.29065.exe", result);
    }

    [Theory]
    [InlineData("")]
    public void GetDownFilename_In_OutError(string url)
    {
        HttpClient httpClient = new();

        var logger = LoggerHelper.LoggerMock<HttpDownload>();

        HttpDownload webDown = new(httpClient, logger.Object);

        var result = webDown.GetDownFilename(url);

        Assert.Equal(string.Empty, result);
    }
}