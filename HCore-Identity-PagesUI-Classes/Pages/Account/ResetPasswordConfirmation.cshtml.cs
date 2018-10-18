﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using HCore.Identity.Attributes;

namespace HCore.Identity.PagesUI.Classes.Pages.Account
{
    [SecurityHeaders]
    public class AccessDeniedModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}
