﻿namespace QCUniversidad.WebClient.Models.Shared;

public class NavigationListViewModel<T> : INavigationListViewModel
{
    public IList<T>? Items { get; set; }
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int PagesCount { get; set; }
    public bool FirstPage => CurrentPage == 1;
    public bool LastPage => CurrentPage == PagesCount;
    public int ItemsCount => Items.Count;
}