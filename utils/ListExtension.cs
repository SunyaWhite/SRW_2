namespace Newton.Utils
{
	public static class ListExtension
	{
		public static double GetNorma(this IList<double> values)
		{
			return Math.Sqrt(values.Aggregate(0d, (currentSum, currentElem) => currentSum + (currentElem * currentElem)));
		}

		public static double GetNormaForNewton(this IList<double> values)
		{
			return Math.Sqrt(values.Aggregate(0d, (currentSum, currentElem) => currentSum + (currentElem * currentElem)) / values.Count);
		}
	}
}
