using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class TaxRateView : ContentPage
{
    public TaxRateView()
    {
        InitializeComponent();
        BindingContext = new TaxRateViewModel();
    }

    private void UpdateTaxRateClicked(object sender, EventArgs e)
    {
        (BindingContext as TaxRateViewModel)?.UpdateTaxRate();
    }

    private void GoBackClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }
}