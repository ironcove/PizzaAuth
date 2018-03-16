using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using System;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace aspnet4_sample1.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var claimList = ClaimsPrincipal.Current.Claims;
            string auth0Token = null;
            string auth0UserId = null;
            foreach(var claim in claimList)
            {
                string claimVal = claim.Value;
                if(claim.Type== "Auth0Token")
                {
                    auth0Token = claimVal;
                }
                if(claim.Type == ClaimTypes.Gender)
                {
                    ViewBag.Gender = claimVal;
                }
                if(claim.Type== "Auth0UserId")
                {
                    auth0UserId = claimVal;
                }
            }

            if (auth0UserId != null)
            {

                var client = new ManagementApiClient("eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik9FUkdNa1l3TlRNMFJqaEZRVVV3UVRrNE1EVkZRVEpDTWpRMFJqWXlSa0U1TmpaRE5ESTBPQSJ9.eyJpc3MiOiJodHRwczovL3NoYW5ld2hpdGUuYXV0aDAuY29tLyIsInN1YiI6IjRuNE82aHltZGtmNGxUSG00Rk5TSkpWN1Rrc3ZhTG9DQGNsaWVudHMiLCJhdWQiOiJodHRwczovL3NoYW5ld2hpdGUuYXV0aDAuY29tL2FwaS92Mi8iLCJpYXQiOjE1MjEyMjQzODQsImV4cCI6MTE1MjEyMTA3ODQsImF6cCI6IjRuNE82aHltZGtmNGxUSG00Rk5TSkpWN1Rrc3ZhTG9DIiwic2NvcGUiOiJyZWFkOmNsaWVudF9ncmFudHMgY3JlYXRlOmNsaWVudF9ncmFudHMgZGVsZXRlOmNsaWVudF9ncmFudHMgdXBkYXRlOmNsaWVudF9ncmFudHMgcmVhZDp1c2VycyB1cGRhdGU6dXNlcnMgZGVsZXRlOnVzZXJzIGNyZWF0ZTp1c2VycyByZWFkOnVzZXJzX2FwcF9tZXRhZGF0YSB1cGRhdGU6dXNlcnNfYXBwX21ldGFkYXRhIGRlbGV0ZTp1c2Vyc19hcHBfbWV0YWRhdGEgY3JlYXRlOnVzZXJzX2FwcF9tZXRhZGF0YSBjcmVhdGU6dXNlcl90aWNrZXRzIHJlYWQ6Y2xpZW50cyB1cGRhdGU6Y2xpZW50cyBkZWxldGU6Y2xpZW50cyBjcmVhdGU6Y2xpZW50cyByZWFkOmNsaWVudF9rZXlzIHVwZGF0ZTpjbGllbnRfa2V5cyBkZWxldGU6Y2xpZW50X2tleXMgY3JlYXRlOmNsaWVudF9rZXlzIHJlYWQ6Y29ubmVjdGlvbnMgdXBkYXRlOmNvbm5lY3Rpb25zIGRlbGV0ZTpjb25uZWN0aW9ucyBjcmVhdGU6Y29ubmVjdGlvbnMgcmVhZDpyZXNvdXJjZV9zZXJ2ZXJzIHVwZGF0ZTpyZXNvdXJjZV9zZXJ2ZXJzIGRlbGV0ZTpyZXNvdXJjZV9zZXJ2ZXJzIGNyZWF0ZTpyZXNvdXJjZV9zZXJ2ZXJzIHJlYWQ6ZGV2aWNlX2NyZWRlbnRpYWxzIHVwZGF0ZTpkZXZpY2VfY3JlZGVudGlhbHMgZGVsZXRlOmRldmljZV9jcmVkZW50aWFscyBjcmVhdGU6ZGV2aWNlX2NyZWRlbnRpYWxzIHJlYWQ6cnVsZXMgdXBkYXRlOnJ1bGVzIGRlbGV0ZTpydWxlcyBjcmVhdGU6cnVsZXMgcmVhZDpydWxlc19jb25maWdzIHVwZGF0ZTpydWxlc19jb25maWdzIGRlbGV0ZTpydWxlc19jb25maWdzIHJlYWQ6ZW1haWxfcHJvdmlkZXIgdXBkYXRlOmVtYWlsX3Byb3ZpZGVyIGRlbGV0ZTplbWFpbF9wcm92aWRlciBjcmVhdGU6ZW1haWxfcHJvdmlkZXIgYmxhY2tsaXN0OnRva2VucyByZWFkOnN0YXRzIHJlYWQ6dGVuYW50X3NldHRpbmdzIHVwZGF0ZTp0ZW5hbnRfc2V0dGluZ3MgcmVhZDpsb2dzIHJlYWQ6c2hpZWxkcyBjcmVhdGU6c2hpZWxkcyBkZWxldGU6c2hpZWxkcyB1cGRhdGU6dHJpZ2dlcnMgcmVhZDp0cmlnZ2VycyByZWFkOmdyYW50cyBkZWxldGU6Z3JhbnRzIHJlYWQ6Z3VhcmRpYW5fZmFjdG9ycyB1cGRhdGU6Z3VhcmRpYW5fZmFjdG9ycyByZWFkOmd1YXJkaWFuX2Vucm9sbG1lbnRzIGRlbGV0ZTpndWFyZGlhbl9lbnJvbGxtZW50cyBjcmVhdGU6Z3VhcmRpYW5fZW5yb2xsbWVudF90aWNrZXRzIHJlYWQ6dXNlcl9pZHBfdG9rZW5zIGNyZWF0ZTpwYXNzd29yZHNfY2hlY2tpbmdfam9iIGRlbGV0ZTpwYXNzd29yZHNfY2hlY2tpbmdfam9iIHJlYWQ6Y3VzdG9tX2RvbWFpbnMgZGVsZXRlOmN1c3RvbV9kb21haW5zIGNyZWF0ZTpjdXN0b21fZG9tYWlucyByZWFkOmVtYWlsX3RlbXBsYXRlcyBjcmVhdGU6ZW1haWxfdGVtcGxhdGVzIHVwZGF0ZTplbWFpbF90ZW1wbGF0ZXMiLCJndHkiOiJjbGllbnQtY3JlZGVudGlhbHMifQ.b5YB_j5uzSDGYhHibodwYEBOMFMd22NLE4fXjJuGnWV_nuqX7lps5LiQ8EG3aHRfoRMGF1CI8kLqX2p6VjLlz_993mSzHM0PJJRSNVzzljcmLtbn69N1wrh0bMYErF_-ZXXXSYu9chCgjhAjQf_krERMvEtCGn4BHpGEV-7PspKNG7g_sxjsBGzGZP_uOTA0qQByp6JHA0zF0KdTJuUUK9K09Oi2Y3eYN-u7ZFNkoJTs-OGfnbIM0OVx70ETsuyUUMzZWFGAghy0zN-6X7rwB19yWSZEP6Npb0lDXxmivifyNIeviq2vp3g63KNFe1_XlYlwgkaFi7M0H80xUmT14Q", ConfigurationManager.AppSettings["auth0:Domain"]);
                var user = await client.Users.GetAsync(auth0UserId);
                if(user!=null)
                {
                    ViewBag.PictureUrl = user.Picture;
                    ViewBag.emailVerified = user.EmailVerified;
                }
                
            }
            //string name = ClaimsPrincipal.Current.FindFirst("Name")?.Value;
            //string email = ClaimsPrincipal.Current.FindFirst("email")?.Value;
            var currentUser = ClaimsPrincipal.Current;
            if (User.Identity.IsAuthenticated)
            {
                string name = User.Identity.Name;
                ViewBag.Name = name;
                //user is authenticated
            }

            //ViewBag.emailVerified = emailVerified;


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}