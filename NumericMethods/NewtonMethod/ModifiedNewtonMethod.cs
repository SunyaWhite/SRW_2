using MathNet.Numerics.LinearAlgebra;
using Newton.Utils;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.NewtonMethod
{
	public class ModifiedNewtonMethod : NewtonMethod
	{
		private Matrix<double> _inversedJacobian { get; set; }

		public ModifiedNewtonMethod([NotNull] EquationSystem system, double errorRate = 1E-08, double stepValue = 0.0001) : base(system, errorRate, stepValue)
		{
		}

		protected override void SetInitialStatus(Vector<double> values)
		{
			base.SetInitialStatus(values);
			this._inversedJacobian = this.ComputeJacobian(values, this._equationSystem.Compute(values)).Inverse();
		}

		protected override bool ComputeIteration(int iteration)
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			var newSolution = this.Solution - (this._inversedJacobian * this._equationSystem.Compute(this.Solution));
			var newNormaValue = newSolution.GetNormaForNewton();
			var isCompleted = Math.Abs(newSolution.Norm(2) - this.Solution.Norm(2)) <= this._errorRate;

			this.Solution = newSolution;
			this._currentNorma = newNormaValue;
			this.Iteration = iteration;

			return isCompleted;
		}
	}
}
