using CodingSeb.ExpressionEvaluator;

namespace Emotions;

class Upgrade
{
  public static ExpressionEvaluator eval = new ExpressionEvaluator();
  public string Description;
  public int Cost;
  public Dictionary<string, string> Boost;

  public Dictionary<string, double> EvalBoost(int level) 
  {
    eval.Variables = new Dictionary<string, object>() {
      { "level", level }
    };

    return this.Boost.ToDictionary(x => x.Key, x => (double) eval.Evaluate(x.Value));
  }
}