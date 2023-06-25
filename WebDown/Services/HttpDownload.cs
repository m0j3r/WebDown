namespace HttpDownload.Services;
public class HttpDownload
{
    public readonly HttpClient _httpClient;

    private readonly ILogger<HttpDownload> _logger;

    public HttpDownload(HttpClient httpClient, ILogger<HttpDownload> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// 得到下载文件大小，单位kb
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns> 
    public async Task<long> GetDownloadFileSize(string url)
    {
        try
        {
            var hrm = new HttpRequestMessage(HttpMethod.Head, url);

            var res = await _httpClient.SendAsync(hrm);

            var fileSize = res.Content.Headers.ContentLength ?? -1;

            return fileSize;

        }
        catch (Exception)
        {
            _logger.LogError("无法获得文件大小");
        }

        return -1;
    }

    /// <summary>
    /// 得到下载文件的md5值
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<string> GetDownloadFileMd5(string url)
    {
        try
        {
            var hrm = new HttpRequestMessage(HttpMethod.Head, url);

            var res = await _httpClient.SendAsync(hrm);

            return res.Headers.GetValues("X-Cos-Meta-Md5").First();
        }
        catch (Exception)
        {
            _logger.LogError("文件Md5值不存在");
        }

        return "";
    }

    /// <summary>
    /// 这里得到的文件名是url最后一部分，没有去获得ContentHeader中的文件名
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public string GetDownFilename(string url)
    {
        string fileName = Path.GetFileName(url);

        return fileName;
    }

    public async Task<bool> DownLoad(string url, string filePath)
    {
        bool sign = true;
        filePath += GetDownFilename(url);
        try
        {
            using Stream netStream = await _httpClient.GetStreamAsync(url);
            using FileStream fileStream = File.OpenWrite(filePath);
            await netStream.CopyToAsync(fileStream);
        }
        catch (IOException)
        {
            //这里的异常捕获并不完善，请结合实际操作而定
            sign = false;
            _logger.LogError("Downloader.DownloadFile：请检查文件名是否重复！");
        }

        return sign;
    }

    public string OutFilePath(string url)
    {
        var res = "/home/Data/";
        res += GetDownFilename(url);

        return res;
    }

}
