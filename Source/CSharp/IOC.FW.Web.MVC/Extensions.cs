using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Mvc;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace IOC.FW.Web.MVC
{
    public static class Extensions
    {
        public const int ImageMinimumBytes = 512;
        
        public static bool IsImage(this HttpPostedFileBase postedFile)
        {
            
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            try
            {
                if (!postedFile.InputStream.CanRead)
                {
                    return false;
                }

                if (postedFile.ContentLength < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[512];
                postedFile.InputStream.Read(buffer, 0, 512);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static MvcHtmlString CheckBoxForHelper<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression
        )
        {
            return CheckBoxForHelper(htmlHelper, expression, null);
        }

        public static MvcHtmlString CheckBoxForHelper<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression,
            IDictionary<string, object> htmlAttributes
        )
        {
            return CheckBoxForHelper(htmlHelper, expression, htmlAttributes as object);
        }

        public static MvcHtmlString CheckBoxForHelper<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression,
            object htmlAttributes
        )
        {
            // get the name of the property
            string[] propertyNameParts = expression.Body.ToString().Split('.');
            string propertyName = propertyNameParts.Last();

            // get the value of the property
            Func<TModel, bool> compiled = expression.Compile();
            bool isChecked = compiled(htmlHelper.ViewData.Model);

            // create checkbox
            TagBuilder checkbox = new TagBuilder("input");
            checkbox.MergeAttribute("id", propertyName);
            checkbox.MergeAttribute("name", propertyName);
            checkbox.MergeAttribute("type", "checkbox");
            checkbox.MergeAttribute("value", "true");

            // create or use attributes if parameter htmlAttributes not null
            if (htmlAttributes != null)
            {
                IDictionary<string, object> attrs = htmlAttributes as Dictionary<string, object>;

                if (attrs == null)
                {
                    var type = htmlAttributes.GetType();
                    if (type != null)
                    {
                        var props = type.GetProperties();
                        attrs = props.ToDictionary(p => p.Name, p => p.GetValue(htmlAttributes, null));
                    }
                }

                checkbox.MergeAttributes(attrs);
            }

            // create checked attribute
            if (isChecked)
                checkbox.MergeAttribute("checked", "checked");

            // create hidden attribute
            TagBuilder hidden = new TagBuilder("input");
            hidden.MergeAttribute("name", propertyName);
            hidden.MergeAttribute("type", "hidden");
            hidden.MergeAttribute("value", "false");

            // concatenates tags for utilization
            string tag = string.Concat(
                checkbox.ToString(TagRenderMode.SelfClosing),
                hidden.ToString(TagRenderMode.SelfClosing)
            );

            return MvcHtmlString.Create(tag);
        }

        public static void AddModelErrors(this ModelStateDictionary modelState, ValidationResult validationResult)
        {
            if (validationResult != null && !validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    modelState.AddModelError(
                            error.PropertyName
                        , error.ErrorMessage
                    );
                }
            }
        }
    }
}