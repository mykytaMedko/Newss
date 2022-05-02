using NewssCore.Constants;
using NewssCore.Models;
using System.Diagnostics;
using System.Text.Json;

namespace NewssCore
{
    public class NewsApiClient
    {
        private string _baseUrl = "https://newsapi.org/v2/";

        private HttpClient _httpClient;

        private string _apiKey;
        public NewsApiClient(string apiKey)
        {
            _apiKey = apiKey;

            _httpClient = new HttpClient();
            //_httpClient.DefaultRequestHeaders.Add("user-agent", "News-API-csharp/0.1");
            //_httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
        }

        public async Task<ArticlesResult> GetTopHeadlinesAsync(TopHeadlinesRequest request)
        {
            // build the querystring
            var queryParams = new List<string>();
            // q
            if (!string.IsNullOrWhiteSpace(request.Q))
            {
                queryParams.Add("q=" + request.Q);
            }

            // sources
            if (request.Sources.Count > 0)
            {
                queryParams.Add("sources=" + string.Join(",", request.Sources));
            }

            if (request.Category.HasValue)
            {
                queryParams.Add("category=" + request.Category.Value.ToString().ToLowerInvariant());
            }

            if (request.Language.HasValue)
            {
                queryParams.Add("language=" + request.Language.Value.ToString().ToLowerInvariant());
            }

            if (request.Country.HasValue)
            {
                queryParams.Add("country=" + request.Country.Value.ToString().ToLowerInvariant());
            }

            // page
            if (request.Page > 1)
            {
                queryParams.Add("page=" + request.Page);
            }

            // page size
            if (request.PageSize > 0)
            {
                queryParams.Add("pageSize=" + request.PageSize);
            }

            // join them together
            var querystring = string.Join("&", queryParams.ToArray());

            return await MakeRequest("top-headlines", querystring);
        }

        public ArticlesResult GetTopHeadlines(TopHeadlinesRequest request)
        {
            return GetTopHeadlinesAsync(request).Result;
        }

        public async Task<ArticlesResult> GetEverythingAsync(EverythingRequest request)
        {
            // build the querystring
            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(_apiKey))
            {
                queryParams.Add("apiKey=" + _apiKey);
            }

            // q
            if (!string.IsNullOrWhiteSpace(request.Q))
            {
                queryParams.Add("q=" + request.Q);
            }

            // sources
            if (request.Sources.Count > 0)
            {
                queryParams.Add("sources=" + string.Join(",", request.Sources));
            }

            // domains
            if (request.Domains.Count > 0)
            {
                queryParams.Add("domains=" + string.Join(",", request.Sources));
            }

            // from
            if (request.From.HasValue)
            {
                queryParams.Add("from=" + string.Format("{0:s}", request.From.Value));
            }

            // to
            if (request.To.HasValue)
            {
                queryParams.Add("to=" + string.Format("{0:s}", request.To.Value));
            }

            // language
            if (request.Language.HasValue)
            {
                queryParams.Add("language=" + request.Language.Value.ToString().ToLowerInvariant());
            }

            // sortBy
            if (request.SortBy.HasValue)
            {
                queryParams.Add("sortBy=" + request.SortBy.Value.ToString());
            }

            // page
            if (request.Page > 1)
            {
                queryParams.Add("page=" + request.Page);
            }

            // page size
            if (request.PageSize > 0)
            {
                queryParams.Add("pageSize=" + request.PageSize);
            }

            // join them together
            var querystring = string.Join("&", queryParams.ToArray());

            return await MakeRequest("everything", querystring);
        }

        public ArticlesResult GetEverything(EverythingRequest request)
        {
            return GetEverythingAsync(request).Result;
        }

        private async Task<ArticlesResult> MakeRequest(string endpoint, string querystring)
        {
            // here's the return obj
            var articlesResult = new ArticlesResult();

            // make the http request
            var url = _baseUrl + endpoint + "?" + querystring;
            var httpResponse = await _httpClient.GetAsync(url);

            var json = await httpResponse.Content?.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(json))
            {
                // convert the json to an obj
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(json);
                articlesResult.Status = apiResponse.Status == "ok" ? Statuses.Ok : Statuses.Error;
                if (articlesResult.Status == Statuses.Ok)
                {
                    articlesResult.TotalResults = apiResponse.TotalResults;
                    articlesResult.Articles = apiResponse.Articles;
                }
                else
                {
                    ErrorCodes errorCode = ErrorCodes.UnknownError;
                    try
                    {
                        errorCode = (ErrorCodes)apiResponse.Code;
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("The API returned an error code that wasn't expected: " + apiResponse.Code);
                    }

                    articlesResult.Error = new Error
                    {
                        Code = errorCode,
                        Message = apiResponse.Message
                    };
                }
            }
            else
            {
                articlesResult.Status = Statuses.Error;
                articlesResult.Error = new Error
                {
                    Code = ErrorCodes.UnexpectedError,
                    Message = "The API returned an empty response. Are you connected to the internet?"
                };
            }

            return articlesResult;
        }
    }
}