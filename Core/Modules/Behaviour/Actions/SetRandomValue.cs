namespace Squiggles.Core.BT;

using System;
using Godot;
using Squiggles.Core.Extension;
using SquigglesBT;
using SquigglesBT.Nodes;

/// <summary>
/// A behaviour tree node that sets a random value into the blackboard
/// Params:
/// - `val_type` : string -- a string type of ['float', 'int', 'bool', 'Vector2', 'Vector3'] which determines the type to generate.
/// - `target` : string -- the name of the variable to load into the blackboard
/// - `min` : float -- the floating point value to treat as the min. When generating integers a float is first generated and the floored. Booleans do not use min/max
/// - `max` : float -- the floating point value to treat as the max. See above for use.
/// - `` : _ -- _
/// - `` : _ -- _
/// </summary>
public class SetRandomValue : Leaf {
  private readonly Random _random = new();
  protected override void RegisterParams() {
    Params["val_type"] = "float";
    Params["target"] = "key";
    Params["min"] = 0.0f;
    Params["max"] = 1.0f;
  }

  public override int Tick(Node actor, Blackboard blackboard) {
    var target = GetParam("target", "key", blackboard).AsString();
    var type = GetParam("val_type", "float", blackboard).AsString();
    var min = GetParam("min", 0.0f, blackboard).AsSingle();
    var max = GetParam("min", 1.0f, blackboard).AsSingle();
    var size = max - min;

    float r() => (_random.NextSingle() * size) + min;
    switch (type) {
      case "float":
        blackboard.SetLocal(target, r());
        break;
      case "int":
        blackboard.SetLocal(target, (int)r());
        break;
      case "bool":
        blackboard.SetLocal(target, _random.NextBool());
        break;
      case "Vector2":
        blackboard.SetLocal(target, new Vector2(r(), r()));
        break;
      case "Vector3":
        blackboard.SetLocal(target, new Vector3(r(), r(), r()));
        break;
      default:
        break;
    }
    return SUCCESS;
  }

  public override void LoadDebuggingValues(Blackboard bb) => bb.SetLocal($"debug.{Label}:last_dir", bb.GetLocal(GetParam("target", "key", bb).AsString()));
}
