using System;
using System.Collections.Generic;
using ToDoList.Models;

namespace ListMaker
{
  public class Program
  {
    
    public static void Main()
    {
      Console.WriteLine("Welcome to the To Do List!");
      Console.WriteLine("Would you like to add an item to your list or view your list? (Add/View)");
      string viewOrAddToList = Console.ReadLine();

//Depending on if user says "Add" or "View", add new item to list or view the entire list
//Is this a method?
      //public static void UserChoice()
      //{
        if (viewOrAddToList == "Add")
        {
          Console.WriteLine("What would you like to add to your to do list? Enter description: ");
          string newItemDescription = Console.ReadLine();
          Item newItem1 = new Item(newItemDescription);
          Console.WriteLine(newItemDescription + " was added to the list. Would you like to add another item to your list? (Add/View)");
         // return UserChoice;
        }
        else if (viewOrAddToList == "View")
        {
          List<Item> result = Item.GetAll();
          Console.WriteLine(result);
        }
      //}
    }
  }
}