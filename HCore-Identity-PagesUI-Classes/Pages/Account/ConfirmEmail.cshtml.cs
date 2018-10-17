﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCore.Identity.Attributes;
using HCore.Identity.Generated.Controllers;
using HCore.Web.Exceptions;

namespace HCore.Identity.PagesUI.Classes.Pages.Account
{
    [SecurityHeaders]
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
                await _secureApiController.ConfirmUserEmailAddressAsync(userUuid, new Generated.Models.UserConfirmEmailSpec()
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