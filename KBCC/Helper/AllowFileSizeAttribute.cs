using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KBCC.Helper
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AllowFileSizeAttribute : ValidationAttribute
    {
        public int FileSize { get; set; } = 10 * 1024;

        public override bool IsValid(object value)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;
            bool isValid = false;

            int allowFileSize = this.FileSize;
            if (file != null)
            {
                var fileSize = file.ContentLength;

                isValid = fileSize <= allowFileSize;
            }

            return isValid;
        }
    }
}