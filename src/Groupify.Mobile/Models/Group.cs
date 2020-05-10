using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
#nullable disable
namespace Groupify.Mobile.Models
{
    public class Group
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
