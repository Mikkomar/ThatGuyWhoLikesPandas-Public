using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.DAL.Attributes
{
    public class HistoryLogToTableAttribute : Attribute
    {
        public string HistoryLogTable { get; set; }
        public HistoryLogToTableAttribute(string historyLogTable)
        {
            HistoryLogTable = historyLogTable;
        }
    }
}
