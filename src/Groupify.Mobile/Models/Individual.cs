using SQLite;
#nullable disable
namespace Groupify.Mobile.Models
{
    public class Individual
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; }
    }
}