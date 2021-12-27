using CodingSeb.ExpressionEvaluator;

namespace Emotions;

class Upgrade {
  public static ExpressionEvaluator eval = new ExpressionEvaluator();
  public string Description 
  { get; set; }

  public Dictionary<string, string> Boost
  { get; set; }

  public Upgrade(string description, Dictionary<string, string> boost) {
    this.Description = description;
    this.Boost = boost;
  }

  public Dictionary<string, double> EvalBoost(int level) {
    eval.Variables = new Dictionary<string, object>() {
      { "level", level }
    };

    return this.Boost.ToDictionary(x => x.Key, x => (double) eval.Evaluate(x.Value));
  }
}