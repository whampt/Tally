using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace Tally.Models
{
    [Table(nameof(Item))]
    public class Item : IEquatable<Item>
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

        public override bool Equals(object other)
        {
            if (other == null) return false;
            Item otherAsItem = other as Item;
            if (otherAsItem == null) return false;
            else return Equals(otherAsItem);
        }

        public bool Equals(Item other)
        {
           if (other ==null)return false;
            return (this.Name.Equals(other.Name));
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public Item(string name, string cost)
        {
            Name = name;
            Cost = cost;
        }
        public Item() { }
    }
}
