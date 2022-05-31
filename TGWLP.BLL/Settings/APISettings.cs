using System;

namespace TGWLP.BLL.Settings
{
    public class APISettings
    {
        public GoogleAPISettings GoogleAPI { get; set; }
    }

    public class GoogleAPISettings
    {
        public BooksSettings Books { get; set; }
    }

    public class BooksSettings
    {
        public Uri BaseAddress { get; set; }
        public string Key { get; set; }
    }
}
