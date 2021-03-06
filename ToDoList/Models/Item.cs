using System.Collections.Generic;

namespace ToDoList.Models
{
    public class Item
    {
        public Item()
        {
            this.JoinEntities = new HashSet<CategoryItem>();
        }

        public int ItemId { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public virtual ApplicationUser User { get; set; } //Declared virtual to allow lazy load and improve apps efficiency
        public virtual ICollection<CategoryItem> JoinEntities { get;}
    }
}