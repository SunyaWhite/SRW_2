using MathNet.Numerics.LinearAlgebra;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.NewtonMethod
{
	public class NewtonMethod : AbstractNumericMethod
	{
		protected readonly double _step;

		public NewtonMethod(double errorRate = 1E-08, double stepValue = 1E-04) : base(errorRate)
		{
			this._step = stepValue;
		}

		protected override void SetInitialStatus([NotNull] EquationSystem equationSystem, Vector<double> values)
		{
			this.Solution = values;
			this.EquationSystem = equationSystem;
			this.Status = CompletedStatus.InProcess;
			this.Error = null;
			this.CurrentNorm = this.Solution.Norm(2);
		}

		/// <summary>
		/// Jacobian matrix computation without precalculated function results.
		/// </summary>
		/// <returns></returns>
		protected virtual Matrix<double> ComputeJacobian()
		{
			return this.ComputeJacobian(this.EquationSystem.Compute(this.Solution));
		}

		/// <summary>
		/// Jacobian matrix computation
		/// </summary>
		/// <param name="computedFunctionValues"></param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException"></exception>
		/// <exception cref="ArgumentException"></exception>
		protected virtual Matrix<double> ComputeJacobian(Vector<double> computedFunctionValues)
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			if (this.EquationSystem.Size != this.Solution.Count)
			{
				throw new ArgumentException("Passed values should have the same length as number of equations");
			}

			var jacobian = Matrix<double>.Build.Dense(this.Solution.Count, this.Solution.Count, 0);

			for (var index = 0; index < this.Solution.Count; index++)
			{
				this.Solution[index] += this._step;
				var newColumn = (this.EquationSystem.Compute(this.Solution) - computedFunctionValues).Divide(this._step);
				jacobian.SetColumn(index, newColumn);
				this.Solution[index] -= this._step;
			}

			return jacobian;
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

			var iterationDeviation = this.ComputeDeviation();
			var newSolution = this.Solution - iterationDeviation;

			var newNorm = newSolution.Norm(2);
			var isCompleted = Math.Abs(newNorm - this.CurrentNorm.Value) <= this._errorRate;

			this.CurrentNorm = newNorm;
			this.Solution = newSolution;
			this.Iteration = iteration;

			return isCompleted;
		}

		/// <summary>
		/// Compute current iteration deviation for x = J(x(k))^(-1) * f(x(k))
		/// </summary>
		/// <returns> Vector of deviation values </returns>
		/// <exception cref="NullReferenceException"></exception>
		protected virtual Vector<double> ComputeDeviation()
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			var currentIteartionValues = this.EquationSystem.Compute(this.Solution);

			return this.ComputeJacobian(currentIteartionValues).Inverse() * currentIteartionValues;
		}
	}
}
