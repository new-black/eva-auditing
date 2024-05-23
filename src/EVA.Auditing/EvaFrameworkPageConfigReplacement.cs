using System;
using System.Collections.Generic;
using EVA.Framework.API;

namespace EVA.Framework
{
    public abstract class PageConfigBase
    {
        protected int? MaximumLimit;
        protected bool IsInternal;

        // todo; drop the Apply function and revise some Obsolete attributes here

        [Obsolete("Discouraged; this is the raw input value - please use Apply.")]
        public int? Start { get; set; }

        [Obsolete("Discouraged; this is the raw input value - please use Apply.")]
        public int? Limit { get; set; }

        public string SortProperty { get; set; }

        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        [Obsolete("Discouraged; this constructor may generate warning response headers (start/limit).")]
        protected PageConfigBase()
        {
            IsInternal = false;
        }

        protected PageConfigBase(int? start, int? limit, string sortProperty = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            Start = start;
            Limit = limit;
            SortProperty = sortProperty;
            SortDirection = sortDirection;
            IsInternal = true;
        }

        public void SetMaximumLimit(int limit) => MaximumLimit = limit;

    }

    [Serializable]
    public class PageConfig : PageConfigBase
    {
        public static PageConfig Default => new();

        [Obsolete("Use PageConfig<T>")]
        public Dictionary<string, string> Filter { get; set; }

        public PageConfig()
        {
        }

        public PageConfig(int? start = 0, int? limit = int.MaxValue, string sortProperty = null, SortDirection sortDirection = SortDirection.Ascending)
          : base(start, limit, sortProperty, sortDirection)
        {
            IsInternal = true;
            Filter = new();
        }
    }

    public class PageConfig<T> : PageConfigBase, ICustomRequestValidation where T : new()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public static PageConfig<T> Default => new();

        public T Filter { get; set; }

        [Obsolete("Discouraged; this constructor may generate warning response headers (start/limit).")]
        public PageConfig()
        {
            IsInternal = false;
        }

        public PageConfig(T filter)
        {
            IsInternal = true;
            Filter = filter;
        }

        public PageConfig(int? start = 0, int? limit = int.MaxValue, string sortProperty = null, SortDirection sortDirection = SortDirection.Ascending)
          : base(start, limit, sortProperty, sortDirection)
        {
            IsInternal = true;
        }

        public void Validate(Action<(string id, string type, string description)> addWarning, Action<string> addError, bool shouldBreakOnDeprecations, int evaVersion, string path)
        {
            // TODO: No-op
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }

    public class SortFieldDescriptor
    {
        public string FieldName { get; set; }
        public SortDirection Direction { get; set; }
    }

    public class ScrollablePageConfig<T> where T : new()
    {
        public T Filter { get; set; }
        public string NextResultToken { get; set; }
        public int? Limit { get; set; }

        public static ScrollablePageConfig<T> Default => new()
        {
            Filter = new(),
            Limit = 20
        };
    }

    public class ScrollablePagedResult<T>
    {
        public List<T> Page { get; }
        public string PreviousResultToken { get; }
        public string NextResultToken { get; }

        public ScrollablePagedResult(List<T> page, string previousResultToken, string nextResultToken)
        {
            Page = page;
            NextResultToken = nextResultToken;
            PreviousResultToken = previousResultToken;
        }
    }

    public enum SortDirection
    {
        Ascending = 0,
        Descending = 1
    }
}