using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Data;
using Notes.Models;

namespace Notes.Controllers
{
    public class NoteController : Controller
    {
        
        private readonly ApplicationDbContext _context;
       
        public NoteController(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public IActionResult Index()
        {
            
           string check = HttpContext.Session.GetString("login_session");
            int isdr = int.Parse(check);
            ViewBag.sessioncheck = check;
            ViewBag.loginsession = HttpContext.Session.GetString("login_email");
            if (check != null && isdr != null)
            {
                var row = _context.notes.Where(n => n.UserId == isdr).ToList();
                return View(row);
            }
            else
            {

                return RedirectToAction("Login");
            }
                
        }
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Login(string LoginEmail, string LoginPassword)
        {
            var row = _context.users.FirstOrDefault(u => u.Email == LoginEmail);
            if (row != null && row.Password==LoginPassword)
            {
                HttpContext.Session.SetString("login_session", row.Id.ToString());
                HttpContext.Session.SetString("login_email", row.Email.ToString());
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Invalid Email or Password";
                return View();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("login_session");
            HttpContext.Session.Remove("login_email");
            return RedirectToAction("Login");
        }
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(User user)
        {
            _context.users.Add(user);
            _context.SaveChanges();
            ViewBag.Message = "Registration Successful";
            return RedirectToAction("Login");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Note note)
        {
            string check = HttpContext.Session.GetString("login_session");
            int isdr = int.Parse(check);
            if(isdr ==null)
            {
                return RedirectToAction("Login");
            }
            note.UserId = isdr; // Set the UserId to the logged-in user's ID
            _context.notes.Add(note);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var note = _context.notes.Find(id);
            var check = HttpContext.Session.GetString("login_session");
            var isdr = int.Parse(check);
            if (note == null || note.UserId != isdr)
            {
                return NotFound(); // Return 404 if note not found or user does not have permission
            }
            return View(note);  
        }
        [HttpPost]
        public IActionResult Edit(Note note)
        {
            var check = HttpContext.Session.GetString("login_session");
            var isdr = int.Parse(check);
            if (note.UserId != isdr)
            {
                return NotFound(); // Return 404 if user does not have permission
            }
            note.UserId = isdr; // Ensure the UserId is set to the logged-in user's ID
            _context.notes.Update(note);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            return View(_context.notes.Find(id));
        }
        public IActionResult DeleteParmission(int id)
        {
            var note = _context.notes.Find(id);
            return View(note);
        }
        public IActionResult DeleteConfirmed(int id)
        {
            var note = _context.notes.Find(id);
            if (note != null)
            {
                _context.notes.Remove(note);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            return View(_context.notes.Find(id));
        }
        public IActionResult Profile()
        {
            string check = HttpContext.Session.GetString("login_session");
            int isdr = int.Parse(check);
            var user = _context.users.Find(isdr);   
            return View(user);
        }
        public IActionResult ProfileEdit(int id)
        {
            var user = _context.users.Find(id);
            string check = HttpContext.Session.GetString("login_session");
            int isdr = int.Parse(check);
            if (user == null || user.Id != isdr)
            {
                return NotFound();
            }
            return View(user);

        }
        [HttpPost]
        public IActionResult ProfileEdit(User user)
        {
            string check = HttpContext.Session.GetString("login_session");
            int isdr = int.Parse(check);
            if (user.Id != isdr)
            {
                return NotFound();
            }
            user.Id = isdr; // Ensure the UserId is set to the logged-in user's ID
            _context.users.Update(user);
            _context.SaveChanges();
            HttpContext.Session.SetString("login_name", user.Name);
            HttpContext.Session.SetString("login_email", user.Email);
            HttpContext.Session.SetString("login_address", user.adress);
            return RedirectToAction("Profile");
        }
    }
}
