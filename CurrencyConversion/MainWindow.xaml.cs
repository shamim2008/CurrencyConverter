using System;
using System.Collections.Generic;
using System.Windows;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace CurrencyConversion
{
    /// <summary>
    /// <Author> Shamim Hossain</Author>
    /// <Version> 1.0 </Version>
    /// <Project> Currency Converter </Project> 
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        private string currentCurrency = string.Empty;
        private string newCurrency = string.Empty;
        
        private const string JCurrencyList = "CurrencyList.json";
        public MainWindow()
        {
            InitializeComponent();
            LoadSuportedCurrency();
        }

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
            Newtonsoft.Json.JsonConvert.PopulateObject(jason, cl);

            foreach (var item in cl.Currency)
            {
                CmbCurrentCurrency.Items.Add(item.Key.ToString());
                //if (!item.Key.Contains(CmbCurrentCurrency.Text))
                CmbNewCurrency.Items.Add(item.Key.ToString());
            }
        }



        private void CmbNewCurrency_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TxtResult.Clear();

            string jason;
            using (StreamReader sr = new StreamReader(JCurrencyList))
            {
                jason = sr.ReadToEnd();
            }
            CurrencyList cl = new CurrencyList();
            Newtonsoft.Json.JsonConvert.PopulateObject(jason, cl);

            foreach (var item in cl.Currency)
            {
                if (item.Key == CmbCurrentCurrency.SelectedItem.ToString())
                {
                    currentCurrency = item.Value.ToString();
                }
                if (item.Key == CmbNewCurrency.SelectedItem.ToString())
                {
                    newCurrency = item.Value.ToString();
                }
            }

            if (CmbCurrentCurrency.Text != "" || CmbNewCurrency.Text != "")
            {
                ProcessCurrency();
            }

        }
        /// <summary>
        /// ProcessCurrency() method will load data base on the two selected currency. 
        /// </summary>
        private void ProcessCurrency()
        {
            string json = GetJasonObjectFromUrl("https://api.fixer.io/latest?base=" + currentCurrency + "");

            CurrencyRates cr = new CurrencyRates();

            Newtonsoft.Json.JsonConvert.PopulateObject(json, cr);
            foreach (var item in cr.Rates)
            {
                if (currentCurrency == newCurrency)
                {
                    TxtResult.Text = "1";
                }

                if (item.Key.ToString() == newCurrency)
                {
                    TxtResult.Text = item.Value.ToString();
                }


            }
        }
    }
}
