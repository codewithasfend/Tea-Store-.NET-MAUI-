using TeaStoreApp.Models;
using TeaStoreApp.Services;

namespace TeaStoreApp.Pages;

public partial class FavoritesPage : ContentPage
{
    private BookmarkItemService bookmarkItemService;
    public FavoritesPage()
    {
        InitializeComponent();
        bookmarkItemService = new BookmarkItemService();
    }

    private void CvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as BookmarkedProduct;
        if (currentSelection == null) return;
        Navigation.PushAsync(new ProductDetailPage(currentSelection.ProductId));
        ((CollectionView)sender).SelectedItem = null;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetFavoriteProducts();
    }

    private void GetFavoriteProducts()
    {
        var favoriteProducts = bookmarkItemService.ReadAll();
        CvProducts.ItemsSource = favoriteProducts;
    }
}