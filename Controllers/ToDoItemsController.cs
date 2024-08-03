using Microsoft.AspNetCore.Mvc;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoItemsController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public IActionResult Items()
        {
            var name = Request.Cookies["ToDoItemName"];
            ViewBag.Cookie = name;
            var result = context.toDos.ToList();
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string Name)
        {

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(1)
            };
            Response.Cookies.Append("ToDoItemName", Name, cookieOptions);

            return RedirectToAction("Items");
        }


        public IActionResult CreateNew()
        {

            return View();
        }
        [HttpPost]
        public IActionResult CreateNew(ToDo toDo)
        {
            TempData["SuccessMessage"] = $"Created {toDo.Title} successfully";
            context.toDos.Add(toDo);
            context.SaveChanges();


            return RedirectToAction("Items");
        }

        public IActionResult Edit(int id)
        {
            var result = context.toDos.Find(id);
            if (result == null)
            {
                return RedirectToAction("NotFound");
            }
            return View(result);
        }
        [HttpPost]
        public IActionResult Edit(ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                var existingToDo = context.toDos.Find(toDo.Id);
                if (existingToDo == null)
                {
                    return RedirectToAction("NotFound");
                }

                // Update fields
                existingToDo.Title = toDo.Title;
                existingToDo.Description = toDo.Description;
                existingToDo.Deadline = toDo.Deadline;

                context.toDos.Update(existingToDo);
                context.SaveChanges();

                return RedirectToAction("Items");
            }

            // If the model state is invalid, return the view with the current model.
            return View(toDo);

        }

        public IActionResult Delete(int id)
        {
            var result = context.toDos.Find(id);

            if (result != null)
            {
                context.toDos.Remove(result);
                context.SaveChanges();
                return RedirectToAction("Items");
            }
            else
                return RedirectToAction("NotFound");
        }
    }
}
