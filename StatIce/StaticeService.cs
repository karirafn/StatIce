using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace StatIce
{
    public static class StaticeService
    {
        public static IEnumerable<StaticeData> WageIndex
            => GetStatIceData(DataTypeEnum.WageIndex, 1989);

        public static IEnumerable<StaticeData> BuildingCostIndex
            => GetStatIceData(DataTypeEnum.BuildingCostIndex, 1938);

        public static IEnumerable<StaticeData> ConsumerPriceIndex
            => GetStatIceData(DataTypeEnum.ConsumerPriceIndex, 1988);
        
        private static string GetIndexId(DataTypeEnum type)
        {
            switch (type)
            {
                case DataTypeEnum.WageIndex:
                    return "eb9e5a51-3a42-452e-9f43-04c86953a40d";
                case DataTypeEnum.BuildingCostIndex:
                    return "a6b70eeb-8cfc-4e24-9f2a-f933e550d0af";
                case DataTypeEnum.ConsumerPriceIndex:
                    return "b4d8fe53-4e54-4a56-8980-22c70d9261ab";
                default:
                    return null;
            }
        }

        private static IEnumerable<StaticeData> GetStatIceData(DataTypeEnum dataType, int firstYear)
        {
            var result = string.Empty;
            var url = "http://px.hagstofa.is/pxis/sq/" + GetIndexId(dataType);

            using (var web = new WebClient())
            {
                result = web.DownloadString(url);
            }

            dynamic data = JsonConvert.DeserializeObject(result);
            var date = new DateTime(firstYear, 1, 1);
            var dataset = new List<StaticeData>();
            foreach (var d in data.dataset.value)
            {
                var value = (string)d;
                value = value?.Replace('.', ',');

                if (double.TryParse(value, out double indexValue))
                    dataset.Add(new StaticeData(date, indexValue));

                date = date.AddMonths(1);
            }

            return dataset.Where(d => d.Value > 0);
        }
    }
}
