using System;
using System.Collections.Generic;
using System.Windows;
using Newtonsoft.Json;
using System.IO;
using System.Net;


namespace CurrencyConversion
{
    /// <summary>
    /// <Author> Shamim Hossain</Author>
    /// <Version> 1.0 </Version>
    /// <Project> Currency Converter </Project> 
    /// 
    /// </summary>
    /// <Description>This application will convert currency based on two selected currency. To see that in action, the user need to select a desired current currency from the drop-down menu, the Current currency will work as a base currency. Selecting the new currency will populate the result in the  exchange rate box.
    /// Here I have used https://api.fixer.io/latest API to get exchange rate. 
    /// To work with JSON type data, Newtonsoft.Json framework was used. That was downloaded from NuGet Library.  </Description>
    /// <Note>The user must have to change new currency once they change the current currency. Because the result will only populate on the change event of the new currency</Note>
    public partial class MainWindow : Window
    {

        private string currentCurrency = string.Empty;
        private string newCurrency = string.Empty;
        
        private const string JCurrencyList = "CurrencyList.json";
        public MainWindow()
        {
            InitializeComponent();
            LoadSuportedCurrency(); //when application start, supported currency are loaded in both drop down menu from CurencyList.json file.
        }


        /// <summary>
        /// GetJasonObjectFromUrl() will download the JSON data as strin from api. 
        /// </summary>
        /// <param name="url"> API link (https://api.fixer.io/latest) </param>
        /// <returns>Thats return JSON data as string</returns>
        private string GetJasonObjectFromUrl(string url)
        {
            string jason;
            using (WebClient wc = new WebClient())
            {
                jason = wc.DownloadString(url);
            }

            return jason;
        }


        /// <summary>
        /// LoadSuportedcurrency() method will load two drop down menu from CurrencyList.json file. 
        /// </summary>
        private void LoadSuportedCurrency()
        {
            string jason;
            using (StreamReader sr = new StreamReader(JCurrencyList))
            {
                jason = sr.ReadToEnd();
            }
            CurrencyList cl = new CurrencyList();
            JsonConvert.PopulateObject(jason, cl);

            foreach (var item in cl.Currency)
            {
                CmbCurrentCurrency.Items.Add(item.Key);
                //if (!item.Key.Contains(CmbCurrentCurrency.Text))
                CmbNewCurrency.Items.Add(item.Key);
            }
        }


        /// <summary>
        /// Actual currency conversion occurs when the user selects new currency from the drop-down menu.  
        /// Although we have loaded the data in the drop-down for the user to view, and we need to load then again to compare with the user selected data.
        /// </summary>

        private void CmbNewCurrency_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TxtResult.Clear(); // All previous result are clear before loading new result.

            string jason;
            using (StreamReader sr = new StreamReader(JCurrencyList))  // loading currecy from local CurrencyList.json file
            {
                jason = sr.ReadToEnd();
            }
            CurrencyList cl = new CurrencyList();
            JsonConvert.PopulateObject(jason, cl);

            foreach (var item in cl.Currency)
            {
                if (item.Key == CmbCurrentCurrency.SelectedItem.ToString())
                {
                    currentCurrency = item.Value; // based on user selected currency, values are selected . e.g Key: Pound Sterling (GBP), Value: GBP
                }
                if (item.Key == CmbNewCurrency.SelectedItem.ToString())
                {
                    newCurrency = item.Value; // based on user selected currency, values are selected . e.g Key: Swiss Franc (CHF), Value: CHF
                }
            }

            if (CmbCurrentCurrency.Text != "") // This will ensure that current currency are not empty before it start conversion processing
            {
                ProcessCurrencyConversion();
            }

        }


        /// <summary>
        /// ProcessCurrencyConversion() method will load data base on the two selected currency. 
        /// Once the data loaded from API, data are sent back to CurrncyRates.class with matching properties. the result is displayed base on the matching key and values from the Rates properties.
        /// 
        /// </summary>
        private void ProcessCurrencyConversion()
        {
            string json = GetJasonObjectFromUrl("https://api.fixer.io/latest?base=" + currentCurrency + ""); //API base currency is the current currency

            CurrencyRates cr = new CurrencyRates();

           JsonConvert.PopulateObject(json, cr);
            LblUpdateDate.Content = cr.Date.ToString("dd MMMM yyyy");
            foreach (var item in cr.Rates)
            {
                if (currentCurrency == newCurrency)
                {
                    TxtResult.Text = "1"; // if both currency are same. display result will be 1
                }


                if (item.Key == newCurrency)
                {
                    TxtResult.Text = item.Value.ToString(); // desplayed result as string 
                }


            }

        }
    }
}
