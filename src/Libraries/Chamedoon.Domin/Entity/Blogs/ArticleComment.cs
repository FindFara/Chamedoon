﻿using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.Users;

namespace Chamedoon.Domin.Entity.Blogs;
public class ArticleComment : BaseAuditableEntity
{
    public int ArticleId { get; set; }
    public int UserId { get; set; }
    public bool ReaedAdmin { get; set; }
    public required string Message { get; set; }

    #region Relations

    public required Article Article { get; set; }
    public required User User { get; set; }

    #endregion
}
