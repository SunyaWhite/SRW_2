using MathNet.Numerics.LinearAlgebra;
using Newton.Utils;
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

		public override Vector<double> SolveEquationSystem(Vector<double> values, int maxIteration = 100, bool verbose = false)
		{
			return base.SolveEquationSystem(values, maxIteration, verbose);
		}

		public Vector<double> SolveEquationSystem(Vector<double> values, int maxIteration = 100, int maxDegreeOfParallel = 2, bool verbose = false)
		{
			this._maxDegreeOfParallel = maxDegreeOfParallel;
			return base.SolveEquationSystem(values, maxIteration, verbose);
		}

		// TODO рефакторинг метода
		protected override bool ComputeIteration(int iteration)
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			var newSolution = this._equationSystem.ComputeParallel(this.Solution, this._maxDegreeOfParallel);
			var newNormaValue = newSolution.GetNorma();
			var isCompleted = Math.Abs(newNormaValue - this._currentNorma!.Value) < this._errorRate;

			this.Solution = newSolution;
			this._currentNorma = newNormaValue;
			this.Iteration = iteration;

			return isCompleted;
		}
	}
}
