using MathNet.Numerics.LinearAlgebra;
using Newtow.Equations;

namespace Newton.NumericMethods
{
	public abstract class AbstractNumericMethod
	{
		protected readonly EquationSystem _equationSystem;
		protected readonly double _errorRate;
		public Vector<double>? Solution { get; protected set; }
		public string? Error { get; protected set; }
		public CompletedStatus Status { get; protected set; }
		public int Iteration { get; protected set; }

		public AbstractNumericMethod(EquationSystem equationSystem, double errorRate = 1E-08)
		{
			this._equationSystem = equationSystem;
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

		protected abstract void SetInitialStatus(Vector<double> values);

		protected abstract bool ComputeIteration(int iteration);

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

		public virtual void DisplayResults()
		{
			if (this.Status == CompletedStatus.CompletedWithError)
			{
				DisplayError();
			}
			else if (this.Solution == null)
			{
				DisplayError("Solution is null. Can't display null as a result");
			}
			else
			{
				this.DisplaySolution();
			}
		}

		private void DisplayError(string? errorMessage = null)
		{
			var errorToDisplay = this.Error ?? errorMessage ?? "Unknown error occurred";

			Console.WriteLine($"Iteration: {this.Iteration}");
			Console.WriteLine("\n_________________________\n");
			Console.WriteLine("Completed with an error");
			Console.WriteLine($"Error : {errorToDisplay}");
			Console.WriteLine("\n_________________________\n");
		}

		private void DisplaySolution()
		{
			Console.WriteLine($"Iteration: {this.Iteration}");
			Console.WriteLine("\n_________________________\n");

			foreach (var value in this.Solution!)
			{
				Console.Write($"{value} ");
			}

			Console.WriteLine("\n_________________________\n");
		}
	}
}
