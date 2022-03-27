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
    // Async. Will return a task containing an action result. Then, locate unique identifier for the currently-logged-in user and assign it the variable name userId
    public async Task<ActionResult> Index()
    {
      //this refers to the ItemController itself. FindFirst() method locates the first record that meets the provided criteria.
      //Method takes ClaimTypes.NameIdentifier as argument. This is why we need a using directive for System.Security.Claims
      //We specify ClaimTypes.NameIdentifier to locate the unique ID associated w/ the current account.
      //Name Identifier is a property that refers to an Entity's unique ID
      //Include the ? (existential operator). This states we should only call the property to right of the ? if the method to the left of the ? doesn't return null
      //In other words, if we successfully locate the NameIdentifier of the current user, we will call Value to retrieve the actual unique identifier value
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      //Call UserManager service first that we've injected into this controller
      // Call FindByIdAsync() method to find a users account by their unique id
      //Provide the UserId we just located as an argument to FindByIdAsync
      //This runs asynchronously. We include await so the code will wait for Idnetity to loacet the correct user before moving on
      //Create a variable to store a collection containing only the Items that are associated w/ the currently logged in users unique ID property
      var currentUser = await _userManager.FindByIdAsync(userId);
      var userItems = _db.Items.Where(entry => entry.User.Id == currentUser.Id).ToList();
      return View(userItems);
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
    public async Task<ActionResult> Create(Item item, int CategoryId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      item.User = currentUser;
      _db.Items.Add(item);
      _db.SaveChanges();
      if (CategoryId != 0)
      {
          _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId });
      }
      _db.SaveChanges();
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