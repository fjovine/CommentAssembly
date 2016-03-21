using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentAssembly
{
    public class AssemblyVersion
    {
        public AssemblyVersion(string version)
        {
            string[] elements = version.Split('.');
            try
            {
                this.Major = uint.Parse(elements[0]);
                this.Minor = uint.Parse(elements[1]);
                this.Build = uint.Parse(elements[2]);
                this.Revision = uint.Parse(elements[3]);
            } catch (Exception e)
            {
                throw new FormatException("Unable to parse the version string" + e);
            }
        }

        public AssemblyVersion(AssemblyVersion other)
        {
            this.Major = other.Major;
            this.Minor = other.Minor;
            this.Build = other.Build;
            this.Revision = other.Revision;
        }

        public AssemblyVersion(uint major, uint minor, uint build, uint revision)
        {
            this.Major = major;
            this.Minor = minor;
            this.Build = build;
            this.Revision = revision;
        }

        public AssemblyVersion Next()
        {
            AssemblyVersion result = new AssemblyVersion(this);
            if (result.Revision == uint.MaxValue)
            {
                throw new OverflowException("The revision number is overflowing");
            }
            result.Revision++;
            return result;
        }

        public uint Major
        {
            get;
            private set;
        }
        
        public uint Minor
        {
            get;
            private set;
        }

        public uint Build
        {
            get;
            private set;
        }

        public uint Revision
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", this.Major, this.Minor, this.Build, this.Revision);
        }
    }
}
