using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace safeprojectname
{
    public class infoclassname : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "setNamedCamera";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("$a2ef309a-f2f5-477d-9ea0-bb624346ab19$");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}