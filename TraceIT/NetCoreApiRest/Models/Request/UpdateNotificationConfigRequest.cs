using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Request
{
    public class UpdateNotificationConfigRequest
    {
        public int userId { get; set; }
        public int configNotification { get; set; }
    }
}
