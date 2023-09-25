namespace Squiggles.Core.BT;
using Godot;
using SquigglesBT;
using SquigglesBT.Nodes;

/// <summary>
/// Perform a mathematical operation on a given Vector3
///     target = Vec3($target) {op} value
/// Params:
/// - `op` : string -- the operator to use [+, -, *, /]
/// - `target` : string -- the name of the target vector to perform the operations on in the blackboard (name not value)
/// - `value` : Vec3 -- the value to be applied as the secondary value in the operation.
/// </summary>
public class Vec3Math : Leaf {

  private readonly float[] _defaultArray = new float[3];
  protected override void RegisterParams() {
    Params["op"] = "+";
    Params["target"] = "key";
    Params["value"] = _defaultArray;
  }

  public override int Tick(Node actor, Blackboard blackboard) {
    var target = GetParam("target", "key", blackboard).AsString();
    var op = GetParam("op", "+", blackboard).AsString();
    var value = blackboard.GetLocal(target).AsVector3();
    var nums = GetParam("value", _defaultArray, blackboard).AsFloat32Array();
    switch (op) {
      case "+":
        blackboard.SetLocal(target, new Vector3 {
          X = value.X + nums[0],
          Y = value.Y + nums[1],
          Z = value.Z + nums[2],
        });
        break;
      case "*":
        blackboard.SetLocal(target, new Vector3 {
          X = value.X * nums[0],
          Y = value.Y * nums[1],
          Z = value.Z * nums[2],
        });
        break;
      case "-":
        blackboard.SetLocal(target, new Vector3 {
          X = value.X - nums[0],
          Y = value.Y - nums[1],
          Z = value.Z - nums[2],
        });
        break;
      case "/":
        blackboard.SetLocal(target, new Vector3 {
          X = value.X / nums[0],
          Y = value.Y / nums[1],
          Z = value.Z / nums[2],
        });
        break;
      default:
        break;
    }

    return SUCCESS;
  }

  public override void LoadDebuggingValues(Blackboard bb)
    => bb.SetLocal($"debug.{Label}:last_dir", bb.GetLocal(GetParam("target", "key", bb).AsString()));
}
