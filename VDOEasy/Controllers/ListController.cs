using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VDOEasy.Dbcontext;
using VDOEasy.Models;
using System;
using System.Linq;
using VDOEasy.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace VDOEasy.Controllers
{
    public class ListController : Controller
    {
        private readonly ILogger<ListController> _logger;
        private readonly ApplicationDbContext _context;

        public ListController(ILogger<ListController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //list active members
        public IActionResult List()
        {
            try
            {
                var trnMembers = _context.TrnMembers
                    .Where(m => m.IsActive)
                    .ToList();

                foreach (var member in trnMembers)
                {
                    var branch = _context.MasBranches.FirstOrDefault(b => b.ID == member.BranchID);
                    if (branch != null)
                    {
                        member.BranchName = branch.Name;
                    }

                    var memberType = _context.MasMemberTypes.FirstOrDefault(mt => mt.ID == member.MemberTypeID);
                    if (memberType != null)
                    {
                        member.MemberTypeName = memberType.Name;
                        member.MemberTypePrice = memberType.Price;
                    }
                }

                return View(trnMembers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching members");
                return BadRequest("An error occurred while fetching the members.");
            }
        }

        // edit modal
        public IActionResult Edit(int id)
        {
            var member = _context.GetTrnMembersById(id).FirstOrDefault();
            if (member == null)
            {
                return NotFound();
            }

            var selectMovieTypes = _context.GetTrnMembersMovieTypeById(id).ToList();
            var masMovieTypes = _context.MasMovieTypes.ToList();

            var viewModel = new EditMemberViewModel
            {
                Member = member,
                SelectMovieTypes = selectMovieTypes,
                MasMovieTypes = masMovieTypes
            };

            ViewBag.Branches = _context.MasBranches.ToList();
            ViewBag.MemberTypes = _context.MasMemberTypes.ToList();

            return View(viewModel);
        }

        // update IsActive status
        [HttpPost]
        public IActionResult UpdateTrnMembersIsActiveById(int id)
        {
            _logger.LogInformation("Attempting to update IsActive status for trnMembers with ID {ID}", id);

            try
            {
                _context.UpdateTrnMembersIsActiveById(id, false);
                _logger.LogInformation("Successfully updated IsActive status for trnMembers with ID {ID}", id);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating IsActive status for trnMembers with ID {ID}", id);
                return BadRequest("An error occurred while updating the record.");
            }
        }
        // update member details
        [HttpPost]
        public async Task<IActionResult> Update(EditMemberViewModel viewModel)
        {
            if (viewModel == null || viewModel.Member == null)
            {
                return BadRequest("Invalid member data.");
            }

            viewModel.Member.Birthdate = EnsureValidSqlDateTime(viewModel.Member.Birthdate);
            viewModel.Member.IssueDate = EnsureValidSqlDateTime(viewModel.Member.IssueDate);
            viewModel.Member.ReceiptDate = EnsureValidSqlDateTime(viewModel.Member.ReceiptDate);

            try
            {
                // Update main details of TrnMembers
                await _context.UpdateTrnMembers(
                    viewModel.Member.ID,
                    viewModel.Member.BranchID,
                    viewModel.Member.Firstname,
                    viewModel.Member.Lastname,
                    viewModel.Member.Birthdate,
                    viewModel.Member.Address,
                    viewModel.Member.IdCardNumber,
                    viewModel.Member.MemberTypeID
                );

                // Update TrnMembersMovieType
                var existingMovieTypes = _context.GetTrnMembersMovieTypeById(viewModel.Member.ID).ToList();

                // Remove unchecked movie types
                foreach (var movieType in existingMovieTypes)
                {
                    if (!viewModel.SelectedMovieTypeIds.Contains(movieType.MovieTypeID))
                    {
                        _context.DeletetrnMembersMovieTypeById(movieType.MemberID);
                    }
                }

                // Add newly checked movie types
                foreach (var movieTypeId in viewModel.SelectedMovieTypeIds)
                {
                    if (!existingMovieTypes.Any(m => m.MovieTypeID == movieTypeId))
                    {
                        _context.InsertTrnMembersMovieType(movieTypeId, viewModel.Member.ID);
                    }
                }

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating member details for ID {ID}", viewModel.Member.ID);
                return BadRequest("An error occurred while updating the member.");
            }
        }

        private DateTime EnsureValidSqlDateTime(DateTime dateTime)
        {
            if (dateTime < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                return (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            return dateTime;
        }
        // Update IsActive status
        [HttpPost]
        public async Task<IActionResult> UpdateIsActive(int id, bool isActive)
        {
            _logger.LogInformation("Attempting to update IsActive status for TrnMembers with ID {ID}", id);

            try
            {
                _context.UpdateTrnMembersIsActiveById(id, false);
                _logger.LogInformation("Successfully updated IsActive status for TrnMembers with ID {ID}", id);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating IsActive status for TrnMembers with ID {ID}", id);
                return BadRequest("An error occurred while updating the record.");
            }
        }
    }
}
