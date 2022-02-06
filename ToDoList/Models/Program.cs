using System;
using System.Collections.Generic;
using ToDoList.Models;

namespace ListMaker
{
  public class Program
  {
    
     public static void Main()
     {
       Console.WriteLine("Welcome to the To Do List Main Menu!");
//Depending on if user says "Add" or "View", add new item to list or view the entire list
      {
        Console.WriteLine("Would you like to add an item to your list or view your list? (Add/View)");
        string viewOrAddToList = Console.ReadLine();
        if (viewOrAddToList == "Add")
        {
          Console.WriteLine("What would you like to add to your to do list? Enter description: ");
          string newItemDescription = Console.ReadLine();
          Item newItem1 = new Item(newItemDescription);
          Console.WriteLine(newItemDescription + " was added to the list!");
          Console.WriteLine("Do you want to leave the to do list? ('Y' for yes)");
          string leaveOrStay = Console.ReadLine();
          if (leaveOrStay == "Y" || leaveOrStay == "y")
          {
            Console.WriteLine("Ok bye bye!");
          }
          else
          {
            Main();
          }
        }
        else if (viewOrAddToList == "View")
        {
          Item.GetAll();
          List<Item> toDoListItems = Item.GetAll();

          if (toDoListItems.Count == 0)
          {
            Console.WriteLine("Your to do list is empty!");
          }

          else
          {
            for (int i = 0; i < toDoListItems.Count; i++)
            {
              Console.WriteLine(toDoListItems[i].Description + ", ");
            }
          }

          Console.WriteLine("Do you want to leave the do do list? ('Y' for yes or enter for No)");
          string leaveOrStay = Console.ReadLine();
          if (leaveOrStay == "Y" || leaveOrStay == "y")
          {
            Console.WriteLine("Ok bye bye!");
          }
          else
          {
            Main();
          }
        }
      }
    }
  }
}