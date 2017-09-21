using System;
using System.Collections;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using System.Xml;

namespace DIVALib.Math
{
    public class Vector2 : IEnumerator, IEnumerable, IXmlSerializable
    {
        public double x;
        public double y;
        int position = 0;
        double[] components;

        public Vector2()
        {
            x = y = 0;
            components = new double[] {x, y};
        }

        public Vector2(double value)
        {
            x = y = value;
            components = new double[] { x, y };
        }

        public Vector2(double x_value, double y_value)
        {
            x = x_value;
            y = y_value;
            components = new double[] { x, y };
        }

        public double Dot(Vector2 other)
        {
            return (x * other.x) + (y * other.y);
        }

        public override string ToString()
        {
            return string.Format(string.Format("({0}, {1})", x, y));
        }

		public XmlSchema GetSchema()
		{
			return null;
		}

		public static Vector2 Parse(string str)
		{
			if (String.IsNullOrWhiteSpace(str)) throw new ArgumentException(str);

			Match data = Regex.Match(str, @"\W*(\d+\.?\d*)\W+(\d+\.?\d*)\W*");

			return new Vector2(double.Parse(data.Groups[1].Value), double.Parse(data.Groups[2].Value));
		}

		public void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement();
			var c = Parse(reader.ReadContentAsString()).components;
			x = c[0]; y = c[1];
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer)
		{
            writer.WriteAttributeString("type", "vec2");
			writer.WriteString(ToString());
		}

		//IEnumerator and IEnumerable require these methods.
		public IEnumerator GetEnumerator()
		{
			return this;
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

		public void Add(System.Object o)
		{
			throw new NotSupportedException("Can't add more components to a Vector2");
		}

	}

    public class Vector3 : IEnumerator, IEnumerable, IXmlSerializable
    {
        public double x;
        public double y;
        public double z;
        int position;
        double[] components;

        public Vector3()
        {
            x = y = z = 0;
            components = new double[3] { x, y, z };
        }

        public Vector3(double value)
        {
            x = y = z = value;
            components = new double[3] { x, y, z };
        }

        public Vector3(Vector2 vec2, double z_value)
        {
            x = vec2.x;
            y = vec2.y;
            z = z_value;
            components = new double[3] { x, y, z };
        }

        public Vector3(double x_value, double y_value, double z_value)
        {
            x = x_value;
            y = y_value;
            z = z_value;
            components = new double[3] { x, y, z };
        }

        public static Vector3 Parse(string str)
        {
            if (String.IsNullOrWhiteSpace(str)) throw new ArgumentException(str);

            Match data = Regex.Match(str, @"\W*(\d+\.?\d*)\W+(\d+\.?\d*)\W+(\d+\.?\d*)\W*");

            return new Vector3(double.Parse(data.Groups[1].Value), double.Parse(data.Groups[2].Value), double.Parse(data.Groups[3].Value));
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            var c = Parse(reader.ReadContentAsString()).components;
            x = c[0]; y = c[1]; z = c[2];
            reader.ReadEndElement();
        }

		public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("type", "vec3");
            writer.WriteString(ToString());
        }

		//IEnumerator and IEnumerable require these methods.
		public IEnumerator GetEnumerator()
        {
            return this;
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

        public void Add(System.Object o)
        {
			throw new NotSupportedException("Can't add more components to a Vector3");
		}

        public double Dot(Vector3 other)
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

        public class Vector4 : IEnumerator, IEnumerable
        {
            public double x;
            public double y;
            public double z;
            public double w;
            int position;
            double[] components;

            public Vector4()
            {
                x = y = z = w = 0;
                components = new double[] { x, y, z, w };
            }

            public Vector4(double value)
            {
                x = y = z = w = value;
                components = new double[] { x, y, z, w };
            }

            public Vector4(Vector3 vec3)
            {
                x = vec3.x;
                y = vec3.y;
                z = vec3.z;
                w = 0;
                components = new double[] { x, y, z, w };
            }

            public Vector4(Vector3 vec3, double w_value)
            {
                x = vec3.x;
                y = vec3.y;
                z = vec3.z;
                w = w_value;
                components = new double[] { x, y, z, w };
            }

            public Vector4(double x_value, double y_value, double z_value, double w_value)
            {
                x = x_value;
                y = y_value;
                y = z_value;
                w = w_value;
                components = new double[] { x, y, z, w };
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

            public double Dot(Vector4 other)
            {
                return (x * other.x) + (y * other.y) + (z * other.z) + (w * other.w);
            }

            public static Vector4 operator +(Vector4 left, Vector4 right)
            {
                return new Vector4(left.x + right.x, left.y + right.y, left.z + right.z, left.w + right.w);
            }

            public static Vector4 operator +(Vector4 left, float value)
            {
                return new Vector4(left.x + value, left.y + value, left.z + value, left.w + value);
            }

            public static Vector4 operator -(Vector4 left, Vector4 right)
            {
                return new Vector4(left.x - right.x, left.y - right.y, left.z - right.z, left.w - right.w);
            }

            public static Vector4 operator -(Vector4 left, float value)
            {
                return new Vector4(left.x - value, left.y - value, left.z - value, left.w - value);
            }

            public static Vector4 operator *(Vector4 left, Vector4 right)
            {
                return new Vector4(left.x * right.x, left.y * right.y, left.z * right.z, left.w * right.w);
            }

            public static Vector4 operator *(Vector4 left, float value)
            {
                return new Vector4(left.x * value, left.y * value, left.z * value, left.w + value);
            }

            public static Vector4 operator /(Vector4 left, Vector4 right)
            {
                return new Vector4(left.x / right.x, left.y / right.y, left.z / right.z, left.w / right.w);
            }

            public static Vector4 operator /(Vector4 left, float value)
            {
                return new Vector4(left.x / value, left.y / value, left.z / value, left.w / value);
            }

            public override string ToString()
            {
                return string.Format(string.Format("({0}, {1}, {2}, {3})", x, y, z, w));
            }
        }
    }
}