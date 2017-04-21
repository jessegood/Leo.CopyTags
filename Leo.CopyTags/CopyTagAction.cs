namespace Leo.CopyTags
{
    using Sdl.Desktop.IntegrationApi;
    using Sdl.Desktop.IntegrationApi.Extensions;
    using Sdl.FileTypeSupport.Framework.BilingualApi;
    using Sdl.TranslationStudioAutomation.IntegrationApi;
    using Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    [Action("CopyTagAction", Icon = "Brackets", Name = "Copy Tags to Target")]
    [ActionLayout(typeof(TranslationStudioDefaultContextMenus.EditorDocumentContextMenuLocation), 1, DisplayType.Large)]
    [Shortcut(Keys.Alt | Keys.T)]
    public class CopyTagAction : AbstractViewControllerAction<EditorController>
    {
        protected override void Execute()
        {
            var currentSegment = Controller?.ActiveDocument?.GetActiveSegmentPair();

            if (currentSegment != null && currentSegment.Target.Count == 0)
            {
                var source = currentSegment.Source;
                var tags = GetTags(source);

                foreach (var tag in tags)
                {
                    currentSegment.Target.Add((IAbstractMarkupData)tag.Clone());
                }
            }

            Controller.ActiveDocument.UpdateSegmentPair(currentSegment);
        }

        private IList<IAbstractMarkupData> GetTags(ISegment source)
        {
            var tagVisitor = new TagVisitor();
            tagVisitor.VisitSegment((ISegment)source.Clone());

            return tagVisitor.Tags;
        }
    }
}