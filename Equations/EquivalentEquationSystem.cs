namespace Newtow.Equations
{
  /*
    Для тестовых целей я захардкодил сами функции
  */

  public class EquivalentEquationSystem : EquationSystem
  {

    public EquivalentEquationSystem(int number = 10)
    {
      this._size = number;
      this._equations = new List<Func<IList<float>, float>>(this._size);

      for (var index = 0; index < this._size; index++)
      {
        this._equations.Add(this.CreateEquation(index));
      }

      this._equations[0] = (IList<float> values) => (2 * values[0] * values[0] - 2 * values[1] - 3) / -3;
      this._equations[this._size - 1] = (IList<float> values) => (2 * values[this._size - 1] * values[this._size - 1] - 2 * values[this._size - 2] - 4) / -3;
    }

    protected override Func<IList<float>, float> CreateEquation(int index) =>
    (IList<float> values) => (2 * values[index] * values[index] - values[index - 1] - 2 * values[index + 1] - 2) / -3;
  }

}