using System.ComponentModel.DataAnnotations;

namespace Chamedoon.Domin.Base;

public class BaseEntity
{
    [Key]
    public long Id { get; set; }
    public bool IsDeleted { get; set; }

}
