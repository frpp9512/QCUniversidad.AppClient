namespace QCUniversidad.WebClient.Models.Shared;

public interface INavigationListViewModel
{
    int CurrentPage { get; set; }
    bool FirstPage { get; }
    bool LastPage { get; }
    int PagesCount { get; set; }
    int TotalItems { get; }
    int ItemsCount { get; }
}