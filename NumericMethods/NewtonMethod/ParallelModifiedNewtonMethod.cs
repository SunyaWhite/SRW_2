using MathNet.Numerics.LinearAlgebra;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.NewtonMethod
{
	internal class ParallelModifiedNewtonMethod : ParallelNewtonMethod
	{
		private Matrix<double>? _inversedJacobianMatrix { get; set; }

		public ParallelModifiedNewtonMethod(double errorRate = 1E-08, double stepValue = 0.0001, int degreeOfParallelism = 2) : base(errorRate, stepValue, degreeOfParallelism) { }

		protected override void SetInitialStatus([NotNull] EquationSystem equationSystem, Vector<double> values)
		{
			base.SetInitialStatus(equationSystem, values);
			this._inversedJacobianMatrix = this.ComputeJacobian().Inverse();
		}

		/// <summary>
		/// Compute current iteration deviation for x = J(x(k))^(-1) * f(x(k)) in parallel
		/// </summary>
		/// <returns> Vector of deviation values </returns>
		/// <exception cref="NullReferenceException"></exception>
		protected override Vector<double> ComputeDeviation()
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			if (this._inversedJacobianMatrix == null)
			{
				throw new NullReferenceException("Inversed Jacobian matrix should be calculated firstly before launching parallel method");
			}

			if (!this.DegreeOfParallelism.HasValue)
			{
				throw new NullReferenceException("Degree of parallelism should be set first before launching parallel method");
			}

			var currentIteartionValues = this.EquationSystem.ComputeParallel(this.Solution, this.DegreeOfParallelism.Value);

			var result = this._inversedJacobianMatrix.EnumerateRows()
				.AsParallel()
				.AsOrdered()
				.WithDegreeOfParallelism(this.DegreeOfParallelism.Value)
				.Select((vector) => currentIteartionValues * vector).ToArray();

			return Vector<double>.Build.Dense(result);
		}
	}
}
