public interface IItem
{
    string ID { get; }
    string Name { get; }
}
public interface IUsableItem : IItem
{
    void Use(ICharacter target);
}
public interface IEquippable : IItem
{
    string EquipSlot { get; }
    void Equip(ICharacter target);
    void Unequip(ICharacter target);
}
public interface ITradable : IItem
{
    int BuyPrice { get; }
    int SellPrice { get; }
}
