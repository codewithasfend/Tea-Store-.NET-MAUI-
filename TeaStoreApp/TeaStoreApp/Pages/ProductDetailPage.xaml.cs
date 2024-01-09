using TeaStoreApp.Models;
using TeaStoreApp.Services;

namespace TeaStoreApp.Pages;

public partial class ProductDetailPage : ContentPage
{
    private int productId;
    private string imageUrl;
    private BookmarkItemService bookmarkItemService = new BookmarkItemService();
    public ProductDetailPage(int productId)
    {
        InitializeComponent();
        GetProductDetail(productId);
        this.productId = productId;
    }

    private async void GetProductDetail(int productId)
    {
        var product = await ApiService.GetProductDetail(productId);
        LblProductName.Text = product.Name;
        LblProductDescription.Text = product.Detail;
        ImgProduct.Source = product.FullImageUrl;
        LblProductPrice.Text = product.Price.ToString();
        LblTotalPrice.Text = product.Price.ToString();
        imageUrl = product.FullImageUrl;
    }

    private void BtnAdd_Clicked(object sender, EventArgs e)
    {
        var i = Convert.ToInt32(LblQty.Text);
        i++;
        LblQty.Text = i.ToString();
        var totalPrice = i * Convert.ToInt32(LblProductPrice.Text);
        LblTotalPrice.Text = totalPrice.ToString();
    }

    private void BtnRemove_Clicked(object sender, EventArgs e)
    {
        var i = Convert.ToInt32(LblQty.Text);
        i--;
        if (i < 1)
        {
            return;
        }
        LblQty.Text = i.ToString();
        var totalPrice = i * Convert.ToInt32(LblProductPrice.Text);
        LblTotalPrice.Text = totalPrice.ToString();
    }

    private async void BtnAddToCart_Clicked(object sender, EventArgs e)
    {
        var shoppingCart = new ShoppingCart()
        {
            Qty = Convert.ToInt32(LblQty.Text),
            Price = Convert.ToInt32(LblProductPrice.Text),
            TotalAmount = Convert.ToInt32(LblTotalPrice.Text),
            ProductId = productId,
            CustomerId = Preferences.Get("userid", 0)
        };

        var response = await ApiService.AddItemsInCart(shoppingCart);
        if (response)
        {
            await DisplayAlert("", "Your item has been added to the cart", "Alright");
        }
        else
        {
            await DisplayAlert("Oops", "Something went wrong", "Cancel");
        }
    }

    private void ImgBtnFavorite_Clicked(object sender, EventArgs e)
    {
        var existingBookmark = bookmarkItemService.Read(productId);
        if (existingBookmark != null)
        {
            bookmarkItemService.Delete(existingBookmark);
        }
        else
        {
            var bookmarkedProduct = new BookmarkedProduct()
            {
                ProductId = productId,
                IsBookmarked = true,
                Detail = LblProductDescription.Text,
                Name = LblProductName.Text,
                Price = Convert.ToInt32(LblProductPrice.Text),
                ImageUrl = imageUrl
            };

            bookmarkItemService.Create(bookmarkedProduct);
        }
        UpdateFavoriteButton();
    }
    private void UpdateFavoriteButton()
    {
        var existingBookmark = bookmarkItemService.Read(productId);
        if (existingBookmark != null)
        {
            ImgBtnFavorite.Source = "heartfill";
        }
        else
        {
            ImgBtnFavorite.Source = "heart";
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateFavoriteButton();
    }
}