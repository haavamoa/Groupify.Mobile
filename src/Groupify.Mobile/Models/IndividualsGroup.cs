using System.Collections.Generic;
using SQLite;
#nullable disable
namespace Groupify.Mobile.Models
{
    public class IndividualsGroup
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IndividualId { get; set; }
        public int GroupId { get; set; }
    }
}
