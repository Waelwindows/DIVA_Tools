namespace DIVALib.Math
{
	public class Angle
	{
		public enum EUnit
		{
			Degree,
			Radian,
			Gradian,
			Quadrant
		};

	    public const float Degree2Radian = 57.2957f;
	    public const float Radian2Gradian = 0.01570f;
	    public const float Quad2Degree = 90.0f;

		public EUnit Unit;
		public float Value;

		public Angle()
		{
			Value = 0;
		}

		public Angle(float value, EUnit valueUnit=EUnit.Degree)
		{
			Unit = valueUnit;
			Value = value;
		}

		public Angle ToDegree(EUnit newUnit)
		{
			switch(newUnit)
			{
				case EUnit.Degree: return this; 
				case EUnit.Radian: return new Angle(Value * Degree2Radian);
				case EUnit.Gradian: return new Angle((Value * Radian2Gradian) * Degree2Radian); 
				case EUnit.Quadrant: return new Angle(Value * Quad2Degree);
				default: return this;
			}
		}

		public Angle ToRadian(EUnit newUnit)
		{
			switch (newUnit)
			{
				case EUnit.Degree: return new Angle(Value / Degree2Radian, EUnit.Radian);
				case EUnit.Radian: return this;
				case EUnit.Gradian: return new Angle(Value / Radian2Gradian, EUnit.Radian);
				case EUnit.Quadrant: return new Angle((Value * Quad2Degree) / Degree2Radian, EUnit.Radian);
				default: return this;
			}
		}
	}
}
