using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
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

    public static void DisplayResults(IList<float> values)
    {
      foreach (var value in values)
      {
        Console.Write($"{value} ");
      }

      Console.WriteLine("\n_________________________\n");
    }

    public static void Main(string[] args)
    {
      var number = 10;
      var testValues = Vector<float>.Build.Dense(number, 0);


      var system = new EquivalentEquationSystem(number);
      var res = system.Compute(testValues);
      DisplayResults(res);
    }

  }
}