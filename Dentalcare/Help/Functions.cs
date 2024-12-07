using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Dental.Help
{
    public static class Functions
    {
        public static string ConvertToUnSign(string text)
        {
            for (int i = 33; i < 48 && i!=45; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 58; i < 65; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 91; i < 97; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            for (int i = 123; i < 127; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            text = text.Replace(" ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }



        // As the text the: "<span class='glyphicon glyphicon-plus'></span>" can be entered
        public static MvcHtmlString NoEncodeActionLink(this HtmlHelper htmlHelper,
                                             string text, string title, string action,
                                             string controller,
                                             object routeValues = null,
                                             object htmlAttributes = null)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            TagBuilder builder = new TagBuilder("a");
            builder.InnerHtml = text;
            builder.Attributes["title"] = title;
            builder.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
            builder.MergeAttributes(new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));

            return MvcHtmlString.Create(builder.ToString());
        }

        public static string GenerateNewId(string prefix, string lastId)
        {
            // Extract the numeric part of the last id
            string numericPart = lastId.Substring(prefix.Length);
            int numericValue = int.Parse(numericPart);

            // Increment the numeric part
            numericValue++;

            // Format the new id to have 8 digits
            return prefix + numericValue.ToString("D8"); // "D8" ensures 8 digits
        }

        public static string ShowTitleNameByRole(object role)
        {
            if (role is Dentist)
            {
                return "Tên nha sĩ";
            }
            else if (role is Admin)
            {
                return "Tên quản lý";
            }
            else if (role is Patient)
            {
                return "Tên bệnh nhân";
            }
            else if (role is Assisstant)
            {
                return "Tên phụ tá";
            }
            return "Không xác định vai trò"; // Default case if the role is not one of the recognized types
        }

        public static string FormatIntWithDots(int number)
        {
            // Convert the integer to a string
            string numberStr = number.ToString();

            // Initialize an empty string to hold the formatted result
            string formattedNumber = string.Empty;

            // Start from the end of the number string and add dots every 3 digits
            int counter = 0;
            for (int i = numberStr.Length - 1; i >= 0; i--)
            {
                counter++;
                formattedNumber = numberStr[i] + formattedNumber;

                // After every 3 digits, insert a dot if it's not the first group
                if (counter == 3 && i != 0)
                {
                    formattedNumber = "." + formattedNumber;
                    counter = 0;
                }
            }

            return formattedNumber;
        }

    }
}