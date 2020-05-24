using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Groupify.Mobile.Models
{
    public class IndividualGroupings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IndividualId { get; set; }
        public int OtherIndividualId { get; set; }
    }
}
