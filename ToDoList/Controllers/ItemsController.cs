using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Controllers
{

  public class ItemsController : Controller
  {

    //Get request
    [HttpGet("/items")]
    public ActionResult Index()
    {
      List<Item> allItems = Item.GetAll();
      return View(allItems);
    }

    [HttpGet("/categories/{categoryId}/items/new")]
    public ActionResult New(int categoryId)
    {
      Category category = Category.Find(categoryId);
      return View(category);
    }
    //Post request
    [HttpPost("/items")]

    public ActionResult Create(string description)
    {

      Item myItem = new Item(description);
      // The first argument specifies the view that should be returned. This is new functionality we haven't covered before. 
      // In this case, we tell the View() method to return the "Index" view.
      //  We have to do this because we are no longer routing to a view with the same exact name as our route method.
      // We don't need to add a Create.cshtml view to correspond with our Create() route because we are reusing Index.cshtml. 
      return RedirectToAction("Index");
    }

    [HttpPost("/items/delete")]
    public ActionResult DeleteAll() // Route name is DeleteAll
    {
      Item.ClearAll();
      return View();
    }

    [HttpGet("/items/{id}")]
    public ActionResult Show(int id)
    {
      Item foundItem = Item.Find(id);
      return View(foundItem);
    }

    [HttpGet("/categories/{categoryId}/items/{itemId}")]
    public ActionResult Show(int categoryId, int itemId)
    {
      Item item = Item.Find(itemId);
      Category category = Category.Find(categoryId);
      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("item", item);
      model.Add("category", category);
      return View(model);
    }

  }
}