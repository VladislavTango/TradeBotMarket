using System.Windows.Controls;

namespace TradeBotMarket.Services
{
    public class NavigationService
    {
        
            private readonly Dictionary<string, Func<Page>> _pages = new Dictionary<string, Func<Page>>();
            private Page _currentPage;

            public Page CurrentPage
            {
                get => _currentPage;
                private set => _currentPage = value;
            }

            public void RegisterPage(string pageKey, Func<Page> pageFactory)
            {
                _pages[pageKey] = pageFactory;
            }

            public void NavigateTo(string pageKey)
            {
                if (_pages.TryGetValue(pageKey, out var pageFactory))
                {
                    CurrentPage = pageFactory();
                }
                else
                {
                    throw new ArgumentException($"Page {pageKey} not registered.");
                }
            }
        }
    }
