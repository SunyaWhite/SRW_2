using MathNet.Numerics.LinearAlgebra;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.SimpleIteration
{
	public class SimpleIterationMethod : AbstractNumericMethod
	{

		public SimpleIterationMethod(double errorRate = .00001f) : base(errorRate) { }

		protected override void SetInitialStatus([NotNull] EquationSystem equationSystem, Vector<double> values)
		{
			if (equationSystem is not EquivalentEquationSystem)
			{
				throw new ArgumentException("Simple iteration method requires equivalent equation system object. Invalid argument provided");
			}

			this.Solution = values;
			this.EquationSystem = equationSystem;
			this.CurrentNorm = this.Solution.Norm(2);
			this.Status = CompletedStatus.InProcess;
			this.Error = null;
		}

		/// <summary>
		/// Computing function results for current values
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NullReferenceException"></exception>
		protected virtual Vector<double> ComputeNewIterationSolution()
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			if (this.EquationSystem == null)
			{
				throw new NullReferenceException("Equation system object should be set first before launching parallel method");
			}

			return this.EquationSystem.Compute(this.Solution);
		}

		protected override bool ComputeIteration(int iteration)
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			if (this.CurrentNorm == null)
			{
				this.CurrentNorm = this.Solution.Norm(2);
			}

			var newSolution = this.ComputeNewIterationSolution();
			var newNorm = newSolution.Norm(2);
			var isCompleted = Math.Abs(newNorm - this.CurrentNorm!.Value) < this._errorRate;

			this.Solution = newSolution;
			this.CurrentNorm = newNorm;
			this.Iteration = iteration;

			return isCompleted;
		}
	}
}