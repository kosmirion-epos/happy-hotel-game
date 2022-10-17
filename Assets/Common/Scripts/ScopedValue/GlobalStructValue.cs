/// <summary>
/// Implements <see cref="GlobalValue{T}.GetEquivalentInstance(T)"/> to return a copy
/// of its supplied argument.
/// </summary>
public abstract class GlobalStructValue<T> : GlobalValue<T> where T : struct
{
    /// <summary> Returns a copy of the supplied struct </summary>
    protected override T GetEquivalentInstance(T t) => t;
}
