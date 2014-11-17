// Copyright 2014 Christopher Pattison
using System;
using System.Collections.Generic;

namespace PhysicsEngine
{

	public interface IPhysicsObject : ICloneable
	{
		object UniqueID { get; }
		vector Position { get; set; }
		vector Velocity { get; set; }
		double Mass { get; }
		double BaseAtmosDensity { get; }
		double BaseAtmosRadius { get; }
		double AAtmos { get; }
		angle Orientation { get; set; }
		vector Acceleration { get; }
		void Step(double dt);
	}

	public interface IDebug
	{
		void Print(object a);
	}

	public class OrbitalPhysics : ICloneable
	{
		public const double MKGS = 6.67384e-11;		//meter, kilogram, second
		public const double KMKGS = 6.67384e-20;	//kilometer, kilogram, second
		public const double MMKGS = 6.67384e-29;	//megameter, kilogram, second
		public const double GMKGS = 6.67384e-38;	//gigameter, kilogram, second
		public const double AUKGS = 1.99342e-44;	//Astronomical Unit, kilogram, second

		readonly double GravityConstant;
		private List<IPhysicsObject> PhysObjects;
		public double TimeElapsed;


		public OrbitalPhysics(double GravConst) {
			GravityConstant = GravConst;
			PhysObjects = new List<IPhysicsObject>();
			TimeElapsed = 0;
		}

		public OrbitalPhysics(List<IPhysicsObject> state, double GravConst) {
			GravityConstant = GravConst;
			PhysObjects = state;
			TimeElapsed = 0;
		}

		public OrbitalPhysics(IPhysicsObject[] state, double GravConst) {
			GravityConstant = GravConst;
			PhysObjects = new List<IPhysicsObject>();
			PhysObjects.AddRange(state);
			TimeElapsed = 0;
		}

		public void RegisterObject(IPhysicsObject Obj) {
			PhysObjects.Add(Obj);
		}

		public void UnRegisterObject(IPhysicsObject Obj) {
			PhysObjects.Remove(Obj);
		}

		public IPhysicsObject GetObject(object UniqueID) {
			return PhysObjects.Find(candidate => { return candidate.UniqueID.Equals(UniqueID); });
		}

		public void Step(double dt, double domain) {
			dt = dt > domain ? domain : dt;
			for (double i = 0; i < domain; i += dt) {
				foreach (IPhysicsObject a in PhysObjects) {
					foreach (IPhysicsObject b in PhysObjects) {
						if (a != b) {
							a.Velocity += Gravity(a, b) * dt;
						}
					}
					a.Step(dt);
					a.Velocity += a.Acceleration * dt;
				}
				foreach (IPhysicsObject a in PhysObjects) {
					a.Position += a.Velocity * dt;
				}
				if (domain-i < dt) { dt = (domain-i); }
			}
			TimeElapsed += domain;
		}

		public void VirtualStep(double dt, double domain, object UID, out List<vector> result, ref bool running, IDebug dbg) { // returns future positions of an object
			running = true;
			foreach (IPhysicsObject a in PhysObjects)
			{
				dbg.Print(a.GetHashCode());
			}
			OrbitalPhysics VirtualCopy = (OrbitalPhysics)this.Clone();
			foreach (IPhysicsObject a in VirtualCopy.PhysObjects)
			{
				dbg.Print(a.GetHashCode());
			}
			List<vector> resultCopy = new List<vector>();
			for (double i = 0; i < domain; i += dt) {
				foreach (IPhysicsObject a in VirtualCopy.PhysObjects) {
					foreach (IPhysicsObject b in VirtualCopy.PhysObjects) {
						if (a != b) {
							a.Velocity += Gravity(a, b) * dt;
						}
					}
					a.Velocity += a.Acceleration * dt;
				}
				foreach (IPhysicsObject a in VirtualCopy.PhysObjects) {
					a.Position += a.Velocity * dt;
					if (a.UniqueID.Equals(UID)) {
						resultCopy.Add(a.Position);
					}
				}
				if (domain - i < dt) { dt = (domain - i); }
			}
			result = resultCopy;
			running = false;
		}

		private vector Gravity(IPhysicsObject a, IPhysicsObject b) {
			return a.Position.GetAzimuth(b.Position).UnitVector() *
				(this.GravityConstant * b.Mass / a.Position.DistanceSqrd(b.Position));
		}

		public object Clone() {
			List<IPhysicsObject> State = new List<IPhysicsObject>();
			for (int i = 0; i < PhysObjects.Count; i++) {
				State.Add((IPhysicsObject)PhysObjects[i].Clone());
			}
			return new OrbitalPhysics(State, GravityConstant);
		}
	}
}