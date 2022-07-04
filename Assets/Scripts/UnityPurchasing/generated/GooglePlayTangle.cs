// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("iM8XS2HKxq9LK0DhE3C5sPIai8522M8CI5U/TKmGMSTrYlCDf/e2EX/88v3Nf/z3/3/8/P1khDQvJa7y67cxjSkIAdaIKSCPZ5KvDwqLD9zNf/zfzfD79Nd7tXsK8Pz8/Pj9/qtzRdi/CLSxGJKHx+OcJwhP7/YCAajzbKpvzGpFaF78GwgVHE6DCxRpP/YiNmpuSypve8vtm704K/HGp1wYmda1VIkeiSW9pF7yXs34DgZLCal3sYt7r3Umv1KOrQrIUbCjgzKqrn6kMujJR3SauLn5x5u8CDDeQQ1n01wrYXXlJdTa9br36aQD8dhkTmmIQx62wxxr0gcY03svhAdOQT+40VI8BDPERlsAhM4Nz464AfoUROyZGuHdigHnFP/+/P38");
        private static int[] order = new int[] { 10,4,4,11,10,10,12,8,8,12,10,12,13,13,14 };
        private static int key = 253;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
