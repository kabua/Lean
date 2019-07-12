/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using QuantConnect.Data.UniverseSelection;

namespace QuantConnect.Data.Custom.TradingEconomics
{
    /// <summary>
    /// Represents the Trading Economics Calendar information:
    /// The economic calendar covers around 1600 events for more than 150 countries a month.
    /// https://docs.tradingeconomics.com/#events
    /// </summary>
    public class TradingEconomicsCalendar : BaseData
    {
        /// <summary>
        /// Unique calendar ID used by Trading Economics
        /// </summary>
        [JsonProperty(PropertyName = "CalendarId")]
        public string CalendarId { get; set; }

        /// <summary>
        /// Release time and date in UTC
        /// </summary>
        [JsonProperty(PropertyName = "Date"), JsonConverter(typeof(TradingEconomicsDateTimeConverter))]
        public override DateTime EndTime { get; set; }

        /// <summary>
        /// Country name
        /// </summary>
        [JsonProperty(PropertyName = "Country")]
        public string Country { get; set; }

        /// <summary>
        /// Indicator category name
        /// </summary>
        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }

        /// <summary>
        /// Specific event name in the calendar
        /// </summary>
        [JsonProperty(PropertyName = "Event")]
        public string Event { get; set; }

        /// <summary>
        /// The period for which released data refers to
        /// </summary>
        [JsonProperty(PropertyName = "Reference")]
        public string Reference { get; set; }

        /// <summary>
        /// Source of data
        /// </summary>
        [JsonProperty(PropertyName = "Source")]
        public string Source { get; set; }

        /// <summary>
        /// Latest released value
        /// </summary>
        [JsonProperty(PropertyName = "Actual")]
        public string Actual { get; set; }

        /// <summary>
        /// Value for the previous period after the revision (if revision is applicable)
        /// </summary>
        [JsonProperty(PropertyName = "Previous")]
        public string Previous { get; set; }

        /// <summary>
        /// Average forecast among a representative group of economists
        /// </summary>
        [JsonProperty(PropertyName = "Forecast")]
        public string Forecast { get; set; }

        /// <summary>
        /// TradingEconomics own projections
        /// </summary>
        [JsonProperty(PropertyName = "TEForecast")]
        public string TradingEconomicsForecast { get; set; }

        /// <summary>
        /// Hyperlink at Trading Economics
        /// </summary>
        [JsonProperty(PropertyName = "URL")]
        public string Url { get; set; }

        /// <summary>
        /// 0 indicates that the time of the event is known,
        /// 1 indicates that we only know the date of event, the exact time of event is unknown
        /// </summary>
        [JsonProperty(PropertyName = "DateSpan")]
        public string DateSpan { get; set; }

        /// <summary>
        /// Importance of a TradingEconomics information
        /// </summary>
        [JsonProperty(PropertyName = "Importance")]
        public TradingEconomicsImportance Importance { get; set; }

        /// <summary>
        /// Time when new data was inserted or changed
        /// </summary>
        [JsonProperty(PropertyName = "LastUpdate"), JsonConverter(typeof(TradingEconomicsDateTimeConverter))]
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Value reported in the previous period after revision
        /// </summary>
        /// <remarks>
        /// If there is no revision field remains empty
        /// </remarks>
        [JsonProperty(PropertyName = "Revised")]
        public string Revised { get; set; }

        /// <summary>
        /// Country�s original name
        /// </summary>
        [JsonProperty(PropertyName = "OCountry")]
        public string OCountry { get; set; }

        /// <summary>
        /// Category�s original name
        /// </summary>
        [JsonProperty(PropertyName = "OCategory")]
        public string OCategory { get; set; }

        /// <summary>
        /// Unique ticker used by Trading Economics
        /// </summary>
        [JsonProperty(PropertyName = "Ticker")]
        public string Ticker { get; set; }

        /// <summary>
        /// Unique symbol used by Trading Economics
        /// </summary>
        [JsonProperty(PropertyName = "Symbol")]
        public string Symbol { get; set; }

        /// <summary>
        /// Return the Subscription Data Source gained from the URL
        /// </summary>
        /// <param name="config">Configuration object</param>
        /// <param name="date">Date of this source file</param>
        /// <param name="isLiveMode">true if we're in live mode, false for backtesting mode</param>
        /// <returns>Subscription Data Source.</returns>
        public override SubscriptionDataSource GetSource(SubscriptionDataConfig config, DateTime date, bool isLiveMode)
        {
            if (!config.Symbol.Value.EndsWith(".C"))
            {
                throw new ArgumentException($"TradingEconomicsCalendar.GetSource(): Invalid symbol {config.Symbol}");
            }

            var symbol = config.Symbol.Value.ToLower();
            symbol = symbol.Substring(0, symbol.Length - 2);
            var source = Path.Combine(Globals.DataFolder, "alternative", "trading-economics", $"{symbol}_calendar.zip");
            return new SubscriptionDataSource(source, SubscriptionTransportMedium.LocalFile, FileFormat.Collection);
        }

        /// <summary>
        /// Reader converts each line of the data source into BaseData objects.
        /// </summary>
        /// <param name="config">Subscription data config setup object</param>
        /// <param name="content">Content of the source document</param>
        /// <param name="date">Date of the requested data</param>
        /// <param name="isLiveMode">true if we're in live mode, false for backtesting mode</param>
        /// <returns>
        /// Collection of USEnergyInformation objects
        /// </returns>
        public override BaseData Reader(SubscriptionDataConfig config, string content, DateTime date, bool isLiveMode)
        {
            var objectList = JsonConvert.DeserializeObject<List<TradingEconomicsCalendar>>(content);
            foreach (var obj in objectList)
            {
                obj.Symbol = config.Symbol;
                if (obj.LastUpdate > obj.EndTime)
                {
                    obj.EndTime = obj.LastUpdate;
                }
            }
            return new BaseDataCollection(date, config.Symbol, objectList);
        }

        /// <summary>
        /// Formats a string with the Trading Economics Calendar information.
        /// </summary>
        public override string ToString()
        {
            var symbol = string.IsNullOrWhiteSpace(Symbol) ? Symbol : Ticker;
            return $"{symbol} ({Country} - {Category}): {Event} : Importance.{Importance}";
        }
    }

    /// <summary>
    /// Importance of a TradingEconomics information
    /// </summary>
    public enum TradingEconomicsImportance
    {
        /// <summary>
        /// Low importance
        /// </summary>
        [JsonProperty(PropertyName = "low")]
        Low,

        /// <summary>
        /// Medium importance
        /// </summary>
        [JsonProperty(PropertyName = "medium")]
        Medium,

        /// <summary>
        /// High importance
        /// </summary>
        [JsonProperty(PropertyName = "high")]
        High
    }
}