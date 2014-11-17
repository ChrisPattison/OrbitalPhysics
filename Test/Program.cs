#define ANGLE_NORM
using System;
using PhysicsEngine;

namespace PhysicsEngine
{
	class Program
	{
		static void Main(string[] args) {
#if ANGLE_NORM
			angle a = new angle(Math.PI*3);
			a += Math.PI*3;
			a += Math.PI;
			a -= Math.PI*3;
			a -= Math.PI;
#endif
#if VECTOR_ADD
			vector c = new vector(1, 2);
			vector d = new vector(5, 6);
			c += d;
#endif
#if PHYSICS_TEST
			OrbitalPhysics phys = new OrbitalPhysics(OrbitalPhysics.KMKGS);
			phys.RegisterObject(new GravityObject(new vector(1e40, 0), new vector(0, 2), "Object 1"));
			phys.RegisterObject(new GravityObject(new vector(-1e40, 0), new vector(0, -2), "Object 2"));
			for (int i = 0; i < 100; i++) {
				phys.Step(10, i * 1e3);
			}
			vector[] pos = phys.VirtualStep(50, 1000, "Object 1");
#endif
		}
	}
#if PHYSICS_TEST
	class GravityObject : IPhysicsObject
	{
		public object UniqueID { get; set; }
		public vector Position { get; set; }
		public vector Velocity { get; set; }
		public double Mass { get { return 1e20; } }
		public double BaseAtmosDensity { get { return 0; } }
		public double BaseAtmosRadius { get { return 0; } }
		public double AAtmos { get { return 0; } }
		public angle Orientation { get; set; }
		public vector Acceleration { get { return new vector(0, 0); } }
		public void Step(double dt) { }
		public object Clone() { return this.MemberwiseClone(); }

		public GravityObject(vector pos, vector vel, object UID) {
			UniqueID = UID;
			Position = pos;
			Velocity = vel;
		}
	}
#endif
}
