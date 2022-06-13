namespace Newton.Utils
{
	public static class DisplayUtils
	{
		public static void DisplayResults(IList<double> values)
		{
			Console.WriteLine("Completed: Result");
			foreach (var value in values)
			{
				Console.Write($"{value} ");
			}

			Console.WriteLine("\n_________________________\n");
		}

		public static void DisplayException(Exception exc)
		{
			Console.WriteLine("Completed with an error");
			Console.WriteLine(exc.Message);
			Console.WriteLine("\n_________________________\n");
		}
	}
}