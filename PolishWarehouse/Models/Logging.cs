using System;
using PolishWarehouseData;

namespace PolishWarehouse.Models
{
    public class Logging
    {
        public static string LogEvent(LogTypes type, string details, string friendlyMessage, Exception ex = null, long? parentID = null)
        {
            using (var db = new PolishWarehouseEntities())
            {
                var log = new Log() {
                    LogType = type.ToString(),
                    Details = details,
                    FriendlyMessage = friendlyMessage,
                };

                if(ex != null)
                {
                    db.Logs.Add(log);
                    db.SaveChanges();
                    log.Error = ex.Message;
                    log.StackTrace = ex.StackTrace;

                    if(ex.InnerException != null)
                    {
                        LogEvent(type, $"Inner Exception for details: {details}", friendlyMessage,ex.InnerException,log.ID);
                    }
                }
                else
                {
                    db.Logs.Add(log);
                }

                db.SaveChanges();
                return friendlyMessage;
            }
        }
    }

    public enum LogTypes
    {
        Error,
        Info
    }
}