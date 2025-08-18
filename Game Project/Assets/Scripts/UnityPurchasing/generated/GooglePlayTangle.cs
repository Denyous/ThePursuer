// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("0+YGaE9XC8PSrEnO5FcBtPt3ijTxU/uSYNK2NTJnhA0zgjpZ+MI/GtGMudlo+jUQaDAysVwDNi5jcSJ6QLyyzZeRVJw7SzVHmG7r7K5QBPP+QZohz5DaiquySWFKis6+K20nvIfwtRhVRolK7jzipKTakslZVXzJ1Wfkx9Xo4+zPY61jEujk5OTg5ebPUL7jORs0XgS4RpiE2t8U2fZX8Sr8V9mrlQvzQ9Uek7uPsbbRt3pWQFh55RxNpjVCtoehUC6TgFfTox2q2tGD4P0PVOp1M2K83LHcLT/J7Gfk6uXVZ+Tv52fk5OVHC5V1bz6gqpB5UMctyVyP8EW1KrZG37xIm0w7sLCaEZRLq/gjQSy9YRNFCEzF4LDz6pl7NPPq3Ofm5OXk");
        private static int[] order = new int[] { 9,13,11,6,12,5,9,7,10,10,11,13,13,13,14 };
        private static int key = 229;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
