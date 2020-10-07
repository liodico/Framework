#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("VxXUvXCx18yHKlegMGmyodcw2b0eNYOujGnCvDJLY9UQSfCZJVMvv9tYVllp21hTW9tYWFmeHAmAfptOP7tyIZGLcuxxC+qiNnrsSthzxJ5iIsv7Zx0V89wjK+zSJjPMyCGEJUZ8mVwmNLEBv4szLz04xYsW5+pLYDXnax496tzhK6/Cb3KfcK/2SyGfa91UhvAql8sJGlZeOoS0HCxA9WQEt/+j6kUFsRv/Aa968+0mGo9ik8BQlCKSNS1TmECPCYh8sv8Mq5JbUmPVQ5WaK2DjedkqhjL9tBWsbmnbWHtpVF9Qc98R365UWFhYXFlaj9S1dq7npDEZipt3VKnv4pIvtI/xF8aAwHvXNaH51QdyPRtqrjDDTgM/JNArepgKyltaWFlY");
        private static int[] order = new int[] { 4,6,6,6,6,5,8,13,11,10,13,11,12,13,14 };
        private static int key = 89;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
