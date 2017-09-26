namespace packt_asp_net_core_and_angular2.Data.Items
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class Item
  {
    public Item()
    {
    }

    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Notes { get; set; }

    public string Description { get; set; }

    public string Text { get; set; }

    [Required]
    public int Type { get; set; }

    [Required]
    public int Flags { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public int ViewCount { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime LastModifiedDate { get; set; }

    [ForeignKey("UserId")]
    public virtual Data.Users.ApplicationUser Author { get; set; }

    public virtual List<Data.Comments.Comment> Comments { get; set; }
  }
}