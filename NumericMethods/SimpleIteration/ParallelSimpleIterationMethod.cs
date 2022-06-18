using MathNet.Numerics.LinearAlgebra;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

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

		public MultiThreadSimpleIterationMethod([NotNull] EquivalentEquationSystem system, double errorRate = .00001f, int? degreeOfParallelism = 2) : base(system, errorRate)
		{
			this.DegreeOfParallelism = degreeOfParallelism;
		}

		public override Vector<double> SolveEquationSystem(Vector<double> values, int maxIteration = 100, bool verbose = false)
		{
			return base.SolveEquationSystem(values, maxIteration, verbose);
		}

		public Vector<double> SolveEquationSystem(Vector<double> values, int maxIteration = 100, int? degreeOfParallelism = 2, bool verbose = false)
		{
			this.DegreeOfParallelism = degreeOfParallelism ?? this.DegreeOfParallelism;
			return base.SolveEquationSystem(values, maxIteration, verbose);
		}

		protected override Vector<double> ComputeNewIterationSolution()
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			if (this._equationSystem == null)
			{
				throw new NullReferenceException("Equation system object should be set first before launching parallel method");
			}

			if (this.DegreeOfParallelism == null)
			{
				throw new NullReferenceException("Degree of parallelism should be set first before launching parallel method");
			}

			return this._equationSystem.ComputeParallel(this.Solution, this.DegreeOfParallelism.Value);
		}
	}
}
