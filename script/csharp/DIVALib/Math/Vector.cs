using System;
using System.Collections;

namespace DIVALib.Math
{
    public class Vector2
    {
        public float x;
        public float y;

        public Vector2()
        {
            x = y = 0;
        }

		public Vector2(float value)
		{
			x = y = value;
		}

		public Vector2(float xval, float yval)
		{
            x = xval;
            y = yval;
		}

        public float Dot(Vector2 other)
        {
            return (x * other.x) + (y * other.y);
        }

        public override string ToString()
        {
            return string.Format(string.Format("({0}, {1})", x, y));
        }
    }

	public class Vector3 : IEnumerator, IEnumerable
	{
		public float x;
		public float y;
        public float z;
        int position;
        float[] components;

		public Vector3()
		{
			x = y = z = 0;
            components = new float[3] { x, y, z };
		}

		public Vector3(float value)
		{
			x = y = z = value;
            components = new float[3] { x, y, z };
		}

		public Vector3(float x_value, float y_value, float z_value)
		{
			x = x_value;
			y = y_value;
            y = z_value;
            components = new float[3] { x, y, z };
		}

		//IEnumerator and IEnumerable require these methods.
		public IEnumerator GetEnumerator()
		{
			return (IEnumerator)this;
		}

		//IEnumerator
		public bool MoveNext()
		{
			position++;
			return (position < components.Length);
		}

		//IEnumerable
		public void Reset() { position = 0; }

		//IEnumerable
		public object Current
		{
			get { return components[position]; }
		}

        public float Dot(Vector3 other)
        {
            return (x * other.x) + (y * other.y) + (z * other.z);
        }

		public Vector3 Cross(Vector3 other)
		{
            return new Vector3(((y * other.z) - (z * other.y)), ((z * other.x) - (x * other.z)), ((x * other.y) - (y * other.x)));
		}

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x + right.x, left.y + right.y, left.z + right.z);
        }

		public static Vector3 operator +(Vector3 left, float value)
		{
			return new Vector3(left.x + value, left.y + value, left.z + value);
		}

		public static Vector3 operator -(Vector3 left, Vector3 right)
		{
			return new Vector3(left.x - right.x, left.y - right.y, left.z - right.z);
		}

		public static Vector3 operator -(Vector3 left, float value)
		{
			return new Vector3(left.x - value, left.y - value, left.z - value);
		}

		public static Vector3 operator *(Vector3 left, Vector3 right)
		{
			return new Vector3(left.x * right.x, left.y * right.y, left.z * right.z);
		}

		public static Vector3 operator *(Vector3 left, float value)
		{
			return new Vector3(left.x * value, left.y * value, left.z * value);
		}

		public static Vector3 operator /(Vector3 left, Vector3 right)
		{
			return new Vector3(left.x / right.x, left.y / right.y, left.z / right.z);
		}

		public static Vector3 operator /(Vector3 left, float value)
		{
			return new Vector3(left.x / value, left.y / value, left.z / value);
		}

        public override string ToString()
        {
            return string.Format(string.Format("({0}, {1}, {2})", x, y, z));
        }
	}
}
