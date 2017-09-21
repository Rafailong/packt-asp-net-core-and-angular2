namespace packt_asp_net_core_and_angular2.Data.Users
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class ApplicationUser
  {
    public ApplicationUser()
    {
    }

    [Key]
    [Required]
    public string Id { get; set; }

    [Required]
    [MaxLength(128)]
    public string UserName { get; set; }

    [Required]
    public string Email { get; set; }

    public string DisplayName { get; set; }

    public string Notes { get; set; }

    [Required]
    public int Type { get; set; }

    [Required]
    public int Flags { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime LastModifiedDate { get; set; }

    public virtual List<Data.Items.Item> Items { get; set; }

    public virtual List<Data.Comments.Comment> Comments { get; set; }
  }
}