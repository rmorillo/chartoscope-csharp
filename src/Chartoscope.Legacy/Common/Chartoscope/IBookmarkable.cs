using System;

namespace Chartoscope.Common
{
    public interface IBookmarkable<T>
    {
        Bookmark<T> GetBookmark();
    }
}
