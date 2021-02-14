namespace Sentry.Internals
{
    /// <summary>
    /// A minimal subset of information to describe the selected page.
    /// </summary>
    internal class PageInfo
    {
        /// <summary>
        /// The name of the page.
        /// </summary>
        internal string Name { get; set; }
        /// <summary>
        /// The name of the base type, if found.
        /// </summary>
        internal string BaseType { get; set; }
    }
}
