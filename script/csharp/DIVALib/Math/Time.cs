using System;
namespace DIVALib.Math
{
    public class Time
    {
        public enum EUnit 
        {
            MICROSECOND,
            MILLISECOND,
            SECOND,
            MINUTE,
            HOURS
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

		public Time(double value, EUnit unit)
		{
			time = value;
			unit = EUnit.SECOND;
		}
    }
}
