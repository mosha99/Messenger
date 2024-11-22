namespace Share.Model;

public record SenderUser(Guid Guid, string Name, string Device)
{
    public virtual bool Equals(SenderUser? other)
    {
        return other?.Guid.Equals(Guid) == true;
    }
}