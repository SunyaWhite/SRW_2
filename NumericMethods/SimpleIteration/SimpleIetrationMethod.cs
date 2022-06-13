using MathNet.Numerics.LinearAlgebra;
using Newton.utils;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.SimpleIteration
{

	public class SimpleIterationMethod
	{
		protected readonly EquivalentEquationSystem _equationSystem;
		protected readonly double _errorRate;
		protected double? _currentNorma { get; set; }
		public Vector<double>? Solution { get; protected set; }
		public string? Error { get; protected set; }
		public CompletedStatus Status
		{ get; protected set; }
		public int Iteration { get; protected set; }

		public SimpleIterationMethod([NotNull] EquivalentEquationSystem system, double errorRate = .00001f)
		{
			this._equationSystem = system;
			this._errorRate = errorRate;
			this.Status = CompletedStatus.NotStarted;
		}

		public virtual Vector<double> SolveEquationSystem(Vector<double> values, int maxIteration = 100)
		{
			this.SetInitialStatus(values);

			try
			{
				for (var iteration = 1; iteration <= maxIteration && this.Status != CompletedStatus.Completed; iteration++)
				{
					var isFinalIteration = this.ComputeIteration(iteration);
					this.DisplayResults();

					if (isFinalIteration)
					{
						CompleteAsSuccess();
					}
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

		protected virtual void SetInitialStatus(Vector<double> values)
		{
			this.Solution = values;
			this._currentNorma = this.Solution.GetNorma();
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

			var newSolution = this._equationSystem.Compute(this.Solution);
			var newNormaValue = newSolution.GetNorma();
			var isCompleted = Math.Abs(newNormaValue - this._currentNorma!.Value) < this._errorRate;

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