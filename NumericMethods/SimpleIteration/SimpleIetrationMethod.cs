using MathNet.Numerics.LinearAlgebra;
using Newton.Utils;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.SimpleIteration
{
	public class SimpleIterationMethod : AbstractNumericMethod
	{
		protected double? _currentNorma { get; set; }

		public SimpleIterationMethod([NotNull] EquivalentEquationSystem system, double errorRate = .00001f) : base(system, errorRate) { }

		protected override void SetInitialStatus(Vector<double> values)
		{
			this.Solution = values;
			this._currentNorma = this.Solution.GetNorma();
			this.Status = CompletedStatus.InProcess;
			this.Error = null;
		}

		protected virtual Vector<double> ComputeNewIterationSolution()
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			if (this._equationSystem == null)
			{
				throw new NullReferenceException("Equation system object should be set first before launching parallel method");
			}

			return this._equationSystem.Compute(this.Solution);
		}

		protected override bool ComputeIteration(int iteration)
		{
			var newSolution = this.ComputeNewIterationSolution();
			var newNormaValue = newSolution.GetNorma();
			var isCompleted = Math.Abs(newNormaValue - this._currentNorma!.Value) < this._errorRate;

			this.Solution = newSolution;
			this._currentNorma = newNormaValue;
			this.Iteration = iteration;

			return isCompleted;
		}
	}
}