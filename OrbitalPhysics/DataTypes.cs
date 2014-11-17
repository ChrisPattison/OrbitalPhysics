// Copyright 2014 Christopher Pattison
using System;

namespace PhysicsEngine
{
	public struct vector : ICloneable
	{
		public double x;
		public double y;

		public vector(double x, double y) {
			this.x = x;
			this.y = y;
		}

		public vector(angle direction, double magnitude)
		{
			this = direction.UnitVector() * magnitude;
		}

		public static vector operator +(vector a, vector b) {
			return new vector(a.x + b.x, a.y + b.y);
		}

		public static vector operator -(vector a, vector b) {
			return new vector(a.x - b.x, a.y - b.y);
		}

		public static vector operator *(vector a, double b) {
			return new vector(a.x * b, a.y * b);
		}

		public static vector operator *(double a, vector b) {
			return new vector(b.x * a, b.y * a);
		}

		public static vector operator /(vector a, double b) {
			return new vector(a.x / b, a.y / b);
		}

		public static bool operator ==(vector a, vector b) {
			if (a.x == b.x && a.y == b.y) { return true; } else { return false; }
		}

		public static bool operator !=(vector a, vector b) {
			return !(a == b);
		}

		public double Magnitude() {
			return Math.Sqrt(x * x + y * y);
		}

		public double DistanceSqrd(vector b) {
			return Math.Pow(this.x - b.x, 2) + Math.Pow(this.y - b.y, 2);
		}

		public double Distance(vector b) {
			return Math.Sqrt(Math.Pow(this.x - b.x, 2) + Math.Pow(this.y - b.y, 2));
		}

		public angle GetAzimuth(vector b) {
			return new angle(Math.Atan2(b.y - this.y, b.x - this.x));
		}

		public angle Direction() {
			return new angle(Math.Atan2(this.y, this.x));
		}

		public override string ToString() {
			return "X " + Convert.ToString(x) + ", Y " + Convert.ToString(y);
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}

	public struct angle : ICloneable
	{
		private double _ang;
		private double ang { get { return _ang; } set { _ang = Norm(value); } }

		public angle(double radians) {
			_ang = 0;
			ang = radians;
		}

		public static angle operator +(angle a, angle b) {
			return new angle(a.ang+b.ang);
		}

		public static angle operator +(angle a, double b) {
			return new angle(a.ang + b);
		}

		public static angle operator -(angle a, angle b) {
			return new angle(a.ang - b.ang);
		}

		public static angle operator -(angle a, double b) {
			return new angle(a.ang - b);
		}

		public static angle operator *(angle a, double b) {
			return new angle(a.ang * b);
		}

		public static implicit operator double(angle a) {
			return a.ang;
		}

		public static explicit operator float(angle a) {
			return (float)a.ang;
		}

		public double Diff(angle b) {
			return CenterNorm(this.ang-b.ang);
		}

		public vector UnitVector() {
			return new vector(Math.Cos(ang), Math.Sin(ang));
		}

		public static double ToRadians(double degrees) {
			return degrees * Math.PI / 180;
		}

		public static double ToDegrees(double radians) {
			return radians * 180 / Math.PI;
		}

		public double Degrees() {
			return this.ang * 180 / Math.PI;
		}

		private double Norm(double ang) { // returns [0, 2pi)
			while (ang >= 2 * Math.PI) {
				ang -= 2 * Math.PI;
			}
			while (ang < 0) {
				ang += 2 * Math.PI;
			}
			return ang;
		}

		private double CenterNorm(double ang) { // retuns [-pi,pi) 
			while (ang >= Math.PI) {
				ang -= 2 * Math.PI;
			}
			while (ang < -Math.PI) {
				ang += 2 * Math.PI;
			}
			return ang;
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}