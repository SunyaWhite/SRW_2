using MathNet.Numerics.LinearAlgebra;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.NewtonMethod
{
	public class ModifiedNewtonMethod : NewtonMethod
	{
		private Matrix<double>? _inversedJacobianMatrix { get; set; }

		public ModifiedNewtonMethod(double errorRate = 1E-08, double stepValue = 0.0001) : base(errorRate, stepValue) { }

		protected override void SetInitialStatus([NotNull] EquationSystem equationSystem, Vector<double> values)
		{
			base.SetInitialStatus(equationSystem, values);
			this._inversedJacobianMatrix = this.ComputeJacobian(this.EquationSystem.Compute(values)).Inverse();
		}

		/// <summary>
		/// Compute current iteration deviation for x = J(x(k))^(-1) * f(x(k)) with precalculated Jacobian matrix
		/// </summary>
		/// <returns> Vector of deviation values </returns>
		/// <exception cref="NullReferenceException"></exception>
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

			return this._inversedJacobianMatrix * this.EquationSystem.Compute(this.Solution);
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
