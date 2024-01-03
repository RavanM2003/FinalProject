using Final_Project.DAL;
using Final_Project.Helper;
using Final_Project.Models;
using Final_Project.ViewModels.AdminBlog;
using Final_Project.ViewModels.AdminQuestion;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public QuestionController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager = null)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public IActionResult Index(int page = 1, int take = 5)
        {
            QuestionVM questionVM = new QuestionVM();
            questionVM.Questions = _context.HelpQuestions
                .Where(s => !s.IsDeleted)
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();

            var count = _context.HelpQuestions.Where(p => !p.IsDeleted).Count();
            int pageCount = (int)Math.Ceiling((decimal)count / take);

            Pagination<HelpQuestion> pagination = new(questionVM.Questions, pageCount, page);

            return View(pagination);
        }

        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            var existelemnt = _context.HelpQuestions.FirstOrDefault(s => s.Id == id);
            existelemnt.IsDeleted = true;
            existelemnt.DeletedTime = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int id)
        {
            if (id == null) return NotFound();
            var existElement = _context.HelpQuestions
                .FirstOrDefault(x => x.Id == id);
            if (existElement == null) return NotFound();
            return View(existElement);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CreateQuestionVM questionVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            HelpQuestion helpQuestion = new()
            {
                Question = questionVM.Question,
                Answer = questionVM.Answer,
                AddedBy = User.Identity.Name,
                CreatedTime = DateTime.Now
            };
            _context.HelpQuestions.Add(helpQuestion);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            if (id == null) return NotFound();
            var existItem = _context.HelpQuestions.FirstOrDefault(x => x.Id == id);
            if (existItem == null) return NotFound();
            UpdateQuestionVM updateQuestionVM = new UpdateQuestionVM();
            updateQuestionVM.Question = existItem.Question;
            updateQuestionVM.Answer = existItem.Answer;
            return View(updateQuestionVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, UpdateQuestionVM updateQuestionVM)
        {
            if (!ModelState.IsValid) return BadRequest();
            var existItem = _context.HelpQuestions.FirstOrDefault(x => x.Id == id);
            if (existItem == null) return NotFound();
            existItem.Answer = updateQuestionVM.Answer;
            existItem.Question = updateQuestionVM.Question;
            existItem.UpdatedTime = DateTime.Now;
            existItem.AddedBy = User.Identity.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
