using System;
using System.Runtime.InteropServices;

namespace uCAN {
    /// <summary>
    /// This class contains all utility-methods that do not fit somewhere else.
    /// </summary>
    internal static class Util {
        /// <summary>
        /// Checks if a given object is not null.
        /// </summary>
        public static T CheckNonNull<T>(T obj) where T : class {
            if(obj != null) return obj;
#pragma warning disable S3928
            throw new ArgumentNullException();
#pragma warning restore S3928
        }
    }
}
