using MathNet.Numerics.LinearAlgebra;
using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods
{
	public abstract class AbstractNumericMethod
	{
		private EquationSystem _equationSystem;
		protected readonly double _errorRate;

		protected EquationSystem EquationSystem
		{
			get => _equationSystem; set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Equation cannot be set to null");
				}

				_equationSystem = value;
			}
		}
		protected double? CurrentNorm { get; set; }
		public Vector<double>? Solution { get; protected set; }
		public string? Error { get; protected set; }
		public CompletedStatus Status { get; protected set; }
		public int Iteration { get; protected set; }

		public AbstractNumericMethod(double errorRate = 1E-08)
		{
			this._errorRate = errorRate;
			this.Status = CompletedStatus.NotStarted;
		}

		/// <summary>
		/// Solve passed equation system with initial values
		/// </summary>
		/// <param name="equationSystem"></param>
		/// <param name="initialValues"></param>
		/// <param name="maxIteration"></param>
		/// <param name="verbose"></param>
		/// <returns></returns>
		public virtual Vector<double> SolveEquationSystem(EquationSystem equationSystem, Vector<double> initialValues, int maxIteration = 100, bool verbose = false)
		{
			this.SetInitialStatus(equationSystem, initialValues);

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

		/// <summary>
		/// Setting initial params before starting computations
		/// </summary>
		/// <param name="equationSystem"></param>
		/// <param name="values"></param>
		protected abstract void SetInitialStatus([NotNull] EquationSystem equationSystem, Vector<double> values);

		/// <summary>
		/// Computing results for given iteartion
		/// </summary>
		/// <param name="iteration"></param>
		/// <returns> is computation completed </returns>
		protected abstract bool ComputeIteration(int iteration);

		/// <summary>
		/// Set computation to completed status
		/// </summary>
		protected virtual void CompleteAsSuccess()
		{
			this.Status = CompletedStatus.Completed;
		}

		/// <summary>
		/// Set compuattion to completed with error status
		/// </summary>
		/// <param name="errorMesage"></param>
		/// <exception cref="NumericMethodException"></exception>
		protected virtual void CompleteWithError(string errorMesage)
		{
			this.Status = CompletedStatus.CompletedWithError;
			this.Error = errorMesage;
			throw new NumericMethodException(errorMesage);
		}

		/// <summary>
		/// Displaying result depending on status of calculation
		/// </summary>
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
