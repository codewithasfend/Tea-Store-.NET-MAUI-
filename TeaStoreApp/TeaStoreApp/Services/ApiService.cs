using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TeaStoreApp.Models;

namespace TeaStoreApp.Services
{
    public static class ApiService
    {
        public static async Task<bool> RegisterUser(string name, string email, string phone, string password)
        {
            var register = new Register()
            {
                Name = name,
                Email = email,
                Phone = phone,
                Password = password
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(register);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/users/register", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> Login(string email, string password)
        {
            var login = new Login()
            {
                Email = email,
                Password = password
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/users/login", content);
            if (!response.IsSuccessStatusCode) return false;
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Token>(jsonResult);
            Preferences.Set("accesstoken", result.AccessToken);
            Preferences.Set("userid", result.UserId);
            Preferences.Set("username", result.UserName);
            return true;
        }

        public static async Task<ProfileImage> GetUserProfileImage()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/users/profileimage");
            return JsonConvert.DeserializeObject<ProfileImage>(response);
        }

        public static async Task<bool> UploadUserImage(byte[] imageArray)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));
            var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(imageArray), "Image", "image.jpg");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/users/uploadphoto", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<List<Category>> GetCategories()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/categories");
            return JsonConvert.DeserializeObject<List<Category>>(response);
        }

        public static async Task<List<Product>> GetProducts(string productType, string categoryId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/products?productType=" + productType + "&categoryId=" + categoryId);
            return JsonConvert.DeserializeObject<List<Product>>(response);
        }

        public static async Task<ProductDetail> GetProductDetail(int productId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/products/" + productId);
            return JsonConvert.DeserializeObject<ProductDetail>(response);
        }

        public static async Task<bool> AddItemsInCart(ShoppingCart shoppingCart)
        {

            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(shoppingCart);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/shoppingcartitems", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<List<ShoppingCartItem>> GetShoppingCartItems(int userId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/shoppingcartitems/" + userId);
            return JsonConvert.DeserializeObject<List<ShoppingCartItem>>(response);
        }

        public static async Task<bool> UpdateCartQuantity(int productId, string action)
        {

            var httpClient = new HttpClient();
            var content = new StringContent(string.Empty);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));
            var response = await httpClient.PutAsync(AppSettings.ApiUrl + "api/shoppingcartitems?productId=" + productId + "&action=" + action, content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> PlaceOrder(Order order)
        {

            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/orders", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<List<OrderByUser>> GetOrdersByUser(int userId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/orders/ordersbyuser/" + userId);
            return JsonConvert.DeserializeObject<List<OrderByUser>>(response);
        }

        public static async Task<List<OrderDetail>> GetOrderDetails(int orderId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/orders/orderdetails/" + orderId);
            return JsonConvert.DeserializeObject<List<OrderDetail>>(response);
        }
    }
}
