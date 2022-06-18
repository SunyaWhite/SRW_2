using MathNet.Numerics.LinearAlgebra;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.NewtonMethod
{
	public class ParallelNewtonMethod : NewtonMethod
	{
		private int _degreeOfParallelism;
		public int? DegreeOfParallelism
		{
			get => _degreeOfParallelism;
			private set
			{
				if (value == null)
				{
					return;
				}
				_degreeOfParallelism = value.Value;
			}
		}

		public ParallelNewtonMethod([NotNull] EquationSystem system, double errorRate = 1E-08, double stepValue = 0.0001, int degreeOfParallelism = 2) : base(system, errorRate, stepValue)
		{
			this.DegreeOfParallelism = degreeOfParallelism;
		}

		public Vector<double> SolveEquationSystem(Vector<double> values, int maxIteration = 100, bool verbose = false, int? degreeOfParallelism = null)
		{
			this.DegreeOfParallelism = degreeOfParallelism;
			return base.SolveEquationSystem(values, maxIteration, verbose);
		}

		/// <summary>
		/// Parallel impplementation of jacobian matrix computation without precalculated function results.
		/// </summary>
		/// <returns> Jacobian matrix </returns>
		/// <exception cref="NullReferenceException"></exception>
		/// <exception cref="ArgumentException"></exception>
		protected override Matrix<double> ComputeJacobian()
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			if (this._equationSystem.Size != this.Solution.Count)
			{
				throw new ArgumentException("Passed values should have the same length as number of equations");
			}

			if (!this.DegreeOfParallelism.HasValue)
			{
				throw new NullReferenceException("Degree of parallelism should be set first before launching parallel method");
			}

			var computedFunctionValues = this._equationSystem.ComputeParallel(this.Solution, this.DegreeOfParallelism.Value);
			var jacobian = Matrix<double>.Build.Dense(this.Solution.Count, this.Solution.Count, 0);

			for (var index = 0; index < this.Solution.Count; index++)
			{
				this.Solution[index] += this._step;
				var newColumn = (this._equationSystem.ComputeParallel(this.Solution, this.DegreeOfParallelism.Value) - computedFunctionValues).Divide(this._step);
				jacobian.SetColumn(index, newColumn);
				this.Solution[index] -= this._step;
			}

			return jacobian;
		}

		/// <summary>
		/// Parallel impplementation of jacobian matrix computation.
		/// </summary>
		/// <param name="computedFunctionValues">precomputed function values for current solution</param>
		/// <returns> Jacobian matrix </returns>
		/// <exception cref="NullReferenceException"></exception>
		/// <exception cref="ArgumentException"></exception>
		protected override Matrix<double> ComputeJacobian(Vector<double> computedFunctionValues)
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			if (this._equationSystem.Size != this.Solution.Count)
			{
				throw new ArgumentException("Passed values should have the same length as number of equations");
			}

			if (!this.DegreeOfParallelism.HasValue)
			{
				throw new NullReferenceException("Degree of parallelism should be set first before launching parallel method");
			}

			var jacobian = Matrix<double>.Build.Dense(this.Solution.Count, this.Solution.Count, 0);

			for (var index = 0; index < this.Solution.Count; index++)
			{
				this.Solution[index] += this._step;
				var newColumn = (this._equationSystem.ComputeParallel(this.Solution, this.DegreeOfParallelism.Value) - computedFunctionValues).Divide(this._step);
				jacobian.SetColumn(index, newColumn);
				this.Solution[index] -= this._step;
			}

			return jacobian;
		}

		/// <summary>
		/// Compute current iteration deviation for x = J(x(k))^(-1) * f(x(k))
		/// </summary>
		/// <returns> Vector of deviation values </returns>
		/// <exception cref="NullReferenceException"></exception>
		protected Vector<double> ComputeSolutionDeviation()
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			if (!this.DegreeOfParallelism.HasValue)
			{
				throw new NullReferenceException("Degree of parallelism should be set first before launching parallel method");
			}

			var currentIteartionValues = this._equationSystem.ComputeParallel(this.Solution, this.DegreeOfParallelism.Value);

			var inversedJacobianMatrix = this.ComputeJacobian(currentIteartionValues).Inverse();
			var result = inversedJacobianMatrix.EnumerateRows()
				.AsParallel()
				.AsOrdered()
				.WithDegreeOfParallelism(this.DegreeOfParallelism.Value)
				.Select((vector) => vector * currentIteartionValues).ToArray();

			return Vector<double>.Build.Dense(result);
		}

		protected override bool ComputeIteration(int iteration)
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			if (!this.DegreeOfParallelism.HasValue)
			{
				throw new NullReferenceException("Degree of parallelism should be set first before launching parallel method");
			}

			var solutionDeviation = ComputeSolutionDeviation();
			var newSolution = this.Solution - solutionDeviation;
			var isCompleted = Math.Abs(newSolution.Norm(2) - this.Solution.Norm(2)) <= this._errorRate;

			this.Solution = newSolution;
			this.Iteration = iteration;

			return isCompleted;
		}
	}
}
