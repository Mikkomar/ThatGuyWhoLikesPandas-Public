using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.DAL.Entities.HistoryLogs
{
    public class HistoryLog
    {
        public static HistoryLog CreateHistoryLog(Type entityType)
        {
            if (entityType.Namespace == "Castle.Proxies")
            {
                entityType = entityType.BaseType;
            }
            switch (entityType)
            {
                //case Type type when type == typeof(Course):
                //    return new HistoryLogCourse();
                default:
                    return null;
            }
        }
        [Column("Id")]
        public Guid Id { get; set; }
        [Column("EntityId")]
        public Guid EntityId { get; set; }
        [Column("ModifierId")]
        public string ModifierId { get; set; }
        [Column("ModifiedDate")]
        public DateTime ModifiedDate { get; set; }
        [Column("ModifiedGroup")]
        public Guid ModifiedGroup { get; set; }
        [Column("Property")]
        public string Property { get; set; }
        [Column("OldValue")]
        public string OldValue { get; set; }
        [Column("NewValue")]
        public string NewValue { get; set; }
    }
    //[Table("History_AttendanceForm")]
    //public class HistoryLogAttendanceForm : HistoryLog{}
}
