using System;
using API.Entities;

namespace API.Models;

public class Group:BaseEntity
{
    //public User User { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string PictureUrl { get; set; }
    public required Location Location{get; set;}
  //  public required DateTime Start { get; set; }
   // public required DateTime Finish { get; set; }
    public required int NumberOfParticipants { get; set; }
    public required bool IsFree { get; set; }



}
