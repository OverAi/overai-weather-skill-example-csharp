using Newtonsoft.Json;
using SkillExampleWebApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;


namespace SkillExampleWebApi.Skills
{
    /// <summary>
    /// the actual implementation of the skill
    /// </summary>
    /// <seealso cref="SkillExampleWebApi.Skills.Skill" />
    public class WeatherSkill : Skill
    {
        const string BASE_SERVICE_URL = "http://api.openweathermap.org/data/2.5/";
        //you can get it from https://openweathermap.org/appid
        const string APP_ID = "[OPEN_WEATHER_APP_ID]";


        /// <summary>
        /// Main skill entry point.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">city param is required</exception>
        public override SkillResponseModel PerformAction(SkillRequestModel request)
        {
            SkillResponseModel res = null;
            //extract parameter values from response.
            string city = GetParameter("sys.cities", request)?.Value;
            string dateStr = GetParameter("date", request)?.Value;
            string measurementSystem = GetParameter("measurementSystem", request)?.Value;

            //city is required parameter.
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("city param is required");
            }

            city = FixCityCountryNameToCityCountryCode(city);

            //if date was not supplied use now as date
            if (string.IsNullOrWhiteSpace(dateStr))
            {
                dateStr = DateTime.UtcNow.ToString("yyyy-MM-dd");
            }

            if (string.IsNullOrWhiteSpace(measurementSystem))
            {
                measurementSystem = "metric";
            }

            OpenWeatherApiResponse apiRes;
            
            DateTime date = DateTime.Parse(dateStr);
            TimeSpan span = (date - DateTime.UtcNow.Date);
            //we only want to support forecast within 5 days.
            if (span.TotalDays <= 5 && span.TotalDays >= 0)
            {
                switch (request.Intent.Action.ToLower())
                {
                    default:
                        apiRes = GetWeatherForecast(date, city, measurementSystem);
                        break;
                }

                var weather = apiRes.List.Where(w => DateTime.ParseExact(w.Dt_txt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).Month == date.Month &&
                                                     DateTime.ParseExact(w.Dt_txt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).Day == date.Day).FirstOrDefault();

                res = CreateSkillResponse($"the tempreture in {city} {(span.TotalDays > 0 ? "will be" : "is")} {Convert.ToInt32(weather.Main.Temp)} {(measurementSystem.ToLower().Equals("metric") ? "celsius" : "fahrenheit")} and {weather.Weather[0].Description}");
            }
            else
            {
                //if date is in the past
                if (span.TotalDays < 0)
                {
                    res = CreateSkillResponse("Cant get forecast in the past.");
                }
                //else date in the future
                else
                {
                    res = CreateSkillResponse("I still don't possess the power of foresight we can only give a forecast for the next 5 days");
                }

            }

            return res;
        }
        /// <summary>
        /// open weather takes city name and coutry code and NLU returns city name and country name 
        /// delimited by "," so this function in production gets country code from DB, here we will use
        /// some hard coded value which you can change to any logic you want as long as the result will
        /// be [city],[country code]
        /// </summary>
        /// <param name="cityAndCountryName">Name of the city and country.</param>
        /// <returns></returns>
        private string FixCityCountryNameToCityCountryCode(string cityAndCountryName)
        {
            string countryCode = "IL";
            string city = "tel aviv";
            string res = null;

            res = city + "," + countryCode;

            return res;
        }

        /// <summary>
        /// calls the open weather API
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="city">The city.</param>
        /// <param name="measurementSystem">The measurement system.</param>
        /// <returns></returns>
        public OpenWeatherApiResponse GetWeatherForecast(DateTime date, string city, string measurementSystem = "metric")
        {
            string finalUrl = BASE_SERVICE_URL + "forecast";
            finalUrl += $"?q={WebUtility.UrlEncode(city)}&appid={APP_ID}&units={measurementSystem}";

            var apiRes = GetApiResponse(finalUrl);


            return apiRes;

        }

        private OpenWeatherApiResponse GetApiResponse(string url)
        {
            OpenWeatherApiResponse res = null;

            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString(url);
                res = JsonConvert.DeserializeObject<OpenWeatherApiResponse>(json);
            }

            return res;
        }
    }
}