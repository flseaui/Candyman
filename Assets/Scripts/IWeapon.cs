public interface IWeapon
{
    void Use();

    bool Hand { get; set; }
    
    int SpriteIndex { get; }
}