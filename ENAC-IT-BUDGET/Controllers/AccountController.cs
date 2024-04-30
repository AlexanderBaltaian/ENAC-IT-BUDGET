using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Enacit.Lib.Web.Auth;
using ENAC_IT_BUDGET.Models.ENACIT_Budget;
using System.Web.UI.WebControls;
using ENAC_IT_BUDGET.ViewModels;
using System.Collections;
using System.Globalization;

namespace enac_lend.Controllers
{
	public sealed class AccountController : Controller
	{
		/// <summary>
		/// Redirection par défaut de l'utilisateur. Par exemple si il ne spécifie pas d'url de retour.
		/// </summary>
		private const string _DEFAULT_REDIRECTION_PAGE_URL = "~/"; //"~/Account/"

		/// <summary>
		/// Contient la liste des champs qui seront ajouté dans les optionalProperties de l'EPFLUser
		/// champs disponible: http://tequila.epfl.ch/serverinfo
		/// uniqueid (== sciper) est obligatoire pour le template IT2
		/// </summary>
		/// <example>
		/// "uniqueid,username,name,firstname,email,phone,office,allunits,statut,role-respinfo,role-respadmin"
		/// </example>
		private const string _REQUESTED_USER_ATTRIBUTES = "uniqueid,group,username,displayname,email,unit";


		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			TimeSpan authMax = TimeSpan.FromMinutes(10);
			var authDate = (DateTime?)Session["authDate"];
			if (!User.Identity.IsAuthenticated || !Roles.IsUserInRole("auth") || !authDate.HasValue || authDate + authMax < DateTime.Now)
			{
				var tequila = new Tequila(Response);

				//// Paramètres de l'interface 
				tequila.SetParam("usecookie", "on");
				tequila.SetParam("urlacces", Request.Url.ToString());
				tequila.SetParam("service", "ENAC-IT-BUDGET");
				tequila.SetParam("language", System.Threading.Thread.CurrentThread.CurrentCulture.ToString() == "en" ? "en" : "fr");

				tequila.SetRequested("request", _REQUESTED_USER_ATTRIBUTES);

				try
				{
					var attributs = tequila.CheckUser(Request.QueryString["key"]);

					if (attributs != null)
					{
						var userSciper = attributs["uniqueid"];
						var username = attributs["username"];
						var displayname = attributs["displayname"];
						var email = attributs["email"];
						//var email = "andrew.barry@epfl.ch";
						var group = attributs["group"].Split(',');
						var unit = attributs["unit"].Split(',');

                        Session["userSciper"] = userSciper;
						Session["username"] = username;
						Session["displayname"] = displayname;
						Session["email"] = email;
						Session["group"] = group;
						Session["unit"] = unit;



                        Roles.AddUserToRole(userSciper, "auth");

						Session["authDate"] = DateTime.Now;

                        var dbEnacitbudget = new enacit_budget();
						var viewModel = new VariablesTableauViewModel();
                        var isEnacitMember = dbEnacitbudget.tb_user.Any(x => x.AdresseMail == email);

                        if (isEnacitMember)
                        {
                            var units = dbEnacitbudget.tb_unit.ToList();
							Session["unitnames"]= units;
						}
						else
						{
							var unitsAuth = dbEnacitbudget.tb_unit_contact.Where(x => x.AdresseEmail == email).Select(x => x.tb_unit).ToList();

                            Session["unitnames"] = unitsAuth;
							viewModel.UnitsAuth = unitsAuth;
						}

                        FormsAuthentication.RedirectFromLoginPage(userSciper, false);

						return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? _DEFAULT_REDIRECTION_PAGE_URL : returnUrl);
					}
					else
					{
						return null;
					}
				}
				catch (Exception)
				{
                    //#if DEBUG
                    throw;
					//#endif
					//					throw new IT2Exception("Exception Tequila");
				}
			}
			//return Redirect(_DEFAULT_REDIRECTION_PAGE_URL);
			return new HttpUnauthorizedResult();
		}

		/// <summary>
		/// Clear le cookie tequila et la session sourante lors d'un logoff
		/// </summary>
		/// <returns></returns>
		public RedirectResult Logout()
		{
			if (User.Identity.IsAuthenticated)
			{
				Roles.DeleteCookie();
				Session.Abandon();
				Session.Clear();
				FormsAuthentication.SignOut();
			}

			var appPath = string.Empty;
			var request = HttpContext.Request;

			if (request != null)
			{
				appPath = string.Format("{0}://{1}{2}{3}",
										request.Url.Scheme,
										request.Url.Host,
										request.Url.Port == 80 ? string.Empty : ":" + request.Url.Port,
										request.ApplicationPath);

				if (!appPath.EndsWith("/"))
				{
					appPath += "/";
				}
				//forward vers: https://tequila.epfl.ch/cgi-bin/tequila/logout?urlaccess=http%3a%2f%2fgchotline.epfl.ch
				return Redirect("https://tequila.epfl.ch/cgi-bin/tequila/logout?urlaccess=" + appPath);
			}
			else
			{
				return Redirect("https://tequila.epfl.ch/cgi-bin/tequila/logout/");
			}
		}
	}
}
