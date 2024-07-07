using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VDOEasy.Dbcontext;
using VDOEasy.Models;
using VDOEasy.ViewModels;

namespace VDOEasy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var masMemberTypes = _context.MasMemberTypes.ToList();
            var masMovieTypes = _context.MasMovieTypes.ToList();
            var masBranches = _context.MasBranches.ToList();

            ViewData["MasMemberTypes"] = masMemberTypes;
            ViewData["MasMovieTypes"] = masMovieTypes;
            ViewData["MasBranches"] = masBranches;

            return View();
        }


        [HttpPost]
        public IActionResult Index(MemberViewModel model)
        {
            try
            {
                int selectedBranchId = model.BranchId;
                int MemberTypeId = model.MemberTypeId;
                string firstName = model.FirstName;
                string lastName = model.LastName;
                DateTime birthDate = model.BirthDate;
                string address = model.Address;
                string idCardNumber = model.IdCardNumber;

                _context.InsertTrnMembers(selectedBranchId, firstName, lastName, birthDate, address, idCardNumber, MemberTypeId);

                var trnMembers = _context.TrnMembers.ToList();

                var existingMember = trnMembers.FirstOrDefault(m => m.IdCardNumber == idCardNumber);

                if (existingMember != null)
                {
                    foreach (var movieTypeId in model.SelectedMovieTypeIds)
                    {
                        _context.InsertTrnMembersMovieType(movieTypeId, existingMember.ID);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "No member found with the provided idCardNumber.");
                    return View(model);
                }
                return RedirectToAction("Receipt", new { memberId = existingMember.ID });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the data.");
                _logger.LogError(ex, "Error occurred while saving data.");
                return View(model);
            }
        }

        public IActionResult Receipt(int memberId)
        {
            var member = _context.GetTrnMembersById(memberId).FirstOrDefault();

            if (member == null)
            {
                return NotFound();
            }

            var memberType = _context.MasMemberTypes.FirstOrDefault(mt => mt.ID == member.MemberTypeID);
            if (memberType != null)
            {
                member.MemberTypeName = memberType.Name;
                member.MemberTypePrice = memberType.Price;
            }

            var viewModel = new ReceiptViewModel
            {
                MemberId = member.ID,
                FullName = $"{member.Firstname} {member.Lastname}",
                Address = member.Address,
                TotalAmount = member.MemberTypePrice
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
