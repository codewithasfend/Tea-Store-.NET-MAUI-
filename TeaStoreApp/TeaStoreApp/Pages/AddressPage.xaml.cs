namespace TeaStoreApp.Pages;

public partial class AddressPage : ContentPage
{
    public AddressPage()
    {
        InitializeComponent();
    }

    private void BtnSave_Clicked(object sender, EventArgs e)
    {
        Preferences.Set("address", EntAddress.Text + "," + EntPhone.Text + "," + EntName.Text);
        Navigation.PopAsync();
    }
}