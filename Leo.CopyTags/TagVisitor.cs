namespace Leo.CopyTags
{
    using Sdl.FileTypeSupport.Framework.BilingualApi;
    using System.Collections.Generic;

    internal class TagVisitor : IMarkupDataVisitor
    {
        public IList<IAbstractMarkupData> Tags { get; private set; } = new List<IAbstractMarkupData>();

        public void VisitCommentMarker(ICommentMarker commentMarker)
        {
        }

        public void VisitLocationMarker(ILocationMarker location)
        {
        }

        public void VisitLockedContent(ILockedContent lockedContent)
        {
        }

        public void VisitOtherMarker(IOtherMarker marker)
        {
        }

        public void VisitPlaceholderTag(IPlaceholderTag tag)
        {
            Tags.Add(tag);
        }

        public void VisitRevisionMarker(IRevisionMarker revisionMarker)
        {
        }

        public void VisitSegment(ISegment segment)
        {
            VisitChildren(segment);

            foreach (var tag in Tags)
            {
                if (tag is ITagPair)
                {
                    ClearContentInTagPairs((ITagPair)tag);
                }
            }
        }

        public void VisitTagPair(ITagPair tagPair)
        {
            Tags.Add(tagPair);
        }

        public void VisitText(IText text)
        {
        }

        private static bool IsRemovableItem(IAbstractMarkupData item)
        {
            return typeof(IText).IsAssignableFrom(item.GetType()) ||
                   typeof(ICommentMarker).IsAssignableFrom(item.GetType()) ||
                   typeof(ILocationMarker).IsAssignableFrom(item.GetType()) ||
                   typeof(ILockedContent).IsAssignableFrom(item.GetType()) ||
                   typeof(IOtherMarker).IsAssignableFrom(item.GetType()) ||
                   typeof(IRevisionMarker).IsAssignableFrom(item.GetType());
        }

        private void ClearContentInTagPairs(ITagPair tagPair)
        {
            for (int i = tagPair.Count - 1; i >= 0; --i)
            {
                var item = tagPair[i];

                if (IsRemovableItem(item))
                {
                    item.RemoveFromParent();
                }
                else if (item is ITagPair)
                {
                    ClearContentInTagPairs((ITagPair)item);
                }
            }
        }

        private void VisitChildren(IAbstractMarkupDataContainer container)
        {
            foreach (var item in container)
            {
                item.AcceptVisitor(this);
            }
        }
    }
}