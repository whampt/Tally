using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace Tally.Models
{
    [Table(nameof(Item))]
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }
        [NotNull,MaxLength(250)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Cost { get; set; }

        public bool IsValid()
        {
            return (!String.IsNullOrWhiteSpace(Name));
        }
        public Item(string name, string cost)
        {
            Name = name;
            Cost = cost;
        }
        public Item() { }
    }
}
