using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
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

            this.SetImageFileFromFormFile(data);
            _myDatabase.Photos.Add(data.Photo);
            _myDatabase.SaveChanges();

            return RedirectToAction("Index");

        }


        public IActionResult Details(int id)
        {

            Photo? foundedPhoto = _myDatabase.Photos.Where(photo => photo.Id == id).Include(Photo => Photo.categories).FirstOrDefault();

            if (foundedPhoto == null)
            {
                return NotFound($"Photo with id: {id} not founded");
            }
            else
            {
                return View("Details", foundedPhoto);
            }

        }


        //Update Get

 
        [HttpGet]
        public IActionResult Update(int id)
        {

            Photo? photoToEdit = _myDatabase.Photos.Where(photo => photo.Id == id).Include(photo => photo.categories).FirstOrDefault();

            if (photoToEdit == null)
            {
                return View("Error");
            }

            else
            {
             

                List<Category> categorydb = _myDatabase.Categories.ToList();

                List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();

                foreach (Category category in categorydb)
                {
                    allCategoriesSelectList.Add(new SelectListItem
                    {
                        Value = category.Id.ToString(),
                        Text = category.Name,
                        Selected = photoToEdit.categories.Any(c => c.Id == category.Id)
                    });
                }


                PhotoFormModel model
                     = new PhotoFormModel { Photo = photoToEdit, Categories = allCategoriesSelectList };

                return View("Update", model);
            }

        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PhotoFormModel data)
        {
            if (!ModelState.IsValid)
            {
                

                List<Category> categorydb = _myDatabase.Categories.ToList();
                List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();

                foreach (Category category in categorydb)
                {
                    allCategoriesSelectList.Add(new SelectListItem
                    {
                        Value = category.Id.ToString(),
                        Text = category.Name,

                    });
                }

                data.Categories = allCategoriesSelectList;

                return View("Update", data);
            }



            Photo? photoToUpdate = _myDatabase.Photos.Where(Photo => Photo.Id == id).Include(photo => photo.categories).FirstOrDefault();

            if (photoToUpdate != null)
            {
                photoToUpdate.categories.Clear();

                photoToUpdate.Title = data.Photo.Title;
                photoToUpdate.Description = data.Photo.Description;
                photoToUpdate.ImageUrl = data.Photo.ImageUrl;
             

                if (data.SelectedCategoriesId != null)
                {
                    foreach (string catSelectedId in data.SelectedCategoriesId)
                    {
                        int intcatSelectedId = int.Parse(catSelectedId);

                        Category? catInDb = _myDatabase.Categories.Where(tag => tag.Id == intcatSelectedId).FirstOrDefault();

                        if (catInDb != null)
                        {
                            photoToUpdate.categories.Add(catInDb);
                        }
                    }
                }

                if(data.ImageFormFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    data.ImageFormFile.CopyTo(stream);
                    photoToUpdate.ImageFile = stream.ToArray();
                }
                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("No photo to modify");
            }

        }

        //Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            Photo? photoToDelete = _myDatabase.Photos.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (photoToDelete != null)
            {
                _myDatabase.Photos.Remove(photoToDelete);
                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }

        }

      
        private void SetImageFileFromFormFile(PhotoFormModel formData)
        {
            if (formData.ImageFormFile == null)
            {
               return;
            }

            MemoryStream stream = new MemoryStream();
           formData.ImageFormFile.CopyTo(stream);
            formData.Photo.ImageFile = stream.ToArray();

        }



    }
}
