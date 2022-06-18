using MathNet.Numerics.LinearAlgebra;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.NewtonMethod
{
	public class NewtonMethod : AbstractNumericMethod
	{
		protected readonly double _step;

		public NewtonMethod([NotNull] EquationSystem system, double errorRate = 1E-08, double stepValue = 1E-04) : base(system, errorRate)
		{
			this._step = stepValue;
		}

		protected override void SetInitialStatus(Vector<double> values)
		{
			this.Solution = values;
			this.Status = CompletedStatus.InProcess;
			this.Error = null;
		}

		protected override bool ComputeIteration(int iteration)
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			var currentIteartionValues = this._equationSystem.Compute(this.Solution);

			var newSolution = this.Solution - (this.ComputeJacobian(currentIteartionValues).Inverse() * currentIteartionValues);
			var isCompleted = Math.Abs(newSolution.Norm(2) - this.Solution.Norm(2)) <= this._errorRate;

			this.Solution = newSolution;
			this.Iteration = iteration;

			return isCompleted;
		}

		// Порефачить метод. Много чего мне здесь не нравится
		protected virtual Matrix<double> ComputeJacobian(Vector<double> computedFunctionValues)
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			if (this._equationSystem.Size != this.Solution.Count)
			{
				throw new ArgumentException("Passed values should have the same length as number of equations");
			}

			// Задаем нулевую матрицу нужного размера
			var jacobian = Matrix<double>.Build.Dense(this.Solution.Count, this.Solution.Count, 0);

			// TODO провести рефакторинг метода. Мне не нравится
			// Получаем исходную матрицу Якоби для нулевого случая
			for (var index = 0; index < this.Solution.Count; index++)
			{
				this.Solution[index] += this._step;
				var newColumn = (this._equationSystem.Compute(this.Solution) - computedFunctionValues).Divide(this._step);
				jacobian.SetColumn(index, newColumn);
				this.Solution[index] -= this._step;
			}

			return jacobian;
		}

	}
}
