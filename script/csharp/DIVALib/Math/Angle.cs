using System;
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
                case EUnit.DEGREE: return this; break;
                case EUnit.RADIAN: return new Angle(angle * degreeRadian); break;
                case EUnit.GRADIAN: return new Angle((angle * radianGradian) * degreeRadian); break;
                case EUnit.QUADRANT: return new Angle(angle * quadDegree); break;
                default: return this; break;
            }
        }

        public Angle ToRadian(EUnit newUnit)
        {
			switch (newUnit)
			{
				case EUnit.DEGREE: return new Angle(angle / degreeRadian, EUnit.RADIAN); break;
                case EUnit.RADIAN: return this; break;
                case EUnit.GRADIAN: return new Angle(angle / radianGradian, EUnit.RADIAN); break;
                case EUnit.QUADRANT: return new Angle((angle * quadDegree) / degreeRadian, EUnit.RADIAN); break;
                default: return this; break;
			}
        }
    }
}
