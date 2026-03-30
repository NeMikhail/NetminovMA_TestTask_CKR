using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace Extention
{
    public static class UniTaskExtentded
    {
        public static async UniTask SendWebRequestSafely(this UnityWebRequest request)
        {
            request.SendWebRequest();
            await UniTask.WaitUntil(() => request.isDone || request.isNetworkError);
        }
    }

}

