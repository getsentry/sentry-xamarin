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
            //Filter duplicated internal breadcrumbs
            if (options.LastInternalBreadcrumb != null &&
                options.LastInternalBreadcrumb.Message == message &&
                options.LastInternalBreadcrumb.Category == category &&
                options.LastInternalBreadcrumb.Type == type &&
                !options.LastInternalBreadcrumb.Data.Except(data).Any() &&
                DateTimeOffset.UtcNow.Subtract(options.LastInternalBreadcrumb.Timestamp).TotalSeconds < 2)
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