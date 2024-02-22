namespace QCUniversidad.WebClient.Models.Accounts;

public class AccountManagamentViewModel
{
    public required IEnumerable<UserViewModel> Users { get; set; }

    public int PagesCount { get; set; }

    public int CurrentPage { get; set; }

    public int UsersPerPage { get; set; }

    public int UsersCount { get; set; }

    public bool FirstPage => CurrentPage == 1;

    public bool LastPage => CurrentPage == PagesCount;
}