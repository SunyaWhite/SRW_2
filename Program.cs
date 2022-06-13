using MathNet.Numerics.LinearAlgebra;
using Newton.NumericMethods.SimpleIteration;
using Newton.Utils;
using Newtow.Equations;

namespace Newton
{
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