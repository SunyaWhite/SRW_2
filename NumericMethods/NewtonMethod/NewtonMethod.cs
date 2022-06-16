using MathNet.Numerics.LinearAlgebra;
using Newton.Utils;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.NewtonMethod
{
	// TODO провести рефакториг с выделением отдельной иерархии классов (выделить отдельный родительский класс)
	// возможжно нажжо перераспределить обяханности между классами EquationSystem и ...Method
	public class NewtonMethod
	{
		protected readonly EquationSystem _equationSystem;
		protected readonly double _errorRate;
		protected readonly double _step;
		protected double? _currentNorma { get; set; }
		public Vector<double>? Solution { get; protected set; }
		public string? Error { get; protected set; }
		public CompletedStatus Status { get; protected set; }
		public int Iteration { get; protected set; }

		public NewtonMethod([NotNull] EquationSystem system, double errorRate = 1E-08, double stepValue = 1E-04)
		{
			this._equationSystem = system;
			this._errorRate = errorRate;
			this._step = stepValue;
			this.Status = CompletedStatus.NotStarted;
		}

		public virtual Vector<double> SolveEquationSystem(Vector<double> values, int maxIteration = 100, bool verbose = false)
		{
			this.SetInitialStatus(values);

			try
			{
				for (var iteration = 1; iteration <= maxIteration && this.Status != CompletedStatus.Completed; iteration++)
				{
					var isFinalIteration = this.ComputeIteration(iteration);
					if (verbose) this.DisplayResults();
					if (isFinalIteration) CompleteAsSuccess();
				}
			}
			catch (NullReferenceException exc)
			{
				this.CompleteWithError(exc.Message);
			}

			if (this.Iteration == maxIteration && this.Status == CompletedStatus.InProcess)
			{
				CompleteWithError("The maximum number of iteration is exceeded");
			}

			if (this.Solution == null)
			{
				CompleteWithError("Failed to complete computation. Unknown error occurred");
			}

			return this.Solution!;
		}

		// Порефачить метод. Много чего мне здесь не нравится
		protected virtual Matrix<double> ComputeJacobian(Vector<double> values, Vector<double> computedValues)
		{
			if (this._equationSystem.Size != values.Count)
			{
				throw new ArgumentException("Passed values should have the same length as number of equations");
			}

			// Задаем нулевую матрицу нужного размера
			var jacobian = Matrix<double>.Build.Dense(values.Count, values.Count, 0);

			// TODO провести рефакторинг метода. Мне не нравится
			// Получаем исходную матрицу Якоби для нулевого случая
			for (var index = 0; index < values.Count; index++)
			{
				values[index] += this._step;
				var newColumn = (this._equationSystem.Compute(values) - computedValues).Divide(this._step);
				jacobian.SetColumn(index, newColumn);
				values[index] -= this._step;
			}

			return jacobian;
		}

		protected virtual void SetInitialStatus(Vector<double> values)
		{
			this.Solution = values;
			this._currentNorma = this.Solution.GetNormaForNewton();
			this.Status = CompletedStatus.InProcess;
			this.Error = null;
		}

		protected virtual void CompleteAsSuccess()
		{
			this.Status = CompletedStatus.Completed;
		}

		protected virtual void CompleteWithError(string errorMesage)
		{
			this.Status = CompletedStatus.CompletedWithError;
			this.Error = errorMesage;
			throw new NumericMethodException(errorMesage);
		}

		protected virtual bool ComputeIteration(int iteration)
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			var cachedComputation = this._equationSystem.Compute(this.Solution);

			var newSolution = this.Solution - (this.ComputeJacobian(this.Solution, cachedComputation).Inverse() * cachedComputation);
			var newNormaValue = newSolution.GetNormaForNewton();
			var isCompleted = Math.Abs(newSolution.Norm(2) - this.Solution.Norm(2)) <= this._errorRate;

			this.Solution = newSolution;
			this._currentNorma = newNormaValue;
			this.Iteration = iteration;

			return isCompleted;
		}

		public void DisplayResults()
		{
			if (this.Solution == null)
			{
				throw new NullReferenceException("Solution cannot contains null");
			}

			Console.WriteLine($"Iteration: {this.Iteration}");
			Console.WriteLine("\n_________________________\n");

			foreach (var value in this.Solution)
			{
				Console.Write($"{value} ");
			}

			Console.WriteLine("\n_________________________\n");
		}
	}
}
