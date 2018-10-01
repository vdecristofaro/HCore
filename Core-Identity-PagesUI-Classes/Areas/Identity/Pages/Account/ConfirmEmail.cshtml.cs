﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReinhardHolzner.Core.Identity.AuthAPI.Generated.Controllers;
using ReinhardHolzner.Core.Web.Exceptions;

namespace ReinhardHolzner.Core.Identity.PagesUI.Classes.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly ISecureApiController _secureApiController;

        public ConfirmEmailModel(
            ISecureApiController secureApiController)
        {
            _secureApiController = secureApiController;
        }

        public async Task<IActionResult> OnGetAsync(string userUuid, string code)
        {
            ModelState.Clear();

            try
            {
                await _secureApiController.ConfirmUserEmailAddressAsync(userUuid, new AuthAPI.Generated.Models.UserConfirmEmailSpec()
                {                    
                    Code = code
                }).ConfigureAwait(false);

                return Page();
            }
            catch (ApiException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);

                return LocalRedirect("~/");
            }            
        }
    }
}
