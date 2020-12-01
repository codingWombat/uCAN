using System;
using System.IO;

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

        public static void ReadAllBytes(this Stream stream, byte[] buf, int offset, int count) {
            int num = stream.Read(buf, offset, count);
            if(num != count) throw new IOException("unexpected end of stream");
        }

        public static byte[] ReadAllBytes(this Stream stream, int len) {
            var ret = new byte[len];
            stream.ReadAllBytes(ret, 0, ret.Length);
            return ret;
        }

        public static string ReadAscii(this Stream stream, int len) {
            var tmp1 = stream.ReadAllBytes(len);
            var tmp2 = new char[tmp1.Length];
            for(int i = 0; i < tmp1.Length; i++) {
                tmp2[i] = (char)tmp1[i];
            }
            return new string(tmp2);
        }
    }
}
