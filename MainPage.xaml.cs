namespace RadioButtons;

public partial class MainPage : ContentPage
{
    private CurrencyConverter currencyConverter = new();

    public MainPage()
    {
        InitializeComponent();
        LoadComponent();
    }

    private async void LoadComponent()
    {
        await currencyConverter.LoadRates();

        List<string> currencies = currencyConverter.GetCurrencies();

        pickFrom.ItemsSource = currencies;
        pickTo.ItemsSource = currencies;

        pickFrom.SelectedItem = "BRL";
        pickTo.SelectedItem = "USD";
    }

    public void RadioButton_CheckedChanged(object sender, EventArgs e)
    {
        RadioButton rb = (RadioButton)sender;

        if (!decimal.TryParse(txValor.Text, out decimal value))
            return;

        string currency = "";

        switch (rb.Content?.ToString())
        {
            case "Dólar":
                currency = "USD";
                break;

            case "Euro":
                currency = "EUR";
                break;

            case "Pesos":
                currency = "ARS";
                break;
        }

        if (currency == "")
            return;

        decimal result = currencyConverter.ConvertValue(value, "BRL", currency);

        lbValorConvertido.Text = result.ToString("F2");
    }

    public void OnPicker_SelectedIndexChanged(object sender, EventArgs e) { }

    public void btConverterPicker_Clicked(object sender, EventArgs e)
    {
        if (!decimal.TryParse(txValor.Text, out decimal value))
            return;

        if (pickFrom.SelectedItem == null || pickTo.SelectedItem == null)
            return;

        string from = pickFrom.SelectedItem.ToString()!;
        string to = pickTo.SelectedItem.ToString()!;

        decimal result = currencyConverter.ConvertValue(value, from, to);

        lbValorConvertidoPicker.Text = result.ToString("F2");
    }
}
