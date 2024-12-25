using Microsoft.AspNetCore.Mvc;
using RefikHaber.Models;
using RefikHaber_Portal.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace RefikHaber_Portal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleManagerController : Controller
    {
        private readonly RoleManagerRepository _roleManagerRepository;

        public RoleManagerController(RoleManagerRepository roleManagerRepository)
        {
            _roleManagerRepository = roleManagerRepository;
        }

        public async Task<IActionResult> GetRoleList()
        {
            var roles = await _roleManagerRepository.GetAllRolesAsync();
            return View(roles.ToList());
        }

        public IActionResult RoleAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleAdd(AppRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManagerRepository.AddRoleAsync(role);
                if (result.Succeeded)
                {
                    TempData["deleteRoleSuccess"] = "Rol eklendi!";
                    return RedirectToAction("GetRoleList");
                }
                else
                {
                    ModelState.AddModelError("", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> RoleDelete(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                return NotFound("Rol ID'si bulunamadı.");
            }

            if (await _roleManagerRepository.HasUsersInRoleAsync(roleId))
            {
                TempData["deleteRoleError"] = "Bu role sahip en az bir kullanici varoldugu icin rol silinemedi!";
                return RedirectToAction("GetRoleList");
            }

            var result = await _roleManagerRepository.DeleteRoleAsync(roleId);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Rol başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Rol silinirken bir hata oluştu.";
            }

            return RedirectToAction("GetRoleList");
        }


        public IActionResult GetUserList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserListAjax()
        {
            var users = await _roleManagerRepository.GetUsersAsync();
            return Json(users);
        }

        public async Task<IActionResult> ManageUserRoles()
        {
            var users = await _roleManagerRepository.GetAllUsersAsync();
            ViewBag.Roles = await _roleManagerRepository.GetAllRolesAsync();
            ViewBag.UserRoles = await _roleManagerRepository.GetUserRolesGroupedAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(string userId, string[] selectedRoles)
        {
            if (selectedRoles != null && selectedRoles.Length > 0)
            {
                await _roleManagerRepository.UpdateUserRolesAsync(userId, selectedRoles);
                return RedirectToAction("ManageUserRoles");
            }
            ModelState.AddModelError("", "Rol seçimi yapılmadı.");
            return RedirectToAction("ManageUserRoles");
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var users = await _roleManagerRepository.GetAllUsersAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("Kullanıcı ID'si bulunamadı.");
            }

            var result = await _roleManagerRepository.DeleteUserAsync(userId);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Kullanıcı silinirken bir hata oluştu.";
            }

            return RedirectToAction("UserList");
        }
    }
}
