using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UUIDNext;

namespace Lines.Domain.Models.Common;
public class BaseModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Uuid.NewDatabaseFriendly(Database.SqlServer);
    public DateTime CreatedDate { get; set; } 
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;

    public void delete()
    {
        IsDeleted = true;
    }
}