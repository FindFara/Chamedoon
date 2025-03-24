using System.ComponentModel.DataAnnotations;

namespace Chamedoon.Domin.Base;

public abstract  class BaseEntity
{
    [Key]
    public long Id { get; set; }
    public bool IsDeleted { get; set; }

}
