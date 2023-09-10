using RepositoryPatternSample.ClientModels.Base;
using RepositoryPatternSample.ClientModels.Base.Helpers;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Auth.User;
using RepositoryPatternSample.Entities.Data;
using RepositoryPatternSample.Entities.Domain;
using RepositoryPatternSample.Services.IServices.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace RepositoryPatternSample.Services.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        // private readonly IEmailSender _emailSender;

        string forgetPasswordTemplate = "<!DOCTYPE html>\r\n\r\n<html lang=\"en\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:v=\"urn:schemas-microsoft-com:vml\">\r\n\r\n<head>\r\n\t<title></title>\r\n\t<meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\r\n\t<meta content=\"width=device-width, initial-scale=1.0\" name=\"viewport\" />\r\n\t<!--[if mso]><xml><o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG/></o:OfficeDocumentSettings></xml><![endif]-->\r\n\t<!--[if !mso]><!-->\r\n\t<link href=\"https://fonts.googleapis.com/css?family=Abril+Fatface\" rel=\"stylesheet\" type=\"text/css\" />\r\n\t<link href=\"https://fonts.googleapis.com/css?family=Alegreya\" rel=\"stylesheet\" type=\"text/css\" />\r\n\t<link href=\"https://fonts.googleapis.com/css?family=Arvo\" rel=\"stylesheet\" type=\"text/css\" />\r\n\t<link href=\"https://fonts.googleapis.com/css?family=Bitter\" rel=\"stylesheet\" type=\"text/css\" />\r\n\t<link href=\"https://fonts.googleapis.com/css?family=Cabin\" rel=\"stylesheet\" type=\"text/css\" />\r\n\t<link href=\"https://fonts.googleapis.com/css?family=Ubuntu\" rel=\"stylesheet\" type=\"text/css\" />\r\n\t<!--<![endif]-->\r\n\t<style>\r\n\t\t* {\r\n\t\t\tbox-sizing: border-box;\r\n\t\t}\r\n\r\n\t\tbody {\r\n\t\t\tmargin: 0;\r\n\t\t\tpadding: 0;\r\n\t\t}\r\n\r\n\t\ta[x-apple-data-detectors] {\r\n\t\t\tcolor: inherit !important;\r\n\t\t\ttext-decoration: inherit !important;\r\n\t\t}\r\n\r\n\t\t#MessageViewBody a {\r\n\t\t\tcolor: inherit;\r\n\t\t\ttext-decoration: none;\r\n\t\t}\r\n\r\n\t\tp {\r\n\t\t\tline-height: inherit\r\n\t\t}\r\n\r\n\t\t.desktop_hide,\r\n\t\t.desktop_hide table {\r\n\t\t\tmso-hide: all;\r\n\t\t\tdisplay: none;\r\n\t\t\tmax-height: 0px;\r\n\t\t\toverflow: hidden;\r\n\t\t}\r\n\r\n\t\t@media (max-width:520px) {\r\n\r\n\t\t\t.desktop_hide table.icons-inner,\r\n\t\t\t.social_block.desktop_hide .social-table {\r\n\t\t\t\tdisplay: inline-block !important;\r\n\t\t\t}\r\n\r\n\t\t\t.icons-inner {\r\n\t\t\t\ttext-align: center;\r\n\t\t\t}\r\n\r\n\t\t\t.icons-inner td {\r\n\t\t\t\tmargin: 0 auto;\r\n\t\t\t}\r\n\r\n\t\t\t.image_block img.big,\r\n\t\t\t.row-content {\r\n\t\t\t\twidth: 100% !important;\r\n\t\t\t}\r\n\r\n\t\t\t.mobile_hide {\r\n\t\t\t\tdisplay: none;\r\n\t\t\t}\r\n\r\n\t\t\t.stack .column {\r\n\t\t\t\twidth: 100%;\r\n\t\t\t\tdisplay: block;\r\n\t\t\t}\r\n\r\n\t\t\t.mobile_hide {\r\n\t\t\t\tmin-height: 0;\r\n\t\t\t\tmax-height: 0;\r\n\t\t\t\tmax-width: 0;\r\n\t\t\t\toverflow: hidden;\r\n\t\t\t\tfont-size: 0px;\r\n\t\t\t}\r\n\r\n\t\t\t.desktop_hide,\r\n\t\t\t.desktop_hide table {\r\n\t\t\t\tdisplay: table !important;\r\n\t\t\t\tmax-height: none !important;\r\n\t\t\t}\r\n\t\t}\r\n\t</style>\r\n</head>\r\n\r\n<body style=\"background-color: #FFFFFF; margin: 0; padding: 0; -webkit-text-size-adjust: none; text-size-adjust: none;\">\r\n\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"nl-container\" role=\"presentation\"\r\n\t\tstyle=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #FFFFFF;\" width=\"100%\">\r\n\t\t<tbody>\r\n\t\t\t<tr>\r\n\t\t\t\t<td>\r\n\t\t\t\t\r\n\t\t\t\t\t<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row row-2\"\r\n\t\t\t\t\t\trole=\"presentation\"\r\n\t\t\t\t\t\tstyle=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #f5f5f5;\" width=\"100%\">\r\n\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t\t<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n\t\t\t\t\t\t\t\t\t\tclass=\"row-content stack\" role=\"presentation\"\r\n\t\t\t\t\t\t\t\t\t\tstyle=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #ffffff; color: #000000; width: 500px;\"\r\n\t\t\t\t\t\t\t\t\t\twidth=\"500\">\r\n\t\t\t\t\t\t\t\t\t\t<tbody>\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"column column-1\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 15px; padding-bottom: 20px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\twidth=\"100%\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\tclass=\"heading_block block-2\" role=\"presentation\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\twidth=\"100%\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\" style=\"text-align:center;width:100%;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<h1\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"padding-left: 10px; color: #393d47; direction: ltr; font-family: Tahoma, Verdana, Segoe, sans-serif; font-size: 18px; font-weight: normal; letter-spacing: normal; line-height: 120%; text-align: left; margin-top: 0; margin-bottom: 0;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<strong>Dear ***UserName***,</strong>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</h1>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"10\" cellspacing=\"0\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\tclass=\"text_block block-3\" role=\"presentation\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\twidth=\"100%\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: Tahoma, Verdana, sans-serif\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"font-size: 12px; font-family: Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 18px; color: #393d47; line-height: 1.5;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"margin: 0; font-size: 14px; text-align: left; mso-line-height-alt: 21px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tWe have received a request to reset the password for your account. If you did not initiate this request, please ignore this email.\r\n\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<br/>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<br/>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<br/>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<br/>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"font-size: 12px; font-family: Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 18px; color: #393d47; line-height: 1.5;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\" margin: 0; font-size: 18px;font-weight: bold; text-align: center; mso-line-height-alt: 21px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tTo reset your password, please follow this link.\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"15\" cellspacing=\"0\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\tclass=\"button_block block-4\" role=\"presentation\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\twidth=\"100%\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div align=\"center\" class=\"alignment\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<!--[if mso]><v:roundrect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" href=\"***ForgetPasswordLink***\" style=\"height:58px;width:272px;v-text-anchor:middle;\" arcsize=\"35%\" strokeweight=\"0.75pt\" strokecolor=\"#FFC727\" fillcolor=\"#ffc727\"><w:anchorlock/><v:textbox inset=\"0px,0px,0px,0px\"><center style=\"color:#393d47; font-family:Tahoma, Verdana, sans-serif; font-size:18px\"><![endif]--><a\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<a href=\"***ForgetPasswordLink***\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"text-decoration:none;display:inline-block;color:#393d47;background-color:#ffc727;border-radius:20px;width:auto;border-top:1px solid #FFC727;font-weight:undefined;border-right:1px solid #FFC727;border-bottom:1px solid #FFC727;border-left:1px solid #FFC727;padding-top:10px;padding-bottom:10px;font-family:Tahoma, Verdana, Segoe, sans-serif;font-size:18px;text-align:center;mso-border-alt:none;word-break:keep-all;\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\ttarget=\"_blank\"><span\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"padding-left:50px;padding-right:50px;font-size:18px;display:inline-block;letter-spacing:normal;\"><span\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"word-break: break-word;\"><span\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tdata-mce-style=\"\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"line-height: 36px;\"><strong>RESET PASSWORD</strong></span></span></span></a>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<!--[if mso]></center></v:textbox></v:roundrect><![endif]-->\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\tclass=\"text_block block-5\" role=\"presentation\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\twidth=\"100%\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<td class=\"pad\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"padding-bottom:5px;padding-left:10px;padding-right:10px;padding-top:10px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div style=\"font-family: Tahoma, Verdana, sans-serif\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<div class=\"\"\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"font-size: 12px; font-family: Tahoma, Verdana, Segoe, sans-serif; text-align: left; mso-line-height-alt: 18px; color: #393d47; line-height: 1.5;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tstyle=\"margin: 0; mso-line-height-alt: 19.5px;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<span style=\"font-size:13px;\">This link will expire in ***ExpireTime*** hours, so please use it as soon as possible. If you have any questions or concerns, please contact Administrator.</span>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t\t\t\t\t</table>\r\n\t\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</tbody>\r\n\t\t\t\t\t</table>\r\n\t\t\t \r\n\t\t\t\t</td>\r\n\t\t\t</tr>\r\n\t\t</tbody>\r\n\t</table>\r\n</body>\r\n\r\n</html>";

        public AuthService(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }
         
        public async Task<AuthResponseModel> Login(LoginVm model, string ipAddress)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null && user.StatusId == (byte)AppConstants.StatusId.InActive)
                    return new AuthResponseModel
                    {
                        IsSuccess = false,
                        Message = "Login Failed! User is not active!"
                    };

                if (user != null && user.StatusId == (byte)AppConstants.StatusId.Delete)
                    return new AuthResponseModel
                    {
                        IsSuccess = false,
                        Message = "Login Failed! User or password is invalid!"
                    };

                if (user != null && user.StatusId == (byte)AppConstants.StatusId.Active && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    //Uncomment, if need to log out from existing session
                    //var recordsToUpdate = _context.UserTokens.Where(t => t.UserId == id).ToList();
                    //foreach (var record in recordsToUpdate)
                    //{
                    //    record.IsInvalidated = true;
                    //}
                    //_context.UserTokens.UpdateRange(recordsToUpdate);

                    var accessToken = await GenerateToken(user);
                    string refreshToken = GenerateRefreshToken();

                    var result = await SaveTokenDetails(ipAddress, user.Id, accessToken, refreshToken);

                    string? typeName = null;

                    if (user.TypeId != null)
                    {
                        var type = _context.UserTypes.FirstOrDefault(
                                  x => x.Id == user.TypeId && x.StatusId != (byte)AppConstants.StatusId.Delete);

                        if (type != null)
                            typeName = type.Name;
                    }

                    result.User = new
                    {
                        user.Id,
                        user.UserName,
                        user.Name,
                        user.Email,
                        user.Address,
                        Phone = user.PhoneNumber,
                        user.TypeId,
                        TypeName = typeName,
                    };
                    return result;
                }

                return new AuthResponseModel
                {
                    IsSuccess = false,
                    Message = "Login Failed! User or password is invalid!"
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<AuthResponseModel> GetRefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
        {
            try
            {
                var refreshToken = GenerateRefreshToken();

                var token = GetJwtToken(request.ExpiredToken);

                var userTokenObj = _context.UserTokens.FirstOrDefault(
                    x => x.IsInvalidated == false && x.Token == request.ExpiredToken
                    && x.RefreshToken == request.RefreshToken
                    && x.IpAddress == ipAddress);

                if (token.ValidTo > DateTime.UtcNow)
                    return new AuthResponseModel
                    {
                        Token = request.ExpiredToken,
                        RefreshToken = request.RefreshToken,
                        IsSuccess = true,
                        Message = "Token not expired."
                    };

                var authResponse = ValidateDetails(token, userTokenObj);
                if (!authResponse.IsSuccess)
                    return authResponse;

                //--- token checking: end

                userTokenObj.IsInvalidated = true;
                _context.UserTokens.Update(userTokenObj);
                await _context.SaveChangesAsync();

                var userName = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var accessToken = await GenerateToken(user);
                    var response = await SaveTokenDetails(ipAddress, user.Id, accessToken, refreshToken);
                    return response;
                }
                else
                {
                    return new AuthResponseModel() { IsSuccess = false, Message = "Failed to generate refresh token" };
                }
            }
            catch (Exception ex)
            {
                return new AuthResponseModel() { IsSuccess = false, Message = "Failed to generate refresh token" };
            }
        }

        private AuthResponseModel ValidateDetails(JwtSecurityToken token, UserToken? userTokenObj)
        {
            if (userTokenObj == null)
                return new AuthResponseModel { IsSuccess = false, Message = "Invalid Token Details." };
            if (!userTokenObj.IsActive)
                return new AuthResponseModel { IsSuccess = false, Message = "Refresh Token Expired" };
            return new AuthResponseModel { IsSuccess = true };
        }

        private JwtSecurityToken GetJwtToken(string expiredToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ReadJwtToken(expiredToken);
        }

        private string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            var random = RandomNumberGenerator.Create();
            random.GetNonZeroBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

            int expiryTime = Convert.ToInt32(_configuration["JWT:ExpiryMinutes"]);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(expiryTime),
                claims: authClaims,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private async Task<AuthResponseModel> SaveTokenDetails(string ipAddress, int userId, string tokenString, string refreshToken)
        {
            try
            {
                int expiryTime = Convert.ToInt32(_configuration["JWT:RefreshTokenExpiryHours"]);
                var userToken = new UserToken
                {
                    CreationDate = DateTime.UtcNow,
                    ExpirationDate = DateTime.UtcNow.AddHours(expiryTime),
                    IpAddress = ipAddress,
                    IsInvalidated = false,
                    RefreshToken = refreshToken,
                    Token = tokenString,
                    UserId = userId
                };
                await _context.UserTokens.AddAsync(userToken);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new AuthResponseModel { IsSuccess = false, Message = "Failed to save token." };
            }

            return new AuthResponseModel { Token = tokenString, RefreshToken = refreshToken, IsSuccess = true, Message = "Success!" };
        }

        public async Task<bool> Logout(int id)
        {
            string userId = "" + id.ToString();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            try
            {
                var recordsToUpdate = _context.UserTokens.Where(t => t.UserId == id).ToList();
                foreach (var record in recordsToUpdate)
                {
                    record.IsInvalidated = true;
                }
                _context.UserTokens.UpdateRange(recordsToUpdate);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

        }


        public async Task<ResponseModel> ChangePassword(ChangePasswordVm changePassword, string token)
        {
            try
            {
                var tokenInfo = await _context.UserTokens.Where(u => u.Token == token).FirstOrDefaultAsync();
                var user = await _userManager.FindByIdAsync(tokenInfo.UserId.ToString());
                if (user != null && await _userManager.CheckPasswordAsync(user, changePassword.OldPassword))
                {
                    var _token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, _token, changePassword.NewPassword);

                    var result = await _userManager.UpdateAsync(user);
                    return new ResponseModel
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        Status = "Success",
                        Message = "Password changed successfully!"
                    };
                }
                else
                {
                    if (user == null)
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status409Conflict,
                            Status = "Failed",
                            Message = "Invalid User"
                        };
                    else
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status409Conflict,
                            Status = "Failed",
                            Message = "Password don't match with Account."
                        };
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ResponseModel> UpdatePassword(ForgetPasswordVm model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var passwordToken = await _context.ForgetPasswordTokens.Where(u => u.Email == model.Email && u.IsValid == true && u.ExpairOn > DateTime.Now).FirstOrDefaultAsync();



                    if (passwordToken != null)
                    {
                        var _token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        await _userManager.ResetPasswordAsync(user, _token, model.NewPassword);

                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            passwordToken.IsValid = false;
                            var r = _context.ForgetPasswordTokens.Update(passwordToken);
                            await _context.SaveChangesAsync();
                            return new ResponseModel
                            {
                                IsSuccess = true,
                                StatusCode = StatusCodes.Status200OK,
                                Status = "Success",
                                Message = "Password changed successfully!"
                            };
                        }
                        else
                            return new ResponseModel
                            {
                                IsSuccess = true,
                                StatusCode = StatusCodes.Status409Conflict,
                                Status = "Failed",
                                Message = "Failed to Change Password!"
                            };

                    }
                    else
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status409Conflict,
                            Status = "Failed",
                            Message = "Token is Expired or Invalid!"
                        };

                }
                else
                {
                    if (user == null)
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status409Conflict,
                            Status = "Failed",
                            Message = "Invalid User"
                        };
                    else
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status409Conflict,
                            Status = "Failed",
                            Message = "Password don't match with Account."
                        };
                }

            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseModel> ForgetPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                // var user = await _userManager.FindByNameAsync(email);
                if (user == null)
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Status = "Failed",
                        Message = "Invalid User"
                    };

                //Generate 60 Characters Token
                int tokenLength = Convert.ToInt32(_configuration["ForgetPassword:TokenLength"]);
                string token = CommonMethods.GetRandomString(tokenLength);
                //Save Token and Email With Life time
                int tokenLifeTime = Convert.ToInt32(_configuration["ForgetPassword:TokenExpire"]);
                ForgetPasswordToken forgetPassword = new ForgetPasswordToken()
                {
                    Email = user.Email,
                    Token = token,
                    CreateOn = DateTime.Now,
                    ExpairOn = DateTime.Now.AddHours(tokenLifeTime),
                    IsUsed = false,
                    IsValid = true
                };


                var fpTokenId = await AddForgetPasswordTokenAsync(forgetPassword);

                string link = _configuration["ForgetPassword:Link"].Replace("**email**", email).Replace("**token**", token);
                //send mail
                //string forget_password_Template = Path.Combine(_webHostEnvironment.ContentRootPath, AppConstants.ForgetPasswordEmailTemplatePath);
                // string message = File.ReadAllText(forget_password_Template);
                string message = forgetPasswordTemplate;
                message = message.Replace("***UserName***", user.Name);
                message = message.Replace("***ForgetPasswordLink***", link);
                message = message.Replace("***ExpireTime***", tokenLifeTime.ToString());

                //_emailSender.SendEmailAsync(new EmailModel()
                //{
                //    To = user.Email,
                //    Subject = "Forget Password",
                //    Body = message
                //});

                return new ResponseModel
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Status = "Success",
                    Message = "Please check your email to reset password!"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<long> AddForgetPasswordTokenAsync(ForgetPasswordToken entity)
        {
            try
            {
                //Deactive others request

                var sql = @"UPDATE ForgetPasswordTokens SET IsValid = 'false' WHERE Email = '{0}'";
                _context.Database.ExecuteSqlRaw(sql, entity.Email);

                var model = new ForgetPasswordToken()
                {
                    Email = entity.Email,
                    Token = entity.Token,
                    ExpairOn = entity.ExpairOn,
                    AttemptedCount = 0,
                    IsValid = true,
                    IsUsed = false,
                    CreateOn = DateTime.Now
                };
                _context.ForgetPasswordTokens.Add(model);
                await _context.SaveChangesAsync();
                return model.Id;
            }
            catch
            {
                throw;
            }
        }




    }
}
