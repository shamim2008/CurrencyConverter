using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConversion
{
    /// <summary>
    ///This CurrencyRates class get a feed from API source. here I have created three Properties to store API record. These three properties (Parameter) does match with the API source key. Below I have given a short description about three properties.
    /// </summary>
    ///<param name="Base"> Receive base currency information from API</param>
    /// <param name="Date"> Date  Parameter receive date information. This is the updated date for curency.  </param>
    /// <param name="Rates"> Rates is important parameter here. I have store rates as dictionary. </param>
    public class CurrencyRates
    {
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }


    /// <summary>
    /// CurrencyList class facilitate to store suppoted currency in the drop down. I have loaded those currency from CountryList.json file. More currency can be manually added to that file.
    /// </summary>
    /// <param name="Currency"> Currency properties will store all the key and value from CountryList.json file</param>
    public class CurrencyList
    {
        public Dictionary<string,string> Currency { get; set; }
    }
}
