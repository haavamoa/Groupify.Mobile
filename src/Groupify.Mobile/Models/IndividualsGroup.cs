using System.Collections.Generic;
using SQLite;
#nullable disable
namespace Groupify.Mobile.Models
{
    public class IndividualsGroup
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
