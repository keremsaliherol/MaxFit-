using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class MemberNotification
{
    public int NotificationId { get; set; }

    public int MemberId { get; set; }

    public string NotificationType { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string Channel { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public int? SentByUserId { get; set; }

    public bool IsRead { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual User? SentByUser { get; set; }
}
