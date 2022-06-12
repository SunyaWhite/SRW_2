using MathNet.Numerics.LinearAlgebra;
using Newton.Equations;

namespace Newtow.Equations
{

  /*
    Для тестовых целей я захардкодил сами функции
  */

  public class EquationSystem
  {

    protected List<Func<IList<float>, float>> _equations { get; init; }

    protected int _size { get; init; }

    public EquationSystem(int number = 10)
    {
      this._size = number;
      this._equations = new List<Func<IList<float>, float>>(this._size);

      for (var index = 0; index < this._size; index++)
      {
        this._equations.Add(this.CreateEquation(index));
      }

      this._equations[0] = (IList<float> values) => (3 + 2 * values[0]) * values[0] - 2 * values[1] - 3;
      this._equations[this._size - 1] = (IList<float> values) => (3 + 2 * values[this._size - 1]) * values[this._size - 1] - 2 * values[this._size - 2] - 4;
    }

    protected virtual Func<IList<float>, float> CreateEquation(int index) =>
    (IList<float> values) => (3 + 2 * values[index]) * values[index] - values[index - 1] - 2 * values[index + 1] - 2;

    public IList<float> Compute(IList<float> values)
    {
      if (this._size != values.Count)
      {
        throw new EquationSystemException("Invalid size of argument array provided");
      }

      return Vector<float>.Build.DenseOfArray(this._equations.Select((eq) => eq(values)).ToArray());
    }
  }
}