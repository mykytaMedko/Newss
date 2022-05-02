using NewssCore;
using NewssCore.Models;
using NewssCore.Constants;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.Json;

namespace Newss;

public partial class MainPage : ContentPage
{
    private string _apiKey = "5e393e6b41664448b85bb078ef53b78e";
    private HttpClient _hhttpClient;

    public MainPage()
    {
        InitializeComponent();
        _hhttpClient = GetHttpClient();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetArticles(new TopHeadlinesRequest
        {
            Country = Countries.UA,
            Language = Languages.UK
        });

    }

    private void ConfigArticles(IEnumerable<Article> articles)
    {
        foreach (var article in articles)
        {
            if (article.UrlToImage == null) //problem with load images https://github.com/dotnet/maui/issues/6067
                article.UrlToImage = "https://st3.depositphotos.com/23594922/31822/v/600/depositphotos_318221368-stock-illustration-missing-picture-page-for-website.jpg";
            article.SetAuthorAndTime();
        }
    }

    private async void SetCollection(IEnumerable<Article> articles)
    {
        if (articles == null || articles.Count() == 0)
        {
            await DisplayAlert("Повідомлення", "Не знайдено жодної новини на цю тему", "ок");
            return;
        }
        ConfigArticles(articles);
        collectionNews.ItemsSource = articles;
    }

    private HttpClient GetHttpClient()
    {
        var handler = new HttpClientHandler { UseCookies = true, CookieContainer = new CookieContainer() };
        var httpClient = new HttpClient(handler);
        httpClient.DefaultRequestHeaders.Add("user-agent", "News-API-csharp/0.6");
        httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
        return httpClient;
    }

    private void GetArticles(TopHeadlinesRequest request)
    {
        var queryString = QueryString(request);
        var url = "https://newsapi.org/v2/" + "top-headlines" + "?" + queryString;
        MakeRuquest(url);
    }

    private void GetArticles(EverythingRequest request)
    {
        var queryString = QueryString(request);
        var url = "https://newsapi.org/v2/" + "everything" + "?" + queryString;
        MakeRuquest(url);
    }

    public string QueryString(TopHeadlinesRequest request)
    {
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
        return string.Join("&", queryParams.ToArray());
    }

    public string QueryString(EverythingRequest request)
    {
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
        return string.Join("&", queryParams.ToArray());
    }

    private async void MakeRuquest(string url)
    {
        var response = await _hhttpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            await DisplayAlert("Помилка", response.StatusCode.ToString(), "OK");
            return;
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            var articles = JsonSerializer.Deserialize<ApiResponse>(content);
            if (articles.Status == "ok")
            {
                SetCollection(articles.Articles);
            }
            else
            {
                await DisplayAlert("Помилка", articles.Message, "OK");
            }
        }
    }

    private async void collectionNews_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var article = e.CurrentSelection[0] as Article;
        await Browser.OpenAsync(article.Url, new BrowserLaunchOptions
        {
            LaunchMode = BrowserLaunchMode.SystemPreferred,
            TitleMode = BrowserTitleMode.Show,
            PreferredControlColor = Color.FromArgb("#fe988d"),
            PreferredToolbarColor = Color.FromArgb("#fdbd29"),
        });
    }

    private void btnSearch_Clicked(object sender, EventArgs e)
    {
        if (eQuery.Text != null && eQuery.Text.Length > 2)
            GetArticles(new EverythingRequest
            {
                Language = Languages.UK,
                Q = eQuery.Text,
                SortBy = SortBys.PublishedAt
            });
    }
}