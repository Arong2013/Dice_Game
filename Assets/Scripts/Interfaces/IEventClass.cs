
public interface IEventCondition
{
    bool IsMet(ICharacter character);
}
public interface IReward
{
    void ApplyTo(ICharacter character);
}
public interface IDialogueTrigger
{
    void StartDialogue(string dialogueId);
}