using FlowCanvas.Macros;

public class GroupMacro : Macro
{

    //Macros use local blackboard instead of propagated one
    public override bool useLocalBlackboard
    {
        get { return false; }
    }
}