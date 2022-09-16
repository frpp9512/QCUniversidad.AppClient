using System.Collections;

namespace QCUniversidad.WebClient.Models
{
    public interface INavigationListViewModel
    {
        int CurrentPage { get; set; }
        bool FirstPage { get; }
        bool LastPage { get; }
        int PagesCount { get; set; }
        int TotalItems { get; }
        int ItemsCount { get; }
    }
}