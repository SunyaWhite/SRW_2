using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using MathNet.Numerics.LinearAlgebra;
using Newtow.Equations;
using Newton.Utils;

namespace Newton
{

  public class NumericMethodException : Exception
  {
    public NumericMethodException(string? message) : base(message) { }
  }

  public static class ListExtension
  {
    public static double GetNorma(this IList<double> values)
    {
      return Math.Sqrt(values.Aggregate(0d, (currentSum, currentElem) => currentSum + (currentElem * currentElem)));
    }
  }

  public enum CompletedStatus
  {
    NotStarted,
    InProcess,
    Completed,
    CompletedWithError
  }

  class SimpleIterationMethod
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

      foreach (var value in this.Solution!)
      {
        Console.Write($"{value} ");
      }

      Console.WriteLine("\n_________________________\n");
    }
  }

  class MultiThreadSimpleIterationMethod : SimpleIterationMethod
  {
    private int _maxDegreeOfParallel { get; set; }

    public MultiThreadSimpleIterationMethod([NotNull] EquivalentEquationSystem system, double errorRate = .00001f) : base(system, errorRate)
    {
      this._maxDegreeOfParallel = 2;
    }

    public override Vector<double> SolveEquationSystem(Vector<double> values, int maxIteration = 100)
    {
      this._maxDegreeOfParallel = 2;
      return base.SolveEquationSystem(values, maxIteration);
    }

    public Vector<double> SolveEquationSystem(Vector<double> values, int maxIteration = 100, int maxDegreeOfParallel = 2)
    {
      this._maxDegreeOfParallel = maxDegreeOfParallel;
      return base.SolveEquationSystem(values, maxIteration);
    }

    protected override bool ComputeIteration(int iteration)
    {
      if (this.Solution == null)
      {
        throw new NullReferenceException("Solution cannot contains null");
      }

      var newSolution = this._equationSystem.Equations
        .AsParallel()
        .AsOrdered()
        .WithDegreeOfParallelism(this._maxDegreeOfParallel)
        .Select((eq) => eq(this.Solution))
        .ToArray();

      var newNormaValue = newSolution.GetNorma();
      var isCompleted = Math.Abs(newNormaValue - this._currentNorma!.Value) < this._errorRate;

      this.Solution = Vector<double>.Build.DenseOfArray(newSolution);
      this.Iteration = iteration;
      this._currentNorma = newNormaValue;

      return isCompleted;
    }
  }

  class Programm
  {
    /*
      В качестве системы возьмём "Численные методы. Практикум. Python"
      Пока реализуем 
        - модифицированный метод Ньютона (т.е. считаем Якобиан один раз) 
          пробуем распараллелить - создать матрицу алгебраических дополнений для Якобиана
          Далее на каждой итерации нужно будет создавать новую матрицу у которой находим детерминант
          (идея взята из файла  numeric-methods-part2.pdf)
        - метод простой итерации - пробуем распараллеить (тут несложно)
          единственное, надо привести исходное выражение (систему) к эквивалентному виду
        - заюзать BenchmarkDotNet
    */

    public static void Main(string[] args)
    {
      var number = 2;
      var testValues = Vector<double>.Build.DenseOfArray(new double[] { 0.25, 0.75 });

      var equationSystem = new EquivalentEquationSystem(number);
      var simpleIteartionMethod = new SimpleIterationMethod(equationSystem);
      var parallelSimpleIterationMethod = new MultiThreadSimpleIterationMethod(equationSystem);

      try
      {
        var result = simpleIteartionMethod.SolveEquationSystem(testValues);
        DisplayUtils.DisplayResults(result);
      }
      catch (Exception exc)
      {
        simpleIteartionMethod.DisplayResults();
        DisplayUtils.DisplayException(exc);
      }

      try
      {
        var result = parallelSimpleIterationMethod.SolveEquationSystem(testValues);
        DisplayUtils.DisplayResults(result);
      }
      catch (Exception exc)
      {
        simpleIteartionMethod.DisplayResults();
        DisplayUtils.DisplayException(exc);
      }
    }

  }
}