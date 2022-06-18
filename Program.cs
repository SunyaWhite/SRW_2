using MathNet.Numerics.LinearAlgebra;
using Newton.NumericMethods.NewtonMethod;
using Newton.NumericMethods.SimpleIteration;
using Newton.Utils;
using Newtow.Equations;

namespace Newton
{
	class Programm
	{
		public static void ComputeSimpleItartion()
		{
			var numberForSimpleIteration = 2;

			var equivalentEquationSystem = new EquivalentEquationSystem(numberForSimpleIteration);
			var simpleIteartionMethod = new SimpleIterationMethod(equivalentEquationSystem);
			var parallelSimpleIterationMethod = new MultiThreadSimpleIterationMethod(equivalentEquationSystem);

			try
			{
				var testValuesForSimpleIteration = Vector<double>.Build.DenseOfArray(new double[] { 0.25, 0.75 });
				var result = simpleIteartionMethod.SolveEquationSystem(testValuesForSimpleIteration, verbose: true);
				DisplayUtils.DisplayResults(result);
			}
			catch (Exception exc)
			{
				simpleIteartionMethod.DisplayResults();
				DisplayUtils.DisplayException(exc);
			}

			try
			{
				var testValuesForSimpleIteration = Vector<double>.Build.DenseOfArray(new double[] { 0.25, 0.75 });
				var result = parallelSimpleIterationMethod.SolveEquationSystem(testValuesForSimpleIteration, verbose: true);
				DisplayUtils.DisplayResults(result);
			}
			catch (Exception exc)
			{
				simpleIteartionMethod.DisplayResults();
				DisplayUtils.DisplayException(exc);
			}
		}

		public static void ComputeNewtonMethods()
		{
			var numberForNewton = 10;
			var equationSystem = new EquationSystem(numberForNewton);
			var newtonMethod = new NewtonMethod(equationSystem);
			var modifiedNewtonMethod = new ModifiedNewtonMethod(equationSystem);

			try
			{
				Console.WriteLine("Basic Newton numeric method");
				var testValues = Vector<double>.Build.Dense(numberForNewton, 0);
				var result = newtonMethod.SolveEquationSystem(testValues, verbose: true);
				DisplayUtils.DisplayResults(result);
			}
			catch (Exception exc)
			{
				newtonMethod.DisplayResults();
				DisplayUtils.DisplayException(exc);
			}

			try
			{
				Console.WriteLine("Modified Newton numeric method");
				var testValues = Vector<double>.Build.Dense(numberForNewton, 0.5);
				var result = modifiedNewtonMethod.SolveEquationSystem(testValues, 200, true);
				DisplayUtils.DisplayResults(result);
			}
			catch (Exception exc)
			{
				modifiedNewtonMethod.DisplayResults();
				DisplayUtils.DisplayException(exc);
			}
		}

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
			// ComputeSimpleItartion();
			ComputeNewtonMethods();
		}

	}
}