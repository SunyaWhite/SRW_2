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
			var simpleIteartionMethod = new SimpleIterationMethod();
			var parallelSimpleIterationMethod = new MultiThreadSimpleIterationMethod();

			try
			{
				var testValuesForSimpleIteration = Vector<double>.Build.DenseOfArray(new double[] { 0.25, 0.75 });
				var result = simpleIteartionMethod.SolveEquationSystem(equivalentEquationSystem, testValuesForSimpleIteration, verbose: true);
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
				var result = parallelSimpleIterationMethod.SolveEquationSystem(equivalentEquationSystem, testValuesForSimpleIteration, verbose: true);
				DisplayUtils.DisplayResults(result);
			}
			catch (Exception exc)
			{
				simpleIteartionMethod.DisplayResults();
				DisplayUtils.DisplayException(exc);
			}
		}

		public static void ComputeNewtonMethods(int numberForNewton = 10)
		{
			var equationSystem = new EquationSystem(numberForNewton);
			var newtonMethod = new NewtonMethod();
			var modifiedNewtonMethod = new ModifiedNewtonMethod();

			try
			{
				Console.WriteLine("Basic Newton numeric method");
				var testValues = Vector<double>.Build.Dense(numberForNewton, 0);
				var result = newtonMethod.SolveEquationSystem(equationSystem, testValues, verbose: true);
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
				var result = modifiedNewtonMethod.SolveEquationSystem(equationSystem, testValues, 200, true);
				DisplayUtils.DisplayResults(result);
			}
			catch (Exception exc)
			{
				modifiedNewtonMethod.DisplayResults();
				DisplayUtils.DisplayException(exc);
			}
		}

		public static void ComputeParallelNewtonMethods(int numberForNewton = 10, int degreeOfParallelism = 2)
		{
			var equationSystem = new EquationSystem(numberForNewton);
			var paralleNewtonMethod = new ParallelNewtonMethod(degreeOfParallelism: degreeOfParallelism);
			var parallelModifiedNewtonMethod = new ParallelModifiedNewtonMethod(degreeOfParallelism: degreeOfParallelism);

			try
			{
				Console.WriteLine("Basic Parallel Newton numeric method");
				var testValues = Vector<double>.Build.Dense(numberForNewton, 0);
				var result = paralleNewtonMethod.SolveEquationSystem(equationSystem, testValues, verbose: true);
				DisplayUtils.DisplayResults(result);
			}
			catch (Exception exc)
			{
				paralleNewtonMethod.DisplayResults();
				DisplayUtils.DisplayException(exc);
			}

			try
			{
				Console.WriteLine("Modified Parallel Newton numeric method");
				var testValues = Vector<double>.Build.Dense(numberForNewton, 0.5);
				var result = parallelModifiedNewtonMethod.SolveEquationSystem(equationSystem, testValues, 200, true);
				DisplayUtils.DisplayResults(result);
			}
			catch (Exception exc)
			{
				parallelModifiedNewtonMethod.DisplayResults();
				DisplayUtils.DisplayException(exc);
			}
		}

		public static void ComputeBasicNewtonMethods(int numberForNewton = 10, int degreeOfParallelism = 2)
		{
			var equationSystem = new EquationSystem(numberForNewton);
			var newtonMethod = new NewtonMethod();
			var parallelNewtonMethod = new ParallelNewtonMethod(degreeOfParallelism: degreeOfParallelism);

			try
			{
				Console.WriteLine("Basic Newton numeric method");
				var testValues = Vector<double>.Build.Dense(numberForNewton, 0);
				var result = newtonMethod.SolveEquationSystem(equationSystem, testValues, verbose: true);
				DisplayUtils.DisplayResults(result);
			}
			catch (Exception exc)
			{
				newtonMethod.DisplayResults();
				DisplayUtils.DisplayException(exc);
			}

			try
			{
				Console.WriteLine("Basic Parallel Newton numeric method");
				var testValues = Vector<double>.Build.Dense(numberForNewton, 0);
				var result = parallelNewtonMethod.SolveEquationSystem(equationSystem, testValues, verbose: true);
				DisplayUtils.DisplayResults(result);
			}
			catch (Exception exc)
			{
				parallelNewtonMethod.DisplayResults();
				DisplayUtils.DisplayException(exc);
			}
		}

		public static void ComputeModifiedNewtonMethods(int numberForNewton = 10, int degreeOfParallelism = 2)
		{
			var equationSystem = new EquationSystem(numberForNewton);
			var modifiedNewtonMethod = new ModifiedNewtonMethod();
			var parallelModifiedNewtonMethod = new ParallelModifiedNewtonMethod(degreeOfParallelism: degreeOfParallelism);

			try
			{
				Console.WriteLine("Modified Newton numeric method");
				var testValues = Vector<double>.Build.Dense(numberForNewton, 0.5);
				var result = modifiedNewtonMethod.SolveEquationSystem(equationSystem, testValues, 200);
				DisplayUtils.DisplayResults(result);
			}
			catch (Exception exc)
			{
				modifiedNewtonMethod.DisplayResults();
				DisplayUtils.DisplayException(exc);
			}

			try
			{
				Console.WriteLine("Modified Parallel Newton numeric method");
				var testValues = Vector<double>.Build.Dense(numberForNewton, 0.5);
				var result = parallelModifiedNewtonMethod.SolveEquationSystem(equationSystem, testValues, 200);
				DisplayUtils.DisplayResults(result);
			}
			catch (Exception exc)
			{
				parallelModifiedNewtonMethod.DisplayResults();
				DisplayUtils.DisplayException(exc);
			}
		}

		public static void Main(string[] args)
		{
			// ComputeSimpleItartion();
			// ComputeNewtonMethods();
			// ComputeParallelNewtonMethods();
			// ComputeModifiedNewtonMethods();
			ComputeBasicNewtonMethods();
		}

	}
}