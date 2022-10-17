/// <summary>
/// Implements <see cref="GlobalValue{T}.GetEquivalentInstance(T)"/> to return its argument instead of providing a copy.
/// This may produce unwanted side effects, since both the returned and the original reference
/// share their referenced instance.
/// If the two references need to be decoupled, inherit from <see cref="GlobalValue{T}"/> instead
/// and provide a custom implementation of <see cref="GetEquivalentInstance(T)"/>.
/// </summary>
public abstract class GlobalClassValue<T> : GlobalValue<T> where T : class
{
    /// <summary>
    /// Simply returns the supplied reference instead of copying it.
    /// This may produce unwanted side effects, since both the returned and the original reference
    /// share their referenced instance.
    /// If the two references need to be decoupled, inherit from <see cref="GlobalValue{T}"/> instead
    /// and provide a custom implementation.
    /// </summary>
    protected override T GetEquivalentInstance(T t) => t;
}
