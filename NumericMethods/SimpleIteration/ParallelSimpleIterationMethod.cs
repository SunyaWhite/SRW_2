using MathNet.Numerics.LinearAlgebra;
using Newtow.Equations;

namespace Newton.NumericMethods.SimpleIteration
{
	public class MultiThreadSimpleIterationMethod : SimpleIterationMethod
	{
		private int _degreeOfParallelism;
		public int? DegreeOfParallelism
		{
			get => _degreeOfParallelism;
			protected set
			{
				if (value == null)
				{
					return;
				}
				_degreeOfParallelism = value.Value;
			}
		}

		public MultiThreadSimpleIterationMethod(double errorRate = .00001f, int? degreeOfParallelism = 2) : base(errorRate)
		{
			this.DegreeOfParallelism = degreeOfParallelism;
		}

		public Vector<double> SolveEquationSystem(EquationSystem equationSystem, Vector<double> values, int maxIteration = 100, int? degreeOfParallelism = null, bool verbose = false)
		{
			this.DegreeOfParallelism = degreeOfParallelism;
			return base.SolveEquationSystem(equationSystem, values, maxIteration, verbose);
		}

		/// <summary>
		/// Computing function results for current values in parallel
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NullReferenceException"></exception>
		protected override Vector<double> ComputeNewIterationSolution()
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			if (this.EquationSystem == null)
			{
				throw new NullReferenceException("Equation system object should be set first before launching parallel method");
			}

			if (this.DegreeOfParallelism == null)
			{
				throw new NullReferenceException("Degree of parallelism should be set first before launching parallel method");
			}

			return this.EquationSystem.ComputeParallel(this.Solution, this.DegreeOfParallelism.Value);
		}
	}
}
