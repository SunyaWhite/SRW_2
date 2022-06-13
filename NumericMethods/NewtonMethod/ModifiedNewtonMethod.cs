using MathNet.Numerics.LinearAlgebra;
using Newton.Utils;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.NewtonMethod
{
	// TODO провести рефакториг с выделением отдельной иерархии классов (выделить отдельный родительский класс)
	// возможжно нажжо перераспределить обяханности между классами EquationSystem и ...Method
	public class ModifiedNewtonMethod
	{
		protected readonly EquationSystem _equationSystem;
		protected readonly double _errorRate;
		protected double? _currentNorma { get; set; }
		// TODO найти нормальное имя для свойства
		/// <summary>
		/// Матрица арифметических дополнений для матрицы Якоби
		/// </summary>
		protected Matrix<double>? _jacobian { get; set; }
		protected double? _jacobianDeterminat { get; set; }
		public Vector<double>? Solution { get; protected set; }
		public string? Error { get; protected set; }
		public CompletedStatus Status { get; protected set; }
		public int Iteration { get; protected set; }

		public ModifiedNewtonMethod([NotNull] EquationSystem system, double errorRate = .00001f)
		{
			this._equationSystem = system;
			this._errorRate = errorRate;
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

		protected virtual void ComputeJacobian(Vector<double> values)
		{
			throw new NotImplementedException("");
		}

		protected virtual void SetInitialStatus(Vector<double> values)
		{
			this.Solution = values;
			this._currentNorma = this.Solution.GetNorma();
			this.Status = CompletedStatus.InProcess;
			this.Error = null;
			ComputeJacobian(this.Solution);
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

			if (this._jacobian is null || !this._jacobianDeterminat.HasValue)
			{
				throw new NullReferenceException("Jacobian matrix can't be null.");
			}

			throw new NotImplementedException();
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
