using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ToDoList.Controllers
{
  [Authorize] // Allows access to ItemsController only if a user is logged in. Can use AllowAnonymous attribute any specific methods we want unauthorized users to have access to
  public class ItemsController : Controller
  {
    private readonly ToDoListContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public ItemsController(UserManager<ApplicationUser> userManager, ToDoListContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public ActionResult Index()
    {
        return View(_db.Items.ToList());
    }

    public ActionResult Create()
    {
      //Assigns the available categories to the SelectList object
      //First argument is the data that will populate our selectLists option elements
      //Second is the value of every option element
      //Third is the display text of every option element
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Item item, int CategoryId)
    {
        _db.Items.Add(item); //Adds item to database
        _db.SaveChanges(); //save or else local item will not have an id to enter into the join
        if (CategoryId != 0)
        {
          //The line of code inside the if block creates the association between the newly created Item and a Category. 
          //Because the Item has been added and a new ItemId has been assigned, we can create a new CategoryItem join entity. This 
          //combines the ItemId with the CategoryId specified in the dropdown menu and passed in through our route's parameters.
            _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId });
            _db.SaveChanges();
        }
        return RedirectToAction("Index");
    }
    public ActionResult Details(int id)
    {
        var thisItem = _db.Items
            .Include(item => item.JoinEntities)
            .ThenInclude(join => join.Category)
            .FirstOrDefault(item => item.ItemId == id); // This specifies which item from the database were working with
        return View(thisItem);
    }

    public ActionResult Edit(int id)
    {
      Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      //Assigns the available categories to the SelectList object
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View(thisItem);
    }

    [HttpPost]
    public ActionResult Edit(Item item, int CategoryId)
    {
      if (CategoryId != 0)
      {
        _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId });
      }
      _db.Entry(item).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddCategory(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View(thisItem);
    }

    [HttpPost]
    public ActionResult AddCategory(Item item, int CategoryId)
    {
      if (CategoryId != 0)
      {
        _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId });
        _db.SaveChanges();
      }
    return RedirectToAction("Index");
    }
    //It may seem redundant to have a separate page where we add a categories because we already have the ability to 
    //add a category in our edit view. However, in that case, we had only established a one-to-many relationship as 
    //we never gave the user the ability to add more than one category to an item at a time, only the ability to edit 
    //an item's category. 
    //By adding a separate route to add categories, we give the user the option of adding many categories to an item.

    public ActionResult Delete(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      return View(thisItem);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      _db.Items.Remove(thisItem);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

//This route will find the entry in the join table by using the join entry's CategoryItemId. The CategoryItemId is
// being passed in through the variable joinId in our route's parameter and came from the BeginForm() HTML helper
// method in our details view.

    [HttpPost]
    public ActionResult DeleteCategory(int joinId)
    {
      var joinEntry = _db.CategoryItem.FirstOrDefault(entry => entry.CategoryItemId == joinId);
      _db.CategoryItem.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}