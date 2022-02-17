using System.Collections.Generic;

namespace ToDoList.Models
{
  public class Item
  {

    public string Description { get; set; }
    public int Id { get; }
    private static List<Item> _instances = new List<Item> {};

    public Item(string description)
    {
      Description = description;
      //Every time an item is created, add it to the List
      _instances.Add(this);
      Id = _instances.Count;
    }

//Variables and methods dealing with entire classes must be static
    public static List<Item> GetAll()
    {
      return _instances;
    }

    public static void ClearAll()
    {
      _instances.Clear();
    }

    public static Item Find(int searchId)
    {
      return _instances[searchId-1];
    }
  }
}