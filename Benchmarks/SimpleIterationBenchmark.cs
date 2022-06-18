using BenchmarkDotNet.Attributes;
using MathNet.Numerics.LinearAlgebra;
using Newton.NumericMethods.SimpleIteration;
using Newtow.Equations;

namespace Newton.Benchmarks
{
	[JsonExporterAttribute.Full]
	[MarkdownExporterAttribute.GitHub]
	[RPlotExporterAttribute]
	public class SimpleIterationBenchmark
	{
		public const int maxNumberOfIteration = 2000;

		[Benchmark(Baseline = true)]
		public void SimpleIteartionMethod()
		{
			// Arrange
			var equivalentEquationSystem = new EquivalentEquationSystem(2);
			var simpleIteartionMethod = new SimpleIterationMethod();
			var testValuesForSimpleIteration = Vector<double>.Build.DenseOfArray(new double[] { 0.25, 0.75 });
			simpleIteartionMethod.SolveEquationSystem(equivalentEquationSystem, testValuesForSimpleIteration, verbose: false);
		}

		[Benchmark]
		public void ParallelSimpleIteartionMethod2()
		{
			// Arrange
			var equivalentEquationSystem = new EquivalentEquationSystem(2);
			var simpleIteartionMethod = new ParallelSimpleIterationMethod();
			var testValuesForSimpleIteration = Vector<double>.Build.DenseOfArray(new double[] { 0.25, 0.75 });
			simpleIteartionMethod.SolveEquationSystem(equivalentEquationSystem, testValuesForSimpleIteration, degreeOfParallelism: 2, verbose: false);
		}

		[Benchmark]
		public void ParallelSimpleIteartionMethod4()
		{
			// Arrange
			var equivalentEquationSystem = new EquivalentEquationSystem(2);
			var simpleIteartionMethod = new ParallelSimpleIterationMethod();
			var testValuesForSimpleIteration = Vector<double>.Build.DenseOfArray(new double[] { 0.25, 0.75 });
			simpleIteartionMethod.SolveEquationSystem(equivalentEquationSystem, testValuesForSimpleIteration, degreeOfParallelism: 4, verbose: false);
		}

		[Benchmark]
		public void ParallelSimpleIteartionMethod6()
		{
			// Arrange
			var equivalentEquationSystem = new EquivalentEquationSystem(2);
			var simpleIteartionMethod = new ParallelSimpleIterationMethod();
			var testValuesForSimpleIteration = Vector<double>.Build.DenseOfArray(new double[] { 0.25, 0.75 });
			simpleIteartionMethod.SolveEquationSystem(equivalentEquationSystem, testValuesForSimpleIteration, degreeOfParallelism: 6, verbose: false);
		}

		[Benchmark]
		public void ParallelSimpleIteartionMethod8()
		{
			// Arrange
			var equivalentEquationSystem = new EquivalentEquationSystem(2);
			var simpleIteartionMethod = new ParallelSimpleIterationMethod();
			var testValuesForSimpleIteration = Vector<double>.Build.DenseOfArray(new double[] { 0.25, 0.75 });
			simpleIteartionMethod.SolveEquationSystem(equivalentEquationSystem, testValuesForSimpleIteration, degreeOfParallelism: 8, verbose: false);
		}
	}
}
