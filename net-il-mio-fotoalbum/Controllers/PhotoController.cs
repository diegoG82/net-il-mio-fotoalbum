using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {

        private PhotoContext _myDatabase;
        public PhotoController(PhotoContext db)
        {
            _myDatabase = db;
        }


        //Index View
        public IActionResult Index()
        {
            List<Photo> photos = _myDatabase.Photos.Include(Photo => Photo.categories).ToList();
            return View("index", photos);
        }






        //Create Get
        [HttpGet]
        public IActionResult Create()
        {
           
            List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();
            List<Category> categorydb = _myDatabase.Categories.ToList();

            foreach (Category category in categorydb)
            {
                allCategoriesSelectList.Add(
                       new SelectListItem
                       {
                           Text = category.Name,
                           Value = category.Id.ToString()
                       });
            }


            PhotoFormModel model = new PhotoFormModel { Photo = new Photo(),  Categories = allCategoriesSelectList };

            return View("Create", model);
        }


        //Create Post

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhotoFormModel data)
        {


            if (!ModelState.IsValid)
            {
               

                List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();
                List<Category> categorydb = _myDatabase.Categories.ToList();

                foreach (Category category in categorydb)
                {
                    allCategoriesSelectList.Add(
                           new SelectListItem
                           {
                               Text = category.Name,
                               Value = category.Id.ToString()
                           });
                }

                data.Categories = allCategoriesSelectList;

                return View("Create", data);
            }

            data.Photo.categories = new List<Category>();

            if (data.SelectedCategoriesId != null)
            {
                foreach (string categorySelectedId in data.SelectedCategoriesId)
                {
                    int intCategorySelectedId = int.Parse(categorySelectedId);

                    Category? CategoryInDb = _myDatabase.Categories.Where(Category => Category.Id == intCategorySelectedId).FirstOrDefault();

                    if (CategoryInDb != null)
                    {
                        data.Photo.categories.Add(CategoryInDb);
                    }
                }
            }


            _myDatabase.Photos.Add(data.Photo);
            _myDatabase.SaveChanges();

            return RedirectToAction("Index");

        }




    }
}
