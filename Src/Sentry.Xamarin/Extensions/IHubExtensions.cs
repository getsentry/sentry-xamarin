using System;
using System.Collections.Generic;
using System.Linq;

namespace Sentry.Extensions
{
    internal static class IHubExtensions
    {
        internal const string DuplicatedBreadcrumbDropped = "Duplicated breadrumb dropped.";

        /// <summary>
        /// Adds an automatic breadcrumb to the current scope if the previous one wasn't the same.
        /// </summary>
        /// <param name="hub">The Hub which holds the scope stack</param>
        /// <param name="options">The SentryXamarinOptions that holds the last breadcrumb info.</param>
        /// <param name="message">The message.</param>
        /// <param name="category">Category.</param>
        /// <param name="type">Breadcrumb type.</param>
        /// <param name="data">Additional data.</param>
        /// <param name="level">Breadcrumb level.</param>
        internal static void AddInternalBreadcrumb(this IHub hub, SentryXamarinOptions options, string message, string? category = null, string? type = null, Dictionary<string, string>? data = null, BreadcrumbLevel level = BreadcrumbLevel.Info)
        {
            var previousBreadcrumb = options.LastInternalBreadcrumb;
            //Filter duplicated internal breadcrumbs
            if (previousBreadcrumb != null &&
                previousBreadcrumb.Message == message &&
                previousBreadcrumb.Category == category &&
                previousBreadcrumb.Type == type &&
                !previousBreadcrumb.Data.Except(data).Any() &&
                DateTimeOffset.UtcNow.Subtract(previousBreadcrumb.Timestamp).TotalSeconds < options.InternalBreadcrumbDuplicationTimeSpan)
            {
                //Skip
                options.DiagnosticLogger?.Log(SentryLevel.Debug, DuplicatedBreadcrumbDropped);
            }
            else
            {
                var breadcrumb = new Breadcrumb(message, type, data, category, level);
                hub.ConfigureScope(s => s.AddBreadcrumb(breadcrumb));
                options.LastInternalBreadcrumb = breadcrumb;
            }
        }
    }
}
