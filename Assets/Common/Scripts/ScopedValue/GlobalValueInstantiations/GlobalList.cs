using System.Collections.Generic;

public class GlobalList<T> : GlobalValue<List<T>>
{
    protected override List<T> GetEquivalentInstance(List<T> t) => t != null ? new(t) : new();
}
