using MathNet.Numerics.LinearAlgebra;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.NewtonMethod
{
	public class ModifiedNewtonMethod : NewtonMethod
	{
		private Matrix<double>? _inversedJacobianMatrix { get; set; }

		public ModifiedNewtonMethod([NotNull] EquationSystem system, double errorRate = 1E-08, double stepValue = 0.0001) : base(system, errorRate, stepValue) { }

		protected override void SetInitialStatus(Vector<double> values)
		{
			base.SetInitialStatus(values);
			this._inversedJacobianMatrix = this.ComputeJacobian(this._equationSystem.Compute(values)).Inverse();
		}

		protected override Vector<double> ComputeDeviation()
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot be null while computing next iteration values");
			}

			if (this._inversedJacobianMatrix == null)
			{
				throw new NullReferenceException("Inversed jacobian matrix cannot be null while computing next iteration values");
			}

			return this._inversedJacobianMatrix * this._equationSystem.Compute(this.Solution);
		}

		protected override bool ComputeIteration(int iteration)
		{
			if (this._inversedJacobianMatrix == null)
			{
				throw new NullReferenceException("Inversed jacobian matrix cannot be null while computing next iteration values");
			}

			return base.ComputeIteration(iteration);
		}
	}
}
