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

		public static void ComputeNewtonMethods(int numberForNewton = 10)
		{
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

		public static void ComputeParallelNewtonMethods(int numberForNewton = 10, int degreeOfParallelism = 2)
		{
			var equationSystem = new EquationSystem(numberForNewton);
			var paralleNewtonMethod = new ParallelNewtonMethod(equationSystem, degreeOfParallelism: degreeOfParallelism);
			var parallelModifiedNewtonMethod = new ParallelModifiedNewtonMethod(equationSystem, degreeOfParallelism: degreeOfParallelism);

			try
			{
				Console.WriteLine("Basic Parallel Newton numeric method");
				var testValues = Vector<double>.Build.Dense(numberForNewton, 0);
				var result = paralleNewtonMethod.SolveEquationSystem(testValues, verbose: true);
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
				var result = parallelModifiedNewtonMethod.SolveEquationSystem(testValues, 200, true);
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
			var newtonMethod = new NewtonMethod(equationSystem);
			var parallelNewtonMethod = new ParallelNewtonMethod(equationSystem, degreeOfParallelism: degreeOfParallelism);

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
				Console.WriteLine("Basic Parallel Newton numeric method");
				var testValues = Vector<double>.Build.Dense(numberForNewton, 0);
				var result = parallelNewtonMethod.SolveEquationSystem(testValues, verbose: true);
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
			var modifiedNewtonMethod = new ModifiedNewtonMethod(equationSystem);
			var parallelModifiedNewtonMethod = new ParallelModifiedNewtonMethod(equationSystem, degreeOfParallelism: degreeOfParallelism);

			try
			{
				Console.WriteLine("Modified Newton numeric method");
				var testValues = Vector<double>.Build.Dense(numberForNewton, 0.5);
				var result = modifiedNewtonMethod.SolveEquationSystem(testValues, 200);
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
				var result = parallelModifiedNewtonMethod.SolveEquationSystem(testValues, 200);
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