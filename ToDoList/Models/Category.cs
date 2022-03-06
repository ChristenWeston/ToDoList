using System.Collections.Generic;

namespace ToDoList.Models
{
    public class Category
    {
        public Category()
        {
          //HashSet is an unordered collection of unique elements. HashSet cannot have duplicates
            this.Items = new HashSet<Item>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        //Declare Items as an instance of ICollection - generic interface built into the .NET framework
        //ICollection has methods for querying and changing data
        //Declaring as virtual to allow Entity to use lazy loading
        public virtual ICollection<Item> Items { get; set; }
    }
}