public interface ISelectionState
{
    void EnterState(CharacterSelectionManager characterSelectionManager);
    void ExitState(CharacterSelectionManager characterSelectionManager);
    void UpdateState(CharacterSelectionManager characterSelectionManager);
}