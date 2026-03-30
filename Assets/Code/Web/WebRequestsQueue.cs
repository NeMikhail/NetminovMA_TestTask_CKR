using System.Collections.Generic;
using UnityEngine.Networking;
using Zenject;

namespace Web
{
    public class WebRequestsQueue
    {
        private LinkedList<(UnityWebRequest request, WebRequestToken token)> _requestsQueue;

        [Inject]
        public void Construct()
        {
            _requestsQueue = new LinkedList<(UnityWebRequest, WebRequestToken)>();
        }

        public WebRequestToken AddRequest(UnityWebRequest request)
        {
            var token = new WebRequestToken();
            _requestsQueue.AddLast((request, token));
            return token;
        }

        public (UnityWebRequest request, WebRequestToken token) Dequeue()
        {
            var entry = _requestsQueue.First.Value;
            _requestsQueue.RemoveFirst();
            return entry;
        }

        public bool TryRemove(WebRequestToken token)
        {
            var node = _requestsQueue.First;
            while (node != null)
            {
                if (node.Value.token == token)
                {
                    node.Value.request.Abort();
                    node.Value.request.Dispose();
                    _requestsQueue.Remove(node);
                    return true;
                }
                node = node.Next;
            }
            return false;
        }

        public bool IsEmpty()
        {
            return _requestsQueue.Count == 0;
        }

        public void Clear()
        {
            var node = _requestsQueue.First;
            while (node != null)
            {
                node.Value.token.Cancel();
                node.Value.request.Abort();
                node.Value.request.Dispose();
                node = node.Next;
            }
            _requestsQueue.Clear();
        }
    }
}
