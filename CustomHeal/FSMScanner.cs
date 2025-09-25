using HutongGames.PlayMaker;
using UnityEngine;

public class FSMScanner : MonoBehaviour
{
    void Start()
    {
        foreach (var fsm in FindObjectsOfType<PlayMakerFSM>())
        {
            foreach (var state in fsm.FsmStates)
            {
                foreach (var action in state.Actions)
                {
                    if (action is GetConstantsValue gcv)
                    {
                        Debug.Log($"FSM '{fsm.FsmName}' on {fsm.gameObject.name} " +
                                  $"uses GetConstantsValue in state '{state.Name}' " +
                                  $"with variable '{gcv.variableName}'");
                    }
                }
            }
        }
    }
     public static void Scan()
    {
        foreach (var fsm in Object.FindObjectsOfType<PlayMakerFSM>())
        {
            foreach (var state in fsm.FsmStates)
            {
                foreach (var action in state.Actions)
                {
                    if (action is GetConstantsValue gcv)
                    {
                        Debug.Log($"FSM '{fsm.FsmName}' on {fsm.gameObject.name} " +
                                  $"uses GetConstantsValue in state '{state.Name}' " +
                                  $"with variable '{gcv.variableName}'");
                    }
                }
            }
        }
    }
}
