using System;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
namespace DIVALib.Math
{
    public class Time : IXmlSerializable
    {
        public enum EUnit 
        {
            MICROSECOND,
            ME5SECOND,
            MILLISECOND,
            SECOND,
            MINUTE,
            HOUR
        }

        public EUnit  unit;
        public double time;


        public Time()
        {
        }

		public Time(double value)
		{
            time = value;
            unit = EUnit.SECOND;
		}

		public Time(double value, EUnit timeUnit)
		{
			time = value;
			unit = timeUnit;
		}

        public Time ToMicroseconds()
        {
			var cTime = new Time(time, unit);
			switch (cTime.unit)
            {
                case EUnit.MICROSECOND: return cTime;
                case EUnit.ME5SECOND: cTime.time *= 10; cTime.unit = EUnit.MICROSECOND; break;
                case EUnit.MILLISECOND: cTime.time *= 1000; cTime.unit = EUnit.MICROSECOND; break;
                default: cTime.ToMilliseconds(); cTime.time *= 1000; break;
            }
            return cTime;
        }

		public Time ToME5Seconds()
		{
			var cTime = new Time(time, unit);
			switch (cTime.unit)
			{
				case EUnit.MICROSECOND: cTime.time /= 10; cTime.unit = EUnit.ME5SECOND; break;
                case EUnit.ME5SECOND: return cTime;
                default: cTime.ToMilliseconds(); cTime.time *= 100; cTime.unit = EUnit.ME5SECOND; break;
			}
			return cTime;
		}

        public Time ToMilliseconds()
		{
			var cTime = new Time(time, unit);
			switch (cTime.unit)
			{
                case EUnit.MICROSECOND: cTime.time /= 1000; cTime.unit = EUnit.MILLISECOND; break;
                case EUnit.ME5SECOND: cTime.time /= 100; cTime.unit = EUnit.MILLISECOND; break;
                case EUnit.MILLISECOND: return cTime;
                default: cTime.ToSeconds(); cTime.time *= 1000; break;
			}
            return cTime;
		}

		public Time ToSeconds()
		{
			var cTime = new Time(time, unit);
			switch (cTime.unit)
			{
				case EUnit.MICROSECOND: cTime.time /= 1000_000; cTime.unit = EUnit.SECOND; break;
                case EUnit.ME5SECOND: cTime.time /= 100_000; cTime.unit = EUnit.SECOND; break;
				case EUnit.MILLISECOND: cTime.time /= 1000; cTime.unit = EUnit.SECOND; break;
                case EUnit.SECOND: return cTime;
                default: cTime.ToMinutes(); cTime.time *= 60; break;
			}
            return cTime;
		}

		public Time ToMinutes()
		{
			var cTime = new Time(time, unit);
			switch (cTime.unit)
			{
                case EUnit.MICROSECOND: 
                case EUnit.ME5SECOND:
				case EUnit.MILLISECOND: cTime.ToSeconds(); cTime.time /= 60; break;
				case EUnit.SECOND: cTime.time /= 60; cTime.unit = EUnit.MINUTE; break;
                case EUnit.MINUTE: return cTime;
                default: cTime.ToHours(); cTime.time *= 60; break;
			}
            return cTime;
		}

		public Time ToHours()
		{
            var cTime = new Time(time, unit);
			switch (cTime.unit)
			{
				case EUnit.MICROSECOND:
                case EUnit.ME5SECOND:
				case EUnit.MILLISECOND: 
                case EUnit.SECOND: cTime.ToMinutes(); cTime.time /= 60; break;
                case EUnit.MINUTE: cTime.time /= 60; cTime.unit = EUnit.HOUR; break;
                case EUnit.HOUR: return cTime;
                default: return cTime;
			}
            return cTime;
		}

        public override string ToString()
        {
            string unitName = "";
            switch(unit)
            {
                case EUnit.MICROSECOND: unitName = "us"; break;
                case EUnit.MILLISECOND: unitName = "ms"; break;
                case EUnit.SECOND: unitName = "s"; break;
                case EUnit.MINUTE: unitName = "m"; break;
                case EUnit.HOUR: unitName = "h"; break;
            }
            return string.Format("{0} {1}", time, unitName);
        }

        public static Time Parse(string str)
        {
			if (String.IsNullOrWhiteSpace(str)) throw new ArgumentException(str);

			Match data = Regex.Match(str, @"\W*(\d+\.?\d*)\W*(\w+)");

            EUnit timeUnit;

            switch(data.Groups[2].Value.ToLower())
            {
                case "μs":
                case "μsec":
                case "μsecs":
                case "us":
                case "usec":
                case "usecs":
                case "microsec":
                case "microsecond":
                    timeUnit = EUnit.MICROSECOND; break;
                case "ms":
                case "msec":
                case "msecs":
                case "millisecond":
                case "milliseconds":
                    timeUnit = EUnit.MILLISECOND; break;
                case "s":
                case "sec":
                case "secs":
                case "second":
                case "seconds":
                    timeUnit = EUnit.SECOND; break;
                case "m":
                case "min":
                case "minute":
                    timeUnit = EUnit.MINUTE; break;
                case "h":
                case "hr":
                case "hrs":
                case "hour":
                case "hours":
                    timeUnit = EUnit.HOUR; break;
                default : timeUnit = EUnit.SECOND; break;
            }

            return new Time(double.Parse(data.Groups[1].Value), timeUnit);
        }

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement();
            var readNode = Parse(reader.ReadContentAsString());
            time = readNode.time; unit = readNode.unit;
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteString(ToString());
		}
    }
}
