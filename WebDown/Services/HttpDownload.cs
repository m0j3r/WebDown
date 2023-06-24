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
            _logger.LogError("无法获得文件Md5");
        }

        return "";
    }
}
