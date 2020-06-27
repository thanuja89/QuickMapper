namespace QuickMapper.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource">Type of Source</typeparam>
    /// <typeparam name="TTarget">Type of Target</typeparam>
    /// <param name="source">Source</param>
    /// <param name="target">Target</param>
    /// <returns>Target</returns>
    public delegate TTarget MappingDelegate<in TSource, TTarget>(TSource source, TTarget target);
}
