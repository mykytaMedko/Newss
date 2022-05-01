using Newss.Core;
using Newss.Core.Models;
using Newss.Core.Constants;
using System.Collections.ObjectModel;
using System.Linq;

namespace Newss;

public partial class MainPage : ContentPage
{
    private readonly NewsApiClient _newsClient;
    private ObservableCollection<Article> newsArticle;
    private string apiKey = "5e393e6b41664448b85bb078ef53b78e";
    private string _query;
    public MainPage()
    {
        InitializeComponent();
        _newsClient = new NewsApiClient(apiKey);

        LoadInitArticles();
    }

    public void LoadInitArticles(string q = null)
    {
        if (q == null)
            q = "Україна";

        var result = _newsClient.GetEverything(new EverythingRequest
        {
            Language = Languages.UK,
            SortBy = SortBys.Popularity,
            PageSize = 30,
            Q = q
        });

        //var result = GetErrorMock();
        //var result = GetOkMock();

        if (result.Status == Statuses.Error)
        {
            DisplayAlert("Помилка", result.Error.Message, "OK");
            return;
        }
        newsArticle = new ObservableCollection<Article>();
        foreach (var article in result.Articles)
        {
            newsArticle.Add(article);
        }
        newsView.ItemsSource = newsArticle;
    }

    public ArticlesResult GetOkMock()
    {
        var result = new ArticlesResult();
        result.Error = null;
        result.TotalResults = 25;
        result.Status = Statuses.Ok;

        var sd = new List<Article>();
        for (int i = 1; i <= result.TotalResults; i++)
        {
            sd.Add(new Article
            {
                Title = $"Title {i}",
                Description = $"Some desc {i}",
                Author = $"Author {i}",
                PublishedAt = DateTime.Today.AddDays(-i),
                Url = "https://www.msn.com/en-xl/europe/top-stories/pelosi-in-surprise-kyiv-trip-vows-u-s-support-until-the-fight-is-done/ar-AAWNAko?ocid=msedgntp&cvid=bbf271aba6844391b5f3d14bb3bed654",
                UrlToImage = "https://images.unian.net/photos/2022_04/1649313913-1098.jpg?r=389264",
                Source = new Source
                {
                    Name = ""
                }
            });
        }
        result.Articles = sd;
        return result;
    }

    public ArticlesResult GetErrorMock()
    {
        return new ArticlesResult
        {
            Error = new Error { Message = "", Code = ErrorCodes.ApiKeyDisabled },
            Status = Statuses.Error,
            TotalResults = 0,
            Articles = null
        };
    }

    private void newsView_Scrolled(object sender, ScrolledEventArgs e)
    {


        //var listView = sender as ListView;
        //double listViewHeight = (newsArticle.Count * listView.Height) / 2;
        //if (listViewHeight > e.ScrollY)
        //    return;





        //var listView = sender as ListView;
        //double scrollingSpace = listView.Height;
        //if (scrollingSpace > e.ScrollY)
        //    return;

        //SetBusy(true);

        //Application.Current.Dispatcher.Dispatch(async () =>
        //{
        //    var result = GetErrorMock(); //await _newsClient.FetchNewsAsync(
        //                                 //new TopHeadlinesRequest(_query, Country.Ukraine)
        //                                 //);

        //    if (result.ResponseStatus == ResponseStatus.Error)
        //    {
        //        await DisplayAlert("Помилка", result.Error.Value.ToString(), "OK");
        //        return;
        //    }
        //    newsArticle = new ObservableCollection<NewsArticle>();
        //    foreach (var article in result.Articles)
        //    {
        //        newsArticle.Add(article);
        //    }
        //    newsView.ItemsSource = newsArticle;
        //});

        //SetBusy(false);
    }

    //private void SetBusy(bool state)
    //{
    //    indicator.IsRunning = state;
    //    indicator.IsVisible = state;
    //    indicator.Color = Color.FromArgb("#e92323");
    //}

    private async void newsView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var article = e.SelectedItem as Article;
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
        var query = eQuery.Text;
        if (query.Length >= 3)
        {
            LoadInitArticles(query);
        }
    }

    private void eQuery_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (eQuery.Text.Length == 0)
        {
            LoadInitArticles();
        }
    }
}