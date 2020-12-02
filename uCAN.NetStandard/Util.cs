using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static async Task ReadAllBytesAsync(this Stream stream, byte[] buf, int offset, int count) {
            int num = await stream.ReadAsync(buf, offset, count);
            if(num != count) throw new IOException("unexpected end of stream");
        }

        public static async Task<byte[]> ReadAllBytesAsync(this Stream stream, int len) {
            var ret = new byte[len];
            await stream.ReadAllBytesAsync(ret, 0, ret.Length);
            return ret;
        }

        public static async Task<string> ReadAsciiAsync(this Stream stream, int len) {
            var tmp1 = await stream.ReadAllBytesAsync(len);
            var tmp2 = new char[tmp1.Length];
            for(int i = 0; i < tmp1.Length; i++) {
                tmp2[i] = (char)tmp1[i];
            }
            return new string(tmp2);
        }

        public static string Repeat(this string str, int count) {
            return string.Join("", Enumerable.Repeat(str, count));
        }

        public static byte[] ToByteArray(this string str) => new StringBuilder(str).ToByteArray();

        public static byte[] ToByteArray(this StringBuilder b) {
            var ret = new byte[b.Length];
            for(int i = 0; i < b.Length; i++) {
                ret[i] = (byte)b[i];
                if(ret[i] != b[i]) throw new ArgumentException("unicode character found");
            }
            return ret;
        }
    }
}
