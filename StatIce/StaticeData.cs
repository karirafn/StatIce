using System;

namespace StatIce
{
    public class StaticeData
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }

        public StaticeData(DateTime date, double value)
        {
            Date = date;
            Value = value;
        }
    }
}
