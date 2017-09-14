using System;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
namespace DIVALib.Math
{
    public class Angle
    {
		public enum EUnit
		{
			DEGREE,
			RADIAN,
			GRADIAN,
            QUADRANT
		};

        const float degreeRadian = 57.2957f;
        const float radianGradian = 0.01570f;
        const float quadDegree = 90.0f;


		public EUnit unit;
        public float angle;

        public Angle()
        {
            angle = 0;
        }

        public Angle(float value, EUnit valueUnit=EUnit.DEGREE)
        {
            unit = valueUnit;
            angle = value;
        }

        public Angle ToDegree(EUnit newUnit)
        {
            switch(newUnit)
            {
                case EUnit.DEGREE: return this; 
                case EUnit.RADIAN: return new Angle(angle * degreeRadian);
                case EUnit.GRADIAN: return new Angle((angle * radianGradian) * degreeRadian); 
                case EUnit.QUADRANT: return new Angle(angle * quadDegree);
                default: return this;
            }
        }

        public Angle ToRadian(EUnit newUnit)
        {
			switch (newUnit)
			{
				case EUnit.DEGREE: return new Angle(angle / degreeRadian, EUnit.RADIAN);
                case EUnit.RADIAN: return this;
                case EUnit.GRADIAN: return new Angle(angle / radianGradian, EUnit.RADIAN);
                case EUnit.QUADRANT: return new Angle((angle * quadDegree) / degreeRadian, EUnit.RADIAN);
                default: return this;
			}
        }
    }
}
