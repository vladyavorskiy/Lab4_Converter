using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using Flurl.Http;


namespace con4
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }
    }

    public class Valute
    {
        public ValuteItem AUD { get; set; }
        public ValuteItem AZN { get; set; }
        public ValuteItem GBP { get; set; }
        public ValuteItem AMD { get; set; }
        public ValuteItem BYN { get; set; }
        public ValuteItem BGN { get; set; }
        public ValuteItem BRL { get; set; }
        public ValuteItem HUF { get; set; }
        public ValuteItem VND { get; set; }
        public ValuteItem HKD { get; set; }
        public ValuteItem GEL { get; set; }
        public ValuteItem DKK { get; set; }
        public ValuteItem AED { get; set; }
        public ValuteItem EUR { get; set; }
        public ValuteItem EGP { get; set; }
        public ValuteItem USD { get; set; }
        public ValuteItem INR { get; set; }
        public ValuteItem IDR { get; set; }
        public ValuteItem KZT { get; set; }
        public ValuteItem CAD { get; set; }
        public ValuteItem QAR { get; set; }
        public ValuteItem KGS { get; set; }
        public ValuteItem CNY { get; set; }
        public ValuteItem MDL { get; set; }
        public ValuteItem NZD { get; set; }
        public ValuteItem NOK { get; set; }
        public ValuteItem PLN { get; set; }
        public ValuteItem RON { get; set; }
        public ValuteItem XDR { get; set; }
        public ValuteItem SGD { get; set; }
        public ValuteItem TJS { get; set; }
        public ValuteItem THB { get; set; }
        public ValuteItem TRY { get; set; }
        public ValuteItem TMT { get; set; }
        public ValuteItem UZS { get; set; }
        public ValuteItem UAH { get; set; }
        public ValuteItem CZK { get; set; }
        public ValuteItem SEK { get; set; }
        public ValuteItem CHF { get; set; }
        public ValuteItem RSD { get; set; }
        public ValuteItem ZAR { get; set; }
        public ValuteItem KRW { get; set; }
        public ValuteItem JPY { get; set; }

    }

    public class ValuteItem
    {
        public string ID { get; set; }
        public string NumCode { get; set; }
        public string CharCode { get; set; }
        public int Nominal { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double Previous { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Root
    {
        public string error { get; set; }
        public int code { get; set; }
        public string explanation { get; set; }
        public DateTime Date { get; set; }
        public DateTime PreviousDate { get; set; }
        public string PreviousURL { get; set; }
        public DateTime Timestamp { get; set; }
        public Valute Valute { get; set; }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private string _valueinput;
        public string ValueInput
        {
            get => _valueinput;
            set
            {
                if (_valueinput == value) return;
                _valueinput = value;
                OnPropertyChanged(nameof(ValueInput));
                OnPropertyChanged(nameof(Res));
            }
        }

        public double? _valuein
        {
            get
            {
                return double.TryParse(_valueinput, out var val) ? val : 0;
                
            }
        }

        private ValuteItem _selectedFirstValute;

        public ValuteItem SelectedFirstValute
        {
            get => _selectedFirstValute;
            set
            {
                if (_selectedFirstValute == value) return;
                _selectedFirstValute = value;
                OnPropertyChanged(nameof(SelectedFirstValute));
                OnPropertyChanged(nameof(Res));
            }
        }

        private ValuteItem _selectedSecondValute;
        public ValuteItem SelectedSecondValute
        {
            get => _selectedSecondValute;
            set
            {
                if (_selectedSecondValute == value) return;
                _selectedSecondValute = value;
                OnPropertyChanged(nameof(SelectedSecondValute));
                OnPropertyChanged(nameof(Res));
            }
        }

        public double? Res
        {
            get
            {
                if (_selectedFirstValute == null || _selectedSecondValute == null ) return 0;
                else
                {
                    return (_valuein * SelectedFirstValute.Value/ SelectedFirstValute.Nominal) / (SelectedSecondValute.Value/ SelectedSecondValute.Nominal);
                }
            }
        }

        public ObservableCollection<ValuteItem> ValutesList { get; set; } = new ObservableCollection<ValuteItem> { };

        
        public MainViewModel()
        {
            LoadData(DateTime.Today.ToString("yyyy/MM/dd").Replace(".", "/"));
        }

        private DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate == value) return;
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                LoadData(SelectedDate.ToString("yyyy/MM/dd").Replace(".", "/"));

            }
        }


        private async Task LoadData(string date)
        {
            int FirstIndexValute = 0;
            int SecondIndexValute = 0;

            if (SelectedFirstValute != null)
            {
                FirstIndexValute = ValutesList.IndexOf(SelectedFirstValute);
            }
            if (SelectedSecondValute != null)
            {
                SecondIndexValute = ValutesList.IndexOf(SelectedSecondValute);
            }
            try
            {
                var result = await $"https://www.cbr-xml-daily.ru/archive/{date}/daily_json.js".GetJsonAsync<Root>();
                
                if (result == null)
                    return;

                ValutesList.Clear();

                ValutesList.Add(new ValuteItem() { Name = result.Valute.AUD.Name, Value = result.Valute.AUD.Value, Nominal = result.Valute.AUD.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.AZN.Name, Value = result.Valute.AZN.Value, Nominal = result.Valute.AZN.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.GBP.Name, Value = result.Valute.GBP.Value, Nominal = result.Valute.GBP.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.AMD.Name, Value = result.Valute.AMD.Value, Nominal = result.Valute.AMD.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.BYN.Name, Value = result.Valute.BYN.Value, Nominal = result.Valute.BYN.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.BGN.Name, Value = result.Valute.BGN.Value, Nominal = result.Valute.BGN.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.BRL.Name, Value = result.Valute.BRL.Value, Nominal = result.Valute.BRL.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.HUF.Name, Value = result.Valute.HUF.Value, Nominal = result.Valute.HUF.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.VND.Name, Value = result.Valute.VND.Value, Nominal = result.Valute.VND.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.HKD.Name, Value = result.Valute.HKD.Value, Nominal = result.Valute.HKD.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.GEL.Name, Value = result.Valute.GEL.Value, Nominal = result.Valute.GEL.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.DKK.Name, Value = result.Valute.DKK.Value, Nominal = result.Valute.DKK.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.AED.Name, Value = result.Valute.AED.Value, Nominal = result.Valute.AED.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.USD.Name, Value = result.Valute.USD.Value, Nominal = result.Valute.USD.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.EUR.Name, Value = result.Valute.EUR.Value, Nominal = result.Valute.EUR.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.EGP.Name, Value = result.Valute.EGP.Value, Nominal = result.Valute.EGP.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.INR.Name, Value = result.Valute.INR.Value, Nominal = result.Valute.INR.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.IDR.Name, Value = result.Valute.IDR.Value, Nominal = result.Valute.IDR.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.KZT.Name, Value = result.Valute.KZT.Value, Nominal = result.Valute.KZT.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.CAD.Name, Value = result.Valute.CAD.Value, Nominal = result.Valute.CAD.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.QAR.Name, Value = result.Valute.QAR.Value, Nominal = result.Valute.QAR.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.KGS.Name, Value = result.Valute.KGS.Value, Nominal = result.Valute.KGS.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.CNY.Name, Value = result.Valute.CNY.Value, Nominal = result.Valute.CNY.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.MDL.Name, Value = result.Valute.MDL.Value, Nominal = result.Valute.MDL.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.NZD.Name, Value = result.Valute.NZD.Value, Nominal = result.Valute.NZD.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.NOK.Name, Value = result.Valute.NOK.Value, Nominal = result.Valute.NOK.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.PLN.Name, Value = result.Valute.PLN.Value, Nominal = result.Valute.PLN.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.RON.Name, Value = result.Valute.RON.Value, Nominal = result.Valute.RON.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.XDR.Name, Value = result.Valute.XDR.Value, Nominal = result.Valute.XDR.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.SGD.Name, Value = result.Valute.SGD.Value, Nominal = result.Valute.SGD.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.TJS.Name, Value = result.Valute.TJS.Value, Nominal = result.Valute.TJS.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.THB.Name, Value = result.Valute.THB.Value, Nominal = result.Valute.THB.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.TRY.Name, Value = result.Valute.TRY.Value, Nominal = result.Valute.TRY.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.TMT.Name, Value = result.Valute.TMT.Value, Nominal = result.Valute.TMT.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.UZS.Name, Value = result.Valute.UZS.Value, Nominal = result.Valute.UZS.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.UAH.Name, Value = result.Valute.UAH.Value, Nominal = result.Valute.UAH.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.CZK.Name, Value = result.Valute.CZK.Value, Nominal = result.Valute.CZK.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.SEK.Name, Value = result.Valute.SEK.Value, Nominal = result.Valute.SEK.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.CHF.Name, Value = result.Valute.CHF.Value, Nominal = result.Valute.CHF.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.RSD.Name, Value = result.Valute.RSD.Value, Nominal = result.Valute.RSD.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.ZAR.Name, Value = result.Valute.ZAR.Value, Nominal = result.Valute.ZAR.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.KRW.Name, Value = result.Valute.KRW.Value, Nominal = result.Valute.KRW.Nominal });
                ValutesList.Add(new ValuteItem() { Name = result.Valute.JPY.Name, Value = result.Valute.JPY.Value, Nominal = result.Valute.JPY.Nominal });
                ValutesList.Add(new ValuteItem() { Name = "Российский рубль", Value = 1, Nominal = 1 });

                //NormalDateTime = SelectedDate;

                App.Current.MainPage.DisplayAlert("Готово", $"Выбранная дата {SelectedDate.ToString("yyyy/MM/dd").Replace(".", "/")}", "OK");

            }
            catch (Exception e)
            {
                SelectedDate = SelectedDate.AddDays(-1);
                //App.Current.MainPage.DisplayAlert("Ошибка", e.Message.ToString(), "OK");
                //App.Current.MainPage.DisplayAlert("Ошибка", "На выбранную дату нет курсов", "OK");
                //SelectedDate = NormalDateTime;
                throw;
            }

            if (FirstIndexValute == 0 && SecondIndexValute == 0)
            {
                SelectedFirstValute = ValutesList.FirstOrDefault(c => c.Name == "Российский рубль");
                SelectedSecondValute = ValutesList.FirstOrDefault(c => c.Name == "Доллар США");
            }
            else
            {
                SelectedFirstValute = ValutesList[FirstIndexValute];
                SelectedSecondValute = ValutesList[SecondIndexValute];
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
