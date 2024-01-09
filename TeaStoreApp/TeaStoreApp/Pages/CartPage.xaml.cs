using System.Collections.ObjectModel;
using TeaStoreApp.Models;
using TeaStoreApp.Services;

namespace TeaStoreApp.Pages;

public partial class CartPage : ContentPage
{
    private ObservableCollection<ShoppingCartItem> ShoppingCartItems = new ObservableCollection<ShoppingCartItem>();
    public CartPage()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetShoppingCartItems();
        bool address = Preferences.ContainsKey("address");
        if (address)
        {
            LblAddress.Text = Preferences.Get("address", string.Empty);
        }
        else
        {
            LblAddress.Text = "Provide Your Address";
        }
    }

    private async void GetShoppingCartItems()
    {
        ShoppingCartItems.Clear();
        var shoppingcartitems = await ApiService.GetShoppingCartItems(Preferences.Get("userid", 0));
        foreach (var shoppingCart in shoppingcartitems)
        {
            ShoppingCartItems.Add(shoppingCart);
        }

        CvCart.ItemsSource = ShoppingCartItems;
        UpdateTotalPrice();
    }


    private void UpdateTotalPrice()
    {
        var totalPrice = ShoppingCartItems.Sum(item => item.Price * item.Qty);
        LblTotalPrice.Text = totalPrice.ToString();
    }




    private async void UpdateCartQuantity(int productId, string action)
    {

        var response = await ApiService.UpdateCartQuantity(productId, action);
        if (response)
        {
            return;
        }
        else
        {
            await DisplayAlert("Oops", "Something went wrong", "Cancel");
        }
    }


    private void BtnDecrease_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ShoppingCartItem cartItem)
        {
            if (cartItem.Qty == 1) return;
            else if (cartItem.Qty > 1)
            {
                cartItem.Qty--;
                UpdateTotalPrice();
                UpdateCartQuantity(cartItem.ProductId, "decrease");
            }
        }
    }

    private void BtnIncrease_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ShoppingCartItem cartItem)
        {
            cartItem.Qty++;
            UpdateTotalPrice();
            UpdateCartQuantity(cartItem.ProductId, "increase");
        }
    }

    private void BtnDelete_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.BindingContext is ShoppingCartItem cartItem)
        {

            ShoppingCartItems.Remove(cartItem);
            UpdateTotalPrice();
            UpdateCartQuantity(cartItem.ProductId, "delete");
        }
    }

    private void BtnEditAddress_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddressPage());
    }

    private async void TapPlaceOrder_Tapped(object sender, TappedEventArgs e)
    {
        var order = new Order()
        {
            Address = LblAddress.Text,
            UserId = Preferences.Get("userid", 0),
            OrderTotal = Convert.ToInt32(LblTotalPrice.Text)
        };

        var response = await ApiService.PlaceOrder(order);
        if (response)
        {
            await DisplayAlert("", "Your order has been placed", "Alright");
            ShoppingCartItems.Clear();
        }
        else
        {
            await DisplayAlert("Oops", "Something went wrong", "Cancel");
        }
    }
}