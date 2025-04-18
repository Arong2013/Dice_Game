
using System;

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
public interface ISignalBus
{
    void Subscribe<T>(Action<T> handler);
    void Unsubscribe<T>(Action<T> handler);
    void Fire<T>(T signal);
}