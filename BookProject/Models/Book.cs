using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookProject.Models
{
    public class Book
{
    public string ISBN { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Authors { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
}

}