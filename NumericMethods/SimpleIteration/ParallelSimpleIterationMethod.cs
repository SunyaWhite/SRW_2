using MathNet.Numerics.LinearAlgebra;
using Newton.utils;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.SimpleIteration
{
	public class MultiThreadSimpleIterationMethod : SimpleIterationMethod
	{
		private int _maxDegreeOfParallel { get; set; }

		public MultiThreadSimpleIterationMethod([NotNull] EquivalentEquationSystem system, double errorRate = .00001f) : base(system, errorRate)
		{
			this._maxDegreeOfParallel = 2;
		}

		public override Vector<double> SolveEquationSystem(Vector<double> values, int maxIteration = 100)
		{
			this._maxDegreeOfParallel = 2;
			return base.SolveEquationSystem(values, maxIteration);
		}

		public Vector<double> SolveEquationSystem(Vector<double> values, int maxIteration = 100, int maxDegreeOfParallel = 2)
		{
			this._maxDegreeOfParallel = maxDegreeOfParallel;
			return base.SolveEquationSystem(values, maxIteration);
		}

		protected override bool ComputeIteration(int iteration)
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			var newSolution = this._equationSystem.Equations
			  .AsParallel()
			  .AsOrdered()
			  .WithDegreeOfParallelism(this._maxDegreeOfParallel)
			  .Select((eq) => eq(this.Solution))
			  .ToArray();

			var newNormaValue = newSolution.GetNorma();
			var isCompleted = Math.Abs(newNormaValue - this._currentNorma!.Value) < this._errorRate;

			this.Solution = Vector<double>.Build.DenseOfArray(newSolution);
			this.Iteration = iteration;
			this._currentNorma = newNormaValue;

			return isCompleted;
		}
	}
}
